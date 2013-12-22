using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinQuery;

namespace WinQuery
{
    public class Wq
    {
        //控件集合,//仅仅dll内部能访问
        public List<Control> Controls { get; set; }

        //控件索引器
        public Control this[int id]
        {
            get
            {
                return Controls[id];
            }
        }

        //click的事件 暂存器
        Dictionary<string, Action<Control, EventArgs>> ClickHandlers { get; set; }
        Dictionary<string, Action<Control, EventArgs>> MouseEnterHandlers
        { get; set; }
        Dictionary<string, Action<Control, EventArgs>> MouseLeaveHandlers
        { get; set; }

        ////失效
        ////索引器
        //public Control this[int id]
        //{
        //    get
        //    {
        //        return this.Controls[id];
        //    }
        //}

        ////控件个数
        //public int Count
        //{
        //    get
        //    {
        //        return this.Controls.Count;
        //    }
        //}
        //#endregion

        //#region 控件添加删除
        //public Wq Add(Control ctl)
        //{
        //    if (!this.Controls.Contains(ctl))
        //    {
        //        this.Controls.Add(ctl);
        //    }
        //    return this;
        //}
        //public Wq Remove(Control ctl)
        //{
        //    if (this.Controls.Contains(ctl))
        //    {
        //        this.Controls.Remove(ctl);
        //    }
        //    return this;
        //}
        //#endregion

        #region 构造函数
        public Wq()
        {
            Controls = new List<Control>();
            ClickHandlers = new Dictionary<string, Action<Control, EventArgs>>();
            MouseEnterHandlers = new Dictionary<string, Action<Control, EventArgs>>();
            MouseLeaveHandlers = new Dictionary<string, Action<Control, EventArgs>>();
        }
        #endregion

        #region 显示,隐藏
        public Wq Show()
        {
            return this.Each(WinQuery.ControlExtension.Show);
        }
        public Wq Hide()
        {
            return this.Each(WinQuery.ControlExtension.Hide);
        }
        public Wq Toggle()
        {
            return this.Each(WinQuery.ControlExtension.Toggle);
        }
        #endregion

        #region 激活,失效
        //激活
        public Wq Enable()
        {
            return this.Each(ControlExtension.Enable);
        }
        //使失效
        public Wq Disable()
        {
            return this.Each(ControlExtension.Disable);
        }
        //切换 是否 激活
        public Wq ToggleEnable()
        {
            return this.Each(ControlExtension.ToggleEnable);
        }
        #endregion

        #region 动画
        //抖动
        public Wq Shake(Options.ShakeOptions options=null)
        {
            return this.Each(ctl=>{
                ControlExtension.Shake(ctl, options);
            });
        }

        public Wq Animate(Options.AnimateOptions options)
        {
            return this.Each(ctl => {
                ControlExtension.Animate(ctl, options);
            });
        }
        #endregion

        #region 事件处理

        #region 实际跟 控件 的event 打交道的
        void Control_Click(object sender, EventArgs e)
        {
            var ctl = sender as Control;
            foreach (var item in ClickHandlers)
            {
                item.Value.Invoke(ctl, e);
            }
        }
        void Control_MouseEnter(object sender, EventArgs e)//MouseEnter
        {
            var ctl = sender as Control;
            foreach (var item in MouseEnterHandlers)
            {
                item.Value(ctl, e);
            }
        }
        void Control_MouseLeave(object sender, EventArgs e)
        {
            var ctl = sender as Control;
            foreach (var item in MouseLeaveHandlers)
            {
                item.Value.Invoke(ctl, e);
            }
        }
        #endregion

        #region 可以取消的事件

        #region click
        public Wq click(Action<Control, EventArgs> act, string name)
        {
            if (ClickHandlers.ContainsKey(name))
            {
                ClickHandlers[name] = act;
            }
            else
            {
                ClickHandlers.Add(name, act);
            }
            //注意不要重复 创建 wrapper
            return this.Each(ctl => {
                ctl.Click -= Control_Click;
                ctl.Click += Control_Click;
            });
        }
        //移除handler
        public Wq RemoveClick(string name)
        {
            if (ClickHandlers.ContainsKey(name))
            {
                ClickHandlers.Remove(name);
            }
            return this;
        }
        #endregion

        #region mouseenter与mouseleave
        public Wq mouseEnter(Action<Control, EventArgs> act, string name)//添加Enter
        {
            if (MouseEnterHandlers.ContainsKey(name))
            {
                MouseEnterHandlers[name] = act;
            }
            else
            {
                MouseEnterHandlers.Add(name, act);
            }
            return this.Each(ctl => {
                ctl.MouseEnter -= Control_MouseEnter;
                ctl.MouseEnter += Control_MouseEnter;
            });
        }
        public Wq RemoveMouseEnter(string name)//删除Enter
        {
            if (MouseEnterHandlers.ContainsKey(name))
            {
                MouseEnterHandlers.Remove(name);
            }
            return this;
        }
        public Wq mouseLeave(Action<Control, EventArgs> act, string name)//添加leave
        {
            if (MouseLeaveHandlers.ContainsKey(name))
            {
                MouseLeaveHandlers[name] = act;
            }
            else
            {
                MouseLeaveHandlers.Add(name, act);
            }

            return this.Each(ctl => {
                ctl.MouseLeave -= Control_MouseLeave;
                ctl.MouseLeave += Control_MouseLeave;
            });
        }
        public Wq RemoveMouseLeave(string name)//删除leave
        {
            if (MouseLeaveHandlers.ContainsKey(name))
            {
                MouseLeaveHandlers.Remove(name);
            }
            return this;
        }
        #endregion mouseenter与mouseleave

        #endregion

        #region 不可取消的事件

        #region click
        public Wq click(Action<Control, EventArgs> act)//两个参数
        {
            return this.Each(ctl => {
                ctl.Click += new EventHandler((sender, e) => {
                    act(ctl, e);
                });
            });
        }
        public Wq click(Action<Control> act)//一个参数
        {
            return this.Each(ctl => {
                ctl.Click += new EventHandler((sender, e) => {
                    act(ctl);
                });
            });
        }

        public Wq click(Action act)//没有参数m
        {
            return this.Each(ctl => {
                ctl.Click += (sender, e) => {
                    act();
                };
            });
        }
        #endregion

        #region mouseenter
        public Wq mouseEnter(Action<Control, EventArgs> act)//正常两个参数
        {
            return this.Each(ctl => {
                ctl.MouseEnter += (sender, e) => {
                    act(ctl, e);
                };
            });
        }
        public Wq mouseEnter(Action<Control> act)
        {
            return this.Each(ctl => {
                ctl.MouseEnter += (sender, e) => {
                    act(ctl);
                };
            });
        }
        public Wq mouseEnter(Action act)
        {
            return this.Each(ctl => {
                ctl.MouseEnter += (sender, e) => {
                    act();
                };
            });
        }
        #endregion

        #region mouseleave
        public Wq mouseLeave(Action<Control, EventArgs> act)
        {
            return this.Each(ctl => {
                ctl.MouseLeave += new EventHandler((sender, e) => {
                    act(ctl, e);
                });
            });
        }
        public Wq mouseLeave(Action<Control> act)
        {
            return this.Each(ctl => {
                ctl.MouseLeave += new EventHandler((sender, e) => {
                    act(ctl);
                });
            });
        }
        public Wq mouseLeave(Action act)
        {
            return this.Each(ctl => {
                ctl.MouseLeave += new EventHandler((sender, e) => {
                    act();
                });
            });
        }
        #endregion

        #endregion 不可取消的事件

        #endregion 事件处理

        #region 工具
        //遍历
        public Wq Each(Action<Control> action)
        {
            foreach (Control ctl in this.Controls)
            {
                action(ctl);
            }
            return this;
        }
        #endregion
    }
}