using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    //[nsatpute][GEOS2-7894][01.08.2025]
    public class ObservableDictionary<TKey, TValue> :
       IDictionary<TKey, TValue>,
       INotifyCollectionChanged,
       INotifyPropertyChanged
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public ObservableDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                if (_dictionary.TryGetValue(key, out TValue oldValue))
                {
                    _dictionary[key] = value;
                    OnCollectionChanged(NotifyCollectionChangedAction.Replace,
                        new KeyValuePair<TKey, TValue>(key, value),
                        new KeyValuePair<TKey, TValue>(key, oldValue));
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            OnCollectionChanged(NotifyCollectionChangedAction.Add,
                new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public void Clear()
        {
            _dictionary.Clear();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) =>
            _dictionary.Contains(item);

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            _dictionary.GetEnumerator();

        public bool Remove(TKey key)
        {
            if (_dictionary.TryGetValue(key, out TValue value))
            {
                _dictionary.Remove(key);
                OnCollectionChanged(NotifyCollectionChangedAction.Remove,
                    new KeyValuePair<TKey, TValue>(key, value));
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public bool TryGetValue(TKey key, out TValue value) =>
            _dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action,
            KeyValuePair<TKey, TValue> changedItem = default,
            KeyValuePair<TKey, TValue> oldItem = default)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Keys)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Values)));
        }
    }
}
