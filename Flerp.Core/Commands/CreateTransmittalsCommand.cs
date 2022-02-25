using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class CreateTransmittalsCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Create Transmittals"; } }
        public override string ConfirmText { get { return "Process basket documents and parties into Transmittals?"; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Transmittal.CreateFromBasket(Cts);
        }

        public override void Unexecute() { }
    }
}
