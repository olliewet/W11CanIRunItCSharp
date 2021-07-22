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

        #region Member Variables 
        //Member Variables 
        private string cpuName;
        private string arch;
        private string boot;
        private bool compat;
        private uint corecount;
        private string freq;
        private string direx;
        private string disk;
        private string ram;
        private bool secureboot;
        private string storage;
        private string GPU;
        private string _cpugen;
        private int HardDrives;
        private bool _hastpm;
        #endregion

        #region Properties
        public string GraphicsCard   // property
        {
            get { return GPU; }   // get method
            set { GPU = value; }  // set method
        }
        public string CPUGen   // property
        {
            get { return _cpugen; }   // get method
            set { _cpugen = value; }  // set method
        }
        public string CPUName   // property
        {
            get { return cpuName; }   // get method
            set { cpuName = value; }  // set method
        }
        public string Architecture   // property
        {
            get { return arch; }   // get method
            set { arch = value; }  // set method
        }
        public string BootMethod   // property
        {
            get { return boot; }   // get method
            set { boot = value; }  // set method
        }
        public bool CpuCompabtibility   // property
        {
            get { return compat; }   // get method
            set { compat = value; }  // set method
        }
        public uint CoreCount   // property
        {
            get { return corecount; }   // get method
            set { corecount = value; }  // set method
        }
        public string Frequency   // property
        {
            get { return freq; }   // get method
            set { freq = value; }  // set method
        }
        public string DirectX   // property
        {
            get { return direx; }   // get method
            set { direx = value; }  // set method
        }
        public string DiskPartition   // property
        {
            get { return disk; }   // get method
            set { disk = value; }  // set method
        }
        public string RAMInstalled   // property
        {
            get { return ram; }   // get method
            set { ram = value; }  // set method
        }
        public bool SecureBoot   // property
        {
            get { return secureboot; }   // get method
            set { secureboot = value; }  // set method
        }
        public string StorageAvailable   // property
        {
            get { return storage; }   // get method
            set { storage = value; }  // set method
        }
        public bool HasTPM   // property
        {
            get { return _hastpm; }   // get method
            set { _hastpm = value; }  // set method
        }

        public int TotalHardDrives   // property
        {
            get { return HardDrives; }   // get method
            set { HardDrives = value; }  // set method
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
