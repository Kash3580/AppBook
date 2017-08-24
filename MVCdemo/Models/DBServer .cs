using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlTypes;

namespace MVCdemo.Models
{
    public class DBServer
    {

        private int dbserverID;
        private string dbserverName;
        private string dbserverEnv;
        private string dbserverVersion;
        private string dbuserName;
        private string dbpassword;
        private string dbserverSize;
        private DateTime createdDate;
        private DateTime modifiedDate; 
        private int _appId;
        [Key]
        public int DbserverID
        {
            get
            {
                return dbserverID;
            }

            set
            {
                dbserverID = value;
            }
        }
        [Required]
        [Display(Name = "DB Server Name")]
        public string DbserverName
        {
            get
            {
                return dbserverName;
            }

            set
            {
                dbserverName = value;
            }
        }
        [Required]
        [Display(Name ="Version")]
        public string DbserverVersion
        {
            get
            {
                return dbserverVersion;
            }

            set
            {
                dbserverVersion = value;
            }
        }
        [Required]
        [Display(Name = "UserName")]
        public string DbuserName
        {
            get
            {
                return dbuserName;
            }

            set
            {
                dbuserName = value;
            }
        }
        [Required]
        [Display(Name = "Password")]
        public string Dbpassword
        {
            get
            {
                return dbpassword;
            }

            set
            {
                dbpassword = value;
            }
        }
        
        public DateTime CreatedDate
        {
            get
            {
                return createdDate;
            }

            set
            {
                createdDate = value;
            }
        }
       
        public DateTime ModifiedDate
        {
            get
            {
                return modifiedDate;
            }

            set
            {
                modifiedDate = value;
            }
        }
        [Display(Name = "Server Size(MB)")]
        public string DbserverSize
        {
            get
            {
                return dbserverSize;
            }

            set
            {
                dbserverSize = value;
            }
        }
        [Display(Name = "Environment")]
        public string DbserverEnv
        {
            get
            {
                return dbserverEnv;
            }

            set
            {
                dbserverEnv = value;
            }
        }

        public virtual int AppId
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
        [ForeignKey("AppId")]
        public virtual Application Application { get; set; }

    }

    public  class DBServerDbContext:DbContext
    {
        public DbSet<DBServer> DBServer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DBServerDbContext>(new DropCreateDatabaseIfModelChanges<DBServerDbContext>());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Application> Applications { get; set; }
    }               

}