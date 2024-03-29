using Application.DataAccess;
using Application.Models;
using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class InvoiceServiceUnitTests
    {
        [Fact]
        public async Task GetInvoiceById_ReturnsCorrectInvoice()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<InvoiceService>>();
            var mockRepository = new Mock<IInvoiceRepository>();
            var expectedInvoice = new Receipt { ReceiptId = 1, PersonalAccount = "Account123" };
            mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedInvoice);
            var invoiceService = new InvoiceService(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await invoiceService.GetInvoiceById(1);

            // Assert
            Assert.Equal(expectedInvoice, result);
        }

        [Fact]
        public async Task GetInvoiceById_ThrowsExceptionIfInvoiceNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<InvoiceService>>();
            var mockRepository = new Mock<IInvoiceRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Invoice not found"));
            var invoiceService = new InvoiceService(mockRepository.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await invoiceService.GetInvoiceById(1));
        }

        [Fact]
        public async Task GetList_ReturnsListOfInvoices()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<InvoiceService>>();
            var mockRepository = new Mock<IInvoiceRepository>();
            var expectedInvoices = new List<Receipt>
            {
                new Receipt { ReceiptId = 1, PersonalAccount = "Account123" },
                new Receipt { ReceiptId = 2, PersonalAccount = "Account456" }
            };
            mockRepository.Setup(repo => repo.All()).ReturnsAsync(expectedInvoices);
            var invoiceService = new InvoiceService(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await invoiceService.GetList();

            // Assert
            Assert.Equal(expectedInvoices, result);
        }

        [Fact]
        public async Task GetList_ThrowsExceptionIfFailedToRetrieveInvoices()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<InvoiceService>>();
            var mockRepository = new Mock<IInvoiceRepository>();
            mockRepository.Setup(repo => repo.All()).ThrowsAsync(new Exception("Failed to retrieve invoices"));
            var invoiceService = new InvoiceService(mockRepository.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await invoiceService.GetList());
        }
    }
}
