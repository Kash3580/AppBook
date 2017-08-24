using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MVCdemo.Models
{
    public class Application
    {

        private int _appId;
        private string _appName;
        private string _appOwner;
        private string _appDesc;
        private DateTime _dateCreated;
        private DateTime _dateModified;

        [Key]
        public int AppId
        {
            get
            {
                return _appId;
            }

            set
            {
                _appId = value;
            }
        }
        [Display(Name ="Application Name")]
        public string AppName
        {
            get
            {
                return _appName;
            }

            set
            {
                _appName = value;
            }
        }
        [Display(Name = "Application Owner")]
        public string AppOwner
        {
            get
            {
                return _appOwner;
            }

            set
            {
                _appOwner = value;
            }
        }
        [Display(Name = "Application Description")]
        [StringLength(500)]
        public string AppDesc
        {
            get
            {
                return _appDesc;
            }

            set
            {
                _appDesc = value;
            }
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true) ]
        [Display(Name = "Created Date")]
        public DateTime DateCreated
        {
            get
            {
                return _dateCreated;
            }

            set
            {
                _dateCreated = value;
            }
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modified Date")]
        public DateTime DateModified
        {
            get
            {
                return _dateModified;
            }

            set
            {
                _dateModified = value;
            }
        }

        public ICollection<Links> Linkss { get; set; }
        public ICollection<Environment> Environments { get; set; }
        public ICollection<DBServer> DBServers { get; set; }
        public ICollection<Server> Servers { get; set; }
    }
    public class ApplicationDBContext:DbContext
    {

        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ApplicationDBContext>(new DropCreateDatabaseIfModelChanges<ApplicationDBContext>());
            base.OnModelCreating(modelBuilder);
        }
        public System.Data.Entity.DbSet<MVCdemo.Models.Links> Links { get; set; }
        public System.Data.Entity.DbSet<MVCdemo.Models.Environment> Environments { get; set; }
        public System.Data.Entity.DbSet<MVCdemo.Models.DBServer> DBServer { get; set; }
        public System.Data.Entity.DbSet<MVCdemo.Models.Server> Server { get; set; }
    }



}