using DepartmentsAndEmployees.Data;
using DepartmentsAndEmployees.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DepartmentsAndEmployees
{
    class Program
    {
        static void Main(string[] args)
        {
            // We must create an Instance of the Repository class in order to use it's methods to interact with the Database:
            Repository repository = new Repository();

            List<Department> departments = repository.GetAllDepartments();

            // PrintDepartmentReport should print a department report to the console, but does it?
            //  Take a look at how it's defined below:
            PrintDepartmentReport("All Departments: ", departments);


            // What is this? Scroll to the bottom of the file and find out for yourself.
            Pause();

            // WRONG PrintEmployee Report location:
            //List<Employee> allEmployees = repository.GetAllEmployees();
            //PrintEmployeeReport("All Employees: ", allEmployees);

            //Pause();


            // Create an New Instance of a Department, so we can save our New Department to the Database:
            Department accounting = new Department { DeptName = "Accounting" };
            // Pass the Accounting Object as an Argument to the Repository's "AddDepartment()" Method:
            repository.AddDepartment(accounting);

            departments = repository.GetAllDepartments();
            PrintDepartmentReport("All Departments AFTER Adding Accounting Department: ", departments);

            Pause();


            // Pull the Object that represents the Accounting Department from the List of Departments that we get from the Database...
            // "First()" is a handy LINQ Method that gives us the First Element in a List that matches the Condition:
            Department accountingDepartmentFromDB = departments.First(d => d.DeptName == "Accounting");

            // How are the "accounting" and "accountingDepartmentFromDB" Objects different?
            //  Why are they different?
            Console.WriteLine($"                accounting --> {accounting.Id}: {accounting.DeptName}");
            Console.WriteLine($"accountingDepartmentFromDB --> {accountingDepartmentFromDB.Id}: {accountingDepartmentFromDB.DeptName}");

            Pause();


            // Change the name of the Accounting Department and Save the Change to the Database:
            accountingDepartmentFromDB.DeptName = "Creative Accounting";
            repository.UpdateDepartment(accountingDepartmentFromDB.Id, accountingDepartmentFromDB);

            departments = repository.GetAllDepartments();
            PrintDepartmentReport("All Departments AFTER Updating Accounting Department: ", departments);

            Pause();


            // Maybe we don't need an Accounting Department after all (Delete it)...
            repository.DeleteDepartment(accountingDepartmentFromDB.Id);

            departments = repository.GetAllDepartments();
            PrintDepartmentReport("All Departments AFTER Deleting Accounting Department: ", departments);

            Pause();


            // Create a New Variable to contain a List of Employees and get that List from the Database:
            List<Employee> employees = repository.GetAllEmployees();

            // Does this Method do what it claims to do, or does it need some work?
            PrintEmployeeReport("All Employees: ", employees, false);

            Pause();


            employees = repository.GetAllEmployeesWithDepartment();
            PrintEmployeeReport("All Employees with Departments: ", employees, true);

            Pause();


            // Here we get the First Department by Index...
            //  We could use "First()" here without Passing it a Condition, but using the Index is easy enough:
            Department firstDepartment = departments[0];
            employees = repository.GetAllEmployeesWithDepartmentByDepartmentId(firstDepartment.Id);
            PrintEmployeeReport($"Employees in {firstDepartment.DeptName}: ", employees, true);

            Pause();


            // Instantiate a New Employee Object...
            //  Note we are making the Employee's DepartmentId refer to an Existing Department;
            //  this is important because if we use an Invalid Department ID, we won't be able to save the New Employee Record to the Database because of a Foreign Key Constraint Violation:
            Employee jane = new Employee
            {
                FirstName = "Jane",
                LastName = "Lucas",
                DepartmentId = firstDepartment.Id
            };
            repository.AddEmployee(jane);

            employees = repository.GetAllEmployeesWithDepartment();
            PrintEmployeeReport("All Employees AFTER Adding Jane: ", employees, true);

            Pause();


            // Once again, we see "First()" in action:
            Employee dbJane = repository.GetAllEmployees().First(e => e.FirstName == "Jane");

            // Get the Second Department by Index ("[1]"):
            Department secondDepartment = departments[1];

            dbJane.DepartmentId = secondDepartment.Id;
            repository.UpdateEmployee(dbJane.Id, dbJane);

            employees = repository.GetAllEmployeesWithDepartment();
            PrintEmployeeReport("All Employees AFTER Updating Jane: ", employees, true);

            Pause();


            repository.DeleteEmployee(dbJane.Id);
            employees = repository.GetAllEmployeesWithDepartment();

            PrintEmployeeReport("All Employees AFTER Deleting Jane", employees, true);

            Pause();

        }


        /// <summary>
        ///  Prints a Simple Report with the given Title and Department information...
        /// </summary>
        /// <remarks>
        ///  Each Line of the Report should include the Department's ID and Name:
        /// </remarks>
        /// <param name="title">Title for the Report.</param>
        /// <param name="departments">Department Data for the Report.</param>
        public static void PrintDepartmentReport(string title, List<Department> departments)
        {
            /*
             * TODO: Complete this Method
             *  For example a Report entitled, "All Departments" should look like this:

                All Departments (I added ": ")
                1: Marketing
                2: Engineering
                3: Design
             */
            Console.WriteLine(title);
            int index = 0;
            foreach (Department singleDept in departments)
            {
                index++;
                Console.WriteLine($"{index}. {singleDept.DeptName}");
                //WRONG Concept: Console.WriteLine($"{singleDept.Id} : {singleDept.DeptName}");
            }
            //UNNECESSARY: Console.WriteLine("\n");
        }

        /// <summary>
        ///  Prints a Simple Report with the given Title and Employee information...
        /// </summary>
        /// <remarks>
        ///  Each Line of the Report should include the Employee's ID, First Name, Last Name, and Department Name IF AND ONLY IF the Department is not Null:
        /// </remarks>
        /// <param name="title">Title for the Report.</param>
        /// <param name="employees">Employee Data for the Report.</param>
        public static void PrintEmployeeReport(string title, List<Employee> employees, bool withDept)
        {
            /*
             * TODO: Complete this method
             *  For example a report entitled, "All Employees", should look like this:

                All Employees (I added ": ")
                1: Margorie Klingerman
                2: Sebastian Lefebvre
                3: Jamal Ross

             *  A Report entitled, "All Employees with Departments", should look like this:

                All Employees with Departments (I added ": ")
                1: Margorie Klingerman. Dept: Marketing
                2: Sebastian Lefebvre. Dept: Engineering
                3: Jamal Ross. Dept: Design

             */
            Console.WriteLine(title);
            int index = 0;

            if (!withDept)
            {
                foreach (Employee singleEmp in employees)
                {
                    index++;
                    Console.WriteLine($"{index}. {singleEmp.FirstName} {singleEmp.LastName}");
                }
                //UNNECESSARY: Console.WriteLine("\n");
            }
            else
            {
                foreach (Employee singleEmp in employees)
                {
                    index++;
                    Console.WriteLine($"{index}. {singleEmp.FirstName} {singleEmp.LastName}.  Dept: {singleEmp.Department.DeptName}");
                }
                //UNNECESSARY: Console.WriteLine("\n");
            }
        }


        /// <summary>
        ///  Custom function that pauses execution of the console app until the user presses a key
        /// </summary>
        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
