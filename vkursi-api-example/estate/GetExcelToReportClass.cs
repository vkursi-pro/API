using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    /// <summary>
    /// 180. Отримати excel для звіту по землі в форматі json
    /// </summary>
    public class GetExcelToReportClass
    {
        public static EstateCreateTaskApiResponseBodyModel GetExcelToReport(ref string token, Guid reportId)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getexceltoreport");
                RestRequest request = new RestRequest(Method.GET);

                request.AddParameter("reportId", reportId);                 // Id звіту

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", $"Bearer {token}");

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            EstateCreateTaskApiResponseBodyModel ECTAResponseBody =
                JsonConvert.DeserializeObject<EstateCreateTaskApiResponseBodyModel>(responseString);

            return ECTAResponseBody;

            //if (ECTAResponseBody.isSuccess == true) // ECTAResponseBody.isSuccess = true - задача створена успішно
            //{
            //    return ECTAResponseBody.taskId;     // Id задачі за яким ми будемо перевіряти її виконання
            //}
            //else
            //{
            //    Console.WriteLine("error: {0}", ECTAResponseBody.status);
            //    return null;

            //    /* ECTAResponseBody.status = "Not enough money" - недостатньо коштів
            //     * ECTAResponseBody.status = "Unexpected server error" - непередвачувана помилка
            //     */
            //}

            /*

                // Python - http.client example:

                import http.client
                import mimetypes
                conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
                payload = "{\"Edrpous\":[\"19124549\"],\"Ipns\":[\"3083707142\"],\"Koatuus\":[\"5621287500\"],\"Cadastrs\":[\"5621287500:03:001:0019\"],\"CalculateCost\":false,\"IsNeedUpdateAll\":false,\"IsReport\":true,\"TaskName\":\"Назва задачі\",\"DzkOnly\":false}"
                headers = {
                  'ContentType': 'application/json',
                  'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
                  'Content-Type': 'application/json',
                  'Cookie': 'ARRAffinity=60c7763e47a70e864d73874a4687c10eb685afc08af8bda506303f7b37b172b8'
                }
                conn.request("POST", "/api/1.0/estate/estatecreatetaskapi", payload, headers)
                res = conn.getresponse()
                data = res.read()
                print(data.decode("utf-8"))


                // Java - OkHttp example:

                OkHttpClient client = new OkHttpClient().newBuilder()
                  .build();
                MediaType mediaType = MediaType.parse("application/json");
                RequestBody body = RequestBody.create(mediaType, "{\"Edrpous\":[\"19124549\"],\"Ipns\":[\"3083707142\"],\"Koatuus\":[\"5621287500\"],\"Cadastrs\":[\"5621287500:03:001:0019\"],\"CalculateCost\":false,\"IsNeedUpdateAll\":false,\"IsReport\":true,\"TaskName\":\"Назва задачі\",\"DzkOnly\":false}");
                Request request = new Request.Builder()
                  .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/estatecreatetaskapi")
                  .method("POST", body)
                  .addHeader("ContentType", "application/json")
                  .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
                  .addHeader("Content-Type", "application/json")
                  .build();
                Response response = client.newCall(request).execute();

            */
        }
    }

    /// <summary>
    /// Модель выдповіді
    /// </summary>
    public class EstateReportFullExtendedExcelModel
    {
        /// <summary>
        /// id звіту
        /// </summary>
        public Guid ReportId { get; set; }
        /// <summary>
        /// Дані по кадастрових номерах
        /// </summary>
        public List<EstateReportFullExtendedCadastrExcelModel> CadastrInfo { get; set; }
    }

    /// <summary>
    /// Дані по кадастрових номерах
    /// </summary>
    public class EstateReportFullExtendedCadastrExcelModel
    {
        /// <summary>
        /// Кадастровий номер
        /// </summary>
        public string CadastrNumber { get; set; }
        /// <summary>
        /// Сільська рада
        /// </summary>
        public string KoatuuName { get; set; }
        /// <summary>
        /// КОАТУУ
        /// </summary>
        public string Koatuu { get; set; }
        /// <summary>
        /// Площа за ДЗК
        /// </summary>
        public double? DzkArea { get; set; }
        /// <summary>
        /// Площа за РРП
        /// </summary>
        public double? RrpArea { get; set; }
        /// <summary>
        /// Різниця площ
        /// </summary>
        public double? DifferenceArea { get; set; }
        /// <summary>
        /// Площа за ПККУ
        /// </summary>
        public double? PkkuArea { get; set; }
        /// <summary>
        /// Категорія земель
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Цільове призначення за ДЗК
        /// </summary>
        public string DzkPurpose { get; set; }
        /// <summary>
        /// Цільове призначення за РРП
        /// </summary>
        public string RrpPurpose { get; set; }
        /// <summary>
        /// Форма власності за ДЗК
        /// </summary>
        public string DzkOwnerForm { get; set; }
        /// <summary>
        /// Форма власності за РРП
        /// </summary>
        public string RrpOwnerForm { get; set; }
        /// <summary>
        /// Вид угіддя
        /// </summary>
        public string VidUgiddya { get; set; }
        /// <summary>
        /// НГО за ділянку, грн
        /// </summary>
        public double? NgoPrice { get; set; }
        /// <summary>
        /// НГО за гектар, грн
        /// </summary>
        public double? NgoPricePerGekt { get; set; }
        /// <summary>
        /// Дата проведення оцінки
        /// </summary>
        public DateTime? NgoDate { get; set; }

        //---Інформація про власників

        /// <summary>
        /// Кількість власників
        /// </summary>
        public int? OwnerCount { get; set; }
        /// <summary>
        /// Власник за ДЗК
        /// </summary>
        public string DzkOwnerName { get; set; }
        /// <summary>
        /// Власник за РРП
        /// </summary>
        public string RrpOwnerName { get; set; }
        /// <summary>
        /// Код Власника за ДЗК
        /// </summary>
        public string DzkOwnerCode { get; set; }
        /// <summary>
        /// Код Власника за РРП
        /// </summary>
        public string RrpOwnerCode { get; set; }
        /// <summary>
        /// % володіння за ДЗК
        /// </summary>
        public string DzkOwnerPercent { get; set; }
        /// <summary>
        /// % володіння за РРП	
        /// </summary>
        public string RrpOwnerPercent { get; set; }
        /// <summary>
        /// Дата реєстрації права за ДЗК
        /// </summary>
        public string DzkOwnerRegistrationDate { get; set; }
        /// <summary>
        /// Дата реєстрації права за РРП
        /// </summary>
        public string RrpOwnerRegistrationDate { get; set; }
        /// <summary>
        /// Номер реєстрації права за ДЗК
        /// </summary>
        public string DzkOwnerEntryNumberRight { get; set; }
        /// <summary>
        /// Номер реєстрації права за РРП  
        /// </summary>
        public string RrpOwnerEntryNumberRight { get; set; }       //                                                                  
        /// <summary>
        /// Вид документу (права власності)
        /// </summary>
        public string OwnerDocumentRight { get; set; }
        /// <summary>
        /// Серія та номер документу
        /// </summary>
        public string OwnerNumberSeriesDocument { get; set; }
        //---Інформація про власників

        //---Інформація про користувачів
        /// <summary>
        /// Кількість користувачів
        /// </summary>
        public int TenantCount { get; set; }
        /// <summary>
        /// Користувач за ДЗК
        /// </summary>
        public string DzkTenantName { get; set; }
        /// <summary>
        /// Користувач за РРП
        /// </summary>
        public string RrpTenantName { get; set; }
        /// <summary>
        /// Код Користувача за ДЗК
        /// </summary>
        public string DzkTenantCode { get; set; }
        /// <summary>
        /// Код Користувача за РРП
        /// </summary>
        public string RrpTenantCode { get; set; }
        /// <summary>
        /// Дата реєстрації права за ДЗК
        /// </summary>
        public string DzkTenantRegistrationDate { get; set; }
        /// <summary>
        /// Дата реєстрації права за РРП
        /// </summary>
        public string RrpTenantRegistrationDate { get; set; }
        /// <summary>
        /// Номер реєстрації права за ДЗК
        /// </summary>
        public string DzkTenantEntryNumberRight { get; set; }
        /// <summary>
        /// Номер реєстрації права за РРП
        /// </summary>
        public string RrpTenantEntryNumberRight { get; set; }
        /// <summary>
        /// Дата укладання договору
        /// </summary>
        public string SignTenantDateContract { get; set; }
        /// <summary>
        /// Тип право користування
        /// </summary>
        public string RightTypeTenant { get; set; }
        /// <summary>
        /// Строк дії права користування
        /// </summary>
        public string ActTermTextTenant { get; set; }
        /// <summary>
        /// Дата завершення дії права користування
        /// </summary>
        public string DefaultEndDateTenant { get; set; }
        /// <summary>
        /// Дата завершення дії права користування (розраховано сервісом)
        /// </summary>
        public string DateEndTenant { get; set; }
        /// <summary>
        /// Орендна плата, грн	
        /// </summary>
        public string SumRentTenant { get; set; }
        /// <summary>
        /// Орендна плата % від НГО
        /// </summary>
        public string PercentSumRentFromNgoTenant { get; set; }

        //---Інформація про користувачів

        /// <summary>
        /// Тип обмеження
        /// </summary>
        public string RestrictionType { get; set; }

        //---Інформація про обтяження за РРП

        /// <summary>
        /// Обтяження за РРП
        /// </summary>
        public string LimitationType { get; set; }
        /// <summary>
        /// Дата реєстрації обтяження
        /// </summary>
        public string LimitationStartDate { get; set; }
        /// <summary>
        /// Номер реєстрації обтяження
        /// </summary>
        public string LimitationNumber { get; set; }
        /// <summary>
        /// Обтяжувач
        /// </summary>
        public string LimitationParticipant { get; set; }
        //---Інформація про обтяження за РРП
        //---Інформація про іпотеку за РРП
        /// <summary>
        /// Розмір зобов'язання
        /// </summary>
        public string MortgageSum { get; set; }
        /// <summary>
        /// Іпотекодавець
        /// </summary>
        public string MortgageParticipant { get; set; }
        /// <summary>
        /// Строк виконання іпотеки
        /// </summary>
        public string MortgageEndDate { get; set; }

        //---Інформація про іпотеку за РРП

        /// <summary>
        /// Кількість судових документів на ділянку
        /// </summary>
        public string CourtDecisionCount { get; set; }
        /// <summary>
        /// Площа володіння за ДЗК
        /// </summary>
        public string DzkOwnerArea { get; set; }
        /// <summary>
        /// Площа володіння за РРП
        /// </summary>
        public string RrpOwnerArea { get; set; }
        /// <summary>
        /// ОНМ
        /// </summary>
        public string Onm { get; set; }

        //---Інформація по полям
        /// <summary>
        /// Площа обработки
        /// </summary>
        public double FieldCultivationArea { get; set; }
        /// <summary>
        /// Площа тех. втрат
        /// </summary>
        public double FieldTehnicalLoseArea { get; set; }
        /// <summary>
        /// Поле входження	
        /// </summary>
        public string FieldIntersectInfo { get; set; }

        //---Інформація по полям

        /// <summary>
        /// Дата укладання договору (розраховано сервісом)
        /// </summary>                                                        
        public string DateStartTenant { get; set; }
        /// <summary>
        /// Опис предмета іншого речового права
        /// </summary>
        public string DescriptionRights { get; set; }
    }
}
