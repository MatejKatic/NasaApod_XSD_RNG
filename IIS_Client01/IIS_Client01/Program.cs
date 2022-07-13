using System;
using System.IO;
using System.Net;
using System.Xml;

namespace IIS_Client01
{
    class Program
    {
        static void Main(string[] args)
        {
            string requestXml = System.IO.File.ReadAllText(@"C:\Users\GraphX\Desktop\IIS_Client01\IIS_Client01\NasaApod.xml");
            Console.WriteLine("Type XML validation method (XSD or RNG):");
            string method = Console.ReadLine();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44326/api/Validate/" + method);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "application/xml";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                Console.WriteLine(responseStr);
            }
        }
    }
}
