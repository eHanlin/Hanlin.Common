using Hanlin.Common.Enums;

namespace Hanlin.Domain.Models
{
    public static class SubjectCode
    {
        public const string ��� = "pc";
        public const string �^�� = "en";
        public const string �ƾ� = "ma";

        public const string �۵M�P�ͬ���� = "na";
        public const string �ͪ� = "bi";
        public const string �z�� = "py";
        public const string �a�� = "ea";
        public const string ���z = "ph";
        public const string �ƾ� = "ce";

        public const string ���| = "so";
        public const string ���v = "hi";
        public const string �a�z = "ge";
        public const string ���� = "cs";
    }

    public class Subject : Enumeration<string>
    {
        public static readonly Subject ��� = new Subject(SubjectCode.���, "���", "�y��", 110, new[] { Education.�ꤤ, Education.���� });
        public static readonly Subject �^�� = new Subject(SubjectCode.�^��, "�^��", "�y��", 120, new[] { Education.�ꤤ, Education.���� });

        public static readonly Subject �ƾ� = new Subject(SubjectCode.�ƾ�, "�ƾ�", "�ƾ�", 230, new[] { Education.�ꤤ, Education.���� });

        public static readonly Subject �ͪ� = new Subject(SubjectCode.�ͪ�, "�ͪ�", "�ۥ�", 340, new[] { Education.�ꤤ, Education.���� });
        public static readonly Subject �z�� = new Subject(SubjectCode.�z��, "�z��", "�ۥ�", 350, new[] { Education.�ꤤ });
        public static readonly Subject �a�� = new Subject(SubjectCode.�a��, "�a��", "�ۥ�", 360, new[] { Education.�ꤤ });
        public static readonly Subject ���z = new Subject(SubjectCode.���z, "���z", "�ۥ�", 350, new[] { Education.���� });
        public static readonly Subject �ƾ� = new Subject(SubjectCode.�ƾ�, "�ƾ�", "�ۥ�", 360, new[] { Education.���� });

        public static readonly Subject ���v = new Subject(SubjectCode.���v, "���v", "���|", 470, new[] { Education.�ꤤ, Education.���� });
        public static readonly Subject �a�z = new Subject(SubjectCode.�a�z, "�a�z", "���|", 480, new[] { Education.�ꤤ, Education.���� });
        public static readonly Subject ���� = new Subject(SubjectCode.����, "����", "���|", 490, new[] { Education.�ꤤ, Education.���� });

        /* ��� */
        public static readonly Subject �۵M�P�ͬ���� = new Subject(SubjectCode.�۵M�P�ͬ����, "�۵M�P�ͬ����", "�ۥ�", 300, new Education[0]);
        public static readonly Subject ���| = new Subject(SubjectCode.���|, "���|", "���|", 400, new Education[0]);


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