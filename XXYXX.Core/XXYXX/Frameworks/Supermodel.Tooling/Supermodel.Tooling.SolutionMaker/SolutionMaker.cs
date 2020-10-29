﻿#nullable enable

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
using System.Security.Cryptography;
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
            ZipFile.ExtractToDirectory(CombineAndAdjustPaths(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ZipFileName), path);

            //Adjust version
            const string versionMarker = "Version X.YY.ZZZ";
            ReplaceInFile(CombineAndAdjustPaths(path, @"Frameworks\Version.txt"), versionMarker, $"Version {SolutionMaker.Version}");

            //Adjust for Xamarin.Forms UI vs Native UI
            AdjustForMobileApi(solutionMakerParams.MobileApi, path);

            //Adjust for WM vs MVC
            AdjustForWebFramework(solutionMakerParams.WebFramework, path);

            //Adjust for Database
            AdjustForDatabase(solutionMakerParams.Database, path);

            //Auto-assign random port
            const string oldPort = "54208";
            var newPort =  Random.Next(41000, 59000);
            ReplaceInDir(path, oldPort, newPort.ToString(), "SolutionMaker.cs");

            //Replace IP address
            const string oldIPAddress = "10.211.55.9";
            var newIP = GetServerIpAddress();
            ReplaceInDir(path, oldIPAddress, newIP, "SolutionMaker.cs");

            //Register MVC with netsh
            if (solutionMakerParams.WebFramework == WebFrameworkEnum.Mvc) RegisterMvcWithNetsh(path);

            //Update batch files to pause after execution
            if (solutionMakerParams.WebFramework == WebFrameworkEnum.WebMonk) UpdateBatchFileToPauseAfterExecution(CombineAndAdjustPaths(path, @"XXYXX\Util\ModelGeneratorWM\RegisterSiteWithNetsh.bat"));
            if (solutionMakerParams.WebFramework == WebFrameworkEnum.Mvc) UpdateBatchFileToPauseAfterExecution(CombineAndAdjustPaths(path, @"XXYXX\Util\ModelGeneratorMVC\RegisterSiteWithNetsh.bat"));

            //Replace GUIDs
            ReplaceGuidsInDir(path);

            //Generate new random key for encrypting username/password locally
            GenerateNewRandomEncryptionKeyForLocalStorage(path);

            //Generate new random key for secure auth
            GenerateNewRandomEncryptionKeyForSecureAuth(path);

            //Generate new secret token for secure auth
            GenerateNewSecretTokenForSecureAuth(path);

            //rename files and directories containing Marker, find and replace Marker inside the files
            const string marker = "XXYXX";
            ReplaceInDir(path, marker, solutionMakerParams.SolutionName, "SolutionMaker.cs");
        }

        private static void AdjustForMobileApi(MobileApiEnum mobileApi, string path)
        {
            if (mobileApi == MobileApiEnum.XamarinForms)
            {
                //Droid
                File.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.cs"));
                File.Move(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.XamarinForms.cs"), CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.cs"));

                //iOS
                File.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.cs"));
                File.Move(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.XamarinForms.cs"), CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.cs"));
            }
            else
            {
                //Droid
                File.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.Droid\MainActivity.XamarinForms.cs"));

                //iOS
                File.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile.iOS\AppDelegate.XamarinForms.cs"));

                //Mobile
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile\AppCore"), true);
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile\EmbeddedResources"), true);
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile\Models"), true);
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile\Pages"), true);

                //Remove icon as embedded resource
                var assemblyName = typeof(SolutionMaker).Assembly.GetName().Name;
                var xxyxxMobileProjFile = CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile\XXYXX.Mobile.csproj");
                var xxyxxMobileProjFileContent = File.ReadAllText(xxyxxMobileProjFile);
                
                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.XXYXXMobileProjIfNativeAPI.snippet1.txt");
                xxyxxMobileProjFileContent = xxyxxMobileProjFileContent.RemoveStrWithCheck(snippet1);

                File.WriteAllText(xxyxxMobileProjFile, xxyxxMobileProjFileContent);
            }
        }
        private static void AdjustForWebFramework(WebFrameworkEnum webFramework, string path)
        {
            var solutionFile = CombineAndAdjustPaths(path, "XXYXX.sln");
            var solutionFileContent = File.ReadAllText(solutionFile);

            var webApiDataContextFile = CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile\Supermodel\Persistence\XXYXXWebApiDataContext.cs");
            var webApiDataContextFileContent = File.ReadAllText(webApiDataContextFile);

            var mobileModelsForRuntimeFile = CombineAndAdjustPaths(path, @"XXYXX\Mobile\XXYXX.Mobile\Supermodel\ModelsForRuntime\Supermodel.Mobile.ModelsForRuntime.cs");
            var mobileModelsForRuntimeFileContent = File.ReadAllText(mobileModelsForRuntimeFile);

            var assemblyName = typeof(SolutionMaker).Assembly.GetName().Name;

            if (webFramework == WebFrameworkEnum.WebMonk)
            {
                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWM.snippet1.txt");
                var snippet2 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWM.snippet2.txt");
                var snippet3 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWM.snippet3.txt");
                var snippet4 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWM.snippet4.txt");
                var snippet5 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWM.snippet5.txt");
                var snippet6 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfWM.snippet6.txt");
                
                solutionFileContent = solutionFileContent
                    .RemoveStrWithCheck(snippet1)
                    .RemoveStrWithCheck(snippet2)
                    .RemoveStrWithCheck(snippet3)
                    .RemoveStrWithCheck(snippet4)
                    .RemoveStrWithCheck(snippet5)
                    .RemoveStrWithCheck(snippet6);

                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Server\WebMVC"), true);
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Server\BatchApiClientMVC"), true);
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Util\ModelGeneratorMVC"), true);

                //Modify XXYXXWebApiDataContext.cs to have the right web api endpoint
                webApiDataContextFileContent = webApiDataContextFileContent.RemoveStrWithCheck(@"//public override string BaseUrl => ""http://10.211.55.9:54208/""; //this one is for MVC");

                //We do not modify runtime models to update RestUrl attribute because WM is the default
            }
            else
            {
                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVC.snippet1.txt");
                var snippet2 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVC.snippet2.txt");
                var snippet3 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVC.snippet3.txt");
                var snippet4 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVC.snippet4.txt");
                var snippet5 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVC.snippet5.txt");
                var snippet6 = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.SolutionIfMVC.snippet6.txt");
                
                solutionFileContent = solutionFileContent
                    .RemoveStrWithCheck(snippet1)
                    .RemoveStrWithCheck(snippet2)
                    .RemoveStrWithCheck(snippet3)
                    .RemoveStrWithCheck(snippet4)
                    .RemoveStrWithCheck(snippet5)
                    .RemoveStrWithCheck(snippet6);

                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Server\WebWM"), true);
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Server\BatchApiClientWM"), true);
                Directory.Delete(CombineAndAdjustPaths(path, @"XXYXX\Util\ModelGeneratorWM"), true);

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
            var assemblyName = typeof(SolutionMaker).Assembly.GetName().Name;

            //DataContext: Sqlite is the default for data context
            if (database == DatabaseEnum.SqlServer)
            {
                var snippet1 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.snippet1.txt");
                var replacement1 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.replacement1.txt");
                var snippet2 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.snippet2.txt");
                var replacement2 = ReadResourceTextFile($"{assemblyName}.Snippets2Replace.DataContextIfSqlServer.replacement2.txt");

                var dataContextFile = CombineAndAdjustPaths(path, @"XXYXX\Server\Domain\Supermodel\Persistence\DataContext.cs");
                var dataContextFileContent = File.ReadAllText(dataContextFile);

                dataContextFileContent = dataContextFileContent
                    .ReplaceStrWithCheck(snippet1, replacement1)
                    .ReplaceStrWithCheck(snippet2, replacement2)
                    .ReplaceStrWithCheck("using Supermodel.Persistence.EFCore.SQLite;", "using Supermodel.Persistence.EFCore.SQLServer;");

                File.WriteAllText(dataContextFile, dataContextFileContent);
            }

            //Solution file
            string snippet;
            if (database == DatabaseEnum.SqlServer) snippet = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.DomainProjIfSQLServer.snippet1.txt");
            else if (database == DatabaseEnum.Sqlite) snippet = ReadResourceTextFile($"{assemblyName}.Snippets2Delete.DomainProjIfSQLite.snippet1.txt");
            else throw new Exception($"Unknown DatabaseEnum {database}");

            var solutionFile = CombineAndAdjustPaths(path, @"XXYXX\Server\Domain\Domain.csproj");
            var solutionFileContent = File.ReadAllText(solutionFile);

            solutionFileContent = solutionFileContent.RemoveStrWithCheck(snippet);

            File.WriteAllText(solutionFile, solutionFileContent);

        }
        private static void RegisterMvcWithNetsh(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var batFile = CombineAndAdjustPaths(path, @"XXYXX\Util\ModelGeneratorMVC\RegisterSiteWithNetsh.bat");
                
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
        private static void UpdateBatchFileToPauseAfterExecution(string batchFilePath)
        {
            var registerSiteWithIISExpressFile = AdjustPath(batchFilePath);
            var registerSiteWithIISExpressFileContent = File.ReadAllText(registerSiteWithIISExpressFile);
            registerSiteWithIISExpressFileContent = registerSiteWithIISExpressFileContent.ReplaceStrWithCheck("rem pause", "pause");
            File.WriteAllText(registerSiteWithIISExpressFile, registerSiteWithIISExpressFileContent);
        }
        private static void ReplaceGuidsInDir(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                var ext = Path.GetExtension(file);
                // ReSharper disable once StringLiteralTypo
                if (ext == ".csproj" || ext == ".projitems" || ext == ".shproj" || ext == ".sln")
                {
                    //Replace Guids in file contents
                    var fileContents = File.ReadAllText(file);

                    //Mobile
                    const string xxyxxMobileProjOldGuidStr = "2D56B4B2-C249-404E-8633-D1E2C25B3F01";
                    var xxyxxMobileProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(xxyxxMobileProjOldGuidStr, xxyxxMobileProjNewGuidStr);
                    fileContents = fileContents.Replace(xxyxxMobileProjOldGuidStr.ToLower(), xxyxxMobileProjNewGuidStr.ToLower());

                    const string xxyxxMobileIOSProjOldGuidStr = "335038D0-C3B3-4CCB-B92C-BF48454F86AA";
                    var xxyxxMobileIOSProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(xxyxxMobileIOSProjOldGuidStr, xxyxxMobileIOSProjNewGuidStr);
                    fileContents = fileContents.Replace(xxyxxMobileIOSProjOldGuidStr.ToLower(), xxyxxMobileIOSProjNewGuidStr.ToLower());

                    const string xxyxxMobileDroidProjOldGuidStr = "C2061A1C-D2FE-41AA-8CC4-6397089EC77F";
                    var xxyxxMobileDroidProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(xxyxxMobileDroidProjOldGuidStr, xxyxxMobileDroidProjNewGuidStr);
                    fileContents = fileContents.Replace(xxyxxMobileDroidProjOldGuidStr.ToLower(), xxyxxMobileDroidProjNewGuidStr.ToLower());

                    
                    //Server
                    const string batchProjOldGuidStr = "0A117320-9AFB-4E93-8F80-BFF93A197DE3";
                    var batchProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(batchProjOldGuidStr, batchProjNewGuidStr);
                    fileContents = fileContents.Replace(batchProjOldGuidStr.ToLower(), batchProjNewGuidStr.ToLower());

                    const string batchApiClientMvcProjOldGuidStr = "0EBAAD4F-173C-41E3-86DC-7ACEF76FC571";
                    var batchApiClientMvcProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(batchApiClientMvcProjOldGuidStr, batchApiClientMvcProjNewGuidStr);
                    fileContents = fileContents.Replace(batchApiClientMvcProjOldGuidStr.ToLower(), batchApiClientMvcProjNewGuidStr.ToLower());

                    const string batchApiClientWMProjOldGuidStr = "17CBF940-4D77-4528-86E1-3BB1E7C5EDFF";
                    var batchApiClientWMProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(batchApiClientWMProjOldGuidStr, batchApiClientWMProjNewGuidStr);
                    fileContents = fileContents.Replace(batchApiClientWMProjOldGuidStr.ToLower(), batchApiClientWMProjNewGuidStr.ToLower());

                    const string domainProjOldGuidStr = "A65A0F48-90BD-4987-8E7C-7431D2A19547";
                    var domainProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(domainProjOldGuidStr, domainProjNewGuidStr);
                    fileContents = fileContents.Replace(domainProjOldGuidStr.ToLower(), domainProjNewGuidStr.ToLower());

                    const string webMvcProjOldGuidStr = "A948AEF7-8737-49A0-A47C-0652ED858D30";
                    var webMvcProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(webMvcProjOldGuidStr, webMvcProjNewGuidStr);
                    fileContents = fileContents.Replace(webMvcProjOldGuidStr.ToLower(), webMvcProjNewGuidStr.ToLower());

                    const string webWMProjOldGuidStr = "52339205-60DC-4289-A179-9DDE6D6DA1B3";
                    var webWMProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(webWMProjOldGuidStr, webWMProjNewGuidStr);
                    fileContents = fileContents.Replace(webWMProjOldGuidStr.ToLower(), webWMProjNewGuidStr.ToLower());
                    
                    
                    //Utils
                    const string modelGeneratorMvcProjOldGuidStr = "7234523C-4609-4197-9DA5-3DC77A172D5B";
                    var modelGeneratorMvcProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(modelGeneratorMvcProjOldGuidStr, modelGeneratorMvcProjNewGuidStr);
                    fileContents = fileContents.Replace(modelGeneratorMvcProjOldGuidStr.ToLower(), modelGeneratorMvcProjNewGuidStr.ToLower());

                    const string modelGeneratorWMProjOldGuidStr = "AD0DFA5F-8D59-4775-8A87-EA83F9A8437B";
                    var modelGeneratorWMProjNewGuidStr = Guid.NewGuid().ToString().ToUpper();
                    fileContents = fileContents.Replace(modelGeneratorWMProjOldGuidStr, modelGeneratorWMProjNewGuidStr);
                    fileContents = fileContents.Replace(modelGeneratorWMProjOldGuidStr.ToLower(), modelGeneratorWMProjNewGuidStr.ToLower());

                    File.WriteAllText(file, fileContents);
                }
            }

            foreach (var subDir in Directory.GetDirectories(path)) ReplaceGuidsInDir(subDir);
        }
        private static void GenerateNewRandomEncryptionKeyForLocalStorage(string path)
        {
            const string oldKey = @"{ 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE };";

            var sb = new StringBuilder();
            for (var i = 0; i < 16; i++)
            {
                var nextHex = Random.Next(255);
                sb.AppendFormat(i == 0 ? "0x{0:X2}" : ", 0x{0:X2}", nextHex);
            }
            var newKey = "{ " + sb + " };";

            ReplaceInDir(path, oldKey, newKey, "SolutionMaker.cs");
        }
        private static void GenerateNewRandomEncryptionKeyForSecureAuth(string path)
        {
            const string oldKey = @"{ 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };";

            var sb = new StringBuilder();
            for (var i = 0; i < 16; i++)
            {
                var nextHex = Random.Next(255);
                sb.AppendFormat(i == 0 ? "0x{0:X2}" : ", 0x{0:X2}", nextHex);
            }
            var newKey = "{ " + sb + " };";

            ReplaceInDir(path, oldKey, newKey, "SolutionMaker.cs");
        }
        private static void GenerateNewSecretTokenForSecureAuth(string path)
        {
            const string oldSecretToken = "[SECRET_TOKEN]";
            var newSecretToken = Convert.ToBase64String(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes($"{Guid.NewGuid()}{Guid.NewGuid()}")))[..86];
            ReplaceInDir(path, oldSecretToken, newSecretToken, "SolutionMaker.cs");
        }
        #endregion

        #region CreateSnpshot Methods
        public static void CreateSnapshot(string projectTemplateDirectory, string? destinationDir = null)
        {
            Console.WriteLine("Deleting files and directories...");
            DeleteWhatWeDoNotNeedForSnapshot(AdjustPath(projectTemplateDirectory));
            Console.WriteLine("Done Deleting.");

            var zipFileNamePath = ZipFileName;
            if (destinationDir != null) zipFileNamePath = CombineAndAdjustPaths(destinationDir, ZipFileName);

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
        private static void ReplaceInFile(string file, string oldStr, string newStr)
        {
            //Replace oldStr in file contents with newStr
            var fileContents = File.ReadAllText(file);
            if (fileContents.Contains(oldStr))
            {
                fileContents = fileContents.Replace(oldStr, newStr);
                File.WriteAllText(file, fileContents);
            }
        }
        private static void ReplaceInDir(string directory, string oldStr, string newStr, params string[]? ignoreFileNames)
        {
            oldStr = oldStr.Replace("\r\n", "\n");
            newStr = newStr.Replace("\r\n", "\n");

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
                
                ReplaceInFile(file, oldStr, newStr);
                
                //Replace marker in file name 
                if (file.Contains(oldStr) && newStr != oldStr) File.Move(file, file.Replace(oldStr, newStr));
            }

            foreach (var subDir in Directory.GetDirectories(newDirectory)) ReplaceInDir(subDir, oldStr, newStr, ignoreFileNames);
        }
        private static string RemoveStrWithCheck(this string me, string str)
        {
            return me.ReplaceStrWithCheck(str, "");
        }
        private static string ReplaceStrWithCheck(this string me, string str1, string str2)
        {
            me = me.Replace("\r\n", "\n");
            str1 = str1.Replace("\r\n", "\n");
            str2 = str2.Replace("\r\n", "\n");

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
                for (var i = 1; i < stackFrames.Length; i++)
                {
                    sb.Append($"\n{stackFrames[i].GetMethod()}");
                }
            }
            return sb.ToString();
        }
        public static string AdjustPath(string path)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? path.Trim().Replace("/", "\\") : path.Trim().Replace("\\", "/");
        }
        public static string CombineAndAdjustPaths(string part1, string part2)
        {
            part1 = AdjustPath(part1);
            part2 = AdjustPath(part2);
            return Path.Combine(part1, part2);
        }
        #endregion

        #region Properties and Contants
        public static Random Random { get; } = new Random(Guid.NewGuid().GetHashCode());
        public const string ZipFileName = "SupermodelSolutionTemplate.XXYXX.zip";
        public const string Version = "2.00.000";
        #endregion
    }
}
