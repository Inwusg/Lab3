using System.Collections;
using System.Diagnostics;

namespace HashTable
{
    public class OpenAddressHashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        private int[] mas =
        {
            3, 11, 19, 37, 79, 149, 281, 563, 1129, 2251, 4507, 9001, 18013, 36007, 72019, 144013, 288007, 576001,
            1152023, 2300003, 4600003, 9200017
        };
        Pair<TKey, TValue>[] _table;
        private int _capacity;
        public int Count { get; private set; }
        private const double FillFactor = 0.85;
        private int _indexMas;

        public OpenAddressHashTable() : this(14) //9973
        { }
        public OpenAddressHashTable(int m)
        {
            _indexMas = m;
            _capacity = mas[_indexMas];
            _table = new Pair<TKey, TValue>[_capacity];
            Count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            var hash1 = Hash1(key);
            var hash2 = Hash2(key);
            for (int i = 0; i < _capacity; i++)
            {
                var place = (hash1 + i * hash2) % _capacity;
                if (_table[place] == null || _table[place].IsDeleted())
                {
                    _table[place] = new Pair<TKey, TValue>(key, value);
                    Count++;
                    break;
                }
                if (_table[place].Key.Equals(key)) throw new ArgumentException("An element with such a key already exists");
                if (i == _capacity - 1) throw new ApplicationException("HashTable full!!!");
            }
            if (Count >= _capacity * FillFactor) IncreaseTable();
        }

        private Pair<TKey, TValue> Find(TKey key)
        {
            var hash1 = Hash1(key);
            var hash2 = Hash2(key);
            for (int i = 0; i < _capacity; i++)
            {
                int place = (hash1 + i * hash2) % _capacity;
                if (place < 0) break;
                if (_table[place] == null) return null;
                if (!_table[place].IsDeleted() && _table[place].Key.Equals(key)) return _table[place];
            }
            return null;
        }

        public bool Remove(TKey key)
        {
            var pair = Find(key);
            if (pair == null) return false;
            pair.DeletePair();
            Count--;
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                var pair = Find(key);
                if (pair == null) throw new KeyNotFoundException();
                return pair.Value;
            }
            set
            {
                var pair = Find(key);
                if (pair == null) throw new KeyNotFoundException();
                pair.Value = value;
            }
        }

        private void IncreaseTable()
        {
            _indexMas++;
            _capacity = mas[_indexMas];
            Pair<TKey, TValue>[] table = _table;
            _table = new Pair<TKey, TValue>[_capacity];
            Count = 0;
            foreach (var pair in table)
                if (pair != null && !pair.IsDeleted()) Add(pair.Key, pair.Value);
        }

        public bool ContainsKey(TKey key)
        {
            var pair = Find(key);
            if (pair == null || pair.IsDeleted()) return false;
            return true;

        }

        private int Hash1(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % _capacity;
        }

        private int Hash2(TKey key)
        {
            return 1 + Math.Abs(key.GetHashCode()) % (_capacity - 1);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from pair in _table where pair != null && !pair.IsDeleted() select new KeyValuePair<TKey, TValue>(pair.Key, pair.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}