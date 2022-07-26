using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetDeclarationsInfoClass
    {
        /*
         
        36. Перелік декларантів повязаних з компаніями
        [POST] /api/1.0/organizations/getdeclarationsinfo      
        
        */

        public static GetDeclarationsInfoResponseModel GetDeclarationsInfo(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDeclarationsInfoRequestBodyModel GDIRequestBody = new GetDeclarationsInfoRequestBodyModel
                {
                    Edrpou = new List<string> 
                    {
                        code                                                    // Код ЄДРПОУ аба ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GDIRequestBody);      // Example body: {"Edrpou":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getdeclarationsinfo");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetDeclarationsInfoResponseModel GDIResponse = new GetDeclarationsInfoResponseModel();

            GDIResponse = JsonConvert.DeserializeObject<GetDeclarationsInfoResponseModel>(responseString);

            return GDIResponse;
        }
    }
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetDeclarationsInfoRequestBodyModel                            
    {/// <summary>
     /// Перелік ЄДРПОУ / ІПН (обмеження 1)
     /// </summary>
        public List<string> Edrpou { get; set; }                                
    }
    /// <summary>
    /// Модель відповіді GetDeclarationsInfo
    /// </summary>
    public class GetDeclarationsInfoResponseModel                               
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSucces { get; set; }                                      
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                      
        /// <summary>
        /// Дані
        /// </summary>
        public List<OrgEDRApiApiAnswerModelData> Data { get; set; }             
    }
    /// <summary>
    /// Дані
    /// </summary>
    public class OrgEDRApiApiAnswerModelData                                    
    {/// <summary>
     /// ЄДРПОУ / ІПН 
     /// </summary>
        public string Edrpou { get; set; }                                      
        /// <summary>
        /// Перелік декларантів по рокам
        /// </summary>
        public List<OrgEDRApiApiAnswerModelDataPerYear> DataPerYear { get; set; }        
    }
    /// <summary>
    /// Перелік декларантів по рокам
    /// </summary>
    public class OrgEDRApiApiAnswerModelDataPerYear                             
    {/// <summary>
     ///  Рік
     /// </summary>
        public int Year { get; set; }                                           
        /// <summary>
        /// Перелік декларантів
        /// </summary>
        public List<OrgEDRApiApiAnswerModelDataObj> List { get; set; }          
    }
    /// <summary>
    /// Перелік декларантів
    /// </summary>
    public class OrgEDRApiApiAnswerModelDataObj                                 
    {/// <summary>
     /// ПІБ + тип відношення
     /// </summary>
        public string Name { get; set; }                                        
        /// <summary>
        /// Сума
        /// </summary>
        public decimal Sum { get; set; }                                        
    }
}
