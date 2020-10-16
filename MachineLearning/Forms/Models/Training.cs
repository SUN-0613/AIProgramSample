using Network = NeuralNetwork.NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using WpfLibrary.Windows;

namespace MachineLearning.Forms.Models
{

    /// <summary>MNISTのデータセットを機械学習する.Model</summary>
    public class Training : IDisposable
    {

        #region property

        /// <summary>学習率</summary>
        public double Alpha = 0.1;

        /// <summary>画像のピクセルデータ</summary>
        public readonly List<List<double>> Pixels = new List<List<double>>();

        /// <summary>出力層の誤差計算用の正解データ</summary>
        public readonly List<List<double>> Labels = new List<List<double>>();

        /// <summary>正解データ</summary>
        public readonly List<int> LabelIndexes = new List<int>();

        /// <summary>グラフ座標</summary>
        public Point Pixel;

        #endregion

        #region global variable

        /// <summary>ニューラルネットワーク</summary>
        private readonly Network _NeuralNetwork = new Network();

        /// <summary>機械学習ボタン有効化切替デリゲート</summary>
        /// <param name="value">
        /// true :有効化
        /// false:無効化
        /// </param>
        public delegate void UpdateButtonEnabledDelegate(bool value);

        /// <summary>機械学習ボタン有効化切替メソッド</summary>
        private readonly UpdateButtonEnabledDelegate _UpdateButtonEnabledMethod;

        /// <summary>グラフ初期化デリゲート</summary>
        public delegate void InitializeGraphDelegate();

        /// <summary>グラフ初期化メソッド</summary>
        private readonly InitializeGraphDelegate _InitializeGraphMethod;

        /// <summary>折れ線グラフの座標を更新するデリゲート</summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public delegate void UpdateGraphPointDelegate(double x, double y);

        /// <summary>折れ線グラフの座標を更新するメソッド</summary>
        private readonly UpdateGraphPointDelegate _UpdateGraphPointMethod;

        /// <summary>更新内容の更新デリゲート</summary>
        /// <param name="counter">更新内容</param>
        public delegate void UpdateMessageDelegate(string message);

        /// <summary>更新内容の更新メソッド</summary>
        private readonly UpdateMessageDelegate _UpdateMessageMethod;

        /// <summary>メッセージボックスを表示するデリゲート</summary>
        /// <param name="info">メッセージボックス表示内容</param>
        public delegate MessageBoxResult ShowMessageBoxDelegate(MessageBoxInfo info);

        /// <summary>メッセージボックスを表示するメソッド</summary>
        private readonly ShowMessageBoxDelegate _ShowMessageBoxMethod;

        #endregion

        #region instance

        /// <summary>MNISTのデータセットを機械学習する.Model</summary>
        /// <param name="updateButtonEnabled">機械学習ボタン有効化切替メソッド</param>
        /// <param name="initializeGraph">グラフ初期化メソッド</param>
        /// <param name="updateGraphPoint">折れ線グラフの座標を更新するメソッド</param>
        /// <param name="updateMessage">更新内容の更新メソッド</param>
        /// <param name="showMessageBox">メッセージボックスを表示するメソッド</param>
        public Training(
            UpdateButtonEnabledDelegate updateButtonEnabled,
            InitializeGraphDelegate initializeGraph,
            UpdateGraphPointDelegate updateGraphPoint,
            UpdateMessageDelegate updateMessage,
            ShowMessageBoxDelegate showMessageBox)
        {

            _UpdateButtonEnabledMethod = updateButtonEnabled;
            _InitializeGraphMethod = initializeGraph;
            _UpdateGraphPointMethod = updateGraphPoint;
            _UpdateMessageMethod = updateMessage;
            _ShowMessageBoxMethod = showMessageBox;

            Initialize();

        }

        #endregion

        #region method

        /// <summary>初期化</summary>
        private void Initialize()
        {

            InitializeList();

            // 入力層、隠れ層、出力層の3つをニューラルネットワークに追加
            _NeuralNetwork.AddLayer(DataLengths.InputNodesLength);
            _NeuralNetwork.AddLayer(DataLengths.HiddenNodesLength);
            _NeuralNetwork.AddLayer(DataLengths.OutputNodesLength);

        }

        /// <summary>List初期化</summary>
        private void InitializeList()
        {

            Pixels.ForEach((pixels) => pixels.Clear());
            Pixels.Clear();

            for (var iLoop = 0; iLoop < DataLengths.TotalData; iLoop++)
            {

                Pixels.Add(new List<double>());

                for (var jLoop = 0; jLoop < DataLengths.InputNodesLength; jLoop++)
                {
                    Pixels[iLoop].Add(0d);
                }

            }

            LabelIndexes.Clear();

            Labels.ForEach((labels) => labels.Clear());
            Labels.Clear();

            for (var iLoop = 0; iLoop < DataLengths.TotalData; iLoop++)
            {

                LabelIndexes.Add(0);
                Labels.Add(new List<double>());

                for (var jLoop = 0; jLoop < DataLengths.OutputNodesLength; jLoop++)
                {
                    Labels[iLoop].Add(0);
                }

            }

        }

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            Pixels.ForEach(
                (pixels) => 
                { 
                    pixels.Clear();
                    pixels = null;
                });
            Pixels.Clear();

            LabelIndexes.Clear();

            Labels.ForEach(
                (labels) =>
                {
                    labels.Clear();
                    labels = null;
                });
            Labels.Clear();

            _NeuralNetwork.Dispose();

        }

        /// <summary>機械学習の開始</summary>
        public async void BeginTrainAsync()
        {

            if (_UpdateButtonEnabledMethod == null)
            {
                return;
            }

            await Task.Run(() => 
            {

                _UpdateButtonEnabledMethod.Invoke(false);

                try
                {

                    // 初期化
                    _InitializeGraphMethod.Invoke();
                    LoadData();
                    _NeuralNetwork.InitializeWeight();

                    // 機械学習開始
                    for (var iLoop = 0; iLoop < DataLengths.TrainingData; iLoop++)
                    {

                        _NeuralNetwork.CalcOutputValue(Pixels[iLoop]);
                        _NeuralNetwork.CalcError(Labels[iLoop]);
                        _NeuralNetwork.UpdateWeight(Alpha);

                        if (iLoop > 0 && iLoop % 100 == 0)
                        {
                            BeginTest(100, iLoop);
                            _UpdateMessageMethod.Invoke(iLoop.ToString());
                        }

                    }

                    _UpdateMessageMethod.Invoke("Testing...");

                    var score = BeginTest(DataLengths.TestData, DataLengths.TrainingData);
                    _UpdateMessageMethod.Invoke((score * 100d).ToString("F2") + "%");

                    // メッセージボックスを表示
                    var info = new MessageBoxInfo()
                    {
                        Message = "学習結果を保存しますか？",
                        Title = "Save",
                        Button = MessageBoxButton.YesNo,
                        Image = MessageBoxImage.Question,
                        DefaultResult = MessageBoxResult.Yes,
                    };

                    info.Result = _ShowMessageBoxMethod.Invoke(info);

                    while (info.Result.Equals(MessageBoxResult.None))
                    {
                        Task.Delay(1);
                    }

                    if (info.Result.Equals(MessageBoxResult.Yes))
                    {

                        _NeuralNetwork.SaveParameter();
                        SaveLabels(new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    _UpdateButtonEnabledMethod.Invoke(true);
                }

            });

        }

        /// <summary>MNISTのファイルを読み込む</summary>
        private void LoadData()
        {

            InitializeList();

            using (var stream = new FileStream(FilePaths.TrainImageFilePath, FileMode.Open))
            {

                using (var reader = new BinaryReader(stream))
                {

                    // ヘッダを読み飛ばし
                    for (var iLoop = 0; iLoop < 4; iLoop++)
                    {
                        reader.ReadInt32();
                    }

                    // 手書きデータ読み込み
                    for (var iLoop = 0; iLoop < Pixels.Count; iLoop++)
                    {
                        for (var jLoop = 0; jLoop < Pixels[iLoop].Count; jLoop++)
                        {
                            Pixels[iLoop][jLoop] = (double)reader.ReadByte() / 255d * 0.99 + 0.01;
                        }
                    }

                }

            }

            using (var stream = new FileStream(FilePaths.TrainLabelFilePath, FileMode.Open))
            {

                using (var reader = new BinaryReader(stream))
                {

                    // ヘッダを読み飛ばし
                    for (var iLoop = 0; iLoop < 2; iLoop++)
                    {
                        reader.ReadInt32();
                    }

                    // 正解データ読み込み
                    for (var iLoop = 0; iLoop < Labels.Count; iLoop++)
                    {

                        for (var jLoop = 0; jLoop < Labels[iLoop].Count; jLoop++)
                        {
                            Labels[iLoop][jLoop] = 0.01;
                        }

                        LabelIndexes[iLoop] = reader.ReadByte();
                        Labels[iLoop][LabelIndexes[iLoop]] = 0.99;

                    }

                }

            }

        }

        /// <summary>精度の検証開始</summary>
        /// <param name="length">テストデータ数</param>
        /// <param name="pointX">折れ線グラフのX座標</param>
        /// <returns>検証結果</returns>
        private double BeginTest(int length, int pointX)
        {

            var okCount = 0;
            var offset = DataLengths.TrainingData;
            var lastData = offset + length;

            for (var iLoop = 0; iLoop < length; iLoop++)
            {

                _NeuralNetwork.CalcOutputValue(Pixels[offset + iLoop]);

                if (_NeuralNetwork.GetMaxOutput().Equals(LabelIndexes[offset + iLoop]))
                {
                    okCount++;
                }

            }

            var score = (double)okCount / (double)length;

            _UpdateGraphPointMethod.Invoke((double)pointX / (double)DataLengths.TrainingData, score);

            return score;

        }

        /// <summary>正解の種類をファイルに保存</summary>
        /// <param name="labels">ラベル名一覧</param>
        private void SaveLabels(List<int> labels)
        {

            using (var writer = new StreamWriter(FilePaths.LabelTextFilePath))
            {

                for (var iLoop = 0; iLoop < labels.Count; iLoop++)
                {

                    if (iLoop > 0)
                    {
                        writer.Write(",");
                    }

                    writer.Write(labels[iLoop].ToString());

                }

            }

        }

        #endregion

    }

}
