using Commons.Xml.Relaxng;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NASA_XSD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {

        [HttpPost]
        [Route("XSD")]
        public string ValidateXSD([FromBody] XElement request)
        {

            try
            {
                XDocument doc = XDocument.Parse(request.ToString());
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add("", "validation/NasaXSD.xsd");
                doc.Validate(schema, ValidationEventHandler);
                return "validation succeeded";

            }
            catch (Exception e)
            {
                return e.Message;
            }

        }


        [HttpPost]
        [Route("RNG")]
        public string ValidateRNG([FromBody] XElement request)
        {
            try
            {
                XDocument doc = XDocument.Parse(request.ToString());
                XmlReader instance = doc.CreateReader();
                XmlReader grammar = new XmlTextReader("validation/NasaRNG.rng");

                using RelaxngValidatingReader reader = new RelaxngValidatingReader(instance, grammar);

                while (!reader.EOF)
                {
                    reader.Read();
                }
                return "validation succeeded";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }
        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }
    }
}
