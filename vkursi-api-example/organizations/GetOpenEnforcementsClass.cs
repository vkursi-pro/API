using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.organizations
{
    public class GetOpenEnforcementsClass
    {
        /*
         
        85. Єдиний реєстр боржників
        [POST] /api/1.0/organizations/GetOpenEnforcements 
         
        */

        public static GetOpenEnforcementsResponseModel GetOpenEnforcements(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetOpenEnforcementsRequestBodyModel GOERBody = new GetOpenEnforcementsRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ або ІПН
                };

                string body = JsonConvert.SerializeObject(GOERBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOpenEnforcements");
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
                    Console.WriteLine("За вказаним кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetOpenEnforcementsResponseModel GOEResponse = new GetOpenEnforcementsResponseModel();

            GOEResponse = JsonConvert.DeserializeObject<GetOpenEnforcementsResponseModel>(responseString);

            return GOEResponse;
        }
    }
    /// <summary>
    ///  Модель запиту (Example: {"code":["21560766"]})
    /// </summary>
    public class GetOpenEnforcementsRequestBodyModel                                //
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public List<string> Code { get; set; }                                      // 
    }
    /// <summary>
    /// Модель на відповідь GetOpenEnforcements
    /// </summary>
    public class GetOpenEnforcementsResponseModel                                   // 
    {/// <summary>
     /// Статус запиту (maxLength:128)
     /// </summary>
        public string Status { get; set; }                                          // 
        /// <summary>
        /// Запит виконано успішно (true - так / false - ні)
        /// </summary>
        public bool IsSuccess { get; set; }                                         // 
        /// <summary>
        /// Дані
        /// </summary>
        public List<EnforcementsData> Data { get; set; }                            // 
    }/// <summary>
     /// Дані
     /// </summary>
    public class EnforcementsData                                                   // 
    {/// <summary>
     /// Код ЕДРПОУ (за яким знайдено дані)
     /// </summary>
        public string Code { get; set; }                                            // 
        /// <summary>
        /// Перелік проваджень
        /// </summary>
        public List<EnforcementsDataList> EnforcementsList { get; set; }            // 
    }/// <summary>
     /// Перелік проваджень
     /// </summary>
    public class EnforcementsDataList                                               // 
    {/// <summary>
     /// Номер ВП (maxLength:32)
     /// </summary>
        public string VpNum { get; set; }                                               // 
        /// <summary>
        /// Стан (Приклад: Завершено, Примусове виконання, ...) (maxLength:128)
        /// </summary>
        public string State { get; set; }                                               // 
        /// <summary>
        /// Документ виданий
        /// </summary>
        public string Publisher { get; set; }                                           // 
        /// <summary>
        /// Зв'язок з виконавцем: Назва виконавця
        /// </summary>
        public string DepartmentName { get; set; }                                      // 
        /// <summary>
        /// Зв'язок з виконавцем: Номер телефону виконавця
        /// </summary>
        public string DepartmentPhone { get; set; }                                     // 
        /// <summary>
        /// ПІБ виконавця
        /// </summary>
        public string Executor { get; set; }                                            // 
        /// <summary>
        /// Номер телефону виконавця
        /// </summary>
        public string ExecutorPhone { get; set; }                                       // 
        /// <summary>
        /// Email виконавця
        /// </summary>
        public string ExecutorEmail { get; set; }                                       // 
        /// <summary>
        /// (in orgId) ? боржник : стягувач
        /// </summary>
        public string StateCurrentOrganization { get; set; }                        // 
        /// <summary>
        /// Тип відрахування
        /// </summary>
        public string DeductionType { get; set; }                                   // 
        /// <summary>
        /// Дата відкриття ВП
        /// </summary>
        public DateTime? EnforcementDate { get; set; }                              // 
        /// <summary>
        /// Дата останньої внесеної зміни в ВП
        /// </summary>
        public DateTime? ModifyDate { get; set; }                                   // 
        /// <summary>
        /// Відомості про іншу сторону (якщо наша компанія позивач то відомості про відповідача, якщо позивач навпаки)
        /// </summary>
        public EnforcementsDataOtherSite OtherSite { get; set; }                    // 

    }
    /// <summary>
    /// Відомості про іншу сторону (якщо наша компанія позивач то відомості про відповідача, якщо позивач навпаки)
    /// </summary>
    public class EnforcementsDataOtherSite                                          // 
    {/// <summary>
     /// Назва
     /// </summary>
        public string Name { get; set; }                                            // 
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string Code { get; set; }                                            // 
        /// <summary>
        /// Тип сторони (Фізична / Юридична)
        /// </summary>
        public string Type { get; set; }                                            // 
        /// <summary>
        /// Стан (зареєстровано, припинено)
        /// </summary>
        public string State { get; set; }                                           // 
    }
}
