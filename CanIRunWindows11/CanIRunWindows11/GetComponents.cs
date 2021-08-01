using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace CanIRunWindows11
{
   
    public class GetComponents
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);
        public GetComponents()
        {
            IsWindowsUEFI();
            GetTPMVersion();
            GetDirectXVersion();
            GetHardDriveInfo();
            GetFrequency();
            GetArchitecture();      
            RamInstalled();        
            IsSecureBootSupported();            
            GetProcessor();      
            GetCoreCount();
            GetGraphicsCard();
            GetTPM();
            GetIntelGen();
            IsCPUCompabitle();
        }

        #region Properties
        public string GraphicsCard
        {
            get;
            set;
        }
        public bool HasUEFI
        {
            get;
            set;

        }
        public string CPUGen
        {
            get;
            set;
        }
        public string CPUName
        {
            get;
            set;
        }
        public string Architecture
        {
            get;
            set;
        }
        public string BootMethod 
        {
            get;
            set;
        }
        public bool CpuCompabtibility 
        {
            get;
            set;
        }
        public uint CoreCount   
        {
            get;
            set;
        }
        public string Frequency 
        {
            get;
            set;
        }
        public string DirectX 
        {
            get;
            set;
        }
        public string DiskPartition
        {
            get;
            set;
        }
        public string RAMInstalled
        {
            get;
            set;
        }
        public bool SecureBoot
        {
            get;
            set;
        }
        public string StorageAvailable
        {
            get;
            set;
        }
        public bool HasTPM
        {
            get;
            set;
        }

        public int TotalHardDrives
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Gets the AdressWidth of the processor (64/32 Bit)
        /// Checks what the bit is of the Operating System
        /// </summary>     
        public void GetArchitecture()
        {
            var searcher = new ManagementObjectSearcher(
            "select AddressWidth from Win32_Processor");
            foreach (var item in searcher.Get())
            {
                var arch = item["AddressWidth"];
                Architecture = arch.ToString() + " Bit CPU";
            }
            if (Environment.Is64BitOperatingSystem)
            {
                Architecture = Architecture + " 64 Bit OS";
            }
            else
            {
                Architecture = Architecture + " 32 Bit OS";
            }
        }




        /// <summary>
        /// Returns The Full Name of The Users Processor
        /// </summary>      
        public void GetProcessor()
        {
            ManagementObjectSearcher mosProcessor = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            string Procname = null;
            foreach (ManagementObject moProcessor in mosProcessor.Get())
            {
                if (moProcessor["name"] != null)
                {
                    Procname = moProcessor["name"].ToString();
                }
            }
            CPUName = Procname;
        }

        /// <summary>
        /// Gets the Frequency of the Processor and Stores it within a string 
        /// Example - 3800 HMZ
        /// </summary>
        public void GetFrequency()
        {
            var searcher = new ManagementObjectSearcher(
           "select MaxClockSpeed from Win32_Processor");
            foreach (var item in searcher.Get())
            {
                var clockSpeed = (uint)item["MaxClockSpeed"];
                Frequency = clockSpeed.ToString() + " MHZ";
            }

        }

        /// <summary>
        /// Run PowerShell Script to get TPM Version?
        /// </summary>
        public void GetTPMVersion()
        {
            using (PowerShell powerShell = PowerShell.Create())
            {
                // Source functions.
                powerShell.AddScript("Get-Tpm");
                           
                // invoke execution on the pipeline (collecting output)
                Collection<PSObject> PSOutput = powerShell.Invoke();

                // loop through each output object item
                foreach (PSObject outputItem in PSOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null object may be present here
                    if (outputItem != null)
                    {
                        Console.WriteLine($"Output line: [{outputItem}]");
                    }
                }
                // check the other output streams (for example, the error stream)
                if (powerShell.Streams.Error.Count > 0)
                {
                    // error records were written to the error stream.
                    // Do something with the error
                }
            }

        }

        /// <summary>
        /// Returns the Number Of Cores of the users processor 
        /// </summary>
        public void GetCoreCount()
        {
            var searcher = new ManagementObjectSearcher(
           "select NumberOfCores from Win32_Processor");
            foreach (var item in searcher.Get())
            {
                var NumOfCores = (uint)item["NumberOfCores"];
                CoreCount = NumOfCores;
            }
        }

        /// <summary>
        /// Get the amount of Ram in the users system
        /// </summary>
        public void RamInstalled()
        {
            long memKb;
            GetPhysicallyInstalledSystemMemory(out memKb);
            RAMInstalled = ((memKb / 1024 / 1024) + " GB of RAM installed.");
        }

        private void IsCPUCompabitle()
        {
            List<string> listOfCPUs = File.ReadAllLines("CPUList.txt").ToList();
            foreach (string cpu in listOfCPUs)
            {
                if(cpu == CPUGen)
                {
                    CpuCompabtibility = true;
                    break;
                }
                else
                {
                    CpuCompabtibility = false; 
                }
            }
        }

        private void GetIntelGen()
        {
            string cpuGen = "";
            List<string> CPUNameList = new List<string>();
            CPUNameList = CPUName.Split(' ').ToList();

            foreach(string s in CPUNameList)
            {              
                if(s.StartsWith("i3")| s.StartsWith("i5") | s.StartsWith("i7") | s.StartsWith("i9"))
                {
                   cpuGen = s[3].ToString();
                   break;
                }
            }
            if (cpuGen != "")
            {
                CPUGen = "Intel Core " + cpuGen + "th Gen";
            }
        }




        /// <summary>
        /// Does Not Work as intended 
        /// </summary>
        public void IsSecureBootSupported()
        {
            int rc = 0;
            string key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State";
            string subkey = @"UEFISecureBootEnabled";
            try
            {
                object value = Registry.GetValue(key, subkey, rc);
                if (value != null)
                    rc = (int)value;
            }
            catch
            {
                //add error handling 
            }
            if (rc == 1)
            {
                SecureBoot = true;
            }
            else
            {
                SecureBoot = false;
            }
        }

        /// <summary>
        /// Get the Size of the hard drive which contains operating system
        /// </summary>
        public void GetHardDriveInfo()
        {       
            int totalDrives = 0;
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                totalDrives++;

                if(d.Name == "C:\\")
                {
                    //string driveName = "C:\\ : ";                
                    string totalDrivesize = ByteSizeLib.ByteSize.FromBytes(d.TotalSize).ToString();
                    //StorageAvailable = driveName + " " + totalDrivesize;       
                    StorageAvailable = totalDrivesize;
                }           
            }
            TotalHardDrives = totalDrives;
        }

        /// <summary>
        /// Checks The Version of Direct X 
        /// https://stackoverflow.com/a/17131828/13839509
        /// </summary>
        /// <returns></returns>
        private void GetDirectXVersion()
        {
            Process.Start("dxdiag", "/x dxv.xml");
            while (!File.Exists("dxv.xml"))
                Thread.Sleep(1000);
            XmlDocument doc = new XmlDocument();
            doc.Load("dxv.xml");
            XmlNode dxd = doc.SelectSingleNode("//DxDiag");
            XmlNode dxv = dxd.SelectSingleNode("//DirectXVersion");
            DirectX = Convert.ToInt32(dxv.InnerText.Split(' ')[1]).ToString();
        }

        private void GetGraphicsCard()
        {
            using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    GraphicsCard = ("Name  -  " + obj["Name"]);  
                }
            }
        }


        //Stackover Flow 
        public const int ERROR_INVALID_FUNCTION = 1;

        [DllImport("kernel32.dll",
            EntryPoint = "GetFirmwareEnvironmentVariableW",
            SetLastError = true,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int GetFirmwareType(string lpName, string lpGUID, IntPtr pBuffer, uint size);

        public  void IsWindowsUEFI()
        {
            // Call the function with a dummy variable name and a dummy variable namespace (function will fail because these don't exist.)
            GetFirmwareType("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);

            if (Marshal.GetLastWin32Error() == ERROR_INVALID_FUNCTION)
            {
                // Calling the function threw an ERROR_INVALID_FUNCTION win32 error, which gets thrown if either
                // - The mainboard doesn't support UEFI and/or
                // - Windows is installed in legacy BIOS mode
                HasUEFI = false;
            }
            else
            {
                HasUEFI = true; ;
                // If the system supports UEFI and Windows is installed in UEFI mode it doesn't throw the above error, but a more specific UEFI error

            }
        }


        private void GetTPM()
        {
            List<string> TPMProperties = new List<string>();
            try
            {
                using (var searcher = new ManagementObjectSearcher("select * from Win32_Tpm"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        TPMProperties.Add("Name  -  " + obj["IsActivated_InitialValue"]);
         
                        TPMProperties.Add("Name  -  " + obj["IsEnabled_InitialValue"]);
                        TPMProperties.Add("Name  -  " + obj["IsOwned_InitialValue"]);
                        TPMProperties.Add("Name  -  " + obj["SpecVersion"]);
                        TPMProperties.Add("Name  -  " + obj["ManufacturerVersion"]);
                        TPMProperties.Add("Name  -  " + obj["ManufacturerVersionInfo"]);
                    }
                    HasTPM = true;
                }
            }catch
            {
                HasTPM = false;
            }
        }
    }
}
