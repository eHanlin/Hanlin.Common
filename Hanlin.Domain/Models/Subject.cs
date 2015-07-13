using Hanlin.Common.Enums;

namespace Hanlin.Domain.Models
{
    public static class StandardSubjectCode
    {
        public const string 國語 = "ch";
        public const string 國文 = "pc";
        public const string 英語 = "en";
        public const string 數學 = "ma";
        
        public const string 生活 = "li";
        public const string 健康與體育 = "pe";

        public const string 自然 = "na";
        public const string 生物 = "bi";
        public const string 理化 = "py";
        public const string 地科 = "ea";
        public const string 物理 = "ph";
        public const string 化學 = "ce";

        public const string 社會 = "so";
        public const string 歷史 = "hi";
        public const string 地理 = "ge";
        public const string 公民 = "cs";
        public const string 高中英文 = "he";//add 2015/7/9 by Kevin.
    }

    public class Subject : Enumeration<string>
    {
        public static readonly Subject 國語 = new Subject(StandardSubjectCode.國語, "國語", "語文", 100, new[] { Education.國小 });
        public static readonly Subject 國文 = new Subject(StandardSubjectCode.國文, "國文", "語文", 110, new[] { Education.國中, Education.高中 });
        public static readonly Subject 英語 = new Subject(StandardSubjectCode.英語, "英語", "語文", 120, new[] { Education.國中, Education.高中 });
        public static readonly Subject 生活 = new Subject(StandardSubjectCode.生活, "生活", "生活", 120, new[] { Education.國小 });
        public static readonly Subject 健康與體育 = new Subject(StandardSubjectCode.健康與體育, "健康與體育", "健康與體育", 120, new[] { Education.國小 });

        public static readonly Subject 數學 = new Subject(StandardSubjectCode.數學, "數學", "數學", 230, new[] { Education.國中, Education.高中 });

        public static readonly Subject 生物 = new Subject(StandardSubjectCode.生物, "生物", "自生", 340, new[] { Education.國中, Education.高中 });
        public static readonly Subject 理化 = new Subject(StandardSubjectCode.理化, "理化", "自生", 350, new[] { Education.國中 });
        public static readonly Subject 地科 = new Subject(StandardSubjectCode.地科, "地科", "自生", 360, new[] { Education.國中 });
        public static readonly Subject 物理 = new Subject(StandardSubjectCode.物理, "物理", "自生", 350, new[] { Education.高中 });
        public static readonly Subject 化學 = new Subject(StandardSubjectCode.化學, "化學", "自生", 360, new[] { Education.高中 });

        public static readonly Subject 歷史 = new Subject(StandardSubjectCode.歷史, "歷史", "社會", 470, new[] { Education.國中, Education.高中 });
        public static readonly Subject 地理 = new Subject(StandardSubjectCode.地理, "地理", "社會", 480, new[] { Education.國中, Education.高中 });
        public static readonly Subject 公民 = new Subject(StandardSubjectCode.公民, "公民", "社會", 490, new[] { Education.國中, Education.高中 });

        public static readonly Subject 高中英文 = new Subject(StandardSubjectCode.高中英文, "高中英文", "高中英文", 500, new[] { Education.高中 });//add 2015/7/9 by Kevin.
        /* 領域 */
        public static readonly Subject 自然與生活科技 = new Subject(StandardSubjectCode.自然, "自然與生活科技", "自生", 300, new Education[0]);
        public static readonly Subject 社會 = new Subject(StandardSubjectCode.社會, "社會", "社會", 400, new Education[0]);


        public Subject()
        {
            
        }

        public Subject(string value, string name, string area, int order, Education[] educations)
            : base(value, name)
        {
            Area = area;
            Order = order;
            Educations = educations;
        }

        public string Area { get; private set; }
        public int Order { get; private set; }
        public Education[] Educations { get; private set; }
    }
}
