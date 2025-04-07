using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Net; 

// This is for Assignment 4 - XML processing and validation
// Made by Atmia Srivastava for CSE445

namespace ConsoleApp1
{
    public class Program
    {
        // URLs to my GitHub hosted files
        public static string xmlURL = "https://atmiasrii.github.io/cse445_A4/Hotels.xml";
        public static string xmlErrorURL = "https://atmiasrii.github.io/cse445_A4/HotelsErrors.xml";
        public static string xsdURL = "https://atmiasrii.github.io/cse445_A4/Hotels.xsd";

        public static void Main(string[] args)
        {
            // Test XML validation - should pass
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // Test XML with errors - should fail
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Convert XML to JSON
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Checks if XML is valid according to schema
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                // Setup validation settings
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsdUrl));

                // Start with no errors
                string errorMsg = "No Error";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    // If error occurs, save the message
                    errorMsg = $"Validation error: {e.Message}";
                };

                // Process XML file
                XmlReader reader = XmlReader.Create(xmlUrl, settings);
                while (reader.Read()) { }

                return errorMsg;
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        // Converts XML to JSON format
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Download XML content
                    string xmlContent = client.DownloadString(xmlUrl);

                    // Parse XML
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);

                    // Convert to JSON (keep root element)
                    string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, false);
                    return jsonText;
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}
