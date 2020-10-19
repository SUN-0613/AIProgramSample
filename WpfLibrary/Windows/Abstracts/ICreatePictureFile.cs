using System.Windows.Controls;

namespace WpfLibrary.Windows.Abstracts
{
    public interface ICreatePictureFile
    {

        /// <summary>指定するCanvasの内容で画像ファイルを生成</summary>
        /// <param name="canvas">Canvas</param>
        void CreatePictureFile(Canvas canvas);

    }

}
