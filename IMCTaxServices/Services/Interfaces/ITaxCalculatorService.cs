
using System.Threading.Tasks;
using Taxjar;

namespace IMCTaxServices.Services.Interfaces
{
    /// <summary>
    /// Interface for tax calculator service
    /// </summary>
    public interface ITaxCalculatorService
    {
        /// <summary>
        /// Handles the retrieval of all regions summary tax rates
        /// </summary>
        public Task<SummaryRatesResponse> GetSummaryTaxRatesByRegion();

        /// <summary>
        /// Handles the retrieval of getting tax information for an order that is passed in
        /// </summary>
        public Task<TaxResponse> GetTaxInfoByOrder(object orderInfo = null);
    }
}
