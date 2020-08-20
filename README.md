<h2>Перелік методів Vkusi API:</h2>
<br>

<h3>1. Авторизація (отримання токена авторизації)</h3>

<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/token/AuthorizeClass.cs" target="_blank">[POST] /api/1.0/token/authorize</a></p>

AuthorizeClass.Authorize();

<p><b>Приклад відповіді: </b> <a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/AuthorizeResponse.json" target="_blank">AuthorizeResponse.json</a></p>

<br>
<br>

<h3>2. Запит на отримання скорочених даних по організаціям за кодом ЄДРПОУ</h3>

<p><b>Дані: </b>Назва організації, Код ЄДРПОУ, ПІБ Керівника, Статус реєстрації, Дані про реєстрацію платником ПДВ, Наявний податковий борг, Наявні санкції, Наявні виконавчі провадження, Express score, Відомості про платника ЄП</p>

<pre><code>curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/getorganizations' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: application/json' \
        --data-raw '{"code": ["40073472"]}'</code></pre>

<p><b>Приклад коду: </b><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrganizationsClass.cs" target="_blank">[POST] /api/1.0/organizations/getorganizations</a></p>

<p><b>Модель відповіді: </b><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrganizationsClass.cs#L118" target="_blank">GetOrganizationsResponseModel</a></p>

<p><b>Приклад відповіді: </b> <a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetOrganizationsResponse.json" target="_blank">GetOrganizationsResponse.json</a></p>

<br>
<br>

<h3>3. Запит на отримання коротких даних по ФОП за кодом ІПН</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetFopsClass.cs" target="_blank">[POST] /api/1.0/organizations/getfops</a></p>

GetFopsClass.GetFops("1841404820", token); // 3334800417

<p><b>Приклад відповіді: </b> <a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetFopsResponse.json" target="_blank">GetFopsResponse.json</a></p>

<br>
<br>

<h3>4. Реєстраційні дані мінюсту онлайн. Запит на отримання розширених реєстраційних даних по юридичним або фізичним осіб за кодом ЄДРПОУ / ІПН </h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAdvancedOrganizationClass.cs" target="_blank">[POST] /api/1.0/organizations/getadvancedorganization</a></p>

GetAdvancedOrganizationClass.GetAdvancedOrganization("1841404820", ref token); // 00131305

<p><b>Приклад відповіді: </b> <a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetAdvancedOrganizationResponse.json" target="_blank">GetAdvancedOrganizationResponse.json</a></p>

<br>
<br>

<h3>5. Отримання відомостей про наявні об'єкти нерухоммого майна у фізичних та юридичних осіб за кодом ЄДРПОУ або ІПН</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetEstateByCodeClass.cs" target="_blank">[GET] /api/1.0/estate/getestatebycode</a></p>

GetEstateByCodeClass.GetRealEstateRights("00131305", token);

<p><b>Приклад відповіді: </b> <a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetRealEstateRightsResponse.json" target="_blank">GetRealEstateRightsResponse.json</a></p>

<br>
<br>

<h3>6. Отримати дані щоденного моніторингу по компаніям які додані на моніторинг (стрічка користувача)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/changes/GetChangesClass.cs" target="_blank">[GET] /api/1.0/changes/getchanges</a></p>

GetChangesClass.GetChanges("28.10.2019", token);

<p><b>Приклад відповіді: </b> <a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetChangesResponse.json" target="_blank">GetChangesResponse.json</a></p>

<br>
<br>

<h3>7. Отримати перелік списків (які користувач створив на vkursi.pro/eventcontrol#/reestr). Списки в сервісі використовуються для зберігання контрагентів, витягів та довідок</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/monitoring/GetAllReestrClass.cs" target="_blank">[GET] /api/1.0/monitoring/getAllReestr</a></p>

GetAllReestrClass.GetAllReestr(token);

<br>
<br>

<h3>8. Додати новий список контрагентів (список також можна створиты з інтерфейсу на сторінці vkursi.pro/eventcontrol#/reestr). Списки в сервісі використовуються для зберігання контрагентів, витягів та довідок</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/monitoring/AddNewReestrClass.cs" target="_blank">[POST] /api/1.0/monitoring/addNewReestr</a></p>

AddNewReestrClass.AddNewReestr("Назва нового реєстру", token);

<br>
<br>

<h3>9. Запит на отримання аналітичних даних по організації за кодом ЄДРПОУ</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetAnalyticClass.cs" target="_blank">[POST] /api/1.0/organizations/getanalytic</a></p>

GetAnalyticClass.GetAnalytic("00131305", token);

<br>
<br>

<h3>10. Запит на отримання переліку судових документів організації за критеріями (контент та параметри документа можна отримати в методі /api/1.0/courtdecision/getdecisionbyid)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/courtdecision/GetDecisionsClass.cs" target="_blank">[POST] /api/1.0/courtdecision/getdecisions</a></p>

GetDecisionsClass.GetDecisions("00131305", 0, 1, 2, new List<string>() { "F545D851-6015-455D-BFE7-01201B629774" }, token);

<br>
<br>

<h3>11. Запит на отримання контенту судового рішення за id документа (id документа можна отримати в api/1.0/courtdecision/getdecisions)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/courtdecision/GetContentClass.cs" target="_blank">[POST] /api/1.0/courtdecision/getcontent</a></p>

GetContentClass.GetContent("84583482", token);

<br>
<br>

<h3>12. Додати контрагентів до списку (до списку vkursi.pro/eventcontrol#/reestr)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/monitoring/AddToControlClass.cs" target="_blank">[POST] /api/1.0/Monitoring/addToControl</a></p>

ddToControlClass.AddToControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

<br>
<br>

<h3>13. Видалити контрагентів зі списку</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/monitoring/RemoveFromControlClass.cs" target="_blank">[POST] /api/1.0/Monitoring/removeFromControl</a></p>

RemoveFromControlClass.RemoveFromControl("00131305", "1c891112-b022-4a83-ad34-d1f976c60a0b", token);

<br>
<br>

<h3>14. Отримання переліку кодів ЄДРПОУ або Id фізичних або юридичних осіб які знаходятся за певним КОАТУУ</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetInfoByKoatuuClass.cs" target="_blank">[POST] /api/1.0/organizations/getinfobykoatuu</a></p>

GetInfoByKoatuuClass.GetInfoByKoatuu("510900000", "1", token);

<br>
<br>

<h3>15. Новий бізнес. Запит на отримання списку новозареєстрованих фізичних та юридичних осіб</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetNewRegistrationClass.cs" target="_blank">[POST] /api/1.0/organizations/getnewregistration</a></p>

GetNewRegistrationClass.GetNewRegistration("29.10.2019", "1", 0, 10, true, true, token);

<br>
<br>

<h3>16. Видалити список контрагентів</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/monitoring/RemoveReestrClass.cs" target="_blank">[DELETE] /api/1.0/monitoring/removeReestr</a></p>

RemoveReestrClass.RemoveReestr("1c891112-b022-4a83-ad34-d1f976c60a0b", token);

<br>
<br>

<h3>17. Отримати перелік компаний які відібрані в модулі BI</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/bi/GetBiDataClass.cs" target="_blank">[POST] /api/1.0/bi/getbidata</a></p>

GetBiDataClass.GetBiData(null, 1000, token);
// New
GetDataBiInfoClass.GetDataBiInfo("1c891112-b022-4a83-ad34-d1f976c60a0b", 1000, DateTime.Parse("2019-11-28 19:00:52.059"), token);
// New 
GetDataBiChangeInfoClass.GetDataBiChangeInfo(DateTime.Parse("2019-11-28 19:00:52.059"), "1c891112-b022-4a83-ad34-d1f976c60a0b", false, 100, token);
// New
GetDataBiOrganizationInfoClass.GetDataBiOrganizationInfo(new List<string> { "1c891112-b022-4a83-ad34-d1f976c60a0b" }, new List<string> { "00131305" }, token);

<br>
<br>

<h3>18. Отримати перелік Label доступних в модулі BI</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/bi/GetBiImportLabelsClass.cs" target="_blank">[GET] /api/1.0/bi/getbiimportlabels</a></p>

GetBiImportLabelsClass.GetBiImportLabels(token);
// New
GetBiLabelsClass.GetBiLabels(token);

<br>
<br>

<h3>19. Отримання інформації з ДРРП, НГО, ДЗК + формування звіту по земельним ділянкам</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/EstateTaskApiClass.cs" target="_blank">[POST] /api/1.0/estate/estatecreatetaskapi</a></p>

EstateTaskApiClass.EstateCreateTaskApi(token);

<br>
<br>

<h3>20. Отримання інформації створені задачі (задачі на виконання запитів до ДРРП, НГО, ДЗК)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/EstateTaskApiClass.cs" target="_blank">[GET] /api/1.0/estate/getestatetasklist</a></p>

EstateTaskApiClass.GetEstateTaskList(token);

<br>
<br>

<h3>21. Отримання інформації про виконання формування звіту та запитів до ДРРП, НГО, ДЗК за TaskId</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/EstateTaskApiClass.cs" target="_blank">[POST] /api/1.0/estate/estategettaskdataapi</a></p>

EstateTaskApiClass.EstateGetTaskDataApi(token, "taskId", "7424955100:04:001:0511");

<br>
<br>

<h3>22. ДРОРМ отримання скороченных данных по ІПН / ЄДРПОУ</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/movableloads/GetMovableLoadsClass.cs" target="_blank">[POST] /api/1.0/movableLoads/getmovableloads</a></p>

GetMovableLoadsClass.GetMovableLoads(token, "36679626", "1841404820");

<br>
<br>

<h3>23. ДРОРМ отримання витяга</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/movableloads/GetPayMovableLoadsClass.cs" target="_blank">[POST] /api/1.0/MovableLoads/getpaymovableloads</a></p>

GetPayMovableLoadsClass.GetPayMovableLoads(token, 17374040);

<br>
<br>

<h3>24. ДРРП отримання скороченных данных по ІПН / ЄДРПОУ</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetEstatesClass.cs" target="_blank">[POST] /api/1.0/estate/GetEstates</a></p>

GetEstatesClass.GetEstates(token, "36679626", null);

<br>
<br>

<h3>25. Отримання повного витяга з реєстру нерухомого майна (ДРРП)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetAdvancedRrpReportClass.cs" target="_blank">[POST] /api/1.0/estate/getadvancedrrpreport</a></p>

GetAdvancedRrpReportClass.GetAdvancedRrpReport(token, 5001466269723, 68345530);

<br>
<br>

<h3>26. Рекізити судового документа</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/courtdecision/GetDecisionByIdClass.cs" target="_blank">[POST] /api/1.0/courtdecision/getdecisionbyid</a></p>

GetDecisionByIdClass.GetDecisionById("88234097", token);

<br>
<br>

<h3>27. Обьем ресурсів доспупних користувачу відповідно до тарифного плану</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/token/GetTariffClass.cs" target="_blank">[GET] /api/1.0/token/gettariff</a></p>

GetTariffClass.GetTariff(token);

<br>
<br>

<h3>28. Метод АРІ, який віддає історію по компанії з можливістю обрати період.</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/changes/GetChangesByCodeClass.cs" target="_blank">[POST] /api/1.0/changes/getchangesbyCode</a></p>

GetChangesByCodeClass.GetChangesByCode(token, "00131305", "20.11.2018", "25.11.2019", null);

<br>
<br>

<h3>29. Отримання інформації по фізичній особі</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/person/CheckPersonClass.cs" target="_blank">[POST] /api/1.0/person/checkperson</a></p>

CheckPersonClass.CheckPerson(token, "ШЕРЕМЕТА ВАСИЛЬ АНАТОЛІЙОВИЧ", "2301715013");

<br>
<br>

<h3>30. ДРОРМ отримання витягів які були замовлені раніше в сервісі Vkursi</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/movableloads/GetExistedMovableloadsClass.cs" target="_blank">[POST] /api/1.0/movableloads/getexistedmovableloads</a></p>

<br>
<br>

<h3>31. Основні словники сервісу</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/dictionary/GetDictionaryClass.cs" target="_blank">[POST] /api/1.0/dictionary/getdictionary</a></p>

GetDictionaryClass.GetDictionary(ref token, 0);

<br>
<br>

<h3>32. Інформація про наявний авто транспорт за кодом ІПН / ЄДРПОУ</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgVehicleClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgvehicle</a></p>

GetOrgVehicleClass.GetOrgVehicle(ref token, "00131305");

<br>
<br>

<h3>33. Список виконавчих проваджень по фізичним або юридичним особам за кодом ІПН / ЄДРПОУ</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgEnforcementsClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgenforcements</a></p>

GetOrgEnforcementsClass.GetOrgEnforcements(ref token, "00131305");

<br>
<br>

<h3>34. Загальна статистики по Edata (по компанії)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgPubliicFundsClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgpubliicfunds</a></p>

GetOrgPubliicFundsClass.GetOrgPubliicFunds(ref token, "00131305");

<br>
<br>

<h3>35. Фінансові ризики</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgFinancialRisksClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgFinancialRisks</a></p>

GetOrgFinancialRisksClass.GetOrgFinancialRisks(ref token, "00131305");

<br>
<br>

<h3>36. Перелік декларантів повязаних з компаніями</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetDeclarationsInfoClass.cs" target="_blank">[POST] /api/1.0/organizations/getdeclarationsinfo</a></p>

GetDeclarationsInfoClass.GetDeclarationsInfo(ref token, "00131305");

<br>
<br>

<h3>37. Перелік ліцензій, та дозволів</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgLicensesInfoClass.cs" target="_blank">[POST] /api/1.0/organizations/getorglicensesinfo</a></p>

GetOrgLicensesInfoClass.GetOrgLicensesInfo(ref token, "00131305");

<br>
<br>

<h3>38. Відомості про інтелектуальну власність (патенти, торгові марки, корисні моделі) які повязані по ПІБ з бенефіціарами підприємства</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgIntellectualPropertyClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgintellectualproperty</a></p>

GetOrgIntellectualPropertyClass.GetOrgIntellectualProperty(ref token, "00131305");

<br>
<br>

<h3>39. Відомості про власників пакетів акцій (від 5%)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgShareholdersClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgshareholders</a></p>

GetOrgShareholdersClass.GetOrgShareholders(token, "00131305");

<br>
<br>

<h3>40. Частка державних коштів в доході</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgStateFundsStatisticClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgstatefundsstatistic</a></p>

GetOrgStateFundsStatisticClass.GetOrgStateFundsStatistic(token, "00131305");

<br>
<br>

<h3>41. Отримати список пов'язаних з компанією бенеціціарів, керівників, адрес, власників пакетів акцій</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetRelationsClass.cs" target="_blank">[POST] /api/1.0/organizations/getrelations</a></p>

GetRelationsClass.GetRelations(ref token, "00131305", null);

<br>
<br>

<h3>42. Запит на отримання геопросторових даних ПККУ</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetCadastrCoordinatesClass.cs" target="_blank">[POST] /api/1.0/estate/getcadastrcoordinates</a></p>

GetCadastrCoordinatesClass.GetCadastrCoordinates(token, "0521685603:01:004:0001", "geojson");

<br>
<br>

<h3>43. Загальна характеристика по тендерам</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOrgTenderAnalyticClass.cs" target="_blank">[POST] /api/1.0/organizations/getorgtenderanalytic</a></p>

GetOrgTenderAnalyticClass.GetOrgTenderAnalytic(token, "00131305");

<br>
<br>

<h3>44. Офіційні повідомлення (ЄДР, SMIDA, Банкрутство)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetOfficialNoticesClass.cs" target="_blank">[POST] /api/1.0/organizations/getofficialnotices</a></p>

GetOfficialNoticesClass.GetOfficialNotices(token, "00131305");

<br>
<br>

<h3>45. Додати об'єкт до моніторингу нерухомості за номером ОНМ (sms rrp)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/EstatePutOnMonitoringClass.cs" target="_blank">[POST] /api/1.0/estate/estateputonmonitoring</a></p>

EstatePutOnMonitoringClass.EstatePutOnMonitoring(token, "1260724348000");

<br>
<br>

<h3>46. Змінити період моніторингу об'єкта нерухомості за номером ОНМ (sms rrp)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/EstateInCreaseMonitoringPeriodClass.cs" target="_blank">[POST] /api/1.0/estate/estateincreasemonitoringperiod</a></p>

EstateInCreaseMonitoringPeriodClass.EstateInCreaseMonitoringPeriod(token, 1260724348000);

<br>
<br>

<h3>47. Видалити об'єкт з мониторингу (sms rrp)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/EstateRemoveFromMonitoringClass.cs" target="_blank">[POST] /api/1.0/estate/estateremovefrommonitoring</a></p>

EstateRemoveFromMonitoringClass.EstateRemoveFromMonitoring(token, 1260724348000);

<br>
<br>

<h3>48. Отримати зміни по об'єкту шо на мониторингу (можлимо через webhook)</h3>
// [inprogress]

<br>
<br>

<h3>49.Перевірка наявності об'єкта за ОНМ (sms rrp)</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/SmsRrpSelectIsRealtyExistsClass.cs" target="_blank">[POST] /api/1.0/estate/SmsRrpSelectIsRealtyExists</a></p>

SmsRrpSelectIsRealtyExistsClass.SmsRrpSelectIsRealtyExists(token, 1260724348000);

<br>
<br>

<h3>51. Судові документі по ЮО/ФО</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/courtdecision/GetStanRozgliaduSpravClass.cs" target="_blank">[POST] api/1.0/CourtDecision/getStanRozgliaduSprav</a></p>

<br>
<br>

<h3>52. Оригінальний метод пошуку нерухомості Nais (короткі дані) </h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/estate/GetEstatesAdvancedSearchClass.cs" target="_blank">[POST] api/1.0/Estate/GetEstatesAdvancedSearch</a></p>

<br>
<br>

<h3>53. Фінансовий моніторинг пов'язаних осіб частина 1. Створення задачі</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/SetTaskCompanyDeclarationsAndCourtsClass.cs" target="_blank">[POST] api/1.0/Organizations/SetTaskCompanyDeclarationsAndCourts</a></p>

<br>
<br>

<h3>54. Фінансовий моніторинг пов'язаних осіб частина 2. Отримуємо результат виконання задачі</h3>
<p><a href="https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/organizations/GetTaskCompanyDeclarationsAndCourtsClass.cs" target="_blank">[POST] api/1.0/Organizations/GetTaskCompanyDeclarationsAndCourts</a></p>

GetTaskCompanyDeclarationsAndCourtsClass.GetTaskCompanyDeclarationsAndCourts(ref token, Guid.Parse("691e940c-b61e-4feb-ad1f-fa22c365633f"));

<p><b>Приклад відповіді: </b> <a href="https://raw.githubusercontent.com/vkursi-pro/API/master/vkursi-api-example/responseExample/GetTaskCompanyDeclarationsAndCourtsResponse.json" target="_blank">GetTaskCompanyDeclarationsAndCourtsResponse.json</a></p>

<br>
<br>


<h3>ДРРП отримання витягів які були замовлені раніше в сервісі Vkursi</h3>
// [inprogress] estate/GetRrp

<br>
<br>

<h3>Перелік статусів відповідей API</h3>
