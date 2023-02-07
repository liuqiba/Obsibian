**1 Here Document**

  
**1.1 免交互定义**  
使用 I/O 重定向的方式将命令列表提供给交互式程序或命令，比如 ftp、cat 或 read 命令。

是标准输入的一种替代品可以帮助脚本开发人员不必使用临时文件来构建输入信息，而是直接就地生成出一个 “文件” 并用作 “命令” 的标准输入。

Here Document 也可以与非交互式程序和命令一起使用。

**1.2 语法格式**  
语法格式  
命令 << 标记  
…  
内容 #标记直接是传入内容  
…  
标记  
标记可以使用任意合法字符 （通常使用 EOF）

结尾的标记一定要顶格写，前面不能有任何字符

结尾的标记后面也不能有任何字符（包括空格）

开头标记前后的空格会被省略掉

**1.3 实验**  
1、免交互方式实现对行数的统计，将要统计的内容置于标记 “EOF” 之间，直接将内容传给 wc -l 来统计  
 

![](https://img-blog.csdnimg.cn/9ed509aad2454fd1938f77b36b3c8ce6.png)

2、用 read 命令接收用户的输入值时会有交互过程，在 EOF 两个标记间可以输入变量值

![](https://img-blog.csdnimg.cn/234583e724da49b1b3dd34a54371fb73.png)

3、使用 passwd 给用户设置密码

![](https://img-blog.csdnimg.cn/db53c47b020c4b6bac31e2778921cb2e.png)

**1.4 变量设定**  
Here Document 支持使用变量，如果标记之间有变量被使用，会先替换变量值。

想要将一些内容写入文件，除了常规的方法外，也可以使用 Here Document。

写入的内容中包含变量，在写入文件时要先将变量替换成实际值，在结合 cat 命令完成写入。

**1.5 例子**  
1、在写入文件时会先将变量替换成实际值，再结合 cat 命令完成写入

![](https://img-blog.csdnimg.cn/f4f7664ea32f4fd3aa1393a1c79820cd.png)

2、整体赋值给变量，然后通过 echo 命令将变量值打印出来

![](https://img-blog.csdnimg.cn/a2a9684a6d0647209e118e4792e6affd.png)

3、在标记上添加双引号，关闭变量替换的功能

![](https://img-blog.csdnimg.cn/dd5961e3b3484a13a6eeff156570fa67.png)

## 1.6 多行注释

• Bash 的默认注释是 “#”，该注释方法只支持单行注释: Here Document 的引入解决了多行注释的问题  
• “：” 代表什么都不做的空命令。中间标记区域的内容不会被执行，会被 bash 忽略掉，因此可达到批量注释的效果#

![](https://img-blog.csdnimg.cn/45dd7d0e90bd49a7bedb2e2fd83cf8d4.png)

**2 Expect**  
2.1expect 定义  
是建立在 tcl（tool command language）语言基础上的一个工具

常被用于进行自动化控制和测试，解决 [shell 脚本](https://so.csdn.net/so/search?q=shell%E8%84%9A%E6%9C%AC&spm=1001.2101.3001.7020)中交互的相关问题

不是自带的，需要 yum -y install [expect](https://so.csdn.net/so/search?q=expect&spm=1001.2101.3001.7020) 来安装

**2.2 基本命令**  
脚本解释器  
expect 脚本中首先引入文件，表明使用的事哪一种 shell #！/usr/bin/expect

spawn 启动新的进程（监控，捕捉）

spawn 后面通常跟一 - 个 Linux 执行命令，表示开启一个会话、启动进程，并跟踪后续交互信息 例: spawn passwd root expect 从进程接收字符串

判断上次输出结果中是否包含指定的字符串，如果有则立即返回，否则就等待超时时间后返回; 只能 · 捕捉由 spawn 启动的进程的输出; 用于接收命令执行后的输出，然后和期望的字符串匹配 send 用于向进程发送字符串

向进程发送字符串，用于模拟用户的输入; 该命令不能自动回车换行，一般要加 \ r (回车) 或者 \ n exp_continue 匹配多个字符串在执行动作后加此命令

exp_ continue 类似于控制语句中的 continue 语句。表示允许 expect 继续向下执行指令 expect eof  
表示交互结束，等待执行结束，退回到原用户，与 spawn 对应 比如切换到 root 用户，expect 脚本默认的是等待 10s 当执行完命令后，默认停留 10s 后，自动切回了原用户

**interact 允许用户交互**  
会停留在目标终端而不会退回到原终端，这个时候就可以手工操作了，interact 后的命. 令不起作用; 比如 interact 后添加 exit，并不会退出 root 用户。而如果没有 interact 则登录完成后会退出，而不是留在远程终端上。 使用 interact 会保持在终端而不会退回到原终端; set expect 默认的超时时间是 10 秒，通过 set 命令可以设置会话超时时间，若不限制超时时间则应设置为 - 1 例子： set time out 30 send_users  
表示回显命令与 echo 相同 接收参数

注意：interact 与 expect eof 只能二选一

expect 脚本可以接受从 bash 命令行传递参数，使用 [lindex $argv n] 获得。其中你从 0 开始，分别表示第一个，第二个，第三个… 参数 例子： set hostname [lindex $argv 0] 相当于 hostname=$1 set password [lindex $argv 1] 相当于 passswd=$2 set hostname [lindex $argv 0] 相当于 hostname=$1 set password [lindex $argv 1] 相当于 passswd=$2  
 

### 2.3 实验

![](https://img-blog.csdnimg.cn/73b6283d7b20441882deab8400c347e5.png)

![](https://img-blog.csdnimg.cn/949a646e21504b5a8bed2016cec5ee09.png)