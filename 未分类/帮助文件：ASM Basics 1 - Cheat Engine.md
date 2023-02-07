**最初由_DABhand发布_**

基础

## 操作码[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=1 "编辑部分：操作码")]

好的，什么是操作码？操作码是处理器可以理解的指令。例如

SUB 和 ADD 和 DIV

sub 指令将两个数字相减。大多数操作码都有操作数

SUB 目的地，来源如下

SUB eax, ecx

SUB 有 2 个操作数。在减法的情况下，源和目标。它将源值减去目标值，然后将结果存储在目标中。操作数可以有不同的类型：寄存器、内存位置、立即值。

所以基本上那个指令是这样的，比如说 eax 包含 20 而 ecx 包含 10

```
eax = eax - ecx 
 eax = 20 - 10 
 eax = 10
```

简单一点吧

## 寄存器[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=2 "编辑部分：寄存器")]

啊哈，这里是 asm 的主力军，寄存器包含值和信息，这些值和信息在程序中用于跟踪事物，当刚接触 ASM 时，它看起来确实很乱，但系统实际上是高效的。老实说

让我们看一下使用的主要寄存器，它的 eax。假设它包含值 FFEEDDCCh（h 表示十六进制），稍后使用 softice 时你会看到很多十六进制值，所以现在就习惯它

好的，我将展示寄存器是如何构造的

```
EAX      FFEEDDCC 
 AX       DDCC 
 AH       DD 
 AL       CC
```

ax, ah, al 是 eax 的一部分。EAX是一个32位寄存器（386+才有），ax包含eax的低16位（2字节），ah包含ax的高字节，al包含ax的低字节。所以ax是16位的，al和ah是8位的。因此，在上面的示例中，这些是寄存器的值：

```
eax = FFEEDDCC（32 位）
 ax = DDCC（16 位）
 ah = DD（8 位）
 al = CC（8 位）
```

理解？我知道要接受很多东西，但这就是寄存器的工作方式这里是操作码和使用的寄存器的更多示例...

```
mov eax, 002130 DF // mov 加载一个值到寄存器 mov cl, ah // 将 ax 的高字节 (30h) 移到 cl sub cl, 10                 // 从 cl 中的值减去 10 (dec.) mov al , cl // 并将其存储在 eax 的最低字节中。
```

所以一开始..

eax = 002130DF

最后

eax = 00213026

你知道发生了什么事吗？我希望如此，因为我正在努力让这一切变得尽可能简单

好的让我们讨论寄存器的类型，主要使用4种类型（还有其他的，但稍后会讲）

### 通用寄存器[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=3 "编辑部分：通用寄存器")]

这些 32 位（及其 16 位和 8 位子寄存器）寄存器可以用于任何用途，但它们的主要用途显示在它们之后。

eax (ax/ah/al) 累加器 ebx (bx/bh/bl) 基数 ecx (cx/ch/cl) 计数器 edx (dx/dh/dl) 数据

如前所述，这些现在几乎不用于其主要目的，而是用于在程序和游戏中传递周围信息（例如分数、健康值等）

### 段寄存器[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=4 "编辑部分：段寄存器")]

段寄存器定义了使用的内存段。对于 win32asm，您可能不需要它们，因为 Windows 有一个平面内存系统。在dos中，内存被分成64kb的段，所以如果你想定义一个内存地址，你指定一个段和一个偏移量（比如0172:0500(segment:offset)）。在 Windows 中，段的大小为 4gig，因此在 win 中不需要段。段始终是 16 位寄存器。

```
CS 代码段
 DS 数据段
 SS 堆栈段
 ES 额外段
 FS  （仅 286+）通用段
 GS  （仅 386+）通用段
```

### 指针寄存器[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=5 "编辑部分：指针寄存器")]

实际上，您可以将指针寄存器用作通用寄存器（eip 除外），只要保留它们的原始值即可。指针寄存器之所以称为指针寄存器，是因为它们常用于存储内存地址。一些操作码（以及 movb、scasb 等）使用它们。

```
esi  (si) 源索引
 edi  (di) 目标索引
 eip  (ip) 指令指针
```

EIP（或 16 位程序中的 IP）包含指向处理器将要执行的指令的指针。所以你不能将 eip 用作通用寄存器。

### 堆栈寄存器[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=6 "编辑部分：堆栈寄存器")]

有 2 个堆栈寄存器：esp 和 ebp。ESP 保存内存中的当前堆栈位置（在下一个教程中有更多相关信息）。EBP 在函数中用作指向局部变量的指针。

```
esp  (sp) 堆栈指针
 ebp  (bp) 基指针
```

## 内存[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=7 "编辑部分：MEMORY")]

ASM内部是如何使用内存的，它的布局是怎样的？好吧，希望这能回答一些问题。请记住，还有比这里解释的更高级的东西，但你们这些人并不高级，所以从基础开始

让我们看看不同的类型..

### DOS [[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=8 "编辑部分：DOS")]

在像 DOS（和 Win 3.1）这样的 16 位程序中，内存被分段。这些段的大小为 64kb。要访问内存，需要一个段指针和一个偏移指针。段指针指示使用哪个段（64kb 的部分），偏移指针指示段本身的位置。

看看这个

```
 -  -  -  -  -  -  -  -  -  -  -  -  -  - 记忆 -  -  -  -  -  -  -  -  -  - - ---------- |第 1 段 (64kb)| 第 2段（64 kb）|第 3 段（64 kb）| 等......... |
```

  
希望表现良好

注意下面的解释是针对 16 位程序的，32 位的稍后再解释（但不要跳过这部分，理解 32 位很重要）。

上表是总内存，以64kb为单位划分的段。最多有 65536 个段。现在取其中一个片段：

```
------------------第 1段（64 kb）-------------------- |偏移量 1 | 偏移2 |偏移 3| 偏移量4 |偏移量 5| 等...... |
```

  
要指向段中的位置，使用偏移量。偏移量是段内的位置。每个段最多有 65536 个偏移量。内存中地址的表示法是：

段：偏移

例如：

0145:42A2（记住所有十六进制数字）

这意味着：段 145，偏移量 42A2。要查看该地址的内容，首先转到段 145，然后转到该段中的偏移量 42A2。

希望你记得刚才在这个线程上阅读过那些段寄存器。

```
CS  - 代码段
 DS  - 数据段
 SS  - 堆栈段
 ES  - 额外段
 FS  - 通用
 GS  - 通用 <<< 他们记得
```

名称解释了它们的功能：代码段 (CS) 包含当前正在执行的代码所在的部分的编号。要从中获取数据的当前段的数据段。Stack 表示堆栈段（稍后会详细介绍堆栈），ES、FS、GS 是通用寄存器，可以用于任何段（但不是在 win32 中）。

指针寄存器大部分时间保存偏移量，但通用寄存器（ax、bx、cx、dx 等）也可用于此目的。IP（指针寄存器）表示当前执行的指令的偏移量（在CS（代码段））。SP（堆栈寄存器）保存当前堆栈位置的偏移量（在 SS（堆栈段）中）。

呸，你认为 16 位内存很难吧？

对不起，如果这一切都令人困惑，但这是解释它的最简单方法。重读几次，它最终会深入你的大脑，了解记忆是如何工作的，以及它是如何被读取和写入的

现在我们搬到

### 32 位 Windows [[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=9 "编辑部分：32 位 Windows")]

您可能已经注意到所有这些关于细分的内容真的一点都不好玩。在 16 位编程中，段是必不可少的。幸运的是，这个问题在 32 位 Windows（9x 和 NT）中得到了解决。

您仍然有段，但不要关心它们，因为它们不是 64kb，而是 4 GIG。如果您尝试更改其中一个段寄存器，Windows 甚至可能会崩溃。

这称为平面内存模型。只有偏移量，它们现在是 32 位的，所以在 0 到 4,294,967,295 的范围内。内存中的每个位置仅由偏移量指示。

这确实是 32 位相对于 16 位的最大优势之一。所以你现在可以忘记段寄存器并关注其他寄存器。

哦，这一切都太疯狂了，哇 4 gig bits 可以使用

## 有趣的部分[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=10 "编辑部分：有趣的部分")]

**有趣的部分开始了！！！**

它是

**操作码**

这是一些操作码的列表，您在制作培训师或破解等时会注意到很多。

### MOV [[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=11 "编辑部分：MOV")]

该指令用于将值从一个位置移动（或实际复制）到另一个位置。这个“位置”可以是寄存器、内存位置或立即值（当然仅作为源值）。mov指令的语法是：

```
移动目的地，来源
```

您可以将一个值从一个寄存器移动到另一个寄存器（请注意，该指令将值复制到目的地，尽管其名称为“移动”）。

```
移动 edx , ecx
```

上面的指令将 ecx 的内容复制到 edx。源和目标的大小应该相同，例如这条指令是无效的：

```
mov al, ecx ; 无效_
```

此操作码尝试将 DWORD（32 位）值放入一个字节（8 位）中。这不能通过 mov 指令来完成（还有其他指令可以做到这一点）。但是这些指令是允许的，因为源和目标的大小没有区别，例如......

```
mov  al, bl 
 mov  cl, dl 
 mov  cx, dx 
 mov  ecx, ebx
```

内存位置用偏移量表示（在 win32 中，有关更多信息，请参见上一页）。您还可以从某个内存位置获取一个值并将其放入寄存器中。以下表为例：

```
偏移量   34 35 36 37 38 39 3A 3B 3C 3D 3E 3F 40 41 42
 数据     0D 0A 50 32 44 57 25 7A 5E 72 EF 7D FF AD C7
```

（每个块代表一个字节）

偏移值在这里表示为一个字节，但它是一个 32 位值。以 3A 为例（这不是偏移量的常用值，否则表格将不适合...），这也是一个 32 位值：0000003Ah。只是为了节省空间，使用了一些不寻常的低偏移量。所有值都是十六进制代码。

查看上表中的偏移量 3A。该偏移量处的数据是 25、7A、5E、72、EF 等。要将偏移量 3A 处的值放入例如寄存器中，您也可以使用 mov 指令：

```
mov  eax , dword  ptr  [0000003Ah] ...但是......
```

您会在程序中更常见地看到这一点

```
mov eax, dword ptr [ecx+45h]
```

这意味着 ecx+45 将指向要从中获取 32 位数据的内存位置，由于指令中的双字，我们知道它是 32 位。比如说 16 位数据，我们使用 WORD PTR 或 8 位 BYTE PTR，如下例所示。

```
mov cl, byte ptr [ 34 h] cl 将得到值0  Dh（见上表） mov dx, word ptr [ 3 Eh] dx 将得到值7  DEFh（见上表，记住字节是颠倒的）
```

大小有时不是必需的：

```
移动eax， [ 00403045h ]
```

因为 eax 是一个 32 位寄存器，所以汇编程序假设（这也是唯一的方法）它应该从内存位置 403045 中获取一个 32 位值。

也允许立即数：

```
移动 edx , 5006
```

这只会使寄存器 edx 包含值 5006。括号 [and] 用于从括号之间的内存位置获取值，没有括号它只是一个值。允许使用寄存器作为内存位置（在 32 位程序中应该是 32 位寄存器）：

```
mov eax, 403045 h ; 使 eax 的值为 403045十六进制。 mov cx, [eax] ; 将内存位置EAX ( 403045 )的字长值放入寄存器 CX。
```

在 mov cx, [eax] 中，处理器首先查看 eax 保存的值（=内存位置），然后是内存中该位置的值，然后放入这个字（16 位，因为目标 cx 是 16 位注册）到 CX。

呸

### ADD、SUB、MUL 和 DIV [[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=12 "编辑部分：ADD、SUB、MUL 和 DIV")]

这些很容易理解 很好的老数学，我相信每个人都会加减乘除

无论如何，关于信息

添加操作码具有以下语法：

添加目的地、来源

执行的计算是目标 = 目标 + 源。允许使用以下形式：

```
目标源示例 注册 注册 添加ecx, edx 寄存器内存 添加ecx, dword ptr [ 104 h] /添加ecx, [edx] Register Immediate value                   add eax, 102 Memory 立即值                  add dword ptr [ 401231 h], 80 Memory Register add dword ptr [ 401231 h], edx
```

这个指令非常简单。它只是获取源值，将目标值添加到它，然后将结果放在目标中。其他数学指令是：

```
SUB 目标，源（目标 = 目标 -源） MUL 目标，源（目标 = 目的地 *源） DIV来源(eax = eax / source , edx = remainer)
```

这很简单，是不是还是

减法与加法相同，乘法只是 dest = dest * source。划分有点不同。因为寄存器是整数值（即整数，而不是浮点数），所以除法的结果分为商和余数。例如：

```
28  / 6 --> 商 = 4，余数 = 4 
 30  / 9 --> 商 = 3，余数 = 3 
 97  / 10 --> 商 = 9，余数 = 7 
 18  / 6 --> 商 = 3，余数= 0
```

现在，根据源的大小，商存储在 eax（的一部分）中，余数存储在 edx（的一部分）中：

```
Source size Division Quotient 存储在 Remainder 存储在... BYTE（8 位）ax/ source AL AH WORD（16 位）dx:ax* /源 AX DX DWORD（32 位）edx:eax* / source EAX EDX
```

*   = 例如：如果 dx = 2030h，且 ax = 0040h，dx: ax = 20300040h。dx:ax 是一个双字值，其中 dx 代表

字越高，斧头越低。edx:eax 是一个四字值（64 位），其中较高的双字是 edx，较低的双字是 eax。

div-opcode 的来源可以是：

```
一个 8 位寄存器 (al, ah, cl,...)
 一个 16 位寄存器 (ax, dx, ...)
 一个 32 位寄存器 (eax, edx, ecx...)
 一个 8 位内存value (byte ptr [xxxx])
 一个 16 位内存值 (word ptr [xxxx])
 一个 32 位内存值 (dword ptr [xxxx])
```

源不能是立即数，因为处理器无法确定源操作数的大小。

### 按位操作[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=13 "编辑部分：BITWISE OPS")]

除了“NOT”指令外，这些指令都带有一个目的地和一个来源。将目标中的每一位与源中的相同位进行比较，并根据指令将 0 或 1 放置在目标位中：

```
指令 AND OR XOR NOT 源位 |0 0 1 1| 0  0  1  1 |0 0 1 1| 0  1 | 目标位 | 0  1  0  1 |0 1 0 1| 0  1  0  1 |XX| 输出位 |0 0 0 1| 0  1  1  1 |0 1 1 0| 1  0 |
```

如果源位和目标位均为 1，则 AND 将输出位设置为 1。如果源位或目标位为 1，则 OR 设置输出位 如果源位与目标位不同，则 XOR 设置输出位。NOT 反转源位。

一个例子：

```
mov  ax, 3406 
 mov  dx, 13EAh
 异或 ax, dx
```

ax = 3406（十进制），二进制为 0000110101001110。

dx = 13EA（十六进制），二进制为 0001001111101010。

对这些位执行异或运算：

```
来源                   0001001111101010 (dx)
 目的地0000110101001110             (ax)
 输出                    0001111010100100 (new dx)
```

指令后的新 dx 为 0001111010100100（十进制 7845，十六进制 1EA5）。

另一个例子：

```
mov  ecx, FFFF0000h
 不是 ecx
```

FFFF0000 是二进制 11111111111111110000000000000000（16 个 1，16 个 0）

如果你取每一位的倒数，你会得到：

```
00000000000000001111111111111111（16个 0，16个 1），即十六进制的 0000FFFF 。
```

所以ecx在NOT操作之后是0000FFFFh。

最后一个对于串行生成很方便，就像 XOR 一样。事实上，XOR 比任何其他指令更多地用于串行，广泛用于 Winzip、Winrar、EA Games、Vivendi Universalis 中的串行检查

我不会告诉你如何制作密钥生成器，所以不要问 :)

### INC/DEC(REMENTS) [[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=14 "编辑部分：INC/DEC(REMENTS)")]

有 2 条非常简单的指令，DEC 和 INC。这些指令增加或减少一个内存位置或寄存器。简单的说：

```
inc reg -> reg = reg + 1 dec reg -> reg = reg - 1 inc dword ptr [ 103405 ] -> [ 103405 ] 处的值将增加1。 dec dword ptr [ 103405 ] -> [ 103405 ] 处的值将减一。
```

啊，很容易理解下一个也是如此

### NOP [[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=15 "编辑部分：NOP")]

该指令绝对不执行任何操作。这条指令只是占用空间和时间。它用于填充目的和补丁代码。

### BIT 旋转和移位[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=16 "编辑部分：BIT旋转和移位")]

注意：下面的例子大多使用 8 位数字，但这只是为了让图片更清晰。

换档功能

```
SHL 目的地，计数
 SHR 目的地，计数
```

SHL 和 SHR 将寄存器/内存位置中的位数向左或向右移动。

例子：

```
;  al = 01011011（二进制）这里是
 shr  al, 3
```

这意味着：将 al 寄存器的所有位向右移动 3 位。所以al会变成00001011，左边的位补0，右边的位移出。移出的最后一位保存在进位标志中。进位位是处理器标志寄存器中的一位。这不是您可以直接访问的像 eax 或 ecx 这样的寄存器（尽管有操作码可以这样做），但它的内容取决于指令的结果。这将在后面解释，你现在唯一需要记住的是进位在标志寄存器中，它可以打开或关闭。该位等于移出的最后一位。

shl 与 shr 相同，但向左移动。

```
;  bl = 11100101（二进制）这里是
 shl  bl, 2
```

指令后的 bl 为 10010100（二进制）。最后两位补零，进位位为1，因为最后移出的位是1。

  
然后还有另外两个操作码：

SAL 目标，计数（左移算术） SAR 目标，计数（右移算术）

SAL 与 SHL 相同，但 SAR 与 SHR 不完全相同。SAR 不会移入零，但会复制 MSB（最高有效位 - 第一位，如果为 1，则它从左侧移动 1，如果为 0，则将从左侧放置 0）。例子：

```
al = 10100110 
 sar  al, 3 
 al = 11110100 
 sar  al, 2 
 al = 11111101
 
 bl = 00100110 
 sar  bl, 3 
 bl = 00000100
```

这个你可能有问题要处理

旋转功能

```
rol目的地，计数；向左旋转 ror 目的地，计数；向右旋转 rcl 目的地，计数；通过左进位旋转 rcr 目的地，计数；通过进位旋转
```

旋转看起来像移位，不同之处在于移出的位在另一侧再次移入：

示例：ror（向右旋转）

```
Bit  7 Bit 6 Bit 5 Bit 4 Bit 3 Bit 2 Bit 1 Bit 0 
 Before                                     1 0 0 1 1 0 1 1
 循环 计数 3 1 0 0 1 1 0 1 1 (Shift out)
 结果                                    0 1 1 1 0 0 1 1
```

正如您在上图中看到的那样，这些位是旋转的，即被推出的每一位都在另一侧再次移入。与移位一样，进位位保存最后移出的位。RCL和RCR其实和ROL和ROR是一样的。他们的名字暗示他们使用进位位来表示最后一个移出位，这是正确的，但是由于ROL和ROR的作用相同，因此它们与它们没有区别。

### 交换[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=17 "编辑部分：交换")]

很简单，我不会详细介绍，它只是交换两个寄存器的值（值，地址）。比如例子..

```
eax = 237h 
 ecx = 978h 
 xchg  eax, ecx 
 eax = 978h 
 ecx = 237h
```

无论如何，第一天结束后，如果您将这一点牢记在心，接下来的日子就会变得容易而不是困难。这是我教给你的基础知识。好好学习。

## 链接[[编辑](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_1&action=edit&section=18 "编辑部分：链接")]

*   [帮助文件](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Help_File "作弊引擎：帮助文件")

*   [后退](https://wiki.cheatengine.org/index.php?title=Help_File:What_is_the_difference_in_bytetype "帮助文件：字节类型有什么区别")

*   [下一个](https://wiki.cheatengine.org/index.php?title=Help_File:ASM_Basics_2 "帮助文件：ASM基础2")

NewPP 限制报告缓存时间：20230206123902 缓存过期：86400 动态内容：false CPU 使用时间：0.046 秒实时使用：0.047 秒预处理器访问节点数：71/1000000 预处理器生成节点数：76/1000000 扩展后包含大小：0 /2097152 字节模板参数大小：0/2097152 字节最高扩展深度：2/40 昂贵的解析器函数计数：0/100 取消剥离递归深度：0/20 取消剥离后扩展大小：0/5000000 字节

包含扩展时间报告 (%,ms,calls,template) 100.00% 0.000 1 -total

使用密钥 wikidb-cew_:pcache:idhash:965-0!canonical 和时间戳 20230206123902 和修订 ID 4363 保存在解析器缓存中