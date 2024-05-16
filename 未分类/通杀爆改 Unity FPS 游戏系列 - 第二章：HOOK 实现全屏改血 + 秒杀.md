![](https://avatar.52pojie.cn/data/avatar/000/65/78/24_avatar_middle.jpg)

lyl610abc _本帖最后由 lyl610abc 于 2023-9-18 20:10 编辑_  

本篇为 Windows 篇，Android 篇将和 @正己  梦幻联动，敬请期待

# 索引

[通杀爆改 Unity FPS 游戏系列 - 序章：介绍及游戏下载](https://www.52pojie.cn/thread-1829474-1-1.html)

[通杀爆改 Unity FPS 游戏系列 - 第一章：常规搜索 + 通杀结构解析](https://www.52pojie.cn/thread-1830230-1-1.html)

[通杀爆改 Unity FPS 游戏系列 - 第三章：il2cpp mono 差异](https://www.52pojie.cn/thread-1835443-1-1.html)

# 本章内容

本篇以 windows il2cpp 包为例实现功能：

全屏改血

全屏秒杀

* * *

# 思路

先对全屏秒杀这个功能进行拆解

*   全屏：作用范围为**作用域**内的所有敌人
*   秒杀：修改血量为 0  触发死亡

* * *

## 作用域

出于性能考虑，许多游戏并不会将整个地图上的所有敌人都加载出来

比如经典的吃鸡，其作用域就是几百米，游戏只加载了玩家附近几百米的敌人，透视自瞄也只能读到整个作用域内的敌人

当然我的 demo 肯定没有这个限制，作用域内的敌人就是所有敌人了

* * *

## 全屏

有了作用域的概念后，所谓的全屏实质上就是得要能够作用到作用域内的敌人

这里就有 2 个思路：遍历和公共函数 HOOK

### 遍历

通常来说，游戏开发者为了方便管理，都会将所有敌人放到一个数组 / 列表 中

所谓的遍历，通俗来讲，就是每次从敌人数组 / 列表中取一个敌人出来，一直取到所有敌人都取完为止

取出来 --> 执行秒杀操作

* * *

### 公共函数 HOOK

#### 公共函数

公共函数，即所有目标都会调用到的函数

在此次案例中就是所有敌人都会调用到的函数

既然每个敌人都会调用到，在调用的时候通过 HOOK ，加点私货就可以达到秒杀的效果

* * *

#### HOOK

HOOK：钩子，粗浅来说就是拦截后再放行

好比：正己老师某天要从家里去上班，但好巧不巧，路上被人打劫了，被**劫色**后（杰哥不要 {{{(>_<)}}}），还是被放行回去上班

把这个事情转换一下

类对象是：正己老师

函数是：上班

HOOK 是：打劫

HOOK 干了啥：劫色

正己老师的目标是什么：上班

目标完成了没：完成了，还是一瘸一拐地上班去了。这里的一瘸一拐是什么造成的？ 劫色

也就是可以通过 劫色 对 正己老师 造成 一瘸一拐 的效果；通过 HOOK 对 类对象 的 状态 进行修改

* * *

浅浅总结一下：通过 HOOK，可以对被 HOOK 的对象进行状态修改，但对象原本要做的事情还是会去做

* * *

由此，到这里的全屏就是：

找到一个所有敌人都会调用到的函数，HOOK 这个函数，劫色敌人，不好意思走错片场了，是把敌人的状态弄成死亡，最后再去执行函数原本的逻辑

* * *

# 实战

## 遍历

有了第一章的铺垫，直接使用 mono + 关键词法，可以迅速定位到存储所有敌人的列表：fps.AI 下的 Unity.FPS.AI.EnemyManager

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912183202539.png)

* * *

然后使用第一章的 Lookup instances 大法，可以很轻松地过滤得到所有敌人：

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230906114714781.png)

* * *

把这个内存地址丢到 Structure Dissect 里（忘记在哪的回去翻第一章！╰（‵□′）╯）

展开 _items 后，这里的 _size 表示的是当前敌人数量，毕竟刚开始，所以只有 3 个敌人

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230906115012397.png)

* * *

然后展开到这个敌人的血量，改成 0 试试

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230906114947915.png)

* * *

改成 0 后返回游戏可以看到是有效果的，但是敌人没有死掉 QAQ

这是因为死亡逻辑是在敌人受到伤害时做判断，如果受伤后血量小于 0  才会执行死亡函数

这里直接修改血量，并不会触发死亡，所以遍历后只是修改血量还不够，还得再调用死亡函数

死亡函数的内容放到后面 HOOK 里，这里主要是介绍一下遍历和重温下上一章的内容

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230906120135236.png)

* * *

## 公共函数 HOOK

### 挑选公共函数

按照前面的前面的思路，第一步就是要寻找合适的公共函数

所谓的合适指的是函数执行的时机，还是以前面的正己老师为例

温故下例子：正己老师某天要从家里去上班，但好巧不巧，路上被人打劫了，被**劫色**后（杰哥不要 {{{(>_<)}}}），还是被放行回去上班

这里的时机，就是上班

但万一某天正己老师没上班（未触发函数），那不就没法对他进行劫色（HOOK）了吗

因此可以换个更好的时机，比如正己老师呼吸的时候

正己老师每呼吸一次，就对他进行劫色，这个时机就可以说是能够覆盖到正己老师的大多情况了

* * *

回归主题，对应到游戏内敌人的情况，则是需要找一个函数

这个函数的触发时机最好能够覆盖到所有敌人，且不需要特殊的触发条件，正常就能触发到

先定位到敌人对应的类：fps.AI.dll 下的 Unity.FPS.AI.EnemyController

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912183248311.png)

* * *

虽然敌人没有呼吸这个函数，但有个关键的函数 Update 刷新

这个刷新函数能够完美满足我们的需求，于此同时其实还有一些其他函数，HOOK 他们能实现其他效果

比如 TryAttack 函数，敌人尝试攻击我们的时候直接让它死掉 w(ﾟДﾟ)w

这里就选定 Update 函数了

* * *

### 关联公共函数和功能

先理一下联系

在公共函数处能拿到的类地址是：EnemyController 敌人

实现敌人死亡需要的类地址是：Health 生命

所以需要做的事：EnemyController → m_Health → CurrentHealth 修改为 0  → 调用死亡函数

只是修改敌人血量为 0 不会触发死亡，还需要额外调用死亡函数，死亡函数具体在后面再展开

* * *

## HOOK 实现全屏改血

先从相对简单的改血开始，死亡函数先放在后面

先下双击 Update 函数，跳转到函数头部，然后快捷键下断点（这部分在第一章详细演示过，这里稍微简略些）

查看寄存器和堆栈数据

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230908155028281.png)

* * *

根据第一章的找血量部分的内容

对照函数的参数，其实是可以推测出堆栈中内存地址的含义的

```
[ENABLE]
//code from here to '[DISABLE]' will be used to enable the cheat
alloc(newmem,2048)
label(returnhere)
label(originalcode)
label(exit)
 
newmem: //this is allocated memory, you have read,write,execute access
//place your code here
push eax
mov eax,[esp+4+4]
//将 EnemyController 作为参数
push eax
//调用EnemyController.OnDie
call 1F4F6970
//堆栈外平衡
add esp,4
//还原寄存器
pop eax
 
originalcode:
push ebp
mov ebp,esp
push edi
sub esp,74
 
exit:
jmp returnhere
 
Unity.FPS.AI.EnemyController:Update:
jmp newmem
nop 2
returnhere:
 
 
  
  
[DISABLE]
//code from here till the end of the code will be used to disable the cheat
dealloc(newmem)
Unity.FPS.AI.EnemyController:Update:
db 55 8B EC 57 83 EC 74
//push ebp
//mov ebp,esp
//push edi
//sub esp,74
```

* * *

再使用结构解析面板解析获取到的 EnemyController 的内存地址

可以验证所获取的地址就是我们想要的

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230908160418383.png)

* * *

接下里就是要记下我们目标对应的偏移量

EnemyController 偏移 00D8 得到 m_Health

m_Health 偏移 0020 得到 CurrentHealth

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230908160813079.png)

* * *

至此，HOOK 实现全屏改血所需要的信息已经足够了，Ready Perfectly (_￣(￣_ )

回到 CE 修改器的 Memory Viewer 界面，Tools -> Auto Assemble （或使用快捷键 Ctrl + A）

PS：注意要选中 Update 函数头部，这关系到 HOOK 的位置，这里选择在头部进行 HOOK

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230911163519744.png)

* * *

点击以后出现了新窗口：Auto assemble

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230911164949731.png)

* * *

先通过 Template → Cheat Table Framework Code  载入模板 （或使用快捷键 Ctrl  + Alt + T）

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230911165124667.png)

* * *

载入以后可以看到多了几行代码，主要是用来控制脚本的开关

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230911165315579.png)

* * *

再通过 Template → Code Injection 载入模板 (或使用快捷键 Ctrl + I)

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230911165508515.png)

* * *

弹出新窗口，要求输入想要 HOOK 的地址，默认是当前选中的地址，这也是前面提到要选中函数头部的原因

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230911165620671.png)

* * *

确认以后得到一串代码，我们只需要关注 ENABLE 部分

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230911175719340.png)

* * *

先贴出生成的代码，然后稍微说明一下生成代码的含义

```
[ENABLE]
//code from here to '[DISABLE]' will be used to enable the cheat
alloc(newmem,2048)
label(returnhere)
label(originalcode)
label(exit)

newmem: //this is allocated memory, you have read,write,execute access
//place your code here

originalcode:
push ebp
mov ebp,esp
push -01

exit:
jmp returnhere

"GameAssembly.dll"+1F4800:
jmp newmem
returnhere:

[DISABLE]
//code from here till the end of the code will be used to disable the cheat
dealloc(newmem)
"GameAssembly.dll"+1F4800:
db 55 8B EC 6A FF
//push ebp
//mov ebp,esp
//push -01
```

* * *

正常来说，一般是从上到下来看代码，但是这里以实际的逻辑顺序进行说明

整体的逻辑为：

1.  修改原本的游戏逻辑为跳转到我们自己的逻辑 (会覆盖原本的逻辑)
2.  执行自己的逻辑
3.  执行被我们覆盖掉的部分
4.  跳转回去

* * *

<table><thead><tr><th></th><th>对应段</th><th>相关代码</th><th>说明</th><th>例子</th></tr></thead><tbody><tr><td>修改游戏逻辑为跳转到自己的逻辑</td><td>"GameAssembly.dll"+1F4800:</td><td>jmp newmem&lt;br/&gt;returnhere:</td><td>跳转到自己逻辑，同时记录要跳回的地址</td><td>拦路抢劫</td></tr><tr><td>执行自己的逻辑</td><td>newmem:</td><td></td><td>自己的逻辑，还没写</td><td>劫色?</td></tr><tr><td>执行被覆盖掉的部分</td><td>originalcode:</td><td>push ebp&lt;br/&gt;mov ebp,esp&lt;br/&gt;push -01</td><td>jmp newmem 覆盖掉的部分</td><td>被拦路的人继续走路</td></tr><tr><td>跳转回去</td><td>exit:</td><td>jmp returnhere</td><td>跳转回原本的逻辑</td><td>被拦路的人完成剩下的路</td></tr></tbody></table>

* * *

通过分析生成的代码，可以看到 CE 修改器已经十分智能地为我们搭好了框架，接下来就是编写自己的逻辑了

为了避免大家往回翻，这里贴出之前得到的必要信息：

<table><thead><tr><th></th><th>内容</th></tr></thead><tbody><tr><td>ESP</td><td>返回地址</td></tr><tr><td>ESP+4</td><td>EnemyController</td></tr><tr><td>EnemyController+00D8</td><td>m_Health</td></tr><tr><td>m_Health+20</td><td>CurrentHealth</td></tr></tbody></table>

```
newmem: //this is allocated memory, you have read,write,execute access
//place your code here
//随便找个寄存器保存，因为中途需要借助寄存器来赋值，这里也可以改成 ebx ecx ...
push eax
//注意这个地方，原本 EnemyController 的地址应该是 ESP+4 但是因为我们保存了个寄存器，导致位置变化，所以需要再加 4
//这里就相当于是 eax = EnemyController 
mov eax,[esp+4+4]
//这里就相当于是 eax = EnemyController+d8 = m_Health
mov eax,[eax+d8]
//这里就相当于是 [eax+20]= [m_Health+20] = [CurrentHealth] = 0
mov [eax+20],(float)0
//用完以后要还原回去,保存的是什么寄存器就还原什么寄存器
pop eax
originalcode:
```

* * *

修改完以后得到的完整代码是：

```
[ENABLE]
//code from here to '[DISABLE]' will be used to enable the cheat
alloc(newmem,2048)
label(returnhere)
label(originalcode)
label(exit)

newmem: //this is allocated memory, you have read,write,execute access
//place your code here
push eax
mov eax,[esp+4+4]
mov eax,[eax+d8]
mov [eax+20],(float)0
pop eax
originalcode:
push ebp
mov ebp,esp
push -01

exit:
jmp returnhere

"GameAssembly.dll"+1F4800:
jmp newmem
returnhere:

[DISABLE]
//code from here till the end of the code will be used to disable the cheat
dealloc(newmem)
"GameAssembly.dll"+1F4800:
db 55 8B EC 6A FF
//push ebp
//mov ebp,esp
//push -01
```

* * *

写好代码以后，不急着点执行，先通过 File → Assign to current cheat table 把它保存下来

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912174111477.png)

* * *

保存后，可以在 cheat table 里找到，点击 Active 激活

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912174323106.png)

* * *

### 全屏改血效果展示

可以看到所有的敌人的血量全部为 0

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912175536278.png)

* * *

## 死亡函数

虽然成功把敌人的血量改成了 0，但很尴尬的是敌人并没有立刻死亡，于是得开始寻觅死亡函数

针对 Health 的死亡函数并不难找，就在 fps.Game.dll → Unity.FPS.Game.Health → HandleDeath 里

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912183000721.png)

* * *

但是经过测试会发现一个问题，在 il2cpp 打包的情况下，HandleDeath 函数并不会被触发，但在 mono 打包的情况下又会被触发

这个放到之后的篇章细说 il2cpp 和 mono 的区别

本篇是以 il2cpp 为例，因此死亡函数就不能选择 HandleDeath

但好在死亡函数不止一个，可以找到另一个死亡函数： fps.Game.dll → Unity.FPS.AI.EnemyController → OnDie

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912204313373.png)

* * *

## HOOK 实现全屏秒杀

在第一章中，有提到过：相比函数原型，在汇编之中**其实还会多出一个参数：这个类实例本身的内存地址**

因此想要调用 EnemyController.OnDie 函数，来让敌人死亡，实际上只需要传入 EnemyController 的地址即可

下面给出伪代码：

```
push EnemyController的地址
call EnemyController.OnDie
//这里之所以要 add esp,04 是因为压入了一个参数，如果是压入了两个参数，则应是 add esp,08
//这里是做了一个堆栈的外平衡，且因为调用协定是 stdcall，所以才这么写
add esp,04
```

有关堆栈相关知识点可回顾：[逆向基础笔记七 堆栈图（重点）](https://www.52pojie.cn/thread-1379952-1-1.html)

有关调用协定相关的知识点可回顾：[逆向基础笔记九 C 语言内联汇编和调用协定](https://www.52pojie.cn/thread-1380788-1-1.html)

* * *

可以根据伪代码结合 Update 的上下文写出 HOOK 代码：

```
newmem: //this is allocated memory, you have read,write,execute access
//place your code here
//随便找个寄存器保存，因为中途需要借助寄存器来赋值，这里也可以改成 ebx ecx ...
push eax
//注意这个地方，原本 EnemyController 的地址应该是 ESP+4 但是因为我们保存了个寄存器，导致位置变化，所以需要再加 4
//这里就相当于是 eax = EnemyController 
mov eax,[esp+4+4]
//将 EnemyController 作为参数
push eax
//这里就是调用 EnemyController.OnDie
call GameAssembly.dll+1F2D50
//堆栈外平衡
add esp,4
//用完以后要还原回去,保存的是什么寄存器就还原什么寄存器
pop eax
originalcode:
```

* * *

稍微说明一下 GameAssembly.dll+1F2D50 这个地址的由来，以免有些小伙伴不解

双击 OnDie 函数

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912204313373.png)

* * *

双击后跳转到 Memory Viewer 界面

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912213312658.png)

* * *

最后再给出完整的全屏秒杀代码：

```
[ENABLE]
//code from here to '[DISABLE]' will be used to enable the cheat
alloc(newmem,2048)
label(returnhere)
label(originalcode)
label(exit)

newmem: //this is allocated memory, you have read,write,execute access
//place your code here
push eax
mov  eax,[esp+4+4]
push eax
call GameAssembly.dll+1F2D50
add esp,4
pop eax
originalcode:
push ebp
mov ebp,esp
push -01

exit:
jmp returnhere

"GameAssembly.dll"+1F4800:
jmp newmem
returnhere:

[DISABLE]
//code from here till the end of the code will be used to disable the cheat
dealloc(newmem)
"GameAssembly.dll"+1F4800:
db 55 8B EC 6A FF
//push ebp
//mov ebp,esp
//push -01
```

* * *

保存后，可以在 cheat table 里找到，点击 Active 激活

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912174323106.png)

* * *

### 全屏秒杀效果展示

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/5ede2a4e0a9d04760e8cf5512de1320b.png)

* * *

# 作业

你已经学完 1+1=2 了，是时候来点高等数学了 (〃￣︶￣)(￣︶￣〃)

前面已经提到了 mono 版本是可以 HOOK Health.HandleDeath 的

实现 mono 版本的全屏秒杀就当作是作业了

# 总结

*   通过 HOOK，可以对被 HOOK 的对象进行**状态修改**（用骚操作去搞别的对象也可以，但这里说的是通常情况），但对象原本要做的事情还是会去做
*   HOOK 时需要注意**时机**，即被 HOOK 函数的上下文环境是否能够获取到**想要的数据** (本章例子为死亡函数和血量) 和**被调用的频率**
*   相比函数原型，在汇编之中其实还会多出一个参数：这个类实例本身的内存地址，这个参数往往能作为修改的**突破点**
*   il2cpp 和 mono 在部分地方会有些差异，但 CE 修改器的 mono 功能通常情况都是**通用**的，都能够解析游戏的结构，无需太过在意打包方式

* * *

这一章稍微引入了一些汇编代码，在难度上有一点点提升，但实现的功能强度也有所提升，可以不用每次手动操作了 (￣︶￣*))

同时也抛砖引玉，稍微表现出了 il2cpp 和 mono 不同的点，但**大同小异**，限于篇幅：异处放到后面的章节再细说

最后附上成本的 CE 修改器脚本，即开即用：

![](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/image-20230912221133006.png)

下载地址：

[https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/FpsDemo_By_lyl610abc_il2cpp.CT](https://610-pic-bed.oss-cn-shenzhen.aliyuncs.com/FpsDemo_By_lyl610abc_il2cpp.CT)

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

查小布 期待安卓篇

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

ryanu 感谢大佬的教程，来交作业了  

[Asm] _纯文本查看_ _复制代码_

```
[ENABLE]
//code from here to '[DISABLE]' will be used to enable the cheat
alloc(newmem,2048)
label(returnhere)
label(originalcode)
label(exit)
 
newmem: //this is allocated memory, you have read,write,execute access
//place your code here
push eax
mov eax,[esp+4+4]
//将 EnemyController 作为参数
push eax
//调用EnemyController.OnDie
call 1F4F6970
//堆栈外平衡
add esp,4
//还原寄存器
pop eax
 
originalcode:
push ebp
mov ebp,esp
push edi
sub esp,74
 
exit:
jmp returnhere
 
Unity.FPS.AI.EnemyController:Update:
jmp newmem
nop 2
returnhere:
 
 
  
  
[DISABLE]
//code from here till the end of the code will be used to disable the cheat
dealloc(newmem)
Unity.FPS.AI.EnemyController:Update:
db 55 8B EC 57 83 EC 74
//push ebp
//mov ebp,esp
//push edi
//sub esp,74
```

但是只要重启游戏后就失效了。。

![](https://static.52pojie.cn/static/image/smiley/default/9.gif)

[QQ 截图 20230913225341.png](forum.php?mod=attachment&aid=MjY0Mjg0NHw2ODU5ZmQ4MXwxNzEyMTMwNTIyfDB8MTgzMjk2NA%3D%3D&nothumb=yes) _(1.15 MB, 下载次数: 0)_

[下载附件](forum.php?mod=attachment&aid=MjY0Mjg0NHw2ODU5ZmQ4MXwxNzEyMTMwNTIyfDB8MTgzMjk2NA%3D%3D&nothumb=yes)

2023-9-13 23:01 上传[](javascript:;)

[![](https://static.52pojie.cn/static/image/common/rleft.gif)](javascript:;)

[

![](https://static.52pojie.cn/static/image/common/rright.gif)

](javascript:;)

![](https://attach.52pojie.cn/forum/202309/13/230129agcjitvbnz2nbpxl.png)

![](https://avatar.52pojie.cn/data/avatar/000/29/40/41_avatar_middle.jpg)

艾莉希雅 那万一时机不对呼吸的正己老师没劫到，反而把所有呼吸的人给劫了是不是就大罪过了

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

janken 这篇难上加难么。

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

duokebei 属于是遇见大佬了，学习

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

scbzwv 讲解的非常详细，感谢分享

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

898522783 好家伙，杰哥都来了。楼主好高产。

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

BonnieRan 看完了 强强强，举的例子也是生动形象 一眼看懂，越来越期待 Android 篇啦

![](https://static.52pojie.cn/static/image/smiley/laohu/laohu13.gif)

![](https://avatar.52pojie.cn/images/noavatar_middle.gif)

Chenda1 非常多干货  哈哈

![](https://static.52pojie.cn/static/image/smiley/laohu/laohu13.gif)

![](https://avatar.52pojie.cn/data/avatar/001/53/08/91_avatar_middle.jpg)

尹铭 膜拜大佬~