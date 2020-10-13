using NeuralNetwork.Elements;
using System;
using System.Collections.Generic;

namespace NeuralNetwork
{

    /// <summary>ニューラルネットワーク</summary>
    public class NeuralNetwork : IDisposable
    {

        #region property

        /// <summary>層一覧</summary>
        public List<Layer> Layers = new List<Layer>();

        #endregion

        #region method

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            Layers.ForEach((layer) => layer.Dispose());
            Layers.Clear();
            Layers = null;

        }

        #endregion

    }

}
