using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDataStorage
{
    public interface IDataBaseLayer
    {
        int Add<T>(IDBIdentity entity) where T : IDBIdentity;
        bool Delete<T>(IDBIdentity user) where T : IDBIdentity;
        List<T> Get<T>() where T : IDBIdentity;
        T Get<T>(int id) where T : IDBIdentity;
    }
}
