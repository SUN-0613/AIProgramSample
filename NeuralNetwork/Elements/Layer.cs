using System;
using System.Collections.Generic;

namespace NeuralNetwork.Elements
{

    /// <summary>層</summary>
    public class Layer : IDisposable
    {

        #region property

        /// <summary>本層に属するノード一覧</summary>
        public List<Node> Nodes { get; set; } = new List<Node>();

        #endregion

        #region instance

        /// <summary>層</summary>
        /// <param name="nodesCount">本層に属するノードの数</param>
        public Layer(int nodesCount)
        {

            for (var iLoop = 0; iLoop < nodesCount; iLoop++)
            {
                Nodes.Add(new Node());
            }

        }

        #endregion

        #region method

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            Nodes.Clear();
            Nodes = null;

        }

        /// <summary>全ノードに指定した出力層を結合</summary>
        /// <param name="rightLayer">出力側の層</param>
        public void ConnectDensely(Layer rightLayer)
        {

            Nodes.ForEach(
                (node) =>
                {

                    rightLayer.Nodes.ForEach(
                        (nextNode) =>
                        {
                            node.Connect(nextNode);
                        });

                });

        }

        /// <summary>全ノードの重みを初期化</summary>
        public void InitializeWeight()
        {
            Nodes.ForEach((node) => node.InitializeWeight());
        }

        /// <summary>全ノードにデータ入力</summary>
        /// <param name="inputs">入力値一覧</param>
        public void SetInputData(IReadOnlyList<double> inputs)
        {

            for (var iLoop = 0; iLoop < Nodes.Count; iLoop++)
            {

                if (iLoop < inputs.Count)
                {
                    Nodes[iLoop].OutputValue = inputs[iLoop];
                }
                else
                {
                    Nodes[iLoop].OutputValue = 0d;
                }

            }

        }

        /// <summary>全ノードの出力値の計算</summary>
        public void CalcOutputValue()
        {
            Nodes.ForEach((node) => { node.CalcOutputValue(); });
        }

        /// <summary>全ノードの重み更新</summary>
        /// <param name="alpha">学習率</param>
        public void UpdateWeight(double alpha)
        {
            Nodes.ForEach((node) => { node.UpdateWeight(alpha); });
        }

        #endregion

    }

}
