using Network = NeuralNetwork.NeuralNetwork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using WpfLibrary.Windows;

namespace MachineLearning.Forms.Models
{

    /// <summary>マウスによる手書き文字を認識.Model</summary>
    public class Detect : IDisposable
    {

        #region property

        /// <summary>画像のピクセルデータ一覧</summary>
        public List<double> Pixels = new List<double>();

        /// <summary>正解データ一覧</summary>
        public List<string> LabelNames = new List<string>();

        #endregion

        #region global variable

        /// <summary>ニューラルネットワーク</summary>
        private readonly Network _NeuralNetwork = new Network();

        #endregion

        #region instance

        /// <summary>マウスによる手書き文字を認識.Model</summary>
        public Detect()
        {

            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Initialize();
            }

        }

        #endregion

        #region method

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            Pixels.Clear();

            _NeuralNetwork.Dispose();

        }

        /// <summary>初期化</summary>
        private void Initialize()
        {

            InitializePixels();

            _NeuralNetwork.LoadParameter();
            LoadLabelNames();

        }

        /// <summary>Pixel一覧を初期化</summary>
        public void InitializePixels()
        {

            Pixels.Clear();

            for (var iLoop = 0; iLoop < DataLengths.InputNodesLength; iLoop++)
            {
                Pixels.Add(0d);
            }

        }

        /// <summary>正解データ一覧を読み込み</summary>
        private void LoadLabelNames()
        {

            LabelNames.Clear();

            using (var reader = new StreamReader(FilePaths.LabelTextFilePath))
            {

                foreach (var name in reader.ReadLine().Split(','))
                {
                    LabelNames.Add(name);
                }

            }

        }

        /// <summary>画像認識を実行</summary>
        /// <returns>画像認識結果の文字</returns>
        public string ImageRecognition()
        {

            _NeuralNetwork.CalcOutputValue(Pixels);

            return LabelNames[_NeuralNetwork.GetMaxOutput()];

        }

        /// <summary>Canvasに描画した内容を画像にするための画像ファイル情報を取得</summary>
        /// <returns>画像ファイル情報</returns>
        public CreateBmpFileInfo GetBmpFileInfo()
        {

            return new CreateBmpFileInfo(FilePaths.FreeHandFilePath)
            {
                PixelWidth = 28,
                PixelHeight = 28,
            };

        }

        /// <summary>作成した画像からピクセル情報を取得</summary>
        public void SetPixels()
        {

            if (File.Exists(FilePaths.FreeHandFilePath))
            {

                using (var bitmap = new Bitmap(FilePaths.FreeHandFilePath))
                {

                    for (var iLoop = 0; iLoop < bitmap.Width; iLoop++)
                    {

                        for (var jLoop = 0; jLoop < bitmap.Height; jLoop++)
                        {

                            var color = bitmap.GetPixel(iLoop, jLoop);
                            var gray = (color.R + color.G + color.B) / 3;
                            var index = iLoop * DataLengths.PixelLength + jLoop;

                            Pixels[index] = !gray.Equals(byte.MaxValue)  ? 1d : 0d;

                        }

                    }

                }

            }

        }

        #endregion

    }

}
