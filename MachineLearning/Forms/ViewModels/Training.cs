using Model = MachineLearning.Forms.Models.Training;
using System;
using System.Windows;
using WpfLibrary.MVVM;
using WpfLibrary.Windows;

namespace MachineLearning.Forms.ViewModels
{

    /// <summary>MNISTのデータセットを機械学習する.ViewModel</summary>
    public class Training : ViewModelBase, IDisposable
    {

        #region property

        /// <summary>機械学習ボタン有効化</summary>
        public bool IsButtonEnabled { get; private set; } = true;

        /// <summary>機械学習コマンド</summary>
        public DelegateCommand TrainCommand
        {
            get
            {

                return new DelegateCommand(
                    () =>
                    {
                        _Model.BeginTrainAsync();
                    },
                    () => IsButtonEnabled);

            }
        }

        /// <summary>折れ線グラフの座標</summary>
        public Point GraphPoint { get; set; }

        /// <summary>折れ線グラフの初期化</summary>
        public bool IsGraphInitialize { get; set; } = false;

        /// <summary>折れ線グラフの最小座標</summary>
        public Point MinPoint { get; set; } = new Point(0d, 0.5);

        /// <summary>折れ線グラフの最大座標</summary>
        public Point MaxPoint { get; set; } = new Point(1d, 1d);

        /// <summary>更新内容</summary>
        public string Message { get; set; }

        /// <summary>メッセージボックス表示内容</summary>
        public MessageBoxInfo MessageInfo { get; set; }

        #endregion

        #region model

        /// <summary>MNISTのデータセットを機械学習する.Model</summary>
        private Model _Model;

        #endregion

        #region instance

        /// <summary>MNISTのデータセットを機械学習する.ViewModel</summary>
        public Training()
        {

            _Model = new Model(
                UpdateButtonEnabled,
                InitializeGraph,
                UpdateGraphPoint,
                UpdateMessage,
                ShowMessageBox);

        }

        #endregion

        #region method

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            _Model.Dispose();
            _Model = null;

        }

        /// <summary>機械学習ボタン有効化切替</summary>
        /// <param name="value">
        /// true :有効化
        /// false:無効化
        /// </param>
        private void UpdateButtonEnabled(bool value)
        {

            IsButtonEnabled = value;
            CallPropertyChanged(nameof(IsButtonEnabled));

        }

        /// <summary>グラフ初期化</summary>
        private void InitializeGraph()
        {

            IsGraphInitialize = !IsGraphInitialize;
            CallPropertyChanged(nameof(IsGraphInitialize));

        }

        /// <summary>折れ線グラフの座標を更新</summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        private void UpdateGraphPoint(double x, double y)
        {

            GraphPoint = new Point(x, y);
            CallPropertyChanged(nameof(GraphPoint));

        }

        /// <summary>更新内容の更新</summary>
        /// <param name="counter">更新内容</param>
        private void UpdateMessage(string message)
        {

            Message = message;
            CallPropertyChanged(nameof(Message));

        }

        /// <summary>メッセージボックスを表示</summary>
        /// <param name="info">メッセージボックス表示内容</param>
        private void ShowMessageBox(MessageBoxInfo info)
        {

            MessageInfo = info;
            CallPropertyChanged(nameof(MessageInfo));

        }

        #endregion

    }

}
