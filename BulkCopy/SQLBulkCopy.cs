//IRRELEVANT TESTING:
//namespace SqlBulkCopyExample
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Data;
//    using System.Data.SqlClient;
//    using System.Diagnostics;
//    using System.Linq;
//    using SqlBulkCopyExample.Properties;

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var people = CreateSamplePeople(10000);

//            using (var connection = new SqlConnection(
//        "Server=.;Database=MostWanted;Integrated Security=SSPI"))
//            {
//                connection.Open();
//                InsertDataUsingSqlBulkCopy(people, connection);
//            }
//        }



//        private static void InsertDataUsingSqlBulkCopy(
//        IEnumerable<Person> people, SqlConnection connection)
//        {
//            var bulkCopy = new SqlBulkCopy(connection);
//            bulkCopy.DestinationTableName = "Person";
//            bulkCopy.ColumnMappings.Add("Name", "Name");
//            bulkCopy.ColumnMappings.Add("DateOfBirth", "DateOfBirth");

//            using (var dataReader = new ObjectDataReader<Person>(people))
//            {
//                bulkCopy.WriteToServer(dataReader);
//            }
//        }


//        private static IEnumerable<Person> CreateSamplePeople(int count)
//        {
//            return Enumerable.Range(0, count)
//                .Select(i => new Person
//                {
//                    Name = "Person" + i,
//                    DateOfBirth = new DateTime(
//                1950 + (i % 50),
//                ((i * 3) % 12) + 1,
//                ((i * 7) % 29) + 1)
//                });
//        }
//    }
//}