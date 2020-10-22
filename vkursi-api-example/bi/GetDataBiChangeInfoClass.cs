using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.bi
{
    public class GetDataBiChangeInfoClass
    {
        /*
        
        Отримати перелік компаній по яким відбулись зміни в межах Bi моніторингу
        [POST] /api/1.0/bi/GetDataBiChangeInfo
         
        Приклад 1: без Body (через параметри в URL)

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo?LabelId=1c891112-b022-4a83-ad34-d1f976c60a0b&Size=1000&IsOnlyNew=true&DateChange=2019-11-28T19:00:00.000' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...'


        Приклад 2: з Body

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":1000,"DateChange":"2019-11-28T19:00:52.059","IsOnlyNew":true}'

        */


        public static GetDataBiChangeInfoRequestModel GetDataBiChangeInfo(DateTime dateChange, string labelId, bool isNewOnly, int size, string token)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetDataBiChangeInfoBodyModel GBDRequestBody = new GetDataBiChangeInfoBodyModel
                {
                    LabelId = labelId,                                              // Id списку(Label) (в якому відбулись зміни)
                    Size = size,                                                    // Розмір даних (від 1 до 10000)
                    IsOnlyNew = isNewOnly,                                          // Отримати тільки нові записи (true - отримати ті які до раніще не отримували / false - отримати всі за дату)
                    DateChange = dateChange                                         // Дата від якої необхадно отримати зміни (при повторному запиті необхідно використовувати MaxDateChange з відповіді)
                };

                string body = JsonConvert.SerializeObject(GBDRequestBody);          // Example Body: {"LabelId":"1c891112-b022-4a83-ad34-d1f976c60a0b","Size":1000,"DateChange":"2019-11-28T19:00:52.059","IsOnlyNew":true}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo");
                // https://vkursi-api.azurewebsites.net/api/1.0/bi/GetDataBiChangeInfo?LabelId=1c891112-b022-4a83-ad34-d1f976c60a0b&Size=1000&IsOnlyNew=true&DateChange=2019-11-28T19:00:00.000
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

            GetDataBiChangeInfoRequestModel GetBiDataList = new GetDataBiChangeInfoRequestModel();

            GetBiDataList = JsonConvert.DeserializeObject<GetDataBiChangeInfoRequestModel>(responseString);

            return GetBiDataList;
        }
    }

    public class GetDataBiChangeInfoBodyModel                                   // Модель Body запиту
    {
        public string LabelId { get; set; }                                     // Id списку (в якому відбулись зміни)
        public int Size { get; set; }                                           // Розмір даних (від 1 до 10000)
        public bool IsOnlyNew { get; set; }                                     // Отримати тільки нові записи (true - отримати ті які до раніще не отримували / false - отримати всі за дату)
        public DateTime DateChange { get; set; }                                // Дата коли відбулись зміни
    }

    public class GetDataBiChangeInfoRequestModel                                // Модель відповіді GetDataBiChangeInfo
    {
        public bool IsSuccess { get; set; }                                     // Успішно виконано?
        public string Status { get; set; }                                      // success, error, (Дані успішно знайдено. Pack: " + part)
        public int Code { get; set; }                                           // 404, 200, ...
        public DateTime MaxDateChange { get; set; }                             // Максимальна дата зміни по копанії в відповіді
        public List<DataBiChangeInfoModel> Data { get; set; }                   // Перелік компаній
    }

    public class DataBiChangeInfoModel                                          // Перелік компаній
    {
        public DateTime? DateChange { get; set; }                               // Дата коли відбулись зміни
        public int ChangeType { get; set; }                                     // 1 - Новий / 2 - Зміна / 3 - На відалення (більше не відповідає критеріям)
        public OrganizationBiInfoModel Data { get; set; }                       // Інформація про організацію
    }

    public class OrganizationBiInfoModel                                        // Інформація про організацію
    {
        public string Id { get; set; }                                          // Id компанії
        public string DateStart { get; set; }                                   // Дата першого додавання компанії в Label
        public string Label { get; set; }                                       // Назва збереженного списку
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

        public string Pratsivnikiv2019 { get; set; }                            // Штатна чисельність працівників - 2019
        public string SumaEksportny2019 { get; set; }                           // Сума експортних операцій - 2019
        public string SumaImporty2019 { get; set; }                             // Сума імпортних операцій - 2019

        public double? d108002UsohozarozdilomID1109502 { get; set; }
        public double? d126002UsohozarozdilomIID1119502 { get; set; }
        public double? d124002IIINeoborotniaktyvy { get; set; }
        public double? d128002BalansaktivuD1130002 { get; set; }
        public double? d138002UsohozarozdilomID1149502 { get; set; }
        public double? d148002UsohozarozdilomIID1159502 { get; set; }
        public double? d162002UsohozarozdilomIIID1169502 { get; set; }
        public double? d1170002D141802IVZobovyazanny { get; set; }
        public double? d203501Chystyydokhid { get; set; }
        public double? d210001Finansovyyrezul { get; set; }
        public double? d210501FinRez { get; set; }
        public double? d217001FinRezdoopodatkuvannyaprybutok { get; set; }
        public double? d217501FinRezdoopodatkuvannyazbytok { get; set; }
        public double? d222001ChystyyFinRezprybutokD2235001 { get; set; }
        public double? d222501ChystyyFinRezzbytokD2235501 { get; set; }
        public double? d207001RazomdokhodyD2228001 { get; set; }
        public double? d212001RazomvytratyD2228501 { get; set; }
        public int? forma { get; set; }
    }

}
