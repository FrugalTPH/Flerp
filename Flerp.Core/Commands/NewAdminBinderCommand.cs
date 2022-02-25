using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewAdminBinderCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Admin Binder"; } }
        public override string ConfirmText { get { return "Create a new Admin Binder?"; } }
        public override string CmdDescription { get { return "New Admin Binder."; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Results.Add(Binder.CreateAdmin().Id);
        }

        public override void Unexecute() { }
    }
}
