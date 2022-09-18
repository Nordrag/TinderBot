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

namespace TinderBotGUI.MVVM.View
{
    /// <summary>
    /// Interaction logic for BadooVIew.xaml
    /// </summary>
    public partial class BadooView : UserControl
    {
        public BadooView()
        {
            InitializeComponent();
            DataContext = MainWindow.viewModel;
        }
    }
}
