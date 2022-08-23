using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManageTreeDemo.UserControls.ViewModel;
using ManageTreeDemo.Model;

namespace ManageTreeDemo.UserControls
{
    /// <summary>
    /// NodeDetails.xaml 的交互逻辑
    /// </summary>
    public partial class NodeDetails : UserControl
    {
        public string test { get; set; }

        public NodeDetails(Node _node)
        {
            InitializeComponent();
            this.DataContext = new NodeDetailsVM(_node);
        }
    }
}
