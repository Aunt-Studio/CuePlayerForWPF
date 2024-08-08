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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;

namespace CuePlayerForWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SysEnvInfo;
        private bool foundError = false;
        private bool readyToPlay = false;
        public string checkHashResult;
        private Scripts scripts;
        private Theater theater;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (readyToPlay)
            {
                theater.Show();
            }
            else
            {
                Init();
            }
            
        }

        /// <summary>
        /// 执行所有检查和初始化，为异步方法。
        /// </summary>
        /// <returns></returns>
        private async Task Init()
        {
            Dispatcher.Invoke(() =>
            {
                StatSysEnv.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD4D4D4"));
                StatResCheck.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD4D4D4"));
                StatTheInit.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD4D4D4"));
                StatScrInit.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD4D4D4"));
            });
            await SysEnvCheck();
            await ResCheck();
            await LoadTheater();
            await LoadScripts();
            if (!foundError)
            {
                Dispatcher.Invoke(() =>
                {
                    InitOrStart.Content = "开始播放";
                    readyToPlay = true;
                });
            }
        }

        private async Task LoadTheater()
        {
            await Task.Run(() => {
                Dispatcher.Invoke(() =>
                {
                    scripts = new Scripts();
                    theater = new Theater(scripts);
                });
                
            });
            Dispatcher.Invoke(() =>
            {
                StatTheInit.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5DFF58"));
            });
        }
        private async Task LoadScripts()
        {
            await Task.Run(() => {
                scripts.Initialize(theater.VideoPlayer, theater.AudioPlayer, theater.SubtitlePlayer);
                scripts.LoadScript();
            });
            Dispatcher.Invoke(() =>
            {
                StatScrInit.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5DFF58"));
            });
        }

        private async Task SysEnvCheck()
        {
            SysEnvInfo = "";
            await Task.Run(() =>
            {
                GetSystemMemory();
                GetCpuModel();
                GetSystemModel();
                GetSystemVersion();
                GetGpuInfo();
            });
            Dispatcher.Invoke(() =>
            {
                StatSysEnv.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5DFF58"));
            });

        }

        private async Task ResCheck()
        {
            checkHashResult = "";
            bool isUnespected = false;
            await Task.Run(() =>
            {
                string programFolderPath = AppDomain.CurrentDomain.BaseDirectory;
                string assetsFolderPath = System.IO.Path.Combine(programFolderPath, "Assets");
                Dictionary<string, string> originalHashes = GetOriginalHashes();

                List<string> modifiedFiles = new List<string>();
                List<string> missingFiles = new List<string>();

                foreach (var filePath in Directory.GetFiles(assetsFolderPath, "*.*", SearchOption.AllDirectories))
                {
                    string relativePath = GetRelativePath(assetsFolderPath, filePath);
                    if (relativePath != "Assets\\hashes.json")
                    {
                        if (originalHashes.ContainsKey(relativePath))
                        {
                            string originalHash = originalHashes[relativePath];
                            if (!CheckHash(filePath, originalHash))
                            {
                                modifiedFiles.Add(relativePath);
                                checkHashResult += "\n!!发现已修改资源: " + relativePath;
                                isUnespected = true;
                            }
                            else
                            {
                                checkHashResult += "\n已检查资源: " + relativePath;
                            }
                        }
                        else
                        {
                            modifiedFiles.Add(relativePath); // 如果文件不在原始哈希列表中，也认为它被修改了
                            checkHashResult += "\n!!发现可能异常资源 (不在原始哈希列表中): " + relativePath;
                            isUnespected = true;
                        }
                    }
                }
                // 查找缺失的文件
                foreach (var originalFile in originalHashes.Keys)
                {
                    string fullPath = programFolderPath + originalFile;
                    if (!File.Exists(fullPath))
                    {
                        missingFiles.Add(originalFile);
                        checkHashResult += "\n!!发现缺失资源: " + originalFile;
                        isUnespected = true;
                    }
                }
                if (isUnespected)
                {
                    foundError = true;

                    Dispatcher.Invoke(() =>
                    {
                        StatResCheck.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF73434"));  //显示为失败，红色。
                    });

                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatResCheck.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5DFF58"));  //显示为成功，绿色。
                    });

                }

            });


        }

        private void StatSysEnv_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Task.Run(() =>
            {
                MessageBox.Show(SysEnvInfo, "系统兼容性检查结果 & 客户机硬件信息", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void GetSystemMemory()
        {
            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

            foreach (ManagementObject item in searcher.Get())
            {
                ulong totalPhysicalMemory = (ulong)item["TotalPhysicalMemory"];
                SysEnvInfo += $"Total Physical Memory: {totalPhysicalMemory} KB";

                double memoryInGB = totalPhysicalMemory / (1024.0 * 1024.0 * 1024.0);
                SysEnvInfo += $"\nTotal Physical Memory: {memoryInGB} GB";
            }

        }

        private void GetCpuModel()
        {

            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_Processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

            foreach (ManagementObject item in searcher.Get())
            {
                SysEnvInfo += $"\nCPU Model: {item["Name"]}";
            }

        }

        private void GetSystemModel()
        {

            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

            foreach (ManagementObject item in searcher.Get())
            {
                SysEnvInfo += $"\nSystem Model: {item["Model"]}";
            }
        }

        private void GetSystemVersion()
        {

            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

            foreach (ManagementObject item in searcher.Get())
            {
                SysEnvInfo += $"\nOperating System: {item["Caption"]} {item["Version"]}";
            }

        }

        private void GetGpuInfo()
        {

            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_VideoController");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

            foreach (ManagementObject item in searcher.Get())
            {
                SysEnvInfo += $"\nGPU Model: {item["Name"]}";
                if (item["AdapterRAM"] != null)
                {
                    try
                    {
                        ulong adapterRam = (ulong)item["AdapterRAM"];
                        double ramInGB = adapterRam / (1024.0 * 1024.0 * 1024.0);
                        SysEnvInfo += $"\nTotal Physical Memory: {ramInGB} GB";
                    }
                    catch (Exception ex)
                    {

                        SysEnvInfo += $"\nGPU Memory: Failed.\n{ex.ToString()}\n\n Maybe Not available.";
                    }

                }
                else
                {
                    SysEnvInfo += "\nGPU Memory: Not available";
                }
            }


        }

        private bool CheckHash(string filePath, string originalHash)
        {
            string computedHash = ComputeHash(filePath);
            return originalHash == computedHash;
        }
        static string ComputeHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        private Dictionary<string, string> GetOriginalHashes()
        {
            string hashFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets\\hashes.json");
            //使用HashGenerator.exe获得哈希原始值
            if (!File.Exists(hashFilePath))
            {
                checkHashResult += "原始哈希字典不存在! 请检查是否已经将Hash校验文件存放在Assets\\hashes.json内。";
                return new Dictionary<string, string>
                {
                    { "", "" }
                };
            }

            Dictionary<string, string> originalHashes = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(hashFilePath));

            return originalHashes;
        }
        private static string GetRelativePath(string basePath, string fullPath)
        {
            Uri baseUri = new Uri(basePath);
            Uri fullUri = new Uri(fullPath);
            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', System.IO.Path.DirectorySeparatorChar));
        }

        private void StatResCheck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Task.Run(() =>
            {
                MessageBox.Show(checkHashResult, "资源哈希校验结果", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void Label_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (MessageBox.Show("确认强制允许播放？\n请确保已经初始化。", "你触发了奇怪的东西！", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                readyToPlay = true;
            }
        }
    }
}
