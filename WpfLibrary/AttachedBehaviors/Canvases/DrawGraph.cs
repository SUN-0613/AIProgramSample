using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfLibrary.AttachedBehaviors.Canvases
{

    /// <summary>Canvasに折れ線グラフを描画する添付ビヘイビア</summary>
    public class DrawGraph
    {

        #region dependency property

        /// <summary>直線の終端座標</summary>
        public static readonly DependencyProperty LinePointProperty
            = DependencyProperty.RegisterAttached(
                "LinePoint",
                typeof(Point),
                typeof(DrawGraph),
                new PropertyMetadata(new Point(double.NaN, double.NaN), OnLinePointChanged));

        /// <summary>直線の終端座標を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static Point GetLinePoint(DependencyObject sender)
        {
            return (Point)sender.GetValue(LinePointProperty);
        }

        /// <summary>直線の終端座標を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetLinePoint(DependencyObject sender, Point value)
        {
            sender.SetValue(LinePointProperty, value);
        }

        /// <summary>グラフの最小値となる座標</summary>
        public static readonly DependencyProperty MinPointProperty
            = DependencyProperty.RegisterAttached(
                "MinPoint",
                typeof(Point),
                typeof(DrawGraph),
                new PropertyMetadata(new Point(double.MinValue, double.MinValue)));

        /// <summary>グラフの最小値となる座標を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static Point GetMinPoint(DependencyObject sender)
        {
            return (Point)sender.GetValue(MinPointProperty);
        }

        /// <summary>グラフの最小値となる座標を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetMinPoint(DependencyObject sender, Point value)
        {
            sender.SetValue(MinPointProperty, value);
        }

        /// <summary>グラフの最大値となる座標</summary>
        public static readonly DependencyProperty MaxPointProperty
            = DependencyProperty.RegisterAttached(
                "MaxPoint",
                typeof(Point),
                typeof(DrawGraph),
                new PropertyMetadata(new Point(double.MaxValue, double.MaxValue)));

        /// <summary>グラフの最大値となる座標を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static Point GetMaxPoint(DependencyObject sender)
        {
            return (Point)sender.GetValue(MaxPointProperty);
        }

        /// <summary>グラフの最大値となる座標を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetMaxPoint(DependencyObject sender, Point value)
        {
            sender.SetValue(MaxPointProperty, value);
        }

        /// <summary>描画する線の色</summary>
        public static readonly DependencyProperty StrokeProperty
            = DependencyProperty.RegisterAttached(
                "Stroke",
                typeof(Brush),
                typeof(DrawGraph),
                new PropertyMetadata(Brushes.Black));

        /// <summary>描画する線の色を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>描画する線の色</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static Brush GetStroke(DependencyObject sender)
        {
            return (Brush)sender.GetValue(StrokeProperty);
        }

        /// <summary>描画する線の色を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">描画する線の色</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetStroke(DependencyObject sender, Brush value)
        {
            sender.SetValue(StrokeProperty, value);
        }

        /// <summary>描画する線の太さ</summary>
        public static readonly DependencyProperty StrokeThicknessProperty
            = DependencyProperty.RegisterAttached(
                "StrokeThickness",
                typeof(double),
                typeof(DrawGraph),
                new PropertyMetadata(2d));

        /// <summary>描画する線の太さを取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>描画する線の太さ</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static double GetStrokeThickness(DependencyObject sender)
        {
            return (double)sender.GetValue(StrokeThicknessProperty);
        }

        /// <summary>描画する線の太さを設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetStrokeThickness(DependencyObject sender, double value)
        {
            sender.SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>グラフ初期化実行FLG</summary>
        public static readonly DependencyProperty IsInitializeProperty
            = DependencyProperty.RegisterAttached(
                "IsInitialize",
                typeof(bool),
                typeof(DrawGraph),
                new PropertyMetadata(false, OnIsInitializeChanged));

        /// <summary>グラフ初期化実行FLGを取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static bool GetIsInitialize(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsInitializeProperty);
        }

        /// <summary>グラフ初期化実行FLGを設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetIsInitialize(DependencyObject sender, bool value)
        {
            sender.SetValue(IsInitializeProperty, value);
        }

        #endregion

        #region global variable

        /// <summary>Point初期化用値</summary>
        private static readonly Point _NaNPoint = new Point(double.NaN, double.NaN);

        /// <summary>現在のグラフ終端位置</summary>
        private static Point _LineEndPoint = _NaNPoint;

        /// <summary>線を描写するコントロール</summary>
        private static Polyline _Line = null;

        #endregion

        #region event

        /// <summary>直線の終端座標の更新イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnLinePointChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (sender is Canvas canvas
                && canvas.IsLoaded
                && e.NewValue is Point point
                && !point.Equals(_NaNPoint))
            {

                if (_Line == null)
                {

                    _Line = new Polyline()
                    {
                        Stroke = GetStroke(canvas),
                        StrokeThickness = GetStrokeThickness(canvas),
                    };

                    canvas.Children.Clear();
                    canvas.Children.Add(_Line);

                }

                if (_LineEndPoint.Equals(_NaNPoint)
                    || !_LineEndPoint.Equals(point))
                {

                    _Line.Points.Add(CalcLineEndPoint(canvas, point));
                    _LineEndPoint = point;

                }

            }

        }

        /// <summary>指定座標をCanvas座標に合わせる</summary>
        /// <param name="canvas">Canvas</param>
        /// <param name="point">指定座標</param>
        /// <returns>Canvas座標</returns>
        private static Point CalcLineEndPoint(Canvas canvas, Point point)
        {

            var min = GetMinPoint(canvas);
            var max = GetMaxPoint(canvas);

            var x = (canvas.ActualWidth * (point.X - min.X)) / (max.X - min.X);
            var y = canvas.ActualHeight - ((canvas.ActualHeight * (point.Y - min.Y)) / (max.Y - min.Y));

            return new Point(x, y);

        }

        /// <summary>グラフ初期化実行FLGの更新イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnIsInitializeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (sender is Canvas canvas)
            {

                _Line?.Points.Clear();
                _Line = null;

                _LineEndPoint = _NaNPoint;

                canvas.Children.Clear();

            }

        }

        #endregion

    }

}
