using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using net.phraustbyte.dal.mysql;

namespace net.phraustbyte.tst
{
    public class TTDataLayer : BaseDAL
    {
        public TTDataLayer() : base("Server=192.168.0.161;Database=dbtest;Uid=phrausty;Pwd=dr@g0ns;SslMode=None")
        {
        }
        public override Task<int> Create<T>(T Obj)
        {
            this.Query = "usptesttable_Create";
            return base.Create(Obj);
        }
        public override Task Delete<T>(T Obj)
        {
            this.Query = "usptesttable_Delete";
            return base.Delete(Obj);
        }
        public override Task<T> Read<T>(int Id)
        {
            this.Query = "usptesttable_Read";
            return base.Read<T>(Id);
        }
        public override Task Update<T>(T Obj)
        {
            this.Query = "usptesttable_Update";
            return base.Update(Obj);
        }
        public override Task<List<T>> ReadAll<T>()
        {
            this.Query = "usptesttable_ReadAll";
            return base.ReadAll<T>();
        }
    }

}
