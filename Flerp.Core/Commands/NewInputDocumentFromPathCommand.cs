using System.IO;
using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewInputDocumentFromPathCommand : CommandBase
    {
        public override string CmdDescription { get { return string.Format("New input document from path '{0}'.", Path.GetFileName(Args["SourcePath"])); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public NewInputDocumentFromPathCommand(Binder binder, string sourcePath)
        {
            Args.Add("BinderId", binder.Id);
            Args.Add("SourcePath", sourcePath);
        }

        public override void Execute()
        {
            var binder = Controller.GetEntityById<Binder>(Args["BinderId"]);
            var sourceEntity = FsEntity.Create(Args["SourcePath"]);

            Results.Add(Document.CreateInputFromPath(binder, sourceEntity, Cts).Id);
        }

        public override void Unexecute() { }
    }
}
