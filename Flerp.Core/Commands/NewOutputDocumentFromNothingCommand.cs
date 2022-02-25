using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewOutputDocumentFromNothingCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Document"; } }
        public override string ConfirmText { get { return string.Format("Create a new unauditable output document in {0}?", Args["BinderId"]); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public NewOutputDocumentFromNothingCommand(Binder binder)
        {
            Args.Add("BinderId", binder.Id);
        }

        public override void Execute()
        {
            var binder = Controller.GetEntityById<Binder>(Args["BinderId"]);

            Results.Add(Document.CreateOutputFromNothing(binder, Cts).Id);
        }

        public override void Unexecute() { }
    }
}
