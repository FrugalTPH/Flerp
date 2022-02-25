using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewEmailFromImportCommand : CommandBase
    {
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public NewEmailFromImportCommand(string sourcePath)
        {
            Args.Add("SourcePath", sourcePath);
        }

        public override void Execute()
        {
            Results.Add(Email.Create(FsEntity.Create(Args["SourcePath"]), false, true, Cts).Id);
        }

        public override void Unexecute() { }
    }
}
