using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewWorkBinderCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Work Binder"; } }
        public override string ConfirmText { get { return "Create a new Work Binder?"; } }
        public override string CmdDescription { get { return "New Work Binder."; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Results.Add(Binder.CreateWork().Id);
        }

        public override void Unexecute() { }
    }
}
