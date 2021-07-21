using IMCTaxServices.Controllers;
using IMCTaxServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxjar;

namespace IMCTaxServicesTests.ControllerTests
{
    /// <summary>
    /// Test class for <see cref="TaxCalculatorV1Controller"/>
    /// </summary>
    [TestFixture]
    public class TaxCalculatorV1ControllerTests
    {
        private TaxCalculatorV1Controller _taxCalculatorV1Controller { get; set; }
        private Mock<ILogger<TaxCalculatorV1Controller>> _loggerMock = new Mock<ILogger<TaxCalculatorV1Controller>>();
        private Mock<ITaxCalculatorService> _taxCalculatorService = new Mock<ITaxCalculatorService>();

        [SetUp]
        public void Setup()
        {
            _taxCalculatorV1Controller = new TaxCalculatorV1Controller(_loggerMock.Object, _taxCalculatorService.Object);
        }

        /// <summary>
        ///  <see cref="TaxCalculatorV1Controller.GetRegionSummaries"/> returns success
        /// </summary>
        [Test]
        public async Task GetSummaryTaxRatesByRegionReturnsSuccess()
        {
            var regionSummaries = new SummaryRatesResponse
            {
                SummaryRates = new List<SummaryRate>
                {
                    new SummaryRate { AverageRate = new SummaryRateObject(), Country = "USA", CountryCode = "US", MinimumRate = new SummaryRateObject(), Region = "test", RegionCode = "test" },
                    new SummaryRate { AverageRate = new SummaryRateObject(), Country = "USA", CountryCode = "US", MinimumRate = new SummaryRateObject(), Region = "test", RegionCode = "test" },
                    new SummaryRate { AverageRate = new SummaryRateObject(), Country = "USA", CountryCode = "US", MinimumRate = new SummaryRateObject(), Region = "test", RegionCode = "test" }
                }
            };
            _taxCalculatorService.Setup(mock => mock.GetSummaryTaxRatesByRegion()).ReturnsAsync(regionSummaries);
            var result = await _taxCalculatorV1Controller.GetRegionSummaries() as OkObjectResult;
            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(3, (result.Value as SummaryRatesResponse).SummaryRates.Count, "Incorrect count of items returned");
            _taxCalculatorService.Verify(mock => mock.GetSummaryTaxRatesByRegion(), Times.Once(), "Get region info was not called once.");
        }

        /// <summary>
        ///  <see cref="TaxCalculatorV1Controller.GetOrderTaxInformation"/> returns success
        /// </summary>
        [Test]
        public async Task GetOrderReturnsSingleSuccess()
        {
            _taxCalculatorService.Setup(mock => mock.GetTaxInfoByOrder(It.IsAny<object>())).ReturnsAsync(new TaxResponse());
            var result = await _taxCalculatorV1Controller.GetOrderTaxInformation() as OkObjectResult;
            Assert.IsNotNull(result, "Result is null");
            Assert.IsNotNull(result.Value as TaxResponse, "null tax object");
            _taxCalculatorService.Verify(mock => mock.GetTaxInfoByOrder(It.IsAny<object>()), Times.Once(), "Get order info was not called once.");
        }
    }
}
