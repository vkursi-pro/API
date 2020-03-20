using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.organizations;

namespace vkursi_api_example.movableloads
{
    public class GetPayMovableLoadsClass
    {
        /*
         
        23. ДРОРМ отримання витяга
        [POST] api/1.0/MovableLoads/getpaymovableloads
        
        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw '{"Id":1278898}'
             
         */

        public static GetPayMovableLoadsResponseModel GetPayMovableLoads(string token, int movableId)
        {
            if (String.IsNullOrEmpty(token))
                token = AuthorizeClass.Authorize();

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetPayMovableLoadsRequestBodyModel GPMLRequestBody = new GetPayMovableLoadsRequestBodyModel
                {
                    Id = movableId
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GPMLRequestBody);

                // Example Body: {"Id":1278898}

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

    public class GetPayMovableLoadsRequestBodyModel
    {
        public int Id { get; set; }
    }

    public class GetPayMovableLoadsResponseModel
    {
        public bool isSuccess { get; set; }
        public string status { get; set; }
        public Data data { get; set; }
    }


    public class Data
    {
        public Guid userId { get; set; }
        public Dictionary<int, Guid> success { get; set; }
        public List<int> withError { get; set; }
        public string reportRef { get; set; }
        public Report report { get; set; }
    }

    public class Report
    {
        public DateTime createdOn { get; set; }
        public string dataObjectOriginal { get; set; } 
        public DataObjectClear dataObject { get; set; }
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
        public object prPrID { get; set; }
        public int opOpID { get; set; }
        public object adAdID { get; set; }
        public object reReID { get; set; }
        public object reLandRegNumHash { get; set; }
        public object reLetterHash { get; set; }
        public object reCadNumHash { get; set; }
        public string mvRegNum { get; set; }
        public string prType { get; set; }
        public object reLandRegNum { get; set; }
        public object reLandType { get; set; }
        public object reLetter { get; set; }
        public object reCadNum { get; set; }
        public string changeType { get; set; }
        public string prCategory { get; set; }
        public string mvSrNum { get; set; }
        public object otherRegNum { get; set; }
        public string mvSrNumHash { get; set; }
        public string mvRegNumHash { get; set; }
        public object otherRegNumHash { get; set; }
        public object additional { get; set; }
        public string prTypeExtension { get; set; }
        public object reLandTypeExtension { get; set; }
        public object otherRegNumNID { get; set; }
        public object isArchive { get; set; }
        public object prRegNum { get; set; }
        public object prRegNumNID { get; set; }
        public object driRegDate { get; set; }
        public object driRegNum { get; set; }
        public string fullExtension { get; set; }
        public List<object> prAttr { get; set; }
        public List<PrTypeAttr> prTypeAttr { get; set; }
        public string addProperties { get; set; }
    }

    public class Subject
    {
        public int id { get; set; }
        public string sbjType { get; set; }
        public object subjectParentID { get; set; }
        public object rusNID { get; set; }
        public object atuAtuID { get; set; }
        public object cnCnID { get; set; }
        public int rlRlID { get; set; }
        public int opOpID { get; set; }
        public int ukrNID { get; set; }
        public object foreignSubject { get; set; }
        public object birthMonth { get; set; }
        public string codeAbsence { get; set; }
        public string code { get; set; }
        public string changeType { get; set; }
        public object birthYear { get; set; }
        public object birthDay { get; set; }
        public object addressIndex { get; set; }
        public object additional { get; set; }
        public object birthPlace { get; set; }
        public string name { get; set; }
        public string simpleAddress { get; set; }
        public object addressDetails { get; set; }
        public object document { get; set; }
        public object rusName { get; set; }
        public string fullName { get; set; }
        public object dcCountry { get; set; }
        public object room { get; set; }
        public string dcRoomType { get; set; }
        public string dcBuildingType { get; set; }
        public object building { get; set; }
        public string dcObjectNumType { get; set; }
        public object objectNum { get; set; }
        public string dcHouseType { get; set; }
        public object house { get; set; }
        public object streetAtuID { get; set; }
        public object phone { get; set; }
        public object atuAtuStr { get; set; }
        public string rlName { get; set; }
        public string addSubject { get; set; }
    }

    public class CauseDocument
    {
        public int id { get; set; }
        public int opOpID { get; set; }
        public DateTime pubDate { get; set; }
        public string cdType { get; set; }
        public string serNum { get; set; }
        public object additional { get; set; }
        public string publisher { get; set; }
        public string cdTypeExtension { get; set; }
        public object cdParentID { get; set; }
        public string changeType { get; set; }
        public string causeDocumentInfo { get; set; }
    }

    public class Document
    {
        public int docID { get; set; }
        public int regNum { get; set; }
        public int objObjID { get; set; }
        public int empEmpID { get; set; }
        public string docType { get; set; }
        public string generateddocument { get; set; }
        public DateTime regDate { get; set; }
        public object seSeID { get; set; }
        public string docState { get; set; }
        public string docTypeName { get; set; }
        public string empFullName { get; set; }
    }

    public class DataObjectClear
    {
        public int id { get; set; }
        public int opOpID { get; set; }
        public string lmType { get; set; }
        public string opStatus { get; set; }
        public string opType { get; set; }
        public DateTime actTerm { get; set; }
        public object archiveDate { get; set; }
        public DateTime execTerm { get; set; }
        public object prevRegistration { get; set; }
        public int reqReqID { get; set; }
        public object bnBnID { get; set; }
        public string regNum { get; set; }
        public string contractSum { get; set; }
        public string checkSum { get; set; }
        public object archiveNum { get; set; }
        public object penaltyInit { get; set; }
        public string currencyType { get; set; }
        public string lmSort { get; set; }
        public string alPossible { get; set; }
        public object lmTypeExtension { get; set; }
        public string additional { get; set; }
        public object archRegName { get; set; }
        public DateTime regDate { get; set; }
        public int currentObjID { get; set; }
        public object startDay { get; set; }
        public object startMonth { get; set; }
        public object startYear { get; set; }
        public double validContractSum { get; set; }
        public List<Property> properties { get; set; }
        public List<Subject> subjects { get; set; }
        public List<CauseDocument> causeDocuments { get; set; }
        public DateTime opRegDate { get; set; }
        public string registrarInfo { get; set; }
        public string lmState { get; set; }
        public List<Document> documents { get; set; }
        public string lmStateName { get; set; }
        public string currentObjName { get; set; }
        public string currentRegistrar { get; set; }
        public object penalty { get; set; }
    }
}
