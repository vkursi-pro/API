using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetKilkistPracivnukivClass
    {

        /*
        
        83. Штатна чисельність працівників
        [POST] /api/1.0/organizations/GetKilkistPracivnukiv
         
        */
        public static GetKilkistPracivnukivResponseModel GetKilkistPracivnukiv(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetKilkistPracivnukivRequestBodyModel GKPRBody = new GetKilkistPracivnukivRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ або ІПН
                };

                string body = JsonConvert.SerializeObject(GKPRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetKilkistPracivnukiv");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetKilkistPracivnukivResponseModel GKPResponse = new GetKilkistPracivnukivResponseModel();

            GKPResponse = JsonConvert.DeserializeObject<GetKilkistPracivnukivResponseModel>(responseString);

            return GKPResponse;
        }
    }
    /// <summary>
    /// Модель запиту (Example: {"code":["21560766"]})
    /// </summary>
    public class GetKilkistPracivnukivRequestBodyModel                              
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public List<string> Code { get; set; }                                       
    }
    /// <summary>
    /// Модель на відповідь GetKilkistPracivnukiv
    /// </summary>
    public class GetKilkistPracivnukivResponseModel                                  
    {/// <summary>
     /// Статус відповіді по API
     /// </summary>
        public string Status { get; set; }                                           
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }                                          
        /// <summary>
        /// Дані відповіді (в розрізі кодів ЄДРПОУ)
        /// </summary>
        public List<EmployeesData> Data { get; set; }                                
    }/// <summary>
     /// Дані відповіді (в розрізі кодів ЄДРПОУ)
     /// </summary>
    public class EmployeesData                                                      
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Code { get; set; }                                            
        /// <summary>
        /// Перелік по рокам
        /// </summary>
        public List<KilkistPracivnukivModel> EmployeesList { get; set; }            
    }/// <summary>
     /// Перелік по рокам
     /// </summary>
    public class KilkistPracivnukivModel                                            
    {/// <summary>
     /// Рік
     /// </summary>
        public int? year { get; set; }                                              
        /// <summary>
        /// К-ть працівників
        /// </summary>
        public int? count { get; set; }                                             
        /// <summary>
        /// Різниця з попереднім
        /// </summary>
        public int? differentPrevCount { get; set; }                                
    }
}
