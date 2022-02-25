using Flerp.Commands;
using Flerp.Properties;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Flerp.DomainModel
{
    public class Email : DocumentBase
    {
        public static Email Create(FsEntity sourceEntity, bool removeOriginal, bool checkDuplicates, CancellationTokenSource token)
        {
            var n = new Email();

            try
            {
                var message = MimeMessage.Load(sourceEntity.FullPath, token.Token);

                Email existing;
                if (TryGetByMessageId(message.MessageId, out existing))
                    throw new InvalidOperationException(sourceEntity.Id + " clashed with " + existing.Id);

                var binder = GetEmailBinderByDate(message.Date.DateTime);

                var type = EmailAccount.GetAll().Any(x => message.From.ToString().Contains(x.UserName))
                    ? BinderContentType.X
                    : BinderContentType.N;

                n.Id = new FlerpId(binder, type).ToString();
                n.CreatedDate = message.Date.DateTime;
                n.ModifiedDate = DateTime.Now;
                n.Source = message.From.ToString();
                n.Name = message.Subject;
                n.Status = LcStatusType.Pending;
                n.Attachments = message.Attachments.Count();
                n.Bcc = message.Bcc.ToString();
                n.Category = Resources.Category_Email;
                n.Cc = message.Cc.ToString();
                n.To = message.To.ToString();
                n.IsIgnorable = false;
                n.Privacy = PrivacyType.Default;
                n.MasterHash = message.MessageId;

                var master = sourceEntity.ToMaster(n, removeOriginal, token);
                if (master != null) n.MasterExtension = master.Extension;

                Controller.RegisterNew(n);
                n.Save();
                return n;
            }
            finally
            {
                if (n.MasterExtension == null) n.Abort();
            }
        }


        private string _to;
        public string To { get { return _to; } set { SetProperty(ref _to, value); } }

        private int _attachments;
        public int Attachments { get { return _attachments; } set { SetProperty(ref _attachments, value); } }

        private string _cc;
        public string Cc { get { return _cc; } set { SetProperty(ref _cc, value); } }

        private string _bcc;
        public string Bcc { get { return _bcc; } set { SetProperty(ref _bcc, value); } }

        private bool _isIgnorable;
        public bool IsIgnorable { get { return _isIgnorable; } set { SetProperty(ref _isIgnorable, value); } } 


        public Dictionary<string, FsEntity> ExtractAttachments(CancellationTokenSource token)
        {
            var dict = new Dictionary<string, FsEntity>();

            try
            {
                var master = GetFsEntity();

                var message = MimeMessage.Load(master.FullPath, token.Token);

                var i = 1;
                foreach (var attachment in message.Attachments)
                {
                    token.Token.ThrowIfCancellationRequested();

                    string targetPath;
                    string zipPath;

                    var extension = Path.GetExtension(attachment.FileName);
                    if (extension == Resources.FileExtension_Zip)
                    {
                        targetPath = (Directory.CreateDirectory(FsEntity.GetTempDir() + FsEntity.GetGuidFilename(""))).FullName;
                        zipPath = targetPath + Path.DirectorySeparatorChar + attachment.FileName;
                    }
                    else targetPath = zipPath = FsEntity.GetTempDir() + FsEntity.GetGuidFilename(extension);

                    try
                    {
                        var name = string.Format(Resources.Email_ExtractAttachments_Label, i++.ToString(Resources.FlerpId_RevPad), Path.GetFileNameWithoutExtension(attachment.FileName));
                        using (var stream = File.Create(zipPath)) attachment.ContentObject.DecodeTo(stream);
                        dict.Add(name, FsEntity.Create(targetPath));
                    }
                    catch
                    {
                        Controller.Logger.Info(string.Format("Could not extract attachment from {0} - {1}.", Id, targetPath));
                    }
                }
                return dict;
            }
            catch
            {
                foreach (var fsEntity in dict) { fsEntity.Value.Delete(); }
                throw;
            }            
        }

        public FsEntity ExtractMessageOnly(CancellationTokenSource token)
        {
            var master = GetFsEntity();

            var message = MimeMessage.Load(master.FullPath, token.Token);

            var multipart = message.Body as Multipart;
            while (multipart != null && message.Attachments.Any()) multipart.Remove(message.Attachments.ElementAt(0));
            message.Body = multipart;

            var filePath = FsEntity.GetTempDir() + FsEntity.GetGuidFilename(Resources.FileExtension_Email);

            message.WriteTo(filePath);
            return FsEntity.Create(filePath);           
        }

        private static Binder GetEmailBinderByDate(DateTime date)
        {
            try
            {
                return Controller.GetEntityById<Binder>(BinderType.E.ToString() + date.ToString("yyyy"));
            }
            catch (Exception)
            {
                new NewEmailBinderCommand(date).Execute();
                return GetEmailBinderByDate(date);
            }
        }

        public override void ToBinder(Binder target, CancellationTokenSource cts)
        {
            var results = new List<Document>();

            var finished = false;
            var email = Controller.GetEntityById<Email>(Id);

            try
            {
                var binder = Controller.GetEntityById<Binder>(target.Id);

                if (email.Id.Contains(BinderContentType.N.ToString()))
                {
                    results.Add(Document.CreateInputFromEmail(binder, email, cts));
                    cts.Token.ThrowIfCancellationRequested();

                    var attachments = Document.CreateInputsFromEmailAttachments(binder, email, cts);
                    results.AddRange(attachments);
                }
                else if (email.Id.Contains(BinderContentType.X.ToString()))
                {
                    results.Add(Document.CreateOutputFromEmail(binder, email, cts));
                    cts.Token.ThrowIfCancellationRequested();
                }

                Views++;
                Save();

                finished = true;
            }
            finally
            {
                if (!finished) foreach (var entity in results) entity.Abort();
                else if (!Equals(email.Status, LcStatusType.Released)) email.Release(cts, false);
            }
        }

        public static bool TryGetByMessageId(string messageId, out Email email)
        {
            email = Controller.Collection.OfType<Email>().FirstOrDefault(x => x.MasterHash == messageId);
            return email != null;
        }
    }
}