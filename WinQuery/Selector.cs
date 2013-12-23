using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.Reflection;
namespace WinQuery
{
    /// <summary>
    /// 选择器,是对Form的扩展
    /// </summary>
    public static class Selector
    {
        //根据String 选择器来选择
        /// <summary>
        /// 选择器
        /// </summary>
        /// <param name="frm">当前窗体</param>
        /// <param name="selector">选择器</param>
        /// <param name="context">上下文:即包含要选出的控件的容器,默认为当前窗体</param>
        /// <returns></returns>
        public static Wq Wq(this Form frm, string selector, Control context = null)
        {
            Wq wrapper = new Wq();
            if (context == null)
            {
                //上下文默认为form
                context = frm;
            }
            //在context中选出selector的控件,并添加到wrapper中
            string pattern = @"(?<type>\w+)(?<filter>\[(?<prop>\w+)(?<equalType>[*^$]?=)(?<value>\w+)\])*";
            var match = Regex.Match(selector, pattern, RegexOptions.ExplicitCapture);

            if (match.Success)//找到了
            {
                #region 找出指定类型的控件
                Type type = Type.GetType(typeof(Button).AssemblyQualifiedName.Replace(
                    "Button", match.Groups["type"].Value));
                var query = context.GetAllControls().OfType<Control>().Where(ctl => ctl.GetType() == type);
                #endregion

                #region Filter
                for (int i = 0; i < match.Groups["filter"].Captures.Count; i++)
                {
                    string prop = match.Groups["prop"].Captures[i].Value;
                    string equalType = match.Groups["equalType"].Captures[i].Value;
                    string value = match.Groups["value"].Captures[i].Value;

                    #region 属性选择器
                    query = query.Where(ctl => {
                        //控件真值
                        string ctlRealVal = ctl.GetType().GetProperty(prop).GetValue(ctl, null).ToString();

                        //结果
                        var res = false;
                        switch (equalType)
                        {
                            case "=":
                                res = ctlRealVal == value;
                                break;
                            case "!=":
                                res = ctlRealVal != value;
                                break;
                            case "*=":
                                res = ctlRealVal.Contains(value);
                                break;
                            case "^=":
                                res = ctlRealVal.StartsWith(value);
                                break;
                            case "$=":
                                res = ctlRealVal.EndsWith(value);
                                break;
                            default:
                                return false;
                        }
                        return res;
                    });
                    #endregion
                }
                #endregion

                wrapper.Controls.AddRange(query);
            }


            return wrapper;
        }

        //直接选择控件
        /// <summary>
        /// 直接选择控件
        /// </summary>
        /// <param name="frm">当前窗体</param>
        /// <param name="controls">要选择的控件</param>
        /// <returns></returns>
        public static Wq Wq(this Form frm, params Control[] controls)
        {
            Wq wrapper = new Wq();
            wrapper.Controls.AddRange(controls);
            return wrapper;
        }

        //根据一个Predicate委托来选择
        /// <summary>
        /// 根据一个Predicate委托来选择
        /// </summary>
        /// <param name="frm">当前窗体</param>
        /// <param name="predicate">Predicate委托</param>
        /// <param name="context">上下文:即包含要选出的控件的容器,默认为当前窗体</param>
        /// <returns></returns>
        public static Wq Wq(this Form frm, Func<Control, bool> predicate, Control context = null)
        {
            Wq wrapper = new Wq();
            if (context == null)
            {
                context = frm;
            }
            wrapper.Controls.AddRange(context.Controls.OfType<Control>().Where(predicate));
            return wrapper;
        }
    }
}