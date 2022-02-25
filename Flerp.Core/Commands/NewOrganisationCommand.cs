using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewOrganisationCommand : CommandBase
    {
        public override string CmdDescription { get { return "New Organisation."; } }
        public override string ConfirmCaption { get { return "New Organisation"; } }
        public override string ConfirmText { get { return "Create a new Organisation?"; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Results.Add(Party.CreateOrganisation().Id);
        }

        public override void Unexecute() { }
    }
}
