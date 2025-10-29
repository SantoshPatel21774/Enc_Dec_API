using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.KeyIvModel
{
    public class KeyIVResponse
    {
        // <summary>
        /// encrypted result after encrypted.
        /// </summary>
        public string? Response { get; set; }
        public int StatusCode { get; set; }
    }
}
