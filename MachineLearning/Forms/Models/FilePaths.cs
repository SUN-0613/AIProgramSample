namespace MachineLearning.Forms.Models
{

    /// <summary>使用するファイルパス一覧</summary>
    public class FilePaths
    {

        /// <summary>画像イメージファイルパス</summary>
        public const string TrainImageFilePath = @"..\..\..\MNIST\train-images-idx3-ubyte\train-images.idx3-ubyte";

        /// <summary>ラベルファイルパス</summary>
        public const string TrainLabelFilePath = @"..\..\..\MNIST\train-labels-idx1-ubyte\train-labels.idx1-ubyte";

        /// <summary>正解ラベルファイルパス</summary>
        public const string LabelTextFilePath = @".\Label.txt";

        /// <summary>ニューラルネットワークの構成と全体の重みを保存したファイル</summary>
        public const string ParameterFilePath = @".\Weight.dat";

        /// <summary>フリーハンドで描画した画像ファイルパス</summary>
        public const string FreeHandFilePath = @".\FreeHand.bmp";

    }

}
