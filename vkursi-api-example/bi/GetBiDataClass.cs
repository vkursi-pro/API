﻿using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.organizations;
using vkursi_api_example.token;

namespace vkursi_api_example.bi
{
    class GetBiDataClass
    {
        /* 17. Отримати перелік компаний які відібрані по BI
         * [POST] api/1.0/bi/getbidata
         * 
         */

        public static List<GetBiDataResponseModel> GetBiData(string label, int size, string token)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            link1:

            RestClient client = new RestClient("https://vkursi-api.azurewebsites.net");
            RestRequest request = new RestRequest("api/1.0/bi/getbidata", Method.POST);

            // Example Body: {"Label":null,"Size":10}

            GetBiDataBodyModel GetBiDataBodyRequestRow = new GetBiDataBodyModel
            {
                Label = label,                  // Назва збереженного списку (перелік можна в GET /api/1.0/BI/getbiimportlabels)
                Size = size,                    // К-ть записів в відповіді. При кожному отриманні відповіді всі записи зберігаються з певним Pack id по значенню якого записи можуть бути отримані повторно.
                Pack = null                     // Отримати записи повторно по Pack id
            };

            string body = JsonConvert.SerializeObject(GetBiDataBodyRequestRow); // body = "{\"Label\":null,\"Size\":1000}";

            request.AddHeader("ContentType", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string responseString = response.Content;

            if (responseString == "\"Not found\"")
            {
                Console.WriteLine("Not found");
            }

            if (responseString == "")
            {
                Console.WriteLine("Request is not correct");
                token = AuthorizeClass.Authorize();
                goto link1;
            }

            List<GetBiDataResponseModel> GetBiDataList = new List<GetBiDataResponseModel>();

            return GetBiDataList;
        }
    }

    class GetBiDataBodyModel                                                // Body запиту
    {
        public string Label { get; set; }                                   // Назва збереженного списку (перелік можна в GET /api/1.0/BI/getbiimportlabels)
        public int Size { get; set; }                                       // Розмір списку (від 1 до 10000)
        public int? Pack { get; set; }                                      // Для повторного отримання записів по Pack id
    }

    class GetBiDataResponseModel
    {
        public bool isSuccess { get; set; }                                 // Успішно виконано?
        public string status { get; set; }                                  // success, error, (Дані успішно знайдено. Pack: " + part)
        public int code { get; set; }                                       // 404, 200, ...
        public List<ResponseModel> data { get; set; }                       // Список компаній
    }

    public class ResponseModel
    {
        public string NazvaPidpriyemstva { get; set; }                      // Повне найменування підприємства
        public string KodYedrpou { get; set; }                              // Код ЄДРПОУ
        public string DataReyestratsiyi { get; set; }                       // Дата реєстрації
        public string StatusYuridichnoyiOsobi { get; set; }                 // Статус юридичної особи
        public string VidomostiProBankrutstvo { get; set; }                 // Відомості про банкрутство
        public string VidomostiProPripinennya { get; set; }                 // Відомості про припинення або реорганізацію юридичної особи
        public string AdresaReyestratsiyi { get; set; }                     // Адреса реєстрації підприємства
        public string KvedOsnovniy { get; set; }                            // КВЕД (основний)
        public string StatutniyKapital { get; set; }                        // Статутний капітал
        public string UpovnovazheniOsobi { get; set; }                      // Уповноважені особи
        public string KilkistZasnovnikiv { get; set; }                      // Кількість засновників 
        public string InozomniBenefitsiari { get; set; }                    // Інозомні бенефіціари
        public string KilkistVlasnikivAktsiy { get; set; }                  // Кількість Власників пакетів акцій 
        public string VidkremleniPidrozdili { get; set; }                   // Відкремлені підрозділи
        public string ReyestrPlatnikivPdv { get; set; }                     // Реєстр платників ПДВ
        public string AnulovanoPdv { get; set; }                            // Анульована реєстрація платників ПДВ
        public string ReyestrPlatnikivYep { get; set; }                     // Реєстр платників єдинго податку(дописать в еластик)
        public string SanktsiyniSpiski { get; set; }                        // Санкційні списки
        public string VikonavchiVprovadzhennya { get; set; }                // Виконавчі впровадження
        public string SudoviRishennya { get; set; }                         // Судові рішення 
        public string SudoviZasidannya { get; set; }                        // Судові засідання (Спарави призначені до розгляду)
        public string PlangrafikPerevirok2020 { get; set; }                 // Включено в план-графік перевірок 2020
        public string PereviryayuchiyOrgan { get; set; }                    // Перевіряючий орган
        public string PodatkoviyBorg { get; set; }                          // Податковий борг
        public string PeredMistsevimByudzhetom { get; set; }                // Перед місцевим бюджетом
        public string PeredDerzhavnimByudzhetom { get; set; }               // Перед державним бюджетом
        public string KompaniyZaAdresoyu { get; set; }                      // За юридичним місцезнаходженням зареєстровано більше 1 суб’єкта господарювання
        public string ZvyazkyKerivnyka { get; set; }                        // Зв'язки (керівник) Аналіз компаній, які можливо пов’язані з керівником (ПІБ керівника співпадає з ПІБ керівника в інших компаніях)
        public string ZvyazkyBenefitsiariv { get; set; }                    // Зв'язки (засновники) Наявність можливих афілійованих зв’язків по засновникам та бенефіціарам
        public string ZvyazkiZYedrd { get; set; }                           // Зв'язки з ЄДРД (РЕР Декларанти та члени сім'ї)
        public string TipnazvaLitsenziydozvoliv { get; set; }               // Тип/назва ліцензій/дозволів 
        public string KilkistLitsenziydozvoliv { get; set; }                // Кількість ліцензій/Дозволів
        public string ZaborgovanistPoZp { get; set; }                       // Заборгованість по ЗП
        public string KilkistObyektivNerukhomosti { get; set; }             // Кількість об'єктів нерухомості 
        public string KilkistZemelnikhDilyanok { get; set; }                // Кількість земельних ділянок 
        public string PloshchaZemli { get; set; }                           // Площа землі
        public string KilkistTransportnikhZasobiv { get; set; }             // Кількість транспортних засобів
        public string Pratsivnikiv2018 { get; set; }                        // Штатна чисельність працівників - 2018
        public string SumaEksportny2018 { get; set; }                       // Сума експортних операцій - 2018
        public string SumaImporty2018 { get; set; }                         // Сума імпортних операцій - 2018
        public string KilkistTenderivPriymavUchast { get; set; }            // Кіькість тендерів в яких приймав участь
        public string KilkistVigranikhTenderiv { get; set; }                // Кількість виграних тендерів
        public string SumaVigranikhTenderiv { get; set; }                   // Сума виграних тендерів 
        public string KlasBorzhnikaNbu2018 { get; set; }                    // Клас боржника НБУ - 2018
        public string FinansoviyStan { get; set; }                          // Фінансовий стан 
        public string Zabovyazannya { get; set; }                           // Забов'язання 
        public string MizhnarodnaReytingovaOtsinka { get; set; }            // Міжнародна рейтингова оцінка 
        public string OsnovniZasobi2018 { get; set; }                       // Основні засоби 2018
        public string PotochniZabovyazannya2018 { get; set; }               // Поточні забов'язання 2018
        public string ChistiyDokhidViruchka2018 { get; set; }               // Чистий дохід (виручка) 2018
        public string ChistiyPributokzbitok2018 { get; set; }               // Чистий прибуток/збиток 2018
        public string KontaktnaInformatsiya { get; set; }                   // Контактна інформація
        public string Label { get; set; }                                   // Назва збереженного списку
    }
}
