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

        #endregion

    }

}
