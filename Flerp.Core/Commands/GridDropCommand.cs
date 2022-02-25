using System.Collections.Generic;
using System.Linq;
using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class GridDropCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Input from File-drop"; } }
        public override string ConfirmText { get { return string.Format("Attempt to file {0} items?", Args.Count - 1); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q1]; } }
        
        public GridDropCommand(Binder binder, IEnumerable<string> files)
        {
            Args.Add("BinderId", binder.Id);

            var i = 0;
            foreach (var file in files)
            {
                Args.Add("File_" + i++, file);
            }
        }

        public override void Execute()
        {
            var binder = Controller.GetEntityById<Binder>(Args["BinderId"]);
            var files = Args.Where(x => x.Key.StartsWith("File_")).Select(x => x.Value);

            foreach (var filePath in files)
            {
                Cts.Token.ThrowIfCancellationRequested();
                Controller.IssueCommand(new NewInputDocumentFromPathCommand(binder, filePath));
            }            
        }

        public override void Unexecute() { }
    }
}