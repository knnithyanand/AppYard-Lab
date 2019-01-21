using System;
using System.Collections.Generic;

namespace AppYard.Lab.DataStore.InMemory
{
    public class DocsStore
    {
        static Dictionary<string, Dictionary<string, object>> _store;

        static DocsStore()
        {
            _store = new Dictionary<string, Dictionary<string, object>>();
        }

        void PopulateStore()
        {
        }

        public object Get(string schemaName, string docId)
        {
            return _store.ContainsKey(schemaName) && _store[schemaName].ContainsKey(docId) ? _store[schemaName][docId] : null;
        }

        public void Remove(string schemaName, string docId)
        {
            if (_store.ContainsKey(schemaName) && _store[schemaName].ContainsKey(docId)) _store[schemaName].Remove(docId);
        }

        public void Update(string schemaName, string docId, object value)
        {
            if (_store.ContainsKey(schemaName) && _store[schemaName].ContainsKey(docId)) _store[schemaName][docId] = value;
        }

        public void Add(string schemaName, string docId, object value)
        {
            if (!_store.ContainsKey(schemaName)) _store.Add(schemaName, new Dictionary<string, object>());
            if (!_store[schemaName].ContainsKey(docId)) _store[schemaName].Add(docId, value);
        }

        public IEnumerable<object> GetAll(string schemaName)
        {
            return _store.ContainsKey(schemaName) ? _store[schemaName].Values : null;
        }

    }
}
