using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autodesk.Revit.UI;
using BindingCommands;
using log4net;
using NotifierAPI;
using SE_ElementsSelector.Models;
using SE_GeometryObserver.Entities;
using SE_GeometryObserver.Interfaces;
using SE_GeometryObserver.Service;
using SE_GeometryObserver.Service.Service;

namespace SE_GeometryObserver.ViewModels
{
    public class GeometryViewerViewModel: Notifier
    {
        private RevitBridge.RevitBridge revitBridge;
        private ILog logger;

        public Action RefreshViewsVisibilityAction { get; set; } 


        private string currentDocName;
        public string CurrentDocName
        {
            get => currentDocName;
            set
            {
                currentDocName = value;
                NotifyPropertyChanged(nameof(CurrentDocName));
            }
        }

        private string fileDocName;
        public string FileDocName
        {
            get => fileDocName;
            set
            {
                fileDocName = value;
                NotifyPropertyChanged(nameof(FileDocName));
            }
        }

        private string categoriesDesccription;
        public string CategoriesDesccription
        {
            get => categoriesDesccription;
            set
            {
                categoriesDesccription = value;
                NotifyPropertyChanged(nameof(CategoriesDesccription));
            }
        }

       


        public BindingList<GeometryCompareResult> GeometryCompareResults { get; set; }
        private IEnumerable<IGeometryCompareResult> selectedItems;

        private GeometryViewer geometryViewer;

        private bool isIsolateIntersection = true;
        public bool IsIsolateIntersection
        {
            get => isIsolateIntersection;
            set
            {
                isIsolateIntersection = value;
                NotifyPropertyChanged(nameof(IsIsolateIntersection));
#warning Пока версия только для первого выбранного элемента.
                if (selectedItems != null && selectedItems.Count() > 0)
                    geometryViewer.ViewGeometryCompareResult(selectedItems.First(), IsIsolateIntersection);
            }
        }

        private readonly CommonCompareResults compareResults;

        public GeometryViewerViewModel(RevitBridge.RevitBridge revitBridge, 
            SelectionModel selectionModel, ILog logger, CommonCompareResults compareResults)

        {
            this.compareResults = compareResults;
            CurrentDocName = compareResults.DocumentName;
            FileDocName = compareResults.FileDocumentName;
            CategoriesDesccription = compareResults.CategoriesDescription;
            this.revitBridge = revitBridge;
            this.logger = logger;
            GeometryCompareResults = new BindingList<GeometryCompareResult>(compareResults.CompareResults);
            geometryViewer = new GeometryViewer(revitBridge, selectionModel);
        }


        public RelayCommand SelectItemsCommand => new RelayCommand(obj =>
        {
            try
            {
                IList items = (IList)obj;
                selectedItems = items.Cast<IGeometryCompareResult>();

#warning Пока версия только для первого выбранного элемента.
                geometryViewer.ViewGeometryCompareResult(selectedItems.First(), IsIsolateIntersection);

            }
            catch (Exception e)
            {
                logger?.Error(e);
                TaskDialog.Show("Error", e.Message);
            }
        });


        public RelayCommand SaveCompareResultsCommand => new RelayCommand(obj =>
        {
            throw new NotImplementedException();
        }, _ => false);


        public void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            geometryViewer.ClearGeometryCompareResultsOnViewAsync();
            if (RefreshViewsVisibilityAction != null)
                RefreshViewsVisibilityAction.Invoke();
        }

    }
}
