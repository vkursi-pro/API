using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.bi
{
    public class GetDataBiOrganizationInfoClass
    {
        /*
        
        Отримання відомостей про організацію BI
        [POST] /api/1.0/bi/GetDataBiOrganizationInfo

        Приклад 1: без Body

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiOrganizationInfo?OrganizationsId=1c891112-b022-4a83-ad34-d1f976c60a0b,1c891112-b022-4a83-ad34-d1f976c60a0b&Codes=00131305,00131306' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...'


        Приклад 2: з Body

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiOrganizationInfo' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'ContentType: application/json' \
        --header 'Content-Type: application/json' \
        --data-raw '{"OrganizationsId":["1c891112-b022-4a83-ad34-d1f976c60a0b"],"Codes":["00131305"]}'

        */

        public static GetDataBiOrganizationInfoRequestModel GetDataBiOrganizationInfo(List<string> organizationsId, List<string> codes, string token)
        {
            if (string.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDataBiOrganizationInfoBodyModel GBDRequestBody = new GetDataBiOrganizationInfoBodyModel
                {
                    OrganizationsId = organizationsId,                              // Перелік id організацій
                    Codes = codes,                                                  // Перелік кодів ЄДПРОУ організацій
                };

                string body = JsonConvert.SerializeObject(GBDRequestBody);          // Example Body: {"OrganizationsId":["1c891112-b022-4a83-ad34-d1f976c60a0b"],"Codes":["00131305"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiOrganizationInfo");
                // https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiOrganizationInfo?OrganizationsId=1c891112-b022-4a83-ad34-d1f976c60a0b,1c891112-b022-4a83-ad34-d1f976c60a0b&Codes=00131305,00131306
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetDataBiOrganizationInfoRequestModel GetBiDataList = new GetDataBiOrganizationInfoRequestModel();

            GetBiDataList = JsonConvert.DeserializeObject<GetDataBiOrganizationInfoRequestModel>(responseString);

            return GetBiDataList;
        }
    }

    public class GetDataBiOrganizationInfoBodyModel                             // Модель Body запиту
    {
        public List<string> OrganizationsId { get; set; }                       // Перелік id організацій
        public List<string> Codes { get; set; }                                 // Перелік кодів ЄДПРОУ організацій
    }

    public class GetDataBiOrganizationInfoRequestModel                          // Модель відповіді GetDataBiOrganizationInfo
    {
        public bool IsSuccess { get; set; }                                     // Успішно виконано?
        public string Status { get; set; }                                      // success, error, (Дані успішно знайдено. Pack: " + part)
        public int Code { get; set; }                                           // 404, 200, ...
        public List<DataBiOrganizationInfoModel> Data { get; set; }             // Перелік компаній
    }

    public class DataBiOrganizationInfoModel                                    // Перелік компаній
    {
        public string NazvaPidpriyemstva { get; set; }                          // Повне найменування підприємства
        public string KodYedrpou { get; set; }                                  // Код ЄДРПОУ
        public string DataReyestratsiyi { get; set; }                           // Дата реєстрації
        public string StatusYuridichnoyiOsobi { get; set; }                     // Статус юридичної особи
        public string VidomostiProBankrutstvo { get; set; }                     // Відомості про банкрутство
        public string VidomostiProPripinennya { get; set; }                     // Відомості про припинення або реорганізацію юридичної особи
        public string AdresaReyestratsiyi { get; set; }                         // Адреса реєстрації підприємства
        public string KvedOsnovniy { get; set; }                                // КВЕД (основний)
        public string StatutniyKapital { get; set; }                            // Статутний капітал
        public string UpovnovazheniOsobi { get; set; }                          // Уповноважені особи
        public string KilkistZasnovnikiv { get; set; }                          // Кількість засновників 
        public string InozomniBenefitsiari { get; set; }                        // Інозомні бенефіціари
        public string KilkistVlasnikivAktsiy { get; set; }                      // Кількість Власників пакетів акцій 
        public string VidkremleniPidrozdili { get; set; }                       // Відкремлені підрозділи
        public string ReyestrPlatnikivPdv { get; set; }                         // Реєстр платників ПДВ
        public string AnulovanoPdv { get; set; }                                // Анульована реєстрація платників ПДВ
        public string ReyestrPlatnikivYep { get; set; }                         // Реєстр платників єдинго податку(дописать в еластик)
        public string SanktsiyniSpiski { get; set; }                            // Санкційні списки
        public string VikonavchiVprovadzhennya { get; set; }                    // Виконавчі впровадження
        public string SudoviRishennya { get; set; }                             // Судові рішення 
        public string SudoviZasidannya { get; set; }                            // Судові засідання (Спарави призначені до розгляду)
        public string PlangrafikPerevirok2020 { get; set; }                     // Включено в план-графік перевірок 2020
        public string PereviryayuchiyOrgan { get; set; }                        // Перевіряючий орган
        public string PodatkoviyBorg { get; set; }                              // Податковий борг
        public string PeredMistsevimByudzhetom { get; set; }                    // Перед місцевим бюджетом
        public string PeredDerzhavnimByudzhetom { get; set; }                   // Перед державним бюджетом
        public string KompaniyZaAdresoyu { get; set; }                          // За юридичним місцезнаходженням зареєстровано більше 1 суб’єкта господарювання
        public string ZvyazkyKerivnyka { get; set; }                            // Зв'язки (керівник) Аналіз компаній, які можливо пов’язані з керівником (ПІБ керівника співпадає з ПІБ керівника в інших компаніях)
        public string ZvyazkyBenefitsiariv { get; set; }                        // Зв'язки (засновники) Наявність можливих афілійованих зв’язків по засновникам та бенефіціарам
        public string ZvyazkiZYedrd { get; set; }                               // Зв'язки з ЄДРД (РЕР Декларанти та члени сім'ї)
        public string TipnazvaLitsenziydozvoliv { get; set; }                   // Тип/назва ліцензій/дозволів 
        public string KilkistLitsenziydozvoliv { get; set; }                    // Кількість ліцензій/Дозволів
        public string ZaborgovanistPoZp { get; set; }                           // Заборгованість по ЗП
        public string KilkistObyektivNerukhomosti { get; set; }                 // Кількість об'єктів нерухомості 
        public string KilkistZemelnikhDilyanok { get; set; }                    // Кількість земельних ділянок 
        public string PloshchaZemli { get; set; }                               // Площа землі
        public string KilkistTransportnikhZasobiv { get; set; }                 // Кількість транспортних засобів
        public string Pratsivnikiv2018 { get; set; }                            // Штатна чисельність працівників - 2018
        public string SumaEksportny2018 { get; set; }                           // Сума експортних операцій - 2018
        public string SumaImporty2018 { get; set; }                             // Сума імпортних операцій - 2018
        public string KilkistTenderivPriymavUchast { get; set; }                // Кіькість тендерів в яких приймав участь
        public string KilkistVigranikhTenderiv { get; set; }                    // Кількість виграних тендерів
        public string SumaVigranikhTenderiv { get; set; }                       // Сума виграних тендерів 
        public string KlasBorzhnikaNbu2018 { get; set; }                        // Клас боржника НБУ - 2018
        public string FinansoviyStan { get; set; }                              // Фінансовий стан 
        public string Zabovyazannya { get; set; }                               // Забов'язання 
        public string MizhnarodnaReytingovaOtsinka { get; set; }                // Міжнародна рейтингова оцінка 
        public string OsnovniZasobi2018 { get; set; }                           // Основні засоби 2018
        public string PotochniZabovyazannya2018 { get; set; }                   // Поточні забов'язання 2018
        public string ChistiyDokhidViruchka2018 { get; set; }                   // Чистий дохід (виручка) 2018
        public string ChistiyPributokzbitok2018 { get; set; }                   // Чистий прибуток/збиток 2018
        public string KontaktnaInformatsiya { get; set; }                       // Контактна інформація
        public string Label { get; set; }                                       // Назва збереженного списку
    }
}
