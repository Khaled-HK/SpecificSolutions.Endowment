//using Common;
//using MathNet.Numerics.Statistics;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using Org.BouncyCastle.Tls.Crypto.Impl.BC;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Vue.Models;
//using Web.Services;
//using static Web.Services.Helper;

//namespace Management.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/admin/Students")]
//    public class StudentsController : Controller
//    {
//        private Helper help;
//        IConfiguration configuration;

//        private readonly TraneemBetaContext db;

//        public StudentsController(IConfiguration iConfig, TraneemBetaContext context)
//        {
//            this.db = context;
//            help = new Helper(iConfig, context);
//        }


//        public partial class BodyObject
//        {
//            public long Id { get; set; }
//            public short AcademicSpecializationId { get; set; }
//            public string FirstName { get; set; }
//            public string FatherName { get; set; }
//            public string SirName { get; set; }
//            public string Phone { get; set; }
//            public string ExtraPhone { get; set; }
//            public string LoginName { get; set; }
//            public string Email { get; set; }
//            public string Descriptions { get; set; }
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
//                    Count = db.Students.Include(x => x.User).Where(x => x.User.Status != 9 && x.User.Status != 3).Count(),
//                    Active = db.Students.Include(x => x.User).Where(x => x.User.Status == 1).Count(),
//                    NotActive = db.Students.Include(x => x.User).Where(x => x.User.Status == 3).Count(),
//                    HaveMony = db.StudentsWallet.Where(x => x.Value > 0).Count(),
//                };

//                int Count = db.Students
//                    .Include(x => x.User)
//                    .Where(x => x.User.Status != 9 && x.User.Status != 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        //x.Rate.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.Students
//                     .Include(x => x.User)
//                     .Include(x => x.AcademicSpecialization)
//                     .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.User.Status != 9 && x.User.Status != 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.FirstName,
//                        x.FatherName,
//                        x.SirName,
//                        x.Points,
//                        x.Rate,
//                        x.UserId,
//                        x.User.Phone,
//                        ExtraPhone=x.Phone,
//                        x.User.LoginName,
//                        x.User.Email,
//                        x.User.Image,
//                        x.User.LastLoginOn,
//                        x.AcademicSpecializationId,
//                        x.Descriptions,
//                        AcademicSpecialization = x.AcademicSpecialization.Name,
//                        x.AcademicSpecialization.AcademicLevelId,
//                        AcademicLevel = x.AcademicSpecialization.AcademicLevel.Name,
//                        CourseCount = db.StudentsCourses.Where(k => k.StudentId == x.Id && k.Status != 9).Count(),
//                        WalletValue = x.Wallet.Value,
//                        CreatedOn=x.User.CreatedOn,
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

//                if (!string.IsNullOrEmpty(bodyObject.ExtraPhone))
//                {
//                    if (!help.IsValidPhone(bodyObject.ExtraPhone))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.ExtraPhoneNotValid);
//                }

//                if (string.IsNullOrEmpty(bodyObject.FirstName) ||
//                    string.IsNullOrEmpty(bodyObject.FatherName) ||
//                    string.IsNullOrEmpty(bodyObject.SirName))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


//                //isExist
//                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
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

//                string Name = bodyObject.FirstName + " " + bodyObject.FatherName + " " + bodyObject.SirName;

//                db.Users.Where(x => x.Name == Name && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);


//                Users user = new Users();
//                user.Name = Name;
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

//                StudentsWallet Wallet = new StudentsWallet();
//                Wallet.Value = 0;
//                Wallet.CreatedOn = DateTime.Now;
//                Wallet.CreatedBy = userId;
//                Wallet.Status = 1;

//                Students row = new Students();
//                row.FirstName = bodyObject.FirstName;
//                row.FatherName = bodyObject.FatherName;
//                row.SirName = bodyObject.SirName;
//                row.Name = Name;
//                row.Descriptions = bodyObject.Descriptions;
//                row.Phone = bodyObject.ExtraPhone;
//                row.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
//                row.Points = 0;
//                row.Rate = 0;
//                row.User = user;
//                row.Wallet = Wallet;
//                db.Students.Add(row);


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Students";
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

//                if (!string.IsNullOrEmpty(bodyObject.ExtraPhone))
//                {
//                    if (!help.IsValidPhone(bodyObject.ExtraPhone))
//                        return StatusCode(BackMessages.StatusCode, BackMessages.ExtraPhoneNotValid);
//                }

//                if (string.IsNullOrEmpty(bodyObject.FirstName) ||
//                    string.IsNullOrEmpty(bodyObject.FatherName) ||
//                    string.IsNullOrEmpty(bodyObject.SirName))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.Students.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.User.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var user = db.Users.Where(x => x.Status != 9 && x.Id == row.UserId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.AcademicSpecializationId,
//                    row.WalletId,
//                    row.UserId,
//                    row.Descriptions,
//                    row.Rate,
//                    row.Points,
//                    row.Phone,
//                    ExtraPhone = row.User.Phone,
//                    row.User.Image,
//                    row.User.LoginName,
//                    row.User.Email,
//                    row.User.Status,
//                });


//                //isExist
//                var IsExist = db.Users.Where(x => x.Id!=row.UserId && x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
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

//                string Name = bodyObject.FirstName + " " + bodyObject.FatherName + " " + bodyObject.SirName;

//                db.Users.Where(x => x.Id != row.UserId && x.Name == Name && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                


//                user.Name = Name;
//                user.LoginName = bodyObject.LoginName;
//                user.Phone = bodyObject.Phone;
//                user.Email = bodyObject.Email;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    user.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
                
//                row.FirstName = bodyObject.FirstName;
//                row.FatherName = bodyObject.FatherName;
//                row.SirName = bodyObject.SirName;
//                row.Name = Name;
//                row.Descriptions = bodyObject.Descriptions;
//                row.Phone = bodyObject.ExtraPhone;
//                row.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
                
                
//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.Descriptions = "تعديل بيانات  ";
//                rowTrans.Controller = "Students";
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
       
//        [HttpPost("{Id}/ResetDevice")]
//        public IActionResult ResetDevice(long Id)
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


//                var row = db.Students.Include(x => x.User).Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var UsersDevices = db.UsersDevices.Where(x => x.Status == 1 && x.UserId == row.UserId).SingleOrDefault();
//                if(UsersDevices == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NoUsersDevicesActive);

//                UsersDevices.Status = 2;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Reset;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تهيئة الجهاز  ";
//                rowTrans.Controller = "Students";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                    row.User.Phone,
//                    UsersDevices.MachineName,
//                    UsersDevices.IpAddress,
//                    UsersDevices.DeviceSignature,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessResetOperations);
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


//                var row = db.Students.Include(x => x.User).Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                string Password = this.help.GenreatePass();
//                row.User.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Reset;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تهيئة كلمة المرور  ";
//                rowTrans.Controller = "Students";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                    row.User.Phone,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessResetOperations + "كلمة المرور الجديدة : " +Password);
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

//                var row = db.Students
//                    .Include(x=>x.User)
//                    .Include(x=>x.Wallet)
//                    .Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if(row.Wallet.Value>0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CannotDeleteWithWalletValue);

//                row.User.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Students";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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




//        //Suspends
//        [HttpPost("Suspends/Add")]
//        public IActionResult SuspendsStudents([FromBody] CoursesDropBodyObject bodyObject)
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

//                if(string.IsNullOrEmpty(bodyObject.DropResone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ResoneEmpty);


//                var row = db.Students.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.User.Status != 9).SingleOrDefault();
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
//                    row.Name,
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
//                    Count = db.Students.Include(x => x.User).Where(x => x.User.Status != 9 && x.User.Status != 3).Count(),
//                    Active = db.Students.Include(x => x.User).Where(x => x.User.Status == 1).Count(),
//                    NotActive = db.Students.Include(x => x.User).Where(x => x.User.Status == 3).Count(),
//                    HaveMony = db.StudentsWallet.Where(x => x.Value > 0).Count(),
//                };

//                int Count = db.Students
//                    .Include(x => x.User)
//                    .Where(x => x.User.Status != 9 && x.User.Status == 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        //x.Rate.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.Students
//                     .Include(x => x.User)
//                     .Include(x => x.AcademicSpecialization)
//                     .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.User.Status != 9 && x.User.Status == 3
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.FirstName,
//                        x.FatherName,
//                        x.SirName,
//                        x.Points,
//                        x.Rate,
//                        x.UserId,
//                        x.User.Phone,
//                        ExtraPhone = x.Phone,
//                        x.User.LoginName,
//                        x.User.Email,
//                        x.User.Image,
//                        x.User.LastLoginOn,
//                        x.AcademicSpecializationId,
//                        x.Descriptions,
//                        AcademicSpecialization = x.AcademicSpecialization.Name,
//                        x.AcademicSpecialization.AcademicLevelId,
//                        AcademicLevel = x.AcademicSpecialization.AcademicLevel.Name,
//                        CourseCount = db.StudentsCourses.Where(k => k.StudentId == x.Id && k.Status != 9).Count(),
//                        SusbendInfo=db.UsersSuspends.Where(k=>k.UserId==x.UserId && k.Status!=9).SingleOrDefault(),
//                        SusbendCreatedBy = db.Users.Where(k => k.Id == db.UsersSuspends.Where(k => k.UserId == x.UserId && k.Status != 9).SingleOrDefault().CreatedBy).SingleOrDefault().Name,
//                        WalletValue = x.Wallet.Value,
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


//                var row = db.Students.Include(x=>x.User).Where(x => x.Id == Id && x.User.Status == 3).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var Susbend = db.UsersSuspends.Where(x => x.UserId == row.UserId && x.Status != 9).SingleOrDefault();
//                if(Susbend==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                Susbend.Status = 9;
//                row.User.Status = 1;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تفعيل الحساب ";
//                rowTrans.Controller = "Students/Susbend";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Phone,
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





//        //ChangeRequest
//        [HttpGet("ChangeRequest/Get")]
//        public IActionResult GetChangeRequest(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.UsersChangeRequest.Where(x => x.Status != 9 && x.User.Status != 3 && x.Status != 9).Count(),
//                    Accept = db.UsersChangeRequest.Where(x => x.Status != 9 && x.User.Status != 3 && x.Status ==2).Count(),
//                    Reject = db.UsersChangeRequest.Where(x => x.Status != 9 && x.User.Status != 3 && x.Status ==3).Count(),
//                    Deleted = db.UsersChangeRequest.Where(x => x.Status == 9 && x.User.Status != 3).Count(),
//                };

//                int Count = db.UsersChangeRequest
//                    .Include(x => x.User)
//                    .Where(x => x.User.Status != 9 && x.User.Status != 3 && x.Status!=9
//                    && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.UsersChangeRequest
//                     .Include(x => x.User)
//                    .Where(x => x.User.Status != 9 && x.User.Status != 3 && x.Status != 9
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
//                        x.User.Image,
//                        x.User.Phone,
//                        x.OldMachineName,
//                        x.NewMachineName,
//                        x.OldIpAddress,
//                        x.NewIpAddress,
//                        x.CreatedOn,
//                        x.Status,
//                        ProssedBy = db.Users.Where(k => k.Id == x.ProssedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/ChangeRequest/Accept")]
//        public IActionResult AcceptChangeRequest(long Id)
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

//                var row = db.UsersChangeRequest.Include(x=>x.User).Where(x => x.Status != 9 && x.Id == Id).SingleOrDefault();
//                if(row==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 2;

//                var Devices = db.UsersDevices.Where(x => x.UserId == row.UserId && x.Status != 9).ToList();
//                if(Devices!=null && Devices.Count>0)
//                {
//                    foreach (var item in Devices)
//                    {
//                        item.Status = 2;
//                    }
//                }

//                UsersDevices usersDevices = new UsersDevices();
//                usersDevices.IpAddress = row.NewIpAddress;
//                usersDevices.MachineName = row.NewMachineName;
//                usersDevices.CreatedBy = userId;
//                usersDevices.CreatedOn = DateTime.Now;
//                usersDevices.Status = 1;
//                usersDevices.UserId = row.UserId;
//                db.UsersDevices.Add(usersDevices);


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تفعيل جهاز جديد ";
//                rowTrans.Controller = "Students/ChangeRequest";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.NewIpAddress,
//                    row.OldIpAddress,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAcceptedRequest);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("ChangeRequest/AcceptAll")]
//        public IActionResult AcceptAllChangeRequest()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var row = db.UsersChangeRequest.Include(x => x.User).Where(x => x.Status ==1).ToList();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if(row!=null && row.Count>0)
//                {
//                    foreach (var item in row)
//                    {
//                        item.Status = 2;

//                        var Devices = db.UsersDevices.Where(x => x.UserId == item.UserId && x.Status != 9).ToList();
//                        if (Devices != null && Devices.Count > 0)
//                        {
//                            foreach (var item1 in Devices)
//                            {
//                                item1.Status = 2;
//                            }
//                        }

//                        UsersDevices usersDevices = new UsersDevices();
//                        usersDevices.IpAddress = item.NewIpAddress;
//                        usersDevices.MachineName = item.NewMachineName;
//                        usersDevices.CreatedBy = userId;
//                        usersDevices.CreatedOn = DateTime.Now;
//                        usersDevices.Status = 1;
//                        usersDevices.UserId = item.UserId;
//                        db.UsersDevices.Add(usersDevices);


//                    }
                   
//                }

               

                

                


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                //rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تفعيل جميع الطلبات جديد ";
//                rowTrans.Controller = "Students/ChangeRequest";
//                //rowTrans.NewObject = JsonConvert.SerializeObject(new
//                //{
//                //    row.Id,
//                //    row.Name,
//                //    row.NewIpAddress,
//                //    row.OldIpAddress,
//                //});
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAcceptedRequest);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/ChangeRequest/Reject")]
//        public IActionResult RejectChangeRequest(long Id)
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

//                var row = db.UsersChangeRequest.Include(x => x.User).Where(x => x.Status != 9 && x.Id == Id).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 3;

                
//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "رفض طلب جهاز جديد ";
//                rowTrans.Controller = "Students/ChangeRequest";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.NewIpAddress,
//                    row.OldIpAddress,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAcceptedRequest);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/ChangeRequest/Delete")]
//        public IActionResult DeleteChangeRequest(long Id)
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

//                var row = db.UsersChangeRequest.Include(x => x.User).Where(x => x.Status != 9 && x.Id == Id).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف طلب جهاز جديد ";
//                rowTrans.Controller = "Students/ChangeRequest";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.NewIpAddress,
//                    row.OldIpAddress,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAcceptedRequest);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("ChangeRequest/DeleteAll")]
//        public IActionResult DeleteAllChangeRequest()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var row = db.UsersChangeRequest.Include(x => x.User).Where(x => x.Status != 9).ToList();

//                int Count = 0;
//                if(row!=null && row.Count>0)
//                {
//                    foreach (var item in row)
//                    {
//                        item.Status = 9;
//                        Count++;
//                    }
//                }


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                //rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف جميع الطلبات الخاصة بتغير الجهاز ";
//                rowTrans.Controller = "Students/ChangeRequest";
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAcceptedRequest + " عدد الطلبات النحدوفة " + Count);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }







//        //CoursesRequests
//        [HttpGet("CoursesRequests/Get")]
//        public IActionResult GetCoursesRequests(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.StudentsCoursesRequests.Where(x => x.Status != 9).Count(),
//                    New = db.StudentsCoursesRequests.Where(x => x.Status ==1).Count(),
//                    Accept = db.StudentsCoursesRequests.Where(x => x.Status ==2).Count(),
//                    Reject = db.StudentsCoursesRequests.Where(x => x.Status ==3).Count(),
//                };

//                int Count = db.StudentsCoursesRequests
//                    .Include(x => x.Student)
//                    .Include(x => x.Student.User)
//                    .Include(x => x.Course)
//                    .Include(x => x.Course.Instructor)
//                    .Include(x => x.Course.Subject)
//                    .Include(x => x.Course.AcademicSpecialization)
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status != 9
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Student.Name.Contains(Search.Trim()) ||
//                        x.Student.User.Phone.Contains(Search.Trim()) ||
//                        x.Course.Name.Contains(Search.Trim()) ||
//                        x.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.StudentsCoursesRequests
//                     .Include(x => x.Student)
//                    .Include(x => x.Student.User)
//                    .Include(x => x.Course)
//                    .Include(x => x.Course.Instructor)
//                    .Include(x => x.Course.Instructor.User)
//                    .Include(x => x.Course.Subject)
//                    .Include(x => x.Course.AcademicSpecialization)
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status != 9
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Student.Name.Contains(Search.Trim()) ||
//                        x.Student.User.Phone.Contains(Search.Trim()) ||
//                        x.Course.Name.Contains(Search.Trim()) ||
//                        x.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Student.Name,
//                        x.Student.User.Phone,
//                        x.Student.User.Image,
//                        Course=x.Course.Name,
//                        Subject=x.Course.Subject.Name,
//                        AcademicSpecialization=x.Course.AcademicSpecialization.Name,
//                        AcademicLevel=x.Course.AcademicSpecialization.AcademicLevel.Name,
//                        Instructor=x.Course.Instructor.User.Name,
//                        x.CreatedOn,
//                        x.Status,
//                        ProssedBy = db.Users.Where(k => k.Id == x.ProssedBy).SingleOrDefault().Name,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/CoursesRequests/Accept")]
//        public IActionResult AcceptCoursesRequests(long Id)
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

//                var row = db.StudentsCoursesRequests
//                    .Include(x => x.Student)
//                    .Include(x => x.Course)
//                    .Where(x => x.Status ==1 && x.Id==Id).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 2;
//                row.ProssedBy = userId;


//                var isExist = db.StudentsCourses.Where(x => x.StudentId == row.StudentId
//                        && x.CourseId == row.CourseId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);


//                var Student = db.Students
//                    .Include(x => x.Wallet)
//                    .Include(x => x.User).Where(x => x.User.Status != 9 && x.Id == row.StudentId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الطالب ");

//                var Course = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Where(x => x.Id == row.CourseId && x.Status != 9 && x.Status != 3).SingleOrDefault();
//                if (Course == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الدورة التدريبية  ");


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName = Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    bodyObjectValue = 0,
//                    Description="الموافقة علي التسجيل المجاني للطالب ",
//                });


//                StudentsWalletTracker studentsWalletTracker = new StudentsWalletTracker();
//                studentsWalletTracker.WalletId = Student.WalletId;
//                studentsWalletTracker.ProcessType = 1;
//                studentsWalletTracker.Descriptions = "  الإشتراك في دورة بشكل مجاني  : " + Course.Name + " للمدرب : " + Course.Instructor.User.Name;
//                studentsWalletTracker.Value = 0;
//                studentsWalletTracker.Befroe = Student.Wallet.Value;
//                studentsWalletTracker.After = Student.Wallet.Value - 0;
//                studentsWalletTracker.Channel = 1;
//                studentsWalletTracker.CreatedBy = userId;
//                studentsWalletTracker.CreatedOn = DateTime.Now;
//                studentsWalletTracker.Status = 1;
//                db.StudentsWalletTracker.Add(studentsWalletTracker);


//                List<StudentsWalletPurchases> StudentWalletPurchasesList = new List<StudentsWalletPurchases>();
//                StudentsWalletPurchases StudentWalletPurchasesRow = new StudentsWalletPurchases();
//                StudentWalletPurchasesRow.WalletId = Student.WalletId;
//                StudentWalletPurchasesRow.Value = 0;
//                StudentWalletPurchasesRow.CoursePrice = Course.Price;
//                StudentWalletPurchasesRow.CreatedOn = DateTime.Now;
//                StudentWalletPurchasesRow.CreatedBy = userId;
//                StudentWalletPurchasesRow.Status = 1;
//                StudentWalletPurchasesList.Add(StudentWalletPurchasesRow);





//                StudentsCourses studentsCourses = new StudentsCourses();
//                studentsCourses.StudentsWalletPurchases = StudentWalletPurchasesList;
//                studentsCourses.CourseId = row.CourseId;
//                studentsCourses.StudentId = row.StudentId;
//                studentsCourses.Value = 0;
//                studentsCourses.Channel = 1;
//                studentsCourses.RegisterType = 2;
//                studentsCourses.CreatedOn = DateTime.Now;
//                studentsCourses.CreatedBy = userId;
//                studentsCourses.Status = 1;
//                db.StudentsCourses.Add(studentsCourses);







//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.ItemId = row.Id;
//                rowTrans.Controller = "Students/Courses/RequestJoinFree";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName = Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    bodyObjectValue = 0,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);


//                db.SaveChanges();
//                return Ok(BackMessages.SucessAcceptedRequest);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/CoursesRequests/Reject")]
//        public IActionResult RejectCoursesRequests(long Id)
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

//                var row = db.StudentsCoursesRequests.Where(x => x.Status ==1 && x.Id == Id).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 3;
//                row.ProssedBy = userId;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "رفض طلب الإنظمام للدورة التدريبيية ";
//                rowTrans.Controller = "Students/CoursesRequests";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.StudentId,
//                    row.CourseId,
//                    row.ProssedBy,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessRejectRequest);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/CoursesRequests/Delete")]
//        public IActionResult DeleteCoursesRequests(long Id)
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

//                var row = db.StudentsCoursesRequests.Where(x => x.Status == 1 && x.Id == Id).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;
//                row.ProssedBy = 9;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف طلب الإنظمام لدورة تدريبية  ";
//                rowTrans.Controller = "Students/CoursesRequests";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.ProssedBy,
//                    row.StudentId,
//                    row.CourseId,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessAcceptedRequest);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }










//        //Courses
//        public partial class CoursesBodyObject
//        {
//            public long? Id { get; set; }
//            public long CourseId { get; set; }
//            public long StudentId { get; set; }
//            public short Value { get; set; }
//        }

//        [HttpGet("Courses/Get")]
//        public IActionResult GetCourse(long Id, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Active = db.StudentsCourses.Where(x => x.Status == 1 && x.StudentId == Id).Count(),
//                    NotActive = db.StudentsCourses.Where(x => x.Status == 9 && x.StudentId == Id).Count(),
//                };

//                int Count = db.StudentsCourses
//                    .Include(x => x.Course)
//                    .Where(x => x.Status != 9 && x.StudentId == Id
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Course.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.StudentsCourses
//                    .Include(x => x.Course)
//                    .Include(x => x.Course.Instructor)
//                    .Include(x => x.Course.Instructor.User)
//                    .Include(x => x.Course.Subject)
//                    .Include(x => x.Course.AcademicSpecialization)
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status != 9 && x.StudentId == Id
//                     && (string.IsNullOrEmpty(Search) ? true : (x.Course.Name.Contains(Search.Trim()) ||
//                        x.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.CourseId,
//                        x.Course.Name,
//                        x.Course.Image,
//                        Instructor = x.Course.Instructor.User.Name,
//                        x.Course.AcademicSpecialization.AcademicLevelId,
//                        AcademicLevel = x.Course.AcademicSpecialization.AcademicLevel.Name,
//                        x.Course.AcademicSpecializationId,
//                        AcademicSpecialization = x.Course.AcademicSpecialization.Name,
//                        x.Course.SubjectId,
//                        Subject = x.Course.Subject.Name,
//                        x.Course.Price,
//                        x.Channel,
//                        x.RegisterType,
//                        x.Value,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderBy(x => x.CreatedOn).ToList();


//                var DropInfo = db.StudentsCourses
//                   .Include(x => x.Course)
//                   .Include(x => x.Course.Instructor)
//                   .Include(x => x.Course.Instructor.User)
//                   .Include(x => x.Course.Subject)
//                   .Include(x => x.Course.AcademicSpecialization)
//                   .Include(x => x.Course.AcademicSpecialization.AcademicLevel)
//                   .Where(x => x.Status == 9 && x.StudentId == Id
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Course.Name.Contains(Search.Trim()) ||
//                       x.Course.Subject.Name.Contains(Search.Trim()) ||
//                       x.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                       x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                       x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                       x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                       x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                   ).Select(x => new
//                   {
//                       x.Id,
//                       x.CourseId,
//                       x.Course.Name,
//                       x.Course.Image,
//                       Instructor = x.Course.Instructor.User.Name,
//                       x.Course.AcademicSpecialization.AcademicLevelId,
//                       AcademicLevel = x.Course.AcademicSpecialization.AcademicLevel.Name,
//                       x.Course.AcademicSpecializationId,
//                       AcademicSpecialization = x.Course.AcademicSpecialization.Name,
//                       x.Course.SubjectId,
//                       Subject = x.Course.Subject.Name,
//                       x.Course.Price,
//                       x.Value,
//                       x.DropResone,
//                       DropBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                       x.DropOn,
//                       x.ReturnedValue,
//                       x.Status,
//                       x.CreatedOn,
//                       CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                   }).OrderBy(x => x.CreatedOn).ToList();




//                return Ok(new { info = Info, count = Count, Statistics, dropInfo = DropInfo });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Courses/Add")]
//        public IActionResult AddStudentsCourses([FromBody] CoursesBodyObject bodyObject)
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

//                if (bodyObject.Value <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ValueEmpty);


//                var isExist = db.StudentsCourses.Where(x => x.StudentId == bodyObject.StudentId
//                        && x.CourseId == bodyObject.CourseId
//                        && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);


//                var Student = db.Students
//                    .Include(x=>x.Wallet)
//                    .Include(x => x.User).Where(x => x.User.Status != 9 && x.Id==bodyObject.StudentId).SingleOrDefault();
//                if(Student==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الطالب ");

//                var Course= db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Where(x => x.Id == bodyObject.CourseId && x.Status != 9 && x.Status != 3).SingleOrDefault();
//                if(Course==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الدورة التدريبية  ");

//                if (Course.Price > Student.Wallet.Value)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

//                if(bodyObject.Value>Course.Price)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName=Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    bodyObjectValue=bodyObject.Value,
//                });


//                StudentsWalletTracker studentsWalletTracker = new StudentsWalletTracker();
//                studentsWalletTracker.WalletId = Student.WalletId;
//                studentsWalletTracker.ProcessType = 1;
//                studentsWalletTracker.Descriptions = "  الإشتراك في دورة : " + Course.Name + " للمدرب : " + Course.Instructor.User.Name;
//                studentsWalletTracker.Value = bodyObject.Value;
//                studentsWalletTracker.Befroe = Student.Wallet.Value;
//                studentsWalletTracker.After = Student.Wallet.Value - bodyObject.Value;
//                studentsWalletTracker.Channel = 1;
//                studentsWalletTracker.CreatedBy = userId;
//                studentsWalletTracker.CreatedOn = DateTime.Now;
//                studentsWalletTracker.Status = 1;
//                db.StudentsWalletTracker.Add(studentsWalletTracker);


//                Student.Wallet.Value -= bodyObject.Value;

//                List<StudentsWalletPurchases> StudentWalletPurchasesList = new List<StudentsWalletPurchases>();
//                StudentsWalletPurchases StudentWalletPurchasesRow = new StudentsWalletPurchases();
//                StudentWalletPurchasesRow.WalletId = Student.WalletId;
//                StudentWalletPurchasesRow.Value = bodyObject.Value;
//                StudentWalletPurchasesRow.CoursePrice = Course.Price;
//                StudentWalletPurchasesRow.CreatedOn = DateTime.Now;
//                StudentWalletPurchasesRow.CreatedBy = userId;
//                StudentWalletPurchasesRow.Status = 1;
//                StudentWalletPurchasesList.Add(StudentWalletPurchasesRow);

                
                    


//                StudentsCourses row = new StudentsCourses();
//                row.StudentsWalletPurchases = StudentWalletPurchasesList;
//                row.CourseId = bodyObject.CourseId;
//                row.StudentId = bodyObject.StudentId;
//                row.Channel = 1;
//                row.RegisterType = 3;
//                row.Value = bodyObject.Value;
//                row.CreatedOn = DateTime.Now;
//                row.CreatedBy = userId;
//                row.Status = 1;
//                db.StudentsCourses.Add(row);

               





//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Students/Courses";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName = Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    bodyObjectValue = bodyObject.Value,
//                });
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





//        //CoursesValue
//        public partial class CoursesValueBodyObject
//        {
//            public long Id { get; set; }
//            public short Value { get; set; }
//        }

//        [HttpPost("Courses/AddValue")]
//        public IActionResult AddStudentsCoursesValue([FromBody] CoursesValueBodyObject bodyObject)
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

//                if (bodyObject.Value <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ValueEmpty);


//                var StudentCourse = db.StudentsCourses.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if(StudentCourse == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var Student = db.Students
//                    .Include(x => x.Wallet)
//                    .Include(x => x.User).Where(x => x.User.Status != 9 && x.Id == StudentCourse.StudentId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الطالب ");

//                var Course = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Where(x => x.Id == StudentCourse.CourseId && x.Status != 9 && x.Status != 3).SingleOrDefault();
//                if (Course == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الدورة التدريبية  ");

//                if ((Course.Price -StudentCourse.Value)> Student.Wallet.Value)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

//                if (bodyObject.Value > (Course.Price-StudentCourse.Value))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName = Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    bodyObjectValue = bodyObject.Value,
//                    PayiedValue=StudentCourse.Value,
//                });


//                StudentsWalletTracker studentsWalletTracker = new StudentsWalletTracker();
//                studentsWalletTracker.WalletId = Student.WalletId;
//                studentsWalletTracker.ProcessType = 1;
//                studentsWalletTracker.Descriptions = "  دفع قيمة مستحقة لدورة : " + Course.Name + " للمدرب : " + Course.Instructor.User.Name;
//                studentsWalletTracker.Value = bodyObject.Value;
//                studentsWalletTracker.Befroe = Student.Wallet.Value;
//                studentsWalletTracker.After = Student.Wallet.Value - bodyObject.Value;
//                studentsWalletTracker.Channel = 1;
//                studentsWalletTracker.CreatedBy = userId;
//                studentsWalletTracker.CreatedOn = DateTime.Now;
//                studentsWalletTracker.Status = 1;
//                db.StudentsWalletTracker.Add(studentsWalletTracker);



//                Student.Wallet.Value -= bodyObject.Value;

//                StudentsWalletPurchases StudentWalletPurchasesRow = new StudentsWalletPurchases();
//                StudentWalletPurchasesRow.StudentCourseId = StudentCourse.Id;
//                StudentWalletPurchasesRow.WalletId = Student.WalletId;
//                StudentWalletPurchasesRow.Value = bodyObject.Value;
//                StudentWalletPurchasesRow.CoursePrice = Course.Price;
//                StudentWalletPurchasesRow.CreatedOn = DateTime.Now;
//                StudentWalletPurchasesRow.CreatedBy = userId;
//                StudentWalletPurchasesRow.Status = 1;
//                db.StudentsWalletPurchases.Add(StudentWalletPurchasesRow);



//                StudentCourse.Value += bodyObject.Value;


//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة قيمة  ";
//                rowTrans.Controller = "Students/CoursesValur";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName = Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    bodyObjectValue = bodyObject.Value,
//                    PayiedValue = StudentCourse.Value,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessPayOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }



//        //CoursesValue
//        public partial class CoursesDropBodyObject
//        {
//            public long Id { get; set; }
//            public string DropResone { get; set; }
//        }

//        [HttpPost("Courses/Drop")]
//        public IActionResult DropStudentsCourses([FromBody] CoursesDropBodyObject bodyObject)
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

//                var StudentCourse = db.StudentsCourses.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (StudentCourse == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var Student = db.Students
//                    .Include(x => x.Wallet)
//                    .Include(x => x.User).Where(x => x.User.Status != 9 && x.Id == StudentCourse.StudentId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الطالب ");

//                var Course = db.Courses
//                    .Include(x=>x.Instructor)
//                    .Include(x=>x.Instructor.User)
//                    .Where(x => x.Id == StudentCourse.CourseId && x.Status != 9 && x.Status != 3).SingleOrDefault();
//                if (Course == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الدورة التدريبية  ");





//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName = Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    PayiedValue = StudentCourse.Value,
//                    StudentCourse.ReturnedValue,
//                });

//                StudentsWalletTracker studentsWalletTracker = new StudentsWalletTracker();
//                studentsWalletTracker.WalletId = Student.WalletId;
//                studentsWalletTracker.ProcessType = 2;
//                studentsWalletTracker.Descriptions = "  قيمة راجعة من عملية الإنسحاب من دورة  : " + Course.Name + " للمدرب : " + Course.Instructor.User.Name;
//                studentsWalletTracker.Value = StudentCourse.Value;
//                studentsWalletTracker.Befroe = Student.Wallet.Value;
//                studentsWalletTracker.After = Student.Wallet.Value + StudentCourse.Value;
//                studentsWalletTracker.Channel = 1;
//                studentsWalletTracker.CreatedBy = userId;
//                studentsWalletTracker.CreatedOn = DateTime.Now;
//                studentsWalletTracker.Status = 1;
//                db.StudentsWalletTracker.Add(studentsWalletTracker);


//                StudentCourse.ReturnedValue = StudentCourse.Value;
//                StudentCourse.Value -= StudentCourse.Value;
               
//                Student.Wallet.Value += StudentCourse.Value;
//                StudentCourse.DropResone = bodyObject.DropResone;
//                StudentCourse.DropBy = userId;
//                StudentCourse.DropOn = DateTime.Now;
//                StudentCourse.Status=9;



//                rowTrans.Operations = TransactionsType.Drop;
//                rowTrans.Descriptions = "الإنسحاب من دورة تدريبية  ";
//                rowTrans.Controller = "Students/CourseDrop";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    CourseName = Course.Name,
//                    Student.WalletId.Value,
//                    Course.Price,
//                    PayiedValue = StudentCourse.Value,
//                    StudentCourse.ReturnedValue,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessOut);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }











//        //Wallet
//        public partial class WalletBodyObject
//        {
//            public long StudentId { get; set; }
//            public int Value { get; set; }
//            public short PaymentMethodId { get; set; }
//        }

//        [HttpGet("Wallet/Get")]
//        public IActionResult GetWallet(long Id)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var Student = db.Students.Where(x => x.Id == Id).SingleOrDefault();
//                if(Student==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var Info = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Wallet.Status != 9 && x.WalletId==Student.WalletId
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        PaymentMethod=x.PaymentMethod.Name,
//                        x.PaymentMethodId,
//                        x.Value,
//                        x.ProcessType,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).ToList();

//                var InfoPurchases = db.StudentsWalletPurchases
//                   .Include(x => x.Wallet)
//                   .Include(x => x.Wallet.Students)
//                   .Include(x => x.StudentCourse)
//                   .Include(x => x.StudentCourse.Course)
//                   .Include(x => x.StudentCourse.Course.Instructor)
//                   .Include(x => x.StudentCourse.Course.Instructor.User)
//                   .Include(x => x.StudentCourse.Course.Subject)
//                   .Include(x => x.StudentCourse.Course.AcademicSpecialization)
//                   .Include(x => x.StudentCourse.Course.AcademicSpecialization.AcademicLevel)
//                   .Where(x => x.Wallet.Status != 9 && x.WalletId == Student.WalletId
//                   ).Select(x => new
//                   {
//                       x.Id,
//                       x.StudentCourse.Course.Name,
//                       x.StudentCourse.Course.Image,
//                       Subject=x.StudentCourse.Course.Subject.Name,
//                       AcademicLevel=x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name,
//                       AcademicSpecialization=x.StudentCourse.Course.AcademicSpecialization.Name,
//                       Instructor=x.StudentCourse.Course.Instructor.User.Name,
//                       x.Value,
//                       x.CoursePrice,
//                       x.Status,
//                       x.CreatedOn,
//                       CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                   }).OrderByDescending(x => x.CreatedOn).ToList();
                
                
//                var InfoTracker = db.StudentsWalletTracker
//                   .Include(x => x.Wallet)
//                   .Include(x => x.Wallet.Students)
//                   .Where(x => x.Wallet.Status != 9 && x.WalletId == Student.WalletId
//                   ).Select(x => new
//                   {
//                       x.Id,
//                       x.ProcessType,
//                       x.Channel,
//                       x.Descriptions,
//                       x.Value,
//                       x.Befroe,
//                       x.After,
//                       x.Status,
//                       x.CreatedOn,
//                       CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                   }).OrderByDescending(x => x.CreatedOn).ToList();


//                return Ok(new { info = Info , InfoPurchases, InfoTracker });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Wallet/Charge")]
//        public IActionResult ChargeWallet([FromBody] WalletBodyObject bodyObject)
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

//                var Student = db.Students
//                    .Include(x => x.Wallet)
//                    .Include(x => x.User).Where(x => x.User.Status != 9 && x.Id == bodyObject.StudentId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الطالب ");



//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    Student.WalletId.Value,
//                    AddValue=bodyObject.Value,
//                    bodyObject.PaymentMethodId,
//                });

//                StudentsWalletTracker studentsWalletTracker = new StudentsWalletTracker();
//                studentsWalletTracker.WalletId = Student.WalletId;
//                studentsWalletTracker.ProcessType = 3;
//                studentsWalletTracker.Descriptions = "شحن المحفظة الإلكترونية ";
//                studentsWalletTracker.Value = bodyObject.Value;
//                studentsWalletTracker.Befroe = Student.Wallet.Value;
//                studentsWalletTracker.After = Student.Wallet.Value + bodyObject.Value;
//                studentsWalletTracker.Channel = 1;
//                studentsWalletTracker.CreatedBy = userId;
//                studentsWalletTracker.CreatedOn = DateTime.Now;
//                studentsWalletTracker.Status = 1;
//                db.StudentsWalletTracker.Add(studentsWalletTracker);


//                Student.Wallet.Value += bodyObject.Value;


//                StudentsWalletTransactions row = new StudentsWalletTransactions();
//                row.WalletId = Student.WalletId;
//                row.PaymentMethodId = bodyObject.PaymentMethodId;
//                row.Value = bodyObject.Value;
//                row.ProcessType = 1;
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.StudentsWalletTransactions.Add(row);


//                rowTrans.Operations = TransactionsType.Drop;
//                rowTrans.Descriptions = "شحن المحفظ الإلكترونية   ";
//                rowTrans.Controller = "Students/Wallet";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    Student.Name,
//                    Student.User.Phone,
//                    Student.WalletId.Value,
//                    AddValue = bodyObject.Value,
//                    bodyObject.PaymentMethodId,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessPayOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }










//        //Devices
//        [HttpGet("Devices/Get")]
//        public IActionResult GetDevices(long Id)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Student = db.Students.Where(x => x.Id == Id).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                var Info = db.UsersDevices
//                    .Include(x => x.User)
//                    .Where(x => x.Status != 9 && x.UserId == Student.UserId
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.MachineName,
//                        x.IpAddress,
//                        x.Name,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).ToList();

//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }








//        //ChartInfo
//        [HttpGet("Chart/GetAll")]
//        public IActionResult GetAllChartInfo()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);



//                int DeviceRequestCount = db.UsersChangeRequest.Where(x => x.Status != 9).Count();
//                int WaitingDeviceReques = db.UsersChangeRequest.Where(x => x.Status ==1).Count();
//                int AcceptDeviceReques = db.UsersChangeRequest.Where(x => x.Status ==2).Count();
//                int RejectDeviceReques = db.UsersChangeRequest.Where(x => x.Status ==3).Count();
//                double AvgDeviceReques = DeviceRequestCount > 0 ? (double)(AcceptDeviceReques+RejectDeviceReques)  / DeviceRequestCount : 0;


//                int CoursesRequestsCount = db.StudentsCoursesRequests.Where(x => x.Status != 9).Count();
//                int WaitingCoursesRequests = db.StudentsCoursesRequests.Where(x => x.Status == 1).Count();
//                int AcceptCoursesRequests = db.StudentsCoursesRequests.Where(x => x.Status == 2).Count();
//                int RejectCoursesRequests = db.StudentsCoursesRequests.Where(x => x.Status == 3).Count();
//                double AvgCoursesRequests = CoursesRequestsCount > 0 ? (double)(AcceptCoursesRequests+RejectCoursesRequests) / CoursesRequestsCount : 0;





//                int StudentCount = db.Students.Include(x => x.User).Where(x => x.User.Status != 9).Count();
//                DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
//                DateTime endOfWeek = startOfWeek.AddDays(7);

//                // Count Student added this week
//                int StudentCountThisWeek = db.Students.Include(x=>x.User).Where(x => x.User.Status != 9 && x.User.CreatedOn >= startOfWeek && x.User.CreatedOn < endOfWeek).Count();

//                //Student Chart This Week
//                int[] DailyEnrolledCounts = new int[8];
//                for (int i = 1; i < 8; i++)
//                {
//                    DateTime day = startOfWeek.AddDays(i); // Get each day of the week
//                    int count = db.Students.Include(x=>x.User)
//                        .Where(x => x.User.Status != 9 && x.User.CreatedOn.Value.Date == day.Date)
//                        .Count();

//                    DailyEnrolledCounts[i] = count; // Store the count in the dictionary
//                }


//                int Wallet = db.StudentsWallet.Where(x => x.Status != 9).Sum(x => x.Value).Value;
//                int Pay = db.StudentsCourses.Where(x => x.Status != 9).Sum(x => x.Value).Value;
//                int Free = db.StudentsCourses.Where(x => x.Status != 9 && x.Value == 0).Count();






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
//                    .Take(5) // Get top 5
//                    .ToList();


//                //Top Student Exam 
//                var TopFiveStudentsExam = db.StudentsExams
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
//                    DeviceRequestCount,
//                    WaitingDeviceReques,
//                    AcceptDeviceReques,
//                    RejectDeviceReques,
//                    AvgDeviceReques,

//                    CoursesRequestsCount,
//                    WaitingCoursesRequests,
//                    AcceptCoursesRequests,
//                    RejectCoursesRequests,
//                    AvgCoursesRequests,


//                    StudentCount,
//                    StudentCountThisWeek,
//                    DailyEnrolledCounts,
//                    Wallet,
//                    Pay,
//                    Free,


//                    TopFiveStudents,
//                    TopFiveStudentsExam,
//                    TopStudentsByPayment,

//                };


//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }








//        //[HttpGet("Get")]
//        //public IActionResult Get(int pageNo, int pageSize, long id)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        if (id <= 0)
//        //        {

//        //            int Count = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4).Count();
//        //            var Info = db.Students
//        //                .Include(k => k.User)
//        //                .Include(k => k.StudentsProfiles)
//        //                .Include(k => k.StudentWallet)
//        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.User.Status != 9).Select(x => new
//        //                {
//        //                    UserInfo = new
//        //                    {
//        //                        x.UserId,
//        //                        x.User.LoginName,
//        //                        x.User.Email,
//        //                        x.User.Phone,
//        //                        x.User.Gender,
//        //                        x.User.ImagePath,
//        //                        x.User.CreatedOn,
//        //                        x.User.CreatedBy,
//        //                        x.User.Status,
//        //                        x.User.Levels,
//        //                        x.User.LastLoginOn,
//        //                        x.User.BirthDate,
//        //                        x.User.ExtraPhone,
//        //                    },
//        //                    StudentProfile = db.StudentsProfiles
//        //                .Include(k => k.AcademicSpecialization)
//        //                .Include(k => k.AcademicSpecialization.AcademicLevel)
//        //                .Where(k => k.Status == 1 && k.Id == x.StudentsProfiles.Where(k => k.Status == 1).FirstOrDefault().Id).Select(k => new
//        //                {

//        //                    AcademicSpecialization = k.AcademicSpecialization.Name,
//        //                    AcademicLevel = k.AcademicSpecialization.AcademicLevel.Name,
//        //                    CourseCount = k.StudentCourses.Where(k => k.Status == 1).Count(),
//        //                    CourseSumCash = k.StudentCourses.Where(k => k.Status == 1).Sum(k => k.Value),


//        //                }).FirstOrDefault(),
//        //                    WalletValue = x.StudentWallet.Where(k => k.Status != 9).FirstOrDefault().Value,
//        //                    x.Id,
//        //                    x.FullName,
//        //                    x.FirstName,
//        //                    x.FatherName,
//        //                    x.GrandFatherName,
//        //                    x.SirName,
//        //                    x.AcountPoints,
//        //                    x.AcountRate,
//        //                    x.CreatedOn,
//        //                    x.Status,
//        //                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //            return Ok(new { info = Info, count = Count });
//        //        }
//        //        else
//        //        {

//        //            int Count = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == id).Count();
//        //            var Info = db.Students
//        //                .Include(k => k.User)
//        //                .Include(k => k.StudentsProfiles)
//        //                .Include(k => k.StudentWallet)
//        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == id).Select(x => new
//        //                {
//        //                    UserInfo = new
//        //                    {
//        //                        x.UserId,
//        //                        x.User.LoginName,
//        //                        x.User.Email,
//        //                        x.User.Phone,
//        //                        x.User.Gender,
//        //                        x.User.ImagePath,
//        //                        x.User.CreatedOn,
//        //                        x.User.CreatedBy,
//        //                        x.User.Status,
//        //                        x.User.Levels,
//        //                        x.User.LastLoginOn,
//        //                        x.User.BirthDate,
//        //                        x.User.ExtraPhone,
//        //                    },
//        //                    StudentProfile = db.StudentsProfiles
//        //                .Include(k => k.AcademicSpecialization)
//        //                .Include(k => k.AcademicSpecialization.AcademicLevel)
//        //                .Where(k => k.Status == 1 && k.Id == x.StudentsProfiles.Where(k => k.Status == 1).FirstOrDefault().Id).Select(k => new
//        //                {

//        //                    AcademicSpecialization = k.AcademicSpecialization.Name,
//        //                    AcademicLevel = k.AcademicSpecialization.AcademicLevel.Name,
//        //                    CourseCount = k.StudentCourses.Where(k => k.Status == 1).Count(),
//        //                    CourseSumCash = k.StudentCourses.Where(k => k.Status == 1).Sum(k => k.Value),


//        //                }).FirstOrDefault(),
//        //                    WalletValue = x.StudentWallet.Where(k => k.Status != 9).FirstOrDefault().Value,
//        //                    x.Id,
//        //                    x.FullName,
//        //                    x.FirstName,
//        //                    x.FatherName,
//        //                    x.GrandFatherName,
//        //                    x.SirName,
//        //                    x.AcountPoints,
//        //                    x.AcountRate,
//        //                    x.CreatedOn,
//        //                    x.Status,
//        //                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //            return Ok(new { info = Info, count = Count });
//        //        }

//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetStudentPhone")]
//        //public IActionResult GetStudentPhone(string code)
//        //{
//        //    try
//        //    {

//        //        var userId = this.help.GetCurrentUser(HttpContext);

//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//        //        if (code.StartsWith("0"))
//        //            code = code.Substring(1);


//        //        var Info = db.Students.Include(x => x.User).Where(x => x.Status != 9 && x.User.Phone.Contains(code)).Select(x => new
//        //        {
//        //            x.Id,
//        //            Name = x.FullName,
//        //            Phone = x.User.Phone,
//        //        }).ToList();
//        //        return Ok(new { info = Info });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetStudentByStudentName")]
//        //public IActionResult GetStudentByStudentName(string code)
//        //{
//        //    try
//        //    {

//        //        var userId = this.help.GetCurrentUser(HttpContext);

//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//        //        var Info = db.Students.Include(x => x.User).Where(x => x.Status != 9 && x.FullName.Contains(code)).Select(x => new
//        //        {
//        //            x.Id,
//        //            Name = x.FullName,
//        //            x.User.Phone,
//        //        }).ToList();
//        //        return Ok(new { info = Info });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetByName")]
//        //public IActionResult GetByName(int pageNo, int pageSize, int Id)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
//        //        if (Id > 0)
//        //        {
//        //            int Count = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == Id).Count();
//        //            var Info = db.Students
//        //                .Include(k => k.User)
//        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.User.Levels == 4 && x.Id == Id).Select(x => new
//        //                {
//        //                    UserInfo = new
//        //                    {
//        //                        x.UserId,
//        //                        x.User.LoginName,
//        //                        x.User.Email,
//        //                        x.User.Phone,
//        //                        x.User.Gender,
//        //                        x.User.ImagePath,
//        //                        x.User.CreatedOn,
//        //                        x.User.CreatedBy,
//        //                        x.User.Status,
//        //                        x.User.Levels,
//        //                        x.User.LastLoginOn,
//        //                        x.User.BirthDate,
//        //                        x.User.ExtraPhone,
//        //                    },
//        //                    x.Id,
//        //                    x.FullName,
//        //                    x.FirstName,
//        //                    x.FatherName,
//        //                    x.GrandFatherName,
//        //                    x.SirName,
//        //                    x.AcountPoints,
//        //                    x.AcountRate,
//        //                    x.CreatedOn,
//        //                    x.Status,
//        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //            return Ok(new { info = Info, count = Count });
//        //        }
//        //        else
//        //        {
//        //            int Count = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4).Count();
//        //            var Info = db.Students
//        //                .Include(k => k.User)
//        //                .Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
//        //                {
//        //                    UserInfo = new
//        //                    {
//        //                        x.UserId,
//        //                        x.User.LoginName,
//        //                        x.User.Email,
//        //                        x.User.Phone,
//        //                        x.User.Gender,
//        //                        x.User.ImagePath,
//        //                        x.User.CreatedOn,
//        //                        x.User.CreatedBy,
//        //                        x.User.Status,
//        //                        x.User.Levels,
//        //                        x.User.LastLoginOn,
//        //                        x.User.BirthDate,
//        //                        x.User.ExtraPhone,
//        //                    },
//        //                    x.Id,
//        //                    x.FullName,
//        //                    x.FirstName,
//        //                    x.FatherName,
//        //                    x.GrandFatherName,
//        //                    x.SirName,
//        //                    x.AcountPoints,
//        //                    x.AcountRate,
//        //                    x.CreatedOn,
//        //                    x.Status,
//        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //            return Ok(new { info = Info, count = Count });
//        //        }
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetByPhone")]
//        //public IActionResult GetByPhone(int pageNo, int pageSize, int Id)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
//        //        if (Id > 0)
//        //        {
//        //            int Count = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == Id).Count();
//        //            var Info = db.Students
//        //                .Include(k => k.User)
//        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.User.Levels == 4 && x.Id == Id).Select(x => new
//        //                {
//        //                    UserInfo = new
//        //                    {
//        //                        x.UserId,
//        //                        x.User.LoginName,
//        //                        x.User.Email,
//        //                        x.User.Phone,
//        //                        x.User.Gender,
//        //                        x.User.ImagePath,
//        //                        x.User.CreatedOn,
//        //                        x.User.CreatedBy,
//        //                        x.User.Status,
//        //                        x.User.Levels,
//        //                        x.User.LastLoginOn,
//        //                        x.User.BirthDate,
//        //                        x.User.ExtraPhone,
//        //                    },
//        //                    x.Id,
//        //                    x.FullName,
//        //                    x.FirstName,
//        //                    x.FatherName,
//        //                    x.GrandFatherName,
//        //                    x.SirName,
//        //                    x.AcountPoints,
//        //                    x.AcountRate,
//        //                    x.CreatedOn,
//        //                    x.Status,
//        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //            return Ok(new { info = Info, count = Count });
//        //        }
//        //        else
//        //        {
//        //            int Count = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4).Count();
//        //            var Info = db.Students
//        //                .Include(k => k.User)
//        //                .Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
//        //                {
//        //                    UserInfo = new
//        //                    {
//        //                        x.UserId,
//        //                        x.User.LoginName,
//        //                        x.User.Email,
//        //                        x.User.Phone,
//        //                        x.User.Gender,
//        //                        x.User.ImagePath,
//        //                        x.User.CreatedOn,
//        //                        x.User.CreatedBy,
//        //                        x.User.Status,
//        //                        x.User.Levels,
//        //                        x.User.LastLoginOn,
//        //                        x.User.BirthDate,
//        //                        x.User.ExtraPhone,
//        //                    },
//        //                    x.Id,
//        //                    x.FullName,
//        //                    x.FirstName,
//        //                    x.FatherName,
//        //                    x.GrandFatherName,
//        //                    x.SirName,
//        //                    x.AcountPoints,
//        //                    x.AcountRate,
//        //                    x.CreatedOn,
//        //                    x.Status,
//        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //            return Ok(new { info = Info, count = Count });
//        //        }
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetNames")]
//        //public IActionResult GetNames()
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//        //        var Info = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
//        //        {
//        //            x.Id,
//        //            Name = x.FullName,
//        //        }).OrderByDescending(x => x.Name).ToList();

//        //        return Ok(new { info = Info });

//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetPhoneStudents")]
//        //public IActionResult GetPhoneStudents()
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
//        //        var Info = db.Students.Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
//        //        {

//        //            x.Id,
//        //            x.User.Phone,
//        //        }).OrderByDescending(x => x.Phone).ToList();
//        //        return Ok(new { info = Info });

//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //public class BodyObject
//        //{
//        //    public long? Id { get; set; }

//        //    public string FirstName { get; set; }
//        //    public string FatherName { get; set; }
//        //    public string GrandFatherName { get; set; }
//        //    public string SirName { get; set; }

//        //    public string LoginName { get; set; }
//        //    public DateTime? BirthDate { get; set; }
//        //    public short? Gender { get; set; }

//        //    public string Phone { get; set; }
//        //    public string ExtraPhone { get; set; }
//        //    public string Email { get; set; }
//        //    public long? LocationId { get; set; }

//        //    public int Value { get; set; }

//        //    public long AcademicSpecializationId { get; set; }

//        //    public string ImageName { get; set; }
//        //    public string ImageType { get; set; }
//        //    public string FileBase64 { get; set; }
//        //}

//        //[HttpPost("Add")]
//        //[DisableRequestSizeLimit]
//        //public IActionResult Add([FromBody] BodyObject bodyObject)
//        //{
//        //    try
//        //    {
//        //        if (bodyObject == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        //validations
//        //        if (string.IsNullOrEmpty(bodyObject.LoginName))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

//        //        if (!string.IsNullOrEmpty(bodyObject.Email))
//        //        {
//        //            if (!help.IsValidEmail(bodyObject.Email))
//        //                return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);
//        //        }

//        //        if (string.IsNullOrEmpty(bodyObject.Phone))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//        //        if (!help.IsValidPhone(bodyObject.Phone))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

//        //        if (string.IsNullOrEmpty(bodyObject.FirstName) ||
//        //            string.IsNullOrEmpty(bodyObject.FatherName) ||
//        //            string.IsNullOrEmpty(bodyObject.GrandFatherName) ||
//        //            string.IsNullOrEmpty(bodyObject.SirName))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


//        //        //isExist
//        //        var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
//        //        if (IsExist != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//        //        if (!string.IsNullOrEmpty(bodyObject.Email))
//        //        {
//        //            IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9).SingleOrDefault();
//        //            if (IsExist != null)
//        //                return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
//        //        }

//        //        IsExist = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9).SingleOrDefault();
//        //        if (IsExist != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

//        //        string FullName = bodyObject.FirstName + " " + bodyObject.FatherName + " " + bodyObject.GrandFatherName + " " + bodyObject.SirName;

//        //        var IsExistStudent = db.Students.Where(x => x.FirstName == FullName).SingleOrDefault();
//        //        if (IsExistStudent != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);


//        //        Users user = new Users();
//        //        user.Name = bodyObject.LoginName;
//        //        user.LoginName = bodyObject.LoginName;
//        //        user.Phone = bodyObject.Phone;
//        //        user.LocationId = bodyObject.LocationId;
//        //        user.ExtraPhone = bodyObject.ExtraPhone;
//        //        user.Email = bodyObject.Email;
//        //        user.Password = Security.ComputeHash("12345", HashAlgorithms.SHA512, null);
//        //        //user.Password = Security.ComputeHash(help.GenreatePass(bodyObject.BirthDate.GetValueOrDefault().Year.ToString()), HashAlgorithms.SHA512, null);
//        //        user.UserType = 60;
//        //        if (!string.IsNullOrEmpty(bodyObject.ImageName))
//        //        {
//        //            user.ImageName = bodyObject.ImageName;
//        //            user.ImagePath = this.help.UploadFile(bodyObject.ImageName, bodyObject.ImageType, bodyObject.FileBase64);
//        //        }
//        //        else
//        //            user.ImagePath = this.help.UploadFile(bodyObject.LoginName, ".jpg", help.GetDefaultImage());
//        //        user.BirthDate = bodyObject.BirthDate;
//        //        user.Gender = bodyObject.Gender;
//        //        user.CreatedBy = userId;
//        //        user.CreatedOn = DateTime.Now;
//        //        user.Levels = 4;
//        //        user.Status = 1;

//        //        //Student Wallet
//        //        List<StudentWallet> studentWallets = new List<StudentWallet>();
//        //        StudentWallet studentWallet = new StudentWallet();
//        //        studentWallet.Value = bodyObject.Value;
//        //        studentWallet.CreatedOn = DateTime.Now;
//        //        studentWallet.CreatedBy = userId;
//        //        studentWallet.Status = 1;
//        //        studentWallets.Add(studentWallet);

//        //        List<StudentsProfiles> studentsProfiles = new List<StudentsProfiles>();
//        //        StudentsProfiles studentsProfile = new StudentsProfiles();
//        //        studentsProfile.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
//        //        studentsProfile.CourseCount = 0;
//        //        studentsProfile.Points = 0;
//        //        studentsProfile.CreatedOn = DateTime.Now;
//        //        studentsProfile.CreatedBy = userId;
//        //        studentsProfile.Status = 1;
//        //        studentsProfiles.Add(studentsProfile);

//        //        Students row = new Students();
//        //        row.FirstName = bodyObject.FirstName;
//        //        row.FatherName = bodyObject.FatherName;
//        //        row.GrandFatherName = bodyObject.GrandFatherName;
//        //        row.SirName = bodyObject.SirName;
//        //        row.FullName = FullName;
//        //        row.AcountRate = 2;
//        //        row.AcountPoints = 0;
//        //        row.CreatedBy = userId;
//        //        row.CreatedOn = DateTime.Now;
//        //        row.ModifiedBy = userId;
//        //        row.ModifiedOn = DateTime.Now;
//        //        row.Status = 1;
//        //        row.User = user;
//        //        row.StudentWallet = studentWallets;
//        //        row.StudentsProfiles = studentsProfiles;
//        //        db.Students.Add(row);
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessAddOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("Edit")]
//        //public IActionResult Edit([FromBody] BodyObject bodyObject)
//        //{
//        //    try
//        //    {
//        //        if (bodyObject == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        if (bodyObject.Id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Info = db.Students.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        //validations
//        //        if (string.IsNullOrEmpty(bodyObject.LoginName))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

//        //        if (string.IsNullOrEmpty(bodyObject.Email))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

//        //        if (!help.IsValidEmail(bodyObject.Email))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

//        //        if (string.IsNullOrEmpty(bodyObject.Phone))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//        //        if (!help.IsValidPhone(bodyObject.Phone))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

//        //        if (string.IsNullOrEmpty(bodyObject.FirstName) ||
//        //            string.IsNullOrEmpty(bodyObject.FatherName) ||
//        //            string.IsNullOrEmpty(bodyObject.GrandFatherName) ||
//        //            string.IsNullOrEmpty(bodyObject.SirName))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


//        //        //isExist
//        //        var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 && x.Id != Info.UserId).SingleOrDefault();
//        //        if (IsExist != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//        //        if (!string.IsNullOrEmpty(bodyObject.Email))
//        //        {
//        //            IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9 && x.Id != Info.UserId).SingleOrDefault();
//        //            if (IsExist != null)
//        //                return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
//        //        }

//        //        IsExist = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9 && x.Id != Info.UserId).SingleOrDefault();
//        //        if (IsExist != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

//        //        string FullName = bodyObject.FirstName + " " + bodyObject.FatherName + " " + bodyObject.GrandFatherName + " " + bodyObject.SirName;

//        //        var IsExistStudent = db.Students.Where(x => x.FullName == FullName && x.Id != bodyObject.Id).SingleOrDefault();
//        //        if (IsExistStudent != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);


//        //        Info.User.LoginName = bodyObject.LoginName;
//        //        Info.User.Email = bodyObject.Email;
//        //        Info.User.BirthDate = bodyObject.BirthDate;
//        //        Info.User.Gender = bodyObject.Gender;
//        //        Info.User.ModifiedBy = userId;
//        //        Info.User.ModifiedOn = DateTime.Now;
//        //        Info.FirstName = bodyObject.FirstName;
//        //        Info.FatherName = bodyObject.FatherName;
//        //        Info.GrandFatherName = bodyObject.GrandFatherName;
//        //        Info.SirName = bodyObject.SirName;
//        //        Info.FullName = FullName;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessEditOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{Id}/Delete")]
//        //public IActionResult Delete(long Id)
//        //{
//        //    try
//        //    {
//        //        if (Id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.Students.Include(x => x.User).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        Info.Status = 9;
//        //        Info.User.Status = 9;
//        //        Info.User.ModifiedBy = userId;
//        //        Info.User.ModifiedOn = DateTime.Now;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessDeleteOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{Id}/ResetPassword")]
//        //public IActionResult ResetPassword(long Id)
//        //{
//        //    try
//        //    {
//        //        if (Id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.Students.Include(x => x.User).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var User = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
//        //        if (User == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        string Password = this.help.GenreatePass();
//        //        User.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);
//        //        User.ModifiedBy = userId;
//        //        User.ModifiedOn = DateTime.Now;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessEditOperations + " " + "كلمة المرور الجديدة : " + Password);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/ChangeStatus")]
//        //public IActionResult ChangeStatus(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.Students.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        if (Info.Status == 1)
//        //        {
//        //            Info.Status = 2;
//        //        }
//        //        else if (Info.Status == 2)
//        //        {
//        //            Info.Status = 1;
//        //        }

//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SuccessChangeStatus);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/ChangeStatusAcount")]
//        //public IActionResult ChangeStatusAcount(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.Students.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        if (Info.User.Status == 1)
//        //        {
//        //            Info.User.Status = 2;
//        //        }
//        //        else if (Info.User.Status == 2)
//        //        {
//        //            Info.User.Status = 1;
//        //        }

//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SuccessChangeStatus);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/ActiveAcount")]
//        //public IActionResult ActiveAcount(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Student = db.Students.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Student == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Info = db.Users.Where(x => x.Status != 9 && x.Id == Student.UserId).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        Info.Status = 1;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SuccessChangeStatus);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}





//        ////Student Profile
//        //public class StudentsProfileBodyObject
//        //{
//        //    public long? Id { get; set; }
//        //    public long StudentId { get; set; }
//        //    public long AcademicSpecializationId { get; set; }
//        //}

//        ////Student Profile 
//        //[HttpPost("AddStudentsProfile")]
//        //public IActionResult AddStudentsProfile([FromBody] StudentsProfileBodyObject bodyObject)
//        //{
//        //    try
//        //    {
//        //        if (bodyObject == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var IsExist = db.StudentsProfiles.Where(x => x.StudentId == bodyObject.StudentId && x.AcademicSpecializationId == bodyObject.AcademicSpecializationId
//        //        && x.Status != 9).SingleOrDefault();
//        //        if (IsExist != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecordExist);

//        //        StudentsProfiles row = new StudentsProfiles();
//        //        row.StudentId = bodyObject.StudentId;
//        //        row.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
//        //        row.CourseCount = 0;
//        //        row.Points = 0;
//        //        row.CreatedOn = DateTime.Now;
//        //        row.CreatedBy = userId;
//        //        db.StudentsProfiles.Add(row);
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessAddOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}







//        //public class StudentCoursesBodyObject
//        //{
//        //    public long? Id { get; set; }
//        //    public long StudentId { get; set; }
//        //    public long CourseId { get; set; }
//        //    public int Value { get; set; }
//        //}

//        ////Student Course 
//        //[HttpPost("AddStudentCourse")]
//        //public IActionResult AddStudentCourse([FromBody] StudentCoursesBodyObject bodyObject)
//        //{
//        //    try
//        //    {
//        //        if (bodyObject == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//        //        var StudentProfile = db.StudentsProfiles.Where(x => x.StudentId == bodyObject.StudentId && x.Status == 1).SingleOrDefault();
//        //        if (StudentProfile == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Course = db.Courses.Where(x => x.Id == bodyObject.CourseId).SingleOrDefault();
//        //        if (Course == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var IsExist = db.StudentCourses.Where(x => x.StudentProfileId == StudentProfile.Id && x.CourseId == bodyObject.CourseId
//        //        && x.Status != 9).SingleOrDefault();
//        //        if (IsExist != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecordExist);


//        //        /////////////Repleace with method for get mony 
//        //        ///
//        //        ///
//        //        ///
//        //        ///
//        //        /// 
//        //        var walit = db.StudentWallet.Where(x => x.StudentId == bodyObject.StudentId).SingleOrDefault();

//        //        if (Course.Price > walit.Value)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

//        //        walit.Value -= bodyObject.Value;

//        //        List<StudentWalletPurchases> StudentWalletPurchasesList = new List<StudentWalletPurchases>();
//        //        StudentWalletPurchases StudentWalletPurchasesRow = new StudentWalletPurchases();
//        //        StudentWalletPurchasesRow.WalletId = walit.Id;
//        //        StudentWalletPurchasesRow.Value = bodyObject.Value;
//        //        StudentWalletPurchasesRow.CoursePrice = Course.Price;
//        //        StudentWalletPurchasesRow.CreatedOn = DateTime.Now;
//        //        StudentWalletPurchasesRow.CreatedBy = userId;
//        //        StudentWalletPurchasesRow.Status = 1;
//        //        StudentWalletPurchasesList.Add(StudentWalletPurchasesRow);
//        //        /// 
//        //        ///
//        //        ///
//        //        ///
//        //        ///



//        //        StudentCourses row = new StudentCourses();
//        //        row.StudentWalletPurchases = StudentWalletPurchasesList;
//        //        row.StudentProfileId = StudentProfile.Id;
//        //        row.CourseId = bodyObject.CourseId;
//        //        row.Value = bodyObject.Value;
//        //        row.CreatedOn = DateTime.Now;
//        //        row.CreatedBy = userId;
//        //        db.StudentCourses.Add(row);
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessAddOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetStudentCourse")]
//        //public IActionResult GetStudentCourse(long id)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Student = db.Students.Where(x => x.Status != 9 && x.Id == id).SingleOrDefault();
//        //        if (Student == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var StudentProfile = db.StudentsProfiles.Where(x => x.StudentId == id && x.Status == 1).SingleOrDefault();
//        //        if (StudentProfile == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Info = db.StudentCourses
//        //            .Include(x => x.Course)
//        //            .Include(x => x.Course.Subject)
//        //            .Include(x => x.Course.AcademicSpecialization)
//        //            .Include(x => x.StudentWalletPurchases)
//        //            .Where(x => x.StudentProfileId == StudentProfile.Id && x.Status != 9)
//        //            .Select(x => new
//        //            {

//        //                x.Id,
//        //                x.CourseId,
//        //                Course = new
//        //                {
//        //                    x.Course.Name,
//        //                    x.Course.TeacherName,
//        //                    Subject = x.Course.Subject.Name,
//        //                    Specialization = x.Course.AcademicSpecialization.Name,
//        //                    x.Course.Price,
//        //                    x.Course.TaregerLevel,
//        //                },
//        //                x.Value,
//        //                x.CreatedOn,
//        //                x.CreatedBy
//        //            }).OrderByDescending(x => x.CreatedOn).ToList();

//        //        return Ok(new { info = Info });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //public class PayStudentCourseBodyObject
//        //{
//        //    public long? Id { get; set; }
//        //    public int Value { get; set; }
//        //}

//        ////Student Course 
//        //[HttpPost("PayStudentCourse")]
//        //public IActionResult PayStudentCourse([FromBody] PayStudentCourseBodyObject bodyObject)
//        //{
//        //    try
//        //    {
//        //        if (bodyObject == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var StudentCourse = db.StudentCourses.Include(x => x.Course).Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//        //        if (StudentCourse == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        if (StudentCourse.Course.Price <= StudentCourse.Value)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PayBefore);

//        //        int remind = StudentCourse.Course.Price.GetValueOrDefault() - StudentCourse.Value.GetValueOrDefault();
//        //        if (remind <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PayBefore);

//        //        if (bodyObject.Value > remind)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);

//        //        StudentCourse.Value += bodyObject.Value;

//        //        if (StudentCourse.Value > StudentCourse.Course.Price)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);



//        //        var StudentProfile = db.StudentsProfiles.Where(x => x.Id == StudentCourse.StudentProfileId && x.Status == 1).SingleOrDefault();
//        //        if (StudentProfile == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Course = db.Courses.Where(x => x.Id == StudentCourse.CourseId).SingleOrDefault();
//        //        if (Course == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


//        //        /////////////Repleace with method for get mony 
//        //        ///
//        //        ///
//        //        ///
//        //        ///
//        //        /// 
//        //        var walit = db.StudentWallet.Where(x => x.StudentId == StudentProfile.StudentId).SingleOrDefault();

//        //        if (remind > walit.Value)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

//        //        walit.Value -= bodyObject.Value;

//        //        StudentWalletPurchases StudentWalletPurchasesRow = new StudentWalletPurchases();
//        //        StudentWalletPurchasesRow.StudentCourseId = StudentCourse.Id;
//        //        StudentWalletPurchasesRow.WalletId = walit.Id;
//        //        StudentWalletPurchasesRow.Value = bodyObject.Value;
//        //        StudentWalletPurchasesRow.CoursePrice = Course.Price;
//        //        StudentWalletPurchasesRow.CreatedOn = DateTime.Now;
//        //        StudentWalletPurchasesRow.CreatedBy = userId;
//        //        StudentWalletPurchasesRow.Status = 1;
//        //        db.StudentWalletPurchases.Add(StudentWalletPurchasesRow);


//        //        TransactionsObject rowTrans = new TransactionsObject();
//        //        rowTrans.Operations = TransactionsType.Pay;
//        //        rowTrans.ItemId = StudentWalletPurchasesRow.Id;
//        //        rowTrans.Descriptions = " الاشتراك في كورس ودفع القيمة   ";
//        //        rowTrans.Controller = "Student/StudentWalletPurchasesRow";
//        //        rowTrans.NewObject = JsonConvert.SerializeObject(StudentWalletPurchasesRow, Formatting.None,
//        //                new JsonSerializerSettings()
//        //                {
//        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//        //                });
//        //        rowTrans.CreatedBy = userId;
//        //        this.help.WriteTransactions(rowTrans);

//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessAddOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/OutFromStudentCourse")]
//        //public IActionResult OutFromStudentCourse(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var StudentCourse = db.StudentCourses.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (StudentCourse == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var StudentProfile = db.StudentsProfiles.Where(x => x.Id == StudentCourse.StudentProfileId).SingleOrDefault();
//        //        if (StudentProfile == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Student = db.Students
//        //            .Where(x => x.Id == StudentProfile.StudentId && x.Status != 9).SingleOrDefault();
//        //        if (Student == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Wallet = db.StudentWallet.Where(x => x.StudentId == Student.Id && x.Status != 9).SingleOrDefault();
//        //        if (Wallet == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        Wallet.Value += StudentCourse.Value;
//        //        StudentCourse.Status = 9;
//        //        StudentCourse.CreatedBy = userId;
//        //        StudentCourse.CreatedOn = DateTime.Now;


//        //        TransactionsObject rowTrans = new TransactionsObject();
//        //        rowTrans.Operations = TransactionsType.Delete;
//        //        rowTrans.ItemId = StudentCourse.Id;
//        //        rowTrans.Descriptions = "  حذف طالب من الدورة وارجاع القيمة المالية الخاصة به   ";
//        //        rowTrans.Controller = "Student/StudentCourse";
//        //        rowTrans.NewObject = JsonConvert.SerializeObject(StudentCourse, Formatting.None,
//        //                new JsonSerializerSettings()
//        //                {
//        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//        //                });
//        //        rowTrans.CreatedBy = userId;
//        //        this.help.WriteTransactions(rowTrans);

//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessOut);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}







//        ////Student Wallet
//        //[HttpGet("GetWalletInfo")]
//        //public IActionResult GetWalletInfo(long id)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Student = db.Students.Where(x => x.Status != 9 && x.Id == id).SingleOrDefault();
//        //        if (Student == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Info = db.StudentWallet
//        //            .Include(x => x.StudentWalletTransactions)
//        //            .Include(x => x.StudentWalletPurchases)
//        //            .Where(x => x.StudentId == id && x.Status != 9)
//        //            .Select(x => new
//        //            {

//        //                x.Id,
//        //                x.Value,
//        //                x.CreatedOn,
//        //                Transactions = x.StudentWalletTransactions.Where(k => k.Status != 9).OrderByDescending(k => k.CreatedOn)
//        //                    .Select(k => new
//        //                    {
//        //                        k.Id,
//        //                        k.Value,
//        //                        k.ProcessType,
//        //                        k.PaymentMethodId,
//        //                        k.CreatedOn,
//        //                        CreatedBy = db.Users.Where(z => z.Id == k.CreatedBy).SingleOrDefault().Name,
//        //                    }).ToList(),
//        //                Purchases = x.StudentWalletPurchases.Where(k => k.Status != 9).OrderByDescending(k => k.CreatedOn).ToList(),
//        //            }).SingleOrDefault();

//        //        return Ok(new { info = Info });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("GetWalletPurchasesInfo")]
//        //public IActionResult GetWalletPurchasesInfo(long id)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Student = db.Students.Where(x => x.Status != 9 && x.Id == id).SingleOrDefault();
//        //        if (Student == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Wallet = db.StudentWallet.Where(x => x.StudentId == id && x.Status != 9).SingleOrDefault();
//        //        if (Wallet == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Info = db.StudentWalletPurchases
//        //            .Include(x => x.StudentCourse)
//        //            .Include(x => x.StudentCourse.Course)
//        //            .Where(x => x.WalletId == Wallet.Id && x.Status != 9).Select(x => new
//        //            {
//        //                x.Id,
//        //                x.Value,
//        //                x.CoursePrice,
//        //                Course = new
//        //                {
//        //                    x.StudentCourse.Course.Name,
//        //                    AcademicSpecialization = x.StudentCourse.Course.AcademicSpecialization.Name,
//        //                    Subject = x.StudentCourse.Course.Subject.Name,
//        //                },
//        //                x.CreatedOn,
//        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,

//        //            }).OrderByDescending(x => x.CreatedOn).ToList();
//        //        return Ok(new { info = Info });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //public class StudentWalletBodyObject
//        //{
//        //    public long? Id { get; set; }
//        //    public long StudentId { get; set; }
//        //    public short PaymentMethodId { get; set; }
//        //    public short ProcessType { get; set; }
//        //    public int Value { get; set; }
//        //}

//        ////Student Profile 
//        //[HttpPost("RechargeWallet")]
//        //public IActionResult RechargeWallet([FromBody] StudentWalletBodyObject bodyObject)
//        //{
//        //    try
//        //    {
//        //        if (bodyObject == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        if (bodyObject.Value <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.ValueEmpty);

//        //        if (bodyObject.PaymentMethodId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.PaymentMethodEmpty);

//        //        var Wallet = db.StudentWallet.Where(x => x.StudentId == bodyObject.StudentId && x.Status != 9).SingleOrDefault();
//        //        if (Wallet == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        StudentWalletTransactions row = new StudentWalletTransactions();
//        //        row.WalletId = Wallet.Id;
//        //        row.PaymentMethodId = bodyObject.PaymentMethodId;
//        //        row.Value = bodyObject.Value;
//        //        row.ProcessType = bodyObject.ProcessType;
//        //        row.CreatedOn = DateTime.Now;
//        //        row.CreatedBy = userId;
//        //        row.Status = 1;
//        //        db.StudentWalletTransactions.Add(row);
//        //        Wallet.Value += bodyObject.Value;


//        //        TransactionsObject rowTrans = new TransactionsObject();
//        //        rowTrans.Operations = TransactionsType.Pay;
//        //        rowTrans.ItemId = row.Id;
//        //        rowTrans.Descriptions = "  شحن محفظة طالب  ";
//        //        rowTrans.Controller = "Student/RechargeWallet";
//        //        rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
//        //                new JsonSerializerSettings()
//        //                {
//        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//        //                });
//        //        rowTrans.CreatedBy = userId;
//        //        this.help.WriteTransactions(rowTrans);

//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessAddOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{Id}/DeletetWalletTransacitons")]
//        //public IActionResult DeletetWalletTransacitons(long Id)
//        //{
//        //    try
//        //    {
//        //        if (Id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.StudentWalletTransactions.Include(x => x.Wallet).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Wallet = db.StudentWallet.Where(x => x.Id == Info.WalletId && x.Status != 9).SingleOrDefault();
//        //        if (Wallet == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        Info.Status = 9;
//        //        Info.CreatedBy = userId;
//        //        Info.CreatedOn = DateTime.Now;
//        //        Wallet.Value -= Info.Value;

//        //        StudentWalletTransactions row = new StudentWalletTransactions();
//        //        row.WalletId = Wallet.Id;
//        //        row.PaymentMethodId = 1;
//        //        row.Value = Info.Value;
//        //        row.ProcessType = 2;
//        //        row.CreatedOn = DateTime.Now;
//        //        row.CreatedBy = userId;
//        //        row.Status = 1;
//        //        db.StudentWalletTransactions.Add(row);

//        //        TransactionsObject rowTrans = new TransactionsObject();
//        //        rowTrans.Operations = TransactionsType.Delete;
//        //        rowTrans.ItemId = row.Id;
//        //        rowTrans.Descriptions = "  حذف عملية من حركات الحافظة  ";
//        //        rowTrans.Controller = "Student/StudentWalletTransactions";
//        //        rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
//        //                new JsonSerializerSettings()
//        //                {
//        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//        //                });
//        //        rowTrans.CreatedBy = userId;
//        //        this.help.WriteTransactions(rowTrans);

//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessDeleteOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}


//        ////Student Subscribtions
//        //[HttpGet("Subscriptions/Get")]
//        //public IActionResult GetStudentSubscribtions(int pageNo, int pageSize, long AcademicLevelId, long AcademicSpecializationId, long SubjectId,
//        //    long CourseId, long studentId, short SubscriptionType)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        int Count = db.StudentCourses
//        //            .Include(x => x.Course)
//        //            .Include(x => x.Course.AcademicSpecialization)
//        //            .Include(x => x.StudentProfile)
//        //            .Where(x => x.Status != 9
//        //        && (AcademicLevelId > 0 ? x.Course.AcademicSpecialization.AcademicLevelId == AcademicLevelId : true)
//        //        && (AcademicSpecializationId > 0 ? x.Course.AcademicSpecializationId == AcademicSpecializationId : true)
//        //        && (SubjectId > 0 ? x.Course.SubjectId == SubjectId : true)
//        //        && (CourseId > 0 ? x.CourseId == CourseId : true)
//        //        && (studentId > 0 ? x.StudentProfile.StudentId == studentId : true)
//        //        && (SubscriptionType == 1 ? x.Value > 0 : true)
//        //        && (SubscriptionType == 2 ? x.Value == 0 : true)

//        //        ).Count();
//        //        var Info = db.StudentCourses
//        //            .Include(k => k.Course)
//        //            .Include(k => k.Course.AcademicSpecialization)
//        //            .Include(k => k.Course.Subject)
//        //            .Include(k => k.StudentProfile)
//        //            .Include(k => k.StudentProfile.Student)
//        //            .Include(k => k.StudentProfile.Student.User)
//        //            .Where(x => x.Status != 9
//        //            && (AcademicLevelId > 0 ? x.Course.AcademicSpecialization.AcademicLevelId == AcademicLevelId : true)
//        //            && (AcademicSpecializationId > 0 ? x.Course.AcademicSpecializationId == AcademicSpecializationId : true)
//        //            && (SubjectId > 0 ? x.Course.SubjectId == SubjectId : true)
//        //            && (CourseId > 0 ? x.CourseId == CourseId : true)
//        //            && (studentId > 0 ? x.StudentProfile.StudentId == studentId : true)
//        //            && (SubscriptionType == 2 ? x.Value == 0 : true)
//        //            && (SubscriptionType == 1 ? x.Value > 0 : true)

//        //            )
//        //            .Select(x => new
//        //            {
//        //                x.Id,
//        //                x.Value,
//        //                Course = new
//        //                {
//        //                    x.CourseId,
//        //                    x.Course.Name,
//        //                    x.Course.Price,
//        //                    AcademicSpecialization = x.Course.AcademicSpecialization.Name,
//        //                    Subject = x.Course.Subject.Name,
//        //                },
//        //                Student = new
//        //                {
//        //                    x.StudentProfile.StudentId,
//        //                    x.StudentProfile.Student.FullName,
//        //                    x.StudentProfile.Student.User.Phone,
//        //                    x.StudentProfile.Student.User.ImagePath
//        //                },
//        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//        //                x.CreatedOn,
//        //                x.Status,
//        //            }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //        return Ok(new { info = Info, count = Count });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}








//        //public class SuspendBodyObject
//        //{
//        //    public long StudentId { get; set; }
//        //    public string Resone { get; set; }
//        //}

//        ////Student Suspend 
//        //[HttpPost("Suspend/Add")]
//        //public IActionResult SuspendStudent([FromBody] SuspendBodyObject bodyObject)
//        //{
//        //    try
//        //    {
//        //        if (bodyObject == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        if (string.IsNullOrEmpty(bodyObject.Resone))
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.ResoneEmpty);

//        //        var Student = db.Students.Include(x => x.User).Where(x => x.Id == bodyObject.StudentId && x.Status != 9).SingleOrDefault();
//        //        if (Student == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        if (Student.User.Status == 3)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.SuspendBefore);


//        //        Student.User.Status = 3;
//        //        UserSuspends row = new UserSuspends();
//        //        row.UserId = Student.UserId;
//        //        row.Resone = bodyObject.Resone;
//        //        row.CreatedOn = DateTime.Now;
//        //        row.CreatedBy = userId;
//        //        row.Status = 1;
//        //        db.UserSuspends.Add(row);
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessAddOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpGet("Suspend/Get")]
//        //public IActionResult GetStudentsSuspend(int pageNo, int pageSize, long studentId)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        int Count = db.Students
//        //            .Include(x => x.User)
//        //            .Where(x => x.User.Status == 3 && x.Status != 9
//        //            && (studentId > 0 ? x.Id == studentId : true)
//        //            ).Count();
//        //        var Info = db.Students
//        //            .Include(x => x.User)
//        //            .Include(x => x.User.UserSuspends)
//        //            .Where(x => x.User.Status == 3 && x.Status != 9
//        //            && (studentId > 0 ? x.Id == studentId : true)
//        //            ).Select(x => new
//        //            {
//        //                x.Id,
//        //                x.FullName,
//        //                x.User.Phone,
//        //                x.User.ImagePath,
//        //                Resone = db.UserSuspends.Where(k => k.UserId == x.UserId).FirstOrDefault().Resone,
//        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//        //                x.CreatedOn,
//        //                x.Status,
//        //            }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //        return Ok(new { info = Info, count = Count });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}


//        //[HttpPost("{id}/Suspend/UnSuspend")]
//        //public IActionResult UnSuspend(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.Students.Include(x => x.User).Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var StudentSuspend = db.UserSuspends.Include(x => x.User).Where(x => x.UserId == Info.UserId).ToList();
//        //        if (StudentSuspend.Count > 0)
//        //        {
//        //            foreach (var item in StudentSuspend)
//        //            {
//        //                item.Status = 9;
//        //                item.User.Status = 9;
//        //            }
//        //        }
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SuccessChangeStatus);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}



//        ////Change Device Request
//        //[HttpGet("ChangeRequests/Get")]
//        //public IActionResult GetChangeRequests(int pageNo, int pageSize, long studentId)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        int Count = db.ChangeRequest
//        //            .Include(x => x)
//        //            .Where(x => x.Status != 9
//        //            && (studentId > 0 ? db.Students.Where(k => k.UserId == x.UserId && k.Id == studentId).Count() > 0 : true)
//        //        ).Count();
//        //        var Info = db.ChangeRequest
//        //            .Where(x => x.Status != 9
//        //            && (studentId > 0 ? db.Students.Where(k => k.UserId == x.UserId && k.Id == studentId).Count() > 0 : true)
//        //            ).Select(x => new
//        //            {
//        //                x.Id,
//        //                x.Name,
//        //                Phone = db.Users.Where(k => k.Id == x.UserId).SingleOrDefault().Phone,
//        //                ProssedBy = db.Users.Where(k => k.Id == x.ProssedBy).SingleOrDefault().Name,
//        //                Count = db.ChangeRequest.Where(k => k.UserId == x.UserId && x.Status != 9).Count(),
//        //                x.OldMachineName,
//        //                x.NewMachineName,
//        //                x.NewIpAddress,
//        //                x.OldIpAddress,
//        //                CountProsedProsses = db.ChangeRequest.Where(k => k.UserId == x.UserId && x.Status != 9 && k.ProssedBy > 0).Count(),
//        //                CountProsses = db.ChangeRequest.Where(k => k.UserId == x.UserId && x.Status != 9).Count(),
//        //                x.CreatedOn,
//        //                LastLoginOn = db.Users.Where(k => k.Id == x.UserId).SingleOrDefault().LastLoginOn,
//        //                x.Status,
//        //            }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //        return Ok(new { info = Info, count = Count });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/ChangeRequests/Approve")]
//        //public IActionResult Approve(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//        //        if (user == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.ChangeRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var Count = db.ChangeRequest.Where(x => x.UserId == Info.UserId && x.Status != 9 && x.ProssedBy > 0).Count();
//        //        if (Count <= 10)
//        //        {
//        //            var row = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
//        //            row.MachineName = Info.NewMachineName;
//        //            Info.Status = 2;
//        //            Info.ProssedBy = userId;
//        //            db.SaveChanges();
//        //            return Ok(BackMessages.SucessAcceptedRequest);
//        //        }
//        //        else
//        //        {
//        //            if (user.UserType != 1)
//        //                return StatusCode(BackMessages.StatusCode, BackMessages.MaxChangeRequest);

//        //            var row = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
//        //            row.MachineName = Info.NewMachineName;
//        //            Info.Status = 2;
//        //            Info.ProssedBy = userId;
//        //            db.SaveChanges();
//        //            return Ok(BackMessages.SucessAcceptedRequest);
//        //        }



//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/ChangeRequests/ResetDeviecInfo")]
//        //public IActionResult ResetDeviecInfo(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.Students.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var row = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
//        //        row.MachineName = null;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessSaveOperations);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}


//        //[HttpPost("{id}/ChangeRequests/Reject")]
//        //public IActionResult Reject(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.ChangeRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        Info.Status = 3;
//        //        Info.ProssedBy = userId;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessRejectRequest);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}





//        ////Regester Course Request
//        //[HttpGet("RegesterCourse/Get")]
//        //public IActionResult GetRegesterCourseRequest(int pageNo, int pageSize, long CourseId, long studentId)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        int Count = db.StudentCoursesRequest.Where(x => x.Status != 9
//        //        && (CourseId > 0 ? x.CourseId == CourseId : true)
//        //        && (studentId > 0 ? x.StudentId == studentId : true)
//        //        ).Count();
//        //        var Info = db.StudentCoursesRequest
//        //            .Where(x => x.Status != 9
//        //            && (CourseId > 0 ? x.CourseId == CourseId : true)
//        //        && (studentId > 0 ? x.StudentId == studentId : true)
//        //            ).Select(x => new
//        //            {
//        //                x.Id,
//        //                x.Name,
//        //                x.Phone,
//        //                x.CourseName,
//        //                ProssedBy = db.Users.Where(k => k.Id == x.ProssedBy).SingleOrDefault().Name,
//        //                x.CreatedOn,
//        //                x.Status,
//        //            }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //        return Ok(new { info = Info, count = Count });
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/RegesterCourse/Approve")]
//        //public IActionResult ApproveRegesterCourseRequest(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.StudentCoursesRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        var StudentsProfile = db.StudentsProfiles.Where(x => x.StudentId == Info.StudentId && x.Status == 1).SingleOrDefault();
//        //        if (StudentsProfile == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


//        //        var isExist = db.StudentCourses.Where(x => x.StudentProfileId == StudentsProfile.Id && x.CourseId == Info.CourseId && x.Status == 1).SingleOrDefault();
//        //        if (isExist != null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);

//        //        StudentCourses row = new StudentCourses();
//        //        row.StudentProfileId = StudentsProfile.Id;
//        //        row.CourseId = Info.CourseId;
//        //        row.Value = 0;
//        //        row.CreatedOn = DateTime.Now;
//        //        row.CreatedBy = userId;
//        //        row.Status = 1;
//        //        db.StudentCourses.Add(row);
//        //        Info.Status = 2;
//        //        Info.ProssedBy = userId;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessAcceptedRequest);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}


//        //[HttpPost("{id}/RegesterCourse/Reject")]
//        //public IActionResult RejectRegesterCourseRequest(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.StudentCoursesRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        Info.Status = 3;
//        //        Info.ProssedBy = userId;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessRejectRequest);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}

//        //[HttpPost("{id}/RegesterCourse/Delete")]
//        //public IActionResult DeleteRegesterCourseRequest(long id)
//        //{
//        //    try
//        //    {
//        //        if (id <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        var Info = db.StudentCoursesRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
//        //        if (Info == null)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//        //        Info.Status = 9;
//        //        Info.ProssedBy = userId;
//        //        db.SaveChanges();
//        //        return Ok(BackMessages.SucessRejectRequest);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}


//    }
//}