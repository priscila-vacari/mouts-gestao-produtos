using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Infra.Context;
using GestaoProdutos.InfraEstructure.Repositories;
using GestaoProdutos.Tests.Fakes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace GestaoProdutos.Tests.Infra
{
    public class RepositoryTests
    {
        private readonly ILogger<Repository<Order>> _logger;

        public RepositoryTests()
        {
            _logger = Substitute.For<ILogger<Repository<Order>>>();
        }

        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllItems()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Order>(context, _logger);

            var ordersFake = new OrderFake().Generate(3);

            context.Orders.AddRange(ordersFake);
            await context.SaveChangesAsync();

            var orders = await repository.GetAllAsync();

            foreach (var orderFake in ordersFake)
                Assert.Contains(orders, o => o.Id == orderFake.Id);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewItem()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Order>(context, _logger);

            var orderFake = new OrderFake().Generate();

            await repository.AddAsync(orderFake);
            var orders = await repository.GetAllAsync();

            Assert.Contains(orders, o => o.Id == orderFake.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectItem()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Order>(context, _logger);

            var orderFake = new OrderFake().Generate();

            context.Orders.Add(orderFake);
            await context.SaveChangesAsync();

            var order = await repository.GetByKeysAsync(orderFake.Id);

            Assert.NotNull(order);
            Assert.Equal(order.OrderId, order.OrderId);
            Assert.Equal(order.ClientId, order.ClientId);
            Assert.Equal(order.Status, order.Status);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveItem()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Order>(context, _logger);

            var orderFake = new OrderFake().Generate();

            context.Orders.Add(orderFake);
            await context.SaveChangesAsync();

            await repository.DeleteAsync(orderFake.Id);
            var orders = await repository.GetAllAsync();

            Assert.DoesNotContain(orders, o => o.Id == orderFake.Id);
        }
    }
}
