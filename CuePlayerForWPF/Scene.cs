using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuePlayerForWPF
{
    public class Scene
    {
        public string Line { get; set; }
        public string VideoPath { get; set; }
        public string AudioPath { get; set; }
    }

    public class SceneManager
    {
        private List<Scene> scenes;
        private int currentIndex = -1;

        public SceneManager()
        {
            // 初始化场景列表
            scenes = new List<Scene>
                {
                    new Scene { Line = "第一句台词", VideoPath = "video1.mp4", AudioPath = "audio1.mp3" },
                    new Scene { Line = "第二句台词", VideoPath = "video2.mp4", AudioPath = "audio2.mp3" },
                };
        }

        public Scene GetNextScene()
        {
            if (currentIndex < scenes.Count - 1)
            {
                currentIndex++;
            }
            return scenes[currentIndex];
        }
    }

}
