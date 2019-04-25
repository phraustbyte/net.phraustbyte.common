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
            mock.Setup(x => x.Create<TestClass>(It.IsAny<TestClass>())).ReturnsAsync(Guid.NewGuid());
            mock.Setup(x => x.ReadAllByFilter<TestClass, object>(It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(new List<TestClass>{

                    new TestClass { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameFive" },
                    new TestClass { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameSix" },
                    new TestClass { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameSeven" }
                });
            mock.Setup(x => x.Delete(It.IsAny<TestClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Update(It.IsAny<TestClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Read<TestClass>(It.IsAny<Guid>()))
                .ReturnsAsync(new TestClass { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Changer = "User", Name = "Name"});
            mock.Setup(x => x.ReadAll<TestClass>())
                .ReturnsAsync(new List<TestClass>
                {
                    new TestClass { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameFive" },
                    new TestClass { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Changer = "UserSix", Name = "NameSix" },
                    new TestClass { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Changer = "UserSeven", Name = "NameSeven" }
                });
            return mock;

        }
        public static Mock<IBaseDAL> GetBLLTestMock()
        {
            Guid guid = Guid.NewGuid();
            var mock = new Mock<IBaseDAL>();
            mock.Setup(x => x.Create<TestBLLClass>(It.IsAny<TestBLLClass>())).ReturnsAsync(guid);
            mock.Setup(x => x.ReadAllByFilter<TestBLLClass, object>(It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(new List<TestBLLClass>{

                    new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameFive"},
                    new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameSix"},
                    new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameSeven"}
                });
            mock.Setup(x => x.Delete(It.IsAny<TestBLLClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Update(It.IsAny<TestBLLClass>())).Returns(Task.CompletedTask).Verifiable();
            mock.Setup(x => x.Read<TestBLLClass>(guid))
                .ReturnsAsync(new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "User", Name = "Name"});
            mock.Setup(x => x.Read<TestBLLClass>(It.IsAny<Guid>()))
                .ReturnsAsync(new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "User", Name = "Name" });
            mock.Setup(x => x.ReadAll<TestBLLClass>())
                .ReturnsAsync(new List<TestBLLClass>
                {
                    new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "UserFive", Name = "NameFive"},
                    new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "UserSix", Name = "NameSix"},
                    new TestBLLClass { Id = guid, CreatedDate = DateTime.UtcNow, Changer = "UserSeven", Name = "NameSeven"}
                });
            return mock;

        }
    }
}
