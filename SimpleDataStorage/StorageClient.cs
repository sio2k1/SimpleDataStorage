using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace SimpleDataStorage
{
    public class StorageClient : IDataBaseLayer
    {
        static readonly object idGenLock = new object();
        public int Add<T>(IDBIdentity entity) where T : IDBIdentity
        {
            if (!typeof(T).IsAssignableFrom(entity.GetType()))
            {
                throw new NotSupportedException("The entity has a wrong type");
            }
            List<T> storage = Storage.GetList<T>();
            lock (idGenLock)
            {
                entity.Id = storage.Count() > 0 ? storage.Max(x => x.Id) + 1 : 1;
                storage.Add((T)entity);
            }
            return storage.Find(x => x.Id == entity.Id).Id;
        }
        public bool Delete<T>(IDBIdentity entity) where T : IDBIdentity
        {
            if (!typeof(T).IsAssignableFrom(entity.GetType()))
            {
                throw new NotSupportedException("The entity has a wrong type");
            }
            List<T> storage = Storage.GetList<T>();
            bool res = false;
            lock (idGenLock)
            {
                storage.RemoveAll(x => x.Id == entity.Id);
                res = !storage.Any(x => x.Id == entity.Id);
            }
            return res;
        }
        public bool Update<T>(IDBIdentity entity) where T : IDBIdentity
        {
            if (!typeof(T).IsAssignableFrom(entity.GetType()))
            {
                throw new NotSupportedException("The entity has a wrong type");
            }
            List<T> storage = Storage.GetList<T>();
            storage[storage.IndexOf(storage.Find(x => x.Id == entity.Id))] = (T)entity;
            return true;
        }
        public List<T> Get<T>() where T : IDBIdentity
        {
            return Storage.GetList<T>();
        }
        public T Get<T>(int id) where T : IDBIdentity
        {
            var res = Storage.GetList<T>().Find(x => x.Id == id);
            if (res == null)
            {
                T t = Activator.CreateInstance<T>();
                t.Id = 0;
                return t;
            } else
                return res;
        }
    }
}
