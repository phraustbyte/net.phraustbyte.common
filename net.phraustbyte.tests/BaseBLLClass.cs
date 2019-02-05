using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace net.phraustbyte.tests
{
    public class BaseBLLClass
    {
        [Fact]
        public async Task BLL_Create_Success()
        {

            TestBLLClass test = new TestBLLClass(TestConfig.GetBLLTestMock().Object)
            {
                 Id = 5, Active = true, Adjunct = Guid.NewGuid(), Changer="Script", CreatedDate=DateTime.UtcNow, Description="Some Description", Name="Some Name"
            };
            var result = await test.Create<Guid>();
            Assert.IsType<Guid>(result);
            Assert.NotEqual(new Guid(), result);
        }
        [Fact]
        public async Task BLL_Read_Success() {
            var result = await (new TestBLLClass(TestConfig.GetBLLTestMock().Object)).Read<Guid, TestBLLClass>(new Guid());
            Assert.IsType<TestBLLClass>(result);
        }
        [Fact]
        public async Task BLL_ReadAll_Success() {
            var result = await (new TestBLLClass(TestConfig.GetBLLTestMock().Object)).ReadAll<TestBLLClass>(  );
            Assert.IsType<List<TestBLLClass>>(result);
        }
        [Fact]
        public async Task BLL_Update_Success() {
            TestBLLClass test = new TestBLLClass(TestConfig.GetBLLTestMock().Object)
            {
                Id = 5,
                Active = true,
                Adjunct = Guid.NewGuid(),
                Changer = "Script",
                CreatedDate = DateTime.UtcNow,
                Description = "Some Description",
                Name = "Some Name"
            };
            await test.Update();
        }
        [Fact]
        public async Task BLL_Delete_Success() {
            TestBLLClass test = new TestBLLClass(TestConfig.GetBLLTestMock().Object)
            {
                Id = 5,
                Active = true,
                Adjunct = Guid.NewGuid(),
                Changer = "Script",
                CreatedDate = DateTime.UtcNow,
                Description = "Some Description",
                Name = "Some Name"
            };
            await test.Delete();
        }
        //[Fact]
        //public async Task BLL_Filter() { }

    }
}
