using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfLibrary.AttachedBehaviors.Canvases
{

    /// <summary>Canvasの内容をBitmapファイルに保存する添付ビヘイビア</summary>
    public class CreateBmpFile
    {

        #region dependency property

        /// <summary>Bitmapファイル生成FLG</summary>
        public static readonly DependencyProperty IsCreateProperty
            = DependencyProperty.RegisterAttached(
                "IsCreate",
                typeof(bool),
                typeof(CreateBmpFile),
                new PropertyMetadata(false, OnIsCreateChanged));

        /// <summary>Bitmapファイル生成FLGの値取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>Bitmapファイル生成FLGの値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static bool GetIsCreate(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsCreateProperty);
        }

        /// <summary>Bitmapファイル生成FLGの値を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetIsCreate(DependencyObject sender, bool value)
        {
            sender.SetValue(IsCreateProperty, value);
        }

        /// <summary>
        /// 画像サイズ：幅
        /// 指定しない場合はCanvas.ActualWidthを使用
        /// </summary>
        public static readonly DependencyProperty WidthProperty
            = DependencyProperty.RegisterAttached(
                "Width",
                typeof(int),
                typeof(CreateBmpFile),
                new PropertyMetadata(-1));

        /// <summary>画像サイズ：幅を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>画像サイズ：幅</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static int GetWidth(DependencyObject sender)
        {
            return (int)sender.GetValue(WidthProperty);
        }

        /// <summary>画像サイズ：幅を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetWidth(DependencyObject sender, int value)
        {
            sender.SetValue(WidthProperty, value);
        }

        /// <summary>
        /// 画像サイズ：高さ
        /// 指定しない場合はCanvas.ActualHeightを使用
        /// </summary>
        public static readonly DependencyProperty HeightProperty
            = DependencyProperty.RegisterAttached(
                "Height",
                typeof(int),
                typeof(CreateBmpFile),
                new PropertyMetadata(-1));

        /// <summary>画像サイズ：高さを取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>画像サイズ：高さ</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static int GetHeight(DependencyObject sender)
        {
            return (int)sender.GetValue(HeightProperty);
        }

        /// <summary>画像サイズ：高さを設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetHeight(DependencyObject sender, int value)
        {
            sender.SetValue(HeightProperty, value);
        }

        /// <summary>X軸：dots per inch</summary>
        public static readonly DependencyProperty DpiXProperty
            = DependencyProperty.RegisterAttached(
                "DpiX",
                typeof(double),
                typeof(CreateBmpFile),
                new PropertyMetadata(96d));

        /// <summary>X軸：dots per inchを取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>X軸：dots per inch</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static double GetDpiX(DependencyObject sender)
        {
            return (double)sender.GetValue(DpiXProperty);
        }

        /// <summary>X軸：dots per inchを設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetDpiX(DependencyObject sender, double value)
        {
            sender.SetValue(DpiXProperty, value);
        }

        /// <summary>Y軸：dots per inch</summary>
        public static readonly DependencyProperty DpiYProperty
            = DependencyProperty.RegisterAttached(
                "DpiY",
                typeof(double),
                typeof(CreateBmpFile),
                new PropertyMetadata(96d));

        /// <summary>Y軸：dots per inchを取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>Y軸：dots per inch</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static double GetDpiY(DependencyObject sender)
        {
            return (double)sender.GetValue(DpiYProperty);
        }

        /// <summary>Y軸：dots per inchを設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetDpiY(DependencyObject sender, double value)
        {
            sender.SetValue(DpiYProperty, value);
        }

        /// <summary>Pixel形式</summary>
        public static readonly DependencyProperty PixelFormatProperty
            = DependencyProperty.RegisterAttached(
                "PixelFormat",
                typeof(PixelFormat),
                typeof(CreateBmpFile),
                new PropertyMetadata(PixelFormats.Pbgra32));

        /// <summary>Pixel形式を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>Pixel形式</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static PixelFormat GetPixelFormat(DependencyObject sender)
        {
            return (PixelFormat)sender.GetValue(PixelFormatProperty);
        }

        /// <summary>Pixel形式を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetPixelFormat(DependencyObject sender, PixelFormat value)
        {
            sender.SetValue(PixelFormatProperty, value);
        }

        /// <summary>画像に変換するCanvasのX軸開始地点</summary>
        public static readonly DependencyProperty LeftProperty
            = DependencyProperty.RegisterAttached(
                "Left",
                typeof(double),
                typeof(CreateBmpFile),
                new PropertyMetadata(0d));

        /// <summary>画像に変換するCanvasのX軸開始地点を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>画像に変換するCanvasのX軸開始地点</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static double GetLeft(DependencyObject sender)
        {
            return (double)sender.GetValue(LeftProperty);
        }

        /// <summary>画像に変換するCanvasのX軸開始地点を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetLeft(DependencyObject sender, double value)
        {
            sender.SetValue(LeftProperty, value);
        }

        /// <summary>画像に変換するCanvasのY軸開始地点</summary>
        public static readonly DependencyProperty TopProperty
            = DependencyProperty.RegisterAttached(
                "Top",
                typeof(double),
                typeof(CreateBmpFile),
                new PropertyMetadata(0d));

        /// <summary>画像に変換するCanvasのY軸開始地点を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>画像に変換するCanvasのY軸開始地点</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static double GetTop(DependencyObject sender)
        {
            return (double)sender.GetValue(TopProperty);
        }

        /// <summary>画像に変換するCanvasのY軸開始地点を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetTop(DependencyObject sender, double value)
        {
            sender.SetValue(TopProperty, value);
        }

        /// <summary>画像ファイルのファイル名</summary>
        public static readonly DependencyProperty FilePathProperty
            = DependencyProperty.RegisterAttached(
                "FilePath",
                typeof(string),
                typeof(CreateBmpFile),
                new PropertyMetadata(string.Empty));

        /// <summary>画像ファイルのファイル名を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>画像ファイルのファイル名</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static string GetFilePath(DependencyObject sender)
        {
            return (string)sender.GetValue(FilePathProperty);
        }

        /// <summary>画像ファイルのファイル名を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetFilePath(DependencyObject sender, string value)
        {
            sender.SetValue(FilePathProperty, value);
        }

        #endregion

        #region event

        /// <summary>Bitmapファイル生成FLGの値変更イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnIsCreateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is bool value
                && value
                && sender is Canvas canvas)
            {

                // RenderTargetBitmapインスタンス生成
                var target = new RenderTargetBitmap(
                                    GetWidth(canvas),
                                    GetHeight(canvas),
                                    GetDpiX(canvas),
                                    GetDpiY(canvas),
                                    GetPixelFormat(canvas));

                // 一部を切り取るためにCanvasの位置を移動
                var isTranslate = false;
                var transform = canvas.RenderTransform;
                var left = GetLeft(canvas);
                var top = GetTop(canvas);

                if (!left.Equals(0d)
                    || !top.Equals(0d))
                {

                    canvas.RenderTransform = new TranslateTransform(left, top);
                    canvas.UpdateLayout();

                    isTranslate = true;

                }

                // bitmapへ出力
                using (var stream = new FileStream(GetFilePath(canvas), FileMode.OpenOrCreate))
                {

                    var encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(target));
                    encoder.Save(stream);

                }

                // Canvasの位置を元に戻す
                if (isTranslate)
                {
                    canvas.RenderTransform = transform;
                    canvas.UpdateLayout();
                }

            }

        }

        #endregion

    }

}
