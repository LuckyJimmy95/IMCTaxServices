
namespace IMCTaxServices.Models
{
    /// <summary>
    /// The event model
    /// </summary>
    public class Event
    {
        /// <summary>
        /// url of event
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// visitor id of event
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// timestamp of event
        /// </summary>
        public double Timestamp { get; set; }
    }
}
