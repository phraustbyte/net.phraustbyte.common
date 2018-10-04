using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.phraustbyte.bll;
using net.phraustbyte.dal;

namespace net.phraustbyte.tst
{
    public class testtable : IBaseBLL
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Changer { get; set; }
        public bool Active { get; set; }

        public IBaseDAL DataLayer { get; }

        public testtable()
        {
            DataLayer = new TTDataLayer();
        }

        public async Task<int> Create()
        {
            return await DataLayer.Create(this);
        }

        public async Task Delete()
        {
            await DataLayer.Delete(this);
        }

        public bool Equals(IBaseBLL other)
        {
            var oth = (testtable)other;
            return this.Id == oth.Id && this.Value == oth.Value;
        }

        public async Task<IBaseBLL> Read(int Id)
        {
            return await DataLayer.Read<testtable>(Id);
        }

        public async Task<List<IBaseBLL>> ReadAll()
        {
            var result = await DataLayer.ReadAll<testtable>();
            List<IBaseBLL> res = result.ToList<IBaseBLL>();
            return res;
        }

        public async Task Update()
        {
            await DataLayer.Update(this);
        }
    }
}
