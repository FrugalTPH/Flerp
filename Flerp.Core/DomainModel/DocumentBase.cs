using Flerp.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Flerp.DomainModel
{
    public abstract class DocumentBase : IFlerpEntity, IBinderContent, IBinderable, INotifyPropertyChanged
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
                return Equals(_privacy, PrivacyType.Default) 
                    ? _privacy.GetDefault(this) 
                    : _privacy;
            }
            set
            {
                SetProperty(ref _privacy, value);
            }
        }

        private string _aliasId;
        public string AliasId { get { return _aliasId; } set { SetProperty(ref _aliasId, value); } }

        private string _category;
        public string Category { get { return _category; } set { SetProperty(ref _category, value); } }
        
        private string _name;
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }

        private LcStatusType _status;
        public LcStatusType Status { get { return _status; } protected set { SetProperty(ref _status, value); } }

        private string _source;
        public string Source { get { return _source; } protected set { SetProperty(ref _source, value); } }
        
        private int _views;
        public int Views { get { return _views; } set { SetProperty(ref _views, value); } }

        private DateTime _createdDate;
        public DateTime CreatedDate { get { return _createdDate; } set { SetProperty(ref _createdDate, value); } }

        private DateTime _modifiedDate;
        public DateTime ModifiedDate { get { return _modifiedDate; } set { SetProperty(ref _modifiedDate, value); } }

        private DateTime _disposedDate;
        public DateTime DisposedDate { get { return _disposedDate; } set { SetProperty(ref _disposedDate, value); } }

        private string _masterExtension;
        public string MasterExtension { get { return _masterExtension; } set { SetProperty(ref _masterExtension, value); } }

        private string _masterHash;
        public string MasterHash { get { return _masterHash; } set { SetProperty(ref _masterHash, value); } }


        public bool IsCancellable { get { return Equals(Status, LcStatusType.Released); } }

        public virtual bool IsConvertibleToDirectory { get { return false; } }

        public virtual bool IsConvertibleToFile { get { return false; } }

        public virtual bool IsCopyable { get { return false; } }

        public bool IsOnCloud { get { return true; } }
       
        public bool IsReleasable { get { return Equals(Status, LcStatusType.Pending); } }
                
        public virtual bool IsRevisable { get { return false; } }


        public FsEntity GetFsEntity()
        {
            FsEntity entity;

            if (!TryGetFsEntity(out entity))
                throw new InvalidOperationException(string.Format("Could not locate {0}.", Id));

            return entity;
        }
        
        protected bool TryGetFsEntity(out FsEntity fsEntity)
        {
            var docDir = new DirectoryInfo(FsEntity.GetDocDir(Id));

            var files = docDir
                .GetFiles(Id + Resources.Wildcard_Any, SearchOption.TopDirectoryOnly)
                .Select(f => new FsFile(f.FullName, this) as FsEntity);

            var dirs = docDir
                .GetDirectories(Id + Resources.Wildcard_Any, SearchOption.TopDirectoryOnly)
                .Select(d => new FsDirectory(d.FullName, this));

            fsEntity = files.Concat(dirs).FirstOrDefault(x => x.Extension == MasterExtension);

            return fsEntity != null;
        }

        protected BindingList<DocumentBase> Revisions
        {
            get
            {
                var revs = Controller.Collection
                    .OfType<DocumentBase>()
                    .Where(x => x.Id.Contains(FlerpId.Parse(Id).BinderAndDocumentId))
                    .OrderByDescending(x => x.Id);

                return new BindingList<DocumentBase>(revs.ToList());
            }
        }

        internal void Abort()
        {
            var dbEntity = Controller.Repository.GetById(Id);
            if (dbEntity != null) Controller.Repository.Delete(dbEntity);

            DocumentBase cacheEntity;
            if (!Controller.TryGetEntityById(Id, out cacheEntity)) return;
        }

        public void Cancel()
        {
            var finished = false;

            try
            {
                Status = LcStatusType.Cancelled;
                finished = true;
            }
            finally
            {
                if (!finished) Status = LcStatusType.Released;
                Save();
            }
        }

        public void CascadeUpdateBinder()
        {
            var b = Controller.GetEntityById<Binder>(IdF.BinderId);
            if (b != null) b.Save();
        }

        public void Delete()
        {
            throw new InvalidOperationException("Documents and Emails cannot be deleted.");
        }

        private void Read()
        {
            try
            {
                var master = GetFsEntity();

                if (Equals(Status, LcStatusType.Pending)) master.OpenWrite();
                else master.OpenRead();
            }
            catch
            {
                Controller.Logger.Info("Could not open requested document / email.");
            }
            
        }

        public void Release(CancellationTokenSource token, bool rehash)
        {
            var finished = false;
            DocumentBase prevRelease = null;

            try
            {
                var master = GetFsEntity();
                if (rehash) MasterHash = master.ComputeHash(token);

                prevRelease = Revisions.FirstOrDefault(x => x.Status.Equals(LcStatusType.Released));
                if (prevRelease != null) prevRelease.Cancel();
                Status = LcStatusType.Released;

                finished = true;
            }
            finally
            {
                if (!finished)
                {
                    Status = LcStatusType.Pending;
                    if (prevRelease != null) 
                    {
                        prevRelease.Status = LcStatusType.Released;
                        prevRelease.Save();
                    }
                }
                Save();
            }
        }

        public void Save()
        {
            ModifiedDate = DateTime.Now;
            CascadeUpdateBinder();
            Controller.Update(this);
        }

        public abstract void ToBinder(Binder target, CancellationTokenSource cts);

        public void IdDoubleClick()
        {
            Read();
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