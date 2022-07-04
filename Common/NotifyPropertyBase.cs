using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;
using ManageTreeDemo.Common;

namespace ManageTreeDemo.Common
{
    #region  INotifyPropertyChanged
    /// <summary>
    /// 
    /// </summary>
    public class NotifyPropertyBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性更改通知
        /// </summary>
        /// <param name="propertyName"></param>
        #region INotifyPropertyChanged
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    /// <summary>
    /// 扩展方法
    /// 避免硬编码问题
    /// </summary>
    public static class NotifyPropertyBaseEx
    {
        /// <summary>
        /// 设置通知属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="tvm"></param>
        /// <param name="expre"></param>
        public static void SetProperty<T, U>(this T tvm, Expression<Func<T, U>> expre) where T : NotifyPropertyBase, new()
        {
            string _pro = CommonFun.GetPropertyName(expre);
            tvm.OnPropertyChanged(_pro);
        }
    }
    #endregion


}
