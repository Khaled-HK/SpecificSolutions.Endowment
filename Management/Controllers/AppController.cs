//using Common;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using NPOI.POIFS.Properties;
//using NPOI.SS.Formula.Functions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Vue.Models;
//using Web.Services;
//using static Web.Services.Helper;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using Microsoft.AspNetCore.Hosting.Server;
//using NPOI.HSSF.Record;
//using Org.BouncyCastle.Bcpg;
//using Org.BouncyCastle.Crypto.Engines;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Text;

//namespace Management.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/admin/App")]
//    public class AppController : Controller
//    {
//        private Helper help;

//        private readonly TraneemBetaContext db;
//        private IConfiguration _configuration;

//        public AppController(TraneemBetaContext context, IConfiguration iConfig)
//        {
//            _configuration = iConfig;
//            this.db = context;
//            help = new Helper(iConfig, context);
            
//        }







//        //Auth 
//        public class AuthBodyObject
//        {
//            public string Email { get; set; }
//            public string Password { get; set; }
//            public string CaptchaVerify { get; set; }
//            public string DeviceSignature { get; set; }
//        }


//        [HttpPost("Auth/Lgoin")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Login([FromBody] AuthBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);


//                bool isRecaptchaWork = bool.Parse(_configuration.GetSection("Links").GetSection("IsRecaptchaActive").Value);

//                if (isRecaptchaWork)
//                {
//                    bool IsCaptchaValid = (ReCaptchaClass.Validate(bodyObject.CaptchaVerify) == "true" ? true : false);
//                    if (!IsCaptchaValid)
//                    {
//                        return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.Recaptha);
//                    }
//                }

//                //Chaeck version 
//                List<string> BlockDevices = _configuration.GetSection("BlockedDevices").Get<List<string>>();
//                if (BlockDevices.Any(blockedVersion => HttpContext.Request.Headers["User-Agent"].ToString().Contains(blockedVersion)))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.BlcockVersione);


//                if (string.IsNullOrWhiteSpace(bodyObject.Email))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterEmailandUserName);

//                if (string.IsNullOrWhiteSpace(bodyObject.Password))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

//                var row = db.Users.Where(x => x.Status!=9
//                    && x.UserType == 60 
//                    && (x.LoginName == bodyObject.Email || x.Phone == bodyObject.Email)).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RongPasswordandEmail);

//                if (row.Status == 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcount);

//                if (row.Status == 2)
//                {
//                    if (row.LoginTryAttemptDate != null)
//                    {
//                        DateTime dt = row.LoginTryAttemptDate.Value;
//                        double minuts = 30;
//                        dt = dt.AddMinutes(minuts);
//                        if (dt >= DateTime.Now)
//                        {
//                            return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcountTryAttamp);
//                        }
//                        else
//                        {
//                            row.Status = 1;
//                            db.SaveChanges();
//                        }
//                    }
//                    else { return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcountTryAttamp); }
//                }

//                if (!Security.VerifyHash(bodyObject.Password, row.Password, HashAlgorithms.SHA512))
//                {

//                    row.LoginTryAttempts++;
//                    if (row.LoginTryAttempts >= 5 && row.Status == 1)
//                    {
//                        row.LoginTryAttemptDate = DateTime.Now;
//                        row.Status = 2;
//                    }
//                    db.SaveChanges();
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RongPasswordandEmail);
//                }

//                row.LoginTryAttempts = 0;
//                row.LastLoginOn = DateTime.Now;

//                var Suspend = db.UsersSuspends.Where(x => x.UserId == row.Id && x.Status != 9).FirstOrDefault();
//                if (Suspend != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentBlockedAcount);


//                var UserDevices = db.UsersDevices.Where(x => x.Status == 1 && x.UserId == row.Id).SingleOrDefault();
//                if(UserDevices==null)
//                {
//                    UsersDevices NewDevice = new UsersDevices();
//                    NewDevice.UserId = row.Id;
//                    NewDevice.MachineName= HttpContext.Request.Headers["User-Agent"].ToString();
//                    NewDevice.IpAddress= HttpContext.Connection.RemoteIpAddress?.ToString();
//                    NewDevice.DeviceSignature = bodyObject.DeviceSignature;
//                    NewDevice.CreatedOn = DateTime.Now;
//                    NewDevice.CreatedBy = row.Id;
//                    NewDevice.Status = 1;
//                    db.UsersDevices.Add(NewDevice);

//                }
//                else if(UserDevices.MachineName != HttpContext.Request.Headers["User-Agent"].ToString()
//                        && UserDevices.IpAddress != HttpContext.Connection.RemoteIpAddress?.ToString()
//                        && UserDevices.DeviceSignature != bodyObject.DeviceSignature)
//                {
//                    int Count = db.UsersChangeRequest.Where(x => x.Status != 9 && x.UserId == row.Id).Count();

//                    return StatusCode(BackMessages.StatusCode, BackMessages.MachineNameNotValid + "<br>" + "طلبات تغير الأجهزة المستهلكة " + " " + Count + "من أصل 3 ");
//                }
              
//                var Student = db.Students
//                    .Include(x=>x.Wallet)
//                    .Where(x => x.UserId == row.Id).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentNotFound);

//                db.SaveChanges();

//                var Info = new
//                {
//                    Student.FirstName,
//                    Student.FatherName,
//                    Student.SirName,
//                    ExtraPhone=Student.Phone,
//                    Student.Wallet.Value,
//                    Student.Rate,
//                    Student.Points,
//                    row.Id,
//                    row.Name,
//                    row.Phone,
//                    row.LoginName,
//                    row.Email,
//                    row.Image,
//                    row.About,
//                    row.CreatedOn,
//                };

//                const string Issuer = "https://platform.traneem.ly/";
//                var claims = new List<Claim>
//        {
//            new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/Id", row.Id.ToString(), ClaimValueTypes.Integer64, Issuer),
//            new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/userType", row.UserType.ToString(), ClaimValueTypes.Integer32, Issuer)
//        };

//                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                var userPrincipal = new ClaimsPrincipal(userIdentity);

//                await HttpContext.SignInAsync(
//                    CookieAuthenticationDefaults.AuthenticationScheme,
//                    userPrincipal,
//                    new AuthenticationProperties
//                    {
//                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
//                        IsPersistent = true,
//                        AllowRefresh = true
//                    });

//                return Ok(Info);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }


//        [AllowAnonymous]
//        [HttpPost("Auth/Logout")]
//        public async Task<IActionResult> Logout()
//        {
//            try
//            {
//                foreach (var cookie in Request.Cookies.Keys)
//                {
//                    Response.Cookies.Delete(cookie);
//                }

//                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//                return Ok();
//            }
//            catch (Exception)
//            {
//                return StatusCode(BackMessages.StatusCode, BackMessages.LogoutError);
//            }
//        }

//        [AllowAnonymous]
//        [HttpGet("Auth/IsLoggedin")]
//        public async Task<IActionResult> IsLogin(string returnUrl = null)
//        {
//            bool isAuthenticated = User.Identity.IsAuthenticated;
//            if (isAuthenticated)
//            {
//                return Ok(true);
//            }
//            else
//            {
//                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//                return Ok(false);
//            }
//        }


//        [HttpPost("Auth/RequestCahngeDevice")]
//        [AllowAnonymous]
//        public async Task<IActionResult> CahngeDevice([FromBody] AuthBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);


//                bool isRecaptchaWork = bool.Parse(_configuration.GetSection("Links").GetSection("IsRecaptchaActive").Value);

//                if (isRecaptchaWork)
//                {
//                    bool IsCaptchaValid = (ReCaptchaClass.Validate(bodyObject.CaptchaVerify) == "true" ? true : false);

//                    if (!IsCaptchaValid)
//                    {
//                        return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.Recaptha);
//                    }
//                }

//                if (string.IsNullOrWhiteSpace(bodyObject.Email))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterEmailandUserName);

//                if (string.IsNullOrWhiteSpace(bodyObject.Password))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

//                var user = db.Users.Where(x => (x.LoginName == bodyObject.Email || x.Phone == bodyObject.Email) 
//                    && x.Status != 9 && x.UserType == 60).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RongPasswordandEmail);

//                var UserDevices = db.UsersDevices.Where(x => x.Status == 1 && x.UserId == user.Id).SingleOrDefault();
//                if (UserDevices == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ChangeRequestNotValidNow);

                
//                if (UserDevices.MachineName == HttpContext.Request.Headers["User-Agent"].ToString()
//                        || UserDevices.IpAddress == HttpContext.Connection.RemoteIpAddress?.ToString()
//                        || UserDevices.DeviceSignature == bodyObject.DeviceSignature)
//                {
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ChangeRequestNotValidFromThsDevice);
//                }

//                int Count = db.UsersChangeRequest.Where(x => x.Status != 9 && x.UserId == user.Id).Count();

//                if (Count >= int.Parse(_configuration.GetSection("Settings").GetSection("MaxChangeRequestCount").Value))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ChangeRequestCompleate);

//                var isExist = db.UsersChangeRequest.Where(x => x.Status == 1 && x.UserId == user.Id).Count();
//                if (isExist > 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ChangeRequestPending);


//                var Suspend = db.UsersSuspends.Where(x => x.UserId == user.Id && x.Status != 9).FirstOrDefault();
//                if (Suspend != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentBlockedAcount);

//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RongPasswordandEmail);

//                if (user.Status == 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcount);

//                if (user.Status == 2)
//                {
//                    if (user.LoginTryAttemptDate != null)
//                    {
//                        DateTime dt = user.LoginTryAttemptDate.Value;
//                        double minuts = 30;
//                        dt = dt.AddMinutes(minuts);
//                        if (dt >= DateTime.Now)
//                        {
//                            return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcountTryAttamp);
//                        }
//                        else
//                        {
//                            user.Status = 1;
//                            db.SaveChanges();
//                        }
//                    }
//                    else { return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcountTryAttamp); }
//                }

//                if (!Security.VerifyHash(bodyObject.Password, user.Password, HashAlgorithms.SHA512))
//                {

//                    user.LoginTryAttempts++;
//                    if (user.LoginTryAttempts >= 5 && user.Status == 1)
//                    {
//                        user.LoginTryAttemptDate = DateTime.Now;
//                        user.Status = 2;
//                    }
//                    db.SaveChanges();
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RongPasswordandEmail);
//                }


//                UsersChangeRequest row = new UsersChangeRequest();
//                row.UserId = user.Id;
//                row.Name = user.Name;
//                row.OldMachineName = UserDevices.MachineName;
//                row.NewMachineName = HttpContext.Request.Headers["User-Agent"].ToString();
//                row.OldIpAddress = UserDevices.IpAddress;
//                row.NewIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.UsersChangeRequest.Add(row);
//                db.SaveChanges();

//                return Ok(BackMessages.SucessRequestOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }


//        //Auth Students
//        public partial class StudentBodyObject
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
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//            public string Password { get; set; }
//            public string ConfirmPassword { get; set; }
//        }


//        [AllowAnonymous]
//        [HttpPost("Auth/Regester")]
//        public IActionResult Add([FromBody] StudentBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

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

//                if(!this.help.IsValidPassword(bodyObject.Password))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PasswordNotValid);

//                if (!help.IsCorrectPassword(bodyObject.Password, bodyObject.ConfirmPassword))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ConfirmPassword);


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
//                user.Password = Security.ComputeHash(bodyObject.Password, HashAlgorithms.SHA512, null);
//                user.UserType = 60;
//                if (string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    user.Image = "/Uploads/User.jpg";
//                }
//                else
//                {
//                    user.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                }
//                //user.Otp = Security.ComputeHash(this.help.GenerateRandomNo().ToString(), HashAlgorithms.SHA512, null); 
//                user.Otp = Security.ComputeHash("12345", HashAlgorithms.SHA512, null);
//                user.Otpdate = DateTime.Now;
//                user.OtptryAtempt = 0;
//                user.CreatedOn = DateTime.Now;
//                user.Status = 1;

//                StudentsWallet Wallet = new StudentsWallet();
//                Wallet.Value = 0;
//                Wallet.CreatedOn = DateTime.Now;
//                Wallet.Status = 1;

//                Students row = new Students();
//                row.FirstName = bodyObject.FirstName;
//                row.FatherName = bodyObject.FatherName;
//                row.SirName = bodyObject.SirName;
//                row.Name = Name;
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
//                this.help.WriteTransactions(rowTrans);
//                db.SaveChanges();
//                return Ok(BackMessages.SucessRegestperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        public partial class OtpBodyObject
//        {
//            public string Phone { get; set; }
//            public string Otp { get; set; }
//            public string DeviceSignature { get; set; }
//        }

//        [AllowAnonymous]
//        [HttpPost("Auth/CheckOtp")]
//        public async Task<IActionResult> CheckOtp([FromBody] OtpBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                if(string.IsNullOrEmpty(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//                if (!help.IsValidPhone(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

//                var User = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (User.Otpdate.GetValueOrDefault() >= DateTime.Now.AddMinutes(15))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.OtpExpiry);


//                if (User.OtptryAtempt >= 5 && User.OtptryAtemptDate.GetValueOrDefault() <= DateTime.Now.AddMinutes(15))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.OtpBlcockTry);



//                if (!Security.VerifyHash(bodyObject.Otp, User.Otp, HashAlgorithms.SHA512))
//                {
//                    User.OtptryAtempt++;
//                    User.OtptryAtemptDate = DateTime.Now;
//                    db.SaveChanges();
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RongOtp);
//                }

//                User.OtptryAtempt = 0;
//                User.LastLoginOn = DateTime.Now;

//                var Suspend = db.UsersSuspends.Where(x => x.UserId == User.Id && x.Status != 9).FirstOrDefault();
//                if (Suspend != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentBlockedAcount);


//                var UserDevices = db.UsersDevices.Where(x => x.Status == 1 && x.UserId == User.Id).SingleOrDefault();
//                if (UserDevices == null)
//                {
//                    UsersDevices NewDevice = new UsersDevices();
//                    NewDevice.UserId = User.Id;
//                    NewDevice.MachineName = HttpContext.Request.Headers["User-Agent"].ToString();
//                    NewDevice.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//                    NewDevice.DeviceSignature = bodyObject.DeviceSignature;
//                    NewDevice.CreatedOn = DateTime.Now;
//                    NewDevice.CreatedBy = User.Id;
//                    NewDevice.Status = 1;
//                    db.UsersDevices.Add(NewDevice);

//                }
                

//                var Student = db.Students
//                    .Include(x => x.Wallet)
//                    .Where(x => x.UserId == User.Id).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentNotFound);

//                db.SaveChanges();

//                var Info = new
//                {
//                    Student.FirstName,
//                    Student.FatherName,
//                    Student.SirName,
//                    ExtraPhone = Student.Phone,
//                    Student.Wallet.Value,
//                    Student.Rate,
//                    Student.Points,
//                    User.Id,
//                    User.Name,
//                    User.Phone,
//                    User.LoginName,
//                    User.Email,
//                    User.Image,
//                    User.About,
//                    User.CreatedOn,
//                };

//                const string Issuer = "https://platform.traneem.ly/";
//                var claims = new List<Claim>();
//                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/Id", User.Id.ToString(), ClaimValueTypes.Integer64, Issuer));

//                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/userType", User.UserType.ToString(), ClaimValueTypes.Integer32, Issuer));
//                var userIdentity = new ClaimsIdentity("9@F4h#6*sT8g2D^1!5qY7rP3wE0zX2cV9bN4mG1");
//                userIdentity.AddClaims(claims);
//                var userPrincipal = new ClaimsPrincipal(userIdentity);

//                await HttpContext.SignInAsync(
//                    CookieAuthenticationDefaults.AuthenticationScheme,
//                    userPrincipal,
//                    new AuthenticationProperties
//                    {
//                        ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddHours(1)),
//                        IsPersistent = true,
//                        AllowRefresh = true
//                    });

//                return Ok(Info);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        public partial class ForgetPasswordBodyObject
//        {
//            public string Phone { get; set; }
//            public string Otp { get; set; }
//            public string NewPassword { get; set; }
//            public string ConfirmPassword { get; set; }
//            public string DeviceSignature { get; set; }
//        }

//        [AllowAnonymous]
//        [HttpPost("Auth/ForgetPassword")]
//        public IActionResult ForgetPassword([FromBody] ForgetPasswordBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                if (string.IsNullOrEmpty(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//                if (!help.IsValidPhone(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

//                var User = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone).SingleOrDefault();
//                if(User==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);
//                if (User.Status == 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcount);

//                if (User.OtptryAtempt >= 5 && User.OtptryAtemptDate.GetValueOrDefault() <= DateTime.Now.AddMinutes(15))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.OtpBlcockTry);

//                User.Otp = Security.ComputeHash("12345", HashAlgorithms.SHA512, null);
//                User.Otpdate = DateTime.Now;
//                User.OtptryAtempt = 0;
//                User.CreatedOn = DateTime.Now;

//                return Ok(BackMessages.SucessSentOtpMessage);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [AllowAnonymous]
//        [HttpPost("Auth/ChangePassword")]
//        public async Task<IActionResult> ChangePasswordAsync([FromBody] ForgetPasswordBodyObject bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                if (string.IsNullOrEmpty(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//                if (!help.IsValidPhone(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);


//                var User = db.Users.Where(x => x.Phone == bodyObject.Phone && x.Status != 9).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (User.Status == 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.BlockedAcount);

//                if(User.Otpdate.GetValueOrDefault()>= DateTime.Now.AddMinutes(15))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.OtpExpiry);


//                if (User.OtptryAtempt >= 5 && User.OtptryAtemptDate.GetValueOrDefault()<=DateTime.Now.AddMinutes(15))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.OtpBlcockTry);
                    


//                if (!Security.VerifyHash(bodyObject.Otp, User.Otp, HashAlgorithms.SHA512))
//                {
//                    User.OtptryAtempt++;
//                    User.OtptryAtemptDate = DateTime.Now;
//                    db.SaveChanges();
//                    return StatusCode(BackMessages.StatusCode, BackMessages.RongOtp);
//                }

//                User.OtptryAtempt = 0;
//                User.LastLoginOn = DateTime.Now;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    User.Image,
//                    User.Password,
//                    User.Name,
//                    User.LoginName,
//                    User.Email,
//                    User.Phone
//                });


//                if (string.IsNullOrEmpty(bodyObject.NewPassword))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterPassword);

//                if (!this.help.IsValidPassword(bodyObject.NewPassword))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PasswordNotValid);

//                if (!help.IsCorrectPassword(bodyObject.NewPassword, bodyObject.ConfirmPassword))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.ConfirmPassword);

//                User.Password = Security.ComputeHash(bodyObject.NewPassword, HashAlgorithms.SHA512, null);



//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.Descriptions = " نسيان كلمة المرور  ";
//                rowTrans.Controller = "Users";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = User.Id;
//                this.help.WriteTransactions(rowTrans);



//                var Suspend = db.UsersSuspends.Where(x => x.UserId == User.Id && x.Status != 9).FirstOrDefault();
//                if (Suspend != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentBlockedAcount);


//                var UserDevices = db.UsersDevices.Where(x => x.Status == 1 && x.UserId == User.Id).SingleOrDefault();
//                if (UserDevices == null)
//                {
//                    UsersDevices NewDevice = new UsersDevices();
//                    NewDevice.UserId = User.Id;
//                    NewDevice.MachineName = HttpContext.Request.Headers["User-Agent"].ToString();
//                    NewDevice.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
//                    NewDevice.DeviceSignature = bodyObject.DeviceSignature;
//                    NewDevice.CreatedOn = DateTime.Now;
//                    NewDevice.CreatedBy = User.Id;
//                    NewDevice.Status = 1;
//                    db.UsersDevices.Add(NewDevice);
//                }
//                else if (UserDevices.MachineName != HttpContext.Request.Headers["User-Agent"].ToString()
//                        && UserDevices.IpAddress != HttpContext.Connection.RemoteIpAddress?.ToString()
//                        && UserDevices.DeviceSignature != bodyObject.DeviceSignature)
//                {
//                    int Count = db.UsersChangeRequest.Where(x => x.Status != 9 && x.UserId == User.Id).Count();

//                    return StatusCode(BackMessages.StatusCode, BackMessages.MachineNameNotValid + "<br>" + "طلبات تغير الأجهزة المستهلكة " + " " + Count + "من أصل 3 ");
//                }


//                var Student = db.Students
//                    .Include(x => x.Wallet)
//                    .Where(x => x.UserId == User.Id).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.StudentNotFound);

//                db.SaveChanges();

//                var Info = new
//                {
//                    Student.FirstName,
//                    Student.FatherName,
//                    Student.SirName,
//                    ExtraPhone = Student.Phone,
//                    Student.Wallet.Value,
//                    Student.Rate,
//                    Student.Points,
//                    User.Id,
//                    User.Name,
//                    User.Phone,
//                    User.LoginName,
//                    User.Email,
//                    User.Image,
//                    User.About,
//                    User.CreatedOn,
//                };

//                const string Issuer = "https://platform.traneem.ly/";
//                var claims = new List<Claim>();
//                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/Id", User.Id.ToString(), ClaimValueTypes.Integer64, Issuer));

//                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2022/10/identity/claims/userType", User.UserType.ToString(), ClaimValueTypes.Integer32, Issuer));
//                var userIdentity = new ClaimsIdentity("9@F4h#6*sT8g2D^1!5qY7rP3wE0zX2cV9bN4mG1");
//                userIdentity.AddClaims(claims);
//                var userPrincipal = new ClaimsPrincipal(userIdentity);

//                await HttpContext.SignInAsync(
//                    CookieAuthenticationDefaults.AuthenticationScheme,
//                    userPrincipal,
//                    new AuthenticationProperties
//                    {
//                        ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddHours(1)),
//                        IsPersistent = true,
//                        AllowRefresh = true
//                    });

//                return Ok(Info);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }












//        //Courses
//        [AllowAnonymous]
//        [HttpGet("Courses/Get")]
//        public IActionResult GetCourses(int pageNo, int pageSize, string Search, bool IsFree, bool IsDiscount
//           , short SalesStatus, short AcademicLevelId, short AcademicSpecializationId, short SubjectId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Courses.Where(x => x.Status == 1).Count(),
//                    Shapter = db.Shapters.Where(x => x.Status == 1 && x.Course.Status == 1).Count(),
//                    Lecture = db.Lectures.Where(x => x.Status == 1 && x.Shapter.Status == 1 && x.Shapter.Course.Status == 1).Count(),
//                    Attachments = db.LecturesAttashments.Where(x => x.Status == 1 
//                        && x.Lecture.Status == 1
//                        && x.Lecture.Shapter.Status == 1
//                        && x.Lecture.Shapter.Course.Status == 1
//                        ).Count(),
//                };

//                int Count = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Include(x => x.Subject)
//                    .Include(x => x.AcademicSpecialization)
//                    .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status ==1
//                    && (IsFree ? x.IsFree == IsFree : true)
//                    && (IsDiscount ? x.IsDiscount == IsDiscount : true)
//                    && (SalesStatus > 0 ? x.SalesStatus == SalesStatus : true)
//                    && (AcademicLevelId > 0 ? x.AcademicSpecialization.AcademicLevelId == AcademicLevelId : true)
//                    && (AcademicSpecializationId > 0 ? x.AcademicSpecializationId == AcademicSpecializationId : true)
//                    && (SubjectId > 0 ? x.SubjectId == SubjectId : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Subject.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Include(x => x.Subject)
//                    .Include(x => x.AcademicSpecialization)
//                    .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status ==1
//                    && (IsFree ? x.IsFree == IsFree : true)
//                    && (IsDiscount ? x.IsDiscount == IsDiscount : true)
//                    && (SalesStatus > 0 ? x.SalesStatus == SalesStatus : true)
//                    && (AcademicLevelId > 0 ? x.AcademicSpecialization.AcademicLevelId == AcademicLevelId : true)
//                    && (AcademicSpecializationId > 0 ? x.AcademicSpecializationId == AcademicSpecializationId : true)
//                    && (SubjectId > 0 ? x.SubjectId == SubjectId : true)
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.Instructor.User.Name.Contains(Search.Trim()) ||
//                        x.Subject.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.Name.Contains(Search.Trim()) ||
//                        x.AcademicSpecialization.AcademicLevel.Name.Contains(Search.Trim())))
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
//                        InstructorImage = x.Instructor.User.Image,
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
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [AllowAnonymous]    
//        [HttpGet("Courses/GetCourseInfo")]
//        public IActionResult GetCourseInfo(long CourseId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Info = db.Shapters
//                    .Include(x => x.Course)
//                    .Where(x => x.Status == 1 && x.CourseId==CourseId
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.Number,
//                        x.Descriptions, 
//                        x.Image,
//                        x.CreatedOn,
//                        Lectures= x.Lectures.Where(k => k.ShapterId == x.Id && k.Status==1).Select(k => new
//                        {
//                            k.Id,
//                            k.Name,
//                            k.Rate,
//                            k.Descriptions,
//                            k.Number,
//                            k.CreatedOn
//                        }).OrderBy(k=>k.Number).ToList(),
//                        Exam=x.Exams.Where(k => k.ShapterId == x.Id && k.Status == 1).Select(k => new
//                        {
//                            k.Id,
//                            k.Name,
//                            k.Number,
//                            k.CountStudentPass,
//                            k.CountQuestions,
//                            k.CountStudent,
//                            k.Rate,
//                            k.Descriptions,
//                            k.CreatedOn
//                        }).OrderBy(k => k.Number).ToList(),
//                        Reviews = db.CoursesReview.Where(k=> k.Status!=9 && k.CourseId==x.CourseId).Select(k=>new
//                        {
//                            k.Message,
//                            k.Rate,
//                            k.CreatedOn,
//                            CreatedBy=db.Users.Where(s=>s.Id==k.CreatedBy).Select(s=> new
//                            {
//                                s.Name,
//                                s.Image
//                            }).SingleOrDefault()
//                        }).ToList(),
//                    }).OrderBy(x => x.Number).ToList();


//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }






//        //Instructors
//        [AllowAnonymous]
//        [HttpGet("Instructors/Get")]
//        public IActionResult GetInstructors(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.Instructors
//                    .Include(x => x.User)
//                    .Where(x => x.User.Status ==1
//                    && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim())))
//                    ).Count();

//                var Info = db.Instructors
//                     .Include(x => x.User)
//                     .Include(x => x.Courses)
//                    .Where(x => x.User.Status ==1
//                    && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                        x.User.Phone.Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.User.Name,
//                        x.Rate,
//                        x.User.Phone,
//                        x.User.Email,
//                        x.User.Image,
//                        x.Descriptions,
//                        x.FacebookProfile,
//                        CourseCount = x.Courses.Where(k => k.Status != 9).Count(),
//                        CreatedOn = x.User.CreatedOn
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [AllowAnonymous]
//        [HttpGet("Instructors/GetCourses")]
//        public IActionResult GetInstructorsCourses(long InstructorId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Include(x => x.Subject)
//                    .Include(x => x.AcademicSpecialization)
//                    .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status == 1 && x.InstructorId== InstructorId
//                    ).Count();

//                var Info = db.Courses
//                    .Include(x => x.Instructor)
//                    .Include(x => x.Instructor.User)
//                    .Include(x => x.Subject)
//                    .Include(x => x.AcademicSpecialization)
//                    .Include(x => x.AcademicSpecialization.AcademicLevel)
//                    .Where(x => x.Status == 1 && x.InstructorId== InstructorId
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
//                        InstructorImage = x.Instructor.User.Image,
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
//                    }).OrderByDescending(x => x.CreatedOn).ToList();


//                return Ok(new { info = Info, count = Count });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }






//        //Students
//        [HttpGet("Students/Get")]
//        public IActionResult GetLoginInfo()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Status == 1  && x.UserType == 60 && x.Id == userId).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Info = db.Students
//                    .Include(x => x.User)
//                    .Where(x => x.UserId == userId && x.User.Status == 1).Select(x => new
//                    {
//                        x.Id,
//                        x.User.Name,
//                        x.User.Phone,
//                        x.User.CreatedOn,
//                        x.User.LoginName,
//                        x.User.Email,
//                        x.User.LastLoginOn,
//                        x.User.About,
//                        x.User.Image,
//                        x.Points,
//                        x.Rate,
//                        StudentsCourses=x.StudentsCourses.Where(k=>k.Status==1).Count(),
//                        x.Wallet.Value,
//                        x.FirstName,
//                        x.FatherName,
//                        x.SirName,
//                    }).SingleOrDefault();

//                if (Info == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Students/GetStudentsCourses")]
//        public IActionResult GetStudentsCourses()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Status == 1 && x.UserType == 60 && x.Id == userId).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Student = db.Students.Where(x => x.UserId == userId).SingleOrDefault();
//                if(Student==null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);


//                var Info = db.StudentsCourses
//                    .Include(x => x.Course)
//                    .Include(x => x.Course.AcademicSpecialization)
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.Course.Subject)
//                    .Include(x => x.Course.Instructor)
//                    .Include(x => x.Course.Instructor.User)
//                    .Include(x => x.Course.Shapters)
//                    .Include(x => x.StudentsShapters)
//                    .Where(x => x.StudentId==Student.Id && x.Status == 1 && x.Course.Status==1)
//                    .Select(x => new {
//                        x.Id,
//                        x.CreatedOn,
//                        x.Course.Name,
//                        x.Course.Image,
//                        x.Course.Rate,
//                        x.Course.Price,
//                        x.Course.IsFree,
//                        Instructor = x.Course.Instructor.User.Name,
//                        InstructorImage = x.Course.Instructor.User.Image,
//                        AcademicSpecialization = x.Course.AcademicSpecialization.Name,
//                        AcademicLevel = x.Course.AcademicSpecialization.AcademicLevel.Name,
//                        Subject = x.Course.Subject.Name,
//                        ShaptersCount = x.Course.Shapters.Where(z => z.Status == 1).Count(),
//                        LecturesCount = db.Lectures.Include(k=>k.Shapter).Where(k=> k.Status==1 && k.Shapter.CourseId==x.CourseId).Count(),
//                        RateCount = x.Course.CoursesReview.Where(z => z.Status == 1).Count(),
//                        AverageInrollLectures = (double)x.StudentsLectures
//                            .Where(k => k.Status == 1 && k.Lecture.Shapter.CourseId == x.CourseId)
//                            .Count() /
//                            (db.Lectures
//                                .Include(k => k.Shapter)
//                                .Where(k => k.Status == 1 && k.Shapter.CourseId == x.CourseId)
//                                .Count() > 0 ?
//                                db.Lectures
//                                    .Include(k => k.Shapter)
//                                    .Where(k => k.Status == 1 && k.Shapter.CourseId == x.CourseId)
//                                    .Count() : 1)
//                    }).OrderByDescending(x => x.CreatedOn).ToList();

//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Students/GetStudentsCoursesRequests")]
//        public IActionResult GetStudentsCoursesRequests()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Status == 1 && x.UserType == 60 && x.Id == userId).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Student = db.Students.Where(x => x.UserId == userId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);


//                var Info = db.StudentsCoursesRequests
//                    .Include(x => x.Course)
//                    .Include(x => x.Course.AcademicSpecialization)
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.Course.Instructor)
//                    .Include(x => x.Course.Instructor.User)
//                    .Where(x => x.StudentId == Student.Id && x.Status != 1)
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.CreatedOn,
//                        x.Course.Name,
//                        x.Course.Image,
//                        x.Course.Rate,
//                        x.Course.Price,
//                        x.Course.IsFree,
//                        Instructor = x.Course.Instructor.User.Name,
//                        InstructorImage = x.Course.Instructor.User.Image,
//                        AcademicSpecialization = x.Course.AcademicSpecialization.Name,
//                        AcademicLevel = x.Course.AcademicSpecialization.AcademicLevel.Name,
//                        Subject = x.Course.Subject.Name,
//                        x.Status
//                    }).OrderByDescending(x => x.CreatedOn).ToList();

//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Students/GetStudentsExams")]
//        public IActionResult GetStudentsExams()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Status == 1 && x.UserType == 60 && x.Id == userId).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Student = db.Students.Where(x => x.UserId == userId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);


//                var Info = db.StudentsExams
//                    .Include(x => x.Exam)
//                    .Include(x => x.Exam.Shapter)
//                    .Include(x => x.Exam.Shapter.Course)
//                    .Include(x => x.Exam.Shapter.Course.Subject)
//                    .Include(x => x.Exam.Shapter.Course.Instructor)
//                    .Include(x => x.Exam.Shapter.Course.Instructor.User)
//                    .Include(x => x.Exam.Shapter.Course.AcademicSpecialization)
//                    .Include(x => x.Exam.Shapter.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.Exam.Shapter.Course.Subject)
//                    .Include(x => x.StudentCourse.Course)
//                    .Where(x => x.Status == 1 && x.Exam.Status == 1)
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.IsPassTest,
//                        x.Marck,
//                        x.CreatedOn,
//                        x.ExamId,
//                        ExmaInfo = new
//                        {
//                            x.ExamId,
//                            x.Exam.Name,
//                            x.Exam.Descriptions,
//                            x.Exam.CreatedOn,
//                            x.Exam.Marck,
//                            x.Exam.CountQuestions,
//                            ShapterName = x.Exam.Shapter.Name,
//                            ShapterDescriptions = x.Exam.Shapter.Descriptions,
//                            CourseName = x.Exam.Shapter.Course.Name,
//                            AcademicSpecialization = x.Exam.Shapter.Course.AcademicSpecialization.Name,
//                            Subjcet = x.Exam.Shapter.Course.Subject.Name,
//                            Instructor = x.Exam.Shapter.Course.Instructor.User.Name,
//                            InstructorImage = x.Exam.Shapter.Course.Instructor.User.Image,
//                            AcademicLevel = x.Exam.Shapter.Course.AcademicSpecialization.AcademicLevel.Name,
//                        }
//                    }).Distinct().OrderByDescending(x => x.CreatedOn).ToList();

//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Students/GetStudentsWallet")]
//        public IActionResult GetStudentsWallet()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Status == 1 && x.UserType == 60 && x.Id == userId).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Student = db.Students.Where(x => x.UserId == userId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Info = db.StudentsWalletTransactions
//                    .Include(x => x.Wallet)
//                    .Include(x => x.Wallet.Students)
//                    .Include(x => x.PaymentMethod)
//                    .Where(x => x.Wallet.Status != 9 && x.WalletId == Student.WalletId
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        PaymentMethod = x.PaymentMethod.Name,
//                        x.Value,
//                        x.ProcessType,
//                        x.CreatedOn,
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
//                       Subject = x.StudentCourse.Course.Subject.Name,
//                       AcademicLevel = x.StudentCourse.Course.AcademicSpecialization.AcademicLevel.Name,
//                       AcademicSpecialization = x.StudentCourse.Course.AcademicSpecialization.Name,
//                       Instructor = x.StudentCourse.Course.Instructor.User.Name,
//                       InstructorImage = x.StudentCourse.Course.Instructor.User.Image,
//                       x.Value,
//                       x.CoursePrice,
//                       x.Status,
//                       x.CreatedOn,
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
//                       x.CreatedOn,
//                   }).OrderByDescending(x => x.CreatedOn).ToList();

//                return Ok(new { Info, InfoPurchases, InfoTracker });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Students/GetStudentsCoursesReview")]
//        public IActionResult GetStudentsCoursesReview()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Status == 1 && x.UserType == 60 && x.Id == userId).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Student = db.Students.Where(x => x.UserId == userId).SingleOrDefault();
//                if (Student == null)
//                    return StatusCode(BackMessages.NotAuthroizedStatusCode, BackMessages.NotAuthorized);

//                var Info = db.CoursesReview
//                    .Include(x => x.Course)
//                    .Include(x => x.Course.Shapters)
//                    .Include(x => x.Course.AcademicSpecialization)
//                    .Include(x => x.Course.AcademicSpecialization.AcademicLevel)
//                    .Include(x => x.Course.Instructor)
//                    .Include(x => x.Course.Instructor.User)
//                    .Where(x => x.CreatedBy == Student.Id && x.Status == 1)
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.Course.Name,
//                        x.Course.Image,
//                        CourseRate=x.Course.Rate,
//                        x.Course.Price,
//                        x.Course.IsFree,
//                        Instructor = x.Course.Instructor.User.Name,
//                        InstructorImage = x.Course.Instructor.User.Image,
//                        AcademicSpecialization = x.Course.AcademicSpecialization.Name,
//                        AcademicLevel = x.Course.AcademicSpecialization.AcademicLevel.Name,
//                        Subject = x.Course.Subject.Name,
//                        x.Message,
//                        x.CreatedOn,
//                        x.Rate,
//                    }).OrderByDescending(x => x.CreatedOn).ToList();

//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }





//    }
//}