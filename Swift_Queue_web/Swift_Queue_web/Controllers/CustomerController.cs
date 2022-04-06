using Swift_Queue_web.Models;
using Swift_Queue_web.Utills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swift_Queue_web.Controllers
{

    public class CustomerController : Controller
    {
        Swift_Queue_Db db = new Swift_Queue_Db();
        // GET: Customer
        public ActionResult CustomerSignUp()
        {

            return View();
        }
        public ActionResult StartQueue()
        {
            var queues = db.Queue_Setting.Where(x => x.Status == (int)QueueStatus.Active).ToList();
            return View(queues);
        }
        public ActionResult JoinQueue()
        {
          var queueData=  db.Queues.FirstOrDefault(x => x.Queue_Setting.Status == (int)QueueStatus.Active && x.customer_fid == BaseHelper.CurrentCustomer.Customer_id);

            return View(queueData);
        }
        [HttpPost]
        public ActionResult joinQueueRequest(int partSize)
        {
            Queue queue = new Queue();
            queue.customer_fid = BaseHelper.CurrentCustomer.Customer_id;
            queue.Party_size = partSize;
            queue.Queue_fid = db.Queue_Setting.FirstOrDefault(x => x.Status == (int)QueueStatus.Active).Id;
            if (!db.Queues.Any(x => x.Queue_fid == queue.Queue_fid))
            {
                queue.Status = (int)CustomerStatus.Proceed;

                queue.Queue_no = 1;
            }
            else
            {
                queue.Status = (int)CustomerStatus.Waiting;

                var queuedata = db.Queues.Where(x => x.Queue_fid == queue.Queue_fid).ToList();
               var maxQueueNo= queuedata.Max(x => x.Queue_no);
                maxQueueNo++;
                queue.Queue_no = maxQueueNo;

            }
            db.Queues.Add(queue);
            db.SaveChanges();
            return RedirectToAction("JoinQueue");
        }
        [HttpPost]
        public ActionResult CustomerSignUp(Customer customer)
        {
            if (ModelState.IsValid)
            {
              var isEmailAlreadyregisterd=  db.Customers.Any(x => x.Customer_Email==customer.Customer_Email);
                if (!isEmailAlreadyregisterd)
                {
                    customer.Customer_password= Data.EnryptString(customer.Customer_password);
                    db.Customers.Add(customer);

                    db.SaveChanges();
                    TempData["msg"] = "  Your account is Created Please login Now ";
                    return RedirectToAction("CustomerLogin");
                }
                TempData["error"] = " Email Already Registerd  ";

            }

            return View();
        }
        public ActionResult CustomerLogin()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CustomerLogin(Customer customer)
        {
            customer.Customer_password = Data.EnryptString(customer.Customer_password);

            var CutomerLoginData =  db.Customers.FirstOrDefault(x => x.Customer_Email == customer.Customer_Email && x.Customer_password == customer.Customer_password);
            if (CutomerLoginData != null)
            {
                BaseHelper.CurrentCustomer = CutomerLoginData;
                return RedirectToAction("JoinQueue");
            }
            else
            {
                TempData["error"]= "Invalid Email And password ";
            }
            return View();
        }
    }
}