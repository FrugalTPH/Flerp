using Flerp.Properties;
using MongoDB.Bson;
using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Flerp.DomainModel
{
    public struct FlerpId : IComparable<FlerpId>, IComparer<FlerpId>
    {
        public char BinderType { get; set; }
        public char DocumentType { get; set; }
        private int BinderNumber { get; set; }
        private int DocumentNumber { get; set; }
        private int RevisionNumber { get; set; }
        private string OId { get; set; }

        public string BinderId
        {
            get
            {
                return BinderNumber != Empty.BinderNumber
                    ? BinderType + BinderNumber.ToString(Resources.FlerpId_BinderPad)
                    : string.Empty;
            }
        }

        private string DocumentId
        {
            get
            {
                return DocumentNumber != Empty.DocumentNumber
                    ? DocumentType + DocumentNumber.ToString(Resources.FlerpId_DocPad)
                    : string.Empty;
            }
        }
        public string BinderAndDocumentId
        {
            get
            {
                return BinderNumber != Empty.BinderNumber && DocumentNumber != Empty.DocumentNumber
                    ? BinderId + Resources.Delim_Hyphen + DocumentId
                    : string.Empty;
            }
        }

        private string RevisionId
        {
            get
            {
                return RevisionNumber != Empty.RevisionNumber
                    ? RevisionNumber.ToString(Resources.FlerpId_RevPad)
                    : string.Empty;
            }
        }
        public static FlerpId Empty
        {
            get
            {
                return new FlerpId
                {
                    BinderType = '\0',
                    BinderNumber = 0,
                    DocumentType = '\0',
                    DocumentNumber = 0,
                    RevisionNumber = -1,
                    OId = string.Empty
                };
            }
        }
        public override string ToString()
        {
            var str = string.Empty;
            if (BinderId != string.Empty) { str += BinderId; }
            if (DocumentId != string.Empty) { str += Resources.Delim_Hyphen + DocumentId; }
            if (RevisionId != string.Empty) { str += Resources.Delim_Hyphen + RevisionId; }
            if (!string.IsNullOrEmpty(OId)) { str += OId.ToString(CultureInfo.InvariantCulture); }
            return str;
        }

        public FlerpId(string id)
        {
            TryParse(id, out this);
        }
        public FlerpId(BinderType type)
        {
            TryParse(type.ToString(), out this);
            var c = BinderType;

            var binders = GetCache().OfType<Binder>().Where(x => x.IdF.BinderType == c);

            var enumerable = binders as Binder[] ?? binders.ToArray();

            BinderNumber = 1 + 
                (enumerable.Any() 
                    ? enumerable.MaxBy(x => x.IdF.BinderNumber).IdF.BinderNumber 
                    : 0);
        }
        public FlerpId(PartyType type)
        {
            TryParse(type.ToString(), out this);
            var c = BinderType;

            var parties = GetCache().OfType<Party>().Where(x => x.IdF.BinderType == c);

            var enumerable = parties as Party[] ?? parties.ToArray();

            BinderNumber = 1 +
                (enumerable.Any()
                ? enumerable.MaxBy(x => x.IdF.BinderNumber).IdF.BinderNumber 
                : 0);
        }
        public FlerpId(Binder binder, BinderContentType type)
        {
            TryParse(binder.Id + Resources.Delim_Hyphen + type, out this);
            var binderId = BinderId;
            var documentType = char.Parse(type.ToString());

            var docs = GetCache().OfType<IBinderContent>()
                .Where(x => x.IdF.BinderId == binderId && x.IdF.DocumentType == documentType);

            var enumerable = docs as IBinderContent[] ?? docs.ToArray();

            DocumentNumber = 1 + 
                (enumerable.Any() 
                ? enumerable.MaxBy(x => x.IdF.DocumentNumber).IdF.DocumentNumber 
                : 0);

            if (type == BinderContentType.X) RevisionNumber = 0;
        }
        public FlerpId(Document document)
        {
            TryParse(document.Id, out this);
            var binderId = BinderId;
            var documentId = DocumentId;

            var revs = GetCache().OfType<IBinderContent>()
                .Where(x => x.IdF.BinderId == binderId && x.IdF.DocumentId == documentId);

            var enumerable = revs as IBinderContent[] ?? revs.ToArray();

            RevisionNumber = 1 + 
                (enumerable.Any()
                ? enumerable.MaxBy(x => x.IdF.RevisionNumber).IdF.RevisionNumber
                : -1);
        }

        public static FlerpId Parse(string s)
        {
            FlerpId x;
            if (!(TryParse(s, out x))) throw new ArgumentException("Could not parse - not a valid FlerpId.");
            return x;
        }
        private static bool TryParse(string s, out FlerpId fId)
        {
            fId = Empty;

            ObjectId oId;
            if (ObjectId.TryParse(s, out oId))
            {
                fId.OId = oId.ToString();
                return true;
            }

            var splitId = s.Split(char.Parse(Resources.Delim_Hyphen));

            switch (splitId.Length)
            {
                case 3:
                    fId.RevisionNumber = int.Parse(splitId[2]);
                    goto case 2;
                case 2:
                    fId.DocumentType = char.Parse(splitId[1].Substring(0, 1));
                    if (splitId[1].Length > 1) { fId.DocumentNumber = int.Parse(splitId[1].Substring(1)); }
                    goto case 1;
                case 1:
                    fId.BinderType = char.Parse(splitId[0].Substring(0, 1));
                    if (splitId[0].Length > 1) { fId.BinderNumber = int.Parse(splitId[0].Substring(1)); }
                    break;
                default:
                    fId = Empty;
                    return false;
            }
            return true;
        }

        public static bool operator <(FlerpId lhs, FlerpId rhs) { return lhs.CompareTo(rhs) < 0; }
        public static bool operator <=(FlerpId lhs, FlerpId rhs) { return lhs.CompareTo(rhs) <= 0; }
        public static bool operator ==(FlerpId lhs, FlerpId rhs) { return lhs.Equals(rhs); }
        public static bool operator !=(FlerpId lhs, FlerpId rhs) { return !lhs.Equals(rhs); }
        public static bool operator >=(FlerpId lhs, FlerpId rhs) { return lhs.CompareTo(rhs) >= 0; }
        public static bool operator >(FlerpId lhs, FlerpId rhs) { return lhs.CompareTo(rhs) > 0; }

        public override bool Equals(object obj)
        {
            return obj is FlerpId && ToString() == ((FlerpId)obj).ToString();
        }

        public override int GetHashCode()
        {
            var s = ToString();
            return s.Aggregate(0, (current, c) => c + (current << 6) + (current << 16) - current);
        }

        private bool IsFlerpId { get { return string.IsNullOrEmpty(OId); } }
        public int CompareTo(FlerpId other)
        {
            if (!IsFlerpId) { return !other.IsFlerpId ? string.Compare(OId, other.OId, StringComparison.Ordinal) : 1; }
            if (!other.IsFlerpId) { return -1; }

            var x = BinderType.CompareTo(other.BinderType);
            if (x != 0) { return x; }
            x = BinderNumber.CompareTo(other.BinderNumber);
            if (x != 0) { return x; }
            x = DocumentType.CompareTo(other.DocumentType);
            if (x != 0) { return x; }
            x = DocumentNumber.CompareTo(other.DocumentNumber);
            if (x != 0) { return x; }
            x = RevisionNumber.CompareTo(other.RevisionNumber);
            return x;
        }

        private static IList GetCache() { return Controller.Collection; }

        public int Compare(FlerpId x, FlerpId y) { return x.CompareTo(y); }
    }
}