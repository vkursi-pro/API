using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;
using vkursi_api_example.organizations;

namespace vkursi_api_example._2._0
{
    public class ApiConstructorClass
    {
        /*

        Метод:
            69. API 2.0 Конструктор API 
            [POST] /api/2.0/ApiConstructor

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/ApiConstructorResponse.json
        
         */

        public static ApiConstructorResponseModel ApiConstructor(ref string token, string edrpou, HashSet<int> methodsToExecute)
        {
            if (string.IsNullOrEmpty(token)) 
            {
                AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/2.0/ApiConstructor");
                RestRequest request = new RestRequest(Method.POST);

                ApiConstructorRequestBodyModel GOFSRequest = new ApiConstructorRequestBodyModel
                {
                    Edrpou = new List<string> { edrpou },
                    MethodsToExecute = methodsToExecute
                };

                string body = JsonConvert.SerializeObject(GOFSRequest); // Example: {"Edrpou":["41462280"],"Ipn":null,"MethodsToExecute":[4,9,41,37,57,66,32,39,70],"GetAdvancedOrganizationFilter":null,"GetRelationsFilter":null,"ShortFinanceYearFilter":null}

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

            ApiConstructorResponseModel ACResponseRow = new ApiConstructorResponseModel();

            ACResponseRow = JsonConvert.DeserializeObject<ApiConstructorResponseModel>(responseString);

            return ACResponseRow;
        }
    }

    public class ApiConstructorRequestBodyModel                             // Модель запиту 
    {
        public List<string> Edrpou { get; set; }                            // Код ЄДРПОУ
        public List<string> Ipn { get; set; }                               // Код ИПН
        public HashSet<int> MethodsToExecute { get; set; }                  // Перелік методів по яким буде віконано пошук
        public Api2GetAdvancedOrganizationFilter GetAdvancedOrganizationFilter { get; set; }    // Додатковый параметр. Чи потрыбный прямый запит на Nais (true - так / false - ні)
        public Api2GetRelationsFilter GetRelationsFilter { get; set; }      // Додатковый параметр. Для звязків 
        public HashSet<int> ShortFinanceYearFilter { get; set; }            // Додатковый параметр. Для фінансів 
    }

    public class Api2GetAdvancedOrganizationFilter                          // Додатковый параметр. Чи потрыбный прямий запит на Nais (true - так / false - ні)
    {
        public bool? NeedUpdate { get; set; } = true;                       // Чи потрібний прямий запит на Nais (true - так / false - ні)
    }

    public class Api2GetRelationsFilter                                     // Додаткові параметри для звязків 
    {
        public HashSet<int> FilterRelationType { get; set; }                // Фільтр по тіпу звязків
        public int? MaxRelationLevel { get; set; } = 2;                     // Кількість рівнів
        public List<string> RelationId { get; set; }                        // Id звязку
    }


    public class ApiConstructorResponseModel                                // Відповідь на запит
    {
        public List<int> ErrorList { get; set; }                            // Перелік методів по яким віявлені помилки
        public List<Api2AnswerModelRelationData> GetRelationsData { get; set; } // Відповідь по методу 41. Отримати список пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій
        public List<OrgLicensesApiApiAnswerModelData> GetOrgLicensesInfoData { get; set; }  // Відповідь по методу 37. Перелік ліцензій, та дозволів
        public List<Api2AnswerModelOrgFinanceData> GetOrgFinanceData { get; set; } // Відповідь по методу 57. Аналіз фінансових показників підприємства за кодом ЄДРПОУ
        public List<GetAnalyticResponseModel> GetAnalyticData { get; set; } // 9. Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ
        public List<OrganizationaisElasticModel> GetAdvancedOrganizationData { get; set; } // Відповідь по методу 4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН 
        public List<Api2AnswerModelGetRequisites> GetRequisitesData { get; set; } // 66. Отримати дані реквізитів для строреня картки ФОП / ЮО
        public List<Api2AnswerModelVehicleData> VehicleData { get; set; } // 32. Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ
        public List<OrgShareHoldersApiAnswerModelData> ShareHoldersData { get; set; } // 39. Відомості про власників пакетів акцій (від 5%)
        public List<GetOrgFinanceShortAnswerData> ShortFinanceData { get; set; } // 69. Скорочені основні фінансові показники діяльності підприємства 
        public string StatusMessage { get; set; } // Повідомлення про помилку
        public int StatusCode { get; set; } // Статус відповіді
    }

    public class Api2AnswerModelRelationData
    {
        public string Edrpou { get; set; }
        public List<GetRelationApiModelAnswerData> Data { get; set; }
    }
    public class Api2AnswerModelOrgFinanceData
    {
        public string Code { get; set; }
        public object Data { get; set; }
    }

    public class Api2AnswerModelGetRequisites
    {
        public string Code { get; set; }
        public GetRequisitesResponseData Data { get; set; }
    }

    public class Api2AnswerModelVehicleData
    {
        public string Code { get; set; }
        public List<VehicleOrgApiAnswerModelDataVehicle> Data { get; set; }
    }


    // 4.
    // api/1.0/organizations/getadvancedorganization
    // 9.
    // api/1.0/organizations/getanalytic
    // 41.
    // api/1.0/organizations/getrelations
    // 37.
    // api/1.0/organizations/getorglicensesinfo
    // 57.
    // api/1.0/organizations/GetOrgFinance
    // 66. 
    // api/1.0/organizations/GetRequisites
    // 32.
    // api/1.0/organizations/getorgvehicle
    // 39.
    // api/1.0/organizations/getorgshareholders
    // 70.
    // api/1.0/organizations/GetOrgFinanceShort
}
