#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Supermodel.Tooling.SolutionMaker
{
    public static class SolutionMaker
    {
        #region CreateSupermodelShell Methods
        public static void CreateSupermodelShell(ISolutionMakerParams solutionMakerParams)
        {
            //Create path
            var path = solutionMakerParams.CalculateFullPath();
            
            //Create dir and extract files into it
            if (Directory.Exists(path)) throw new CreatorException($"Unable to create the new Solution.\n\nDirectory '{path}' already exists.");
            Directory.CreateDirectory(path);
            // ReSharper disable once AssignNullToNotNullAttribute
            ZipFile.ExtractToDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ZipFileName), path);

            //Adjust for Xamarin.Forms UI vs Native UI
            AdjustForMobileApi(solutionMakerParams.MobileApi, path);

            //Adjust for WM vs MVC
            AdjustForWebFramework(solutionMakerParams.WebFramework, path);

            //Adjust for Database
            AdjustForDatabase(solutionMakerParams.Database, path);

            //Auto-assign random port
            var newPort =  Random.Next(41000, 59000);
            ReplaceInDir(path, "54208", newPort.ToString(), "SolutionMaker.cs");

            //Replace IP address
            var newIP = GetServerIpAddress();
            ReplaceInDir(path, "10.211.55.9", newIP, "SolutionMaker.cs");

            //Register MVC with netsh
            if (solutionMakerParams.WebFramework == WebFrameworkEnum.Mvc) RegisterMvcWithNetsh(path);

            //Update batch files to pause after execution
            if (solutionMakerParams.WebFramework == WebFrameworkEnum.WebMonk) UpdateBatchFileToPauseAfterExecution(Path.Combine(path, @"XXYXX\Util\ModelGeneratorWM\RegisterSiteWithNetsh.bat"));
            if (solutionMakerParams.WebFramework == WebFrameworkEnum.Mvc) UpdateBatchFileToPauseAfterExecution(Path.Combine(path, @"XXYXX\Util\ModelGeneratorMVC\RegisterSiteWithNetsh.bat"));



        }

        private static void AdjustForMobileApi(MobileApiEnum mobileApi, string path)
        {
            if (mobileApi == MobileApiEnum.XamarinForms)
            {
                //Droid
                File.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.cs"));
                File.Move(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.XamarinForms.cs"), Path.Combine(path, @"\XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.cs"));

                //iOS
                File.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.cs"));
                File.Move(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.XamarinForms.cs"), Path.Combine(path, @"\XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.cs"));
            }
            else
            {
                //Droid
                File.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.XamarinForms.cs"));

                //iOS
                File.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.XamarinForms.cs"));

                //Mobile
                Directory.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile\AppCore"), true);
                Directory.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile\EmbeddedResources"), true);
                Directory.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile\Models"), true);
                Directory.Delete(Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile\Pages"), true);

                //Remove icon as embedded resource
                var assemblyName = typeof(SolutionMaker).Assembly.GetName().Name;
                var xxyxxMobileProjFile = Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile\XXYXX.Mobile.csproj");
                var xxyxxMobileProjFileContent = File.ReadAllText(xxyxxMobileProjFile);
                
                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.XXYXXMobileProjIfNativeAPI.snippet1.txt");
                xxyxxMobileProjFileContent = xxyxxMobileProjFileContent.RemoveStrWithCheck(snippet1);

                File.WriteAllText(xxyxxMobileProjFile, xxyxxMobileProjFileContent);
            }
        }
        private static void AdjustForWebFramework(WebFrameworkEnum webFramework, string path)
        {
            var solutionFile = Path.Combine(path, @"XXYXX.Core.sln");
            var solutionFileContent = File.ReadAllText(solutionFile);

            var webApiDataContextFile = Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile\Supermodel\Persistence\XXYXXWebApiDataContext.cs");
            var webApiDataContextFileContent = File.ReadAllText(webApiDataContextFile);

            var mobileModelsForRuntimeFile = Path.Combine(path, @"XXYXX\Mobile\XXYXX.Mobile\Supermodel\ModelsForRuntime\Supermodel.Mobile.ModelsForRuntime.cs");
            var mobileModelsForRuntimeFileContent = File.ReadAllText(mobileModelsForRuntimeFile);

            var assemblyName = typeof(SolutionMaker).Assembly.GetName().Name;

            if (webFramework == WebFrameworkEnum.WebMonk)
            {
                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWMSelected.snippet1.txt");
                var snippet2 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWMSelected.snippet2.txt");
                var snippet3 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWMSelected.snippet3.txt");
                var snippet4 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWMSelected.snippet4.txt");
                var snippet5 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWMSelected.snippet5.txt");
                var snippet6 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWMSelected.snippet6.txt");
                
                solutionFileContent = solutionFileContent
                    .RemoveStrWithCheck(snippet1)
                    .RemoveStrWithCheck(snippet2)
                    .RemoveStrWithCheck(snippet3)
                    .RemoveStrWithCheck(snippet4)
                    .RemoveStrWithCheck(snippet5)
                    .RemoveStrWithCheck(snippet6);

                Directory.Delete(Path.Combine(path, @"XXYXX\Server\WebMVC"), true);
                Directory.Delete(Path.Combine(path, @"XXYXX\Server\BatchApiClientMVC"), true);
                Directory.Delete(Path.Combine(path, @"XXYXX\Util\ModelGeneratorMVC"), true);

                //Modify XXYXXWebApiDataContext.cs to have the right web api endpoint
                webApiDataContextFileContent = webApiDataContextFileContent.RemoveStrWithCheck(@"//public override string BaseUrl => ""http://10.211.55.9:54208/""; //this one is for MVC");

                //We do not modify runtime models to update RestUrl attribute because WM is the default
            }
            else
            {
                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVCSelected.snippet1.txt");
                var snippet2 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVCSelected.snippet2.txt");
                var snippet3 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVCSelected.snippet3.txt");
                var snippet4 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVCSelected.snippet4.txt");
                var snippet5 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVCSelected.snippet5.txt");
                var snippet6 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVCSelected.snippet6.txt");
                
                solutionFileContent = solutionFileContent
                    .RemoveStrWithCheck(snippet1)
                    .RemoveStrWithCheck(snippet2)
                    .RemoveStrWithCheck(snippet3)
                    .RemoveStrWithCheck(snippet4)
                    .RemoveStrWithCheck(snippet5)
                    .RemoveStrWithCheck(snippet6);

                Directory.Delete(Path.Combine(path, @"XXYXX\Server\WebWM"), true);
                Directory.Delete(Path.Combine(path, @"XXYXX\Server\BatchApiClientWM"), true);
                Directory.Delete(Path.Combine(path, @"XXYXX\Util\ModelGeneratorWM"), true);

                //Modify XXYXXWebApiDataContext.cs to have the right web api endpoint
                webApiDataContextFileContent = webApiDataContextFileContent.ReplaceStrWithCheck(@"//public override string BaseUrl => ""http://10.211.55.9:54208/""; //this one is for MVC", @"public override string BaseUrl => ""http://10.211.55.9:54208/"";");
                webApiDataContextFileContent = webApiDataContextFileContent.RemoveStrWithCheck(@"public override string BaseUrl => ""http://10.211.55.9:54208/api/""; //this one is for WM");

                //Modify runtime models to update RestUrl attribute
                mobileModelsForRuntimeFileContent = mobileModelsForRuntimeFileContent.ReplaceStrWithCheck(@"[RestUrl(""XXYXXUserUpdatePassword"")]", @"[RestUrl(""XXYXXUserUpdatePasswordApi"")]");
            }

            File.WriteAllText(mobileModelsForRuntimeFile, mobileModelsForRuntimeFileContent);
            File.WriteAllText(webApiDataContextFile, webApiDataContextFileContent);
            File.WriteAllText(solutionFile, solutionFileContent);
        }
        private static void AdjustForDatabase(DatabaseEnum database, string path)
        {
            //Sqlite is the default
            if (database == DatabaseEnum.SqlServer)
            {
                var assemblyName = typeof(SolutionMaker).Assembly.GetName().Name;

                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.snippet1.txt");
                var replacement1 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.replacement1.txt");
                var snippet2 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.snippet2.txt");
                var replacement2 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.replacement2.txt");

                var dataContextFile = Path.Combine(path, @"XXYXX\Server\Domain\Supermodel\Persistence\DataContext.cs");
                var dataContextFileContent = File.ReadAllText(dataContextFile);

                dataContextFileContent = dataContextFileContent
                    .ReplaceStrWithCheck(snippet1, replacement1)
                    .ReplaceStrWithCheck(snippet2, replacement2)
                    .ReplaceStrWithCheck("using Supermodel.Persistence.EFCore.SQLite;", "using Supermodel.Persistence.EFCore.SQLServer;");

                File.WriteAllText(dataContextFile, dataContextFileContent);
            }
        }
        private static void RegisterMvcWithNetsh(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var batFile = Path.Combine(path, @"XXYXX\Util\ModelGeneratorMVC\RegisterSiteWithNetsh.bat");
                
                var info = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = true,
                    Arguments = $"/c \"{batFile}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Verb = "runas"
                };
                
                try
                {
                    var process = Process.Start(info);
                    if (process == null) throw new Exception("batch process after starting is null");
                    process.WaitForExit();
                    if (process.ExitCode != 0) throw new Exception($"returned exit code {process.ExitCode}");
                }
                catch (Win32Exception ex1)
                {
                    const int errorCancelled = 1223; //The operation was canceled by the user.
                    if (ex1.NativeErrorCode == errorCancelled) throw new CreatorException("You must allow Administrator access in order to register web projects with netsh.");
                    throw;
                }
                catch (Exception ex2)
                {
                    throw new CreatorException($"Error executing RegisterSitesWithIISExpress.bat: {ex2.Message}");
                }
            }
        }
        private static void UpdateBatchFileToPauseAfterExecution(string filePath)
        {
            var registerSiteWithIISExpressFile = Path.Combine(filePath);
            var registerSiteWithIISExpressFileContent = File.ReadAllText(registerSiteWithIISExpressFile);
            registerSiteWithIISExpressFileContent = registerSiteWithIISExpressFileContent.ReplaceStrWithCheck("rem pause", "pause");
            File.WriteAllText(registerSiteWithIISExpressFile, registerSiteWithIISExpressFileContent);
        }
        #endregion

        #region CreateSnpshot Methods
        public static void CreateSnapshot(string projectTemplateDirectory, string? destinationDir = null)
        {
            Console.WriteLine("Deleting files and directories...");
            DeleteWhatWeDoNotNeedForSnapshot(projectTemplateDirectory);
            Console.WriteLine("Done Deleting.");

            var zipFileNamePath = ZipFileName;
            if (destinationDir != null) zipFileNamePath = Path.Combine(destinationDir, ZipFileName);

            if (File.Exists(zipFileNamePath)) File.Delete(zipFileNamePath);
            ZipFile.CreateFromDirectory(projectTemplateDirectory, zipFileNamePath);
        }
        public static void DeleteWhatWeDoNotNeedForSnapshot(string directory)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                var ext = Path.GetExtension(file);
                var fileName = Path.GetFileName(file);
                if (fileName == "project.lock.json" || fileName == ZipFileName || ext == ".suo" || ext == ".user") 
                {
                    Console.WriteLine(Path.GetFullPath(file));
                    File.Delete(file);
                }
            }

            foreach (var dir in Directory.GetDirectories(directory))
            {
                var dirName = Path.GetFileName(dir);
                if (dirName == "bin" || dirName == "obj" || dirName == "packages") 
                {
                    Console.WriteLine(Path.GetFullPath(dir));
                    Directory.Delete(dir, true);
                }
                else DeleteWhatWeDoNotNeedForSnapshot(dir);
            }
        }
        #endregion

        #region Helper Methods
        private static void ReplaceInDir(string directory, string oldStr, string newStr, params string[]? ignoreFileNames)
        {
            //Ignore Frameworks directory
            var directoryName = Path.GetFileName(directory);
            if (directoryName == "Frameworks") return;

            //Rename dir
            var newDirectory = directory.Replace(oldStr, newStr);
            if (directory.Contains(oldStr) && newStr != oldStr) Directory.Move(directory, newDirectory);

            foreach (var file in Directory.GetFiles(newDirectory))
            {
                var fileName = Path.GetFileName(file);
                if (ignoreFileNames != null && ignoreFileNames.Any(x => x == fileName)) continue;
                
                //Replace marker in file contents
                var fileContents = File.ReadAllText(file);
                if (fileContents.Contains(oldStr))
                {
                    fileContents = fileContents.Replace(oldStr, newStr);
                    File.WriteAllText(file, fileContents);
                }

                //Replace marker in file name 
                if (file.Contains(oldStr) && newStr != oldStr) File.Move(file, file.Replace(oldStr, newStr));
            }

            foreach (var subDir in Directory.GetDirectories(newDirectory)) ReplaceInDir(subDir, oldStr, newStr, ignoreFileNames);
        }
        private static string RemoveStrWithCheck(this string me, string str)
        {
            if (!me.Contains(str)) throw new Exception($"RemoveStrWithCheck: '{str.Substring(0, 60)}...' not found. \n" + GetStackTrace());
            return me.Replace(str, "");
        }
        private static string ReplaceStrWithCheck(this string me, string str1, string str2)
        {
            if (!me.Contains(str1)) throw new Exception($"ReplaceStrWithCheck: '{str1.Substring(0, 60)}...' not found. \n" + GetStackTrace());
            return me.Replace(str1, str2);
        }
        private static string ReadResourceTextFile(string resourceName)
        {
            using (var stream = typeof(SolutionMaker).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new ArgumentException(resourceName + " is not found.");
                using (var reader = new StreamReader(stream)) { return reader.ReadToEnd(); }
            }
        }
        private static string GetServerIpAddress()
        {
            var localIp = "?";

            //try to find an ip using new method
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var ipProperties = ni.GetIPProperties();
                    foreach (var ip in ipProperties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork && ipProperties.DnsSuffix == "localdomain")
                        {
                            localIp = ip.Address.ToString();
                        }
                    }
                }
            }

            //if new method did not work, use old method
            if (localIp == "?")
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var addressList = host.AddressList;
                foreach (var ip in addressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) localIp = ip.ToString();
                }
            }

            return localIp;
        }
        private static string GetStackTrace()
        {
            var stackTrace = new StackTrace();
            var stackFrames = stackTrace.GetFrames();
            var sb = new StringBuilder();
            if (stackFrames != null)
            {
                for(var i = 1; i < stackFrames.Length; i++)
                {
                    sb.Append($"\n{stackFrames[i].GetMethod()}");
                }
            }
            return sb.ToString();
        }        
        #endregion

        #region Properties and Contants
        private static Random Random { get; } = new Random(Guid.NewGuid().GetHashCode());
        private const string Marker2 = "XXYXX";
        public const string ZipFileName = "SupermodelSolutionTemplate.XXYXX.zip";
        #endregion
    }
}
