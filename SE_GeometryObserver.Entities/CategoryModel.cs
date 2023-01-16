using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using NotifierAPI;

namespace SE_GeometryObserver.Entities
{
    public class CategoryModel: Notifier
    {
        public CategoryModel(Category category)
        {
            Name = category.Name;
            Category = category;
        }

        public string Name { get; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                NotifyPropertyChanged(nameof(IsSelected));
            }
        }

        public Category Category { get; }
    }
}
