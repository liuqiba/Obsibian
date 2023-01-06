### 文章目录

*   *   [1. 修改 DNS](#1DNS_5)
    *   [2.Steam++](#2Steam_15)
    *   [3. 开发者边车（推荐）](#3_31)
    *   [4.FastGithub（Windows 下超推荐）](#4FastGithubWindows_45)

经常访问 github 的小伙伴经常会遇到网站无法访问的情况，而且是一会能访问，一会又不能访问，即使现在能访问，刷新页面就可能无法访问了。

  为了解决这种情况，本人也尝试了许多方法，最终也找到了几种解决方案。

## 1. 修改 DNS

  这也是目前网络上流传的最广的方案，具体做法是修改本地的 hosts 中几个有关几个 github 资源的域名的对于 ip 地址。

  GitHub520 已经列举了所有的域名和对应的 ip 地址，可以直接拿来使用，每天都会自动更新。

  如果嫌每天手动设置很麻烦，还可以下载 SwitchHosts，每天自动更新 GitHub520 中的内容。

  遗憾的是，这种方式似乎只能起到安慰作用，在访问 github 的过程中仍然会遇到无法访问的情况。

## 2.Steam++

  Steam++ 看名字也知道是加速 steam 社区使用的

官方网站：[https://steampp.net/](https://steampp.net/)

Gitee：[https://gitee.com/rmbgame/SteamTools](https://gitee.com/rmbgame/SteamTools)

github：[https://github.com/SteamTools-Team/SteamTools](https://github.com/SteamTools-Team/SteamTools)

Windows10 及以上用户还能直接从微软商店搜索 Steam++ 下载。

  说下使用体验：  
  功能很多，瑞士军刀型软件；但也正是因为功能太多，显得软件很繁杂。  
  在使用的过程中稳定性还可以，基本上能稳定访问到 github，但是也有几次无法正常访问，总是还是比修改 dns 稳定。

## 3. 开发者边车（推荐）

  专门给开发者使用加速代理软件

Gitee：[https://gitee.com/docmirror/dev-sidecar](https://gitee.com/docmirror/dev-sidecar)

github：[https://github.com/docmirror/dev-sidecar](https://github.com/docmirror/dev-sidecar)

  使用体验：  
  使用 electron 制作，界面很漂亮，能加速的东西也很多。  
  在 windows 平台下使用如果在开着软件时关机，会在下一次开机时无法上网，这时只有打开软件才能上网，所以最好设置开机自启动。  
  第一次使用需要设置根证书。  
  开着软件时无法访问微软商店

## 4.FastGithub（Windows 下超推荐）

  这是我目前在使用的访问方案。因为我认为它是一款简洁且专一的软件。

  [https://github.com/dotnetcore/FastGithub](https://github.com/dotnetcore/FastGithub)

  解压文件后可以在当前文件夹使用 fastgithub.exe start（powershell 下使用./fastgithub.exe start）命令以 windows 服务安装并启动。

  这样每天就能在没有察觉的情况下稳定访问 github 了，简直不要太爽。