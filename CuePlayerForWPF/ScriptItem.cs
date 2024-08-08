using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CuePlayerForWPF
{
    public class Scripts
    {
        private MediaElement videoPlayer;
        private MediaElement audioPlayer;
        private TextBlock subtitleText;
        private List<ScriptItem> scriptItems;
        private int currentIndex = -1;
        private bool endReached = false;

        public Scripts()
        {

        }
        public void Initialize(MediaElement videoPlayer, MediaElement audioPlayer, TextBlock subtitleText)
        {
            this.videoPlayer = videoPlayer;
            this.audioPlayer = audioPlayer;
            this.subtitleText = subtitleText;
        }
        public void LoadScript()
        {
            // 在这里加载台词、视频和音频的对应关系
            scriptItems = new List<ScriptItem>
            {
                new ScriptItem { Subtitle = "" },
                new ScriptItem { Subtitle = "法老的时代终将逝去", VideoPath = "video1.mp4", AudioPath = "audio1.mp3" },
                new ScriptItem { Subtitle = "《摩诃婆罗多》的故事，还会不会有人记得？" }, // 没有视频和音频更换
                new ScriptItem { Subtitle = "不要忘记月神的后裔，不要忘记美索不达米亚文明……"},
                new ScriptItem { Subtitle = "我的朋友，你是否还记得他们？"},

                new ScriptItem { Subtitle = "『阿波罗的诗琴回溯悠长』" , VideoPath = "video2.mp4"},
                new ScriptItem { Subtitle = "『日出之邦的风奏响了争鸣八荒』" },
                new ScriptItem { Subtitle = "『槲寄生的沉语咏叹末世的悲怆』" },
                new ScriptItem { Subtitle = "『东征呼马蹄扬独霸一方』" },

                new ScriptItem { Subtitle = "逝去的，已然逝去。说起来，那些能一同饮茶的故人，都已经不在了。" , VideoPath = "video3.mp4", AudioPath = "stop"},
                new ScriptItem { Subtitle = "但仍在这里，在东方的这片土地上，迎着新一天的日出，告诉新生的孩子们——宅兹中国。我在这里，这里是中国。" },

                new ScriptItem { Subtitle = "『我听见庞贝的乐章，盲诗人的恸哭绝唱』" ,VideoPath = "video4.mp4", AudioPath = "audio2.mp3"},
                new ScriptItem { Subtitle = "『我听见奥尔良的引吭，属于法兰西的光』" ,VideoPath = "video5.mp4"},
                new ScriptItem { Subtitle = "『大荒驼铃东起古城墙』" ,VideoPath = "video6.mp4"},
                new ScriptItem { Subtitle = "『扬帆驶入万海千江』" ,VideoPath = "video7.mp4"},
                new ScriptItem { Subtitle = "『新大陆的征途之上，星条飞扬』", VideoPath = "video8.mp4" },

                new ScriptItem { Subtitle = "时间很快来到近代，清政府故步自封，闭关锁国，错过了轰轰烈烈的工业革命，她却说……" , AudioPath = "stop", VideoPath = "stop"},
                new ScriptItem { Subtitle = "我天朝上国，物产丰富，怎须与你们这些蛮夷相交往……你们算什么东西！", VideoPath = "video9.mp4", AudioPath = "audio3.mp3" },

                new ScriptItem { Subtitle = "Ciao！中国，我是意大利呀！我家的佛罗伦萨现在可漂亮了，听罗马爷爷说，你最喜欢诗歌和绘画了……你要来看看吗", AudioPath = "audio4.mp3"},
                new ScriptItem { Subtitle = "凡事要讲个先来后到啊——Здравствыйте，我是你的邻居，拜占庭双头鹰的后裔，俄罗斯帝国" },
                new ScriptItem { Subtitle = "America——世界的Hero要到你家去，就这样说定了！" },
                new ScriptItem { Subtitle = "德意志也希望能与中国有贸易往来，不知您意下如何？" },
                new ScriptItem { Subtitle = "等等，这种Bonnes choses怎么能少得了我法兰西？" },
                new ScriptItem { Subtitle = "真是抱歉，本人近日身体抱恙，诸位，还是请回吧" },
                new ScriptItem { Subtitle = "哼，不识抬举" },
                new ScriptItem { Subtitle = "那我们来日方长啊" },

                new ScriptItem { Subtitle = "交流？贸易？朋友？老师，您难道真的不知道他们想做什么吗？" },
                new ScriptItem { Subtitle = "……我明白了，那么中国，就此别过吧" },

                new ScriptItem { Subtitle = "怎么了，my dear？是什么事情让你如此黯然神伤？" },
                new ScriptItem { Subtitle = "你又是谁？" },
                new ScriptItem { Subtitle = "我来自日不落的帝国，给你带来了一件——有趣的东西" },
                new ScriptItem { Subtitle = "相信我，你绝对会爱上它的" },
                new ScriptItem { Subtitle = "『翡冷翠的花蕾，重生绽放』" },
                new ScriptItem { Subtitle = "『黑船来航锚声，再塑了明治东洋』" },
                new ScriptItem { Subtitle = "『日耳曼的战车烙上，复仇的悲凉』" },
                new ScriptItem { Subtitle = "『燃乌香破虚妄，惊涛骇浪』" },

                new ScriptItem { Subtitle = "日本！你……为什么？" },
                new ScriptItem { Subtitle = "老师！" },
                new ScriptItem { Subtitle = "我从来都没有把您当做过兄长。失礼します" },
                new ScriptItem { Subtitle = "等等……" },
                new ScriptItem { Subtitle = "先生!" },
                new ScriptItem { Subtitle = "香港!" },

                new ScriptItem { Subtitle = "1840年，英国的炮弹重重地炸开了中国的大门；1890年，中国签订了丧权辱国的《马关条约》……" },
                new ScriptItem { Subtitle = "……中华大地分崩离析、支离破碎，四万万人齐下泪，天涯何处是神州！我们……该怎么办？" },

                new ScriptItem { Subtitle = "逃，快逃啊！那些侵略者毫无人性、嗜血残暴，还能怎么办，快逃啊……" },
                new ScriptItem { Subtitle = "逃，又能逃去哪儿？醒醒吧，我们不能再麻木了，我们得站起来，让世界看看我们的力量，看看中国的力量！" },
                new ScriptItem { Subtitle = "驱除鞑虏，恢复中华，创立民国，平均地权；兼容并包，百家争鸣，我们要民主、要科学！" },
                new ScriptItem { Subtitle = "中国不能失去山东，就像西方不能失去耶路撒冷！" },
                new ScriptItem { Subtitle = "共产主义的光辉，将在这里发扬它的力量！" },
                new ScriptItem { Subtitle = "外争主权，内除国贼！" },
                new ScriptItem { Subtitle = "还我青岛，废除二十一条！" },
                new ScriptItem { Subtitle = "国要亡了，同胞们，起来呀！" },

                new ScriptItem { Subtitle = "『我听见汽笛的嚣响，齿轮轻声咬合碰撞』" },
                new ScriptItem { Subtitle = "『我听见怒号猩红癫狂，推开苏维埃的窗』" },
                new ScriptItem { Subtitle = "『血染塞纳三色覆波旁』" },
                new ScriptItem { Subtitle = "『自由颂往南地北方』" },
                new ScriptItem { Subtitle = "『千年王朝终成过往，民生嘹亮』" },

                new ScriptItem { Subtitle = "都安静！我要Vorwärts Marsch，要成为第一强大的国家，就算是要掠夺、是要无止境地争斗！你们要为曾经的所作所为付出千百倍的代价！" },
                new ScriptItem { Subtitle = "德意志，那样不行的！你会消失的，就像罗马爷爷那样……！" },
                new ScriptItem { Subtitle = "莫斯科，可不是你想来就来、想走就走的地方!" },

                new ScriptItem { Subtitle = "豺狼虎豹！就算流尽最后一滴血，也不会让你踏进我家一步！" },
                new ScriptItem { Subtitle = "既然如此，休怪我手下不留情。" },
                new ScriptItem { Subtitle = "是吗？那看来不得不由英雄出场了。" },
                new ScriptItem { Subtitle = "休战吧，法兰西。现在可不是纠结私人恩怨的时候。" },
                new ScriptItem { Subtitle = "居然能在是非面前这么识大体，还真是不容易啊，不列颠。" },
                
                new ScriptItem { Subtitle = "『青天下红旗冉冉心之所往』" },
                new ScriptItem { Subtitle = "『炮嚣鸣梦魇天降局势激荡』" },
                new ScriptItem { Subtitle = "『冬将军逆转天平的倾向』" },
                new ScriptItem { Subtitle = "『势如破竹霸王行动拓战场』" },

                new ScriptItem { Subtitle = "1939年，二战爆发，战火蔓延，生灵涂炭。世界之大，竟没有一个何平之处！战争要何时才能停止，和平要什么时候才能到来呀！" },
                new ScriptItem { Subtitle = "英格兰的人民们！是时候反击了！将那些侵略者，一个不留地清除出去！" },
                new ScriptItem { Subtitle = "我们将在海滩上战斗，我们将在敌人的登陆地点战斗，我们将在田野和街道间战斗，我们将在山丘上战斗，我们绝不投降！" },
                new ScriptItem { Subtitle = "Товарищ，莫斯科的冬天好冷，我想回家……" },
                new ScriptItem { Subtitle = "我的孩子，我们必须坚守下去，因为我们的身后就是莫斯科。" },
                new ScriptItem { Subtitle = "我们不会放弃，我们仍在这里，天佑法兰西！" },
                new ScriptItem { Subtitle = "对于你们这群家伙，美国人民可不会袖手旁观！" },
                new ScriptItem { Subtitle = "我是美利坚合众国——" },
                new ScriptItem { Subtitle = "We got the job done！" },
                new ScriptItem { Subtitle = "『我听见败落的彷惶，落笔终了史书一行』" },
                new ScriptItem { Subtitle = "『我听见白鸽啼自远方，泪雨欢歌洒沿巷』" },
                new ScriptItem { Subtitle = "『铁幕一语沙盘乾坤荡』" },
                new ScriptItem { Subtitle = "『破碎之身归入星芒』" },
                new ScriptItem { Subtitle = "『巨龙乘于五星翱翔，震天雄邦』" },

                new ScriptItem { Subtitle = "" },
                // 添加更多台词
            };
        }


        public void NextScript()
        {
            currentIndex++;
            if (currentIndex >= scriptItems.Count)
            {
                endReached = true;
            }

            if (!endReached)
            {
                var currentItem = scriptItems[currentIndex];
                subtitleText.Text = currentItem.Subtitle;

                if (!string.IsNullOrEmpty(currentItem.VideoPath))
                {
                    if (currentItem.VideoPath == "stop") //设置VideoPath = "stop"来停止播放。
                    {
                        videoPlayer.Stop();
                        return;
                    }
                    videoPlayer.Source = new Uri(currentItem.VideoPath, UriKind.Relative);
                    videoPlayer.Play();
                }

                if (!string.IsNullOrEmpty(currentItem.AudioPath))
                {
                    if (currentItem.AudioPath == "stop") //设置AudioPath = "stop"来停止播放。
                    {
                        audioPlayer.Stop();
                        return;
                    }
                    audioPlayer.Source = new Uri(currentItem.AudioPath, UriKind.Relative);
                    audioPlayer.Play();
                }
            }

        }
    }

    public class ScriptItem
    {
        public string Subtitle { get; set; }
        public string VideoPath { get; set; }
        public string AudioPath { get; set; }
    }
}


