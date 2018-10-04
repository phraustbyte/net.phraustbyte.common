using System;
using System.Threading.Tasks;

namespace net.phraustbyte.tst
{
    class Program
    {
        static void Main(string[] args)
        {
            testtable table = new testtable { Value = "NumberOne", Active = true, CreatedDate = DateTime.UtcNow, Changer = "user", Id = 0 };
            var result = Task.Run(async() => { return await table.Create(); }).Result;
            table.Value = "NumberTwo";
            var resultTwo = Task.Run(async () => { return await table.Create(); }).Result;
            table.Id = resultTwo;
            table.Value = "NumberThree";
            Task.Run(async () => { await table.Update(); });
            Task.Run(async () => { await table.Delete(); });
            var ReadOne = Task.Run(async () => { return await table.Read(1); }).Result;
            Console.WriteLine($"ReadOne: {ReadOne}");
            var ReadAll = Task.Run(async () => { return await table.ReadAll(); }).Result;
            Console.WriteLine($"ReadAll: {ReadAll}");
        }
    }
}
