# VNFramework

![Logo](./Docs/Img/logo.png)

## Program Architecture

![VNFrameworkStructure](./Docs/Img/VNFramework_Struct.svg)

## Todo List

- [x] 基础的VNScript语法解释器
- [x] 实现VNFramework的基础UI
- [x] ChapterView的章节加载
- [x] 引入QFramework，将框架转换为MVC架构
- [x] 视觉小说资源的AB包加载
- [ ] 窗口多分辨率切换
- [ ] 为 VSCode 编写 VNScript 语法检查插件
- [ ] `game_info` 增加更多自定义属性
- [ ] Dialogue Box 中，对话中的英文单词在行末自动添加换行符
- [ ] 剧情的多分支支持

## Screenshot

由于没有美术，所以当前版本的界面比较丑，请见谅。。。

![screenshot01](./Docs/Img/screenshot01.png)
![screenshot02](./Docs/Img/screenshot02.png)
![screenshot03](./Docs/Img/screenshot03.png)

## VNScript Syntax

### Script Syntax Design

一个好的视觉小说的剧本格式应该达到以下三点要求。

1. 能够让剧作家在阅读剧本时就能想到游戏运行时是什么样子。
2. 剧作家并不是程序员，不能将过多的技术细节暴露给剧作家。
3. 剧本格式应该尽量简单且美观，减少阅读剧本时的视觉负担。

为了达到以上三点要求，剧本格式我采用了标记语言的形式，以下是剧本格式的一段样例。

```
# 播放背景音乐，音乐名称 the_rain_of_night
[ bgm_play: the_rain_of_night ]

# 开始对话
写下这篇序言的时候我在赤道以南的巴厘岛，这是我今年第二次来印度尼西亚。
-> 这边的酒店都会给客人准备一个很宽敞的露台，露台上放一盏烛和一盒火柴，
-> 外面是星垂平野，或者雷电打落在海面上，黑暗那么深邃。

# 播放语音
江南 : (chapter01_001)在寂静的夜里点燃蜡烛放在栏杆上，心就安静下来。
> (chapter01_002)仿佛一种仪式开始，神秘的气息氤氲的降下，可以开始缓缓的讲诉平生。

# 停止播放背景音乐
[ bgm_stop ]
# 设置背景音乐音量，然后播放新的背景音乐
[ bgm_vol: 0.4 ] [ bgm_play: goodbye_black_bird ]
```

#### 对话语句

对话语句有两种格式，一种格式是，一行单独的字符串，用于表现主视角角色的陈述或者内心想法。

另一种格式由两个字符串组成（字符串 : 字符串），常用于对话，第一个字符串是说话角色名字，第二个字符串是该角色说的话。

```
# 这是第一种对话格式
写下这篇序言的时候我在赤道以南的巴厘岛，这是我今年第二次来印度尼西亚。

#这是第二种对话格式
江南 : 在寂静的夜里点燃蜡烛放在栏杆上，心就安静下来。
```

#### 继续输出语句

有的时候我们会希望在文字输出的中间添加一个暂停，等到用户输入一个回车或者是空格之后再继续打印接下来的字符，这样的使用场景下，我们可以使用继续输出的语法。

继续输出语法有两种格式，一种格式是使用符号 ->，它会在用户输入回车之后将新文本输出到旧文本的后面。

另一种是使用符号 > ，它会在用户输出回车之后，另起一个新行输出新文本。

```
# 第一种继续输出方法，将新文本输出到旧文本的后面
写下这篇序言的时候我在赤道以南的巴厘岛，这是我今年第二次来印度尼西亚。
-> 这边的酒店都会给客人准备一个很宽敞的露台，露台上放一盏烛和一盒火柴，
-> 外面是星垂平野，或者雷电打落在海面上，黑暗那么深邃。

# 第二种继续输出方法，另起新行输出新文本
在寂静的夜里点燃蜡烛放在栏杆上，心就安静下来。
> 仿佛一种仪式开始，神秘的气息氤氲的降下，可以开始缓缓的讲诉平生。
```

#### 角色语音语句

如果希望在输出某段对话时同步播放角色语音，可以在对话的开头使用角色语音语法，即使用圆括号将需要调用的语音名称括起来。

```
江南 : (chapter01_001)在寂静的夜里点燃蜡烛放在栏杆上，心就安静下来。

(chapter01_002)仿佛一种仪式开始，神秘的气息氤氲的降下，可以开始缓缓的讲诉平生。
```

#### 剧本中使用Function

在一部视觉小说中，不可避免的需要在剧情的进行途中加载背景图片、加载图片立绘、播放音乐。因此剧本格式应该提供一个简便的语法去实现这些功能。

剧本方法的语法是用中括号将函数的名称包裹起来，如果函数需要使用参数，则在方法名称后添加冒号，在冒号后填写函数的参数，参数与参数之间用逗号隔开。

```
# 不使用参数的函数
[ bgm_stop ]

# 使用一个参数的函数
[ bgm_play : audio_name]

# 使用两个参数的函数
[ role_pic: pos, pic_name]
```

在剧本中可使用的命令可以参照ILScript中列出的命令

常用的命令有以下几种

```
# 显示角色立绘语法
# pos : left / mid / right
# mode : fading / immediate
[ role_pic: pos, pic_name, mode ]

# 播放背景音乐语法
[ bgm: audio_name ]

# 设置音量语法
# 音量大小在 0 ~ 1 之间
# bgm_vol: 背景音乐音量
# bgs_vol: 背景音效音量
# role_vol: 角色语音音量
# 在ILCommand中，role_vol被简写为chs_vol，意为character sound volume
[ bgm_vol: vol_value ]
[ bgs_vol: vol_value ]
[ role_vol: vol_value ]
```

### IL Command

一条VNScript对应一条或多条ILCommand

VNScript中的空白将默认解释为 `[ clear_dialogue ]` 与 `[ clear_name ]` 两条命令。

若空白多行连续，则会忽略多余的空白。

以下是 IL Command 的列表。

| IL Command                                     | Description                                      |
| ---------------------------------------------- | ------------------------------------------------ |
| [ dialogue: dialogue_content ]                 | 在对话框逐字显示文本                             |
| [ role_dialogue: role_name, dialogue_content ] | 在对话框逐字显示文本, 同时在姓名框显示角色的名称 |
| [ dialogue_continue: dialogue_content ]        | 新文本会在当前文本框的文本后追加                 |
| [ dialogue_newline ]                           | 将文本框内的文本进行换行                         |
| [ clear_dialogue ]                             | 清除对话框中所有的内容                           |
| [ name: role_name]                             | 在姓名框显示角色姓名                             |
| [ clear_name ]                                 | 清除名称框中所有的内容                           |
| [ bgp: pic_name, mode]                         | 以指定的方式显示图片到背景图                     |
| [ bgp_hide: mode ]                             | 以指定的方式将背景图片隐藏                       |
| [ role_pic: pos, pic_name , mode ]             | 在指定的位置以指定的方式显示图片到人物立绘       |
| [ role_pic_hide: pos, mode ]                   | 以指定的方式隐藏指定位置的人物立绘               |
| [ role_act: pos, mode ]                        | 以指定的方式来让人物立绘做出特定的动作           |
| [ bgm_play: audio_name ]                       | 播放背景音乐                                     |
| [ bgm_stop ]                                   | 停止播放背景音乐                                 |
| [ bgm_continue ]                               | 播放被暂停的的背景音乐                           |
| [ bgm_vol: audio_vol ]                         | 设置背景音乐音量大小                             |
| [ bgs_play: audio_name]                        | 播放游戏背景音效                                 |
| [ bgs_stop ]                                   | 停止播放游戏背景音效                             |
| [ gms_play: audio_name ]                       | 播放游戏音效                                     |
| [ gms_stop ]                                   | 停止播放背景音效                                 |
| [ role_say: audio_name ]                       | 播放角色语音                                     |
| [ role_vol: audio_vol ]                        | 设置角色语音音量大小                             |

虽然ILScript 中的命令是剧本的最小单元，可它并不是程序执行时的最小单元。程序执行时的最小单元被称为 Asm Command （assembler command）。

ILScript会被转换为Asm Command，程序为每一个 Asm Command 生成一个 Hash table，然后交给 Performance Controller 执行。


### Asm Command

Asm Command 是剧本最小的执行单元，命令的基本格式为 **对象 操作 参数（可选）**

主要的对象主要有以下十一个

- name：角色名称框
- dialogue：对话框
- bgm：背景音乐
- bgs：背景音效
- chs：角色语音
- gms：游戏音效
- gm：游戏管理员
- ch_left：人物立绘_左
- ch_right：人物立绘_右
- ch_mid：人物立绘_中
- bgp：背景图片

IL Command 与 Asm 的对应关系如下图

| IL Command                                     | Asm                                                                                       |
| ---------------------------------------------- | ----------------------------------------------------------------------------------------- |
| [ dialogue: dialogue_content ]                 | name clear<br>dialogue clear<br>dialogue append dialogue_content<br>gm stop               |
| [ role_dialogue: role_name, dialogue_content ] | name clear<br>dialogue clear<br>name append role_name<br>dialogue append dialogue_content |
| [ dialogue_append: dialogue_content ]          | dialogue append dialogue_content                                                          |
| [ dialogue_newline ]                           | dialogue newline                                                                          |
| [ clear_dialogue ]                             | dialogue clear                                                                            |
| [ name: role_name]                             | name append role_name                                                                     |
| [ clear_name ]                                 | name clear                                                                                |
| [ bgp: pic_name, mode]                         | bgp set pic_name mode                                                                     |
| [ bgp_hide: mode ]                             | bgp hide mode                                                                             |
| [ role_pic: pos, pic_name , mode ]             | ch_pos set pic_name mode                                                                  |
| [ role_pic_hide: pos, mode ]                   | ch_pos hide mode                                                                          |
| [ role_act: pos, mode ]                        | ch_pos act mode                                                                           |
| [ bgm_play: audio_name ]                       | bgm play audio_name                                                                       |
| [ bgm_stop ]                                   | bgm stop                                                                                  |
| [ bgm_continue ]                               | bgm continue                                                                              |
| [ bgm_vol: audio_vol ]                         | bgm vol audio_vol                                                                         |
| [ bgs_play: audio_name]                        | bgs play audio_name                                                                       |
| [ bgs_stop ]                                   | bgs stop                                                                                  |
| [ gms_play: audio_name ]                       | gms play audio_name                                                                       |
| [ gms_stop ]                                   | gms stop                                                                                  |
| [ role_say: audio_name ]                       | chs play audio_name                                                                       |
| [ role_vol: audio_vol ]                        | chs vol audio_vol                                                                         |


## Game Config

### file: chapter_info

`chapter_info.txt` 用于声明视觉小说中会用到的剧本的信息，当前版本的 VNFramework 会将`chapter_info` 中的顺序当作是剧本的顺序

```
<|
    [ chapter_name: xxxxxx ]
    [ file_name: xxxxxx ]
    [ resume: xxxxxxxxxx ]
    [ resume_pic: xxxxxx ]
|>

<|
    [ chapter_name: xxxxxx ]
    [ file_name: xxxxxx ]
    [ resume: xxxxxxxxxx ]
    [ resume_pic: xxxxxx ]
|>
```

### file: chapter_record

`chapter_record`用于记录玩家当前已完成的章节

```
[ chapter_name:  chapter_01 ]
[ chapter_name:  chapter_02 ]
[ chapter_name:  chapter_03 ]
```

### file: game_info

`game_info` 用于简化定制视觉小说UI的步骤

```
title: VN Framework
start_view_bgm: 月姬
start_view_bgp: white
```

## License

本项目基于MIT许可证

![MIT](./Docs/Img/MIT_logo.svg)