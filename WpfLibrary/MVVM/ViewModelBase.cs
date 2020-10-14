using System.ComponentModel;
using System.Diagnostics;

namespace WpfLibrary.MVVM
{

    /// <summary>ViewModelベースクラス</summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        #region event

        /// <summary>プロパティ変更イベントハンドラ</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region property

        /// <summary>プロパティ更新実行済FLG</summary>
        private bool _IsUpdatedProperties = false;

        /// <summary>プロパティ更新実行済FLG</summary>
        /// <remarks>
        /// true :更新済
        /// false:未更新
        /// </remarks>
        public bool IsUpdatedProperties
        {
            get => _IsUpdatedProperties;
            private set
            {
                _IsUpdatedProperties = value;
                CallPropertyChanged(false);
            }
        }

        #endregion

        #region global variable 

        /// <summary>プロパティ更新実行済FLGの更新を行うか</summary>
        private readonly bool _UpdateIsUpdatedProperties;

        #endregion

        #region instance

        /// <summary>ViewModelベースクラス</summary>
        public ViewModelBase()
        {
            _UpdateIsUpdatedProperties = false;
        }

        /// <summary>ViewModelベースクラス</summary>
        /// <param name="updateIsUpdatedProperties">プロパティ更新実行済FLGの更新を行うか</param>
        public ViewModelBase(bool updateIsUpdatedProperties)
        {
            _UpdateIsUpdatedProperties = updateIsUpdatedProperties;
        }

        #endregion

        #region method

        /// <summary>プロパティ更新実行済FLGのリセット</summary>
        protected void ResetIsUpdatedProperties()
        {

            if (_UpdateIsUpdatedProperties)
            {
                _IsUpdatedProperties = false;
            }

        }

        /// <summary>PropertyChanged()の実行</summary>
        protected virtual void CallPropertyChanged()
        {
            CallPropertyChanged(2, true);
        }

        /// <summary>PropertyChanged()の実行</summary>
        /// <param name="updateIsUpdatedProperties">プロパティ更新実行済FLGを更新するか</param>
        protected virtual void CallPropertyChanged(bool updateIsUpdatedProperties)
        {
            CallPropertyChanged(2, updateIsUpdatedProperties);
        }

        /// <summary>PropertyChanged()の実行</summary>
        /// <param name="stackFrameIndex">呼び出し元メソッドIndex</param>
        protected virtual void CallPropertyChanged(int stackFrameIndex)
        {
            CallPropertyChanged(2, true);
        }

        /// <summary>PropertyChanged()の実行</summary>
        /// <param name="stackFrameIndex">呼び出し元メソッドIndex</param>
        /// <param name="updateIsUpdatedProperties">プロパティ更新実行済FLGを更新するか</param>
        protected virtual void CallPropertyChanged(int stackFrameIndex, bool updateIsUpdatedProperties)
        {

            // 呼び出し元メソッド名をプロパティ名として取得
            var caller = new StackFrame(stackFrameIndex);
            var methodNames = caller.GetMethod().Name.Split('_');
            var propertyName = methodNames[methodNames.Length - 1];

            CallPropertyChanged(propertyName, updateIsUpdatedProperties);

        }

        /// <summary>PropertyChanged()の実行</summary>
        /// <param name="propertyName">イベントを発生させたいプロパティ名</param>
        protected virtual void CallPropertyChanged(string propertyName)
        {
            CallPropertyChanged(propertyName, true);
        }

        /// <summary>PropertyChanged()の実行</summary>
        /// <param name="propertyName">イベントを発生させたいプロパティ名</param>
        /// <param name="updateIsUpdatedProperties">プロパティ更新実行済FLGを更新するか</param>
        protected virtual void CallPropertyChanged(string propertyName, bool updateIsUpdatedProperties)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (_UpdateIsUpdatedProperties && updateIsUpdatedProperties)
            {
                IsUpdatedProperties = true;
            }

        }

        #endregion

    }

}
