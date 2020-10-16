using System;
using System.Windows;

namespace WpfLibrary.Windows
{

    /// <summary>指定ダイアログを表示する添付ビヘイビア</summary>
    public class ShowDialog
    {

        #region dependency property

        /// <summary>ダイアログ表示実行管理FLG</summary>
        public static readonly DependencyProperty IsShowDialogProperty
            = DependencyProperty.RegisterAttached(
                "IsShowDialog",
                typeof(bool),
                typeof(ShowDialog),
                new PropertyMetadata(false, OnIsShowDialogChanged));

        /// <summary>ダイアログ表示実行管理FLGの現在値取得</summary>
        /// <param name="sender">Window</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsShowDialog(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsShowDialogProperty);
        }

        /// <summary>ダイアログ表示実行管理FLGの設定</summary>
        /// <param name="sender">Window</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetIsShowDialog(DependencyObject sender, bool value)
        {
            sender.SetValue(IsShowDialogProperty, value);
        }

        /// <summary>表示するダイアログ</summary>
        public static readonly DependencyProperty DialogTypeProperty
            = DependencyProperty.RegisterAttached(
                "DialogType",
                typeof(Type),
                typeof(ShowDialog),
                new PropertyMetadata(null));

        /// <summary>表示するダイアログの現在値を取得</summary>
        /// <param name="sender">Window</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static Type GetDialogType(DependencyObject sender)
        {
            return (Type)sender.GetValue(DialogTypeProperty);
        }

        /// <summary>表示するダイアログを設定</summary>
        /// <param name="sender">Window</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetDialogType(DependencyObject sender, Type value)
        {
            sender.SetValue(DialogTypeProperty, value);
        }

        /// <summary>表示したダイアログの結果</summary>
        public static readonly DependencyProperty ResultProperty
            = DependencyProperty.RegisterAttached(
                "Result",
                typeof(bool?),
                typeof(ShowDialog),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>表示したダイアログの結果を取得</summary>
        /// <param name="sender">Window</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool? GetResult(DependencyObject sender)
        {
            return (bool?)sender.GetValue(ResultProperty);
        }

        /// <summary>表示したダイアログの結果を設定</summary>
        /// <param name="sender">Window</param>
        /// <param name="value">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetResult(DependencyObject sender, bool? value)
        {
            sender.SetValue(ResultProperty, value);
        }

        #endregion

        #region event

        /// <summary>ダイアログ表示実行管理FLGの値変更イベント</summary>
        /// <param name="sender">Window</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnIsShowDialogChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (sender is Window owner)
            {

                var type = GetDialogType(owner);

                if (type != null
                    && Activator.CreateInstance(type) is Window dialog)
                {

                    dialog.Owner = owner;
                    dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                    var result = dialog.ShowDialog();

                    SetResult(owner, result);

                }

            }

        }

        #endregion

    }

}
