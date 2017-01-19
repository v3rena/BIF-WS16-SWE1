using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class Address
    {
        public string City { get; internal set; }
        public string Street { get; internal set; }
        public string PostCode { get; internal set; }

        /// <summary>
        /// Checks whether a city-street-pair is found.
        /// </summary>
        /// <returns>true or false.</returns>
        public bool isComplete()
        {
            if (!String.IsNullOrEmpty(City) && !String.IsNullOrEmpty(Street))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
