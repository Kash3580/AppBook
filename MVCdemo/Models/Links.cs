using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
namespace MVCdemo.Models
{
    public class Links
    {
        private int _linkId;
        private string _linkType;
        private string _Link;
        private string _username;
        private string _password;
        private int _appId;
        private DateTime _createdDate;
        private DateTime _modifiedDate;
        [Key]
        public int LinkId
        {
            get
            {
                return _linkId;
            }

            set
            {
                _linkId = value;
            }
        }
        [Display(Name ="Link Type")]
        [Required]
        public string LinkType
        {
            get
            {
                return _linkType;
            }

            set
            {
                _linkType = value;
            }
        }
        [Display(Name = "Link")]
        [Required]
        public string Link
        {
            get
            {
                return _Link;
            }

            set
            {
                _Link = value;
            }
        }
        [Display(Name = "User Name")]
        [Required]
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }
        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }
     
     
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modified Date")]
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

    public class LinksDBContext : DbContext
    {
       
        public  DbSet<Links> Links { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LinksDBContext>(new DropCreateDatabaseIfModelChanges<LinksDBContext>());
            base.OnModelCreating(modelBuilder);
        }
         

    }

}