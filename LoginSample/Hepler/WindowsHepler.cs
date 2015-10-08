using System;
using System.Runtime.InteropServices;

namespace LoginSample.Hepler
{
    public class WindowsHepler
    {
        const int GCL_STYLE = (-26);
        const int CS_DROPSHADOW = 0x20000;

        /// <summary> 
        /// window的基本样式 
        /// </summary> 
        public const int GWL_STYLE = -16;

        /// <summary> 
        /// 带有外边框和标题的windows的样式 
        /// </summary> 
        public const int WS_CAPTION = 0xC00000;

        public const int WS_CAPTION_2 = 0xC0000;

        /// <summary> 
        /// 设置窗体的样式 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="oldStyle">进行设置窗体的样式类型.</param> 
        /// <param name="newStyle">新样式</param> 
        [DllImport("User32.dll")]

        public static extern int SetWindowLong(IntPtr handle, int oldStyle, Int32 newStyle);


        /// <summary> 
        /// 获取窗体指定的样式. 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="style">要进行返回的样式</param> 
        /// <returns>当前window的样式</returns> 
        [DllImport("user32.dll")]

        public static extern int GetWindowLong(IntPtr handle, int style);



        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        //=================================================================================

        /// <summary>
        /// 设置窗体为无边框风格
        /// </summary>
        /// <param name="hWnd"></param>
        public static void SetWindowNoBorder(IntPtr hWnd)
        {
            var oldstyle = GetWindowLong(hWnd, GWL_STYLE);

            SetWindowLong(hWnd, GWL_STYLE, oldstyle & (~(WS_CAPTION | WS_CAPTION_2)));
            //SetWindowLong(hWnd, GWL_EXSTYLE, WS_CHILD);
            SetClassLong(hWnd, GCL_STYLE, GetClassLong(hWnd, GCL_STYLE) | CS_DROPSHADOW); //API函数加载，实现窗体边框阴影效果
        }
    }
}
