using System.ComponentModel;

namespace NotifierAPI
{
    /// <summary>
    /// Класс-уведомитель об изменениях. При наследовании от него добавляет поддержку интерфейса INotifyPropertyChanged без необходимости явной реализации.
    /// </summary>
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}