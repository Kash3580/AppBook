using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace MVCdemo.Models
{
    public class Server
    {

        private int serverID;
        private string serverName;
        private string serverType;
        private string  serverEnv;
        private string serverVersion;
        private string userName;
        private string password;
        private DateTime createdDate;
        private DateTime modifiedDate;
        private int _appId;

        [Key]
        public int ServerID
        {
            get
            {
                return serverID;
            }

            set
            {
                serverID = value;
            }
        }
        [Required]
        [Display(Name ="Server Name")]
        public string ServerName
        {
            get
            {
                return serverName;
            }

            set
            {
                serverName = value;
            }
        }
   
        [Display(Name = "Server Type")]
        public string ServerType
        {
            get
            {
                return serverType;
            }

            set
            {
                serverType = value;
            }
        }
        [Required]
        [Display(Name = "Server Version")]
        public string ServerVersion
        {
            get
            {
                return serverVersion;
            }

            set
            {
                serverVersion = value;
            }
        }
        [Required]
        [Display(Name = "User Name")]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }
        [Required]
        [Display(Name = "Password")]
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        [Display(Name = "Created Date")]
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
        [Display(Name = "Modified Date")]
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
        [Display(Name = "Environment")]
        public string ServerEnv
        {
            get
            {
                return serverEnv;
            }

            set
            {
                serverEnv = value;
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

    public  class ServerDbContext:DbContext
    {
        public DbSet<Server> Server { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ServerDbContext>(new DropCreateDatabaseIfModelChanges<ServerDbContext>());
            base.OnModelCreating(modelBuilder);
        }
    }               

}