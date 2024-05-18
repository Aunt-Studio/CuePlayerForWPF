using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuePlayerForWPF
{
    public class ScriptItem
    {
        public string Subtitle { get; set; }
        public string VideoPath { get; set; }
        public string AudioPath { get; set; }

        // Constructor
        public ScriptItem(string subtitle, string videoPath = null, string audioPath = null)
        {
            Subtitle = subtitle;
            VideoPath = videoPath;
            AudioPath = audioPath;
        }

        // Method to preload media
        public void PreloadMedia()
        {
            if (!string.IsNullOrEmpty(VideoPath))
            {
                // Code to preload video (if necessary)
            }
            if (!string.IsNullOrEmpty(AudioPath))
            {
                // Code to preload audio (if necessary)
            }
        }
    }
}

}
