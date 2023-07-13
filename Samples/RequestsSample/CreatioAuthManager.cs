using System.Net;
using Newtonsoft.Json;

namespace Samples;

public class AuthManager
{
    private const string LoginUrl = "http://localhost:89/ServiceModel/AuthService.svc/Login";
    private const string UserName = "Supervisor";
    private const string UserPassword = "ekoblova23DIV"; 
    public readonly CookieContainer _cookieContainer = new CookieContainer();
    public string BpmcsrfToken { get; set; }

    public bool Login()
    {
        var request = (HttpWebRequest)WebRequest.Create(LoginUrl);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.CookieContainer = _cookieContainer;

        var postData = JsonConvert.SerializeObject(new
        {
            UserName = UserName,
            UserPassword = UserPassword
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
                ExtractTokenFromCookies(response);
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
        catch (WebException ex)
        {
            // Обработка ошибок
            return false;
        }
    }

    private void ExtractTokenFromCookies(HttpWebResponse response)
    {
        var cookies = response.Cookies;
        var csrfCookie = cookies["BPMCSRF"];
        if (csrfCookie != null)
        {
            BpmcsrfToken = csrfCookie.Value;
        }
    }
}