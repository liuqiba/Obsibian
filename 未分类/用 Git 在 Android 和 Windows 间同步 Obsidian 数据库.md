### **Matrix 首页推荐**

[Matrix](https://sspai.com/matrix) 是少数派的写作社区，我们主张分享真实的产品体验，有实用价值的经验与思考。我们会不定期挑选 Matrix 最优质的文章，展示来自用户的最真实的体验和观点。

文章代表作者个人观点，少数派仅对标题和排版略作修改。

## 前言

如果你使用 [Android 版 Obsidian](https://sspai.com/link?target=https%3A%2F%2Fhelp.obsidian.md%2FObsidian%2FAndroid%2Bapp)，一个必须考虑的问题就是如何与电脑端同步笔记。

对此，诚然可以选择官方的 [Obsidian Sync](https://sspai.com/link?target=https%3A%2F%2Fhelp.obsidian.md%2FLicenses%2B%2526%2Badd-on%2Bservices%2FObsidian%2BSync) 服务，或者使用 Dropbox 等第三方同步工具，但我最推荐的还是**使用 Git 同步**。理由在于：

*   相比其他解决方案，Git 是开源的，不仅免费，而且安全性有保证；
*   作为最先进的分布式版本控制系统（没有之一），用 Git 可以方便地实现版本回溯和协同编辑等功能；
*   Git 支持多种操作系统（Windows、macOS、Linux、Android、iOS），可以按需托管在国内外多家平台上（国外的 [GitHub](https://sspai.com/link?target=https%3A%2F%2Fgithub.com%2F)、[GitLab](https://sspai.com/link?target=https%3A%2F%2Fabout.gitlab.com%2F)，国内的 [Gitee](https://sspai.com/link?target=https%3A%2F%2Fgitee.com%2F)、[CODING](https://sspai.com/link?target=https%3A%2F%2Fcoding.net%2Fproducts%2Frepo)）。

当然，**用 Git 同步的方案也有局限**，包括有一定的学习成本，对纯新手不友好；多设备同时编辑容易产生冲突；以及需要另行手动备份存档等。

但瑕不掩瑜，Git 仍然是我在 Windows 桌面端和 Android 端同步 Obsidian 的首选。本文就为大家介绍我的具体实现方案。

（**注：**行文简洁起见，本文假定读者掌握命令行环境的基本操作，对于 Git 的功能和机制有初步了解。如果你不了解 Git，那么本文食用起来可能会比较困难，不妨先通过[廖雪峰的 Git 教程](https://sspai.com/link?target=https%3A%2F%2Fwww.liaoxuefeng.com%2Fwiki%2F896043488029600)简单学习后再来尝试。）

## Windows 桌面端的准备工作

### 第一步：安装 Git

在开始之前，请确保自己的 Windows 系统中已经安装了 Git。如果还没有安装，可以参考[廖雪峰的教程](https://sspai.com/link?target=https%3A%2F%2Fwww.liaoxuefeng.com%2Fwiki%2F896043488029600%2F896067074338496)。如果你已经安装了，那么可以跳过这一步。

### 第二步：在 Git 托管平台中新建仓库

接下来，我们在自己选用的 Git 托管平台上新建一个笔记专用仓库。

考虑到国内用户的访问便捷程度和中文支持，本文将主要使用国产服务 Gitee 演示（[官网注册链接](https://sspai.com/link?target=https%3A%2F%2Fgitee.com%2F)），并简单说明更为主流的 GitHub 平台的对应操作。

*   **如果使用 Gitee：**在顶部导航条点击「+」>「新建仓库」，根据提示填写信息即可（暂不勾选给出的初始化选项）。
*   **如果使用 GitHub：**在顶部导航条点击「+」>「New Repository」，根据提示填写信息即可（暂不勾选给出的初始化选项）。

![](https://cdn.sspai.com/2021/09/28/61f5b3c159cda3f8f99ec3328995cdeb.png)

### 第三步：生成 SSH 密钥

接着，我们需要生成一个 SSH 密钥（SSH Key），用于在本地和远程 Git 仓库同步时验证身份。

#### 方法一：自动配置

为方便新手用户，我准备了一个配置脚本，按照如下步骤运行即可完成配置：

1.  创建一个空白的 txt（后缀为 `.txt`）文档；
2.  将下面的代码复制粘贴到上面的空白文档中，将其中标注的用户名、邮箱等信息修改为你自己的相应信息，然后保存；
3.  修改文档后缀为 `.sh` 并保存，双击执行脚本。

```
#!/bin/bash 
splitLine="====================================================="
userName="这里改成你的用户名"
email="这里改成你的Email"

# 配置 git 用户名和邮箱
git config --global  user.name ${userName}
git config --global user.email ${email}
git config --global  --list 
echo "Enter 或者 y 键确认"
ssh-keygen -t rsa -C "${email}"

# 打印公钥
echo "复制保存下面的公钥添加到远程仓库"
echo ${splitLine}
cat ~/.ssh/id_rsa.pub
echo ${splitLine}
echo "按任意键退出"
read -n 1
echo "继续运行"
```

**注意：**

1.  如果你本地之前生成过 SSH，那么脚本会提示你是否覆盖，此时输入 `y` 回车即可。
2.  如果操作中途不小心关掉了脚本窗口，不要慌，双击重新执行脚本即可。
3.  执行过程中会出现如下图所示的四次提示确认步骤，简单起见，建议大家**直接回车**不设置参数（当然如果你明白这些步骤的具体含义，也可以设置需要的参数）。

![](https://cdn.sspai.com/2021/09/26/5890d693616616ce19b4e15154a45478.png)

脚本执行完后，**将分割线里面的内容复制出来保存好**。后面会用到。

#### 方法二：手动配置

如果你希望自己动手完成配置过程，下面介绍手动配置方法，这与前述脚本是等效的。

在空白位置点击鼠标右键，选择 **Git Bash**（或者用你习惯的其他终端模拟器，如 Windows Terminal 等访问 Bash 亦可）：

![](https://cdn.sspai.com/2021/09/28/e8dc6683e82f0a0f4a7a6bd1bfdb132b.png)

在弹出的 Bash 窗口中，输入下面的命令**配置用户名**：

```
git config --global  user.name 你的用户名
```

以及用下面的命令**配置邮箱**：

```
git config --global user.email 你的邮箱
```

完成前两步后，可以用下面的命令检查**是否配置成功**：

```
git config --global  --list
```

如下图所示，如果返回的结果正确显示了你输入的用户名和邮箱，说明配置是正确的：

![](https://cdn.sspai.com/2021/09/28/747b64e705ff56751d1ed95e8126b99b.png)

接着，输入下面的命令**生成 SSH Key**。参考上面脚本教程的截图，按四个回车即可。

```
ssh-keygen -t rsa -C "你的邮箱替换中文"
```

最后，用如下命令**显示生成的公钥**，将其**复制出来备用**：

```
cat ~/.ssh/id_rsa.pub
```

### 第四步：上传 SSH 公钥

下面，我们将生成的 SSH 公钥上传至 Git 托管平台。

*   **对于 Gitee：**点击导航栏右上角头像，选择「设置」，然后在侧边栏菜单选择「SSH 公钥」，填入上一步保存的公钥内容并保存确认。
*   **对于 GitHub：**点击导航栏右上角头像，选择「Settings」，然后在侧边栏菜单选择「SSH and GPG keys」，填入上一步保存的公钥内容并保存确认。

![](https://cdn.sspai.com/2021/09/28/cd494a41d331643c00c6d1738bd39d63.png)

### 第五步：配置本地 Obsidian 仓库

下面，我们在本地配置用于同步的 Obsidian 笔记文件夹。

从 Gitee 或 GitHub 网页端访问刚才新建的仓库，由于尚未初始化，网页上会展示下图所示的配置指引：

![](https://cdn.sspai.com/2021/09/26/37bde3db137342b326215438813da7eb.png)

这里，我们按其中的「创建 Git 仓库」一节配置。

具体而言，进入你用作 Obsidian 笔记库的文件夹，进入后在空白处点击右键进入 Bash，依次执行如下命令：

```
touch README.md
git init
git add README.md
git commit -m "first commit"
git branch -M main
git remote add origin "替换为上图中本条命令位置提示的远端地址（形如 git@gitee.com:user/repo.git）"
git push -u origin main
```

### 第六步：配置 Git 仓库同步

完成了本地仓库的配置，接下来要解决的就是在本地和远端之间推送和拉取笔记文件。这里，固然可以定期、手动通过命令行的 `git` 命令来同步，但那未免繁琐。

笔者惯用的方式是使用 [GitKraken](https://sspai.com/link?target=https%3A%2F%2Fwww.gitkraken.com%2F)。这是一款免费、图形界面的 Git 管理工具。

![](https://cdn.sspai.com/2021/09/26/00a7e28f5f7bd06c3422e3b95c2a4331.png)

安装后，先点击左上角的文件夹图标，选择本地 Obsidian 笔记库打开。然后即可通过工具栏按钮快捷地同步，或者在主界面中追踪同步历史等。

![](https://cdn.sspai.com/2021/09/30/03439bea9dbb991a76b04e80a5044cc0.png)

除了用 GitKraken 外，你也可以通过 [Obsidian Git 插件](https://sspai.com/link?target=https%3A%2F%2Fgithub.com%2Fdenolehov%2Fobsidian-git)，直接在 Obsidian 内部完成仓库的同步。

## Android 端的准备工作

Android 端的配置相对简单，除了 Obsidian app，只需要另外安装一个 Git 客户端即可。

[Obsidian](https://play.google.com/store/apps/details?id=md.obsidian)

这里，我推荐使用 MGit，它是笔者能找到的最符合个人需求的 app；其他的如 GitJournal 等 app 多少有些小问题。如果你有更好用的推荐，欢迎在评论区分享。

[MGit](https://play.google.com/store/apps/details?id=com.manichord.mgit)

MGit 的配置步骤如下：

1.  在设置界面的「repos 的根存储位置」拟用于存放 Android 设备上 Obsidian 笔记的路径：
2.  在设置页面，点击「SSH Keys」>「+」，新建 SSH 密钥；
3.  参考前面的方法，将生成的公钥上传到 Gitee 或 GitHub；
4.  从 Gitee 或 GitHub 复制 SSH Remote 地址（即上述 Windows 配置部分第五步用到的地址，也可以在仓库主页面点击「下载 / 克隆」（GitHub 点击「Code」）查看），填入远程地址，点击克隆。

![](https://cdn.sspai.com/2021/09/28/dd5023c1632a7d143571fa07d27314cd.png)

![](https://cdn.sspai.com/2021/09/28/cb40697d52165ac965d48e022712886a.png)

成功之后，打开 Obsidian for Android。一般来说，Obsidian 自动扫描到你克隆到手机的工作空间。如果没有扫描到，那么手动从 Obsidian 进入上述步骤设置的仓库路径，用作工作空间即可。

![](https://cdn.sspai.com/2021/09/26/24f28d81a3353976e0f1829326232094.png)

## 结语

细心的朋友可能已经看出来了，就是 Obsidian 的工作空间在 PC 和手机上都是通用的。这方面要为 Obsidian 的开发团队点赞。

不过，个人不建议在手机端编辑文档。因为手机端屏幕太小影响体验和发挥，只负责查阅即可，编辑还是放到 PC 端比较好。

如果大家对本文方案有什么更好的建议，可以在评论区提出。

**关联阅读：**

*   [或许也是你「入坑」的好机会：Obsidian 移动端上手体验](https://sspai.com/post/67698)
*   [Obsidian 的 Android 伴侣：GitJournal 使用指南](https://sspai.com/post/65316)

> 下载少数派 [客户端](https://sspai.com/page/client) 、关注 [少数派公众号](https://sspai.com/s/J71e) ，了解更妙的数字生活 🍃

> 想申请成为少数派会员？[冲！](https://sspai.com/prime)