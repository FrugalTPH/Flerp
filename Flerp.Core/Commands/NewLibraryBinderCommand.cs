using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewLibraryBinderCommand : CommandBase
    {
        public override string CmdDescription { get { return "New Library Binder."; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Binder existing;
            if (!Controller.TryGetEntityById("L0001", out existing)) Results.Add(Binder.CreateLibrary().Id);
        }

        public override void Unexecute() { }
    }
}
