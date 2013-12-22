using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinQuery.Options
{
    //抖动窗口 选项
    public class ShakeOptions
    {
        public ShakeType ShakeType { get; set; }//抖动类型
        public ShakeDirection ShakeDirection { get; set; }//摇动方向
        public int Times { get; set; }//转动,或者,水平摆动次数
        public int Ridus { get; set; }//抖动幅度,半径 or 水平距离

        //构造函数
        public ShakeOptions()
        {
            this.ShakeType = ShakeType.Circle;//默认circle
            this.ShakeDirection = ShakeDirection.Default;//默认方向
            this.Times = 2;//默认两圈
            this.Ridus = 10;//默认幅度20
        }
    }

    //摇动类型
    public enum ShakeType
    { 
        Circle=0,
        Horizontal=1//在水平方向上抖动
    }

    //摇动方向
    public enum ShakeDirection
    {
        Default=0,//默认,circle 逆时针;水平 从左到右
        Reverse=1//与默认相反
    }
}
