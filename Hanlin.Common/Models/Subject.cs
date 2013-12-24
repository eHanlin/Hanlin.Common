using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanlin.Common.Enums;

namespace Hanlin.Common.Models
{
    public class Subject : Enumeration<string>
    {
        public static readonly Subject 國文 = new Subject("PC", "國文");
        public static readonly Subject 英文 = new Subject("EN", "英文");
        public static readonly Subject 數學 = new Subject("MA", "數學");
        public static readonly Subject 自然與生活科技 = new Subject("NA", "自然與生活科技");
        public static readonly Subject 社會 = new Subject("SO", "社會");
        public static readonly Subject 生物 = new Subject("BI", "生物");
        public static readonly Subject 理化 = new Subject("PY", "理化");
        public static readonly Subject 地科 = new Subject("EA", "地科");
        public static readonly Subject 地理 = new Subject("GE", "地理");
        public static readonly Subject 歷史 = new Subject("HI", "歷史");
        public static readonly Subject 公民 = new Subject("CS", "公民");
        public static readonly Subject 物理 = new Subject("PH", "物理");
        public static readonly Subject 化學 = new Subject("CE", "化學");

        public Subject()
        {
            
        }

        public Subject(string value, string name)
            : base(value, name)
        {
            
        }
    }
}
