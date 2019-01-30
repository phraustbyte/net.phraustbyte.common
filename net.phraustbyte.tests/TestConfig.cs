using Moq;
using net.phraustbyte.dal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace net.phraustbyte.tests
{
    public static class TestConfig
    {
        public static Mock<IBaseDAL> GetMock()
        {
            var mock = new Mock<IBaseDAL>();
            mock.Setup(x => x.Create<TestClass, int>(It.IsAny<TestClass>())).ReturnsAsync(5);
            mock.Setup(x => x.ReadAllByFilter<TestClass, object>(It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(new List<TestClass>{

                    new TestClass { Id = 5, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameFive" },
                    new TestClass { Id = 6, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameSix" },
                    new TestClass { Id = 7, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameSeven" }
                });
            mock.Setup(x => x.Delete(It.IsAny<TestClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Update(It.IsAny<TestClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Read<int, TestClass>(5))
                .ReturnsAsync(new TestClass { Id = 5, CreatedDate = DateTime.UtcNow, Changer = "User", Name = "Name" });
            mock.Setup(x => x.Read<Guid, TestClass>(It.IsAny<Guid>()))
                .ReturnsAsync(new TestClass { Id = 5, CreatedDate = DateTime.UtcNow, Changer = "User", Name = "Name", Adjunct = new Guid() });
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
