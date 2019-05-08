using System;
using System.Collections.Generic;

namespace DepartmentsAndEmployees
{
    internal class ObjectDataReader<T> : IDisposable
    {
        private IEnumerable<Person> people;

        public ObjectDataReader(IEnumerable<Person> people)
        {
            this.people = people;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}