using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetRussianFoundersAndBeneficiarsClass
    {
        /// <summary>
        /// 89. Отримання відомостей про наявних в компанії засновників / бенефіціарів / власників пакетів акцій пов'язаних з росією або білорусією
        /// </summary>
        /// <param name="code">Код ЄДРПОУ</param>
        /// <param name="token">Токен</param>
        /// <returns></returns>

        /*

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRussianFoundersAndBeneficiars' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsI...' \
            --header 'ContentType: application/json' \
            --header 'Content-Type: application/json' \
            --data-raw '{"EdrpouList":["00222166"]}' 

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetRussianFoundersAndBeneficiarsResponse.json
 
        */
        public static GetRussianFoundersAndBeneficiarsResponseModel GetRussianFoundersAndBeneficiars(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetRussianFoundersAndBeneficiarsRequestBodyModel GRFABRequestBody = new GetRussianFoundersAndBeneficiarsRequestBodyModel
                {
                    EdrpouList = new List<string> { code }                     // Код ЄДРПОУ / ІПН
                };

                string body = JsonConvert.SerializeObject(GRFABRequestBody);   // Example body: {"edrpouList":"00222166"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRussianFoundersAndBeneficiars");
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
                    Console.WriteLine("За вказаным кодом організації не знайдено");
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

            GetRussianFoundersAndBeneficiarsResponseModel organizationAnalytic = new GetRussianFoundersAndBeneficiarsResponseModel();

            organizationAnalytic = JsonConvert.DeserializeObject<GetRussianFoundersAndBeneficiarsResponseModel>(responseString);

            return organizationAnalytic;
        }
    }

    /*
     
    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"EdrpouList\":[\"00222166\"]}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetRussianFoundersAndBeneficiars")
          .method("POST", body)
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI...")
          .addHeader("ContentType", "application/json")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();


     // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "EdrpouList": [
            "00222166"
          ]
        })
        headers = {
          'Authorization': 'Bearer eyJhbGciOiJIUzI1...',
          'ContentType': 'application/json',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/GetRussianFoundersAndBeneficiars", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))
     
    */

    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetRussianFoundersAndBeneficiarsRequestBodyModel
    {
        /// <summary>
        /// Перелік кодів ЄДРПОУ
        /// </summary>
        public List<string> EdrpouList { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetRussianFoundersAndBeneficiarsResponseModel
    {
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Статус відповіді
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Дані
        /// </summary>
        public List<GetRussianFoundersAndBeneficiarsModelAnswerItem> Data { get; set; }
    }

    /// <summary>
    /// Дані
    /// </summary>
    public class GetRussianFoundersAndBeneficiarsModelAnswerItem
    {
        /// <summary>
        /// Код ЄДПРОУ
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Перелік засновників / бенефіціарів / власників пакетів акцій пов'язаних з росією або білорусією
        /// </summary>
        public List<RussianFoundersAndBeneficiares> Data { get; set; }
    }

    /// <summary>
    /// Перелік засновників / бенефіціарів / власників пакетів акцій пов'язаних з росією або білорусією
    /// </summary>
    public class RussianFoundersAndBeneficiares
    {
        /// <summary>
        /// Id запису про бенефіціара (унікальний) 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код ЕДРПОУ компанії по якої має зв'язок
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Оригінальний запис про бенефіціара/засновника
        /// </summary>
        public string ObjName { get; set; }
        /// <summary>
        /// ПІБ / назва компанії
        /// </summary>
        public string ClearName { get; set; }
        /// <summary>
        /// Країна резиденства / громадянства
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Адреса
        /// </summary>
        public string ObjAdress { get; set; }
        /// <summary>
        /// Тип бенефіціарного впливу
        /// </summary>
        public string Influence { get; set; }
        /// <summary>
        /// Розмір / відсоток частки
        /// </summary>
        public string ShareSize { get; set; }
        /// <summary>
        /// Тип звязку: 1 - засновник / 2 - бенеціфіар / 3 - Власник пакетів акцій
        /// </summary>
        public int TypeRel { get; set; }
        /// <summary>
        /// Це фізична особа: true - так / false - ні
        /// </summary>
        public bool IsPerson { get; set; }
    }
}
