using System.Windows;
using WpfLibrary.Windows;

namespace WpfLibrary.AttachedBehaviors.Windows
{

    /// <summary>メッセージボックスを表示する添付ビヘイビア</summary>
    public class ShowMessageBox
    {

        #region dependency property

        /// <summary>メッセージボックス表示内容</summary>
        public static readonly DependencyProperty InfoProperty
            = DependencyProperty.RegisterAttached(
                "Info",
                typeof(MessageBoxInfo),
                typeof(ShowMessageBox),
                new PropertyMetadata(null, OnInfoChanged));

        /// <summary>メッセージボックス表示内容を取得</summary>
        /// <param name="sender">Window</param>
        /// <returns>現在値</returns>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public MessageBoxInfo GetInfo(DependencyObject sender)
        {
            return (MessageBoxInfo)sender.GetValue(InfoProperty);
        }

        /// <summary>メッセージボックス表示内容を設定</summary>
        /// <param name="sender">Window</param>
        /// <param name="info">設定値</param>
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public void SetInfo(DependencyObject sender, MessageBoxInfo info)
        {
            sender.SetValue(InfoProperty, info);
        }

        #endregion

        #region event

        /// <summary>メッセージボックス表示内容の変更イベント</summary>
        /// <param name="sender">Window</param>
        /// <param name="e">プロパティ変更イベントデータ</param>
        private static void OnInfoChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is MessageBoxInfo info)
            {

                info.Result = MessageBox.Show(
                    info.Message,
                    info.Title,
                    info.Button,
                    info.Image,
                    info.DefaultResult,
                    info.Options);

            }

        }

        #endregion

    }

}
