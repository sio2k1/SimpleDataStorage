using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDataStorage.Test
{
    class UserModel:IDBIdentity
    {
        public string Name { get; set; }
        public string Pwd { get; set; }
        public int Id { get; set; }
    }

    class UserModel2 : IDBIdentity
    {
        public string Name2 { get; set; }
        public string Pwd2 { get; set; }
        public int Id { get; set; }
    }
}
