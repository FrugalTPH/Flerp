using Flerp.Properties;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Flerp.DomainModel
{
    public class Stub : IFlerpEntity, IBinderContent, IBinderable, ILibraryable, INotifyPropertyChanged
    {
        public static Stub Create(Binder binder)
        {
            var n = new Stub
            {
                Id = new FlerpId(binder, BinderContentType.S).ToString(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Privacy = PrivacyType.Default,
                Category = Resources.Category_Null
            };
            
            Controller.RegisterNew(n);
            return n;
        }

        private static Stub CreateCopy(Stub source, Binder binder)
        {
            var n = new Stub
            {
                Id = new FlerpId(binder, BinderContentType.S).ToString(),
                ActionableDate = source.ActionableDate,
                Category = source.Category,
                ClosingDate = source.ClosingDate,
                ClosingRemark = source.ClosingRemark,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Name = source.Name,
                Privacy = source.Privacy
            };

            Controller.RegisterNew(n);
            return n;
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

        private string _category;
        public string Category { get { return _category; } set { SetProperty(ref _category, value); } }

        private string _name;
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }

        private string _reference;
        public string Reference { get { return _reference; } set { SetProperty(ref _reference, value); } }

        private DateTime? _actionableDate;
        public DateTime? ActionableDate { get { return _actionableDate; } set { SetProperty(ref _actionableDate, value); } }

        private DateTime? _closingDate;
        public DateTime? ClosingDate 
        { 
            get { return _closingDate; } 
            set
            {
                if (string.IsNullOrWhiteSpace(ClosingRemark)) _closingRemark = null;

                if (!ActionableDate.HasValue || ActionableDate.Value > value) value = ActionableDate.Value;

                SetProperty(ref _closingDate, value);
            }
        }

        private string _closingRemark;
        public string ClosingRemark 
        { 
            get { return _closingRemark; } 
            set 
            {
                if (!ClosingDate.HasValue) _closingDate = DateTime.Now;
                SetProperty(ref _closingRemark, value);
            } 
        }
        
        private DateTime _createdDate;
        public DateTime CreatedDate { get { return _createdDate; } set { SetProperty(ref _createdDate, value); } }

        private DateTime _modifiedDate;
        public DateTime ModifiedDate { get { return _modifiedDate; } set { SetProperty(ref _modifiedDate, value); } }

        private DateTime _disposedDate;
        public DateTime DisposedDate { get { return _disposedDate; } set { SetProperty(ref _disposedDate, value); } }

        public bool? IsComplete
        { 
            get 
            {
                if (!ActionableDate.HasValue || ActionableDate.Value > DateTime.Now) return null;
                return ActionableDate.Value > DateTime.Now || (ClosingDate.HasValue && ClosingDate.Value <= DateTime.Now);
            } 
        }


        public void CascadeUpdateBinder()
        {
            var b = Controller.GetEntityById<Binder>(IdF.BinderId);
            if (b != null) b.Save();
        }

        public void Delete()
        {
            throw new InvalidOperationException(Resources.Msg_StubsCannotBeDeleted);
        }

        public void IdDoubleClick() 
        { 
            // Do nothing. 
        }

        public void Save()
        {
            ModifiedDate = DateTime.Now;
            CascadeUpdateBinder();
            Controller.Update(this);
        }
        
        public void ToBinder(Binder binder, CancellationTokenSource cts)
        {
            CreateCopy(this, binder);
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