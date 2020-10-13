using NeuralNetwork.Elements;
using System;
using System.Collections.Generic;
using System.IO;

namespace NeuralNetwork
{

    /// <summary>ニューラルネットワーク</summary>
    public class NeuralNetwork : IDisposable
    {

        #region property

        /// <summary>層一覧</summary>
        public List<Layer> Layers { get; set; } = new List<Layer>();

        #endregion

        #region global variable

        /// <summary>ニューラルネットワークの構成と全体の重みを保存したファイル</summary>
        private const string _ParameterFilePath = @".\Weight.dat";

        #endregion

        #region method

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            Layers.ForEach((layer) => layer.Dispose());
            Layers.Clear();
            Layers = null;

        }

        /// <summary>層を作成し隣接した層と全接合させる</summary>
        /// <param name="numberOfNodes">ノードの数を指定</param>
        public void AddLayer(int numberOfNodes)
        {

            var layer = new Layer(numberOfNodes);

            if (!Layers.Count.Equals(0))
            {
                Layers[Layers.Count - 1].ConnectDensely(layer);
            }

            Layers.Add(layer);

        }

        /// <summary>入力層にデータを入力したあとに出力層に向けて計算を行う</summary>
        /// <param name="inputs">入力するデータ一覧</param>
        public void CalcOutputValue(double[] inputs)
        {

            if (!Layers.Count.Equals(0))
            {

                // 入力層にデータを入力
                Layers[0].SetInputData(inputs);

                // 隣接する層に向けてデータを出力
                Layers.ForEach((layer) => layer.CalcOutputValue());

            }

        }

        /// <summary>出力層から認識結果を取得</summary>
        /// <returns>最大値を持つノードのIndex</returns>
        public int GetMaxOutput()
        {

            var maxIndex = 0;
            var maxValue = 0d;

            // 出力層のノードの中から最大値を検索
            for (var iLoop = 0; iLoop < Layers[Layers.Count - 1].Nodes.Count; iLoop++)
            {

                var value = Layers[Layers.Count - 1].Nodes[iLoop].OutputValue;

                if (value > maxValue)
                {
                    maxIndex = iLoop;
                    maxValue = value;
                }

            }

            return maxIndex;

        }

        /// <summary>ニューラルネットワーク全体の重みを初期化</summary>
        public void InitializeWeight()
        {
            Layers.ForEach((layer) => layer.InitializeWeight());
        }

        /// <summary>ニューラルネットワーク全体の重みを更新</summary>
        /// <param name="alpha">学習率</param>
        public void UpdateWeight(double alpha)
        {
            Layers.ForEach((layer) => layer.UpdateWeight(alpha));
        }

        /// <summary>出力層から遡って誤差を計算</summary>
        /// <param name="trainData">教師データ</param>
        public void CalcError(double[] trainData)
        {

            if (!Layers.Count.Equals(0))
            {

                var maxIndex = Layers.Count - 1;

                // 出力層のノードの数を取得
                var outputNodesCount = Layers[maxIndex].Nodes.Count;

                // 出力層の各ノードの誤差を計算
                for (var iLoop = 0; iLoop < outputNodesCount; iLoop++)
                {

                    if (iLoop < trainData.Length)
                    {
                        Layers[maxIndex].Nodes[iLoop].CalcError(trainData[iLoop]);
                    }
                    
                }

                // 隠れ層のノードの誤差を計算
                for (var iLoop = maxIndex - 1; iLoop >= 0; iLoop--)
                {
                    Layers[iLoop].Nodes.ForEach((node) => node.CalcError());
                }

            }

        }

        /// <summary>ニューラルネットワークの構成と全体の重みをファイルに保存</summary>
        public void SaveParameter()
        {

            using (var stream = new FileStream(_ParameterFilePath, FileMode.OpenOrCreate))
            {

                using (var writer = new BinaryWriter(stream))
                {

                    // 層の全体数
                    writer.Write(Layers.Count);

                    // 各層のノード数
                    Layers.ForEach((layer) => writer.Write(layer.Nodes.Count));

                    // 重み
                    Layers.ForEach(
                        (layer) => 
                        {
                            layer.Nodes.ForEach(
                                (node) =>
                                {
                                    node.Inputs.ForEach(
                                        (edge) => 
                                        {
                                            writer.Write(edge.Weight);
                                        });
                                });
                        });

                }

            }

        }

        /// <summary>ニューラルネットワークの構成と全体の重みをファイルから読み込み設定する</summary>
        public void LoadParameter()
        {

            using (var stream = new FileStream(_ParameterFilePath, FileMode.Open))
            {

                using (var reader = new BinaryReader(stream))
                {

                    // 初期化
                    if (Layers != null)
                    {

                        if (!Layers.Count.Equals(0))
                        {
                            Layers.ForEach((layer) => layer.Dispose());
                        }

                        Layers.Clear();
                        Layers = null;

                    }

                    Layers = new List<Layer>();

                    // 層数
                    var layerCount = reader.ReadInt32();

                    // 各層のノード数に従いノードを生成
                    for (var iLoop = 0; iLoop < layerCount; iLoop++)
                    {
                        AddLayer(reader.ReadInt32());
                    }

                    // 重み
                    Layers.ForEach(
                        (layer) => 
                        {
                            layer.Nodes.ForEach(
                                (node) => 
                                {
                                    node.Inputs.ForEach(
                                        (edge) =>
                                        {
                                            edge.Weight = reader.ReadDouble();
                                        });
                                });
                        });

                }

            }

        }

        #endregion

    }

}
