using Hanlin.Common.Enums;

namespace Hanlin.Domain.Models
{
    public static class SubjectCode
    {
        public const string 國文 = "pc";
        public const string 英文 = "en";
        public const string 數學 = "ma";

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
    }

    public class Subject : Enumeration<string>
    {
        public static readonly Subject 國文 = new Subject(SubjectCode.國文, "國文", "語文", 110, new[] { Education.國中, Education.高中 });
        public static readonly Subject 英文 = new Subject(SubjectCode.英文, "英文", "語文", 120, new[] { Education.國中, Education.高中 });

        public static readonly Subject 數學 = new Subject(SubjectCode.數學, "數學", "數學", 230, new[] { Education.國中, Education.高中 });

        public static readonly Subject 生物 = new Subject(SubjectCode.生物, "生物", "自生", 340, new[] { Education.國中, Education.高中 });
        public static readonly Subject 理化 = new Subject(SubjectCode.理化, "理化", "自生", 350, new[] { Education.國中 });
        public static readonly Subject 地科 = new Subject(SubjectCode.地科, "地科", "自生", 360, new[] { Education.國中 });
        public static readonly Subject 物理 = new Subject(SubjectCode.物理, "物理", "自生", 350, new[] { Education.高中 });
        public static readonly Subject 化學 = new Subject(SubjectCode.化學, "化學", "自生", 360, new[] { Education.高中 });

        public static readonly Subject 歷史 = new Subject(SubjectCode.歷史, "歷史", "社會", 470, new[] { Education.國中, Education.高中 });
        public static readonly Subject 地理 = new Subject(SubjectCode.地理, "地理", "社會", 480, new[] { Education.國中, Education.高中 });
        public static readonly Subject 公民 = new Subject(SubjectCode.公民, "公民", "社會", 490, new[] { Education.國中, Education.高中 });

        /* 領域 */
        public static readonly Subject 自然與生活科技 = new Subject(SubjectCode.自然, "自然與生活科技", "自生", 300, new Education[0]);
        public static readonly Subject 社會 = new Subject(SubjectCode.社會, "社會", "社會", 400, new Education[0]);


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

    public class SeniorSubject : Enumeration<string>
    {
        public const string Prefix = "h";

        public static readonly SeniorSubject 國文 = new SeniorSubject(Prefix + SubjectCode.國文, "國文");
        public static readonly SeniorSubject 英文 = new SeniorSubject(Prefix + SubjectCode.英文, "英文");

        public static readonly SeniorSubject 數學 = new SeniorSubject(Prefix + SubjectCode.數學, "數學");

        public static readonly SeniorSubject 生物 = new SeniorSubject(Prefix + SubjectCode.生物, "生物");
        public static readonly SeniorSubject 物理 = new SeniorSubject(Prefix + SubjectCode.物理, "物理");
        public static readonly SeniorSubject 化學 = new SeniorSubject(Prefix + SubjectCode.化學, "化學");

        public static readonly SeniorSubject 歷史 = new SeniorSubject(Prefix + SubjectCode.歷史, "歷史");
        public static readonly SeniorSubject 地理 = new SeniorSubject(Prefix + SubjectCode.地理, "地理");
        public static readonly SeniorSubject 公民與社會 = new SeniorSubject(Prefix + SubjectCode.公民, "公民與社會");

        public SeniorSubject()
        {

        }

        public SeniorSubject(string value, string name)
            : base(value, name)
        {
        }
    }
}
