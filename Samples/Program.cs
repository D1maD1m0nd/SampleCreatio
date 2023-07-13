using Samples;
using Terrasoft.Configuration.HaleonReport.Helper;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Terrasoft.Configuration.SummaryTableVisa;

var authManager = new AuthManager();
Console.WriteLine("Auth Status = " +  authManager.Login());
Console.WriteLine("BPMCSRF Token " + authManager.BpmcsrfToken);
//пример выполнения запроса к рест сервису
var visaService = new VisaService(authManager);
var responseVisa = visaService.GetVisaItems("42533c5f-b173-4386-a1d9-8e02e5b91d4d", "f4c9e1ef-167e-4aef-b2c1-56950486df79");
Console.WriteLine(responseVisa);

//пример выполнения запрос к сервису по вызову бизнес процессов
var contactService = new ContactService(authManager);
var reponseContact = contactService.GetAllContacts();
Console.WriteLine(reponseContact);