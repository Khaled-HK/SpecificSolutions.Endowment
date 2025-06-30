//using Common;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using NPOI.SS.Formula.Functions;
//using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel;
//using OfficeOpenXml;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Runtime.Intrinsics.X86;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.RegularExpressions;
//using Vue.Models;
//using Web.Services;
//using static Web.Services.Helper;

//namespace Management.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/admin/VoucherCards")]
//    public class VoucherCardsController : Controller
//    {
//        private Helper help;
//        IConfiguration configuration;
//        private readonly TraneemBetaContext db;

//        public VoucherCardsController(TraneemBetaContext context, IConfiguration iConfig)
//        {
//            this.db = context;
//            help = new Helper(iConfig, context);
//            this.configuration = iConfig;
//        }


//        //Student Finanial Info

//        public class BodyObject
//        {
//            public long? Id { get; set; }
//            public long DistributorsId { get; set; }
//            public string Name { get; set; }
//            public int CardCount { get; set; }
//            public int CardAmount { get; set; }
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

//                if (bodyObject.CardCount <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CardCountEmpty);

//                if (bodyObject.CardAmount <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CardAmountEmpty);

//                if (bodyObject.CardCount > int.Parse(configuration.GetSection("VoucherCard").GetSection("VoucherCardPackegeMaxCardCount").Value.ToString()))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CardGenerteMaxSize +
//                        configuration.GetSection("VoucherCard").GetSection("VoucherCardPackegeMaxCardCount").Value.ToString()
//                        + BackMessages.CardGenerteMaxSizeTitle
//                        );

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                var isExist = db.VoucherCardsPackages.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                var Company = db.Users.Where(x => x.Id == bodyObject.DistributorsId && x.Status != 9).SingleOrDefault();
//                if (Company == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CompanyNameEmpty);


//                string Key = this.help.GenerateFilePassword(bodyObject.CardCount, bodyObject.CardAmount);
//                string CardKey = configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString();

//                int counter = 0;
//                long MaxNumber = db.VoucherCards.Max(x => x.SerialNumber).GetValueOrDefault();
//                if (MaxNumber > 0)
//                {
//                    counter = int.Parse(MaxNumber.ToString().Substring(3));
//                    counter++;
//                }

//                List<VoucherCards> voucherCardsList = new List<VoucherCards>();
//                for (int i = 0; i < bodyObject.CardCount; i++)
//                {
//                    VoucherCards voucherCards = new VoucherCards();
//                    voucherCards.SerialNumber = this.help.GenerateSerialNumber(counter);
//                    voucherCards.VoucherNumber = this.help.EncryptString(this.help.GenerateVoucherNumber(int.Parse(configuration.GetSection("VoucherCard").GetSection("VoucherNumberLenght").Value.ToString())), CardKey);
//                    voucherCards.Amount = bodyObject.CardAmount;
//                    voucherCards.Level = 1;
//                    voucherCards.CreatedBy = userId;
//                    voucherCards.CreatedOn = DateTime.Now;
//                    voucherCards.Status = 1;
//                    voucherCardsList.Add(voucherCards);
//                    counter++;
//                }



//                VoucherCardsPackages row = new VoucherCardsPackages();
//                row.UserId = bodyObject.DistributorsId;
//                row.Name = bodyObject.Name;
//                row.SerialNumber = this.help.GeneratePackegeSerialNumberNumber(16);
//                row.CardCount = bodyObject.CardCount;
//                row.CardAmount = bodyObject.CardAmount;
//                row.ChargeingCount = 0;
//                row.RemindCount = bodyObject.CardCount;
//                row.FileSecretKey = this.help.EncryptString(Key, configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString());
//                row.ExpiryOn = DateTime.Now.AddYears(1);
//                row.VoucherCards = voucherCardsList;
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.VoucherCardsPackages.Add(row);


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = " توليد باقة جديدة ";
//                rowTrans.Controller = "VoucherCards";
//                rowTrans.NewObject = JsonConvert.SerializeObject(row, Formatting.None,
//                        new JsonSerializerSettings()
//                        {
//                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//                        });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessCardGenretOperations);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Get")]
//        public IActionResult Get(int pageNo, int pageSize, string Search,long DistributorsId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Waiting = db.VoucherCardsPackages.Where(x => x.Status == 1).Count(),
//                    WaitingToPruches = db.VoucherCardsPackages.Where(x => x.Status == 2).Count(),
//                    Active = db.VoucherCardsPackages.Where(x => x.Status == 3).Count(),
//                    Frozen = db.VoucherCardsPackages.Where(x => x.Status == 4).Count(),
//                };

//                var Count = db.VoucherCardsPackages
//                    .Include(x=>x.User)
//                    .Where(x => x.Status != 9
//                        && (DistributorsId > 0 ? x.UserId == DistributorsId : true)
//                        && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                            x.SerialNumber.Contains(Search.Trim()) ||
//                            x.User.Name.Contains(Search.Trim()) ||
//                            x.User.Phone.Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                ).Count();

//                var Info = db.VoucherCardsPackages
//                     .Include(x => x.User)
//                    .Where(x => x.Status != 9
//                        && (DistributorsId > 0 ? x.UserId == DistributorsId : true)
//                        && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                            x.SerialNumber.Contains(Search.Trim()) ||
//                            x.User.Name.Contains(Search.Trim()) ||
//                            x.User.Phone.Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        User = x.User.Name,
//                        x.UserId,
//                        x.User.Image,
//                        x.User.Phone,
//                        x.User.Email,
//                        PackgeCount=db.VoucherCardsPackages.Where(k=>k.UserId==x.UserId && k.Status!=9).Count(),
//                        PackgeCountFrozen=db.VoucherCardsPackages.Where(k=>k.UserId==x.UserId && k.Status==3).Count(),
//                        x.Name,
//                        x.CardCount,
//                        x.CardAmount,
//                        x.SerialNumber,
//                        x.ChargeingCount,
//                        x.RemindCount,
//                        SalesAvg = x.ChargeingCount > 0 ?
//                            (int) x.ChargeingCount  / x.CardCount : 0 ,
//                        FileSecretKey = this.help.DecryptString(x.FileSecretKey, configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString()),
//                        x.ExpiryOn,
//                        x.SolidOn,
//                        SolidBy= x.SolidBy.GetValueOrDefault()>0 ? db.Users.Where(k => k.Id == x.SolidBy).SingleOrDefault().Name : "",
//                        x.OpenOn,
//                        OpenBy = x.OpenBy.GetValueOrDefault()>0 ? db.Users.Where(k => k.Id == x.OpenBy).SingleOrDefault().Name : "",
//                        x.FreezeOn,
//                        FreezeBy= x.FreezeBy.GetValueOrDefault() > 0 ? db.Users.Where(k => k.Id == x.FreezeBy).SingleOrDefault().Name : "",
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy=db.Users.Where(k=>k.Id==x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Sale")]
//        public IActionResult Sale(long Id)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var VoucherCardsPackages = db.VoucherCardsPackages.Include(x => x.User).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
//                if (VoucherCardsPackages == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (VoucherCardsPackages.Status == 2)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeSolidBefore);

//                if (VoucherCardsPackages.Status != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeNotActive);




//                VoucherCardsPackages.SolidOn = DateTime.Now;
//                VoucherCardsPackages.SolidBy = userId;
//                VoucherCardsPackages.ExpiryOn = DateTime.Now.AddYears(1);
//                VoucherCardsPackages.Status = 2;



//                int CardsStatus = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Level != 1 && x.Status != 9).Count();
//                if (CardsStatus > 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CardsLevelNotOne);

//                CardsStatus = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Level == 1 && x.Status != 9).Count();
//                if (CardsStatus != VoucherCardsPackages.CardCount)
//                {
//                    this.FrozinPackge(Id, BackMessages.CardsLevelNotOne, userId);
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CardsLevelNotOne);
//                }



//                var Cards = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Status != 9).ToList();
//                if (Cards == null)
//                {
//                    this.FrozinPackge(Id, BackMessages.CardsEmpty, userId);
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CardsEmpty);
//                }

//                foreach (var item in Cards)
//                {
//                    item.Level = 2;
//                }



//                string Key = this.help.DecryptString(VoucherCardsPackages.FileSecretKey, configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString());

//                string CardKey = configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString();


//                var sb = new StringBuilder();
//                sb.AppendLine("Serial Number,Voucher Number,Amount");

//                foreach (var card in Cards)
//                {
//                    sb.AppendLine($"{card.SerialNumber},{this.help.DecryptString(card.VoucherNumber, CardKey)},{card.Amount}");
//                }

//                var csvBytes = Encoding.UTF8.GetBytes(this.help.EncryptString(sb.ToString(), Key));

//                var fileDownloadName = string.Format("{0}{1}{2}.csv", "VoucherCards", DateTime.Now.ToString("yyyyMMddHHmm"), VoucherCardsPackages.User.LoginName);


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Sole;
//                rowTrans.ItemId = VoucherCardsPackages.Id;
//                rowTrans.Descriptions = " بيع باقة جديدة ";
//                rowTrans.Controller = "VoucherCards";
//                rowTrans.NewObject = JsonConvert.SerializeObject(VoucherCardsPackages, Formatting.None,
//                        new JsonSerializerSettings()
//                        {
//                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//                        });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);


//                db.SaveChanges();
//                return File(csvBytes, "text/csv", fileDownloadName);

//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Freeze")]
//        public IActionResult Forze(long Id)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var VoucherCardsPackages = db.VoucherCardsPackages.Include(x => x.User).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
//                if (VoucherCardsPackages == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (VoucherCardsPackages.Status == 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeActive);


//                var PackecgeCard = db.VoucherCards.Where(x => x.PackageId == Id && x.Status != 9 && x.Level != 3).ToList();
//                if (PackecgeCard == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeDontHaveCard);


//                this.FrozinPackge(Id, "تجميد بطلب من الادارة ", userId);

//                VoucherCardsPackages.Status = 4;
//                VoucherCardsPackages.FreezeOn = DateTime.Now;
//                VoucherCardsPackages.FreezeBy = userId;


//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Froze;
//                rowTrans.ItemId = VoucherCardsPackages.Id;
//                rowTrans.Descriptions = " تحميد باقة";
//                rowTrans.Controller = "VoucherCards";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    VoucherCardsPackages.Id,
//                    VoucherCardsPackages.UserId,
//                    VoucherCardsPackages.Name,
//                    VoucherCardsPackages.ChargeingCount,
//                    VoucherCardsPackages.RemindCount,
//                    VoucherCardsPackages.CardCount,
//                    VoucherCardsPackages.CardAmount,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);


//                db.SaveChanges();

//                return Ok(BackMessages.SucessFrozenRequest);

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
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var VoucherCardsPackages = db.VoucherCardsPackages.Include(x => x.User).Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
//                if (VoucherCardsPackages == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (VoucherCardsPackages.Status != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeActiveNotEdit);


//                var PackecgeCard = db.VoucherCards.Where(x => x.PackageId == Id && x.Status != 9 && x.Level != 3).ToList();
//                if (PackecgeCard == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeDontHaveCard);

//                int CardsAcitveCheck = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Status != 9 && x.Level != 2).Count();
//                if (CardsAcitveCheck > 0)
//                {
//                    this.FrozinPackge(VoucherCardsPackages.Id, BackMessages.CardsLevelNotTwo, userId);
//                    return StatusCode(BackMessages.StatusCode, BackMessages.CardsLevelNotTwo);
//                }

//                var Cards = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Status != 9).ToList();
//                if (Cards==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeDontHaveCard);


//                foreach (var item in Cards)
//                {
//                    item.Status = 9;
//                }

//                VoucherCardsPackages.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = VoucherCardsPackages.Id;
//                rowTrans.Descriptions = " حذف باقة";
//                rowTrans.Controller = "VoucherCards";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    VoucherCardsPackages.Id,
//                    VoucherCardsPackages.UserId,
//                    VoucherCardsPackages.Name,
//                    VoucherCardsPackages.ChargeingCount,
//                    VoucherCardsPackages.RemindCount,
//                    VoucherCardsPackages.CardCount,
//                    VoucherCardsPackages.CardAmount,
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

//        public void FrozinPackge(long Id, string Resone, long UserId)
//        {
//            var VoucherCards = db.VoucherCards.Where(x => x.PackageId == Id && x.Level == 22 && x.Status != 9).ToList();
//            if (VoucherCards == null)
//                return;

//            foreach (var item in VoucherCards)
//            {
//                VoucherCardsFrozen row = new VoucherCardsFrozen();
//                row.VoucherCardId = item.Id;
//                row.Resone = Resone;
//                row.CreatedOn = DateTime.Now;
//                row.CreatedBy = UserId;
//                row.Status = 1;
//                db.VoucherCardsFrozen.Add(row);
//            }


//            TransactionsObject rowTrans = new TransactionsObject();
//            rowTrans.Operations = TransactionsType.Froze;
//            rowTrans.ItemId = Id;
//            rowTrans.Descriptions = " تجميد بيانات باقة  ";
//            rowTrans.Controller = "VoucherCards";
//            rowTrans.NewObject = JsonConvert.SerializeObject(new
//            {
//                Id,
//                Resone,
//                UserId
//            });
//            rowTrans.CreatedBy = UserId;
//            this.help.WriteTransactions(rowTrans);


//            db.SaveChanges();
//        }








//        //Cards
//        [HttpGet("Cards/Get")]
//        public IActionResult GetCards()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    CardCount = db.VoucherCards.Where(x => x.Status != 9).Count(),
//                    CardValue = db.VoucherCards.Where(x => x.Status != 9).Sum(x=>x.Amount).Value,
//                    ActiveCount = db.VoucherCards.Where(x => x.Status !=9 && x.Level==22).Count(),
//                    ActiveValue = db.VoucherCards.Where(x => x.Status != 9 && x.Level == 22).Sum(x => x.Amount).Value,
//                    ChargeCount = db.VoucherCards.Where(x => x.Status != 9 && x.Level == 3).Count(),
//                    ChargeValue = db.VoucherCards.Where(x => x.Status != 9 && x.Level == 3).Sum(x => x.Amount).Value,
//                    ChargeFaildCount = db.VoucherCardsChargingTryAttemp.Where(x => x.Status != 9).Count(),
                   
//                };


//                return Ok(new { Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Cards/GetCardInfoBySerialNumber")]
//        public IActionResult GetCardInfoBySerialNumber(string SerialNumber)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Info = db.VoucherCards
//                    .Include(x => x.Package)
//                    .Include(x => x.Package.User)
//                    .Where(x => x.Status != 9 && x.SerialNumber == long.Parse(SerialNumber))
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.SerialNumber,
//                        VoucherNumber = this.help.DecryptString(x.VoucherNumber, configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString()),
//                        x.Amount,
//                        x.Level,
//                        x.CreatedOn,
//                        Package=new
//                        {
//                            x.Package.Name,
//                            x.Package.SerialNumber,
//                            User = x.Package.User.Name,
//                            x.Package.User.Image,
//                            x.Package.User.Phone,
//                            x.Package.User.Email,
//                            x.Package.CardCount,
//                            x.Package.CardAmount,
//                            x.Package.ChargeingCount,
//                            x.Package.RemindCount,
//                            x.Package.ExpiryOn,
//                            x.Package.SolidOn,
//                            PackgeCount = db.VoucherCardsPackages.Where(k => k.UserId == x.Package.UserId && k.Status != 9).Count(),
//                            PackgeCountFrozen = db.VoucherCardsPackages.Where(k => k.UserId == x.Package.UserId && k.Status == 3).Count(),
//                            SolidBy = x.Package.SolidBy.GetValueOrDefault() > 0 ? db.Users.Where(k => k.Id == x.Package.SolidBy).SingleOrDefault().Name : "",
//                            x.Package.OpenOn,
//                            OpenBy = x.Package.OpenBy.GetValueOrDefault() > 0 ? db.Users.Where(k => k.Id == x.Package.OpenBy).SingleOrDefault().Name : "",
//                            x.Package.FreezeOn,
//                            FreezeBy = x.Package.FreezeBy.GetValueOrDefault() > 0 ? db.Users.Where(k => k.Id == x.Package.FreezeBy).SingleOrDefault().Name : "",
//                            x.Package.Status,
//                            x.Package.CreatedOn,
//                            CreatedBy = db.Users.Where(k => k.Id == x.Package.CreatedBy).SingleOrDefault().Name,
//                        }

//                    })
//                    .SingleOrDefault();

//                if (Info == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


//                var FillInfo = db.VoucherCardsCharging
//                    .Include(x => x.StudentWalletTransactions)
//                    .Include(x => x.StudentWalletTransactions.Wallet)
//                    .Where(x => x.VoucherCardId == Info.Id && x.Status != 9).Select(x => new
//                    {
//                        x.Id,
//                        x.CreatedOn,
//                        x.Chanel,
//                        x.VoucherCard.Amount,
//                        db.Students.Where(k => k.WalletId == x.StudentWalletTransactions.WalletId).SingleOrDefault().Name,
//                        db.Students.Where(k => k.WalletId == x.StudentWalletTransactions.WalletId).SingleOrDefault().User.Phone,
//                    }).SingleOrDefault();


//                return Ok(new { info = Info,FillInfo });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Cards/GetCardInfoByVoucherNumber")]
//        public IActionResult GetCardInfoByVoucherNumber(string VoucherNumber)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Info = db.VoucherCards
//                    .Include(x => x.Package)
//                    .Where(x => x.Status != 9 && x.VoucherNumber == this.help.EncryptString(VoucherNumber, configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString()) )
//                    .Select(x => new
//                    {
//                        x.Id,
//                        x.SerialNumber,
//                        VoucherNumber = this.help.DecryptString(x.VoucherNumber, configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString()),
//                        x.Amount,
//                        x.Level,
//                        x.Package.Name,
//                        x.CreatedOn,
//                        PackageSerialNumber = x.Package.SerialNumber,

//                    })
//                    .SingleOrDefault();

//                if (Info == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                if (Info.Level != 3)
//                    return Ok(new { info = Info });

//                var Card = db.VoucherCardsCharging
//                    .Include(x => x.StudentWalletTransactions)
//                    .Include(x => x.StudentWalletTransactions.Wallet)
//                    //.Include(x => x.StudentWalletTransactions.Wallet.Student)
//                    //.Include(x => x.StudentWalletTransactions.Wallet.Student.User)
//                    .Where(x => x.VoucherCardId == Info.Id && x.Status != 9).Select(x => new
//                    {
//                        x.Id,
//                        x.CreatedOn,
//                        x.Chanel,
//                        FullName = db.Students.Where(k => k.WalletId == x.StudentWalletTransactions.WalletId).SingleOrDefault().Name,
//                        Phone = db.Students.Where(k => k.WalletId == x.StudentWalletTransactions.WalletId).SingleOrDefault().User.Phone,
//                    }).SingleOrDefault();

//                if (Card == null)
//                    return Ok(new { info = Info });

//                var LastInfo = new
//                {
//                    Card.CreatedOn,
//                    Card.Chanel,
//                    Card.FullName,
//                    Card.Phone,
//                    Info.Id,
//                    Info.Name,
//                    CardCreatedOn = Info.CreatedOn,
//                    Info.SerialNumber,
//                    Info.VoucherNumber,
//                    Info.Amount,
//                    Info.Level,
//                    Info.PackageSerialNumber
//                };


//                return Ok(new { info = LastInfo });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }




//        //TryAttemp
//        [HttpGet("TryAttemp/Get")]
//        public IActionResult GetTryAttemp(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                var Count = db.VoucherCardsChargingTryAttemp
//                    .Include(x => x.User)
//                    .Where(x => x.Status != 9
//                        && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                            x.VoucherNumber.Contains(Search.Trim()) ||
//                            x.Resone.Contains(Search.Trim()) ||
//                            x.User.Phone.Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                ).Count();

//                var Info = db.VoucherCardsChargingTryAttemp
//                     .Include(x => x.User)
//                    .Where(x => x.Status != 9
//                        && (string.IsNullOrEmpty(Search) ? true : (x.User.Name.Contains(Search.Trim()) ||
//                            x.VoucherNumber.Contains(Search.Trim()) ||
//                            x.Resone.Contains(Search.Trim()) ||
//                            x.User.Phone.Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                            x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.User.Name,
//                        x.UserId,
//                        x.User.Image,
//                        x.User.Phone,
//                        x.Resone,
//                        x.ProcessStatus,
//                        x.VoucherNumber,
//                        x.Proseger,
//                        x.CreatedOn
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//                return Ok(new { info = Info, count = Count });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }



//        //ChartInfo
//        [HttpGet("Chart/Get")]
//        public IActionResult GetAllChartInfo()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);


//                int CardCount = db.VoucherCards.Where(x => x.Status != 9).Count();
//                int CardValue = db.VoucherCards.Where(x => x.Status != 9).Sum(x => x.Amount).Value;
//                int ActiveCount = db.VoucherCards.Where(x => x.Status != 9 && x.Level == 22).Count();
//                int ActiveValue = db.VoucherCards.Where(x => x.Status != 9 && x.Level == 22).Sum(x => x.Amount).Value;
//                int ChargeCount = db.VoucherCards.Where(x => x.Status != 9 && x.Level == 3).Count();
//                int ChargeValue = db.VoucherCards.Where(x => x.Status != 9 && x.Level == 3).Sum(x => x.Amount).Value;


//                int PackgeCount= db.VoucherCardsPackages.Where(x => x.Status != 9).Count();
//                int PackgeActive = db.VoucherCardsPackages.Where(x => x.Status==3).Count();
//                int PackgeWaiting = db.VoucherCardsPackages.Where(x => x.Status==1).Count();
//                int PackgeWaitingOpen = db.VoucherCardsPackages.Where(x => x.Status==2).Count();
//                int PackgeCardCount = db.VoucherCardsPackages.Where(x => x.Status == 3).Sum(x => x.CardCount).Value;
//                int PackgeCardAmount = db.VoucherCardsPackages.Where(x => x.Status == 3).Sum(x => x.CardAmount).Value;
//                int PackgeCardRemind = db.VoucherCardsPackages.Where(x => x.Status == 3).Sum(x => x.RemindCount).Value;
//                int PackgeCardCharge = db.VoucherCardsPackages.Where(x => x.Status == 3).Sum(x => x.ChargeingCount).Value;




//                // Calculate start of the week (Monday)
//                DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
//                DateTime endOfWeek = startOfWeek.AddDays(7);

//                DateTime startOfLastWeek = startOfWeek.AddDays(-7);
//                DateTime endOfLastWeek = endOfWeek.AddDays(-7);

//                // Charge Value added this week
//                int ChargeValueThisWeek = db.VoucherCardsCharging.Include(x=>x.VoucherCard).Where(x => x.Status != 9 && x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek).Sum(x=>x.VoucherCard.Amount).Value;
                
//                int ChargeValueThisLastWeek = db.VoucherCardsCharging.Include(x=>x.VoucherCard).Where(x => x.Status != 9 && x.CreatedOn >= startOfLastWeek && x.CreatedOn < endOfLastWeek).Sum(x=>x.VoucherCard.Amount).Value;

//                //Inrolled Chart This Week
//                int[] DailyChargeCounts = new int[8];
//                for (int i = 1; i < 8; i++)
//                {
//                    DateTime day = startOfWeek.AddDays(i); // Get each day of the week
//                    int count = db.VoucherCardsCharging.Include(x=>x.VoucherCard)
//                        .Where(x => x.Status != 9 && x.CreatedOn.Value.Date == day.Date)
//                        .Sum(x=>x.VoucherCard.Amount).Value;

//                    DailyChargeCounts[i] = count; // Store the count in the dictionary
//                }



//                // Count Value added this week
//                int SalesThisWeek = db.VoucherCardsCharging.Include(x => x.VoucherCard).Where(x => x.Status != 9 && x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek).Count();

//                int SalesThisLastWeek = db.VoucherCardsCharging.Include(x => x.VoucherCard).Where(x => x.Status != 9 && x.CreatedOn >= startOfLastWeek && x.CreatedOn < endOfLastWeek).Count();

//                //Inrolled Chart This Week
//                int[] DailySalesCounts = new int[8];
//                for (int i = 1; i < 8; i++)
//                {
//                    DateTime day = startOfWeek.AddDays(i); // Get each day of the week
//                    int count = db.VoucherCardsCharging.Include(x => x.VoucherCard)
//                        .Where(x => x.Status != 9 && x.CreatedOn.Value.Date == day.Date)
//                        .Count();

//                    DailySalesCounts[i] = count; // Store the count in the dictionary
//                }





//                // Calculate the start and end of the current year
//                DateTime startOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
//                DateTime endOfCurrentYear = startOfCurrentYear.AddYears(1);

//                int ChargeValueThisyear = db.VoucherCardsCharging.Include(x => x.VoucherCard).Where(x => x.Status != 9 && x.CreatedOn >= startOfCurrentYear && x.CreatedOn < endOfCurrentYear).Sum(x => x.VoucherCard.Amount).Value;

//                int SalesThisyear = db.VoucherCardsCharging.Include(x => x.VoucherCard).Where(x => x.Status != 9 && x.CreatedOn >= startOfCurrentYear && x.CreatedOn < endOfCurrentYear).Count();

//                // Initialize an array to hold sales counts for each month
//                int[] MonthlySalesCounts = new int[12]; // 12 months
//                int[] MonthlyChargeCounts = new int[12];
//                int[] MonthlySalesCountsSix = new int[6]; // 12 months
//                int[] MonthlyChargeCountsSix = new int[6]; // 12 months
//                // Loop through each month
//                for (int month = 1; month <= 12; month++)
//                {
//                    // Get the total count of sales for the current month
//                    int count = db.VoucherCardsCharging.Include(x => x.VoucherCard)
//                        .Where(x => x.Status != 9 && x.CreatedOn.Value.Month == month && x.CreatedOn.Value.Year == startOfCurrentYear.Year)
//                        .Count();
                    
//                    int charge = db.VoucherCardsCharging.Include(x => x.VoucherCard)
//                        .Where(x => x.Status != 9 && x.CreatedOn.Value.Month == month && x.CreatedOn.Value.Year == startOfCurrentYear.Year)
//                        .Sum(x=>x.VoucherCard.Amount).Value;

//                    if(month<=6)
//                    {
//                        MonthlySalesCountsSix[month - 1] = count; // Store the count in the array
//                        MonthlyChargeCountsSix[month - 1] = charge;
//                    }

//                    MonthlySalesCounts[month - 1] = count; // Store the count in the array
//                    MonthlyChargeCounts[month - 1] = charge; // Store the count in the array
//                }







//                //Top Five Distrputed Watching 
//                var TopFiveDistrputedSales = db.VoucherCardsCharging
//                    .Include(x => x.VoucherCard) 
//                    .Include(x => x.VoucherCard.Package) 
//                    .Include(x => x.VoucherCard.Package.User) 
//                    .Where(x => x.Status != 9) 
//                    .GroupBy(x => new { 
//                        x.VoucherCard.Package.UserId,
//                        x.VoucherCard.Package.User.Name,
//                        x.VoucherCard.Package.User.Phone,
//                        x.VoucherCard.Package.User.Image,
//                    }) 
//                    .Select(g => new
//                    {
//                        g.Key.Name,
//                        g.Key.Phone,
//                        g.Key.Image,
//                        Count = g.Count() // Count of views
//                    })
//                    .OrderByDescending(x => x.Count) // Order by count descending
//                    .Take(6) // Get top 5
//                    .ToList();



//                //Top Five Distrputed Watching 
//                var TopFiveDistrputedCharge = db.VoucherCards
//                    .Include(x => x.Package) 
//                    .Include(x => x.Package.User) 
//                    .Where(x => x.Status != 9 && x.Level==3) 
//                    .GroupBy(x => new { 
//                        x.Package.UserId,
//                        x.Package.User.Name,
//                        x.Package.User.Phone,
//                        x.Package.User.Image,
//                    }) 
//                    .Select(g => new
//                    {
//                        g.Key.Name,
//                        g.Key.Phone,
//                        g.Key.Image,
//                        Cash = g.Sum(x=>x.Amount).Value
//                    })
//                    .OrderByDescending(x => x.Cash) 
//                    .Take(6) // Get top 5
//                    .ToList();
                
//                //Top Five Distrputed Watching 
//                var TopFiveInventory = db.VoucherCardsPackages
//                    .Include(x => x.User) 
//                    .Where(x => x.Status ==3) 
//                    .GroupBy(x => new { 
//                        x.UserId,
//                        x.User.Name,
//                        x.User.Phone,
//                        x.User.Image,
//                    }) 
//                    .Select(g => new
//                    {
//                        g.Key.Name,
//                        g.Key.Phone,
//                        g.Key.Image,
//                        Count = g.Sum(x => x.RemindCount).Value // Count of views
//                    })
//                    .OrderByDescending(x => x.Count) // Order by count descending
//                    .Take(6) // Get top 5
//                    .ToList();




//                var Info = new
//                {
//                    CardCount,
//                    CardValue,
//                    ActiveCount,
//                    ActiveValue,
//                    ChargeCount,
//                    ChargeValue,

//                    PackgeCount,
//                    PackgeActive,
//                    PackgeWaiting,
//                    PackgeWaitingOpen,
//                    PackgeCardCount,
//                    PackgeCardAmount,
//                    PackgeCardRemind,
//                    PackgeCardCharge,

//                    ChargeValueThisWeek,
//                    ChargeValueThisLastWeek,
//                    DailyChargeCounts,

//                    SalesThisWeek,
//                    SalesThisLastWeek,
//                    DailySalesCounts,


//                    ChargeValueThisyear,
//                    SalesThisyear,
//                    MonthlySalesCounts,
//                    MonthlyChargeCounts,
//                    MonthlySalesCountsSix,
//                    MonthlyChargeCountsSix,


//                    TopFiveDistrputedSales,
//                    TopFiveDistrputedCharge,
//                    TopFiveInventory,

//            };


//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }





//        //Distributors
//        public partial class DistributorsBodyObject
//        {
//            public long? Id { get; set; }
//            public string Name { get; set; }
//            public string LoginName { get; set; }
//            public string Email { get; set; }
//            public string Phone { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//        }
//        [HttpGet("Distributors/Get")]
//        public IActionResult GetDistributors(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Users.Where(x => x.Status != 9 && x.UserType==50).Count(),
//                    Active = db.Users.Where(x => x.Status == 1 && x.UserType == 50).Count(),
//                    NotActive = db.Users.Where(x => x.Status == 2 && x.UserType == 50).Count(),
//                    Deleted = db.Users.Where(x => x.Status == 9 && x.UserType == 50).Count(),
//                };

//                var Count = db.Users.Where(x => x.Status != 9  && x.UserType == 50
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Phone.Contains(Search.Trim()) ||
//                        x.Email.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                ).Count();

//                var Info = db.Users.Where(x => x.Status != 9  && x.UserType == 50
//                        && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Phone.Contains(Search.Trim()) ||
//                        x.Email.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                ).Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    x.LoginName,
//                    x.Email,
//                    x.UserType,
//                    PackegeCount=db.VoucherCardsPackages.Where(k=>k.UserId==x.Id && x.Status!=9).Count(),
//                    CardCount= db.VoucherCardsPackages.Where(k => k.UserId == x.Id && x.Status != 9).Sum(k=>k.CardCount).Value,
//                    CardValue= db.VoucherCardsPackages.Where(k => k.UserId == x.Id && x.Status != 9).Sum(k=>k.CardAmount * k.CardCount).Value,
//                    CardCharge= db.VoucherCardsPackages.Where(k => k.UserId == x.Id && x.Status != 9).Sum(k=>k.ChargeingCount).Value,
//                    CardRemind= db.VoucherCardsPackages.Where(k => k.UserId == x.Id && x.Status != 9).Sum(k=>k.RemindCount).Value,
//                    x.Image,
//                    x.Phone,
//                    x.LastLoginOn,
//                    x.Status,
//                    x.CreatedOn,
//                    CreatedBy=db.Users.Where(k=>k.Id==x.CreatedBy).SingleOrDefault().Name
//                }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count,Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Distributors/GetAll")]
//        public IActionResult GetAllDistributors()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Info = db.Users.Where(x => x.Status != 9  && x.UserType == 50).Select(x => new
//                {
//                    x.Id,
//                    x.Name,
//                    x.LoginName,
//                }).OrderByDescending(x => x.Name).ToList();

//                return Ok(new { info = Info });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Distributors/Add")]
//        public IActionResult AddDistributors([FromBody] DistributorsBodyObject bodyObject)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if(User.UserType!=1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (string.IsNullOrEmpty(bodyObject.Email))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

//                if(!this.help.IsValidEmail(bodyObject.Email))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

//                if (string.IsNullOrWhiteSpace(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//                if (!this.help.IsValidPhone(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);


//                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                IsExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

//                IsExist = db.Users.Where(x => x.Status != 9 && x.Email == bodyObject.Email).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);

//                Users row = new Users();
//                row.Name = bodyObject.Name;
//                row.LoginName = bodyObject.LoginName;
//                row.Email = bodyObject.Email;
//                string Password = this.help.GenreatePass();
//                row.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);
//                if (string.IsNullOrEmpty(bodyObject.Image))
//                {
//                    row.Image = "/Uploads/User.jpg";
//                }
//                else
//                {
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                }
//                row.UserType = 50;
//                row.Phone = bodyObject.Phone;
//                row.LoginTryAttempts = 0;
//                row.CreatedBy = userId;
//                row.CreatedOn = DateTime.Now;
//                row.Status = 1;
//                db.Users.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "VoucherCard/Distributors";
//                rowTrans.NewObject = JsonConvert.SerializeObject(bodyObject);
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);


//                db.SaveChanges();
//                return Ok(BackMessages.SucessAddOperations +" " + "كلمة المرور الخاصة بالموزع : " + Password);


//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("Distributors/Edit")]
//        public IActionResult EditDistributors([FromBody] DistributorsBodyObject bodyObject)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Id == userId && x.Status != 9).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (User.UserType != 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrWhiteSpace(bodyObject.LoginName))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (string.IsNullOrEmpty(bodyObject.Name))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameEmpty);

//                if (string.IsNullOrEmpty(bodyObject.Email))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailEmpty);

//                if (!this.help.IsValidEmail(bodyObject.Email))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailNotValid);

//                if (string.IsNullOrWhiteSpace(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneEmpty);

//                if (!this.help.IsValidPhone(bodyObject.Phone))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneNotValid);

                
//                var row = db.Users.Where(x => x.Status != 9 && x.UserType == 50 && x.Id == bodyObject.Id).SingleOrDefault();
//                if(row==null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);
                
//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Image,
//                    row.LoginName,
//                    row.Email,
//                    row.Phone,
//                    row.CreatedBy,
//                    row.CreatedOn,
//                    row.Status,
//                });

//                var IsExist = db.Users.Where(x => x.LoginName == bodyObject.LoginName && x.Status != 9 && x.Id!=bodyObject.Id).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                IsExist = db.Users.Where(x => x.Status != 9 && x.Phone == bodyObject.Phone && x.Id != bodyObject.Id).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PhoneExist);

//                IsExist = db.Users.Where(x => x.Status != 9 && x.Email == bodyObject.Email && x.Id != bodyObject.Id).SingleOrDefault();
//                if (IsExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmailExist);


//                row.Name = bodyObject.Name;
//                row.LoginName = bodyObject.LoginName;
//                row.Email = bodyObject.Email;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
//                row.Phone = bodyObject.Phone;

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.Descriptions = "تعديل بيانات  ";
//                rowTrans.Controller = "VoucherCard/Distributors";
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

//        [HttpPost("{Id}/Distributors/ResetPassword")]
//        public IActionResult ResetPasswordDistributors(long Id)
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


//                var row = db.Users.Where(x => x.Id == Id && x.Status != 9 && x.UserType==50).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                string Password = this.help.GenreatePass();
//                row.Password = Security.ComputeHash(Password, HashAlgorithms.SHA512, null);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Reset;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تهيئة كلمة المرور  ";
//                rowTrans.Controller = "VoucherCard/Distributors";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Phone,
//                    row.CreatedOn,
//                    row.CreatedBy,
//                    row.Status
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SucessResetOperations + " "+ "كلمة المرور الجديدة : " + Password);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Distributors/ChangeStatus")]
//        public IActionResult ChangeStatusDistributors(long Id)
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

//                var row = db.Users
//                    .Where(x => x.Id == Id && x.Status != 9 && x.UserType == 50).SingleOrDefault();

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
//                rowTrans.Operations = TransactionsType.CahngeStatus;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = " تغير حالة    ";
//                rowTrans.Controller = "VoucherCard/Distributors";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Phone,
//                    row.CreatedBy,
//                    row.CreatedOn,
//                });
//                rowTrans.CreatedBy = userId;
//                this.help.WriteTransactions(rowTrans);

//                db.SaveChanges();
//                return Ok(BackMessages.SuccessChangeStatus);
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Distributors/Delete")]
//        public IActionResult DeleteDistributors(long Id)
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

//                var row = db.Users
//                    .Where(x => x.Id == Id && x.Status != 9 && x.UserType==50).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);


//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "VoucherCard/Distributors";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Phone,
//                    row.CreatedBy,
//                    row.CreatedOn,
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







//        public class BodyObjectActivePackge
//        {
//            public string FileSecretKey { get; set; }
//            public string SerialNumber { get; set; }
//            public string fileBase64 { get; set; }
//        }

//        [HttpPost("Distributors/ActivePackge")]
//        public IActionResult ActivePackge([FromBody] BodyObjectActivePackge bodyObject)
//        {
//            try
//            {
//                if (bodyObject == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EmptyBodyObject);

//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var User = db.Users.Where(x => x.Id == userId && x.Status != 9 && x.UserType == 50 ).SingleOrDefault();
//                if (User == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                if (string.IsNullOrWhiteSpace(bodyObject.SerialNumber))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.SerialNumberEmpty);

//                if (string.IsNullOrWhiteSpace(bodyObject.FileSecretKey))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.FileSecretKeyEmpty);

//                if (string.IsNullOrEmpty(bodyObject.fileBase64))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.EnterFile);

//                var VoucherCardsPackages = db.VoucherCardsPackages
//                    .Include(x => x.User)
//                    .Where(x => x.UserId == userId && x.SerialNumber == bodyObject.SerialNumber && x.Status != 9).SingleOrDefault();
//                if (VoucherCardsPackages == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeNotFound);

//                if (VoucherCardsPackages.CreatedOn < DateTime.Now.Date.AddMonths(-1))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeOpenExpiry);

//                if (VoucherCardsPackages.UserId != userId)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.DontHavePackgePermisine);

//                if (VoucherCardsPackages.Status == 1)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeNotFound);

//                if (VoucherCardsPackages.Status == 4)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegeFrozin);

//                if (bodyObject.FileSecretKey != this.help.DecryptString(VoucherCardsPackages.FileSecretKey, configuration.GetSection("VoucherCard").GetSection("VoucherSecretKey").Value.ToString()))
//                    return StatusCode(BackMessages.StatusCode, BackMessages.PackegePasswordNotCorrect);

//                var isOpenBefore = db.VoucherCardsPackagesFileRead.Where(x => x.Status == 1 && x.PackageId == VoucherCardsPackages.Id).ToList();
//                if (isOpenBefore != null && isOpenBefore.Count() > 3)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.YouOpenTheFileMoreThen3Time);

//                if (VoucherCardsPackages.Status == 3)
//                {
//                    var Cards = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Status != 9 && x.Level == 22).ToList();
//                    if (Cards == null)
//                        return StatusCode(BackMessages.StatusCode, BackMessages.PackegeDontHaveCard1);


//                    VoucherCardsPackagesFileRead row = new VoucherCardsPackagesFileRead();
//                    row.PackageId = VoucherCardsPackages.Id;
//                    row.Status = 1;
//                    row.CreatedOn = DateTime.Now;
//                    row.CreatedBy = userId;
//                    db.VoucherCardsPackagesFileRead.Add(row);



//                    TransactionsObject rowTrans = new TransactionsObject();
//                    rowTrans.Operations = TransactionsType.Sole;
//                    rowTrans.ItemId = VoucherCardsPackages.Id;
//                    rowTrans.Descriptions = " فتح مرة أخري للباقة من قبل الموزع";
//                    rowTrans.Controller = "Distributors/VoucherCards";
//                    rowTrans.NewObject = JsonConvert.SerializeObject(VoucherCardsPackages, Formatting.None,
//                            new JsonSerializerSettings()
//                            {
//                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//                            });
//                    rowTrans.CreatedBy = userId;
//                    this.help.WriteTransactions(rowTrans);


//                    string prefixToRemove = "data:text/csv;base64,";
//                    bodyObject.fileBase64 = bodyObject.fileBase64.Substring(prefixToRemove.Length);


//                    byte[] encryptedBytes = Convert.FromBase64String(bodyObject.fileBase64);
//                    string decodedCsv = Encoding.UTF8.GetString(encryptedBytes);
//                    string decryptedBytes = this.help.DecryptString(decodedCsv, bodyObject.FileSecretKey);

//                    byte[] csvBytes = Encoding.UTF8.GetBytes(decryptedBytes);

//                    VoucherCardsPackages.OpenBy = userId;
//                    VoucherCardsPackages.OpenOn = DateTime.Now;

//                    db.SaveChanges();

//                    return File(csvBytes, "text/csv", string.Format("{0}{1}{2}.csv", "VoucherCards", DateTime.Now.ToString("yyyyMMddHHmm"), VoucherCardsPackages.User.LoginName));
//                }
//                else
//                {

//                    var CardsAcitve = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Status != 9 && x.Level != 2).Count();
//                    if (CardsAcitve > 0)
//                    {
//                        this.FrozinPackge(VoucherCardsPackages.Id, BackMessages.CardsLevelNotTwo, userId);
//                        return StatusCode(BackMessages.StatusCode, BackMessages.CardsLevelNotTwo);
//                    }


//                    var Cards = db.VoucherCards.Where(x => x.PackageId == VoucherCardsPackages.Id && x.Status != 9 && x.Level == 2).ToList();
//                    if (Cards == null)
//                        return StatusCode(BackMessages.StatusCode, BackMessages.PackegeDontHaveCard1);

//                    foreach (var item in Cards)
//                    {
//                        item.Level = 22;
//                    }

//                    VoucherCardsPackages.Status = 3;

//                    VoucherCardsPackagesFileRead row = new VoucherCardsPackagesFileRead();
//                    row.PackageId = VoucherCardsPackages.Id;
//                    row.Status = 1;
//                    row.CreatedOn = DateTime.Now;
//                    row.CreatedBy = userId;
//                    db.VoucherCardsPackagesFileRead.Add(row);


//                    TransactionsObject rowTrans = new TransactionsObject();
//                    rowTrans.Operations = TransactionsType.Sole;
//                    rowTrans.ItemId = VoucherCardsPackages.Id;
//                    rowTrans.Descriptions = " فتح الباقة من قبل الموزع";
//                    rowTrans.Controller = "Distributors/VoucherCards";
//                    rowTrans.NewObject = JsonConvert.SerializeObject(VoucherCardsPackages, Formatting.None,
//                            new JsonSerializerSettings()
//                            {
//                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//                            });
//                    rowTrans.CreatedBy = userId;
//                    this.help.WriteTransactions(rowTrans);



//                    string prefixToRemove = "data:text/csv;base64,";
//                    bodyObject.fileBase64 = bodyObject.fileBase64.Substring(prefixToRemove.Length);


//                    byte[] encryptedBytes = Convert.FromBase64String(bodyObject.fileBase64);
//                    string decodedCsv = Encoding.UTF8.GetString(encryptedBytes);
//                    string decryptedBytes = this.help.DecryptString(decodedCsv, bodyObject.FileSecretKey);

//                    byte[] csvBytes = Encoding.UTF8.GetBytes(decryptedBytes);

//                    VoucherCardsPackages.OpenBy = userId;
//                    VoucherCardsPackages.OpenOn = DateTime.Now;

//                    db.SaveChanges();

//                    return File(csvBytes, "text/csv", string.Format("{0}{1}{2}.csv", "VoucherCards", DateTime.Now.ToString("yyyyMMddHHmm"), VoucherCardsPackages.User.LoginName));

//                }

//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, BackMessages.UnkownError + BackMessages.ErorFile);
//                //return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Distributors/Packages/Get")]
//        public IActionResult GetDistributorsPackages(int pageNo, int pageSize)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
//                if (user == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Count = db.VoucherCardsPackages.Where(x => x.Status != 9 && x.Status >= 2 && x.UserId == userId).Count();

//                var Info = db.VoucherCardsPackages
//                    .Include(x => x.User)
//                    .Where(x => x.Status != 9 && x.Status >= 2 && x.UserId == userId).Select(x => new
//                    {
//                        x.Id,
//                        User = x.User.Name,
//                        x.UserId,
//                        x.Name,
//                        x.CardCount,
//                        x.CardAmount,
//                        x.SerialNumber,
//                        x.ExpiryOn,
//                        x.SolidOn,
//                        x.Status,
//                        x.CreatedOn
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

//                return Ok(new { info = Info, count = Count });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }






//    }
//}