using IMCTaxServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IMCTaxServices.Controllers
{
    /// <summary>
    /// The main analytics controller handling any analytics data requests
    /// </summary>
    [ApiController]
    public class TaxCalculatorV1Controller : ControllerBase
    {
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly ILogger<TaxCalculatorV1Controller> _logger;

        private const string baseRoutePrefix = "taxCalculator/";

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="logger"></param>
        public TaxCalculatorV1Controller(ILogger<TaxCalculatorV1Controller> logger, ITaxCalculatorService taxCalculatorService)
        {
            _logger = logger;
            _taxCalculatorService = taxCalculatorService;
        }

        /// <summary>
        /// Get list of region summary tax information
        /// </summary>
        [HttpGet, Route(baseRoutePrefix + "regionSummary")]
        public async Task<ActionResult> GetRegionSummaries()
        {
            _logger.LogInformation("Performing get tax rates by region task.");
            var summaries = await _taxCalculatorService.GetSummaryTaxRatesByRegion();

            return Ok(summaries);
        }

        /// <summary>
        /// Get tax information from order. Pass in empty json "{}" to get default value.
        /// </summary>
        [HttpPost, Route(baseRoutePrefix + "order")]
        public async Task<ActionResult> GetOrderTaxInformation([FromBody] object? orderInfo = null)
        {
            _logger.LogInformation("Performing get order tax information task.");
            var orderTaxInfo = await _taxCalculatorService.GetTaxInfoByOrder(orderInfo);

            return base.Ok(orderTaxInfo);
        }
    }
}
