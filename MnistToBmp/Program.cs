using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace MnistToBmp
{

    /// <summary>MNISTからダウンロードしたtrain-images-idx3-ubyteから画像を生成する</summary>
    class Program
    {

        /// <summary>処理開始</summary>
        /// <param name="args">引数</param>
        /// <remarks>
        /// MNISTは下記URL
        /// http://yann.lecun.com/exdb/mnist/
        /// </remarks>
        static void Main(string[] args)
        {

            Console.WriteLine("START!");

            const string idx3FilePath = @"..\..\..\..\MNIST\train-images-idx3-ubyte\train-images.idx3-ubyte";

            var dataSize = 28;
            var dataLength = dataSize * dataSize;

            var pixel = new byte[dataLength];
            var bitmap = new Bitmap(dataSize, dataSize);
            var directory = Path.GetDirectoryName(idx3FilePath);

            directory = Path.Combine(directory, "BMP");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(idx3FilePath, FileMode.Open))
            {

                using (var reader = new BinaryReader(stream))
                {

                    // ヘッダを読み飛ばし
                    for (var iLoop = 0; iLoop < 4; iLoop++)
                    {
                        reader.ReadInt32();
                    }

                    // Pixelデータを読み込んでBMP形式で保存
                    for (var iLoop = 0; iLoop < 60000; iLoop++)
                    {

                        // 出力するBitmapファイル名
                        var pictureFilePath = Path.Combine(directory, "image" + iLoop.ToString() + ".bmp");

                        for (var jLoop = 0; jLoop < dataLength; jLoop++)
                        {
                            pixel[jLoop] = reader.ReadByte();
                        }

                        for (var y = 0; y < dataSize; y++)
                        {
                            for (var x = 0; x < dataSize; x++)
                            {
                                bitmap.SetPixel(x, y, GetColor(pixel, x, y, dataSize));
                            }
                        }

                        bitmap.Save(pictureFilePath, ImageFormat.Bmp);
                        
                        Console.Write("Output:" + Path.GetFileName(pictureFilePath));
                        Console.SetCursorPosition(0, Console.CursorTop);

                    }

                }

            }

            Console.WriteLine("");
            Console.WriteLine("Finish!");
            Console.ReadKey();

        }

        /// <summary>指定PixelのColorを取得</summary>
        /// <param name="pixel">Pixelデータ</param>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="size">ピクセルサイズ</param>
        /// <returns>Color</returns>
        private static Color GetColor(byte[] pixel, int x, int y, int size)
        {

            var value = (int)pixel[y * size + x];

            return Color.FromArgb(value, value, value);

        }

    }

}
