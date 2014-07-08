using Hanlin.Common.Enums;

namespace Hanlin.Domain.Models
{
    public class Curriculum : Enumeration<string>
    {
        public static readonly Curriculum 國中小92課綱 = new Curriculum("92", "92年國民中小學九年一貫課程綱要");
        public static readonly Curriculum 國中小97課綱 = new Curriculum("100", "97年國民中小學九年一貫課程綱要"); // The value was mistakenly chosen as '100'.
        public static readonly Curriculum 高中99課綱 = new Curriculum("99", "99普通高級中學課程綱要");
        
        /// <summary>
        /// 在7月31日公佈後，將自103學年度入學的新生起逐年實施。
        /// http://www.edu.tw/pages/detail.aspx?Node=1088&Page=20786&Index=3&wid=DDC91D2B-ACE4-4E00-9531-FC7F63364719
        /// 
        /// </summary>
        public static readonly Curriculum 高中103課綱 = new Curriculum("103", "103普通高級中學課程綱要");

        public Curriculum()
        {
        }

        public Curriculum(string value, string name)
            : base(value, name)
        {
        }
    }
}
