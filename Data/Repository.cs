using DepartmentsAndEmployees.Models; // Add a Using Statement - so we can access the Models!
using System.Data.SqlClient; // We're using SQL Client, too!
using System;
using System.Collections.Generic;
using System.Text;

namespace DepartmentsAndEmployees.Data
{
    public class Repository
    {
        public SqlConnection Connection
        {
            get
            {
                // This is "Address" of the Database:
                string _connectionString = "Server = localhost\\SQLEXPRESS; Database = Employees; Trusted_Connection = True";
                return new SqlConnection(_connectionString);
            }
        }
        /************************************************************************************
        * Departments
        ************************************************************************************/

        /// <summary>
        ///  Returns a List of All Departments in the Database:
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            // We must "use" the Database Connection.
            //  Because a Database is a Shared Resource (Other Applications may be using it, too.), we must be careful about how we interact with it. Specifically, we "Open()" connections when we need to interact with the database and we "Close()" them when we're finished.
            //  In C#, a "Using" Block ensures we correctly Disconnect from a Resource, even if there is an Error.  For Database Connections, this means the Connection will be properly Closed:
            using (SqlConnection conn = Connection)
            {
                // Note, we must "Open()" the Connection; "Using" Block doesn't do that for us:
                conn.Open();

                // We must "Use" Commands, too:
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the Command with the SQL; we want to establish it before we Execute it:
                    cmd.CommandText = "SELECT Id, DeptName FROM Department";

                    // Execute the SQL in the Database and get a "Reader" that will give us Access to the Data:
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A List to hold the Departments we Retrieve from the Database:
                    List<Department> departments = new List<Department>();

                    // "Read()" will return "true", if there is more Data to Read:
                    while (reader.Read())
                    {
                        // The "Ordinal" is the Numeric Position of the Column in the Query Results.
                        //  For our Query, "Id" has an Ordinal Value of '0' and "DeptName" is '1':
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We use the Reader's GetXXX Methods to get the Value for a particular Ordinal:
                        int idValue = reader.GetInt32(idColumnPosition);

                        int deptNameColumnPosition = reader.GetOrdinal("DeptName");
                        string deptNameValue = reader.GetString(deptNameColumnPosition);

                        // Now, let's Create a New Department Object, using the Data from the Database:
                        Department department = new Department
                        {
                            Id = idValue,
                            DeptName = deptNameValue
                        };

                        // ...And Add said Department Object to our List:
                        departments.Add(department);
                    }

                    // We should "Close()" the Reader. Unfortunately, a "Using" Block won't work here:
                    reader.Close();

                    // Return the List of Departments for whomever called this Method:
                    return departments;
                }
            }
        }

        internal void DeleteDepartment(object id)
        {
            throw new NotImplementedException();
        }

        internal List<Employee> GetAllEmployeesWithDepartmentByDepartmentId(object id)
        {
            throw new NotImplementedException();
        }

        internal void UpdateDepartment(object id, Department accountingDepartmentFromDB)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Returns a Single Department with the given ID:
        /// </summary>
        public Department GetDepartmentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // String Interpolation lets us Inject the ID passed into this Method:
                    cmd.CommandText = $"SELECT DeptName FROM Department WHERE Id = {id}";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Department department = null;
                    if (reader.Read())
                    {
                        department = new Department
                        {
                            Id = id,
                            DeptName = reader.GetString(reader.GetOrdinal("DeptName"))
                        };
                    }

                    reader.Close();

                    return department;
                }
            }
        }

        internal void UpdateEmployee(object id, Employee dbJane)
        {
            throw new NotImplementedException();
        }

        internal void DeleteEmployee(object id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Add a New Department to the Database.
        ///   NOTE: This Method sends Data to the Database;
        ///   it does not get anything from the Database, so there is nothing to Return:
        /// </summary>
        public void AddDepartment(Department department)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More String Interpolation:
                    cmd.CommandText = $"INSERT INTO Department (DeptName) Values ('{department.DeptName}')";
                    cmd.ExecuteNonQuery();
                }
            }

            // When this Method is finished, we can look in the Database and see the New Department.
        }

        /// <summary>
        ///  Updates the Department with the given ID:
        /// </summary>
        public void UpdateDepartment(int id, Department department)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we do something a little different...
                    //  We're using a "Parameterized" Query to avoid SQL Injection Attacks.
                    //  First, we add Variable Names with @ signs in our SQL.
                    //  Then, we add "SqlParamter(s)" for each of those Variables:
                    cmd.CommandText = @"UPDATE Department
                                           SET DeptName = @deptName
                                         WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@deptName", department.DeptName));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    // Maybe we should Refactor our other SQL to use Parameters?

                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        ///  Delete the Department with the given ID:
        /// </summary>
        public void DeleteDepartment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Department WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }


        /************************************************************************************
         * Employees
         ************************************************************************************/

        public List<Employee> GetAllEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, DepartmentId FROM Employee";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                        };

                        employees.Add(employee);
                    }

                    reader.Close();

                    return employees;
                }
            }
        }

        /// <summary>
        ///  Get an Individual Employee by ID.
        /// </summary>
        /// <param name="id">The Employee's ID</param>
        /// <returns>The Employee with that given ID:</returns>
        public Employee GetEmployeeById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    /*
                     * TODO: Complete this Method...
                     */
                    // String Interpolation allows for Injecting the "Id" Passed into this Method:
                    cmd.CommandText = $"SELECT Id, FirstName, LastName, DepartmentId FROM Employee WHERE Id = {id}";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employee = null;
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                        };
                    }

                    reader.Close();

                    //return null;
                    return employee;
                }
            }
        }


        /// <summary>
        ///  Get All Employees, along with their Departments.
        /// </summary>
        /// <returns>A List of Employees, in which each Employee Object contains their Department Object:</returns>
        public List<Employee> GetAllEmployeesWithDepartment()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    /*
                     * TODO: Complete this Method...
                     *  Look at "GetAllEmployeesWithDepartmentByDepartmentId(int departmentId)" for inspiration:
                     */

                    cmd.CommandText = $@"SELECT e.Id AS EmpId, e.FirstName, e.LastName, e.DepartmentId, d.DeptName, d.Id AS DeptId
                                           FROM Employee e INNER JOIN Department d ON e.DepartmentID = d.Id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("EmpId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            Department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DeptId")),
                                DeptName = reader.GetString(reader.GetOrdinal("DeptName"))
                            }
                        };

                        employees.Add(employee);
                    }

                    reader.Close();

                    //return null;
                    return employees;
                }
            }
        }


        /// <summary>
        ///  Get Employees who are in the GIVEN Department. Include the Employee's Department Object.
        /// </summary>
        /// <param name="departmentId">Only include Employees in this Department.</param>
        /// <returns>A List of Employees, in which each Employee Object contains their Department Object.</returns>
        /// 
        public List<Employee> GetAllEmployeesWithDepartmentByDepartmentId(int departmentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId,
                                                d.DeptName
                                           FROM Employee e INNER JOIN Department d ON e.DepartmentID = d.id
                                          WHERE d.id = @departmentId";
                    cmd.Parameters.Add(new SqlParameter("@departmentId", departmentId));
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            Department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                DeptName = reader.GetString(reader.GetOrdinal("DeptName"))
                            }
                        };

                        employees.Add(employee);
                    }

                    reader.Close();

                    return employees;
                }
            }
        }


        /// <summary>
        ///  Add a New Employee to the Database.
        ///   NOTE: This Method sends Data to the Database;
        ///   it does not get anything from the Database, so there is nothing to Return.
        /// </summary>
        public void AddEmployee(Employee employee)
        {
            /*
             * TODO: Complete this Method by using an "INSERT" Statement with SQL
             *  (Remember to use SqlParameters!):
             */
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"INSERT INTO Employee (FirstName, LastName, DepartmentId) 
                                              VALUES ('{employee.FirstName}', '{employee.LastName}', '{employee.DepartmentId}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  Updates the Employee with the Given ID:
        /// </summary>
        public void UpdateEmployee(int id, Employee employee)
        {
            /*
             * TODO: Complete this Method using an "UPDATE" Statement with SQL
             *  (Remember to use SqlParameters!):
             */
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Employee
                                           SET FirstName = @FirstName, LastName = @LastName, DepartmentId = @DepartmentId
                                         WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentId", employee.DepartmentId));

                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        ///  Delete the Employee with the Given ID:
        /// </summary>
        public void DeleteEmployee(int id)
        {
            /*
             * TODO: Complete this Method, using a "DELETE" Statement with SQL
             *  (Remember to use SqlParameters!):
             */
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Employee WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
