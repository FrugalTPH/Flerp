using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Flerp.DomainModel
{
    public class Party : BinderBase, IBasketable, IBinderable, IPartyable
    {
        public static Party CreateHuman()
        {
            var n = new Party(new FlerpId(PartyType.H).ToString());
            Controller.RegisterNew(n);
            return n;
        }
        
        public static Party CreateOrganisation()
        {
            var n = new Party(new FlerpId(PartyType.O).ToString());
            Controller.RegisterNew(n);
            return n;
        }

        private Party(string id)
        {
            Id = id;
            Privacy = PrivacyType.Default;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }


        public IEnumerable<PartyDetail> Details
        {
            get
            {
                return Controller.Collection
                    .OfType<PartyDetail>()
                    .Where(x => x.MajorId == Id)
                    .OrderBy(x => x.DetailType)
                    .ThenBy(x => x.Name);
            }
        }

        private string _transmittalTarget;
        public string TransmittalTarget { get { return _transmittalTarget; } set { SetProperty(ref _transmittalTarget, value); } }
        

        public void ToBasket(CancellationTokenSource cts)
        {
            if (Controller.Basket.Contains(this)) 
                throw new InvalidOperationException(string.Format("'{0}' is already in the basket.", Id));

            if (Controller.Basket.Any(x => x is Transmittal)) 
                throw new InvalidOperationException(string.Format("'{0}' cannot be added to the basket in transmittal mode.", Id));

            Controller.Basket.Add(this);
        }

        public void ToBinder(Binder target, CancellationTokenSource cts)
        {
            if (Controller.Collection.OfType<PartyBinder>().Any(x => x.MajorId == Id && x.MinorId == target.Id)) return;
            PartyBinder.Create(this, target);   
        }
        
        public void ToParty(Party target)
        {
            if (Controller.Collection.OfType<PartyParty>().Any(x => x.MajorId == Id && x.MinorId == target.Id)) return;
            PartyParty.Create(this, target);
        }

        private string GetEmail()
        {
            var email = Details.FirstOrDefault(x => Equals(x.DetailType, PartyDetailType.Email));
            return email != null ? email.MinorId : null;
        }
    }
}