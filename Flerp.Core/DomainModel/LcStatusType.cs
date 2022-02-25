namespace Flerp.DomainModel
{
    public class LcStatusType : EnumEx
    {
        public static readonly LcStatusType Pending = new LcStatusType { Value = 1, DisplayName = "Pending" };
        public static readonly LcStatusType Released = new LcStatusType { Value = 2, DisplayName = "Released" };
        public static readonly LcStatusType Cancelled = new LcStatusType { Value = 3, DisplayName = "Cancelled" };
    }
}
