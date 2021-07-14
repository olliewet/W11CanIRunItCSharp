using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            Architecture();
        }
   
        /// <summary>
        /// Button Clicks 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gitlink_btn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/olliewet");
        }

        private void RunScript()
        {
            Architecture();
        }
        private void Architecture()
        {
            if(getComponents.Architecture == "64 Bit CPU 64 Bit OS")
            {
                arch_result.Text = getComponents.Architecture;
                arch_btn.Text = "";
                arch_btn.BackColor = Color.Green;
            }
        }


    }
}
