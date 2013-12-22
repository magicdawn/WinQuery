using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinQuery
{
    public static class ListExtension
    {
        //批量删除
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
