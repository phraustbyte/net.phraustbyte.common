using System;
using System.Collections.Generic;
using net.phraustbyte.dal;
using System.Threading.Tasks;

namespace net.phraustbyte.bll
{
    public interface IBaseBLL : IEquatable<IBaseBLL>
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
        string Changer { get; set; }
        bool Active { get; set; }

        Task<int> Create();
        Task Update();
        Task Delete();
        Task<List<IBaseBLL>> ReadAll();
        Task<IBaseBLL> Read(int Id);

        IBaseDAL DataLayer { get; }
    }
}
