using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZXing;

namespace pos
{
    /// <summary>
    /// edit_product.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class edit_product : Window
    {
        List<product_info> product_Infos = new List<product_info>();
        Product_manager product_manager = new Product_manager();
        long lastTimestamp = 0;
        DispatcherTimer timer;
        DsDevice webcam;
        private readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        public edit_product(DsDevice dev)
        {
            InitializeComponent();
            Loaded += Edit_product_Loaded;
            webcam = dev;
        }

        private void Edit_product_Loaded(object sender, RoutedEventArgs e)
        {
            product_Infos = product_manager.GetAllProductInfo();
            product_list.ItemsSource = product_Infos;
            product_list.Items.Refresh();
            video.VideoCaptureDevice = webcam;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private long currentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            product_info info = new product_info()
            {
                barcode = barcode.Text,
                product_name = goods_name.Text,
                product_count = Convert.ToInt32(goods_count.Text),
                product_money = goods_price.Text,
                product_sale = goods_sale.Text
            };
            int index = product_Infos.FindIndex((a) => a.barcode == barcode.Text);
            product_manager.EditProductInfo(product_Infos[product_list.SelectedIndex].barcode, info);
            product_Infos = product_manager.GetAllProductInfo();
            product_list.ItemsSource = product_Infos;
            product_list.Items.Refresh();
            product_list.SelectedIndex = index;
            product_list.ScrollIntoView(product_list.Items[index]);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            product_info info = new product_info()
            {
                barcode = barcode.Text,
                product_name = goods_name.Text,
                product_count = Convert.ToInt32(goods_count.Text),
                product_money = goods_price.Text,
                product_sale = goods_sale.Text
            };
            product_manager.EditProductInfo(product_Infos[product_list.SelectedIndex].barcode, info);
            Close();
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void barcode_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void barcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void product_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (product_list.SelectedIndex == -1)
                return;
            product_list.ScrollIntoView(product_list.Items[product_list.SelectedIndex]);
            product_info infos = product_Infos[product_list.SelectedIndex];
            barcode.Text = infos.barcode;
            goods_name.Text = infos.product_name;
            goods_count.Text = infos.product_count.ToString();
            goods_price.Text = infos.money.ToString();
            goods_sale.Text = infos.sale.ToString();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)video.ActualWidth, (int)video.ActualHeight, 96, 96,
            PixelFormats.Default);
            bmp.Render(video);
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bmp);
            if (result != null)
            {
                int delay = 1000;

                if (currentTimeMillis() - lastTimestamp < delay)
                {
                    return;
                }
                //TODO: 다시 스캔을 누를시 카메라가 정상복구 되기전에 인식되는 문제 해결(너무 빨리 인식하는 문제 해결)
                //아마도 고침
                SoundPlayer player = new SoundPlayer();
                player.Stream = Properties.Resources.barcode_beep;
                player.PlaySync();
                //video.VideoCaptureDevice = null;
                product_info info = product_manager.GetProductInfo(result.Text);
                if (info != null)
                {
                    int index = product_Infos.FindIndex((a) => a.barcode == result.Text);
                    product_list.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show("등록된 상품이 없습니다.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                player.Dispose();
                lastTimestamp = currentTimeMillis();
                //timer.Stop();
            }
        }
    }
}
