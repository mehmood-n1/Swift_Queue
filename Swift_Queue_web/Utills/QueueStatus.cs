using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swift_Queue_web.Utills
{
    public enum QueueStatus
    {
        Active=1,
        End=0,
    }
    public enum CustomerStatus
    {
        Waiting=1,
        Proceed=2,
        Complete=3,
        Leave=4,
    }
}