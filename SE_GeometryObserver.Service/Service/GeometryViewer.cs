using Autodesk.Revit.DB;
using SE_ElementsSelector.Models;
using SE_GeometryObserver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Service
{
    public class GeometryViewer : IGeometryViewer
    {

        private readonly Document documentToInView;
        private readonly RevitBridge.RevitBridge revitBridge;
        private readonly SelectionModel selectionModel;

        Element currentViewElement;

        private Color RootElementColor { get; set; } = new Color((byte)0, (byte)255, (byte)0);

        private Color FileElementColor { get; set; } = new Color((byte)255, (byte)0, (byte)0);

        private readonly int colorLineWeght = 3;


        public GeometryViewer(RevitBridge.RevitBridge revitBridge, SelectionModel selectionModel)
        {
            this.documentToInView = revitBridge.Doc;
            this.revitBridge = revitBridge;
            this.selectionModel = selectionModel;
        }

        public async Task ClearGeometryCompareResultsOnViewAsync()
        {
            if (currentViewElement != null)
                documentToInView.Delete(currentViewElement.Id);
            await selectionModel.ClearSelection();
        }

        public async Task ViewGeometryCompareResult(IGeometryCompareResult geometryCompareResult, bool isIsolate)
        {
            await ClearGeometryCompareResultsOnViewAsync();

            List<GeometryModel> elements = new List<GeometryModel>();
            if (geometryCompareResult.CompareResult == GeomAndCoordsCompareResult.NotFoundEqual)
            {
                IGeometryElementModel model = geometryCompareResult.FileGeometryModel == null ?
                    geometryCompareResult.CurrentGeometryModel : geometryCompareResult.FileGeometryModel;
                Color displayColor = geometryCompareResult.FileGeometryModel == null ? RootElementColor : FileElementColor;
                elements.Add(new GeometryModel()
                {
                    Name = model.ElementName,
                    Geometry = model.GeometryElement,
                    SelectColor = displayColor

                });
            }
            else if (geometryCompareResult.CompareResult
                == GeomAndCoordsCompareResult.CoordsEqualButGeometryNot ||
                geometryCompareResult.CompareResult
                == GeomAndCoordsCompareResult.GeometryEqualButCoordsNot ||
                geometryCompareResult.CompareResult == GeomAndCoordsCompareResult.Match)
            {
                elements.AddRange(new List<GeometryModel>{
                    new GeometryModel()
                    {
                        Name = geometryCompareResult.CurrentGeometryModel.ElementName,
                        Geometry = geometryCompareResult.CurrentGeometryModel.GeometryElement,
                        SelectColor = RootElementColor
                    },
                    new GeometryModel()
                    {
                        Name = geometryCompareResult.FileGeometryModel.ElementName,
                        Geometry = geometryCompareResult.FileGeometryModel.GeometryElement,
                        SelectColor = FileElementColor
                    }
                });
            }
            await selectionModel.SelectGeometriesOnView(elements, isIsolate, true);
        }
    }
}
