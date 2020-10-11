# SimpleDataStorage

C# data storage based on Lists<YourType>

Can store data as Lists<YourType>, can have many Lists and creates them on fly.

Can return serialized state to save and restore its state.

# Not for production

This is not for production. I wrote this code as an experiment to store small amount of data in apps.

# Usage scenario

In your application you have different entities to store in Lists, you also want to save database state and restore it to/from JSON

# Usage

1. Define a model and implement IDBIdentity

```c#
  class UserModel:IDBIdentity
  {
      public string Name { get; set; }
      public string Pwd { get; set; }
      public int Id { get; set; }
  }
```

2. Instantiate the client and load Store from JSON if required.

```c#
  var db = new StorageClient();
  Storage.JSONStringToDB("[]"); // load state of Store from a json
```

```c#
  string jsonData = Storage.DBtoJSONString() // get serialized state of Store
```

3. Add object to Store

```c#
  var u = new UserModel() { Name = "test", Pwd = "testpwd" };
  int id = db.Add<UserModel>(u); // will return an ID
```

4. Modify object in Store

```c#
  var u = new UserModel() { Name = "test", Pwd = "testpwd" };
  int id = db.Add<UserModel>(u);

  db.Get<UserModel>(id).Name="newName"; //test->newName
```

5. Replace object in Store

```c#
  var u = new UserModel() { Name = "test", Pwd = "testpwd" };
  int id = db.Add<UserModel>(u);

  var u2 = new UserModel() { Name = "u2test2", Pwd = "testpwd2", Id=id }; // Id is important!
  db.Update<UserModel>(u2); // will replace u with u2

  db.Get<UserModel>(id).Name="newName"; //u2test2->newName
```

6. Delete object from Store

```c#
  var u = new UserModel() { Name = "test", Pwd = "testpwd" };
  int id = db.Add<UserModel>(u);

  db.Delete<UserModel>(id) // will delete UserModel with Id == id
```

7. Retrieve object from Store

```c#
  var u = new UserModel() { Name = "test", Pwd = "testpwd" };
  int id = db.Add<UserModel>(u);

  var obj = db.Get<UserModel>(id); // will retrieve one record by id
  var list = db.Get<UserModel>(id); // will retrieve whole List of UserModel
  //list.Where(...)
```
