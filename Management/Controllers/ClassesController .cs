using Common;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Model.Models;
using Newtonsoft.Json;
using NPOI.Util;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static Common.TransactionsInfo;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/Classes")]
    public class ClassesController : Controller
    {
        private Security security;
        private TransactionsInfo transactions;
        private FileHandler fileHandler;
        IConfiguration configuration;

        private readonly JeelContext db;

        public ClassesController(IConfiguration iConfig, JeelContext context)
        {
            this.db = context;
            security = new Security(iConfig, context);
            transactions = new TransactionsInfo(iConfig, context);
            fileHandler = new FileHandler(iConfig, context);
        }


        public partial class BodyObject
        {
            public long Id { get; set; }
            public short SchoolsId { get; set; }
            public string Name { get; set; }
            public int Capacity { get; set; }
            public string Descriptions { get; set; }
            public int Price { get; set; }
        }


        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize, string Search,long SchoolsId,short ProfileYearsId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.Classes.Where(x => x.Status!=9).Count(),
                    Active = db.Classes.Where(x => x.Status==1).Count(),
                    NotActive = db.Classes.Where(x => x.Status == 2).Count(),
                    Deleted = db.Classes.Where(x => x.Status == 9).Count(),
                };

                int Count = db.Classes
                    .Where(x => x.Status != 9
                    && (SchoolsId>0 ? x.SchoolsId==SchoolsId : true)
                    && (ProfileYearsId > 0 ? x.ProfileYearsId== ProfileYearsId : true)
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Name.Contains(Search.Trim())))
                    ).Count();

                var Info = db.Classes
                    .Include(x=>x.Schools)
                    .Include(x=>x.ProfileYears)
                    .Include(x=>x.ClassesStudents)
                      .Where(x => x.Status != 9
                    && (SchoolsId > 0 ? x.SchoolsId == SchoolsId : true)
                    && (ProfileYearsId > 0 ? x.ProfileYearsId == ProfileYearsId : true)
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Name.Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        Schools=x.Schools.Name,
                        x.SchoolsId,
                        ProfileYears=x.ProfileYears.Name,
                        x.ProfileYearsId,
                        x.Descriptions,
                        x.Capacity,
                        x.Price,
                        StudentCount=x.ClassesStudents.Where(k=>k.Status==1).Count(),
                        CreatedOn = x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                        x.Status,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] BodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                //validations
                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if(bodyObject.SchoolsId<=0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "المدرسة ");
                
                if(bodyObject.Price<=0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "سعر الاشتراك ");

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId==bodyObject.SchoolsId && x.Status==1).SingleOrDefault();
                if (ActiveProfile==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);
                
                var Schools = db.Schools
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.Id==bodyObject.SchoolsId)
                    .SingleOrDefault();
                if(Schools.User.Status!=1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveAcountSchools);


                //isExist
                var IsExist = db.Classes.Where(x => x.Name == bodyObject.Name 
                    && x.ProfileYearsId==ActiveProfile.Id
                    && x.SchoolsId==bodyObject.SchoolsId 
                    && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Exist+ " الفصل الدراسي ");


                Classes row = new Classes();
                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                row.Capacity = bodyObject.Capacity;
                row.SchoolsId = bodyObject.SchoolsId;
                row.ProfileYearsId = ActiveProfile.Id;
                row.Price = bodyObject.Price;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.Classes.Add(row);


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);
                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] BodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                //validations
                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (bodyObject.SchoolsId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "المدرسة ");

                if (bodyObject.Price <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "سعر الاشتراك ");



                var row = db.Classes.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if(row==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId==bodyObject.SchoolsId && x.Status==1).SingleOrDefault();
                if(ActiveProfile==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var Schools = db.Schools
                   .Include(x => x.User)
                   .Where(x => x.User.Status != 9 && x.Id == bodyObject.SchoolsId)
                   .SingleOrDefault();

                if (Schools.User.Status != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveAcountSchools);

                //isExist
                var IsExist = db.Classes.Where(x => x.Name == bodyObject.Name
                    && x.ProfileYearsId == ActiveProfile.Id
                    && x.SchoolsId == bodyObject.SchoolsId
                    && x.Id!=bodyObject.Id
                    && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Exist + " الفصل الدراسي ");

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject= JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Price,
                    row.Capacity,
                    row.SchoolsId,
                    row.ProfileYearsId,
                    row.Status
                });


                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                row.Capacity = bodyObject.Capacity;
                row.Price = bodyObject.Price;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;


                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);
                db.SaveChanges();
                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/ChangeStatus")]
        public IActionResult ChangeStatus(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Classes
                    .Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.Id == row.ProfileYearsId).SingleOrDefault();
                if (ActiveProfile.Status != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var Schools = db.Schools
                   .Include(x => x.User)
                   .Where(x => x.User.Status != 9 && x.Id == row.SchoolsId)
                   .SingleOrDefault();

                if (Schools.User.Status != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveAcountSchools);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject= JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Status,
                });

                if (row.Status == 1)
                {
                    row.Status = 2;
                }
                else
                {
                    row.Status = 1;
                }

                
                rowTrans.Operations = TransactionsType.CahngeStatus;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تغير حالة العرض    ";
                rowTrans.Controller = "Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Status,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Delete")]
        public IActionResult Delete(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Classes
                    .Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.Id == row.ProfileYearsId).SingleOrDefault();
                if (ActiveProfile.Status != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var HaveStudent = db.ClassesStudents.Where(x => x.ClassesId == row.Id && x.Status == 1).Count();
                if (HaveStudent > 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.HasChild);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }







        
        public partial class CoursesBodyObject
        {
            public long Id { get; set; }
            public long ClasseId { get; set; }
            public short TeacherId { get; set; }
            public short SubjectsId { get; set; }
            public string ImageName { get; set; }
            public string Image { get; set; }
            public string Descriptions { get; set; }
        }

        //Courses
        [HttpGet("Courses/Get")]
        public IActionResult GetCourses(int pageNo, int pageSize, string Search,long ClasseId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.Courses.Where(x => x.Status != 9 && x.ClassesId==ClasseId).Count(),
                    Active = db.Courses.Where(x => x.Status == 1 && x.ClassesId== ClasseId).Count(),
                    NotActive = db.Courses.Where(x => x.Status == 2 && x.ClassesId == ClasseId).Count(),
                    Deleted = db.Courses.Where(x => x.Status == 9 && x.ClassesId == ClasseId).Count(),
                };

                int Count = db.Courses
                    .Include(x=>x.Subjects)
                    .Include(x=>x.Teacher)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Subjects.Name.Contains(Search.Trim()) ||
                        x.Teacher.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim())
                        ))
                    ).Count();
                var Info = db.Courses
                    .Include(x => x.Subjects)
                    .Include(x => x.Teacher)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Subjects.Name.Contains(Search.Trim()) ||
                        x.Teacher.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim())
                        ))
                    ).Select(x => new
                    {
                        x.Id,
                        x.SubjectsId,
                        Subjects=x.Subjects.Name,
                        x.TeacherId,
                        Teacher=x.Teacher.Name,
                        x.Image,
                        x.Descriptions,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Courses/GetAll")]
        public IActionResult GetAllCourses(long ClasseId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                
                var Info = db.Courses
                    .Include(x => x.Subjects)
                    .Include(x => x.Teacher)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                    ).Select(x => new
                    {
                        x.Id,
                        x.SubjectsId,
                        x.Subjects.Name,
                        x.TeacherId,
                        Teacher = x.Teacher.Name,
                    }).OrderByDescending(x => x.Name).ToList();


                return Ok(new { info = Info });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Courses/Add")]
        public IActionResult AddCourses([FromBody] CoursesBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.TeacherId<=0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الأستاذ");
                
                if (bodyObject.SubjectsId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "المادة الدراسية");
                
                var isExist = db.Courses.Where(x => x.TeacherId == bodyObject.TeacherId
                    && x.SubjectsId ==bodyObject.SubjectsId
                    && x.ClassesId ==bodyObject.ClasseId
                    && x.Status != 9
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                Courses row = new Courses();
                row.TeacherId = bodyObject.TeacherId;
                row.SubjectsId = bodyObject.SubjectsId;
                row.ClassesId = bodyObject.ClasseId;
                row.Descriptions = bodyObject.Descriptions;
                if (string.IsNullOrEmpty(bodyObject.Image))
                {
                    row.Image = "/Uploads/Cover.png";
                }
                else
                {
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);
                }

                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.Courses.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "Courses/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Courses/Edit")]
        public IActionResult EditCourses([FromBody] CoursesBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.TeacherId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الأستاذ");

                if (bodyObject.SubjectsId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "المادة الدراسية");

                var row = db.Courses.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if(row==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


                var isExist = db.Courses.Where(x => x.TeacherId == bodyObject.TeacherId
                    && x.SubjectsId == bodyObject.SubjectsId
                    && x.ClassesId == bodyObject.ClasseId
                    && x.Status != 9
                    && x.Id!=bodyObject.Id
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.TeacherId,
                    row.SubjectsId,
                    row.ClassesId,
                    row.Status,
                    row.Image
                });



                row.TeacherId = bodyObject.TeacherId;
                row.SubjectsId = bodyObject.SubjectsId;
                row.ClassesId = bodyObject.ClasseId;
                row.Descriptions = bodyObject.Descriptions;
                if (!string.IsNullOrEmpty(bodyObject.ImageName))
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);


                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "Courses/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Courses/Delete")]
        public IActionResult DeleteCourses(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var row = db.Courses.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "Courses/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.TeacherId,
                    row.SubjectsId,
                    row.ClassesId,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Courses/ChangeStatus")]
        public IActionResult ChangeStatusCourses(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var row = db.Courses.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                if (row.Status == 1)
                {
                    row.Status = 2;
                }
                else
                {
                    row.Status = 1;
                }


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تغير حالة العرض  ";
                rowTrans.Controller = "Courses/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.TeacherId,
                    row.ClassesId,
                    row.SubjectsId,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessChangeStatusOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        
        
        public partial class CoursesSchedulesBodyObject
        {
            public long Id { get; set; }
            public long CoursesId { get; set; }
            public short Day { get; set; }
            public short Number { get; set; }
        }

        //CoursesSchedules
        [HttpGet("CoursesSchedules/Get")]
        public IActionResult GetCoursesSchedules(int pageNo, int pageSize, string Search,long ClasseId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.CoursesSchedules.Include(x=>x.Courses).Where(x => x.Status != 9 && x.Courses.ClassesId ==ClasseId).Count(),
                    Active = db.CoursesSchedules.Include(x => x.Courses).Where(x => x.Status == 1 && x.Courses.ClassesId == ClasseId).Count(),
                    NotActive = db.CoursesSchedules.Include(x => x.Courses).Where(x => x.Status == 2 && x.Courses.ClassesId == ClasseId).Count(),
                    Deleted = db.CoursesSchedules.Include(x => x.Courses).Where(x => x.Status == 9 && x.Courses.ClassesId == ClasseId).Count(),
                };

                int Count = db.CoursesSchedules
                    .Include(x=>x.Courses)
                    .Include(x=>x.Courses.Subjects)
                    .Where(x => x.Status != 9 && x.Courses.ClassesId == ClasseId
                    ).Count();
                var Info = db.CoursesSchedules
                    .Include(x => x.Courses)
                    .Where(x => x.Status != 9 && x.Courses.ClassesId == ClasseId
                    ).Select(x => new
                    {
                        x.Id,
                        x.Day,
                        x.Number,
                        x.Courses.Subjects.Name,
                        x.CoursesId,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.Day).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("CoursesSchedules/Add")]
        public IActionResult AddCoursesSchedules([FromBody] CoursesSchedulesBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.Day<=0 || bodyObject.Day>7)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "اليوم");
                
                if (bodyObject.Number<=0 || bodyObject.Number>7)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "رقم المحاضرة ");
                
                var isExist = db.CoursesSchedules.Where(x => x.CoursesId == bodyObject.CoursesId
                    &&  x.Day ==bodyObject.Day
                    && x.Number ==bodyObject.Number
                    && x.Status != 9
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                CoursesSchedules row = new CoursesSchedules();
                row.CoursesId = bodyObject.CoursesId;
                row.Day = bodyObject.Day;
                row.Number = bodyObject.Number;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.CoursesSchedules.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "CoursesSchedules/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost("CoursesSchedules/Edit")]
        public IActionResult EditCoursesSchedules([FromBody] CoursesSchedulesBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.Day<=0 || bodyObject.Day>7)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "اليوم");
                
                if (bodyObject.Number<=0 || bodyObject.Number>7)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "رقم المحاضرة ");
                
                var isExist = db.CoursesSchedules.Where(x => x.CoursesId == bodyObject.CoursesId
                    &&  x.Day ==bodyObject.Day
                    && x.Number ==bodyObject.Number
                    && x.Status != 9
                    && x.Id!=bodyObject.Id
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                var row = db.CoursesSchedules.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Day,
                    row.Number,
                    row.Status
                });

                row.CoursesId = bodyObject.CoursesId;
                row.Day = bodyObject.Day;
                row.Number = bodyObject.Number;

                rowTrans.ItemId = bodyObject.Id;
                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "CoursesSchedules/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost("{Id}/CoursesSchedules/Delete")]
        public IActionResult DeleteCoursesSchedules(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.CoursesSchedules.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "Courses/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Number,
                    row.Day,
                    row.Status
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }





        //Students
        public partial class StudentsBodyObject
        {
            public long Id { get; set; }
            public long ClasseId { get; set; }
            public long StudentId { get; set; }
            public int Value { get; set; }
            public string Descriptions { get; set; }
        }

        //Courses
        [HttpGet("Students/Get")]
        public IActionResult GetStudents(string Search,long ClasseId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.ClassesStudents.Where(x => x.Status != 9 && x.ClassesId ==ClasseId).Count(),
                    Active = db.ClassesStudents.Where(x => x.Status == 1 && x.ClassesId == ClasseId).Count(),
                    NotActive = db.ClassesStudents.Where(x => x.Status == 2 && x.ClassesId == ClasseId).Count(),
                    Deleted = db.ClassesStudents.Where(x => x.Status == 9 && x.ClassesId == ClasseId).Count(),
                };

                int Count = db.ClassesStudents
                    .Include(x=>x.Student)
                    .Include(x=>x.Student.Parent)
                    .Include(x=>x.Classes)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Student.Name.Contains(Search.Trim()) ||
                        x.Student.Phone.Contains(Search.Trim()) ||
                        x.Student.Parent.Name.Contains(Search.Trim()) ||
                        x.Student.Parent.Phone.Contains(Search.Trim())
                        ))
                    ).Count();
                var Info = db.ClassesStudents
                    .Include(x => x.Student)
                    .Include(x => x.Student.Parent)
                    .Include(x => x.Student.AcademicSpecialization)
                    .Include(x => x.Student.AcademicSpecialization.AcademicLevel)
                    .Include(x => x.Classes)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Student.Name.Contains(Search.Trim()) ||
                        x.Student.Phone.Contains(Search.Trim()) ||
                        x.Student.Parent.Name.Contains(Search.Trim()) ||
                        x.Student.Parent.Phone.Contains(Search.Trim())
                        ))
                    ).Select(x => new
                    {
                        x.Id,
                        x.StudentId,
                        x.Student.Name,
                        x.Student.Phone,
                        x.Student.Parent.Image,
                        ParentPhone=x.Student.Parent.Phone,
                        ParentName=x.Student.Parent.Name,
                        x.Value,
                        x.ClassPrice,
                        x.Descriptions,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Students/Add")]
        public IActionResult AddStudents([FromBody] StudentsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.StudentId<=0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الطالب");

                var ClassInfo = db.Classes.Where(x => x.Id == bodyObject.ClasseId && x.Status != 9).SingleOrDefault();
                if(ClassInfo==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId == ClassInfo.SchoolsId && x.Status == 1).SingleOrDefault();
                if (ActiveProfile == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var isExist = db.ClassesStudents.Where(x => x.ClassesId == bodyObject.ClasseId
                    && x.StudentId ==bodyObject.StudentId
                    && x.Classes.ProfileYearsId == ActiveProfile.Id
                    && x.Status != 9
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);

                ClassesStudents row = new ClassesStudents();
                row.ClassesId = bodyObject.ClasseId; 
                row.StudentId = bodyObject.StudentId;
                row.Value = bodyObject.Value;
                row.ClassPrice = ClassInfo.Price;
                row.Descriptions = bodyObject.Descriptions;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.ClassesStudents.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "ClassesStudents/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Students/Edit")]
        public IActionResult EditStudents([FromBody] StudentsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.StudentId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الطالب");

                var ClassInfo = db.Classes.Where(x => x.Id == bodyObject.ClasseId && x.Status != 9).SingleOrDefault();
                if (ClassInfo == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId == ClassInfo.SchoolsId && x.Status == 1).SingleOrDefault();
                if (ActiveProfile == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var isExist = db.ClassesStudents.Where(x => x.ClassesId == bodyObject.ClasseId
                    && x.StudentId == bodyObject.StudentId
                    && x.Classes.ProfileYearsId == ActiveProfile.Id
                    && x.Status != 9
                    && x.Id!=bodyObject.Id
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);

                var row = db.ClassesStudents.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.StudentId,
                    row.ClassPrice,
                    row.Value,
                });


                row.ClassesId = bodyObject.ClasseId;
                row.StudentId = bodyObject.StudentId;
                row.Value = bodyObject.Value;
                row.ClassPrice = ClassInfo.Price;
                row.Descriptions = bodyObject.Descriptions;

                
                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "ClassesStudents/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Students/Delete")]
        public IActionResult DeleteStudents(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.ClassesStudents.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "ClassesStudents/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.ClassesId,
                    row.StudentId,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }




        //Certificates
        public partial class CertificatesBodyObject
        {
            public long Id { get; set; }
            public long ClasseId { get; set; }
            public long StudentId { get; set; }
            public short Type { get; set; }
            public string Descriptions { get; set; }
            public string ImageName { get; set; }
            public string Image { get; set; }
        }

        //Certificates
        [HttpGet("Certificates/Get")]
        public IActionResult GetCertificates(int pageNo, int pageSize, string Search, long ClasseId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.ClassesCertificates.Where(x => x.Status != 9 && x.ClassesId == ClasseId).Count(),
                    Active = db.ClassesCertificates.Where(x => x.Status == 1 && x.ClassesId == ClasseId).Count(),
                    NotActive = db.ClassesCertificates.Where(x => x.Status == 2 && x.ClassesId == ClasseId).Count(),
                    Deleted = db.ClassesCertificates.Where(x => x.Status == 9 && x.ClassesId == ClasseId).Count(),
                };

                int Count = db.ClassesCertificates
                    .Include(x => x.Student)
                    .Include(x => x.Student.Parent)
                    .Include(x => x.Classes)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Student.Name.Contains(Search.Trim()) ||
                        x.Student.Phone.Contains(Search.Trim()) ||
                        x.Student.Parent.Name.Contains(Search.Trim()) ||
                        x.Student.Parent.Phone.Contains(Search.Trim())
                        ))
                    ).Count();
                var Info = db.ClassesCertificates
                    .Include(x => x.Student)
                    .Include(x => x.Student.Parent)
                    .Include(x => x.Student.AcademicSpecialization)
                    .Include(x => x.Student.AcademicSpecialization.AcademicLevel)
                    .Include(x => x.Classes)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Student.Name.Contains(Search.Trim()) ||
                        x.Student.Phone.Contains(Search.Trim()) ||
                        x.Student.Parent.Name.Contains(Search.Trim()) ||
                        x.Student.Parent.Phone.Contains(Search.Trim())
                        ))
                    ).Select(x => new
                    {
                        x.Id,
                        x.StudentId,
                        x.Student.Name,
                        x.Student.Phone,
                        ParentImage=x.Student.Parent.Image,
                        ParentPhone = x.Student.Parent.Phone,
                        ParentName = x.Student.Parent.Name,
                        x.Type,
                        x.Image,
                        x.Descriptions,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Certificates/Add")]
        public IActionResult AddCertificates([FromBody] CertificatesBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.StudentId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الطالب");

                if(string.IsNullOrEmpty(bodyObject.Image))
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الصحيفة");

                var ClassInfo = db.Classes.Where(x => x.Id == bodyObject.ClasseId && x.Status != 9).SingleOrDefault();
                if (ClassInfo == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId == ClassInfo.SchoolsId && x.Status == 1).SingleOrDefault();
                if (ActiveProfile == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var isExist = db.ClassesCertificates.Where(x => x.ClassesId == bodyObject.ClasseId
                    && x.StudentId == bodyObject.StudentId
                    && x.Classes.ProfileYearsId == ActiveProfile.Id
                    && x.Status != 9
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);

                ClassesCertificates row = new ClassesCertificates();
                row.ClassesId = bodyObject.ClasseId;
                row.StudentId = bodyObject.StudentId;
                row.Type = bodyObject.Type;
                row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);
                row.Descriptions = bodyObject.Descriptions;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.ClassesCertificates.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "ClassesCertificates/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Certificates/Edit")]
        public IActionResult EditCertificates([FromBody] CertificatesBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.StudentId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الطالب");

                var ClassInfo = db.Classes.Where(x => x.Id == bodyObject.ClasseId && x.Status != 9).SingleOrDefault();
                if (ClassInfo == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId == ClassInfo.SchoolsId && x.Status == 1).SingleOrDefault();
                if (ActiveProfile == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var isExist = db.ClassesCertificates.Where(x => x.ClassesId == bodyObject.ClasseId
                    && x.StudentId == bodyObject.StudentId
                    && x.Classes.ProfileYearsId == ActiveProfile.Id
                    && x.Status != 9
                    && x.Id != bodyObject.Id
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);

                var row = db.ClassesCertificates.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.StudentId,
                    row.Image,
                    row.Type,
                });


                row.ClassesId = bodyObject.ClasseId;
                row.StudentId = bodyObject.StudentId;
                if (!string.IsNullOrEmpty(bodyObject.ImageName))
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);
                row.Type = bodyObject.Type;
                row.Descriptions = bodyObject.Descriptions;


                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "ClassesCertificates/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Certificates/Delete")]
        public IActionResult DeleteCertificates(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.ClassesCertificates.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "ClassesCertificates/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.ClassesId,
                    row.StudentId,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }





        //Enrollments
        public partial class EnrollmentsBodyObject
        {
            public long Id { get; set; }
            public long ClasseId { get; set; }
            public long StudentId { get; set; }
            public DateTime? EnrollmentDate { get; set; }
            public short EnrollmentState { get; set; }
            public string Descriptions { get; set; }

        }

        //Enrollments
        [HttpGet("Enrollments/Get")]
        public IActionResult GetEnrollments(int pageNo, int pageSize, string Search, long ClasseId,DateTime? ByDate)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.ClassesEnrollments.Where(x => x.Status != 9 && x.ClassesId == ClasseId).Count(),
                    Active = db.ClassesEnrollments.Where(x => x.EnrollmentState == 1 && x.ClassesId == ClasseId).Count(),
                    NotActive = db.ClassesEnrollments.Where(x => x.EnrollmentState == 2 && x.ClassesId == ClasseId).Count(),
                    Deleted = db.ClassesEnrollments.Where(x => x.Status == 9 && x.ClassesId == ClasseId).Count(),
                };

                int Count = db.ClassesEnrollments
                    .Include(x => x.Student)
                    .Include(x => x.Student.Parent)
                    .Include(x => x.Classes)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                     && ((ByDate.GetValueOrDefault() != null && ByDate.GetValueOrDefault() != DateTime.MinValue) ?
                         x.EnrollmentDate.Value.Date == ByDate.Value.Date : true)
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Student.Name.Contains(Search.Trim()) ||
                        x.Student.Phone.Contains(Search.Trim()) ||
                        x.Student.Parent.Name.Contains(Search.Trim()) ||
                        x.Student.Parent.Phone.Contains(Search.Trim())
                        ))
                    ).Count();
                    
                var Info = db.ClassesEnrollments
                    .Include(x => x.Student)
                    .Include(x => x.Student.Parent)
                    .Include(x => x.Student.AcademicSpecialization)
                    .Include(x => x.Student.AcademicSpecialization.AcademicLevel)
                    .Include(x => x.Classes)
                    .Where(x => x.Status != 9 && x.ClassesId == ClasseId
                     && ((ByDate.GetValueOrDefault() != null && ByDate.GetValueOrDefault() != DateTime.MinValue) ?
                         x.EnrollmentDate.Value.Date == ByDate.Value.Date : true)
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Student.Name.Contains(Search.Trim()) ||
                        x.Student.Phone.Contains(Search.Trim()) ||
                        x.Student.Parent.Name.Contains(Search.Trim()) ||
                        x.Student.Parent.Phone.Contains(Search.Trim())
                        ))
                    ).Select(x => new
                    {
                        x.Id,
                        x.StudentId,
                        x.Student.Name,
                        x.Student.Phone,
                        ParentImage = x.Student.Parent.Image,
                        ParentPhone = x.Student.Parent.Phone,
                        ParentName = x.Student.Parent.Name,
                        x.EnrollmentDate,
                        x.EnrollmentState,
                        x.Descriptions,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Enrollments/AddList")]
        public IActionResult AddEnrollments([FromBody] EnrollmentsBodyObject[] bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                foreach (var item in bodyObject)
                {

                    if (item.EnrollmentState == 0)
                        continue;

                    if (item.StudentId <= 0)
                        return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الطالب");

                    if (item.EnrollmentDate.GetValueOrDefault() == DateTime.MinValue)
                        return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "التاريخ ");

                    var ClassInfo = db.Classes.Where(x => x.Id == item.ClasseId && x.Status != 9).SingleOrDefault();
                    if (ClassInfo == null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                    var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId == ClassInfo.SchoolsId && x.Status == 1).SingleOrDefault();
                    if (ActiveProfile == null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                    var isExist = db.ClassesEnrollments.Where(x => x.ClassesId == item.ClasseId
                        && x.StudentId == item.StudentId
                        && x.Classes.ProfileYearsId == ActiveProfile.Id
                        && x.EnrollmentDate.HasValue
                        && x.EnrollmentDate.Value.Date == item.EnrollmentDate.Value.Date
                        && x.Status != 9
                        ).SingleOrDefault();
                    if (isExist != null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);


                    ClassesEnrollments row = new ClassesEnrollments();
                    row.ClassesId = item.ClasseId;
                    row.StudentId = item.StudentId;
                    row.EnrollmentState = item.EnrollmentState;
                    row.EnrollmentDate = item.EnrollmentDate;
                    row.Descriptions = item.Descriptions;
                    row.CreatedBy = userId;
                    row.CreatedOn = DateTime.Now;
                    row.Status = 1;
                    db.ClassesEnrollments.Add(row);

                    TransactionsObject rowTrans = new TransactionsObject();
                    rowTrans.Operations = TransactionsType.Add;
                    rowTrans.Descriptions = "إضافة بيانات  ";
                    rowTrans.Controller = "ClassesEnrollments/Classes";
                    rowTrans.NewObject = JsonConvert.SerializeObject(item);
                    rowTrans.CreatedBy = userId;
                    transactions.WriteTransactions(rowTrans);
                }

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Enrollments/Add")]
        public IActionResult AddEnrollments([FromBody] EnrollmentsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.StudentId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الطالب");

                if (bodyObject.EnrollmentDate.GetValueOrDefault()==DateTime.MinValue)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "التاريخ ");

                var ClassInfo = db.Classes.Where(x => x.Id == bodyObject.ClasseId && x.Status != 9).SingleOrDefault();
                if (ClassInfo == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId == ClassInfo.SchoolsId && x.Status == 1).SingleOrDefault();
                if (ActiveProfile == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var isExist = db.ClassesEnrollments.Where(x => x.ClassesId == bodyObject.ClasseId
                    && x.StudentId == bodyObject.StudentId
                    && x.Classes.ProfileYearsId == ActiveProfile.Id
                    && x.EnrollmentDate.HasValue
                    && x.EnrollmentDate.Value.Date==bodyObject.EnrollmentDate.Value.Date
                    && x.Status != 9
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                ClassesEnrollments row = new ClassesEnrollments();
                row.ClassesId = bodyObject.ClasseId;
                row.StudentId = bodyObject.StudentId;
                row.EnrollmentState = bodyObject.EnrollmentState;
                row.EnrollmentDate = bodyObject.EnrollmentDate;
                row.Descriptions = bodyObject.Descriptions;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.ClassesEnrollments.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "ClassesEnrollments/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Enrollments/Edit")]
        public IActionResult EditEnrollments([FromBody] EnrollmentsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.StudentId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "الطالب");

                if (bodyObject.EnrollmentDate.GetValueOrDefault() == DateTime.MinValue)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "التاريخ ");

                var ClassInfo = db.Classes.Where(x => x.Id == bodyObject.ClasseId && x.Status != 9).SingleOrDefault();
                if (ClassInfo == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var ActiveProfile = db.ProfileYears.Where(x => x.SchoolsId == ClassInfo.SchoolsId && x.Status == 1).SingleOrDefault();
                if (ActiveProfile == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotActiveProfileYear);

                var isExist = db.ClassesEnrollments.Where(x => x.ClassesId == bodyObject.ClasseId
                    && x.StudentId == bodyObject.StudentId
                    && x.Classes.ProfileYearsId == ActiveProfile.Id
                    && x.EnrollmentDate.HasValue
                    && x.EnrollmentDate.Value.Date == bodyObject.EnrollmentDate.Value.Date
                    && x.Status != 9
                    && x.Id!=bodyObject.Id
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                var row = db.ClassesEnrollments.Where(x => x.Status != 9 && x.Id == bodyObject.Id).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.EnrollmentDate,
                    row.EnrollmentState,
                    row.Status,
                });

                row.StudentId = bodyObject.StudentId;
                row.EnrollmentState = bodyObject.EnrollmentState;
                row.EnrollmentDate = bodyObject.EnrollmentDate;
                row.Descriptions = bodyObject.Descriptions;

                
                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "ClassesEnrollments/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Enrollments/Delete")]
        public IActionResult DeleteEnrollments(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.ClassesEnrollments.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "ClassesEnrollments/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.ClassesId,
                    row.StudentId,
                    row.EnrollmentDate,
                    row.EnrollmentState
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }




        //CoursesExams
        public partial class CoursesExamsBodyObject
        {
            public long Id { get; set; }
            public long CoursesId { get; set; }
            public short Type { get; set; }
            public DateTime ExamDate { get; set; }
            public short Degree { get; set; }
            public string ImageName { get; set; }
            public string Image { get; set; }
            public string Descriptions { get; set; }
        }

        //CoursesExams
        [HttpGet("CoursesExams/Get")]
        public IActionResult GetCoursesExams(int pageNo, int pageSize, string Search, long ClasseId,long CoursesId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.CoursesExams.Include(x=>x.Courses)
                        .Where(x => x.Status != 9 && x.Courses.ClassesId == ClasseId).Count(),
                    Active = db.CoursesExams.Include(x => x.Courses)
                        .Where(x => x.Status == 1 && x.Courses.ClassesId == ClasseId).Count(),
                    NotActive = db.CoursesExams.Include(x => x.Courses)
                        .Where(x => x.Status == 2 && x.Courses.ClassesId == ClasseId).Count(),
                    Deleted = db.CoursesExams.Include(x => x.Courses)
                        .Where(x => x.Status == 9 && x.Courses.ClassesId == ClasseId).Count(),
                };

                int Count = db.CoursesExams
                    .Include(x => x.Courses)
                    .Include(x => x.Courses.Subjects)
                    .Include(x => x.Courses.Teacher)
                    .Where(x => x.Status != 9 
                        && x.Courses.ClassesId == ClasseId
                        && (CoursesId>0 ? x.CoursesId==CoursesId : true)
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Courses.Subjects.Name.Contains(Search.Trim()) ||
                        x.Courses.Teacher.Name.Contains(Search.Trim()) ||
                        x.Courses.Descriptions.Contains(Search.Trim()) || 
                        x.Descriptions.Contains(Search.Trim()) 
                        ))
                    ).Count();
                var Info = db.CoursesExams
                    .Include(x => x.Courses)
                    .Include(x => x.Courses.Subjects)
                    .Include(x => x.Courses.Teacher)
                    .Include(x=>x.ExamsGrades)
                    .Where(x => x.Status != 9
                        && x.Courses.ClassesId == ClasseId
                        && (CoursesId > 0 ? x.CoursesId == CoursesId : true)
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Courses.Subjects.Name.Contains(Search.Trim()) ||
                        x.Courses.Teacher.Name.Contains(Search.Trim()) ||
                        x.Courses.Descriptions.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim())
                        ))
                    ).Select(x => new
                    {
                        x.Id,
                        x.CoursesId,
                        Subjects=x.Courses.Subjects.Name,
                        Teacher=x.Courses.Teacher.Name,
                        x.ExamDate,
                        x.Type,
                        x.Degree,
                        x.Descriptions,
                        StudentCount=x.ExamsGrades.Where(k=>k.Status!=9).Count(),
                        x.Image,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("CoursesExams/Add")]
        public IActionResult AddCoursesExams([FromBody] CoursesExamsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.Degree <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "درجة الاختبار ");

                var isExist = db.CoursesExams.Include(x=>x.Courses)
                    .Where(x => x.CoursesId == bodyObject.CoursesId
                    && x.ExamDate.HasValue
                    && x.ExamDate.Value.Date== bodyObject.ExamDate.Date
                    && x.Status != 9
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                CoursesExams row = new CoursesExams();
                row.CoursesId = bodyObject.CoursesId;
                row.Type = bodyObject.Type;
                row.ExamDate = bodyObject.ExamDate;
                row.Degree = bodyObject.Degree;
                row.Descriptions = bodyObject.Descriptions;
                if (string.IsNullOrEmpty(bodyObject.Image))
                {
                    row.Image = "/Uploads/Cover.png";
                }
                else
                {
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);
                }

                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.CoursesExams.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "CoursesExams/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("CoursesExams/Edit")]
        public IActionResult EditCoursesExams([FromBody] CoursesExamsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.Degree <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "درجة الاختبار ");

                var row = db.CoursesExams.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


                var isExist = db.CoursesExams.Include(x => x.Courses)
                    .Where(x => x.CoursesId == bodyObject.CoursesId
                    && x.ExamDate.HasValue
                    && x.ExamDate.Value.Date == bodyObject.ExamDate.Date
                    && x.Status != 9
                    && x.Id!=bodyObject.Id
                    ).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.CoursesId,
                    row.Type,
                    row.ExamDate,
                    row.Degree,
                    row.Status,
                    row.Image
                });



                row.CoursesId = bodyObject.CoursesId;
                row.ExamDate = bodyObject.ExamDate;
                row.Degree = bodyObject.Degree;
                row.Descriptions = bodyObject.Descriptions;
                if (!string.IsNullOrEmpty(bodyObject.ImageName))
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);


                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "CoursesExams/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessEditOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/CoursesExams/Delete")]
        public IActionResult DeleteCoursesExams(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var row = db.CoursesExams.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "Courses/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.CoursesId,
                    row.Degree,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/CoursesExams/ChangeStatus")]
        public IActionResult ChangeStatusCoursesExams(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.CoursesExams.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                if (row.Status == 1)
                {
                    row.Status = 2;
                }
                else
                {
                    row.Status = 1;
                }


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تغير حالة العرض  ";
                rowTrans.Controller = "Courses/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.CoursesId,
                    row.Degree,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessChangeStatusOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }





        //ExamsGrades
        public partial class ExamsGradesBodyObject
        {
            public long Id { get; set; }
            public long StudentId { get; set; }
            public long ExamId { get; set; }
            public int Degree { get; set; }
            public string Descriptions { get; set; }
        }

        //ExamsGrades
        [HttpGet("ExamsGrades/Get")]
        public IActionResult GetExamsGrades(long ExamId,string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var Statistics = new
                {
                    Count = db.ExamsGrades.Where(x => x.Status != 9 && x.ExamId == ExamId).Count(),
                    FullDegree = db.ExamsGrades
                    .Include(x=>x.Exam)
                    .Where(x => x.Status != 9 && x.ExamId == ExamId  && x.Degree==x.Exam.Degree).Count(),
                    MoreHalf = db.ExamsGrades
                    .Include(x => x.Exam)
                    .Where(x => x.Status != 9 && x.ExamId == ExamId && x.Degree > (x.Exam.Degree/2)).Count(),
                    LessHalf = db.ExamsGrades
                    .Include(x => x.Exam)
                    .Where(x => x.Status != 9 && x.ExamId == ExamId && x.Degree < (x.Exam.Degree/2)).Count(),
                   
                };

                var Info = db.ExamsGrades
                    .Include(x => x.Student)
                    .Include(x => x.Exam)
                    .Where(x => x.Status != 9
                        && x.ExamId == ExamId
                    && (string.IsNullOrEmpty(Search) ? true : (
                        x.Student.Name.Contains(Search.Trim()) ||
                        x.Student.Phone.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim())
                        ))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Student.Name,
                        x.Student.Phone,
                        x.Degree,
                        x.Descriptions,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).ToList();


                return Ok(new { info = Info , Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("ExamsGrades/Add")]
        public IActionResult AddExamsGrades([FromBody] ExamsGradesBodyObject[] bodyObject)
        {
            try
            {
                if (bodyObject == null || bodyObject.Count()<=0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var ExamInfo = db.CoursesExams.Where(x => x.Id == bodyObject[0].ExamId && x.Status != 9).SingleOrDefault();
                if(ExamInfo==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                foreach (var item in bodyObject)
                {
                    if (item.Degree <= 0)
                        continue;

                    if(item.Degree>ExamInfo.Degree)
                        return StatusCode(BackMessages.StatusCode, BackMessages.ExamDegreeNotCorrect);

                    var isExist = db.ExamsGrades
                        .Where(x => x.StudentId == item.StudentId
                        && x.ExamId==item.ExamId
                        && x.Status != 9
                        ).SingleOrDefault();
                    if (isExist != null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                    ExamsGrades row = new ExamsGrades();
                    row.ExamId = item.ExamId;
                    row.StudentId = item.StudentId;
                    row.Degree = item.Degree;
                    row.Descriptions = item.Descriptions;
                    row.CreatedBy = userId;
                    row.CreatedOn = DateTime.Now;
                    row.Status = 1;
                    db.ExamsGrades.Add(row);

                    TransactionsObject rowTrans = new TransactionsObject();
                    rowTrans.Operations = TransactionsType.Add;
                    rowTrans.Descriptions = "إضافة بيانات  ";
                    rowTrans.Controller = "ExamsGrades/Classes";
                    rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                    rowTrans.CreatedBy = userId;
                    transactions.WriteTransactions(rowTrans);
                }

                

                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("ExamsGrades/Edit")]
        public IActionResult EditExamsGrades([FromBody] ExamsGradesBodyObject[] bodyObject)
        {
            try
            {
                if (bodyObject == null || bodyObject.Count() <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var ExamInfo = db.CoursesExams.Where(x => x.Id == bodyObject[0].ExamId && x.Status != 9).SingleOrDefault();
                if (ExamInfo == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                foreach (var item in bodyObject)
                {
                    if (item.Degree <= 0)
                        continue;

                    if (item.Degree > ExamInfo.Degree)
                        return StatusCode(BackMessages.StatusCode, BackMessages.ExamDegreeNotCorrect);

                    var row = db.ExamsGrades.Where(x => x.Id == item.Id && x.Status != 9).SingleOrDefault();
                    if(row==null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                    TransactionsObject rowTrans = new TransactionsObject();
                    rowTrans.OldObject = JsonConvert.SerializeObject(new
                    {
                        row.Id,
                        row.StudentId,
                        row.ExamId,
                        row.Degree,
                        row.Descriptions,
                        row.CreatedBy,
                        row.CreatedOn,
                        row.Status
                    });

                    var isExist = db.ExamsGrades
                        .Where(x => x.StudentId == item.StudentId
                        && x.ExamId == item.ExamId
                        && x.Id!=item.Id
                        && x.Status != 9
                        ).SingleOrDefault();
                    if (isExist != null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.ItemExist);

                    row.ExamId = item.ExamId;
                    row.StudentId = item.StudentId;
                    row.Degree = item.Degree;
                    row.Descriptions = item.Descriptions;

                    
                    rowTrans.Operations = TransactionsType.Edit;
                    rowTrans.ItemId=item.Id;
                    rowTrans.Descriptions = "تعديل بيانات  ";
                    rowTrans.Controller = "ExamsGrades/Classes";
                    rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
                    rowTrans.CreatedBy = userId;
                    transactions.WriteTransactions(rowTrans);
                }



                db.SaveChanges();
                return Ok(BackMessages.SucessAddOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/ExamsGrades/Delete")]
        public IActionResult DeleteExamsGrades(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var row = db.ExamsGrades.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "ExamsGrades/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Degree,
                    row.Descriptions,
                    row.Status
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessDeleteOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/ExamsGrades/ChangeStatus")]
        public IActionResult ChangeStatusExamsGrades(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.ExamsGrades.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                if (row.Status == 1)
                {
                    row.Status = 2;
                }
                else
                {
                    row.Status = 1;
                }


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تغير حالة العرض  ";
                rowTrans.Controller = "ExamsGrades/Classes";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.ExamId,
                    row.StudentId,
                    row.Degree,
                    row.Descriptions,
                    row.Status
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessChangeStatusOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }





    }
}