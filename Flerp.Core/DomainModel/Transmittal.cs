using System;
using System.Linq;
using System.Threading;

namespace Flerp.DomainModel
{
    // Basket / transmittals not yet functional.
    public class Transmittal : RelationBase, IBasketable
    {
        private Transmittal(Party party, Document document)
        {
            MajorId = party.Id;
            MinorId = document.Id;
            Name = document.GetFsEntity().Id;
            Target = party.TransmittalTarget;
            PartyLink = GetAccessLink();
        }

        public static void CreateFromBasket(CancellationTokenSource token)
        {
            var documents = Controller.Basket.OfType<Document>() as Document[] ?? Controller.Basket.OfType<Document>().ToArray();
            var parties = Controller.Basket.OfType<Party>() as Party[] ?? Controller.Basket.OfType<Party>().ToArray();

            if (!documents.Any() || !parties.Any()) 
                throw new InvalidOperationException("The basket must contain Documents and Parties in order to create Transmittals.");

            if (parties.Any(party => string.IsNullOrWhiteSpace(party.TransmittalTarget)))
                throw new InvalidOperationException("All parties must have an appropriate target address noted.");

            foreach (var party in parties)
            {
                var p = party;
                foreach (var n in documents.Select(document => new Transmittal(p, document))) 
                    n.ToBasket(token);
                Controller.Basket.Remove(party);
            }
            foreach (var document in documents) 
                Controller.Basket.Remove(document);    
        }

        private string _target;
        public string Target { get { return _target; } set { SetProperty(ref _target, value); } }

        private string _partyLink;
        public string PartyLink { get { return _partyLink; } set { SetProperty(ref _partyLink, value); } }

        private string _notes;
        public string Notes { get { return _notes; } set { SetProperty(ref _notes, value); } }

        public string PartyName { get { return (Controller.GetEntityById<Party>(MajorId)).Name; } }

        private Document Doc { get { return (Controller.GetEntityById<Document>(MinorId)); } }

        public string DocName { get { return Doc.Name; } }

        public string DocAliasId { get { return Doc.AliasId; } }


        private string GetAccessLink()
        {
            // Generate from Flerp.Data.Cloud method that gets a Shared Access Signature from Cloud Service.
            return "<Secure Access Signature>";
        }

        public void ToBasket(CancellationTokenSource cts)
        {
            Controller.Basket.Add(this);
        }

    }
}