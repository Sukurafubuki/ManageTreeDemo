using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ManageTreeDemo.Common;
using ManageTreeDemo.Model;

namespace ManageTreeDemo.ViewModel
{
    public class MainTreeMenuVm: NotifyPropertyBase
    {
        #region
        /// <summary>
        /// 命令要执行的方法
        /// </summary>
        void UpdateNameExecute(object sender)
        {
            
        }

        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <returns></returns>
        bool CanUpdateNameExecute()
        {
            return true;
        }
        /// <summary>
        /// 创建新命令
        /// </summary>
        public ICommand ClickAction
        {
            get
            {
                return new RelayCommand<object>(UpdateNameExecute, CanUpdateNameExecute);
            }
        }
        #endregion

        #region 树菜单数据实体
        /// <summary>
        /// 视图树数据实体
        /// </summary>
        public ObservableCollection<MenuItem> MennuItems
        {
            get;
            set;
        }
        #endregion
    }
}
