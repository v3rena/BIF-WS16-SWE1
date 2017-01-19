using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer
{
    public class NaviDictionary
    {
        /// <summary>
        /// Contains city-street-pairs.
        /// </summary>
        public IDictionary<string, string> AddressDictionary { get; } = new Dictionary<string, string>();

        public string GetCities()
        {
            string citiesFound = "\n=>";

            foreach (KeyValuePair<string, string> kvp in AddressDictionary)
            {
                citiesFound = String.Concat(citiesFound, "\n", kvp.Key);
            }
            return citiesFound;
        }


    /// <summary>
    /// Contains a list of streets for every city.
    /// </summary>
    public IDictionary<string, List<string>> StreetListDictionary { get; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Adds city-street-pairs to Dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddItem(string key, string value)
        {
            if (this.StreetListDictionary.ContainsKey(key))
            {
                List<string> list = this.StreetListDictionary[key];
                if (list.Contains(value) == false)
                {
                    list.Add(value);
                }
            }
            else
            {
                List<string> list = new List<string>();
                list.Add(value);
                this.StreetListDictionary.Add(key, list);
            }
        }
    }
}
