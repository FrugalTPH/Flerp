namespace Flerp.DomainModel
{
    public interface IPersistable
    {
        string Id { get; set; }

        FlerpId IdF { get; }

        void Save();

        void IdDoubleClick();
    }
}