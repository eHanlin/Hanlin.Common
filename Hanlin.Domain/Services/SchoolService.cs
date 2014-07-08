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

        public string GetBaselineRocYear()
        {
            return "103";
        }

        public Term GetCurrentTerm()
        {
            return new Term(DateTime.Now);
        }

        public Curriculum GetCurrentCurriculum(string educationCode, string subjectCode, string headerPart41)
        {
            int header41Numeric;

            int.TryParse(headerPart41, out header41Numeric);

            if (educationCode == Education.國中.Value)
            {
                return Curriculum.國中小97課綱;
            }

            if (educationCode == Education.高中.Value)
            {
                switch (subjectCode)
                {
                    case SharedSubjectCode.數學:
                        switch (header41Numeric)
                        {
                            case 1:
                            case 2:
                                return Curriculum.高中103課綱;
                            default:
                                return Curriculum.高中99課綱;
                        }
                        break;
                    case SharedSubjectCode.物理:
                    case SharedSubjectCode.化學:
                        switch (header41Numeric)
                        {
                            case 1:
                                return Curriculum.高中103課綱;
                            default:
                                return Curriculum.高中99課綱;
                        }
                        break;
                    default:
                        return Curriculum.高中103課綱;
                }
            }

            throw new ArgumentException("Unable to determine curriculum for education " + educationCode);
        }
    }
}
