using System;
using System.Linq;

namespace Flerp.DomainModel
{
    public class Binder : BinderBase
    {
        public static Binder CreateAdmin()
        {
            var n = new Binder(new FlerpId(BinderType.A).ToString());

            Controller.RegisterNew(n);
            return n;
        }
        public static Binder CreateEmail(DateTime date)
        {
            var tempId = BinderType.E + date.ToString("yyyy");

            Binder existing;
            if (Controller.TryGetEntityById(tempId, out existing)) return existing;

            var n = new Binder(new FlerpId(tempId).ToString(), string.Format("Email archive for {0}.", tempId.TrimStart('E')));

            Controller.RegisterNew(n);
            return n;
        }
        public static Binder CreateLibrary()
        {
            var n = new Binder(new FlerpId(BinderType.L).ToString(), "Reference library.");

            Controller.RegisterNew(n);
            return n;
        }
        public static Binder CreateWork()
        {
            var n = new Binder(new FlerpId(BinderType.W).ToString());

            Controller.RegisterNew(n);
            return n;
        }

        private Binder(string id, string name = null)
        {
            Id = id;
            Privacy = PrivacyType.Default;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;

            if (name != null) Name = name;
        }


        private string _client;
        public string Client { get { return _client; } set { SetProperty(ref _client, value); } }

        private decimal _hourRate;
        public decimal HourRate { get { return _hourRate; } set { SetProperty(ref _hourRate, value); } }

        private decimal _mileRate;
        public decimal MileRate { get { return _mileRate; } set { SetProperty(ref _mileRate, value); } }

        private decimal _expenseRate;
        public decimal ExpenseRate { get { return _expenseRate; } set { SetProperty(ref _expenseRate, value); } }

        public bool Contains(BinderContentType type, string hash)
        {
            var bcType = char.Parse(type.ToString());

            var duplicate = Controller.Collection.OfType<DocumentBase>()
                .Any(x =>
                    x.IdF.BinderId == Id &&
                    x.IdF.DocumentType == bcType &&
                    x.MasterHash == hash);

            return hash != "0e0c1d516e19e52d7b716edb327654f1" && duplicate;  // Where "0e0c1d516e19e52d7b716edb327654f1" is a null document.
        }
    }
}