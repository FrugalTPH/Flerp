using Flerp.Commands;
using Flerp.Data.EmailCrawler.Properties;
using Flerp.DomainModel;
using MailKit;
using MailKit.Net.Imap;
using System.Net;

namespace Flerp.Data.EmailCrawler
{
    public class EmailClient : IEmailClient
    {
        public void GetEmails()
        {
            foreach (var account in EmailAccount.GetAll())
            {
                using (var client = new ImapClient())
                {
                    client.Connect(account.Uri);
                    if (!client.IsConnected) client.Connect(account.Uri, int.Parse(account.SpecifiedPort), account.UseSsl);

                    //if (account.UseSsl || int.TryParse(account.SpecifiedPort, out result)) client.Connect(account.Uri, result, account.UseSsl);
                    //else client.Connect(account.Uri);
                    
                    client.Authenticate(new NetworkCredential(account.UserName, account.Password));
                    Controller.Logger.Info(string.Format(Resources.uiMsg_Email_ConnectedTo, account.UserName));

                    var altFolder = client.GetFolder(SpecialFolder.Sent);
                    if (string.IsNullOrWhiteSpace(account.SpecificMailbox)) GetMimeMessages(client.Inbox, account.Cap);
                    else altFolder = client.GetFolder(account.SpecificMailbox);
                    if (altFolder != null) GetMimeMessages(altFolder, account.Cap);

                    client.Disconnect(true);
                    Controller.Logger.Info(string.Format(Resources.uiMsg_Email_DisconnectedFrom, account.UserName));
                }
            }
        }

        private void GetMimeMessages(IMailFolder folder, int cap)
        {
            var i = 0;
            folder.Open(FolderAccess.ReadOnly);
            foreach (var msgSummary in folder.Fetch(0, -1, MessageSummaryItems.Envelope))
            {
                Email existing;
                if (!Email.TryGetByMessageId(msgSummary.Envelope.MessageId, out existing))
                {
                    var subject = msgSummary.Envelope.Subject ?? Resources.uiMsg_Email_NoSubject;
                    if (subject.Length > 30) subject = subject.Substring(0, 29) + Resources.String_Truncated;

                    Controller.Logger.Info(string.Format(Resources.uiMsg_Email_Downloading, subject));

                    var message = folder.GetMessage(msgSummary.Index);
                    var path = FsEntity.GetTempDir() + FsEntity.GetGuidFilename(Resources.Email_FileType);
                    message.WriteTo(path);

                    Controller.IssueCommand(new NewEmailCommand(path));
                    i++;
                }
                if (cap > 0 && i >= cap) break;
            }
            Controller.Logger.Info(i > 0
                ? string.Format(Resources.uiMsg_Email_Retrieved, i)
                : Resources.uiMsg_Email_NoneRetrieved);
        }
    }
}
