using System;
using System.Text;

namespace Hanlin.Common.Utils
{
    public class LogBuilder
    {
        private readonly StringBuilder _log = new StringBuilder();

        public LogBuilder Header(string header)
        {
            return Paragraph(header);
        }

        public LogBuilder AppendLine(string text)
        {
            DateTime timeNow = DateTime.Now;
            _log.AppendLine(string.Format("{0} {1}: {2}", timeNow.ToShortDateString(), timeNow.ToShortTimeString(), text));
            return this;
        }

        public void AppendLineFormat(string format, params object[] args)
        {
            AppendLine(string.Format(format, args));
        }

        public LogBuilder Paragraph(string text)
        {
            AppendLine(text);
            return NewLine();
        }

        public LogBuilder NewLine()
        {
            _log.AppendLine();
            return this;
        }

        public override string ToString()
        {
            return _log.ToString();
        }

        public void ParagraphFormat(string format, params object[] args)
        {
            Paragraph(string.Format(format, args));
        }

        public void Clear()
        {
            _log.Clear();
        }
    }
}