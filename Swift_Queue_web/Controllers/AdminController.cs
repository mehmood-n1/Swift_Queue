using Swift_Queue_web.Models;
using Swift_Queue_web.Utills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swift_Queue_web.Controllers
{
    public class AdminController : Controller
    {
        Swift_Queue_Db db = new Swift_Queue_Db();

        // GET: Admin
        public ActionResult StartQueue()
        {
            var queues = db.Queue_Setting.Where(x => x.Status == (int)QueueStatus.Active).ToList();
            return View(queues);
        } 
        public ActionResult AllEndQueue()
        {
            var queues = db.Queue_Setting.Where(x=>x.Status==(int)QueueStatus.End).OrderByDescending(x=>x.Id).ToList();
            return View(queues);
        }
        public ActionResult AllQueuCustomers(int id)
        {
            
            return View(db.Queues.Where(x => x.Queue_fid == id).ToList());
        }
        public ActionResult AllQueuActiveCustomers(int id)
        {
            
            return View(db.Queues.Where(x => x.Queue_fid == id && x.Status==(int)CustomerStatus.Waiting && x.Status==(int)CustomerStatus.Proceed ).ToList());
        }
        public ActionResult GenrateNewQueue()
        {
            return View();
        }
        public ActionResult EndQueue(int id)
        {
           var queue= db.Queue_Setting.Find(id);
            queue.Status =(int) QueueStatus.End;
            db.Entry(queue).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

            TempData["msg"] = "Queue is end Successfuly";


            return RedirectToAction("AllEndQueue");
        }
        [HttpPost]
        public ActionResult GenrateNewQueue(Queue_Setting queue_Setting)
        {
            queue_Setting.Acc_fid = BaseHelper.CurrentAccount.Acciunt_id;
            queue_Setting.Status = (int)QueueStatus.Active;
            queue_Setting.Queue_CreatedDatetime = DateTime.Now;

            var queue = db.Queue_Setting.Where(x=>x.Status==(int)QueueStatus.Active).FirstOrDefault();
            if (queue != null)
            {
                queue.Status = (int)QueueStatus.End;
                db.Entry(queue).State = System.Data.Entity.EntityState.Modified;
            }
            db.Queue_Setting.Add(queue_Setting);
            db.SaveChanges();

            return RedirectToAction("StartQueue");
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            

            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(Account account)
        {
            account.Account_password = Data.EnryptString(account.Account_password);

            var adminLoginData = db.Accounts.FirstOrDefault(x => x.Account_Email == account.Account_Email && x.Account_password == account.Account_password);
            if (adminLoginData != null)
            {

                BaseHelper.CurrentAccount = adminLoginData;

                return RedirectToAction("Index");
            }
            else
            {
              
                TempData["error"] = "  Invalid Email And password ";
            }

            

            return View();
        }
    }
}