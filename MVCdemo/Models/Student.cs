using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MVCdemo.Models
{
    public class Student
    {
        private int _RollNo;
        private string _Name;
        private string _City;
        [Key]
        public int RollNo
        {
            get
            {
                return _RollNo;
            }

            set
            {
                _RollNo = value;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }

        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                _City = value;
            }
        }


    }

    public class StudentDBContext: DbContext
    {
        public DbSet<Student> Students { get; set; }
    }

}