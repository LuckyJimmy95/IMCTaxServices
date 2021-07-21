using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMCTaxServices.Models
{
    /// <summary>
    /// Class with user session information
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// timestamp start time of the session
        /// </summary>
        public double TimestampStartTime { get; set; }

        /// <summary>
        /// Duration time of the session
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// pages session accessed
        /// </summary>
        public List<string> Pages { get; set; }
    }
}
