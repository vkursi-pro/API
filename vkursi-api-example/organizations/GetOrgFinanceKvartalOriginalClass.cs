using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations
{
    public static class GetOrgFinanceKvartalOriginalClass
    {
        /*
        165. Отримання відповіді з даними по фінансовій звітності юридичної особи за конкретний рік, та конкретний період

        ЗВЕРНІТЬ БУДЬ-ЛАСКА УВАГУ!!!
        
        1. У фінансовій звітності до 2024 "Форма 1", "Форма 1м", "Форма 1мс", "Форма 2" - частково відсутні загальні дані про
        юридичну особу, які стосуються шапки звіту, наприклад: кервіник, квед, адреса, теритрія і т.д. Перебуває в процесі наповлення даних.
        (всі відомості щодо фінансових показників присутні!)
        
        2. Для більшого сприйняття структури полів фінансової звітності, додано pdf файли з описом TeamplateFiles\FinZvitStructExample 
        (літери які присутні в назві полів можуть відрізнятись у pdf файлі та моделях, однак коди полів завжди збігаються)
        
        [POST] api/1.0/organizations/GetOrgFinanceKvartalOriginal

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinanceKvartalOriginal' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"Code":"00131512","periodYear": 2024, "periodType": 3}'
        */

        public static GetOrgFinanceOriginalDataResponse GetOrgFinanceKvartalOriginal(ref string token, string code, int periodYear, int periodType)
        {

            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass(); token = _authorize.Authorize(); }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetOrgFinanceKvartalOriginal");
                RestRequest request = new RestRequest(Method.POST);

                GetOrgFinanceOriginalData GOFRequesRow = new GetOrgFinanceOriginalData()
                {
                    Code = code,                                             //00131512
                    periodYear = periodYear,                                 //2024
                    periodType = periodType                                  // 3 - перший квартал, 6 - півріччя, 9 - дев'ять місяців, 12 - річна
                };

                string body = JsonConvert.SerializeObject(GOFRequesRow);      // Example: {"Code":["00131512"]}

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

            GetOrgFinanceOriginalDataResponse GOFResponseRow = new();

            GOFResponseRow = JsonConvert.DeserializeObject<GetOrgFinanceOriginalDataResponse>(responseString, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error});

            return GOFResponseRow;
        }
    }

    /// <summary>
    /// Модель запиту
    /// </summary>
    public class GetOrgFinanceOriginalData
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Рік
        /// </summary>
        public int periodYear { get; set; }
        /// <summary>
        /// 3 - перший квартал, 6 - півріччя, 9 - дев'ять місяців, 12 - річна
        /// </summary>
        public int periodType { get; set; }
    }
    /// <summary>
    /// Модель відповіді 
    /// </summary>
    public class GetOrgFinanceOriginalDataResponse
    {
        public string Status { get; set; }
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Основний масив даних
        /// </summary>
        public List<OrgFinancePerYears> Data { get; set; }
    }

    public class OrgFinancePerYears
    {
        /// <summary>
        /// Рік
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Переод: 12 - за рік / 9 - за 3й квартал / 6 - за півроку / 3 - за 1й квартал
        /// </summary>
        public int PeriodType { get; set; }
        /// <summary>
        /// Назва звіту
        /// </summary>
        public string FormName { get; set; }
        /// <summary>
        /// Модель для фінансова звітність малого підприємства
        /// </summary>
        public FinResFinZvitMalogoPidprForma FinResFinZvitMalogoPidprForma { get; set; }
        /// <summary>
        /// Фiнансова звiтнiсть мiкропiдприємства
        /// </summary>
        public FinResFinZvitMikroPidpr FinResFinZvitMikroPidpr { get; set; }
        /// <summary>
        /// Звiт про фiнансовi результати (Звiт про сукупний дохiд)
        /// </summary>
        public FinResZvitFinRez FinResZvitFinRez { get; set; }
        /// <summary>
        /// Баланс (Звiт про фiнансовий стан)
        /// </summary>
        public FinResBalans FinResBalans { get; set; }
        /// <summary>
        /// Звіт про рух грошових коштів (за прямим методом)
        /// </summary>
        public FinZvitForm3 FinZvitForm3 { get; set; }
        /// <summary>
        /// Звіт про власний капітал
        /// </summary>
        public FinZvitForm4 FinZvitForm4 { get; set; }
        /// <summary>
        /// Привітки до річної фінансової звітності
        /// </summary>
        public UnitedFinZvitForm5Model FinZvitForm5 { get; set; }
        /// <summary>
        /// Додаток до приміток до річної фінансової звітності "Іформація за сегментами"
        /// </summary>
        public UnitedFinZvitForm6Model FinZvitForm6 { get; set; }
    }
    public class FinResBalans
    {
        //Зверніть увагу Вказані поля актуальні для звіту 2024+ 
        /// <summary>
        /// Заборгованість за внесками до статутного капіталу інших підприємств на початок звітного період
        /// </summary>
        public double? R1036G3 { get; set; }
        /// <summary>
        /// Заборгованість за внесками до статутного капіталу інших підприємств на кінець звітного період
        /// </summary>
        public double? R1036G4 { get; set; }


        /// <summary>
        /// Айді запису в реєстрі
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код ЄДРПОУ (у разі якщо код містить спереду "00" Накриклад: 00131305 нулі буде обрізано і в цьому полі буде значення 131305
        /// </summary>
        public int Tin { get; set; }
        /// <summary>
        /// Повне наіменування компанії
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Код форми звітності
        /// </summary>
        public string FormCode { get; set; }
        /// <summary>
        /// Назва форми звітності
        /// </summary>
        public string FormName { get; set; }
        /// <summary>
        /// за який період подано фінансову звітність
        /// </summary>
        public int PeriodMonth { get; set; }
        /// <summary>
        /// за який рік подано фінансову звітність
        /// </summary>
        public int PeriodYear { get; set; }
        /// <summary>
        /// гранична дата подання фінзвітності
        /// </summary>
        public string DGet { get; set; }
        /// <summary>
        /// Дата подачі документу
        /// </summary>
        public DateTime? DocDate { get; set; }
        /// <summary>
        /// Персонал
        /// </summary>
        public int? N3 { get; set; }
        /// <summary>
        /// Нематеріальні активи На початок період
        /// </summary>
        public double R1000G3 { get; set; }
        /// <summary>
        /// Нематеріальні активи На кінець період
        /// </summary>
        public double R1000G4 { get; set; }
        /// <summary>
        /// Нематеріальні активи первісна вартість На початок період
        /// </summary>
        public double R1001G3 { get; set; }
        /// <summary>
        /// Нематеріальні активи первісна вартість На кінець період
        /// </summary>
        public double R1001G4 { get; set; }
        /// <summary>
        /// Нематеріальні активи накопичена амортизація На початок період
        /// </summary>
        public double R1002G3 { get; set; }
        /// <summary>
        /// Нематеріальні активи накопичена амортизація На кінець період
        /// </summary>
        public double R1002G4 { get; set; }
        /// <summary>
        /// Незавершені капітальні інвестиції На початок період
        /// </summary>
        public double R1005G3 { get; set; }
        /// <summary>
        /// Незавершені капітальні інвестиції На кінець період
        /// </summary>
        public double R1005G4 { get; set; }
        /// <summary>
        /// Основні засоби На початок період
        /// </summary>
        public double R1010G3 { get; set; }
        /// <summary>
        /// Основні засоби На кінець період
        /// </summary>
        public double R1010G4 { get; set; }
        /// <summary>
        /// Основні засоби первісна вартість На початок період
        /// </summary>
        public double R1011G3 { get; set; }
        /// <summary>
        /// Основні засоби первісна вартість На кінець період
        /// </summary>
        public double R1011G4 { get; set; }
        /// <summary>
        /// Основні засоби знос На початок період
        /// </summary>
        public double R1012G3 { get; set; }
        /// <summary>
        /// Основні засоби знос На кінець період
        /// </summary>
        public double R1012G4 { get; set; }
        /// <summary>
        /// Інвестиційна нерухомість На початок період
        /// </summary>
        public double R1015G3 { get; set; }
        /// <summary>
        /// Інвестиційна нерухомість На кінець період
        /// </summary>
        public double R1015G4 { get; set; }
        /// <summary>
        /// Первісна вартість інвестиційної нерухомості На початок період
        /// </summary>
        public double R1016G3 { get; set; }
        /// <summary>
        /// Первісна вартість інвестиційної нерухомості На кінець період
        /// </summary>
        public double R1016G4 { get; set; }
        /// <summary>
        /// Знос інвестиційної нерухомості На початок період
        /// </summary>
        public double R1017G3 { get; set; }
        /// <summary>
        /// Знос інвестиційної нерухомості На кінець період
        /// </summary>
        public double R1017G4 { get; set; }
        /// <summary>
        /// Довгострокові біологічні активи На початок період
        /// </summary>
        public double R1020G3 { get; set; }
        /// <summary>
        /// Довгострокові біологічні активи На кінець період
        /// </summary>
        public double R1020G4 { get; set; }
        /// <summary>
        /// Первісна вартість довгострокових біологічних активів На початок період
        /// </summary>
        public double R1021G3 { get; set; }
        /// <summary>
        /// Первісна вартість довгострокових біологічних активів На кінець період
        /// </summary>
        public double R1021G4 { get; set; }
        /// <summary>
        /// Накопичена амортизація довгострокових біологічних активів На початок період
        /// </summary>
        public double R1022G3 { get; set; }
        /// <summary>
        /// Накопичена амортизація довгострокових біологічних активів На кінець період
        /// </summary>
        public double R1022G4 { get; set; }
        /// <summary>
        /// Довгострокові фінансові інвестиції На початок період
        /// </summary>
        public double R1030G3 { get; set; }
        /// <summary>
        /// Довгострокові фінансові інвестиції На кінець період
        /// </summary>
        public double R1030G4 { get; set; }
        /// <summary>
        /// Інші фінансові інвестиції На початок період
        /// </summary>
        public double R1035G3 { get; set; }
        /// <summary>
        /// Інші фінансові інвестиції На кінець період
        /// </summary>
        public double R1035G4 { get; set; }
        /// <summary>
        /// Довгострокова дебіторська заборгованість На початок період
        /// </summary>
        public double R1040G3 { get; set; }
        /// <summary>
        /// Довгострокова дебіторська заборгованість На кінець період
        /// </summary>
        public double R1040G4 { get; set; }
        /// <summary>
        /// Відстрочені податкові активи На початок період
        /// </summary>
        public double R1045G3 { get; set; }
        /// <summary>
        /// Відстрочені податкові активи На кінець період
        /// </summary>
        public double R1045G4 { get; set; }
        /// <summary>
        /// Гудвіл На початок період
        /// </summary>
        public double R1050G3 { get; set; }
        /// <summary>
        /// Гудвіл На кінець період
        /// </summary>
        public double R1050G4 { get; set; }
        /// <summary>
        /// Відстрочені аквізиційні витрати На початок період
        /// </summary>
        public double R1060G3 { get; set; }
        /// <summary>
        /// Відстрочені аквізиційні витрати На кінець період
        /// </summary>
        public double R1060G4 { get; set; }
        /// <summary>
        /// Залишок коштів у централізованих страхових резервних фондах На початок період
        /// </summary>
        public double R1065G3 { get; set; }
        /// <summary>
        /// Залишок коштів у централізованих страхових резервних фондах На кінець період
        /// </summary>
        public double R1065G4 { get; set; }
        /// <summary>
        /// Інші необоротні активи На початок період
        /// </summary>
        public double R1090G3 { get; set; }
        /// <summary>
        /// Інші необоротні активи На кінець період
        /// </summary>
        public double R1090G4 { get; set; }
        /// <summary>
        /// Усього за розділом I На початок період
        /// </summary>
        public double R1095G3 { get; set; }
        /// <summary>
        /// Усього за розділом I На кінець період
        /// </summary>
        public double R1095G4 { get; set; }
        /// <summary>
        /// Запаси На початок період
        /// </summary>
        public double R1100G3 { get; set; }
        /// <summary>
        /// Запаси На кінець період
        /// </summary>
        public double R1100G4 { get; set; }
        /// <summary>
        /// Виробничі запаси На початок період
        /// </summary>
        public double R1101G3 { get; set; }
        /// <summary>
        /// Виробничі запаси На кінець період
        /// </summary>
        public double R1101G4 { get; set; }
        /// <summary>
        /// Незавершене виробництво На початок період
        /// </summary>
        public double R1102G3 { get; set; }
        /// <summary>
        /// Незавершене виробництво На кінець період
        /// </summary>
        public double R1102G4 { get; set; }
        /// <summary>
        /// Готова продукція На початок період
        /// </summary>
        public double R1103G3 { get; set; }
        /// <summary>
        /// Готова продукція На кінець період
        /// </summary>
        public double R1103G4 { get; set; }
        /// <summary>
        /// Товари На початок період
        /// </summary>
        public double R1104G3 { get; set; }
        /// <summary>
        /// Товари На кінець період
        /// </summary>
        public double R1104G4 { get; set; }
        /// <summary>
        /// Поточні біологічні активи На початок період
        /// </summary>
        public double R1110G3 { get; set; }
        /// <summary>
        /// Поточні біологічні активи На кінець період
        /// </summary>
        public double R1110G4 { get; set; }
        /// <summary>
        /// Депозити перестрахування На початок період
        /// </summary>
        public double R1115G3 { get; set; }
        /// <summary>
        /// Депозити перестрахування На кінець період
        /// </summary>
        public double R1115G4 { get; set; }
        /// <summary>
        /// Векселі одержані На початок період
        /// </summary>
        public double R1120G3 { get; set; }
        /// <summary>
        /// Векселі одержані На кінець період
        /// </summary>
        public double R1120G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за продукцію, товари, роботи, послуги На початок період
        /// </summary>
        public double R1125G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за продукцію, товари, роботи, послуги На кінець період
        /// </summary>
        public double R1125G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за виданими авансами На початок період
        /// </summary>
        public double R1130G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за виданими авансами На кінець період
        /// </summary>
        public double R1130G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за розрахунками з бюджетом На початок період
        /// </summary>
        public double R1135G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за розрахунками з бюджетом На кінець період
        /// </summary>
        public double R1135G4 { get; set; }
        /// <summary>
        /// у тому числі з податку на прибуток На початок період
        /// </summary>
        public double R1136G3 { get; set; }
        /// <summary>
        /// у тому числі з податку на прибуток На кінець період
        /// </summary>
        public double R1136G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість з нарахованих доходів На початок період
        /// </summary>
        public double R1140G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість з нарахованих доходів На кінець період
        /// </summary>
        public double R1140G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість із внутрішніх розрахунків На початок період
        /// </summary>
        public double R1145G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість із внутрішніх розрахунків На кінець період
        /// </summary>
        public double R1145G4 { get; set; }
        /// <summary>
        /// Інша поточна дебіторська заборгованість На початок період
        /// </summary>
        public double R1155G3 { get; set; }
        /// <summary>
        /// Інша поточна дебіторська заборгованість На кінець період
        /// </summary>
        public double R1155G4 { get; set; }
        /// <summary>
        /// Поточні фінансові інвестиції На початок період
        /// </summary>
        public double R1160G3 { get; set; }
        /// <summary>
        /// Поточні фінансові інвестиції На кінець період
        /// </summary>
        public double R1160G4 { get; set; }
        /// <summary>
        /// Гроші та їх еквіваленти На початок період
        /// </summary>
        public double R1165G3 { get; set; }
        /// <summary>
        /// Гроші та їх еквіваленти На кінець період
        /// </summary>
        public double R1165G4 { get; set; }
        /// <summary>
        /// готівка На початок період
        /// </summary>
        public double R1166G3 { get; set; }
        /// <summary>
        /// готівка На кінець період
        /// </summary>
        public double R1166G4 { get; set; }
        /// <summary>
        /// рахунки в банках На початок період
        /// </summary>
        public double R1167G3 { get; set; }
        /// <summary>
        /// рахунки в банках На кінець період
        /// </summary>
        public double R1167G4 { get; set; }
        /// <summary>
        /// Витрати майбутніх періодів На початок період
        /// </summary>
        public double R1170G3 { get; set; }
        /// <summary>
        /// Витрати майбутніх періодів На кінець період
        /// </summary>
        public double R1170G4 { get; set; }
        /// <summary>
        /// Частка перестраховика у страхових резервах На початок період
        /// </summary>
        public double R1180G3 { get; set; }
        /// <summary>
        /// Частка перестраховика у страхових резервах На кінець період
        /// </summary>
        public double R1180G4 { get; set; }
        /// <summary>
        /// у тому числі в: резервах довгострокових зобов’язань На початок період
        /// </summary>
        public double R1181G3 { get; set; }
        /// <summary>
        /// у тому числі в: резервах довгострокових зобов’язань На кінець період
        /// </summary>
        public double R1181G4 { get; set; }
        /// <summary>
        /// резервах збитків або резервах належних виплат На початок період
        /// </summary>
        public double R1182G3 { get; set; }
        /// <summary>
        /// резервах збитків або резервах належних виплат На кінець період
        /// </summary>
        public double R1182G4 { get; set; }
        /// <summary>
        /// резервах незароблених премій На початок період
        /// </summary>
        public double R1183G3 { get; set; }
        /// <summary>
        /// резервах незароблених премій На кінець період
        /// </summary>
        public double R1183G4 { get; set; }
        /// <summary>
        /// інших страхових резервах На початок період
        /// </summary>
        public double R1184G3 { get; set; }
        /// <summary>
        /// інших страхових резервах На кінець період
        /// </summary>
        public double R1184G4 { get; set; }
        /// <summary>
        /// Інші оборотні активи На початок період
        /// </summary>
        public double R1190G3 { get; set; }
        /// <summary>
        /// Інші оборотні активи На кінець період
        /// </summary>
        public double R1190G4 { get; set; }
        /// <summary>
        /// Усього за розділом II На початок період
        /// </summary>
        public double R1195G3 { get; set; }
        /// <summary>
        /// Усього за розділом II На кінець період
        /// </summary>
        public double R1195G4 { get; set; }
        /// <summary>
        /// III. Необоротні активи, утримувані для продажу, та групи вибуття На початок період
        /// </summary>
        public double R1200G3 { get; set; }
        /// <summary>
        /// III. Необоротні активи, утримувані для продажу, та групи вибуття На кінець період
        /// </summary>
        public double R1200G4 { get; set; }
        /// <summary>
        /// Баланс актіву На початок період
        /// </summary>
        public double R1300G3 { get; set; }
        /// <summary>
        /// Баланс актіву На кінець період
        /// </summary>
        public double R1300G4 { get; set; }
        /// <summary>
        /// Зареєстрований (пайовий) капітал На початок період
        /// </summary>
        public double R1400G3 { get; set; }
        /// <summary>
        /// Зареєстрований (пайовий) капітал На кінець період
        /// </summary>
        public double R1400G4 { get; set; }
        /// <summary>
        /// Внески до незареєстрованого статутного капіталу На початок період
        /// </summary>
        public double R1401G3 { get; set; }
        /// <summary>
        /// Внески до незареєстрованого статутного капіталу На кінець період
        /// </summary>
        public double R1401G4 { get; set; }
        /// <summary>
        /// Капітал у дооцінках На початок період
        /// </summary>
        public double R1405G3 { get; set; }
        /// <summary>
        /// Капітал у дооцінках На кінець період
        /// </summary>
        public double R1405G4 { get; set; }
        /// <summary>
        /// Додатковий капітал На початок період
        /// </summary>
        public double R1410G3 { get; set; }
        /// <summary>
        /// Додатковий капітал На кінець період
        /// </summary>
        public double R1410G4 { get; set; }
        /// <summary>
        /// емісійний дохід На початок період
        /// </summary>
        public double R1411G3 { get; set; }
        /// <summary>
        /// емісійний дохід На кінець період
        /// </summary>
        public double R1411G4 { get; set; }
        /// <summary>
        /// накопичені курсові різниці На початок період
        /// </summary>
        public double R1412G3 { get; set; }
        /// <summary>
        /// накопичені курсові різниці На кінець період
        /// </summary>
        public double R1412G4 { get; set; }
        /// <summary>
        /// Резервний капітал На початок період
        /// </summary>
        public double R1415G3 { get; set; }
        /// <summary>
        /// Резервний капітал На кінець період
        /// </summary>
        public double R1415G4 { get; set; }
        /// <summary>
        /// Нерозподілений прибуток (непокритий збиток) На початок період
        /// </summary>
        public double R1420G3 { get; set; }
        /// <summary>
        /// Нерозподілений прибуток (непокритий збиток) На кінець період
        /// </summary>
        public double R1420G4 { get; set; }
        /// <summary>
        /// Неоплачений капітал На початок період
        /// </summary>
        public double R1425G3 { get; set; }
        /// <summary>
        /// Неоплачений капітал На кінець період
        /// </summary>
        public double R1425G4 { get; set; }
        /// <summary>
        /// Вилучений капітал На початок період
        /// </summary>
        public double R1430G3 { get; set; }
        /// <summary>
        /// Вилучений капітал На кінець період
        /// </summary>
        public double R1430G4 { get; set; }
        /// <summary>
        /// Інші резерви На початок період
        /// </summary>
        public double R1435G3 { get; set; }
        /// <summary>
        /// Інші резерви На кінець період
        /// </summary>
        public double R1435G4 { get; set; }
        /// <summary>
        /// Усього за розділом I На початок період
        /// </summary>
        public double R1495G3 { get; set; }
        /// <summary>
        /// Усього за розділом I На кінець період
        /// </summary>
        public double R1495G4 { get; set; }
        /// <summary>
        /// Відстрочені податкові зобов’язання На початок період
        /// </summary>
        public double R1500G3 { get; set; }
        /// <summary>
        /// Відстрочені податкові зобов’язання На кінець період
        /// </summary>
        public double R1500G4 { get; set; }
        /// <summary>
        /// Пенсійні зобов’язання На початок період
        /// </summary>
        public double R1505G3 { get; set; }
        /// <summary>
        /// Пенсійні зобов’язання На кінець період
        /// </summary>
        public double R1505G4 { get; set; }
        /// <summary>
        /// Довгострокові кредити банків На початок період
        /// </summary>
        public double R1510G3 { get; set; }
        /// <summary>
        /// Довгострокові кредити банків На кінець період
        /// </summary>
        public double R1510G4 { get; set; }
        /// <summary>
        /// Інші довгострокові зобов’язання На початок період
        /// </summary>
        public double R1515G3 { get; set; }
        /// <summary>
        /// Інші довгострокові зобов’язання На кінець період
        /// </summary>
        public double R1515G4 { get; set; }
        /// <summary>
        /// Довгострокові забезпечення На початок період
        /// </summary>
        public double R1520G3 { get; set; }
        /// <summary>
        /// Довгострокові забезпечення На кінець період
        /// </summary>
        public double R1520G4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double R1521G3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double R1521G4 { get; set; }
        /// <summary>
        /// Цільове фінансування На початок період
        /// </summary>
        public double R1525G3 { get; set; }
        /// <summary>
        /// Цільове фінансування На кінець період
        /// </summary>
        public double R1525G4 { get; set; }
        /// <summary>
        /// благодійна допомога На початок період
        /// </summary>
        public double R1526G3 { get; set; }
        /// <summary>
        /// благодійна допомога На кінець період
        /// </summary>
        public double R1526G4 { get; set; }
        /// <summary>
        /// Страхові резерви На початок період
        /// </summary>
        public double R1530G3 { get; set; }
        /// <summary>
        /// Страхові резерви На кінець період
        /// </summary>
        public double R1530G4 { get; set; }
        /// <summary>
        /// у тому числі резерв довгострокових зобов’язань На початок період
        /// </summary>
        public double R1531G3 { get; set; }
        /// <summary>
        /// у тому числі резерв довгострокових зобов’язань На кінець період
        /// </summary>
        public double R1531G4 { get; set; }
        /// <summary>
        /// резерв збитків або резерв належних виплат На початок період
        /// </summary>
        public double R1532G3 { get; set; }
        /// <summary>
        /// резерв збитків або резерв належних виплат На кінець період
        /// </summary>
        public double R1532G4 { get; set; }
        /// <summary>
        /// резерв незароблених премій На початок період
        /// </summary>
        public double R1533G3 { get; set; }
        /// <summary>
        /// резерв незароблених премій На кінець період
        /// </summary>
        public double R1533G4 { get; set; }
        /// <summary>
        /// інші страхові резерви На початок період
        /// </summary>
        public double R1534G3 { get; set; }
        /// <summary>
        /// інші страхові резерви На кінець період
        /// </summary>
        public double R1534G4 { get; set; }
        /// <summary>
        /// Інвестиційні контракти На початок період
        /// </summary>
        public double R1535G3 { get; set; }
        /// <summary>
        /// Інвестиційні контракти На кінець період
        /// </summary>
        public double R1535G4 { get; set; }
        /// <summary>
        /// Призовий фонд На початок період
        /// </summary>
        public double R1540G3 { get; set; }
        /// <summary>
        /// Призовий фонд На кінець період
        /// </summary>
        public double R1540G4 { get; set; }
        /// <summary>
        /// Резерв на виплату джек-поту На початок період
        /// </summary>
        public double R1545G3 { get; set; }
        /// <summary>
        /// Резерв на виплату джек-поту На кінець період
        /// </summary>
        public double R1545G4 { get; set; }
        /// <summary>
        /// Усього за розділом II На початок період
        /// </summary>
        public double R1595G3 { get; set; }
        /// <summary>
        /// Усього за розділом II На кінець період
        /// </summary>
        public double R1595G4 { get; set; }
        /// <summary>
        /// Короткострокові кредити банків На початок період
        /// </summary>
        public double R1600G3 { get; set; }
        /// <summary>
        /// Короткострокові кредити банків На кінець період
        /// </summary>
        public double R1600G4 { get; set; }
        /// <summary>
        /// Векселі видані На початок період
        /// </summary>
        public double R1605G3 { get; set; }
        /// <summary>
        /// Векселі видані На кінець період
        /// </summary>
        public double R1605G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за довгостроковими зобов’язаннями На початок період
        /// </summary>
        public double R1610G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за довгостроковими зобов’язаннями На кінець період
        /// </summary>
        public double R1610G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за товари, роботи, послуги На початок період
        /// </summary>
        public double R1615G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за товари, роботи, послуги На кінець період
        /// </summary>
        public double R1615G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з бюджетом На початок період
        /// </summary>
        public double R1620G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з бюджетом На кінець період
        /// </summary>
        public double R1620G4 { get; set; }
        /// <summary>
        /// у тому числі з податку на прибуток На початок період
        /// </summary>
        public double R1621G3 { get; set; }
        /// <summary>
        /// у тому числі з податку на прибуток На кінець період
        /// </summary>
        public double R1621G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками зі страхування На початок період
        /// </summary>
        public double R1625G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками зі страхування На кінець період
        /// </summary>
        public double R1625G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з оплати праці На початок період
        /// </summary>
        public double R1630G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з оплати праці На кінець період
        /// </summary>
        public double R1630G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за одержаними авансами На початок період
        /// </summary>
        public double R1635G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за одержаними авансами На кінець період
        /// </summary>
        public double R1635G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з учасниками На початок період
        /// </summary>
        public double R1640G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з учасниками На кінець період
        /// </summary>
        public double R1640G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість із внутрішніх розрахунків На початок період
        /// </summary>
        public double R1645G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість із внутрішніх розрахунків На кінець період
        /// </summary>
        public double R1645G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за страховою діяльністю На початок період
        /// </summary>
        public double R1650G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за страховою діяльністю На кінець період
        /// </summary>
        public double R1650G4 { get; set; }
        /// <summary>
        /// Поточні забезпечення На початок період
        /// </summary>
        public double R1660G3 { get; set; }
        /// <summary>
        /// Поточні забезпечення На кінець період
        /// </summary>
        public double R1660G4 { get; set; }
        /// <summary>
        /// Доходи майбутніх періодів На початок період
        /// </summary>
        public double R1665G3 { get; set; }
        /// <summary>
        /// Доходи майбутніх періодів На кінець період
        /// </summary>
        public double R1665G4 { get; set; }
        /// <summary>
        /// Відстрочені комісійні доходи від перестраховиків На початок період
        /// </summary>
        public double R1670G3 { get; set; }
        /// <summary>
        /// Відстрочені комісійні доходи від перестраховиків На кінець період
        /// </summary>
        public double R1670G4 { get; set; }
        /// <summary>
        /// Інші поточні зобов’язання На початок період
        /// </summary>
        public double R1690G3 { get; set; }
        /// <summary>
        /// Інші поточні зобов’язання На кінець період
        /// </summary>
        public double R1690G4 { get; set; }
        /// <summary>
        /// Усього за розділом IІІ На початок період
        /// </summary>
        public double R1695G3 { get; set; }
        /// <summary>
        /// Усього за розділом IІІ На кінець період
        /// </summary>
        public double R1695G4 { get; set; }
        /// <summary>
        /// ІV. Зобов’язання, пов’язані з необоротними активами, утримуваними для продажу, та групами вибуття На початок період
        /// </summary>
        public double R1700G3 { get; set; }
        /// <summary>
        /// ІV. Зобов’язання, пов’язані з необоротними активами, утримуваними для продажу, та групами вибуття На кінець період
        /// </summary>
        public double R1700G4 { get; set; }
        /// <summary>
        /// V. Чиста вартість активів НПФ На початок період
        /// </summary>
        public double R1800G3 { get; set; }
        /// <summary>
        /// V. Чиста вартість активів НПФ На кінець період
        /// </summary>
        public double R1800G4 { get; set; }
        /// <summary>
        /// Баланс пасіву На початок період
        /// </summary>
        public double R1900G3 { get; set; }
        /// <summary>
        /// Баланс пасіву На кінець період
        /// </summary>
        public double R1900G4 { get; set; }
        /// <summary>
        /// ФІнансова звітність станом на
        /// </summary>
        public string? MyDate { get; set; }
        /// <summary>
        /// Контактний номер телефону юр. особи
        /// </summary>
        public string? FirmTelorg { get; set; }
        /// <summary>
        /// Адреса юр. особи
        /// </summary>
        public string? FirmAdr { get; set; }
        /// <summary>
        /// Граничнне число
        /// </summary>
        public string? N1 { get; set; }
        /// <summary>
        /// Граничний місяць
        /// </summary>
        public string? N2 { get; set; }
        /// <summary>
        /// Звіт зроблено за "Національними положеннями (стандартами) бухглатерського обліку"
        /// </summary>
        public string? N4 { get; set; }
        /// <summary>
        /// Звіт зроблено за "Міжнародними стандартами фінансової звітності"
        /// </summary>
        public string? N5 { get; set; }
        /// <summary>
        /// Територія
        /// </summary>
        public string? FirmTerr { get; set; }
        /// <summary>
        /// Назва КВЕД компанії
        /// </summary>
        public string? FirmKvednm { get; set; }
        /// <summary>
        /// Код КВЕД компанії
        /// </summary>
        public string? FirmKved { get; set; }
        /// <summary>
        /// ПІБ директора компанії
        /// </summary>
        public string? FirmRuk { get; set; }
        /// <summary>
        /// ПІБ головного бухгалтера компанії
        /// </summary>
        public string? FirmBuh { get; set; }
        /// <summary>
        /// Код організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfcd { get; set; }
        /// <summary>
        /// Назва організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfnm { get; set; }
        /// <summary>
        /// Останній день звітного період
        /// </summary>
        public string? LastDay { get; set; }
        /// <summary>
        /// Код області
        /// </summary>
        public string? Obl { get; set; }
        /// <summary>
        /// Код району
        /// </summary>
        public string? Ray { get; set; }
        /// <summary>
        /// Код КАТОТТГ
        /// </summary>

        public string? KATOTTG { get; set; }
        /// <summary>
        /// Назва програмного забезпечення через який подано звіт
        /// </summary>
        public string? Software { get; set; }
        /// <summary>
        ///Тип період
        /// </summary>
        public string? PeriodType { get; set; }
        /// <summary>
        /// Код типу податкової звітності.
        /// </summary>
        public string? CDocType { get; set; }
        /// <summary>
        /// Кількість податкових звітів у файлі.
        /// </summary>
        public string? CDocCnt { get; set; }
        /// <summary>
        /// Код регіону.
        /// </summary>
        public string? CReg { get; set; }
        /// <summary>
        /// Код району.
        /// </summary>
        public string? CRaj { get; set; }

        /// <summary>
        /// Назва файлу
        /// </summary>
        public string? FileName { get; set; }



    }
    public class FinResFinZvitMalogoPidprForma
    {
        /// <summary>
        /// Ідентифікатор фінансвової звітності
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код ЄДРПОУ (у разі якщо код містить спереду "00" Накриклад: 00131305 нулі буде обрізано і в цьому полі буде значення 131305
        /// </summary>
        public int Tin { get; set; }
        /// <summary>
        /// Повне наіменування юр. особи
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// тип форми звітності
        /// </summary>
        public string FormCode { get; set; }
        /// <summary>
        /// назва форми звітності
        /// </summary>
        public string FormName { get; set; }
        /// <summary>
        /// Період місяців за яку сформовано звіт
        /// </summary>
        public int PeriodMonth { get; set; }
        /// <summary>
        /// Період року за який сформовано звіт
        /// </summary>
        public int PeriodYear { get; set; }
        /// <summary>
        /// Гранична дата подання звітності
        /// </summary>
        public string DGet { get; set; }
        /// <summary>
        /// Нематеріальні активи На початок період
        /// </summary>
        public double R1000G3 { get; set; }
        /// <summary>
        /// Нематеріальні активи На кінець період
        /// </summary>
        public double R1000G4 { get; set; }
        /// <summary>
        /// Нематеріальні активи первісна вартість На початок період
        /// </summary>
        public double R1001G3 { get; set; }
        /// <summary>
        /// Нематеріальні активи первісна вартість На кінець період
        /// </summary>
        public double R1001G4 { get; set; }
        /// <summary>
        /// Нематеріальні активи накопичена амортизація На початок період
        /// </summary>
        public double R1002G3 { get; set; }
        /// <summary>
        /// Нематеріальні активи накопичена амортизація На кінець період
        /// </summary>
        public double R1002G4 { get; set; }
        /// <summary>
        /// Незавершені капітальні інвестиції На початок період
        /// </summary>
        public double R1005G3 { get; set; }
        /// <summary>
        /// Незавершені капітальні інвестиції На кінець період
        /// </summary>
        public double R1005G4 { get; set; }
        /// <summary>
        /// Основні засоби На початок період
        /// </summary>
        public double R1010G3 { get; set; }
        /// <summary>
        /// Основні засоби На кінець період
        /// </summary>
        public double R1010G4 { get; set; }
        /// <summary>
        /// Основні засоби первісна вартість На початок період
        /// </summary>
        public double R1011G3 { get; set; }
        /// <summary>
        /// Основні засоби первісна вартість На кінець період
        /// </summary>
        public double R1011G4 { get; set; }
        /// <summary>
        /// Основні засоби знос На початок період
        /// </summary>
        public double R1012G3 { get; set; }
        /// <summary>
        /// Основні засоби знос На кінець період
        /// </summary>
        public double R1012G4 { get; set; }
        /// <summary>
        /// Довгострокові біологічні активи На початок період
        /// </summary>
        public double R1020G3 { get; set; }
        /// <summary>
        /// Довгострокові біологічні активи На кінець період
        /// </summary>
        public double R1020G4 { get; set; }
        /// <summary>
        /// Довгострокові фінансові інвестиції На початок період
        /// </summary>
        public double R1030G3 { get; set; }
        /// <summary>
        /// Довгострокові фінансові інвестиції На кінець період
        /// </summary>
        public double R1030G4 { get; set; }
        /// <summary>
        /// Інші необоротні активи На початок період
        /// </summary>
        public double R1090G3 { get; set; }
        /// <summary>
        /// Інші необоротні активи На кінець період
        /// </summary>
        public double R1090G4 { get; set; }
        /// <summary>
        /// Усього за розділом І На початок період
        /// </summary>
        public double R1095G3 { get; set; }
        /// <summary>
        /// Усього за розділом І На кінець період
        /// </summary>
        public double R1095G4 { get; set; }
        /// <summary>
        /// Запаси На початок період
        /// </summary>
        public double R1100G3 { get; set; }
        /// <summary>
        /// Запаси На кінець період
        /// </summary>
        public double R1100G4 { get; set; }
        /// <summary>
        /// Запаси готова продукція На початок період
        /// </summary>
        public double R1103G3 { get; set; }
        /// <summary>
        /// Запаси готова продукція На кінець період
        /// </summary>
        public double R1103G4 { get; set; }
        /// <summary>
        /// Поточні біологічні активи На початок період
        /// </summary>
        public double R1110G3 { get; set; }
        /// <summary>
        /// Поточні біологічні активи На кінець період
        /// </summary>
        public double R1110G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за товари, роботи, послуги На початок період
        /// </summary>
        public double R1125G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за товари, роботи, послуги На кінець період
        /// </summary>
        public double R1125G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за розрахунками з бюджетом На початок період
        /// </summary>
        public double R1135G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за розрахунками з бюджетом На кінець період
        /// </summary>
        public double R1135G4 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за розрахунками з бюджетом у тому числі з податку на прибуток На початок період
        /// </summary>
        public double R1136G3 { get; set; }
        /// <summary>
        /// Дебіторська заборгованість за розрахунками з бюджетом у тому числі з податку на прибуток На кінець період
        /// </summary>
        public double R1136G4 { get; set; }
        /// <summary>
        /// Інша поточна дебіторська заборгованість На початок період
        /// </summary>
        public double R1155G3 { get; set; }
        /// <summary>
        /// Інша поточна дебіторська заборгованість На кінець період
        /// </summary>
        public double R1155G4 { get; set; }
        /// <summary>
        /// Поточні фінансові інвестиції На початок період
        /// </summary>
        public double R1160G3 { get; set; }
        /// <summary>
        /// Поточні фінансові інвестиції На кінець період
        /// </summary>
        public double R1160G4 { get; set; }
        /// <summary>
        /// Гроші та їх еквіваленти На початок період
        /// </summary>
        public double R1165G3 { get; set; }
        /// <summary>
        /// Гроші та їх еквіваленти На кінець період
        /// </summary>
        public double R1165G4 { get; set; }
        /// <summary>
        /// Витрати майбутніх періодів На початок період
        /// </summary>
        public double R1170G3 { get; set; }
        /// <summary>
        /// Витрати майбутніх періодів На кінець період
        /// </summary>
        public double R1170G4 { get; set; }
        /// <summary>
        /// Інші оборотні активи На початок період
        /// </summary>
        public double R1190G3 { get; set; }
        /// <summary>
        /// Інші оборотні активи На кінець період
        /// </summary>
        public double R1190G4 { get; set; }
        /// <summary>
        /// Усього за розділом II На початок період
        /// </summary>
        public double R1195G3 { get; set; }
        /// <summary>
        /// Усього за розділом II На кінець період
        /// </summary>
        public double R1195G4 { get; set; }
        /// <summary>
        /// ІІІ. Необоротні активи, утримувані для продажу, та групи вибуття На початок період
        /// </summary>
        public double R1200G3 { get; set; }
        /// <summary>
        /// ІІІ. Необоротні активи, утримувані для продажу, та групи вибуття На кінець період
        /// </summary>
        public double R1200G4 { get; set; }
        /// <summary>
        /// Баланс актіву На початок період
        /// </summary>
        public double R1300G3 { get; set; }
        /// <summary>
        /// Баланс актіву На кінець період
        /// </summary>
        public double R1300G4 { get; set; }
        /// <summary>
        /// Зареєстрований (пайовий) капітал На початок період
        /// </summary>
        public double R1400G3 { get; set; }
        /// <summary>
        /// Зареєстрований (пайовий) капітал На кінець період
        /// </summary>
        public double R1400G4 { get; set; }
        /// <summary>
        /// Додатковий капітал На початок період
        /// </summary>
        public double R1410G3 { get; set; }
        /// <summary>
        /// Додатковий капітал На кінець період
        /// </summary>
        public double R1410G4 { get; set; }
        /// <summary>
        /// Резервний капітал На початок період
        /// </summary>
        public double R1415G3 { get; set; }
        /// <summary>
        /// Резервний капітал На кінець період
        /// </summary>
        public double R1415G4 { get; set; }
        /// <summary>
        /// Нерозподілений прибуток (непокритий збиток) На початок період
        /// </summary>
        public double R1420G3 { get; set; }
        /// <summary>
        /// Нерозподілений прибуток (непокритий збиток) На кінець період
        /// </summary>
        public double R1420G4 { get; set; }
        /// <summary>
        /// Неоплачений капітал На початок період
        /// </summary>
        public double R1425G3 { get; set; }
        /// <summary>
        /// Неоплачений капітал На кінець період
        /// </summary>
        public double R1425G4 { get; set; }
        /// <summary>
        /// Усього за розділом I На початок період
        /// </summary>
        public double R1495G3 { get; set; }
        /// <summary>
        /// Усього за розділом I На кінець період
        /// </summary>
        public double R1495G4 { get; set; }
        /// <summary>
        /// II. Довгострокові зобов’язання, цільове фінансування та забезпечення На початок період
        /// </summary>
        public double R1595G3 { get; set; }
        /// <summary>
        /// II. Довгострокові зобов’язання, цільове фінансування та забезпечення На кінець період
        /// </summary>
        public double R1595G4 { get; set; }
        /// <summary>
        /// Короткострокові кредити банків На початок період
        /// </summary>
        public double R1600G3 { get; set; }
        /// <summary>
        /// Короткострокові кредити банків На кінець період
        /// </summary>
        public double R1600G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за довгостроковими зобов’язаннями На початок період
        /// </summary>
        public double R1610G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за довгостроковими зобов’язаннями На кінець період
        /// </summary>
        public double R1610G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за товари, роботи, послуги На початок період
        /// </summary>
        public double R1615G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за товари, роботи, послуги На кінець період
        /// </summary>
        public double R1615G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з бюджетом На початок період
        /// </summary>
        public double R1620G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з бюджетом На кінець період
        /// </summary>
        public double R1620G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за: у тому числі з податку на прибуток На початок період
        /// </summary>
        public double R1621G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за: у тому числі з податку на прибуток На кінець період
        /// </summary>
        public double R1621G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками зі страхування На початок період
        /// </summary>
        public double R1625G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками зі страхування На кінець період
        /// </summary>
        public double R1625G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з оплати праці На початок період
        /// </summary>
        public double R1630G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з оплати праці На кінець період
        /// </summary>
        public double R1630G4 { get; set; }
        /// <summary>
        /// Доходи майбутніх періодів На початок період
        /// </summary>
        public double R1665G3 { get; set; }
        /// <summary>
        /// Доходи майбутніх періодів На кінець період
        /// </summary>
        public double R1665G4 { get; set; }
        /// <summary>
        /// Інші поточні зобов’язання На початок період
        /// </summary>
        public double R1690G3 { get; set; }
        /// <summary>
        /// Інші поточні зобов’язання На кінець період
        /// </summary>
        public double R1690G4 { get; set; }
        /// <summary>
        /// Усього за розділом IІІ На початок період
        /// </summary>
        public double R1695G3 { get; set; }
        /// <summary>
        /// Усього за розділом IІІ На кінець період
        /// </summary>
        public double R1695G4 { get; set; }
        /// <summary>
        /// ІV. Зобов’язання, пов’язані з необоротними активами, утримуваними для продажу, та групами вибуття На початок період
        /// </summary>
        public double R1700G3 { get; set; }
        /// <summary>
        /// ІV. Зобов’язання, пов’язані з необоротними активами, утримуваними для продажу, та групами вибуття На кінець період
        /// </summary>
        public double R1700G4 { get; set; }
        /// <summary>
        /// Баланс по пасивах На початок період
        /// </summary>
        public double R1900G3 { get; set; }
        /// <summary>
        /// Баланс по пасивах На кінець період
        /// </summary>
        public double R1900G4 { get; set; }
        /// <summary>
        /// Чистий дохід від реалізації продукції (товарів, робіт, послуг) За звітний період
        /// </summary>
        public double R2000G3 { get; set; }
        /// <summary>
        /// Чистий дохід від реалізації продукції (товарів, робіт, послуг) За аналогічний період попереднього року
        /// </summary>
        public double R2000G4 { get; set; }
        /// <summary>
        /// Інші операційні доходи За звітний період
        /// </summary>
        public double R2120G3 { get; set; }
        /// <summary>
        /// Інші операційні доходи За аналогічний період попереднього року
        /// </summary>
        public double R2120G4 { get; set; }
        /// <summary>
        /// Інші доходи За звітний період
        /// </summary>
        public double R2240G3 { get; set; }
        /// <summary>
        /// Інші доходи За аналогічний період попереднього року
        /// </summary>
        public double R2240G4 { get; set; }
        /// <summary>
        /// Разом доходи (2000 + 2120 + 2240) За звітний період
        /// </summary>
        public double R2280G3 { get; set; }
        /// <summary>
        /// Разом доходи (2000 + 2120 + 2240) За аналогічний період попереднього року
        /// </summary>
        public double R2280G4 { get; set; }
        /// <summary>
        /// Собівартість реалізованої продукції (товарів, робіт, послуг) За звітний період
        /// </summary>
        public double R2050G3 { get; set; }
        /// <summary>
        /// Собівартість реалізованої продукції (товарів, робіт, послуг) За аналогічний період попереднього року
        /// </summary>
        public double R2050G4 { get; set; }
        /// <summary>
        /// Інші операційні витрати За звітний період
        /// </summary>
        public double R2180G3 { get; set; }
        /// <summary>
        /// Інші операційні витрати За аналогічний період попереднього року
        /// </summary>
        public double R2180G4 { get; set; }
        /// <summary>
        /// Інші витрати За звітний період
        /// </summary>
        public double R2270G3 { get; set; }
        /// <summary>
        /// Інші витрати За аналогічний період попереднього року
        /// </summary>
        public double R2270G4 { get; set; }
        /// <summary>
        /// Разом витрати За звітний період
        /// </summary>
        public double R2285G3 { get; set; }
        /// <summary>
        /// Разом витрати За аналогічний період попереднього року
        /// </summary>
        public double R2285G4 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування (2268 – 2285) За звітний період
        /// </summary>
        public double R2290G3 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування (2268 – 2285) За аналогічний період попереднього року
        /// </summary>
        public double R2290G4 { get; set; }
        /// <summary>
        /// Податок на прибуток За звітний період
        /// </summary>
        public double R2300G3 { get; set; }
        /// <summary>
        /// Податок на прибуток За аналогічний період попереднього року
        /// </summary>
        public double R2300G4 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) (2290 – 2300) За звітний період
        /// </summary>
        public double R2350G3 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) (2290 – 2300) За аналогічний період попереднього року
        /// </summary>
        public double R2350G4 { get; set; }
        /// <summary>
        /// Дата фактичного подання звітності
        /// </summary>
        public DateTime? DocDate { get; set; }
        /// <summary>
        /// Середня кількість співробітників
        /// </summary>
        public int? SCh { get; set; }
        /// <summary>
        /// Тип звіту
        /// </summary>
        public string? CDocType { get; set; }

        /// <summary>
        /// Кількість податкових звітів у файлі.
        /// </summary>
        public string? CDocCnt { get; set; }

        /// <summary>
        /// Код регіону.
        /// </summary>
        public string? CReg { get; set; }

        /// <summary>
        /// Код району.
        /// </summary>
        public string? CRaj { get; set; }

        /// <summary>
        /// Тип період звіту
        /// </summary>
        public string? PeriodType { get; set; }

        /// <summary>
        /// Назва програмного забезпечення, яке використовувалося для підготовки та подання звіту.
        /// </summary>
        public string? Software { get; set; }
        /// <summary>
        /// Адреса підприємства
        /// </summary>
        public string? FirmAdr { get; set; }
        /// <summary>
        /// код КВЕД (основного виду діяльності)
        /// </summary>
        public string? FirmKved { get; set; }
        /// <summary>
        /// пона назва КВЕД (основного виду діяльності)
        /// </summary>
        public string? FirmKvednm { get; set; }
        /// <summary>
        /// код за КОПФГ
        /// </summary>
        public string? FirmOpfcd { get; set; }
        /// <summary>
        /// Організаційно-правова форма господарювання
        /// </summary>
        public string? FirmOpfnm { get; set; }
        /// <summary>
        /// ПІБ керівника підприємства
        /// </summary>
        public string? FirmRuk { get; set; }
        /// <summary>
        /// Територія
        /// </summary>
        public string? FirmTerr { get; set; }
        /// <summary>
        /// Звіт про фінансові результати за цю дату
        /// </summary>
        public string? RepPernm { get; set; }
        /// <summary>
        /// код за КАТОТТГ
        /// </summary>
        public string? Katottg { get; set; }
        /// <summary>
        /// ПІБ бухгалтера
        /// </summary>
        public string? FirmBuh { get; set; }
        /// <summary>
        /// Контактний телефон підприємства
        /// </summary>
        public string? FirmTelorg { get; set; }
        /// <summary>
        /// Остнанній день
        /// </summary>
        public string? Lastday { get; set; }
        /// <summary>
        /// Баланс на станом на цю дату
        /// </summary>
        public string? MyDate { get; set; }
        /// <summary>
        /// код області
        /// </summary>
        public string? Obl { get; set; }
        /// <summary>
        /// Код району
        /// </summary>
        public string? Ray { get; set; }

        /// <summary>
        /// "Коди" рік
        /// </summary>
        public string? N13 { get; set; }
        /// <summary>
        /// "Коди" місяць
        /// </summary>
        public string? N14 { get; set; }

        /// <summary>
        /// Назва файлу
        /// </summary>
        public string? FileName { get; set; }
    }
    public class FinResFinZvitMikroPidpr
    {
        /// <summary>
        /// ідентифікатор фін. звіту 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код ЄДРПОУ (у разі якщо код містить спереду "00" Накриклад: 00131305 нулі буде обрізано і в цьому полі буде значення 131305
        /// </summary>
        public int Tin { get; set; }
        /// <summary>
        /// Повне наіменування юр. особи
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Тип форми звітності
        /// </summary>
        public string FormCode { get; set; }
        /// <summary>
        /// Назва форми фінансової звітності
        /// </summary>
        public string FormName { get; set; }
        /// <summary>
        /// Місяці за які подано звіт
        /// </summary>
        public int PeriodMonth { get; set; }
        /// <summary>
        /// Рік за який подано звіт
        /// </summary>
        public int PeriodYear { get; set; }
        /// <summary>
        /// Гранична дата подання звіту
        /// </summary>
        public string DGet { get; set; }
        /// <summary>
        /// Основні засоби На початок період
        /// </summary>
        public double R1010G3 { get; set; }
        /// <summary>
        /// Основні засоби На кінець період
        /// </summary>
        public double R1010G4 { get; set; }
        /// <summary>
        /// Основні засоби первісна вартість На початок період
        /// </summary>
        public double R1011G3 { get; set; }
        /// <summary>
        /// Основні засоби первісна вартість На кінець період
        /// </summary>
        public double R1011G4 { get; set; }
        /// <summary>
        /// Основні засоби знос На початок період
        /// </summary>
        public double R1012G3 { get; set; }
        /// <summary>
        /// Основні засоби знос На кінець період
        /// </summary>
        public double R1012G4 { get; set; }
        /// <summary>
        /// Інші необоротні активи На початок період
        /// </summary>
        public double R1090G3 { get; set; }
        /// <summary>
        /// Інші необоротні активи На кінець період
        /// </summary>
        public double R1090G4 { get; set; }
        /// <summary>
        /// Усього за розділом І На початок період
        /// </summary>
        public double R1095G3 { get; set; }
        /// <summary>
        /// Усього за розділом І На кінець період
        /// </summary>
        public double R1095G4 { get; set; }
        /// <summary>
        /// Запаси На початок період
        /// </summary>
        public double R1100G3 { get; set; }
        /// <summary>
        /// Запаси На кінець період
        /// </summary>
        public double R1100G4 { get; set; }
        /// <summary>
        /// Інша поточна дебіторська заборгованість На початок період
        /// </summary>
        public double R1155G3 { get; set; }
        /// <summary>
        /// Інша поточна дебіторська заборгованість На кінець період
        /// </summary>
        public double R1155G4 { get; set; }
        /// <summary>
        /// Гроші та їх еквіваленти На початок період
        /// </summary>
        public double R1165G3 { get; set; }
        /// <summary>
        /// Гроші та їх еквіваленти На кінець період
        /// </summary>
        public double R1165G4 { get; set; }
        /// <summary>
        /// Інші оборотні активи На початок період
        /// </summary>
        public double R1190G3 { get; set; }
        /// <summary>
        /// Інші оборотні активи На кінець період
        /// </summary>
        public double R1190G4 { get; set; }
        /// <summary>
        /// Усього за розділом II На початок період
        /// </summary>
        public double R1195G3 { get; set; }
        /// <summary>
        /// Усього за розділом II На кінець період
        /// </summary>
        public double R1195G4 { get; set; }
        /// <summary>
        /// Баланс актіву На початок період
        /// </summary>
        public double R1300G3 { get; set; }
        /// <summary>
        /// Баланс актіву На кінець період
        /// </summary>
        public double R1300G4 { get; set; }
        /// <summary>
        /// Зареєстрований (пайовий) капітал На початок період
        /// </summary>
        public double R1400G3 { get; set; }
        /// <summary>
        /// Зареєстрований (пайовий) капітал На кінець період
        /// </summary>
        public double R1400G4 { get; set; }
        /// <summary>
        /// Нерозподілений прибуток (непокритий збиток) На початок період
        /// </summary>
        public double R1420G3 { get; set; }
        /// <summary>
        /// Нерозподілений прибуток (непокритий збиток) На кінець період
        /// </summary>
        public double? R1420G4 { get; set; }
        /// <summary>
        /// Неоплачений капітал На початок період
        /// </summary>
        public double? R1425G3 { get; set; }
        /// <summary>
        /// Неоплачений капітал На кінець період
        /// </summary>
        public double? R1425G4 { get; set; }
        /// <summary>
        /// Усього за розділом I На початок період
        /// </summary>
        public double R1495G3 { get; set; }
        /// <summary>
        /// Усього за розділом I На кінець період
        /// </summary>
        public double R1495G4 { get; set; }
        /// <summary>
        /// II. Довгострокові зобов’язання, цільове фінансування та забезпечення На початок період
        /// </summary>
        public double R1595G3 { get; set; }
        /// <summary>
        /// II. Довгострокові зобов’язання, цільове фінансування та забезпечення На кінець період
        /// </summary>
        public double R1595G4 { get; set; }
        /// <summary>
        /// Короткострокові кредити банків На початок період
        /// </summary>
        public double R1600G3 { get; set; }
        /// <summary>
        /// Короткострокові кредити банків На кінець період
        /// </summary>
        public double R1600G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за товари, роботи, послуги На початок період
        /// </summary>
        public double R1615G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за товари, роботи, послуги На кінець період
        /// </summary>
        public double R1615G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з бюджетом На початок період
        /// </summary>
        public double R1620G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з бюджетом На кінець період
        /// </summary>
        public double R1620G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками зі страхування На початок період
        /// </summary>
        public double R1625G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками зі страхування На кінець період
        /// </summary>
        public double R1625G4 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з оплати праці На початок період
        /// </summary>
        public double R1630G3 { get; set; }
        /// <summary>
        /// Поточна кредиторська заборгованість за розрахунками з оплати праці На кінець період
        /// </summary>
        public double R1630G4 { get; set; }
        /// <summary>
        /// Інші поточні зобов’язання На початок період
        /// </summary>
        public double R1690G3 { get; set; }
        /// <summary>
        /// Інші поточні зобов’язання На кінець період
        /// </summary>
        public double R1690G4 { get; set; }
        /// <summary>
        /// Усього за розділом IІІ На початок період
        /// </summary>
        public double R1695G3 { get; set; }
        /// <summary>
        /// Усього за розділом IІІ На кінець період
        /// </summary>
        public double R1695G4 { get; set; }
        /// <summary>
        /// Баланс по пасивах На початок період
        /// </summary>
        public double R1900G3 { get; set; }
        /// <summary>
        /// Баланс по пасивах На кінець період
        /// </summary>
        public double R1900G4 { get; set; }
        /// <summary>
        /// Чистий дохід від реалізації продукції (товарів, робіт, послуг) За звітний період
        /// </summary>
        public double R2000G3 { get; set; }
        /// <summary>
        /// Чистий дохід від реалізації продукції (товарів, робіт, послуг) За попередній період
        /// </summary>
        public double R2000G4 { get; set; }
        /// <summary>
        /// Інші доходи За звітний період
        /// </summary>
        public double R2160G3 { get; set; }
        /// <summary>
        /// Інші доходи За попередній період
        /// </summary>
        public double R2160G4 { get; set; }
        /// <summary>
        /// Разом доходи (2000 + 2160) За звітний період
        /// </summary>
        public double R2280G3 { get; set; }
        /// <summary>
        /// Разом доходи (2000 + 2160) За попередній період
        /// </summary>
        public double R2280G4 { get; set; }
        /// <summary>
        /// Собівартість реалізованої продукції (товарів, робіт, послуг) За звітний період
        /// </summary>
        public double R2050G3 { get; set; }
        /// <summary>
        /// Собівартість реалізованої продукції (товарів, робіт, послуг) За попередній період
        /// </summary>
        public double R2050G4 { get; set; }
        /// <summary>
        /// Інші витрати За звітний період
        /// </summary>
        public double R2165G3 { get; set; }
        /// <summary>
        /// Інші витрати За попередній період
        /// </summary>
        public double R2165G4 { get; set; }
        /// <summary>
        /// разом витрати (2050 - 2165) За звітний період
        /// </summary>
        public double R2285G3 { get; set; }
        /// <summary>
        /// разом витрати (2050 - 2165) За попередній період
        /// </summary>
        public double R2285G4 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування (2268 – 2285) За звітний період
        /// </summary>
        public double R2290G3 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування (2268 – 2285) За попередній період
        /// </summary>
        public double R2290G4 { get; set; }
        /// <summary>
        /// Податок на прибуток За звітний період
        /// </summary>
        public double R2300G3 { get; set; }
        /// <summary>
        /// Податок на прибуток За попередній період
        /// </summary>
        public double R2300G4 { get; set; }
        /// <summary>
        /// Витрати (доходи), які змнешують (збільшують) фінансовий результат після оподаткування За звітний період
        /// </summary>
        public double R2310G3 { get; set; }
        /// <summary>
        /// Витрати (доходи), які змнешують (збільшують) фінансовий результат після оподаткування За попередній період
        /// </summary>
        public double R2310G4 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) (2290 – 2300 -(+) 2310) За звітний період
        /// </summary>
        public double R2350G3 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) (2290 – 2300 -(+) 2310) За попередній період
        /// </summary>
        public double R2350G4 { get; set; }
        /// <summary>
        /// Дата фактичного подання звіту
        /// </summary>
        public DateTime? DocDate { get; set; }
        /// <summary>
        /// Середня кількість співробітників
        /// </summary>
        public int? SCh { get; set; }
        /// <summary>
        /// Область
        /// </summary>
        public string? Obl { get; set; }
        /// <summary>
        /// Район
        /// </summary>
        public string? Ray { get; set; }
        /// <summary>
        /// Адреса юр. сооби
        /// </summary>
        public string? FirmAdr { get; set; }
        /// <summary>
        /// Програмне забезпечення з якого відбувалось подання фіз. звітності
        /// </summary>
        public string? Software { get; set; }
        /// <summary>
        /// Тип період
        /// </summary>
        public string? PeriodType { get; set; }
        /// <summary>
        /// Дип документу
        /// </summary>
        public string? CDocType { get; set; }
        /// <summary>
        /// Кількість документів
        /// </summary>
        public string? CDocCnt { get; set; }
        /// <summary>
        /// Код регіону
        /// </summary>
        public string? CReg { get; set; }
        /// <summary>
        /// Код району
        /// </summary>
        public string? CRaj { get; set; }
        /// <summary>
        /// Керівник юр. особи
        /// </summary>
        public string? FirmRuk { get; set; }
        /// <summary>
        /// Бухгалтер юр. особи
        /// </summary>
        public string? FirmBuh { get; set; }
        /// <summary>
        /// Звіт за цей період
        /// </summary>
        public string? RepPernm { get; set; }
        /// <summary>
        /// Територія
        /// </summary>
        public string? FirmTerr { get; set; }
        /// <summary>
        /// Рік исло подання звітності
        /// </summary>
        public int? N13 { get; set; }
        /// <summary>
        /// Число подання звітності
        /// </summary>
        public int? N14 { get; set; }
        /// <summary>
        /// Останній день подання звітності у звітному періоді
        /// </summary>
        public int? Lastday { get; set; }
        /// <summary>
        /// Код за КАТОТТГ
        /// </summary>
        public string? Katottg { get; set; }
        /// <summary>
        /// Код організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfcd { get; set; }
        /// <summary>
        /// Назва організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfnm { get; set; }
        /// <summary>
        /// Назва КВЕД компанії
        /// </summary>
        public string? FirmKvednm { get; set; }
        /// <summary>
        /// Код КВЕД компанії
        /// </summary>
        public string? FirmKved { get; set; }
        /// <summary>
        /// Контактний номер телефону юр. особи
        /// </summary>
        public string? FirmTelorg { get; set; }
        /// <summary>
        /// Заголовок "Звіт на цю дату"
        /// </summary>
        public string? MyDate { get; set; }

        /// <summary>
        /// Назва файлу
        /// </summary>
        public string? FileName { get; set; }
    }
    public class FinResZvitFinRez
    {
        /// <summary>
        /// Ідентифікатор звіту
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код ЄДРПОУ (у разі якщо код містить спереду "00" Накриклад: 00131305 нулі буде обрізано і в цьому полі буде значення 131305
        /// </summary>
        public int Tin { get; set; }
        /// <summary>
        /// Повне наіменування юр. особи
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Код форми звітності
        /// </summary>
        public string FormCode { get; set; }
        /// <summary>
        /// Наіменування форми звітності
        /// </summary>
        public string FormName { get; set; }
        /// <summary>А
        /// Період місяців за яку сформовано звіт
        /// </summary>
        public int PeriodMonth { get; set; }
        /// <summary>
        /// Рік звіту 
        /// </summary>
        public int PeriodYear { get; set; }
        /// <summary>
        /// Гранична дата подання звітності
        /// </summary>
        public string DGet { get; set; }
        /// <summary>
        /// Чистий дохід від реалізації продукції (товарів, робіт, послуг) За звітний період
        /// </summary>
        public double R2000G3 { get; set; }
        /// <summary>
        /// Чистий дохід від реалізації продукції (товарів, робіт, послуг) За аналогічний період попереднього року
        /// </summary>
        public double R2000G4 { get; set; }
        /// <summary>
        /// Чисті зароблені страхові премії За звітний період
        /// </summary>
        public double R2010G3 { get; set; }
        /// <summary>
        /// Чисті зароблені страхові премії За аналогічний період попереднього року
        /// </summary>
        public double R2010G4 { get; set; }
        /// <summary>
        /// премії підписані, валова сума За звітний період
        /// </summary>
        public double R2011G3 { get; set; }
        /// <summary>
        /// премії підписані, валова сума За аналогічний період попереднього року
        /// </summary>
        public double R2011G4 { get; set; }
        /// <summary>
        /// премії, передані у перестрахування За звітний період
        /// </summary>
        public double R2012G3 { get; set; }
        /// <summary>
        /// премії, передані у перестрахування За аналогічний період попереднього року
        /// </summary>
        public double R2012G4 { get; set; }
        /// <summary>
        /// зміна резерву незароблених премій, валова сума За звітний період
        /// </summary>
        public double R2013G3 { get; set; }
        /// <summary>
        /// зміна резерву незароблених премій, валова сума За аналогічний період попереднього року
        /// </summary>
        public double R2013G4 { get; set; }
        /// <summary>
        /// зміна частки перестраховиків у резерві незароблених премій За звітний період
        /// </summary>
        public double R2014G3 { get; set; }
        /// <summary>
        /// зміна частки перестраховиків у резерві незароблених премій За аналогічний період попереднього року
        /// </summary>
        public double R2014G4 { get; set; }
        /// <summary>
        /// Собівартість реалізованої продукції (товарів, робіт, послуг) За звітний період
        /// </summary>
        public double R2050G3 { get; set; }
        /// <summary>
        /// Собівартість реалізованої продукції (товарів, робіт, послуг) За аналогічний період попереднього року
        /// </summary>
        public double R2050G4 { get; set; }
        /// <summary>
        /// Чисті понесені збитки за страховими виплатами За звітний період
        /// </summary>
        public double R2070G3 { get; set; }
        /// <summary>
        /// исті понесені збитки за страховими виплатами За аналогічний період попереднього року
        /// </summary>
        public double R2070G4 { get; set; }
        /// <summary>
        /// Валовий прибуток За звітний період
        /// </summary>
        public double R2090G3 { get; set; }
        /// <summary>
        /// Валовий прибуток За аналогічний період попереднього року
        /// </summary>
        public double R2090G4 { get; set; }
        /// <summary>
        /// Валовий збиток За звітний період
        /// </summary>
        public double R2095G3 { get; set; }
        /// <summary>
        /// Валовий збиток За аналогічний період попереднього року
        /// </summary>
        public double R2095G4 { get; set; }
        /// <summary>
        /// Дохід (витрати) дів зміни у резервах довгострокових зобов'язань За звітний період
        /// </summary>
        public double R2105G3 { get; set; }
        /// <summary>
        /// Дохід (витрати) дів зміни у резервах довгострокових зобов'язань За аналогічний період попереднього року
        /// </summary>
        public double R2105G4 { get; set; }
        /// <summary>
        /// Дохід (витрати) дів зміни інших страхових резервів За звітний період
        /// </summary>
        public double R2110G3 { get; set; }
        /// <summary>
        /// Дохід (витрати) дів зміни інших страхових резервів За аналогічний період попереднього року
        /// </summary>
        public double R2110G4 { get; set; }
        /// <summary>
        /// зміна інших страхових резервів, валова сума За звітний період
        /// </summary>
        public double R2111G3 { get; set; }
        /// <summary>
        /// зміна інших страхових резервів, валова сума За аналогічний період попереднього року
        /// </summary>
        public double R2111G4 { get; set; }
        /// <summary>
        /// зміна частки перестраховиків в інших страхових резервах За звітний період
        /// </summary>
        public double R2112G3 { get; set; }
        /// <summary>
        /// зміна частки перестраховиків в інших страхових резервах За аналогічний період попереднього року
        /// </summary>
        public double R2112G4 { get; set; }
        /// <summary>
        /// Інші операційні доходи За звітний період
        /// </summary>
        public double R2120G3 { get; set; }
        /// <summary>
        /// Інші операційні доходи За аналогічний період попереднього року
        /// </summary>
        public double R2120G4 { get; set; }
        /// <summary>
        /// у тому числі: дохід від зміни вартості активів, які оцінюються за справедливою вартістю За звітний період
        /// </summary>
        public double R2121G3 { get; set; }
        /// <summary>
        /// у тому числі: дохід від зміни вартості активів, які оцінюються за справедливою вартістю За аналогічний період попереднього року
        /// </summary>
        public double R2121G4 { get; set; }
        /// <summary>
        /// дохід від первісного визнання біологічних активів і сільськогосподарської продукції За звітний період
        /// </summary>
        public double R2122G3 { get; set; }
        /// <summary>
        /// дохід від первісного визнання біологічних активів і сільськогосподарської продукції За аналогічний період попереднього року
        /// </summary>
        public double R2122G4 { get; set; }
        /// <summary>
        /// дохід від використання коштів, вивільнених від оподаткування За звітний період
        /// </summary>
        public double R2123G3 { get; set; }
        /// <summary>
        /// дохід від використання коштів, вивільнених від оподаткування За аналогічний період попереднього року
        /// </summary>
        public double R2123G4 { get; set; }
        /// <summary>
        /// Адміністративні витрати За звітний період
        /// </summary>
        public double R2130G3 { get; set; }
        /// <summary>
        /// Адміністративні витрати За аналогічний період попереднього року
        /// </summary>
        public double R2130G4 { get; set; }
        /// <summary>
        /// Витрати на збут За звітний період
        /// </summary>
        public double R2150G3 { get; set; }
        /// <summary>
        /// Витрати на збут За аналогічний період попереднього року
        /// </summary>
        public double R2150G4 { get; set; }
        /// <summary>
        /// Інші операційні витрати За звітний період
        /// </summary>
        public double R2180G3 { get; set; }
        /// <summary>
        /// Інші операційні витрати За аналогічний період попереднього року
        /// </summary>
        public double R2180G4 { get; set; }
        /// <summary>
        /// у тому числі: витрати від зміни вартості активів, які оцінюються за справедливою вартістю За звітний період
        /// </summary>
        public double R2181G3 { get; set; }
        /// <summary>
        /// у тому числі: витрати від зміни вартості активів, які оцінюються за справедливою вартістю За аналогічний період попереднього року
        /// </summary>
        public double R2181G4 { get; set; }
        /// <summary>
        /// витрати від первісного визнання біологічних активів і сільськогосподарської продукції За звітний період
        /// </summary>
        public double R2182G3 { get; set; }
        /// <summary>
        /// витрати від первісного визнання біологічних активів і сільськогосподарської продукції За аналогічний період попереднього року
        /// </summary>
        public double R2182G4 { get; set; }
        /// <summary>
        /// Фінансовий результат від операційної діяльності прибуток За звітний період
        /// </summary>
        public double R2190G3 { get; set; }
        /// <summary>
        /// Фінансовий результат від операційної діяльності прибуток За аналогічний період попереднього року
        /// </summary>
        public double R2190G4 { get; set; }
        /// <summary>
        /// Фінансовий результат від операційної діяльності збиток За звітний період
        /// </summary>
        public double R2195G3 { get; set; }
        /// <summary>
        /// Фінансовий результат від операційної діяльності збиток За аналогічний період попереднього року
        /// </summary>
        public double R2195G4 { get; set; }
        /// <summary>
        /// Дохід від участі в капіталі За звітний період
        /// </summary>
        public double R2200G3 { get; set; }
        /// <summary>
        /// Дохід від участі в капіталі За аналогічний період попереднього року
        /// </summary>
        public double R2200G4 { get; set; }
        /// <summary>
        /// Інші фінансові доходи За звітний період
        /// </summary>
        public double R2220G3 { get; set; }
        /// <summary>
        /// Інші фінансові доходи За аналогічний період попереднього року
        /// </summary>
        public double R2220G4 { get; set; }
        /// <summary>
        /// Інші доходи За звітний період
        /// </summary>
        public double R2240G3 { get; set; }
        /// <summary>
        /// Інші доходи За аналогічний період попереднього року
        /// </summary>
        public double R2240G4 { get; set; }
        /// <summary>
        /// у тому числі: дохід від благодійної допомоги За звітний період
        /// </summary>
        public double R2241G3 { get; set; }
        /// <summary>
        /// у тому числі: дохід від благодійної допомоги За аналогічний період попереднього року
        /// </summary>
        public double R2241G4 { get; set; }
        /// <summary>
        /// Фінансові витрати За звітний період
        /// </summary>
        public double R2250G3 { get; set; }
        /// <summary>
        /// Фінансові витрати За аналогічний період попереднього року
        /// </summary>
        public double R2250G4 { get; set; }
        /// <summary>
        /// Витрати від участі в капіталі За звітний період
        /// </summary>
        public double R2255G3 { get; set; }
        /// <summary>
        /// Витрати від участі в капіталі За аналогічний період попереднього року
        /// </summary>
        public double R2255G4 { get; set; }
        /// <summary>
        /// Інші витрати За звітний період
        /// </summary>
        public double R2270G3 { get; set; }
        /// <summary>
        /// Інші витрати За аналогічний період попереднього року
        /// </summary>
        public double R2270G4 { get; set; }
        /// <summary>
        /// Прибуток (збиток) від впливу інфляції на монетарні статті За звітний період
        /// </summary>
        public double R2275G3 { get; set; }
        /// <summary>
        /// Прибуток (збиток) від впливу інфляції на монетарні статті За аналогічний період попереднього року
        /// </summary>
        public double R2275G4 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування прибуток За звітний період
        /// </summary>
        public double R2290G3 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування прибуток За аналогічний період попереднього року
        /// </summary>
        public double R2290G4 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування збиток За звітний період
        /// </summary>
        public double R2295G3 { get; set; }
        /// <summary>
        /// Фінансовий результат до оподаткування збиток За аналогічний період попереднього року
        /// </summary>
        public double R2295G4 { get; set; }
        /// <summary>
        /// Витрати (дохід) з податку на прибуток За звітний період
        /// </summary>
        public double R2300G3 { get; set; }
        /// <summary>
        /// Витрати (дохід) з податку на прибуток За аналогічний період попереднього року
        /// </summary>
        public double R2300G4 { get; set; }
        /// <summary>
        /// Прибуток (збиток) від припиненої діяльності після оподаткування За звітний період
        /// </summary>
        public double R2305G3 { get; set; }
        /// <summary>
        /// Прибуток (збиток) від припиненої діяльності після оподаткування За аналогічний період попереднього року
        /// </summary>
        public double R2305G4 { get; set; }
        /// <summary>
        /// Чистий фінансовий результат прибуток За звітний період
        /// </summary>
        public double R2350G3 { get; set; }
        /// <summary>
        /// Чистий фінансовий результат прибуток За аналогічний період попереднього року
        /// </summary>
        public double R2350G4 { get; set; }
        /// <summary>
        /// Чистий фінансовий результат збиток За звітний період
        /// </summary>
        public double R2355G3 { get; set; }
        /// <summary>
        /// Чистий фінансовий результат збиток За аналогічний період попереднього року
        /// </summary>
        public double R2355G4 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів За звітний період
        /// </summary>
        public double R2400G3 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів За аналогічний період попереднього року
        /// </summary>
        public double R2400G4 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів За звітний період
        /// </summary>
        public double R2405G3 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів За аналогічний період попереднього року
        /// </summary>
        public double R2405G4 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці За звітний період
        /// </summary>
        public double R2410G3 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці За аналогічний період попереднього року
        /// </summary>
        public double R2410G4 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих та спільних підприємств За звітний період
        /// </summary>
        public double R2415G3 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих та спільних підприємств За аналогічний період попереднього року
        /// </summary>
        public double R2415G4 { get; set; }
        /// <summary>
        /// Інший сукупний дохід За звітний період
        /// </summary>
        public double R2445G3 { get; set; }
        /// <summary>
        /// Інший сукупний дохід За аналогічний період попереднього року
        /// </summary>
        public double R2445G4 { get; set; }
        /// <summary>
        /// Інший сукупний дохід до оподаткування За звітний період
        /// </summary>
        public double R2450G3 { get; set; }
        /// <summary>
        /// Інший сукупний дохід до оподаткування За аналогічний період попереднього року
        /// </summary>
        public double R2450G4 { get; set; }
        /// <summary>
        /// Податок на прибуток, пов’язаний з іншим сукупним доходом За звітний період
        /// </summary>
        public double R2455G3 { get; set; }
        /// <summary>
        /// Податок на прибуток, пов’язаний з іншим сукупним доходом За аналогічний період попереднього року
        /// </summary>
        public double R2455G4 { get; set; }
        /// <summary>
        /// Інший сукупний дохід після оподаткування За звітний період
        /// </summary>
        public double R2460G3 { get; set; }
        /// <summary>
        /// Інший сукупний дохід після оподаткування За аналогічний період попереднього року
        /// </summary>
        public double R2460G4 { get; set; }
        /// <summary>
        /// Сукупний дохід (сума рядків 2350, 2355 та 2460) За звітний період
        /// </summary>
        public double R2465G3 { get; set; }
        /// <summary>
        /// Сукупний дохід (сума рядків 2350, 2355 та 2460 За аналогічний період попереднього року
        /// </summary>
        public double R2465G4 { get; set; }
        /// <summary>
        /// Матеріальні затрати За звітний період
        /// </summary>
        public double R2500G3 { get; set; }
        /// <summary>
        /// Матеріальні затрати За аналогічний період попереднього року
        /// </summary>
        public double R2500G4 { get; set; }
        /// <summary>
        /// Витрати на оплату праці За звітний період
        /// </summary>
        public double R2505G3 { get; set; }
        /// <summary>
        /// Витрати на оплату праці За аналогічний період попереднього року
        /// </summary>
        public double R2505G4 { get; set; }
        /// <summary>
        /// Відрахування на соціальні заходи За звітний період
        /// </summary>
        public double R2510G3 { get; set; }
        /// <summary>
        /// Відрахування на соціальні заходи За аналогічний період попереднього року
        /// </summary>
        public double R2510G4 { get; set; }
        /// <summary>
        /// Амортизація За звітний період
        /// </summary>
        public double R2515G3 { get; set; }
        /// <summary>
        /// Амортизація За аналогічний період попереднього року
        /// </summary>
        public double R2515G4 { get; set; }
        /// <summary>
        /// Інші операційні витрати За звітний період
        /// </summary>
        public double R2520G3 { get; set; }
        /// <summary>
        /// Інші операційні витрати За аналогічний період попереднього року
        /// </summary>
        public double R2520G4 { get; set; }
        /// <summary>
        /// Разом За звітний період
        /// </summary>
        public double R2550G3 { get; set; }
        /// <summary>
        /// Разом За аналогічний період попереднього року
        /// </summary>
        public double R2550G4 { get; set; }
        /// <summary>
        /// Середньорічна кількість простих акцій За звітний період
        /// </summary>
        public double R2600G3 { get; set; }
        /// <summary>
        /// Середньорічна кількість простих акцій За аналогічний період попереднього року
        /// </summary>
        public double R2600G4 { get; set; }
        /// <summary>
        /// Скоригована середньорічна кількість простих акцій За звітний період
        /// </summary>
        public double R2605G3 { get; set; }
        /// <summary>
        /// Скоригована середньорічна кількість простих акцій За аналогічний період попереднього року
        /// </summary>
        public double R2605G4 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) на одну просту акцію За звітний період
        /// </summary>
        public double R2610G3 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) на одну просту акцію За аналогічний період попереднього року
        /// </summary>
        public double R2610G4 { get; set; }
        /// <summary>
        /// Скоригований чистий прибуток (збиток) на одну просту акцію За звітний період
        /// </summary>
        public double R2615G3 { get; set; }
        /// <summary>
        /// Скоригований чистий прибуток (збиток) на одну просту акцію За аналогічний період попереднього року
        /// </summary>
        public double R2615G4 { get; set; }
        /// <summary>
        /// Дивіденди на одну просту акцію За звітний період
        /// </summary>
        public double R2650G3 { get; set; }
        /// <summary>
        ///  Дивіденди на одну просту акцію За аналогічний період попереднього року
        /// </summary>
        public double R2650G4 { get; set; }
        /// <summary>
        /// Дата фактичного подання звіту
        /// </summary>
        public DateTime? DocDate { get; set; }
        /// <summary>
        /// Код КВЕД
        /// </summary>

        public string? KVED { get; set; }
        /// <summary>
        /// Код за КАТОТТГ
        /// </summary>

        public string? KATOTTG { get; set; }

        /// <summary>
        ///Тип звіту: 0 - загальний звіт, 1 - коригуючий звіт.
        /// </summary>
        public string? CDocType { get; set; }
        /// <summary>
        /// Кількість податкових звітів у файлі.
        /// </summary>
        public string? CDocCnt { get; set; }
        /// <summary>
        /// Код регіону.
        /// </summary>
        public string? CReg { get; set; }
        /// <summary>
        /// Код району.
        /// </summary>
        public string? CRaj { get; set; }
        /// <summary>
        /// Тип період
        /// </summary>
        public string? PeriodType { get; set; }
        /// <summary>
        /// Програмне забезпечення, через яке здійснювалось подання звітності
        /// </summary>
        public string? Software { get; set; }
        /// <summary>
        /// Звіт за цей період
        /// </summary>
        public string? RepPernm { get; set; }
        /// <summary>
        /// ПІБ головного бухгалтера компанії
        /// </summary>
        public string? FirmBuh { get; set; }
        /// <summary>
        /// ПІБ директора компанії
        /// </summary>
        public string? FirmRuk { get; set; }
        /// <summary>
        /// Рік подачі звітності
        /// </summary>
        public string? N1 { get; set; }
        /// <summary>
        /// Місяць полдачі звітності
        /// </summary>
        public string? N2 { get; set; }
        /// <summary>
        /// Крайній день подачі звітності
        /// </summary>
        public string? LastDay { get; set; }
        /// <summary>
        /// Область
        /// </summary>

        public string? Obl { get; set; }
        /// <summary>
        /// Район
        /// </summary>
        public string? Ray { get; set; }
        /// <summary>
        /// Назва файлу
        /// </summary>
        public string? FileName { get; set; }
    }
    /// <summary>
    /// Форма 3 звіту про рух грошових коштів (за прямим методом)
    /// Розшифровка другої цифри коду:
    /// 0 => Надходження від:
    /// 1 => Витрачення на оптау:
    /// 2 => Надходження від реалізації:
    /// 3 => Рух коштів у результаті фінансвої діяльності:
    /// 4 => Чистий рух коштів за звітний період
    /// </summary>
    public class FinZvitForm3
    {
        /// <summary>
        /// Айді
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public int Tin { get; set; }
        /// <summary>
        /// Тип звіту: 0 - загальний звіт, 1 - коригуючий звіт.
        /// </summary>
        public string? CDocType { get; set; }
        /// <summary>
        ///  Кількість податкових звітів у файлі.
        /// </summary>
        public string? CDocCnt { get; set; }
        /// <summary>
        /// Код регіону.
        /// </summary>
        public string? CReg { get; set; }
        /// <summary>
        ///  Код району.
        /// </summary>
        public string? CRaj { get; set; }
        /// <summary>
        /// Тип періоду звіту: 0 - за місяць, 1 - за квартал, 2 - за півріччя, 3 - за 9 місяців, 4 - за рік, 5 - за інший період.
        /// </summary>
        public string? PeriodType { get; set; }
        /// <summary>
        /// Місяц періоду звіту
        /// </summary>
        public int PeriodMonth { get; set; }
        /// <summary>
        /// Рік періоду звіту
        /// </summary>
        public int PeriodYear { get; set; }
        /// <summary>
        /// Назва програмного забезпечення, яке використовувалося для підготовки та подання звіту.
        /// </summary>
        public string? Software { get; set; }
        /// <summary>
        /// Назва компанії
        /// </summary>
        public string? FirmName { get; set; }
        /// <summary>
        /// Форм код
        /// </summary>
        public string? FormCode { get; set; }
        /// <summary>
        /// Звіт про рух грошових коштів (за прямим методом)
        /// </summary>
        public string? FormName { get; set; }
        /// <summary>
        /// Дата та час подачі
        /// </summary>
        public string? DGet { get; set; }
        /// <summary>
        /// Дата час подачі звіту
        /// </summary>
        public DateTime? FilingDate { get; set; }

        /// <summary>
        /// ПІБ бухгалтера
        /// </summary>
        public string? FirmBuh { get; set; }
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string? FirmEdrpou { get; set; }
        /// <summary>
        /// ПІБ керівника
        /// </summary>
        public string? FirmRuk { get; set; }
        /// <summary>
        /// Звіт про фінансові результати за цю дату
        /// </summary>
        public string? RepPernm { get; set; }
        /// <summary>
        /// Останній день
        /// </summary>
        public string? Lastday { get; set; }
        /// <summary>
        /// код області
        /// </summary>
        public string? Obl { get; set; }
        /// <summary>
        /// код району
        /// </summary>
        public string? Ray { get; set; }
        /// <summary>
        /// Рік
        /// </summary>
        public string N1 { get; set; }
        /// <summary>
        /// Місяць
        /// </summary>
        public string N2 { get; set; }
        /// <summary>
        /// Кодифікатор адміністративно-територіальних одиниць та територій територіальних громад
        /// </summary>
        public string? KATOTTG { get; set; }
        /// <summary>
        /// Надходження від реалізації (товарів, робіт, послуг) за звітний період
        /// </summary>
        public double R3000G3 { get; set; }
        /// <summary>
        /// Надходження від реалізації (товарів, робіт, послуг) за аналогічний період минулого року
        /// </summary>
        public double R3000G4 { get; set; }
        /// <summary>
        /// Надходження від:повернення податків і зборів за звітний період
        /// </summary>
        public double R3005G3 { get; set; }
        /// <summary>
        /// Надходження від:повернення податків і зборів за аналогічний період минулого року
        /// </summary>
        public double R3005G4 { get; set; }
        /// <summary>
        /// Надходження від:цільового фінансування за звітний період
        /// </summary>
        public double R3010G3 { get; set; }
        /// <summary>
        /// Надходження від:цільового фінансування за аналогічний період минулого року
        /// </summary>
        public double R3010G4 { get; set; }
        /// <summary>
        /// Надходження від:у тому числі ПДВ за звітний період
        /// </summary>
        public double R3006G3 { get; set; }
        /// <summary>
        /// Надходження від:у тому числі ПДВ за аналогічний період минулого року
        /// </summary>
        public double R3006G4 { get; set; }
        /// <summary>
        /// Інші надходження за звітний період
        /// </summary>
        public double R3095G3 { get; set; }
        /// <summary>
        /// Інші надходження за аналогічний період минулого року
        /// </summary>
        public double R3095G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату Товарів (робіт, послуг) за звітний період
        /// </summary>
        public double R3100G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату Товарів (робіт, послуг) за аналогічний період минулого року
        /// </summary>
        public double R3100G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату Праці за звітний період
        /// </summary>
        public double R3105G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату Праці за аналогічний період минулого року
        /// </summary>
        public double R3105G4 { get; set; }
        /// <summary>
        /// Відрахувань на соціальні заходи за звітний період
        /// </summary>
        public double R3110G3 { get; set; }
        /// <summary>
        /// Відрахувань на соціальні заходи за аналогічний період минулого року
        /// </summary>
        public double R3110G4 { get; set; }
        /// <summary>
        /// Зобов'язань з податків і зборів за звітний період
        /// </summary>
        public double R3115G3 { get; set; }
        /// <summary>
        /// Зобов'язань з податків і зборів за аналогічний період минулого року
        /// </summary>
        public double R3115G4 { get; set; }
        /// <summary>
        /// Інші витрачання за звітний період
        /// </summary>
        public double R3190G3 { get; set; }
        /// <summary>
        /// Інші витрачання за аналогічний період минулого року
        /// </summary>
        public double R3190G4 { get; set; }
        /// <summary>
        /// Чистий рух коштів за звітний період
        /// </summary>
        public double R3195G3 { get; set; }
        /// <summary>
        /// Чистий рух коштів за аналогічний період минулого року
        /// </summary>
        public double R3195G4 { get; set; }
        /// <summary>
        /// Надходження від реалізації фінансові інвестиції за звітний період
        /// </summary>
        public double R3200G3 { get; set; }
        /// <summary>
        /// Надходження від реалізації фінансові інвестиції за аналогічний період минулого року
        /// </summary>
        public double R3200G4 { get; set; }
        /// <summary>
        /// Надходження від реалізації необоротних активів за звітний період
        /// </summary>
        public double R3205G3 { get; set; }
        /// <summary>
        /// Надходження від реалізації необоротних активів за аналогічний період минулого року
        /// </summary>
        public double R3205G4 { get; set; }
        /// <summary>
        /// Надходження від отриманих відсотків за звітний період
        /// </summary>
        public double R3215G3 { get; set; }
        /// <summary>
        /// Надходження від отриманих відсотків за аналогічний період минулого року
        /// </summary>
        public double R3215G4 { get; set; }
        /// <summary>
        /// Надходження від отриманих девідендів за звітний період
        /// </summary>
        public double R3220G3 { get; set; }
        /// <summary>
        /// Надходження від отриманих девідендів за аналогічний період минулого року
        /// </summary>
        public double R3220G4 { get; set; }
        /// <summary>
        /// Надходження від девідендів за звітний період
        /// </summary>
        public double R3225G3 { get; set; }
        /// <summary>
        /// Надходження від девідендів за аналогічний період минулого року
        /// </summary>
        public double R3225G4 { get; set; }
        /// <summary>
        /// Інші надходження за звітний період
        /// </summary>
        public double R3250G3 { get; set; }
        /// <summary>
        /// Інші надходження за аналогічний період минулого року
        /// </summary>
        public double R3250G4 { get; set; }
        /// <summary>
        /// Витрачання на придбання фінансових інвестицій за звітний період
        /// </summary>
        public double R3255G3 { get; set; }
        /// <summary>
        /// Витрачання на придбання фінансових інвестицій за аналогічний період минулого року
        /// </summary>
        public double R3255G4 { get; set; }
        /// <summary>
        /// Необоротних активів за звітний період
        /// </summary>
        public double R3260G3 { get; set; }
        /// <summary>
        /// Необоротних активів за аналогічний період минулого року
        /// </summary>
        public double R3260G4 { get; set; }
        /// <summary>
        /// Виплати за деривативами за звітний період
        /// </summary>
        public double R3270G3 { get; set; }
        /// <summary>
        /// Виплати за деривативами за аналогічний період минулого року
        /// </summary>
        public double R3270G4 { get; set; }
        /// <summary>
        /// Інші платежі за звітний період
        /// </summary>
        public double R3290G3 { get; set; }
        /// <summary>
        /// Інші платежі за аналогічний період минулого року
        /// </summary>
        public double R3290G4 { get; set; }
        /// <summary>
        /// Чистий рух коштів від інвестиційної діяльностіза звітний період
        /// </summary>
        public double R3295G3 { get; set; }
        /// <summary>
        /// Чистий рух коштів від інвестиційної діяльності за аналогічний період минулого року
        /// </summary>
        public double R3295G4 { get; set; }
        /// <summary>
        /// Надходження від власного капіталу за звітний період
        /// </summary>
        public double R3300G3 { get; set; }
        /// <summary>
        /// Надходження від власного капіталу за аналогічний період минулого року
        /// </summary>
        public double R3300G4 { get; set; }
        /// <summary>
        /// Надходження від Отримання позик за звітний період
        /// </summary>
        public double R3305G3 { get; set; }
        /// <summary>
        /// Надходження від Отримання позик за аналогічний період минулого року
        /// </summary>
        public double R3305G4 { get; set; }
        /// <summary>
        /// Інші надходження за звітний період
        /// </summary>
        public double R3340G3 { get; set; }
        /// <summary>
        /// Інші надходження за аналогічний період минулого року
        /// </summary>
        public double R3340G4 { get; set; }
        /// <summary>
        /// Витрачання на викуп власних акцій за звітний період
        /// </summary>
        public double R3345G3 { get; set; }
        /// <summary>
        /// Витрачання на викуп власних акцій за аналогічний період минулого року
        /// </summary>
        public double R3345G4 { get; set; }
        /// <summary>
        /// Витрачання на Погашення позик за звітний період
        /// </summary>
        public double R3350G3 { get; set; }
        /// <summary>
        /// Витрачання на Погашення позик за аналогічний період минулого року
        /// </summary>
        public double R3350G4 { get; set; }
        /// <summary>
        /// Витрачання на Сплату девидендів за звітний період
        /// </summary>
        public double R3355G3 { get; set; }
        /// <summary>
        /// Витрачання на Сплату девидендів за аналогічний період минулого року
        /// </summary>
        public double R3355G4 { get; set; }
        /// <summary>
        /// Інші платежі за звітний період
        /// </summary>
        public double R3390G3 { get; set; }
        /// <summary>
        /// Інші платежі за аналогічний період минулого року
        /// </summary>
        public double R3390G4 { get; set; }
        /// <summary>
        /// Чистий рух коштів від фінансової діяльності за звітний період
        /// </summary>
        public double R3395G3 { get; set; }
        /// <summary>
        /// за аналогічний період минулого року
        /// </summary>
        public double R3395G4 { get; set; }
        /// <summary>
        /// Чистий рух коштів за звітний період за звітний період
        /// </summary>
        public double R3400G3 { get; set; }
        /// <summary>
        /// Чистий рух коштів за звітний період за аналогічний період минулого року
        /// </summary>
        public double R3400G4 { get; set; }
        /// <summary>
        /// Залишок окштів на початок року за звітний період
        /// </summary>
        public double R3405G3 { get; set; }
        /// <summary>
        /// Залишок окштів на початок року за аналогічний період минулого року
        /// </summary>
        public double R3405G4 { get; set; }
        /// <summary>
        /// Вплив зміни валютних курсів на залишок коштів за звітний період
        /// </summary>
        public double R3410G3 { get; set; }
        /// <summary>
        /// Вплив зміни валютних курсів на залишок коштів за аналогічний період минулого року
        /// </summary>
        public double R3410G4 { get; set; }
        /// <summary>
        /// Залишок коштів на кінець року за звітний період
        /// </summary>
        public double R3415G3 { get; set; }
        /// <summary>
        /// Залишок коштів на кінець року за аналогічний період минулого року
        /// </summary>
        public double R3415G4 { get; set; }
        /// <summary>
        /// Надходження від отримання субсидій, дотацій за звітний період
        /// </summary>
        public double R3011G3 { get; set; }
        /// <summary>
        /// Надходження від отримання субсидій, дотацій за аналогічний період минулого року
        /// </summary>
        public double R3011G4 { get; set; }
        /// <summary>
        /// Надходження авансів від покупців і замовників за звітний період
        /// </summary>
        public double R3015G3 { get; set; }
        /// <summary>
        /// Надходження авансів від покупців і замовників за аналогічний період минулого року
        /// </summary>
        public double R3015G4 { get; set; }
        /// <summary>
        /// Надходження і повернення авансів за звітний період
        /// </summary>
        public double R3020G3 { get; set; }
        /// <summary>
        /// Надходження і повернення авансів за аналогічний період минулого року
        /// </summary>
        public double R3020G4 { get; set; }
        /// <summary>
        /// Надходження від відсотків за залишками коштів на поточних рахунках за звітний період
        /// </summary>
        public double R3025G3 { get; set; }
        /// <summary>
        /// Надходження від відсотків за залишками коштів на поточних рахунках за аналогічний період минулого року
        /// </summary>
        public double R3025G4 { get; set; }
        /// <summary>
        /// Надходження від боржників неустойки (штрафів, пені) за звітний період
        /// </summary>
        public double R3035G3 { get; set; }
        /// <summary>
        /// Надходження від боржників неустойки (штрафів, пені) за аналогічний період минулого року
        /// </summary>
        public double R3035G4 { get; set; }
        /// <summary>
        /// Надходження від операційної оренди за звітний період
        /// </summary>
        public double R3040G3 { get; set; }
        /// <summary>
        /// Надходження від операційної оренди за аналогічний період минулого року
        /// </summary>
        public double R3040G4 { get; set; }
        /// <summary>
        /// Надходження від отримання роялті, авторських винагород за звітний період
        /// </summary>
        public double R3045G3 { get; set; }
        /// <summary>
        /// Надходження від отримання роялті, авторських винагород за аналогічний період минулого року
        /// </summary>
        public double R3045G4 { get; set; }
        /// <summary>
        /// Надходження від страхових премій за звітний період
        /// </summary>
        public double R3050G3 { get; set; }
        /// <summary>
        /// Надходження від страхових премій за аналогічний період минулого року
        /// </summary>
        public double R3050G4 { get; set; }
        /// <summary>
        /// Надходження фінансових установ від повернення позик за звітний період
        /// </summary>
        public double R3055G3 { get; set; }
        /// <summary>
        /// Надходження фінансових установ від повернення позик за аналогічний період минулого року
        /// </summary>
        public double R3055G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату зобов'язань з податку на прибуток за звітний період
        /// </summary>
        public double R3116G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату зобов'язань з податку на прибутокза аналогічний період минулого року
        /// </summary>
        public double R3116G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату зобов'язань ПДВ за звітний період
        /// </summary>
        public double R3117G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату зобов'язань ПДВ за аналогічний період минулого року
        /// </summary>
        public double R3117G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату зобов'язань з інших податків і зборів за звітний період
        /// </summary>
        public double R3118G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату зобов'язань з інших податків і зборів за аналогічний період минулого року
        /// </summary>
        public double R3118G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату авансів за звітний період
        /// </summary>
        public double R3135G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату авансів за аналогічний період минулого року
        /// </summary>
        public double R3135G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату повернення авансів за звітний період
        /// </summary>
        public double R3140G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату повернення авансів за аналогічний період минулого року
        /// </summary>
        public double R3140G4 { get; set; }
        /// <summary>
        /// Витрачення на оплату цільнових внесків за звітний період
        /// </summary>
        public double R3145G3 { get; set; }
        /// <summary>
        /// Витрачення на оплату цільнових внесків за аналогічний період минулого року
        /// </summary>
        public double R3145G4 { get; set; }
        /// <summary>
        /// Витрачання на оплату зобов'яязань за страховими котрактами за звітний період
        /// </summary>
        public double R3150G3 { get; set; }
        /// <summary>
        /// Витрачання на оплату зобов'яязань за страховими котрактами за аналогічний період минулого року
        /// </summary>
        public double R3150G4 { get; set; }
        /// <summary>
        /// Витрачання фінансових установ на надання позик за звітний період
        /// </summary>
        public double R3155G3 { get; set; }
        /// <summary>
        /// Витрачання фінансових установ на надання позик за аналогічний період минулого року
        /// </summary>
        public double R3155G4 { get; set; }
        /// <summary>
        /// Надходження від погашених позик за звітний період
        /// </summary>
        public double R3230G3 { get; set; }
        /// <summary>
        /// Надходження від погашених позик за аналогічний період минулого року
        /// </summary>
        public double R3230G4 { get; set; }
        /// <summary>
        /// Надходження від вибуття дочірнього підприємства та іншої господарської одиниці за звітний період
        /// </summary>
        public double R3235G3 { get; set; }
        /// <summary>
        /// Надходження від вибуття дочірнього підприємства та іншої господарської одиниці за аналогічний період минулого року
        /// </summary>
        public double R3235G4 { get; set; }
        /// <summary>
        /// Витрачення на надання позик за звітний період
        /// </summary>
        public double R3275G3 { get; set; }
        /// <summary>
        /// Витрачення на надання позик за аналогічний період минулого року
        /// </summary>
        public double R3275G4 { get; set; }
        /// <summary>
        /// Витрачення на придбання дочірнього підприємства та іншої господарської одиниці за звітний період
        /// </summary>
        public double R3280G3 { get; set; }
        /// <summary>
        /// Витрачення на придбання дочірнього підприємства та іншої господарської одиниці за аналогічний період минулого року
        /// </summary>
        public double R3280G4 { get; set; }
        /// <summary>
        /// Надходження від продажу частки в дочірньому підприємстві за звітний період
        /// </summary>
        public double R3310G3 { get; set; }
        /// <summary>
        /// Надходження від продажу частки в дочірньому підприємстві за аналогічний період минулого року
        /// </summary>
        public double R3310G4 { get; set; }
        /// <summary>
        /// Витрачення на сплату відсотків за звітний період
        /// </summary>
        public double R3360G3 { get; set; }
        /// <summary>
        /// Витрачення на сплату відсотків за аналогічний період минулого року
        /// </summary>
        public double R3360G4 { get; set; }
        /// <summary>
        /// Витрачення на сплату заборгованості з фінансової оренди за звітний період
        /// </summary>
        public double R3365G3 { get; set; }
        /// <summary>
        /// Витрачення на сплату заборгованості з фінансової оренди за аналогічний період минулого року
        /// </summary>
        public double R3365G4 { get; set; }
        /// <summary>
        /// Витрачення на придбання частки в дочірньому підприємстві за звітний період
        /// </summary>
        public double R3370G3 { get; set; }
        /// <summary>
        /// Витрачення на придбання частки в дочірньому підприємстві за аналогічний період минулого року
        /// </summary>
        public double R3370G4 { get; set; }
        /// <summary>
        /// Витрати на виплачення неконтрольованих часткам у дочірніх підприємствах за звітний період
        /// </summary>
        public double R3375G3 { get; set; }
        /// <summary>
        /// Витрати на виплачення неконтрольованих часткам у дочірніх підприємствах за аналогічний період минулого року
        /// </summary>
        public double R3375G4 { get; set; }

        /// <summary>
        /// Назва файлу на Blob
        /// </summary>
        public string? FileName { get; set; }

    }
    /// <summary>
    /// Звіт про власний капітал
    /// </summary>
    public class FinZvitForm4
    {
        /// <summary>
        /// Айді
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ідентифікаційний номер платника податків.
        /// </summary>
        public int Tin { get; set; }
        /// <summary>
        /// Назва компанії
        /// </summary>
        public string? FirmName { get; set; }
        /// <summary>
        /// Тип звіту: 0 - загальний звіт, 1 - коригуючий звіт.
        /// </summary>
        public string? CDocType { get; set; }
        /// <summary>
        /// Кількість податкових звітів у файлі.
        /// </summary>
        public string? CDocCnt { get; set; }
        /// <summary>
        /// Код регіону.
        /// </summary>
        public string? CReg { get; set; }
        /// <summary>
        /// Код району.
        /// </summary>
        public string? CRaj { get; set; }
        /// <summary>
        /// Тип періоду звіту: 0 - за місяць, 1 - за квартал, 2 - за півріччя, 3 - за 9 місяців, 4 - за рік, 5 - за інший період.
        /// </summary>
        public string? PeriodType { get; set; }
        /// <summary>
        /// Місяц періоду звіту
        /// </summary>
        public int PeriodMonth { get; set; }
        /// <summary>
        /// Рік періоду звіту
        /// </summary>
        public int PeriodYear { get; set; }
        /// <summary>
        /// Назва програмного забезпечення, яке використовувалося для підготовки та подання звіту.
        /// </summary>
        public string? Software { get; set; }
        /// <summary>
        /// Форм код
        /// </summary>
        public string? FormCode { get; set; }
        /// <summary>
        /// Назва типу звіту
        /// </summary>
        public string? FormName { get; set; }

        /// <summary>
        /// Дата та час подачі
        /// </summary>
        public string? DGet { get; set; }
        /// <summary>
        /// Дата час подачі звіту
        /// </summary>
        public DateTime? FilingDate { get; set; }
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string? FirmEdrpou { get; set; }
        /// <summary>
        /// Звіт про фінансові результати за цю дату
        /// </summary>
        public string? RepPernm { get; set; }
        /// <summary>
        /// ПІБ керівника
        /// </summary>
        public string? FirmRuk { get; set; }
        /// <summary>
        /// ПІБ бухгалтера
        /// </summary>
        public string? FirmBuh { get; set; }
        /// <summary>
        /// Останній день
        /// </summary>
        public string? LastDay { get; set; }
        /// <summary>
        /// код області
        /// </summary>
        public string? Obl { get; set; }
        /// <summary>
        /// код району
        /// </summary>
        public string? Ray { get; set; }
        /// <summary>
        /// Кодифікатор адміністративно-територіальних одиниць та територій територіальних громад
        /// </summary>
        public string? KATOTTG { get; set; }
        /// <summary>
        /// Рік
        /// </summary>
        public string? N1 { get; set; }
        /// <summary>
        /// Місяць
        /// </summary>
        public string? N2 { get; set; }
        /// <summary>
        /// Залишок на початок року зареєстрований (пайовий) капітал
        /// </summary>
        public double R4000G3 { get; set; }
        /// <summary>
        /// Залишок на початок року капітал у дооцінках
        /// </summary>
        public double R4000G4 { get; set; }
        /// <summary>
        /// Залишок на початок року додатковий капітал
        /// </summary>
        public double R4000G5 { get; set; }
        /// <summary>
        /// Залишок на початок року резервний капітал
        /// </summary>
        public double R4000G6 { get; set; }
        /// <summary>
        /// Залишок на початок року нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4000G7 { get; set; }
        /// <summary>
        /// Залишок на початок року неоплачений капітал
        /// </summary>
        public double R4000G8 { get; set; }
        /// <summary>
        /// Залишок на початок року вилучений капітал
        /// </summary>
        public double R4000G9 { get; set; }
        /// <summary>
        /// Залишок на початок року всього
        /// </summary>
        public double R4000G10 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики  зареєстрований (пайовий) капітал
        /// </summary>
        public double R4005G3 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики  капітал у дооцінках
        /// </summary>
        public double R4005G4 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики додатковий капітал
        /// </summary>
        public double R4005G5 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики  резервний капітал
        /// </summary>
        public double R4005G6 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики  нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4005G7 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики  неоплачений капітал
        /// </summary>
        public double R4005G8 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики вилучений капітал
        /// </summary>
        public double R4005G9 { get; set; }
        /// <summary>
        /// Коригування: Зміна облікової політики всього
        /// </summary>
        public double R4005G10 { get; set; }
        /// <summary>
        /// Виправлення помилок зареєстрований (пайовий) капітал
        /// </summary>
        public double R4010G3 { get; set; }
        /// <summary>
        /// Виправлення помилок  капітал у дооцінках
        /// </summary>
        public double R4010G4 { get; set; }
        /// <summary>
        /// Виправлення помилок додатковий капітал
        /// </summary>
        public double R4010G5 { get; set; }
        /// <summary>
        /// Виправлення помилок  резервний капітал
        /// </summary>
        public double R4010G6 { get; set; }
        /// <summary>
        /// Виправлення помилок  нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4010G7 { get; set; }
        /// <summary>
        /// Виправлення помилок  неоплачений капітал
        /// </summary>
        public double R4010G8 { get; set; }
        /// <summary>
        /// Виправлення помилок вилучений капітал
        /// </summary>
        public double R4010G9 { get; set; }
        /// <summary>
        /// Виправлення помилок всього
        /// </summary>
        public double R4010G10 { get; set; }
        /// <summary>
        /// Інші зміни  зареєстрований (пайовий) капітал
        /// </summary>
        public double R4090G3 { get; set; }
        /// <summary>
        /// Інші зміни  капітал у дооцінках
        /// </summary>
        public double R4090G4 { get; set; }
        /// <summary>
        /// Інші зміни додатковий капітал
        /// </summary>
        public double R4090G5 { get; set; }
        /// <summary>
        /// Інші зміни  резервний капітал
        /// </summary>
        public double R4090G6 { get; set; }
        /// <summary>
        /// Інші зміни  нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4090G7 { get; set; }
        /// <summary>
        /// Інші зміни  неоплачений капітал
        /// </summary>
        public double R4090G8 { get; set; }
        /// <summary>
        /// Інші зміни вилучений капітал
        /// </summary>
        public double R4090G9 { get; set; }
        /// <summary>
        /// Інші зміни всього
        /// </summary>
        public double R4090G10 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року зареєстрований (пайовий) капітал
        /// </summary>
        public double R4095G3 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року капітал у дооцінках
        /// </summary>
        public double R4095G4 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року додатковий капітал
        /// </summary>
        public double R4095G5 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року резервний капітал
        /// </summary>
        public double R4095G6 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4095G7 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року неоплачений капітал
        /// </summary>
        public double R4095G8 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року вилучений капітал
        /// </summary>
        public double R4095G9 { get; set; }
        /// <summary>
        /// Скоригований залишок на початок року всього
        /// </summary>
        public double R4095G10 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період зареєстрований (пайовий) капітал
        /// </summary>
        public double R4100G3 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період капітал у дооцінках
        /// </summary>
        public double R4100G4 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період додатковий капітал
        /// </summary>
        public double R4100G5 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період резервний капітал
        /// </summary>
        public double R4100G6 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4100G7 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період неоплачений капітал
        /// </summary>
        public double R4100G8 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період вилучений капітал
        /// </summary>
        public double R4100G9 { get; set; }
        /// <summary>
        /// Чистий прибуток (збиток) за звітний період всього
        /// </summary>
        public double R4100G10 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період  зареєстрований (пайовий) капітал
        /// </summary>
        public double R4110G3 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період  капітал у дооцінках
        /// </summary>
        public double R4110G4 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період додатковий капітал
        /// </summary>
        public double R4110G5 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період  резервний капітал
        /// </summary>
        public double R4110G6 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період  нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4110G7 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період  неоплачений капітал
        /// </summary>
        public double R4110G8 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період  вилучений капітал
        /// </summary>
        public double R4110G9 { get; set; }
        /// <summary>
        /// Інший сукупний дохід за звітний період  всього
        /// </summary>
        public double R4110G10 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) зареєстрований (пайовий) капітал
        /// </summary>
        public double R4200G3 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) капітал у дооцінках
        /// </summary>
        public double R4200G4 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) додатковий капітал
        /// </summary>
        public double R4200G5 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) резервний капітал
        /// </summary>
        public double R4200G6 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4200G7 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) неоплачений капітал
        /// </summary>
        public double R4200G8 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) вилучений капітал
        /// </summary>
        public double R4200G9 { get; set; }
        /// <summary>
        /// Розподіл прибутку: виплати власникам (девіденди) всього
        /// </summary>
        public double R4200G10 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу зареєстрований (пайовий) капітал
        /// </summary>
        public double R4205G3 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу капітал у дооцінках
        /// </summary>
        public double R4205G4 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу додатковий капітал
        /// </summary>
        public double R4205G5 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу резервний капітал
        /// </summary>
        public double R4205G6 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4205G7 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу неоплачений капітал
        /// </summary>
        public double R4205G8 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу вилучений капітал
        /// </summary>
        public double R4205G9 { get; set; }
        /// <summary>
        /// Спрямування прибутку до зареєстованого капіталу всього
        /// </summary>
        public double R4205G10 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу зареєстрований (пайовий) капітал
        /// </summary>
        public double R4210G3 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу капітал у дооцінках
        /// </summary>
        public double R4210G4 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу додатковий капітал
        /// </summary>
        public double R4210G5 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу резервний капітал
        /// </summary>
        public double R4210G6 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4210G7 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу неоплачений капітал
        /// </summary>
        public double R4210G8 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу вилучений капітал
        /// </summary>
        public double R4210G9 { get; set; }
        /// <summary>
        /// Відрахування до резеврвного капіталу всього
        /// </summary>
        public double R4210G10 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства зареєстрований (пайовий) капітал
        /// </summary>
        public double R4215G3 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства капітал у дооцінках
        /// </summary>
        public double R4215G4 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства додатковий капітал
        /// </summary>
        public double R4215G5 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства резервний капітал
        /// </summary>
        public double R4215G6 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4215G7 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства неоплачений капітал
        /// </summary>
        public double R4215G8 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства вилучений капітал
        /// </summary>
        public double R4215G9 { get; set; }
        /// <summary>
        /// Сума чистого прибутку, належна до бюджету відповідно до законодавства всього
        /// </summary>
        public double R4215G10 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів зареєстрований (пайовий) капітал
        /// </summary>
        public double R4220G3 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів капітал у дооцінках
        /// </summary>
        public double R4220G4 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів додатковий капітал
        /// </summary>
        public double R4220G5 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів резервний капітал
        /// </summary>
        public double R4220G6 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4220G7 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів неоплачений капітал
        /// </summary>
        public double R4220G8 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів вилучений капітал
        /// </summary>
        public double R4220G9 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на творення спеціальних (цільових) фондів всього
        /// </summary>
        public double R4220G10 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення зареєстрований (пайовий) капітал
        /// </summary>
        public double R4225G3 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення капітал у дооцінках
        /// </summary>
        public double R4225G4 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення додатковий капітал
        /// </summary>
        public double R4225G5 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення резервний капітал
        /// </summary>
        public double R4225G6 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4225G7 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення неоплачений капітал
        /// </summary>
        public double R4225G8 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення вилучений капітал
        /// </summary>
        public double R4225G9 { get; set; }
        /// <summary>
        /// Сума чистого прибутку на матеріальне заохочення всього
        /// </summary>
        public double R4225G10 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу зареєстрований (пайовий) капітал
        /// </summary>
        public double R4240G3 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу капітал у дооцінках
        /// </summary>
        public double R4240G4 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу додатковий капітал
        /// </summary>
        public double R4240G5 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу резервний капітал
        /// </summary>
        public double R4240G6 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4240G7 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу неоплачений капітал
        /// </summary>
        public double R4240G8 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу вилучений капітал
        /// </summary>
        public double R4240G9 { get; set; }
        /// <summary>
        /// Внески учасників: внески до капіталу всього
        /// </summary>
        public double R4240G10 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу зареєстрований (пайовий) капітал
        /// </summary>
        public double R4245G3 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу капітал у дооцінках
        /// </summary>
        public double R4245G4 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу додатковий капітал
        /// </summary>
        public double R4245G5 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу резервний капітал
        /// </summary>
        public double R4245G6 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4245G7 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу неоплачений капітал
        /// </summary>
        public double R4245G8 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу вилучений капітал
        /// </summary>
        public double R4245G9 { get; set; }
        /// <summary>
        /// Погашення заборгованості з капіталу всього
        /// </summary>
        public double R4245G10 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) зареєстрований (пайовий) капітал
        /// </summary>
        public double R4260G3 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) капітал у дооцінках
        /// </summary>
        public double R4260G4 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) додатковий капітал
        /// </summary>
        public double R4260G5 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) резервний капітал
        /// </summary>
        public double R4260G6 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4260G7 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) неоплачений капітал
        /// </summary>
        public double R4260G8 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) вилучений капітал
        /// </summary>
        public double R4260G9 { get; set; }
        /// <summary>
        /// Вилучення капіталу: викуп акцій (цасток) всього
        /// </summary>
        public double R4260G10 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) зареєстрований (пайовий) капітал
        /// </summary>
        public double R4265G3 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) капітал у дооцінках
        /// </summary>
        public double R4265G4 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) додатковий капітал
        /// </summary>
        public double R4265G5 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) резервний капітал
        /// </summary>
        public double R4265G6 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4265G7 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) неоплачений капітал
        /// </summary>
        public double R4265G8 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) вилучений капітал
        /// </summary>
        public double R4265G9 { get; set; }
        /// <summary>
        /// Перепродаж викуплених акцій (часток) всього
        /// </summary>
        public double R4265G10 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) зареєстрований (пайовий) капітал
        /// </summary>
        public double R4270G3 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) капітал у дооцінках
        /// </summary>
        public double R4270G4 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) додатковий капітал
        /// </summary>
        public double R4270G5 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) резервний капітал
        /// </summary>
        public double R4270G6 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4270G7 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) неоплачений капітал
        /// </summary>
        public double R4270G8 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) вилучений капітал
        /// </summary>
        public double R4270G9 { get; set; }
        /// <summary>
        /// Анулювання викуплених акцій (часток) всього
        /// </summary>
        public double R4270G10 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі зареєстрований (пайовий) капітал
        /// </summary>
        public double R4275G3 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі капітал у дооцінках
        /// </summary>
        public double R4275G4 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі додатковий капітал
        /// </summary>
        public double R4275G5 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі резервний капітал
        /// </summary>
        public double R4275G6 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4275G7 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі неоплачений капітал
        /// </summary>
        public double R4275G8 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі вилучений капітал
        /// </summary>
        public double R4275G9 { get; set; }
        /// <summary>
        /// Вилучення частки в капіталі всього
        /// </summary>
        public double R4275G10 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій зареєстрований (пайовий) капітал
        /// </summary>
        public double R4280G3 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій капітал у дооцінках
        /// </summary>
        public double R4280G4 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій додатковий капітал
        /// </summary>
        public double R4280G5 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій резервний капітал
        /// </summary>
        public double R4280G6 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4280G7 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій неоплачений капітал
        /// </summary>
        public double R4280G8 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій вилучений капітал
        /// </summary>
        public double R4280G9 { get; set; }
        /// <summary>
        /// Зменшення номінальної вартості акцій всього
        /// </summary>
        public double R4280G10 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі зареєстрований (пайовий) капітал
        /// </summary>
        public double R4290G3 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі капітал у дооцінках
        /// </summary>
        public double R4290G4 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі додатковий капітал
        /// </summary>
        public double R4290G5 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі резервний капітал
        /// </summary>
        public double R4290G6 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4290G7 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі неоплачений капітал
        /// </summary>
        public double R4290G8 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі вилучений капітал
        /// </summary>
        public double R4290G9 { get; set; }
        /// <summary>
        /// Інші зміни в капіталі всього
        /// </summary>
        public double R4290G10 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві зареєстрований (пайовий) капітал
        /// </summary>
        public double R4291G3 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві капітал у дооцінках
        /// </summary>
        public double R4291G4 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві додатковий капітал
        /// </summary>
        public double R4291G5 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві резервний капітал
        /// </summary>
        public double R4291G6 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4291G7 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві неоплачений капітал
        /// </summary>
        public double R4291G8 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві вилучений капітал
        /// </summary>
        public double R4291G9 { get; set; }
        /// <summary>
        /// Придбання (продаж) неконтрольованої частки в дочірньому підприємстві всього
        /// </summary>
        public double R4291G10 { get; set; }
        /// <summary>
        /// Разом змін у капіталі зареєстрований (пайовий) капітал
        /// </summary>
        public double R4295G3 { get; set; }
        /// <summary>
        /// Разом змін у капіталі капітал у дооцінках
        /// </summary>
        public double R4295G4 { get; set; }
        /// <summary>
        /// Разом змін у капіталі додатковий капітал
        /// </summary>
        public double R4295G5 { get; set; }
        /// <summary>
        /// Разом змін у капіталі резервний капітал
        /// </summary>
        public double R4295G6 { get; set; }
        /// <summary>
        /// Разом змін у капіталі нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4295G7 { get; set; }
        /// <summary>
        /// Разом змін у капіталі неоплачений капітал
        /// </summary>
        public double R4295G8 { get; set; }
        /// <summary>
        /// Разом змін у капіталі вилучений капітал
        /// </summary>
        public double R4295G9 { get; set; }
        /// <summary>
        /// Разом змін у капіталі всього
        /// </summary>
        public double R4295G10 { get; set; }
        /// <summary>
        /// Залишок на кінець року зареєстрований (пайовий) капітал
        /// </summary>
        public double R4300G3 { get; set; }
        /// <summary>
        /// Залишок на кінець року капітал у дооцінках
        /// </summary>
        public double R4300G4 { get; set; }
        /// <summary>
        /// Залишок на кінець року додатковий капітал
        /// </summary>
        public double R4300G5 { get; set; }
        /// <summary>
        /// Залишок на кінець року резервний капітал
        /// </summary>
        public double R4300G6 { get; set; }
        /// <summary>
        /// Залишок на кінець року нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4300G7 { get; set; }
        /// <summary>
        /// Залишок на кінець року неоплачений капітал
        /// </summary>
        public double R4300G8 { get; set; }
        /// <summary>
        /// Залишок на кінець року вилучений капітал
        /// </summary>
        public double R4300G9 { get; set; }
        /// <summary>
        /// Залишок на кінець року всього
        /// </summary>
        public double R4300G10 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів зареєстрований (пайовий) капітал
        /// </summary>
        public double R4111G3 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів капітал у дооцінках
        /// </summary>
        public double R4111G4 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів додатковий капітал
        /// </summary>
        public double R4111G5 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів резервний капітал
        /// </summary>
        public double R4111G6 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4111G7 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів неоплачений капітал
        /// </summary>
        public double R4111G8 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів вилучений капітал
        /// </summary>
        public double R4111G9 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) необоротних активів всього
        /// </summary>
        public double R4111G10 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів зареєстрований (пайовий) капітал
        /// </summary>
        public double R4112G3 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів капітал у дооцінках
        /// </summary>
        public double R4112G4 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів додатковий капітал
        /// </summary>
        public double R4112G5 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів резервний капітал
        /// </summary>
        public double R4112G6 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4112G7 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів неоплачений капітал
        /// </summary>
        public double R4112G8 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів вилучений капітал
        /// </summary>
        public double R4112G9 { get; set; }
        /// <summary>
        /// Дооцінка (уцінка) фінансових інструментів всього
        /// </summary>
        public double R4112G10 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці зареєстрований (пайовий) капітал
        /// </summary>
        public double R4113G3 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці капітал у дооцінках
        /// </summary>
        public double R4113G4 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці додатковий капітал
        /// </summary>
        public double R4113G5 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці резервний капітал
        /// </summary>
        public double R4113G6 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4113G7 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці неоплачений капітал
        /// </summary>
        public double R4113G8 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці вилучений капітал
        /// </summary>
        public double R4113G9 { get; set; }
        /// <summary>
        /// Накопичені курсові різниці всього
        /// </summary>
        public double R4113G10 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств зареєстрований (пайовий) капітал
        /// </summary>
        public double R4114G3 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств капітал у дооцінках
        /// </summary>
        public double R4114G4 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств додатковий капітал
        /// </summary>
        public double R4114G5 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств резервний капітал
        /// </summary>
        public double R4114G6 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4114G7 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств неоплачений капітал
        /// </summary>
        public double R4114G8 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств вилучений капітал
        /// </summary>
        public double R4114G9 { get; set; }
        /// <summary>
        /// Частка іншого сукупного доходу асоційованих і спільних підприємств всього
        /// </summary>
        public double R4114G10 { get; set; }
        /// <summary>
        /// Інший сукупний дохід зареєстрований (пайовий) капітал
        /// </summary>
        public double R4116G3 { get; set; }
        /// <summary>
        /// Інший сукупний дохід капітал у дооцінках
        /// </summary>
        public double R4116G4 { get; set; }
        /// <summary>
        /// Інший сукупний дохід резервний капітал
        /// </summary>
        public double R4116G6 { get; set; }
        /// <summary>
        /// Інший сукупний дохід додатковий капітал
        /// </summary>
        public double R4116G5 { get; set; }
        /// <summary>
        /// Інший сукупний дохід нерозподілений прибуток (непокритий збиток)
        /// </summary>
        public double R4116G7 { get; set; }
        /// <summary>
        /// Інший сукупний дохід неоплачений капітал
        /// </summary>
        public double R4116G8 { get; set; }
        /// <summary>
        /// Інший сукупний дохід вилучений капітал
        /// </summary>
        public double R4116G9 { get; set; }
        /// <summary>
        /// Інший сукупний дохід всього
        /// </summary>
        public double R4116G10 { get; set; }

        /// <summary>
        /// Назва файлу на Blob
        /// </summary>
        public string? FileName { get; set; }
    }
    public class UnitedFinZvitForm5Model
    {
        /// <summary>
        /// Айді
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ідентифікаційний номер платника податків.
        /// </summary>
        public int Tin { get; set; }

        /// <summary>
        /// Форм код
        /// </summary>
        public string? FormCode { get; set; }

        /// <summary>
        /// Назва Форми? "Фінансова звітність малого підприємства"
        /// </summary>
        public string? FormName { get; set; }

        /// <summary>
        /// Місяц періоду звіту
        /// </summary>
        public int PeriodMonth { get; set; }

        /// <summary>
        /// Рік періоду звіту
        /// </summary>
        public int PeriodYear { get; set; }

        /// <summary>
        ///Тип звіту: 0 - загальний звіт, 1 - коригуючий звіт.
        /// </summary>
        public string? CDocType { get; set; }

        /// <summary>
        /// Кількість податкових звітів у файлі.
        /// </summary>
        public string? CDocCnt { get; set; }

        /// <summary>
        /// Код регіону.
        /// </summary>
        public string? CReg { get; set; }

        /// <summary>
        /// Код району.
        /// </summary>
        public string? CRaj { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? PeriodType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Software { get; set; }

        /// <summary>
        /// Дата та час подачі
        /// </summary>
        public string? DGet { get; set; }

        /// <summary>
        /// Дата час подачі звіту
        /// </summary>
        public DateTime? FilingDate { get; set; }

        /// <summary>
        /// Місяць звітності
        /// 
        /// </summary>
        public string? RepNmonth { get; set; }

        /// <summary>
        /// Назва компанії
        /// 
        /// </summary>
        public string? FirmName { get; set; }

        /// <summary>
        /// ЄДРПОУ компанії
        /// 
        /// </summary>
        public string? FirmEdrpou { get; set; }

        /// <summary>
        ///
        /// 
        /// </summary>
        public string? FirmTerr { get; set; }

        /// <summary>
        /// Територія компанії
        /// 
        /// </summary>
        public string? FirmOgu { get; set; }

        /// <summary>
        /// ОГУ компанії
        /// 
        /// </summary>
        public string? FirmSpodu { get; set; }

        /// <summary>
        /// Назва КВЕД компанії
        /// 
        /// </summary>
        public string? FirmKvednm { get; set; }

        /// <summary>
        /// Код КВЕД компанії
        /// 
        /// </summary>
        public string? FirmKved { get; set; }

        /// <summary>
        /// Рік звітності
        /// 
        /// </summary>
        public string? RepNyear { get; set; }

        /// <summary>
        /// ПІБ директора компанії
        /// 
        /// </summary>
        public string? FirmRuk { get; set; }

        /// <summary>
        /// ПІБ головного бухгалтера компанії
        /// 
        /// </summary>
        public string? FirmBuh { get; set; }

        /// <summary>
        /// Код організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfcd { get; set; }

        /// <summary>
        /// Назва організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfnm { get; set; }

        /// <summary>
        /// Останній день звітного періоду
        /// 
        /// </summary>
        public string? LastDay { get; set; }

        /// <summary>
        /// Код області
        /// 
        /// </summary>
        public string? Obl { get; set; }

        /// <summary>
        /// Код району
        /// 
        /// </summary>
        public string? Ray { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>

        public string? KATOTTG { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// первісна (переоцінена) вартість
        /// </summary>
        public double A01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Надійшло за рік
        /// </summary>
        public double C01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Нараховано амортизації за рік
        /// </summary>
        public double H01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Витрати від зменшення корисності
        /// </summary>
        public double I01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L01 { get; set; }

        /// <summary>
        /// Права користування природними ресурсами
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M01 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Надійшло за рік
        /// </summary>
        public double C02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Нараховано амортизації за рік
        /// </summary>
        public double H02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Витрати від зменшення корисності
        /// </summary>
        public double I02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L02 { get; set; }

        /// <summary>
        /// Права користування майном
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M02 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Надійшло за рік
        /// </summary>
        public double C03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Нараховано амортизації за рік
        /// </summary>
        public double H03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Витрати від зменшення корисності
        /// </summary>
        public double I03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L03 { get; set; }

        /// <summary>
        /// Права на комерційні позначення
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M03 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Надійшло за рік
        /// </summary>
        public double C04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Нараховано амортизації за рік
        /// </summary>
        public double H04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Витрати від зменшення корисності
        /// </summary>
        public double I04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L04 { get; set; }

        /// <summary>
        /// Права на об'єкти промислової вартості
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M04 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Надійшло за рік
        /// </summary>
        public double C05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Нараховано амортизації за рік
        /// </summary>
        public double H05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Витрати від зменшення корисності
        /// </summary>
        public double I05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L05 { get; set; }

        /// <summary>
        /// Авторське право та суміжні з ним права
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M05 { get; set; }

        /// <summary>
        /// Групи нематеріальних активів : *ТИП*
        /// *Тип зазначеється ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// 
        /// </summary>
        public string? A6 { get; set; }

        /// <summary>
        /// -(*ТИП*)- = А6 
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Надійшло за рік
        /// </summary>
        public double C06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Нараховано амортизації за рік
        /// </summary>
        public double H06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Витрати від зменшення корисності
        /// </summary>
        public double I06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L06 { get; set; }

        /// <summary>
        ///-(*ТИП*)- = А6 
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M06 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Надійшло за рік
        /// </summary>
        public double C07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Нараховано амортизації за рік
        /// </summary>
        public double H07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Витрати від зменшення корисності
        /// </summary>
        public double I07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L07 { get; set; }

        /// <summary>
        ///Інші нематеріальні активи
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M07 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Надійшло за рік
        /// </summary>
        public double C08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Нараховано амортизації за рік
        /// </summary>
        public double H08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Витрати від зменшення корисності
        /// </summary>
        public double I08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L08 { get; set; }

        /// <summary>
        ///"Групи нематеріальних активів": Разом
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M08 { get; set; }

        /// <summary>
        ///Вартість нематеріальних активів, щодо яких існує обмеженн права власності (з рядка  L08)
        /// 
        /// </summary>
        public double N2 { get; set; }

        /// <summary>
        ///Вартість офотмлених у заставу нематеріальних активів
        /// 
        /// </summary>
        public double N3 { get; set; }

        /// <summary>
        ///Вартість створених підприємством нематеріальних активів
        /// 
        /// </summary>
        public double N4 { get; set; }

        /// <summary>
        ///Вартість нематеріальних активів, отриманих за рахунок цільових асигувань (з рядка С08)
        /// 
        /// </summary>
        public double N5 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Залишок на початок року знос
        /// </summary>
        public double B10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Надійшло за рік
        /// </summary>
        public double C10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Вибуло за рік знос
        /// </summary>
        public double G10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Нараховано амортизації за рік
        /// </summary>
        public double H10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Витрати від зменшення корисності
        /// </summary>
        public double I10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Інші зміни за рік зносу
        /// </summary>
        public double K10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// Залишок на кінець року знос
        /// </summary>
        public double M10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P10 { get; set; }

        /// <summary>
        ///Земельні ділянки
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q10 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Залишок на початок року знос
        /// </summary>
        public double B11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Надійшло за рік
        /// </summary>
        public double C11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Вибуло за рік знос
        /// </summary>
        public double G11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Нараховано амортизації за рік
        /// </summary>
        public double H11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Витрати від зменшення корисності
        /// </summary>
        public double I11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Інші зміни за рік зносу
        /// </summary>
        public double K11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// Залишок на кінець року знос
        /// </summary>
        public double M11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P11 { get; set; }

        /// <summary>
        ///Капітальні витрати на поліпшення земель
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q11 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої 
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Залишок на початок року знос
        /// </summary>
        public double B12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Надійшло за рік
        /// </summary>
        public double C12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Вибуло за рік знос
        /// </summary>
        public double G12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Нараховано амортизації за рік
        /// </summary>
        public double H12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Витрати від зменшення корисності
        /// </summary>
        public double I12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Інші зміни за рік зносу
        /// </summary>
        public double K12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// Залишок на кінець року знос
        /// </summary>
        public double M12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P12 { get; set; }

        /// <summary>
        ///Будинки, споруди та передавальні пристрої
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q12 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Залишок на початок року знос
        /// </summary>
        public double B13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Надійшло за рік
        /// </summary>
        public double C13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Вибуло за рік знос
        /// </summary>
        public double G13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Нараховано амортизації за рік
        /// </summary>
        public double H13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Витрати від зменшення корисності
        /// </summary>
        public double I13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Інші зміни за рік зносу
        /// </summary>
        public double K13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// Залишок на кінець року знос
        /// </summary>
        public double M13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P13 { get; set; }

        /// <summary>
        ///Машини та обладнання
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q13 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Залишок на початок року знос
        /// </summary>
        public double B14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Надійшло за рік
        /// </summary>
        public double C14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Вибуло за рік знос
        /// </summary>
        public double G14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Нараховано амортизації за рік
        /// </summary>
        public double H14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Витрати від зменшення корисності
        /// </summary>
        public double I14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Інші зміни за рік зносу
        /// </summary>
        public double K14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// Залишок на кінець року знос
        /// </summary>
        public double M14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P14 { get; set; }

        /// <summary>
        ///Транспортні засоби
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q14 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Залишок на початок року знос
        /// </summary>
        public double B15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Надійшло за рік
        /// </summary>
        public double C15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Вибуло за рік знос
        /// </summary>
        public double G15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Нараховано амортизації за рік
        /// </summary>
        public double H15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Витрати від зменшення корисності
        /// </summary>
        public double I15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Інші зміни за рік зносу
        /// </summary>
        public double K15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// Залишок на кінець року знос
        /// </summary>
        public double M15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P15 { get; set; }

        /// <summary>
        ///Інструменди, приладни, інвентар(меблі)
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q15 { get; set; }

        /// <summary>
        ///Тварини
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A16 { get; set; }

        /// <summary>
        ///Тварини
        /// Залишок на початок року знос
        /// </summary>
        public double B16 { get; set; }

        /// <summary>
        ///Тварини
        /// Надійшло за рік
        /// </summary>
        public double C16 { get; set; }

        /// <summary>
        ///Тварини
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D16 { get; set; }

        /// <summary>
        ///Тварини
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E16 { get; set; }

        /// <summary>
        ///Тварини
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F16 { get; set; }

        /// <summary>
        ///Тварини
        /// Вибуло за рік знос
        /// </summary>
        public double G16 { get; set; }

        /// <summary>
        ///Тварини
        /// Нараховано амортизації за рік
        /// </summary>
        public double H16 { get; set; }

        /// <summary>
        ///Тварини
        /// Витрати від зменшення корисності
        /// </summary>
        public double I16 { get; set; }

        /// <summary>
        ///Тварини
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J16 { get; set; }

        /// <summary>
        ///Тварини
        /// Інші зміни за рік зносу
        /// </summary>
        public double K16 { get; set; }

        /// <summary>
        ///Тварини
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L16 { get; set; }

        /// <summary>
        ///Тварини
        /// Залишок на кінець року знос
        /// </summary>
        public double M16 { get; set; }

        /// <summary>
        ///Тварини
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N16 { get; set; }

        /// <summary>
        ///Тварини
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O16 { get; set; }

        /// <summary>
        ///Тварини
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P16 { get; set; }

        /// <summary>
        ///Тварини
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q16 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Залишок на початок року знос
        /// </summary>
        public double B17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Надійшло за рік
        /// </summary>
        public double C17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Вибуло за рік знос
        /// </summary>
        public double G17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Нараховано амортизації за рік
        /// </summary>
        public double H17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Витрати від зменшення корисності
        /// </summary>
        public double I17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Інші зміни за рік зносу
        /// </summary>
        public double K17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// Залишок на кінець року знос
        /// </summary>
        public double M17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P17 { get; set; }

        /// <summary>
        ///Багаторічні насадження
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q17 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Залишок на початок року знос
        /// </summary>
        public double B18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Надійшло за рік
        /// </summary>
        public double C18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Вибуло за рік знос
        /// </summary>
        public double G18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Нараховано амортизації за рік
        /// </summary>
        public double H18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Витрати від зменшення корисності
        /// </summary>
        public double I18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Інші зміни за рік зносу
        /// </summary>
        public double K18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// Залишок на кінець року знос
        /// </summary>
        public double M18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P18 { get; set; }

        /// <summary>
        ///Інші основні засоби
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q18 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Залишок на початок року знос
        /// </summary>
        public double B19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Надійшло за рік
        /// </summary>
        public double C19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Вибуло за рік знос
        /// </summary>
        public double G19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Нараховано амортизації за рік
        /// </summary>
        public double H19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Витрати від зменшення корисності
        /// </summary>
        public double I19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Інші зміни за рік зносу
        /// </summary>
        public double K19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// Залишок на кінець року знос
        /// </summary>
        public double M19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P19 { get; set; }

        /// <summary>
        ///Бібліотечні Фонди
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q19 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Залишок на початок року знос
        /// </summary>
        public double B20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Надійшло за рік
        /// </summary>
        public double C20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Вибуло за рік знос
        /// </summary>
        public double G20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Нараховано амортизації за рік
        /// </summary>
        public double H20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Витрати від зменшення корисності
        /// </summary>
        public double I20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Інші зміни за рік зносу
        /// </summary>
        public double K20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// Залишок на кінець року знос
        /// </summary>
        public double M20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P20 { get; set; }

        /// <summary>
        ///Малоцінні необоротні матеріальні активи
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q20 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Залишок на початок року знос
        /// </summary>
        public double B21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Надійшло за рік
        /// </summary>
        public double C21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Вибуло за рік знос
        /// </summary>
        public double G21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Нараховано амортизації за рік
        /// </summary>
        public double H21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Витрати від зменшення корисності
        /// </summary>
        public double I21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Інші зміни за рік зносу
        /// </summary>
        public double K21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// Залишок на кінець року знос
        /// </summary>
        public double M21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P21 { get; set; }

        /// <summary>
        ///Тимчасові (нетитульні) споруди
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q21 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Залишок на початок року знос
        /// </summary>
        public double B22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Надійшло за рік
        /// </summary>
        public double C22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E22 { get; set; }

        /// <summary>
        ///Природні ресурси
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Вибуло за рік знос
        /// </summary>
        public double G22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Нараховано амортизації за рік
        /// </summary>
        public double H22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Витрати від зменшення корисності
        /// </summary>
        public double I22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Інші зміни за рік зносу
        /// </summary>
        public double K22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// Залишок на кінець року знос
        /// </summary>
        public double M22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P22 { get; set; }

        /// <summary>
        ///Природні ресурси
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q22 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Залишок на початок року знос
        /// </summary>
        public double B23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Надійшло за рік
        /// </summary>
        public double C23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Вибуло за рік знос
        /// </summary>
        public double G23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Нараховано амортизації за рік
        /// </summary>
        public double H23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Витрати від зменшення корисності
        /// </summary>
        public double I23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Інші зміни за рік зносу
        /// </summary>
        public double K23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// Залишок на кінець року знос
        /// </summary>
        public double M23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P23 { get; set; }

        /// <summary>
        ///Інвентарна тара
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q23 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Залишок на початок року знос
        /// </summary>
        public double B24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Надійшло за рік
        /// </summary>
        public double C24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E24 { get; set; }

        /// <summary>
        ///Предмети прокату
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Вибуло за рік знос
        /// </summary>
        public double G24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Нараховано амортизації за рік
        /// </summary>
        public double H24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Витрати від зменшення корисності
        /// </summary>
        public double I24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Інші зміни за рік зносу
        /// </summary>
        public double K24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// Залишок на кінець року знос
        /// </summary>
        public double M24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P24 { get; set; }

        /// <summary>
        ///Предмети прокату
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q24 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Залишок на початок року знос
        /// </summary>
        public double B25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Надійшло за рік
        /// </summary>
        public double C25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Вибуло за рік знос
        /// </summary>
        public double G25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Нараховано амортизації за рік
        /// </summary>
        public double H25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Витрати від зменшення корисності
        /// </summary>
        public double I25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Інші зміни за рік зносу
        /// </summary>
        public double K25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// Залишок на кінець року знос
        /// </summary>
        public double M25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P25 { get; set; }

        /// <summary>
        ///Інші необоротні матеріальні активи
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q25 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Залишок на початок року знос
        /// </summary>
        public double B26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Надійшло за рік
        /// </summary>
        public double C26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        ///  Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Вибуло за рік знос
        /// </summary>
        public double G26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Нараховано амортизації за рік
        /// </summary>
        public double H26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Витрати від зменшення корисності
        /// </summary>
        public double I26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Інші зміни за рік зносу
        /// </summary>
        public double K26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// Залишок на кінець року знос
        /// </summary>
        public double M26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P26 { get; set; }

        /// <summary>
        ///Групи основних засобів "Разом"
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q26 { get; set; }

        /// <summary>
        ///Вартість основних засобів, щодо яких існують передбачені чинним законодавством омбеження 
        ///права власності (рядок Р26)
        /// 
        /// </summary>
        public double N6 { get; set; }

        /// <summary>
        ///Вартість оформлення у заставу основних засобів
        /// 
        /// </summary>
        public double N7 { get; set; }

        /// <summary>
        ///Залишкова вартість основних засобів, що тимчасово 
        ///не використовується (консервація, реконструкція тощо)
        /// 
        /// </summary>
        public double N8 { get; set; }

        /// <summary>
        ///Первісна (переоцінена) вартість повністю амортизованих основних засобів
        /// 
        /// </summary>
        public double N9 { get; set; }

        /// <summary>
        ///Вартість основних засобів призначених для продажу (рядок F26)
        /// 
        /// </summary>
        public double N101 { get; set; }

        /// <summary>
        ///Вартість основних засобів, придбаних за рахунок цільового фінансування (рядок С26)
        /// 
        /// </summary>
        public double N111 { get; set; }

        /// <summary>
        ///Вартість основних засобів, що взяті в операційну оренду
        /// 
        /// </summary>
        public double N121 { get; set; }

        /// <summary>
        ///Знос основних засобів, щодо яких інсують обмеження права власності(рядок М26)
        /// 
        /// </summary>
        public double N122 { get; set; }

        /// <summary>
        ///Капітальне будівництво
        /// За рік
        /// </summary>
        public double A28 { get; set; }

        /// <summary>
        ///Капітальне будівництво
        /// На кінець року
        /// </summary>
        public double B28 { get; set; }

        /// <summary>
        ///Придбання (виготовлення) основних засобів
        /// За рік
        /// </summary>
        public double A29 { get; set; }

        /// <summary>
        ///Придбання (виготовлення) основних засобів
        /// На кінець року
        /// </summary>
        public double B29 { get; set; }

        /// <summary>
        ///Придбання (виготовлення) інших необоротних матеріальних активів
        /// За рік
        /// </summary>
        public double A30 { get; set; }

        /// <summary>
        ///Придбання (виготовлення) інших необоротних матеріальних активів
        /// На кінець року
        /// </summary>
        public double B30 { get; set; }

        /// <summary>
        ///Придбання (створення) нематеріальних активів
        /// За рік
        /// </summary>
        public double A31 { get; set; }

        /// <summary>
        ///Придбання (створення) нематеріальних активів
        /// На кінець року
        /// </summary>
        public double B31 { get; set; }

        /// <summary>
        ///Придбання (вирощування) довсострокових біологічних активів
        /// За рік
        /// </summary>
        public double A32 { get; set; }

        /// <summary>
        ///Придбання (вирощування) довсострокових біологічних активів
        /// На кінець року
        /// </summary>
        public double B32 { get; set; }

        /// <summary>
        ///Інші показники
        /// За рік
        /// </summary>
        public double A33 { get; set; }

        /// <summary>
        ///Інші показники
        /// На кінець року
        /// </summary>
        public double B33 { get; set; }

        /// <summary>
        ///Показники разом
        /// За рік
        /// </summary>
        public double A34 { get; set; }

        /// <summary>
        ///Показники разом
        /// На кінець року
        /// </summary>
        public double B34 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в: 
        ///частки і паї у статутному капіталі інших підприємств
        /// За рік
        /// </summary>
        public double A35 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в: 
        ///частки і паї у статутному капіталі інших підприємств
        /// На кінець року довгострокові
        /// </summary>
        public double B35 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в: 
        ///частки і паї у статутному капіталі інших підприємств
        /// На кінець року поточні
        /// </summary>
        public double C35 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в:
        ///дочірні підприємства
        /// За рік
        /// </summary>
        public double A36 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в:
        ///дочірні підприємства
        /// На кінець року довгострокові
        /// </summary>
        public double B36 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в:
        ///дочірні підприємства
        /// На кінець року поточні
        /// </summary>
        public double C36 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в:
        ///спільну діяльність
        /// За рік
        /// </summary>
        public double A37 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в:
        ///спільну діяльність
        /// На кінець року довгострокові
        /// </summary>
        public double B37 { get; set; }

        /// <summary>
        ///Фінансові інвестиції за методом участі в капіталі в:
        ///спільну діяльність
        /// На кінець року поточні
        /// </summary>
        public double C37 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///частки і паї у статутному капіталіінших підприємств
        /// За рік
        /// </summary>
        public double A38 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///частки і паї у статутному капіталі інших підприємств
        /// На кінець року довгострокові
        /// </summary>
        public double B38 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///частки і паї у статутному капіталі інших підприємств
        /// На кінець року поточні
        /// </summary>
        public double C38 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///акції
        /// За рік
        /// </summary>
        public double A39 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///акції
        /// На кінець року довгострокові
        /// </summary>
        public double B39 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///акції На кінець року поточні
        /// </summary>
        public double C39 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///облігації За рік
        /// </summary>
        public double A40 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        ///облігації
        /// На кінець року довгострокові
        /// </summary>
        public double B40 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в: облігації
        ///На кінець року поточні
        /// </summary>
        public double C40 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        /// Інші За рік
        /// 
        /// </summary>
        public double A41 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        /// Інші
        /// На кінець року довгострокові
        /// </summary>
        public double B41 { get; set; }

        /// <summary>
        ///Інші фінансвоі інвестиції в:
        /// Інші
        /// На кінець року поточні
        /// </summary>
        public double C41 { get; set; }

        /// <summary>
        ///Показники інвестицій "Разом" (з А35 + по А42(включно))
        /// За рік
        /// </summary>
        public double A42 { get; set; }

        /// <summary>
        ///Показники інвестицій "Разом" (з В35 + по В42(включно))
        /// На кінець року довгострокові
        /// </summary>
        public double B42 { get; set; }

        /// <summary>
        ///Показники інвестицій "Разом" (з С35 + по С42 (включно))
        /// На кінець року поточні
        /// </summary>
        public double C42 { get; set; }

        /// <summary>
        /// З рядка 1035 графа 4 Балансу (звіту про фінансовий стан)
        /// Інші довгострокові фінансові інвестиції відображені: 
        /// за собівартістю
        /// 
        /// </summary>
        public double N131 { get; set; }

        /// <summary>
        /// З рядка 1035 графа 4 Балансу (звіту про фінансовий стан)
        ///Інші довгострокові фінансові інвестиції відображені: 
        ///за справедливою вартістю
        /// 
        /// </summary>
        public double N141 { get; set; }

        /// <summary>
        /// З рядка 1035 графа 4 Балансу (звіту про фінансовий стан)
        ///Інші довгострокові фінансові інвестиції відображені: 
        ///за амортизованою собівартістю
        /// 
        /// </summary>
        public double N151 { get; set; }

        /// <summary>
        ///З рядка 1160 графа 4 Балансу(звіту про фінансовий стан)
        ///Поточні фінансові інвестиції відображення:
        /// за собівартістю
        /// 
        /// </summary>
        public double N161 { get; set; }

        /// <summary>
        ///З рядка 1160 графа 4 Балансу(звіту про фінансовий стан)
        ///Поточні фінансові інвестиції відображення:
        ///за справедливою вартістю
        /// 
        /// </summary>
        public double N171 { get; set; }

        /// <summary>
        ///З рядка 1160 графа 4 Балансу(звіту про фінансовий стан)
        ///Поточні фінансові інвестиції відображення:
        ///за амотризованою собівартісю
        /// 
        /// </summary>
        public double N181 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Операційна оренда активів
        /// Доходи
        /// </summary>
        public double A44 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Операційна оренда активів Витрати
        /// 
        /// </summary>
        public double B44 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Операційна курсова різниця
        /// Доходи
        /// </summary>
        public double A45 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Операційна курсова різниця
        /// Витрати
        /// </summary>
        public double B45 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Реалізація іншихоборотних активів 
        /// Доходи
        /// </summary>
        public double A46 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Реалізація іншихоборотних активів 
        /// Витрати
        /// </summary>
        public double B46 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Штрафи, пені, неустойки
        /// Доходи
        /// </summary>
        public double A47 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///Штрафи, пені, неустойки
        /// Витрати
        /// </summary>
        public double B47 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///утримання об'єктів житлово-комунального і соціально-культурного призначення
        /// Доходи
        /// </summary>
        public double A48 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати:
        ///утримання об'єктів житлово-комунального і соціально-культурного призначення
        /// Витрати
        /// </summary>
        public double B48 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати
        /// Доходи
        /// </summary>
        public double A49 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати
        /// Витрати
        /// </summary>
        public double B49 { get; set; }

        /// <summary>
        ///Доходи і витрати від участі в капіталі за інвестиціями в:
        ///асоційовані підприємства
        /// Доходи
        /// </summary>
        public double A50 { get; set; }

        /// <summary>
        ///Доходи і витрати від участі в капіталі за інвестиціями в:
        ///асоційовані підприємства
        /// Витрати
        /// </summary>
        public double B50 { get; set; }

        /// <summary>
        ///Доходи і витрати від участі в капіталі за інвестиціями в:
        ///дочірні підприємства
        /// Доходи
        /// </summary>
        public double A51 { get; set; }

        /// <summary>
        ///Доходи і витрати від участі в капіталі за інвестиціями в:
        ///дочірні підприємства
        /// Витрати
        /// </summary>
        public double B51 { get; set; }

        /// <summary>
        ///Доходи і витрати від участі в капіталі за інвестиціями в:
        ///спільну діяльність
        /// Доходи
        /// </summary>
        public double A52 { get; set; }

        /// <summary>
        ///Доходи і витрати від участі в капіталі за інвестиціями в:
        ///спільну діяльність
        /// Витрати
        /// </summary>
        public double B52 { get; set; }

        /// <summary>
        ///Інші фінансові доходи і витрати 
        ///Дивіденди
        /// Доходи
        /// </summary>
        public double A53 { get; set; }

        /// <summary>
        ///Інші фінансові доходи і витрати 
        ///Проценти
        /// Витрати
        /// </summary>
        public double B54 { get; set; }

        /// <summary>
        /// Інші фінансові доходи і витрати 
        ///Фінансова оренда активів
        /// Доходи
        /// </summary>
        public double A55 { get; set; }

        /// <summary>
        ///Інші фінансові доходи і витрати 
        ///Фінансова оренда активів
        /// Витрати
        /// </summary>
        public double B55 { get; set; }

        /// <summary>
        ///Інші фінансові доходи і витрати 
        /// Доходи
        /// </summary>
        public double A56 { get; set; }

        /// <summary>
        ///Інші фінансові доходи і витрати 
        /// Витрати
        /// </summary>
        public double B56 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Реалізація фінансових інвестицій
        /// Доходи
        /// </summary>
        public double A57 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Реалізація фінансових інвестицій
        /// Витрати
        /// </summary>
        public double B57 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Доходи від об'єднання підприємств
        /// Доходи
        /// </summary>
        public double A58 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Доходи від об'єднання відприємств
        /// Витрати
        /// </summary>
        public double B58 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Результат оцінки корисності
        /// Доходи
        /// </summary>
        public double A59 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Результат оцінки корисності
        /// Витрати
        /// </summary>
        public double B59 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Неопераційна курсова різниця
        /// Доходи
        /// </summary>
        public double A60 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Неопераційна курсова різниця
        /// Витрати
        /// </summary>
        public double B60 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Бесоплатно одержані активи
        /// Доходи
        /// </summary>
        public double A61 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        ///Списання необоротних активів
        /// Витрати
        /// </summary>
        public double B62 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        /// Доходи
        /// </summary>
        public double A63 { get; set; }

        /// <summary>
        ///Інші доходи і витрати:
        /// Витрати
        /// </summary>
        public double B63 { get; set; }

        /// <summary>
        ///Товарообмінні (бартерні) операції з продукцією (товарами, роботами, послугами) 
        /// 
        /// </summary>
        public double N191 { get; set; }

        /// <summary>
        ///Частка доходу від реалізації продукції (товарів, робіт, послуг) 
        ///за товарообмінними (бартерними) контрактами з по'вязаними сторонами
        ///*ЗНАЧЕННЯ ВІДОБРАЖАЄТЬСЯ У "%"
        /// 
        /// </summary>
        public double N201 { get; set; }

        /// <summary>
        ///Готівка на кінець року
        /// </summary>
        public double A64 { get; set; }

        /// <summary>
        ///Поточний розрахунок у банку на кінець року
        /// 
        /// </summary>
        public double A65 { get; set; }

        /// <summary>
        /// Інші рахунки в банку (акредитиви, чекові книжки) на кінець року
        /// 
        /// </summary>
        public double A66 { get; set; }

        /// <summary>
        ///Грошові кошти в дорозі на кінець року
        /// 
        /// </summary>
        public double A67 { get; set; }

        /// <summary>
        ///Еквіваленти грошових коштів на кінець року
        /// 
        /// </summary>
        public double A68 { get; set; }

        /// <summary>
        ///Разом "Грошові активи" на кінець року
        /// 
        /// </summary>
        public double A69 { get; set; }

        /// <summary>
        ///З рядка 1990 графи 4 Балансу (Звіту про фінансвоий стан) 
        ///Грошові кошти, використання яких обмежено
        /// 
        /// </summary>
        public double N211 { get; set; }

        /// <summary>
        ///Забезпечення на виплату відпусток працівникам
        /// Залишок на початок року
        /// </summary>
        public double A71 { get; set; }

        /// <summary>
        ///Забезпечення на виплату відпусток працівникам
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B71 { get; set; }

        /// <summary>
        ///Забезпечення на виплату відпусток працівникам
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C71 { get; set; }

        /// <summary>
        ///Забезпечення на виплату відпусток працівникам
        /// Використано у звітному році
        /// </summary>
        public double D71 { get; set; }

        /// <summary>
        ///Забезпечення на виплату відпусток працівникам
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E71 { get; set; }

        /// <summary>
        ///Забезпечення на виплату відпусток працівникам
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F71 { get; set; }

        /// <summary>
        ///Забезпечення на виплату відпусток працівникам
        /// 
        /// </summary>
        public double G71 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на додаткове пенсійне забезпечення
        /// Залишок на початок року
        /// </summary>
        public double A72 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на додаткове пенсійне забезпечення
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B72 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на додаткове пенсійне забезпечення
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C72 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на додаткове пенсійне забезпечення
        /// Використано у звітному році
        /// </summary>
        public double D72 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на додаткове пенсійне забезпечення
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E72 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на додаткове пенсійне забезпечення
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F72 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на додаткове пенсійне забезпечення
        /// 
        /// </summary>
        public double G72 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання грантійних зобов'язань
        /// Залишок на початок року
        /// </summary>
        public double A73 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання грантійних зобов'язань
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B73 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання грантійних зобов'язань
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C73 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання грантійних зобов'язань
        /// Використано у звітному році
        /// </summary>
        public double D73 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання грантійних зобов'язань
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E73 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання грантійних зобов'язань
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F73 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання грантійних зобов'язань
        /// 
        /// </summary>
        public double G73 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на реструктурізацію
        /// Залишок на початок року
        /// </summary>
        public double A74 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на реструктурізацію
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B74 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на реструктурізацію
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C74 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на реструктурізацію
        /// Використано у звітному році
        /// </summary>
        public double D74 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на реструктурізацію
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E74 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на реструктурізацію
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F74 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на реструктурізацію
        /// 
        /// </summary>
        public double G74 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання зобов'язань
        ///щодо обтяжливих контрактів
        /// Залишок на початок року
        /// </summary>
        public double A75 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання зобов'язань
        ///щодо обтяжливих контрактів
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B75 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання зобов'язань
        ///щодо обтяжливих контрактів
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C75 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання зобов'язань
        ///щодо обтяжливих контрактів
        /// Використано у звітному році
        /// </summary>
        public double D75 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання зобов'язань
        ///щодо обтяжливих контрактів
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E75 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання зобов'язань
        ///щодо обтяжливих контрактів
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F75 { get; set; }

        /// <summary>
        ///Забезпечення наступних витрат на виконання зобов'язань
        ///щодо обтяжливих контрактів
        /// 
        /// </summary>
        public double G75 { get; set; }

        /// <summary>
        /// *Значення типу в полі ST_1* 
        /// Залишок на початок року
        /// </summary>
        public double A76 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_1* 
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B76 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_1* 
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C76 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_1* 
        /// Використано у звітному році
        /// </summary>
        public double D76 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_1* 
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E76 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_1* 
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F76 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_1* 
        /// 
        /// </summary>
        public double G76 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_2*
        /// Залишок на початок року
        /// </summary>
        public double A77 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_2*
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B77 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_2*
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C77 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_2*
        /// Використано у звітному році
        /// </summary>
        public double D77 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_2*
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E77 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_2*
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F77 { get; set; }

        /// <summary>
        ///*Значення типу в полі ST_2*
        /// 
        /// </summary>
        public double G77 { get; set; }

        /// <summary>
        ///Резерв сумнівних боргів
        /// Залишок на початок року
        /// </summary>
        public double A775 { get; set; }

        /// <summary>
        ///Резерв сумнівних боргів
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B775 { get; set; }

        /// <summary>
        ///Резерв сумнівних боргів
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C775 { get; set; }

        /// <summary>
        ///Резерв сумнівних боргів
        /// Використано у звітному році
        /// </summary>
        public double D775 { get; set; }

        /// <summary>
        ///Резерв сумнівних боргів
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E775 { get; set; }

        /// <summary>
        ///Резерв сумнівних боргів
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F775 { get; set; }

        /// <summary>
        ///Резерв сумнівних боргів
        /// 
        /// </summary>
        public double G775 { get; set; }

        /// <summary>
        ///Види забезпечень і резервів "Разом"
        /// Залишок на початок року
        /// </summary>
        public double A78 { get; set; }

        /// <summary>
        ///Види забезпечень і резервів "Разом"
        /// Збільшення за звітній рік нараховано (створено)
        /// </summary>
        public double B78 { get; set; }

        /// <summary>
        ///Види забезпечень і резервів "Разом"
        /// Збільшення за звітній рік додаткові відрахування
        /// </summary>
        public double C78 { get; set; }

        /// <summary>
        ///Види забезпечень і резервів "Разом"
        /// Використано у звітному році
        /// </summary>
        public double D78 { get; set; }

        /// <summary>
        ///Види забезпечень і резервів "Разом"
        /// Сторновано невикористану суму у звітному році
        /// </summary>
        public double E78 { get; set; }

        /// <summary>
        ///Види забезпечень і резервів "Разом"
        /// Сума очікуваного відшкодування витрат іншою стороною,
        /// що врахована при оцінці забезпечення
        /// </summary>
        public double F78 { get; set; }

        /// <summary>
        ///Види забезпечень і резервів "Разом"
        /// 
        /// </summary>
        public double G78 { get; set; }

        /// <summary>
        ///Сировина і матеріали 
        /// Балансова вартість на кінець року
        /// </summary>
        public double A80 { get; set; }

        /// <summary>
        ///Сировина і матеріали 
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B80 { get; set; }

        /// <summary>
        ///Сировина і матеріали 
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C80 { get; set; }

        /// <summary>
        ///Купівельні напівфабрикати та комплектуючі вироби
        /// Балансова вартість на кінець року
        /// </summary>
        public double A81 { get; set; }

        /// <summary>
        ///Купівельні напівфабрикати та комплектуючі вироби
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B81 { get; set; }

        /// <summary>
        ///Купівельні напівфабрикати та комплектуючі вироби
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C81 { get; set; }

        /// <summary>
        ///Паливо
        /// Балансова вартість на кінець року
        /// </summary>
        public double A82 { get; set; }

        /// <summary>
        ///Паливо
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B82 { get; set; }

        /// <summary>
        ///Паливо
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C82 { get; set; }

        /// <summary>
        ///Тара та тарні матеріали
        /// Балансова вартість на кінець року
        /// </summary>
        public double A83 { get; set; }

        /// <summary>
        ///Тара та тарні матеріали
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B83 { get; set; }

        /// <summary>
        ///Тара та тарні матеріали
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C83 { get; set; }

        /// <summary>
        ///Будівельні матеріали
        /// Балансова вартість на кінець року
        /// </summary>
        public double A84 { get; set; }

        /// <summary>
        ///Будівельні матеріали
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B84 { get; set; }

        /// <summary>
        ///Будівельні матеріали
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C84 { get; set; }

        /// <summary>
        ///Запасні частини
        /// Балансова вартість на кінець року
        /// </summary>
        public double A85 { get; set; }

        /// <summary>
        ///Запасні частини
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B85 { get; set; }

        /// <summary>
        ///Запасні частини
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C85 { get; set; }

        /// <summary>
        ///Матеріали сільськогосподарського призначення
        /// Балансова вартість на кінець року
        /// </summary>
        public double A86 { get; set; }

        /// <summary>
        ///Матеріали сільськогосподарського призначення
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B86 { get; set; }

        /// <summary>
        ///Матеріали сільськогосподарського призначення
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C86 { get; set; }

        /// <summary>
        ///Поточні біологічні активи
        /// Балансова вартість на кінець року
        /// </summary>
        public double A87 { get; set; }

        /// <summary>
        ///Поточні біологічні активи
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B87 { get; set; }

        /// <summary>
        ///Поточні біологічні активи
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C87 { get; set; }

        /// <summary>
        ///Малоцінні та швидкозношувальні предмети
        /// Балансова вартість на кінець року
        /// </summary>
        public double A88 { get; set; }

        /// <summary>
        ///Малоцінні та швидкозношувальні предмети
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B88 { get; set; }

        /// <summary>
        ///Малоцінні та швидкозношувальні предмети
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C88 { get; set; }

        /// <summary>
        ///Незавершене виробництво
        /// Балансова вартість на кінець року
        /// </summary>
        public double A89 { get; set; }

        /// <summary>
        ///Незавершене виробництво
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B89 { get; set; }

        /// <summary>
        ///Незавершене виробництво
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C89 { get; set; }

        /// <summary>
        ///Готова продукція
        /// Балансова вартість на кінець року
        /// </summary>
        public double A90 { get; set; }

        /// <summary>
        ///Готова продукція
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B90 { get; set; }

        /// <summary>
        ///Готова продукція
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C90 { get; set; }

        /// <summary>
        ///Товари
        /// Балансова вартість на кінець року
        /// </summary>
        public double A91 { get; set; }

        /// <summary>
        ///Товари
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B91 { get; set; }

        /// <summary>
        ///Товари
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C91 { get; set; }

        /// <summary>
        ///Запаси "Разом"
        /// Балансова вартість на кінець року
        /// </summary>
        public double A92 { get; set; }

        /// <summary>
        ///Запаси "Разом"
        /// Переоцінка за рік збільшення чистої вартості реалізації
        /// </summary>
        public double B92 { get; set; }

        /// <summary>
        ///Запаси "Разом"
        /// Переоцінка за рік уцінка
        /// </summary>
        public double C92 { get; set; }

        /// <summary>
        ///Балансова вартість запасів відображених за чистою вартістю реалізації (З рядка А92)
        /// 
        /// </summary>
        public double N221 { get; set; }

        /// <summary>
        ///Балансова вартість запасів переданих у переробку
        /// 
        /// </summary>
        public double N231 { get; set; }

        /// <summary>
        ///Балансова вартість запасів  оформлених в заставу
        /// 
        /// </summary>
        public double N241 { get; set; }

        /// <summary>
        ///Балансова вартість запасів переданих на комісію
        /// 
        /// </summary>
        public double N251 { get; set; }

        /// <summary>
        ///Активи на відповідальному зберіганні (позабалансовий рахунок 02)
        /// 
        /// </summary>
        public double N261 { get; set; }

        /// <summary>
        ///Дебіторська заборгованість за товари, роботи, послуги
        /// Всього на цінець року
        /// </summary>
        public double A94 { get; set; }

        /// <summary>
        ///Дебіторська заборгованість за товари, роботи, послуги
        /// у т.ч. за строками непогашення до 12 місяців
        /// </summary>
        public double B94 { get; set; }

        /// <summary>
        ///Дебіторська заборгованість за товари, роботи, послуги
        /// у т.ч. за строками непогашення від 12 до 18 місяців
        /// </summary>
        public double C94 { get; set; }

        /// <summary>
        ///Дебіторська заборгованість за товари, роботи, послуги
        /// у т.ч. за строками непогашення від 18 до 36 місяців
        /// </summary>
        public double D94 { get; set; }

        /// <summary>
        ///Інша поточна дебіторська заборгованість
        /// Всього на цінець року
        /// </summary>
        public double A95 { get; set; }

        /// <summary>
        ///Інша поточна дебіторська заборгованість
        /// у т.ч. за строками непогашення до 12 місяців
        /// </summary>
        public double B95 { get; set; }

        /// <summary>
        ///Інша поточна дебіторська заборгованість
        /// у т.ч. за строками непогашення від 12 до 18 місяців
        /// </summary>
        public double C95 { get; set; }

        /// <summary>
        ///Інша поточна дебіторська заборгованість
        /// у т.ч. за строками непогашення від 18 до 36 місяців
        /// </summary>
        public double D95 { get; set; }

        /// <summary>
        ///Списано у звітному році безнадійної дебіторської заборгованості
        /// 
        /// </summary>
        public double N27 { get; set; }

        /// <summary>
        /// Заборгованість з пов'язаними сторонами (Із рядків А94 і А95)
        /// 
        /// </summary>
        public double N952 { get; set; }

        /// <summary>
        ///Виявлено (списано) за рік нестач і витрат
        /// </summary>
        public double A96 { get; set; }

        /// <summary>
        ///Визнано заборгованістю винних осіб у звітному році
        /// 
        /// </summary>
        public double A97 { get; set; }

        /// <summary>
        ///Сума нестач і втрат, остаточне рішення щодо винуватців, 
        ///за якими на кінець року не прийнято (позабалансовий рахунок 072)
        /// 
        /// </summary>
        public double A98 { get; set; }

        /// <summary>
        ///Дохід за будівельними контрактами за звітний рік
        /// 
        /// </summary>
        public double A111 { get; set; }

        /// <summary>
        ///Заборгованість на кінець звітного року: валова замовників
        /// 
        /// </summary>
        public double A112 { get; set; }

        /// <summary>
        ///Заборгованість на кінець звітного року: валова замовниками
        /// 
        /// </summary>
        public double A113 { get; set; }

        /// <summary>
        ///Заборгованість на кінець звітного року: з авансових отриманих
        /// 
        /// </summary>
        public double A114 { get; set; }

        /// <summary>
        ///Сума затриманих коштів на кінець року
        /// 
        /// </summary>
        public double A115 { get; set; }

        /// <summary>
        ///Вартість виконаних субпідрядниками робіт за незавершеними будівельними контрактами
        /// 
        /// </summary>
        public double A116 { get; set; }

        /// <summary>
        ///Поточний податок на прибуток
        /// 
        /// </summary>
        public double A1210 { get; set; }

        /// <summary>
        ///Відстрочені податкові активи: на початок звітного року
        /// 
        /// </summary>
        public double A1220 { get; set; }

        /// <summary>
        ///Відстрочені податкові активи: на кінець звітного року
        /// 
        /// </summary>
        public double A1225 { get; set; }

        /// <summary>
        ///Відстрочені податкові зобов'язання: на початок звітного року
        /// 
        /// </summary>
        public double A1230 { get; set; }

        /// <summary>
        ///Відстрочені податкові зобов'язання: на кінець звітного року
        /// 
        /// </summary>
        public double A1235 { get; set; }

        /// <summary>
        ///Включено до Звіту про фінансові результати - усього
        /// 
        /// </summary>
        public double A1240 { get; set; }

        /// <summary>
        ///Включено до Звіту про фінансові результати - усього
        ///у тому числі: поточний податок на прибуток
        /// 
        /// </summary>
        public double A1241 { get; set; }

        /// <summary>
        ///Включено до Звіту про фінансові результати - усього
        ///у тому числі: зменшення (збільшення) відтрочених податкових активів
        /// 
        /// </summary>
        public double A1242 { get; set; }

        /// <summary>
        ///Включено до Звіту про фінансові результати - усього
        ///у тому числі: збільшення (зменшення) відстрочення податкових зобов'язань
        /// 
        /// </summary>
        public double A1243 { get; set; }

        /// <summary>
        ///Відображено у складі власного капіталу - усього
        /// 
        /// </summary>
        public double A1250 { get; set; }

        /// <summary>
        ///Відображено у складі власного капіталу - усього
        ///у тому числі: поточний податок на прибуток
        /// 
        /// </summary>
        public double A1251 { get; set; }

        /// <summary>
        ///Відображено у складі власного капіталу - усього
        ///у тому числі: (збільшення) відтрочених податкових активів
        /// 
        /// </summary>
        public double A1252 { get; set; }

        /// <summary>
        ///Відображено у складі власного капіталу - усього
        ///у тому числі: збільшення (зменшення) відстрочення податкових зобов'язань
        /// 
        /// </summary>
        public double A1253 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування нараховано за звітний рік
        /// 
        /// </summary>
        public double A1300 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування використано за рік - усього
        /// 
        /// </summary>
        public double A1310 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування використано за рік - усього
        ///в тому числі на: будівництво об'єктів
        /// 
        /// </summary>
        public double A1311 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування використано за рік - усього
        ///в тому числі на: придбання (виготовлення) та поліпшення основних засобів
        /// 
        /// </summary>
        public double A1312 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування використано за рік - усього
        ///в тому числі на: придбання (виготовлення) та поліпшення основних засобів
        ///з них машини та обладнання
        /// 
        /// </summary>
        public double A1313 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування використано за рік - усього
        ///в тому числі на: придбання (виготовлення) та поліпшення основних засобів
        ///з них придбання творення нематеріальних активів
        /// 
        /// </summary>
        public double A1314 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування використано за рік - усього
        ///в тому числі на: придбання (виготовлення) та поліпшення основних засобів
        ///з них погашення отриманих на капітальні інвестиції позик
        /// 
        /// </summary>
        public double A1315 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування *ТИП В ПОЛІ A1316_1*
        /// 
        /// </summary>
        public double A1316 { get; set; }

        /// <summary>
        ///Амортизаційні відрахування *ТИП В ПОЛІ A1317_1*
        /// 
        /// </summary>
        public double A1317 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати
        ///у тому числі: відрахування до резерву сумнівних боргів
        /// Витрати
        /// </summary>
        public double B491 { get; set; }

        /// <summary>
        ///Інші операційні доходи і витрати
        ///непродуктивні витрати і витрати
        /// Витрати
        /// </summary>
        public double B492 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Залишок на початок року накопичена амортизація
        /// </summary>
        public double B09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Надійшло за рік
        /// </summary>
        public double C09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Переоцінка (дооцінка+, уцінка-) накопиченої амортизації
        /// </summary>
        public double E09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Вибуло за рік накопичена амортизація
        /// </summary>
        public double G09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Нараховано амортизації за рік
        /// </summary>
        public double H09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Витрати від зменшення корисності
        /// </summary>
        public double I09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Інші зміни за рік первісної (переоціненої) вартості
        /// </summary>
        public double J09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Інші зміни за рік накопиченої амортизації
        /// </summary>
        public double K09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L09 { get; set; }

        /// <summary>
        ///Гудвіл
        /// Залишок на кінець року накопичена амортизація
        /// </summary>
        public double M09 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        ///Залишок на початок року первісна (переоцінена) вартість
        /// </summary>
        public double A105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Залишок на початок року знос
        /// </summary>
        public double B105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Надійшло за рік
        /// </summary>
        public double C105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Переоцінка (дооцінка+, уцінка-) первісної (переоціненої) вартості
        /// </summary>
        public double D105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Переоцінка (дооцінка+, уцінка-) зносу
        /// </summary>
        public double E105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Вибуло за рік первісна (переоцінена) вартість
        /// </summary>
        public double F105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Вибуло за рік знос
        /// </summary>
        public double G105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Нараховано амортизації за рік
        /// </summary>
        public double H105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Витрати від зменшення корисності
        /// </summary>
        public double I105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Інші зміни за рік первісної (переофіненої) вартості
        /// </summary>
        public double J105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Інші зміни за рік зносу
        /// </summary>
        public double K105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Залишок на кінець року первісна (переоцінена) вартість
        /// </summary>
        public double L105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// Залишок на кінець року знос
        /// </summary>
        public double M105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// у тому числі одержані за фінансовою орендою первісна (переоцінена) вартість
        /// </summary>
        public double N105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// у тому числі одержані за фінансовою орендою знос
        /// </summary>
        public double O105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// у тому числі передані в оперативну оренду первісна (переоцінена) вартість
        /// </summary>
        public double P105 { get; set; }

        /// <summary>
        ///Інвестиційна нерухомість
        /// у тому числі передані в оперативну оренду знос
        /// </summary>
        public double Q105 { get; set; }

        /// <summary>
        ///Основні засоби орендованих єдиних (ціліних) майнових комплексів
        /// 
        /// </summary>
        public double N123 { get; set; }

        /// <summary>
        ///Залишкова вартість основних засобів, утрачених унаслідок надзвичайних подій
        /// 
        /// </summary>
        public double N124 { get; set; }

        /// <summary>
        ///Вартість інвестиційної нерухомості, оціненої за справледивою вартістю (рядок L105)
        /// 
        /// </summary>
        public double N125 { get; set; }

        /// <summary>
        ///Капітальні інвестиції в інвестиційну нерухомість (рядок В34)
        /// 
        /// </summary>
        public double N341 { get; set; }

        /// <summary>
        ///Фінансові витрати, включені до капітальних інвестицій
        /// 
        /// </summary>
        public double N342 { get; set; }

        /// <summary>
        ///з рядків (B54-B56) уключені до собівартості активів
        /// 
        /// </summary>
        public double N633 { get; set; }

        /// <summary>
        ///Запаси, призначені для продажу (З рядка 1200 графа 4 (Звіту про фінансовий стан))
        /// 
        /// </summary>
        public double N926 { get; set; }

        /// <summary>
        ///Вартість біологічних активів, придбаних за рахунок цільового фінансування
        ///(З рядка А1430_5 і А1430_14)
        /// 
        /// </summary>
        public double N1431 { get; set; }

        /// <summary>
        ///Залишкова вартість довгострокових біологічних активів, первісна вартість
        ///поточних біологічних активів і справедлива вартіть біологічних активів, 
        ///утрачених унаслідок надзвичайних подій 
        ///(З рядку А1430_6 і 1430_16)
        /// 
        /// </summary>
        public double N1432 { get; set; }

        /// <summary>
        ///Балансова вартість біологічних активів, щодо яких існують передбачені 
        ///законодавством обмеження права власності
        ///(з рядка 1430_11 і  1430_17)
        /// 
        /// </summary>
        public double N1433 { get; set; }

        /// <summary>
        /// *ТИП*
        /// *Один з видів забезпечень і резервів розділ VII, який зазначається особою яка подала звіт
        ///</summary>
        public string? St1 { get; set; }

        /// <summary>
        /// *ТИП*
        /// *Один з видів забезпечень і резервів розділ VII, який зазначається особою яка подала звіт
        /// 
        ///</summary>
        public string? St2 { get; set; }

        /// <summary>
        /// Накопичена амортизація нематеріальних активів, щодо яких існує омеження права власності (рядок М08)
        /// 
        ///</summary>
        public double N51 { get; set; }

        /// <summary>
        /// Акортизаціні відрахування *ТИП*
        /// *ТИП ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// 
        ///</summary>
        public string? R1316G1 { get; set; }

        /// <summary>
        /// Акортизаціні відрахування *ТИП*
        /// *ТИП ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// 
        ///</summary>
        public string? R1317G1 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1410G3 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, накопичена амортизація
        ///</summary>
        public double R1410G4 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1410G5 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1410G6 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, накопичена амортизація
        ///</summary>
        public double R1410G7 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// нараховано амортизації за рік
        ///</summary>
        public double R1410G8 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1410G9 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1410G10 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1410G11 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, накопичена амортизація
        ///</summary>
        public double R1410G12 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1410G13 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1410G14 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1410G15 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1410G16 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1410G17 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1411G3 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1411G5 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1411G6 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, накопичена амортизація
        ///</summary>
        public double R1411G7 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// нараховано амортизації за рік
        ///</summary>
        public double R1411G8 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1411G9 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1411G10 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1411G11 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, накопичена амортизація
        ///</summary>
        public double R1411G12 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1411G13 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1411G14 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1411G15 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1411G16 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1411G17 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1412G3 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, накопичена амортизація
        ///</summary>
        public double R1412G4 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1412G5 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1412G6 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, накопичена амортизація
        ///</summary>
        public double R1412G7 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// нараховано амортизації за рік
        ///</summary>
        public double R1412G8 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1412G9 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1412G10 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1412G11 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, накопичена амортизація
        ///</summary>
        public double R1412G12 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1412G13 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1412G14 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1412G15 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1412G16 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: продуктивна худоба
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1412G17 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1413G3 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, накопичена амортизація
        ///</summary>
        public double R1413G4 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1413G5 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1413G6 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, накопичена амортизація
        ///</summary>
        public double R1413G7 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// нараховано амортизації за рік
        ///</summary>
        public double R1413G8 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1413G9 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1413G10 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1413G11 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, накопичена амортизація
        ///</summary>
        public double R1413G12 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1413G13 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1413G14 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1413G15 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1413G16 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: багаторічні насадження
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1413G17 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// 
        ///</summary>
        public string? R1414G1 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1414G3 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, накопичена амортизація
        ///</summary>
        public double R1414G4 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1414G5 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1414G6 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, накопичена амортизація
        ///</summary>
        public double R1414G7 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// нараховано амортизації за рік
        ///</summary>
        public double R1414G8 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1414G9 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1414G10 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1414G11 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, накопичена амортизація
        ///</summary>
        public double R1414G12 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1414G13 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1414G14 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1414G15 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1414G16 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: (*ТИП* А1414_1)
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1414G17 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1415G3 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, накопичена амортизація
        ///</summary>
        public double R1415G4 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1415G5 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1415G6 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, накопичена амортизація
        ///</summary>
        public double R1415G7 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// нараховано амортизації за рік
        ///</summary>
        public double R1415G8 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1415G9 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1415G10 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1415G11 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, накопичена амортизація
        ///</summary>
        public double R1415G12 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1415G13 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1415G14 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1415G15 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1415G16 { get; set; }

        /// <summary>
        /// Інші довгострокові біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1415G17 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1420G3 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1420G5 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1420G6 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1420G9 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1420G10 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1420G11 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1420G13 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1420G14 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1420G15 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1420G16 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1420G17 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1421G3 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1421G5 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1421G6 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1421G9 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1421G10 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1421G11 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1421G13 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1421G14 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1421G15 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1421G16 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: тварини на вирощуванні та відгодівлі
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1421G17 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1422G3 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1422G5 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1422G6 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1422G9 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1422G10 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1422G11 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1422G13 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1422G14 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1422G15 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1422G16 { get; set; }

        /// <summary>
        /// Поточні біологічні активи усього
        /// в тому числі: біологічні активи в стані біологічних перетворень 
        /// (крім тварин на вирощуванні та відгодівлі)
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1422G17 { get; set; }

        /// <summary>
        /// Одна з груп біологічних активів
        /// *ТИП* ЯКИЙ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ, ЯКА ЗАПОВНЮЄ ЗВІТ
        /// 
        ///</summary>
        public string? R1423G1 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1423G3 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1423G5 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1423G6 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1423G9 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1423G10 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1423G11 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1423G13 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1423G14 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1423G15 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1423G16 { get; set; }

        /// <summary>
        /// *ТИП* А1423_1
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1423G17 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1424G3 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1424G5 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1424G6 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1424G9 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1424G10 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1424G11 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1424G13 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1424G14 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1424G15 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1424G16 { get; set; }

        /// <summary>
        /// Інші поточні біологічні активи
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1424G17 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, первісна вартість
        ///</summary>
        public double R1430G3 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, накопичена амортизація
        ///</summary>
        public double R1430G4 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1430G5 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, первісна вартість
        ///</summary>
        public double R1430G6 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// вибуло за рік, накопичена амортизація
        ///</summary>
        public double R1430G7 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// нараховано амортизації за рік
        ///</summary>
        public double R1430G8 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// витрати від зменшення корисності
        ///</summary>
        public double R1430G9 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// вигоди від відновлення корисності
        ///</summary>
        public double R1430G10 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, первісна вартість
        ///</summary>
        public double R1430G11 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за первісною вартістю, 
        /// залишок на кінець року, накопичена амортизація
        ///</summary>
        public double R1430G12 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за справелдивою вартістю, 
        /// залишок на початок року
        ///</summary>
        public double R1430G13 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за справелдивою вартістю, 
        /// надійшло за рік
        ///</summary>
        public double R1430G14 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за справелдивою вартістю, 
        /// зміни вартості за рік
        ///</summary>
        public double R1430G15 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за справелдивою вартістю, 
        /// вибуло за рік
        ///</summary>
        public double R1430G16 { get; set; }

        /// <summary>
        /// Групи біологічних активів "Разом"
        /// Обліковується за справелдивою вартістю, 
        /// залишок на кінець року
        ///</summary>
        public double R1430G17 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// вартість первісного визнання
        ///</summary>
        public double R1500G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1500G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1500G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1500G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Уцінка
        ///</summary>
        public double R1500G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Виручка від реалізації
        ///</summary>
        public double R1500G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Собівартість реалізації
        ///</summary>
        public double R1500G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1500G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1500G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// вартість первісного визнання
        ///</summary>
        public double R1510G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1510G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1510G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1510G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Уцінка
        ///</summary>
        public double R1510G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Виручка від реалізації
        ///</summary>
        public double R1510G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Собівартість реалізації
        ///</summary>
        public double R1510G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1510G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1510G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// вартість первісного визнання
        ///</summary>
        public double R1511G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1511G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1511G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1511G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Уцінка
        ///</summary>
        public double R1511G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Виручка від реалізації
        ///</summary>
        public double R1511G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Собівартість реалізації
        ///</summary>
        public double R1511G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1511G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : пшениця
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1511G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// вартість первісного визнання
        ///</summary>
        public double R1512G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1512G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1512G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1512G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Уцінка
        ///</summary>
        public double R1512G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Виручка від реалізації
        ///</summary>
        public double R1512G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Собівартість реалізації
        ///</summary>
        public double R1512G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1512G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: зернові і зернобобові, з них : соя
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1512G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// вартість первісного визнання
        ///</summary>
        public double R1513G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1513G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1513G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1513G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Уцінка
        ///</summary>
        public double R1513G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Виручка від реалізації
        ///</summary>
        public double R1513G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Собівартість реалізації
        ///</summary>
        public double R1513G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1513G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: соняшник
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1513G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// вартість первісного визнання
        ///</summary>
        public double R1514G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1514G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1514G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1514G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Уцінка
        ///</summary>
        public double R1514G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Виручка від реалізації
        ///</summary>
        public double R1514G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Собівартість реалізації
        ///</summary>
        public double R1514G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1514G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: ріпак
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1514G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// вартість первісного визнання
        ///</summary>
        public double R1515G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1515G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1515G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1515G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Уцінка
        ///</summary>
        public double R1515G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Виручка від реалізації
        ///</summary>
        public double R1515G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Собівартість реалізації
        ///</summary>
        public double R1515G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1515G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: цукрові буряки (фабричні)
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1515G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// вартість первісного визнання
        ///</summary>
        public double R1516G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1516G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1516G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1516G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Уцінка
        ///</summary>
        public double R1516G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Виручка від реалізації
        ///</summary>
        public double R1516G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Собівартість реалізації
        ///</summary>
        public double R1516G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1516G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: картопля
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1516G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// вартість первісного визнання
        ///</summary>
        public double R1517G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1517G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1517G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1517G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Уцінка
        ///</summary>
        public double R1517G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Виручка від реалізації
        ///</summary>
        public double R1517G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Собівартість реалізації
        ///</summary>
        public double R1517G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1517G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: плюди (зерняткові, кісточкові)
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1517G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// вартість первісного визнання
        ///</summary>
        public double R1518G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1518G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1518G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1518G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Уцінка
        ///</summary>
        public double R1518G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Виручка від реалізації
        ///</summary>
        public double R1518G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Собівартість реалізації
        ///</summary>
        public double R1518G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1518G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// у тому числі: інша продукція рослинництва
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1518G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// вартість первісного визнання
        ///</summary>
        public double R1519G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1519G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1519G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1519G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Уцінка
        ///</summary>
        public double R1519G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Виручка від реалізації
        ///</summary>
        public double R1519G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Собівартість реалізації
        ///</summary>
        public double R1519G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1519G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи рослинництва - усього
        /// додаткові біологічні активи рослинництва
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1519G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вартість первісного визнання
        ///</summary>
        public double R1520G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1520G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1520G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1520G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Уцінка
        ///</summary>
        public double R1520G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Виручка від реалізації
        ///</summary>
        public double R1520G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Собівартість реалізації
        ///</summary>
        public double R1520G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1520G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1520G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// вартість первісного визнання
        ///</summary>
        public double R1530G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1530G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1530G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1530G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Уцінка
        ///</summary>
        public double R1530G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Виручка від реалізації
        ///</summary>
        public double R1530G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Собівартість реалізації
        ///</summary>
        public double R1530G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1530G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1530G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// вартість первісного визнання
        ///</summary>
        public double R1531G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1531G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1531G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1531G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Уцінка
        ///</summary>
        public double R1531G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Виручка від реалізації
        ///</summary>
        public double R1531G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Собівартість реалізації
        ///</summary>
        public double R1531G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1531G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: великої рогатої худоби
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1531G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// вартість первісного визнання
        ///</summary>
        public double R1532G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// вартість первісного визнання
        ///</summary>
        public double R1533G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1532G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1532G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1532G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Уцінка
        ///</summary>
        public double R1532G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Виручка від реалізації
        ///</summary>
        public double R1532G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Собівартість реалізації
        ///</summary>
        public double R1532G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1532G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// у тому числі: приріст живої маси з нього: свиней
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1532G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1533G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1533G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1533G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Уцінка
        ///</summary>
        public double R1533G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Виручка від реалізації
        ///</summary>
        public double R1533G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Собівартість реалізації
        ///</summary>
        public double R1533G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1533G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// молоко
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1533G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// вартість первісного визнання
        ///</summary>
        public double R1534G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1534G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1534G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1534G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Уцінка
        ///</summary>
        public double R1534G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Виручка від реалізації
        ///</summary>
        public double R1534G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Собівартість реалізації
        ///</summary>
        public double R1534G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1534G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// вовна
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1534G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// вартість первісного визнання
        ///</summary>
        public double R1535G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1535G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1535G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1535G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Уцінка
        ///</summary>
        public double R1535G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Виручка від реалізації
        ///</summary>
        public double R1535G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Собівартість реалізації
        ///</summary>
        public double R1535G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1535G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// яйця
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1535G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// вартість первісного визнання
        ///</summary>
        public double R1536G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1536G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1536G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1536G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Уцінка
        ///</summary>
        public double R1536G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Виручка від реалізації
        ///</summary>
        public double R1536G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Собівартість реалізації
        ///</summary>
        public double R1536G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1536G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// інша продукція тваринництва
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1536G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// вартість первісного визнання
        ///</summary>
        public double R1537G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1537G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1537G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1537G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Уцінка
        ///</summary>
        public double R1537G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Виручка від реалізації
        ///</summary>
        public double R1537G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Собівартість реалізації
        ///</summary>
        public double R1537G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1537G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// додткові біологічні активи тваринництва
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1537G11 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// вартість первісного визнання
        ///</summary>
        public double R1538G3 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1538G4 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1538G5 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1538G6 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Уцінка
        ///</summary>
        public double R1538G7 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Виручка від реалізації
        ///</summary>
        public double R1538G8 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Собівартість реалізації
        ///</summary>
        public double R1538G9 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1538G10 { get; set; }

        /// <summary>
        /// Продукція та додаткові біологічні активи тваринництва - усього
        /// продукція рибництва
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1538G11 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// вартість первісного визнання
        ///</summary>
        public double R1539G3 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1539G4 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1539G5 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1539G6 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Уцінка
        ///</summary>
        public double R1539G7 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Виручка від реалізації
        ///</summary>
        public double R1539G8 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Собівартість реалізації
        ///</summary>
        public double R1539G9 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1539G10 { get; set; }

        /// <summary>
        /// *ТИП* А1539_1
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1539G11 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// вартість первісного визнання
        ///</summary>
        public double R1540G3 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Витрати, пов'язані з біологічними перетвореннями
        ///</summary>
        public double R1540G4 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Результат від первісного визнання: дохід
        ///</summary>
        public double R1540G5 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Результат від первісного визнання: витрати
        ///</summary>
        public double R1540G6 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Уцінка
        ///</summary>
        public double R1540G7 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Виручка від реалізації
        ///</summary>
        public double R1540G8 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Собівартість реалізації
        ///</summary>
        public double R1540G9 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Фінансовий результат (прибуток +, збиток-) від реалізації
        ///</summary>
        public double R1540G10 { get; set; }

        /// <summary>
        /// Сільськогосподарська продукція та додаткові біологічні активи - разом
        /// Фінансовий результат (прибуток +, збиток-) від первісного визнання та реалізації
        ///</summary>
        public double R1540G11 { get; set; }

        /// <summary>
        /// Довгострокові біологічні активи - усього 
        /// в тому числі: робоча худоба
        /// Обліковується за первісною вартістю, 
        /// залишок на початок року, накопичена амортизація
        ///</summary>
        public double R1411G4 { get; set; }

        /// <summary>
        /// *ТИП* Зазначенається особою, яка зоповнила звіт
        /// 
        ///</summary>
        public string? R1539G1 { get; set; }

        public string? FileName { get; set; }

    }
    public class UnitedFinZvitForm6Model
    {
        /// <summary>
        /// Айді
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ідентифікаційний номер платника податків.
        /// </summary>
        public int Tin { get; set; }

        /// <summary>
        /// Форм код
        /// </summary>
        public string? FormCode { get; set; }

        /// <summary>
        /// Назва типу звіту
        /// </summary>
        public string? FormName { get; set; }

        /// <summary>
        /// Місяц періоду звіту
        /// </summary>
        public int PeriodMonth { get; set; }

        /// <summary>
        /// Рік періоду звіту
        /// </summary>
        public int PeriodYear { get; set; }

        /// <summary>
        /// Дата та час подачі
        /// </summary>
        public string? DGet { get; set; }

        /// <summary>
        /// Дата час подачі звіту
        /// </summary>
        public DateTime? FilingDate { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0103 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів  _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0104 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S2*)_ звітний рік 
        /// </summary>
        public double A0105 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0106 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0107 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0108 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0109 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S4*)_ минулий рік
        /// </summary>
        public double A01010 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A01011 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A01012 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A01013 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A01014 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A01015 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A01016 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів Усього звітний рік
        /// </summary>
        public double A01017 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів Усього минулий рік
        /// </summary>
        public double A01018 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0113 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S1*)_ минулий рік 
        /// </summary>
        public double A0114 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S2*)_ звітний рік 
        /// </summary>
        public double A0115 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0116 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям_(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0117 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0118 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0119 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям  _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A01110 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A01111 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A01112 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A01113 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A01114 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям Нерозподілені статті звітний рік
        /// </summary>
        public double A01115 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям Нерозподілені статті минулий рік
        /// </summary>
        public double A01116 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям Усього звітний рік
        /// </summary>
        public double A01117 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): зовнішнім покупцям Усього минулий рік
        /// </summary>
        public double A01118 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0123 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0124 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0125 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0126 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам_(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0127 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0128 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0129 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A01210 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A01211 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A01212 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A01213 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A01214 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам Нерозподілені статті звітний рік
        /// </summary>
        public double A01215 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам Нерозподілені статті минулий рік
        /// </summary>
        public double A01216 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам Усього звітний рік
        /// </summary>
        public double A01217 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): іншим звітним сегментам Усього минулий рік
        /// </summary>
        public double A01218 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0133 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0134 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0135 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0136 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи_(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0137 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0138 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0139 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A01310 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A01311 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A01312 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A01313 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A01314 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи Нерозподілені статті звітний рік
        /// </summary>
        public double A01315 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи Нерозподілені статті минулий рік
        /// </summary>
        public double A01316 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи Усього звітний рік
        /// </summary>
        public double A01317 { get; set; }

        /// <summary>
        /// Доходи від операційної діяльності звітних сегментів: з них доходи від реалізації продукції
        /// (товарів, робіт, послуг): інші операційні доходи Усього минулий рік
        /// </summary>
        public double A01318 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0203 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0204 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0205 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0206 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0207 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0208 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0209 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A02010 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A02011 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A02012 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A02013 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A02014 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A02015 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A02016 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів Усього звітний рік
        /// </summary>
        public double A02017 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів Усього минулий рік
        /// </summary>
        public double A02018 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0213 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0214 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0215 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0216 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0217 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0218 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0219 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A02110 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A02111 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A02112 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A02113 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A02114 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента Нерозподілені статті звітний рік
        /// </summary>
        public double A02115 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента Нерозподілені статті минулий рік
        /// </summary>
        public double A02116 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента Усього звітний рік
        /// </summary>
        public double A02117 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них доходи від участі в капіталі, 
        /// які беспосередньо стосуються звітного сегмента Усього минулий рік
        /// </summary>
        public double A02118 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0223 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0224 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0225 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0226 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0227 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0228 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0229 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A02210 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A02211 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A02212 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A02213 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A02214 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи Нерозподілені статті звітний рік
        /// </summary>
        public double A02215 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи Нерозподілені статті минулий рік
        /// </summary>
        public double A02216 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи Усього звітний рік
        /// </summary>
        public double A02217 { get; set; }

        /// <summary>
        /// Фінансові доходи звітних сегментів: з них інші фінансові доходи Усього минулий рік
        /// </summary>
        public double A02218 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0303 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0304 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0305 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0306 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0307 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0308 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0309 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A03010 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A03011 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A03012 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A03013 { get; set; }

        /// <summary>
        /// Інші доходи _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A03014 { get; set; }

        /// <summary>
        /// Інші доходи Нерозподілені статті звітний рік
        /// </summary>
        public double A03015 { get; set; }

        /// <summary>
        /// Інші доходи Нерозподілені статті минулий рік
        /// </summary>
        public double A03016 { get; set; }

        /// <summary>
        /// Інші доходи Усього звітний рік
        /// </summary>
        public double A03017 { get; set; }

        /// <summary>
        /// Інші доходи Усього минулий рік
        /// </summary>
        public double A03018 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0403 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0404 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0405 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0406 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0407 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0408 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0409 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A04010 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A04011 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A04012 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A04013 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A04014 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A04015 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A04016 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів Усього звітний рік
        /// </summary>
        public double A04017 { get; set; }

        /// <summary>
        /// Усього доходів звітних сегментів Усього минулий рік
        /// </summary>
        public double A04018 { get; set; }

        /// <summary>
        /// Нерозподілені доходи Нерозподілені статті звітний рік
        /// </summary>
        public double A05015 { get; set; }

        /// <summary>
        /// Нерозподілені доходи Нерозподілені статті минулий рік
        /// </summary>
        public double A05016 { get; set; }

        /// <summary>
        /// Нерозподілені доходи Усього звітний рік
        /// </summary>
        public double A05017 { get; set; }

        /// <summary>
        /// Нерозподілені доходи Усього минулий рік
        /// </summary>
        public double A05018 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: доходи від операційної діяльності Нерозподілені статті звітний рік
        /// </summary>
        public double A05115 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: доходи від операційної діяльності Нерозподілені статті минулий рік
        /// </summary>
        public double A05116 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: доходи від операційної діяльності Усього звітний рік
        /// </summary>
        public double A05117 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: доходи від операційної діяльності Усього минулий рік
        /// </summary>
        public double A05118 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: фінансові доходи Нерозподілені статті звітний рік
        /// </summary>
        public double A05215 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: фінансові доходи Нерозподілені статті минулий рік
        /// </summary>
        public double A05216 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: фінансові доходи Усього звітний рік
        /// </summary>
        public double A05217 { get; set; }

        /// <summary>
        /// Нерозподілені доходи з них: фінансові доходи Усього минулий рік
        /// </summary>
        public double A05218 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0603 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0604 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0605 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0606 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0607 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0608 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0609 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A06010 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A06011 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A06012 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A06013 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A06014 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам Нерозподілені статті звітний рік
        /// </summary>
        public double A06015 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам Нерозподілені статті минулий рік
        /// </summary>
        public double A06016 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам Усього звітний рік
        /// </summary>
        public double A06017 { get; set; }

        /// <summary>
        /// Вирахування доходів від реалізації продукції (товарів, робіт, послуг) іншим звітним сегментам Усього минулий рік
        /// </summary>
        public double A06018 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0703 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0704 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0705 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0706 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0707 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0708 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0709 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A07010 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A07011 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A07012 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A07013 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A07014 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) Нерозподілені статті звітний рік
        /// </summary>
        public double A07015 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) Нерозподілені статті минулий рік
        /// </summary>
        public double A07016 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) Усього звітний рік
        /// </summary>
        public double A07017 { get; set; }

        /// <summary>
        /// Усього доходів підприємства (р. 040 + р. 050 - р.060) Усього минулий рік
        /// </summary>
        public double A07018 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0803 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0804 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0805 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0806 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0807 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0808 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0809 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A08010 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A08011 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A08012 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A08013 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A08014 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності Нерозподілені статті звітний рік
        /// </summary>
        public double A08015 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності Нерозподілені статті минулий рік
        /// </summary>
        public double A08016 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності Усього звітний рік
        /// </summary>
        public double A08017 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: витрати операційної діяльності Усього минулий рік
        /// </summary>
        public double A08018 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0813 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0814 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг):
        /// зовнішнім покупцям _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0815 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг):
        /// зовнішнім покупцям _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0816 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0817 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0818 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0819 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A08110 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A08111 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A08112 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A08113 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A08114 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям Нерозподілені статті звітний рік
        /// </summary>
        public double A08115 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям Нерозподілені статті минулий рік
        /// </summary>
        public double A08116 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям Усього звітний рік
        /// </summary>
        public double A08117 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// зовнішнім покупцям Усього минулий рік
        /// </summary>
        public double A08118 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0823 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0824 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0825 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0826 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0827 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0828 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0829 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A08210 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A08211 { get; set; }

        /// <summary>
        /// Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A08212 { get; set; }

        /// <summary>
        ///  Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A08213 { get; set; }

        /// <summary>
        ///  Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A08214 { get; set; }

        /// <summary>
        ///  Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам Нерозподілені статті звітний рік
        /// </summary>
        public double A08215 { get; set; }

        /// <summary>
        ///  Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам Нерозподілені статті минулий рік
        /// </summary>
        public double A08216 { get; set; }

        /// <summary>
        ///  Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам Усього звітний рік
        /// </summary>
        public double A08217 { get; set; }

        /// <summary>
        ///  Витрати звітних сегментів: з них собівартість реалізованої продукції (товарів, робіт, послуг): 
        /// іншим звітним сегментам Усього минулий рік
        /// </summary>
        public double A08218 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A0903 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A0904 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A0905 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A0906 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A0907 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A0908 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A0909 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A09010 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A09011 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A09012 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A09013 { get; set; }

        /// <summary>
        /// Адміністративні витрати _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A09014 { get; set; }

        /// <summary>
        /// Адміністративні витрати Нерозподілені статті звітний рік
        /// </summary>
        public double A09015 { get; set; }

        /// <summary>
        /// Адміністративні витрати Нерозподілені статті минулий рік
        /// </summary>
        public double A09016 { get; set; }

        /// <summary>
        /// Адміністративні витрати Усього звітний рік
        /// </summary>
        public double A09017 { get; set; }

        /// <summary>
        /// Адміністративні витрати Усього минулий рік
        /// </summary>
        public double A09018 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1003 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1004 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1005 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1006 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1007 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1008 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1009 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A10010 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A10011 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A10012 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A10013 { get; set; }

        /// <summary>
        /// Витрати на збут _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A10014 { get; set; }

        /// <summary>
        /// Витрати на збут Нерозподілені статті звітний рік
        /// </summary>
        public double A10015 { get; set; }

        /// <summary>
        /// Витрати на збут Нерозподілені статті минулий рік
        /// </summary>
        public double A10016 { get; set; }

        /// <summary>
        /// Витрати на збут Усього звітний рік
        /// </summary>
        public double A10017 { get; set; }

        /// <summary>
        /// Витрати на збут Усього минулий рік
        /// </summary>
        public double A10018 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1103 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1104 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1105 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1106 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1107 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1108 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1109 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A11010 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A11011 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A11012 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A11013 { get; set; }

        /// <summary>
        /// Інші операційні витрати _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A11014 { get; set; }

        /// <summary>
        /// Інші операційні витрати Нерозподілені статті звітний рік
        /// </summary>
        public double A11015 { get; set; }

        /// <summary>
        /// Інші операційні витрати Нерозподілені статті минулий рік
        /// </summary>
        public double A11016 { get; set; }

        /// <summary>
        /// Інші операційні витрати Усього звітний рік
        /// </summary>
        public double A11017 { get; set; }

        /// <summary>
        /// Інші операційні витрати Усього минулий рік
        /// </summary>
        public double A11018 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1203 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1204 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1205 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1206 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1207 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1208 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1209 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A12010 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A12011 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A12012 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A12013 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A12014 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A12015 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A12016 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів Усього звітний рік
        /// </summary>
        public double A12017 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів Усього минулий рік
        /// </summary>
        public double A12018 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1213 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1214 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1215 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1216 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1217 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1218 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1219 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A12110 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A12111 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A12112 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A12113 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A12114 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента Нерозподілені статті звітний рік
        /// </summary>
        public double A12115 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента Нерозподілені статті минулий рік
        /// </summary>
        public double A12116 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента Усього звітний рік
        /// </summary>
        public double A12117 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: з них витрати від участі в капіталі,
        /// які безпосередньо можна відносити до звітного сегмента Усього минулий рік
        /// </summary>
        public double A12118 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: "ТИП"
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1223 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1224 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1225 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1226 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1227 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1228 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1229 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A12210 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A12211 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A12212 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A12213 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A12214 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" Нерозподілені статті звітний рік
        /// </summary>
        public double A12215 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" Нерозподілені статті минулий рік
        /// </summary>
        public double A12216 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" Усього звітний рік
        /// </summary>
        public double A12217 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ" Усього минулий рік
        /// </summary>
        public double A12218 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1303 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1304 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1305 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1306 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1307 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1308 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1309 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A13010 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A13011 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A13012 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A13013 { get; set; }

        /// <summary>
        /// Інші витрати _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A13014 { get; set; }

        /// <summary>
        /// Інші витрати Нерозподілені статті звітний рік
        /// </summary>
        public double A13015 { get; set; }

        /// <summary>
        /// Інші витрати Нерозподілені статті минулий рік
        /// </summary>
        public double A13016 { get; set; }

        /// <summary>
        /// Інші витрати Усього звітний рік
        /// </summary>
        public double A13017 { get; set; }

        /// <summary>
        /// Інші витрати Усього минулий рік
        /// </summary>
        public double A13018 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1403 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1404 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1405 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1406 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1407 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1408 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1409 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A14010 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A14011 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A14012 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A14013 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A14014 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A14015 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A14016 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів Усього звітний рік
        /// </summary>
        public double A14017 { get; set; }

        /// <summary>
        /// Усього витрати звітних сегментів Усього минулий рік
        /// </summary>
        public double A14018 { get; set; }

        /// <summary>
        /// Нерозподілені витрати Нерозподілені статті звітний рік
        /// </summary>
        public double A15015 { get; set; }

        /// <summary>
        /// Нерозподілені витрати Нерозподілені статті минулий рік
        /// </summary>
        public double A15016 { get; set; }

        /// <summary>
        /// Нерозподілені витрати Усього звітний рік
        /// </summary>
        public double A15017 { get; set; }

        /// <summary>
        /// Нерозподілені витрати Усього минулий рік
        /// </summary>
        public double A15018 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них адміністративні, збутові та інші витрати операційної діяльності,
        /// не розподілені на звітні сегменти Нерозподілені статті звітний рік
        /// </summary>
        public double A15115 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них адміністративні, збутові та інші витрати операційної діяльності,
        /// не розподілені на звітні сегменти Нерозподілені статті минулий рік
        /// </summary>
        public double A15116 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них адміністративні, збутові та інші витрати операційної діяльності,
        /// не розподілені на звітні сегменти Усього звітний рік
        /// </summary>
        public double A15117 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них адміністративні, збутові та інші витрати операційної діяльності,
        /// не розподілені на звітні сегменти Усього минулий рік
        /// </summary>
        public double A15118 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них фінансові витрати Нерозподілені статті звітний рік
        /// </summary>
        public double A15215 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них фінансові витрати Нерозподілені статті минулий рік
        /// </summary>
        public double A15216 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них фінансові витрати Усього звітний рік
        /// </summary>
        public double A15217 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: з них фінансові витрати Усього минулий рік
        /// </summary>
        public double A15218 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: податок на прибуток Нерозподілені статті звітний рік
        /// </summary>
        public double A15415 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: податок на прибуток Нерозподілені статті минулий рік
        /// </summary>
        public double A15416 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: податок на прибуток Усього звітний рік
        /// </summary>
        public double A15417 { get; set; }

        /// <summary>
        /// Нерозподілені витрати: податок на прибуток Усього минулий рік
        /// </summary>
        public double A15418 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1603 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1604 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1605 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1606 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1607 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1608 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1609 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A16010 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A16011 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A16012 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A16013 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A16014 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам Нерозподілені статті звітний рік
        /// </summary>
        public double A16015 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам Нерозподілені статті минулий рік
        /// </summary>
        public double A16016 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам Усього звітний рік
        /// </summary>
        public double A16017 { get; set; }

        /// <summary>
        /// Вирахування собівартості релізованої продукції (товарів, робіт, послуг)
        /// іншим звітним сегментам Усього минулий рік
        /// </summary>
        public double A16018 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1703 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1704 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1705 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1706 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1707 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1708 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1709 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A17010 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A17011 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A17012 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A17013 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A17014 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) Нерозподілені статті звітний рік
        /// </summary>
        public double A17015 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) Нерозподілені статті минулий рік
        /// </summary>
        public double A17016 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) Усього звітний рік
        /// </summary>
        public double A17017 { get; set; }

        /// <summary>
        /// Усього витрат підприємства (р. 140 + р. 150 - р. 160) Усього минулий рік
        /// </summary>
        public double A17018 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1803 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1804 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1805 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1806 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1807 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1808 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1809 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A18010 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A18011 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A18012 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A18013 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A18014 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) Нерозподілені статті звітний рік
        /// </summary>
        public double A18015 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) Нерозподілені статті минулий рік
        /// </summary>
        public double A18016 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) Усього звітний рік
        /// </summary>
        public double A18017 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності сегмента (р. 040 - р. 140) Усього минулий рік
        /// </summary>
        public double A18018 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A1903 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A1904 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A1905 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A1906 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A1907 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A1908 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A1909 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A19010 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A19011 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A19012 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A19013 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A19014 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) Нерозподілені статті звітний рік
        /// </summary>
        public double A19015 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) Нерозподілені статті минулий рік
        /// </summary>
        public double A19016 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) Усього звітний рік
        /// </summary>
        public double A19017 { get; set; }

        /// <summary>
        /// Фінансовий результат діяльності підприємства (р. 070 - р. 170) Усього минулий рік
        /// </summary>
        public double A19018 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2003 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2004 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2005 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2006 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2007 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2008 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2009 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі s4*)_ минулий рік
        /// </summary>
        /// <summary>
        /// Активи звітних сегментів 
        /// </summary>
        public double A20010 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A20011 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A20012 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A20013 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A20014 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  Нерозподілені статті звітний рік
        /// </summary>
        public double A20015 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  Нерозподілені статті минулий рік
        /// </summary>
        public double A20016 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  Усього звітний рік
        /// </summary>
        public double A20017 { get; set; }

        /// <summary>
        /// Активи звітних сегментів  Усього минулий рік
        /// </summary>
        public double A20018 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2013 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2014 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2015 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2016 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2017 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2018 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2019 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A20110 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A20111 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A20112 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A20113 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A20114 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) Нерозподілені статті звітний рік
        /// </summary>
        public double A20115 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) Нерозподілені статті минулий рік
        /// </summary>
        public double A20116 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) Усього звітний рік
        /// </summary>
        public double A20117 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A201_1(A2011) Усього минулий рік
        /// </summary>
        public double A20118 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2023 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2024 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2025 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2026 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2027 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2028 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2029 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A20210 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A20211 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A20212 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A20213 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A20214 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) Нерозподілені статті звітний рік
        /// </summary>
        public double A20215 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) Нерозподілені статті минулий рік
        /// </summary>
        public double A20216 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) Усього звітний рік
        /// </summary>
        public double A20217 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A202_1(A2021) Усього минулий рік
        /// </summary>
        public double A20218 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2033 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2034 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2035 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2036 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2037 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2038 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2039 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A20310 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A20311 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A20312 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A20313 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A20314 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) Нерозподілені статті звітний рік
        /// </summary>
        public double A20315 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) Нерозподілені статті минулий рік
        /// </summary>
        public double A20316 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) Усього звітний рік
        /// </summary>
        public double A20317 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A203_1(A2031) Усього минулий рік
        /// </summary>
        public double A20318 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2043 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2044 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2045 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2046 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2047 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2048 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2049 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A20410 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A20411 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A20412 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A20413 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A20414 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) Нерозподілені статті звітний рік
        /// </summary>
        public double A20415 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) Нерозподілені статті минулий рік
        /// </summary>
        public double A20416 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) Усього звітний рік
        /// </summary>
        public double A20417 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A204_1(A2041) Усього минулий рік
        /// </summary>
        public double A20418 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2053 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2054 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2055 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2056 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2057 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2058 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2059 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A20510 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A20511 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A20512 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A20513 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A20514 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) Нерозподілені статті звітний рік
        /// </summary>
        public double A20515 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) Нерозподілені статті минулий рік
        /// </summary>
        public double A20516 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) Усього звітний рік
        /// </summary>
        public double A20517 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: "*ТИП*" = A205_1(A2051) Усього минулий рік
        /// </summary>
        public double A20518 { get; set; }

        /// <summary>
        /// Нерозподілені активи Нерозподілені статті звітний рік
        /// </summary>
        public double A22015 { get; set; }

        /// <summary>
        /// Нерозподілені активи Нерозподілені статті минулий рік
        /// </summary>
        public double A22016 { get; set; }

        /// <summary>
        /// Нерозподілені активи Усього звітний рік
        /// </summary>
        public double A22017 { get; set; }

        /// <summary>
        /// Нерозподілені активи Усього минулий рік
        /// </summary>
        public double A22018 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А221_1(А2211) Нерозподілені статті звітний рік
        /// </summary>
        public double A22115 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А221_1(А2211) Нерозподілені статті минулий рік
        /// </summary>
        public double A22116 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А221_1(А2211) Усього звітний рік
        /// </summary>
        public double A22117 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А221_1(А2211) Усього минулий рік
        /// </summary>
        public double A22118 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А222_1(А2221) Нерозподілені статті звітний рік
        /// </summary>
        public double A22215 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А222_1(А2221) Нерозподілені статті минулий рік
        /// </summary>
        public double A22216 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А222_1(А2221) Усього звітний рік
        /// </summary>
        public double A22217 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А222_1(А2221) Усього минулий рік
        /// </summary>
        public double A22218 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А223_1(А2231) Нерозподілені статті звітний рік
        /// </summary>
        public double A22315 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А223_1(А2231) Нерозподілені статті минулий рік
        /// </summary>
        public double A22316 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А223_1(А2231) Усього звітний рік
        /// </summary>
        public double A22317 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А223_1(А2231) Усього минулий рік
        /// </summary>
        public double A22318 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А224_1(А2241) Нерозподілені статті звітний рік
        /// </summary>
        public double A22415 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А224_1(А2241) Нерозподілені статті минулий рік
        /// </summary>
        public double A22416 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А224_1(А2241) Усього звітний рік
        /// </summary>
        public double A22417 { get; set; }

        /// <summary>
        /// Нерозподілені активи з них: "*ТИП*" = А224_1(А2241) Усього минулий рік
        /// </summary>
        public double A22418 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2304 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2305 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2306 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2307 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2308 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2309 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A23010 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A23011 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A23012 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A23013 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A23014 { get; set; }

        /// <summary>
        /// Усього активів підприємства Нерозподілені статті звітний рік
        /// </summary>
        public double A23015 { get; set; }

        /// <summary>
        /// Усього активів підприємства Нерозподілені статті минулий рік
        /// </summary>
        public double A23016 { get; set; }

        /// <summary>
        /// Усього активів підприємства Усього звітний рік
        /// </summary>
        public double A23017 { get; set; }

        /// <summary>
        /// Усього активів підприємства Усього минулий рік
        /// </summary>
        public double A23018 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2403 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2404 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2405 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2406 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2407 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2408 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2409 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A24010 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A24011 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A24012 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A24013 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A24014 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A24015 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A24016 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів Усього звітний рік
        /// </summary>
        public double A24017 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів Усього минулий рік
        /// </summary>
        public double A24018 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2413 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2414 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2415 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2416 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2417 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2418 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2419 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A24110 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A24111 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A24112 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A24113 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A24114 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) Нерозподілені статті звітний рік
        /// </summary>
        public double A24115 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) Нерозподілені статті минулий рік
        /// </summary>
        public double A24116 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) Усього звітний рік
        /// </summary>
        public double A24117 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А241_1(А2411) Усього минулий рік
        /// </summary>
        public double A24118 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2423 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2424 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2425 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2426 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2427 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2428 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2429 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A24210 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A24211 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A24212 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A24213 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A24214 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) Нерозподілені статті звітний рік
        /// </summary>
        public double A24215 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) Нерозподілені статті минулий рік
        /// </summary>
        public double A24216 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) Усього звітний рік
        /// </summary>
        public double A24217 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А242_1(А2421) Усього минулий рік
        /// </summary>
        public double A24218 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2433 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2434 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2435 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2436 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2437 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2438 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2439 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A24310 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A24311 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A24312 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A24313 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A24314 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) Нерозподілені статті звітний рік
        /// </summary>
        public double A24315 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) Нерозподілені статті минулий рік
        /// </summary>
        public double A24316 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) Усього звітний рік
        /// </summary>
        public double A24317 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А243_1(А2431) Усього минулий рік
        /// </summary>
        public double A24318 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2443 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2444 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2445 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2446 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2447 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2448 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2449 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A24410 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A24411 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A24412 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A24413 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A24414 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) Нерозподілені статті звітний рік
        /// </summary>
        public double A24415 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) Нерозподілені статті минулий рік
        /// </summary>
        public double A24416 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) Усього звітний рік
        /// </summary>
        public double A24417 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: з них "*ТИП*" = А244_1(А2441) Усього минулий рік
        /// </summary>
        public double A24418 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання Нерозподілені статті звітний рік
        /// </summary>
        public double A26015 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання Нерозподілені статті минулий рік
        /// </summary>
        public double A26016 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання Усього звітний рік
        /// </summary>
        public double A26017 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання Усього минулий рік
        /// </summary>
        public double A26018 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А261_1(А2611) Нерозподілені статті звітний рік
        /// </summary>
        public double A26115 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А261_1(А2611) Нерозподілені статті минулий рік
        /// </summary>
        public double A26116 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А261_1(А2611) Усього звітний рік
        /// </summary>
        public double A26117 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А261_1(А2611) Усього минулий рік
        /// </summary>
        public double A26118 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А262_1(А2621) Нерозподілені статті звітний рік
        /// </summary>
        public double A26215 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А262_1(А2621) Нерозподілені статті минулий рік
        /// </summary>
        public double A26216 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А262_1(А2621) Усього звітний рік
        /// </summary>
        public double A26217 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А262_1(А2621) Усього минулий рік
        /// </summary>
        public double A26218 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А263_1(А2631) Нерозподілені статті звітний рік
        /// </summary>
        public double A26315 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А263_1(А2631) Нерозподілені статті минулий рік
        /// </summary>
        public double A26316 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А263_1(А2631) Усього звітний рік
        /// </summary>
        public double A26317 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А263_1(А2631) Усього минулий рік
        /// </summary>
        public double A26318 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А264_1(А2641) Нерозподілені статті звітний рік
        /// </summary>
        public double A26415 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А264_1(А2641) Нерозподілені статті минулий рік
        /// </summary>
        public double A26416 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А264_1(А2641) Усього звітний рік
        /// </summary>
        public double A26417 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: з них "*ТИП*" = А264_1(А2641) Усього минулий рік
        /// </summary>
        public double A26418 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2703 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2704 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2705 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2706 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2707 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2708 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2709 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A27010 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A27011 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A27012 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A27013 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A27014 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) Нерозподілені статті звітний рік
        /// </summary>
        public double A27015 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) Нерозподілені статті минулий рік
        /// </summary>
        public double A27016 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) Усього звітний рік
        /// </summary>
        public double A27017 { get; set; }

        /// <summary>
        /// Усього зобов'язань підприємства (р. 240 + р. 260) Усього минулий рік
        /// </summary>
        public double A27018 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2803 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2804 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2805 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2806 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2807 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2808 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2809 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A28010 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A28011 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A28012 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A28013 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A28014 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Нерозподілені статті звітний рік
        /// </summary>
        public double A28015 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Нерозподілені статті минулий рік
        /// </summary>
        public double A28016 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Усього звітний рік
        /// </summary>
        public double A28017 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Усього минулий рік
        /// </summary>
        public double A28018 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2903 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A2904 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A2905 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A2906 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A2907 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A2908 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A2909 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A29010 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A29011 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A29012 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A29013 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A29014 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів Нерозподілені статті звітний рік
        /// </summary>
        public double A29015 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів Нерозподілені статті минулий рік
        /// </summary>
        public double A29016 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів Усього звітний рік
        /// </summary>
        public double A29017 { get; set; }

        /// <summary>
        /// Амортизація необоротних активів Усього минулий рік
        /// </summary>
        public double A29018 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3003 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3004 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3005 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3006 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3007 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3008 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3009 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A30010 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A30011 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A30012 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A30013 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A30014 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Нерозподілені статті звітний рік
        /// </summary>
        public double A30015 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Нерозподілені статті минулий рік
        /// </summary>
        public double A30016 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Усього звітний рік
        /// </summary>
        public double A30017 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Усього минулий рік
        /// </summary>
        public double A30018 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3103 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3104 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3105 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3106 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3107 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3108 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3109 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A31010 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A31011 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A31012 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A31013 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A31014 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A31015 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A31016 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Усього звітний рік
        /// </summary>
        public double A31017 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Усього минулий рік
        /// </summary>
        public double A31018 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3203 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3204 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3205 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3206 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3207 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3208 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3209 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A32010 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A32011 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A32012 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A32013 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A32014 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Нерозподілені статті звітний рік
        /// </summary>
        public double A32015 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Нерозподілені статті минулий рік
        /// </summary>
        public double A32016 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Усього звітний рік
        /// </summary>
        public double A32017 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Усього минулий рік
        /// </summary>
        public double A32018 { get; set; }

        /// <summary>
        /// Фінансові витрати звітних сегментів: ТИП
        /// *"ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ"
        /// </summary>
        public string? A1221 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2021 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2031 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2041 { get; set; }

        /// <summary>
        /// Активи звітних сегментів з них: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2051 { get; set; }

        /// <summary>
        /// Нерозподілені активи: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2221 { get; set; }

        /// <summary>
        /// Нерозподілені активи: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2231 { get; set; }

        /// <summary>
        /// Нерозподілені активи: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2241 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2421 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2431 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2441 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2621 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2631 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2641 { get; set; }
        /// <summary>
        /// Активи звітних сегментів, вказаних юр. особою
        /// </summary>
        public string? A2011 { get; set; }

        /// <summary>
        /// Нерозподілені активи: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2211 { get; set; }

        /// <summary>
        /// Зобов'язання звітних сегментів: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2411 { get; set; }

        /// <summary>
        /// Нерозподілені зобов'язання: ТИП
        /// *ТИП ВИТРАТ ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A2611 { get; set; }

        /// <summary>
        /// Усього активів підприємства _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A2303 { get; set; }

        /// <summary>
        /// Тип звіту: 0 - загальний звіт, 1 - коригуючий звіт.
        /// </summary>
        public string? CDocType { get; set; }

        /// <summary>
        /// Кількість податкових звітів у файлі.
        /// </summary>
        public string? CDocCnt { get; set; }

        /// <summary>
        /// Код регіону.
        /// </summary>
        public string? CReg { get; set; }

        /// <summary>
        /// Код району.
        /// </summary>
        public string? CRaj { get; set; }

        /// <summary>
        /// Тип періоду звіту: 0 - за місяць, 1 - за квартал, 2 - за півріччя, 3 - за 9 місяців, 4 - за рік, 5 - за інший період.
        /// </summary>
        public string? PeriodType { get; set; }

        /// <summary>
        /// Назва програмного забезпечення, яке використовувалося для підготовки та подання звіту.
        /// </summary>
        public string? Software { get; set; }

        /// <summary>
        /// Рік звітності
        /// </summary>
        public string? RepNyear { get; set; }

        /// <summary>
        /// Місяць звітності
        /// </summary>
        public string? RepNmonth { get; set; }

        /// <summary>
        /// Назва компанії
        /// </summary>
        public string? FirmName { get; set; }

        /// <summary>
        /// ЄДРПОУ компанії
        /// </summary>
        public string? FirmEdrpou { get; set; }

        /// <summary>
        /// Територія компанії
        /// </summary>
        public string? FirmTerr { get; set; }

        /// <summary>
        /// ОГУ компанії
        /// </summary>
        public string? FirmOgu { get; set; }

        /// <summary>
        /// СПОДУ компанії
        /// </summary>
        public string? FirmSpodu { get; set; }

        /// <summary>
        /// Назва КВЕД компанії
        /// </summary>
        public string? FirmKvednm { get; set; }

        /// <summary>
        /// Код КВЕД компанії
        /// </summary>
        public string? FirmKved { get; set; }

        /// <summary>
        /// КСКЗ звітності
        /// </summary>
        public string? RepKskz { get; set; }

        /// <summary>
        /// ПІБ директора компанії
        /// </summary>
        public string? FirmRuk { get; set; }

        /// <summary>
        /// ПІБ головного бухгалтера компанії
        /// </summary>
        public string? FirmBuh { get; set; }

        /// <summary>
        /// Код організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfcd { get; set; }

        /// <summary>
        /// Назва організаційно-правової форми компанії
        /// </summary>
        public string? FirmOpfnm { get; set; }

        /// <summary>
        /// Кодифікатор адміністративно-територіальних одиниць та територій територіальних громад
        /// </summary>
        public string KATOTTG { get; set; }

        /// <summary>
        /// Останній день звітного періоду
        /// </summary>
        public string? Lastday { get; set; }

        /// <summary>
        /// Показники пріоритетних звітних ___(господарських, географічний виробничий, географічний збутовий)____ сегментів 
        /// </summary>
        public string? N1 { get; set; }

        /// <summary>
        /// Показники за допоміжними звітними ___(господарських, географічний виробничий, географічний збутовий)____ сегментами
        /// </summary>
        public string? N2 { get; set; }

        /// <summary>
        /// Показники за допоміжними звітними географічними ___(господарських, географічний виробничий, географічний збутовий)___ сегментами
        /// </summary>
        public string? N3 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S1 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S2 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S3 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S4 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S5 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S6 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S7 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S8 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S9 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S10 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S11 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S12 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S13 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S14 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S15 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S16 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S17 { get; set; }

        /// <summary>
        /// Найменування звітних сегментів
        /// </summary>
        public string? S18 { get; set; }

        /// <summary>
        /// *ТИП ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A3301 { get; set; }

        /// <summary>
        /// *ТИП ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A3401 { get; set; }

        /// <summary>
        /// *ТИП ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A3801 { get; set; }

        /// <summary>
        /// *ТИП ЗАЗНАЧАЄТЬСЯ ОСОБОЮ ЯКА ПОДАЛА ЗВІТ
        /// </summary>
        public string? A3901 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3303 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3304 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3305 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3306 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3307 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3308 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3309 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A33010 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A33011 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A33012 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A33013 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A33014 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) Нерозподілені статті звітний рік
        /// </summary>
        public double A33015 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) Нерозподілені статті минулий рік
        /// </summary>
        public double A33016 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) Усього звітний рік
        /// </summary>
        public double A33017 { get; set; }

        /// <summary>
        /// "*ТИП*" = А330_1(А3301) Усього минулий рік
        /// </summary>
        public double A33018 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3403 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3404 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3405 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3406 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3407 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3408 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3409 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A34010 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A34011 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A34012 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A34013 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A34014 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) Нерозподілені статті звітний рік
        /// </summary>
        public double A34015 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) Нерозподілені статті минулий рік
        /// </summary>
        public double A34016 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) Усього звітний рік
        /// </summary>
        public double A34017 { get; set; }

        /// <summary>
        /// "*ТИП*" = А340_1(А3401) Усього минулий рік
        /// </summary>
        public double A34018 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3503 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3504 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3505 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3506 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3507 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3508 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3509 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A35010 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A35011 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A35012 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A35013 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A35014 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Нерозподілені статті звітний рік
        /// </summary>
        public double A35015 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Нерозподілені статті минулий рік
        /// </summary>
        public double A35016 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Усього звітний рік
        /// </summary>
        public double A35017 { get; set; }

        /// <summary>
        /// Доходи від реалізації продукції (товарів, робіт, послуг) зовнішнім покупцям Усього минулий рік
        /// </summary>
        public double A35018 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3603 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3604 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3605 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3606 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3607 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3608 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3609 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A36010 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A36011 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A36012 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A36013 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A36014 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Нерозподілені статті звітний рік
        /// </summary>
        public double A36015 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Нерозподілені статті минулий рік
        /// </summary>
        public double A36016 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Усього звітний рік
        /// </summary>
        public double A36017 { get; set; }

        /// <summary>
        /// Балансова вартість активів звітних сегментів Усього минулий рік
        /// </summary>
        public double A36018 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3703 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3704 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3705 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3706 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3707 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3708 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3709 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A37010 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A37011 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A37012 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A37013 { get; set; }

        /// <summary>
        /// Капітальні інвестиції _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A37014 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Нерозподілені статті звітний рік
        /// </summary>
        public double A37015 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Нерозподілені статті минулий рік
        /// </summary>
        public double A37016 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Усього звітний рік
        /// </summary>
        public double A37017 { get; set; }

        /// <summary>
        /// Капітальні інвестиції Усього минулий рік
        /// </summary>
        public double A37018 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3803 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3804 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3805 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3806 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3807 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3808 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3809 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A38010 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A38011 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A38012 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A38013 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A38014 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) Нерозподілені статті звітний рік
        /// </summary>
        public double A38015 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) Нерозподілені статті минулий рік
        /// </summary>
        public double A38016 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) Усього звітний рік
        /// </summary>
        public double A38017 { get; set; }

        /// <summary>
        /// "*ТИП*" = А380_1(А3801) Усього минулий рік
        /// </summary>
        public double A38018 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S1*)_ звітний рік 
        /// </summary>
        public double A3903 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S1*)_ минулий рік
        /// </summary>
        public double A3904 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S2*)_ звітний рік
        /// </summary>
        public double A3905 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S2*)_ минулий рік
        /// </summary>
        public double A3906 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S3*)_ звітний рік
        /// </summary>
        public double A3907 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S3*)_ минулий рік
        /// </summary>
        public double A3908 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S4*)_ звітний рік
        /// </summary>
        public double A3909 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі s4*)_ минулий рік
        /// </summary>
        public double A39010 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S5*)_ звітний рік
        /// </summary>
        public double A39011 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S5*)_ минулий рік
        /// </summary>
        public double A39012 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S6*)_ звітний рік
        /// </summary>
        public double A39013 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) _(*значення в полі S6*)_ минулий рік
        /// </summary>
        public double A39014 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) Нерозподілені статті звітний рік
        /// </summary>
        public double A39015 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) Нерозподілені статті минулий рік
        /// </summary>
        public double A39016 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) Усього звітний рік
        /// </summary>
        public double A39017 { get; set; }

        /// <summary>
        /// "*ТИП*" = А390_1(А3901) Усього минулий рік
        /// </summary>
        public double A39018 { get; set; }

        public string? FileName { get; set; }

    }
}
