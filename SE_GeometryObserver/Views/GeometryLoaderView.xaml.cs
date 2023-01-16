using log4net;
using SE_ElementsSelector.Models;
using SE_GeometryObserver.Interfaces;
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
    /// Логика взаимодействия для GeometryLoader.xaml
    /// </summary>
    public partial class GeometryLoaderView : Window
    {
        public GeometryLoaderView(RevitBridge.RevitBridge revitBridge, IGeometryComparer geometryComparer, SelectionModel selectionModel, ILog logger)
        {
            DataContext = new GeometryLoaderViewModel(revitBridge,  selectionModel, geometryComparer, logger) 
            { SetViewVisibilityAction = (x) => this.Visibility = x? Visibility.Visible: Visibility.Hidden };
            InitializeComponent();
            revitBridge.SetRevitAsAddinWindowOwner(this);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
