using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Flerp.DomainModel
{
    public abstract class FsEntity : INotifyPropertyChanged
    {
        public static FsEntity Create(string path) 
        {
            if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory) return new FsDirectory(path);
            return new FsFile(path);
        }


        public string Id { get { return ToString(); } }

        private string _fullPath;
        public string FullPath { get { return _fullPath; } protected set { SetProperty(ref _fullPath, value); } }

        private string _parentId;
        protected string ParentId { get { return _parentId; } set { SetProperty(ref _parentId, value); } }

        protected DocumentBase Parent { get { return Controller.GetEntityById <DocumentBase>(ParentId); } }

        private string _fsHash;
        public string FsHash 
        { 
            get 
            {
                if (_fsHash == null) FsHash = ComputeHash(new CancellationTokenSource());
                return _fsHash; 
            }
            set { SetProperty(ref _fsHash, value); } }

        public string Extension { get { return FsInfo.Extension; } }

        protected abstract FileSystemInfo FsInfo { get; }


        public abstract string ComputeHash(CancellationTokenSource token);

        public abstract void Delete();

        private static string GetUserDir()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                Path.DirectorySeparatorChar +
                "FLERP";

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        public static string GetDataDir()
        {
            var dir = GetUserDir() +
                Path.DirectorySeparatorChar +
                "DATA";

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        public static string GetDocDir(string id)
        {
            var fId = FlerpId.Parse(id);
            if (fId == FlerpId.Empty) throw new ArgumentException("Not a valid FlerpId.");

            var dir = GetUserDir() +
                Path.DirectorySeparatorChar +
                GetFsLabels()[fId.BinderType] +
                Path.DirectorySeparatorChar +
                fId.BinderId +
                Path.DirectorySeparatorChar +
                GetFsLabels()[fId.DocumentType];

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        public static string GetFlerpFilename(string id, string sourcePath, string extension = null)
        {
            FlerpId.Parse(id);

            var s = Path.GetExtension(sourcePath);
            if (s != null) extension = extension == null ? s.ToLowerInvariant() : extension.ToLowerInvariant();

            return Path.DirectorySeparatorChar + id + extension;
        }

        private static Dictionary<char, string> GetFsLabels()
        {
            return new Dictionary<char, string>
                {
                    {'A', "ADMIN" },
                    {'E', "EMAIL" },
                    {'L', "LIBRARY" },
                    {'W', "WORK" },
                    {'H', "HUMAN" },
                    {'O', "ORGANISATION" },
                    {'N', "INPUT" },
                    {'X', "OUTPUT" },
                    {'S', "STUB" }
                };
        }

        public static string GetGuidFilename(string extension)
        {
            return Path.DirectorySeparatorChar +
                Guid.NewGuid().ToString() +
                extension.ToLowerInvariant();
        }

        public static string GetTempDir()
        {
            var dir = GetUserDir() +
                Path.DirectorySeparatorChar +
                "TEMP";

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        public void OpenRead()
        {
            try
            {
                SetReadOnly();

                Process.Start(FullPath);
                Parent.Views++;
                Parent.Save();
            }
            catch
            {
                Controller.Logger.Info("WARNING: Could not open (read-only) '" + FullPath + "'.");
            }
        }

        public void OpenWrite()
        {
            try
            {
                SetReadWrite();

                Process.Start(FullPath);
                Parent.Views++;
                Parent.Save();
            }
            catch
            {
                Controller.Logger.Info("WARNING: Could not open (read-write) '" + FullPath + "'.");
            }
        }

        protected abstract void SetReadOnly();

        protected abstract void SetReadWrite();

        public abstract FsEntity ToMaster(IPersistable entity, bool removeOriginal, CancellationTokenSource token);

        public override string ToString() { return Path.DirectorySeparatorChar + FsInfo.Name; }
        
        #region INotifyPropertyChanged
        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value) || propertyName == null) return;

            storage = value;
            OnPropertyChanged(propertyName);
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}