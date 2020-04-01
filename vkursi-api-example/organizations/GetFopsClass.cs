using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetFopsClass
    {
        /*
         
        3. Запит на отримання коротких даних по ФОП за кодом ІПН
        [POST] /api/1.0/organizations/getfops  

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getfops' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw '{"code": ["3334800417"]}'

        */

        public static List<GetFopsResponseModel> GetFops(string code, string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetFopsRequestBodyModel GFRequestBody = new GetFopsRequestBodyModel
                {
                    code = new List<string>
                    {
                        code                                                // Перелік кодів ІПН
                    }
                };

                string body = JsonConvert.SerializeObject(GFRequestBody);   // Example body: {"code": ["3334800417"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/getfops");
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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаным кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            List<GetFopsResponseModel> FopsShortList = new List<GetFopsResponseModel>();

            FopsShortList = JsonConvert.DeserializeObject<List<GetFopsResponseModel>>(responseString);

            return FopsShortList;
        }
    }

    public class GetFopsRequestBodyModel                            // Модель Body запиту
    {
        public List<string> code = new List<string>();              // Перелік кодів ІПН
    }

    public class GetFopsResponseModel                               // Модель відповіді
    {
        public Guid Id { get; set; }                                // Системний Id Fop
        public string Name { get; set; }                            // ПІБ ФОПа
        public string State { get; set; }                           // Статус реєстрації
        public string Code { get; set; }                            // Код ІПН
        public string Inn { get; set; }                             // Код ІПН (ПДВ)
        public DateTime? DateCanceledInn { get; set; }              // Дата анулючання свідоцтва платника ПДВ
        public DateTime? DateRegInn { get; set; }                   // Дата реєстрації платником ПДВ
        public int? Introduction { get; set; }                      // Наявні виконавчі провадження
        public int? ExpressScore { get; set; }                      // Загальна кількість ризиків
        public bool? HasBorg { get; set; }                          // Наявний податковий борг (true - так / false - ні)
        public bool? InSanctions { get; set; }                      // Наявні санкції (true - так / false - ні)
        public SingleTaxPayer singleTaxPayer { get; set; }          // Відомості про платника ЄП
    }
    public class SingleTaxPayer                                     // Відомості про платника ЄП
    {
        public DateTime dateStart { get; set; }                     // Дата реєстрації платником ЄП
        public double rate { get; set; }                            // Ставка
        public int group { get; set; }                              // Група
        public object dateEnd { get; set; }                         // Дата анулювання
        public string kindOfActivity { get; set; }                  // Вид діяльності
        public bool status { get; set; }                            // Статус (true - платник ЄП / false - не платник ЄП)
    }
}
