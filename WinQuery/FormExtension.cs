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
    public static class FormExtension
    {
        /// <summary>
        /// 选择器
        /// </summary>
        /// <param name="form">扩展方法,Form里面可以直接调用</param>
        /// <param name="selector">选择器</param>
        /// <param name="context">上下文,容器,默认为当前form</param>
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
                                res = ctlRealVal==value;
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

        //直接选择 控件
        public static Wq Wq(this Form frm, params Control[] controls)
        {
            Wq wrapper = new Wq();
            wrapper.Controls.AddRange(controls);
            return wrapper;
        }
    }
}
