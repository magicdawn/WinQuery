using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinQuery.Options
{
    public class AnimateOptions//动画 选项
    {
        public int X { get; set; }//X坐标
        public int Y { get; set; }//Y坐标

        public int Speed { get; set; }//毫秒数,运动快慢
        public string SpeedX
        {
            set
            {
                switch (value)
                {
                    case "fast":
                        Speed = 200;
                        break;
                    case "normal":
                        Speed = 400;
                        break;
                    case "slow":
                        Speed = 600;
                        break;
                    default:
                        Speed = 200;//默认为fast
                        break;
                }
            }
        }//可以用fast slow来控制
    }
}
