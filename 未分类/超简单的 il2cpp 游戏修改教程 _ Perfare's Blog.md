![](https://data.perfare.net/wp-content/uploads/2016/12/60232267.jpg)

本文较旧，已不适合参考

锁链战记 3.0 版本也是换上了 il2cpp，所以刚好就用它来写一篇简单的 il2cpp 修改教程  
使用的是日服 3.0.1 版的锁链战记

首先要使用一款工具：[Il2CppDumper](https://github.com/Jumboperson/Il2CppDumper)，暑假 pokemon go 火起来的时候国外出现了一堆 U3D 的修改工具，这就是其中之一。这里推荐使用这个增加了交互功能的版本，可以省的自己重新编译。至于这工具具体功能我就懒得介绍了，反正往下看就知道啦~。

**使用 Il2CppDumper**

从 apk 解压出 libil2cpp.so 和 global-metadata.dat，把 libil2cpp.so 丢进 ida，等分析结束后，在左侧 Functions window 搜索 il2cpp::vm::MetadataCache::Register  

![](https://data.perfare.net/wp-content/uploads/2016/12/QQ%E6%88%AA%E5%9B%BE20161206174523.png)

  
双击. plt 那一行，在右侧可以看到一个引用，双击

![](https://data.perfare.net/wp-content/uploads/2016/12/QQ%E6%88%AA%E5%9B%BE20161206175012.png)

可以看到这个

![](https://data.perfare.net/wp-content/uploads/2016/12/QQ%E6%88%AA%E5%9B%BE20161206175145.png)

  
接下来把上面的 Il2CppDumper 和 libil2cpp.so，global-metadata.dat 放在一起，双击运行，分别输入上图的头两个 offset，就是 174E858 和 1739C10，等待几秒后就能生成 dump.cs 啦

**修改**

打开 dump.cs 看一眼，你大概就可以猜出 Il2CppDumper 的功能了。接下来就是找修改的位置，这里直接参考了我去年写的一篇[文章](https://www.perfare.net/83.html)，里面列举了非常基础的修改位置，可以看出修改攻击力的话就是修改 CardInfo 下的 get_ATK 函数的返回值，在 dump.cs 里搜索就能找到，右侧的值就是函数所在的位置啦  

![](https://data.perfare.net/wp-content/uploads/2016/12/QQ%E6%88%AA%E5%9B%BE20161206180421.png)

接下来就是修改 so，因为这就是个返回攻击力 int 数值的函数，所以修改思路就是让它返回一个大值，这里 il 的代码还是有点参考价值的，arm 下也就是两句话  
mov r0,#0x19000  —>  ldc.i4 0x19000  
bx lr                       —>  ret  
注意 arm 里不是所有数都可以是立即数的，具体的就自行百度啦  
接下来就把这两句话转换成 HEX，用[这个](http://armconverter.com/)在线转换网站，输入上面两段代码，在 ida 里明显可以看出代码的间隔是 4 字节，也就是 32 位，所以就选 x32，点击 Convert 后就可以得到 HEX  
190AA0E3  
1EFF2FE1  
接下来就在 16 进制编辑器里，跳转到偏移 0x91ae50，把上面的 HEX 写进去就修改成功啦~