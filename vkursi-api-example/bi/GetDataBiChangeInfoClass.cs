using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.bi
{
    public class GetDataBiChangeInfoClass
    {
        /*
        
        Отримати перелік компаній по яким відбулись зміни в межах Bi моніторингу
        [POST] /api/1.0/bi/GetDataBiChangeInfo
         
        Приклад 1: без Body

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo?LabelId=1c891112-b022-4a83-ad34-d1f976c60a0b&Size=1000&IsOnlyNew=true&DateChange=2019-11-28T19:00:00.000' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...'


        Приклад 2: з Body

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":1000,"DateChange":"2019-11-28T19:00:52.059","IsOnlyNew":true}'

        */


        public static GetDataBiChangeInfoRequestModel GetDataBiChangeInfo(DateTime dateChange, string labelId, bool isNewOnly, int size, string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDataBiChangeInfoBodyModel GBDRequestBody = new GetDataBiChangeInfoBodyModel
                {
                    LabelId = labelId,                                              // Id списку (в якому відбулись зміни)
                    Size = size,                                                    // Розмір даних (від 1 до 10000)
                    IsOnlyNew = isNewOnly,                                          // Отримати тільки нові записи (true - отримати ті які до раніще не отримували / false - отримати всі за дату)
                    DateChange = dateChange                                         // Дата коли відбулись зміни
                };

                string body = JsonConvert.SerializeObject(GBDRequestBody);          // Example Body: {"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":1000,"DateChange":"2019-11-28T19:00:52.059","IsOnlyNew":true}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo");
                // https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo?LabelId=1c891112-b022-4a83-ad34-d1f976c60a0b&Size=1000&IsOnlyNew=true&DateChange=2019-11-28T19:00:00.000
                RestRequest request = new RestRequest(Method.POST);

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

            GetDataBiChangeInfoRequestModel GetBiDataList = new GetDataBiChangeInfoRequestModel();

            GetBiDataList = JsonConvert.DeserializeObject<GetDataBiChangeInfoRequestModel>(responseString);

            return GetBiDataList;
        }
    }

    public class GetDataBiChangeInfoBodyModel                                   // Модель Body запиту
    {
        public string LabelId { get; set; }                                     // Id списку (в якому відбулись зміни)
        public int Size { get; set; }                                           // Розмір даних (від 1 до 10000)
        public bool IsOnlyNew { get; set; }                                     // Отримати тільки нові записи (true - отримати ті які до раніще не отримували / false - отримати всі за дату)
        public DateTime DateChange { get; set; }                                // Дата коли відбулись зміни
    }

    public class GetDataBiChangeInfoRequestModel                                // Модель відповіді GetDataBiChangeInfo
    {
        public bool IsSuccess { get; set; }                                     // Успішно виконано?
        public string Status { get; set; }                                      // success, error, (Дані успішно знайдено. Pack: " + part)
        public int Code { get; set; }                                           // 404, 200, ...
        public List<DataBiChangeInfoModel> Data { get; set; }                   // Перелік компаній
    }

    public class DataBiChangeInfoModel
    {
        public Guid Id { get; set; }                                            // Id організації (пізніше будуть ще ФОП)
        public bool IsCompany { get; set; }                                     // Це компанію (так/ні)
        public string Code { get; set; }                                        // Код ЕДРПОУ (для компанії)
        public bool IsNew { get; set; }                                         // Новий запис (раніше не передавався по API)
        public int ChangeType { get; set; }                                     // 1 - Новий / 2 - Зміна / 3 - на відалення (більше не відповідає критеріям)
        public int DateChange { get; set; }                                     // Дата коли відбулись зміни
    }
}
