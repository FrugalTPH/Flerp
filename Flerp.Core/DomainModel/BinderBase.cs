using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Flerp.DomainModel
{
    public abstract class BinderBase : IFlerpEntity, INotifyPropertyChanged
    {
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

        private PrivacyType _privacy;
        public PrivacyType Privacy 
        {
            get
            {
                return Equals(_privacy, PrivacyType.Default) ? _privacy.GetDefault(this) : _privacy;
            }
            set
            {
                SetProperty(ref _privacy, value);
            }
        }

        private string _name;
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }

        private DateTime _modifiedDate;
        public DateTime ModifiedDate { get { return _modifiedDate; } set { SetProperty(ref _modifiedDate, value); } }

        private DateTime _createdDate;
        public DateTime CreatedDate { get { return _createdDate; } set { SetProperty(ref _createdDate, value); } }

        private DateTime _disposedDate;
        public DateTime DisposedDate { get { return _disposedDate; } set { SetProperty(ref _disposedDate, value); } }

        private string _description;
        public string Description { get { return _description; } set { SetProperty(ref _description, value); } }


        public void Save()
        {
            ModifiedDate = DateTime.Now;
            Controller.Update(this);
        }

        public void Delete() 
        { 
            throw new InvalidOperationException("Binders cannot be deleted."); 
        }

        public void IdDoubleClick()
        {
            Show();
        }

        private void Show()
        {
            Controller.MainView.Show_BinderOrPartyView(this);
        }

        #region INotifyPropertyChanged
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
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