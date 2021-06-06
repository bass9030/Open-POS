using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPFMediaKit.DirectShow.Controls;

namespace pos
{
    /// <summary>
    /// camara_settings.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class camara_settings : Page
    {
        DsDevice[] list;
        Process process;
        bool is_setting_change = false;
        bool first_change = true;
        bool is_setting_save = false;
        public camara_settings(DsDevice[] devices)
        {
            InitializeComponent();
            Loaded += Camara_settings_Loaded;
            Unloaded += Camara_settings_Unloaded;
            list = devices;
        }

        private void Camara_settings_Unloaded(object sender, RoutedEventArgs e)
        {
            video.VideoCaptureDevice = null;
            if (is_setting_change && !is_setting_save)
            {
                var result = MessageBox.Show("설정이 변경되었습니다. 적용하시겠습니까?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void Camara_settings_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DsDevice i in list)
            {
                camara_list.Items.Add(i.Name);
            }
            if(Properties.Settings.Default.index != -1)
            {
                camara_list.SelectedIndex = Properties.Settings.Default.index;
            }
        }

        private void camara_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            video.VideoCaptureDevice = list[camara_list.SelectedIndex];
            if (!first_change)
            {
                is_setting_change = true;
                is_setting_save = true;
                confirm.IsEnabled = true;
            }
            else
            {
                first_change = false;
            }
        }

        private void camara_setting_show_Click(object sender, RoutedEventArgs e)
        {
            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C ffmpeg -f dshow -show_video_device_dialog true -i video=\"" + list[Properties.Settings.Default.index].Name + "\"";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            is_setting_save = true;
            confirm.IsEnabled = false;
        }
    }
}
