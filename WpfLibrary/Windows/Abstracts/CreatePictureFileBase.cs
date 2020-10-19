using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfLibrary.Windows.Abstracts
{

    /// <summary>Canvasから画像ファイルを作成するためのデータ情報のベースクラス</summary>
    public abstract class CreatePictureFileBase : ICreatePictureFile
    {

        #region const

        /// <summary>DPI既定値</summary>
        /// <remarks>dots per inch</remarks>
        public const double DpiDefaultValue = 96d;

        #endregion

        #region property

        /// <summary>
        /// 生成するBitmapファイルのピクセル幅
        /// 指定しない場合はCanvas.ActualWidthを使用
        /// </summary>
        public int PixelWidth { get; set; } = -1;

        /// <summary>
        /// 生成するBitmapファイルのピクセル高さ
        /// 指定しない場合はCanvas.ActualHeightを使用
        /// </summary>
        public int PixelHeight { get; set; } = -1;

        /// <summary>X軸DPI</summary>
        public double DpiX { get; set; } = DpiDefaultValue;

        /// <summary>Y軸DPI</summary>
        public double DpiY { get; set; } = DpiDefaultValue;

        /// <summary>Pixel形式</summary>
        public PixelFormat PixelFormat { get; set; } = PixelFormats.Pbgra32;

        /// <summary>画像に変換するCanvasのX軸開始地点</summary>
        public double Left { get; set; } = 0d;

        /// <summary>画像に変換するCanvasのY軸開始地点</summary>
        public double Top { get; set; } = 0d;

        /// <summary>生成するBitmapファイルのパス</summary>
        public string FilePath { get; private set; }

        #endregion

        #region instance

        /// <summary>Canvasから画像ファイルを作成するためのデータ情報のベースクラス</summary>
        /// <param name="filePath">生成する画像ファイルのパス</param>
        public CreatePictureFileBase(string filePath)
        {
            FilePath = filePath;
        }

        #endregion

        #region method

        /// <summary>出力する画像ファイルの形式に合わせたエンコーダを取得</summary>
        protected abstract BitmapEncoder GetEncorder();

        /// <summary>指定するCanvasの内容で画像ファイルを生成</summary>
        /// <param name="canvas">Canvas</param>
        public void CreatePictureFile(Canvas canvas)
        {

            if (!canvas.IsLoaded)
            {
                throw new Exception("Canvas is not loaded.");
            }

            // 出力フォルダの作成
            CreateDirectoryPath(FilePath);

            // 幅、高さの設定
            var width = PixelWidth.Equals(-1) ? (int)canvas.ActualWidth : PixelWidth;
            var height = PixelHeight.Equals(-1) ? (int)canvas.ActualHeight : PixelHeight;

            if (width <= 0 || height <= 0
                || DpiX <= 0 || DpiY <= 0)
            {
                throw new ArgumentOutOfRangeException("Properties's value is out of range.");
            }

            // RenderTargetBitmapインスタンス生成
            var target = new RenderTargetBitmap(width, height, DpiX, DpiY, PixelFormat);

            // 一部を切り取るためにCanvasの位置を移動
            var isTranslate = false;
            var renderTransform = canvas.RenderTransform;
            var layoutTransform = canvas.LayoutTransform;

            try
            {

                var left = Left - canvas.Margin.Left;
                var top = Top - canvas.Margin.Top;

                if (!left.Equals(0) || !top.Equals(0))
                {

                    canvas.RenderTransform = new TranslateTransform(left, top);

                    isTranslate = true;

                }

                if (!width.Equals(canvas.ActualWidth)
                    || !height.Equals(canvas.ActualHeight))
                {

                    var scaleX = width / canvas.ActualWidth;
                    var scaleY = height / canvas.ActualHeight;

                    canvas.LayoutTransform = new ScaleTransform(scaleX, scaleY);

                    isTranslate = true;

                }

                if (isTranslate)
                {
                    canvas.UpdateLayout();
                }

                target.Render(canvas);

                // bitmapへ出力
                using (var stream = new FileStream(FilePath, FileMode.OpenOrCreate))
                {

                    var encoder = GetEncorder();
                    encoder.Frames.Add(BitmapFrame.Create(target));
                    encoder.Save(stream);

                }

            }
            finally
            {

                // Canvasの位置を元に戻す
                if (isTranslate)
                {
                    canvas.RenderTransform = renderTransform;
                    canvas.LayoutTransform = layoutTransform;
                    canvas.UpdateLayout();
                }

            }

        }

        /// <summary>指定したファイルパスのフォルダを作成</summary>
        /// <param name="filePath">ファイルパス</param>
        private void CreateDirectoryPath(string filePath)
        {

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Specify File Path.");
            }
            if (filePath.Length.Equals(0))
            {
                throw new ArgumentException("Specify File Path.");
            }

            var directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

        }

        #endregion

    }

}
