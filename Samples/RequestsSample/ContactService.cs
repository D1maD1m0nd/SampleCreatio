using System.Net;

namespace Samples;

public class ContactService
{
    private const string ServiceUrl = "http://localhost:89/0/ServiceModel/ProcessEngineService.svc/ItqGetContact/Execute?ResultParameterName=ContactList";
    private readonly AuthManager authManager;

    public ContactService(AuthManager authManager)
    {
        this.authManager = authManager;
    }

    public string GetAllContacts()
    {
        var request = (HttpWebRequest)WebRequest.Create(ServiceUrl);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.CookieContainer = authManager._cookieContainer;
        request.Headers.Add("BPMCSRF", authManager.BpmcsrfToken);

        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        catch (WebException ex)
        {
            return null;
        }
    }
}