using System;

namespace Flerp.DomainModel
{
    public interface IFlerpEntity : IPersistable
    {
        PrivacyType Privacy { get; set; }

        string Name { get; set; }

        DateTime CreatedDate { get; set; }

        DateTime ModifiedDate { get; set; }

        DateTime DisposedDate { get; set; }

        void Delete();
    }
}