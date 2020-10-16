using MachineLearning.Forms.Views;
using System.Windows;

namespace MachineLearning
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {

        /// <summary>処理開始</summary>
        /// <param name="e">引数データ</param>
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

            new Detect().ShowDialog();

        }

    }

}
