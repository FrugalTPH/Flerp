using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Flerp.DomainModel
{
    // Partially deprecated - now only handles view layout save/restores. These need migrating to use ConfigManager.AppSettings, and this class deleted.
    public class AppSetting : IPersistable, INotifyPropertyChanged
    {
        public AppSetting(string key, string value)
        {
            Id = key;
            Value = value;
        }

        private string _id;
        public string Id { get { return _id; } set { SetProperty(ref _id, value); } }

        private FlerpId _idF;
        public FlerpId IdF
        {
            get
            {
                if (_idF == default(FlerpId)) _idF = FlerpId.Parse(_id);
                return _idF;
            }
        }

        private string _value;
        public string Value { get { return _value; } set { SetProperty(ref _value, value); } }
                

        public void Save()
        {
            Controller.AppSettings.Save(this);
        }

        public void IdDoubleClick() 
        { 
            throw new System.NotImplementedException(); 
        }

        #region INotifyPropertyChanged

        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value) || propertyName == null) return;

            storage = value;
            OnPropertyChanged(propertyName);
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler == null) return;
            eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}