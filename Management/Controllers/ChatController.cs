//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using NPOI.POIFS.Properties;
//using NPOI.SS.Formula.Functions;
//using System;
//using System.Linq;
//using Vue.Models;
//using Web.Services;
//using static Web.Services.Helper;

//namespace Management.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/admin/Chat")]
//    public class ChatController : Controller
//    {
//        private Helper help;

//        private readonly TraneemBetaContext db;

//        public ChatController(TraneemBetaContext context, IConfiguration iConfig)
//        {
//            this.db = context;
//            help = new Helper(iConfig, context);
//        }



        
//        public partial class BodyObject
//        {
//            public long? Id { get; set; }
//            public string Name { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//            public string Descriptions { get; set; }
//        }
        
//        public partial class BodyObjectWithParent
//        {
//            public long? Id { get; set; }
//            public short ParentId { get; set; }
//            public string Name { get; set; }
//            public string Image { get; set; }
//            public string ImageName { get; set; }
//            public string Descriptions { get; set; }
//        }










//        //PaymentMethods
//        [HttpGet("PaymentMethods/Get")]
//        public IActionResult GetPaymentMethods(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.PaymentMethods.Where(x => x.Status != 9).Count(),
//                    Active = db.PaymentMethods.Where(x => x.Status == 1).Count(),
//                    NotActive = db.PaymentMethods.Where(x => x.Status == 2).Count(),
//                    Deleted = db.PaymentMethods.Where(x => x.Status == 9).Count(),
//                };

//                int Count = db.PaymentMethods
//                    .Where(x => x.Status != 9
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.PaymentMethods
//                    .Where(x => x.Status != 9
//                     && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
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
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("PaymentMethods/GetAll")]
//        public IActionResult GetAllPaymentMethods()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.PaymentMethods
//                    .Where(x => x.Status == 1).Count();
//                var Info = db.PaymentMethods
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

//        [HttpPost("PaymentMethods/Add")]
//        public IActionResult AddPaymentMethods([FromBody] BodyObject bodyObject)
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


//                var isExist = db.PaymentMethods.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                PaymentMethods row = new PaymentMethods();
//                row.Name = bodyObject.Name;
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
//                db.PaymentMethods.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "PaymentMethods/Dictionaries";
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
        
//        [HttpPost("PaymentMethods/Edit")]
//        public IActionResult EditPaymentMethods([FromBody] BodyObject bodyObject)
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

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.PaymentMethods.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new { 
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });

//                var isExist = db.PaymentMethods.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "PaymentMethods/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/PaymentMethods/Delete")]
//        public IActionResult DeletePaymentMethods(long Id)
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


//                var row = db.PaymentMethods.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "PaymentMethods/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/PaymentMethods/ChangeStatus")]
//        public IActionResult ChangeStatusPaymentMethods(long Id)
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


//                var row = db.PaymentMethods.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
//                rowTrans.Controller = "PaymentMethods/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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







//        //Subjects
//        [HttpGet("Subjects/Get")]
//        public IActionResult GetSubjects(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Subjects.Where(x => x.Status != 9).Count(),
//                    Active = db.Subjects.Where(x => x.Status == 1).Count(),
//                    NotActive = db.Subjects.Where(x => x.Status == 2).Count(),
//                    Deleted = db.Subjects.Where(x => x.Status == 9).Count(),
//                };

//                int Count = db.Subjects
//                    .Where(x => x.Status != 9
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())))
//                    ).Count();
//                var Info = db.Subjects
//                    .Where(x => x.Status != 9
//                     && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
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
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count , Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("Subjects/GetAll")]
//        public IActionResult GetAllSubjects()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.Subjects
//                    .Where(x => x.Status == 1).Count();
//                var Info = db.Subjects
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

//        [HttpPost("Subjects/Add")]
//        public IActionResult AddSubjects([FromBody] BodyObject bodyObject)
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


//                var isExist = db.Subjects.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                Subjects row = new Subjects();
//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                if(string.IsNullOrEmpty(bodyObject.Image))
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
//                db.Subjects.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "Subjects/Dictionaries";
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

//        [HttpPost("Subjects/Edit")]
//        public IActionResult EditSubjects([FromBody] BodyObject bodyObject)
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

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.Subjects.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });

//                var isExist = db.Subjects.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);

//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "Subjects/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/Subjects/Delete")]
//        public IActionResult DeleteSubjects(long Id)
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


//                var row = db.Subjects.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Subjects/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/Subjects/ChangeStatus")]
//        public IActionResult ChangeStatusSubjects(long Id)
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


//                var row = db.Subjects.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
//                rowTrans.Controller = "Subjects/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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







//        //AcademicLevels
//        [HttpGet("AcademicLevels/Get")]
//        public IActionResult GetAcademicLevels(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);



//                var Statistics = new
//                {
//                    Count = db.AcademicLevels.Where(x => x.Status != 9).Count(),
//                    Active=db.AcademicLevels.Where(x=>x.Status==1).Count(),
//                    NotActive=db.AcademicLevels.Where(x=>x.Status==2).Count(),
//                    Deleted=db.AcademicLevels.Where(x=>x.Status==9).Count(),
//                };

//                int Count = db.AcademicLevels
//                    .Where(x => x.Status != 9
//                    && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
//                    ))
//                    ).Count();
//                var Info = db.AcademicLevels
//                    .Where(x => x.Status != 9
//                     && (string.IsNullOrEmpty(Search) ? true : (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
//                     ))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.Image,
//                        x.Descriptions,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("AcademicLevels/GetAll")]
//        public IActionResult GetAllAcademicLevels()
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.AcademicLevels
//                    .Where(x => x.Status == 1).Count();
//                var Info = db.AcademicLevels
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

//        [HttpPost("AcademicLevels/Add")]
//        public IActionResult AddAcademicLevels([FromBody] BodyObject bodyObject)
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


//                var isExist = db.AcademicLevels.Where(x => x.Name == bodyObject.Name && x.Status != 9).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                AcademicLevels row = new AcademicLevels();
//                row.Name = bodyObject.Name;
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
//                db.AcademicLevels.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "AcademicLevels/Dictionaries";
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

//        [HttpPost("AcademicLevels/Edit")]
//        public IActionResult EditAcademicLevels([FromBody] BodyObject bodyObject)
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

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.AcademicLevels.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });

//                var isExist = db.AcademicLevels.Where(x => x.Name == bodyObject.Name && x.Status != 9 && x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);
               


//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "AcademicLevels/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/AcademicLevels/Delete")]
//        public IActionResult DeleteAcademicLevels(long Id)
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


//                var row = db.AcademicLevels.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "AcademicLevels/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/AcademicLevels/ChangeStatus")]
//        public IActionResult ChangeStatusAcademicLevels(long Id)
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


//                var row = db.AcademicLevels.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
//                rowTrans.Controller = "AcademicLevels/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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





//        //AcademicSpecializations
//        [HttpGet("AcademicSpecializations/Get")]
//        public IActionResult GetAcademicSpecializations(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.AcademicSpecializations.Where(x => x.Status != 9).Count(),
//                    Active = db.AcademicSpecializations.Where(x => x.Status == 1).Count(),
//                    NotActive = db.AcademicSpecializations.Where(x => x.Status == 2).Count(),
//                    Deleted = db.AcademicSpecializations.Where(x => x.Status == 9).Count(),
//                };

//                int Count = db.AcademicSpecializations
//                    .Include(x=>x.AcademicLevel)
//                    .Where(x => x.Status != 9 
//                    && (string.IsNullOrEmpty(Search) ? true : 
//                        (x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) || 
//                        x.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
//                        ))
//                    ).Count();
//                var Info = db.AcademicSpecializations.Include(x=>x.AcademicLevel)
//                    .Where(x => x.Status != 9
//                       && (string.IsNullOrEmpty(Search) ? true : (
//                        x.Name.Contains(Search.Trim()) ||
//                        x.Descriptions.Contains(Search.Trim()) ||
//                        x.AcademicLevel.Name.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
//                        ))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Name,
//                        x.AcademicLevelId,
//                        AcademicLevel=x.AcademicLevel.Name,
//                        ParentId=x.AcademicLevelId,
//                        x.Image,
//                        x.Descriptions,
//                        x.Status,
//                        x.CreatedOn,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count , Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpGet("AcademicSpecializations/GetAll")]
//        public IActionResult GetAllAcademicSpecializations(short AcademicLevelId)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                int Count = db.AcademicSpecializations
//                    .Where(x => x.Status ==1 && x.AcademicLevelId==AcademicLevelId).Count();
//                var Info = db.AcademicSpecializations
//                    .Where(x => x.Status ==1 && x.AcademicLevelId==AcademicLevelId ).Select(x => new
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

//        [HttpPost("AcademicSpecializations/Add")]
//        public IActionResult AddAcademicSpecializations([FromBody] BodyObjectWithParent bodyObject)
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


//                var isExist = db.AcademicSpecializations.Where(x => x.Status != 9 &&
//                x.Name == bodyObject.Name &&
//                x.AcademicLevelId==bodyObject.ParentId
//                ).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                AcademicSpecializations row = new AcademicSpecializations();
//                row.AcademicLevelId = bodyObject.ParentId;
//                row.Name = bodyObject.Name;
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
//                db.AcademicSpecializations.Add(row);

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Add;
//                rowTrans.Descriptions = "إضافة بيانات  ";
//                rowTrans.Controller = "AcademicSpecializations/Dictionaries";
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

//        [HttpPost("AcademicSpecializations/Edit")]
//        public IActionResult EditAcademicSpecializations([FromBody] BodyObjectWithParent bodyObject)
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

//                TransactionsObject rowTrans = new TransactionsObject();
//                var row = db.AcademicSpecializations.Where(x => x.Id == bodyObject.Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                rowTrans.OldObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
//                    row.Descriptions,
//                    row.Image,
//                    row.Status
//                });

//                var isExist = db.AcademicSpecializations.Where(x =>  x.Status != 9 &&
//                x.Name == bodyObject.Name &&
//                x.AcademicLevelId == bodyObject.ParentId &&
//                x.Id != bodyObject.Id).SingleOrDefault();
//                if (isExist != null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NameExist);

//                row.AcademicLevelId = bodyObject.ParentId;
//                row.Name = bodyObject.Name;
//                row.Descriptions = bodyObject.Descriptions;
//                if (!string.IsNullOrEmpty(bodyObject.Image))
//                    row.Image = this.help.UploadFile(bodyObject.ImageName, bodyObject.Image);


//                rowTrans.Operations = TransactionsType.Edit;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تعديل بيانات   ";
//                rowTrans.Controller = "AcademicSpecializations/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/AcademicSpecializations/Delete")]
//        public IActionResult DeleteAcademicSpecializations(long Id)
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


//                var row = db.AcademicSpecializations.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "AcademicSpecializations/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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

//        [HttpPost("{Id}/AcademicSpecializations/ChangeStatus")]
//        public IActionResult ChangeStatusAcademicSpecializations(long Id)
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


//                var row = db.AcademicSpecializations.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();

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
//                rowTrans.Controller = "AcademicSpecializations/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Name,
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












//        //FAQ
//        [HttpGet("Faq/Get")]
//        public IActionResult GetFaq(int pageNo, int pageSize, string Search)
//        {
//            try
//            {
//                var userId = this.help.GetCurrentUser(HttpContext);
//                if (userId <= 0)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotAuthorized);

//                var Statistics = new
//                {
//                    Count = db.Faq.Where(x => x.Status != 9).Count(),
//                    Active = db.Faq.Where(x => x.Status == 1).Count(),
//                    NotActive = db.Faq.Where(x => x.Status == 2).Count(),
//                    Deleted = db.Faq.Where(x => x.Status == 9).Count(),
//                };

//                int Count = db.Faq
//                    .Where(x => x.Status != 9
//                    && (string.IsNullOrEmpty(Search) ? true :
//                        (x.Email.Contains(Search.Trim()) ||
//                        x.Phone.Contains(Search.Trim()) ||
//                        x.Message.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
//                        ))
//                    ).Count();
//                var Info = db.Faq
//                    .Where(x => x.Status != 9
//                       && (string.IsNullOrEmpty(Search) ? true : (
//                        x.Email.Contains(Search.Trim()) ||
//                        x.Phone.Contains(Search.Trim()) ||
//                        x.Message.Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Date.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Year.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Month.ToString().Contains(Search.Trim()) ||
//                        x.CreatedOn.Value.Day.ToString().Contains(Search.Trim())
//                        ))
//                    ).Select(x => new
//                    {
//                        x.Id,
//                        x.Message,
//                        x.Phone,
//                        x.Email,
//                        x.CreatedOn,
//                        x.Status,
//                        CreatedBy = db.Users.Where(k => k.Id == x.CreatedBy).SingleOrDefault().Name,
//                        x.ProceedingOn,
//                        ProceedingBy = db.Users.Where(k => k.Id == x.ProceedingBy).SingleOrDefault().Name,
//                    }).OrderByDescending(x => x.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


//                return Ok(new { info = Info, count = Count, Statistics });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e.Message);
//            }
//        }

//        [HttpPost("{Id}/Faq/Delete")]
//        public IActionResult DeleteFaq(long Id)
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


//                var row = db.Faq.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 9;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.Delete;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "حذف بيانات   ";
//                rowTrans.Controller = "Faq/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Message,
//                    row.Phone,
//                    row.Email,
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

//        [HttpPost("{Id}/Faq/ChangeStatus")]
//        public IActionResult ChangeStatusFaq(long Id)
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
                
//                var row = db.Faq.Where(x => x.Id == Id && x.Status != 9).SingleOrDefault();
//                if (row == null)
//                    return StatusCode(BackMessages.StatusCode, BackMessages.NotFound);

//                row.Status = 2;
//                row.ProceedingBy = userId;
//                row.ProceedingOn = DateTime.Now;

//                TransactionsObject rowTrans = new TransactionsObject();
//                rowTrans.Operations = TransactionsType.CahngeStatus;
//                rowTrans.ItemId = row.Id;
//                rowTrans.Descriptions = "تغير حالة العرض  ";
//                rowTrans.Controller = "Faq/Dictionaries";
//                rowTrans.NewObject = JsonConvert.SerializeObject(new
//                {
//                    row.Id,
//                    row.Email,
//                    row.Phone,
//                    row.Message,
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

















//    }
//}