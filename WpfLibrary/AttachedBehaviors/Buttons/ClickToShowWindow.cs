using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfLibrary.AttachedBehaviors.Buttons
{

    /// <summary>ボタンをクリックすることで子Windowを表示する添付ビヘイビア</summary>
    public class ClickToShowWindow
    {

        #region dependency property

        /// <summary>表示する子Windowの型</summary>
        public static readonly DependencyProperty WindowTypeProperty
            = DependencyProperty.RegisterAttached(
                "WindowType",
                typeof(Type),
                typeof(ClickToShowWindow),
                new PropertyMetadata(null, OnWindowTypeChanged));

        /// <summary>表示する子Windowの型を取得</summary>
        /// <param name="sender">Button</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static Type GetWindowType(DependencyObject sender)
        {
            return (Type)sender.GetValue(WindowTypeProperty);
        }

        /// <summary>表示する子Windowの型を設定</summary>
        /// <param name="sender">Button</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static void SetWindowType(DependencyObject sender, Type value)
        {
            sender.SetValue(WindowTypeProperty, value);
        }

        /// <summary>子Windowをダイアログとして表示するか</summary>
        public static readonly DependencyProperty IsDialogFormatProperty
            = DependencyProperty.RegisterAttached(
                "IsDialogFormat",
                typeof(bool),
                typeof(ClickToShowWindow),
                new PropertyMetadata(false));

        /// <summary>子Windowをダイアログとして表示するか値を取得</summary>
        /// <param name="sender">Button</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static bool GetIsDialogFormat(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsDialogFormatProperty);
        }

        /// <summary>子Windowをダイアログとして表示するか値を設定</summary>
        /// <param name="sender">Button</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static void SetIsDialogFormat(DependencyObject sender, bool value)
        {
            sender.SetValue(IsDialogFormatProperty, value);
        }

        /// <summary>ダイアログ戻り値</summary>
        public static readonly DependencyProperty DialogResultProperty
            = DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(ClickToShowWindow),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>ダイアログ戻り値を取得</summary>
        /// <param name="sender">Button</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static bool? GetDialogResult(DependencyObject sender)
        {
            return (bool?)sender.GetValue(DialogResultProperty);
        }

        /// <summary>ダイアログ戻り値を設定</summary>
        /// <param name="sender">Button</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static void SetDialogResult(DependencyObject sender, bool? value)
        {
            sender.SetValue(DialogResultProperty, value);
        }

        #endregion

        #region event

        /// <summary>Windowの型変更イベント</summary>
        /// <param name="sender">親Window</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnWindowTypeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is Type type && type != null)
            {

                if (sender is Button button)
                {

                    button.Unloaded += OnUnloaded;
                    button.Click += OnClick;

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
        /// <param name="sender">Button</param>
        /// <param name="e">イベントデータ</param>
        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {

            if (sender is Button button)
            {

                button.Unloaded -= OnUnloaded;
                button.Click -= OnClick;

            }

        }

        /// <summary>ボタンクリックイベント</summary>
        /// <param name="sender">Button</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>子Window表示</remarks>
        private static void OnClick(object sender, RoutedEventArgs e)
        {

            if (sender is Button button)
            {

                var owner = Window.GetWindow(button);

                if (owner != null)
                {

                    var type = GetWindowType(button);
                    var isDialog = GetIsDialogFormat(button);

                    if (Activator.CreateInstance(type) is Window window)
                    {

                        window.Owner = owner;
                        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (isDialog)
                        {
                            SetDialogResult(button, window.ShowDialog());
                        }
                        else
                        {
                            window.Show();
                        }

                    }

                }

            }

        }

        #endregion

    }

}
