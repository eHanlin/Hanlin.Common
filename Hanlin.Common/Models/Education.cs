using Hanlin.Common.Enums;

namespace Hanlin.Common.Models
{
    public class Education : Enumeration<string>
    {
        public static readonly Education 國小 = new Education("primary", "國小");
        public static readonly Education 國中 = new Education("junior", "國中");
        public static readonly Education 高中 = new Education("senior", "高中");
        public static readonly Education 高職 = new Education("vocational", "高職");
        public static readonly Education 幼教 = new Education("preschool", "幼教");

        public Education()
        {
        }

        public Education(string value, string name) : base(value, name)
        {
        }

    }
}