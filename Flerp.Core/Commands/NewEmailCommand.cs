using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewEmailCommand : CommandBase
    {
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public NewEmailCommand(string sourcePath)
        {
            Args.Add("SourcePath", sourcePath);
        }

        public override void Execute()
        {
            Results.Add(Email.Create(FsEntity.Create(Args["SourcePath"]), true, false, Cts).Id);   
        }

        public override void Unexecute() { }
    }
}
