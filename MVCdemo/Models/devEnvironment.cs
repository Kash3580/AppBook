using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace MVCdemo.Models
{
    public class Environment
    {
        private int  _EnvID;
        private string _ArchDiagramPath;
        private string _LinksParameter;
        private string _ServerParameter;
        private string _DBServerParameter;
        private string _FileShareParameter;
        private string _envType;
        private int _appId;


        [Key]
        public int EnvID
        {
            get
            {
                return _EnvID;
            }

            set
            {
                _EnvID = value;
            }
        }
        
        [Display(Name ="Architecture")]
        public string ArchDiagramPath
        {
            get
            {
                return _ArchDiagramPath;
            }

            set
            {
                _ArchDiagramPath = value;
            }
        }
 
        [Display(Name = "Links")]
        public string LinksParameter
        {
            get
            {
                return _LinksParameter;
            }

            set
            {
                _LinksParameter = value;
            }
        }
       
        [Display(Name = "Server Details")]
        public string ServerParameter
        {
            get
            {
                return _ServerParameter;
            }

            set
            {
                _ServerParameter = value;
            }
        }
    
        [Display(Name = "DB Details")]
        public string DBServerParameter
        {
            get
            {
                return _DBServerParameter;
            }

            set
            {
                _DBServerParameter = value;
            }
        }
       
        [Display(Name = "FileShare Details")]
        public string FileShareParameter
        {
            get
            {
                return _FileShareParameter;
            }

            set
            {
                _FileShareParameter = value;
            }
        }

        public string EnvType
        {
            get
            {
                return _envType;
            }

            set
            {
                _envType = value;
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

    public class  EnvironmentDBContext:DbContext
    {
        public DbSet<Environment> Environments { get; set; }
        public DbSet<Application> Applications { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EnvironmentDBContext>(new DropCreateDatabaseIfModelChanges<EnvironmentDBContext>());
            base.OnModelCreating(modelBuilder);
        }

    }
}