using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTranslate.Win32
{
    internal static class NativeMethods
    {
        private const double AnInch = 14.4;

        public const int WM_USER = 0x0400;
        public const int EM_FORMATRANGE = WM_USER + 57;
        public const int WM_PAINT = 0x000F;
        public const int WM_CONTEXTMENU = 0x007b;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_MOUSEHWHEEL = 0x20E;

        public const int EM_LINEINDEX = 0x00BB;
        public const int EM_LINELENGTH = 0x00C1;

        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);

        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;

        public const int SB_HORZ = 0x0;
        public const int SB_VERT = 0x1;

        public const int SIF_TRACKPOS = 0x10;
        public const int SIF_RANGE = 0x1;
        public const int SIF_POS = 0x4;
        public const int SIF_PAGE = 0x2;
        public const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
        public const int SIF_DISABLENOSCROLL = 0x8;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CHARRANGE
        {
            // First character of range (0 for start of doc)
            public int cpMin;
            // Last character of range (-1 for end of doc)
            public int cpMax;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FORMATRANGE
        {
            // Actual DC to draw on
            public IntPtr hdc;
            // Target DC for determining text formatting
            public IntPtr hdcTarget;
            // Region of the DC to draw to (in twips)
            public RECT rc;
            // Region of the whole DC (page size) (in twips)
            public RECT rcPage;
            // Range of text to draw (see earlier declaration)
            public CHARRANGE chrg;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SCROLLINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wp, ref FORMATRANGE lp);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr handle, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr handle, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32")]
        public static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("user32")]
        public static extern int GetWindowLong(IntPtr hWnd, int index);

        [DllImport("user32")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, SCROLLINFO lpsi);

        public static int GetBaselineOffsetAtCharIndex(System.Windows.Forms.RichTextBox textBox, int index)
        {
            int lineNumber = textBox.GetLineFromCharIndex(index);
            int lineIndex = (int)SendMessage(textBox.Handle, EM_LINEINDEX, new IntPtr(lineNumber), IntPtr.Zero);
            int lineLength = (int)SendMessage(textBox.Handle, EM_LINELENGTH, new IntPtr(lineNumber), IntPtr.Zero);

            CHARRANGE charRange;
            charRange.cpMin = lineIndex;
            charRange.cpMax = lineIndex + lineLength;
            // charRange.cpMin = index; 
            // charRange.cpMax = index + 1; 

            RECT rect;
            rect.Top = 0;
            rect.Bottom = (int)AnInch;
            rect.Left = 0;
            rect.Right = 10000000; // (int)(rtb.Width * anInch + 20); 

            RECT rectPage;
            rectPage.Top = 0;
            rectPage.Bottom = (int)AnInch;
            rectPage.Left = 0;
            rectPage.Right = 10; // 10000000; // (int)(rtb.Width * anInch + 20); 

            FORMATRANGE formatRange;

            var hDC = GetDC(textBox.Handle);

            try
            {
                formatRange.chrg = charRange;
                formatRange.hdc = hDC;
                formatRange.hdcTarget = hDC;
                formatRange.rc = rect;
                formatRange.rcPage = rectPage;

                SendMessage(textBox.Handle, EM_FORMATRANGE, IntPtr.Zero, ref formatRange);
            }
            finally
            {
                ReleaseDC(textBox.Handle, hDC);
            }

            return (int)((formatRange.rc.Bottom - formatRange.rc.Top) / AnInch);
        }

        public static class Util
        {
            public static int LOWORD(IntPtr n)
            {
                return LOWORD(unchecked((int)(long)n));
            }

            public static int LOWORD(int n)
            {
                return n & 0xffff;
            }

            public static int HIWORD(int n)
            {
                return (n >> 16) & 0xffff;
            }

            public static int HIWORD(IntPtr n)
            {
                return HIWORD(unchecked((int)(long)n));
            }

            public static int SignedHIWORD(IntPtr n)
            {
                return SignedHIWORD(unchecked((int)(long)n));
            }
            public static int SignedLOWORD(IntPtr n)
            {
                return SignedLOWORD(unchecked((int)(long)n));
            }

            public static int SignedHIWORD(int n)
            {
                return (short)((n >> 16) & 0xffff);
            }

            public static int SignedLOWORD(int n)
            {
                return (short)(n & 0xFFFF);
            }
        }
    }
}
