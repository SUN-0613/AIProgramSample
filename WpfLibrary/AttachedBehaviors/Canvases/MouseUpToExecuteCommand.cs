using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfLibrary.AttachedBehaviors.Canvases
{

    /// <summary>マウスを離上したときに指定したコマンドを実行する添付ビヘイビア</summary>
    public class MouseUpToExecuteCommand
    {

        #region dependency property

        /// <summary>実行するコマンド</summary>
        public static readonly DependencyProperty CommandProperty
            = DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(MouseUpToExecuteCommand),
                new PropertyMetadata(null, OnCommandChanged));

        /// <summary>実行するコマンドの現在値を取得</summary>
        /// <param name="sender">Canvas</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Canvas))]
        public static ICommand GetCommand(DependencyObject sender)
        {
            return (ICommand)sender.GetValue(CommandProperty);
        }

        /// <summary>実行するコマンドを設定</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="value">設定値</param>
        public static void SetCommand(DependencyObject sender, ICommand value)
        {
            sender.SetValue(CommandProperty, value);
        }

        #endregion

        #region event

        /// <summary>実行するコマンド変更イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is ICommand command
                && command != null)
            {

                if (sender is Canvas canvas)
                {

                    canvas.Unloaded += OnUnloaded;
                    canvas.MouseUp += OnMouseUp;

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
                canvas.MouseUp -= OnMouseUp;

            }

        }

        /// <summary>マウス離上イベント</summary>
        /// <param name="sender">Canvas</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        /// <remarks>指定コマンドを実行</remarks>
        private static void OnMouseUp(object sender, MouseButtonEventArgs e)
        {

            if (sender is Canvas canvas)
            {

                var command = GetCommand(canvas);

                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }

            }

        }

        #endregion

    }

}
