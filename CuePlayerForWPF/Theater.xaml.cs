using System;
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
using System.Windows.Shapes;

namespace CuePlayerForWPF
{
    /// <summary>
    /// Theater.xaml 的交互逻辑
    /// </summary>
    public partial class Theater : Window
    {
        private Scripts scripts;
        private int escapeTimes = 0;
        public Theater(Scripts scripts)
        {
            InitializeComponent();
            this.scripts = scripts;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                scripts.NextScript();
            } else if(e.Key == Key.Escape){
                escapeTimes++;
                if(escapeTimes == 5)
                {
                    esc.Visibility = Visibility.Visible;
                }
                else if (escapeTimes > 5)
                {
                    Close();
                }
                
            }
        }
    }
}
