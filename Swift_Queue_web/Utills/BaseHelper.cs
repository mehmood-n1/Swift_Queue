using Swift_Queue_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swift_Queue_web.Utills
{
    public static class BaseHelper
    {
        public static Customer CurrentCustomer { get; set; }
        public static Account CurrentAccount { get; set; }
        
    }
}