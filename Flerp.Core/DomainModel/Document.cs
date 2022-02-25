using Flerp.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Flerp.DomainModel
{
    public class Document : DocumentBase, IBasketable, ILibraryable //, ILibraryDocument
    {
        public static Document CreateInputFromPath(Binder binder, FsEntity source, CancellationTokenSource token)
        {
            var n = new Document();

            try
            {
                n.Id = new FlerpId(binder, BinderContentType.N).ToString();
                n.CreatedDate = DateTime.Now;
                n.ModifiedDate = DateTime.Now;
                n.Status = LcStatusType.Pending;
                n.Source = source.Id;
                n.Name = Path.GetFileNameWithoutExtension(source.FullPath);
                n.Privacy = PrivacyType.Default;
                n.Category = Resources.Category_Null;

                n.MasterHash = source.ComputeHash(token);

                if (binder.Contains(BinderContentType.N, n.MasterHash)) throw new InvalidOperationException("Input already exists in this Binder.");

                var master = source.ToMaster(n, false, token);
                if (master != null) n.MasterExtension = master.Extension;

                Controller.RegisterNew(n);
                n.Release(token, false);
                return n;
            }
            finally
            {
                if (!Equals(n.Status, LcStatusType.Released)) n.Abort();
            }
        }

        private static void CreateInputFromDocument(Binder binder, Document document, CancellationTokenSource token)
        {
            var n = new Document();

            try
            {
                var source = document.GetFsEntity();

                n.Id = new FlerpId(binder, BinderContentType.N).ToString();
                n.CreatedDate = DateTime.Now;
                n.ModifiedDate = DateTime.Now;
                n.Status = LcStatusType.Pending;
                n.Source = source.Id;
                n.Name = document.Name;
                n.Category = document.Category;
                n.Privacy = PrivacyType.Default;
                n.MasterHash = source.ComputeHash(token);
                if (binder.Contains(BinderContentType.N, n.MasterHash)) throw new InvalidOperationException("Input already exists in this Binder.");

                var master = source.ToMaster(n, false, token);
                if (master != null) n.MasterExtension = master.Extension;

                Controller.RegisterNew(n);
                n.Release(token, false);
            }
            finally
            {
                if (!Equals(n.Status, LcStatusType.Released)) n.Abort();
            }
        }
        
        public static Document CreateInputFromEmail(Binder binder, Email email, CancellationTokenSource token)
        {
            var n = new Document();

            try
            {
                var source = email.GetFsEntity();

                var message = email.ExtractMessageOnly(token);

                n.Id = new FlerpId(binder, BinderContentType.N).ToString();
                n.CreatedDate = email.CreatedDate;
                n.ModifiedDate = email.ModifiedDate;
                n.Status = LcStatusType.Pending;
                n.Source = source.Id;
                n.Name = email.Name;
                n.Category = Resources.Category_Email;
                n.Privacy = PrivacyType.Default;
                n.MasterHash = message.ComputeHash(token);
                if (binder.Contains(BinderContentType.N, n.MasterHash)) throw new InvalidOperationException("Input already exists in this Binder.");

                var master = message.ToMaster(n, false, token);
                if (master != null) n.MasterExtension = master.Extension;

                Controller.RegisterNew(n);
                n.Release(token, false);
                return n;
            }
            finally
            {
                if (!Equals(n.Status, LcStatusType.Released)) n.Abort();
            }
        }
        
        public static List<Document> CreateInputsFromEmailAttachments(Binder binder, Email email, CancellationTokenSource token)
        {
            var outputs = new List<Document>();

            try
            {
                var eml = email.GetFsEntity();

                var attachments = email.ExtractAttachments(token);

                foreach (var attachment in attachments)
                {
                    token.Token.ThrowIfCancellationRequested();

                    var n = new Document();

                    try
                    {
                        n.Id = new FlerpId(binder, BinderContentType.N).ToString();
                        n.CreatedDate = email.CreatedDate;
                        n.ModifiedDate = email.ModifiedDate;
                        n.Status = LcStatusType.Pending;
                        n.Source = eml.Id;
                        n.Name = attachment.Key;
                        n.Privacy = PrivacyType.Default;
                        n.MasterHash = attachment.Value.ComputeHash(token);
                        n.Category = Resources.Category_Null;

                        if (binder.Contains(BinderContentType.N, n.MasterHash)) throw new InvalidOperationException("Input already exists in this Binder.");

                        var master = attachment.Value.ToMaster(n, false, token);
                        if (master != null) n.MasterExtension = master.Extension;

                        Controller.RegisterNew(n);
                        n.Release(token, false);
                        outputs.Add(n);
                    }
                    catch
                    {
                        n.Abort();
                    }
                }
                return outputs;
            }
            catch
            {
                foreach (var doc in outputs)
                {
                    doc.Abort();
                }
                throw;
            }
        }
        
        public static Document CreateOutputFromDocument(Binder binder, Document document, CancellationTokenSource token)
        {
            var n = new Document();

            try
            {
                var source = document.GetFsEntity();

                n.Id = new FlerpId(binder, BinderContentType.X).ToString();
                n.CreatedDate = DateTime.Now;
                n.ModifiedDate = DateTime.Now;
                n.Status = LcStatusType.Pending;
                n.Source = source.Id;
                n.Name = document.Name;
                n.Category = document.Category;
                n.Privacy = PrivacyType.Default;

                var master = source.ToMaster(n, false, token);
                if (master != null) n.MasterExtension = master.Extension;

                Controller.RegisterNew(n);
                return n;
            }
            finally
            {
                IPersistable entity;
                if (!Controller.TryGetEntityById(n.Id, out entity)) n.Abort();
            }            
        }
        
        public static Document CreateOutputRevision(Document document, CancellationTokenSource token)
        {
            var n = new Document();
            if (!document.IsRevisable) throw new InvalidOperationException("Source document is not revisable.");

            try
            {
                var source = document.GetFsEntity();

                n.Id = new FlerpId(document).ToString();
                n.CreatedDate = DateTime.Now;
                n.ModifiedDate = DateTime.Now;
                n.Status = LcStatusType.Pending;
                n.Source = source.Id;
                n.Name = document.Name;
                n.Category = document.Category;
                n.Privacy = PrivacyType.Default;

                var master = source.ToMaster(n, false, token);
                if (master != null) n.MasterExtension = master.Extension;

                Controller.RegisterNew(n);
                return n;
            }
            finally
            {
                IPersistable entity;
                if (!Controller.TryGetEntityById(n.Id, out entity)) n.Abort();
            }
        }

        public static Document CreateOutputFromEmail(Binder binder, Email email, CancellationTokenSource token)
        {
            var n = new Document();

            try
            {
                var source = email.GetFsEntity();

                var message = email.ExtractMessageOnly(token);

                n.Id = new FlerpId(binder, BinderContentType.X).ToString();
                n.CreatedDate = email.CreatedDate;
                n.ModifiedDate = email.ModifiedDate;
                n.Status = LcStatusType.Pending;
                n.Source = source.Id;
                n.Name = email.Name;
                n.Category = Resources.Category_Email;
                n.Privacy = PrivacyType.Default;
                n.MasterHash = message.ComputeHash(token);
                if (binder.Contains(BinderContentType.X, n.MasterHash)) throw new InvalidOperationException("Output already exists in this Binder.");

                var master = message.ToMaster(n, false, token);
                if (master != null) n.MasterExtension = master.Extension;

                Controller.RegisterNew(n);
                n.Release(token, false);
                return n;
            }
            finally
            {
                if (!Equals(n.Status, LcStatusType.Released)) n.Abort();
            }
        }

        public static Document CreateOutputFromNothing(Binder binder, CancellationTokenSource token)
        {
            var n = new Document();

            try
            {
                n.Id = new FlerpId(binder, BinderContentType.X).ToString();
                n.CreatedDate = DateTime.Now;
                n.ModifiedDate = DateTime.Now;
                n.Status = LcStatusType.Pending;
                n.Name = Resources.Name_Unauditable;
                n.Privacy = PrivacyType.Default;
                n.Category = Resources.Category_Null;

                var path = FsEntity.GetDocDir(n.Id) + Path.DirectorySeparatorChar + n.Id;
                Directory.CreateDirectory(path);

                var master = FsEntity.Create(path);
                if (master != null) n.MasterExtension = string.Empty;

                Controller.RegisterNew(n);
                return n;
            }
            finally
            {
                IPersistable entity;
                if (!Controller.TryGetEntityById(n.Id, out entity)) n.Abort();
            } 
        }

        
        private string _privateNotes;
        public string PrivateNotes { get { return _privateNotes; } set { SetProperty(ref _privateNotes, value); } }

        public override bool IsConvertibleToDirectory 
        { 
            get 
            {
                FsEntity master;
                TryGetFsEntity(out master);

                return master is FsFile && !Directory.Exists(FsEntity.GetDocDir(Id) + FsEntity.GetFlerpFilename(Id, null, string.Empty));  
            } 
        }

        public override bool IsConvertibleToFile
        {
            get
            {
                FsEntity master;
                TryGetFsEntity(out master);

                if (!(master is FsDirectory)) return false;

                var fileCount = Directory.GetFiles(master.FullPath, Resources.Wildcard_Any, SearchOption.TopDirectoryOnly)
                    .Count(x => !x.EndsWith(Resources.FileExtension_Flerp));

                var dirCount = Directory.GetDirectories(master.FullPath, Resources.Wildcard_Any, SearchOption.TopDirectoryOnly)
                    .Length;

                return fileCount == 1 && dirCount == 0;
            }
        }

        public override bool IsCopyable { get { return true; } }

        public override bool IsRevisable
        {
            get
            {
                return IdF.DocumentType == BinderContentType.X.ToString()[0] && Revisions.All(x => !Equals(x.Status, LcStatusType.Pending));
            }
        }


        public void ToBasket(CancellationTokenSource cts)
        {
            try
            {
                if (!IsOnCloud) { throw new InvalidOperationException(string.Format("'{0}' does not exist on the cloud.", Id)); }
                if (Controller.Basket.Contains(this)) throw new InvalidOperationException(string.Format("'{0}' is already in the basket.", Id));
                if (Controller.Basket.Any(x => x is Transmittal)) throw new InvalidOperationException(string.Format("'{0}' cannot be added to the basket in transmittal mode.", Id));

                Controller.Basket.Add(this);
                Views++;
                Save();
            }
            catch (InvalidOperationException ex)
            {
                Controller.Logger.Info(ex.Message);
            }
        }

        public override void ToBinder(Binder target, CancellationTokenSource cts)
        {
            CreateInputFromDocument(target, this, cts);
            Views++;
            Save();
        }

        public void ToDirectory()
        {
            if (!IsConvertibleToDirectory) throw new InvalidOperationException(string.Format("'{0}' cannot be converted - the master must be an appropriate file.", Id));

            var master = (FsFile) GetFsEntity();
            master.ToDirectory();
            Save();
        }

        public void ToFile()
        {
            if (!IsConvertibleToFile) throw new InvalidOperationException(string.Format("'{0}' cannot be converted - the master must be an appropriate directory.", Id));

            var master = (FsDirectory)GetFsEntity();
            master.ToFile();
            Save();
        }
    
    }
}