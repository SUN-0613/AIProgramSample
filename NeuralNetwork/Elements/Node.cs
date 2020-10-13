using System;
using System.Collections.Generic;

namespace NeuralNetwork.Elements
{

    /// <summary>データ判定を行うノード</summary>
    /// <remarks>ニューロンともいう</remarks>
    public class Node : IDisposable
    {

        #region property

        /// <summary>入力エッジ</summary>
        public List<Edge> Inputs = new List<Edge>();

        /// <summary>出力エッジ</summary>
        public List<Edge> Outputs = new List<Edge>();

        /// <summary>入力値の合計</summary>
        public double InputTotalValue;

        /// <summary>出力値</summary>
        public double OutputValue;

        #endregion

        #region method

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            Inputs.Clear();
            Inputs = null;

            Outputs.Clear();
            Outputs = null;

        }

        /// <summary>活性化関数</summary>
        /// <param name="value">入力値</param>
        /// <returns>シグモイド関数</returns>
        public double Activation(double value)
        {
            return 1d / (1d + Math.Exp((-1d) * value));
        }

        /// <summary>出力値の計算</summary>
        public void CalcOutputValue()
        {

            if (!Inputs.Count.Equals(0))
            {

                // 初期化
                InputTotalValue = 0d;

                // 入力値の合計を計算
                Inputs.ForEach(
                    (edge) =>
                    {
                        InputTotalValue += edge.Left.OutputValue * edge.Weight;
                    });

                // 活性化関数で出力値を求める
                OutputValue = Activation(InputTotalValue);

            }

        }

        #endregion

    }

}
