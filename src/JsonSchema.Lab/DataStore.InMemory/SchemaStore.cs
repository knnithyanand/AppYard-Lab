using System;
using System.Collections.Generic;

namespace AppYard.Lab.DataStore.InMemory
{
    public class SchemaStore
    {
        static Dictionary<string, object> _store;

        static SchemaStore()
        {
            _store = new Dictionary<string, object>();
        }

        void PopulateStore()
        {
            //_store.Add("address-book", JsonValue.Parse("{\"$schema\":\"http://json-schema.org/draft-07/schema#\",\"definitions\":{\"AddressBook\":{\"description\":\"\",\"title\":\"AddressBook\",\"type\":\"object\",\"properties\":{\"contacts\":{\"description\":\"A dictionary of Contacts, indexed by unique ID\",\"type\":\"object\",\"additionalProperties\":{\"$ref\":\"#/definitions/Contact\"},\"title\":\"contacts\"}},\"required\":[\"contacts\"]},\"Contact\":{\"title\":\"Contact\",\"type\":\"object\",\"properties\":{\"firstName\":{\"type\":\"string\",\"title\":\"firstName\"},\"lastName\":{\"type\":\"string\",\"title\":\"lastName\"},\"birthday\":{\"type\":\"string\",\"format\":\"date-time\",\"title\":\"birthday\"},\"title\":{\"enum\":[\"Mr.\",\"Mrs.\",\"Ms.\",\"Prof.\"],\"type\":\"string\",\"title\":\"title\"},\"emails\":{\"type\":\"array\",\"items\":{\"type\":\"string\"},\"title\":\"emails\"},\"phones\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/PhoneNumber\"},\"title\":\"phones\"},\"highScore\":{\"type\":\"number\",\"title\":\"highScore\"}},\"required\":[\"emails\",\"firstName\",\"highScore\",\"phones\"]},\"PhoneNumber\":{\"description\":\"A Contact's phone number.\",\"title\":\"PhoneNumber\",\"type\":\"object\",\"properties\":{\"number\":{\"type\":\"string\",\"title\":\"number\"},\"label\":{\"description\":\"An optional label (e.g. \"mobile\")\",\"type\":\"string\",\"title\":\"label\"}},\"required\":[\"number\"]}}}").ToString());
            //_store.Add("parent", JsonValue.Parse("{\"$schema\":\"http://json-schema.org/draft-06/schema#\",\"$ref\":\"#/definitions/Parent\",\"definitions\":{\"Parent\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"Name\":{\"type\":\"string\"},\"Children\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/Child\"}}},\"required\":[\"Children\",\"Name\"],\"title\":\"Parent\"},\"Child\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"Name\":{\"type\":\"string\"},\"$ref\":{\"type\":\"string\"}},\"required\":[],\"title\":\"Child\"}}}").ToString());
        }

        public object Get(string schemaName)
        {
            return _store.ContainsKey(schemaName) ? _store[schemaName] : null;
        }

        public void Remove(string schemaName)
        {
            if (_store.ContainsKey(schemaName)) _store.Remove(schemaName);
        }

        public void Update(string schemaName, object value)
        {
            if (_store.ContainsKey(schemaName)) _store[schemaName] = value;
        }

        public void Add(string schemaName, object value)
        {
            if (!_store.ContainsKey(schemaName)) _store.Add(schemaName, value);
        }

        public IEnumerable<object> GetAll()
        {
            return _store.Values;
        }

    }
}
