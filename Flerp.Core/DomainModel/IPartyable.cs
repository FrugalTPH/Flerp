namespace Flerp.DomainModel
{
    public interface IPartyable : IPersistable 
    {
        void ToParty(Party target);
    }
}