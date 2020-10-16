using Network = NeuralNetwork.NeuralNetwork;
using System;
using System.Collections.Generic;

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

            Initialize();

        }

        #endregion

        #region method

        /// <summary>初期化</summary>
        private void Initialize()
        {

            InitializeList();

            _NeuralNetwork.LoadParameter();
            LoadLabelNames();

        }

        /// <summary>List初期化</summary>
        private void InitializeList()
        {

            Pixels.Clear();

            for (var iLoop = 0; iLoop < DataLengths.InputNodesLength; iLoop++)
            {
                Pixels.Add(0d);
            }

            LabelNames.Clear();

        }

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            Pixels.Clear();

            _NeuralNetwork.Dispose();

        }

        /// <summary>正解データ一覧を読み込み</summary>
        private void LoadLabelNames()
        {



        }

        #endregion

    }

}
