using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ChrisTec.WindowsMobile.TodayScreen;

namespace todayDeviceID
{
    [TodayScreenItem("todayScreenDeviceID")]
    public partial class todayDeviceID : UserControl
    {
        private Label label1;
        private PictureBox pictureBox1;
    
        public todayDeviceID()
        {
            InitializeComponent();
            int w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            int h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            int factor = w / this.Width;
            this.Width = w;
            this.Height = this.Height * factor;
            
            this.pictureBox1.Height = this.Height - (pictureBox1.Top*2);
            pictureBox1.Width = pictureBox1.Height;    //make a square box

            label1.Width = this.Width - pictureBox1.Width;
            label1.Height = this.Height;

            label1.Text = ITC_DeviceID.getDeviceID();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(todayDeviceID));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(35, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 28);
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 28);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // todayDeviceID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Name = "todayDeviceID";
            this.Size = new System.Drawing.Size(230, 37);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.todayDeviceID_Paint);
            this.ResumeLayout(false);

        }

        private void todayDeviceID_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Font newFont = GetAdjustedFont(g, label1.Text, label1.Font, label1.ClientRectangle.Width, 24, 9, true);
            label1.Font = newFont;
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
                    TestFont = new Font(OriginalFont.Name, AdjustedSize-1, OriginalFont.Style);
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
