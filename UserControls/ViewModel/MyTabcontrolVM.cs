using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageTreeDemo.Common;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ManageTreeDemo.UserControls.ViewModel
{
    public class MyTabcontrolVM:NotifyPropertyBase
    {
        /// <summary>
        /// 选项卡集合
        /// </summary>
        public ObservableCollection<testitem> TabItems { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MyTabcontrolVM()
        {
            TabItems = new ObservableCollection<testitem>();
            testitem tab1 = new testitem();
            tab1.title = "test1";
            tab1.testcontent = "aaaaaaaaa";
            TabItems.Add(tab1);
            testitem tab2 = new testitem();
            tab2.title = "test2";
            tab2.testcontent = "bbbbbbbbbbb";
            TabItems.Add(tab2);
            testitem tab3 = new testitem();
            tab3.title = "test2";
            tab3.testcontent = "cccccccccccccc";
            TabItems.Add(tab3);
            TabItems.Add(tab3);
            TabItems.Add(tab3);
            TabItems.Add(tab3);
            TabItems.Add(tab3);
            TabItems.Add(tab3);
            TabItems.Add(tab3);
            //TabItems = new ObservableCollection<TabItem>();
            //TabItem tab2 = new TabItem();
            //tab2.Header = "test2";
            //TabItems.Add(tab2);
            //TabItem tab3 = new TabItem();
            //tab3.Header = "test3";
            //TabItems.Add(tab3);
        }

        /// <summary>
        /// 初始化测试数据
        /// </summary>
        private void testinit()
        {
            //TabItem tab2 = new TabItem();
            //tab1.Header = "test2";
            //TabItems.Add(tab2);
            //TabItem tab3 = new TabItem();
            //tab1.Header = "test3";
            //TabItems.Add(tab3);
        }

        public class testitem
        { 
            public string title { get; set; }
            public string testcontent { get; set; }
        }
    }
}
