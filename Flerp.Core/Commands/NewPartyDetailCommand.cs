using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewPartyDetailCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "New Party Detail"; } }
        public override string ConfirmText { get { return string.Format("Create a new detail for {0}?", Args["PartyId"]); } }
        public override string CmdDescription { get { return string.Format("New Party Detail for '{0}'.", Args["PartyId"]); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public NewPartyDetailCommand(Party party)
        {
            Args.Add("PartyId", party.Id);
        }

        public override void Execute()
        {
            Results.Add(PartyDetail.Create(Controller.GetEntityById<Party>(Args["PartyId"])).Id);
        }

        public override void Unexecute() { }
    }
}
