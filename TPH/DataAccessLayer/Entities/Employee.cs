using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPH.DataAccessLayer.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
    }

    public class Admin : Employee
    {
        public int SSN { get; set; }
        public string Manager { get; set; }

        public override string ToString()
        {
            return $"Admin => SSN = {SSN}, Manager = {Manager}";
        }
    }

    public class Developer : Employee
    {
        public int Salary { get; set; }
        public string JobTitle { get; set; }

        public override string ToString()
        {
            return $"Developer => Job Title = {JobTitle}, Salary = {Salary}";
        }
    }
}
