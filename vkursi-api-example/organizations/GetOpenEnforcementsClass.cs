﻿using System;
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
        [POST] /api/organizations/GetOpenEnforcements 
         
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
                    Code = new List<string> { code }                        // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GOERBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/organizations/GetOpenEnforcements");
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

    public class GetOpenEnforcementsRequestBodyModel                                // Модель запиту (Example: {"code":["21560766"]})
    {
        public List<string> Code { get; set; }                                      // Код ЄДРПОУ
    }

    public class GetOpenEnforcementsResponseModel                                   // Модель на відповідь GetOpenEnforcements
    {
        public string Status { get; set; }                                          // Статус запиту (maxLength:128)
        public bool IsSuccess { get; set; }                                         // Запит виконано успішно (true - так / false - ні)
        public List<EnforcementsData> Data { get; set; }                            // Дані
    }
    public class EnforcementsData                                                   // Дані
    {
        public string Code { get; set; }                                            // Код ЕДРПОУ (за яким знайдено дані)
        public List<EnforcementsDataList> EnforcementsList { get; set; }            // Перелік проваджень
    }
    public class EnforcementsDataList                                               // Перелік проваджень
    {

        public DateTime? DateOpen { get; set; }                                         // Дата відкриття ВП
        public string VpNum { get; set; }                                               // Номер ВП (maxLength:32)
        public string Status { get; set; }                                              // Статус ВП (Приклад: Боржник, Стягувач, ...) (maxLength:128)
        public string Category { get; set; }                                            // Категорія ВП (maxLength:256)
        public string State { get; set; }                                               // Стан (Приклад: Завершено, Примусове виконання, ...) (maxLength:128)
        public string Contractor { get; set; }                                          // Інша сторона (Приклад: Київське міжрегіональне управління укртрансбезпеки Код ЄДРПОУ: 37995466) (maxLength:512)
        public string Publisher { get; set; }                                           // Документ виданий
        public string DepartmentName { get; set; }                                      // Зв'язок з виконавцем: Назва виконавця
        public string DepartmentPhone { get; set; }                                     // Зв'язок з виконавцем: Номер телефону виконавця
        public string Executor { get; set; }                                            // ПІБ виконавця
        public string ExecutorPhone { get; set; }                                       // Номер телефону виконавця
        public string ExecutorEmail { get; set; }                                       // Email виконавця

        [JsonProperty("creditorType")]
        public string CreditorType { get; set; }                                        // Тип стягувача (Фізична / Юридична / Держава / ...)

        [JsonProperty("creditorName")]
        public string CreditorName { get; set; }                                        // Назва стягувача

        [JsonProperty("creditorBithday")]
        public object CreditorBithday { get; set; }                                     // Дата народження стягувача (якщо ФО)

        [JsonProperty("debtorName")]
        public string DebtorName { get; set; }                                          // Назва боржника

        [JsonProperty("debtorCode")]
        public object DebtorCode { get; set; }                                          // Код боржника

        [JsonProperty("debtorType")]
        public string DebtorType { get; set; }                                          // Тип боржника (Фізична / Юридична / Держава / ...)

        public string StateCurrentOrganization { get; set; }                        // (in orgId) ? боржник : стягувач
        public string DeductionType { get; set; }                                   // 
        public DateTime? EnforcementDate { get; set; }                              // 
        public DateTime? ModifyDate { get; set; }                                   // 
        public EnforcementsDataOtherSite OtherSite { get; set; }                    // 
    }

    public class EnforcementsDataOtherSite                                          // 
    {
        public string Name { get; set; }                                            // 
        public string Code { get; set; }                                            // 
        public string Type { get; set; }                                            // 
        public string State { get; set; }                                           // 
    }
}