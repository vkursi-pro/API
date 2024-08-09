using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.movableloads
{
    public class GetPayMovableLoadsClass
    {
        /*
         
        23. ДРОРМ отримання витяга по Id обтяжння
        [POST] api/1.0/MovableLoads/getpaymovableloads
        
        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw '{"Id":1278898}'
             
         */

        public static GetPayMovableLoadsResponseModel GetPayMovableLoads(ref string token, int movableId)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetPayMovableLoadsRequestBodyModel GPMLRequestBody = new GetPayMovableLoadsRequestBodyModel
                {
                    Id = movableId                                          // Id обтяжння
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GPMLRequestBody); // Example Body: {"Id":1278898}

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

            GetPayMovableLoadsResponseModel GPMLResponseRow = new GetPayMovableLoadsResponseModel();

            GPMLResponseRow = JsonConvert.DeserializeObject<GetPayMovableLoadsResponseModel>(responseString);

            return GPMLResponseRow;
        }
    }


    /*
        // Python - http.client example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("text/plain");
        RequestBody body = RequestBody.create(mediaType, "{\"Id\":1278898}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .build();
        Response response = client.newCall(request).execute();

        
        // Java - OkHttp example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Id\":1278898}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...'
        }
        conn.request("POST", "/api/1.0/MovableLoads/getpaymovableloads", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

    */

    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetPayMovableLoadsRequestBodyModel                         // 
    {/// <summary>
     /// Id обтяжння
     /// </summary>
        public int Id { get; set; }                                         // 
    }
    /// <summary>
    /// Модель відповіді
    /// </summary>
    public class GetPayMovableLoadsResponseModel                            // 
    {
        /// <summary>
        /// Успішний запит (true - так / false - ні)
        /// </summary>
        public bool isSuccess { get; set; }                                 // 
        /// <summary>
        /// Статус запиту
        /// </summary>
        public string status { get; set; }                                  // 
        /// <summary>
        /// Дані
        /// </summary>
        public Data data { get; set; }                                      // 
    }

    /// <summary>
    /// ???
    /// </summary>
    public class Data                                                       
    {
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public Guid userId { get; set; }                                    // 
        /// <summary>
        /// Системний id Vkursi
        /// </summary>
        public Dictionary<int, Guid> success { get; set; }
        /// <summary>
        /// Перелік не успішнио отриманих обтяжень
        /// </summary>
        public List<int> withError { get; set; }                            // 
        /// <summary>
        /// Посилання на перегляд витягу на Vkursi
        /// </summary>
        public string reportRef { get; set; }                               // 
        /// <summary>
        /// Дані витягу
        /// </summary>
        public Report report { get; set; }                                  // 
    }
    /// <summary>
    /// ???
    /// </summary>
    public class Report
    {/// <summary>
     /// Дата отримання
     /// </summary>
        public DateTime createdOn { get; set; }                             // 
        /// <summary>
        /// Оригінальні дані відповіді від Nais (1в1).  Опис полів від ДП Nais https://nais.gov.ua/files/general/2020/01/27/20200127160802-47.docx
        /// </summary>
        public string dataObjectOriginal { get; set; }                      //                      
        /// <summary>
        /// Витяг (Оригінальні дані відповіді Nais перетворені в об'єкт).Опис полів від ДП Nais https://nais.gov.ua/files/general/2020/01/27/20200127160802-47.docx 
        /// </summary>
        public DataObjectClear dataObject { get; set; }                     //    
    }
    /// <summary>
    /// Витяг
    /// </summary>
    public class DataObjectClear                                            // 
    {/// <summary>
     /// Ідентифікатор запису (для АПІ не несе інформаційного навантаження)
     /// </summary>
        public int id { get; set; }                             // 
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int opOpID { get; set; }                         // 
        /// <summary>
        /// тип обтяження / Податкова застава   LM_TYPE=12 / Звернення стягнення LM_TYPE=13 / Заборона на рухоме майно LM_TYPE=7 / Арешт рухомого майна LM_TYPE = 8 / Застава рухомого майна LM_TYPE = 10 / Інше обтяження рухомого майна LM_TYPE=999
        /// </summary>
        public string lmType { get; set; } //   
        /// <summary>
        /// Стан запису (активний анульований) – в АПІ надходить тільки статус 1 - активний
        /// </summary>
        public string opStatus { get; set; } // 
        /// <summary>
        /// стан реєстрації обтяжень (тип операції - 1 реєстрація обтяження 2 -припинення обтяження)
        /// </summary>
        public string opType { get; set; } // 
        /// <summary>
        /// Термін дії: 
        /// </summary>
        public DateTime actTerm { get; set; } // 
        /// <summary>
        /// Архівна дата
        /// </summary>
        public string archiveDate { get; set; } // 
        /// <summary>
        /// Термін виконання зобов’язання
        /// </summary>
        public DateTime execTerm { get; set; } // 
        /// <summary>
        /// ???
        /// </summary>
        public string prevRegistration { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int reqReqID { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string bnBnID { get; set; }
        /// <summary>
        /// Реєстраційний номер обтяження (зовнішній ключ)
        /// </summary>
        public string regNum { get; set; } // 
        /// <summary>
        /// Розмір основного зобов’язання: сума
        /// </summary>
        public string contractSum { get; set; } // 
        /// <summary>
        /// Контрольна сума заяви (для АПІ не несе інформаційного навантаження)
        /// </summary>
        public string checkSum { get; set; } // 
        /// <summary>
        /// ???
        /// </summary>
        public string archiveNum { get; set; }
        /// <summary>
        /// Ознака наявності “Звернення стягнення”
        /// </summary>
        public string penaltyInit { get; set; } // 
        /// <summary>
        /// Розмір основного зобов’язання: валюта
        /// </summary>
        public string currencyType { get; set; } // 
        /// <summary>
        /// вид обтяження / • публічне обтяження(LM_SORT= 1) / • приватне обтяження(LM_SORT= 2)
        /// </summary>
        public string lmSort { get; set; } // 
        /// <summary>
        /// 1. Дозволено відчужувати / 2. Заборонено відчужувати / 3.	За погодженням з обтяжувачем
        /// </summary>
        public string alPossible { get; set; } // 
        /// <summary>
        /// Опис у довільному форматі типу обтяження
        /// </summary>
        public string lmTypeExtension { get; set; } // 
        /// <summary>
        /// Додаткові дані до обтяження
        /// </summary>
        public string additional { get; set; } // 
        /// <summary>
        /// ???
        /// </summary>
        public string archRegName { get; set; }
        /// <summary>
        /// Дата обтяження
        /// </summary>
        public DateTime regDate { get; set; } // 
        /// <summary>
        /// ???
        /// </summary>
        public int currentObjID { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string startDay { get; set; } // 
        /// <summary>
        /// ???
        /// </summary>
        public string startMonth { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string startYear { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public double validContractSum { get; set; }
        /// <summary>
        /// Опис майна
        /// </summary>
        public List<Property> properties { get; set; } // 
        /// <summary>
        /// Інформація про обтяжувача 
        /// </summary>
        public List<Subject> subjects { get; set; } // 
        /// <summary>
        /// ???
        /// </summary>
        public List<CauseDocument> causeDocuments { get; set; }
        /// <summary>
        /// Дата проведення операції формування витягу
        /// </summary>
        public DateTime opRegDate { get; set; } // 
        /// <summary>
        /// Інформація про реєстратора
        /// </summary>
        public string registrarInfo { get; set; }       // 
        /// <summary>
        /// Стан документа
        /// </summary>
        public string lmState { get; set; }             // 
        /// <summary>
        /// ???
        /// </summary>
        public List<Document> documents { get; set; }
        /// <summary>
        /// Назва стану документа
        /// </summary>
        public string lmStateName { get; set; }         // 
        /// <summary>
        /// Організація реєстратора, який сформував витяг
        /// </summary>
        public string currentObjName { get; set; }      // 
        /// <summary>
        /// Реєстратора, який сформував витяг
        /// </summary>
        public string currentRegistrar { get; set; }    // 
        /// <summary>
        /// Ознака наявності “Звернення стягнення”
        /// </summary>
        public string penalty { get; set; }             // 
    }

    public class PrTypeAttr
    {
        public string xtype { get; set; }
        public string name { get; set; }
        public string fieldLabel { get; set; }
        public string attrTyp { get; set; }
        public int dataType { get; set; }
        public bool enforceMaxLength { get; set; }
        public int daRnNum { get; set; }
        public string width { get; set; }
        public int flex { get; set; }
        public bool allowBlank { get; set; }
        public int maxLength { get; set; }
        public string vtype { get; set; }
    }

    public class Property
    {
        public int id { get; set; }
        public int? prParentID { get; set; }
        public string prPrID { get; set; }
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int opOpID { get; set; } // 
        public string adAdID { get; set; }
        public string reReID { get; set; }
        public string reLandRegNumHash { get; set; }
        public string reLetterHash { get; set; }
        public string reCadNumHash { get; set; }
        public string mvRegNum { get; set; }
        public string prType { get; set; }
        public string reLandRegNum { get; set; }
        public string reLandType { get; set; }
        public string reLetter { get; set; }
        public string reCadNum { get; set; }
        public string changeType { get; set; }
        public string prCategory { get; set; }
        public string mvSrNum { get; set; }
        public string otherRegNum { get; set; }
        public string mvSrNumHash { get; set; }
        public string mvRegNumHash { get; set; }
        public string otherRegNumHash { get; set; }
        public string additional { get; set; }
        public string prTypeExtension { get; set; }
        public string reLandTypeExtension { get; set; }
        public string otherRegNumNID { get; set; }
        public string isArchive { get; set; }
        /// <summary>
        /// Реєстраційний номер майна
        /// </summary>
        public string prRegNum { get; set; } // 
        public string prRegNumNID { get; set; }
        public string driRegDate { get; set; }
        public string driRegNum { get; set; }
        /// <summary>
        /// Опис майна
        /// </summary>
        public string fullExtension { get; set; } // 
        /// <summary>
        /// Масив переліку динамічних атрибутів та їх значень
        /// </summary>
        public List<string> prAttr { get; set; } // 
        public List<PrTypeAttr> prTypeAttr { get; set; }
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string addProperties { get; set; } // 
    }
    /// <summary>
    /// Інформація про обтяжувача 
    /// </summary>
    public class Subject // 
    {
        public int id { get; set; }
        /// <summary>
        /// Тип суб'єкта: / 1 (фіз.особа) / 2 (юр.особа)
        /// </summary>
        public string sbjType { get; set; } // 
        public string subjectParentID { get; set; }
        public string rusNID { get; set; }
        public string atuAtuID { get; set; }
        public string cnCnID { get; set; }
        public int rlRlID { get; set; }
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int opOpID { get; set; } // 
        public int ukrNID { get; set; }
        /// <summary>
        /// Ознака нерезедентності
        /// </summary>
        public string foreignSubject { get; set; } // 
        public string birthMonth { get; set; }
        public string codeAbsence { get; set; }
        public string code { get; set; }
        /// <summary>
        /// Тип змін (для АПІ не несе інформаційного навантаження)
        /// </summary>
        public string changeType { get; set; } // 
        public string birthYear { get; set; }
        public string birthDay { get; set; }
        public string addressIndex { get; set; }
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string additional { get; set; } // 
        public string birthPlace { get; set; }
        public string name { get; set; }
        public string simpleAddress { get; set; }
        public string addressDetails { get; set; }
        public string document { get; set; }
        public string rusName { get; set; }
        public string fullName { get; set; }
        public string dcCountry { get; set; }
        public string room { get; set; }
        public string dcRoomType { get; set; }
        public string dcBuildingType { get; set; }
        public string building { get; set; }
        public string dcObjectNumType { get; set; }
        public string objectNum { get; set; }
        public string dcHouseType { get; set; }
        public string house { get; set; }
        public string streetAtuID { get; set; }
        public string phone { get; set; }
        public string atuAtuStr { get; set; }
        public string rlName { get; set; }
        public string addSubject { get; set; }
    }

    public class CauseDocument
    {/// <summary>
     /// Id
     /// </summary>
        public int id { get; set; } // 
        /// <summary>
        /// Номер обтяження
        /// </summary>
        public int opOpID { get; set; } // 
        /// <summary>
        /// Дата видачі документа
        /// </summary>
        public DateTime pubDate { get; set; } // 
        /// <summary>
        /// Тип документа
        /// </summary>
        public string cdType { get; set; } // 
        /// <summary>
        /// Номер документа
        /// </summary>
        public string serNum { get; set; } // 
        /// <summary>
        /// Додаткові відомості
        /// </summary>
        public string additional { get; set; } // 
        /// <summary>
        /// Ким виданий
        /// </summary>
        public string publisher { get; set; } // 
        /// <summary>
        /// Опис типу документу
        /// </summary>
        public string cdTypeExtension { get; set; } // 
        /// <summary>
        /// ???
        /// </summary>
        public string cdParentID { get; set; }
        /// <summary>
        /// Тип зміни запису про документ (додавання, виправлення тощо)
        /// </summary>
        public string changeType { get; set; } // 
        /// <summary>
        /// Інформація про документ - підставу
        /// </summary>
        public string causeDocumentInfo { get; set; } // 
    }

    public class Document
    {
        public int docID { get; set; }
        public int regNum { get; set; }
        public int objObjID { get; set; }
        public int empEmpID { get; set; }
        public string docType { get; set; }
        public string generateddocument { get; set; }
        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public DateTime regDate { get; set; } // 
        public string seSeID { get; set; }
        public string docState { get; set; }
        public string docTypeName { get; set; }
        public string empFullName { get; set; }
    }
}
