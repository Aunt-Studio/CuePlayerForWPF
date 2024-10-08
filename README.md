# CuePlayerForWPF

这是一个开源、可自定义、安全完备且易用的舞台背景应用程序。

> [!NOTE]  
> 本分支的自定义需要修改源代码
> 
> 已有计划开发更加通用的分支，开发进度请见[dev 分支](https://github.com/Aunt-Studio/CuePlayerForWPF/tree/dev)

## 特性

✔ 台词展示

✔ 背景音乐播放

✔ 背景视频播放

✔ 轻量主程序

✔ 可自定义

✔ 资源文件Hash检查

✔ 打勾勾

## 快速开始

### 1. Clone 本项目

打开终端，输入git clone指令:

```bash
git clone https://github.com/Aunt-Studio/CuePlayerForWPF.git
```

### 2. 在Visual Studio中打开解决方案

在Microsoft Visual Studio中打开解决方案文件`CuePlayerForWPF.sln`。

> 注意: 请确保你的Microsoft Visual Studio 版本为`10.0.40219.1` 及以上。
>
> 请确保你已经安装 .NET 桌面开发负载以及 [.NET 8.0 SDK](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0)。

### 3. 生成、运行项目

选择`CuePlayerForWPF`项目，按下 <button>F5</button> 以开始调试运行项目。


## 自定义手册

### 1. 完成快速开始步骤

参考 [快速开始](#快速开始)，Clone并生成项目。 

### 2. 检查代码

在解决方案中，你应该能看到两个项目: `CuePlayerForWPF` 和 `HashGenerator`。

`CuePlayerForWPF` 项目为主项目，`HashGenerator` 项目为计算并导出资源文件Hash时所需要的工具。

打开 `CuePlayerForWPF` 项目。你应该能看到以下目录结构: 
```
CuePlayerForWPF
│  App.config
│  App.xaml
│  App.xaml.cs
│  CuePlayerForWPF.csproj
│  MainWindow.xaml
│  MainWindow.xaml.cs
│  Scene.cs
│  ScriptItem.cs
│  Theater.xaml
│  Theater.xaml.cs
│
├─Assets
│      hashes.json
│
└─Properties
        Resources.Designer.cs
        Resources.resx
        Settings.Designer.cs
        Settings.settings
```

请注意[ScriptItem.cs](https://github.com/Aunt-Studio/CuePlayerForWPF/blob/master/CuePlayerForWPF/ScriptItem.cs)文件，这是存放台词、链接媒体文件、台词控制等等操作的类。我们稍后的自定义修改大多都在这个类中。

### 3. 修改台词

[ScriptItem.cs](https://github.com/Aunt-Studio/CuePlayerForWPF/blob/master/CuePlayerForWPF/ScriptItem.cs) 文件中已经提供了示例台词: 

https://github.com/Aunt-Studio/CuePlayerForWPF/blob/4d105ae4da79fe4ae30952a2c51df71b95ad7fc0/CuePlayerForWPF/ScriptItem.cs#L31-L51

你可以仿照示例台词修改和添加台词。例如将第31行至51行的代码替换为:

```csharp
            // 在这里加载台词、视频和音频的对应关系
            scriptItems = new List<ScriptItem>
            {
                //编写台词，如下例子
                new ScriptItem { Subtitle = "" },
                new ScriptItem { Subtitle = "这是我的第一行台词", VideoPath = "第一行台词开始播放的视频.mp4", AudioPath = "第一行台词开始播放的音频.mp3" },
                new ScriptItem { Subtitle = "这是我的第二行台词" }, 
                // 没有视频和音频更换, 延续上一个台词的音频和视频
                new ScriptItem { Subtitle = "第三句台词", VideoPath = "第二行台词切换的视频.mp4"},
                // 更换视频, 但不更换音频, 音频延续最近一次台词设定
                new ScriptItem { Subtitle = "第四句台词", VideoPath = "stop"},
                //停止播放视频, 背景显示为黑色, 台词和音频正常播放
                new ScriptItem { Subtitle = "第五句台词", AudioPath = "stop" },
                //停止播放音频, 台词正常显示
                new ScriptItem { Subtitle = "" },
            };
```

以上几行代码展示了本项目的核心功能。

### 4. 你刚刚做了什么?

让我们来理解上面的代码。

首先，这段代码出现在类 `Scripts` 中的 `LoadScripts()` 方法中。

`LoadScripts()` 方法用于加载台词、视频和音频的对应关系。因此修改这一方法，即为修改和添加台词、媒体资源文件的主要方式。

你应该注意到了，该方法实际上为类 `Scripts` 中的字段 `scriptItems` 赋值了一个 `List<ScriptItem>` 对象。而 `List<ScriptItem>` 列表中的每个对象都是一个 `ScriptItem` 对象。

`ScriptItem` 类包含了三个属性: `Subtitle`, `VideoPath` 和 `AudioPath` :

https://github.com/Aunt-Studio/CuePlayerForWPF/blob/4d105ae4da79fe4ae30952a2c51df71b95ad7fc0/CuePlayerForWPF/ScriptItem.cs#L94-L99

其中

- `Subtitle` 属性用于存放**台词文本**。
- `VideoPath` 属性用于存放**视频文件路径**。
- `AudioPath` 属性用于存放**音频文件路径**。

因此该类将台词文本、视频文件、音频文件三者链接在一起，使得做到切换台词而视频音频同步切换。

### 5. 这是怎么做到的?

观察 [ `Scripts` 类的剩余代码](https://github.com/Aunt-Studio/CuePlayerForWPF/blob/4d105ae4da79fe4ae30952a2c51df71b95ad7fc0/CuePlayerForWPF/ScriptItem.cs#L10-L92) , 你应该注意到了 `NextScript()` 方法，这是你每次按下切换键切换台词时的核心逻辑。

该方法总是选择 `scriptItems` 列表中下一行台词对象 (也就是一个 `ScriptItem` 对象)，并将[初始化时传入的各控件](https://github.com/Aunt-Studio/CuePlayerForWPF/blob/4d105ae4da79fe4ae30952a2c51df71b95ad7fc0/CuePlayerForWPF/ScriptItem.cs#L23-L28)中`subtitleText` Textblock 的 `Text` 属性修改为对应台词，并获得台词对应媒体文件的Uri，并将其传递给 `videoPlayer` 或 `audioPlayer` 进行播放。若某个台词对象的 `VideoPath` 或 `AudioPath` 为 `"stop"` ，则停止对应媒体的播放。

### 6. 查看主窗口MainWindow相关代码

查看主窗口后端代码 ([MainWindow.xaml.cs](https://github.com/Aunt-Studio/CuePlayerForWPF/blob/master/CuePlayerForWPF/MainWindow.xaml.cs))，你应该能看见创建播放器的相关逻辑。

下图展示了程序主要流程逻辑: 

![主要流程图](https://github.com/user-attachments/assets/47b7dbe6-9c46-4061-8b57-aa5d040cad03)

我们将稍后介绍各流程的详细情况。

### 7. 准备媒体资源文件

#### 7.1 确定台词与音视频资源的联系

思考并构造好每一句台词是否应该切换背景和音频，如果需要，应该切换到哪个视频和音频?

> 你可以在你的台本上添加注解来完成这一步骤。

#### 7.2 准备媒体资源文件

根据上一步确定的各媒体资源文件，收集/处理/渲染视频资源文件，收集/切分/合并/制作音频资源文件。

请确保所有媒体资源文件都是WPF中的MediaElement `Source` 属性接受的格式。

> MediaElement支持的具体媒体格式取决于系统上安装的解码器。常见的支持格式包括：视频格式：MP4 (H.264), WMV, AVI, MPEG... ; 音频格式：MP3, WMA, WAV...
>
> 请注意，WPF本身依赖于Windows Media Player的编解码器，因此支持的格式与Windows Media Player支持的格式基本一致。
>
> 若你的媒体文件可能不兼容，请转换为上述可兼容格式。

然后将所有媒体资源文件放入`.\CuePlayerForWPF\Assets\` 目录中，并做好命名。

> 虽然程序对文件的命名没有强制要求，但我推荐您按顺序进行命名，如`Video1.mp4`、`Audio1.mp3`。


#### 7.3 为文件生成Hash 以供校验

在开始播放前，为确保文件完整性，程序自带资源Hash校验功能。并且解决方案中也提供了简易的Hash生成工具。

**7.3.1** 在Microsoft Visual Studio的解决方案管理器中，你应该能看到`HashGenerator`项目。

**7.3.2** 打开该项目。

**7.3.3** 打开项目中的 `Program.cs` 文件。

**7.3.4** 按下<button>Shift</button> + <button>F6</button>生成项目。

![image](https://github.com/user-attachments/assets/ac093a35-ca38-42b0-a070-4b093fc493c3)

**7.3.5** 在解决方案管理器中，右键点击`HashGenerator`项目，选择`在文件资源管理器中打开文件夹`。

**7.3.6** 你应该已经被定位到了`HashGenerator`项目的文件夹。打开`.\bin\Debug\net8.0`文件夹。

**7.3.7** 你应该能看到已经生成的`HashGenerator.exe`文件。

**7.3.8** 在文件资源管理器的地址栏中，输入`cmd`，然后按下 <button>Enter</button>。

**7.3.9** 你应该已经在命令提示符中打开该目录。

**7.3.10** 输入`HashGenerator.exe`，然后按下 <button>Enter</button>，你应该能看见该工具的用法。

![image](https://github.com/user-attachments/assets/98cd0fa5-4744-4b73-bbd2-757a30315f75)

```cmd
HashGenerator <包含所有资源文件的目录> <输出Hash元JSON文件的路径>
```

其中包含所有资源文件的目录即为[第7.2步骤](#72-准备媒体资源文件) 中放置的`.\CuePlayerForWPF\Assets\` 目录。

**7.3.11** 参照上文用法，为资源文件生成Hash元文件: 例如我的Assets目录在`C:\Users\Administrator.WIN-JP5DLIFTFIU\source\repos\CuePlayerForWPF\CuePlayerForWPF\Assets`，那么我应该输入以下命令:

```cmd
HashGenerator C:\Users\Administrator.WIN-JP5DLIFTFIU\source\repos\CuePlayerForWPF\CuePlayerForWPF\Assets C:\Users\Administrator.WIN-JP5DLIFTFIU\source\repos\CuePlayerForWPF\CuePlayerForWPF\Assets\hashes.json
```

程序输出: 
```
Hashes have been successfully saved to C:\Users\Administrator.WIN-JP5DLIFTFIU\source\repos\CuePlayerForWPF\CuePlayerForWPF\Assets\hashes.json
```

程序将对Assets目录内的所有文件进行Hash计算，并将结果保存至`hashes.json`文件中。

> 请确保生成的Hash元文件位于Assets目录中, 且文件名为`hashes.json`。
>
> 生成前您应当确定所有需要使用的资源文件都已经准备好。

**7.3.12** 在Microsoft Visual Studio的解决方案管理器中，展开`CuePlayerForWPF`项目。你应该在项目中的`Assets`文件夹下看到你添加的媒体文件以及`hashes.json`。

**7.3.13** 选择所有媒体资源文件以及`hashes.json`，在`属性`面板中的**复制到输出目录**设置为**始终复制**或者**如果较新则复制**。

![image](https://github.com/user-attachments/assets/f6ffb958-8b26-4092-8e8f-ecb3d128df3a)

**7.3.14** 重新[运行程序](#3-生成运行项目)，点击**开始初始化**按钮。你应该能看到`资源完整性检查`左侧指示灯为绿色。

> 您可以单击`资源完整性检查`左侧指示灯，将会展示完整资源检查信息。
>
> 若指示灯显示为红色，请参照提示中缺失/修改的文件列表，重新生成Hash。
>
> 如果资源完整性检查报告发现缺失资源文件，请确保在第7.3.13步骤中，将所有媒体文件设置为**始终复制**或者**如果较新则复制**。

### 8. 写入台词

参照 [3. 修改台词](#3-修改台词) 的步骤，将你的实际台词与音视频媒体写入列表中。

### 9. 自定义按键

背景播放器默认切换键为<button>Enter</button>键。这一行为被编写在舞台背景播放器窗口的后端代码中:

https://github.com/Aunt-Studio/CuePlayerForWPF/blob/4d105ae4da79fe4ae30952a2c51df71b95ad7fc0/CuePlayerForWPF/Theater.xaml.cs#L32-L35

因此，如果需要修改切换键，你只需要修改`Theater.xaml.cs`文件中的`Window_KeyDown`事件处理函数中的第一个对`e.Key`的判定。

例如，将切换键修改为<button>Space</button>，你只需要将
```cs
if (e.Key == Key.Enter)
```

替换为:

```cs
if (e.Key == Key.Space)
```

### 10. 运行程序
[运行程序](#3-生成运行项目)，点击开始初始化，待所有初始化与检查过程完成后，单击开始播放。你应该能看见舞台背景的效果。

> 在舞台背景播放过程中，按6下 <button>ESC</button> 键可以退出播放。

## 检查流程与初始化

程序在开始播放舞台背景之前，会要求用户检查以下内容: 

- 系统环境兼容性

- 资源文件完整性

以及进行以下对象的初始化:

- 舞台背景播放器窗口 `Theater`

- 台词类 `Scripts`

### 系统环境兼容性

该环节会读取并列举当前系统环境下的系统运行内存、CPU型号信息、操作系统版本、GPU型号信息等。

由于系统性能会很大程度上影响台词与音视频的播放同步性，并且播放视频需要占用不小的内存，因此需要获得以上信息。

由于程序没有进行极端环境测试，因此没有可供参考的基准值。不过用户可以在 `系统环境兼容性` 左侧绿灯亮起后，单击指示灯，阅读并判断上述环节枚举的内容，以判断该系统环境是否能使程序达到预期效果。

### 资源文件完整性

该环节会自动读取 `Assets` 目录下的 `hashes.json` 文件，该文件包含了所有依赖的资源文件哈希值，并将与当前 `Assets` 目录下所有资源文件的运行时计算哈希值进行比对，若存在缺失或修改过的资源文件，则会在 `资源完整性检查` 左侧指示灯为红色。

`hashes.json` 的文件类型为JSON，格式如下:
```json
{
  "Assets\\<1st Resource File Name>": "<Hash Value>",
  "Assets\\<2nd Resource File Name>": "<Hash Value>",
  "Assets\\<3rd Resource File Name>": "<Hash Value>"
  //...
}
``` 

通常情况下 `hashes.json` 可以通过解决方案自带的 `HashGenerator` 项目生成。

使用 `HashGenerator` 项目生成 `hashes.json` 时，请注意:

1. 确保所有资源文件是最终版本，后续不应存在修改。

2. 确保所有资源都已经生成哈希值且包含在 `hashes.json` 文件中。

3. 确保 `hashes.json` 中包含的所有资源都将被拷贝到输出目录。

4. 确保 `hashes.json` 文件在 `Assets` 目录中，且不应嵌套其它文件夹。

5. 生成 `hashes.json` 后，不应再向 `Assets` 目录中的文件作出修改、删除、重命名、添加等操作。应始终保持最终状态。

在 `资源完整性检查` 左侧指示灯亮起为绿灯或红灯后，用户可以单击指示灯，查看缺失/修改的文件列表。

### 台词类初始化

台词类 `Scripts` 包含了台词、视频和音频的对应关系，并且控制台词切换的相关逻辑。该步骤将执行以下内容:

- 台词类 `Scripts` 的构造

- 视频控件 `videoPlayer` 的指定

- 音频控件 `audioPlayer` 的指定

- 台词控件 `subtitleText` 的指定

- 台词列表 `scriptItems` 的初始化

## 开始播放后的切换逻辑

台词类 `Scripts` 包含一个类型为 `List<ScriptItem>` 的字段。该字段以列表的形式包含所有台词文本、音频和视频的关联关系。

每当用户按下切换键，播放器窗口将调用 `Scripts.NextScript()` 方法。`Scripts.NextScript()` 方法首先将控制台词索引的 `currentIndex` 字段自增1，随后判断是否已经到达最后一句台词，如果未到达则更改各控件的相应属性值: 

https://github.com/Aunt-Studio/CuePlayerForWPF/blob/b75004965dc65b13922faa6660179e7ac1f7629b/CuePlayerForWPF/ScriptItem.cs#L55-L91

## 为什么会有这个项目?

起因是由于学校搞的~~奇异搞笑~~戏剧节，班内剧本需要一个背景+台词+音乐的同步播放程序，考虑到PPT特有的元素播放不同步以及不同平台可能造成的差异问题，加上市面上类似项目不是要钱就是复杂，于是随手就写了这个。

结果特喵的程序踩着ddl写完了，背景视频没时间做了，导致最终舞台没用上。

所以整理了一下开源了。

> 其实从这个项目被创建以来，这个仓库一直都是public的 (小声

## 许可证

本项目基于MIT许可证开源，查看仓库内的[License.txt](https://github.com/Aunt-Studio/CuePlayerForWPF/blob/master/LICENSE.txt) 获得更多信息。

有关工具的开源信息请参考Nuget 包管理器。
