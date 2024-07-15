using FluentAssertions;
using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;
using Moq;
using Moq.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.UnitTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IFoodShareDbContext> _contextMock;

        private IOrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<IFoodShareDbContext>();

            _orderService = new OrderService(_contextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _contextMock = null;
        }

        [Test]
        public async Task CreateOrderAsync_IfOrderIsValid_ShouldSaveOrder()
        {
            // Arrange
            var order = GetOrderMockData();
            var donation = GetDonationMockData();

            _contextMock.Setup(x => x.Donations).ReturnsDbSet(new List<Donation> { donation });
            _contextMock.Setup(x => x.Orders).ReturnsDbSet(new List<Order> { order });

            // Act
            var result = await _orderService.CreateOrderAsync(order);

            // Assert
            _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
            Assert.That(result.Id, Is.Not.Null);

            result.DeliveryDate.Should()
                    .NotBeNull()
                    .And
                    .NotBeBefore(result.CreationDate)
                    .And
                    .BeAfter(Convert.ToDateTime("06/02/2024"));
        }

        [Test]
        public async Task CreateOrderAsync_IfOrderHasNoDonations_ShouldThrowNotFoundException()
        {
            // Arrange
            var order = GetOrderMockData();

            _contextMock.Setup(x => x.Donations).ReturnsDbSet(new List<Donation>());
            _contextMock.Setup(x => x.Orders).ReturnsDbSet(new List<Order> { order });

            // Act

            // Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(() => _orderService.CreateOrderAsync(order));
            Assert.That(exception?.Message, Is.Not.Null);
        }

        [TestCase(2)]
        [TestCase(10)]
        public async Task CreateOrderAsync_IfDonationQuantityIsSmmallerThanOrderQuantity_ShouldThrowOrderException(int quantity)
        {
            // Arrange
            var order = GetOrderMockData();
            order.Quantity = quantity;
            var donation = GetDonationMockData();

            _contextMock.Setup(x => x.Donations).ReturnsDbSet(new List<Donation> { donation });
            _contextMock.Setup(x => x.Orders).ReturnsDbSet(new List<Order> { order });

            // Act

            // Assert
            var exception = Assert.ThrowsAsync<OrderException>(() => _orderService.CreateOrderAsync(order));
            Assert.That(exception?.Message, Is.EqualTo("Requested quantity exceeds available quantity for Donation ID 900901."));
        }


        //public async Task<bool> updateOrderStatusAsync(int orderId, Domain.Enums.OrderStatus orderStatus)
        //{
        //    var order = await _context.Orders.FindAsync(orderId);

        //    if (order == null)
        //    {
        //        throw new NotFoundException($"Order with ID {orderId} not found.");
        //    }

        //    if (!_context.OrderStatuses.Any(s => s.Id == (int)orderStatus))
        //    {
        //        throw new NotFoundException($"Status with ID {(int)orderStatus} not found.");
        //    }

        //    order.OrderStatusId = (int)orderStatus;
        //    order.DeliveryDate = orderStatus == Domain.Enums.OrderStatus.Delivered ? DateTime.UtcNow : order.DeliveryDate;

        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        public async Task UpdateOrderStatusAsync_IfOrderIsNull_ShouldThrowNotFoundException(int orderId)
        {
            // Arrange
            var orderStatus = Domain.Enums.OrderStatus.Delivered;

            _contextMock.Setup(x => x.Orders.FindAsync(orderId)).ReturnsAsync((Order)null);

            // Act

            // Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(() => _orderService.UpdateOrderStatusAsync(orderId, orderStatus));
            Assert.That(exception?.Message, Is.EqualTo($"Order with ID {orderId} not found."));
        }

        [Test]
        public async Task UpdateOrderStatusAsync_IfStatusIsInvalid_ShouldThrowNotFoundException()
        {
            // Arrange
            var order = GetOrderMockData();
            var orderStatus = (Domain.Enums.OrderStatus)999; // Invalid status

            _contextMock.Setup(x => x.Orders.FindAsync(order.Id)).ReturnsAsync(order);
            _contextMock.Setup(x => x.OrderStatuses).ReturnsDbSet(new List<OrderStatus>());

            // Act

            // Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(() => _orderService.UpdateOrderStatusAsync(order.Id, orderStatus));
            Assert.That(exception?.Message, Is.EqualTo($"Status with ID {(int)orderStatus} not found."));
        }

        [Test]
        public async Task UpdateOrderStatusAsync_IfOrderAndStatusAreValid_ShouldReturnTrue()
        {
            // Arrange
            var order = GetOrderMockData();
            var orderStatus = Domain.Enums.OrderStatus.Delivered;
            var statuses = new List<OrderStatus>
            {
                new OrderStatus { Id = (int)Domain.Enums.OrderStatus.Delivered, Name = "Delivered" }
            };

            _contextMock.Setup(x => x.Orders.FindAsync(order.Id)).ReturnsAsync(order);
            _contextMock.Setup(x => x.OrderStatuses).ReturnsDbSet(statuses);

            // Act
            var result = await _orderService.UpdateOrderStatusAsync(order.Id, orderStatus);

            // Assert
            _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
            Assert.That(result, Is.True);
            Assert.That(order.OrderStatusId, Is.EqualTo((int)orderStatus));
            if (orderStatus == Domain.Enums.OrderStatus.Delivered)
            {
                Assert.That(order.DeliveryDate, Is.EqualTo(DateTime.UtcNow).Within(1).Seconds);
            }
        }


        private static Order GetOrderMockData()
        {
            return new Order()
            {
                Id = 100,
                CourierId = 88231,
                BeneficiaryId = 7007001,
                Quantity = 1,
                CreationDate = Convert.ToDateTime("06.01.2024"),
                DeliveryDate = Convert.ToDateTime("06.03.2024"),
                DonationId = 900901,
                Donation = GetDonationMockData()
            };
        }

        private static Donation GetDonationMockData()
        {
            return new Donation
            {
                Id = 900901,
                DonorId = 1113,
                Status = new DonationStatus { Id = 923, Name = "Available" },
                Quantity = 1,
                ExpirationDate = Convert.ToDateTime("07.01.2024"),
                ProductId = 55123,
            };
        }
    }


}
