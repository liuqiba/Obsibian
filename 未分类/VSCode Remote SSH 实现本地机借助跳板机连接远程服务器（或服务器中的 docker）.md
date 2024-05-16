## 背景

实验室的服务器都只有内网 ip 192.168.1.xx，在家里无法直接访问连接，经过师兄的配置，通过另一台服务器 47.110.xx.yy 可以进行跳板连接 在使用命令行连接到实验室服务器的操作如下：

![](https://pic4.zhimg.com/v2-6cfcc575bed9ec1fe12c4f885678a63f_r.jpg)

如果还希望连接服务器中的 docker container，就还需要 sudo exec 一次，并且还要再输入一次密码，非常头秃，因此想借助 vscode remote ssh 实现两级跳转直接连接到服务器中的 docker

## 预备知识

VSCode Remote SSH 实现本地机连接远程服务器（其中的 docker）的操作就暂且略过，可以参考

[doubleZ：VSCode 连接远程服务器里的 docker 容器](https://zhuanlan.zhihu.com/p/361934730)

或是参考其他使用 vscode 插件连接远程服务器的博客

*   [Docker Ubuntu 上安装 ssh 和连接 ssh_JustToFaith-CSDN 博客](https://link.zhihu.com/?target=https%3A//blog.csdn.net/qq_43914736/article/details/90608587)
*   [VSCode+Docker: 打造最舒适的深度学习环境 - 知乎](https://zhuanlan.zhihu.com/p/80099904)
*   [VSCode 中利用 Remote SSH 插件远程连接服务器并进行远程开发_lenfranky 的博客 - CSDN 博客](https://link.zhihu.com/?target=https%3A//blog.csdn.net/lenfranky/article/details/89972889)

这里主要介绍如何通过中间的跳板机连接远程服务器

## 开始配置

1.  点击 ssh 插件 - 点击右上角的齿轮

![](https://pic1.zhimg.com/v2-9cad6f6f060cd7dfae07576deb41afd0_r.jpg)

2. 可以直接选择第一个默认的 ssh config 文件

![](https://pic4.zhimg.com/v2-cbebd4754cda14a6d809adc1a959187b_r.jpg)

3. 根据自己跳板机和远程服务器的情况进行配置

我的情况是

*   跳板机 ip 为 47.110.xx.yy，已经在其上将本机的 ssh-key 授权（可以免密码登陆），免密码方法可以参考

[doubleZ：SSH 免密码登陆服务器](https://zhuanlan.zhihu.com/p/370422638)

*   服务器 ip 为 192.168.1.xx，docker 容器开放的端口是 6789

其实最根本实现跳板的一句配置是 `ProxyCommand ssh -W %h:%p <SPRINGBOARD_NAME>`，而且要确保这个名字和上面定义的跳板名字一致

```
Host invix_springboard
  HostName 47.110.xx.yy
  Port 6000
  User root
  IdentityFile ~/.ssh/id_rsa

![38D6999E-E733-479D-88AA-9E14E1048603.png](https://p6-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/68e94f8586414fe9bbbc5e9896ebc1c1~tplv-k3u1fbpfcp-watermark.image)
Host server
  HostName 192.168.1.xx
  Port 6789
  User root
  ProxyCommand ssh -W %h:%p invix_springboard
```

## 连接使用

然后就可以直接点击远程服务器的连接按钮 - 输入密码，完成连接

![](https://pic2.zhimg.com/v2-585ff475dddf5415dbc91476fc0a55e9_r.jpg)

## Resources

[vscode 通过跳板机 (堡垒机) 连接 remote 服务器_TheWaySoFar-CSDN 博客](https://link.zhihu.com/?target=https%3A//blog.csdn.net/dcz1994/article/details/103120254)