using System.Collections.Specialized;
using System.Linq;
using Hanlin.Common.Utils;
using NUnit.Framework;

namespace Hanlin.Common.Tests.Utils
{
    internal class Company
    {
        public string Name { set; get; }
        public string ValidationUnit;
    }


    public class ReflectionUtilsTests
    {
        [TestCase("Hanlin", "name", true, false)]
        [TestCase("Hanlin", "Name", false, false)]
        public void Property(string name, string fieldName, bool isUpperCaes, bool isLowerCase)
        {
            var company = new Company {Name = name};
            Assert.AreEqual(name, ReflectionUtils.GetPropertyValue<string>(company, fieldName, isUpperCaes, isLowerCase));
        }

        [TestCase]
        public void PropertyNames()
        {
            var company = new Company();
            var propNames = ReflectionUtils.GetPropertyNames(company);
            Assert.AreEqual(true, propNames.Contains("Name"));
            Assert.AreEqual(1, propNames.Count());
        }

        [TestCase("gov", "validationUnit", true, false)]
        [TestCase("gov", "ValidationUnit", false, false)]
        public void Field(string validationUnit, string fieldName, bool isUpperCaes, bool isLowerCase)
        {
            var company = new Company();
            company.ValidationUnit = validationUnit;
            Assert.AreEqual(validationUnit, ReflectionUtils.GetFieldValue<string>(company, fieldName, isUpperCaes, isLowerCase));
        }

        [TestCase]
        public void FieldNames()
        {
            var company = new Company();
            var fieldNames = ReflectionUtils.GetFieldNames(company);
            Assert.AreEqual(true, fieldNames.Contains("ValidationUnit"));
            Assert.AreEqual(1, fieldNames.Count());
        }

        [TestCase]
        public void MemberName()
        {
            Assert.AreEqual(ReflectionUtils.GetMemberName((Company c) => c.ValidationUnit), "ValidationUnit");
            Assert.AreEqual(ReflectionUtils.GetMemberName((Company c) => c.ValidationUnit, true), "validationUnit");
        }
    }
}