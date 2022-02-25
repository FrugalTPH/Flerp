using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewOutputDocumentFromDocumentCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Document"; } }
        public override string ConfirmText { get { return string.Format("Create a new output document like {0}?", Args["DocumentId"]); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }
        
        public NewOutputDocumentFromDocumentCommand(Document document)
        {
            Args.Add("DocumentId", document.Id);
        }

        public override void Execute()
        {
            var document = Controller.GetEntityById<Document>(Args["DocumentId"]);
            var binder = Controller.GetEntityById<Binder>(FlerpId.Parse(document.Id).BinderId);

            Results.Add(Document.CreateOutputFromDocument(binder, document, Cts).Id);
        }

        public override void Unexecute() { }
    }
}
