using System.Windows;
using System.Windows.Controls;
using WpfLibrary.Windows;

namespace WpfLibrary.AttachedBehaviors.Canvases
{

    /// <summary>Canvasの内容をBitmapファイルに保存する添付ビヘイビア</summary>
    public class CreateBmpFile
    {

        #region dependency property

        /// <summary>Bitmapファイル情報</summary>
        public static readonly DependencyProperty BmpFileInfoProperty
            = DependencyProperty.RegisterAttached(
                "BmpFileInfo",
                typeof(CreateBmpFileInfo),
                typeof(CreateBmpFile),
                new PropertyMetadata(false, BmpFileInfoChanged));

        /// <summary>Bitmapファイル情報の取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static CreateBmpFileInfo GetBmpFileInfo(DependencyObject sender)
        {
            return (CreateBmpFileInfo)sender.GetValue(BmpFileInfoProperty);
        }

        /// <summary>Bitmapファイル情報を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetBmpFileInfo(DependencyObject sender, CreateBmpFileInfo value)
        {
            sender.SetValue(BmpFileInfoProperty, value);
        }

        #endregion

        #region event

        /// <summary>Bitmapファイル情報変更イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void BmpFileInfoChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is CreateBmpFileInfo info
                && sender is Canvas canvas)
            {
                info.CreateBitmapFile(canvas);
            }

        }

        #endregion

    }

}
