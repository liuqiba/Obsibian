[编程之家](https://www.jb51.cc/ "编程之家")收集整理的这篇文章主要介绍了[发布到 steam 的 unity(Il2cpp) 游戏破解方法](https://www.jb51.cc/unity/3756485.html "发布到steam的unity(Il2cpp)游戏破解方法")，[编程之家](https://www.jb51.cc/ "编程之家")小编觉得挺不错的，现在分享给大家，也给大家做个参考。

工具下载：

IDA 7.0

Il2CppDumper

Reflector

[百度](https://www.jb51.cc/tag/baidu/)网盘下载：

[链接](https://www.jb51.cc/tag/lianjie/)：https://pan.baidu.com/s/1EpYlq6VQfoKPzH9ZkF9Nug 

[提取](https://www.jb51.cc/tag/tiqu/)码：8u2b 

unity 是现在独立游戏的常用游戏引擎，Steam 上就有很多。这篇记录就先简单介绍一下破解游戏做出游戏补丁让游戏脱离 steam 也能运行的办法。

以最近上线 steam 的游戏《回门》为例：

首先是下载游戏，找到游戏本体存在的目录：

比如我的 steam 安装在 G:\Games\ 路径大概就是：

G:\Game\Steam\steamapps\common \ 回门 Way Back Home

放在其他盘其他目录的童鞋可以自行推断位置

![](https://www.jb51.cc/res/2022/10-08/06/54c6be617fdf8d32bfaf965d3f718d20.png)

可以看到里面的 GameAssembly.dll 用 reflector 打开 GameAssembly.dll 会发现它不是 C# 的 DLL [文件](https://www.jb51.cc/tag/wenjian/)，

![](https://www.jb51.cc/res/2022/10-08/06/999512f0252e12cb07254d654d9e0e1b.png)

打开 XXX_Data 目录，里面有个 il2cpp_data

从这两点可以看出这个游戏是使用的 Il2cpp 的后端，这种后端需要懂点汇编才好破解，不过只是断开 steam 的话是难度不高的。对付 il2cpp 后端的游戏，我们可以用 Il2cppDumper 这个工具来做辅助[提取](https://www.jb51.cc/tag/tiqu/)和[修改](https://www.jb51.cc/tag/xiugai/)。

进行这个[修改](https://www.jb51.cc/tag/xiugai/)还需要找另外[一个](https://www.jb51.cc/tag/yige/)[文件](https://www.jb51.cc/tag/wenjian/) global-[Meta](https://www.jb51.cc/tag/Meta/)data.dat，通常在 il2cpp_data 下面，我找了一下在：

Bash

```
WayBackHome_Data/il2cpp_data/Metadata/global-Metadata.dat
```

我们先创建[一个](https://www.jb51.cc/tag/yige/)目录，把破解需要的中间[文件](https://www.jb51.cc/tag/wenjian/)都放[在这里](https://www.jb51.cc/tag/zaizheli/)，比如我放在了 g:\works\hack\hm \ 里面

然后我们编写[一个](https://www.jb51.cc/tag/yige/)简单的批处理脚本，放在游戏目录下（也就是 GameAssembly.dll 所在目录） [调用](https://www.jb51.cc/tag/diaoyong/) il2cppdumper 来破解，其实只有一行：

Bash

```
Il2CppDumper GameAssembly.dll WayBackHome_Data/il2cpp_data/Metadata/global-Metadata.dat g:\works\hack\hm\
```

编写完保存成 bat [文件](https://www.jb51.cc/tag/wenjian/)，运行后等待结束，成功的话就会在 g:\works\hack\hm\ 下面[生成](https://www.jb51.cc/tag/shengcheng/)一些中间[文件](https://www.jb51.cc/tag/wenjian/)，比如：

![](https://www.jb51.cc/res/2022/10-08/06/f3a8b93f48645d2bb48f74bd078f11a5.png)

其中 DummyDll 里是一些 C#[代码](https://www.jb51.cc/tag/daima/)，里面并没有从 c 和汇编反编译出来的具体[代码](https://www.jb51.cc/tag/daima/)[内容](https://www.jb51.cc/tag/neirong/)，只有元数据，也就是可以用 reflector 或者 ilspy 之类打开查看[代码](https://www.jb51.cc/tag/daima/)结构。要[修改](https://www.jb51.cc/tag/xiugai/)的话 还是只能去用 IDA 去[修改](https://www.jb51.cc/tag/xiugai/)原本的 GameAssembly.dll [文件](https://www.jb51.cc/tag/wenjian/)。

IDA 是用来反编译机器语言到汇编语言，还可以返到不是很好看的 C [代码](https://www.jb51.cc/tag/daima/)。

注意这个 Tool 是我把[文章](https://www.jb51.cc/tag/wenzhang/)开头下载下来的工具解压之后放[在这里](https://www.jb51.cc/tag/zaizheli/)的。然后现在启动我们的神器 IDA, 注意 32 位的就可以，64 位的话可能会出现无法反编译到 C [代码](https://www.jb51.cc/tag/daima/)。

在 IDA 打开这个 GameAssembly.dll 之后，[默](https://www.jb51.cc/tag/mo/)认选项确认进去，可以看到 IDA 会[自动](https://www.jb51.cc/tag/zidong/)开始扫描分析这个 dll [文件](https://www.jb51.cc/tag/wenjian/)，这个时候等待左下角的[提示](https://www.jb51.cc/tag/tishi/)，不再来回闪动了才是分析完成。

好了现在选择 File-》Script File

运行 Il2cpp 所在[文件](https://www.jb51.cc/tag/wenjian/)夹下的 ida_with_struct.py 

然后 IDA 会弹出窗口让你选择几个前面 il2cppdumper [提取](https://www.jb51.cc/tag/tiqu/)出的 json [文件](https://www.jb51.cc/tag/wenjian/)，也就是 script.json 和 il2cpp.h 根据[提示](https://www.jb51.cc/tag/tishi/)窗口[标题](https://www.jb51.cc/tag/biaoti/)就能看到。上面截图就有。

选择完毕 IDA 会开始进一步分析和替换[函数](https://www.jb51.cc/tag/hanshu/)名，这样就可以[快速](https://www.jb51.cc/tag/kuaisu/)确认 steam 连接操作的[代码](https://www.jb51.cc/tag/daima/)位置了。等待分析完成。

寻找到 SteamManager$$Awake() 之后，把它的第一句改成 ret

记得应用[修改](https://www.jb51.cc/tag/xiugai/)

补丁打完了 关掉或者卸载 STEAM 都可以直接进入游戏了。

破解完成

## 总结

以上是[编程之家](https://www.jb51.cc/ "编程之家")为你收集整理的[发布到 steam 的 unity(Il2cpp) 游戏破解方法](https://www.jb51.cc/unity/3756485.html "发布到steam的unity(Il2cpp)游戏破解方法")全部内容。

如果觉得[编程之家](https://www.jb51.cc/ "编程之家")网站内容还不错，欢迎将[编程之家网站](https://www.jb51.cc/ "编程之家网站")推荐给好友。

原文地址：https://www.cnblogs.com/fancybit/p/14389547.html

版权声明：本文内容由互联网用户自发贡献，该文观点与技术仅代表作者本人。本站仅提供信息存储空间服务，不拥有所有权，不承担相关法律责任。如发现本站有涉嫌侵权 / 违法违规的内容， 请发送邮件至 dio@foxmail.com 举报，一经查实，本站将立刻删除。