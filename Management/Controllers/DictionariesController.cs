using Common;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.Models;
using Newtonsoft.Json;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using System;
using System.Linq;
using static Common.TransactionsInfo;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/Dictionaries")]
    public class DictionariesController : Controller
    {

        private readonly JeelContext db;
        private Security security;
        private TransactionsInfo transactions;
        private FileHandler fileHandler;

        public DictionariesController(JeelContext context, IConfiguration iConfig)
        {
            this.db = context;
            security = new Security(iConfig, context);
            transactions = new TransactionsInfo(iConfig, context);
            fileHandler = new FileHandler(iConfig, context);
        }




        public partial class BodyObject
        {
            public long? Id { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string ImageName { get; set; }
            public string Descriptions { get; set; }
        }
        
        public partial class SubscriptionsBodyObject
        {
            public long? Id { get; set; }
            public string Name { get; set; }
            public short Lenght { get; set; }
            public short Price { get; set; }
            public string Image { get; set; }
            public string ImageName { get; set; }
            public string Descriptions { get; set; }
        }

        public partial class BodyObjectWithParent
        {
            public long? Id { get; set; }
            public short ParentId { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string ImageName { get; set; }
            public string Descriptions { get; set; }
        }




        [HttpGet("Students/GetAll")]
        public IActionResult GetAllStudents()
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

               
                var Info = db.Students
                    .Include(x=>x.Parent)
                    .Where(x => x.Status !=9).Select(x => new
                    {
                        x.Id,
                        Name= x.Name + " || "+ x.Phone+ " || "+ x.Parent.Name+ " || "+ x.Parent.Phone,
                    }).OrderByDescending(x => x.Name).ToList();


                return Ok(new { info = Info});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        
        [HttpGet("Students/GetAllByClass")]
        public IActionResult GetAllStudentsByClass(long ClassesId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

               
                var Info = db.ClassesStudents
                    .Include(x=>x.Student)
                    .Include(x=>x.Student.Parent)
                    .Where(x => x.Status !=9 && x.ClassesId==ClassesId).Select(x => new
                    {
                        Id=x.StudentId,
                        Name= x.Student.Name + " || "+ x.Student.Phone+ " || "+ x.Student.Parent.Name+ " || "+ x.Student.Parent.Phone,
                    }).OrderByDescending(x => x.Name).ToList();

                return Ok(new { info = Info});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Students/GetAllForEnrollments")]
        public IActionResult GetAllForEnrollments(long ClassesId,DateTime EnrollmentDate)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                short EnrollmentState = 0;
                short Id = 0;

                var Info = db.ClassesStudents
                    .Include(x => x.Student)
                    .Where(x => x.Status != 9 && x.ClassesId == ClassesId 
                        && !db.ClassesEnrollments.Where(k => k.EnrollmentDate.Value.Date == EnrollmentDate
                                      && k.ClassesId == ClassesId
                                      && k.Status != 9)
                         .Select(k => k.StudentId)
                         .Contains(x.StudentId)
                    ).Select(x => new
                    {
                        Id,
                        x.StudentId,
                        x.Student.Name,
                        ClasseId=x.ClassesId,
                        x.Student.Phone,
                        EnrollmentDate,
                        EnrollmentState,
                        x.Descriptions,
                    }).OrderByDescending(x => x.Name).ToList();

                return Ok(new { info = Info });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("Students/GetAllStudentsForDegree")]
        public IActionResult GetAllStudentsByClassForDegree(long ExamId)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                short Id = 0;
                short Degree = 0;

                var ExamInfo = db.CoursesExams
                    .Include(x=>x.Courses)
                    .Where(x => x.Id == ExamId && x.Status != 9).SingleOrDefault();
                if(ExamInfo==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var Info = db.ClassesStudents
                    .Include(x => x.Student)
                    .Where(x => x.Status != 9 && x.ClassesId == ExamInfo.Courses.ClassesId 
                        && !db.ExamsGrades.Where(k => k.ExamId==ExamId
                                      && k.Status != 9)
                         .Select(k => k.StudentId)
                         .Contains(x.StudentId)
                    ).Select(x => new
                    {
                        Id,
                        x.StudentId,
                        x.Student.Name,
                        ExamId,
                        x.Student.Phone,
                        Degree,
                        x.Descriptions,
                    }).OrderByDescending(x => x.Name).ToList();

                return Ok(new { info = Info });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }





        //PaymentsMethods
        [HttpGet("PaymentsMethods/Get")]
        public IActionResult GetPaymentsMethods(int pageNo, int pageSize, string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.PaymentsMethods.Where(x => x.Status != 9).Count(),
                    Active = db.PaymentsMethods.Where(x => x.Status == 1).Count(),
                    NotActive = db.PaymentsMethods.Where(x => x.Status == 2).Count(),
                    Deleted = db.PaymentsMethods.Where(x => x.Status == 9).Count(),
                };

                int Count = db.PaymentsMethods
                    .Where(x => x.Status != 9
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Count();
                var Info = db.PaymentsMethods
                    .Where(x => x.Status != 9
                     && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Name,
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

        [HttpGet("PaymentsMethods/GetAll")]
        public IActionResult GetAllPaymentsMethods()
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                int Count = db.PaymentsMethods
                    .Where(x => x.Status == 1).Count();
                var Info = db.PaymentsMethods
                    .Where(x => x.Status == 1).Select(x => new
                    {
                        x.Id,
                        x.Name,

                    }).OrderByDescending(x => x.Name).ToList();


                return Ok(new { info = Info, count = Count });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("PaymentsMethods/Add")]
        public IActionResult AddPaymentsMethods([FromBody] BodyObject bodyObject)
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

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                if (string.IsNullOrWhiteSpace(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


                var isExist = db.PaymentsMethods.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                PaymentsMethods row = new PaymentsMethods();
                row.Name = bodyObject.Name;
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
                db.PaymentsMethods.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "PaymentsMethods/Dictionaries";
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

        [HttpPost("PaymentsMethods/Edit")]
        public IActionResult EditPaymentsMethods([FromBody] BodyObject bodyObject)
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

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                TransactionsObject rowTrans = new TransactionsObject();
                var row = db.PaymentsMethods.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });

                var isExist = db.PaymentsMethods.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                if (!string.IsNullOrEmpty(bodyObject.Image))
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);

                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تعديل بيانات   ";
                rowTrans.Controller = "PaymentsMethods/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });
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

        [HttpPost("{Id}/PaymentsMethods/Delete")]
        public IActionResult DeletePaymentsMethods(long Id)
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


                var row = db.PaymentsMethods.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "PaymentsMethods/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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

        [HttpPost("{Id}/PaymentsMethods/ChangeStatus")]
        public IActionResult ChangeStatusPaymentsMethods(long Id)
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


                var row = db.PaymentsMethods.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
                rowTrans.Controller = "PaymentsMethods/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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







        //SubscriptionsType
        [HttpGet("SubscriptionsType/Get")]
        public IActionResult GetSubscriptionsType(int pageNo, int pageSize, string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.SubscriptionsType.Where(x => x.Status != 9).Count(),
                    Active = db.SubscriptionsType.Where(x => x.Status == 1).Count(),
                    NotActive = db.SubscriptionsType.Where(x => x.Status == 2).Count(),
                    Deleted = db.SubscriptionsType.Where(x => x.Status == 9).Count(),
                };

                int Count = db.SubscriptionsType
                    .Where(x => x.Status != 9
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Count();
                var Info = db.SubscriptionsType
                    .Where(x => x.Status != 9
                     && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Image,
                        x.Lenght,
                        x.Price,
                        x.Descriptions,
                        x.Status,
                        x.CreatedOn,
                        SubscriptionsCount= db.SchoolsSubscriptions.Where(k=>k.SubscriptionsTypeId==x.Id && x.Status!=9).Count(),
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("SubscriptionsType/GetAll")]
        public IActionResult GetAllSubscriptionsType()
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                int Count = db.SubscriptionsType
                    .Where(x => x.Status == 1).Count();
                var Info = db.SubscriptionsType
                    .Where(x => x.Status == 1).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Lenght,
                        x.Price,
                        x.Image
                    }).OrderByDescending(x => x.Name).ToList();


                return Ok(new { info = Info, count = Count });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("SubscriptionsType/Add")]
        public IActionResult AddSubscriptionsType([FromBody] SubscriptionsBodyObject bodyObject)
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

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                if (string.IsNullOrWhiteSpace(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if(bodyObject.Lenght<=0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.LenghtEmpty);
                
                if(bodyObject.Price<=0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PriceEmpty);


                var isExist = db.SubscriptionsType.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                SubscriptionsType row = new SubscriptionsType();
                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                row.Price = bodyObject.Price;
                row.Lenght=bodyObject.Lenght;
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
                db.SubscriptionsType.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "SubscriptionsType/Dictionaries";
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

        [HttpPost("SubscriptionsType/Edit")]
        public IActionResult EditSubscriptionsType([FromBody] SubscriptionsBodyObject bodyObject)
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

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                if (bodyObject.Lenght <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.LenghtEmpty);

                if (bodyObject.Price <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PriceEmpty);

                TransactionsObject rowTrans = new TransactionsObject();
                var row = db.SubscriptionsType.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Lenght,
                    row.Price,
                    row.Image,
                    row.Status
                });

                var isExist = db.SubscriptionsType.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                var isAnySubscribe = db.SchoolsSubscriptions.Where(x => x.SubscriptionsTypeId == bodyObject.Id && x.Status != 9).Count();
                if(isAnySubscribe > 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.CannotUpdateWithSubscriber);

                row.Name = bodyObject.Name;
                row.Price = bodyObject.Price;
                row.Lenght = bodyObject.Lenght;
                row.Descriptions = bodyObject.Descriptions;
                if (!string.IsNullOrEmpty(bodyObject.Image))
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);

                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تعديل بيانات   ";
                rowTrans.Controller = "SubscriptionsType/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });
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

        [HttpPost("{Id}/SubscriptionsType/Delete")]
        public IActionResult DeleteSubscriptionsType(long Id)
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


                var row = db.SubscriptionsType.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "SubscriptionsType/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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

        [HttpPost("{Id}/SubscriptionsType/ChangeStatus")]
        public IActionResult ChangeStatusSubscriptionsType(long Id)
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


                var row = db.SubscriptionsType.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
                rowTrans.Controller = "SubscriptionsType/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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



        //Subjects
        [HttpGet("Subjects/Get")]
        public IActionResult GetSubjects(int pageNo, int pageSize, string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.Subjects.Where(x => x.Status != 9).Count(),
                    Active = db.Subjects.Where(x => x.Status == 1).Count(),
                    NotActive = db.Subjects.Where(x => x.Status == 2).Count(),
                    Deleted = db.Subjects.Where(x => x.Status == 9).Count(),
                };

                int Count = db.Subjects
                    .Where(x => x.Status != 9
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Count();
                var Info = db.Subjects
                    .Where(x => x.Status != 9
                     && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.Descriptions.Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Name,
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

        [HttpGet("Subjects/GetAll")]
        public IActionResult GetAllSubjects()
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                int Count = db.Subjects
                    .Where(x => x.Status == 1).Count();
                var Info = db.Subjects
                    .Where(x => x.Status == 1).Select(x => new
                    {
                        x.Id,
                        x.Name,

                    }).OrderByDescending(x => x.Name).ToList();


                return Ok(new { info = Info, count = Count });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Subjects/Add")]
        public IActionResult AddSubjects([FromBody] BodyObject bodyObject)
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

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                if (string.IsNullOrWhiteSpace(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


                var isExist = db.Subjects.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                Subjects row = new Subjects();
                row.Name = bodyObject.Name;
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
                db.Subjects.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "Subjects/Dictionaries";
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

        [HttpPost("Subjects/Edit")]
        public IActionResult EditSubjects([FromBody] BodyObject bodyObject)
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

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

                TransactionsObject rowTrans = new TransactionsObject();
                var row = db.Subjects.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });

                var isExist = db.Subjects.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                if (!string.IsNullOrEmpty(bodyObject.Image))
                    row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);

                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تعديل بيانات   ";
                rowTrans.Controller = "Subjects/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Image,
                    row.Status
                });
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

        [HttpPost("{Id}/Subjects/Delete")]
        public IActionResult DeleteSubjects(long Id)
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


                var row = db.Subjects.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                row.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "Subjects/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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

        [HttpPost("{Id}/Subjects/ChangeStatus")]
        public IActionResult ChangeStatusSubjects(long Id)
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


                var row = db.Subjects.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
                rowTrans.Controller = "Subjects/Dictionaries";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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





        ////AcademicLevels
        //[HttpGet("AcademicLevels/Get")]
        //public IActionResult GetAcademicLevels(int pageNo, int pageSize, string Search)
        //{
        //    try
        //    {
        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);



        //        var Statistics = new
        //        {
        //            Count = db.AcademicLevels.Where(x => x.Status != 9).Count(),
        //            Active = db.AcademicLevels.Where(x => x.Status == 1).Count(),
        //            NotActive = db.AcademicLevels.Where(x => x.Status == 2).Count(),
        //            Deleted = db.AcademicLevels.Where(x => x.Status == 9).Count(),
        //        };

        //        int Count = db.AcademicLevels
        //            .Where(x => x.Status != 9
        //            && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
        //                x.Descriptions.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
        //            ))
        //            ).Count();
        //        var Info = db.AcademicLevels
        //            .Where(x => x.Status != 9
        //             && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
        //                x.Descriptions.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
        //             ))
        //            ).Select(x => new
        //            {
        //                x.Id,
        //                x.Name,
        //                x.Image,
        //                x.Descriptions,
        //                x.Status,
        //                x.CreatedOn,
        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
        //            }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


        //        return Ok(new { info = Info, count = Count, Statistics });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[AllowAnonymous]
        //[HttpGet("AcademicLevels/GetAll")]
        //public IActionResult GetAllAcademicLevels()
        //{
        //    try
        //    {
        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        int Count = db.AcademicLevels
        //            .Where(x => x.Status == 1).Count();
        //        var Info = db.AcademicLevels
        //            .Where(x => x.Status == 1).Select(x => new
        //            {
        //                x.Id,
        //                x.Name,
        //                x.Image,

        //            }).OrderByDescending(x => x.Name).ToList();


        //        return Ok(new { info = Info, count = Count });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("AcademicLevels/Add")]
        //public IActionResult AddAcademicLevels([FromBody] BodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        if (string.IsNullOrWhiteSpace(bodyObject.Name))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


        //        var isExist = db.AcademicLevels.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        AcademicLevels row = new AcademicLevels();
        //        row.Name = bodyObject.Name;
        //        row.Descriptions = bodyObject.Descriptions;
        //        if (string.IsNullOrEmpty(bodyObject.Image))
        //        {
        //            row.Image = "/Uploads/Cover.png";
        //        }
        //        else
        //        {
        //            row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);
        //        }
        //        row.CreatedBy = userId;
        //        row.CreatedOn = DateTime.Now;
        //        row.Status = 1;
        //        db.AcademicLevels.Add(row);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Add;
        //        rowTrans.Descriptions = "إضافة بيانات  ";
        //        rowTrans.Controller = "AcademicLevels/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("AcademicLevels/Edit")]
        //public IActionResult EditAcademicLevels([FromBody] BodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (string.IsNullOrEmpty(bodyObject.Name))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        var row = db.AcademicLevels.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        rowTrans.OldObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });

        //        var isExist = db.AcademicLevels.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        row.Name = bodyObject.Name;
        //        row.Descriptions = bodyObject.Descriptions;
        //        if (!string.IsNullOrEmpty(bodyObject.Image))
        //            row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);



        //        rowTrans.Operations = TransactionsType.Edit;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "تعديل بيانات   ";
        //        rowTrans.Controller = "AcademicLevels/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();

        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/AcademicLevels/Delete")]
        //public IActionResult DeleteAcademicLevels(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var row = db.AcademicLevels.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        row.Status = 9;

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "حذف بيانات   ";
        //        rowTrans.Controller = "AcademicLevels/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessDeleteOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/AcademicLevels/ChangeStatus")]
        //public IActionResult ChangeStatusAcademicLevels(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var row = db.AcademicLevels.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (row.Status == 1)
        //        {
        //            row.Status = 2;
        //        }
        //        else
        //        {
        //            row.Status = 1;
        //        }





        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "تغير حالة العرض  ";
        //        rowTrans.Controller = "AcademicLevels/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessChangeStatusOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}





        ////AcademicSpecializations
        //[HttpGet("AcademicSpecializations/Get")]
        //public IActionResult GetAcademicSpecializations(int pageNo, int pageSize, string Search)
        //{
        //    try
        //    {
        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Statistics = new
        //        {
        //            Count = db.AcademicSpecializations.Where(x => x.Status != 9).Count(),
        //            Active = db.AcademicSpecializations.Where(x => x.Status == 1).Count(),
        //            NotActive = db.AcademicSpecializations.Where(x => x.Status == 2).Count(),
        //            Deleted = db.AcademicSpecializations.Where(x => x.Status == 9).Count(),
        //        };

        //        int Count = db.AcademicSpecializations
        //            .Include(x => x.AcademicLevel)
        //            .Where(x => x.Status != 9
        //            && (string.IsNullOrEmpty(Search) ? true :
        //                (x.Name.Contains(Search.Trim()) ||
        //                x.Descriptions.Contains(Search.Trim()) ||
        //                x.AcademicLevel.Name.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
        //                ))
        //            ).Count();
        //        var Info = db.AcademicSpecializations.Include(x => x.AcademicLevel)
        //            .Where(x => x.Status != 9
        //               && (string.IsNullOrEmpty(Search) ? true : (
        //                x.Name.Contains(Search.Trim()) ||
        //                x.Descriptions.Contains(Search.Trim()) ||
        //                x.AcademicLevel.Name.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
        //                ))
        //            ).Select(x => new
        //            {
        //                x.Id,
        //                x.Name,
        //                x.AcademicLevelId,
        //                AcademicLevel = x.AcademicLevel.Name,
        //                ParentId = x.AcademicLevelId,
        //                x.Image,
        //                x.Descriptions,
        //                x.Status,
        //                x.CreatedOn,
        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
        //            }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


        //        return Ok(new { info = Info, count = Count, Statistics });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[AllowAnonymous]
        //[HttpGet("AcademicSpecializations/GetAll")]
        //public IActionResult GetAllAcademicSpecializations(short AcademicLevelId)
        //{
        //    try
        //    {
        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        int Count = db.AcademicSpecializations
        //            .Where(x => x.Status == 1 && x.AcademicLevelId == AcademicLevelId).Count();
        //        var Info = db.AcademicSpecializations
        //            .Where(x => x.Status == 1 && x.AcademicLevelId == AcademicLevelId).Select(x => new
        //            {
        //                x.Id,
        //                x.Name,
        //                x.Image
        //            }).OrderByDescending(x => x.Name).ToList();


        //        return Ok(new { info = Info, count = Count });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("AcademicSpecializations/Add")]
        //public IActionResult AddAcademicSpecializations([FromBody] BodyObjectWithParent bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        if (string.IsNullOrWhiteSpace(bodyObject.Name))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


        //        var isExist = db.AcademicSpecializations.Where(x => x.Status != 9 &&
        //        x.Name == bodyObject.Name &&
        //        x.AcademicLevelId == bodyObject.ParentId
        //        ).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        AcademicSpecializations row = new AcademicSpecializations();
        //        row.AcademicLevelId = bodyObject.ParentId;
        //        row.Name = bodyObject.Name;
        //        row.Descriptions = bodyObject.Descriptions;
        //        if (string.IsNullOrEmpty(bodyObject.Image))
        //        {
        //            row.Image = "/Uploads/Cover.png";
        //        }
        //        else
        //        {
        //            row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);
        //        }
        //        row.CreatedBy = userId;
        //        row.CreatedOn = DateTime.Now;
        //        row.Status = 1;
        //        db.AcademicSpecializations.Add(row);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Add;
        //        rowTrans.Descriptions = "إضافة بيانات  ";
        //        rowTrans.Controller = "AcademicSpecializations/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("AcademicSpecializations/Edit")]
        //public IActionResult EditAcademicSpecializations([FromBody] BodyObjectWithParent bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (string.IsNullOrEmpty(bodyObject.Name))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        var row = db.AcademicSpecializations.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        rowTrans.OldObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });

        //        var isExist = db.AcademicSpecializations.Where(x => x.Status != 9 &&
        //        x.Name == bodyObject.Name &&
        //        x.AcademicLevelId == bodyObject.ParentId &&
        //        x.Id != bodyObject.Id).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        row.AcademicLevelId = bodyObject.ParentId;
        //        row.Name = bodyObject.Name;
        //        row.Descriptions = bodyObject.Descriptions;
        //        if (!string.IsNullOrEmpty(bodyObject.Image))
        //            row.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);


        //        rowTrans.Operations = TransactionsType.Edit;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "تعديل بيانات   ";
        //        rowTrans.Controller = "AcademicSpecializations/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();

        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/AcademicSpecializations/Delete")]
        //public IActionResult DeleteAcademicSpecializations(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var row = db.AcademicSpecializations.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        row.Status = 9;

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "حذف بيانات   ";
        //        rowTrans.Controller = "AcademicSpecializations/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessDeleteOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/AcademicSpecializations/ChangeStatus")]
        //public IActionResult ChangeStatusAcademicSpecializations(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var row = db.AcademicSpecializations.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (row.Status == 1)
        //        {
        //            row.Status = 2;
        //        }
        //        else
        //        {
        //            row.Status = 1;
        //        }


        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "تغير حالة العرض  ";
        //        rowTrans.Controller = "AcademicSpecializations/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Descriptions,
        //            row.Image,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessChangeStatusOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}












        ////FAQ
        //[HttpGet("Faq/Get")]
        //public IActionResult GetFaq(int pageNo, int pageSize, string Search)
        //{
        //    try
        //    {
        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Statistics = new
        //        {
        //            Count = db.Faq.Where(x => x.Status != 9).Count(),
        //            Active = db.Faq.Where(x => x.Status == 1).Count(),
        //            NotActive = db.Faq.Where(x => x.Status == 2).Count(),
        //            Deleted = db.Faq.Where(x => x.Status == 9).Count(),
        //        };

        //        int Count = db.Faq
        //            .Where(x => x.Status != 9
        //            && (string.IsNullOrEmpty(Search) ? true :
        //                (x.Email.Contains(Search.Trim()) ||
        //                x.Phone.Contains(Search.Trim()) ||
        //                x.Message.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
        //                ))
        //            ).Count();
        //        var Info = db.Faq
        //            .Where(x => x.Status != 9
        //               && (string.IsNullOrEmpty(Search) ? true : (
        //                x.Email.Contains(Search.Trim()) ||
        //                x.Phone.Contains(Search.Trim()) ||
        //                x.Message.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
        //                ))
        //            ).Select(x => new
        //            {
        //                x.Id,
        //                x.Message,
        //                x.Phone,
        //                x.Email,
        //                x.CreatedOn,
        //                x.Status,
        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
        //                x.ProceedingOn,
        //                ProceedingBy = db.Users.Where(k => k.Id == x.ProceedingBy).SingleOrDefault().Name,
        //            }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


        //        return Ok(new { info = Info, count = Count, Statistics });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/Faq/Delete")]
        //public IActionResult DeleteFaq(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var row = db.Faq.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        row.Status = 9;

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "حذف بيانات   ";
        //        rowTrans.Controller = "Faq/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Message,
        //            row.Phone,
        //            row.Email,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessDeleteOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/Faq/ChangeStatus")]
        //public IActionResult ChangeStatusFaq(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var row = db.Faq.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        row.Status = 2;
        //        row.ProceedingBy = userId;
        //        row.ProceedingOn = DateTime.Now;

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.CahngeStatus;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "تغير حالة العرض  ";
        //        rowTrans.Controller = "Faq/Dictionaries";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Email,
        //            row.Phone,
        //            row.Message,
        //            row.Status
        //        });
        //        rowTrans.CreatedBy = userId;
        //        transactions.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessChangeStatusOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}
















        ////Dashboard
        //[HttpGet("Dashboard/Get")]
        //public IActionResult GetDashboardInfo()
        //{
        //    try
        //    {
        //        var userId = security.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);



        //        int DeviceRequestCount = db.UsersChangeRequest.Where(x => x.Status != 9).Count();
        //        int WaitingDeviceReques = db.UsersChangeRequest.Where(x => x.Status == 1).Count();
        //        int AcceptDeviceReques = db.UsersChangeRequest.Where(x => x.Status == 2).Count();
        //        int RejectDeviceReques = db.UsersChangeRequest.Where(x => x.Status == 3).Count();
        //        double AvgDeviceReques = DeviceRequestCount > 0 ? (double)(AcceptDeviceReques + RejectDeviceReques) / DeviceRequestCount : 0;


        //        int CoursesRequestsCount = db.StudentsCoursesRequests.Where(x => x.Status != 9).Count();
        //        int WaitingCoursesRequests = db.StudentsCoursesRequests.Where(x => x.Status == 1).Count();
        //        int AcceptCoursesRequests = db.StudentsCoursesRequests.Where(x => x.Status == 2).Count();
        //        int RejectCoursesRequests = db.StudentsCoursesRequests.Where(x => x.Status == 3).Count();
        //        double AvgCoursesRequests = CoursesRequestsCount > 0 ? (double)(AcceptCoursesRequests + RejectCoursesRequests) / CoursesRequestsCount : 0;



        //        int Student = db.Students.Include(x => x.User).Where(x => x.User.Status != 9).Count();
        //        int Course = db.Courses.Where(x => x.Status != 9).Count();
        //        int Lecture = db.Lectures.Where(x => x.Status != 9).Count();
        //        int Exams = db.Exams.Where(x => x.Status != 9).Count();

        //        int StudentCourseCount = db.StudentsCourses.Where(x => x.CreatedOn.Value.Date == DateTime.Now.Date && x.Status == 1).Count();
        //        int StudentCourseCash = db.StudentsCourses.Where(x => x.CreatedOn.Value.Date == DateTime.Now.Date && x.Status == 1).Sum(x => x.Value).Value;

        //        int WalletCharge = db.StudentsWalletTransactions.Where(x => x.CreatedOn.Value.Date == DateTime.Now.Date && x.Status != 9).Sum(x => x.Value).Value;


        //        int CardPurchesCount = db.VoucherCardsCharging.Where(x => x.CreatedOn.Value.Date == DateTime.Now.Date && x.Status != 9).Count();
        //        int CardPurchesCash = db.VoucherCardsCharging.Include(x => x.VoucherCard).Where(x => x.CreatedOn.Value.Date == DateTime.Now.Date && x.Status != 9).Sum(x => x.VoucherCard.Amount).Value;





        //        var Info = new
        //        {
        //            DeviceRequestCount,
        //            WaitingDeviceReques,
        //            AcceptDeviceReques,
        //            RejectDeviceReques,
        //            AvgDeviceReques,

        //            CoursesRequestsCount,
        //            WaitingCoursesRequests,
        //            AcceptCoursesRequests,
        //            RejectCoursesRequests,
        //            AvgCoursesRequests,


        //            Student,
        //            Course,
        //            Lecture,
        //            Exams,

        //            StudentCourseCount,
        //            StudentCourseCash,

        //            CardPurchesCount,
        //            CardPurchesCash

        //        };


        //        return Ok(new { info = Info });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}






    }
}