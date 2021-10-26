using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetRelationsClass
    {
        /*

        Метод:
            41. Отримати список пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій
            [POST] /api/1.0/organizations/getrelations

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getrelations' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Edrpou":["42556505"],"RelationId":null,"FilterRelationType":null,"MaxRelationLevel":3}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetRelationsResponse.json

        */

        public static GetRelationsResponseModel GetRelations(ref string token, string edrpou, string relationId)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetRelationsRequestBodyModel GRRequestBody = new GetRelationsRequestBodyModel
                {
                    Edrpou = new List<string>                                           // Перелік кодів ЄДРПОУ (обмеження 1)
                    {
                        edrpou
                    },

                    MaxRelationLevel = 1,                                               // Фільтр по к-ті рівнів зв'язків які будуть отриммані в відповіді

                    //FilterRelationType = new List<int>                                // Фільтр по типам зв'язків
                    //{ 
                    //    1,2,3
                    //},

                    //RelationId = new List<string>                                     // Перелік Id зв'язків за якими буде проведений пошук зв'язків
                    //{
                    //    edrpou
                    //}
                };

                string body = JsonConvert.SerializeObject(GRRequestBody);

                // Example Body: {"Edrpou":["42556505"],"RelationId":null,"FilterRelationType":null,"MaxRelationLevel":3}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getrelations");
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

            GetRelationsResponseModel GRResponseRow = new GetRelationsResponseModel();

            GRResponseRow = JsonConvert.DeserializeObject<GetRelationsResponseModel>(responseString);

            return GRResponseRow;
        }
    }

    /*
     
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Edrpou\":[\"42556505\"],\"RelationId\":null,\"FilterRelationType\":null,\"MaxRelationLevel\":3}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/getrelations", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:
        
        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Edrpou\":[\"42556505\"],\"RelationId\":null,\"FilterRelationType\":null,\"MaxRelationLevel\":3}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getrelations")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */

    public class GetRelationsRequestBodyModel                                           // Модель запиту 
    {
        public List<string> Edrpou { get; set; }                                        // Перелік кодів ЕДРПОУ за якими буде проведений пошук зв'язків
        public List<string> RelationId { get; set; }                                    // Перелік Id зв'язків за якими буде проведений пошук зв'язків
        public List<string> Name { get; set; }                                          // Перелік ПІБ зв'язків за якими буде проведений пошук зв'язків (мах 1)
        public List<int> FilterRelationType { get; set; }                               // Фільтр по типам зв'язків (LocationType = 1, ChiefType = 2, FounderType = 3, OldNodeNameType = 4, OldNodeChiefType = 5, OldNodeAdressType = 6, OldNodeFounder = 7, Branch = 8, Assignee = 9, Signatorie = 10, Shareholder = 11, ContactType = 12)
        public int? MaxRelationLevel { get; set; }                                      // Фільтр по к-ті рівнів зв'язків які будуть отриммані в відповіді
    }

    public class GetRelationsResponseModel                                              // Модель на відповідь
    {
        public bool IsSucces { get; set; }                                              // Чи успішний запит
        public string Status { get; set; }                                              // Статус відповіді по API
        public List<GetRelationApiModelAnswerData> Data { get; set; }                   // Перелік даних
    }

    public class GetRelationApiModelAnswerData                                          // Перелік даних
    {
        public string ParentId { get; set; }                                            // Id зв'язку
        public string ChildId { get; set; }                                             // Id дочірнього зв'язку 
        public string ParentName { get; set; }                                          // Назва зв'язку (керівник, бенефіціар)
        public string ParentEdrpou { get; set; }                                        // ЄДРПОУ зв'язку
        public string ChildName { get; set; }                                           // Назва дочірнього зв'язку
        public string ChildEdrpou { get; set; }                                         // ЄДРПОУ дочірнього зв'язку
        public List<int> ParentSanctions { get; set; }                                  // Санкції
        public int? ParentPublicPerson { get; set; }                                    // Статус публічної особи (// 0 - не публічна особа, 1 - звязана с публічною особою, 2 - публічная особо, 3 - невідомо, null - нема інформації)
        public int? ChildPublicPerson { get; set; }                                     // Дочірній  зв'язок
        public double? Percent { get; set; }                                            // Відсоток володіння (якщо бенефіціар / засновник / або власник пакетік акцій)
        public List<int> ChildSanctions { get; set; }                                   // Санкції дочірнього зв'язку
        public int? RelationLevel { get; set; }                                         // Рівень зв'язків
        public string Type { get; set; }                                                // Тип зв'язку (керівник, бенефіціар)

                                                                                        // FounderType Бенефіціар
                                                                                        // LocationType Адреса
                                                                                        // ChiefType Керівник
                                                                                        // OldNodeAdressType Попередня адреса
                                                                                        // OldNodeFounder Попередній бенефіціар
                                                                                        // OldNodeChiefType Попередній керівник
                                                                                        // OldNodeNameType Попередня назва
                                                                                        // Branch Філія
                                                                                        // Shareholder Власники пакетів акцій
                                                                                        // Assignee Правонаступник

        [JsonProperty("adresa")]
        public string Adresa { get; set; }                                              // 

        [JsonProperty("typBenefVolodinnya")]
        public string TypBenefVolodinnya { get; set; }                                  // 

        [JsonProperty("vplyvCherezUO")]
        public string VplyvCherezUo { get; set; }                                       // 

        [JsonProperty("aktsiy")]
        public List<AktsiyRelationModel> Aktsiy { get; set; }                                   // 
    }

    public class AktsiyRelationModel
    {
        [JsonProperty("vydDeponenta")]
        public string VydDeponenta { get; set; }                                        // 

        [JsonProperty("vydTsinnohoPaperu")]
        public string VydTsinnohoPaperu { get; set; }                                   // 

        [JsonProperty("kodIsin")]
        public string KodIsin { get; set; }                                             // 

        [JsonProperty("nominalVartist")]
        public string NominalVartist { get; set; }                                      // 

        [JsonProperty("kilkistAktsiy")]
        public string KilkistAktsiy { get; set; }                                       // 

        [JsonProperty("vidsotokAktsiy")]
        public string VidsotokAktsiy { get; set; }                                      // 
    }
}
