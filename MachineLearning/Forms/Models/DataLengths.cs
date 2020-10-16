namespace MachineLearning.Forms.Models
{

    /// <summary>各種データ数を管理するクラス</summary>
    public class DataLengths
    {

        /// <summary>使用する画像のピクセル数</summary>
        public const int PixelLength = 28;

        /// <summary>入力層のノード数</summary>
        public const int InputNodesLength = PixelLength * PixelLength;

        /// <summary>出力層のノード数</summary>
        public const int OutputNodesLength = 10;

        /// <summary>隠れ層のノード数</summary>
        public const int HiddenNodesLength = OutputNodesLength * 10;

        /// <summary>学習用文字画像データ</summary>
        public const int TrainingData = 50000;

        /// <summary>検証用文字画像データ</summary>
        public const int TestData = 10000;

        /// <summary>文字画像データ総数</summary>
        public const int TotalData = TrainingData + TestData;

    }

}
