using Model = MachineLearning.Forms.Models.Main;
using WpfLibrary.MVVM;

namespace MachineLearning.Forms.ViewModels
{

    /// <summary>MNISTのデータセットを機械学習する.ViewModel</summary>
    public class Main : ViewModelBase
    {

        #region property

        #endregion

        #region model

        /// <summary>MNISTのデータセットを機械学習する.Model</summary>
        private readonly Model _Model;

        #endregion

        #region instance

        /// <summary>MNISTのデータセットを機械学習する.ViewModel</summary>
        public Main()
        {

            _Model = new Model();

        }

        #endregion

    }

}
