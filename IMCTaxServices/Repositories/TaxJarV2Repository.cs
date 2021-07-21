using IMCTaxServices.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Taxjar;

namespace IMCTaxServices.Repositories
{
    /// <summary>
    /// The TaxJarV2Repository that handles external http requests for TaxJar data
    /// </summary>
    public class TaxJarV2Repository : ITaxJarV2Repository
    {
        private const string basePrefixUri = "https://api.taxjar.com/v2/";
        private const string summaryRatesUri = "summary_rates";
        private const string taxesUri = "taxes";

        private const string userKey = "5da2f821eee4035db4771edab942a4cc";
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<TaxJarV2Repository> _logger;

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="logger"></param>
        public TaxJarV2Repository(ILogger<TaxJarV2Repository> logger)
        {
            _logger = logger;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userKey);
        }

        /// <summary>
        /// Calls to retrieve the GET https://api.taxjar.com/v2/summary_rates data
        /// </summary>
        public async Task<SummaryRatesResponse> GetSummaryTaxRatesByRegionV2Async()
        {
            var uri = basePrefixUri + summaryRatesUri;
            _logger.LogInformation(nameof(GetSummaryTaxRatesByRegionV2Async) + " making repository call to " + uri);
            var response = await client.GetAsync(uri);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError(nameof(GetSummaryTaxRatesByRegionV2Async) + " got " + response.StatusCode + " error with " + uri);

                throw new HttpResponseException(response);
            }

            var result = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<SummaryRatesResponse>(result);

            return events;
        }

        /// <summary>
        /// Calls to post the POST https://api.taxjar.com/v2/taxes data
        /// </summary>
        public async Task<TaxResponse> PostGetTaxDataFromOrderV2Async(object orderInformation)
        {
            var orderInfoString = JsonConvert.SerializeObject(orderInformation);
            var uri = basePrefixUri + taxesUri;
            _logger.LogInformation(nameof(PostGetTaxDataFromOrderV2Async) + " making repository call to " + uri);

            var content = new StringContent(orderInfoString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError(nameof(PostGetTaxDataFromOrderV2Async) + " got " + response.StatusCode + " error with " + uri + " and payload: " + orderInfoString);

                throw new HttpResponseException(response);
            }

            var result = await response.Content.ReadAsStringAsync();
            var taxFromOrder = JsonConvert.DeserializeObject<TaxResponse>(result);

            return taxFromOrder;
        }

    }
}
