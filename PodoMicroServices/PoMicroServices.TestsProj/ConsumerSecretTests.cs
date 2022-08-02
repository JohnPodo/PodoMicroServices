using PodoMicroServices.Common.Dto.SecretDto;
using PodoMicroServices.Consumer;

namespace PoMicroServices.TestsProj
{
    public class ConsumerSecretTests
    {
        public PodoConsumer Consumer { get; set; }

        public ConsumerSecretTests()
        {
            Consumer = new PodoConsumer("https://localhost:1312");
            var result = Consumer.Login("TestUser", "TestUser", 3).Result;
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetSecrets()
        {
            var result = await Consumer.GetSecrets();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);

        }

        [Fact]
        public async Task FullCircleSecret()
        {
            var result = await Consumer.AddSecret(new SecretDto()
            {

                Name = "TestSecret",
                Value = "Test"
            });
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            var check = await Consumer.GetSecret("TestSecret");
            Assert.NotNull(check);
            Assert.True(check.Success);
            var secret = check.Data;
            Assert.NotNull(secret);
            if (secret is not null)
                Assert.True(secret.Value == "Test");
            if (secret is null) return;
            result = await Consumer.UpdateSecret(secret.Id, new SecretDto()
            {

                Name = "TestSecret",
                Value = "Test12"
            });
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            check = await Consumer.GetSecret("TestSecret");
            Assert.NotNull(check);
            Assert.True(check.Success);
            secret = check.Data;
            Assert.NotNull(secret);
            if (secret is not null)
                Assert.True(secret.Value == "Test12");

            if (secret is not null)
            {
                result = await Consumer.DeleteSecret(secret.Id);
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.True(string.IsNullOrEmpty(result.Message));
                check = await Consumer.GetSecret("TestSecret");
                Assert.NotNull(check);
                Assert.False(check.Success);
                secret = check.Data;
                Assert.Null(secret);
            }
            else
            {

                Assert.True(false);
            }
        }

        [Fact]
        public async Task AddSecret_Unsuccesfull()
        {
            var result = await Consumer.AddSecret(new SecretDto()
            {
            });
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }
        [Fact]
        public async Task AddSecret_Unsuccesfull1()
        {
            var result = await Consumer.AddSecret(new SecretDto()
            {
                ExpiresIn = DateTime.Now.AddDays(-1),
                CreatedAt = DateTime.Now,
                Name = "DesignedToFail",
                Value = "Test",
            });
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));
            var check = await Consumer.GetSecret("DesignedToFail");
            Assert.NotNull(check);
            Assert.False(check.Success);
            var secret = check.Data;
            Assert.Null(secret);
        }

        [Fact]
        public async Task DeleteSecret_Unsuccesfull()
        {
            var result = await Consumer.DeleteSecret(82200);
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public async Task UpdateSecret_Unsuccesfull()
        {
            var result = await Consumer.UpdateSecret(950522152, new SecretDto()
            {

                Name = "TestSecret",
                Value = "Test12"
            });
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));

        }
    }
}
