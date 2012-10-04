using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_ESM.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class EventsEntities : DbContext
    {
        public EventsEntities()
            : base("name=EventsEntities")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    throw new UnintentionalCodeFirstException();
        //}

        public DbSet<Event> Events { get; set; }
    }
    public partial class Event
    {
        [Key]
        public virtual int id { get; set; }
        //public virtual String Title { get; set ; }
        //public virtual string Description { get; set; }
        [Required]
        public virtual DateTime start_date { get; set; }
        [Required]
        public virtual DateTime end_date { get; set; }
        public virtual string text { get; set; }
        public virtual string details { get; set; }
    }
}