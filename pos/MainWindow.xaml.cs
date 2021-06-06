using System;
using System.Windows;
using System.Windows.Media.Imaging;
using ZXing;
using System.Windows.Threading;
using System.Windows.Media;
using System.ComponentModel;
using WPFMediaKit.DirectShow.Controls;
using DirectShowLib;
using System.Media;
using System.IO;
using System.Data.SQLite;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Win32;
using pos;

namespace pos
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class MainWindow : Window
    {
        long lastTimestamp = 0;
        DispatcherTimer timer;
        DsDevice[] list = MultimediaUtil.VideoInputDevices;
        private readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        List<product_info> product_lists = new List<product_info>();
        product_count_change count_change;
        product_sale_change sale_change;
        Product_manager product_manager = new Product_manager();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closed;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            if (!File.Exists("product_list.db")) {
                product_manager.createDB();
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.index != -1)
            {
                video.VideoCaptureDevice = list[Properties.Settings.Default.index];
            }
            else
            {
                Properties.Settings.Default.index = 0;
                video.VideoCaptureDevice = list[Properties.Settings.Default.index];
            }
            timer.Start();
        }

        private void Item_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void MainWindow_Closed(object sender, CancelEventArgs e)
        {
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(!timer.IsEnabled)
                timer.Start();
        }

        private long currentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            video.VideoCaptureDevice = null;
            add_goods add = new add_goods(list[Properties.Settings.Default.index]);
            add.IsEnabled = true;
            add.Show();
            add.Closed += Window_Closed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            video.VideoCaptureDevice = list[Properties.Settings.Default.index];
        }

        private void camara_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            video.VideoCaptureDevice = list[Properties.Settings.Default.index];
            Properties.Settings.Default.index = 0;
        }

        private void product_count_change_Click(object sender, RoutedEventArgs e)
        {
            if (product_list.SelectedIndex != -1)
            {
                count_change = new product_count_change(product_lists[product_list.SelectedIndex].product_name, product_lists[product_list.SelectedIndex].product_count);
                count_change.Closing += Count_change_Closing;
                count_change.Show();
            }
            else
            {
                MessageBox.Show("상품을 선택해주세요.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Count_change_Closing(object sender, CancelEventArgs e)
        {
            bool is_change = count_change.is_change;
            if (is_change)
            {
                int i = product_list.SelectedIndex;
                product_lists[i].product_count = Convert.ToInt32(count_change.count.Text);
                product_list.ItemsSource = product_lists;
                product_list.Items.Refresh();
            }
        }

        private void product_remove_Click(object sender, RoutedEventArgs e)
        {
            if(product_list.SelectedIndex != -1)
            {
                product_lists.RemoveAt(product_list.SelectedIndex);
                product_list.ItemsSource = product_lists;
                product_list.Items.Refresh();
            }
            else
            {
                MessageBox.Show("상품을 선택해주세요.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        bool AltKeyPressed = false;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (AltKeyPressed)
            {
                AltKeyPressed = false;
                if (e.SystemKey == Key.C) Console.WriteLine("Alt+S is pressed!");
                if (e.SystemKey == Key.D) Console.WriteLine("Alt+V is pressed!");
                if (e.SystemKey == Key.S) Console.WriteLine("Alt+A is pressed!");

                // Others ...
            }

            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                AltKeyPressed = true;
            }
        }

        private void product_sale_change_Click(object sender, RoutedEventArgs e)
        {
            if (product_list.SelectedIndex != -1)
            {
                sale_change = new product_sale_change(product_lists[product_list.SelectedIndex].product_name, product_lists[product_list.SelectedIndex].sale);
                sale_change.Closing += Sale_change_Closing;
                sale_change.Show();
            }
            else
            {
                MessageBox.Show("상품을 선택해주세요.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Sale_change_Closing(object sender, CancelEventArgs e)
        {
            bool is_change = sale_change.is_change;
            if (is_change)
            {
                int i = product_list.SelectedIndex;
                if(Convert.ToInt32(sale_change.count.Text) != 0)
                {
                    product_lists[i].product_sale = sale_change.count.Text;
                    product_list.ItemsSource = product_lists;
                    product_list.Items.Refresh();
                }
                else
                {
                    product_lists[i].product_sale = sale_change.count.Text;
                    product_list.ItemsSource = product_lists;
                    product_list.Items.Refresh();
                }
            }
        }

        private void Load_DB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "데이터 베이스 파일 (*.db)|*.db";
            dialog.Title = "DB파일 선택...";
            dialog.Multiselect = false;
            dialog.ShowDialog();
            if(dialog.FileName != null)
            {
                if (product_manager.is_correct_DB(dialog.FileName))
                {
                    try
                    {
                        File.Copy(dialog.FileName, "./product_list.db");
                    }
                    catch
                    {
                        MessageBox.Show("DB파일 복사에 실패하였습니다. 권한이 있는지 확인해 주세요", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("DB파일이 올바르지 않습니다. 이 프로그램에서 생성한 DB파일인지 확인해주세요.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Edit_product_Click(object sender, RoutedEventArgs e)
        {
            video.VideoCaptureDevice = null;
            edit_product edit_Product = new edit_product(list[Properties.Settings.Default.index]);
            edit_Product.Closed += Edit_Product_Closed;
            edit_Product.Show();
        }

        private void Edit_Product_Closed(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            video.VideoCaptureDevice = list[Properties.Settings.Default.index];
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            video.VideoCaptureDevice = null;
            setting setting = new setting(list);
            setting.Closed += Setting_Closing;
            setting.Show();
        }

        private void Setting_Closing(object sender, EventArgs e)
        {
            //TODO: get settings value
            System.Threading.Thread.Sleep(500);
            video.VideoCaptureDevice = list[Properties.Settings.Default.index];
        }

        //TODO: 다른창으로 넘어가도 타이머 계속 돌아가는 현상 해결
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)video.ActualWidth, (int)video.ActualHeight, 96, 96,
                    PixelFormats.Default);
                bmp.Render(video);
                BarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(bmp);
                if (result != null)
                {
                    string barcode_num = result.Text;
                    int delay = 1000;

                    if (currentTimeMillis() - lastTimestamp < delay)
                    {
                        return;
                    }
                    SoundPlayer player = new SoundPlayer();
                    player.Stream = Properties.Resources.barcode_beep;
                    player.PlaySync();
                    int i = 0;
                    product_info info = product_manager.GetProductInfo(result.Text);
                    if(info != null)
                    {
                        bool is_has = false;
                        product_lists.ForEach(new Action<product_info>((y) =>
                        {
                            if (result.Text == y.barcode)
                            {
                                try
                                {
                                    is_has = true;
                                    product_lists[i].product_count = ++product_lists[i].product_count;
                                    product_list.ItemsSource = product_lists;
                                    product_list.Items.Refresh();
                                    product_list.SelectedIndex = product_lists.Count - 1;
                                    product_list.ScrollIntoView(product_list.Items[product_list.SelectedIndex]);
                                }
                                catch
                                {
                                }
                            }
                            i++;
                        }));
                        if (!is_has)
                        {
                            info.product_count = 1;
                            product_lists.Add(info);
                            product_list.ItemsSource = product_lists;
                            product_list.Items.Refresh();
                            product_list.SelectedIndex = product_lists.Count - 1;
                            product_list.ScrollIntoView(product_list.Items[product_list.SelectedIndex]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("등록되지 않은 상품입니다.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    player.Dispose();
                    lastTimestamp = currentTimeMillis();
                }
            }
            catch (StackOverflowException)
            {
                MessageBox.Show("가격 계산에 실패 하였습니다.\n총 가격 또는 물건당 가격이 18,446,744,073,709,551,615원이 넘어간것 같습니다.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
