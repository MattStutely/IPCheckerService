using System.ServiceModel;
using System.ServiceModel.Web;

namespace ServiceLibrary
{
    [ServiceContract]
    public interface IIPCheckerService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Xml, UriTemplate = "ValidateIP.xml")]
        bool ValidateIPXml();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "ValidateIP.json")]
        bool ValidateIPJson();


    }
}