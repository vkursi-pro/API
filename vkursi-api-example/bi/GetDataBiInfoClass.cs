using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;
using System.ComponentModel;

namespace vkursi_api_example.bi
{
    class GetDataBiInfoClass
    {
        /*
         
        Отримати перелік компаний які відібрані в модулі BI
        [POST] /api/1.0/bi/GetDataBiInfo

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiInfo' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":100,"DateStart":"2019-11-28T19:00:52.059"}'
         
        */

        public static GetDataBiInfoRequestModel GetDataBiInfo(string labelId, int size, DateTime dateStart, string token)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDataBiInfoRequestBodyModel GBDRequestBody = new GetDataBiInfoRequestBodyModel
                {
                    LabelId = labelId,                                              // Назва збереженного списку (перелік можна в GET /api/1.0/BI/getbiimportlabels)
                    Size = size,                                                    // К-ть записів в відповіді. При кожному отриманні відповіді всі записи зберігаються з певним Pack id по значенню якого записи можуть бути отримані повторно.
                    DateStart = dateStart                                           // Дата з якої почнеться відбір (відсортованій в порядку зменшення) від якої буде братись Size
                };

                string body = JsonConvert.SerializeObject(GBDRequestBody);          // Example Body: {"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":1000,"DateStart":"2019-11-28T19:00:52.059"}

                // 

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiInfo");
                // https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiInfo?LabelId=1c891112-b022-4a83-ad34-d1f976c60a0b&Size=1000&DateStart=2019-11-28T19:00:52.059
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

            GetDataBiInfoRequestModel GetBiDataList = new GetDataBiInfoRequestModel();

            GetBiDataList = JsonConvert.DeserializeObject<GetDataBiInfoRequestModel>(responseString);

            return GetBiDataList;
        }
    }
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetDataBiInfoRequestBodyModel                                  // 
    {/// <summary>
     /// Назва збереженного списку
     /// </summary>
        public string LabelId { get; set; }                                     // 
        /// <summary>
        /// Розмір списку (від 1 до 10000)
        /// </summary>
        public int Size { get; set; }                                           // 
        /// <summary>
        /// Дата з якої почнеться відбір (відсортованій в порядку зменшення) від якої буде братись Size
        /// </summary>
        public DateTime? DateStart { get; set; }                                // 
    }

    /// <summary>
    /// Модель відповіді GetBiData
    /// </summary>
    class GetDataBiInfoRequestModel                                             // 
    {/// <summary>
     /// Успішно виконано?
     /// </summary>
        public bool isSuccess { get; set; }                                     // 
        /// <summary>
        /// success, error, (Дані успішно знайдено. Pack: " + part)
        /// </summary>
        public string status { get; set; }                                      // 
        /// <summary>
        /// 404, 200, ...
        /// </summary>
        public int code { get; set; }                                           // 
        /// <summary>
        ///Перелік компаній 
        /// </summary>
        public List<GetDataCompanyModel> data { get; set; }                     // 
    }
    /// <summary>
    /// Перелік компаній
    /// </summary>
    public class GetDataCompanyModel                                                  // 
    {
        /// <summary>
        /// Унікальний ідентифікатор
        /// </summary>
        [DisplayName("Унікальний ідентифікатор")]
        public string Id { get; set; }
        /// <summary>
        /// Дата початку
        /// </summary>
        [DisplayName("Дата початку")]
        public string DateStart { get; set; }
        /// <summary>
        /// Назва мітки
        /// </summary>
        [DisplayName("Назва мітки")]
        public string Label { get; set; }

        /// <summary>
        /// Державний класифікатор об'єктів адміністративно-територіального устрою України
        /// </summary>
        [DisplayName("КОАТУУ")]
        public string Koatuu { get; set; }

        /// <summary>
        /// Повне найменування підприємства
        /// </summary>
        [DisplayName("Повне найменування підприємства")]
        public string NazvaPidpriyemstva { get; set; }

        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        [DisplayName("Код ЄДРПОУ")]
        public string KodYedrpou { get; set; }

        /// <summary>
        /// Дата реєстрації
        /// </summary>
        [DisplayName("Дата реєстрації")]
        public string DataReyestratsiyi { get; set; }
        /// <summary>
        /// Статус юридичної особи
        /// </summary>
        [DisplayName("Статус юридичної особи")]
        public string StatusYuridichnoyiOsobi { get; set; }

        /// <summary>
        /// Відомості про банкрутство
        /// </summary>
        [DisplayName("Відомості про банкрутство")]
        public string VidomostiProBankrutstvo { get; set; }

        /// <summary>
        /// Відомості про припинення або реорганізацію юридичної особи
        /// </summary>
        [DisplayName("Відомості про припинення або реорганізацію юридичної особи")]
        public string VidomostiProPripinennya { get; set; }

        /// <summary>
        /// Адреса реєстрації підприємства
        /// </summary>
        [DisplayName("Адреса реєстрації підприємства")]
        public string AdresaReyestratsiyi { get; set; }

        /// <summary>
        /// КВЕД (основний)
        /// </summary>
        [DisplayName("КВЕД (основний)")]
        public string KvedOsnovniy { get; set; }
        /// <summary>
        /// Статутний капітал
        /// </summary>
        [DisplayName("Статутний капітал")]
        public string StatutniyKapital { get; set; }
        /// <summary>
        /// Уповноважені особи
        /// </summary>
        [DisplayName("Уповноважені особи")]
        public string UpovnovazheniOsobi { get; set; }
        /// <summary>
        /// Кількість засновників
        /// </summary>
        [DisplayName("Кількість засновників")]
        public string KilkistZasnovnikiv { get; set; }
        /// <summary>
        /// Іноземні бенефіціари
        /// </summary>
        [DisplayName("Іноземні бенефіціари")]
        public string InozomniBenefitsiari { get; set; }
        /// <summary>
        /// Кількість власників пакетів акцій
        /// </summary>
        [DisplayName("Кількість власників пакетів акцій")]
        public string KilkistVlasnikivAktsiy { get; set; }
        /// <summary>
        /// Відокремлені підрозділи
        /// </summary>
        [DisplayName("Відокремлені підрозділи")]
        public string VidkremleniPidrozdili { get; set; }
        /// <summary>
        /// Реєстр платників ПДВ
        /// </summary>
        [DisplayName("Реєстр платників ПДВ")]
        public string ReyestrPlatnikivPdv { get; set; }

        /// <summary>
        /// Анульована реєстрація платників ПДВ
        /// </summary>
        [DisplayName("Анульовано ПДВ")]
        public string AnulovanoPdv { get; set; }
        /// <summary>
        /// Реєстр платників єдиного податку
        /// </summary>
        [DisplayName("Реєстр платників єдиного податку")]
        public string ReyestrPlatnikivYep { get; set; }
        /// <summary>
        /// Санкційні списки
        /// </summary>
        [DisplayName("Санкційні списки")]
        public string SanktsiyniSpiski { get; set; }
        /// <summary>
        /// Виконавчі впровадження
        /// </summary>
        [DisplayName("Виконавчі впровадження")]
        public string VikonavchiVprovadzhennya { get; set; }

        /// <summary>
        /// Судові рішення
        /// </summary>
        [DisplayName("Судові рішення")]
        public string SudoviRishennya { get; set; }

        /// <summary>
        /// Судові засідання (Справи призначені до розгляду)
        /// </summary>
        [DisplayName("Судові засідання")]
        public string SudoviZasidannya { get; set; }

        ///// <summary>
        ///// Включено в план-графік перевірок 2020
        ///// </summary>
        //[DisplayName("План-графік перевірок 2020")]
        //public string PlangrafikPerevirok2020 { get; set; }
        /// <summary>
        /// Перевіряючий орган
        /// </summary>
        [DisplayName("Перевіряючий орган")]
        public string PereviryayuchiyOrgan { get; set; }
        /// <summary>
        /// Податковий борг
        /// </summary>
        [DisplayName("Податковий борг")]
        public string PodatkoviyBorg { get; set; }
        /// <summary>
        /// Перед місцевим бюджетом
        /// </summary>
        [DisplayName("Перед місцевим бюджетом")]
        public string PeredMistsevimByudzhetom { get; set; }
        /// <summary>
        /// Перед державним бюджетом
        /// </summary>
        [DisplayName("Перед державним бюджетом")]
        public string PeredDerzhavnimByudzhetom { get; set; }
        /// <summary>
        /// За юридичним місцезнаходженням зареєстровано більше 1 суб'єкта господарювання
        /// </summary>
        [DisplayName("Компанії за адресою")]
        public string KompaniyZaAdresoyu { get; set; }

        /// <summary>
        /// Зв'язки (керівник) Аналіз компаній, які можливо пов'язані з керівником
        /// </summary>
        [DisplayName("Зв'язки керівника")]
        public string ZvyazkyKerivnyka { get; set; }

        /// <summary>
        /// Зв'язки (засновники) Наявність можливих афілійованих зв'язків по засновникам та бенефіціарам
        /// </summary>
        [DisplayName("Зв'язки бенефіціарів")]
        public string ZvyazkyBenefitsiariv { get; set; }

        /// <summary>
        /// Зв'язки з ЄДРД (РЕР Декларанти та члени сім'ї)
        /// </summary>
        [DisplayName("Зв'язки з ЄДРД")]
        public string ZvyazkiZYedrd { get; set; }

        /// <summary>
        /// Тип/назва ліцензій/дозволів
        /// </summary>
        [DisplayName("Тип/назва ліцензій/дозволів")]
        public string TipnazvaLitsenziydozvoliv { get; set; }

        /// <summary>
        /// Кількість ліцензій/Дозволів
        /// </summary>
        [DisplayName("Кількість ліцензій/дозволів")]
        public string KilkistLitsenziydozvoliv { get; set; }

        /// <summary>
        /// Заборгованість по ЗП
        /// </summary>
        [DisplayName("Заборгованість по ЗП")]
        public string ZaborgovanistPoZp { get; set; }

        /// <summary>
        /// Кількість об'єктів нерухомості
        /// </summary>
        [DisplayName("Кількість об'єктів нерухомості")]
        public string KilkistObyektivNerukhomosti { get; set; }

        /// <summary>
        /// Кількість земельних ділянок
        /// </summary>
        [DisplayName("Кількість земельних ділянок")]
        public string KilkistZemelnikhDilyanok { get; set; }

        /// <summary>
        /// Площа землі
        /// </summary>
        [DisplayName("Площа землі")]
        public string PloshchaZemli { get; set; }

        /// <summary>
        /// Кількість транспортних засобів
        /// </summary>
        [DisplayName("Кількість транспортних засобів")]
        public string KilkistTransportnikhZasobiv { get; set; }

        // <summary>
        /// Кількість тендерів в яких приймав участь
        /// </summary>
        [DisplayName("Кількість тендерів в яких приймав участь")]
        public string? KilkistTenderivPriymavUchast { get; set; }

        /// <summary>
        /// Кількість виграних тендерів
        /// </summary>
        [DisplayName("Кількість виграних тендерів")]
        public string KilkistVigranikhTenderiv { get; set; }

        /// <summary>
        /// Сума виграних тендерів
        /// </summary>
        [DisplayName("Сума виграних тендерів")]
        public string? SumaVigranikhTenderiv { get; set; }

        /// <summary>
        /// Фінансовий стан
        /// </summary>
        [DisplayName("Фінансовий стан")]
        public string? FinansoviyStan { get; set; }

        /// <summary>
        /// Забов'язання
        /// </summary>
        [DisplayName("Забов'язання")]
        public string? Zabovyazannya { get; set; }

        /// <summary>
        /// Міжнародна рейтингова оцінка
        /// </summary>
        [DisplayName("Міжнародна рейтингова оцінка")]
        public string MizhnarodnaReytingovaOtsinka { get; set; }

        /// <summary>
        /// Основні показники в розрізі років
        /// </summary>
        [DisplayName("Основні показники в розрізі років")]
        public List<BiNewExcelMainPerYearModel>? BiNewExcelMainPerYear { get; set; }

        /// <summary>
        /// Контактна інформація
        /// </summary>
        [DisplayName("Контактна інформація")]
        public string KontaktnaInformatsiya { get; set; }

        /// <summary>
        /// Реєстраційний номер
        /// </summary>
        [DisplayName("Реєстраційний номер")]
        public string RegNumb { get; set; }
    }
}
