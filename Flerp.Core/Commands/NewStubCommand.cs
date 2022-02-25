using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewStubCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Stub"; } }
        public override string ConfirmText { get { return string.Format("Create a new stub in {0}?", Args["BinderId"]); } }
        public override string CmdDescription { get { return string.Format("New Stub in '{0}'.", Args["BinderId"]); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public NewStubCommand(Binder binder)
        {
            Args.Add("BinderId", binder.Id);
        }

        public override void Execute()
        {
            Results.Add(Stub.Create(Controller.GetEntityById<Binder>(Args["BinderId"])).Id);          
        }

        public override void Unexecute() { }
    }
}
