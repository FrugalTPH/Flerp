namespace Flerp.DomainModel
{
    public interface IBinderContent : IPersistable
    {
        string Category { get; set; }

        void CascadeUpdateBinder();
    }
}