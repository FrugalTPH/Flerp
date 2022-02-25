using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;

namespace Flerp.DomainModel
{
    public abstract class EnumEx : IComparable, IBsonSerializable
    {
        protected int Value { private get; set; }

        public string DisplayName { get; protected set; }

        public override string ToString() { return DisplayName; }

        public override bool Equals(object obj)
        {
            var otherValue = obj as EnumEx;
            if (otherValue == null) { return false; }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() { return Value.GetHashCode(); }

        public int CompareTo(object other) { return Value.CompareTo(((EnumEx)other).Value); }

        public bool GetDocumentId(out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            id = null;
            idNominalType = null;
            idGenerator = null;
            return false;
        }
        
        public void SetDocumentId(object id) { }
        
        public void Serialize(BsonWriter bsonWriter, Type nominalType, IBsonSerializationOptions options)
        {
            BsonSerializer.Serialize(bsonWriter, DisplayName);
        }
        
        public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            return GetType().GetField(BsonSerializer.Deserialize<string>(bsonReader)).GetValue(this);
        }
    }
}
