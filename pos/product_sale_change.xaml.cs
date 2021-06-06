using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pos
{
    /// <summary>
    /// product_cale_change.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class product_sale_change : Window
    {
        public bool is_change = false;
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text

        public product_sale_change(string product_name, int sale)
        {
            InitializeComponent();
            title.Content = product_name + " 할인율 변경";
            count.Text = sale.ToString();
            Loaded += product_cale_change_Loaded;
        }

        private void product_cale_change_Loaded(object sender, RoutedEventArgs e)
        {
            count.Focus();
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (count.Text != "")
            {
                is_change = true;
                Close();
            }
        }

        private void count_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Console.WriteLine("close");
                is_change = true;
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void count_Pasting(object sender, DataObjectPastingEventArgs e)
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

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void count_MouseDown(object sender, RoutedEventArgs e)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(delegate
            {
                (sender as TextBox).SelectAll();
            }));
        }
    }
}
