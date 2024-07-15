using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using FoodShareNet.Application

namespace FoodShareNet.UnitTest
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IFoodShareDbContext> _contextMock;
        private Mock<IOrderService> _orderService;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<IFoodShareDbContext>();
            _orderService = new OrderService(_contextMock.Object);
        }

        [Test]
    }
}

//public async task<bool> updateorderstatusasync(int orderid, domain.enums.orderstatus orderstatus)
//{
//    var order = await _context.orders.findasync(orderid);

//    if (order == null)
//    {
//        throw new notfoundexception($"order with id {orderid} not found.");
//    }

//    if (!_context.orderstatuses.any(s => s.id == (int)orderstatus))
//    {
//        throw new notfoundexception($"status with id {(int)orderstatus} not found.");
//    }

//    order.orderstatusid = (int)orderstatus;
//    order.deliverydate = orderstatus == domain.enums.orderstatus.delivered ? datetime.utcnow : order.deliverydate;

//    await _context.savechangesasync();

//    return true;
//}