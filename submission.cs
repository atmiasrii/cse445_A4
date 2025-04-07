using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Net; // Add this for WebClient



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Program
    {
        public static string xmlURL = "https://atmiasrii.github.io/cse445_A4/Hotels.xml";
        public static string xmlErrorURL = "https://atmiasrii.github.io/cse445_A4/HotelsErrors.xml";
        public static string xsdURL = "https://atmiasrii.github.io/cse445_A4/Hotels.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsdUrl));

                string errorMsg = "No Error";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errorMsg = $"Validation error: {e.Message}";
                };

                XmlReader reader = XmlReader.Create(xmlUrl, settings);
                while (reader.Read()) { }

                return errorMsg;
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }


            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
        }

        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string xmlContent = client.DownloadString(xmlUrl);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);

                    // Use JsonSerializerSettings to preserve the root element
                    string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, false);
                    return jsonText;
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }

            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
        }
    }

}
