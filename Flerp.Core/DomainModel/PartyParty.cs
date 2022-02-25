namespace Flerp.DomainModel
{
    public class PartyParty : RelationBase
    {
        public static PartyParty Create(Party party1, Party party2)
        {
            var n = new PartyParty(party1, party2);

            Controller.RegisterNew(n);
            return n;
        }

        private PartyParty(Party party1, Party party2)
        {
            MajorId = party1.Id;
            MinorId = party2.Id;           
        }


        public string MajorName
        {
            get
            {
                var party = Controller.GetEntityById<Party>(MajorId);
                return party.Name;
            }
        }

        public string MinorName
        {
            get
            {
                var party = Controller.GetEntityById<Party>(MinorId);
                return party.Name;
            }
        }
    }
}