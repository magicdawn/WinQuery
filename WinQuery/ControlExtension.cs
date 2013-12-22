using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using WinQuery.Options;

namespace WinQuery
{
    //一些控件的扩展方法
    public static class ControlExtension
    {
        #region 显示,隐藏
        public static void Show(this Control ctl)
        {
            ctl.Visible = true;
        }
        public static void Hide(this Control ctl)
        {
            ctl.Visible = false;
        }
        public static void Toggle(this Control ctl)
        {
            ctl.Visible = !ctl.Visible;
        }
        #endregion

        #region 激活,失效
        public static void Enable(this Control ctl)
        {
            ctl.Enabled = true;
        }
        public static void Disable(this Control ctl)
        {
            ctl.Enabled = false;
        }
        public static void ToggleEnable(this Control ctl)
        {
            ctl.Enabled = !ctl.Enabled;
        }
        #endregion

        #region 动画
        public static void Shake(this Control ctl, ShakeOptions Options=null)
        {
            //Shake代码实现
            #region 我写的版本,网上再找找
            //判断,是否传递了ShakeOptions
            if (Options == null)
            {
                Options = new ShakeOptions();
            }

            //开启新线程 抖动 控件
            new Thread(() => {
                var old = ctl.Location;//保存原来位置

                //一些系数
                //默认转圈,转圈1 水平0,乘到y里面
                int fType = Options.ShakeType == ShakeType.Circle ? 1 : 0;
                //默认逆时针,1 ;顺时针-1乘到角度里面去
                int fDir = Options.ShakeDirection == ShakeDirection.Default ? 1 : -1;

                //改变Location
                for (int i = 0; i < Options.Times; i++)//按Options确定圈数或者水平次数
                {
                    for (double j = 0; j < 2 * Math.PI; j += 0.1)
                    {
                        ctl.Invoke((Action)(() => {
                            ctl.Location = new Point(
                                old.X + (int)(Math.Cos(j * fDir) * Options.Ridus),
                                old.Y + (int)(Math.Sin(j * fDir) * Options.Ridus * fType));
                        }));
                        Thread.Sleep(3);
                    }
                }
                //恢复
                ctl.Invoke(new Action(() => {
                    ctl.Location = old;
                }));
            }) { IsBackground = true }.Start();
            #endregion
        }

        public static void Animate(this Control ctl, AnimateOptions Options)
        {
            var oldX = ctl.Location.X;
            var oldY = ctl.Location.Y;

            double stepX = Options.X == 0 ? 0 : (Options.X - oldX) * 13 / Options.Speed;
            double stepY = Options.Y == 0 ? 0 : (Options.Y - oldY) * 13 / Options.Speed;

            double currentX = oldX;
            double currentY = oldY;

            //通过 一段时间 把Location old->new
            new Thread(() => {
                var timer = new System.Threading.Timer(state => {
                    //这是要做的事
                    currentX += stepX;
                    currentY += stepY;
                    ctl.Invoke(new Action(() => {
                        ctl.Location = new Point((int)currentX, (int)currentY);
                    }));
                }, null, 0, 13);//Jquery 13ms

                Thread.Sleep(Options.Speed);//运行这么长时间,然后关掉timer
                timer.Dispose();
            }) { IsBackground = true }.Start();
        }
        #endregion

        #region 工具
        public static List<Control> GetAllControls(this Control ctl)
        {
            List<Control> ctls = new List<Control>();
            foreach (Control subCtl in ctl.Controls)
            {
                ctls.Add(subCtl);
                if (subCtl.Controls.Count > 0)
                {
                    ctls.AddRange(GetAllControls(subCtl));
                }

            }
            return ctls;
        }
        #endregion
    }
}
