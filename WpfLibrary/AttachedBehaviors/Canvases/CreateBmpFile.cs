using System.Windows;
using System.Windows.Controls;
using WpfLibrary.Windows.Abstracts;

namespace WpfLibrary.AttachedBehaviors.Canvases
{

    /// <summary>Canvasの内容をBitmapファイルに保存する添付ビヘイビア</summary>
    public class CreateBmpFile
    {

        #region dependency property

        /// <summary>Bitmapファイル情報</summary>
        public static readonly DependencyProperty FileInfoProperty
            = DependencyProperty.RegisterAttached(
                "FileInfo",
                typeof(ICreatePictureFile),
                typeof(CreateBmpFile),
                new PropertyMetadata(null, BmpFileInfoChanged));

        /// <summary>Bitmapファイル情報の取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static ICreatePictureFile GetFileInfo(DependencyObject sender)
        {
            return (CreatePictureFileBase)sender.GetValue(FileInfoProperty);
        }

        /// <summary>Bitmapファイル情報を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetFileInfo(DependencyObject sender, ICreatePictureFile value)
        {
            sender.SetValue(FileInfoProperty, value);
        }

        #endregion

        #region event

        /// <summary>Bitmapファイル情報変更イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void BmpFileInfoChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is ICreatePictureFile info
                && sender is Canvas canvas)
            {
                info.CreatePictureFile(canvas);
            }

        }

        #endregion

    }

}
