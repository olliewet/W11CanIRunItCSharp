using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.IO;

namespace CanIRunWindows11
{
    public class GetComponents
    {
        public GetComponents()
        {
            GetHardDriveInfo();
            IsSecureBootSupported();
            RamInstalled();
            GetArchitecture();
            GetProcessor();
            GetFrequency();
            GetCoreCount();
        }
        //Member Variables 
        private string cpuName;
        private string arch;
        private string boot;
        private string compat;
        private uint corecount;
        private string freq;
        private string direx;
        private string disk;
        private string ram;
        private string secureboot;
        private string storage;
        private string tpm;
        /// <summary>
        /// Properties 
        /// </summary>
        /// 

        #region Properties
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
        public string CpuCompabtibility   // property
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
        public string SecureBoot   // property
        {
            get { return secureboot; }   // get method
            set { secureboot = value; }  // set method
        }
        public string StorageAvailable   // property
        {
            get { return storage; }   // get method
            set { storage = value; }  // set method
        }
        public string TPMVersion   // property
        {
            get { return tpm; }   // get method
            set { tpm = value; }  // set method
        }
        #endregion


        //Doesnt Work As Expected Returns 9 instead of 64 bit
        public void GetArchitecture()
        {
            var searcher = new ManagementObjectSearcher(
           "select AddressWidth from Win32_Processor");
            foreach (var item in searcher.Get())
            {
                var arch = item["AddressWidth"];
                Architecture = arch.ToString() + " bit CPU";
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

        //Returns The Processor Name 
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

        public void RamInstalled()
        {
            var searcher = new ManagementObjectSearcher(
          "select TotalPhysicalMemory from Win32_ComputerSystem ");
            foreach (var item in searcher.Get())
            {
                var raminstalled = item["TotalPhysicalMemory"];
                RAMInstalled = raminstalled.ToString() + "GB";
            }
        }

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

            }
            if (rc == 1)
            {
                SecureBoot = "Supported";
            }
            else
            {
                SecureBoot = "Not Supported";
            }
        }


        public void GetHardDriveInfo()
        {

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }









        }
    }
}
