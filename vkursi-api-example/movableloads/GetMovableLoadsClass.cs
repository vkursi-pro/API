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
        
        22. ДРОРМ отримання скороченных даних про наявні обтяження на рухоме майно по ІПН / ЄДРПОУ
        [POST] /api/1.0/MovableLoads/getmovableloads

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getmovableloads' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Edrpou":["36679626"],"Ipn":["1841404820"]}'
        
        */

        public static GetMovableLoadsResponseModel GetMovableLoads(string token, string edrpou, string ipn)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getmovableloads");
                RestRequest request = new RestRequest(Method.POST);

                GetMovableLoadsRequestBodyModel GMLRequestBodyRow = new GetMovableLoadsRequestBodyModel
                {
                    Edrpou = new List<string> {
                        edrpou
                    },
                    Ipn = new List<string> {
                        ipn
                    }
                };

                string body = JsonConvert.SerializeObject(GMLRequestBodyRow);

                // Example Body: 

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    token = AuthorizeClass.Authorize();
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

    public class GetMovableLoadsRequestBodyModel
    {
        public List<string> Edrpou { get; set; }    // Перелік кодів ІПН (обмеження 1)
        public List<string> Ipn { get; set; }       // Перелік кодів ЄДРПОУ (обмеження 1)
    }


    public class GetMovableLoadsResponseModel       // Відповідь на запит
    {
        public bool isSuccess { get; set; }         // Запит виконано успішно (true - так / false - ні)
        public string status { get; set; }          // Спапус запиту (maxLength:128)
        public List<Datum> data { get; set; }       // Перелік обтяжень
    }

    public class SidesBurdenList                    // Перелік обтяжувачів
    {
        public string sideId { get; set; }          // Системний id vkursi (maxLength:64)
        public string burdenName { get; set; }      // Назва (maxLength:512)
        public string burdenCode { get; set; }      // Код (maxLength:10)
    }

    public class SidesDebtorList                    // Перелік боржників
    {
        public string sideId { get; set; }          // Системний id vkursi (maxLength:64)
        public string debtorName { get; set; }      // Назва (maxLength:512)
        public string debtorCode { get; set; }      // Код (maxLength:10)
        public List<string> organizationIdList { get; set; }        // Системний id vkursi
        public object personCardIdList { get; set; }                // Системний id vkursi
    }

    public class Datum                              // Перелік обтяжень
    {
        public string number { get; set; }          // Номер обтяження  (maxLength:32)
        public DateTime regDate { get; set; }       // Дата реєстрації обтяження
        public bool isActive { get; set; }          // Діюче (true - так / false - ні)
        public List<SidesBurdenList> sidesBurdenList { get; set; }  // Перелік обтяжувачів
        public List<SidesDebtorList> sidesDebtorList { get; set; }  // Перелік боржників
        public string property { get; set; }        // Опис майна (maxLength:512)
    }
}
