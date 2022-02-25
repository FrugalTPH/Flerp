using System.Linq;
using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class CommitTransmittalsCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Commit Transmittals"; } }
        public override string ConfirmText { get { return "Save basket transmittals?"; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            foreach (var transmittal in Controller.Basket.OfType<Transmittal>().ToList())
            {
                Controller.RegisterNew(transmittal);
                Controller.Basket.Remove(transmittal);
            }
        }

        public override void Unexecute() { }
    }
}
