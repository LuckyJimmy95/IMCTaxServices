using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMCTaxServices.Models
{
    /// <summary>
    /// Class with user session information
    /// </summary>
    public class UserSessionInformation
    {
        /// <summary>
        /// visitor id of event
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// List of user session
        /// </summary>
        public List<UserSession> Sessions { get; set; }
    }
}
