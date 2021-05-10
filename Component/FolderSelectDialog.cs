using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace MyNotepad.Component {
    public class FolderSelectDialog {

        #region Public Property
        public string Path { get; set; }
        public string Title { get; set; }
        #endregion

        #region Public Method
        public bool ShowDialog() {
            return ShowDialog(IntPtr.Zero);
        }

        public bool ShowDialog(Window owner) {
            var handle = new WindowInteropHelper(owner).Handle;
            return ShowDialog(handle);
        }
        #endregion

        #region Private Method
        private bool ShowDialog(IntPtr owner) {
            var dlg = new FileOpenDialogInternal() as IFileOpenDialog;
            try {
                dlg.SetOptions(FOS.FOS_PICKFOLDERS | FOS.FOS_FORCEFILESYSTEM);

                IShellItem item;
                if (!string.IsNullOrEmpty(this.Path)) {
                    IntPtr idl;
                    uint atts = 0;
                    if (NativeMethods.SHILCreateFromPath(this.Path, out idl, ref atts) == 0) {
                        if (NativeMethods.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, out item) == 0) {
                            dlg.SetFolder(item);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.Title)) {
                    dlg.SetTitle(this.Title);
                }

                var hr = dlg.Show(owner);
                if (hr.Equals(NativeMethods.ERROR_CANCELLED)) {
                    return false;
                }
                if (!hr.Equals(0)) {
                    return false;
                }

                dlg.GetResult(out item);
                string outputPath;
                item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out outputPath);
                this.Path = outputPath;

                return true;
            } finally {
                Marshal.FinalReleaseComObject(dlg);
            }
        }
        #endregion

        #region Api Declaration
        [ComImport]
        [Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
        private class FileOpenDialogInternal {
        }

        [ComImport]
        [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileOpenDialog {
            [PreserveSig]
            UInt32 Show([In] IntPtr hwndParent);
            void SetFileTypes();     // not fully defined
            void SetFileTypeIndex();     // not fully defined
            void GetFileTypeIndex();     // not fully defined
            void Advise(); // not fully defined
            void Unadvise();
            void SetOptions([In] FOS fos);
            void GetOptions(); // not fully defined
            void SetDefaultFolder(); // not fully defined
            void SetFolder(IShellItem psi);
            void GetFolder(); // not fully defined
            void GetCurrentSelection(); // not fully defined
            void SetFileName();  // not fully defined
            void GetFileName();  // not fully defined
            void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            void SetOkButtonLabel(); // not fully defined
            void SetFileNameLabel(); // not fully defined
            void GetResult(out IShellItem ppsi);
            void AddPlace(); // not fully defined
            void SetDefaultExtension(); // not fully defined
            void Close(); // not fully defined
            void SetClientGuid();  // not fully defined
            void ClearClientData();
            void SetFilter(); // not fully defined
            void GetResults(); // not fully defined
            void GetSelectedItems(); // not fully defined
        }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem {
            void BindToHandler(); // not fully defined
            void GetParent(); // not fully defined
            void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            void GetAttributes();  // not fully defined
            void Compare();  // not fully defined
        }

        private enum SIGDN : uint // not fully defined
        {
            SIGDN_FILESYSPATH = 0x80058000,
        }

        [Flags]
        private enum FOS // not fully defined
        {
            FOS_FORCEFILESYSTEM = 0x40,
            FOS_PICKFOLDERS = 0x20,
        }

        private class NativeMethods {
            [DllImport("shell32.dll")]
            public static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

            [DllImport("shell32.dll")]
            public static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out IShellItem ppsi);

            public const uint ERROR_CANCELLED = 0x800704C7;
        }
        #endregion
    }
}
