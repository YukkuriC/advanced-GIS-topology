# 2020《高级地理信息系统》大作业
- [2020《高级地理信息系统》大作业](#2020高级地理信息系统大作业)
    - [功能](#功能)
        - [主窗口](#主窗口)
        - [控制](#控制)
        - [图层系统](#图层系统)
        - [数据](#数据)
        - [算法](#算法)
    - [TODO](#todo)
        - [控制](#控制-1)
        - [渲染](#渲染)
        - [算法](#算法-1)

## 功能

### 主窗口
1. 图层显示
    * 图层右键菜单
        * 可见性控制
            * 图层显示/隐藏
            * 图层各部分可见性
        * 删除图层
        * 图层上下移动
            * 置于顶层/底层
            * 上移/下移一层
        * 画布聚焦至图层
        * 属性设置面板
1. 画布
    * 信息栏
        * 以科学计数法显示当前坐标、缩放
1. 工具栏
    * 从CSV导入多点图层
    * 格网模型
        * 由离散点生成格网模型
        * 格网模型加密
    * TIN模型
        * 由离散点创建TIN
    * 等值线
        * 由数据图层生成等值线
    * 拓扑生成
        * 使用弧段图层+MBR边框+米字分割，生成含拓扑关系的多边形图层

### 控制
1. 浏览功能
    * 左键拖拽
    * 右键重置镜头
    * 滚轮缩放
    * 同步位置信息至画布信息栏
1. 对象查看功能
    * 移动鼠标查看数据
    * 点击位置弹出详细信息对话框
    * 支持图层类型:
        * 栅格图层
        * 多边形矢量图层

### 图层系统
* 图层
    * 继承TreeNode类，可在控件中显示
    * 基类数据：
        * 控制参数
        * 渲染样式
    * 方法
        * 添加图层
        * 删除图层
    1. 矢量图层
        * 接入几何体渲染接口
		* 属性设置：
			* 点线面颜色
			* 点线尺寸
    1. 数据图层
        * 按高低色阶渲染数据
        * 属性设置：
            * 色阶高低数值&对应颜色
            * 边框尺寸&颜色
        * 分类
            * 栅格图层
            * TIN三角网图层

### 数据
0. 基本几何体`BaseGeom`
    * 基类数据：
        * ID
        * 取值
    * 接口方法：
        * 渲染至窗口
        * 计算外包矩形`Rect`
    1. 点`GeomPoint`
        * 数据：
            * X坐标
            * Y坐标
    1. 弧段`GeomArc`
        * 数据：
            * 点列
        * 方法：
            * 迭代包含线段
        * 可计算：
            * 长度
    1. 多边形`GeomPoly`
        * 数据：
            * 弧段
            * 孔洞多边形列表
        * 方法：
            * 查询坐标是否在多边形内
            * 生成时检查弧段正反、连接
            * 迭代返回所有点
        * 可计算：
            * 周长
            * 面积
            * 是否为顺时针排列

1. 矩形`Rect`
    * 数据：
        * X、Y覆盖范围
    * 方法：
        * 判断某点是否在矩形内

1. 栅格`Grid`
    * 继承自`Rect`
    * 数据：
        * X、Y覆盖范围
        * 等分数
        * 栅格数据
    * 方法：
        * 求最大最小值

1. 二维向量`Vector2`
    * 数据：
        * X、Y坐标
    * 方法：
        * 与向量、double四则运算
        * 点乘、叉乘
        * 判断相等、Hash、逐位比较大小
        * 与`GeomPoint`互转换

1. 三角形`Triangle`
    * 数据：
        * 各点坐标(`Vector2`)
    * 方法：
        * 计算外接圆圆心、半径
        * 计算外包矩形
        * 判断某点与三角形内外关系

1. 边`Edge`
    * 继承自`Tuple<Vector2, Vector2>`
    * 方法：
        * 判断是否为正向边
        * 翻转当前边

1. 线段`LineSegment`
    * 继承自`Tuple<GeomPoint, GeomPoint>`
    * 数据：起点、终点
    * 计算：坐标差、长度、角度、线段相交







1. 拓扑信息`Topology`
    * 内容：
        * 由点起始的弧段
        * 弧段左右多边形
    * 方法：
        * 导出到文件流

### 算法
0. 基于有限状态机的CSV parser
1. 栅格插值算法
1. Delaunay生成算法
1. 等值线生成算法
    1. 栅格图层
    1. TIN图层
1. 折线平滑插值算法
    1. 三次样条（未使用）
    1. 张力样条+迭代防止相交
1. 时针递归扫描拓扑生成算法
1. 其它
    1. 科学计数法表示数字
    1. 行列式求值
    1. 判断线段交点算法

## TODO
### 控制
* 选择功能

### 渲染
* 图层
    * 渲染文本注释信息

### 算法
* 三角网处理共线
* 拓扑生成
    * 处理“线头”
