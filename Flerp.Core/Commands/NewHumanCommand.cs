using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewHumanCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Human"; } }
        public override string ConfirmText { get { return "Create a new Human?"; } }
        public override string CmdDescription { get { return "New Human."; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Results.Add(Party.CreateHuman().Id);
        }

        public override void Unexecute() { }

    }
}
