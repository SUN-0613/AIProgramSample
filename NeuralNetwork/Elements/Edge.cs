namespace NeuralNetwork.Elements
{

    /// <summary>前後の層のノードを繋ぐエッジ</summary>
    public class Edge
    {

        #region property

        /// <summary>入力側のノード</summary>
        public Node Left { get; set; }

        /// <summary>出力側のノード</summary>
        public Node Right { get; set; }

        /// <summary>重み</summary>
        public double Weight { get; set; }

        #endregion

    }

}
