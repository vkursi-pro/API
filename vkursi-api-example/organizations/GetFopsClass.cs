using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vkursi_api_example.token;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace vkursi_api_example.organizations
{
    public class GetFopsClass
    {
        /// <summary>
        /// 3. Запит на отримання коротких даних по ФОП за кодом ІПН
        /// [POST] /api/1.0/organizations/getfops
        /// </summary>
        /// <param name="code"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        
        /*       
            cURL запиту:
                curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getfops' \
                --header 'ContentType: application/json' \
                --header 'Authorization: Bearer eyJhbGciOiJI...' \
                --header 'Content-Type: application/json' \
                --data-raw '{"code": ["3334800417"]}'
        */
        public static List<GetFopsResponseModel> GetFops(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

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
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
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
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetFopsRequestBodyModel                            
    {/// <summary>
     /// Перелік кодів ІПН
     /// </summary>
     /// <example
        public List<string> code = new List<string>();               
    }
    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetFopsResponseModel                                
    {/// <summary>
     /// Системний Id Fop
     /// </summary>
     /// <example></example>
        public Guid Id { get; set; }                                 
        /// <summary>
        /// ПІБ ФОПа (maxLength:256)
        /// </summary>
        public string Name { get; set; }                             
        /// <summary>
        /// Статус реєстрації (maxLength:64)
        /// </summary>
        public string State { get; set; }                            
        /// <summary>
        /// Код ІПН (maxLength:10)
        /// </summary>
        public string Code { get; set; }                             
        /// <summary>
        /// Код ІПН (ПДВ) (maxLength:10)
        /// </summary>
        public string Inn { get; set; }                             
        /// <summary>
        /// Дата анулючання свідоцтва платника ПДВ
        /// </summary>
        public DateTime? DateCanceledInn { get; set; }               
        /// <summary>
        /// Дата реєстрації платником ПДВ
        /// </summary>
        public DateTime? DateRegInn { get; set; }                    
        /// <summary>
        /// Наявні виконавчі провадження
        /// </summary>
        public int? Introduction { get; set; }                       
        /// <summary>
        /// Загальна кількість ризиків
        /// </summary>
        public int? ExpressScore { get; set; }                       
        /// <summary>
        /// Наявний податковий борг (true - так / false - ні)
        /// </summary>
        public bool? HasBorg { get; set; }                           
        /// <summary>
        /// Наявні санкції (true - так / false - ні)
        /// </summary>
        public bool? InSanctions { get; set; }                       
        /// <summary>
        /// Відомості про платника ЄП
        /// </summary>
        public SingleTaxPayer singleTaxPayer { get; set; }           
    }/// <summary>
     /// Відомості про платника ЄП
     /// </summary>
    public class SingleTaxPayer                                      
    {/// <summary>
     /// Дата реєстрації платником ЄП
     /// </summary>
        public DateTime dateStart { get; set; }                      
        /// <summary>
        /// Ставка
        /// </summary>
        public double rate { get; set; }                            
        /// <summary>
        /// Група
        /// </summary>
        public int group { get; set; }                              
        /// <summary>
        /// Дата анулювання
        /// </summary>
        public DateTime? dateEnd { get; set; }                      
        /// <summary>
        /// Вид діяльності (maxLength:256)
        /// </summary>
        public string kindOfActivity { get; set; }                   
        /// <summary>
        /// Статус (true - платник ЄП / false - не платник ЄП)
        /// </summary>
        public bool status { get; set; }                            
    }
}
