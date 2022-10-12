using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.movableloads
{
    public class GetMovableLoadsClass
    {
        /*
        
        Метод:
            22. ДРОРМ отримання скороченных даних про наявні обтяження на рухоме майно по ІПН / ЄДРПОУ
            [POST] /api/1.0/MovableLoads/getmovableloads

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getmovableloads' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Edrpou":["36679626"]}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetMovableLoadsResponse.json


        *Важливо: повертає тільки відкриті обтяження
        
        */

        public static GetMovableLoadsResponseModel GetMovableLoads(ref string token, string edrpou, string ipn)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getmovableloads");
                RestRequest request = new RestRequest(Method.POST);

                GetMovableLoadsRequestBodyModel GMLRequestBodyRow = new GetMovableLoadsRequestBodyModel();

                if (!string.IsNullOrEmpty(ipn))
                    GMLRequestBodyRow.Ipn = new List<string>() { ipn };

                if (!string.IsNullOrEmpty(edrpou))
                    GMLRequestBodyRow.Edrpou = new List<string>() { edrpou };

                string body = JsonConvert.SerializeObject(GMLRequestBodyRow);

                // Example Body: {"Ipn":["2333700948"]}  // 28169247

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

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetMovableLoadsResponseModel GMLResponseRow = new GetMovableLoadsResponseModel();
            GMLResponseRow = JsonConvert.DeserializeObject<GetMovableLoadsResponseModel>(responseString);
            return GMLResponseRow;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Edrpou\":[\"36679626\"],\"Ipn\":[\"1841404820\"]}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/MovableLoads/getmovableloads", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:
        
        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"36679626\"],\"Ipn\":[\"1841404820\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getmovableloads")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */
    /// <summary>
    /// ???
    /// </summary>
    public class GetMovableLoadsRequestBodyModel
    {/// <summary>
     /// Перелік кодів ІПН (обмеження 1)
     /// </summary>
        [JsonProperty("edrpou", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Edrpou { get; set; }    
        /// <summary>
        /// Перелік кодів ЄДРПОУ (обмеження 1)
        /// </summary>
        [JsonProperty("ipn", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Ipn { get; set; }       
    }

    /// <summary>
    /// Відповідь на запит
    /// </summary>
    public class GetMovableLoadsResponseModel       
    {/// <summary>
     /// Запит виконано успішно (true - так / false - ні)
     /// </summary>
        public bool isSuccess { get; set; }         
        /// <summary>
        /// Статус запиту (maxLength:128)
        /// </summary>
        public string status { get; set; }          
        /// <summary>
        /// Перелік обтяжень
        /// </summary>
        public List<MovableLoadsDatum> data { get; set; }       
        /// <summary>
        /// ???
        /// </summary>
        public Dictionary<string, string> originalData { get; set; }
    }
    /// <summary>
    /// Перелік обтяжувачів
    /// </summary>
    public class SidesBurdenList                    
    {/// <summary>
     /// Системний id vkursi (maxLength:64)
     /// </summary>
        public string sideId { get; set; }          
        /// <summary>
        /// Назва (maxLength:512)
        /// </summary>
        public string burdenName { get; set; }       
        /// <summary>
        /// Код (maxLength:10)
        /// </summary>
        public string burdenCode { get; set; }       
    }
    /// <summary>
    /// Перелік боржників
    /// </summary>
    public class SidesDebtorList                    
    {/// <summary>
     /// Системний id vkursi (maxLength:64)
     /// </summary>
        public string sideId { get; set; }          
        /// <summary>
        /// Назва (maxLength:512)
        /// </summary>
        public string debtorName { get; set; }     
        /// <summary>
        /// Код (maxLength:10)
        /// </summary>
        public string debtorCode { get; set; }      
        /// <summary>
        /// Системний id vkursi
        /// </summary>
        public List<string> organizationIdList { get; set; }         
        /// <summary>
        /// Системний id vkursi
        /// </summary>
        public List<string> personCardIdList { get; set; }                
    }
    /// <summary>
    /// Перелік обтяжень
    /// </summary>
    public class MovableLoadsDatum                              
    {/// <summary>
     /// Номер обтяження  (maxLength:32)
     /// </summary>
        public string Number { get; set; }          
        /// <summary>
        /// Дата реєстрації обтяження
        /// </summary>
        public DateTime? RegDate { get; set; }       
        /// <summary>
        /// Діюче (true - так / false - ні)
        /// </summary>
        public bool IsActive { get; set; }          
        /// <summary>
        /// Перелік обтяжувачів
        /// </summary>
        public List<SidesBurdenList> SidesBurdenList { get; set; }  
        /// <summary>
        /// Перелік боржників
        /// </summary>
        public List<SidesDebtorList> SidesDebtorList { get; set; }  
        /// <summary>
        /// Опис майна (maxLength:512)
        /// </summary>
        public string Property { get; set; }         
        /// <summary>
        /// Системний id обтяження в сервісі Vkursi
        /// </summary>
        public Guid Id { get; set; }                
        /// <summary>
        /// Дата запиту до розпорядника
        /// </summary>
        public DateTime? RequestDate { get; set; }  
        /// <summary>
        /// Об’єкт обтяження
        /// </summary>
        public string ObjectEncumbrance { get; set; }
        /// <summary>
        /// Вид обтяження
        /// </summary>
        public string Type { get; set; }            
        /// <summary>
        /// Валюта обтяження
        /// </summary>
        public string OriginalCurrency { get; set; } 
        /// <summary>
        /// Сума обтяження (в валюті яка вказана в OriginalCurrency)
        /// </summary>
        public decimal? OriginalSum { get; set; }   
        /// <summary>
        /// Сума обтяження в грн по курсу на дату реєстрації обтяження
        /// </summary>
        public decimal? SumUah { get; set; }        
        /// <summary>
        /// Термін дії
        /// </summary>
        public DateTime? EndDate { get; set; }      
        /// <summary>
        /// Дата коли обтяження з'явисоль в сервісі Vkursi
        /// </summary>
        public DateTime? CreateDate { get; set; }


        [JsonProperty("searchCode")]
        public string SearchCode { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("nameRegister")]
        public string NameRegister { get; set; }

        [JsonProperty("objRegister")]
        public string ObjRegister { get; set; }

        [JsonProperty("execTerm")]
        public DateTime? ExecTerm { get; set; }
    }
}
