
namespace Swift_Queue_web.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class Swift_Queue_Db : DbContext
    {
        public Swift_Queue_Db()
            : base("name=Swift_Queue_Db")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Queue> Queues { get; set; }
        public virtual DbSet<Queue_Setting> Queue_Setting { get; set; }
    }
}
