using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;

namespace WpfLibrary.Windows
{

    /// <summary>Windowを閉じて良いか判定する添付ビヘイビア</summary>
    public class CheckClosable
    {

        #region dependency property

        /// <summary>Windowsを閉じて良いか管理するFLG</summary>
        public static readonly DependencyProperty IsClosableProperty
            = DependencyProperty.RegisterAttached(
                "IsClosable",
                typeof(bool),
                typeof(CheckClosable),
                new PropertyMetadata(false, OnIsClosableChanged));

        /// <summary>Windowsを閉じて良いか管理するFLGの値を取得</summary>
        /// <param name="sender">Window</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsClosable(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsClosableProperty);
        }

        /// <summary>Windowsを閉じて良いか管理するFLGを設定</summary>
        /// <param name="sender">Window</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetIsClosable(DependencyObject sender, bool value)
        {
            sender.SetValue(IsClosableProperty, value);
        }

        #endregion

        #region event

        /// <summary>管理FLGの値変更イベント</summary>
        /// <param name="sender">Window</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnIsClosableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is bool value && value)
            {

                if (sender is Window window)
                {

                    window.Unloaded += OnUnloaded;
                    window.Closing += OnClosing;

                    SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);

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
        /// <param name="sender">Window</param>
        /// <param name="e">イベントデータ</param>
        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {

            if (sender is Window window)
            {

                window.Unloaded -= OnUnloaded;
                window.Closing -= OnClosing;

                SystemEvents.SessionEnding -= new SessionEndingEventHandler(SystemEvents_SessionEnding);

            }

        }

        /// <summary>Windowを閉じるか判定する</summary>
        /// <param name="sender">Window</param>
        /// <param name="e">キャンセルイベントデータ</param>
        private static void OnClosing(object sender, CancelEventArgs e)
        {

            if (sender is Window window)
            {

                var isClosable = GetIsClosable(window);

                if (!isClosable)
                {

                    e.Cancel = true;
                    MessageBox.Show("現在Windowを閉じることはできません", window.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);

                }

            }

        }

        /// <summary>Windowsの終了処理</summary>
        private static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {

            if (sender is Window window)
            {

                switch (e.Reason)
                {
                    case SessionEndReasons.Logoff:
                    case SessionEndReasons.SystemShutdown:
                        SetIsClosable(window, true);
                        break;

                }

            }

        }

        #endregion

    }

}
