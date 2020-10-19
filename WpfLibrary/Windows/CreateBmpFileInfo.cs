using System.Windows.Media.Imaging;
using WpfLibrary.Windows.Abstracts;

namespace WpfLibrary.Windows
{

    /// <summary>CanvasからBitmapファイルを作成するためのデータ情報</summary>
    public class CreateBmpFileInfo : CreatePictureFileBase
    {

        #region instance

        /// <summary>CanvasからBitmapファイルを作成するためのデータ情報</summary>
        /// <param name="bmpFilePath">生成するBitmapファイルのパス</param>
        public CreateBmpFileInfo(string filePath) : base(filePath)
        { }

        #endregion

        #region method

        /// <summary>Bitmapファイルを作成するエンコーダを取得</summary>
        protected override BitmapEncoder GetEncorder()
        {
            return new BmpBitmapEncoder();
        }

        #endregion

    }

}
