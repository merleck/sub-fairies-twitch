using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

//WARNING!! Running this code inside Unity when not in a build will make the entire development environment transparent
//Restarting Unity would however resolve

public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
    static extern int SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);
    [DllImport("user32.dll")]
    static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);
    [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
    private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern long GetWindowLong(IntPtr hwnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);


    void Start()
    {
#if !UNITY_EDITOR   // You really don't want to enable this in the editor..
        var margins = new MARGINS() { cxLeftWidth = -1 };

        var handle = GetActiveWindow();
        int fWidth = Screen.width;
        int fHeight = Screen.height;

        //Remove title bar
        long lCurStyle = GetWindowLong(handle, -16);     // GWL_STYLE=-16
        int a = 12582912;   //WS_CAPTION  = 0x00C00000L
        int b = 1048576;    //WS_HSCROLL  = 0x00100000L
        int c = 2097152;    //WS_VSCROLL  = 0x00200000L
        int d = 524288;     //WS_SYSMENU  = 0x00080000L
        int e = 16777216;   //WS_MAXIMIZE = 0x01000000L
        uint f = 268435456;  //WS_VISIBLE  = 0x10000000L
        uint g = 2147483648; //WS_POPUP    = 0x80000000L

        //lCurStyle &= ~(a | b | c | d);
        //lCurStyle &= e;
        //lCurStyle &= f;
        //lCurStyle &= g;

        SetWindowLong(handle, -16, f | g);// GWL_STYLE=-16

        // Transparent windows with click through
        SetWindowLong(handle, -20, 524288 | 32);//GWL_EXSTYLE=-20; WS_EX_LAYERED=524288=&h80000, WS_EX_TRANSPARENT=32=0x00000020L
        SetLayeredWindowAttributes(handle, 0, 255, 2);// Transparency=51=20%, LWA_ALPHA=2

        DwmExtendFrameIntoClientArea(handle, ref margins);
        //SetWindowPos(handle, 0, 0, 0, fWidth, fHeight, 32 | 64); //SWP_FRAMECHANGED = 0x0020 (32); //SWP_SHOWWINDOW = 0x0040 (64)
        ShowWindowAsync(handle, 3); //Forces window to show in case of unresponsive app    // SW_SHOWMAXIMIZED(3)
#endif
    }

    private void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }
}