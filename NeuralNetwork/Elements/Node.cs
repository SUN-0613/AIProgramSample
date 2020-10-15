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
        public List<Edge> Inputs { get; set; } = new List<Edge>();

        /// <summary>出力エッジ</summary>
        public List<Edge> Outputs { get; set; } = new List<Edge>();

        /// <summary>入力値の合計</summary>
        public double InputTotalValue { get; set; }

        /// <summary>出力値</summary>
        public double OutputValue { get; set; }

        /// <summary>誤差</summary>
        public double Error { get; set; }

        #endregion

        #region global variable

        /// <summary>乱数補正値</summary>
        private const double _RandomAlpha = 0.1;

        /// <summary>乱数</summary>
        private static readonly Random _Random = new Random();

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

        /// <summary>活性化関数を微分</summary>
        /// <param name="value">入力値</param>
        /// <returns>微分した結果</returns>
        public double DActivation(double value)
        {
            return (1d - value) * value;
        }

        /// <summary>隣のノードと接続</summary>
        /// <param name="right">右側のノード</param>
        /// <returns>隣のノードと繋がるエッジ</returns>
        public Edge Connect(Node right)
        {

            var edge = new Edge()
            {
                Left = this,
                Right = right,
            };

            right.Inputs.Add(edge);
            Outputs.Add(edge);

            return edge;

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

        /// <summary>乱数を取得</summary>
        /// <returns>乱数の取得結果</returns>
        public static double GetRandom()
        {
            return Math.Sqrt(-2d * Math.Log(_Random.NextDouble())) * Math.Cos(2d * Math.PI * _Random.NextDouble()) * _RandomAlpha;
        }

        /// <summary>重みを初期化</summary>
        public void InitializeWeight()
        {
            Inputs.ForEach((edge) => edge.Weight = GetRandom());
        }

        /// <summary>出力層の誤差を計算</summary>
        /// <param name="value">正解データ</param>
        public void CalcError(double value)
        {
            Error = value - OutputValue;
        }

        /// <summary>隠れ層の誤差を計算</summary>
        /// <remarks>隠れ層 = 中間層</remarks>
        public void CalcError()
        {

            Error = 0d;
            Outputs.ForEach((edge) => Error += edge.Weight * edge.Right.Error);

        }

        /// <summary>重み更新</summary>
        /// <param name="alpha">学習率</param>
        public void UpdateWeight(double alpha)
        {

            Inputs.ForEach(
                (edge) => 
                {
                    edge.Weight += alpha * Error * DActivation(OutputValue) * edge.Left.OutputValue;
                });

        }

        #endregion

    }

}
