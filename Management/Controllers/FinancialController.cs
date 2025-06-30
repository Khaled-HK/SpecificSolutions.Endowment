//using Common;
//using MathNet.Numerics.Distributions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Vue.Models;
//using Web.Services;
//using static Web.Services.Helper;

//namespace Management.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/admin/Financial")]
//    public class FinancialController : Controller
//    {
//        private Helper help;

//        private readonly TraneemBetaContext db;

//        public FinancialController(TraneemBetaContext context, IConfiguration iConfig)
//        {
//            this.db = context;
//            help = new Helper(iConfig, context);
//        }





//        [HttpGet("GetSubscriptions")]
//        public IActionResult GetSubscriptions(int pageNo, int pageSize, string Search,DateTime? From,DateTime? To)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.StudentsWalletPurchases.Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.StudentCourse)
//                    .Include(x => x.StudentCourse.Course)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.StudentCourse.Course.Instructor)
//                    .Include(x => x.StudentCourse.Course.Instructor.User)
//                    .Include(x => x.StudentCourse.Course.Subject)
//                    .Include(x => x.StudentCourse.Student)
//                    .Include(x => x.StudentCourse.Student.User)
//                    .Where(x => x.Status != 9
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (
//                        x.StudentCourse.Course.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.User.Phone.Contains(Search.Trim())))
//                    ).Count(),


//                    Value = db.StudentsWalletPurchases.Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.StudentCourse)
//                    .Include(x => x.StudentCourse.Course)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.StudentCourse.Course.Instructor)
//                    .Include(x => x.StudentCourse.Course.Instructor.User)
//                    .Include(x => x.StudentCourse.Course.Subject)
//                    .Include(x => x.StudentCourse.Student)
//                    .Include(x => x.StudentCourse.Student.User)
//                    .Where(x => x.Status != 9
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (
//                        x.StudentCourse.Course.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.User.Phone.Contains(Search.Trim())))
//                    ).Sum(x=>x.Value).Value,


//                    Price = db.StudentsWalletPurchases.Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.StudentCourse)
//                    .Include(x => x.StudentCourse.Course)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.StudentCourse.Course.Instructor)
//                    .Include(x => x.StudentCourse.Course.Instructor.User)
//                    .Include(x => x.StudentCourse.Course.Subject)
//                    .Include(x => x.StudentCourse.Student)
//                    .Include(x => x.StudentCourse.Student.User)
//                    .Where(x => x.Status != 9
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (
//                        x.StudentCourse.Course.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.User.Phone.Contains(Search.Trim())))
//                    ).Sum(x=>x.CoursePrice).Value,


//                    Free = db.StudentsWalletPurchases.Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.StudentCourse)
//                    .Include(x => x.StudentCourse.Course)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.StudentCourse.Course.Instructor)
//                    .Include(x => x.StudentCourse.Course.Instructor.User)
//                    .Include(x => x.StudentCourse.Course.Subject)
//                    .Include(x => x.StudentCourse.Student)
//                    .Include(x => x.StudentCourse.Student.User)
//                    .Where(x => x.Status != 9 && x.Value == 0
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (
//                        x.StudentCourse.Course.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.User.Phone.Contains(Search.Trim())))
//                    ).Count(),
//                };

//                int Count = db.StudentsWalletPurchases
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.StudentCourse)
//                    .Include(x => x.StudentCourse.Course)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.StudentCourse.Course.Instructor)
//                    .Include(x => x.StudentCourse.Course.Instructor.User)
//                    .Include(x => x.StudentCourse.Course.Subject)
//                    .Include(x => x.StudentCourse.Student)
//                    .Include(x => x.StudentCourse.Student.User)
//                    .Where(x => x.Status!=9
//                    && ((From.HasValue && From.Value>DateTime.MinValue) ? x.CreatedOn.Value>=From.Value : true)
//                    && ((To.HasValue && To.Value>DateTime.MinValue) ? x.CreatedOn.Value>=To.Value : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (
//                        x.StudentCourse.Course.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.User.Phone.Contains(Search.Trim())
//                        ))
//                    ).Count();

//                var Info = db.StudentsWalletPurchases
//                      .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.StudentCourse)
//                    .Include(x => x.StudentCourse.Course)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization)
//                    .Include(x => x.StudentCourse.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.StudentCourse.Course.Instructor)
//                    .Include(x => x.StudentCourse.Course.Instructor.User)
//                    .Include(x => x.StudentCourse.Course.Subject)
//                    .Include(x => x.StudentCourse.Student)
//                    .Include(x => x.StudentCourse.Student.User)
//                    .Where(x => x.Status != 9
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (
//                        x.StudentCourse.Course.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Subject.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Course.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.Name.Contains(Search.Trim()) ||
//                        x.StudentCourse.Student.User.Phone.Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.StudentCourse.Course.Name,
//                        x.StudentCourse.Course.Image,
//                        Instructor=x.StudentCourse.Course.Instructor.User.Name,
//                        AcademicSpecialization=x.StudentCourse.Course.AcademicSpecialization.Name,
//                        AcademicLevel=x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name,
//                        Subject=x.StudentCourse.Course.Subject.Name,
//                        Student=x.StudentCourse.Student.Name,
//                        x.StudentCourse.Student.User.Phone,
//                        StudentImage=x.StudentCourse.Student.User.Image,
//                        x.CoursePrice,
//                        x.Value,
//                        CreatedOn = x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }


//        [HttpGet("GetRecharge")]
//        public IActionResult GetRecharge(int pageNo, int pageSize, string Search, DateTime? From, DateTime? To
//            , long UserId, long DistributorsId, long PaymentMethodId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Status != 9
//                    && (UserId > 0 ? x.CreatedBy== UserId : true)
//                    && (DistributorsId > 0 ? x.CreatedBy== DistributorsId : true)
//                    && (PaymentMethodId > 0 ? x.PaymentMethodId == PaymentMethodId : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count(),


//                    Value = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Status != 9
//                    && (UserId > 0 ? x.CreatedBy == UserId : true)
//                    && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//                    && (PaymentMethodId > 0 ? x.PaymentMethodId == PaymentMethodId : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Sum(x => x.Value).Value,


//                    MoreThan50 = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Status != 9 && x.Value>=50
//                    && (UserId > 0 ? x.CreatedBy == UserId : true)
//                    && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//                    && (PaymentMethodId > 0 ? x.PaymentMethodId == PaymentMethodId : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count(),
                    
//                    MoreThan100 = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Status != 9 && x.Value>=100
//                    && (UserId > 0 ? x.CreatedBy == UserId : true)
//                    && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//                    && (PaymentMethodId > 0 ? x.PaymentMethodId == PaymentMethodId : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count(),

//                };

//                int Count = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Status != 9
//                    && (UserId > 0 ? x.CreatedBy == UserId : true)
//                    && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//                    && (PaymentMethodId > 0 ? x.PaymentMethodId == PaymentMethodId : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count();

//                var Info = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Status != 9 
//                    && (UserId > 0 ? x.CreatedBy == UserId : true)
//                    && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//                    && (PaymentMethodId > 0 ? x.PaymentMethodId == PaymentMethodId : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Value,
//                        x.PaymentMethod.Name,
//                        x.ProcessType,
//                        Student = db.Students.Where(k => k.WalletId == x.WalletId).Select(k => new
//                        {
//                            k.Id,
//                            k.Name,
//                            k.User.Image,
//                            k.User.Phone,
//                        }).SingleOrDefault(),
//                        CreatedOn = x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("GetWalletTracker")]
//        public IActionResult GetWalletTracker(int pageNo, int pageSize, string Search, DateTime? From, DateTime? To
//            , long ProcessType, long Channel)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.StudentsWalletTracker
//                    .Where(x => x.Status != 9
//                    && (ProcessType > 0 ? x.ProcessType == ProcessType : true)
//                    && (Channel > 0 ? x.Channel == Channel : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count(),


//                    Value = db.StudentsWalletTracker
//                     .Where(x => x.Status != 9
//                    && (ProcessType > 0 ? x.ProcessType == ProcessType : true)
//                    && (Channel > 0 ? x.Channel == Channel : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Sum(x => x.Value).Value,


//                    MoreThan50 = db.StudentsWalletTracker
//                         .Where(x => x.Status != 9
//                    && (ProcessType > 0 ? x.ProcessType == ProcessType : true)
//                    && (Channel > 0 ? x.Channel == Channel : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count(),

//                    MoreThan100 = db.StudentsWalletTracker
//                         .Where(x => x.Status != 9
//                    && (ProcessType > 0 ? x.ProcessType == ProcessType : true)
//                    && (Channel > 0 ? x.Channel == Channel : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count(),

//                };

//                int Count = db.StudentsWalletTracker
//                     .Where(x => x.Status != 9
//                    && (ProcessType > 0 ? x.ProcessType == ProcessType : true)
//                    && (Channel > 0 ? x.Channel == Channel : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Count();

//                var Info = db.StudentsWalletTracker
//                     .Where(x => x.Status != 9
//                    && (ProcessType > 0 ? x.ProcessType == ProcessType : true)
//                    && (Channel > 0 ? x.Channel == Channel : true)
//                    && ((From.HasValue && From.Value > DateTime.MinValue) ? x.CreatedOn.Value >= From.Value : true)
//                    && ((To.HasValue && To.Value > DateTime.MinValue) ? x.CreatedOn.Value <= To.Value : true)
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.ProcessType,
//                        x.Channel,
//                        x.Descriptions,
//                        Student = db.Students.Where(k => k.WalletId == x.WalletId).Select(k => new
//                        {
//                            k.Id,
//                            k.Name,
//                            k.User.Image,
//                            k.User.Phone,
//                        }).SingleOrDefault(),
//                        x.Value,
//                        x.Befroe,
//                        x.After,
//                        CreatedOn = x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }










//        ////Report
//        //[HttpGet("Report/GetSubscriptionss")]
//        //public IActionResult GetSubscriptions(int pageNo, int pageSize, DateTime? SelectedDate)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//        //        if (SelectedDate == null || SelectedDate == DateTime.MinValue)
//        //        {
//        //            int Count = db.StudentWalletPurchases.Where(x => x.Status != 9).Count();
//        //            int Total = db.StudentWalletPurchases.Where(x => x.Status != 9).Sum(x => x.Value).Value;
//        //            if (Total <= 0)
//        //                Total = 0;
//        //            var Info = db.StudentWalletPurchases
//        //                .Include(k => k.StudentCourse)
//        //                .Include(k => k.StudentCourse.Course)
//        //                .Include(k => k.StudentCourse.StudentProfile)
//        //                .Include(k => k.StudentCourse.StudentProfile.Student)
//        //                .Where(x => x.Status != 9).Select(x => new
//        //                {
//        //                    x.Id,
//        //                    x.StudentCourse.StudentProfile.Student.FullName,
//        //                    x.StudentCourse.Course.Name,
//        //                    x.StudentCourse.Course.Price,
//        //                    x.CoursePrice,
//        //                    x.Value,
//        //                    x.CreatedOn,
//        //                    CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//        //                    x.Status,
//        //                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();



//        //            return Ok(new { info = Info, count = Count, Total });
//        //        }
//        //        else
//        //        {
//        //            int Count = db.StudentWalletPurchases.Where(x => x.Status != 9 && x.CreatedOn.Value.Date == SelectedDate.GetValueOrDefault().Date).Count();
//        //            int Total = db.StudentWalletPurchases.Where(x => x.Status != 9 && x.CreatedOn.Value.Date == SelectedDate.GetValueOrDefault().Date).Sum(x => x.Value).Value;
//        //            if (Total <= 0)
//        //                Total = 0;
//        //            var Info = db.StudentWalletPurchases
//        //                .Include(k => k.StudentCourse)
//        //                .Include(k => k.StudentCourse.Course)
//        //                .Include(k => k.StudentCourse.StudentProfile)
//        //                .Include(k => k.StudentCourse.StudentProfile.Student)
//        //                .Where(x => x.Status != 9 && x.CreatedOn.Value.Date == SelectedDate.GetValueOrDefault().Date).Select(x => new
//        //                {
//        //                    x.Id,
//        //                    x.StudentCourse.StudentProfile.Student.FullName,
//        //                    x.StudentCourse.Course.Name,
//        //                    x.StudentCourse.Course.Price,
//        //                    x.CoursePrice,
//        //                    x.Value,
//        //                    x.CreatedOn,
//        //                    CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//        //                    x.Status,
//        //                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //            return Ok(new { info = Info, count = Count, Total });
//        //        }


//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}


//        //[HttpGet("Report/GetRecharge")]
//        //public IActionResult GetRecharge(int pageNo, int pageSize, DateTime? SelectedDate, long UserId, long DistributorsId, long PaymentMethodsId)
//        //{
//        //    try
//        //    {
//        //        var userId = this.help.GetCurrentUser(HttpContext);
//        //        if (userId <= 0)
//        //            return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//        //        int Count = db.StudentWalletTransactions
//        //            .Where(x => x.Status != 9
//        //            && ((SelectedDate != null || SelectedDate > DateTime.MinValue) ? x.CreatedOn.Value.Date == SelectedDate.GetValueOrDefault().Date : true)
//        //            && (UserId > 0 ? x.CreatedBy == UserId : true)
//        //            && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//        //            && (PaymentMethodsId > 0 ? x.PaymentMethodId == PaymentMethodsId : true)
//        //            ).Count();
//        //        int Total = db.StudentWalletTransactions.Where(x => x.Status != 9
//        //        && ((SelectedDate != null || SelectedDate > DateTime.MinValue) ? x.CreatedOn.Value.Date == SelectedDate.GetValueOrDefault().Date : true)
//        //        && (UserId > 0 ? x.CreatedBy == UserId : true)
//        //        && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//        //        ).Sum(x => x.Value).Value;
//        //        if (Total <= 0)
//        //            Total = 0;

//        //        int Withdraw = db.StudentWalletTransactions.Where(x => x.Status != 9 && x.ProcessType == 2
//        //        && ((SelectedDate != null || SelectedDate > DateTime.MinValue) ? x.CreatedOn.Value.Date == SelectedDate.GetValueOrDefault().Date : true)
//        //        && (UserId > 0 ? x.CreatedBy == UserId : true)
//        //        && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//        //        ).Sum(x => x.Value).Value;
//        //        if (Total <= 0)
//        //            Total = 0;

//        //        if (Withdraw <= 0)
//        //            Withdraw = 0;
//        //        var Info = db.StudentWalletTransactions
//        //            .Include(k => k.Wallet)
//        //            .Include(k => k.Wallet.Student)
//        //            .Include(k => k.PaymentMethod)
//        //            .Where(x => x.Status != 9
//        //                && ((SelectedDate != null || SelectedDate > DateTime.MinValue) ? x.CreatedOn.Value.Date == SelectedDate.GetValueOrDefault().Date : true)
//        //                && (UserId > 0 ? x.CreatedBy == UserId : true)
//        //                && (DistributorsId > 0 ? x.CreatedBy == DistributorsId : true)
//        //                && (PaymentMethodsId > 0 ? x.PaymentMethodId == PaymentMethodsId : true)
//        //                ).Select(x => new
//        //                {
//        //                    x.Id,
//        //                    x.Wallet.Student.FullName,
//        //                    x.PaymentMethod.Name,
//        //                    x.Value,
//        //                    x.CreatedOn,
//        //                    CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//        //                    x.Status,
//        //                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//        //        return Ok(new { info = Info, count = Count, Total, Withdraw });












//        //        //if (SelectedDate==null || SelectedDate==DateTime.MinValue)
//        //        //{
//        //        //    int Count = db.StudentWalletTransactions.Where(x => x.Status != 9).Count();
//        //        //    int Total = db.StudentWalletTransactions.Where(x => x.Status != 9).Sum(x=>x.Value).Value;
//        //        //    if (Total <= 0)
//        //        //        Total = 0;
//        //        //    var Info = db.StudentWalletTransactions
//        //        //        .Include(k => k.Wallet)
//        //        //        .Include(k => k.Wallet.Student)
//        //        //        .Include(k => k.PaymentMethod)
//        //        //        .Where(x => x.Status != 9).Select(x => new
//        //        //        {
//        //        //            x.Id,
//        //        //            x.Wallet.Student.FullName,
//        //        //            x.PaymentMethod.Name,
//        //        //            x.Value,
//        //        //            x.CreatedOn,
//        //        //            CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//        //        //            x.Status,
//        //        //        }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();



//        //        //    return Ok(new { info = Info, count = Count , Total });
//        //        //}
//        //        //else
//        //        //{

//        //        //}


//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return StatusCode(500, e.Message);
//        //    }
//        //}


























//    }
//}