using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanlin.Common.Enums;

namespace Hanlin.Common.Models
{
    public class Subject : Enumeration<string>
    {
        public static readonly Subject 國文 = new Subject("pc", "國文", 110, new[] { Education.國中, Education.高中 });
        public static readonly Subject 英文 = new Subject("en", "英文", 120, new[] { Education.國中, Education.高中 });

        public static readonly Subject 數學 = new Subject("ma", "數學", 230, new[] { Education.國中, Education.高中 });

        public static readonly Subject 生物 = new Subject("bi", "生物", 340, new[] { Education.國中, Education.高中 });
        public static readonly Subject 理化 = new Subject("py", "理化", 350, new[] { Education.國中 });
        public static readonly Subject 地科 = new Subject("ea", "地科", 360, new[] { Education.國中 });
        public static readonly Subject 物理 = new Subject("ph", "物理", 350, new[] { Education.高中 });
        public static readonly Subject 化學 = new Subject("ce", "化學", 360, new[] { Education.高中 });

        public static readonly Subject 歷史 = new Subject("hi", "歷史", 470, new[] { Education.國中, Education.高中 });
        public static readonly Subject 地理 = new Subject("ge", "地理", 480, new[] { Education.國中, Education.高中 });
        public static readonly Subject 公民 = new Subject("cs", "公民", 490, new[] { Education.國中, Education.高中 });

        /* 領域 */
        //public static readonly Subject 自然與生活科技 = new Subject("NA", "自然與生活科技", new[] { Education.國中 });
        //public static readonly Subject 社會 = new Subject("SO", "社會", new[] { Education.國中 });


        public Subject()
        {
            
        }

        public Subject(string value, string name, int order, Education[] educations)
            : base(value, name)
        {
            Order = order;
            Educations = educations;
        }

        public int Order { get; private set; }
        public Education[] Educations { get; private set; }
    }
}
