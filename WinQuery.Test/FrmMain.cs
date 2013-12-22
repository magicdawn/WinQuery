using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using WinQuery;
using CsQuery;
namespace WinQuery.Test
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            wrapper = this.Wq(button1, button2, button3);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var ctls = this.Wq("Button[Name*=button]");
            ctls.Controls.Remove(button5, button4);
            ctls.Each(ctl => {
                ctl.Tag = ctl.Location;
            });
            this.Wq("Panel").mouseEnter(ctl => {
                //移上去
                var name = ctl.Name.Replace("panel", "button");
                var selector = "Button[Name=" + name + "]";
                var btn = this.Wq(selector)[0];
                btn.Animate(new Options.AnimateOptions {
                    X = 1,
                    SpeedX = "fast"
                });
            }).mouseLeave(ctl => {
                var name = ctl.Name.Replace("panel", "button");
                var selector = "Button[Name=" + name + "]";
                var btn = this.Wq(selector)[0];
                btn.Location = (Point)btn.Tag;
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.wq("Button").Controls.Count.ToString());
            //MessageBox.Show(typeof(Button).AssemblyQualifiedName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Wq("Button").click(ctl => {
                MessageBox.Show(ctl.Name);
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Shake(new Options.ShakeOptions {
                ShakeDirection = Options.ShakeDirection.Reverse,
                ShakeType = Options.ShakeType.Horizontal,
                Ridus = 40
            });
        }

        Wq wrapper;
        private void button4_Click(object sender, EventArgs e)
        {
            this.Wq(sender as Button).Shake();
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {

        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button4_MouseHover(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Wq(this).Shake();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Wq(sender as Control).Shake(new Options.ShakeOptions { 
                ShakeDirection=Options.ShakeDirection.Reverse,
                Ridus=30,
                Times=10
            });
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Wq(sender as Control).Shake(new Options.ShakeOptions { 
                ShakeType=Options.ShakeType.Horizontal,
                Ridus=30,
                Times=2
            });
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Wq(label1).Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Wq(label1).Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Wq(label1).Toggle();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Wq(sender as Control).Animate(new Options.AnimateOptions {
                X = 500,
                Y = 300,
                Speed = 300
            });
        }
    }
}
