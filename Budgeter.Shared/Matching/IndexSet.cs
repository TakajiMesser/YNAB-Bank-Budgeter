using System;
using System.Collections.Generic;

namespace Budgeter.Shared.Matching
{
    public class IndexSet
    {
        private Dictionary<Type, int> _indexByType = new();

        public void Initialize(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                _indexByType.Add(type, 0);
            }
        }

        public int Index(Type type) => _indexByType[type];
        public int Index<T>() => _indexByType[typeof(T)];

        public void Increment(Type type) => _indexByType[type]++;
        public void Increment<T>() => _indexByType[typeof(T)]++;

        public void Reset(Type type) => _indexByType[type] = 0;
        public void Reset<T>() => _indexByType[typeof(T)] = 0;

        public void Clear() => _indexByType.Clear();
    }
}
