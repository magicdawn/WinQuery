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
        //控件集合,//public不安全,可以封装下.
        //但是可能有点坑,时间什么的;下面的注释 失效 就是不想写了
        /// <summary>
        /// Wq包装集中的控件载体
        /// </summary>
        public List<Control> Controls { get; set; }

        //控件索引器
        /// <summary>
        /// 控件索引器
        /// </summary>
        /// <param name="id">int类型的id,表示索引</param>
        /// <returns></returns>
        public Control this[int id]
        {
            get
            {
                return Controls[id];
            }
        }

        #region 可以Remove掉的事件暂存器
        /// <summary>
        /// 可以Remove掉的Click事件暂存器
        /// </summary>
        Dictionary<string, Action<Control, EventArgs>> ClickHandlers { get; set; }
        /// <summary>
        /// 可以Remove掉的MouseEnter事件暂存器
        /// </summary>
        Dictionary<string, Action<Control, EventArgs>> MouseEnterHandlers
        { get; set; }
        /// <summary>
        /// 可以Remove掉的MouseLeave事件暂存器
        /// </summary>
        Dictionary<string, Action<Control, EventArgs>> MouseLeaveHandlers
        { get; set; } 
        #endregion

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
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>当前Wq包装集</returns>
        public Wq Show()
        {
            return this.Each(WinQuery.ControlExtension.Show);
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        /// <returns>当前Wq包装集</returns>
        public Wq Hide()
        {
            return this.Each(WinQuery.ControlExtension.Hide);
        }
        /// <summary>
        /// 切换 显示,隐藏
        /// </summary>
        /// <returns>当前Wq包装集</returns>
        public Wq Toggle()
        {
            return this.Each(WinQuery.ControlExtension.Toggle);
        }
        #endregion

        #region 激活,失效
        //激活
        /// <summary>
        /// 激活
        /// </summary>
        /// <returns>当前Wq包装集</returns>
        public Wq Enable()
        {
            return this.Each(ControlExtension.Enable);
        }
        //使失效
        /// <summary>
        /// 失效
        /// </summary>
        /// <returns>当前Wq包装集</returns>
        public Wq Disable()
        {
            return this.Each(ControlExtension.Disable);
        }
        //切换 是否 激活
        /// <summary>
        /// 切换 激活,失效
        /// </summary>
        /// <returns>当前Wq包装集</returns>
        public Wq ToggleEnable()
        {
            return this.Each(ControlExtension.ToggleEnable);
        }
        #endregion

        #region 动画
        //抖动
        /// <summary>
        /// 抖动窗口或控件
        /// </summary>
        /// <param name="options">抖动时的一些参数封装</param>
        /// <returns>当前Wq包装集</returns>
        public Wq Shake(Options.ShakeOptions options=null)
        {
            return this.Each(ctl=>{
                ControlExtension.Shake(ctl, options);
            });
        }

        /// <summary>
        /// 仿Jq动画
        /// </summary>
        /// <param name="options">动画的一些参数封装,必须提供</param>
        /// <returns>当前Wq包装集</returns>
        public Wq Animate(Options.AnimateOptions options)
        {
            return this.Each(ctl => {
                ControlExtension.Animate(ctl, options);
            });
        }
        #endregion

        #region 事件处理

        #region 实际跟 控件 的event 打交道的,不暴露给外部使用
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

        /// <summary>
        /// 可以取消的Click事件
        /// </summary>
        /// <param name="act">实际要绑定的动作</param>
        /// <param name="name">该动作的名称</param>
        /// <returns>当前Wq包装集</returns>
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
        /// <summary>
        /// 取消Click事件绑定的某动作
        /// </summary>
        /// <param name="name">动作的名称</param>
        /// <returns>当前Wq包装集</returns>
        public Wq RemoveClick(string name)
        {
            if (ClickHandlers.ContainsKey(name))
            {
                ClickHandlers.Remove(name);
            }
            return this;
        }

        #endregion click
        #region mouseenter与mouseleave

        /// <summary>
        /// 可以取消的MouseEnter事件
        /// </summary>
        /// <param name="act">实际要绑定的动作</param>
        /// <param name="name">该动作的名称</param>
        /// <returns>当前Wq包装集</returns>
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

        /// <summary>
        /// 取消MouseEnter事件绑定的某动作
        /// </summary>
        /// <param name="name">动作的名称</param>
        /// <returns>当前Wq包装集</returns>
        public Wq RemoveMouseEnter(string name)//删除Enter
        {
            if (MouseEnterHandlers.ContainsKey(name))
            {
                MouseEnterHandlers.Remove(name);
            }
            return this;
        }

        /// <summary>
        /// 可以取消的MouseLeave事件
        /// </summary>
        /// <param name="act">实际要绑定的动作</param>
        /// <param name="name">该动作的名称</param>
        /// <returns>当前Wq包装集</returns>
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

        /// <summary>
        /// 取消MouseLeave事件绑定的某动作
        /// </summary>
        /// <param name="name">动作的名称</param>
        /// <returns>当前Wq包装集</returns>
        public Wq RemoveMouseLeave(string name)//删除leave
        {
            if (MouseLeaveHandlers.ContainsKey(name))
            {
                MouseLeaveHandlers.Remove(name);
            }
            return this;
        }
        #endregion mouseenter与mouseleave
        #endregion 可以取消的事件

        #region 不可取消的事件

        #region click
        /// <summary>
        /// 绑定Click事件
        /// </summary>
        /// <param name="act">两个参数的Action</param>
        /// <returns>当前Wq包装集</returns>
        public Wq click(Action<Control, EventArgs> act)//两个参数
        {
            return this.Each(ctl => {
                ctl.Click += new EventHandler((sender, e) => {
                    act(ctl, e);
                });
            });
        }

        // <summary>
        /// 绑定Click事件
        /// </summary>
        /// <param name="act">一个参数的Action</param>
        /// <returns>当前Wq包装集</returns>
        public Wq click(Action<Control> act)//一个参数
        {
            return this.Each(ctl => {
                ctl.Click += new EventHandler((sender, e) => {
                    act(ctl);
                });
            });
        }

        // <summary>
        /// 绑定Click事件
        /// </summary>
        /// <param name="act">没有参数的Action</param>
        /// <returns>当前Wq包装集</returns>
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
        /// <summary>
        /// 绑定MouseEnter事件
        /// </summary>
        /// <param name="act">两个参数的Action</param>
        /// <returns>当前Wq包装集</returns>
        public Wq mouseEnter(Action<Control, EventArgs> act)//正常两个参数
        {
            return this.Each(ctl => {
                ctl.MouseEnter += (sender, e) => {
                    act(ctl, e);
                };
            });
        }

        /// <summary>
        /// 绑定MouseEnter事件
        /// </summary>
        /// <param name="act">一个参数的Action</param>
        /// <returns>当前Wq包装集</returns>
        public Wq mouseEnter(Action<Control> act)
        {
            return this.Each(ctl => {
                ctl.MouseEnter += (sender, e) => {
                    act(ctl);
                };
            });
        }
        /// <summary>
        /// 绑定MouseEnter事件
        /// </summary>
        /// <param name="act">不带参数的Action</param>
        /// <returns>当前Wq包装集</returns>
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
        /// <summary>
        /// 绑定MouseLeave事件
        /// </summary>
        /// <param name="act">两个参数的Action</param>
        /// <returns>当前Wq包装集</returns>
        public Wq mouseLeave(Action<Control, EventArgs> act)
        {
            return this.Each(ctl => {
                ctl.MouseLeave += new EventHandler((sender, e) => {
                    act(ctl, e);
                });
            });
        }
        /// <summary>
        /// 绑定MouseLeave事件
        /// </summary>
        /// <param name="act">一个参数的Action</param>
        /// <returns>当前Wq包装集</returns>
        public Wq mouseLeave(Action<Control> act)
        {
            return this.Each(ctl => {
                ctl.MouseLeave += new EventHandler((sender, e) => {
                    act(ctl);
                });
            });
        }
        /// <summary>
        /// 绑定MouseLeave事件
        /// </summary>
        /// <param name="act">不带参数的Action</param>
        /// <returns>当前Wq包装集</returns>
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
        /// <summary>
        /// 对Wq包装集内的Conreols进行遍历
        /// </summary>
        /// <param name="action">要对各控件的操作</param>
        /// <returns>当前Wq包装集</returns>
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