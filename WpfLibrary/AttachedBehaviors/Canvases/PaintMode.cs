using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfLibrary.AttachedBehaviors.Canvases
{

    /// <summary>Canvasにフリーハンドで描画する機能を設ける添付ビヘイビア</summary>
    /// <remarks>https://stackoverflow.com/questions/11010177/how-to-draw-a-free-hand-shape-random-text-printing-on-wpf-canvas</remarks>
    public class PaintMode
    {

        #region dependency property

        /// <summary>描画機能の有効化</summary>
        public static readonly DependencyProperty IsEnabledProperty
            = DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(PaintMode),
                new PropertyMetadata(false, OnIsEnabledChanged));

        /// <summary>描画機能有効化の値を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>描画機能有効化の値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static bool GetIsEnabled(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsEnabledProperty);
        }

        /// <summary>描画機能有効化の値を設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static void SetIsEnabled(DependencyObject sender, bool value)
        {
            sender.SetValue(IsEnabledProperty, value);
        }

        /// <summary>描画する線の色</summary>
        public static readonly DependencyProperty StrokeProperty
            = DependencyProperty.RegisterAttached(
                "Stroke",
                typeof(Brush),
                typeof(PaintMode),
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
                typeof(PaintMode),
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
                typeof(PaintMode),
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

        /// <summary>マウスの現在位置</summary>
        private static Point _MouseCurrentPoint;

        /// <summary>線を描写するコントロール</summary>
        private static Polyline _Line;

        #endregion

        #region event

        /// <summary>描画機能有効化の値設定イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is bool value && value)
            {

                if (sender is Canvas canvas)
                {

                    canvas.Unloaded += OnUnloaded;
                    canvas.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    canvas.MouseMove += OnMouseMove;

                }

            }
            else
            {
                try
                {
                    OnUnloaded(sender, null);
                }
                catch { }
            }

        }

        /// <summary>イベント解除</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">イベントデータ</param>
        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {

            if (sender is Canvas canvas)
            {

                canvas.Unloaded -= OnUnloaded;
                canvas.MouseLeftButtonDown -= OnMouseLeftButtonDown;
                canvas.MouseMove -= OnMouseMove;

            }

        }

        /// <summary>マウス左ボタン押下</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        /// <remarks>フリーハンドで描画開始</remarks>
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is Canvas canvas)
            {

                // 開始位置の記憶
                _MouseCurrentPoint = e.GetPosition(canvas);

                // 線コントロールのインスタンス生成
                _Line = new Polyline()
                {
                    Stroke = GetStroke(canvas),
                    StrokeThickness = GetStrokeThickness(canvas),
                };

                // 生成したインスタンスをCanvasに追加
                canvas.Children.Add(_Line);

            }

        }

        /// <summary>マウス移動イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">マウスイベントデータ</param>
        /// <remarks>マウス左ボタン押下中はフリーハンドによる描画を継続</remarks>
        private static void OnMouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton.Equals(MouseButtonState.Pressed)
                && sender is Canvas canvas
                && !_MouseCurrentPoint.Equals(_NaNPoint))
            {

                // マウスの現在位置を取得
                var current = e.GetPosition(canvas);

                // 前回位置から現在位置までの線を描画
                if (!_MouseCurrentPoint.Equals(current))
                {

                    _Line.Points.Add(current);
                    _MouseCurrentPoint = current;

                }

            }

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

                _MouseCurrentPoint = _NaNPoint;

                canvas.Children.Clear();

            }

        }

        #endregion

    }

}
