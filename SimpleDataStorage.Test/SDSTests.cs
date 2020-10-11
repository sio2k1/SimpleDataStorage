using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SimpleDataStorage.Test
{
    public class SDSTests
    {
        static readonly object storageLock = new object();
        [Fact]
        public void ParallelAddDataGenerateId()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                int id = 0;
                List<UserModel> lst = new List<UserModel>();
                for (int i = 0; i < 300; i++)
                {
                    lst.Add(new UserModel() { Name = "test", Pwd = "testpwd" }); 
                }
                lst.AsParallel().ForAll(x => { id = db.Add<UserModel>(x); x.Id = id; });
                Assert.Equal(300, id);
            }
        }

        [Fact]
        public void ParallelAddDelete()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                int id = 0;
                List<UserModel> lst = new List<UserModel>();
                for (int i = 0; i < 300; i++)
                {
                    lst.Add(new UserModel() { Name = "test", Pwd = "testpwd" });
                }
                lst.AsParallel().ForAll(x => { id = db.Add<UserModel>(x); x.Id = id; });
                lst.Select(x => new UserModel() { Id = x.Id }).AsParallel().ForAll(x => db.Delete<UserModel>(x.Id));
                int cnt = db.Get<UserModel>().Count;

                Assert.Equal(0, cnt);
            }
        }

        [Fact]
        public void ParallelUpdate()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                int id = 0;
                List<UserModel> lst = new List<UserModel>();
                for (int i = 0; i < 300; i++)
                {
                    lst.Add(new UserModel() { Name = "test", Pwd = "testpwd" });
                }
                lst.AsParallel().ForAll(x => { id = db.Add<UserModel>(x); x.Id = id; });
                lst.Select(x => new UserModel() { Id = x.Id, Name="PU", Pwd=x.Id.ToString() }).AsParallel().ForAll(x => db.Update<UserModel>(x));
                var u = db.Get<UserModel>(250);

                Assert.Equal("250", u.Pwd);
            }
        }

        [Fact]
        public void GetModifyData()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                var u = new UserModel() { Name = "test", Pwd = "testpwd" };
                int id = db.Add<UserModel>(u);
                Assert.Equal("test", db.Get<UserModel>(id).Name);
                u.Name = "test2";
                Assert.Equal("test2", db.Get<UserModel>(id).Name);
            }
        }
        [Fact]
        public void UpdateDataReplaceEntity()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                var u = new UserModel() { Name = "test", Pwd = "testpwd" };
                int id = db.Add<UserModel>(u); 
                var u2 = new UserModel() { Name = "test2", Pwd = "testpwd2", Id = id };
                db.Update<UserModel>(u2);

                Assert.Equal("test2", db.Get<UserModel>(id).Name);
            }
        }
        [Fact]
        public void GetSeveralRecords()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                var u = new UserModel() { Name = "test", Pwd = "testpwd" };
                db.Add<UserModel>(u);
                var u2 = new UserModel() { Name = "test2", Pwd = "testpwd" };
                db.Add<UserModel>(u2);
                var res = db.Get<UserModel>();

                Assert.Equal(2, res.Count);
            }
        }
        [Fact]
        public void JsonSaveRestoreStorage()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                var u = new UserModel() { Name = "test", Pwd = "testpwd" };
                db.Add<UserModel>(u);
                var u2 = new UserModel() { Name = "test2", Pwd = "testpwd" };
                db.Add<UserModel>(u2);
                var json = Storage.DBtoJSONString();
                Storage.JSONStringToDB(json);
                var res = db.Get<UserModel>();

                Assert.Equal(2, res.Count);
            }
        }
        [Fact]
        public void UpdateDataModifyTransparent()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("{\"$type\":\"System.Collections.Generic.List`1[[SimpleDataStorage.DataEntry, SimpleDataStorage]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"SimpleDataStorage.DataEntry, SimpleDataStorage\",\"TypeFullName\":\"SimpleDataStorage.Test.UserModel\",\"Data\":{\"$type\":\"System.Collections.Generic.List`1[[SimpleDataStorage.Test.UserModel, SimpleDataStorage.Test]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"SimpleDataStorage.Test.UserModel, SimpleDataStorage.Test\",\"Name\":\"test\",\"Pwd\":\"testpwd\",\"Id\":10}]}}]}");
                var u = db.Get<UserModel>(10);
                Assert.Equal("test", u.Name);
                u.Name = "test2";

                Assert.Equal("test2", u.Name);
            }
        }
        [Fact]
        public void RecordNotFound()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                var u = db.Get<UserModel>(10);

                Assert.Equal(0, u.Id);
            }
        }

        [Fact]
        public void Store2Entities()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                var u = new UserModel() { Name = "test", Pwd = "testpwd" };
                int idu = db.Add<UserModel>(u);
                var u2 = new UserModel2() { Name2 = "test2", Pwd2 = "testpwd2"};
                int idu2 = db.Add<UserModel2>(u2);

                Assert.Equal("test", db.Get<UserModel>(idu).Name);
                Assert.Equal("test2", db.Get<UserModel2>(idu2).Name2);
            }
        }
        [Fact]
        public void AddDeleteEntity()
        {
            lock (storageLock)
            {
                var db = new StorageClient();
                Storage.JSONStringToDB("[]");
                int id = db.Add<UserModel>(new UserModel() { Name = "test", Pwd = "testpwd" });
                int id2 = db.Add<UserModel>(new UserModel() { Name = "test2", Pwd = "testpwd2" });

                Assert.Equal(2, db.Get<UserModel>().Count);

                db.Delete<UserModel>(id);

                Assert.Equal("test2", db.Get<UserModel>(id2).Name);

                db.Delete<UserModel>(id2);
                
                Assert.Equal(0, db.Get<UserModel>().Count);
            }
        }
    }
}
