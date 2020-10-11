using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDataStorage
{
    public class Storage
    {
        static List<DataEntry> db = new List<DataEntry>();
        static Func<DataEntry, Type, bool> typeNameComparer = (d, t) => d.TypeFullName == t.FullName;
        public static string DBtoJSONString()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.SerializeObject(db, settings);
        }
        public static void JSONStringToDB(string JSON) 
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            db = JsonConvert.DeserializeObject<List<DataEntry>>(JSON, settings); 
        }
        public static List<T> GetList<T>() where T : IDBIdentity
        {
            DataEntry jd = db.Find(x => typeNameComparer(x, typeof(T)));
            if (jd is null)
                db.Add(new DataEntry() { TypeFullName = typeof(T).FullName, Data = new List<T>() });
            return (jd is null) ?
                db.Find(x => typeNameComparer(x, typeof(T))).Data as List<T> : jd.Data as List<T>;
        }
    }
}
