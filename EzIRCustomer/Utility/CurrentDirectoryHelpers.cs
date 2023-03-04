using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Utility
{
    internal class CurrentDirectoryHelpers
    {
        internal const string AspNetCoreModuleDll = "aspnetcorev2_inprocess.dll";

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [System.Runtime.InteropServices.DllImport(AspNetCoreModuleDll)]
        private static extern int Http_get_application_properties(ref IISConfigurationData iiConfigData);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct IISConfigurationData
        {
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            public readonly string PwzFullApplicationPath;
            private readonly IntPtr pNativeApplication;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            private readonly string pwzVirtualApplicationPath;

            private readonly bool fWindowsAuthEnabled;
            private readonly bool fBasicAuthEnabled;
            private readonly bool fAnonymousAuthEnable;
        }

        /// <summary>
        /// set thư mục gốc về thư mục chứa app
        /// vì .net core 2.2.1 trở về trước khi chạy app trên IIS ,  GetCurrentDirectory() trả về thư mục process được chạy
        /// ví dụ  C:\Windows\System32\inetsrv khi chạy w3wp.exe
        /// link: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/aspnet-core-module?view=aspnetcore-2.2
        /// </summary>
        public static void SetCurrentDirectory()
        {
            try
            {
                // Check if physical path was provided by ANCM
                var sitePhysicalPath = Environment.GetEnvironmentVariable("ASPNETCORE_IIS_PHYSICAL_PATH");
                if (string.IsNullOrEmpty(sitePhysicalPath))
                {
                    // Skip if not running ANCM InProcess
                    if (GetModuleHandle(AspNetCoreModuleDll) == IntPtr.Zero)
                    {
                        return;
                    }

                    IISConfigurationData configurationData = default;
                    if (Http_get_application_properties(ref configurationData) != 0)
                    {
                        return;
                    }

                    sitePhysicalPath = configurationData.PwzFullApplicationPath;
                }

                Environment.CurrentDirectory = sitePhysicalPath;
            }
            catch
            {
                // ignore
            }
        }
    }
}
