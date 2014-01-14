using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanlin.Domain.Models;

namespace Hanlin.Domain.Services
{
    public class SchoolService : ISchoolService
    {
        public SchoolService()
        {
        }

        public Term GetCurrentTerm()
        {
            return new Term(DateTime.Now);
        }
    }
}
