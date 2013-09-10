using System;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Hanlin.Common.Windows.Windows
{
    public class ClipboardOperationFailed : Exception
    {
        public ClipboardOperationFailed(string message)
            : base(message)
        {
        }
    }

    public static class ClipboardHelper
    {
        #region "宣告Win32處理剪貼簿功能的函數"

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetClipboardData(uint format);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll")]
        static extern bool EmptyClipboard();

        private const uint CF_ENHMETAFILE = 14;

        #endregion

        public static void SaveClipboardPng(Stream stream)
        {
            SaveClipboardEmf(stream, ImageFormat.Png);
        }

        private static void SaveClipboardEmf(Stream stream, ImageFormat format)
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                Failed("Cannot open clipboard.");
            }

            try
            {
                if (!IsClipboardFormatAvailable(CF_ENHMETAFILE))
                {
                    Failed("No enhanced metafile data available.");
                }

                IntPtr ptr = GetClipboardData(CF_ENHMETAFILE);

                if (ptr == IntPtr.Zero)
                {
                    Failed("Unable to retrieve data from clipboard even through Clipboard previously indicated data exists.");
                }

                var metafile = new Metafile(ptr, true);
                metafile.Save(stream, format);
            }
            finally
            {
                // "An application should call the CloseClipboard function after every successful call to OpenClipboard."
                //   -- http://msdn.microsoft.com/en-us/library/windows/desktop/ms649048(v=vs.85).aspx
                CloseClipboard();
            }
        }

        private static void Failed(string message)
        {
            throw new ClipboardOperationFailed(message);
        }
    }
}