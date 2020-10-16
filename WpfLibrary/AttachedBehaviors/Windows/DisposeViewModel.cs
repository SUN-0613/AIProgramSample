using System;
using System.Windows;

namespace WpfLibrary.AttachedBehaviors.Windows
{

    /// <summary>Windowを閉じるときにViewModel.Dispose()を実行する添付ビヘイビア</summary>
    public class DisposeViewModel
    {

        #region dependency property

        /// <summary>ViewModel.Dispose()実行管理FLG</summary>
        public static readonly DependencyProperty IsDisposableViewModelProperty
            = DependencyProperty.RegisterAttached(
                "IsDisposableViewModel",
                typeof(bool),
                typeof(DisposeViewModel),
                new PropertyMetadata(false, OnIsDisposableViewModelChanged));

        /// <summary>ViewModel.Dispose()実行管理FLGの値取得</summary>
        /// <param name="sender">Window</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsDisposableViewModel(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsDisposableViewModelProperty);
        }

        /// <summary>ViewModel.Dispose()実行管理FLGの値を設定</summary>
        /// <param name="sender">Window</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetIsDisposableViewModel(DependencyObject sender, bool value)
        {
            sender.SetValue(IsDisposableViewModelProperty, value);
        }

        #endregion

        #region event

        /// <summary>ViewModel.Dispose()実行管理FLGの値変更イベント</summary>
        /// <param name="sender">Window</param>
        /// <param name="e">プロパティ変更イベント</param>
        private static void OnIsDisposableViewModelChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is bool value && value)
            {

                if (sender is Window window)
                {

                    window.Unloaded += OnUnloaded;
                    window.Closed += OnClosed;

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
                window.Closed -= OnClosed;

            }

        }

        /// <summary>ViewModel.Dispose()実行</summary>
        /// <param name="sender">Window</param>
        /// <param name="e">イベントデータ</param>
        private static void OnClosed(object sender, EventArgs e)
        {

            if (sender is Window window
                && window.DataContext is IDisposable viewModel)
            {
                viewModel.Dispose();
            }

        }

        #endregion

    }

}
