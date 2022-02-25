using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewOutputDocumentRevisionCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Revision"; } }
        public override string ConfirmText { get { return string.Format("Create a new revision of {0}?", Args["DocumentId"]); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public NewOutputDocumentRevisionCommand(Document document)
        {
            Args.Add("DocumentId", document.Id);
        }

        public override void Execute()
        {
            Results.Add(Document.CreateOutputRevision(Controller.GetEntityById<Document>(Args["DocumentId"]), Cts).Id);
        }

        public override void Unexecute() { }
    }
}
