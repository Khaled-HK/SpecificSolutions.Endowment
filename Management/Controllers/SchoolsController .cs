using Common;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Model.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.TransactionsInfo;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/Schools")]
    public class SchoolsController : Controller
    {
        private Security security;
        private TransactionsInfo transactions;
        private FileHandler fileHandler;
        IConfiguration configuration;

        private readonly JeelContext db;

        public SchoolsController(IConfiguration iConfig, JeelContext context)
        {
            this.db = context;
            security = new Security(iConfig, context);
            transactions = new TransactionsInfo(iConfig, context);
            fileHandler = new FileHandler(iConfig, context);
        }


        public partial class BodyObject
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Descriptions { get; set; }
            public string UserName { get; set; }
            public string LoginName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Image { get; set; }
            public string ImageName { get; set; }
        }


        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize, string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.Schools.Where(x => x.User.Status!=9).Count(),
                    Active = db.Schools.Where(x => x.User.Status==1).Count(),
                    NotActive = db.Schools.Where(x => x.User.Status == 3).Count(),
                    HaveMony = db.Schools.Include(x=>x.Wallet).Where(x => x.Wallet.Value > 0).Sum(x=>x.Wallet.Value).Value,
                };

                int Count = db.Schools
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.User.Status != 3
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.User.Phone.Contains(Search.Trim()) ||
                        //x.Rate.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Count();

                var Info = db.Schools
                     .Include(x => x.User)
                     .Include(x=>x.Wallet)
                    .Where(x => x.User.Status != 9 && x.User.Status != 3
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.User.Phone.Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Descriptions,
                        x.Image,
                        x.SubscriptionsStatus,
                        x.UserId,
                        UserName=x.User.Name,
                        x.User.Phone,
                        x.User.LoginName,
                        x.User.Email,
                        UserImage=x.User.Image,
                        x.User.LastLoginOn,
                        SubscriptionsCount = db.SchoolsSubscriptions.Where(k=>k.SchoolsId==x.Id && k.Status!=9).Count(),
                        ParentCount = db.Users.Where(k=>k.SchoolsId==x.Id && k.Status!=9).Count(),
                        StudentsCount = db.Students.Include(k=>k.Parent).Where(k=>k.Parent.SchoolsId==x.Id && k.Status!=9).Count(),
                        WalletValue = x.Wallet.Value,
                        CreatedOn = x.User.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.User.CreatedBy).SingleOrDefault().Name,
                        x.User.Status,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                int Count = db.Schools.Include(x=>x.User)
                    .Where(x => x.User.Status != 9).Count();
                var Info = db.Schools.Include(x => x.User)
                    .Where(x => x.User.Status != 9).Select(x => new
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
                if (string.IsNullOrEmpty(bodyObject.LoginName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

                if (!string.IsNullOrEmpty(bodyObject.Email))
                {
                    if (!Validations.IsValidEmail(bodyObject.Email))
                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);
                }

                if (string.IsNullOrEmpty(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (!Validations.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.SchoolNameEmpty);
                
                if (string.IsNullOrEmpty(bodyObject.UserName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


                //isExist
                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameExist);

                if (!string.IsNullOrEmpty(bodyObject.Email))
                {
                    IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9).SingleOrDefault();
                    if (IsExist != null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
                }

                IsExist = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

                db.Users.Where(x => x.Name == bodyObject.UserName && x.Status != 9).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                var IsExistSchool = db.Schools
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.Name == bodyObject.Name).SingleOrDefault();
                if(IsExistSchool!=null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.SchoolNameExist);


                Users user = new Users();
                user.Name = bodyObject.Name;
                user.LoginName = bodyObject.LoginName;
                user.Phone = bodyObject.Phone;
                user.Email = bodyObject.Email;
                string Password = Generate.GenreatePass();
                user.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);
                user.UserType = 40;
                user.Image = "/Uploads/User.jpg";
                user.CreatedBy = userId;
                user.CreatedOn = DateTime.Now;
                user.Status = 1;

                SchoolsWallet Wallet = new SchoolsWallet();
                Wallet.Value = 0;
                Wallet.CreatedOn = DateTime.Now;
                Wallet.CreatedBy = userId;
                Wallet.Status = 1;

                Schools row = new Schools();
                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                if (string.IsNullOrEmpty(bodyObject.Image))
                {
                    user.Image = "/Uploads/User.jpg";
                }
                else

                {
                    user.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);
                }
                row.SubscriptionsStatus = 2;
                row.User = user;
                row.Wallet = Wallet;
                db.Schools.Add(row);


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "Schools";
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
                if (string.IsNullOrEmpty(bodyObject.LoginName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

                if (!string.IsNullOrEmpty(bodyObject.Email))
                {
                    if (!Validations.IsValidEmail(bodyObject.Email))
                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);
                }

                if (string.IsNullOrEmpty(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

                if (!Validations.IsValidPhone(bodyObject.Phone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                if (string.IsNullOrEmpty(bodyObject.Name))
                    return StatusCode(BackMessages.StatusCode, BackMessages.SchoolNameEmpty);

                if (string.IsNullOrEmpty(bodyObject.UserName))
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


                //isExist
                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 && x.Id!=bodyObject.Id).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameExist);

                if (!string.IsNullOrEmpty(bodyObject.Email))
                {
                    IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
                    if (IsExist != null)
                        return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
                }

                TransactionsObject rowTrans = new TransactionsObject();
                var row = db.Schools.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.User.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var user = db.Users.Where(x => x.Status != 9 && x.Id == row.UserId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.WalletId,
                    row.UserId,
                    row.Descriptions,
                    row.User.Image,
                    row.User.LoginName,
                    row.User.Email,
                    row.User.Status,
                    UserName=row.User.Name,
                });


                IsExist = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

                db.Users.Where(x => x.Name == bodyObject.UserName && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
                if (IsExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

                var IsExistSchool = db.Schools
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.Name == bodyObject.Name && x.Id != bodyObject.Id).SingleOrDefault();
                if (IsExistSchool != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.SchoolNameExist);


                user.Name = bodyObject.Name;
                user.LoginName = bodyObject.LoginName;
                user.Phone = bodyObject.Phone;
                user.Email = bodyObject.Email;

                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                if (!string.IsNullOrEmpty(bodyObject.Image))
                    user.Image = fileHandler.UploadFile(bodyObject.ImageName, bodyObject.Image);


                rowTrans.Operations = TransactionsType.Edit;
                rowTrans.Descriptions = "تعديل بيانات  ";
                rowTrans.Controller = "Schools";
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

        [HttpPost("{Id}/ResetPassword")]
        public IActionResult ResetPassword(long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var School = db.Schools.Include(x => x.User).Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();
                if(School==null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var user = db.Users.Where(x => x.Id == School.UserId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var row = db.Schools.Include(x => x.User).Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


                string Password = Generate.GenreatePass();
                row.User.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Reset;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تهيئة كلمة المرور  ";
                rowTrans.Controller = "Schools";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.User.Phone,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessResetOperations + "كلمة المرور الجديدة : " + Password);
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

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var row = db.Schools
                    .Include(x => x.User)
                    .Include(x => x.Wallet)
                    .Where(x => x.Id == Id && x.User.Status != 9).SingleOrDefault();

                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                if (row.Wallet.Value > 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.CannotDeleteWithWalletValue);

                row.User.Status = 9;

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "حذف بيانات   ";
                rowTrans.Controller = "Schools";
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


        //Suspends
        [HttpPost("Suspends/Add")]
        public IActionResult SuspendsSchools([FromBody] SubscriptionsDropBodyObject bodyObject)
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

                if (string.IsNullOrEmpty(bodyObject.DropResone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.ResoneEmpty);


                var row = db.Schools.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.User.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                UsersSuspends usersSuspends = new UsersSuspends();
                usersSuspends.Resone = bodyObject.DropResone;
                usersSuspends.UserId = row.UserId;
                usersSuspends.CreatedOn = DateTime.Now;
                usersSuspends.CreatedBy = userId;
                usersSuspends.Status = 1;
                db.UsersSuspends.Add(usersSuspends);

                row.User.Status = 3;


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Block;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = " إيقاف الحساب  ";
                rowTrans.Controller = "Schools";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.User.Phone,
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

        [HttpGet("Suspends/Get")]
        public IActionResult GetSuspends(int pageNo, int pageSize, string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Count = db.Schools.Where(x => x.User.Status != 9).Count(),
                    Active = db.Schools.Where(x => x.User.Status == 1).Count(),
                    NotActive = db.Schools.Where(x => x.User.Status == 3).Count(),
                    HaveMony = db.Schools.Include(x => x.Wallet).Where(x => x.Wallet.Value > 0).Count(),
                };

                int Count = db.Schools
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.User.Status == 3
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.User.Phone.Contains(Search.Trim()) ||
                        //x.Rate.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Count();

                var Info = db.Schools
                     .Include(x => x.User)
                     .Include(x => x.Wallet)
                    .Where(x => x.User.Status != 9 && x.User.Status == 3
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
                        x.User.Phone.Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
                        x.User.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Descriptions,
                        x.Image,
                        x.SubscriptionsStatus,
                        x.UserId,
                        UserNam = x.User.Name,
                        x.User.Phone,
                        x.User.LoginName,
                        x.User.Email,
                        SusbendInfo = db.UsersSuspends.Where(k => k.UserId == x.UserId && k.Status != 9).SingleOrDefault(),
                        SusbendCreatedBy = db.Users.Where(k => k.Id == db.UsersSuspends.Where(k => k.UserId == x.UserId && k.Status != 9).SingleOrDefault().CreatedBy).SingleOrDefault().Name,
                        UserImage = x.User.Image,
                        x.User.LastLoginOn,
                        ParentCount = db.Users.Where(k => k.SchoolsId == x.Id && k.Status != 9).Count(),
                        StudentsCount = db.Students.Include(k => k.Parent).Where(k => k.Parent.SchoolsId == x.Id && k.Status != 9).Count(),
                        WalletValue = x.Wallet.Value,
                        CreatedOn = x.User.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.User.CreatedBy).SingleOrDefault().Name,
                        x.User.Status,
                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


                return Ok(new { info = Info, count = Count, Statistics });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{Id}/Suspends/Cansel")]
        public IActionResult CanselSuspends(long Id)
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


                var row = db.Schools.Include(x => x.User).Where(x => x.Id == Id && x.User.Status == 3).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var Susbend = db.UsersSuspends.Where(x => x.UserId == row.UserId && x.Status != 9).SingleOrDefault();
                if (Susbend == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                Susbend.Status = 9;
                row.User.Status = 1;


                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Delete;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تفعيل الحساب ";
                rowTrans.Controller = "Schools/Susbend";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.User.Phone,
                    row.UserId,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessActiveOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }







        //Subscriptions
        public partial class SubscriptionsBodyObject
        {
            public long? Id { get; set; }
            public short SchoolsId { get; set; }
            public short SubscriptionsTypeId { get; set; }
            public short Value { get; set; }
        }

        [HttpGet("Subscriptions/Get")]
        public IActionResult GetCourse(long Id, string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var Statistics = new
                {
                    Active = db.SchoolsSubscriptions.Where(x => x.Status == 1 && x.SchoolsId == Id).Count(),
                    Expiry = db.SchoolsSubscriptions.Where(x => x.Status == 2 && x.SchoolsId == Id).Count(),
                    Stopped = db.SchoolsSubscriptions.Where(x => x.Status == 3 && x.SchoolsId == Id).Count(),
                    Value = db.SchoolsSubscriptions.Where(x => x.Status != 9 && x.SchoolsId == Id).Sum(x=>x.Value).Value,
                    Deleted = db.SchoolsSubscriptions.Where(x => x.Status == 9 && x.SchoolsId == Id).Count()
                };

                int Count = db.SchoolsSubscriptions
                    .Include(x => x.SubscriptionsType)
                    .Where(x => x.Status != 9 && x.SchoolsId == Id
                    && (string.IsNullOrEmpty(Search) ? true : (x.SubscriptionsType.Name.Contains(Search.Trim())))
                    ).Count();
                var Info = db.SchoolsSubscriptions
                     .Include(x => x.SubscriptionsType)
                    .Where(x => x.Status != 9 && x.Status!=4 && x.SchoolsId == Id
                    && (string.IsNullOrEmpty(Search) ? true : (x.SubscriptionsType.Name.Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.SubscriptionsTypeId,
                        SubscriptionsType = new
                        {
                            x.SubscriptionsType.Name,
                            x.SubscriptionsType.Price,
                            x.SubscriptionsType.Lenght,
                            x.SubscriptionsType.Image,
                            x.SubscriptionsType.Descriptions,
                            x.CreatedOn,
                        },
                        x.SchoolsId,
                        x.StartDate,
                        x.EndDate,
                        x.Channel,
                        x.Price,
                        x.Value,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderBy(x => x.CreatedOn).ToList();
                
                var DropInfo = db.SchoolsSubscriptions
                     .Include(x => x.SubscriptionsType)
                    .Where(x => x.Status == 4 && x.SchoolsId == Id
                    && (string.IsNullOrEmpty(Search) ? true : (x.SubscriptionsType.Name.Contains(Search.Trim())))
                   ).Select(x => new
                   {
                       x.Id,
                       x.SubscriptionsTypeId,
                       SubscriptionsType = new
                       {
                           x.SubscriptionsType.Name,
                           x.SubscriptionsType.Price,
                           x.SubscriptionsType.Lenght,
                           x.SubscriptionsType.Image,
                           x.SubscriptionsType.Descriptions,
                           x.CreatedOn,
                       },
                       x.SchoolsId,
                       x.StartDate,
                       x.EndDate,
                       x.Price,
                       x.Value,
                       x.DropResone,
                       DropBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                       x.DropOn,
                       x.ReturnedValue,
                       x.Status,
                       x.CreatedOn,
                       CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                   }).OrderBy(x => x.CreatedOn).ToList();

                return Ok(new { info = Info, count = Count, Statistics, dropInfo = DropInfo });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Subscriptions/Add")]
        public IActionResult AddSchoolsSubscriptions([FromBody] SubscriptionsBodyObject bodyObject)
        {
            try
            {
                if (bodyObject == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

                var userId = this.security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (user.UserType != 1)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                if (bodyObject.Value <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ValueEmpty);


                var isExist = db.SchoolsSubscriptions.Where(x => x.SchoolsId == bodyObject.SchoolsId
                        && x.Status == 1).SingleOrDefault();
                if (isExist != null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ActiveSubscriptionsExist);


                var School = db.Schools
                    .Include(x => x.Wallet)
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.Id == bodyObject.SchoolsId).SingleOrDefault();
                if (School == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " المدرسة  ");

                var SubscriptionsType = db.SubscriptionsType
                    .Where(x => x.Id == bodyObject.SubscriptionsTypeId && x.Status != 9).SingleOrDefault();
                if (SubscriptionsType == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الباقة   ");

                if (SubscriptionsType.Price > School.Wallet.Value)
                    return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

                if (bodyObject.Value > SubscriptionsType.Price)
                    return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    SubscriptionsTypeName = SubscriptionsType.Name,
                    School.WalletId.Value,
                    SubscriptionsType.Price,
                    bodyObjectValue = bodyObject.Value,
                });


                SchoolsWalletTracker SchoolsWalletTracker = new SchoolsWalletTracker();
                SchoolsWalletTracker.WalletId = School.WalletId;
                SchoolsWalletTracker.ProcessType = 1;   
                SchoolsWalletTracker.Descriptions = "  الإشتراك في باقة : " + SubscriptionsType.Name + "لمدة  : " + SubscriptionsType.Lenght + " شهر";
                SchoolsWalletTracker.Value = bodyObject.Value;
                SchoolsWalletTracker.Befroe = School.Wallet.Value;
                SchoolsWalletTracker.After = School.Wallet.Value - bodyObject.Value;
                SchoolsWalletTracker.Channel = 1;
                SchoolsWalletTracker.CreatedBy = userId;
                SchoolsWalletTracker.CreatedOn = DateTime.Now;
                SchoolsWalletTracker.Status = 1;
                db.SchoolsWalletTracker.Add(SchoolsWalletTracker);


                School.Wallet.Value -= bodyObject.Value;

                List<SchoolsWalletPurchases> SchoolsWalletPurchasesList = new List<SchoolsWalletPurchases>();
                SchoolsWalletPurchases SchoolsWalletPurchasesRow = new SchoolsWalletPurchases();
                SchoolsWalletPurchasesRow.WalletId = School.WalletId;
                SchoolsWalletPurchasesRow.Value = bodyObject.Value;
                SchoolsWalletPurchasesRow.SubscriptionsPrice = SubscriptionsType.Price;
                SchoolsWalletPurchasesRow.CreatedOn = DateTime.Now;
                SchoolsWalletPurchasesRow.CreatedBy = userId;
                SchoolsWalletPurchasesRow.Status = 1;
                SchoolsWalletPurchasesList.Add(SchoolsWalletPurchasesRow);





                SchoolsSubscriptions row = new SchoolsSubscriptions();
                row.SchoolsWalletPurchases = SchoolsWalletPurchasesList;
                row.SchoolsId = bodyObject.SchoolsId;
                row.SubscriptionsTypeId = bodyObject.SubscriptionsTypeId;
                row.Channel = 1;
                row.Value = bodyObject.Value;
                row.Price = SubscriptionsType.Price;
                row.StartDate = DateTime.Now;
                row.EndDate = DateTime.Now.AddMonths(SubscriptionsType.Lenght.GetValueOrDefault());
                row.CreatedOn = DateTime.Now;
                row.CreatedBy = userId;
                row.Status = 1;
                db.SchoolsSubscriptions.Add(row);

                School.SubscriptionsStatus = 1;



                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة بيانات  ";
                rowTrans.Controller = "Schools/Subscriptions";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    SubscriptionsTypeName = SubscriptionsType.Name,
                    School.WalletId.Value,
                    SubscriptionsType.Price,
                    bodyObjectValue = bodyObject.Value,
                });
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





        //SubscriptionsValue
        public partial class SubscriptionsValueBodyObject
        {
            public long Id { get; set; }
            public short Value { get; set; }
        }

        [HttpPost("Subscriptions/AddValue")]
        public IActionResult AddSchoolsSubscriptionsValue([FromBody] SubscriptionsValueBodyObject bodyObject)
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

                if (bodyObject.Value <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.ValueEmpty);


                var SchoolsSubscription = db.SchoolsSubscriptions
                    .Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
                if (SchoolsSubscription == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var School = db.Schools
                    .Include(x => x.Wallet)
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.Id == SchoolsSubscription.SchoolsId).SingleOrDefault();
                if (School == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " المدرسة  ");

                var Subscription = db.SubscriptionsType
                    .Where(x => x.Id == SchoolsSubscription.SubscriptionsTypeId && x.Status != 9).SingleOrDefault();
                if (Subscription == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الباقة   ");

                if ((Subscription.Price - SchoolsSubscription.Value) > School.Wallet.Value)
                    return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

                if (bodyObject.Value > (Subscription.Price - SchoolsSubscription.Value))
                    return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    SubscriptionName = Subscription.Name,
                    School.WalletId.Value,
                    Subscription.Price,
                    bodyObjectValue = bodyObject.Value,
                    PayiedValue = SchoolsSubscription.Value,
                });


                SchoolsWalletTracker SchoolsWalletTracker = new SchoolsWalletTracker();
                SchoolsWalletTracker.WalletId = School.WalletId;
                SchoolsWalletTracker.ProcessType = 1;
                SchoolsWalletTracker.Descriptions = "  دفع قيمة مستحقة للياقة  : " + Subscription.Name;
                SchoolsWalletTracker.Value = bodyObject.Value;
                SchoolsWalletTracker.Befroe = School.Wallet.Value;
                SchoolsWalletTracker.After = School.Wallet.Value - bodyObject.Value;
                SchoolsWalletTracker.Channel = 1;
                SchoolsWalletTracker.CreatedBy = userId;
                SchoolsWalletTracker.CreatedOn = DateTime.Now;
                SchoolsWalletTracker.Status = 1;
                db.SchoolsWalletTracker.Add(SchoolsWalletTracker);



                School.Wallet.Value -= bodyObject.Value;

                SchoolsWalletPurchases SchoolWalletPurchasesRow = new SchoolsWalletPurchases();
                SchoolWalletPurchasesRow.SchoolsSubscriptionsId = SchoolsSubscription.Id;
                SchoolWalletPurchasesRow.WalletId = School.WalletId;
                SchoolWalletPurchasesRow.Value = bodyObject.Value;
                SchoolWalletPurchasesRow.SubscriptionsPrice = Subscription.Price;
                SchoolWalletPurchasesRow.CreatedOn = DateTime.Now;
                SchoolWalletPurchasesRow.CreatedBy = userId;
                SchoolWalletPurchasesRow.Status = 1;
                db.SchoolsWalletPurchases.Add(SchoolWalletPurchasesRow);



                SchoolsSubscription.Value += bodyObject.Value;


                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "إضافة قيمة  ";
                rowTrans.Controller = "Schools/SubscriptionsValur";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    CourseName = Subscription.Name,
                    School.WalletId.Value,
                    Subscription.Price,
                    bodyObjectValue = bodyObject.Value,
                    PayiedValue = SchoolsSubscription.Value,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessPayOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        //SubscriptionsValue
        public partial class SubscriptionsDropBodyObject
        {
            public long Id { get; set; }
            public string DropResone { get; set; }
        }

        [HttpPost("Subscriptions/Drop")]
        public IActionResult DropSchoolsSubscriptions([FromBody] SubscriptionsDropBodyObject bodyObject)
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

                if (string.IsNullOrEmpty(bodyObject.DropResone))
                    return StatusCode(BackMessages.StatusCode, BackMessages.ResoneEmpty);

                var SchoolsSubscription = db.SchoolsSubscriptions
                    .Where(x => x.Id == bodyObject.Id && x.Status == 1).SingleOrDefault();
                if (SchoolsSubscription == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var School = db.Schools
                    .Include(x => x.Wallet)
                    .Include(x => x.User)
                    .Where(x => x.User.Status != 9 && x.Id == SchoolsSubscription.SchoolsId).SingleOrDefault();
                if (School == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " المدرسة ");

                var SubscriptionsType = db.SubscriptionsType
                    .Where(x => x.Id == SchoolsSubscription.SubscriptionsTypeId && x.Status != 9).SingleOrDefault();
                if (SubscriptionsType == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " الاشتراك   ");





                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    SubscriptionsTypeName = SubscriptionsType.Name,
                    School.WalletId.Value,
                    SubscriptionsType.Price,
                    PayiedValue = SchoolsSubscription.Value,
                    SchoolsSubscription.ReturnedValue,
                });

                SchoolsWalletTracker SchoolsWalletTracker = new SchoolsWalletTracker();
                SchoolsWalletTracker.WalletId = School.WalletId;
                SchoolsWalletTracker.ProcessType = 2;
                SchoolsWalletTracker.Descriptions = "  قيمة راجعة من عملية الإنسحاب من باقة  : " + SubscriptionsType.Name;
                SchoolsWalletTracker.Value = SchoolsSubscription.Value;
                SchoolsWalletTracker.Befroe = School.Wallet.Value;
                SchoolsWalletTracker.After = School.Wallet.Value + SchoolsSubscription.Value;
                SchoolsWalletTracker.Channel = 1;
                SchoolsWalletTracker.CreatedBy = userId;
                SchoolsWalletTracker.CreatedOn = DateTime.Now;
                SchoolsWalletTracker.Status = 1;
                db.SchoolsWalletTracker.Add(SchoolsWalletTracker);


                SchoolsSubscription.ReturnedValue = SchoolsSubscription.Value;
                SchoolsSubscription.Value -= SchoolsSubscription.Value;

                School.Wallet.Value += SchoolsSubscription.Value;
                SchoolsSubscription.DropResone = bodyObject.DropResone;
                SchoolsSubscription.DropBy = userId;
                SchoolsSubscription.DropOn = DateTime.Now;
                SchoolsSubscription.Status = 4;



                rowTrans.Operations = TransactionsType.Drop;
                rowTrans.Descriptions = "الإنسحاب من باقة   ";
                rowTrans.Controller = "Schools/SubscriptionDrop";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    CourseName = SubscriptionsType.Name,
                    School.WalletId.Value,
                    SubscriptionsType.Price,
                    PayiedValue = SchoolsSubscription.Value,
                    SchoolsSubscription.ReturnedValue,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessOut);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }











        //Wallet
        public partial class WalletBodyObject
        {
            public long SchoolsId { get; set; }
            public int Value { get; set; }
            public short PaymentMethodId { get; set; }
        }

        [HttpGet("Wallet/Get")]
        public IActionResult GetWallet(long Id)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var Student = db.Schools.Include(x=>x.User).Where(x => x.Id == Id && x.User.Status!=9).SingleOrDefault();
                if (Student == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                var Info = db.SchoolsWalletTransactions
                    .Include(x => x.Wallet)
                    .Include(x => x.Wallet.Schools)
                    .Include(x => x.PaymentMethod)
                    .Where(x => x.Wallet.Status != 9 && x.WalletId == Student.WalletId
                    ).Select(x => new
                    {
                        x.Id,
                        PaymentMethod = x.PaymentMethod.Name,
                        x.PaymentMethodId,
                        x.Value,
                        x.ProcessType,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderByDescending(x => x.CreatedOn).ToList();

                var InfoPurchases = db.SchoolsWalletPurchases
                   .Include(x => x.Wallet)
                   .Include(x => x.Wallet.Schools)
                   .Include(x => x.SchoolsSubscriptions)
                   .Include(x => x.SchoolsSubscriptions.SubscriptionsType)
                   .Where(x => x.Wallet.Status != 9 && x.WalletId == Student.WalletId
                   ).Select(x => new
                   {
                       x.Id,
                       x.SchoolsSubscriptions.SubscriptionsType.Name,
                       x.SchoolsSubscriptions.SubscriptionsType.Image,
                       x.SchoolsSubscriptions.SubscriptionsType.Lenght,
                       x.Value,
                       x.SubscriptionsPrice,
                       x.Status,
                       x.CreatedOn,
                       CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                   }).OrderByDescending(x => x.CreatedOn).ToList();


                var InfoTracker = db.SchoolsWalletTracker
                   .Include(x => x.Wallet)
                   .Include(x => x.Wallet.Schools)
                   .Where(x => x.Wallet.Status != 9 && x.WalletId == Student.WalletId
                   ).Select(x => new
                   {
                       x.Id,
                       x.ProcessType,
                       x.Channel,
                       x.Descriptions,
                       x.Value,
                       x.Befroe,
                       x.After,
                       x.Status,
                       x.CreatedOn,
                       CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                   }).OrderByDescending(x => x.CreatedOn).ToList();


                return Ok(new { info = Info, InfoPurchases, InfoTracker });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Wallet/Charge")]
        public IActionResult ChargeWallet([FromBody] WalletBodyObject bodyObject)
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

                var School = db.Schools
                    .Include(x => x.Wallet)
                    .Include(x => x.User).Where(x => x.User.Status != 9 && x.Id == bodyObject.SchoolsId).SingleOrDefault();
                if (School == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " المدرسة  ");



                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    School.WalletId.Value,
                    AddValue = bodyObject.Value,
                    bodyObject.PaymentMethodId,
                });

                SchoolsWalletTracker SchoolsWalletTracker = new SchoolsWalletTracker();
                SchoolsWalletTracker.WalletId = School.WalletId;
                SchoolsWalletTracker.ProcessType = 3;
                SchoolsWalletTracker.Descriptions = "شحن المحفظة الإلكترونية ";
                SchoolsWalletTracker.Value = bodyObject.Value;
                SchoolsWalletTracker.Befroe = School.Wallet.Value;
                SchoolsWalletTracker.After = School.Wallet.Value + bodyObject.Value;
                SchoolsWalletTracker.Channel = 1;
                SchoolsWalletTracker.CreatedBy = userId;
                SchoolsWalletTracker.CreatedOn = DateTime.Now;
                SchoolsWalletTracker.Status = 1;
                db.SchoolsWalletTracker.Add(SchoolsWalletTracker);


                School.Wallet.Value += bodyObject.Value;


                SchoolsWalletTransactions row = new SchoolsWalletTransactions();
                row.WalletId = School.WalletId;
                row.PaymentMethodId = bodyObject.PaymentMethodId;
                row.Value = bodyObject.Value;
                row.ProcessType = 1;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 1;
                db.SchoolsWalletTransactions.Add(row);


                rowTrans.Operations = TransactionsType.Charge;
                rowTrans.Descriptions = "شحن المحفظ الإلكترونية   ";
                rowTrans.Controller = "Schools/Wallet";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    School.Name,
                    School.User.Phone,
                    School.WalletId.Value,
                    AddValue = bodyObject.Value,
                    bodyObject.PaymentMethodId,
                });
                rowTrans.CreatedBy = userId;
                transactions.WriteTransactions(rowTrans);

                db.SaveChanges();
                return Ok(BackMessages.SucessPayOperations);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }







        //ProfileYears
        public partial class ProfileYearsBodyObject
        {
            public long? Id { get; set; }
            public short? SchoolsId { get; set; }
            public string Name { get; set; }
            public string Descriptions { get; set; }
        }

        [HttpGet("ProfileYears/Get")]
        public IActionResult GetProfileYears(long Id, string Search)
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                int Count = db.ProfileYears
                    .Where(x => x.Status != 9 && x.SchoolsId == Id
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim())))
                    ).Count();
                var Info = db.ProfileYears
                    .Where(x => x.Status != 9 && x.SchoolsId == Id
                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim())))
                    ).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Descriptions,
                        x.Status,
                        x.CreatedOn,
                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
                    }).OrderBy(x => x.CreatedOn).ToList();

                return Ok(new { info = Info, count = Count});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("ProfileYears/Add")]
        public IActionResult AddProfileYears([FromBody] ProfileYearsBodyObject bodyObject)
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

                var School = db.Schools
                    .Include(x => x.Wallet)
                    .Include(x => x.User).Where(x => x.User.Status != 9 && x.Id == bodyObject.SchoolsId).SingleOrDefault();
                if (School == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFoundThe + " المدرسة  ");

                var isExist = db.ProfileYears.Where(x => x.SchoolsId == bodyObject.SchoolsId && x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
                if(isExist!=null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);
              

                ProfileYears row = new ProfileYears();
                row.SchoolsId = bodyObject.SchoolsId;
                row.Name = bodyObject.Name;
                row.Descriptions = bodyObject.Descriptions;
                row.CreatedBy = userId;
                row.CreatedOn = DateTime.Now;
                row.Status = 2;
                db.ProfileYears.Add(row);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.Operations = TransactionsType.Add;
                rowTrans.Descriptions = "فتح عام دراسي جديد ";
                rowTrans.Controller = "Schools/ProfileYears";
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

        [HttpPost("{Id}/ProfileYears/Active")]
        public IActionResult ActiveProfileYears(long Id)
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


                var row = db.ProfileYears.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Status
                });

                var IsAnyActive = db.ProfileYears.Where(x => x.SchoolsId == row.SchoolsId && x.Status == 1).SingleOrDefault();
                if(IsAnyActive!=null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.OnlyOneActiveProfileYears);

                row.Status = 1;

                rowTrans.Operations = TransactionsType.Open;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "تفعيل فصل دراسي  ";
                rowTrans.Controller = "Schools/ProfileYears";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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

        [HttpPost("{Id}/ProfileYears/Close")]
        public IActionResult CloseProfileYears(long Id)
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


                var row = db.ProfileYears.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
                if (row == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

                TransactionsObject rowTrans = new TransactionsObject();
                rowTrans.OldObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
                    row.Descriptions,
                    row.Status
                });

                row.Status = 2;

                rowTrans.Operations = TransactionsType.Close;
                rowTrans.ItemId = row.Id;
                rowTrans.Descriptions = "اغلاق فصل دراسي  ";
                rowTrans.Controller = "Schools/ProfileYears";
                rowTrans.NewObject = JsonConvert.SerializeObject(new
                {
                    row.Id,
                    row.Name,
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





        ////ChartInfo
        //[HttpGet("Chart/GetAll")]
        //public IActionResult GetAllChartInfo()
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


        //        int SubscriptionsRequestsCount = db.SchoolsSubscriptionsRequests.Where(x => x.Status != 9).Count();
        //        int WaitingSubscriptionsRequests = db.SchoolsSubscriptionsRequests.Where(x => x.Status == 1).Count();
        //        int AcceptSubscriptionsRequests = db.SchoolsSubscriptionsRequests.Where(x => x.Status == 2).Count();
        //        int RejectSubscriptionsRequests = db.SchoolsSubscriptionsRequests.Where(x => x.Status == 3).Count();
        //        double AvgSubscriptionsRequests = SubscriptionsRequestsCount > 0 ? (double)(AcceptSubscriptionsRequests + RejectSubscriptionsRequests) / SubscriptionsRequestsCount : 0;





        //        int StudentCount = db.Schools.Include(x => x.User).Where(x => x.User.Status != 9).Count();
        //        DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
        //        DateTime endOfWeek = startOfWeek.AddDays(7);

        //        // Count Student added this week
        //        int StudentCountThisWeek = db.Schools.Include(x => x.User).Where(x => x.User.Status != 9 && x.User.CreatedOn >= startOfWeek && x.User.CreatedOn < endOfWeek).Count();

        //        //Student Chart This Week
        //        int[] DailyEnrolledCounts = new int[8];
        //        for (int i = 1; i < 8; i++)
        //        {
        //            DateTime day = startOfWeek.AddDays(i); // Get each day of the week
        //            int count = db.Schools.Include(x => x.User)
        //                .Where(x => x.User.Status != 9 && x.User.CreatedOn.Value.Date == day.Date)
        //                .Count();

        //            DailyEnrolledCounts[i] = count; // Store the count in the dictionary
        //        }


        //        int Wallet = db.SchoolsWallet.Where(x => x.Status != 9).Sum(x => x.Value).Value;
        //        int Pay = db.SchoolsSubscriptions.Where(x => x.Status != 9).Sum(x => x.Value).Value;
        //        int Free = db.SchoolsSubscriptions.Where(x => x.Status != 9 && x.Value == 0).Count();

        //        //Top Student Watching 
        //        var TopFiveSchools = db.SchoolsLectures
        //            .Include(x => x.StudentCourse) // Include StudentCourse details
        //            .Include(x => x.StudentCourse.Student) // Include Student details (assuming there's a Student navigation property)
        //            .Include(x => x.StudentCourse.Student.User) // Include Student details (assuming there's a Student navigation property)
        //            .Where(x => x.Status != 9) // Filter by status and CourseId
        //            .GroupBy(x => new
        //            {
        //                x.StudentCourseId,
        //                x.StudentCourse.Student.Name
        //                ,
        //                x.StudentCourse.Student.User.Phone,
        //                x.StudentCourse.Student.User.Image
        //            }) // Group by StudentCourseId and Student
        //            .Select(g => new
        //            {
        //                g.Key.Name, // Assuming Student has a Name property
        //                g.Key.Phone, // Assuming Student has a Name property
        //                g.Key.Image, // Assuming Student has a Name property
        //                Count = g.Count() // Count of lectures watched
        //            })
        //            .OrderByDescending(x => x.Count) // Order by watch count descending
        //            .Take(5) // Get top 5
        //            .ToList();


        //        //Top Student Exam 
        //        var TopFiveSchoolsExam = db.SchoolsExams
        //            .Include(x => x.StudentCourse) // Include StudentCourse details
        //            .Include(x => x.StudentCourse.Student) // Include Student details (assuming there's a Student navigation property)
        //            .Include(x => x.StudentCourse.Student.User) // Include Student details (assuming there's a Student navigation property)
        //            .Where(x => x.Status != 9) // Filter by status and CourseId
        //            .GroupBy(x => new
        //            {
        //                x.StudentCourseId,
        //                x.StudentCourse.Student.Name
        //                ,
        //                x.StudentCourse.Student.User.Phone,
        //                x.StudentCourse.Student.User.Image
        //            }) // Group by StudentCourseId and Student
        //            .Select(g => new
        //            {
        //                g.Key.Name, // Assuming Student has a Name property
        //                g.Key.Phone, // Assuming Student has a Name property
        //                g.Key.Image, // Assuming Student has a Name property
        //                Count = g.Count() // Count of lectures watched
        //            })
        //            .OrderByDescending(x => x.Count) // Order by watch count descending
        //            .Take(5) // Get top 5
        //            .ToList();




        //        // Top Schools by Total Payment
        //        var TopSchoolsByPayment = db.SchoolsSubscriptions
        //            .Include(x => x.Student) // Include the Student details
        //            .Include(x => x.Student.User) // Include the Student details
        //            .Where(x => x.Status != 9) // Filter by status
        //            .GroupBy(x => new
        //            {
        //                x.StudentId,
        //                x.Student.Name, // Assuming Student has a Name property
        //                x.Student.User.Image, // Assuming Student has a Name property
        //                x.Student.User.Phone, // Assuming Student has a Name property
        //            }) // Group by Student Id and Name
        //            .Select(g => new
        //            {
        //                g.Key.StudentId,
        //                g.Key.Name,
        //                g.Key.Phone,
        //                g.Key.Image,
        //                Cash = g.Sum(x => x.Value) // Sum of payments
        //            })
        //            .OrderByDescending(x => x.Cash) // Order by total payment descending
        //            .Take(5) // Get top 5
        //            .ToList();


        //        var Info = new
        //        {
        //            DeviceRequestCount,
        //            WaitingDeviceReques,
        //            AcceptDeviceReques,
        //            RejectDeviceReques,
        //            AvgDeviceReques,

        //            SubscriptionsRequestsCount,
        //            WaitingSubscriptionsRequests,
        //            AcceptSubscriptionsRequests,
        //            RejectSubscriptionsRequests,
        //            AvgSubscriptionsRequests,


        //            StudentCount,
        //            StudentCountThisWeek,
        //            DailyEnrolledCounts,
        //            Wallet,
        //            Pay,
        //            Free,


        //            TopFiveSchools,
        //            TopFiveSchoolsExam,
        //            TopSchoolsByPayment,

        //        };


        //        return Ok(new { info = Info });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}








        //[HttpGet("Get")]
        //public IActionResult Get(int pageNo, int pageSize, long id)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (id <= 0)
        //        {

        //            int Count = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4).Count();
        //            var Info = db.Schools
        //                .Include(k => k.User)
        //                .Include(k => k.SchoolsProfiles)
        //                .Include(k => k.StudentWallet)
        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.User.Status != 9).Select(x => new
        //                {
        //                    UserInfo = new
        //                    {
        //                        x.UserId,
        //                        x.User.LoginName,
        //                        x.User.Email,
        //                        x.User.Phone,
        //                        x.User.Gender,
        //                        x.User.ImagePath,
        //                        x.User.CreatedOn,
        //                        x.User.CreatedBy,
        //                        x.User.Status,
        //                        x.User.Levels,
        //                        x.User.LastLoginOn,
        //                        x.User.BirthDate,
        //                        x.User.ExtraPhone,
        //                    },
        //                    StudentProfile = db.SchoolsProfiles
        //                .Include(k => k.AcademicSpecialization)
        //                .Include(k => k.AcademicSpecialization.AcademicLevel)
        //                .Where(k => k.Status == 1 && k.Id == x.SchoolsProfiles.Where(k => k.Status == 1).FirstOrDefault().Id).Select(k => new
        //                {

        //                    AcademicSpecialization = k.AcademicSpecialization.Name,
        //                    AcademicLevel = k.AcademicSpecialization.AcademicLevel.Name,
        //                    CourseCount = k.StudentSubscriptions.Where(k => k.Status == 1).Count(),
        //                    SubscriptionsumCash = k.StudentSubscriptions.Where(k => k.Status == 1).Sum(k => k.Value),


        //                }).FirstOrDefault(),
        //                    WalletValue = x.StudentWallet.Where(k => k.Status != 9).FirstOrDefault().Value,
        //                    x.Id,
        //                    x.FullName,
        //                    x.FirstName,
        //                    x.FatherName,
        //                    x.GrandFatherName,
        //                    x.SirName,
        //                    x.AcountPoints,
        //                    x.AcountRate,
        //                    x.CreatedOn,
        //                    x.Status,
        //                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //            return Ok(new { info = Info, count = Count });
        //        }
        //        else
        //        {

        //            int Count = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == id).Count();
        //            var Info = db.Schools
        //                .Include(k => k.User)
        //                .Include(k => k.SchoolsProfiles)
        //                .Include(k => k.StudentWallet)
        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == id).Select(x => new
        //                {
        //                    UserInfo = new
        //                    {
        //                        x.UserId,
        //                        x.User.LoginName,
        //                        x.User.Email,
        //                        x.User.Phone,
        //                        x.User.Gender,
        //                        x.User.ImagePath,
        //                        x.User.CreatedOn,
        //                        x.User.CreatedBy,
        //                        x.User.Status,
        //                        x.User.Levels,
        //                        x.User.LastLoginOn,
        //                        x.User.BirthDate,
        //                        x.User.ExtraPhone,
        //                    },
        //                    StudentProfile = db.SchoolsProfiles
        //                .Include(k => k.AcademicSpecialization)
        //                .Include(k => k.AcademicSpecialization.AcademicLevel)
        //                .Where(k => k.Status == 1 && k.Id == x.SchoolsProfiles.Where(k => k.Status == 1).FirstOrDefault().Id).Select(k => new
        //                {

        //                    AcademicSpecialization = k.AcademicSpecialization.Name,
        //                    AcademicLevel = k.AcademicSpecialization.AcademicLevel.Name,
        //                    CourseCount = k.StudentSubscriptions.Where(k => k.Status == 1).Count(),
        //                    SubscriptionsumCash = k.StudentSubscriptions.Where(k => k.Status == 1).Sum(k => k.Value),


        //                }).FirstOrDefault(),
        //                    WalletValue = x.StudentWallet.Where(k => k.Status != 9).FirstOrDefault().Value,
        //                    x.Id,
        //                    x.FullName,
        //                    x.FirstName,
        //                    x.FatherName,
        //                    x.GrandFatherName,
        //                    x.SirName,
        //                    x.AcountPoints,
        //                    x.AcountRate,
        //                    x.CreatedOn,
        //                    x.Status,
        //                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //            return Ok(new { info = Info, count = Count });
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetStudentPhone")]
        //public IActionResult GetStudentPhone(string code)
        //{
        //    try
        //    {

        //        var userId = this.help.GetCurrentUser(HttpContext);

        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        if (code.StartsWith("0"))
        //            code = code.Substring(1);


        //        var Info = db.Schools.Include(x => x.User).Where(x => x.Status != 9 && x.User.Phone.Contains(code)).Select(x => new
        //        {
        //            x.Id,
        //            Name = x.FullName,
        //            Phone = x.User.Phone,
        //        }).ToList();
        //        return Ok(new { info = Info });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetStudentByStudentName")]
        //public IActionResult GetStudentByStudentName(string code)
        //{
        //    try
        //    {

        //        var userId = this.help.GetCurrentUser(HttpContext);

        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var Info = db.Schools.Include(x => x.User).Where(x => x.Status != 9 && x.FullName.Contains(code)).Select(x => new
        //        {
        //            x.Id,
        //            Name = x.FullName,
        //            x.User.Phone,
        //        }).ToList();
        //        return Ok(new { info = Info });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetByName")]
        //public IActionResult GetByName(int pageNo, int pageSize, int Id)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
        //        if (Id > 0)
        //        {
        //            int Count = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == Id).Count();
        //            var Info = db.Schools
        //                .Include(k => k.User)
        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.User.Levels == 4 && x.Id == Id).Select(x => new
        //                {
        //                    UserInfo = new
        //                    {
        //                        x.UserId,
        //                        x.User.LoginName,
        //                        x.User.Email,
        //                        x.User.Phone,
        //                        x.User.Gender,
        //                        x.User.ImagePath,
        //                        x.User.CreatedOn,
        //                        x.User.CreatedBy,
        //                        x.User.Status,
        //                        x.User.Levels,
        //                        x.User.LastLoginOn,
        //                        x.User.BirthDate,
        //                        x.User.ExtraPhone,
        //                    },
        //                    x.Id,
        //                    x.FullName,
        //                    x.FirstName,
        //                    x.FatherName,
        //                    x.GrandFatherName,
        //                    x.SirName,
        //                    x.AcountPoints,
        //                    x.AcountRate,
        //                    x.CreatedOn,
        //                    x.Status,
        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //            return Ok(new { info = Info, count = Count });
        //        }
        //        else
        //        {
        //            int Count = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4).Count();
        //            var Info = db.Schools
        //                .Include(k => k.User)
        //                .Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
        //                {
        //                    UserInfo = new
        //                    {
        //                        x.UserId,
        //                        x.User.LoginName,
        //                        x.User.Email,
        //                        x.User.Phone,
        //                        x.User.Gender,
        //                        x.User.ImagePath,
        //                        x.User.CreatedOn,
        //                        x.User.CreatedBy,
        //                        x.User.Status,
        //                        x.User.Levels,
        //                        x.User.LastLoginOn,
        //                        x.User.BirthDate,
        //                        x.User.ExtraPhone,
        //                    },
        //                    x.Id,
        //                    x.FullName,
        //                    x.FirstName,
        //                    x.FatherName,
        //                    x.GrandFatherName,
        //                    x.SirName,
        //                    x.AcountPoints,
        //                    x.AcountRate,
        //                    x.CreatedOn,
        //                    x.Status,
        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //            return Ok(new { info = Info, count = Count });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetByPhone")]
        //public IActionResult GetByPhone(int pageNo, int pageSize, int Id)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
        //        if (Id > 0)
        //        {
        //            int Count = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4 && x.Id == Id).Count();
        //            var Info = db.Schools
        //                .Include(k => k.User)
        //                .Where(x => x.Status != 9 && x.User.Levels == 4 && x.User.Levels == 4 && x.Id == Id).Select(x => new
        //                {
        //                    UserInfo = new
        //                    {
        //                        x.UserId,
        //                        x.User.LoginName,
        //                        x.User.Email,
        //                        x.User.Phone,
        //                        x.User.Gender,
        //                        x.User.ImagePath,
        //                        x.User.CreatedOn,
        //                        x.User.CreatedBy,
        //                        x.User.Status,
        //                        x.User.Levels,
        //                        x.User.LastLoginOn,
        //                        x.User.BirthDate,
        //                        x.User.ExtraPhone,
        //                    },
        //                    x.Id,
        //                    x.FullName,
        //                    x.FirstName,
        //                    x.FatherName,
        //                    x.GrandFatherName,
        //                    x.SirName,
        //                    x.AcountPoints,
        //                    x.AcountRate,
        //                    x.CreatedOn,
        //                    x.Status,
        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //            return Ok(new { info = Info, count = Count });
        //        }
        //        else
        //        {
        //            int Count = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4).Count();
        //            var Info = db.Schools
        //                .Include(k => k.User)
        //                .Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
        //                {
        //                    UserInfo = new
        //                    {
        //                        x.UserId,
        //                        x.User.LoginName,
        //                        x.User.Email,
        //                        x.User.Phone,
        //                        x.User.Gender,
        //                        x.User.ImagePath,
        //                        x.User.CreatedOn,
        //                        x.User.CreatedBy,
        //                        x.User.Status,
        //                        x.User.Levels,
        //                        x.User.LastLoginOn,
        //                        x.User.BirthDate,
        //                        x.User.ExtraPhone,
        //                    },
        //                    x.Id,
        //                    x.FullName,
        //                    x.FirstName,
        //                    x.FatherName,
        //                    x.GrandFatherName,
        //                    x.SirName,
        //                    x.AcountPoints,
        //                    x.AcountRate,
        //                    x.CreatedOn,
        //                    x.Status,
        //                }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //            return Ok(new { info = Info, count = Count });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetNames")]
        //public IActionResult GetNames()
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var Info = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
        //        {
        //            x.Id,
        //            Name = x.FullName,
        //        }).OrderByDescending(x => x.Name).ToList();

        //        return Ok(new { info = Info });

        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetPhoneSchools")]
        //public IActionResult GetPhoneSchools()
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);
        //        var Info = db.Schools.Where(x => x.Status != 9 && x.User.Levels == 4).Select(x => new
        //        {

        //            x.Id,
        //            x.User.Phone,
        //        }).OrderByDescending(x => x.Phone).ToList();
        //        return Ok(new { info = Info });

        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //public class BodyObject
        //{
        //    public long? Id { get; set; }

        //    public string FirstName { get; set; }
        //    public string FatherName { get; set; }
        //    public string GrandFatherName { get; set; }
        //    public string SirName { get; set; }

        //    public string LoginName { get; set; }
        //    public DateTime? BirthDate { get; set; }
        //    public short? Gender { get; set; }

        //    public string Phone { get; set; }
        //    public string ExtraPhone { get; set; }
        //    public string Email { get; set; }
        //    public long? LocationId { get; set; }

        //    public int Value { get; set; }

        //    public long AcademicSpecializationId { get; set; }

        //    public string ImageName { get; set; }
        //    public string ImageType { get; set; }
        //    public string FileBase64 { get; set; }
        //}

        //[HttpPost("Add")]
        //[DisableRequestSizeLimit]
        //public IActionResult Add([FromBody] BodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        //validations
        //        if (string.IsNullOrEmpty(bodyObject.LoginName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

        //        if (!string.IsNullOrEmpty(bodyObject.Email))
        //        {
        //            if (!help.IsValidEmail(bodyObject.Email))
        //                return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);
        //        }

        //        if (string.IsNullOrEmpty(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

        //        if (!help.IsValidPhone(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

        //        if (string.IsNullOrEmpty(bodyObject.FirstName) ||
        //            string.IsNullOrEmpty(bodyObject.FatherName) ||
        //            string.IsNullOrEmpty(bodyObject.GrandFatherName) ||
        //            string.IsNullOrEmpty(bodyObject.SirName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


        //        //isExist
        //        var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        if (!string.IsNullOrEmpty(bodyObject.Email))
        //        {
        //            IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9).SingleOrDefault();
        //            if (IsExist != null)
        //                return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
        //        }

        //        IsExist = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

        //        string FullName = bodyObject.FirstName + " " + bodyObject.FatherName + " " + bodyObject.GrandFatherName + " " + bodyObject.SirName;

        //        var IsExistStudent = db.Schools.Where(x => x.FirstName == FullName).SingleOrDefault();
        //        if (IsExistStudent != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);


        //        Users user = new Users();
        //        user.Name = bodyObject.LoginName;
        //        user.LoginName = bodyObject.LoginName;
        //        user.Phone = bodyObject.Phone;
        //        user.LocationId = bodyObject.LocationId;
        //        user.ExtraPhone = bodyObject.ExtraPhone;
        //        user.Email = bodyObject.Email;
        //        user.Password = Security.ComputeHash("12345", HashAlgorithms.SHA512, null);
        //        //user.Password = Security.ComputeHash(help.GenreatePass(bodyObject.BirthDate.GetValueOrDefault().Year.ToString()), HashAlgorithms.SHA512, null);
        //        user.UserType = 60;
        //        if (!string.IsNullOrEmpty(bodyObject.ImageName))
        //        {
        //            user.ImageName = bodyObject.ImageName;
        //            user.ImagePath = this.help.UploadFile(bodyObject.ImageName, bodyObject.ImageType, bodyObject.FileBase64);
        //        }
        //        else
        //            user.ImagePath = this.help.UploadFile(bodyObject.LoginName, ".jpg", help.GetDefaultImage());
        //        user.BirthDate = bodyObject.BirthDate;
        //        user.Gender = bodyObject.Gender;
        //        user.CreatedBy = userId;
        //        user.CreatedOn = DateTime.Now;
        //        user.Levels = 4;
        //        user.Status = 1;

        //        //Student Wallet
        //        List<StudentWallet> studentWallets = new List<StudentWallet>();
        //        StudentWallet studentWallet = new StudentWallet();
        //        studentWallet.Value = bodyObject.Value;
        //        studentWallet.CreatedOn = DateTime.Now;
        //        studentWallet.CreatedBy = userId;
        //        studentWallet.Status = 1;
        //        studentWallets.Add(studentWallet);

        //        List<SchoolsProfiles> SchoolsProfiles = new List<SchoolsProfiles>();
        //        SchoolsProfiles SchoolsProfile = new SchoolsProfiles();
        //        SchoolsProfile.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
        //        SchoolsProfile.CourseCount = 0;
        //        SchoolsProfile.Points = 0;
        //        SchoolsProfile.CreatedOn = DateTime.Now;
        //        SchoolsProfile.CreatedBy = userId;
        //        SchoolsProfile.Status = 1;
        //        SchoolsProfiles.Add(SchoolsProfile);

        //        Schools row = new Schools();
        //        row.FirstName = bodyObject.FirstName;
        //        row.FatherName = bodyObject.FatherName;
        //        row.GrandFatherName = bodyObject.GrandFatherName;
        //        row.SirName = bodyObject.SirName;
        //        row.FullName = FullName;
        //        row.AcountRate = 2;
        //        row.AcountPoints = 0;
        //        row.CreatedBy = userId;
        //        row.CreatedOn = DateTime.Now;
        //        row.ModifiedBy = userId;
        //        row.ModifiedOn = DateTime.Now;
        //        row.Status = 1;
        //        row.User = user;
        //        row.StudentWallet = studentWallets;
        //        row.SchoolsProfiles = SchoolsProfiles;
        //        db.Schools.Add(row);
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("Edit")]
        //public IActionResult Edit([FromBody] BodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (bodyObject.Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Info = db.Schools.Include(x => x.User).Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        //validations
        //        if (string.IsNullOrEmpty(bodyObject.LoginName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameEmpty);

        //        if (string.IsNullOrEmpty(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

        //        if (!help.IsValidEmail(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

        //        if (string.IsNullOrEmpty(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

        //        if (!help.IsValidPhone(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

        //        if (string.IsNullOrEmpty(bodyObject.FirstName) ||
        //            string.IsNullOrEmpty(bodyObject.FatherName) ||
        //            string.IsNullOrEmpty(bodyObject.GrandFatherName) ||
        //            string.IsNullOrEmpty(bodyObject.SirName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);


        //        //isExist
        //        var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 && x.Id != Info.UserId).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        if (!string.IsNullOrEmpty(bodyObject.Email))
        //        {
        //            IsExist = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9 && x.Id != Info.UserId).SingleOrDefault();
        //            if (IsExist != null)
        //                return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);
        //        }

        //        IsExist = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9 && x.Id != Info.UserId).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

        //        string FullName = bodyObject.FirstName + " " + bodyObject.FatherName + " " + bodyObject.GrandFatherName + " " + bodyObject.SirName;

        //        var IsExistStudent = db.Schools.Where(x => x.FullName == FullName && x.Id != bodyObject.Id).SingleOrDefault();
        //        if (IsExistStudent != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);


        //        Info.User.LoginName = bodyObject.LoginName;
        //        Info.User.Email = bodyObject.Email;
        //        Info.User.BirthDate = bodyObject.BirthDate;
        //        Info.User.Gender = bodyObject.Gender;
        //        Info.User.ModifiedBy = userId;
        //        Info.User.ModifiedOn = DateTime.Now;
        //        Info.FirstName = bodyObject.FirstName;
        //        Info.FatherName = bodyObject.FatherName;
        //        Info.GrandFatherName = bodyObject.GrandFatherName;
        //        Info.SirName = bodyObject.SirName;
        //        Info.FullName = FullName;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/Delete")]
        //public IActionResult Delete(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.Schools.Include(x => x.User).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        Info.Status = 9;
        //        Info.User.Status = 9;
        //        Info.User.ModifiedBy = userId;
        //        Info.User.ModifiedOn = DateTime.Now;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessDeleteOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/ResetPassword")]
        //public IActionResult ResetPassword(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.Schools.Include(x => x.User).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var User = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
        //        if (User == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        string Password = this.help.GenreatePass();
        //        User.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);
        //        User.ModifiedBy = userId;
        //        User.ModifiedOn = DateTime.Now;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations + " " + "كلمة المرور الجديدة : " + Password);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/ChangeStatus")]
        //public IActionResult ChangeStatus(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.Schools.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (Info.Status == 1)
        //        {
        //            Info.Status = 2;
        //        }
        //        else if (Info.Status == 2)
        //        {
        //            Info.Status = 1;
        //        }

        //        db.SaveChanges();
        //        return Ok(BackMessages.SuccessChangeStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/ChangeStatusAcount")]
        //public IActionResult ChangeStatusAcount(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.Schools.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (Info.User.Status == 1)
        //        {
        //            Info.User.Status = 2;
        //        }
        //        else if (Info.User.Status == 2)
        //        {
        //            Info.User.Status = 1;
        //        }

        //        db.SaveChanges();
        //        return Ok(BackMessages.SuccessChangeStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/ActiveAcount")]
        //public IActionResult ActiveAcount(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Student = db.Schools.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Student == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Info = db.Users.Where(x => x.Status != 9 && x.Id == Student.UserId).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        Info.Status = 1;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SuccessChangeStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}





        ////Student Profile
        //public class SchoolsProfileBodyObject
        //{
        //    public long? Id { get; set; }
        //    public long StudentId { get; set; }
        //    public long AcademicSpecializationId { get; set; }
        //}

        ////Student Profile 
        //[HttpPost("AddSchoolsProfile")]
        //public IActionResult AddSchoolsProfile([FromBody] SchoolsProfileBodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var IsExist = db.SchoolsProfiles.Where(x => x.StudentId == bodyObject.StudentId && x.AcademicSpecializationId == bodyObject.AcademicSpecializationId
        //        && x.Status != 9).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecordExist);

        //        SchoolsProfiles row = new SchoolsProfiles();
        //        row.StudentId = bodyObject.StudentId;
        //        row.AcademicSpecializationId = bodyObject.AcademicSpecializationId;
        //        row.CourseCount = 0;
        //        row.Points = 0;
        //        row.CreatedOn = DateTime.Now;
        //        row.CreatedBy = userId;
        //        db.SchoolsProfiles.Add(row);
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}







        //public class StudentSubscriptionsBodyObject
        //{
        //    public long? Id { get; set; }
        //    public long StudentId { get; set; }
        //    public long CourseId { get; set; }
        //    public int Value { get; set; }
        //}

        ////Student Course 
        //[HttpPost("AddStudentCourse")]
        //public IActionResult AddStudentCourse([FromBody] StudentSubscriptionsBodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var StudentProfile = db.SchoolsProfiles.Where(x => x.StudentId == bodyObject.StudentId && x.Status == 1).SingleOrDefault();
        //        if (StudentProfile == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Course = db.Subscriptions.Where(x => x.Id == bodyObject.CourseId).SingleOrDefault();
        //        if (Course == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var IsExist = db.StudentSubscriptions.Where(x => x.StudentProfileId == StudentProfile.Id && x.CourseId == bodyObject.CourseId
        //        && x.Status != 9).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecordExist);


        //        /////////////Repleace with method for get mony 
        //        ///
        //        ///
        //        ///
        //        ///
        //        /// 
        //        var walit = db.StudentWallet.Where(x => x.StudentId == bodyObject.StudentId).SingleOrDefault();

        //        if (Course.Price > walit.Value)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

        //        walit.Value -= bodyObject.Value;

        //        List<StudentWalletPurchases> StudentWalletPurchasesList = new List<StudentWalletPurchases>();
        //        StudentWalletPurchases StudentWalletPurchasesRow = new StudentWalletPurchases();
        //        StudentWalletPurchasesRow.WalletId = walit.Id;
        //        StudentWalletPurchasesRow.Value = bodyObject.Value;
        //        StudentWalletPurchasesRow.CoursePrice = Course.Price;
        //        StudentWalletPurchasesRow.CreatedOn = DateTime.Now;
        //        StudentWalletPurchasesRow.CreatedBy = userId;
        //        StudentWalletPurchasesRow.Status = 1;
        //        StudentWalletPurchasesList.Add(StudentWalletPurchasesRow);
        //        /// 
        //        ///
        //        ///
        //        ///
        //        ///



        //        StudentSubscriptions row = new StudentSubscriptions();
        //        row.StudentWalletPurchases = StudentWalletPurchasesList;
        //        row.StudentProfileId = StudentProfile.Id;
        //        row.CourseId = bodyObject.CourseId;
        //        row.Value = bodyObject.Value;
        //        row.CreatedOn = DateTime.Now;
        //        row.CreatedBy = userId;
        //        db.StudentSubscriptions.Add(row);
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetStudentCourse")]
        //public IActionResult GetStudentCourse(long id)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Student = db.Schools.Where(x => x.Status != 9 && x.Id == id).SingleOrDefault();
        //        if (Student == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var StudentProfile = db.SchoolsProfiles.Where(x => x.StudentId == id && x.Status == 1).SingleOrDefault();
        //        if (StudentProfile == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Info = db.StudentSubscriptions
        //            .Include(x => x.Course)
        //            .Include(x => x.Course.Subject)
        //            .Include(x => x.Course.AcademicSpecialization)
        //            .Include(x => x.StudentWalletPurchases)
        //            .Where(x => x.StudentProfileId == StudentProfile.Id && x.Status != 9)
        //            .Select(x => new
        //            {

        //                x.Id,
        //                x.CourseId,
        //                Course = new
        //                {
        //                    x.Course.Name,
        //                    x.Course.TeacherName,
        //                    Subject = x.Course.Subject.Name,
        //                    Specialization = x.Course.AcademicSpecialization.Name,
        //                    x.Course.Price,
        //                    x.Course.TaregerLevel,
        //                },
        //                x.Value,
        //                x.CreatedOn,
        //                x.CreatedBy
        //            }).OrderByDescending(x => x.CreatedOn).ToList();

        //        return Ok(new { info = Info });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //public class PayStudentCourseBodyObject
        //{
        //    public long? Id { get; set; }
        //    public int Value { get; set; }
        //}

        ////Student Course 
        //[HttpPost("PayStudentCourse")]
        //public IActionResult PayStudentCourse([FromBody] PayStudentCourseBodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var StudentCourse = db.StudentSubscriptions.Include(x => x.Course).Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
        //        if (StudentCourse == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (StudentCourse.Course.Price <= StudentCourse.Value)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PayBefore);

        //        int remind = StudentCourse.Course.Price.GetValueOrDefault() - StudentCourse.Value.GetValueOrDefault();
        //        if (remind <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PayBefore);

        //        if (bodyObject.Value > remind)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);

        //        StudentCourse.Value += bodyObject.Value;

        //        if (StudentCourse.Value > StudentCourse.Course.Price)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.RecheckTheValue);



        //        var StudentProfile = db.SchoolsProfiles.Where(x => x.Id == StudentCourse.StudentProfileId && x.Status == 1).SingleOrDefault();
        //        if (StudentProfile == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Course = db.Subscriptions.Where(x => x.Id == StudentCourse.CourseId).SingleOrDefault();
        //        if (Course == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


        //        /////////////Repleace with method for get mony 
        //        ///
        //        ///
        //        ///
        //        ///
        //        /// 
        //        var walit = db.StudentWallet.Where(x => x.StudentId == StudentProfile.StudentId).SingleOrDefault();

        //        if (remind > walit.Value)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.WalletNotEnife);

        //        walit.Value -= bodyObject.Value;

        //        StudentWalletPurchases StudentWalletPurchasesRow = new StudentWalletPurchases();
        //        StudentWalletPurchasesRow.StudentCourseId = StudentCourse.Id;
        //        StudentWalletPurchasesRow.WalletId = walit.Id;
        //        StudentWalletPurchasesRow.Value = bodyObject.Value;
        //        StudentWalletPurchasesRow.CoursePrice = Course.Price;
        //        StudentWalletPurchasesRow.CreatedOn = DateTime.Now;
        //        StudentWalletPurchasesRow.CreatedBy = userId;
        //        StudentWalletPurchasesRow.Status = 1;
        //        db.StudentWalletPurchases.Add(StudentWalletPurchasesRow);


        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Pay;
        //        rowTrans.ItemId = StudentWalletPurchasesRow.Id;
        //        rowTrans.Descriptions = " الاشتراك في كورس ودفع القيمة   ";
        //        rowTrans.Controller = "Student/StudentWalletPurchasesRow";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(StudentWalletPurchasesRow, Formatting.None,
        //                new JsonSerializerSettings()
        //                {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                });
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/OutFromStudentCourse")]
        //public IActionResult OutFromStudentCourse(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var StudentCourse = db.StudentSubscriptions.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (StudentCourse == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var StudentProfile = db.SchoolsProfiles.Where(x => x.Id == StudentCourse.StudentProfileId).SingleOrDefault();
        //        if (StudentProfile == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Student = db.Schools
        //            .Where(x => x.Id == StudentProfile.StudentId && x.Status != 9).SingleOrDefault();
        //        if (Student == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Wallet = db.StudentWallet.Where(x => x.StudentId == Student.Id && x.Status != 9).SingleOrDefault();
        //        if (Wallet == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        Wallet.Value += StudentCourse.Value;
        //        StudentCourse.Status = 9;
        //        StudentCourse.CreatedBy = userId;
        //        StudentCourse.CreatedOn = DateTime.Now;


        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = StudentCourse.Id;
        //        rowTrans.Descriptions = "  حذف طالب من الدورة وارجاع القيمة المالية الخاصة به   ";
        //        rowTrans.Controller = "Student/StudentCourse";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(StudentCourse, Formatting.None,
        //                new JsonSerializerSettings()
        //                {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                });
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessOut);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}







        ////Student Wallet
        //[HttpGet("GetWalletInfo")]
        //public IActionResult GetWalletInfo(long id)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Student = db.Schools.Where(x => x.Status != 9 && x.Id == id).SingleOrDefault();
        //        if (Student == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Info = db.StudentWallet
        //            .Include(x => x.StudentWalletTransactions)
        //            .Include(x => x.StudentWalletPurchases)
        //            .Where(x => x.StudentId == id && x.Status != 9)
        //            .Select(x => new
        //            {

        //                x.Id,
        //                x.Value,
        //                x.CreatedOn,
        //                Transactions = x.StudentWalletTransactions.Where(k => k.Status != 9).OrderByDescending(k => k.CreatedOn)
        //                    .Select(k => new
        //                    {
        //                        k.Id,
        //                        k.Value,
        //                        k.ProcessType,
        //                        k.PaymentMethodId,
        //                        k.CreatedOn,
        //                        CreatedBy = db.Users.Where(z => z.Id == k.CreatedBy).SingleOrDefault().Name,
        //                    }).ToList(),
        //                Purchases = x.StudentWalletPurchases.Where(k => k.Status != 9).OrderByDescending(k => k.CreatedOn).ToList(),
        //            }).SingleOrDefault();

        //        return Ok(new { info = Info });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetWalletPurchasesInfo")]
        //public IActionResult GetWalletPurchasesInfo(long id)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Student = db.Schools.Where(x => x.Status != 9 && x.Id == id).SingleOrDefault();
        //        if (Student == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Wallet = db.StudentWallet.Where(x => x.StudentId == id && x.Status != 9).SingleOrDefault();
        //        if (Wallet == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Info = db.StudentWalletPurchases
        //            .Include(x => x.StudentCourse)
        //            .Include(x => x.StudentCourse.Course)
        //            .Where(x => x.WalletId == Wallet.Id && x.Status != 9).Select(x => new
        //            {
        //                x.Id,
        //                x.Value,
        //                x.CoursePrice,
        //                Course = new
        //                {
        //                    x.StudentCourse.Course.Name,
        //                    AcademicSpecialization = x.StudentCourse.Course.AcademicSpecialization.Name,
        //                    Subject = x.StudentCourse.Course.Subject.Name,
        //                },
        //                x.CreatedOn,
        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,

        //            }).OrderByDescending(x => x.CreatedOn).ToList();
        //        return Ok(new { info = Info });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //public class StudentWalletBodyObject
        //{
        //    public long? Id { get; set; }
        //    public long StudentId { get; set; }
        //    public short PaymentMethodId { get; set; }
        //    public short ProcessType { get; set; }
        //    public int Value { get; set; }
        //}

        ////Student Profile 
        //[HttpPost("RechargeWallet")]
        //public IActionResult RechargeWallet([FromBody] StudentWalletBodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (bodyObject.Value <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.ValueEmpty);

        //        if (bodyObject.PaymentMethodId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PaymentMethodEmpty);

        //        var Wallet = db.StudentWallet.Where(x => x.StudentId == bodyObject.StudentId && x.Status != 9).SingleOrDefault();
        //        if (Wallet == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        StudentWalletTransactions row = new StudentWalletTransactions();
        //        row.WalletId = Wallet.Id;
        //        row.PaymentMethodId = bodyObject.PaymentMethodId;
        //        row.Value = bodyObject.Value;
        //        row.ProcessType = bodyObject.ProcessType;
        //        row.CreatedOn = DateTime.Now;
        //        row.CreatedBy = userId;
        //        row.Status = 1;
        //        db.StudentWalletTransactions.Add(row);
        //        Wallet.Value += bodyObject.Value;


        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Pay;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "  شحن محفظة طالب  ";
        //        rowTrans.Controller = "Student/RechargeWallet";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
        //                new JsonSerializerSettings()
        //                {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                });
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/DeletetWalletTransacitons")]
        //public IActionResult DeletetWalletTransacitons(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.StudentWalletTransactions.Include(x => x.Wallet).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Wallet = db.StudentWallet.Where(x => x.Id == Info.WalletId && x.Status != 9).SingleOrDefault();
        //        if (Wallet == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        Info.Status = 9;
        //        Info.CreatedBy = userId;
        //        Info.CreatedOn = DateTime.Now;
        //        Wallet.Value -= Info.Value;

        //        StudentWalletTransactions row = new StudentWalletTransactions();
        //        row.WalletId = Wallet.Id;
        //        row.PaymentMethodId = 1;
        //        row.Value = Info.Value;
        //        row.ProcessType = 2;
        //        row.CreatedOn = DateTime.Now;
        //        row.CreatedBy = userId;
        //        row.Status = 1;
        //        db.StudentWalletTransactions.Add(row);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "  حذف عملية من حركات الحافظة  ";
        //        rowTrans.Controller = "Student/StudentWalletTransactions";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
        //                new JsonSerializerSettings()
        //                {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                });
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessDeleteOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


        ////Student Subscribtions
        //[HttpGet("Subscriptions/Get")]
        //public IActionResult GetSchoolsubscribtions(int pageNo, int pageSize, long AcademicLevelId, long AcademicSpecializationId, long SubjectId,
        //    long CourseId, long studentId, short SubscriptionType)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        int Count = db.StudentSubscriptions
        //            .Include(x => x.Course)
        //            .Include(x => x.Course.AcademicSpecialization)
        //            .Include(x => x.StudentProfile)
        //            .Where(x => x.Status != 9
        //        && (AcademicLevelId > 0 ? x.Course.AcademicSpecialization.AcademicLevelId == AcademicLevelId : true)
        //        && (AcademicSpecializationId > 0 ? x.Course.AcademicSpecializationId == AcademicSpecializationId : true)
        //        && (SubjectId > 0 ? x.Course.SubjectId == SubjectId : true)
        //        && (CourseId > 0 ? x.CourseId == CourseId : true)
        //        && (studentId > 0 ? x.StudentProfile.StudentId == studentId : true)
        //        && (SubscriptionType == 1 ? x.Value > 0 : true)
        //        && (SubscriptionType == 2 ? x.Value == 0 : true)

        //        ).Count();
        //        var Info = db.StudentSubscriptions
        //            .Include(k => k.Course)
        //            .Include(k => k.Course.AcademicSpecialization)
        //            .Include(k => k.Course.Subject)
        //            .Include(k => k.StudentProfile)
        //            .Include(k => k.StudentProfile.Student)
        //            .Include(k => k.StudentProfile.Student.User)
        //            .Where(x => x.Status != 9
        //            && (AcademicLevelId > 0 ? x.Course.AcademicSpecialization.AcademicLevelId == AcademicLevelId : true)
        //            && (AcademicSpecializationId > 0 ? x.Course.AcademicSpecializationId == AcademicSpecializationId : true)
        //            && (SubjectId > 0 ? x.Course.SubjectId == SubjectId : true)
        //            && (CourseId > 0 ? x.CourseId == CourseId : true)
        //            && (studentId > 0 ? x.StudentProfile.StudentId == studentId : true)
        //            && (SubscriptionType == 2 ? x.Value == 0 : true)
        //            && (SubscriptionType == 1 ? x.Value > 0 : true)

        //            )
        //            .Select(x => new
        //            {
        //                x.Id,
        //                x.Value,
        //                Course = new
        //                {
        //                    x.CourseId,
        //                    x.Course.Name,
        //                    x.Course.Price,
        //                    AcademicSpecialization = x.Course.AcademicSpecialization.Name,
        //                    Subject = x.Course.Subject.Name,
        //                },
        //                Student = new
        //                {
        //                    x.StudentProfile.StudentId,
        //                    x.StudentProfile.Student.FullName,
        //                    x.StudentProfile.Student.User.Phone,
        //                    x.StudentProfile.Student.User.ImagePath
        //                },
        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
        //                x.CreatedOn,
        //                x.Status,
        //            }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //        return Ok(new { info = Info, count = Count });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}








        //public class SuspendBodyObject
        //{
        //    public long StudentId { get; set; }
        //    public string Resone { get; set; }
        //}

        ////Student Suspend 
        //[HttpPost("Suspend/Add")]
        //public IActionResult SuspendStudent([FromBody] SuspendBodyObject bodyObject)
        //{
        //    try
        //    {
        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (string.IsNullOrEmpty(bodyObject.Resone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.ResoneEmpty);

        //        var Student = db.Schools.Include(x => x.User).Where(x => x.Id == bodyObject.StudentId && x.Status != 9).SingleOrDefault();
        //        if (Student == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (Student.User.Status == 3)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.SuspendBefore);


        //        Student.User.Status = 3;
        //        UserSuspends row = new UserSuspends();
        //        row.UserId = Student.UserId;
        //        row.Resone = bodyObject.Resone;
        //        row.CreatedOn = DateTime.Now;
        //        row.CreatedBy = userId;
        //        row.Status = 1;
        //        db.UserSuspends.Add(row);
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("Suspend/Get")]
        //public IActionResult GetSchoolsSuspend(int pageNo, int pageSize, long studentId)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        int Count = db.Schools
        //            .Include(x => x.User)
        //            .Where(x => x.User.Status == 3 && x.Status != 9
        //            && (studentId > 0 ? x.Id == studentId : true)
        //            ).Count();
        //        var Info = db.Schools
        //            .Include(x => x.User)
        //            .Include(x => x.User.UserSuspends)
        //            .Where(x => x.User.Status == 3 && x.Status != 9
        //            && (studentId > 0 ? x.Id == studentId : true)
        //            ).Select(x => new
        //            {
        //                x.Id,
        //                x.FullName,
        //                x.User.Phone,
        //                x.User.ImagePath,
        //                Resone = db.UserSuspends.Where(k => k.UserId == x.UserId).FirstOrDefault().Resone,
        //                CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
        //                x.CreatedOn,
        //                x.Status,
        //            }).OrderBy(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //        return Ok(new { info = Info, count = Count });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


        //[HttpPost("{id}/Suspend/UnSuspend")]
        //public IActionResult UnSuspend(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.Schools.Include(x => x.User).Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Schoolsuspend = db.UserSuspends.Include(x => x.User).Where(x => x.UserId == Info.UserId).ToList();
        //        if (Schoolsuspend.Count > 0)
        //        {
        //            foreach (var item in Schoolsuspend)
        //            {
        //                item.Status = 9;
        //                item.User.Status = 9;
        //            }
        //        }
        //        db.SaveChanges();
        //        return Ok(BackMessages.SuccessChangeStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}



        ////Change Device Request
        //[HttpGet("ChangeRequests/Get")]
        //public IActionResult GetChangeRequests(int pageNo, int pageSize, long studentId)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        int Count = db.ChangeRequest
        //            .Include(x => x)
        //            .Where(x => x.Status != 9
        //            && (studentId > 0 ? db.Schools.Where(k => k.UserId == x.UserId && k.Id == studentId).Count() > 0 : true)
        //        ).Count();
        //        var Info = db.ChangeRequest
        //            .Where(x => x.Status != 9
        //            && (studentId > 0 ? db.Schools.Where(k => k.UserId == x.UserId && k.Id == studentId).Count() > 0 : true)
        //            ).Select(x => new
        //            {
        //                x.Id,
        //                x.Name,
        //                Phone = db.Users.Where(k => k.Id == x.UserId).SingleOrDefault().Phone,
        //                ProssedBy = db.Users.Where(k => k.Id == x.ProssedBy).SingleOrDefault().Name,
        //                Count = db.ChangeRequest.Where(k => k.UserId == x.UserId && x.Status != 9).Count(),
        //                x.OldMachineName,
        //                x.NewMachineName,
        //                x.NewIpAddress,
        //                x.OldIpAddress,
        //                CountProsedProsses = db.ChangeRequest.Where(k => k.UserId == x.UserId && x.Status != 9 && k.ProssedBy > 0).Count(),
        //                CountProsses = db.ChangeRequest.Where(k => k.UserId == x.UserId && x.Status != 9).Count(),
        //                x.CreatedOn,
        //                LastLoginOn = db.Users.Where(k => k.Id == x.UserId).SingleOrDefault().LastLoginOn,
        //                x.Status,
        //            }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //        return Ok(new { info = Info, count = Count });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/ChangeRequests/Approve")]
        //public IActionResult Approve(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.ChangeRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var Count = db.ChangeRequest.Where(x => x.UserId == Info.UserId && x.Status != 9 && x.ProssedBy > 0).Count();
        //        if (Count <= 10)
        //        {
        //            var row = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
        //            row.MachineName = Info.NewMachineName;
        //            Info.Status = 2;
        //            Info.ProssedBy = userId;
        //            db.SaveChanges();
        //            return Ok(BackMessages.SucessAcceptedRequest);
        //        }
        //        else
        //        {
        //            if (user.UserType != 1)
        //                return StatusCode(BackMessages.StatusCode, BackMessages.MaxChangeRequest);

        //            var row = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
        //            row.MachineName = Info.NewMachineName;
        //            Info.Status = 2;
        //            Info.ProssedBy = userId;
        //            db.SaveChanges();
        //            return Ok(BackMessages.SucessAcceptedRequest);
        //        }



        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/ChangeRequests/ResetDeviecInfo")]
        //public IActionResult ResetDeviecInfo(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.Schools.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var row = db.Users.Where(x => x.Id == Info.UserId && x.Status != 9).SingleOrDefault();
        //        row.MachineName = null;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessSaveOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


        //[HttpPost("{id}/ChangeRequests/Reject")]
        //public IActionResult Reject(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.ChangeRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        Info.Status = 3;
        //        Info.ProssedBy = userId;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessRejectRequest);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}





        ////Regester Course Request
        //[HttpGet("RegesterCourse/Get")]
        //public IActionResult GetRegesterCourseRequest(int pageNo, int pageSize, long CourseId, long studentId)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        int Count = db.StudentSubscriptionsRequest.Where(x => x.Status != 9
        //        && (CourseId > 0 ? x.CourseId == CourseId : true)
        //        && (studentId > 0 ? x.StudentId == studentId : true)
        //        ).Count();
        //        var Info = db.StudentSubscriptionsRequest
        //            .Where(x => x.Status != 9
        //            && (CourseId > 0 ? x.CourseId == CourseId : true)
        //        && (studentId > 0 ? x.StudentId == studentId : true)
        //            ).Select(x => new
        //            {
        //                x.Id,
        //                x.Name,
        //                x.Phone,
        //                x.CourseName,
        //                ProssedBy = db.Users.Where(k => k.Id == x.ProssedBy).SingleOrDefault().Name,
        //                x.CreatedOn,
        //                x.Status,
        //            }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //        return Ok(new { info = Info, count = Count });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/RegesterCourse/Approve")]
        //public IActionResult ApproveRegesterCourseRequest(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.StudentSubscriptionsRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        var SchoolsProfile = db.SchoolsProfiles.Where(x => x.StudentId == Info.StudentId && x.Status == 1).SingleOrDefault();
        //        if (SchoolsProfile == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


        //        var isExist = db.StudentSubscriptions.Where(x => x.StudentProfileId == SchoolsProfile.Id && x.CourseId == Info.CourseId && x.Status == 1).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.StudentCourseExist);

        //        StudentSubscriptions row = new StudentSubscriptions();
        //        row.StudentProfileId = SchoolsProfile.Id;
        //        row.CourseId = Info.CourseId;
        //        row.Value = 0;
        //        row.CreatedOn = DateTime.Now;
        //        row.CreatedBy = userId;
        //        row.Status = 1;
        //        db.StudentSubscriptions.Add(row);
        //        Info.Status = 2;
        //        Info.ProssedBy = userId;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAcceptedRequest);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


        //[HttpPost("{id}/RegesterCourse/Reject")]
        //public IActionResult RejectRegesterCourseRequest(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.StudentSubscriptionsRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        Info.Status = 3;
        //        Info.ProssedBy = userId;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessRejectRequest);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{id}/RegesterCourse/Delete")]
        //public IActionResult DeleteRegesterCourseRequest(long id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Info = db.StudentSubscriptionsRequest.Where(x => x.Id == id && x.Status != 9).SingleOrDefault();
        //        if (Info == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        Info.Status = 9;
        //        Info.ProssedBy = userId;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessRejectRequest);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


    }
}