using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinQuery
{
    /// <summary>
    /// 对List<control>的扩展,可批量添加,删除
    /// </summary>
    public static class ListExtension
    {
        //批量删除
        /// <summary>
        /// 批量删除,对List<control>的扩展
        /// </summary>
        /// <param name="list">当前List<control></param>
        /// <param name="ctls">要删除的控件</param>
        public static void Remove(this List<Control> list, params Control[] ctls)
        {
            if (list.Count()<=0)
            {
                return;
            }
            foreach (var ctl in ctls)
            {
                if (list.Contains(ctl))
                {
                    list.Remove(ctl);
                }
            }
        }

        //批量添加
        /// <summary>
        /// 批量添加,对List<control>的扩展
        /// </summary>
        /// <param name="list">当前List<control></param>
        /// <param name="ctls">要添加的控件</param>
        public static void Add(this List<Control> list, params Control[] ctls)
        {
            foreach (var ctl in ctls)
            {
                if (!list.Contains(ctl))
                {
                    list.Add(ctl);
                }
            }
        }
    }
}
