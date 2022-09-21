using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.person
{
    public class PersonInOrganizationsAndFopsClass
    {
        /// <summary>
        /// Компанії та ФОП пов`язані з фізичною особою
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="ipn">ІПН ФО</param>
        /// <param name="name">ПІБ ФО</param>
        /// <param name="minimumDateToCheck">Мінімально допустима дата актуальності інформації по компаніях на Вкурсі (в випадку яхщо дані не достатньо актуальні буде зроблено онлайн запит)</param>
        /// <returns></returns>

        /*
            cURL запиту:
                curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/person/personinorganizationsandfops' \
                --header 'ContentType: application/json' \
                --header 'Authorization: Bearer eyJhbGciOiJIUzI1...' \
                --header 'Content-Type: application/json' \
                --data-raw '{"CheckList":[{"Code":"3334800417","Name":"Запека Вадим Віталійович"}], "MinimumDateToCheck":"2022-09-21T13:49:51.141Z"}'

            Приклад відповіді:
                    https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/PersonInOrganizationsAndFopsResponse.json
        */

        public static PersonInOrganizationsAndFopsResponseModel PersonInOrganizationsAndFops(ref string token, string ipn, string name, DateTime? minimumDateToCheck)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                PersonInOrganizationsAndFopsBodyModel GSRSBody = new PersonInOrganizationsAndFopsBodyModel
                {
                    MinimumDateToCheck = minimumDateToCheck,
                    CheckList = new List<PersonInOrganizationsAndFopsModelItem>() { 
                        new PersonInOrganizationsAndFopsModelItem() { 
                            Code = ipn, 
                            Name = name } 
                    }
                };

                string body = JsonConvert.SerializeObject(GSRSBody);

                // Example Body: {"CheckList":[{"Code":"3334800417","Name":"Запека Вадим Віталійович"}], "MinimumDateToCheck":"2022-09-21T13:49:51.141Z"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/person/personinorganizationsandfops");
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

            PersonInOrganizationsAndFopsResponseModel GSRSResponse = new PersonInOrganizationsAndFopsResponseModel();

            GSRSResponse = JsonConvert.DeserializeObject<PersonInOrganizationsAndFopsResponseModel>(responseString);

            return GSRSResponse;
        }
    }
    /// <summary>
    /// Модель запиту
    /// </summary>
    public class PersonInOrganizationsAndFopsBodyModel
    {
        /// <summary>
        /// Перелік параметрів ФО (обмеження 10)
        /// </summary>
        public List<PersonInOrganizationsAndFopsModelItem> CheckList { get; set; }
        /// <summary>
        /// Мінімально допустима дата актуальності інформації по компаніях на Вкурсі (в випадку яхщо дані не достатньо актуальні буде зроблено онлайн запит)
        /// </summary>
        public DateTime? MinimumDateToCheck { get; set; }
    }

    /// <summary>
    /// Параметри ФО
    /// </summary>
    public class PersonInOrganizationsAndFopsModelItem
    {
        /// <summary>
        /// Код ІПН ФО
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// ПІБ ФО
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class PersonInOrganizationsAndFopsResponseModel
    {
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Дані по пов'язаним компаніям та ФОП
        /// </summary>
        public PersonInOrganizationsAndFopsResponseModelData Data { get; set; }
    }

    /// <summary>
    /// Дані по пов'язаним компаніям та ФОП
    /// </summary>
    public class PersonInOrganizationsAndFopsResponseModelData
    {
        /// <summary>
        /// Дані по пов'язаним компаніям та ФОП
        /// </summary>
        public GetOrganizationsConnectedToPersonResponseModel OrganizationData { get; set; }
        /// <summary>
        /// Дані по пов'язаним ФОП
        /// </summary>
        public PersonInOrganizationsAndFopsResponseModelFopData FopData { get; set; }
    }

    /// <summary>
    /// Дані по пов'язаним ФОП
    /// </summary>
    public class PersonInOrganizationsAndFopsResponseModelFopData
    {
        /// <summary>
        /// Перелік ФОП
        /// </summary>
        public List<PersonInOrganizationsAndFopsResponseModelFopItem> Data { get; set; }
    }

    /// <summary>
    /// Перелік ФОП
    /// </summary>
    public class PersonInOrganizationsAndFopsResponseModelFopItem
    {
        /// <summary>
        /// По якому ІПНу були знайдені дані
        /// </summary>
        public string Ipn { get; set; }

        /// <summary>
        /// Дані про ФОП
        /// </summary>
        public List<GetOrganizationModel> Data { get; set; }
    }

    /// <summary>
    /// Дані по пов'язаним компаніям
    /// </summary>
    public class GetOrganizationsConnectedToPersonResponseModel
    {
        /// <summary>
        /// Перелік компаній
        /// </summary>
        public List<PersonInCompanies> PersonInCompaniesList { get; set; }
        /// <summary>
        /// Чи достатньо ресурсів для запитів
        /// </summary>
        public bool NotEnoughMoney { get; set; }
    }

    /// <summary>
    /// Перелік компаній
    /// </summary>
    public class PersonInCompanies
    {
        /// <summary>
        /// Внутрішній Id Vkursi
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// По якому ІПНу були знайдені дані
        /// </summary>
        public string Ipn { get; set; }
        /// <summary>
        /// Перелік компаній в яких бенефіціар (з історичними зв'язками)
        /// </summary>
        public List<GetOrganizationModel> BeneficiarList { get; set; }
        /// <summary>
        /// Перелік компаній в яких бенефіціар (з історичними зв'язками)
        /// </summary>
        public List<GetOrganizationModel> FounderList { get; set; }
        /// <summary>
        /// Перелік компаній в яких бенефіціар (з історичними зв'язками)
        /// </summary>
        public List<GetOrganizationModel> ChiefList { get; set; }
        /// <summary>
        /// Перелік компаній в яких бенефіціар (з історичними зв'язками)
        /// </summary>
        public List<GetOrganizationModel> AssigneeList { get; set; }

        /// <summary>
        /// Перелік компаній в яких бенефіціар (БЕЗ історичних)
        /// </summary>
        public List<GetOrganizationModel> BeneficiarClearList { get; set; }
        /// <summary>
        /// Перелік компаній в яких бенефіціар (БЕЗ історичних)
        /// </summary>
        public List<GetOrganizationModel> FounderClearList { get; set; }
        /// <summary>
        /// Перелік компаній в яких бенефіціар (БЕЗ історичних)
        /// </summary>
        public List<GetOrganizationModel> ChiefClearList { get; set; }
        /// <summary>
        /// Перелік компаній в яких бенефіціар (БЕЗ історичних)
        /// </summary>
        public List<GetOrganizationModel> AssigneeClearList { get; set; }
    }

    /// <summary>
    /// Дані про компанію / ФОП
    /// </summary>
    public class GetOrganizationModel
    {
        /// <summary>
        /// Стан
        /// </summary>
        public int? state { get; set; }
        /// <summary>
        /// Стан текстом
        /// </summary>
        public string state_text { get; set; }
        /// <summary>
        /// Код ЕДРПОУ / ІПН
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// Назва / ПІБ
        /// </summary>
        public string name { get; set; }
    }
}
