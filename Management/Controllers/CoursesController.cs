//using Common;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Composition;
//using System.Configuration;
//using System.Linq;
//using Vue.Models;

//namespace Management.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/admin/Courses")]
//    public class CoursesController : Controller
//    {
//        //private Helper help;

//        private readonly TraneemBetaContext db;

//        public CoursesController(IConfiguration iConfig, TraneemBetaContext context)
//        {
//            this.db = context;
//            //help = new Helper(iConfig, context);
//        }


//        public partial class BodyObject
//        {
//            public long Id { get; set; }
//            public short SubjectId { get; set; }
//            public short AcademicSpecializationId { get; set; }
//            public long InstructorId { get; set; }
//            public string Name { get; set; }
//            public string Descriptions { get; set; }
//            public bool IsFree { get; set; }
//            public bool IsDiscount { get; set; }
//            public int Price { get; set; }
//            public int PriceDiscount { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//            public string IntroUrl { get; set; }
//            public string Telgram { get; set; }
//        }



//        [HttpGet("Get")]
//        public IActionResult Get(int pageNo, int pageSize, string Search, bool IsFree, bool IsDiscount
//            , short SalesStatus, short Status)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Courses.Where(x => x.Status != 9 && x.Status != 3).Count(),
//                    Shapter = db.Shapters.Where(x => x.Status != 9 && x.Course.Status != 3).Count(),
//                    Lecture = db.Lectures.Where(x => x.Status != 9 && x.Shapter.Course.Status != 3).Count(),
//                    Attachments = db.LecturesAttashments.Where(x => x.Status != 9 && x.Lecture.Shapter.Course.Status != 3).Count(),
//                };

//                int Count = db.Courses
//                    .Include(x=>x.Instructor)
//                    .Include(x=>x.Instructor.User)
//                    .Include(x=>x.Subject)
//                    .Include(x=>x.AcademicSpecialization)
//                    .Include(x=>x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status != 9 && x.Status != 3
//                    && (IsFree ? x.IsFree==IsFree : true)
//                    && (IsDiscount ? x.IsDiscount == IsDiscount : true)
//                    && (SalesStatus>0? x.SalesStatus == SalesStatus : true)
//                    && (Status > 0? x.Status == Status : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        //x.Rate.ToString().Contains(Search.Trim()) ||
//                        x.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Subject.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
                
//                var Info = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Include(x => x.Subject)
//                    .Include(x => x.AcademicSpecialization)
//                    .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status != 9 && x.Status != 3
//                    && (IsFree ? x.IsFree == IsFree : true)
//                    && (IsDiscount ? x.IsDiscount == IsDiscount : true)
//                    && (SalesStatus > 0 ? x.SalesStatus == SalesStatus : true)
//                    && (Status > 0 ? x.Status == Status : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        //x.Rate.Value.ToString().Contains(Search.Trim()) ||
//                        x.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Subject.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x=>new
//                    {
//                        x.Id,
//                        x.SubjectId,
//                        Subject=x.Subject.Name,
//                        x.AcademicSpecializationId,
//                        AcademicSpecialization=x.AcademicSpecialization.Name,
//                        x.AcademicSpecialization.AcademicLevelId,
//                        AcademicLevel=x.AcademicSpecialization.AcademicLevel.Name,
//                        x.InstructorId,
//                        Instructor=x.Instructor.User.Name,   
//                        x.Name,
//                        x.Descriptions,
//                        x.Rate,
//                        x.IsFree,
//                        x.IsDiscount,
//                        x.Price,
//                        x.PriceDiscount,
//                        x.Image,
//                        x.IntroUrl,
//                        x.Telgram,
//                        x.SalesStatus,
//                        x.CreatedOn,
//                        StudentCount=db.StudentsCourses.Where(k=>k.Status!=9 && k.CourseId==x.Id).Count(),
//                        StuentCash=db.StudentsCourses.Where(k=>k.Status!=9 && k.CourseId==x.Id).Sum(k=>k.Value).GetValueOrDefault(),
//                        CreatedBy=db.Users.Where(k=>k.Id==x.CreatedBy).SingleOrDefault().Name,
//                        x.Status
//                    }) .OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("GetBySubjectSpecialization")]
//        public IActionResult GetBySubjectSpecialization(short SubjectId,short AcademicSpecializationId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                
//                var Info = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Where(x => x.Status != 9 && x.Status != 3 && x.SubjectId==SubjectId 
//                    && x.AcademicSpecializationId==AcademicSpecializationId
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        Instructor=x.Instructor.User.Name,
//                        x.Price,
//                    }).OrderByDescending(x => x.Name).ToList();


//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("GetAll")]
//        public IActionResult GetAll()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.Courses
//                    .Where(x => x.Status == 1).Count();
//                var Info = db.Courses
//                    .Where(x => x.Status == 1).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,

//                    }).OrderByDescending(x => x.Name).ToList();


//                return Ok(new { info = Info, count = Count });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Add")]
//        public IActionResult Add([FromBody] BodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);
                
//                if (string.IsNullOrEmpty(bodyObject.Descriptions))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.DescriptionsEmpty);

//                if(!bodyObject.IsFree && bodyObject.Price<=0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PriceEmpty);
                
//                if(bodyObject.Price> 0 && bodyObject.IsDiscount && bodyObject.PriceDiscount<=0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PriceDiscountEmpty);

//                if(string.IsNullOrEmpty(bodyObject.IntroUrl))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "رابط تعريفي للدورة التدريبية ");




//                var isExist = db.Courses.Where(x => x.Status != 9 &&
//                    x.Name == bodyObject.Name && 
//                    x.SubjectId == bodyObject.SubjectId && 
//                    x.AcademicSpecializationId==bodyObject.AcademicSpecializationId
//                    ).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                Courses row = new Courses();
//                row.SubjectId = bodyObject.SubjectId;
//                row.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
//                row.InstructorId = bodyObject.InstructorId;
//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                row.Price = bodyObject.Price;
//                row.IsFree = bodyObject.IsFree;
//                row.IsDiscount = bodyObject.IsDiscount;
//                row.PriceDiscount=bodyObject.PriceDiscount;
//                if (string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    row.Image = "/Uploads/Cover.png";
//                }
//                else
//                {
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                }

//                row.IntroUrl = bodyObject.IntroUrl;
//                row.Telgram = bodyObject.Telgram;
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.SalesStatus = 2;
//                row.Rate = 2;
//                row.Status = 1;
//                db.Courses.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAddOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Edit")]
//        public IActionResult Edit([FromBody] BodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (string.IsNullOrEmpty(bodyObject.Descriptions))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.DescriptionsEmpty);

//                if (!bodyObject.IsFree && bodyObject.Price <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PriceEmpty);

//                if (string.IsNullOrEmpty(bodyObject.IntroUrl))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.Enter + "رابط تعريفي للدورة التدريبية ");

                

//                var isExist = db.Courses.Where(x => x.Status != 9 && x.Id != bodyObject.Id &&
//                    x.Name == bodyObject.Name &&
//                    x.SubjectId == bodyObject.SubjectId &&
//                    x.AcademicSpecializationId == bodyObject.AcademicSpecializationId
//                    ).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.Courses.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new {
//                    row.Id,
//                    row.SubjectId,
//                    row.AcademicSpecializationId,
//                    row.InstructorId,
//                    row.Name,
//                    row.Descriptions,
//                    row.Rate,
//                    row.IsFree,
//                    row.IsDiscount,
//                    row.Price,
//                    row.PriceDiscount,
//                    row.Image,
//                    row.IntroUrl,
//                    row.Telgram,
//                    row.SalesStatus,
//                    row.Status,
//                });


//                row.SubjectId = bodyObject.SubjectId;
//                row.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
//                row.InstructorId = bodyObject.InstructorId;
//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);

//                row.IntroUrl = bodyObject.IntroUrl;
//                row.Telgram = bodyObject.Telgram;

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessEditOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Delete")]
//        public IActionResult Delete(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var row = db.Courses.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessDeleteOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/ChangeStatus")]
//        public IActionResult ChangeStatus(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Courses.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (row.Status == 1)
//                {
//                    row.Status = 2;
//                }
//                else
//                {
//                    row.Status = 1;
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة العرض  ";
//                rowTrans.Controller = "Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessChangeStatusOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/ChangeSalesStatus")]
//        public IActionResult ChangeSalesStatus(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Courses.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (row.SalesStatus == 1)
//                {
//                    row.SalesStatus = 2;
//                }
//                else
//                {
//                    row.SalesStatus = 1;
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة البيع  ";
//                rowTrans.Controller = "Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessChangeStatusOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Close")]
//        public IActionResult Close(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Courses.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 3;
//                row.SalesStatus = 2;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Close;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "إعلاق الدورة التدريبية ";
//                rowTrans.Controller = "Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessCloseOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }









//        //ClosedCourses
//        [HttpGet("GetClosed")]
//        public IActionResult GetClosed(int pageNo, int pageSize, string Search, bool IsFree, bool IsDiscount)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Courses.Where(x => x.Status != 9 && x.Status==3).Count(),
//                    Shapter = db.Shapters.Where(x => x.Status != 9 && x.Course.Status == 3).Count(),
//                    Lecture = db.Lectures.Where(x => x.Status != 9 && x.Shapter.Course.Status == 3).Count(),
//                    Attachments = db.LecturesAttashments.Where(x => x.Status != 9 && x.Lecture.Shapter.Course.Status==3).Count(),
//                };

//                int Count = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Include(x => x.Subject)
//                    .Include(x => x.AcademicSpecialization)
//                    .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status != 9 && x.Status == 3
//                    && (IsFree ? x.IsFree == IsFree : true)
//                    && (IsDiscount ? x.IsDiscount == IsDiscount : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        //x.Rate.ToString().Contains(Search.Trim()) ||
//                        x.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Subject.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Include(x => x.Subject)
//                    .Include(x => x.AcademicSpecialization)
//                    .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status != 9 && x.Status == 3
//                    && (IsFree ? x.IsFree == IsFree : true)
//                    && (IsDiscount ? x.IsDiscount == IsDiscount : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        //x.Rate.Value.ToString().Contains(Search.Trim()) ||
//                        x.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Subject.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.SubjectId,
//                        Subject = x.Subject.Name,
//                        x.AcademicSpecializationId,
//                        AcademicSpecialization = x.AcademicSpecialization.Name,
//                        x.AcademicSpecialization.AcademicLevelId,
//                        AcademicLevel = x.AcademicSpecialization.AcademicLevel.Name,
//                        x.InstructorId,
//                        Instructor = x.Instructor.User.Name,
//                        x.Name,
//                        x.Descriptions,
//                        x.Rate,
//                        x.IsFree,
//                        x.IsDiscount,
//                        x.Price,
//                        x.PriceDiscount,
//                        x.Image,
//                        x.IntroUrl,
//                        x.Telgram,
//                        x.SalesStatus,
//                        x.CreatedOn,
//                        StudentCount = db.StudentsCourses.Where(k => k.Status != 9 && k.CourseId == x.Id).Count(),
//                        StuentCash = db.StudentsCourses.Where(k => k.Status != 9 && k.CourseId == x.Id).Sum(k => k.Value).GetValueOrDefault(),
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                        x.Status
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }


//        [HttpPost("{Id}/Open")]
//        public IActionResult Open(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Courses.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 2;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Open;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "إعادة فتح الدورة التدريبية ";
//                rowTrans.Controller = "Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessOpenOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }








//        //ChartInfo
//        [HttpGet("Chart/Get")]
//        public IActionResult GetChartInfo(long Id)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Course = db.Courses.Where(x => x.Id == Id && x.Status!=9).SingleOrDefault();
//                if(Course==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                int LectureCount = db.Lectures.Include(x=>x.Shapter).Where(x => x.Shapter.CourseId == Id && x.Status != 9).Count();
//                int ExamsCount = db.Exams.Include(x => x.Shapter).Where(x => x.Shapter.CourseId == Id && x.Status != 9).Count();


//                int ReviewCount = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9).Count();
//                int ReviewCountFive = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.Rate==5).Count();
//                int ReviewCountFour = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.Rate==4).Count();
//                int ReviewCountThree = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.Rate==3).Count();
//                int ReviewCountTwo = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.Rate==2).Count();
//                int ReviewCountOne = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.Rate==1).Count();
//                double ReviewAvg = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.Rate.GetValueOrDefault() > 0).Count()>0 ?
//                    db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.Rate.GetValueOrDefault()>0).Average(x => x.Rate.GetValueOrDefault()) : 0;

//                //ReviewAvg = ReviewCount>0 ? 0 :(float)Math.Round(ReviewAvg, 2);

                
//                // Calculate start of the week (Monday)
//                DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
//                DateTime endOfWeek = startOfWeek.AddDays(7);

//                // Count reviews added this week
//                int ReviewsAddedThisWeek = db.CoursesReview.Where(x => x.CourseId == Id && x.Status != 9 && x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek).Count();






//                //Inrolled Chart 
//                int InrolledCount = db.StudentsCourses.Where(x => x.CourseId == Id && x.Status != 9).Count();
//                int InrolledCountThisWeek = db.StudentsCourses.Where(x => x.CourseId == Id && x.Status != 9 && x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek).Count();

//                //Inrolled Chart This Week
//                int[] DailyEnrolledCounts = new int[8];
//                for (int i = 1; i < 8; i++)
//                {
//                    DateTime day = startOfWeek.AddDays(i); // Get each day of the week
//                    int count = db.StudentsCourses
//                        .Where(x => x.CourseId == Id && x.Status != 9 && x.CreatedOn.Value.Date == day.Date)
//                        .Count();

//                    DailyEnrolledCounts[i] = count; // Store the count in the dictionary
//                }




//                //Cash Flow Chart 
//                int Cash = db.StudentsCourses.Where(x => x.CourseId == Id && x.Status != 9).Sum(x => x.Value).Value;



//                //Top Five Lecute Watching 
//                var TopFiveLectures = db.StudentsLectures
//                    .Include(x => x.Lecture) // Include the Lecture details
//                    .Where(x => x.Status != 9 && x.StudentCourse.CourseId == Id) // Filter by status and CourseId
//                    .GroupBy(x => new { x.Lecture.Id, x.Lecture.Name }) // Group by Lecture Id and Name
//                    .Select(g => new
//                    {
//                        g.Key.Name,
//                        Count = g.Count() // Count of views
//                    })
//                    .OrderByDescending(x => x.Count) // Order by count descending
//                    .Take(6) // Get top 5
//                    .ToList();

//                int[] TopFiveLecturesCount = new int[6];
//                if(TopFiveLectures.Count==6)
//                {
//                    for (int i = 0; i < 6; i++)
//                    {
//                        TopFiveLecturesCount[i] = TopFiveLectures[i].Count; // Store the count in the dictionary
//                    }
//                }
                


//                //Top Student Watching 
//                var TopFiveStudents = db.StudentsLectures
//                    .Include(x => x.StudentCourse) // Include StudentCourse details
//                    .Include(x => x.StudentCourse.Student) // Include Student details (assuming there's a Student navigation property)
//                    .Include(x => x.StudentCourse.Student.User) // Include Student details (assuming there's a Student navigation property)
//                    .Where(x => x.Status != 9 && x.StudentCourse.CourseId == Id) // Filter by status and CourseId
//                    .GroupBy(x => new { x.StudentCourseId, x.StudentCourse.Student.Name
//                        , x.StudentCourse.Student.User.Phone, x.StudentCourse.Student.User.Image }) // Group by StudentCourseId and Student
//                    .Select(g => new
//                    {
//                        g.Key.Name, // Assuming Student has a Name property
//                        g.Key.Phone, // Assuming Student has a Name property
//                        g.Key.Image, // Assuming Student has a Name property
//                        Count = g.Count() // Count of lectures watched
//                    })
//                    .OrderByDescending(x => x.Count) // Order by watch count descending
//                    .Take(4) // Get top 5
//                    .ToList();

//                var Info = new
//                {
//                    LectureCount,
//                    ExamsCount,

//                    ReviewCount,
//                    ReviewCountFive,
//                    ReviewCountFour,
//                    ReviewCountThree,
//                    ReviewCountTwo,
//                    ReviewCountOne,
//                    ReviewAvg,

//                    ReviewsAddedThisWeek,


//                    InrolledCount,
//                    InrolledCountThisWeek,
//                    DailyEnrolledCounts,

//                    Cash,

//                    TopFiveLectures,
//                    TopFiveLecturesCount,
//                    TopFiveStudents,
//                };


//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }


//        [HttpGet("Chart/GetAll")]
//        public IActionResult GetAllChartInfo()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                int LectureCount = db.Lectures.Include(x => x.Shapter).Where(x =>  x.Status != 9).Count();
//                int ExamsCount = db.Exams.Include(x => x.Shapter).Where(x => x.Status != 9).Count();


//                int ReviewCount = db.CoursesReview.Where(x => x.Status != 9).Count();
//                int ReviewCountFive = db.CoursesReview.Where(x =>x.Status != 9 && x.Rate == 5).Count();
//                int ReviewCountFour = db.CoursesReview.Where(x =>  x.Status != 9 && x.Rate == 4).Count();
//                int ReviewCountThree = db.CoursesReview.Where(x => x.Status != 9 && x.Rate == 3).Count();
//                int ReviewCountTwo = db.CoursesReview.Where(x => x.Status != 9 && x.Rate == 2).Count();
//                int ReviewCountOne = db.CoursesReview.Where(x =>  x.Status != 9 && x.Rate == 1).Count();
//                double ReviewAvg = db.CoursesReview.Where(x =>  x.Status != 9 && x.Rate.GetValueOrDefault() > 0).Count() > 0 ?
//                    db.CoursesReview.Where(x => x.Status != 9 && x.Rate.GetValueOrDefault() > 0).Average(x => x.Rate.GetValueOrDefault()) : 0;

//                //ReviewAvg = ReviewCount>0 ? 0 :(float)Math.Round(ReviewAvg, 2);


//                // Calculate start of the week (Monday)
//                DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
//                DateTime endOfWeek = startOfWeek.AddDays(7);

//                // Count reviews added this week
//                int ReviewsAddedThisWeek = db.CoursesReview.Where(x => x.Status != 9 && x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek).Count();






//                //Inrolled Chart 
//                int InrolledCount = db.StudentsCourses.Where(x =>  x.Status != 9).Count();
//                int InrolledCountThisWeek = db.StudentsCourses.Where(x =>  x.Status != 9 && x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek).Count();

//                //Inrolled Chart This Week
//                int[] DailyEnrolledCounts = new int[8];
//                for (int i = 1; i < 8; i++)
//                {
//                    DateTime day = startOfWeek.AddDays(i); // Get each day of the week
//                    int count = db.StudentsCourses
//                        .Where(x =>  x.Status != 9 && x.CreatedOn.Value.Date == day.Date)
//                        .Count();

//                    DailyEnrolledCounts[i] = count; // Store the count in the dictionary
//                }




//                //Cash Flow Chart 
//                int Cash = db.StudentsCourses.Where(x => x.Status != 9).Sum(x => x.Value).Value;



//                //Top Five Lecute Watching 
//                var TopFiveLectures = db.StudentsLectures
//                    .Include(x => x.Lecture) // Include the Lecture details
//                    .Where(x => x.Status != 9) // Filter by status and CourseId
//                    .GroupBy(x => new { x.Lecture.Id, x.Lecture.Name }) // Group by Lecture Id and Name
//                    .Select(g => new
//                    {
//                        g.Key.Name,
//                        Count = g.Count() // Count of views
//                    })
//                    .OrderByDescending(x => x.Count) // Order by count descending
//                    .Take(6) // Get top 5
//                    .ToList();

//                int[] TopFiveLecturesCount = new int[6];
//                if (TopFiveLectures.Count == 6)
//                {
//                    for (int i = 0; i < 6; i++)
//                    {
//                        TopFiveLecturesCount[i] = TopFiveLectures[i].Count; // Store the count in the dictionary
//                    }
//                }



//                //Top Student Watching 
//                var TopFiveStudents = db.StudentsLectures
//                    .Include(x => x.StudentCourse) // Include StudentCourse details
//                    .Include(x => x.StudentCourse.Student) // Include Student details (assuming there's a Student navigation property)
//                    .Include(x => x.StudentCourse.Student.User) // Include Student details (assuming there's a Student navigation property)
//                    .Where(x => x.Status != 9) // Filter by status and CourseId
//                    .GroupBy(x => new {
//                        x.StudentCourseId,
//                        x.StudentCourse.Student.Name
//                        ,
//                        x.StudentCourse.Student.User.Phone,
//                        x.StudentCourse.Student.User.Image
//                    }) // Group by StudentCourseId and Student
//                    .Select(g => new
//                    {
//                        g.Key.Name, // Assuming Student has a Name property
//                        g.Key.Phone, // Assuming Student has a Name property
//                        g.Key.Image, // Assuming Student has a Name property
//                        Count = g.Count() // Count of lectures watched
//                    })
//                    .OrderByDescending(x => x.Count) // Order by watch count descending
//                    .Take(4) // Get top 5
//                    .ToList();




//                //Top Five Courses Watching 
//                var TopFiveCourses = db.StudentsCourses
//                    .Include(x => x.Course) // Include the Lecture details
//                    .Include(x => x.Course.AcademicSpecialization) // Include the Lecture details
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel) // Include the Lecture details
//                    .Where(x => x.Status != 9) // Filter by status and CourseId
//                    .GroupBy(x => new {
//                        x.CourseId,
//                        x.Course.Name,
//                        AcademicSpecialization=x.Course.AcademicSpecialization.Name,
//                        AcademicLevel = x.Course.AcademicSpecialization.AcademicLevel.Name
//                    }) // Group by Lecture Id and Name
//                    .Select(g => new
//                    {
//                        g.Key.Name,
//                        g.Key.AcademicLevel,
//                        g.Key.AcademicSpecialization,
//                        Count = g.Count() // Count of views
//                    })
//                    .OrderByDescending(x => x.Count) // Order by count descending
//                    .Take(5) // Get top 5
//                    .ToList();


//                //Top Five Courses Watching 
//                var TopFiveCoursesCash = db.StudentsCourses
//                    .Include(x => x.Course) // Include the Lecture details
//                    .Include(x => x.Course.AcademicSpecialization) // Include the Lecture details
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel) // Include the Lecture details
//                    .Where(x => x.Status != 9) // Filter by status and CourseId
//                    .GroupBy(x => new {
//                        x.CourseId,
//                        x.Course.Name,
//                        AcademicSpecialization = x.Course.AcademicSpecialization.Name,
//                        AcademicLevel = x.Course.AcademicSpecialization.AcademicLevel.Name
//                    }) // Group by Lecture Id and Name
//                    .Select(g => new
//                    {
//                        g.Key.Name,
//                        g.Key.AcademicLevel,
//                        g.Key.AcademicSpecialization,
//                        Cash = g.Sum(x => x.Value)// Count of views
//                    })
//                    .OrderByDescending(x => x.Cash) // Order by count descending
//                    .Take(5) // Get top 5
//                    .ToList();


//                // Top Students by Total Payment
//                var TopStudentsByPayment = db.StudentsCourses
//                    .Include(x => x.Student) // Include the Student details
//                    .Include(x => x.Student.User) // Include the Student details
//                    .Where(x => x.Status != 9) // Filter by status
//                    .GroupBy(x => new {
//                        x.StudentId,
//                        x.Student.Name, // Assuming Student has a Name property
//                        x.Student.User.Image, // Assuming Student has a Name property
//                        x.Student.User.Phone, // Assuming Student has a Name property
//                    }) // Group by Student Id and Name
//                    .Select(g => new
//                    {
//                        g.Key.StudentId,
//                        g.Key.Name,
//                        g.Key.Phone,
//                        g.Key.Image,
//                        Cash = g.Sum(x => x.Value) // Sum of payments
//                    })
//                    .OrderByDescending(x => x.Cash) // Order by total payment descending
//                    .Take(5) // Get top 5
//                    .ToList();


//                var Info = new
//                {
//                    LectureCount,
//                    ExamsCount,

//                    ReviewCount,
//                    ReviewCountFive,
//                    ReviewCountFour,
//                    ReviewCountThree,
//                    ReviewCountTwo,
//                    ReviewCountOne,
//                    ReviewAvg,

//                    ReviewsAddedThisWeek,


//                    InrolledCount,
//                    InrolledCountThisWeek,
//                    DailyEnrolledCounts,

//                    Cash,

//                    TopFiveLectures,
//                    TopFiveLecturesCount,
//                    TopFiveStudents,

//                    TopFiveCourses,
//                    TopFiveCoursesCash,
//                    TopStudentsByPayment,
//                };


//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }





//        //Shapters
//        public partial class ShaptersBodyObject
//        {
//            public long? Id { get; set; }
//            public long CourseId { get; set; }
//            public string Name { get; set; }
//            public short Number { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//            public string Descriptions { get; set; }
//        }
        
//        [HttpGet("Shapters/Get")]
//        public IActionResult GetCourseShpaters(long Id,string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Shapters.Where(x => x.Status != 9 && x.CourseId==Id).Count(),
//                    Active = db.Shapters.Where(x => x.Status == 1 && x.CourseId == Id).Count(),
//                    NotActive = db.Shapters.Where(x => x.Status == 2 && x.CourseId == Id).Count(),
//                    Deleted = db.Shapters.Where(x => x.Status == 9 && x.CourseId == Id).Count(),
//                };

//                int Count = db.Shapters
//                    .Where(x => x.Status != 9 && x.CourseId == Id
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.Shapters
//                    .Where(x => x.Status != 9 && x.CourseId == Id
//                     && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.Image,
//                        x.Descriptions,
//                        x.Number,
//                        x.CourseId,
//                        LecturesCount = db.Lectures.Where(k=>k.Status!=9 && k.ShapterId==x.Id).Count(),
//                        ExamsCount = db.Exams.Where(k=>k.Status!=9 && k.ShapterId==x.Id).Count(),
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderBy(x => x.Number).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Shapters/GetAll")]
//        public IActionResult GetAllShapters(long CourseId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.Shapters
//                    .Where(x => x.Status != 9 && x.CourseId == CourseId).Count();
//                var Info = db.Shapters
//                    .Where(x => x.Status != 9 && x.CourseId == CourseId).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                    }).OrderByDescending(x => x.Name).ToList();


//                return Ok(new { info = Info, count = Count });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Shapters/Add")]
//        public IActionResult AddShapters([FromBody] ShaptersBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                if (string.IsNullOrWhiteSpace(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);
                
//                if (bodyObject.Number<=0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);


//                var isExist = db.Shapters.Where(x => x.Name == bodyObject.Name 
//                        && x.CourseId==bodyObject.CourseId 
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);
                
//                isExist = db.Shapters.Where(x => x.Number == bodyObject.Number 
//                        && x.CourseId==bodyObject.CourseId 
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                Shapters row = new Shapters();
//                row.Name = bodyObject.Name;
//                row.Number = bodyObject.Number;
//                row.CourseId = bodyObject.CourseId;
//                row.Descriptions = bodyObject.Descriptions;
//                if (string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    row.Image = "/Uploads/Cover.png";
//                }
//                else
//                {
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                }

//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.Shapters.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Shapters/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAddOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Shapters/Edit")]
//        public IActionResult EditShapters([FromBody] ShaptersBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if(string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);
                
//                if(bodyObject.Number<=0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.Shapters.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.CourseId,
//                    row.Number,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });

//                var isExist = db.Shapters.Where(x => x.Name == bodyObject.Name 
//                    && x.CourseId ==bodyObject.CourseId
//                    && x.Status != 9 
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);
                
//                isExist = db.Shapters.Where(x => x.Number == bodyObject.Number 
//                    && x.CourseId ==bodyObject.CourseId
//                    && x.Status != 9 
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                row.Name = bodyObject.Name;
//                row.Number = bodyObject.Number;
//                row.Descriptions = bodyObject.Descriptions;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "Shapter/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.CourseId,
//                    row.Number,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();

//                return Ok(BackMessages.SucessEditOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Shapters/Delete")]
//        public IActionResult DeleteShapters(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Shapters.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Shapters/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.CourseId,
//                    row.Number,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessDeleteOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Shapters/ChangeStatus")]
//        public IActionResult ChangeStatusShapters(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Shapters.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (row.Status == 1)
//                {
//                    row.Status = 2;
//                }
//                else
//                {
//                    row.Status = 1;
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة العرض  ";
//                rowTrans.Controller = "Shapters/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Number,
//                    row.CourseId,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessChangeStatusOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }









//        //Lectures
//        public partial class LecturesBodyObject
//        {
//            public long? Id { get; set; }
//            public long ShapterId { get; set; }
//            public string Name { get; set; }
//            public short Number { get; set; }
//            public string Descriptions { get; set; }
//        }

//        [HttpGet("Lectures/Get")]
//        public IActionResult GetCourseLectures(long CourseId, long ShapterId, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Lectures.Where(x => x.Status != 9
//                        && x.Shapter.CourseId == CourseId
//                        && (ShapterId>0 ? x.ShapterId == ShapterId : true)).Count(),

//                    Active = db.Lectures.Where(x => x.Status == 1 
//                        && x.Shapter.CourseId == CourseId 
//                        && (ShapterId > 0 ? x.ShapterId == ShapterId : true)).Count(),

//                    NotActive = db.Lectures.Where(x => x.Status == 2 
//                        && x.Shapter.CourseId == CourseId 
//                        && (ShapterId > 0 ? x.ShapterId == ShapterId : true)).Count(),

//                    Deleted = db.Lectures.Where(x => x.Status == 9 
//                        && x.Shapter.CourseId == CourseId 
//                        && (ShapterId > 0 ? x.ShapterId == ShapterId : true)).Count(),
//                };

//                int Count = db.Lectures
//                    .Include(x=>x.Shapter)
//                    .Where(x => x.Status != 9 && x.Shapter.CourseId == CourseId
//                    && (ShapterId > 0 ? x.ShapterId == ShapterId : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        //x.Shapter.Name.ToString().Contains(Search.Trim()) ||
//                        //x.Shapter.Number.Value.ToString().Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.Lectures
//                    .Include(x => x.Shapter)
//                    .Where(x => x.Status != 9 && x.Shapter.CourseId == CourseId
//                    && (ShapterId > 0 ? x.ShapterId == ShapterId : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        //x.Shapter.Name.ToString().Contains(Search.Trim()) ||
//                        //x.Shapter.Number.Value.ToString().Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.Descriptions,
//                        x.Number,
//                        Shapter=x.Shapter.Name,
//                        ShapterNumber=x.Shapter.Number,
//                        x.ShapterId,
//                        AttachmentsCount = db.Lectures.Where(k => k.Status != 9 && k.ShapterId == x.Id).Count(),
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderBy(x => x.Number).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Lectures/Add")]
//        public IActionResult AddLectures([FromBody] LecturesBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                if (string.IsNullOrWhiteSpace(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);


//                var isExist = db.Lectures.Where(x => x.Name == bodyObject.Name
//                        && x.ShapterId == bodyObject.ShapterId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.Lectures.Where(x => x.Number == bodyObject.Number
//                        && x.ShapterId == bodyObject.ShapterId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                Lectures row = new Lectures();
//                row.Name = bodyObject.Name;
//                row.Number = bodyObject.Number;
//                row.ShapterId = bodyObject.ShapterId;
//                row.Descriptions = bodyObject.Descriptions;
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.Lectures.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Lectures/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAddOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Lectures/Edit")]
//        public IActionResult EditLectures([FromBody] LecturesBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.Lectures.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.ShapterId,
//                    row.Rate,
//                    row.Number,
//                    row.Descriptions,
//                    row.Status
//                });

//                var isExist = db.Lectures.Where(x => x.Name == bodyObject.Name
//                    && x.ShapterId == bodyObject.ShapterId
//                    && x.Status != 9
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.Lectures.Where(x => x.Number == bodyObject.Number
//                    && x.ShapterId == bodyObject.ShapterId
//                    && x.Status != 9
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                row.ShapterId = bodyObject.ShapterId;
//                row.Name = bodyObject.Name;
//                row.Number = bodyObject.Number;
//                row.Descriptions = bodyObject.Descriptions;

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "Lectures/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.ShapterId,
//                    row.Number,
//                    row.Descriptions,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();

//                return Ok(BackMessages.SucessEditOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Lectures/Delete")]
//        public IActionResult DeleteLectures(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Lectures.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Lectures/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.ShapterId,
//                    row.Number,
//                    row.Descriptions,
//                    row.Rate,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessDeleteOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Lectures/ChangeStatus")]
//        public IActionResult ChangeStatusLectures(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Lectures.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (row.Status == 1)
//                {
//                    row.Status = 2;
//                }
//                else
//                {
//                    row.Status = 1;
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة العرض  ";
//                rowTrans.Controller = "Lectures/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Number,
//                    row.ShapterId,
//                    row.Descriptions,
//                    row.Rate,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessChangeStatusOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }










//        //LecturesAttashments
//        public partial class LecturesAttashmentsBodyObject
//        {
//            public long? Id { get; set; }
//            public long LectureId { get; set; }
//            public short Type { get; set; }
//            public string Name { get; set; }
//            public short Number { get; set; }
//            public string Descriptions { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//        }

//        [HttpGet("LecturesAttashments/Get")]
//        public IActionResult GetCourseLecturesAttashments(long LectureId, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.LecturesAttashments.Where(x => x.Status != 9 && (LectureId > 0 ? x.LectureId == LectureId : true)).Count(),
//                    Active = db.LecturesAttashments.Where(x => x.Status == 1 && (LectureId > 0 ? x.LectureId == LectureId : true)).Count(),
//                    NotActive = db.LecturesAttashments.Where(x => x.Status == 2 && (LectureId > 0 ? x.LectureId == LectureId : true)).Count(),
//                    Deleted = db.LecturesAttashments.Where(x => x.Status == 9 && (LectureId > 0 ? x.LectureId == LectureId : true)).Count(),
//                };

//                int Count = db.LecturesAttashments
//                    .Include(x => x.Lecture)
//                    .Where(x => x.Status != 9 && x.LectureId==LectureId
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.LecturesAttashments
//                    .Include(x => x.Lecture)
//                    .Where(x => x.Status != 9 && x.LectureId == LectureId
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.Descriptions,
//                        x.Number,
//                        x.LectureId,
//                        x.Path,
//                        x.Type,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderBy(x => x.Number).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("LecturesAttashments/Add")]
//        public IActionResult AddLecturesAttashments([FromBody] LecturesAttashmentsBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                if (string.IsNullOrWhiteSpace(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);

//                if (string.IsNullOrEmpty(bodyObject.Image))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.AttachnmentsEmpty);


//                var isExist = db.LecturesAttashments.Where(x => x.Name == bodyObject.Name
//                        && x.LectureId == bodyObject.LectureId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.LecturesAttashments.Where(x => x.Number == bodyObject.Number
//                        && x.LectureId == bodyObject.LectureId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                LecturesAttashments row = new LecturesAttashments();
//                row.Name = bodyObject.Name;
//                row.Number = bodyObject.Number;
//                row.LectureId = bodyObject.LectureId;
//                row.Type = bodyObject.Type;
//                row.Descriptions = bodyObject.Descriptions;
//                row.Path = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.LecturesAttashments.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "LecturesAttashments/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAddOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("LecturesAttashments/Edit")]
//        public IActionResult EditLecturesAttashments([FromBody] LecturesAttashmentsBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.LecturesAttashments.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.LectureId,
//                    row.Number,
//                    row.Type,
//                    row.Descriptions,
//                    row.Path,
//                    row.Status
//                });

//                var isExist = db.LecturesAttashments.Where(x => x.Name == bodyObject.Name
//                    && x.LectureId == bodyObject.LectureId
//                    && x.Status != 9
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.LecturesAttashments.Where(x => x.Number == bodyObject.Number
//                    && x.LectureId == bodyObject.LectureId
//                    && x.Status != 9
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                row.Name = bodyObject.Name;
//                row.Number = bodyObject.Number;
//                row.Descriptions = bodyObject.Descriptions;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    row.Path = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                    row.Type = bodyObject.Type;
//                }

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "LecturesAttashments/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.LectureId,
//                    row.Number,
//                    row.Descriptions,
//                    row.Path,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();

//                return Ok(BackMessages.SucessEditOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/LecturesAttashments/Delete")]
//        public IActionResult DeleteLecturesAttashments(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.LecturesAttashments.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "LecturesAttashments/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.LectureId,
//                    row.Number,
//                    row.Descriptions,
//                    row.Path,
//                    row.Type,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessDeleteOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/LecturesAttashments/ChangeStatus")]
//        public IActionResult ChangeStaLecturesAttashments(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.LecturesAttashments.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (row.Status == 1)
//                {
//                    row.Status = 2;
//                }
//                else
//                {
//                    row.Status = 1;
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة العرض  ";
//                rowTrans.Controller = "LecturesAttashments/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Number,
//                    row.LectureId,
//                    row.Descriptions,
//                    row.Path,
//                    row.Type,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessChangeStatusOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }








//        //Exams
//        public partial class ExamsBodyObject
//        {
//            public long? Id { get; set; }
//            public long ShapterId { get; set; }
//            public string Name { get; set; }
//            public short Number { get; set; }
//            public string Descriptions { get; set; }
//            public bool HasLimght { get; set; }
//            public short? Limght { get; set; }
//            public short? Marck { get; set; }
//            public short? SucessMarck { get; set; }
//        }

//        [HttpGet("Exams/Get")]
//        public IActionResult GetCourseExams(long CourseId,long ShapterId, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Exams.Where(x => x.Status != 9 
//                        && x.Shapter.CourseId==CourseId 
//                        && (ShapterId > 0 ? x.ShapterId == ShapterId : true)).Count(),

//                    Active = db.Exams.Where(x => x.Status == 1 
//                        && x.Shapter.CourseId == CourseId 
//                        && (ShapterId > 0 ? x.ShapterId == ShapterId : true)).Count(),

//                    NotActive = db.Exams.Where(x => x.Status == 2 
//                        && x.Shapter.CourseId == CourseId 
//                        && (ShapterId > 0 ? x.ShapterId == ShapterId : true)).Count(),

//                    Deleted = db.Exams.Where(x => x.Status == 9 
//                        && x.Shapter.CourseId == CourseId 
//                        && (ShapterId > 0 ? x.ShapterId == ShapterId : true)).Count(),
//                };

//                int Count = db.Exams
//                    .Include(x => x.Shapter)
//                    .Where(x => x.Status != 9 && x.Shapter.CourseId == CourseId
//                    && (ShapterId > 0 ? x.ShapterId == ShapterId : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.Exams
//                    .Include(x => x.Shapter)
//                    .Where(x => x.Status != 9 && x.Shapter.CourseId == CourseId
//                    && (ShapterId > 0 ? x.ShapterId == ShapterId : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.Descriptions,
//                        x.Number,
//                        Shapter = x.Shapter.Name,
//                        ShapterNumber = x.Shapter.Number,
//                        x.ShapterId,
//                        x.CountQuestions,
//                        x.SucessMarck,
//                        x.CountStudent,
//                        x.CountStudentPass,
//                        x.HasLimght,
//                        x.Limght,
//                        x.Marck,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderBy(x => x.Number).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Exams/Add")]
//        public IActionResult AddExams([FromBody] ExamsBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                if (string.IsNullOrWhiteSpace(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);
                
//                if (bodyObject.Marck <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.MarckEmpty);
                
//                if (bodyObject.SucessMarck <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.MarckSuccessEmpty);
                
//                if (bodyObject.HasLimght && bodyObject.Limght<=0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.LimghtEmpty);

//                if(bodyObject.SucessMarck>bodyObject.Marck)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.MarckSucceRong);


//                var isExist = db.Exams.Where(x => x.Name == bodyObject.Name
//                        && x.ShapterId == bodyObject.ShapterId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.Exams.Where(x => x.Number == bodyObject.Number
//                        && x.ShapterId == bodyObject.ShapterId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                Exams row = new Exams();
//                row.ShapterId = bodyObject.ShapterId;
//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                row.Number = bodyObject.Number;
//                row.HasLimght = bodyObject.HasLimght;
//                row.Limght = bodyObject.Limght;
//                row.Marck = bodyObject.Marck;
//                row.SucessMarck = bodyObject.SucessMarck;
//                row.CountQuestions = 0;
//                row.CountStudent = 0;
//                row.CountStudentPass = 0;
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.Exams.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Exams/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAddOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Exams/Edit")]
//        public IActionResult EditExams([FromBody] ExamsBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);

//                if (bodyObject.Marck <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.MarckEmpty);

//                if (bodyObject.SucessMarck <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.MarckSuccessEmpty);

//                if (bodyObject.HasLimght && bodyObject.Limght <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.LimghtEmpty);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.Exams.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.ShapterId,
//                    row.Rate,
//                    row.Number,
//                    row.Descriptions,
//                    row.HasLimght,
//                    row.Limght,
//                    row.Marck,
//                    row.SucessMarck,
//                    row.CountQuestions,
//                    row.Status
//                });

//                var isExist = db.Exams.Where(x => x.Name == bodyObject.Name
//                    && x.ShapterId == bodyObject.ShapterId
//                    && x.Status != 9
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.Exams.Where(x => x.Number == bodyObject.Number
//                    && x.ShapterId == bodyObject.ShapterId
//                    && x.Status != 9
//                    && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                row.ShapterId = bodyObject.ShapterId;
//                row.Name = bodyObject.Name;
//                row.Number = bodyObject.Number;
//                row.Descriptions = bodyObject.Descriptions;
//                row.Marck = bodyObject.Marck;
//                row.SucessMarck = bodyObject.SucessMarck;
//                row.HasLimght = bodyObject.HasLimght;
//                row.Limght = bodyObject.Limght;

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "Exams/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.ShapterId,
//                    row.Number,
//                    row.Descriptions,
//                    row.HasLimght,
//                    row.Limght,
//                    row.Marck,
//                    row.SucessMarck,
//                    row.CountQuestions,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();

//                return Ok(BackMessages.SucessEditOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Exams/Delete")]
//        public IActionResult DeleteExams(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Exams.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Exams/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.ShapterId,
//                    row.Number,
//                    row.Descriptions,
//                    row.SucessMarck,
//                    row.CountStudent,
//                    row.CountQuestions,
//                    row.Marck,
//                    row.Rate,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessDeleteOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Exams/ChangeStatus")]
//        public IActionResult ChangeStatusExams(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.Exams.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (row.Status == 1)
//                {
//                    row.Status = 2;
//                }
//                else
//                {
//                    row.Status = 1;
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة العرض  ";
//                rowTrans.Controller = "Exams/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Number,
//                    row.ShapterId,
//                    row.Descriptions,
//                    row.CountQuestions,
//                    row.Marck,
//                    row.ScoreMarck,
//                    row.CountStudent,
//                    row.CountStudentPass,
//                    row.Rate,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessChangeStatusOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }





//        //ExamsQuestions
//        public partial class ExamsQuestionsBodyObject
//        {
//            public long? Id { get; set; }
//            public long ExamId { get; set; }
//            public string Question { get; set; }
//            public short Marck { get; set; }
//            public short Number { get; set; }
//            public short Type { get; set; }
//            public short Answer { get; set; }
//            public string CompleteAnswer { get; set; }
//            public string A1 { get; set; }
//            public string A2 { get; set; }
//            public string A3 { get; set; }
//            public string A4 { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//        }

//        [HttpGet("ExamsQuestions/Get")]
//        public IActionResult GetCourseExamsQuestions(long ExamId, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.ExamsQuestions.Where(x => x.Status != 9 && (ExamId > 0 ? x.ExamId == ExamId : true)).Count(),
//                    Active = db.ExamsQuestions.Where(x => x.Status == 1 && (ExamId > 0 ? x.ExamId == ExamId : true)).Count(),
//                    NotActive = db.ExamsQuestions.Where(x => x.Status == 2 && (ExamId > 0 ? x.ExamId == ExamId : true)).Count(),
//                    Deleted = db.ExamsQuestions.Where(x => x.Status == 9 && (ExamId > 0 ? x.ExamId == ExamId : true)).Count(),
//                };

//                int Count = db.ExamsQuestions
//                    .Include(x => x.Exam)
//                    .Where(x => x.Status != 9 && x.ExamId == ExamId
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Question.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.ExamsQuestions
//                    .Include(x => x.Exam)
//                    .Where(x => x.Status != 9 && x.ExamId == ExamId
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Question.Contains(Search.Trim()) ||
//                        x.Number.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Question,
//                        x.Number,
//                        x.Marck,
//                        x.ExamId,
//                        x.Image,
//                        x.Type,
//                        x.Answer,
//                        x.CompleteAnswer,
//                        x.A1,
//                        x.A2,
//                        x.A3,
//                        x.A4,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderBy(x => x.Number).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("ExamsQuestions/Add")]
//        public IActionResult AddExamsQuestions([FromBody] ExamsQuestionsBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                if (string.IsNullOrWhiteSpace(bodyObject.Question))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.QuestionEmpty);
                
//                if (string.IsNullOrWhiteSpace(bodyObject.CompleteAnswer))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty);

                

//                if(bodyObject.Type==1)
//                {
//                    if (string.IsNullOrWhiteSpace(bodyObject.A1))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A1");

//                    if (string.IsNullOrWhiteSpace(bodyObject.A2))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A2");

//                    if (string.IsNullOrWhiteSpace(bodyObject.A3))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A3");

//                    if (string.IsNullOrWhiteSpace(bodyObject.A4))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A4");
//                }

//                if (bodyObject.Answer <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty);
                
//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);

                


//                var isExist = db.ExamsQuestions.Where(x => x.Question == bodyObject.Question
//                        && x.ExamId == bodyObject.ExamId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.ExamsQuestions.Where(x => x.Number == bodyObject.Number
//                        && x.ExamId == bodyObject.ExamId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                var Exam = db.Exams.Where(x => x.Id == bodyObject.ExamId && x.Status != 9).SingleOrDefault();
//                if(Exam==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                int TotalMark = db.ExamsQuestions.Where(x => x.Status != 9 && x.ExamId == bodyObject.ExamId).Sum(x => x.Marck).Value;
//                TotalMark = TotalMark + bodyObject.Marck;
//                if(TotalMark>Exam.Marck)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.MarckNotCorrectEmpty);

//                Exam.CountQuestions += 1;

//                ExamsQuestions row = new ExamsQuestions();
//                row.ExamId = bodyObject.ExamId;
//                row.Question = bodyObject.Question;
//                row.Number = bodyObject.Number;
//                row.Marck = bodyObject.Marck;
//                row.Type = bodyObject.Type;
//                row.Answer = bodyObject.Answer;
//                row.CompleteAnswer = bodyObject.CompleteAnswer;
//                row.A1 = bodyObject.A1;
//                row.A2 = bodyObject.A2;
//                row.A3 = bodyObject.A3;
//                row.A4 = bodyObject.A4;

//                if (string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    row.Image = "/Uploads/Cover.png";
//                }
//                else
//                {
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                }
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.ExamsQuestions.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "ExamsQuestions/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAddOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("ExamsQuestions/Edit")]
//        public IActionResult EditExamsQuestions([FromBody] ExamsQuestionsBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                if (string.IsNullOrWhiteSpace(bodyObject.Question))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.QuestionEmpty);

//                if (string.IsNullOrWhiteSpace(bodyObject.CompleteAnswer))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty);

//                if (bodyObject.Type == 1)
//                {
//                    if (string.IsNullOrWhiteSpace(bodyObject.A1))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A1");

//                    if (string.IsNullOrWhiteSpace(bodyObject.A2))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A2");

//                    if (string.IsNullOrWhiteSpace(bodyObject.A3))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A3");

//                    if (string.IsNullOrWhiteSpace(bodyObject.A4))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty + " A4");
//                }

//                if (bodyObject.Answer <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.AnswerEmpty);

//                if (bodyObject.Number <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberEmpty);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.ExamsQuestions.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.ExamId,
//                    row.Question,
//                    row.Number,
//                    row.Type,
//                    row.Answer,
//                    row.CompleteAnswer,
//                    row.A1,
//                    row.A2,
//                    row.A3,
//                    row.A4,
//                    row.Image,
//                    row.Status
//                });

//                var isExist = db.ExamsQuestions.Where(x => x.Question == bodyObject.Question
//                        && x.ExamId == bodyObject.ExamId
//                        && x.Id != bodyObject.Id
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                isExist = db.ExamsQuestions.Where(x => x.Number == bodyObject.Number
//                        && x.ExamId == bodyObject.ExamId
//                        && x.Id != bodyObject.Id
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NumberExist);

//                var Exam = db.Exams.Where(x => x.Id == bodyObject.ExamId && x.Status != 9).SingleOrDefault();
//                if (Exam == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


//                int TotalMark = db.ExamsQuestions.Where(x => x.Status != 9 
//                    && x.ExamId == bodyObject.ExamId
//                    && x.Id != bodyObject.Id
//                    ).Sum(x => x.Marck).Value;
//                TotalMark = TotalMark + bodyObject.Marck;
//                if (TotalMark > Exam.Marck)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.MarckNotCorrectEmpty);


//                row.Question = bodyObject.Question;
//                row.Number = bodyObject.Number;
//                row.Marck = bodyObject.Marck;
//                row.Type = bodyObject.Type;
//                row.Answer = bodyObject.Answer;
//                row.CompleteAnswer = bodyObject.CompleteAnswer;
//                row.A1 = bodyObject.A1;
//                row.A2 = bodyObject.A2;
//                row.A3 = bodyObject.A3;
//                row.A4 = bodyObject.A4;

//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                }

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "LecturesAttashments/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.ExamId,
//                    row.Question,
//                    row.Number,
//                    row.Type,
//                    row.Answer,
//                    row.CompleteAnswer,
//                    row.A1,
//                    row.A2,
//                    row.A3,
//                    row.A4,
//                    row.Image,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();

//                return Ok(BackMessages.SucessEditOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/ExamsQuestions/Delete")]
//        public IActionResult DeleteExamsQuestions(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.ExamsQuestions.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var Exam = db.Exams.Where(x => x.Id == row.ExamId && x.Status != 9).SingleOrDefault();
//                if(Exam==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                Exam.Marck -= row.Marck;

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "ExamsQuestions/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.ExamId,
//                    row.Question,
//                    row.Number,
//                    row.Type,
//                    row.Answer,
//                    row.CompleteAnswer,
//                    row.A1,
//                    row.A2,
//                    row.A3,
//                    row.A4,
//                    row.Image,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessDeleteOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/ExamsQuestions/ChangeStatus")]
//        public IActionResult ChangeStatusExamsQuestions(long Id)
//        {
//            try
//            {
//                if (Id <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (user.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var row = db.ExamsQuestions.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (row.Status == 1)
//                {
//                    row.Status = 2;
//                }
//                else
//                {
//                    row.Status = 1;
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة العرض  ";
//                rowTrans.Controller = "ExamsQuestions/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.ExamId,
//                    row.Question,
//                    row.Number,
//                    row.Type,
//                    row.Answer,
//                    row.CompleteAnswer,
//                    row.A1,
//                    row.A2,
//                    row.A3,
//                    row.A4,
//                    row.Image,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessChangeStatusOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }









//        //Students
//        [HttpGet("Students/Get")]
//        public IActionResult GetCourseStudents(long Id, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.StudentsCourses.Where(x => x.Status != 9 && x.CourseId == Id).Count(),
//                    Active = db.StudentsCourses.Where(x => x.Status == 1 && x.CourseId == Id).Count(),
//                    NotActive = db.StudentsCourses.Where(x => x.Status == 2 && x.CourseId == Id).Count(),
//                    Deleted = db.StudentsCourses.Where(x => x.Status == 9 && x.CourseId == Id).Count(),
//                };

//                int Count = db.StudentsCourses
//                    .Include(x=>x.Student)
//                    .Include(x=>x.Student.User)
//                    .Where(x => x.Status != 9 && x.CourseId == Id
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Student.Name.Contains(Search.Trim()) ||
//                        x.Student.User.Phone.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.StudentsCourses
//                    .Include(x => x.Student)
//                    .Include(x => x.Student.User)
//                    .Where(x => x.Status != 9 && x.CourseId == Id
//                     && (string.IsNullOrEmpty(Search) ? true : (x.Student.Name.Contains(Search.Trim()) ||
//                        x.Student.User.Phone.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Student.User.Image,
//                        x.Student.Name,
//                        x.Student.User.Phone,
//                        x.Value,
//                        x.Status,
//                        CountExams=db.StudentsExams.Where(k=>k.StudentCourseId==x.Id && k.Status!=9).Count(),
//                        CountLectures = db.StudentsLectures.Where(k=>k.StudentCourseId==x.Id && k.Status!=9).Count(),
//                        x.Student.Points,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderBy(x => x.Name).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }








//        //Review
//        [HttpGet("Review/Get")]
//        public IActionResult GetCourseReview(long Id, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.CoursesReview.Where(x => x.Status != 9 && x.CourseId == Id).Count(),
//                    Active = db.CoursesReview.Where(x => x.Status == 1 && x.CourseId == Id).Count(),
//                    NotActive = db.CoursesReview.Where(x => x.Status == 2 && x.CourseId == Id).Count(),
//                    Deleted = db.CoursesReview.Where(x => x.Status == 9 && x.CourseId == Id).Count(),
//                };

//                int Count = db.CoursesReview
//                    .Where(x => x.Status != 9 && x.CourseId == Id
//                    && (string.IsNullOrEmpty(Search) ? true : 
//                        (
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.CoursesReview
//                    .Where(x => x.Status != 9 && x.CourseId == Id
//                     && (string.IsNullOrEmpty(Search) ? true : (
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Message,
//                        x.Rate,
//                        db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Phone,
//                        db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Image,
//                        db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                        x.CreatedOn,
//                    }).OrderByDescending(x => x.CreatedOn).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }



//    }
//}