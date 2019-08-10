using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.HelperModels
{
    public class CacheableList<T> : List<T>
    {
        public delegate void CacheableListActionHandler(CacheableList<T> list, T item, bool changeOnDb);

        public event CacheableListActionHandler OnAdd;
        public event CacheableListActionHandler OnRemove;
        public event CacheableListActionHandler OnUpdate;

        new public void Add(T item)
        {
            Add(item, true);
        }

        public void Add(T item, bool changeOnDb)
        {
            OnAdd?.Invoke(this, item, changeOnDb);
            base.Add(item);
        }

        new public bool Remove(T item)
        {
            return Remove(item, true);
        }

        public bool Remove(T item, bool changeOnDb)
        {
            OnRemove?.Invoke(this, item, changeOnDb);
            return base.Remove(item);
        }

        public void Update(Func<T, bool> predicate, T newValue)
        {
            Update(predicate, newValue, true);
        }

        public void Update(Func<T, bool> predicate, T newValue, bool changeOnDb)
        {
            OnUpdate?.Invoke(this, newValue, changeOnDb);
            T item = this.FirstOrDefault(predicate);
            if (item != null)
            {
                base.Remove(item);
            }
            base.Add(newValue);
        }
    }
}
