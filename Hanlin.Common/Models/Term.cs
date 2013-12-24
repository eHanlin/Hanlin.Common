using System;

namespace Hanlin.Common.Models
{

    // Migrated from ClientServices（翰林經銷雲）project
    public class Term : IEquatable<Term>
    {
        public static readonly int RocYearOffset = 1911;
        public static readonly int SemestersPerSchoolYear = 2;
        public static readonly String IdFormat = "{0}{1:D2}";

        public static readonly int SchoolYearStartMonth = 5;
        public static readonly int SemesterLengthInMonths = 12 / SemestersPerSchoolYear;

        public static string GetYearPart(string termId) { return termId.Substring(0, termId.Length - 2); }
        public static string GetSemesterPart(string termId) { return termId.Substring(termId.Length - 2); }

        public Term() : this(DateTime.Now) { }

        public Term(DateTime date)
        {
            int month = date.Month;
            int year = date.Year;

            WesternSchoolYear = month < SchoolYearStartMonth ? year - 1 : year;

            int monthIndex = month < SchoolYearStartMonth ? month + 12 : month; // Move months remaining from last school year to the end of the month sequence.
            monthIndex = monthIndex - SchoolYearStartMonth; // Reindex the sequence to start at 0
            Semester = Convert.ToByte(monthIndex / SemesterLengthInMonths + 1);

            InitId();
        }

        public Term(int westernSchoolYear, byte semester)
        {
            this.WesternSchoolYear = westernSchoolYear;
            this.Semester = semester;
            InitId();
        }

        private void InitId()
        {
            this.Id = string.Format("{0}{1:D2}", RocSchoolYear, Semester);
        }

        public Term(string termId)
        {
            int yearInRoc;
            byte aSemester;
            ValidateTermId(termId, out yearInRoc, out aSemester);

            this.Id = termId;
            this.RocSchoolYear = yearInRoc;
            this.Semester = aSemester;
        }

        private static void ValidateTermId(string termId, out int yearInRoc, out byte aSemester)
        {
            if (string.IsNullOrEmpty(termId)) throw new ArgumentException("termId");
            if (termId.Length < 3 || termId[0] == '0') throw new ArgumentException("Term id 必須是三個數字以上且不能以零開頭");

            string yearStr = GetYearPart(termId);

            bool isValidYear = int.TryParse(yearStr, out yearInRoc);

            if (!isValidYear) throw new ArgumentException("年份格式錯誤: " + termId, "termId");

            string semesterStr = GetSemesterPart(termId);

            bool isValidSemester = byte.TryParse(semesterStr, out aSemester);

            if (!isValidSemester)
                throw new ArgumentException("學期格式錯誤-不是整數: " + termId, "termId");
        }

        public string Id { get; set; }

        private int _westernSchoolYear;
        public int WesternSchoolYear
        {
            get { return _westernSchoolYear; }
            private set
            {
                if (value < RocYearOffset)
                {
                    throw new ArgumentException("請使用西元年. 給予的year=" + value);
                }
                _westernSchoolYear = value;
            }
        }

        public int RocSchoolYear
        {
            get { return WesternSchoolYear - RocYearOffset; }
            private set { WesternSchoolYear = value + RocYearOffset; }
        }

        private byte _semester;
        public byte Semester
        {
            get { return _semester; }
            private set
            {
                if (value < 1 || value > SemestersPerSchoolYear)
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format("學期格式錯誤-學期 {0} 不在合法範圍 [{1}, {2}]",
                        value, 1, SemestersPerSchoolYear));
                }
                _semester = value;
            }
        }

        public DateTime BeginDate
        {
            get
            {
                int monthsOffsetForNoninitialSemester = SemesterLengthInMonths * (Semester - 1);

                return new DateTime(WesternSchoolYear, SchoolYearStartMonth, 1).AddMonths(monthsOffsetForNoninitialSemester);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return BeginDate.AddMonths(SemesterLengthInMonths).AddDays(-1);
            }
        }

        // Mainly for mocking unit tests
        public virtual DateTime TodaysDate { get { return DateTime.Now.Date; } }

        private Term _previousTerm;
        public Term PreviousTerm
        {
            get
            {
                if (_previousTerm == null)
                {
                    int lastTermYear = WesternSchoolYear;
                    byte lastTermSemister = Semester;
                    if (lastTermSemister > 1)
                    {
                        lastTermSemister--;
                    }
                    else
                    {
                        lastTermYear--;
                        lastTermSemister = Convert.ToByte(SemestersPerSchoolYear);
                    }

                    _previousTerm = new Term(lastTermYear, lastTermSemister);
                }
                return _previousTerm;
            }
        }

        private Term _nextTerm;
        public Term NextTerm
        {
            get
            {
                if (_nextTerm == null)
                {
                    int nextTermYear = WesternSchoolYear;
                    byte nextTermSemester = Semester;
                    if (nextTermSemester < SemestersPerSchoolYear)
                    {
                        nextTermSemester++;
                    }
                    else
                    {
                        nextTermYear++;
                        nextTermSemester = 1;
                    }

                    _nextTerm = new Term(nextTermYear, nextTermSemester);
                }
                return _nextTerm;
            }
        }

        public Term PreviousSchoolYear { get { return new Term(WesternSchoolYear - 1, Semester); } }

        public Term NextSchoolYear { get { return new Term(WesternSchoolYear + 1, Semester); } }

        public string RelativeName
        {
            get
            {
                if (IsCurrentTerm) return "本期";

                if (IsPreviousTerm) return "上一期";

                if (IsNextTerm) return "下一期";

                return ShortName;
            }
        }

        public string ShortName
        {
            get
            {
                return string.Format("{0}{1}", RocSchoolYear, SemesterText);
            }
        }

        public string LongName
        {
            get
            {
                return string.Format("{0}{1}學期", RocSchoolYear, SemesterText);
            }
        }

        private string SemesterText
        {
            get
            {
                string semesterText;
                switch (Semester)
                {
                    case 1:
                        semesterText = "上";
                        break;
                    case 2:
                        semesterText = "下";
                        break;
                    default:
                        semesterText = "Unknown";
                        break;
                }
                return semesterText;
            }
        }

        #region Equality

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var term = obj as Term;
            return Equals(term);
        }

        public bool Equals(Term other)
        {
            if (other == null)
                return false;
            else
                return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Term obj1, Term obj2)
        {
            if ((object)obj1 == null || ((object)obj2) == null)
                return Object.Equals(obj1, obj2);

            return obj1.Equals(obj2);
        }

        public static bool operator !=(Term obj1, Term obj2)
        {
            return !(obj1 == obj2);
        }

        #endregion

        public override string ToString()
        {
            string format = "Term[year={0}, semester={1}]";
            return string.Format(format, WesternSchoolYear, Semester);
        }

        public bool IsCurrentTerm { get { return this == new Term(); } }

        public bool IsPreviousTerm { get { return this == new Term().PreviousTerm; } }

        public bool IsSameTermPreviousYear { get { return this == new Term().PreviousSchoolYear; } }

        public bool IsNextTerm { get { return this == new Term().NextTerm; } }
    }
}
