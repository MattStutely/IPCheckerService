using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace IPChecker.Services.ServiceAPI.Utility.Results
{
    public class XmlResult : BaseResult
    {
        public XmlResult(object objectToSerialize)
        {
            Content = new StringContent(SerializeContent(objectToSerialize));
            Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
        }

        private string SerializeContent(object objectToSerialize)
        {
            var serializer = new XmlSerializer(objectToSerialize.GetType());
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, objectToSerialize);
                return stringWriter.ToString();
            }
        }
    }
}
