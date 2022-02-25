namespace Flerp.DomainModel
{
    public class PrivacyType : EnumEx
    {
        public static readonly PrivacyType Default = new PrivacyType { Value = 1, DisplayName = "Default" };
        public static readonly PrivacyType Share = new PrivacyType { Value = 2, DisplayName = "Share" };
        public static readonly PrivacyType Private = new PrivacyType { Value = 3, DisplayName = "Private" };

        public PrivacyType GetDefault(IPersistable entity)
        {
            if (entity is BinderBase)
            {
                var fId = FlerpId.Parse(entity.Id);
                switch (fId.BinderType)
                {
                    case 'L':
                    case 'W':
                    case 'H':
                    case 'O':
                    return Share;
                }
            }
            
            if (entity is IBinderContent)
            {
                Binder binder;
                if (Controller.TryGetEntityById(FlerpId.Parse(entity.Id).BinderId, out binder)) return binder.Privacy;
            }
            return Private;
        }
    }
}
