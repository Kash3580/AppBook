using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MVCdemo.Models
{
    public class FileShare
    {

        private int _fileShareId;
        private string _fileSharePath;
        private string _fileShareType;
        private string _userName;
        private string _Password;
        private DateTime _PasswordExpiryDate;
        private string _EnvType;
        private DateTime _createdDate;
        private DateTime _modifiedDate;
        private int _appId;
        [Key]
        public int FileShareId
        {
            get
            {
                return _fileShareId;
            }

            set
            {
                _fileShareId = value;
            }
        }
        [Required]
        [Display(Name = "File Share Path")]
        public string FileSharePath
        {
            get
            {
                return _fileSharePath;
            }

            set
            {
                _fileSharePath = value;
            }
        }
 
        [Display(Name = @"Cluster (Yes\No)")]
       public string FileShareType
        {
            get
            {
                return _fileShareType;
            }

            set
            {
                _fileShareType = value;
            }
        }
        [Required]
        [Display(Name = "User Name")]
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }
        [Required]
        [Display(Name = "Password")]
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }
        [Required]
        [Display(Name = "Password Expiry")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime PasswordExpiryDate
        {
            get
            {
                return _PasswordExpiryDate;
            }

            set
            {
                _PasswordExpiryDate = value;
            }
        }
        [Required]

        public string EnvType
        {
            get
            {
                return _EnvType;
            }

            set
            {
                _EnvType = value;
            }
        }
        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }

            set
            {
                _createdDate = value;
            }
        }
        [DataType(DataType.Date)]
        [Display(Name = "Modified Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate
        {
            get
            {
                return _modifiedDate;
            }

            set
            {
                _modifiedDate = value;
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
    
    public class FileShareDBContext:DbContext
    {

        public DbSet<FileShare> FileShare { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<FileShareDBContext>(new DropCreateDatabaseIfModelChanges<FileShareDBContext>());
            base.OnModelCreating(modelBuilder);
        }

    }

}