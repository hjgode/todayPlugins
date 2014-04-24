using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime;

using ChrisTec.WindowsMobile.TodayScreen;

namespace todayApp1
{
    [TodayScreenItem("todayScreenDeviceID")]
    public partial class todayApp1 : UserControl
    {
        theApps apps = new theApps();
        myApps.myApp[] allApps;

        public todayApp1()
        {
            InitializeComponent();
            allApps = apps.myAppList.ToArray();

            //get screen width
            int w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            int h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            int factor = w / this.Width;
            this.Width = w;
            this.Height = this.Height * factor;
            int vScrollWidth = SystemMetric.SystemMetrics.getVScrollWidth();
            int hScrollHeight = SystemMetric.SystemMetrics.getHScrollHeight();

            int btnW = this.ClientRectangle.Width - 
                2 * vScrollWidth - 
                2 * SystemMetric.SystemMetrics.GetSystemMetric(SystemMetric.SystemMetrics.SystemMetricsCodes.SM_CXBORDER);// this.ClientRectangle.Width - pictureBox1.Width - 2 * pictureBox1.Left;
            int btnH = this.ClientRectangle.Height - (this.button1.Top * 2);
            int btnL = vScrollWidth;// 2 * pictureBox1.Left + pictureBox1.Width;
            int btnT = button1.Top;

            //save space for scrollbar if more than two buttons
            if (allApps.Length > 2)
                btnW -= SystemMetric.SystemMetrics.getVScrollWidth();

            int pbH = btnH;
            int pbW = pbH;
            int pbL = 0;
            int pbT = btnT;

            //the first button
            if (System.IO.File.Exists(allApps[0].iconFile))
            {
                this.button1.Left = btnL + pbW + pbL;
                this.button1.Width = btnW - pbW - pbL;
                this.button1.Height = btnH;
                this.button1.Text = allApps[0].title;

                PictureBox pb = new PictureBox();
                loadImage(ref pb, 0);
                pb.Width = pbW;
                pb.Height = pbH;
                pb.Left = pbL;
                pb.Top = pbT;
                this.Controls.Add(pb);
            }
            else
            { //no icon
                this.button1.Left = btnL;
                this.button1.Width = btnW;
                this.button1.Height = btnH;
                this.button1.Text = allApps[0].title;
            }
            this.button1.Click += new EventHandler(button1_Click);

            if (allApps.Length > 1)
            {
                for (int i = 1; i < allApps.Length; i++)
                {
                    Button btn = new Button();
                    if (System.IO.File.Exists(allApps[i].iconFile))
                    {
                        PictureBox pb = new PictureBox();
                        loadImage(ref pb, allApps[i].iconFile);
                        pb.Width = pbW;
                        pb.Height = pbH;
                        pb.Left = pbL;
                        pb.Top = pbT + i*pbH;
                        
                        this.Controls.Add(pb);

                        btn.Text = allApps[i].title;
                        btn.Height = btnH;
                        btn.Width = btnW - pbW;
                        btn.Left = btnL + pbL + pbW;
                        btn.Top = btnT + i * btnH;
                        
                    }
                    else
                    { //no icon
                        btn.Text = allApps[i].title;
                        btn.Height = btnH;
                        btn.Width = btnW;
                        btn.Left = btnL;
                        btn.Top = btnT + i * btnH;
                    }

                    if (i < 4)
                        this.Height += btnH;

                    btn.Click += new EventHandler(button1_Click);
                    this.Controls.Add(btn);
                }
            }
        }

        void loadImage(ref PictureBox pb, string iconFile)
        {
            try
            {
                pb.Image = new Bitmap(iconFile);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception loading icon: '" + iconFile + "' " + ex.Message);
            }
        }

        void loadImage(ref PictureBox pb, int idx)
        {
            try
            {
                pb.Image = new Bitmap(allApps[idx].iconFile);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception loading icon: '" + allApps[idx].iconFile + "' " + ex.Message);
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            for (int idx=0; idx<allApps.Length; idx++)//(myApps.myApp _app in allApps)
            {
                if (allApps[idx].title.Equals(btn.Text))
                {
                    System.Diagnostics.Debug.WriteLine("Button: " + btn.Text + " clicked");
                    startProcess(idx);
                    break;
                }
            }
        }

        void startProcess(int idx)
        {
            string proc = allApps[idx].exeFile;
            string args = allApps[idx].args;
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.Arguments = args;
            startInfo.FileName = proc;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = "";
            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception for Process.Start: '" + startInfo.FileName + "' " + ex.Message); 
            }
        }

        public Font GetAdjustedFont(Graphics GraphicRef, string GraphicString, Font OriginalFont, int ContainerWidth, int MaxFontSize, int MinFontSize, bool SmallestOnFail)
        {
            // We utilize MeasureString which we get via a control instance           
            for (int AdjustedSize = MaxFontSize; AdjustedSize >= MinFontSize; AdjustedSize--)
            {
                Font TestFont = new Font(OriginalFont.Name, AdjustedSize, OriginalFont.Style);

                // Test the string with the new size
                SizeF AdjustedSizeNew = GraphicRef.MeasureString(GraphicString, TestFont);

                if (ContainerWidth > Convert.ToInt32(AdjustedSizeNew.Width))
                {
                    // Good font, return it
                    TestFont = new Font(OriginalFont.Name, AdjustedSize - 1, OriginalFont.Style);
                    return TestFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return MinimumSize or Original?
            if (SmallestOnFail)
            {
                return new Font(OriginalFont.Name, MinFontSize, OriginalFont.Style);
            }
            else
            {
                return OriginalFont;
            }
        }
    }
}