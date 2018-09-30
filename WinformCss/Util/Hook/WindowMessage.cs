using System;
using System.Runtime.InteropServices;

namespace WinformsTheme.Util.Hook
{

    public enum WindowMessage
    {
        Create = 0x0001,
        Destroy = 0x0002,

        SetFocus = 0x0007,
        KillFocus = 0x0008,

        Paint = 0x000F,
        Close = 0x0010,

        Quit = 0x0012,
        EraseBackground = 0x0014,
        SystemColorChange = 0x0015,

        ActivateApplication = 0x001C,
        FontChange = 0x001D,
        TimeChange = 0x001E,

        ContextMenu = 0x007B,
        StyleChanging = 0x007C,
        StyleChanged = 0x007D,

        NonClientCreate = 0x0081,
        NonClientDestroy = 0x0082,
        NonClientCalcSize = 0x0083,
        NonClientHitTest = 0x0084,
        NonClientPaint = 0x0085,
        NonClientActivate = 0x0086,
        NonClientMouseMove = 0x00A0,
        NonClientLeftButtonDown = 0x00A1,
        NonClientLeftButtonUp = 0x00A2,
        NonClientLeftDoubleClick = 0x00A3,
        NonClientRightButtonDown = 0x00A4,
        NonClientRightButtonUp = 0x00A5,
        NonClientRightDoubleClick = 0x00A6,

        KeyDown = 0x0100,
        KeyUp = 0x0101,
        Command = 0x0111,

        ListBoxAddString = 0x0180,
        ListBoxInsertString = 0x0181,
        ListBoxDeleteString = 0x0182,
        ListBoxGetText = 0x0189,
        ListBoxGetTextLength = 0x018A,
        ListBoxGetCount = 0x018B

        /*
		#define LB_SELITEMRANGEEX       0x0183
		#define LB_RESETCONTENT         0x0184
		#define LB_SETSEL               0x0185
		#define LB_SETCURSEL            0x0186
		#define LB_GETSEL               0x0187
		#define LB_GETCURSEL            0x0188
		#define LB_GETTEXT              0x0189
		#define LB_GETTEXTLEN           0x018A
		#define LB_GETCOUNT             0x018B
		#define LB_SELECTSTRING         0x018C
		#define LB_DIR                  0x018D
		#define LB_GETTOPINDEX          0x018E
		#define LB_FINDSTRING           0x018F
		#define LB_GETSELCOUNT          0x0190
		#define LB_GETSELITEMS          0x0191
		#define LB_SETTABSTOPS          0x0192
		#define LB_GETHORIZONTALEXTENT  0x0193
		#define LB_SETHORIZONTALEXTENT  0x0194
		#define LB_SETCOLUMNWIDTH       0x0195
		#define LB_ADDFILE              0x0196
		#define LB_SETTOPINDEX          0x0197
		#define LB_GETITEMRECT          0x0198
		#define LB_GETITEMDATA          0x0199
		#define LB_SETITEMDATA          0x019A
		#define LB_SELITEMRANGE         0x019B
		#define LB_SETANCHORINDEX       0x019C
		#define LB_GETANCHORINDEX       0x019D
		#define LB_SETCARETINDEX        0x019E
		#define LB_GETCARETINDEX        0x019F
		#define LB_SETITEMHEIGHT        0x01A0
		#define LB_GETITEMHEIGHT        0x01A1
		#define LB_FINDSTRINGEXACT      0x01A2
		#define LB_SETLOCALE            0x01A5
		#define LB_GETLOCALE            0x01A6
		#define LB_SETCOUNT             0x01A7
		*/
    }
}
