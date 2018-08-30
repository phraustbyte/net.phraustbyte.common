using System;
using System.Collections.Generic;
using net.phraustbyte.dal;

namespace net.phraustbyte.bll
{
    public interface IBaseBLL
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
        string CreateUser { get; set; }

        int Create();
        void Update();
        void Delete();
        List<IBaseBLL> List();
        IBaseBLL Read(int Id);
    }
}
