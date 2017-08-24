using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace MVCdemo.Models
{
    public class Employee
    {
        private int _EmpID;
        private string _Name = "";
      
        private string _Address = "";
        private DateTime _BOD;
        private int _Salary;

        [Key]
        public int EmpId
        {
            get { return _EmpID; }
            set { _EmpID = value; }
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

       
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = value;
            }
        }

        public DateTime BOD
        {
            get
            {
                return _BOD;
            }

            set
            {
                _BOD = value;
            }
        }

        public int Salary
        {
            get
            {
                return _Salary;
            }

            set
            {
                _Salary = value;
            }
        }
         

    }

    public class EmployeeDBContext:DbContext
    {


        public DbSet<Employee> Employees { get; set; }
    }


}