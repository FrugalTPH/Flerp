using System.Collections.Generic;
using System.Linq;

namespace Flerp.DomainModel
{
    public class PartyBinder : RelationBase
    {
        public static PartyBinder Create(Party party, Binder binder)
        {
            var n = new PartyBinder { MajorId = party.Id, MinorId = binder.Id };

            Controller.RegisterNew(n);
            return n;
        }


        public IEnumerable<PartyDetail> Details
        {
            get
            {
                return Controller.Collection
                    .OfType<PartyDetail>()
                    .Where(x => x.MajorId == MajorId)
                    .OrderBy(x => x.DetailType)
                    .ThenBy(x => x.Name);
            }
        }

        public string PartyName
        {
            get
            {
                var party = Controller.GetEntityById<Party>(MajorId);
                return party.Name;
            }
        }

        public string BinderName
        {
            get
            {
                var binder = Controller.GetEntityById<Binder>(MinorId);
                return binder.Name;
            }
        }

    }
}