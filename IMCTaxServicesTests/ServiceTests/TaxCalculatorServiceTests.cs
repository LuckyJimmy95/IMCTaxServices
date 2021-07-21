using IMCTaxServices.Repositories.Interfaces;
using IMCTaxServices.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Taxjar;

namespace IMCTaxServicesTests.TaxCalculatorServiceTests
{
    /// <summary>
    /// Test class for <see cref="TaxCalculatorService"/>
    /// </summary>
    [TestFixture]
    public class TaxCalculatorServiceTests
    {
        private TaxCalculatorService _taxCalculatorService { get; set; }
        private Mock<ILogger<TaxCalculatorService>> _loggerMock = new Mock<ILogger<TaxCalculatorService>>();
        private Mock<ITaxJarV2Repository> _taxJarV2RepoMock = new Mock<ITaxJarV2Repository>();

        [SetUp]
        public void Setup()
        {
            _taxCalculatorService = new TaxCalculatorService(_loggerMock.Object, _taxJarV2RepoMock.Object);
        }

        /// <summary>
        ///  <see cref="TaxCalculatorService.GetSummaryTaxRatesByRegion"/> returns success multiple objects
        /// </summary>
        [Test]
        public async Task GetSummaryTaxRatesByRegionReturnsSuccess()
        {
            var regionSummaries = new SummaryRatesResponse { SummaryRates = new List<SummaryRate> 
                { 
                    new SummaryRate { AverageRate = new SummaryRateObject(), Country = "USA", CountryCode = "US", MinimumRate = new SummaryRateObject(), Region = "test", RegionCode = "test" },
                    new SummaryRate { AverageRate = new SummaryRateObject(), Country = "USA", CountryCode = "US", MinimumRate = new SummaryRateObject(), Region = "test", RegionCode = "test" },
                    new SummaryRate { AverageRate = new SummaryRateObject(), Country = "USA", CountryCode = "US", MinimumRate = new SummaryRateObject(), Region = "test", RegionCode = "test" }
                } 
            };
            _taxJarV2RepoMock.Setup(mock => mock.GetSummaryTaxRatesByRegionV2Async()).ReturnsAsync(regionSummaries);
            var result = await _taxCalculatorService.GetSummaryTaxRatesByRegion();
            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(3, result.SummaryRates.Count, "Incorrect count of items returned");
        }

        /// <summary>
        ///  <see cref="TaxCalculatorService.GetSummaryTaxRatesByRegion"/> returns success single object
        /// </summary>
        [Test]
        public async Task GetSummaryTaxRatesByRegionReturnsSingleSuccess()
        {
            var regionSummaries = new SummaryRatesResponse
            {
                SummaryRates = new List<SummaryRate>
                {
                    new SummaryRate { AverageRate = new SummaryRateObject(), Country = "USA", CountryCode = "US", MinimumRate = new SummaryRateObject(), Region = "test", RegionCode = "test" }
                }
            };
            _taxJarV2RepoMock.Setup(mock => mock.GetSummaryTaxRatesByRegionV2Async()).ReturnsAsync(regionSummaries);
            var result = await _taxCalculatorService.GetSummaryTaxRatesByRegion();
            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(1, result.SummaryRates.Count, "Incorrect count of items returned");
        }

        /// <summary>
        ///  <see cref="TaxCalculatorService.GetSummaryTaxRatesByRegion"/> returns success empty object
        /// </summary>
        [Test]
        public async Task GetSummaryTaxRatesByRegionReturnsEmptySuccess()
        {
            var regionSummaries = new SummaryRatesResponse
            {
                SummaryRates = new List<SummaryRate>()
            };
            _taxJarV2RepoMock.Setup(mock => mock.GetSummaryTaxRatesByRegionV2Async()).ReturnsAsync(regionSummaries);
            var result = await _taxCalculatorService.GetSummaryTaxRatesByRegion();
            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(0, result.SummaryRates.Count, "Incorrect count of items returned");
        }

        /// <summary>
        /// <see cref="TaxCalculatorService.GetSummaryTaxRatesByRegion"/> throws exception
        /// </summary>
        [Test]
        public void GetSummaryTaxRatesByRegionThrowsException()
        {
            var exception = new HttpResponseException(new HttpResponseMessage());

            _taxJarV2RepoMock.Setup(mock => mock.GetSummaryTaxRatesByRegionV2Async()).ThrowsAsync(exception);

            Assert.ThrowsAsync<HttpResponseException>(() => _taxCalculatorService.GetSummaryTaxRatesByRegion(), "Exception did not throw properly");
        }

        /// <summary>
        ///  <see cref="TaxCalculatorService.GetTaxInfoByOrder()"/> returns success using object instantiator null param
        /// </summary>
        [Test]
        public async Task GetOrderTaxInfoNullParamReturnsSuccess()
        {
            _taxJarV2RepoMock.Setup(mock => mock.PostGetTaxDataFromOrderV2Async(It.Is<object>(o => o.ToString().Length == 474))).ReturnsAsync(new TaxResponse());
            var result = await _taxCalculatorService.GetTaxInfoByOrder();
            Assert.IsNotNull(result, "Result is null");
            Assert.IsInstanceOf(typeof(TaxResponse), result, "Incorrect object");
        }

        /// <summary>
        ///  <see cref="TaxCalculatorService.GetTaxInfoByOrder()"/> returns success using object instantiator
        /// </summary>
        [Test]
        public async Task GetOrderTaxInfoReturnsSuccess()
        {
            var regionSummaries = new object {};
            _taxJarV2RepoMock.Setup(mock => mock.PostGetTaxDataFromOrderV2Async(It.Is<object>(o => o.ToString().Length == 474))).ReturnsAsync(new TaxResponse());
            var result = await _taxCalculatorService.GetTaxInfoByOrder(regionSummaries);
            Assert.IsNotNull(result, "Result is null");
            Assert.IsInstanceOf(typeof(TaxResponse), result, "Incorrect object");
        }

        /// <summary>
        /// <see cref="TaxCalculatorService.GetTaxInfoByOrder()"/> throws exception
        /// </summary>
        [Test]
        public void GetOrderTaxInfoThrowsException()
        {
            var exception = new HttpResponseException(new HttpResponseMessage());

            _taxJarV2RepoMock.Setup(mock => mock.PostGetTaxDataFromOrderV2Async(It.IsAny<object>())).ThrowsAsync(exception);

            Assert.ThrowsAsync<HttpResponseException>(() => _taxCalculatorService.GetTaxInfoByOrder(), "Exception did not throw properly");
        }
    }
}