using log4net;
using SE_GeometryObserver.ViewModels;
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
using System.Windows.Shapes;

namespace SE_GeometryObserver.Views
{
    /// <summary>
    /// Логика взаимодействия для GeometryViewerView.xaml
    /// </summary>
    public partial class GeometryViewerView : Window
    {

        private readonly GeometryViewerViewModel dataContext;

        public GeometryViewerView(GeometryViewerViewModel geometryViewerViewModel, RevitBridge.RevitBridge revitBridge)
        {
            dataContext = geometryViewerViewModel;
            DataContext = dataContext;
            revitBridge.SetRevitAsAddinWindowOwner(this);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Closing += dataContext.OnWindowClosing;
            InitializeComponent();
        }


        //public void CloseWindow()
        //{
        //    Closing -= dataContext.OnWindowClosing;
        //    Close();
        //}

    }
}
