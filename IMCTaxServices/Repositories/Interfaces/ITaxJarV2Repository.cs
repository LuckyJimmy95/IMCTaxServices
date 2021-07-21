using System.Threading.Tasks;
using Taxjar;

namespace IMCTaxServices.Repositories.Interfaces
{
    /// <summary>
    /// The interface for TaxJarV2Repository
    /// </summary>
    public interface ITaxJarV2Repository
    {
        /// <summary>
        /// Calls to retrieve the GET https://api.taxjar.com/v2/summary_rates data
        /// </summary>
        public Task<SummaryRatesResponse> GetSummaryTaxRatesByRegionV2Async();

        /// <summary>
        /// Calls to post the POST https://api.taxjar.com/v2/taxes data
        /// </summary>
        public Task<TaxResponse> PostGetTaxDataFromOrderV2Async(object orderInformation);
    }
}
