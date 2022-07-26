using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetFinancialIndustrialGroupClass
    {
        /*
        
        Метод:
            71. Фінансово промислові групи
            [POST] /api/1.0/organizations/GetFinancialIndustrialGroup

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetFinancialIndustrialGroup' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Codes":["21560766"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetFinancialIndustrialGroupResponse.json

        */

        public static GetFinancialIndustrialGroupResponseModel GetFinancialIndustrialGroup(ref string token, string edrpou)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetFinancialIndustrialGroupRequestBodyModel GFIGRequestBody = new GetFinancialIndustrialGroupRequestBodyModel
                {
                    Codes = new List<string> { edrpou },
                };

                string body = JsonConvert.SerializeObject(GFIGRequestBody);  // {"Codes":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetFinancialIndustrialGroup");
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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetFinancialIndustrialGroupResponseModel GFIGResponseRow = new GetFinancialIndustrialGroupResponseModel();

            GFIGResponseRow = JsonConvert.DeserializeObject<GetFinancialIndustrialGroupResponseModel>(responseString);

            return GFIGResponseRow;
        }
    }

    /*
     
     // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Codes\":[\"21560766\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetFinancialIndustrialGroup")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();     


     // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "Codes": [
            "21560766"
          ]
        })
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/GetFinancialIndustrialGroup", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
     
    */
    /// <summary>
    /// Модель запиту (Example: {"searchType":1,"code":"32352162"})
    /// </summary>
    public class GetFinancialIndustrialGroupRequestBodyModel                            
    {/// <summary>
     /// Перелык Кодів ЄДРПОУ
     /// </summary>
        public List<string> Codes { get; set; }                                          
    }
    /// <summary>
    /// Модель на відповідь GetFinancialIndustrialGroup
    /// </summary>
    public class GetFinancialIndustrialGroupResponseModel                                
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSuccess { get; set; }                                              
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                               
        /// <summary>
        /// Дані методу
        /// </summary>
        public List<GetFinancialIndustrialGroupResponseDataModel> Data { get; set; }     
    }
    /// <summary>
    /// Дані методу
    /// </summary>
    public class GetFinancialIndustrialGroupResponseDataModel                           
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Code { get; set; }                                                 
        /// <summary>
        /// Перелік груп
        /// </summary>
        public List<FinancialIndustrialGroupModel> Data { get; set; }                    
    }
    /// <summary>
    /// Перелік груп
    /// </summary>
    public class FinancialIndustrialGroupModel                                          
    {/// <summary>
     /// Системний id в сервісі Vkursi
     /// </summary>
        public Guid Id { get; set; }                                                    
        /// <summary>
        /// Назва організації
        /// </summary>
        public string OrganizationName { get; set; }                                    
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string OrganizationCode { get; set; }                                    
        /// <summary>
        /// Назва группи
        /// </summary>
        public string FinancialGroupName { get; set; }                                   
        /// <summary>
        /// Id группи
        /// </summary>
        public int FinancialGroupId { get; set; }                                       
        /// <summary>
        /// Системний id організації в сервісі Vkursi
        /// </summary>
        public Guid? OrganizationId { get; set; }                                       
        /// <summary>
        /// Особа через яку повязана группа
        /// </summary>
        public string RelationPersonName { get; set; }                                  
        /// <summary>
        /// Оборот групи грн.
        /// </summary>
        public decimal? FpgNetIncom { get; set; }                                        
    }
}
