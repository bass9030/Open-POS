using DirectShowLib;
using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZXing;

namespace pos
{
    /// <summary>
    /// add_goods.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class add_goods : Window
    {
        private readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        long lastTimestamp = 0;
        DispatcherTimer timer;
        DsDevice webcam;
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        Product_manager product_manager = new Product_manager();

        public add_goods(DsDevice Device)
        {
            InitializeComponent();
            video.VideoCaptureDevice = Device;
            webcam = Device;
            Loaded += Add_goods_Loaded;
            Closing += Add_goods_Closing;
        }

        private void Add_goods_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private long currentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private void Add_goods_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
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
                video.VideoCaptureDevice = null;
                if (product_manager.GetProductInfo(result.Text) != null)
                {
                    var msg_result = MessageBox.Show("이미 등록된 상품입니다.\n상품 정보를 수정하시겠습니까?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (msg_result == MessageBoxResult.Yes)
                    {
                        product_manager.RemoveProductInfo(result.Text);
                        barcode.Text = result.Text;
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    barcode.Text = result.Text;
                }
                player.Dispose();
                lastTimestamp = currentTimeMillis();
                timer.Stop();
            }
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                product_info info = new product_info()
                {
                    barcode = barcode.Text,
                    product_name = goods_name.Text,
                    product_count = Convert.ToInt32(goods_count.Text),
                    product_sale = goods_sale.Text,
                    product_money = goods_price.Text
                };
                product_manager.AddProductInfo(info);
                Close();
            }
            catch
            {
                MessageBox.Show("상품 등록 실패!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void retry_Click(object sender, RoutedEventArgs e)
        {
            video.VideoCaptureDevice = webcam;
            barcode.Text = "";
            //System.Threading.Thread.Sleep(1100);
            lastTimestamp = currentTimeMillis();
            timer.Start();
        }

        private void goods_sale_Pasting(object sender, DataObjectPastingEventArgs e)
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

        private void goods_sale_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
