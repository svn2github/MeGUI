using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.core.util
{
    public delegate bool Predicate<T>(T value);

    class SelectiveCollection<T> : ICollection<T>
    {
        private ICollection<T> impl;

        public ICollection<T> Impl
        {
            get { return impl; }
            set { impl = value; }
        }

        public Predicate<T> Matches
        {
            get { return matches; }
            set { matches = value; }
        }

        private Predicate<T> matches = delegate(T v) { return true; };

        public SelectiveCollection(ICollection<T> impl)
        {
            this.impl = impl;
        }

        #region ICollection<T> Members

        public void Add(T item)
        {
            if (matches(item))
                impl.Add(item);
        }

        public void Clear()
        {
            T[] array = new T[Count];
            CopyTo(array, 0);
            foreach (T t in array)
                impl.Remove(t);
        }

        public bool Contains(T item)
        {
            return matches(item) && impl.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
            {
                array[arrayIndex] = item;
                ++arrayIndex;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (T item in this)
                    count++;
                return count;
            }
        }

        public bool IsReadOnly
        {
            get { return impl.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            if (matches(item))
                return impl.Remove(item);
            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in impl)
            {
                if (matches(item))
                    yield return item;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    class SelectiveDictionary<TKey, TValue> : SelectiveCollection<KeyValuePair<TKey, TValue>>,
                                              IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> impl;
        private Predicate<TValue> matches = delegate(TValue v) { return true; };

        public SelectiveDictionary(IDictionary<TKey, TValue> impl) : base(impl)
        {
            this.impl = impl;
            base.Matches = delegate(KeyValuePair<TKey, TValue> t) { return matches(t.Value); };
        }

        public new IDictionary<TKey, TValue> Impl
        {
            get { return impl; }
            set { impl = value; }
        }

	    public new Predicate<TValue> Matches
	    {
		    get { return matches;}
		    set { matches = value;}
	    }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            if (matches(value))
                impl.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return (impl.ContainsKey(key) && matches(impl[key]));
        }

        public ICollection<TKey> Keys
        {
            get {
                SelectiveCollection<TKey> res = new SelectiveCollection<TKey>(impl.Keys);
                res.Matches = delegate(TKey v) { return this.matches(this.impl[v]); };
                return res;
            }
        }

        public bool Remove(TKey key)
        {
            if (matches(impl[key]))
                return impl.Remove(key);
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            try
            {
                value = this[key];
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                SelectiveCollection<TValue> res = new SelectiveCollection<TValue>(impl.Values);
                res.Matches = delegate(TValue v) { return matches(v); };
                return res;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue res = impl[key];
                if (!matches(res))
                    throw new KeyNotFoundException();
                return res;
            }
            set
            {
                if (!matches(value))
                    throw new Exception("Invalid element added to SelectiveDictionary.");
                impl[key] = value;
            }
        }

        #endregion
    }
}
