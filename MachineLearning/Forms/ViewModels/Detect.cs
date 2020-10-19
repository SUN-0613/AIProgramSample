using Model = MachineLearning.Forms.Models.Detect;
using System;
using WpfLibrary.MVVM;
using WpfLibrary.Windows;

namespace MachineLearning.Forms.ViewModels
{

    /// <summary>マウスによる手書き文字を認識.ViewModel</summary>
    public class Detect : ViewModelBase, IDisposable
    {

        #region model

        /// <summary>マウスによる手書き文字を認識.Model</summary>
        private readonly Model _Model = new Model();

        #endregion

        #region property

        /// <summary>画像認識の結果</summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>作成する画像情報</summary>
        public CreateBmpFileInfo FileInfo { get; set; }

        /// <summary>画像認識コマンド</summary>
        public DelegateCommand ImageRecognitionCommand
        {
            get => new DelegateCommand(
                () => 
                {

                    FileInfo = _Model.GetBmpFileInfo();
                    CallPropertyChanged(nameof(FileInfo));

                    _Model.SetPixels();

                    Result = _Model.ImageRecognition();
                    CallPropertyChanged(nameof(Result));

                }, 
                () => true);
        }

        /// <summary>Canvasの初期化</summary>
        public bool IsInitializeCanvas { get; set; } = false;

        public DelegateCommand InitializeCommand
        {
            get => new DelegateCommand(
                () => 
                {

                    IsInitializeCanvas = !IsInitializeCanvas;
                    CallPropertyChanged(nameof(IsInitializeCanvas));

                    _Model.InitializePixels();

                    Result = string.Empty;
                    CallPropertyChanged(nameof(Result));

                }, 
                () => true);
        }

        #endregion

        #region instance

        /// <summary>マウスによる手書き文字を認識.ViewModel</summary>
        public Detect()
        { }

        #endregion

        #region method

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            _Model.Dispose();

        }

        #endregion

    }

}
