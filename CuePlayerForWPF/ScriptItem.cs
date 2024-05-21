using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CuePlayerForWPF
{
    public class Script
    {
        private MediaElement videoPlayer;
        private MediaElement audioPlayer;
        private TextBlock subtitleText;
        private List<ScriptItem> scriptItems;
        private int currentIndex = -1;

        public Script(MediaElement videoPlayer, MediaElement audioPlayer, TextBlock subtitleText)
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

                new ScriptItem { Subtitle = "逝去的，已然逝去。说起来，那些能一同饮茶的故人，都已经不在了。" },
                new ScriptItem { Subtitle = "但仍在这里，在东方的这片土地上，迎着新一天的日出，告诉新生的孩子们——宅兹中国。我在这里，这里是中国。" },

                new ScriptItem { Subtitle = "『我听见庞贝的乐章，盲诗人的恸哭绝唱』" ,VideoPath = "video3.mp4", AudioPath = "audio2.mp3"},
                new ScriptItem { Subtitle = "『我听见奥尔良的引吭，属于法兰西的光』" ,VideoPath = "video4.mp4"},
                new ScriptItem { Subtitle = "『大荒驼铃东起古城墙』" ,VideoPath = "video5.mp4"},
                new ScriptItem { Subtitle = "『扬帆驶入万海千江』" ,VideoPath = "video6.mp4"},
                new ScriptItem { Subtitle = "『新大陆的征途之上，星条飞扬』", VideoPath = "video7.mp4" },
                new ScriptItem { Subtitle = "『我听见庞贝的乐章，盲诗人的恸哭绝唱』" },
                new ScriptItem { Subtitle = "『我听见庞贝的乐章，盲诗人的恸哭绝唱』" },
                new ScriptItem { Subtitle = "『我听见庞贝的乐章，盲诗人的恸哭绝唱』" },
                new ScriptItem { Subtitle = "『我听见庞贝的乐章，盲诗人的恸哭绝唱』" },
                // 添加更多台词
            };
        }

        public void PreloadMedia()
        {
            // 预加载视频和音频资源
            foreach (var item in scriptItems)
            {
                if (!string.IsNullOrEmpty(item.VideoPath))
                {
                    // 预加载视频
                    PreloadVideo(item.VideoPath);
                }
                if (!string.IsNullOrEmpty(item.AudioPath))
                {
                    // 预加载音频
                    PreloadAudio(item.AudioPath);
                }
            }
        }

        private async void PreloadVideo(string path)
        {
            await Task.Run(() =>
            {
                // 模拟后台预加载视频
                var videoUri = new Uri(path, UriKind.Relative);
                var video = new MediaElement { Source = videoUri, LoadedBehavior = MediaState.Manual };
                video.LoadedBehavior = MediaState.Manual;
                video.UnloadedBehavior = MediaState.Manual;
                video.MediaOpened += (s, e) =>
                {
                    // 视频加载完成
                };
            });
        }

        private async void PreloadAudio(string path)
        {
            await Task.Run(() =>
            {
                // 模拟后台预加载音频
                var audioUri = new Uri(path, UriKind.Relative);
                var audio = new MediaElement { Source = audioUri, LoadedBehavior = MediaState.Manual };
                audio.LoadedBehavior = MediaState.Manual;
                audio.UnloadedBehavior = MediaState.Manual;
                audio.MediaOpened += (s, e) =>
                {
                    // 音频加载完成
                };
            });
        }

        public void NextScript()
        {
            currentIndex++;
            if (currentIndex >= scriptItems.Count) currentIndex = 0;

            var currentItem = scriptItems[currentIndex];
            subtitleText.Text = currentItem.Subtitle;

            if (!string.IsNullOrEmpty(currentItem.VideoPath))
            {
                if(currentItem.VideoPath == "stop") //设置VideoPath = "stop"来停止播放。
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

    public class ScriptItem
    {
        public string Subtitle { get; set; }
        public string VideoPath { get; set; }
        public string AudioPath { get; set; }
    }
}


