using Flerp.Properties;
using MongoDB.Bson;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Flerp.DomainModel
{
    public abstract class RelationBase : IFlerpEntity, INotifyPropertyChanged
    {
        protected RelationBase()
        {
            Id = new FlerpId(ObjectId.GenerateNewId().ToString()).ToString();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Privacy = PrivacyType.Default;
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

        private string _name;
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }

        private string _majorId;
        public string MajorId { get { return _majorId; } set { SetProperty(ref _majorId, value); } }

        private string _minorId;
        public string MinorId { get { return _minorId; } set { SetProperty(ref _minorId, value); } }

        private PrivacyType _privacy;
        public PrivacyType Privacy { get { return _privacy; } set { SetProperty(ref _privacy, value); } }

        private DateTime _creationDate;
        public DateTime CreatedDate { get { return _creationDate; } set { SetProperty(ref _creationDate, value); } }

        private DateTime _modifiedDate;
        public DateTime ModifiedDate { get { return _modifiedDate; } set { SetProperty(ref _modifiedDate, value); } }

        private DateTime _disposedDate;
        public DateTime DisposedDate { get { return _disposedDate; } set { SetProperty(ref _disposedDate, value); } }


        public void Save()
        {
            ModifiedDate = DateTime.Now;
            Controller.Update(this);
        }

        public void Delete()
        {
            var dialogResult = MessageBox.Show(Resources.UiMsg_OkToProceed, Resources.UiLabel_DeleteRelation, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dialogResult != DialogResult.OK) return;

            DisposedDate = DateTime.Now;
            Controller.Collection.Remove(this);
            Controller.Update(this);
        }

        public void IdDoubleClick()
        {
            // RelationBase Id's are non-human readable mongo ObjectId's. Currently rendered in the GUI as a red-cross "delete" button,
            // which deletes the entity (i.e. relationship) when double clicked. 
            Delete();
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