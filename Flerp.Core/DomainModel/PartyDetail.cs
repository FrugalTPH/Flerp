namespace Flerp.DomainModel
{
    public class PartyDetail : RelationBase
    {
        public static PartyDetail Create(Party party)
        {
            var n = new PartyDetail() { MajorId = party.Id, DetailType = PartyDetailType.Misc };

            Controller.RegisterNew(n);
            return n;
        }


        private PartyDetailType _detailType;
        public PartyDetailType DetailType { get { return _detailType; } set { SetProperty(ref _detailType, value); } }
    }
}