
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace VideoPlayerOffsetTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Uri media1;


        public MainWindow()
        {
            InitializeComponent();
            videoPlayer.MediaOpened += VideoPlayer_MediaOpened;
            videoPlayer.LoadedBehavior = MediaState.Manual;
            videoPlayer.UnloadedBehavior = MediaState.Manual;
            //Core.Initialize(vlcLibPath);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //videoPlayer.Media = new Media(libVLC, new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test\\2023-09-09 22-53-27.mkv")));

            //videoPlayer.Media = new Media(libVLC, fd);


            videoPlayer.Play();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            videoPlayer.Stop();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            media1 = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test\\2024-02-22 01-19-45.mp4"));
            videoPlayer.Source = media1;

        }
        private void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            log.Text += $"Media[{media1.ToString()}] Loaded.";
        }
    }
}
