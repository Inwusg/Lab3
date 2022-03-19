using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

namespace HashTable
{
    public class OpenAddressHashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IHashTable<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private int[] mas =
        {
            5, 11, 17, 37, 67, 131, 257, 521, 1031, 2053, 4099, 8209, 16411, 32771, 65543, 131101, 262187, 524341, 1573009
        };
        Pair<TKey, TValue>[] _table;
        private int _capacity;
        HashMaker<TKey> _hashMaker1, _hashMaker2;
        public int Count { get; private set; }
        private const double FillFactor = 0.85;
        private int lastIter;
        private int _indexMas;

        private TKey lastTKey;
        private int lastIndexPair;
        private bool isLastFind;
        public Stopwatch sp1;
        public Stopwatch sp2;
        public Stopwatch sp3;
        public Stopwatch sp4;

        public OpenAddressHashTable() : this(6) //9973
        { }
        public OpenAddressHashTable(int m)
        {
            _indexMas = m;
            _capacity = mas[_indexMas];
            _table = new Pair<TKey, TValue>[_capacity];
            _hashMaker1 = new HashMaker<TKey>(_capacity);
            _hashMaker2 = new HashMaker<TKey>(_capacity - 1);
            Count = 0;
            lastIter = 1;

            sp1 = new Stopwatch();
            sp2 = new Stopwatch();
            sp3 = new Stopwatch();
            sp4 = new Stopwatch();

            //lastTKey = null;
            lastIndexPair = -1;
            isLastFind = false;
        }

        public void Add(TKey key, TValue value)
        {
            sp1.Start();
            if (isLastFind)
            {
                if (lastTKey.Equals(key))
                {
                    _table[lastIndexPair] = new Pair<TKey, TValue>(key, value);
                    Count++;
                }
                if ((double)Count / _capacity >= FillFactor)
                {
                    IncreaseTable();
                }
                isLastFind = false;
                sp1.Stop();
                return;
            }

            var hash = _hashMaker1.ReturnHash(key);

            if (!TryToPut(hash, key, value)) // ячейка занята
            {
                int iterationNumber = 1;
                while (true)
                {
                    var place = (hash + iterationNumber * (1 + _hashMaker2.ReturnHash(key))) % _capacity;
                    if (TryToPut(place, key, value))
                        break;
                    iterationNumber++;
                    if (iterationNumber >= _capacity)
                        throw new ApplicationException("HashTable full!!!");
                }
            }
            if ((double)Count / _capacity >= FillFactor)
            {
                IncreaseTable();
            }
            sp1.Stop();
        }

        private bool TryToPut(int place, TKey key, TValue value)
        {
            sp2.Start();
            if (_table[place] == null || _table[place].IsDeleted())
            {
                _table[place] = new Pair<TKey, TValue>(key, value);
                Count++;
                sp2.Stop();
                return true;
            }
            if (_table[place].Key.Equals(key))
            {
                throw new ArgumentException();
            }
            sp2.Stop();
            return false;
        }

        private Pair<TKey, TValue> Find(TKey x)
        {
            sp3.Start();
            if (isLastFind && lastTKey.Equals(x))
            {
                sp3.Stop();
                return _table[lastIndexPair];
            }

            isLastFind = true;
            lastTKey = x;

            var hash1 = _hashMaker1.ReturnHash(x);
            if (_table[hash1] == null)
            {
                lastIndexPair = hash1;
                //lastTKey = null;
                //isLastFind = false;
                sp3.Stop();
                return null;
            }
            if (!_table[hash1].IsDeleted() && _table[hash1].Key.Equals(x))
            {
                lastIndexPair = hash1;
                //lastTKey = _table[hash];
                sp3.Stop();
                return _table[hash1];
            }
            int iterationNumber = 1;
            var hash2 = _hashMaker2.ReturnHash(x);
            while (true)
            {
                var place = (hash1 + iterationNumber * (1 + hash2)) % _capacity;
                if (_table[place] == null)
                {
                    lastIndexPair = place;
                    //lastTKey = null;
                    sp3.Stop();
                    return null;
                }
                if (!_table[place].IsDeleted() && _table[place].Key.Equals(x))
                {
                    lastIndexPair = place;
                    //lastTKey = _table[place];
                    sp3.Stop();
                    return _table[place];
                }
                iterationNumber++;
                if (iterationNumber >= _capacity)
                {
                    isLastFind = false;
                    lastIndexPair = -1;
                    //lastTKey = null;
                    sp3.Stop();
                    return null;
                }
            }
        }

        public bool Remove(TKey key)
        {
            sp4.Start();
            if (Find(key) == null)
            {
                sp4.Stop();
                return false;
            }
            Find(key).DeletePair();
            sp4.Stop();
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                var pair = Find(key);
                if (pair == null)
                    throw new KeyNotFoundException();
                return pair.Value;
            }

            set
            {
                var pair = Find(key);
                if (pair == null)
                    throw new KeyNotFoundException();
                pair.Value = value;
            }
        }

        private void IncreaseTable()
        {
            // получить число и увеличить таблицу
            _indexMas++;
            _capacity = mas[_indexMas];
            _hashMaker1.SimpleNumber = _capacity;
            _hashMaker2.SimpleNumber = _capacity - 1;
            Array.Resize(ref _table, _capacity);
        }

        public bool ContainsKey(TKey key)
        {
            if (Find(key) != null)
                return true;
            //lastIter = 1;
            return false;

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