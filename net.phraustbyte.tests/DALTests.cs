using Moq;
using net.phraustbyte.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace net.phraustbyte.tests
{
    public class DALTests
    {
        [Fact]
        public async Task MockDAL_Create_Success()
        {
            TestClass tc = new TestClass(GetMock().Object)
            {
                Id = 5,
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            Assert.Equal(5, await tc.Create());
        }
        [Fact]
        public async Task MocDAL_Read_Success()
        {
            var mock = GetMock();
            TestClass tc = new TestClass(mock.Object)
            {
                Id = 5,
                Changer = "User",
                Name = "Name",
                CreatedDate = DateTime.UtcNow
            };
            var result = await tc.Read<TestClass>(5);
            Assert.Equal(tc.Id, result.Id);
            Assert.Equal(tc.Changer, result.Changer);
            Assert.Equal(tc.Name, result.Name);
        }
        [Fact]
        public async Task MocDAL_ReadAll_Success()
        {
            var mock = GetMock();
            TestClass tc = new TestClass(mock.Object);
            var result = await tc.ReadAll<TestClass>();
            Assert.True(result.Count == 3);
        }
        private Mock<IBaseDAL> GetMock()
        {
            var mock = new Mock<IBaseDAL>();
            mock.Setup(x => x.Create(It.IsAny<TestClass>()))
                .ReturnsAsync(5);
            mock.Setup(x => x.Delete(It.IsAny<TestClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Update(It.IsAny<TestClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Read<TestClass>(5))
                .ReturnsAsync(new TestClass { Id = 5, CreatedDate = DateTime.UtcNow, Changer = "User", Name = "Name" });
            mock.Setup(x => x.ReadAll<TestClass>())
                .ReturnsAsync(new List<TestClass>
                {
                    new TestClass { Id = 5, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameFive" },
                    new TestClass { Id = 6, CreatedDate = DateTime.UtcNow, Changer = "UserSix", Name = "NameSix" },
                    new TestClass { Id = 7, CreatedDate = DateTime.UtcNow, Changer = "UserSeven", Name = "NameSeven" }
                });
            return mock;

        }
    }
}
