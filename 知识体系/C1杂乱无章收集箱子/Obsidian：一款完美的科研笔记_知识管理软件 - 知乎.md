**20210218更新**

意识到自己之前对于mdnotes认识存在误差，重新学习并制作了模板。

**20210216更新**

在Obsidian论坛上看见两个对完善工作流有用的帖子

1、[argentum - mdnotes开发者](https://link.zhihu.com/?target=https%3A//forum.obsidian.md/t/zotero-best-practices/164/57)：之前对于mdnotes的使用方法我还存在误解，根据argentum自述的工作流可以解决之前笔记无法跳转到PDF文件中批注的痛点，使得笔记可以反查PDF对应内容。通过mdnotes可以实现Marginnote3或是Highlights的跳转功能。

就我目前的使用经验来看，Zotfile的【Extracted Annotations】对于知网中文文献支持不够好，但在台湾华艺下载的文献就没有这样的问题，应该是知网自身PDF文件的问题。

简述操作方法：利用Zotfile的【Extracted Annotations】生成注释→利用mdnotes的【batch export to markdown】将注释转化为markdown文件

如果按照之前设置的默认输出位置，【batch export to markdown】会在Obsidian的库里生成提取的【注释】、Zotero对应条目的【Metadata】以及【自己的笔记】三个markdown文件。

之前我们设置的【模板】将需要的【Metadata】融进了【自己的笔记】中，因此上述操作会造成两个markdown文件部分重复。目前暂时无法自动解决。

2、[silent - a markdown link to your Zotero Item](https://link.zhihu.com/?target=https%3A//forum.obsidian.md/t/zotero-best-practices/164/72)：可以【导出】或者设置【快捷引用】（快捷键 Command+Shift+C）复制markdown格式选中的Zotero条目的URL。

简述操作方式：下载js文件→拖进Zotero/translators→打开【首选项】进入【导出】→默认格式设置为【Markdown item with URI】（开发者提供了两种，我个人觉得带citekey的更好）→选中条目导出或者快捷引用。

这样可以摆脱mdnotes，实现了任意第三方软件与Zotero联动。通过这个js实现第三方插件跳转到Zotero，通过复制第三方插件的URL实现Zotero跳转到第三方插件。（在我的常用软件中比如Marginnote3就可以实现这个）

---

> 本文旨在介绍Obsidian以及它与其他软件联用，对于Obsidian的基础入门仅给出学习视频不做过多介绍。

## [](https://note.youdao.com/md/#需求我需要一款怎样的笔记软件)需求：我需要一款怎样的笔记软件？

2020年可以说是笔记软件**新元年**，由RoamResearch领头在笔记软件行业内掀起了一场【**双向链接**】革命。双向链接可以视作升级后的Tag，它打破了传统超链接的单一方向跳转，可以让笔记形成网络关系图谱，让笔记之间的脉络关系一目了然。

![](https://pic2.zhimg.com/v2-87da59ccd7318222154e11263ac4ca7d_b.jpg?ynotemdtimestamp=1669885007004)

Obsidian官网展示的demo

在2020年的末尾，国内大量双向链接笔记软件上线，包括但不限于RoamEdit、wolai、葫芦笔记等。这也宣布传统老牌笔记软件如印象笔记（Evernote）、OneNote等笔记管理理念已经落伍了。

当然，回顾双向链接笔记软件的兴起与发展史，双向链接以及一定会与之一起被谈及的【**Zettelkasten**】卡片盒笔记法更像是宣传所需做出的话术。双链的滥用污染了网络关系图谱、我们是否真正需要Roam精度的【**块引用**】、网页在线跨平台与本地同步的优劣等问题仍然需要我们去反思。

双向链接、网页在线跨平台、块引用是否需要？是否对于我们自己需要？

对于笔记软件，我一向反对Notion【**All-In-One**】的口号，在有网络关系图谱的软件中科研与生活内容混在一起会污染图谱，并且过分All-In-One反而会造成困扰。

少数派上有一篇文章《 **href="[https://sspai.com/post/64122](https://link.zhihu.com/?target=https%3A//sspai.com/post/64122)">在 Notion 中创建自己的_记账_应用**》就是个很好的例子。用Notion记账需要的维护和管理费时费力，不如选一个记账App随手记，如果对于自己的数据安全不介意的话大可选择能够监控支付记录的记账App更加省时省力。

根据我不同情境的不同需求，我可以选择不同的笔记软件。比如目前如果我要输出内容（除了微信公众号和知乎两大平台）专栏我更倾向于【语雀】，【Notion】我认为更多适合作为Blog使用；如果我要剪藏内容（虽然我没有剪藏的习惯），那我会选择【Webclipper】插件支持的笔记软件，再整理后归入【Obsidian】中。

关于生活或是输出类的笔记内容对我来说基本上无关紧要，只要满足美观方便都是好软件。但对科研笔记来说，笔记软件必须满足**价格低**（最好免费）、**安全**和**便于迁移**，那么【**Obsidian**】成为了最佳选择。

---

## [](https://note.youdao.com/md/#优点我为什么会选择obsidian)优点：我为什么会选择Obsidian？

> Obsidian is a powerful knowledge base that works on top of  
> a local folder of plain text Markdown files.

Obsidian是基于**Markdown文件**的**本地**知识管理软件，并且开发者承诺Obsidian对于个人使用者**永久免费**。

![](https://pic1.zhimg.com/v2-9ff4e3f35ca4c9ea12f7a906887dc314_b.jpg?ynotemdtimestamp=1669885007004)

Obsidian价格

从目前来看，Obsidian仅对【**发布**】和【**同步**】功能额外收费，8+8 + 4每月的价格可以说是非常良心了，而且还有50%的早鸟优惠。况且如果仅将Obsidian作为本地科研笔记知识管理软件，根本不需要这些额外功能。而且【发布】可以用其他软件替代，【同步】可以用git实现。

Obsidian本地储存的特性不仅使得【**信息安全**】得到一定保障而且能够更好地和其他本地软件联动。在【**3.联动**】一节中我会重点介绍Obsidian与Zotero的联动。

Markdown文件体积小、语法简单、便于迁移。相较于其他双向链接软件迁移后整个双向链接、网络关系崩溃，Obsidian是通过Markdown文件中的语法进行关联的，只需要复制粘贴Obsidian选中的库就可以轻松实现完整地迁移。（虽然配置和插件需要重新设置）

但Markdown文件组织类型目前在层级关系方面明显大大**逊色**于RoamResearch、RoamEdit这类基于大纲结构化组织的笔记软件。Obsidian中的大纲只是目录，Markdown只是一种语法，真正大纲结构化组织的笔记软件可以加载Markdown语法，而Obsidian无法实现结构化的大纲。在设计理念上大纲结构化组织类型是胜过Markdown组织类型的。

失之东隅，收之桑榆？

回到上文提及的【**迁移**】，落后的层级关系让Obsidian的迁移变得便利，并且目前本地端笔记软件中Obsidian算是一枝独秀，我认为目前只有Remnote可以一较高下甚至很有希望超越Obsidian。Remnote虽然PDF内部与笔记跳转功能非常领先，但【PDF Uploads & Highlights】功能需要购买**Pro版**（6月或 6月或 300买断）才能实现。Obsidian个人版永久免费，并且在2021年1月13日发布的v0.10.8版已经实现了软件内部对于PDF文件的调用以及特定页面的跳转。

个人感觉Obsidian的社区生态良好，开发了许多实用的第三方插件。目前Obsidian已经非常稳定，相较于其他各种【**未来可期**】，不如先下载Obsidian用起来吧！

**注**：【Typora】仅是Markdown文本编辑器，不具有知识管理的功能。

## [](https://note.youdao.com/md/#入门帮助文档以及视频)入门：帮助文档以及视频

关于Obsidian的入门我不做多过赘述，Obsidian界面语言可选择【**中文**】并且2021年1月21日Obsidian发布的v0.10.9版本实现了软件内帮助文档【**Obsidian Help**】的汉化，如果对于Markdown语法有一定了解基本上只需要几小时就可以轻松上手。

对于Obsidian的功能展示以及简单的入门介绍我推荐以下两个视频：

**注**：视频中的Obsidian版本比较老，新版本已经更新了许多功能。

## [](https://note.youdao.com/md/#联用与zotero及其他)联用：与Zotero及其他

### [](https://note.youdao.com/md/#文献笔记与zotero联用)**文献笔记：与Zotero联用**

这里放上一个双向联动使用的**demo**。

实现Obsidian与Zotero的联动需要安装Zotero的[MDnotes](https://link.zhihu.com/?target=https%3A//github.com/argenos/zotero-mdnotes)插件，Mdnotes的开发者强烈建议同时安装[Zotfile](https://link.zhihu.com/?target=https%3A//github.com/jlegewie/zotfile)和[Better BibTeX](https://link.zhihu.com/?target=https%3A//github.com/retorquere/zotero-better-bibtex)两个插件保证测试环境一致。

Zotfile插件可以帮助管理附件，详情见我的专栏2.2.2.1一节：

Better BibTeX则可以管理参考文献数据，实现用Zotero管理数据并在LaTeX或是 Markdown中插入Zotero中的数据。用在Markdown插入参考文献我后面会提及，如何在LaTeX中实现可以参考这篇文章：

回到Mdnotes，Mdnotes如何配置以及操作可以参考这篇文章：

![](https://pic1.zhimg.com/v2-6f3fdd70f3834dd7fcbaa63f359b94c4_b.jpg?ynotemdtimestamp=1669885007004)

-   `Use the items citekey as title?`：勾选后将citekey作为Markdown笔记的标题，citekey需要通过Better BibTeX生成以及修改命名格式。
-   `File organization`：选择`Split files`保持清爽。
-   `Internal Links`：选择`Wiki style`实现与其他软件联动。
-   `Export directory`：选择Obsidian库中的文件夹。
-   `Attach file links to Zotero`：勾选后将添加Markdown笔记的链接至Zotero条目下，可以用Quicklook功能快速预览，建议勾选。

![](https://pic3.zhimg.com/v2-1acb424c6480ce1980eb60084d1bf8ee_b.jpg?ynotemdtimestamp=1669885007004)

-   `Template folder`：模板，必须选择命名为`Mdnotes Default Template`的Markdown文件。我自己写了一个模板可以实现在Obsidian中调用Zotero里存储的本地文件，同样附在文末。

如果需要自定义模板，可以参考[Zotero mdnotes](https://link.zhihu.com/?target=https%3A//argentinaos.com/zotero-mdnotes/docs/placeholders/%23item-placeholders)里的占位符。

为了实现可以在Obsidian中调用Zotero中的PDF附件，需要将Obsidian的库设置为Zotero的根目录。

Zotero—首选项—高级—文件和文件夹—数据存储位置—打开数据文件夹即可抵达Zotero根目录。

![](https://pic3.zhimg.com/v2-cc8b12de630683c17e4db3b7443cd7d6_b.jpg?ynotemdtimestamp=1669885007004)

像我的数据存储位置在`/Users/a40781/Zotero`中，同样将Obsidian库的位置同样设置为这个`Zotero`文件夹。

![](https://pic3.zhimg.com/v2-2fb2ef7100ff05449f8807ac5143090a_b.jpg?ynotemdtimestamp=1669885007004)

之后在`/Users/a40781/Zotero`下新建一个`Obsidian`文件夹储存Markdown文件。

![](https://pic1.zhimg.com/v2-5260aa27f8b0ef7f41757c1a30b99660_b.jpg?ynotemdtimestamp=1669885007004)

这样设置的好处是Obsidian可以调用库文件夹下的文件，使用`[[]]`调用PDF文件避免在写文献笔记的时候再切换阅读软件查找原文内容。

![](https://pic4.zhimg.com/v2-f70df132f02a7feaf9b08b7976644657_b.jpg?ynotemdtimestamp=1669885007004)

v0.10.8版本Obsidian通过添加`#page=number`实现了调用指定页的PDF文件，如`[[My file.pdf#page=3]]`。

---

配置结束后，我们来复现Zotero与Obsidian双向互动的工作流。

以`余敏, 朱江, 丁照蕾. 参考文献管理工具研究[J]. 现代情报, 2009, 29(02): 94-98+93.`这个条目为例。

![](https://pic2.zhimg.com/v2-acec13e4ad38bebfe606335d5e440109_b.jpg?ynotemdtimestamp=1669885007004)

右键条目，点击`Create Mdnotes File`，选定存储位置。我的输出位置是`/Users/a40781/Zotero/Obsidian/2021-01`。

![](https://pic4.zhimg.com/v2-092a15253dd68c03712be7a691697c63_b.jpg?ynotemdtimestamp=1669885007004)

此时，Zotero条目下出现链接附件。

![](https://pic4.zhimg.com/v2-249521cf98c39fb20ac8fe0afaa2c913_b.jpg?ynotemdtimestamp=1669885007004)

Obsidian对应文件夹下出现了笔记。

![](https://pic3.zhimg.com/v2-afc210a9c3e1a283077463b6da818cf2_b.jpg?ynotemdtimestamp=1669885007004)

如果要实现在Obsidian中调用PDF文件，则需要在这里填写PDF文件名。如我这里需要填写`余敏 等。 - 2009 - 参考文献管理工具研究.pdf`，Obsidian有智能填充功能只需要输入几个字就可以直接选择库中的附件了。

![](https://pic2.zhimg.com/v2-11b307925ef3d0571a99e7f2e39512b1_b.jpg?ynotemdtimestamp=1669885007004)

切换到预览模式（快捷键`Command+E`）或者在右侧打开一个新的预览面板，然后就可以实现Obsidian内部调用PDF功能。

![](https://pic4.zhimg.com/v2-faa1f71e3585329975da93f6f78fa24f_b.jpg?ynotemdtimestamp=1669885007004)

接着设置快捷键【复制obsidian url】为`Command+D`。

![](https://pic3.zhimg.com/v2-41e4f17b29c9653c0c55e2fb284bb8ba_b.jpg?ynotemdtimestamp=1669885007004)

使用快捷键复制obsidian url，右上角会弹出提示【Url已复制到你的剪切板】。

![](https://pic2.zhimg.com/v2-de2c45d1984cea16ebdfc66750b82821_b.jpg?ynotemdtimestamp=1669885007004)

点击【Local Library】这个链接即可跳转到Zotero对应条目下。

![](https://pic2.zhimg.com/v2-2dbee5da34c0bbfdcefd4125caaa664d_b.jpg?ynotemdtimestamp=1669885007004)

点击【附加URL的链接】，将刚刚复制的obsidian url粘贴进去，命名为【ob】。

![](https://pic3.zhimg.com/v2-4c3ea0de8f70d1c6504d7c73f84e215e_b.jpg?ynotemdtimestamp=1669885007004)

这里可以用Zotero中的[Zutilo](https://link.zhihu.com/?target=https%3A//github.com/wshanks/Zutilo)插件，实现快速添加url链接附件。具体操作不再赘述。

![](https://pic2.zhimg.com/v2-666d55213df0a3c9e536d350116934dd_b.jpg?ynotemdtimestamp=1669885007004)

此时条目下出现命名为ob的链接附件，点击即可跳转到Obsidian对应文献笔记中。（需要选择Obsidian为默认打开方式）

![](https://pic3.zhimg.com/v2-1f65d4a8416af7c1c7318d353474f78e_b.jpg?ynotemdtimestamp=1669885007004)

---

### [](https://note.youdao.com/md/#储存图片与ipic或其他图床联用)**储存图片：与IPic或其他图床联用**

Markdown文件对于保存图片不友好，建议使用图床。对我来说[IPic](https://link.zhihu.com/?target=https%3A//toolinbox.net/iPic/)的基础图床已经够用了，具体使用可以参考官网教程。

### [](https://note.youdao.com/md/#制作slides与deckset联用)**制作Slides：与Deckset联用**

详情参考我上篇专栏： [Calliope - Deckset：实现Markdown到幻灯片的无缝转换](https://zhuanlan.zhihu.com/p/347707503)

### [](https://note.youdao.com/md/#制作公众号和专栏与mdnice联用)**制作公众号和专栏：与Mdnice联用**

[Mdnice](https://link.zhihu.com/?target=https%3A//www.mdnice.com/)简单好用，直接可以看官网教程在实践中上手。

### [](https://note.youdao.com/md/#用markdown写论文与docdown联用)**用Markdown写论文：与Docdown联用**

要实现这个功能还需要下载Pandoc等工具，具体操作可以参考这个视频：

经过我自己实践，我发现Zotpick无法在Obsidian中使用（M1, macos Big Sur 11.2），在Zettlr和Typora可以使用。

直接使用[Docdown](https://link.zhihu.com/?target=https%3A//github.com/lowercasename/docdown)，手动复制粘贴citekey，同样可以实现输出参考文献的效果。

---

个人建议**不要局限**于Obsidian一个软件，All-In-One的理念限制了将自己的使用体验最佳化。

Obsidian无法所见即所得，那么就用Typora作为【默认打开应用】（我设置的快捷键`Command+T`）进行编辑。

![](https://pic4.zhimg.com/v2-a246d5ced1492839c15cba2a8a7549b7_b.jpg?ynotemdtimestamp=1669885007004)

Obsidian无法用Zotpick，那么要输出参考文献的时候换上Zettlr进行再处理。

我将Obsidian定为作科研笔记知识管理软件，在笔记软件中我同样将Obsidian视作为和Zotero一样的管理中枢，集中处理、集中保存，最后根据输出需要选择其他软件。

以上涉及的文件：

---

如果对我感兴趣的话，可以扫描二维码关注我的**公众号**“一圆堂”。