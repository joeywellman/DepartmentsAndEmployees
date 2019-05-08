using System;
using System.Collections.Generic;
using System.Text;

namespace DepartmentsAndEmployees.Models
{
    //class Employee
    //{
    //    public string FirstName { get; internal set; }
    //    public object DepartmentId { get; internal set; }
    //    public string LastName { get; internal set; }
    //    public object Id { get; internal set; }
    //    public Department Department { get; internal set; }

    // C# representation of the Employee Table:
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // This Property is for holding the actual Foreign Key Integer:
        public int DepartmentId { get; set; }

        // This Property is for storing the C# Object representing the Department:
        public Department Department { get; set; }
    }
}
