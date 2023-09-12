using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.person
{
    public class CheckPepClass
    {
        /// <summary>
        /// 90. Перевірка ПЕП
        /// [POST] /api/1.0/person/CheckPep
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        
        public static CheckPepResponseModel CheckPep(ref string token, string name)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                CheckPepBodyModel GSRSBody = new CheckPepBodyModel
                {
                    PersonName = "Половніков Олександр Олександрович",
                };

                string body = JsonConvert.SerializeObject(GSRSBody);

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/person/CheckPep");
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

            CheckPepResponseModel GSRSResponse = new CheckPepResponseModel();

            GSRSResponse = JsonConvert.DeserializeObject<CheckPepResponseModel>(responseString);

            return GSRSResponse;
        }
    }

    public class CheckPepBodyModel
    {
        /// <summary>
        /// ПІБ
        /// </summary>
        public string PersonName { get; set; }
    }

    public class CheckPepResponseModel
    {
        /// <summary>
        /// ПІБ
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// Чи являється ПЕП
        /// </summary>
        public bool PublicPerson { get; set; }
        /// <summary>
        /// Декларації
        /// </summary>
        public List<PersonsInDeclarations> DeclarationIdList { get; set; }
    }

    /// <summary>
    /// Декларації
    /// </summary>
    public class PersonsInDeclarations                                             // 
    {/// <summary>
     /// ПіБ
     /// </summary>
        public string Name { get; set; }                                                // 
        /// <summary>
        /// Посилання
        /// </summary>
        public string Reference { get; set; }                                           // 
        /// <summary>
        /// Рік
        /// </summary>
        public string Year { get; set; }                                                   // 
        /// <summary>
        /// Тип
        /// </summary>
        public string Type { get; set; }                                                // 
        /// <summary>
        /// Посада
        /// </summary>
        public string WorkPost { get; set; }                                            // 
        /// <summary>
        /// Місце роботі
        /// </summary>
        public string WorkPlace { get; set; }                                           // 
        /// <summary>
        /// Наявна нерухомість (true - так / false - ні)
        /// </summary>
        public bool Estates { get; set; }                                               // 
        /// <summary>
        /// Наявні об'єкти незавершеного будівництва (true - так / false - ні)
        /// </summary>
        public bool NotFinishedBuildings { get; set; }                                  // 
        /// <summary>
        /// Наявні обтяження (true - так / false - ні)
        /// </summary>
        public bool Movables { get; set; }                                              // 
        /// <summary>
        /// Наявні транспортні засоби (true - так / false - ні)
        /// </summary>
        public bool Transport { get; set; }                                             // 
        /// <summary>
        /// Наявні цінні папери (true - так / false - ні)
        /// </summary>
        public bool Securities { get; set; }                                            // 
        /// <summary>
        /// Наявні корморативні права (true - так / false - ні)
        /// </summary>
        public bool CorporateRights { get; set; }                                       // 
        /// <summary>
        /// Наявні нематеріальні активи (true - так / false - ні)
        /// </summary>
        public bool Intangible { get; set; }                                            // 
        /// <summary>
        /// Наявні відомості про доходи (true - так / false - ні)
        /// </summary>
        public bool Income { get; set; }                                                // 
        /// <summary>
        /// Статус ПЕП:
        /// 0 - Не публічна особа (ПІБ вказано в декларації і він не ПЕП)
        /// 1 - Особа яку вказано в декларації публічної особи (ПІБ вказано в декларації особи яка є ПЕП)
        /// 2 - Публічна особа (ПІБ вказано в декларації і він є ПЕП)
        /// </summary>
        public int? PublicStatus { get; set; }
    }
}
