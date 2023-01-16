using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BindingCommands;
using log4net;
using NotifierAPI;
using Autodesk.Revit.DB;
using System.Windows.Forms;
using SE_GeometryObserver.Views;
using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;
using SE_ElementsSelector.Models;
using SE_GeometryObserver.Entities.EqualityComparers;
using SE_GeometryObserver.Service;
using SE_GeometryObserver.Service.Service;

namespace SE_GeometryObserver.ViewModels
{
    public class GeometryLoaderViewModel: Notifier
    {
        RevitBridge.RevitBridge revitBridge;
        ILog logger;
        readonly IGeometryComparer geometryComparer;
        private GeometryViewerViewModel geometryViewerViewModel;

        private string comparisonFilePath;
        public string ComparisonFilePath
        {
            get => comparisonFilePath;
            set
            {
                comparisonFilePath = value;
                NotifyPropertyChanged(nameof(ComparisonFilePath));
            }
        }

        public Action<bool> SetViewVisibilityAction { get; set; }


        public BindingList<CategoryModel> SelectableCategories { get; set; }

        private IList<IGeometryCompareResult> currentGeometryCompareResults { get; set; }

        SelectionModel selectionModel;
        public GeometryLoaderViewModel(RevitBridge.RevitBridge revitBridge, 
            SelectionModel selectionModel, IGeometryComparer geometryComparer, ILog logger)
        {
            this.revitBridge = revitBridge;
            this.logger = logger;
            this.selectionModel = selectionModel;
            this.geometryComparer = geometryComparer;
            SetSelectableCategories(revitBridge);
        }


        private void SetSelectableCategories(RevitBridge.RevitBridge revitBridge)
        {
            //TODO добавить иребование открытого документа.     
            FilteredElementCollector collector = new FilteredElementCollector(revitBridge.Doc);
            IList<CategoryModel> categories = collector
                .WhereElementIsNotElementType()
                .ToElements()
                .Select(x => x.Category)
                .Where(x=>x != null)
                .Distinct(new CategoryComparer())
                .Select(x => new CategoryModel(x))
                .OrderBy(x=>x.Name)
                .ToList();

            SelectableCategories = new BindingList<CategoryModel>(categories);
        }


        public RelayCommand CompareGeometry => new RelayCommand(_ =>
        {
            //TODO Переместить логику в сервис
            try
            {
                //TODO проверка на пустой документ
                IEnumerable<BuiltInCategory> selectedCats = SelectableCategories
                .Where(x => x.IsSelected)
                .Select(x => (BuiltInCategory)x.Category.Id.IntegerValue);

                string catsDescription = string.Join(",", SelectableCategories
                .Where(x => x.IsSelected)
                .Select(x => x.Category.Name));
                

                using (Document document = revitBridge.App.OpenDocumentFile(ModelPathUtils.ConvertUserVisiblePathToModelPath(ComparisonFilePath),
                    new OpenOptions()))
                {
                    XYZ cursharedBasePos = revitBridge.GetDocumentProjectBasePoint(revitBridge.Doc).SharedPosition;
                    XYZ docSharedBasePos = revitBridge.GetDocumentProjectBasePoint(document).SharedPosition;
                    XYZ transFormVector =  docSharedBasePos - cursharedBasePos;
                    Transform translation = Transform.CreateTranslation(transFormVector);
                    
                    IGeometryLoader geometryLoader = new GeometryLoader(); //TODO посмотреть возможность DI.
                    IList<IGeometryElementModel> fileGeometryElements = geometryLoader.LoadGeometries(document, null, translation);
                    IList<IGeometryElementModel> curGeometryElements = geometryLoader.LoadGeometries(revitBridge.Doc, selectedCats);

                    currentGeometryCompareResults =  geometryComparer
                    .Compare(curGeometryElements, fileGeometryElements)
                    .ToList();


                    IList<GeometryCompareResult> resToView = currentGeometryCompareResults
                    .Where(x => x.CompareResult != GeomAndCoordsCompareResult.Match)
                    .Cast<GeometryCompareResult>()
                    .ToList();

                    CommonCompareResults results = new CommonCompareResults(revitBridge.Doc.PathName, 
                        document.PathName, 
                        SelectableCategories.Where(x => x.IsSelected).Select(x=>x.Category), 
                        resToView);
                    

                    if (resToView.Count > 0)
                    {

                        geometryViewerViewModel = new GeometryViewerViewModel(revitBridge, selectionModel, logger, results);
                        geometryViewerViewModel.RefreshViewsVisibilityAction = () =>
                        {
                            if (SetViewVisibilityAction != null)
                                SetViewVisibilityAction.Invoke(true);
                        };
                        ShowCurrentCompareResults.Execute(new object());
                    }
                    else
                    {
                        MessageBox.Show("Отличия в геометрии не найдены", 
                            "Сравнения геометрий", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                   
            }
            catch(Exception ex) //TODO более осмысленный лог.
            {
                logger?.Error(ex);
                MessageBox.Show(ex.Message, "Загрузка геометрии", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        },
            (obj) => ComparisonFilePath != null);


       

        public RelayCommand SelectComparisonFile => new RelayCommand(_ =>
        {
            System.Windows.Forms.OpenFileDialog choofdlog = new System.Windows.Forms.OpenFileDialog();
            choofdlog.Filter = "Revit Project(*.rvt)|*.rvt";
            choofdlog.Multiselect = false;
            if(choofdlog.ShowDialog() == DialogResult.OK)
            {
                ComparisonFilePath = choofdlog.FileName;
            }
        });


        public RelayCommand SetSelectAll => new RelayCommand(obj =>
        {
            try
            {
                bool isSelect = bool.Parse(obj as string);
                foreach (CategoryModel cM in SelectableCategories)
                    cM.IsSelected = isSelect;
            }
            catch(Exception ex)
            {
                logger?.Error(ex);
                MessageBox.Show(ex.Message, "Загрузка геометрии", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        });


        public RelayCommand ShowCurrentCompareResults => new RelayCommand(obj =>
        {
            GeometryViewerView view = new GeometryViewerView(geometryViewerViewModel, revitBridge);
            view.Show();
            if (SetViewVisibilityAction != null)
                SetViewVisibilityAction.Invoke(false);
        }, obj => geometryViewerViewModel != null);
    }
}
