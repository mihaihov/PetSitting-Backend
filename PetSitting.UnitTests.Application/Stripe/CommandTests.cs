using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using PetSitting.Application.Features.Stripe.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Stripe;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;

namespace PetSitting.UnitTests.Application.Stripe
{
    public class CommandTests
    {
        [Fact]
        public async Task HandleCommand_ShouldUpdateTransaction_WhenDataIsValid()
        {
            // Arrange
            var mockStripeTransactionRepository = new Mock<IStripeTransactionRepository>();

            var transactionId = "testTransactionId";
            var paymentIntentId = "testPaymentIntentId";
            var status = "succeeded";
            var amount = 10000m; // cents
            var currency = "usd";
            var createdAt = DateTime.Now;
            var paidTo = "testPaidTo";
            var paidBy = "testPaidBy";

            var command = new CreateTransactionCommand(
                paymentIntentId,
                status,
                amount,
                currency,
                createdAt,
                paidTo,
                paidBy
            );

            var existingTransaction = new StripeTransaction
            {
                Id = transactionId,
                PaymentIntentId = null,
                Status = null,
                Amount = null,
                Currency = null,
                CreatedAt = DateTime.MinValue,
                PaidToId = null,
                PaidById = null
            };

            mockStripeTransactionRepository
                .Setup(repo => repo.GetByPaymentIntentId(paymentIntentId))
                .ReturnsAsync(existingTransaction);

            mockStripeTransactionRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<StripeTransaction>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateTransactionCommandHandler(mockStripeTransactionRepository.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockStripeTransactionRepository.Verify(repo => repo.GetByPaymentIntentId(It.IsAny<string>()), Times.Once);
            mockStripeTransactionRepository.Verify(repo => repo.UpdateAsync(It.Is<StripeTransaction>(t =>
                t.PaymentIntentId == paymentIntentId &&
                t.Status == status &&
                t.Amount == amount / 100m &&
                t.Currency == currency &&
                t.CreatedAt == createdAt &&
                t.PaidToId == paidTo &&
                t.PaidById == paidBy
            )), Times.Once);

            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Null(response.ValidationErrors);
        }
    }
}