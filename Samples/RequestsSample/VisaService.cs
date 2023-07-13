using System.Net;
using Newtonsoft.Json;

namespace Samples;

public class VisaService
{
    private const string ServiceUrl = "http://localhost:89/0/rest/VisaCostItemWebService/GetVisaItems";
    private readonly AuthManager _authManager;

    public VisaService(AuthManager authManager)
    {
       _authManager = authManager;
    }
    
    
    public string GetVisaItems(string yearBudgetId, string brandBudgetId)
    {
        var request = (HttpWebRequest)WebRequest.Create(ServiceUrl);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.CookieContainer = _authManager._cookieContainer;
        request.Headers.Add("BPMCSRF", _authManager.BpmcsrfToken);

        var postData = JsonConvert.SerializeObject(new
        {
            yearBudgetId = yearBudgetId,
            brandBudgetId = brandBudgetId
        });

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(postData);
            streamWriter.Flush();
            streamWriter.Close();
        }

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
        catch (WebException ex) {
        
            return null;
        }
    }
}