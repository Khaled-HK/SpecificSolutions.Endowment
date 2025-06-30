using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/Users")]
    public class UserController : Controller
    {

        private readonly JeelContext db;
        private Security security;

        public UserController(IConfiguration iConfig, JeelContext context)
        {
            this.db = context;
            security = new Security(iConfig, context);
        }


        //[HttpGet("Get")]
        //public IActionResult Get(int pageNo, int pageSize, string Search)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Statistics = new
        //        {
        //            Count = db.Users.Where(x => x.Status != 9 && x.UserType <= 10).Count(),
        //            Admin = db.Users.Where(x => x.Status != 9 && x.UserType == 1).Count(),
        //            User = db.Users.Where(x => x.Status != 9 && x.UserType == 2).Count(),
        //            Supplier = db.Users.Where(x => x.Status != 9 && x.UserType == 3).Count(),
        //        };

        //        var Count = db.Users.Where(x => x.Status != 9 && (x.UserType > 0 && x.UserType <= 10)
        //        && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
        //                x.Phone.Contains(Search.Trim()) ||
        //                x.Email.Contains(Search.Trim()) ||
        //                x.LoginName.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
        //        ).Count();

        //        var Info = db.Users.Where(x => x.Status != 9 && (x.UserType > 0 && x.UserType <= 10)
        //        && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
        //                x.Phone.Contains(Search.Trim()) ||
        //                x.Email.Contains(Search.Trim()) ||
        //                x.LoginName.Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
        //                x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
        //        ).Select(x => new
        //        {
        //            x.Id,
        //            x.Name,
        //            x.LoginName,
        //            x.Email,
        //            x.UserType,
        //            x.Image,
        //            x.Phone,
        //            x.LastLoginOn,
        //            x.Status,
        //            x.CreatedOn,
        //            CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
        //        }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


        //        return Ok(new { info = Info, count = Count, Statistics });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpGet("GetTransactions")]
        //public IActionResult GetTransactions(int pageNo, int pageSize, long Id)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var Count = db.Transactions.Where(x => x.CreatedBy == Id).Count();

        //        var Info = db.Transactions.Where(x => x.CreatedBy == Id).Select(x => new
        //        {
        //            x.Id,
        //            x.Operations,
        //            x.Descriptions,
        //            x.Controller,
        //            OldObject = x.OldObject != null ? x.OldObject : "",
        //            NewObject = x.NewObject != null ? x.NewObject : "",
        //            x.CreatedOn,
        //        }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


        //        return Ok(new { info = Info, count = Count });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var userId = security.GetCurrentUser(HttpContext);
                if (userId <= 0)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
                if (user == null)
                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


                var Info = db.Users.Where(x => x.Status != 9 && (x.UserType > 0 && x.UserType <= 10)).Select(x => new
                {
                    x.Id,
                    x.Name,
                }).OrderByDescending(x => x.Name).ToList();


                return Ok(new { info = Info });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        //public partial class BodyObject
        //{
        //    public long? Id { get; set; }
        //    public string Name { get; set; }
        //    public string LoginName { get; set; }
        //    public string Email { get; set; }
        //    public string Phone { get; set; }
        //    public short UserType { get; set; }
        //    public string Image { get; set; }
        //    public string ImageName { get; set; }
        //}

        //[HttpPost("Add")]
        //public IActionResult AddDistributors([FromBody] BodyObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var User = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (User == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (User.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //        if (string.IsNullOrEmpty(bodyObject.Name))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //        if (string.IsNullOrEmpty(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

        //        if (!this.help.IsValidEmail(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

        //        if (string.IsNullOrWhiteSpace(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

        //        if (!this.help.IsValidPhone(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

        //        if (bodyObject.UserType <= 0 || bodyObject.UserType > 10)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.userPermissone);


        //        var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        IsExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

        //        IsExist = db.Users.Where(x => x.Status != 9 && x.Email == bodyObject.Email).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);

        //        Users row = new Users();
        //        row.Name = bodyObject.Name;
        //        row.LoginName = bodyObject.LoginName;
        //        row.UserType = bodyObject.UserType;
        //        row.Email = bodyObject.Email;
        //        string Password = this.help.GenreatePass();
        //        row.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);
        //        if (string.IsNullOrEmpty(bodyObject.Image))
        //        {
        //            row.Image = "/Uploads/User.jpg";
        //        }
        //        else
        //        {
        //            row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
        //        }
        //        row.Phone = bodyObject.Phone;
        //        row.LoginTryAttempts = 0;
        //        row.CreatedBy = userId;
        //        row.CreatedOn = DateTime.Now;
        //        row.Status = 1;
        //        db.Users.Add(row);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Add;
        //        rowTrans.Descriptions = "إضافة بيانات  ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessAddOperations + " " + "كلمة المرور الخاصة بالمستخدم : " + Password);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("Edit")]
        //public IActionResult EditDistributors([FromBody] BodyObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var User = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (User == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (User.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //        if (string.IsNullOrEmpty(bodyObject.Name))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //        if (string.IsNullOrEmpty(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

        //        if (!this.help.IsValidEmail(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

        //        if (string.IsNullOrWhiteSpace(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

        //        if (!this.help.IsValidPhone(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

        //        if (bodyObject.UserType <= 0 || bodyObject.UserType > 10)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.userPermissone);


        //        var row = db.Users.Where(x => x.Status != 9 && x.Id == bodyObject.Id).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.OldObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Image,
        //            row.LoginName,
        //            row.Email,
        //            row.Phone,
        //            row.CreatedBy,
        //            row.CreatedOn,
        //            row.Status,
        //            row.UserType
        //        });

        //        var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        IsExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone && x.Id != bodyObject.Id).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

        //        IsExist = db.Users.Where(x => x.Status != 9 && x.Email == bodyObject.Email && x.Id != bodyObject.Id).SingleOrDefault();
        //        if (IsExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);


        //        row.Name = bodyObject.Name;
        //        row.LoginName = bodyObject.LoginName;
        //        row.Email = bodyObject.Email;
        //        row.UserType = bodyObject.UserType;
        //        if (!string.IsNullOrEmpty(bodyObject.Image))
        //            row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
        //        row.Phone = bodyObject.Phone;

        //        rowTrans.Operations = TransactionsType.Edit;
        //        rowTrans.Descriptions = "تعديل بيانات  ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);


        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations);


        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/ResetPassword")]
        //public IActionResult ResetPasswordDistributors(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


        //        var row = db.Users.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        string Password = this.help.GenreatePass();
        //        row.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Reset;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "تهيئة كلمة المرور  ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Phone,
        //            row.CreatedOn,
        //            row.CreatedBy,
        //            row.Status,
        //            row.UserType
        //        });
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessResetOperations + " " + "كلمة المرور الجديدة : " + Password);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/ChangeStatus")]
        //public IActionResult ChangeStatusDistributors(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var row = db.Users
        //            .Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
        //        rowTrans.Operations = TransactionsType.CahngeStatus;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = " تغير حالة    ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Phone,
        //            row.CreatedBy,
        //            row.CreatedOn,
        //            row.UserType,
        //        });
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SuccessChangeStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("{Id}/Delete")]
        //public IActionResult DeleteDistributors(long Id)
        //{
        //    try
        //    {
        //        if (Id <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var user = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (user == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (user.UserType != 1)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var row = db.Users
        //            .Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


        //        row.Status = 9;

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.Operations = TransactionsType.Delete;
        //        rowTrans.ItemId = row.Id;
        //        rowTrans.Descriptions = "حذف بيانات   ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Id,
        //            row.Name,
        //            row.Phone,
        //            row.CreatedBy,
        //            row.CreatedOn,
        //            row.UserType
        //        });
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










        //public partial class ProfileBodyObject
        //{
        //    public string Name { get; set; }
        //    public string LoginName { get; set; }
        //    public string Email { get; set; }
        //    public string Phone { get; set; }
        //    public string Image { get; set; }
        //    public string ImageName { get; set; }
        //    public string Password { set; get; }
        //    public string NewPassword { set; get; }
        //}

        //[HttpPost("Profile/ChangeInfo")]
        //public IActionResult ChangeInfo([FromBody] ProfileBodyObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var User = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (User == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (string.IsNullOrWhiteSpace(bodyObject.Name))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //        if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.LocationNameEmpty);

        //        if (string.IsNullOrWhiteSpace(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

        //        if (!this.help.IsValidPhone(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

        //        if (string.IsNullOrWhiteSpace(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

        //        if (!this.help.IsValidEmail(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

        //        var isExist = db.Users.Where(x => x.Status != 9 && x.Id != userId && x.Name == bodyObject.Name).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //        isExist = db.Users.Where(x => x.Status != 9 && x.Id != userId && x.Phone == bodyObject.Phone).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

        //        isExist = db.Users.Where(x => x.Status != 9 && x.Id != userId && x.LoginName == bodyObject.LoginName).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.LoginNameExist);

        //        isExist = db.Users.Where(x => x.Status != 9 && x.Id != userId && x.Email == bodyObject.Email).SingleOrDefault();
        //        if (isExist != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);

        //        var row = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.OldObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Image,
        //            row.Password,
        //            row.Name,
        //            row.LoginName,
        //            row.Email,
        //            row.Phone
        //        });



        //        row.Name = bodyObject.Name;
        //        row.LoginName = bodyObject.LoginName;
        //        row.Phone = bodyObject.Phone;
        //        row.Email = bodyObject.Email;

        //        rowTrans.Operations = TransactionsType.Edit;
        //        rowTrans.Descriptions = " تغير البيانات الشخصية   ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations + " " + "يتم تسجيل الخروج لحفظ البيانات ");
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //[HttpPost("Profile/ChangePicture")]
        //public IActionResult ChangePicture([FromBody] ProfileBodyObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        var User = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (User == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (string.IsNullOrWhiteSpace(bodyObject.Image) || string.IsNullOrWhiteSpace(bodyObject.ImageName))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.ImageEmpty);


        //        var row = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.OldObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Image,
        //            row.Password,
        //            row.Name,
        //            row.LoginName,
        //            row.Email,
        //            row.Phone
        //        });



        //        row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);

        //        rowTrans.Operations = TransactionsType.Edit;
        //        rowTrans.Descriptions = " تغير صورة الشخصية  ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations + " " + "يتم تسجيل الخروج لحفظ البيانات ");
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


        //[HttpPost("Profile/ChangePassword")]
        //public IActionResult ChangePassword([FromBody] ProfileBodyObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);



        //        var row = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        TransactionsObject rowTrans = new TransactionsObject();
        //        rowTrans.OldObject = JsonConvert.SerializeObject(new
        //        {
        //            row.Image,
        //            row.Password,
        //            row.Name,
        //            row.LoginName,
        //            row.Email,
        //            row.Phone
        //        });


        //        if (string.IsNullOrEmpty(bodyObject.Password))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

        //        var areMatched = Security.VerifyHash(bodyObject.Password, row.Password, HashAlgorithms.SHA512);

        //        if (!areMatched)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PasswordRong);

        //        if (bodyObject.NewPassword.Length < 6)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PasswordLenght);

        //        row.Password = Security.ComputeHash(bodyObject.NewPassword, HashAlgorithms.SHA512, null);



        //        rowTrans.Operations = TransactionsType.Edit;
        //        rowTrans.Descriptions = " تغير كلمة المرور  ";
        //        rowTrans.Controller = "Users";
        //        rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
        //        rowTrans.CreatedBy = userId;
        //        this.help.WriteTransactions(rowTrans);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}














        //public class ChangeImageBodyObject
        //{
        //    public long Id { set; get; }
        //    public string ImageName { get; set; }
        //    public string FileType { get; set; }
        //    public string FileBase64 { get; set; }
        //}

        //[HttpPost("ChangeImage")]
        //public IActionResult ChangeImage([FromBody] ChangeImageBodyObject bodyObject)
        //{
        //    var userId = this.help.GetCurrentUser(HttpContext);
        //    if (userId <= 0)
        //        return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //    if (bodyObject == null)
        //        return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //    var row = db.Users.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
        //    if (row == null)
        //        return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.FileType, bodyObject.FileBase64);
        //    db.SaveChanges();
        //    return Ok(BackMessages.SucessEditOperations);
        //}


        //public class ProfilePicObject
        //{
        //    public string ImageName { get; set; }
        //    public string FileBase64 { get; set; }
        //}

        //[HttpPost("ChangeProfilePicture")]
        //public IActionResult ChangeProfilePicture([FromBody] ProfilePicObject bodyObject)
        //{
        //    var userId = this.help.GetCurrentUser(HttpContext);
        //    if (userId <= 0)
        //        return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //    if (bodyObject == null)
        //        return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //    var row = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
        //    if (row == null)
        //        return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //    row.Image = this.help.UploadFile(bodyObject.ImageName ,bodyObject.FileBase64);
        //    db.SaveChanges();
        //    return Ok(BackMessages.SucessEditOperations);
        //}


        //public class ChangePasswordBodyObject
        //{
        //    public string Password { set; get; }
        //    public string NewPassword { set; get; }
        //}
        //[HttpPost("ChangePassword")]
        //public IActionResult ChangePassword([FromBody] ChangePasswordBodyObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var row=db.Users.Where(x=>x.Id==userId).SingleOrDefault();
        //        if(row== null)
        //            return StatusCode(BackMessages.StatusCode,BackMessages.NotFound);

        //        if(string.IsNullOrEmpty(bodyObject.Password))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

        //        var areMatched = Security.VerifyHash(bodyObject.Password, row.Password, HashAlgorithms.SHA512);

        //        if (!areMatched)
        //            return StatusCode(BackMessages.StatusCode,BackMessages.PasswordRong);   

        //        if (bodyObject.NewPassword.Length < 6)
        //            return StatusCode(BackMessages.StatusCode,BackMessages.PasswordLenght);

        //        row.Password = Security.ComputeHash(bodyObject.NewPassword, HashAlgorithms.SHA512, null);
        //        db.SaveChanges();

        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}



        //public class NameObject
        //{
        //    public string FullName { get; set; }
        //    public string FirstName { get; set; }
        //    public string FatherName { get; set; }
        //    public string GrandFateherName { get; set; }
        //    public string SirName { get; set; }
        //    public string LoginName { get; set; }
        //    public short Gender { get; set; }
        //}
        //[HttpPost("EditPersonalInfo")]
        //public IActionResult EditPersonalInfo([FromBody] NameObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var row = db.Users.Where(x => x.Id == userId).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if(row.UserType==1)
        //        {
        //            if (string.IsNullOrEmpty(bodyObject.FullName) || string.IsNullOrEmpty(bodyObject.LoginName))
        //                return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

        //            var isExsit = db.Users.Where(x => x.Name == bodyObject.FullName && x.Status != 9 && x.Id != userId).SingleOrDefault();
        //            if(isExsit!=null)
        //                return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //            isExsit = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 && x.Id != userId).SingleOrDefault();
        //            if(isExsit!=null)
        //                return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

        //            row.Name = bodyObject.FullName;
        //        }

        //        row.LoginName = bodyObject.LoginName;
        //        db.SaveChanges();

        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //public class ContactObject
        //{
        //    public string Phone { get; set; }
        //    public string ExtraPhone { get; set; }
        //    public string Email { get; set; }
        //}
        //[HttpPost("EditContactInfo")]
        //public IActionResult EditContactInfo([FromBody] ContactObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var row = db.Users.Where(x => x.Id == userId).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (string.IsNullOrEmpty(bodyObject.Email))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

        //        if (string.IsNullOrEmpty(bodyObject.Phone))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

        //        var isExsit = db.Users.Where(x => x.Email == bodyObject.Email && x.Status != 9 && x.Id != userId).SingleOrDefault();
        //        if (isExsit != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);

        //        isExsit = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9 && x.Id != userId).SingleOrDefault();
        //        if (isExsit != null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);


        //        row.Phone = bodyObject.Phone;
        //        row.Email = bodyObject.Email;
        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}

        //public class AboutMeInfoObject
        //{
        //    public string AboutMe { get; set; }
        //}
        //[HttpPost("AboutMeInfo")]
        //public IActionResult AboutMeInfo([FromBody] AboutMeInfoObject bodyObject)
        //{
        //    try
        //    {
        //        var userId = this.help.GetCurrentUser(HttpContext);
        //        if (userId <= 0)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

        //        if (bodyObject == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

        //        var row = db.Users.Where(x => x.Id == userId).SingleOrDefault();
        //        if (row == null)
        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

        //        if (string.IsNullOrEmpty(bodyObject.AboutMe))
        //            return StatusCode(BackMessages.StatusCode, BackMessages.AboutMeEmpty);

        //        db.SaveChanges();
        //        return Ok(BackMessages.SucessEditOperations);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}


























    }
}