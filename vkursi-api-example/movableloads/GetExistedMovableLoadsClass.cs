using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using vkursi_api_example.organizations;
using vkursi_api_example.token;

namespace vkursi_api_example.movableloads
{
    public static class GetExistedMovableLoadsClass
    {

        /*
        
        Метод:
             30. ДРОРМ отримання витягів які були замовлені раніше в сервісі Vkursi
             [POST] /api/1.0/movableloads/getexistedmovableloads

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getmovableloads' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"IdList":[282154],"Edrpou":null,"Ipn":null,"DateStart":"2022-01-04T00:00:00","DateEnd":"2022-01-06T00:00:00"}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetMovableLoadsResponse.json


        */

        public static ApiGetExistedAdvancedReportAnswer GetExistedMovableLoads(ref string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {

                ApiGetExistedAdvancedReport requestModel= new()
                {
                    IdList = new()
                    {
                        802710,
                    },
                    DateStart = DateTime.Parse("04.01.2000"),
                    DateEnd = DateTime.Parse("06.01.2005"),
                };

                string body = JsonConvert.SerializeObject(requestModel);// {"IdList":[282154],"Edrpou":null,"Ipn":null,"DateStart":"2022-01-04T00:00:00","DateEnd":"2022-01-06T00:00:00"}        


                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getexistedmovableloads");
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", $"Bearer {token}");
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
                else if ((int)response.StatusCode == 200 && responseString.Contains("Update in progress, total objects"))
                {
                    Console.WriteLine("Триває процес оновлення інформації за вказанними параметрами, спробуйте повторити запит через 30 секунд");
                    return null;
                }
                else if ((int)response.StatusCode == 200 && responseString.Contains("Update in progress, try again later"))
                {
                    Console.WriteLine("Триває процес оновлення інформації за вказанними параметрами, спробуйте повторити запит через 30 секунд");
                    return null;
                }
                else if ((int)response.StatusCode == 403 && responseString.Contains("Not enough cards to form a request"))
                {
                    Console.WriteLine("Недостатньо ресурсів для виконання запиту, відповідно до вашого тарифу. Дізнатися об'єм доступних ресурсів - /api/1.0/token/gettariff");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            ApiGetExistedAdvancedReportAnswer GCEEResponseRow = new ApiGetExistedAdvancedReportAnswer();

            GCEEResponseRow = JsonConvert.DeserializeObject<ApiGetExistedAdvancedReportAnswer>(responseString);

            return GCEEResponseRow;
        }
    }
    /// <summary>
    /// Модель запиту в яких DateStart, DateEnd, а також один з апараметрів пошуку IdList/Edrpou/Ipn є обов'язкові
    /// </summary>
    public class ApiGetExistedAdvancedReport
    {
        public List<int> IdList { get; set; }
        public List<string> Edrpou { get; set; }
        public List<string> Ipn { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
    /// <summary>
    /// МОдель відповіді
    /// </summary>
    public class ApiGetExistedAdvancedReportAnswer
    {
        public bool isSuccess { get; set; }
        public string status { get; set; }
        /// <summary>
        /// Основні дані
        /// </summary>
        public List<MovableLoadsReturnGetModel> data { get; set; }
    }
    public class MovableLoadsReturnGetModel
    {
        public Guid UserId { get; set; }
        public Dictionary<int, Guid> Success { get; set; }
        public List<int> WithError { get; set; }
        public List<int> NotFoundList { get; set; }
        public string ReportRef { get; set; }
        public GetAdvancedMovableReportAPIModel Report { get; set; }
    }

    public class CheckMovableLoadsReturnGetModel
    {
        public string ResultMessage { get; set; }
        public MovableLoadsReturnGetModel TaskResult { get; set; }
    }

    public class GetAdvancedMovableReportModel
    {
        public DateTime CreatedOn { get; set; }
        public string DataObject { get; set; }
    }
    public class GetAdvancedMovableReportAPIModel
    {
        public DateTime CreatedOn { get; set; }
        public string dataObjectOriginal { get; set; }
        public string dataObjectSign { get; set; }
        public MovableLoadsModel dataObject { get; set; }
    }
    public class MovableLoadsModel
    {
        /// <summary>
        /// Ідентифікатор запису (для АПІ не несе інформаційного навантаження)
        /// </summary>
        public int? ID { get; set; }
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int? opOpID { get; set; }
        /// <summary>
        /// Тип запису (код)
        /// </summary>
        public int? opType { get; set; }
        /// <summary>
        /// Тип обтяження - Податкова застава   LM_TYPE=12 / Звернення стягнення LM_TYPE=13 / Заборона на рухоме майно LM_TYPE=7 / Арешт рухомого майна LM_TYPE = 8 / Застава рухомого майна LM_TYPE = 10 / Інше обтяження рухомого майна LM_TYPE=999
        /// </summary>
        public string? lmType { get; set; }
        /// <summary>
        /// Стан запису (активний анульований) – в АПІ надходить тільки статус 1 - активний
        /// </summary>
        public int? opStatus { get; set; }
        /// <summary>
        /// Термін дії обтяження 
        /// </summary>
        public DateTime? actTerm { get; set; } // 
        /// <summary>
        /// Архівна дата
        /// </summary>
        public string? archiveDate { get; set; } // 
        /// <summary>
        /// Термін виконання зобов’язання
        /// </summary>
        public DateTime? execTerm { get; set; } // 

        /// <summary>
        /// Реєстраційний номер обтяження (зовнішній ключ)
        /// </summary>
        public string? regNum { get; set; } // 
        /// <summary>
        /// Розмір основного зобов’язання: сума
        /// </summary>
        public string? contractSum { get; set; } // 
        /// <summary>
        /// Контрольна сума заяви (для АПІ не несе інформаційного навантаження)
        /// </summary>
        public string? checkSum { get; set; } // 

        /// <summary>
        /// Ознака наявності “Звернення стягнення”
        /// </summary>
        public int? penaltyInit { get; set; } // 
        /// <summary>
        /// Розмір основного зобов’язання: валюта
        /// </summary>
        public string? currencyType { get; set; } // 
        /// <summary>
        /// Вид обтяження (публічне = 1,  приватне = 2)
        /// </summary>
        public string? lmSort { get; set; } // 
        /// <summary>
        /// 1. Дозволено відчужувати 2. Заборонено відчужувати 3. За погодженням з обтяжувачем
        /// </summary>
        public string? alPossible { get; set; } // 
        /// <summary>
        /// Опис у довільному форматі типу обтяження
        /// </summary>
        public string? lmTypeExtension { get; set; } // 
        /// <summary>
        /// Додаткові дані до обтяження
        /// </summary>
        public string? additional { get; set; } // 

        public string? archRegName { get; set; }
        /// <summary>
        /// Дата обтяження
        /// </summary>
        public DateTime? regDate { get; set; } // 

        /// <summary>
        /// Опис майна
        /// </summary>
        public List<MovableLoadsPropertyModel> properties { get; set; } // 
        /// <summary>
        /// Інформація про сторони (обтяжувач, боржник)
        /// </summary>
        public List<MovableLoadSubjectsModel> subjects { get; set; } // 
        /// <summary>
        /// Документ(и) на підставі яких внесено обтяження
        /// </summary>
        public List<MovableLoadCauseDocumentsModel> causeDocuments { get; set; }
        /// <summary>
        /// Дата проведення операції формування витягу
        /// </summary>
        public DateTime? opRegDate { get; set; } // 
        /// <summary>
        /// Інформація про реєстратора
        /// </summary>
        public string? registrarInfo { get; set; }       // 
        /// <summary>
        /// Стан документа
        /// </summary>
        public string? lmState { get; set; }             // 
        /// <summary>
        /// Документи (підтверджуючі реєстрацію обтяження???)
        /// </summary>
        public List<MovableLoadsDocumentsModel> documents { get; set; }
        /// <summary>
        /// Назва стану документа
        /// </summary>
        public string? lmStateName { get; set; }         // 
        /// <summary>
        /// Організація реєстратора, який сформував витяг
        /// </summary>
        public string? currentObjName { get; set; }      // 
        /// <summary>
        /// Реєстратора, який сформував витяг
        /// </summary>
        public string? currentRegistrar { get; set; }    // 
        /// <summary>
        /// Ознака наявності “Звернення стягнення”
        /// </summary>
        public string? penalty { get; set; }             // 
        /// <summary>
        /// Наш ID для ідентифікації вкладеності. Формується з усього json
        /// </summary>
        public Guid MainId { get; set; }

        public string? prevRegistration { get; set; }
        /// <summary>
        /// ID запиту
        /// </summary>
        public int? reqReqID { get; set; }
        public string? bnBnID { get; set; }

        public string? archiveNum { get; set; }

        public int? currentObjID { get; set; }
        /// <summary>
        /// Число початку дії обтяження
        /// </summary>
        public string? startDay { get; set; } // 
        /// <summary>
        /// Місяць початку дії обтяження
        /// </summary>
        public string? startMonth { get; set; }
        /// <summary>
        /// Рік початку дії обтяження
        /// </summary>
        public string? startYear { get; set; }
        /// <summary>
        /// Розмір основного зобов’язання: сума (вказано в deciaml)
        /// </summary>
        public double? validContractSum { get; set; }
    }
    /// <summary>
    /// Об'єкт(и) обтяження
    /// </summary>
    public class MovableLoadsPropertyModel
    {
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int? MovableNum { get; set; }

        /// <summary>
        /// Державні номерні знаки, рухомого майна (або інший ідентифікуючий державний номер)
        /// </summary>
        public string? mvRegNum { get; set; }
        /// <summary>
        /// Тип рухомого транспортного засобу
        /// </summary>
        public string? prType { get; set; }
        /// <summary>
        /// Тип змін (для АПІ не несе інформаційного навантаження)
        /// </summary>
        public string? changeType { get; set; }
        /// <summary>
        /// Вид майна (рухоме/невизначене)
        /// </summary>
        public int? prCategory { get; set; }
        /// <summary>
        /// Номер кузова/шасі/серійний номер/інші ідентифікуючі ознаки рухомого майна
        /// </summary>
        public string? mvSrNum { get; set; }
        /// <summary>
        /// Інший номер державної реєстрації
        /// </summary>
        public string? otherRegNum { get; set; }
        /// <summary>
        /// Номер кузова/шасі/серійний номер/інші ідентифікуючі ознаки рухомого майна (додаткове перевірочне поле для mvSrNum)
        /// </summary>
        public string? mvSrNumHash { get; set; }
        /// <summary>
        /// Державні номерні знаки, рухомого майна (або інший ідентифікуючий державний номер) унікальний ідентифікатор
        /// </summary>
        public string? mvRegNumHash { get; set; }

        /// <summary>
        /// Додатковий реєстраційний номер
        /// </summary>
        public string? otherRegNumHash { get; set; }

        /// <summary>
        /// Опис об'єкту обтяження
        /// </summary>
        public string? additional { get; set; }

        /// <summary>
        /// Опис майна дубль значення з полем fullExtension
        /// </summary>
        public string? prTypeExtension { get; set; }

        /// <summary>
        /// prParentID
        /// </summary>
        public int? prParentID { get; set; }
        /// <summary>
        /// prPrID
        /// </summary>
        public long? prPrID { get; set; }

        public int? opOpID { get; set; }

        public int? adAdID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? isArchive { get; set; }
        /// <summary>
        /// Реєстраційний номер майна
        /// </summary>
        public string? prRegNum { get; set; }
        /// <summary>
        /// ID Реєстраційний номер майна
        /// </summary>
        public string? prRegNumNID { get; set; }
        /// <summary>
        /// Опис майна дубль значення з полем prTypeExtension
        /// </summary>
        public string? fullExtension { get; set; }
        /// <summary>
        /// Масив переліку динамічних атрибутів та їх значень
        /// </summary>
        public List<object>? prAttr { get; set; }
        public List<MovableLoadsPrTypeAttrModel> prTypeAttr { get; set; }
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string? addProperties { get; set; }
        /// <summary>
        /// MainId верхнього рівня
        /// </summary>
        public Guid ParentId { get; set; }

        public int? ID { get; set; }
    }
    public class MovableLoadsPrTypeAttrModel
    {
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int? MovableNum { get; set; }

        public string? xtype { get; set; }
        public string? name { get; set; }
        public string? fieldLabel { get; set; }
        public string? attrTyp { get; set; }
        public int? dataType { get; set; }
        public bool? enforceMaxLength { get; set; }
        public int? daRnNum { get; set; }
        public string? width { get; set; }
        public int? flex { get; set; }
        public bool? allowBlank { get; set; }
        public int? maxLength { get; set; }
        public string? vtype { get; set; }
        /// <summary>
        /// Ідентифікатор PropId
        /// </summary>
        public Guid ParentId { get; set; }
    }

    public class MovableLoadSubjectsModel
    {
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int? MovableNum { get; set; }
        /// <summary>
        /// Роль суб'єкта (обтяжувач, боржник)
        /// </summary>
        public string? rlName { get; set; }
        /// <summary>
        /// Назва суб'єкта
        /// </summary>
        public string? addSubject { get; set; }
        /// <summary>
        /// Тип суб'єкта: / 1 (фіз.особа) / 2 (юр.особа)
        /// </summary>
        public string? sbjType { get; set; } //

        /// <summary>
        /// Ознака нерезедентності
        /// </summary>
        public string? foreignSubject { get; set; } // 
        /// <summary>
        /// Інформація про резиденство
        /// </summary>
        public string? codeAbsence { get; set; }
        /// <summary>
        /// Код ЄДРПОУ
        /// </summary>
        public string? code { get; set; }
        /// <summary>
        /// Тип змін (для АПІ не несе інформаційного навантаження)
        /// </summary>
        public string? changeType { get; set; } // 
        /// <summary>
        /// Рік народження
        /// </summary>
        public string? birthYear { get; set; }
        /// <summary>
        /// Місяць народження
        /// </summary>
        public string? birthMonth { get; set; }
        /// <summary>
        /// День народження
        /// </summary>
        public string? birthDay { get; set; }
        /// <summary>
        /// Дата народження
        /// </summary>
        public string? birthDate { get; set; }
        /// <summary>
        /// Поштовий індекс
        /// </summary>
        public int? addressIndex { get; set; }
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string? additional { get; set; } // 
        /// <summary>
        /// Місце народження
        /// </summary>
        public string? birthPlace { get; set; }
        /// <summary>
        /// Наіменування суб'єкта
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// Країна резиденства
        /// </summary>
        public string? cnCnStr { get; set; }
        /// <summary>
        /// Додаткова адреса
        /// </summary>
        public string? simpleAddress { get; set; }

        /// <summary>
        /// Частина адреси (вулиця і будинок)
        /// </summary>
        public string? addressDetails { get; set; }
        /// <summary>
        /// Документ особи
        /// </summary>
        public string? document { get; set; }
        public string? rusName { get; set; }
        /// <summary>
        /// Повна назва суб'єкта
        /// </summary>
        public string? fullName { get; set; }
        /// <summary>
        /// Код країни громадянства/походження
        /// </summary>
        public string? dcCountry { get; set; }
        /// <summary>
        /// Країна
        /// </summary>
        public string? country { get; set; }
        /// <summary>
        /// Номер кімнати/квартири
        /// </summary>
        public string? room { get; set; }
        /// <summary>
        /// Тип приміщення
        /// </summary>
        public string? dcRoomType { get; set; }
        /// <summary>
        /// Тип поділенн будівлі (під'їзд, секція і т.д.)
        /// </summary>
        public string? dcBuildingType { get; set; }
        /// <summary>
        /// Корпус бідвлі
        /// </summary>
        public string? building { get; set; }
        /// <summary>
        /// Тип об'єкту
        /// </summary>
        public string? dcObjectNumType { get; set; }
        /// <summary>
        /// Номер об'єкту
        /// </summary>
        public string? objectNum { get; set; }
        /// <summary>
        /// Тип будівлі повна назва
        /// </summary>
        public string? dcHouseType { get; set; }
        /// <summary>
        /// Тип будівлі коротка назва
        /// </summary>
        public string? house { get; set; }
        public string? streetAtuID { get; set; }
        /// <summary>
        /// Контактний номер телефону
        /// </summary>
        public string? phone { get; set; }
        /// <summary>
        /// Перша частина адреси (Обл., р-н, нас. пункт)
        /// </summary>
        public string? atuAtuStr { get; set; }
        public int? id { get; set; }
        public string? subjectParentID { get; set; }
        public string? rusNID { get; set; }
        public string? atuAtuID { get; set; }
        public string? cnCnID { get; set; }
        public int? rlRlID { get; set; }
        public int? opOpID { get; set; } // 
        public int? ukrNID { get; set; }
        public Guid ParentId { get; set; }

    }

    public class MovableLoadCauseDocumentsModel
    {
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int? MovableNum { get; set; }

        /// <summary>
        /// Дата видачі документа
        /// </summary>
        public DateTime? pubDate { get; set; } // 
        /// <summary>
        /// Тип документа
        /// </summary>
        public string? cdType { get; set; } // 
        /// <summary>
        /// Номер документа
        /// </summary>
        public string? serNum { get; set; } // 
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string? additional { get; set; } // 
        /// <summary>
        /// Ким виданий
        /// </summary>
        public string? publisher { get; set; } // 
        /// <summary>
        /// Опис типу документу
        /// </summary>
        public string? cdTypeExtension { get; set; } // 
        /// <summary>
        /// 
        /// </summary>
        public string? cdParentID { get; set; }
        /// <summary>
        /// Тип зміни запису про документ (додавання, виправлення тощо)
        /// </summary>
        public string? changeType { get; set; } // 
        /// <summary>
        /// Інформація про документ - підставу
        /// </summary>
        public string? causeDocumentInfo { get; set; } // 
        /// <summary>
        /// MainId верхнього рівня
        /// </summary>
        public Guid ParentId { get; set; }
        public int? id { get; set; } // 

        public int? opOpID { get; set; } // 
    }

    public class MovableLoadsDocumentsModel
    {
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int? MovableNum { get; set; }

        public int? docID { get; set; }
        /// <summary>
        /// Реєстраційний номер документа
        /// </summary>
        public int? regNum { get; set; }
        public int? objObjID { get; set; }
        public int? empEmpID { get; set; }
        /// <summary>
        /// Тип документа (код)
        /// </summary>
        public int? docType { get; set; }
        /// <summary>
        /// generateddocument
        /// </summary>
        public string? generateddocument { get; set; }
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public DateTime? regDate { get; set; }
        public string? seSeID { get; set; }
        /// <summary>
        /// Стан документа
        /// </summary>
        public string? docState { get; set; }
        /// <summary>
        /// Тип документа
        /// </summary>
        public string? docTypeName { get; set; }
        /// <summary>
        /// Повне наіменнування служюової особи
        /// </summary>
        public string? empFullName { get; set; }
        /// <summary>
        /// MainId верхнього рівня
        /// </summary>
        public Guid ParentId { get; set; }
    }
}
