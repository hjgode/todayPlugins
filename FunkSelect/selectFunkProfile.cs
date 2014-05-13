using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ChrisTec.WindowsMobile.TodayScreen;

namespace FunkSelect
{
    [TodayScreenItem("selectFunkProfile")]
    public partial class selectFunkProfile : UserControl
    {
        wlan _wlan = new wlan();
        public selectFunkProfile()
        {
            InitializeComponent();
            updateProfile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _wlan.setProfile("Profile_1");
            updateProfile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _wlan.setProfile("Profile_2");
            updateProfile();
        }

        private void updateProfile()
        {
            string sOut = "                              ";
            _wlan.getProfile(1, out sOut);
            System.Diagnostics.Debug.WriteLine("active profile=" + sOut);
            label1.Text = String.Format("Select WLAN profile ({0})", sOut);
        }
    }
}