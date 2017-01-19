using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using System.IO;

namespace MyWebServer
{
    public class SaxParser
    {
        /// <summary>
        /// Opens OSM file and calls ReadOsm().
        /// </summary>
        /// <param name="streetValue"></param>
        /// <param name="Dictionary"></param>
        public static void Update(string streetValue, NaviDictionary Dictionary)
        {
            var file = "C:\\Users\\Verena\\Desktop\\SWE1\\faroe-islands-latest.osm";
            //var file = "E:\\FH\\WS2016\\SWE\\SWE1-CS\\faroe-islands-latest.osm";
            using (var fs = File.OpenRead(file))
            using (var xml = new System.Xml.XmlTextReader(fs))
            {
                while (xml.Read())
                {
                    if (xml.NodeType == System.Xml.XmlNodeType.Element
                     && xml.Name == "osm")
                    {
                        ReadOsm(xml, streetValue, Dictionary.AddressDictionary);
                    }
                }
            }
        }

        /// <summary>
        /// Opens OSM file and calls function that reads OSM.
        /// </summary>
        /// <param name="Dictionary"></param>
        public static void Update(NaviDictionary Dictionary)
        {
            var file = "C:\\Users\\Verena\\Desktop\\SWE1\\faroe-islands-latest.osm";
            //var file = "E:\\FH\\WS2016\\SWE\\SWE1-CS\\faroe-islands-latest.osm";
            using (var fs = File.OpenRead(file))
            using (var xml = new System.Xml.XmlTextReader(fs))
            {
                while (xml.Read())
                {
                    if (xml.NodeType == System.Xml.XmlNodeType.Element
                     && xml.Name == "osm")
                    {
                        ReadOsm(xml, Dictionary);
                    }
                }
            }
        }

        /// <summary>
        /// Reads OSM and calls function to read nodes or ways.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="streetValue"></param>
        /// <param name="AddressDictionary"></param>
        private static void ReadOsm(System.Xml.XmlTextReader xml, string streetValue, IDictionary<string, string> AddressDictionary)
        {
            using (var osm = xml.ReadSubtree())
            {
                while (osm.Read())
                {
                    if (osm.NodeType == System.Xml.XmlNodeType.Element
                    && (osm.Name == "node" || osm.Name == "way"))
                    {
                        ReadAnyOsmElement(osm, streetValue, AddressDictionary);
                    }
                }
            }
        }

        /// <summary>
        /// Reads OSM and calls function to read nodes or ways.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="Dictionary"></param>
        private static void ReadOsm(System.Xml.XmlTextReader xml, NaviDictionary Dictionary)
        {
            using (var osm = xml.ReadSubtree())
            {
                while (osm.Read())
                {
                    if (osm.NodeType == System.Xml.XmlNodeType.Element
                    && (osm.Name == "node" || osm.Name == "way"))
                    {
                        ReadAnyOsmElement(osm, Dictionary);
                    }
                }
            }
        }

        /// <summary>
        /// Reads nodes or ways and calls function that reads tags. If a matching city to the inquired street is found, the pair gets added to AddressDictionary.
        /// </summary>
        /// <param name="osm"></param>
        /// <param name="streetValue"></param>
        /// <param name="AddressDictionary"></param>
        private static void ReadAnyOsmElement(System.Xml.XmlReader osm, string streetValue, IDictionary<string, string> AddressDictionary)
        {
            using (var element = osm.ReadSubtree())
            {
                Address address = new Address();
                while (element.Read())
                {
                    if (element.NodeType == System.Xml.XmlNodeType.Element
                     && element.Name == "tag")
                    {
                        ReadTag(element, address);

                        if (address.isComplete())
                        {
                            if (address.Street.Equals(streetValue))
                            {
                                if (!AddressDictionary.ContainsKey(address.City))
                                {
                                    AddressDictionary.Add(address.City, address.Street);
                                }                                
                            }
                        }
                    }                    
                }
            }
        }

        /// <summary>
        /// Reads nodes or ways and calls function that reads tags.
        /// </summary>
        /// <param name="osm"></param>
        /// <param name="Dictionary"></param>
        private static void ReadAnyOsmElement(System.Xml.XmlReader osm, NaviDictionary Dictionary)
        {
            using (var element = osm.ReadSubtree())
            {
                Address address = new Address();
                while (element.Read())
                {
                    if (element.NodeType == System.Xml.XmlNodeType.Element
                     && element.Name == "tag")
                    {
                        ReadTag(element, address);

                        if (address.isComplete())
                        {
                            Dictionary.AddItem(address.City, address.Street);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads tags and adds them to Address.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="a"></param>
        private static void ReadTag(System.Xml.XmlReader element, Address a)
        {
            string tagType = element.GetAttribute("k");
            string value = element.GetAttribute("v");

            switch (tagType)
            {
                case "addr:city":
                    a.City = value;
                    break;
                case "addr:postcode":
                    a.PostCode = value;
                    break;
                case "addr:street":
                    a.Street = value;
                    break;
            }                 
        }
    }
}
