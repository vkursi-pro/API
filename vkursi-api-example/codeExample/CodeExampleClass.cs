using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using vkursi_api_example.estate;
using vkursi_api_example.movableloads;
using vkursi_api_example.organizations;
using vkursi_api_example.person;

namespace vkursi_api_example.codeExample
{
    public class CodeExampleClass
    {
        // Приклад отримання списку пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій
        public void GetRelationsCodeExample(string token)
        {
            // Читаемо перелік кодів ЄДРПОУ з файлу
            List<string> QueryUserId = new List<string>();

            using (var sr = new StreamReader(@"C:\Users\user\Downloads\relationIdList.txt"))
            {
                string textLine = string.Empty;

                while ((textLine = sr.ReadLine()) != null)
                {
                    // textLine = textLine.PadLeft(8, '0');
                    QueryUserId.Add(textLine);
                }
            }

            // Шлях до файлу в який буде саписано результат
            DirectoryInfo filePath = new DirectoryInfo(@"C:\Users\user\Downloads");

            List<string> TextLineList = new List<string>();

            int count = 0;

            foreach (var item in QueryUserId)
            {
                count++;
                if ((count % 100) == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b{0}", count);
                    // Записуемо в файл по 100 записів
                    WriteOneLineToTxt(TextLineList, filePath, "ClientsState.txt");
                    TextLineList.Clear();
                }

                GetRelationsResponseModel GRResponse = GetRelationsClass.GetRelations(ref token, item, null);

                if (GRResponse != null && GRResponse.Data != null)
                {
                    //foreach (var itemData in GRResponse.Data)
                    //{
                    //    string textLine = item +
                    //        "\t" + itemData.Id +
                    //        "\t" + itemData.DirectionIn +
                    //        "\t" + itemData.Type +
                    //        "\t" + itemData.Name +
                    //        "\t" + itemData.Edrpou;

                    //    TextLineList.Add(textLine);
                    //}
                }
                else
                {

                }
            }
            // Записуемо решту в файл
            WriteOneLineToTxt(TextLineList, filePath, "ClientsState.txt");
        }

        // Запис в файл
        public static bool WriteOneLineToTxt(List<string> TextLineist, DirectoryInfo filePath, string fileName)
        {
            try
            {
                FileStream stream = new FileStream(filePath + @"\" + fileName, FileMode.Append, FileAccess.Write);

                using (StreamWriter sw = new StreamWriter(stream))
                {
                    foreach (var item in TextLineist)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        // Приклад отримання скорочених даних ДЗК за списком кадастрових номерів
        public static void GetPKKUinfo(string token)
        {
            List<string> FileTextLineList = new List<string>();

            using (var sr = new StreamReader(@"C:\Users\vadim\Desktop\cadNumb.txt"))
            {
                string textLine = string.Empty;

                while ((textLine = sr.ReadLine()) != null)
                {
                    FileTextLineList.Add(textLine?.Trim());
                }

                FileTextLineList = FileTextLineList.Distinct().ToList();
            }

            int count = 0;

            List<string> CadNumberList = new List<string>();

            List<KadastrMapApiDetailsEstate> KadastrMapApiDetails = new List<KadastrMapApiDetailsEstate>();

            foreach (var item in FileTextLineList)
            {
                count++;

                CadNumberList.Add(item);

                if ((count % 99) == 0 || count == FileTextLineList.Count)
                {
                    GetPKKUinfoResponseModel GetPKKUinfoResponse = GetPKKUinfoClass.GetPKKUinfo(ref token, CadNumberList);
                    CadNumberList.Clear();

                    KadastrMapApiDetails.AddRange(GetPKKUinfoResponse.Data);
                }
            }

            using (var writer = new StreamWriter(@"C:\Users\vadim\Desktop\cadNumbResult.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(KadastrMapApiDetails);
            }

            Console.WriteLine();
        }


        // Приклад Отримання Виконавчих проваджень по ФО
        public static void GetPersonEnforcements()
        {
            string path = @"C:\Users\vadim\Desktop\temp";

            string token = string.Empty;

            List<string> textLineList = FileReader(path + "\\ipn_name.txt");

            List<person.OrgEnforcementApiAnswerModelData> orgEnforcementApiAnswerList = new List<person.OrgEnforcementApiAnswerModelData>();

            int count = 0;

            foreach (var item in textLineList)
            {
                count++;
                if (count % 10 == 0)
                    Console.WriteLine("\b\b\b\b{0}", count);

                string[] textLineSplit = item.Split('\t');

                string[] nameSplit = textLineSplit[1].Split(' ');

                GetPersonEnforcementsResponseModel GetPersonEnforcementsResponseRow = GetPersonEnforcementsClass.GetPersonEnforcements(ref token, textLineSplit[0], nameSplit[0], nameSplit[1], nameSplit.Length  == 3 ? nameSplit[2] : null );

                if (GetPersonEnforcementsResponseRow.Data != null)
                    orgEnforcementApiAnswerList.AddRange(GetPersonEnforcementsResponseRow.Data);

                var orgEnforcementApiAnswerTemp2 = orgEnforcementApiAnswerList.Where(w => w.TotalCount > 0).ToList();

                var config2 = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };
                using (var writer = new StreamWriter(@"C:\Users\vadim\Desktop\temp\orgEnforcementApiAnswerTemp2.csv"))
                using (var csv = new CsvWriter(writer, config2))
                {
                    csv.WriteRecords(orgEnforcementApiAnswerTemp2);
                }

                Console.WriteLine();
            }

            var orgEnforcementApiAnswerTemp = orgEnforcementApiAnswerList.Where(w => w.TotalCount > 0).ToList();

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            using (var writer = new StreamWriter(@"C:\Users\vadim\Desktop\temp\orgEnforcementApiAnswerTemp.csv"))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(orgEnforcementApiAnswerTemp);
            }

            WriteJsonTextToTxt(JsonConvert.SerializeObject(orgEnforcementApiAnswerTemp, Formatting.Indented), new DirectoryInfo(path), DateTime.Now.ToString("dd.MM.yyyy") + "OrgEnforcementApiAnswerList.json");

            Console.WriteLine();
        }

        // Вичитка чайлу з кодами
        public static List<string> FileReader(string path)
        {
            // Читаемо перелік кодів ЄДРПОУ з файлу
            List<string> TextLineList = new List<string>();

            using (var sr = new StreamReader(path))
            {
                string textLine = string.Empty;

                while ((textLine = sr.ReadLine()) != null)
                {
                    // textLine = textLine.PadLeft(8, '0');
                    TextLineList.Add(textLine);
                }
            }

            return TextLineList;
        }

        // Приклад Отримання ДРОРМа
        public static void GetDrorm()
        {
            // Читаемо перелік кодів ЄДРПОУ з файлу
            List<string> TextLineList = new List<string>();

            using (var sr = new StreamReader(@"C:\Users\vadim\Desktop\tasIpn.txt"))
            {
                string textLine = string.Empty;

                while ((textLine = sr.ReadLine()) != null)
                {
                    // textLine = textLine.PadLeft(8, '0');
                    TextLineList.Add(textLine);
                }
            }

            int count = 0;

            string token = string.Empty;

            DirectoryInfo filePath = new DirectoryInfo(@"C:\Users\vadim\Desktop\temp");

            List<movableloads.Datum> data = new List<movableloads.Datum>();

            foreach (var item in TextLineList)
            {
                string codeIpm = item;

                count++;
                if ((count % 1000) == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b{0}", count);
                    // Записуемо в файл по 100 записів
                    //WriteJsonTextToTxt(JsonConvert.SerializeObject(data), filePath, "GetMovableLoadsResponse2.json");
                    //TextLineList.Clear();
                }


                GetMovableLoadsResponseModel GetMovableLoadsResponseRow = GetMovableLoadsClass.GetMovableLoads(ref token, null, codeIpm);

                while (GetMovableLoadsResponseRow.data == null)
                {
                    Console.WriteLine("GetMovableLoadsResponseRow.data == null");

                    System.Threading.Thread.Sleep(1000);

                    GetMovableLoadsResponseRow = GetMovableLoadsClass.GetMovableLoads(ref token, null, codeIpm);
                }

                if (GetMovableLoadsResponseRow.data.Count > 0)
                {
                    data.AddRange(GetMovableLoadsResponseRow.data);
                }

                Console.WriteLine(count + ". " + item + " | " + GetMovableLoadsResponseRow.data.Count);
            }

            WriteJsonTextToTxt(JsonConvert.SerializeObject(data, Formatting.Indented), filePath, "GetMovableLoadsResponse2.json");

            Console.WriteLine();
        }

        // 3. Запись в один json файл
        public static bool WriteJsonTextToTxt(string textLineRow, DirectoryInfo filePath, string fileName)
        {
            try
            {
                FileStream stream = new FileStream(filePath + @"\" + fileName, FileMode.Create, FileAccess.Write);

                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(textLineRow);
                    return true;
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.ToString();
                return false;
            }
        }
    }
}
