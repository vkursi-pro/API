using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public class GetFoundingDocumentsClass
    {
        /// <summary>
        /// 156. Отримання статутних документів за кодом ЄДРПОУ
        /// [POST] /api/1.0/organizations/GetFoundingDocuments
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static GetFoundingDocumentsResponseModel GetFoundingDocuments(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetFoundingDocumentsRequestBodyModel COLRBodyModel = new GetFoundingDocumentsRequestBodyModel
                {
                    Code = code                               // Код ЄДРПОУ
                };

                string body = JsonConvert.SerializeObject(COLRBodyModel);           // Example body: {"Code":"00131305"}

                RestClient client = new RestClient(
                    "https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetFoundingDocuments");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetFoundingDocumentsResponseModel GNWEResponse =
                JsonConvert.DeserializeObject<GetFoundingDocumentsResponseModel>(responseString);

            return GNWEResponse;
        }
    }

    /// <summary>
    /// Модель Запиту
    /// </summary>
    public class GetFoundingDocumentsRequestBodyModel
    {
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }     
    }

    /// <summary>
    /// Модель выдповіді
    /// </summary>
    public class GetFoundingDocumentsResponseModel
    {
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Дані відповіді
        /// </summary>
        public GetFoundingDocumentsData Data { get; set; }
    }

    /// <summary>
    /// Дані відповіді
    /// </summary>
    public class GetFoundingDocumentsData
    {
        [JsonProperty("naisId")]
        public int NaisId { get; set; }

        /// <summary>
        /// Найменування суб'єкта
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Повідомлення про відсутність сканованих копій установчих документів. 
        /// Якщо копії документів відстутні, то виводиться текст: “В ЄДР відсутні скановані копії установчих документів". 
        /// null - якщо копії документів наявні.
        /// </summary>
        [JsonProperty("absentReason")]
        public object AbsentReason { get; set; }

        /// <summary>
        /// Масив з атрибутами установчих документів. 
        /// null - документи відсутні
        /// </summary>
        [JsonProperty("documents")]
        public GetFoundingDocumentsDocument[] Documents { get; set; }

        /// <summary>
        /// Посилання на завантаження підписаного .zip архіву
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// Посилання на завантаження файлу підпису
        /// </summary>
        [JsonProperty("signLink")]
        public string SignLink { get; set; }
    }

    /// <summary>
    /// Масив з атрибутами установчих документів. 
    /// null - документи відсутні
    /// </summary>
    public class GetFoundingDocumentsDocument
    {
        /// <summary>
        /// Назва документа
        /// </summary>
        [JsonProperty("docName")]
        public string DocName { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        [JsonProperty("fileType")]
        public string FileType { get; set; }

        /// <summary>
        /// Розмір документа
        /// </summary>
        [JsonProperty("docSize")]
        public long DocSize { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// Дата “від”
        /// </summary>
        [JsonProperty("dateFrom")]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Посилання на завантаження файлу (файл з розпакованого .zip архіву без підпису ЕЦП)
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
