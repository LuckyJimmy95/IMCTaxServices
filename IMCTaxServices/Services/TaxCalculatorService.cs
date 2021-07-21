using IMCTaxServices.Repositories.Interfaces;
using IMCTaxServices.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Taxjar;

namespace IMCTaxServices.Services
{
    /// <summary>
    /// The tax calculator service that handles business logic for analytics based functionality
    /// </summary>
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly ITaxJarV2Repository _taxJarV2Repository;
        private readonly ILogger<TaxCalculatorService> _logger;

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="logger"></param>
        public TaxCalculatorService(ILogger<TaxCalculatorService> logger, ITaxJarV2Repository taxJarRepository)
        {
            _logger = logger;
            _taxJarV2Repository = taxJarRepository;
        }

        /// <summary>
        /// Handles the retrieval of all regions summary tax rates
        /// </summary>
        public async Task<SummaryRatesResponse> GetSummaryTaxRatesByRegion()
        {
            SummaryRatesResponse taxInfoByRegion = null;
            try
            {
                taxInfoByRegion = await _taxJarV2Repository.GetSummaryTaxRatesByRegionV2Async();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }

            return taxInfoByRegion;
        }

        /// <summary>
        /// Handles the retrieval of getting tax information for an order that is passed in
        /// </summary>
        public async Task<TaxResponse> GetTaxInfoByOrder(object orderInfo = null)
        {
            TaxResponse taxInfoByOrder = null;
            try
            {
                var orderObject = orderInfo == null || orderInfo.ToString() == "{}" || orderInfo.ToString() == "System.Object" ? CreateOrderObject() : orderInfo;
                taxInfoByOrder = await _taxJarV2Repository.PostGetTaxDataFromOrderV2Async(orderObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }

            return taxInfoByOrder;
        }

        private object CreateOrderObject()
        {
            object order = new
            {
                from_country = "US",
                from_zip = "92093",
                from_state = "CA",
                from_city = "La Jolla",
                from_street = "9500 Gilman Drive",
                to_country = "US",
                to_zip = "90002",
                to_state = "CA",
                to_city = "Los Angeles",
                to_street = "1335 E 103rd St",
                amount = 15,
                shipping = 1.5,
                nexus_addresses = new[] {
                    new {
                      id = "Main Location",
                      country = "US",
                      zip = "92093",
                      state = "CA",
                      city = "La Jolla",
                      street = "9500 Gilman Drive",
                    }
                  },
                line_items = new[] {
                    new {
                      id = "1",
                      quantity = 1,
                      product_tax_code = "20010",
                      unit_price = 15,
                      discount = 0
                    }
                  }
            };
            return order;
        }
    }
}
