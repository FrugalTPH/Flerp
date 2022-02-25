using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Flerp.Commands
{
    public class EmailDropCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Input from Email-drop"; } }
        public override string ConfirmText { get { return string.Format("Attempt to file {0} emails?", Args.Count); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q1]; } }
        
        public EmailDropCommand(IEnumerable<string> files)
        {
            var i = 0;
            foreach (var file in files.Where(file => Path.GetExtension(file) == ".eml")) Args.Add("File_" + i++, file);
        }

        public override void Execute()
        {
            var files = Args.Where(x => x.Key.StartsWith("File_")).Select(x => x.Value);

            foreach (var filePath in files)
            {
                Cts.Token.ThrowIfCancellationRequested();
                Controller.IssueCommand(new NewEmailFromImportCommand(filePath));
            }            
        }

        public override void Unexecute() { }
    }
}