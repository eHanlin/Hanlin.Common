using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanlin.Common.Extensions
{
    public static class SizeExtensions
    {
        public static Size Scale(this Size size, float scale)
        {
            if (scale <= 0) return new Size(size.Width, size.Height);

            var aspect = (double)size.Width / size.Height;

            var width = Math.Floor(size.Width * scale);
            var height = width / aspect;

            var newSize = new Size((int)width, (int)height);

            return newSize;
        }
    }
}
