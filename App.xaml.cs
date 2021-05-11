using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyNotepad {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {
        #region Declaration
        private Mutex Mutex = new Mutex(false, "hogehoge");
        #endregion

        #region Event
        private void AppStart(object sender, StartupEventArgs e) {
            if (!this.Mutex.WaitOne(0, false)) {
                // 起動済みのウィンドウをアクティブにする
                IntPtr hMWnd = NativeMethods.FindWindow(null, "MyNotepad");
                if (hMWnd != null && NativeMethods.IsWindow(hMWnd)) {
                    var hCWnd = NativeMethods.GetLastActivePopup(hMWnd);
                    if (hCWnd != null && NativeMethods.IsWindow(hCWnd) && NativeMethods.IsWindowVisible(hCWnd)) {
                        NativeMethods.ShowWindow(hCWnd, (int)SW.SHOWNORMAL);
                        NativeMethods.SetForegroundWindow(hCWnd);
                    }
                }
                // ここまで
                this.Mutex.Close();
                this.Mutex = null;
                Shutdown();
            }
        }

        private void AppExit(object sender, ExitEventArgs e) {
            if (this.Mutex != null) {
                this.Mutex.ReleaseMutex();
                this.Mutex.Close();
            }
        }
        #endregion
    }

    class NativeMethods {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetLastActivePopup(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
    public enum SW {
        HIDE = 0,
        SHOWNORMAL = 1,
        SHOWMINIMIZED = 2,
        SHOWMAXIMIZED = 3,
        SHOWNOACTIVATE = 4,
        SHOW = 5,
        MINIMIZE = 6,
        SHOWMINNOACTIVE = 7,
        SHOWNA = 8,
        RESTORE = 9,
        SHOWDEFAULT = 10,
    }
}
