#pragma checksum "..\..\..\edit_product.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "00C0109F9510D380660ADC20900EE54D31102414265F56A6F77BA98448C9DC12"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPFMediaKit.DirectShow.Controls;
using pos;


namespace pos {
    
    
    /// <summary>
    /// edit_product
    /// </summary>
    public partial class edit_product : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView product_list;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WPFMediaKit.DirectShow.Controls.VideoCaptureElement video;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox barcode;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox goods_name;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox goods_count;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox goods_price;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox goods_sale;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button confirm;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ok;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\edit_product.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/pos;component/edit_product.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\edit_product.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.product_list = ((System.Windows.Controls.ListView)(target));
            
            #line 16 "..\..\..\edit_product.xaml"
            this.product_list.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.product_list_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.video = ((WPFMediaKit.DirectShow.Controls.VideoCaptureElement)(target));
            return;
            case 3:
            this.barcode = ((System.Windows.Controls.TextBox)(target));
            
            #line 47 "..\..\..\edit_product.xaml"
            this.barcode.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.barcode_Pasting));
            
            #line default
            #line hidden
            
            #line 47 "..\..\..\edit_product.xaml"
            this.barcode.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.barcode_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.goods_name = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.goods_count = ((System.Windows.Controls.TextBox)(target));
            
            #line 59 "..\..\..\edit_product.xaml"
            this.goods_count.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.barcode_Pasting));
            
            #line default
            #line hidden
            
            #line 59 "..\..\..\edit_product.xaml"
            this.goods_count.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.barcode_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 6:
            this.goods_price = ((System.Windows.Controls.TextBox)(target));
            
            #line 62 "..\..\..\edit_product.xaml"
            this.goods_price.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.barcode_Pasting));
            
            #line default
            #line hidden
            
            #line 62 "..\..\..\edit_product.xaml"
            this.goods_price.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.barcode_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 7:
            this.goods_sale = ((System.Windows.Controls.TextBox)(target));
            
            #line 65 "..\..\..\edit_product.xaml"
            this.goods_sale.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.barcode_Pasting));
            
            #line default
            #line hidden
            
            #line 65 "..\..\..\edit_product.xaml"
            this.goods_sale.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.barcode_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 8:
            this.confirm = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\edit_product.xaml"
            this.confirm.Click += new System.Windows.RoutedEventHandler(this.confirm_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ok = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\..\edit_product.xaml"
            this.ok.Click += new System.Windows.RoutedEventHandler(this.ok_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.cancel = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\..\edit_product.xaml"
            this.cancel.Click += new System.Windows.RoutedEventHandler(this.cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

