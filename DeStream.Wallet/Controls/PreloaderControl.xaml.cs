using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DeStream.Wallet.Controls
{
    /// <summary>
    /// Interaction logic for PreloaderControl.xaml
    /// </summary>
    public partial class PreloaderControl : UserControl
    {
        public PreloaderControl()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, x =>
            {
                var source = GetSource();
                ImageControl.Source = _source;
                ImageAnimator.Animate(_bitmap, OnFrameChanged);
            });
        }
        ImageSource _source;
        Bitmap _bitmap;

        private BitmapSource GetSource()
        {
            if (_bitmap == null)
            {
                _bitmap = new Bitmap("/Images/preloader.gif.gif");
            }
            IntPtr handle = IntPtr.Zero;
            handle = _bitmap.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(
                    handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                    new Action(FrameUpdatedCallback));
        }

        private void FrameUpdatedCallback()
        {
            ImageAnimator.UpdateFrames();
            if (_source != null)
                _source.Freeze();
            _source = GetSource();
            ImageControl.Source = _source;
            InvalidateVisual();
        }
    }
}
