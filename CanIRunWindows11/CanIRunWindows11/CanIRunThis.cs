using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanIRunWindows11
{
    public partial class CanIRunThis : Form
    {
        GetComponents getComponents = new GetComponents();
        public CanIRunThis()
        {       
            InitializeComponent();
            RunScript();
        }
   
        private void gitlink_btn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/olliewet");
        }

        private void RunScript()
        {
            Architecture();
            SetPCParts();
            SetCPUCores();
            SetDirectX();
            SetRAM();
            SetStorage();
            SetGPU();
            SetCPUComp();
            SetTPM();
            SetSecureBoot();
            SetBootMethod();
        }

        private void Architecture()
        {
            if(getComponents.Architecture == "64 Bit CPU 64 Bit OS")
            {
                arch_result.Text = getComponents.Architecture;
                Architecture_PB.Text = "";
                Architecture_PB.BackColor = Color.Green;
            }
        }

        private void SetBootMethod()
        {
            bool hasUefi = getComponents.HasUEFI;
            if(hasUefi)
            {
                boot_result.Text = "Uefi";
                boot_pb.BackColor = Color.Green;
            }
            else
            {
                boot_result.Text = "Uefi Not Detected";
            }
        }

        private void SetPCParts()
        {
            cpu_lbl.Text = getComponents.CPUName;
        }

        private void SetCPUCores()
        {        
            int Cores = Int32.Parse(getComponents.DirectX);
            if (Cores >= 2)
            {
                core_result.Text = getComponents.CoreCount.ToString() + " Cores";
                core_pb.BackColor = Color.Green;             
            }
        }
        private void SetCPUComp()
        {
            bool isComp = getComponents.CpuCompabtibility;
            if(isComp)
            {
                comp_result.Text = "CPU is Compabtible";
                cpuc_pb.BackColor = Color.Green;
            }
            else
            {
                comp_result.Text = "CPU is not currently listed in Compabtible CPUs";
            }
        }


        private void SetRAM()
        {
            var data = Regex.Match(getComponents.RAMInstalled, @"\d+").Value;
            int ramInstalled = Int32.Parse(data);
            if (ramInstalled >= 4)
            {
                ram_result.Text = getComponents.RAMInstalled;
                ram_pb.BackColor = Color.Green;
            }
            else
            {
                ram_result.Text = getComponents.CoreCount.ToString() + "GB Installed";
            }
        }

        private void SetStorage()
        {
            var data = Regex.Match(getComponents.StorageAvailable, @"\d+").Value;
            int avaliableStorage = Int32.Parse(data);
            if (avaliableStorage >= 64)
            {
                storavaliable_result.Text = getComponents.StorageAvailable + " Avaliable";
                sa_pb.BackColor = Color.Green;
            }
            else
            {
                storavaliable_result.Text = "64 GB Required, this system contains" + getComponents.StorageAvailable;
            }
        }

        private void SetDirectX()
        {
            //Add error checking to make sure user has got direct x 
            try
            {
                int DVersion = Int32.Parse(getComponents.DirectX);
                directX_result.Text = "Direct X " + getComponents.DirectX.ToString();
                if(DVersion >= 12)
                {
                    dx_pb.BackColor = Color.Green;
                    directX_result.Text = "Direct X " + getComponents.DirectX.ToString();
                }
                else
                {
                    directX_result.Text = "Direct X 12 is Required for Windows 11";
                }              
            }
            catch
            {
                directX_result.Text = "Direct X Not Installed";
            }
            
        }

        private void SetTPM()
        {
            if(!getComponents.HasTPM)
            {
                tpm_result.Text = "TPM Not Installed";              
            }
            else
            {
                tpm_result.Text = "TPM is Installed";
                tpm_pb.BackColor = Color.Green;
            }
        }
        private void SetGPU()
        {
            gpu_lbl.Text = getComponents.GraphicsCard;        
        }

        private void SetSecureBoot()
        {
            if(getComponents.SecureBoot)
            {
                sb_pb.BackColor = Color.Green;
                secure_result.Text = "Secure Boot Supported";
            }
            else
            {
                secure_result.Text = "Secure Boot Not Supported";
            }
        }

    }
}
