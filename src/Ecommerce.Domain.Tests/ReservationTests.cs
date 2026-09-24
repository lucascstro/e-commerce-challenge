using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Enum;
using Ecommerce.Domain.Exceptions;
using Xunit;

namespace Ecommerce.Domain.Tests
{
    public class ReservationTests
    {
        [Fact]
        public void Reservation_Should_Have_A_Valid_Id()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var reservation = new Reservation(customerId, productId);
            Assert.IsType<Guid>(reservation.ReservationId);
        }

        [Fact]
        public void Reservation_Should_Have_A_Valid_CustomerId()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var reservation = new Reservation(customerId, productId);
            Assert.Equal(customerId, reservation.CustomerId);
        }

        [Fact]
        public void Reservation_Should_Have_A_Valid_ProductId()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var reservation = new Reservation(customerId, productId);
            Assert.Equal(productId, reservation.ProductId);
        }

        [Fact]
        public void Reservation_Should_Be_Invalid_When_CustomerId_Is_Empty()
        {
            var customerId = Guid.Empty;
            var productId = Guid.NewGuid();
            Assert.Throws<DomainException>(() => new Reservation(customerId, productId));
        }

        [Fact]
        public void Reservation_Should_Be_Invalid_When_ProductId_Is_Empty()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.Empty;
            Assert.Throws<DomainException>(() => new Reservation(customerId, productId));
        }

        [Fact]
        public void Reservation_Should_Have_A_Valid_Status()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var reservation = new Reservation(customerId, productId);
            Assert.Equal(Status.Active, reservation.Status);
        }

        [Fact]
        public void Reservation_Should_Be_Expired_When_Status_Is_Updated()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var reservation = new Reservation(customerId, productId);
            reservation.UpdateStatus(Status.Expired);
            Assert.Equal(Status.Expired, reservation.Status);
        }
    }
}