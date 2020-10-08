#nullable enable

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Supermodel.Tooling.SolutionMaker
{
    public static class SolutionMaker
    {
        #region CreateSupermodelShell Methods
        public static void CreateSupermodelShell(ISolutionMakerParams solutionMakerParams)
        {
            //Create path
            var path = Path.Combine(solutionMakerParams.SolutionDirectory, solutionMakerParams.SolutionName);
            
            //Create dir and extract files into it
            if (Directory.Exists(path)) throw new CreatorException($"Unable to create the new Solution.\n\nDirectory '{path}' already exists.");
            Directory.CreateDirectory(path);
            // ReSharper disable once AssignNullToNotNullAttribute
            ZipFile.ExtractToDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ZipFileName), path);

        }

        private static void AdjustForXamarinFormsUIvsNativeUI(MobileApiEnum? mobileApi, string path)
        {
            if (mobileApi == MobileApiEnum.XamarinForms)
            {
                //Droid
                File.Delete(path + @"\XXYXX\Mobile\XXYXX.Droid\MainActivity.cs");
                File.Move(path + @"\XXYXX\Mobile\XXYXX.Droid\MainActivity.XamarinForms.cs", path + @"\XXYXX\Mobile\XXYXX.Droid\MainActivity.cs");

                //iOS
                File.Delete(path + @"\XXYXX\Mobile\XXYXX.iOS\AppDelegate.cs");
                File.Move(path + @"\XXYXX\Mobile\XXYXX.iOS\AppDelegate.XamarinForms.cs", path + @"\XXYXX\Mobile\XXYXX.iOS\AppDelegate.cs");
            }
            else
            {
                //Droid
                File.Delete(path + @"\XXYXX\Mobile\XXYXX.Droid\MainActivity.XamarinForms.cs");

                //iOS
                File.Delete(path + @"\XXYXX\Mobile\XXYXX.iOS\AppDelegate.XamarinForms.cs");

                //Shared
                //XXYXX.Shared.projitems
                Directory.Delete(path + @"\XXYXX\Mobile\XXYXX.Shared\AppCore", true);

                Directory.Delete(path + @"\XXYXX\Mobile\XXYXX.Mobile\EmbeddedResources", true);
                Directory.Delete(path + @"\XXYXX\Mobile\XXYXX.Mobile\Models", true);
                Directory.Delete(path + @"\XXYXX\Mobile\XXYXX.Mobile\Pages", true);
            }
        }

        #endregion

        #region CreateSnpshot Methods
        public static void CreateSnapshot(string projectTemplateDirectory, string? destinationDir = null)
        {
            DeleteWhatWeDoNotNeedForSnapshot(projectTemplateDirectory);

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
                    Console.WriteLine($"Deleting file: {Path.GetFullPath(file)}");
                    File.Delete(file);
                }
            }

            foreach (var dir in Directory.GetDirectories(directory))
            {
                var dirName = Path.GetFileName(dir);
                if (dirName == "bin" || dirName == "obj" || dirName == "packages") 
                {
                    Console.WriteLine($"Deleting directory: {Path.GetFullPath(dir)}");
                    Directory.Delete(dir, true);
                }
                else DeleteWhatWeDoNotNeedForSnapshot(dir);
            }
        }
        #endregion

        #region Helper Methods
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
        private static Random Random { get; } = new Random();
        private const string Marker = "XXYXX";
        private const string ZipFileName = "SupermodelSolutionTemplate.XXYXX.zip";
        #endregion

    }
}
