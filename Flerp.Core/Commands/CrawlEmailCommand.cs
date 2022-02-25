using Autofac;
using Flerp.Data;

namespace Flerp.Commands
{
    public class CrawlEmailCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Get Emails"; } }
        public override string ConfirmText { get { return "Crawl all IMAP accounts for new emails?"; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q1]; } }

        public override void Execute()
        {
            var ec = Controller.Container.Resolve<IEmailClient>();
            ec.GetEmails();
        }

        public override void Unexecute() { }
    }
}
