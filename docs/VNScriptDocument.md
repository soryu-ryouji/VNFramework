
# Intermediate Language

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


### Assembly Command

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

