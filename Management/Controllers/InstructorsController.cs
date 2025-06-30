//using Common;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Vue.Models;
//using Web.Services;
//using static Management.Controllers.StudentsController;
//using static Web.Services.Helper;

//namespace Management.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/admin/Instructors")]
//    public class InstructorsController : Controller
//    {
//        private Helper help;
//        private readonly TraneemBetaContext db;

//        //public long? CityId { get; private set; }         

//        public InstructorsController(IConfiguration iConfig, TraneemBetaContext context)
//        {
//            this.db = context;
//            help = new Helper(iConfig, context);
//        }



//        public partial class BodyObject
//        {
//            public long Id { get; set; }
//            public string Name { get; set; }
//            public string Phone { get; set; }
//            public string LoginName { get; set; }
//            public string Email { get; set; }
//            public string Descriptions { get; set; }
//            public string FacebookProfile { get; set; }
//            public short Percentage { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//        }


//        [HttpGet("Get")]
//        public IActionResult Get(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Instructors.Include(x => x.User).Where(x => x.User.Status != 9 && x.User.Status != 3).Count(),
//                    Active = db.Instructors.Include(x => x.User).Where(x => x.User.Status == 1).Count(),
//                    NotActive = db.Instructors.Include(x => x.User).Where(x => x.User.Status == 3).Count(),
//                    ZeroCourse = db.Instructors.Include(x => x.User).Where(x => x.User.Status != 3 && x.User.Status!=3 && x.Courses.Count()==0).Count(),
//                };

//                int Count = db.Instructors
//                    .Include(x => x.User)
//                    .Where(x => x.User.Status != 9 && x.User.Status != 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        //x.Rate.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.Instructors
//                     .Include(x => x.User)
//                     .Include(x => x.Courses)
//                    .Where(x => x.User.Status != 9 && x.User.Status != 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.User.Name,
//                        x.Rate,
//                        x.UserId,
//                        x.User.Phone,
//                        x.User.LoginName,
//                        x.User.Email,
//                        x.User.Image,
//                        x.User.LastLoginOn,
//                        x.Descriptions,
//                        x.FacebookProfile,
//                        x.Percentage,
//                        CourseCount = x.Courses.Where(k=>k.Status!=9).Count(),
//                        CreatedOn = x.User.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.User.CreatedBy).SingleOrDefault().Name,
//                        x.User.Status,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
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

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var Info = db.Instructors.Include(x => x.User).Where(x => x.User.Status ==1).Select(x => new {
//                    x.Id,
//                    x.User.Name,
//                }).OrderByDescending(x => x.Name).ToList();


//                return Ok(new { info = Info });
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

//                //validations
//                if (string.IsNullOrEmpty(bodyObject.LoginName))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

//                if (!string.IsNullOrEmpty(bodyObject.Email))
//                {
//                    if (!help.IsValidEmail(bodyObject.Email))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);
//                }

//                if (string.IsNullOrEmpty(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//                if (!help.IsValidPhone(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


//                //isExist
//                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 ).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameExist);

//                if (!string.IsNullOrEmpty(bodyObject.Email))
//                {
//                    IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9).SingleOrDefault();
//                    if (IsExist != null)
//                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
//                }

//                IsExist = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

//                db.Users.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);


//                Users user = new Users();
//                user.Name = bodyObject.Name;
//                user.LoginName = bodyObject.LoginName;
//                user.Phone = bodyObject.Phone;
//                user.Email = bodyObject.Email;
//                string Password = this.help.GenreatePass();
//                user.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);
//                user.UserType = 60;
//                if (string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    user.Image = "/Uploads/User.jpg";
//                }
//                else
//                {
//                    user.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                }
//                user.CreatedBy = userId;
//                user.CreatedOn = DateTime.Now;
//                user.Status = 1;

//                Instructors row = new Instructors();
//                row.Descriptions = bodyObject.Descriptions;
//                row.FacebookProfile = bodyObject.FacebookProfile;
//                row.Percentage = bodyObject.Percentage;
//                row.Rate = 0;
//                row.User = user;
//                db.Instructors.Add(row);


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Instructors";
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

//                //validations
//                if (string.IsNullOrEmpty(bodyObject.LoginName))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

//                if (!string.IsNullOrEmpty(bodyObject.Email))
//                {
//                    if (!help.IsValidEmail(bodyObject.Email))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);
//                }

//                if (string.IsNullOrEmpty(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//                if (!help.IsValidPhone(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);


//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.Instructors.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.User.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var user = db.Users.Where(x => x.Status != 9 && x.Id == row.UserId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.User.Name,
//                    row.UserId,
//                    row.Descriptions,
//                    row.Rate,
//                    row.User.Phone,
//                    row.User.Image,
//                    row.User.LoginName,
//                    row.User.Email,
//                    row.User.Status,
//                    row.FacebookProfile,
//                    row.Percentage
//                });


//                //isExist
//                var IsExist = db.Users.Where(x => x.Id != row.UserId && x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameExist);

//                if (!string.IsNullOrEmpty(bodyObject.Email))
//                {
//                    IsExist = db.Users.Where(x => x.Id != row.UserId && x.Email == bodyObject.Email && x.Status != 9).SingleOrDefault();
//                    if (IsExist != null)
//                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
//                }

//                IsExist = db.Users.Where(x => x.Id != row.UserId && x.Phone == bodyObject.Phone && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);


//                db.Users.Where(x => x.Id != row.UserId && x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);




//                user.Name = bodyObject.Name;
//                user.LoginName = bodyObject.LoginName;
//                user.Phone = bodyObject.Phone;
//                user.Email = bodyObject.Email;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    user.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);

//                row.Descriptions = bodyObject.Descriptions;
//                row.FacebookProfile = bodyObject.FacebookProfile;
//                row.Percentage = bodyObject.Percentage;


//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.Descriptions = "تعديل بيانات  ";
//                rowTrans.Controller = "Instructors";
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
        
//        [HttpPost("{Id}/ResetPassword")]
//        public IActionResult ResetPassword(long Id)
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


//                var row = db.Instructors.Include(x => x.User).Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                string Password = this.help.GenreatePass();
//                row.User.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Reset;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تهيئة كلمة المرور  ";
//                rowTrans.Controller = "Instructors";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.User.Name,
//                    row.Descriptions,
//                    row.User.Phone,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessResetOperations + "كلمة المرور الجديدة : " + Password);
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

//                var row = db.Instructors
//                    .Include(x => x.User)
//                    .Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.User.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Instructors";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.User.Name,
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



//        public partial class SuspendsBodyObject
//        {
//            public long Id { get; set; }
//            public string DropResone { get; set; }
//        }

//        //Suspends
//        [HttpPost("Suspends/Add")]
//        public IActionResult SuspendsStudents([FromBody] SuspendsBodyObject bodyObject)
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

//                if (string.IsNullOrEmpty(bodyObject.DropResone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ResoneEmpty);


//                var row = db.Instructors.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.User.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                UsersSuspends usersSuspends = new UsersSuspends();
//                usersSuspends.Resone = bodyObject.DropResone;
//                usersSuspends.UserId = row.UserId;
//                usersSuspends.CreatedOn = DateTime.Now;
//                usersSuspends.CreatedBy = userId;
//                usersSuspends.Status = 1;
//                db.UsersSuspends.Add(usersSuspends);

//                row.User.Status = 3;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Block;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = " إيقاف الحساب  ";
//                rowTrans.Controller = "Students";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.User.Name,
//                    row.Descriptions,
//                    row.User.Phone,
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

//        [HttpGet("Suspends/Get")]
//        public IActionResult GetSuspends(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Instructors.Include(x => x.User).Where(x => x.User.Status != 9 && x.User.Status != 3).Count(),
//                    Active = db.Instructors.Include(x => x.User).Where(x => x.User.Status == 1).Count(),
//                    NotActive = db.Instructors.Include(x => x.User).Where(x => x.User.Status == 3).Count(),
//                    ZeroCourse = db.Instructors.Include(x => x.User).Where(x => x.User.Status != 3 && x.User.Status != 3 && x.Courses.Count() == 0).Count(),
//                };

//                int Count = db.Instructors
//                    .Include(x => x.User)
//                    .Where(x => x.User.Status != 9 && x.User.Status == 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        //x.Rate.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.Instructors
//                     .Include(x => x.User)
//                     .Include(x => x.Courses)
//                    .Where(x => x.User.Status != 9 && x.User.Status == 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.User.Name,
//                        x.Rate,
//                        x.UserId,
//                        x.User.Phone,
//                        x.User.LoginName,
//                        x.User.Email,
//                        x.User.Image,
//                        x.User.LastLoginOn,
//                        x.Descriptions,
//                        x.FacebookProfile,
//                        x.Percentage,
//                        SusbendInfo = db.UsersSuspends.Where(k => k.UserId == x.UserId && k.Status != 9).SingleOrDefault(),
//                        SusbendCreatedBy = db.Users.Where(k => k.Id == db.UsersSuspends.Where(k => k.UserId == x.UserId && k.Status != 9).SingleOrDefault().CreatedBy).SingleOrDefault().Name,
//                        CourseCount = x.Courses.Where(k => k.Status != 9).Count(),
//                        CreatedOn = x.User.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.User.CreatedBy).SingleOrDefault().Name,
//                        x.User.Status,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Suspends/Cansel")]
//        public IActionResult CanselSuspends(long Id)
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


//                var row = db.Instructors.Include(x => x.User).Where(x => x.Id == Id && x.User.Status == 3).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var Susbend = db.UsersSuspends.Where(x => x.UserId == row.UserId && x.Status != 9).SingleOrDefault();
//                if (Susbend == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                Susbend.Status = 9;
//                row.User.Status = 1;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تفعيل الحساب ";
//                rowTrans.Controller = "Instructors/Susbend";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.User.Name,
//                    row.User.Phone,
//                    row.UserId,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessActiveOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }


//    }
//}