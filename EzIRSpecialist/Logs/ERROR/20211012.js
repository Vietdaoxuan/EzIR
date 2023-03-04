2021-10-12 16:23:38.643 +07:00 [ERR] =================
                Message     = Could not find a part of the path 'E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\Upload\CVManager\File_1_CBTT_Báocáotàichính_test_download.pdf'.
                StackTrace  = at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)
   at System.IO.FileStream.CreateFileOpenHandle(FileMode mode, FileShare share, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access)
   at CoreLib.Commons.CheckUtils.ReadFile(String filePath) in E:\Devops\EZIR\Code\EzIR\CoreLib\Commons\CheckUtils.cs:line 229
   at EzIRSpecialist.Controllers.BaoCaoTienIch.ViewApproveController.DownloadFile(ManagerViewModel managerViewModel) in E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\Controllers\BaoCaoTienIch\ViewApproveController.cs:line 494
                TargetSite  = Microsoft.Win32.SafeHandles.SafeFileHandle ValidateFileHandle(Microsoft.Win32.SafeHandles.SafeFileHandle)
2021-10-12 16:58:30.027 +07:00 [ERR] =================
                Message     = Path cannot be null. (Parameter 'path')
                StackTrace  = at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access)
   at CoreLib.Commons.CheckUtils.ReadFile(String filePath) in E:\Devops\EZIR\Code\EzIR\CoreLib\Commons\CheckUtils.cs:line 229
   at EzIRSpecialist.Controllers.BaoCaoTienIch.ViewApproveController.DownloadFile(ManagerViewModel managerViewModel) in E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\Controllers\BaoCaoTienIch\ViewApproveController.cs:line 494
                TargetSite  = Void .ctor(System.String, System.IO.FileMode, System.IO.FileAccess, System.IO.FileShare, Int32, System.IO.FileOptions)
2021-10-12 17:21:58.422 +07:00 [ERR] =================
                Message     = The filename, directory name, or volume label syntax is incorrect. : 'E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\https:\localhost:44378\'
                StackTrace  = at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)
   at System.IO.FileStream.CreateFileOpenHandle(FileMode mode, FileShare share, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access)
   at CoreLib.Commons.CheckUtils.ReadFile(String filePath) in E:\Devops\EZIR\Code\EzIR\CoreLib\Commons\CheckUtils.cs:line 229
   at EzIRSpecialist.Controllers.BaoCaoTienIch.ViewApproveController.DownloadFile(ManagerViewModel managerViewModel) in E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\Controllers\BaoCaoTienIch\ViewApproveController.cs:line 500
                TargetSite  = Microsoft.Win32.SafeHandles.SafeFileHandle ValidateFileHandle(Microsoft.Win32.SafeHandles.SafeFileHandle)
2021-10-12 17:25:44.604 +07:00 [ERR] =================
                Message     = The filename, directory name, or volume label syntax is incorrect. : 'E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\https:\localhost:44378\'
                StackTrace  = at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)
   at System.IO.FileStream.CreateFileOpenHandle(FileMode mode, FileShare share, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access)
   at CoreLib.Commons.CheckUtils.ReadFile(String filePath) in E:\Devops\EZIR\Code\EzIR\CoreLib\Commons\CheckUtils.cs:line 229
   at EzIRSpecialist.Controllers.BaoCaoTienIch.ViewApproveController.DownloadFile(ManagerViewModel managerViewModel) in E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\Controllers\BaoCaoTienIch\ViewApproveController.cs:line 507
                TargetSite  = Microsoft.Win32.SafeHandles.SafeFileHandle ValidateFileHandle(Microsoft.Win32.SafeHandles.SafeFileHandle)
2021-10-12 17:30:04.660 +07:00 [ERR] =================
                Message     = The filename, directory name, or volume label syntax is incorrect. : 'E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\https:\localhost:44378\Upload\CVManager\File_1_CBTT_Tinkhác_QĐxửphạtthuế.pdf'
                StackTrace  = at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)
   at System.IO.FileStream.CreateFileOpenHandle(FileMode mode, FileShare share, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access)
   at CoreLib.Commons.CheckUtils.ReadFile(String filePath) in E:\Devops\EZIR\Code\EzIR\CoreLib\Commons\CheckUtils.cs:line 229
   at EzIRSpecialist.Controllers.BaoCaoTienIch.ViewApproveController.DownloadFile(ManagerViewModel managerViewModel) in E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\Controllers\BaoCaoTienIch\ViewApproveController.cs:line 507
                TargetSite  = Microsoft.Win32.SafeHandles.SafeFileHandle ValidateFileHandle(Microsoft.Win32.SafeHandles.SafeFileHandle)
2021-10-12 17:39:14.523 +07:00 [ERR] =================
                Message     = Could not find file 'E:\Devops\EZIR\Code\EzIR\EzIRCustomer\${data}'.
                StackTrace  = at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)
   at System.IO.FileStream.CreateFileOpenHandle(FileMode mode, FileShare share, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access)
   at CoreLib.Commons.CheckUtils.ReadFile(String filePath) in E:\Devops\EZIR\Code\EzIR\CoreLib\Commons\CheckUtils.cs:line 229
   at EzIRSpecialist.Controllers.BaoCaoTienIch.ViewApproveController.DownloadFile(ManagerViewModel managerViewModel) in E:\Devops\EZIR\Code\EzIR\EzIRSpecialist\Controllers\BaoCaoTienIch\ViewApproveController.cs:line 506
                TargetSite  = Microsoft.Win32.SafeHandles.SafeFileHandle ValidateFileHandle(Microsoft.Win32.SafeHandles.SafeFileHandle)
