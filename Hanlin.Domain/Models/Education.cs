using System.Collections.Generic;
using System.Linq;
using Hanlin.Common.Enums;

namespace Hanlin.Domain.Models
{
    public class Education : Enumeration<string>
    {
        public static readonly Education 國小 = new Education("e", "國小");
        public static readonly Education 國中 = new Education("j", "國中");
        public static readonly Education 高中 = new Education("h", "高中");
        public static readonly Education 高職 = new Education("v", "高職");

        public Education()
        {
        }

        public Education(string value, string name) : base(value, name)
        {
        }

    }
}