#WinQuery 中文文档

#var WinQuery = WinForm + jQuery;
WinForm平台上的jQuery实现

***
#Selector 选择器,可以传一个context值,表示容器
-

尝试`this.Wq(this).Shake()`就是抖动窗口

1. string类型,像`"Button[Name=haha]"`  选择所有的按钮,并且Name属性中包含haha
已经实现的有
+ = 等于
+ *=包含关系
+ ^= 以什么什么开头
+ $= 以什么什么结尾
+ != 不等于关系

2. `params Control[]`类型,直接写上各控件名字即可.

#方法
1.Shake 抖动窗口
-
可作为扩展方法调用,可以传一个`ShakeOptions`参数 .
ShakeOptions可以选择如下参数

+ `ShakeType` 可选Circle 或者 Horizontal ,分别是转圈 , 水平方向抖动
+ `ShakeDirection` 可选 Default 或者 Reverse ,分别代表默认方向 和 逆方向
默认方向是指, 单位圆 逆时针转 角度增加,水平时是从右至左
+ `Times` 转的圈数
+ `Ridus` 半径,指的是动作的幅度


2. Animate 动画 
-
同样扩展方法,一个参数`AnimateOptions`,表示动画的选项

+ `int X` 代表要运动的点的X坐标
+ `int Y` 代表要运动的点的X坐标
+ `int Speed` 代表运动的毫秒数
+ `string SpeedX` 同上,可设置`"fast" or "normal" or "slow"`,分别代表200 400 600毫秒


3. Show Hide Toggle
-
控件的显示,隐藏,切换

4. Enable Disable ToggleEnable
-
控件的激活,使失效,切换,同jQuery

***
#事件处理
实现click mouseenter mouseleave

实现了<br>

	WQ.click(()=>{
		//这里 方法处理里面 用不到参数,可以不写lambda的参数
	})

	WQ.click((ctl)=>{
		//只用到一个参数,可以这样使用
	});

	WQ.click((ctl,e)=>{
		//用到两个参数
	});

	WQ.click((ctl,e)=>{},string name)给handler加上name,可以用于取消
**取消事件**
	`WQ.RemoveClick(string name)`

mouseenter 和 mouseleave 用法及实现同click

...

写文档好累


	

 