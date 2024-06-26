﻿using System;
using System.Collections.Generic;
using System.Text;

namespace vkursi_api_example.dictionary
{
    public static class CommonDictionaries
    {
        // 1. Стан суб’єкта
        public static Dictionary<int, string> GetOrganizationStateDict()
        {
            Dictionary<int, string> OrganizationStateDict = new Dictionary<int, string> 
            {
                { 1, "Зареєстровано"},
                { 2, "В стані припинення"},
                { 3, "Припинено"},
                { 4, "Відомості про банкрутство"},
                { 5, "Санація"},
                { 6, "Свідоцтво недійсне"},
                { 99, "Статус не визначено"}
            };

            return OrganizationStateDict;
        }

        // 2. Роль по відношенню до пов’язаного суб’єкта

        public static Dictionary<int, string> GetNaisPersonRoleDict()
        {
            Dictionary<int, string> NaisPersonRoleDict = new Dictionary<int, string>
            {
                {1, "cуб’єкт підприємницької діяльності"},
                {2, "підписант"},
                {3, "керівник"},
                {4, "засновник"},
                {5, "відокремлений підрозділ"},
                {6, "особа - управитель майна"},
                {7, "комісія з припинення (комісія з реорганізації, ліквідаційна комісія)"},
                {8, "голова комісії з припинення або ліквідатор"},
                {9, "правонаступник"},
                {10, "попередник"},
                {11, "керівник комісії з виділу"},
                {12, "член комісії з виділу"},
                {13, "ліквідатор"},
                {14, "керуючий санацією"},
                {15, "Розпорядник майна"},
                {16, "Заявник"},
                {17, "Керівний орган"},
                {18, "Уповноважена особа Фонду гарантування вкладів фізичних осіб"},
            };

            return NaisPersonRoleDict;
        }

        // 3.

        // 4. Перевіряючий орган regulatorId
        public static Dictionary<int, string> GetRegulatorDict()
        {
            Dictionary<int, string> RegulatorDict = new Dictionary<int, string> 
            {
                { 0, "Тестовий орган"},
                { 1, "Державна служба України з надзвичайних ситуацій"},
                { 2, "Державна служба України з питань безпечності харчових продуктів та захисту споживачів"},
                { 3, "Державна служба України з питань праці"},
                { 4, "Державна екологічна інспекція України"},
                { 5, "Пенсійний фонд України"},
                { 6, "Державна інспекція з енергетичного нагляду за режимами споживання електричної і теплової енергії"},
                { 7, "Державна архітектурно-будівельна інспекція України"},
                { 8, "Державна служба України з лікарських засобів та контролю за наркотиками"},
                { 9, "Державна служба України з безпеки на транспорті"},
                { 10, "Державне агентство рибного господарства України"},
                { 11, "Державна служба геології та надр України"},
                { 12, "Державна інспекція ядерного регулювання України"},
                { 13, "Національна комісія, що здійснює державне регулювання у сфері ринків фінансових послуг"},
                { 14, "Міністерство внутрішніх справ України"},
                { 15, "Державна служба України з питань геодезії, картографії та кадастру"},
                { 16, "Міністерство охорони здоров'я України"},
                { 17, "Національна комісія, що здійснює державне регулювання у сфері зв'язку та інформатизації"},
                { 18, "Державне агентство лісових ресурсів України"},
                { 19, "Державна служба якості освіти України"},
                { 20, "Національна комісія, що здійснює державне регулювання у сферах енергетики та комунальних послуг"},
                { 21, "Міністерство екології та природних ресурсів України"},
                { 22, "Державна архівна служба України"},
                { 23, "Міністерство економічного розвитку і торгівлі України"},
                { 24, "Міністерство освіти і науки України"},
                { 25, "Служба безпеки України"},
                { 26, "Державна служба морського та річкового транспорту України"},
                { 27, "Державна фіскальна служба України"}
            };

            return RegulatorDict;
        }

        // 5. Статус перевірки
        public static Dictionary<int, string> GetAuditsResultStatusDict()
        {
            Dictionary<int, string> AuditsResultStatusDict = new Dictionary<int, string>
            {
                { 1, "Проведено"},
                { 2, "Не буде проведена"},
                { 3, "Заплановано"},
                { 4, "Не проведено"}
            };

            return AuditsResultStatusDict;
        }


        // 6. Орган ліцензування
        public static Dictionary<int, string> GetOrganOfLicensesDict()
        {
            Dictionary<int, string> OrganOfLicensesDict = new Dictionary<int, string> 
            {
                { 1, "Державна архітектурно-будівельна інспекція України"},
                { 2, "Міністерство охорони здоров'я України"},
                { 3, "Державна фіскальна служба України"},
                { 4, "Національна комісія, що здійснює державне регулювання у сфері зв'язку та інформатизації"},
                { 5, "Міністерство економічного розвитку і торгівлі України"},
                { 6, "Державна служба України з надзвичайних ситуацій"},
                { 7, "Національна комісія, що здійснює державне регулювання у сфері ринків фінансових послуг"},
                { 8, "Національна комісія, що здійснює державне регулювання у сферах енергетики та комунальних послуг"},
                { 9, "Міністерство екології та природних ресурсів України"},
                { 10, "Національний банк України"},
                { 11, "Вища кваліфікаційна комісія адвокатури"},
                { 12, "Аудиторська палата України"},
                { 13, "Державна казначейська служба України"},
                { 14, "Міністерство аграрної політики та продовольства України"},
                { 15, "Державна служба України з безпеки на транспорті"}
            };

            return OrganOfLicensesDict;
        }


        // 7. Тип ліцензії
        public static Dictionary<int, string> GetTypeOfLicensesDict()
        {
            Dictionary<int, string> TypeOfLicensesDict = new Dictionary<int, string>
            {
                { 29, "ДАБІ дозвільний документ підрядника"},
                { 28, "ДАБІ дозвільний документ проектувальника"},
                { 27, "ДАБІ дозвільний документ замовника"},
                { 1, "Ліцензії державної архітектурно-будівельної інспекції України"},
                { 7, "Ліцензія на право здійснення діяльності з обігу наркотичних засобів"},
                { 2, "Реєстр місць провадження діяльності з оптової та роздрібної торгівлі ЛЗ"},
                { 39, "Реєстр платників акцизного податку з реалізації пального"},
                { 38, "Відомості з реєстру неприбуткових установ та організацій"},
                { 24, "Роздрібна торгівля алкогольними напоями - сидром та перрі (без додання спирту)"},
                { 23, "Ліцензія на оптову торгівлю пивом"},
                { 22, "Ліцензія на оптову торгівлю тютюном"},
                { 21, "Ліцензія на оптову торгівлю сидром та перрі"},
                { 20, "Право реалізації та зберігання безхазяйного та іншого майна, що перейшло у власність держави"},
                { 18, "Ліцензія на оптову торгівлю алкогольними напоями, крім сидру та перрі"},
                { 17, "Ліцензія на виробництво спирту, алкогольних напоїв та тютюнових виробів"},
                { 14, "Роздрібна торгівля алкогольними напоями (пивом)"},
                { 8, "Дозвіл на провадження митної брокерської діяльності"},
                { 4, "Роздрібна торгівля тютюновими виробами"},
                { 3, "Роздрібна торгівля алкогольними напоями"},
                { 9, "Відомості з реєстру операторів, провайдерів телекомунікацій"},
                { 6, "Ліцензія на здійснення діяльності у сфері телекомунікацій"},
                { 5, "Ліцензія на користування радіочастотним ресурсом України"},
                { 10, "Ліцензія на ведення туроператорської діяльності"},
                { 11, "Ліцензія на надання послуг і виконання робіт протипожежного призначення"},
                { 12, "Відомості з державного реєстру фінансових установ (НАЦКОМФІНПОСЛУГ)"},
                { 13, "Ліцензія НКРЕКП"},
                { 15, "Право на здійснення операцій у сфері поводження з небезпечними відходами"},
                { 19, "Реєстр банків України"},
                { 16, "Генеральна ліцензія на здійснення валютних операцій"},
                { 25, "Свідоцтво про право на заняття адвокатською діяльністю"},
                { 30, "Сертифікат аудитора"},
                { 26, "Відомості з реєстру аудиторів та суб’єктів аудиторської діяльності"},
                { 31, "Відомості з реєстру заяв про повернення суми бюджетного відшкодування ПДВ"},
                { 36, "Відомості з реєстру Мінагрополітики одержувачів компенсації відсоткової ставки за залученим кредитом"},
                { 35, "Відомості з реєстру розподілу коштів за програмою Державна підтримка галузі тваринництва "},
                { 34, "Відомості реєстру з одержувачів коштів на компенсацію вартості закупленого насіння рослин вітчизняної селекції"},
                { 33, "Реєстр сільськогосподарських товаровиробників отримувачів компенсації вартості техніки"},
                { 32, "Відомості з реєстру фінансової підтримки на розвиток фермерських господарств"},
                { 40, "Ліцензія на право перевезення пасажирів або вантажів автомобільним транспортом"},
                { 37, "Ліцензія на транспортний засіб"}
            };

            return TypeOfLicensesDict;
        }


        // 8. Країна

        // http://www.ukrstat.gov.ua/klasf/st_kls/PKKS.zip

        // 9. Регіон

        public static Dictionary<int, string> GetRegionDict()
        {
            Dictionary<int, string> RegionDict = new Dictionary<int, string>
            {
                { 0, "Регіон відсутній"},
                { 1, "Вінницька область"},
                { 2, "Волинська область"},
                { 3, "Дніпропетровська область"},
                { 4, "Донецька область"},
                { 5, "Житомирська область"},
                { 6, "Закарпатська область"},
                { 7, "Запорізька область"},
                { 8, "Івано-Франківська область"},
                { 9, "Київська область"},
                { 10, "Кіровоградська область"},
                { 11, "Луганська область"},
                { 12, "Львівська область"},
                { 13, "Миколаївська область"},
                { 14, "Одеська область"},
                { 15, "Полтавська область"},
                { 16, "Рівненська область"},
                { 17, "Сумська область"},
                { 18, "Тернопільська область"},
                { 19, "Харківська область"},
                { 20, "Херсонська область"},
                { 21, "Хмельницька область"},
                { 22, "Черкаська область"},
                { 23, "Чернігівська область"},
                { 24, "Чернівецька область"},
                { 25, "Київ"},
                { 26, "Севастополь"},
                { 27, "Автономна Республіка Крим"}
            };

            return RegionDict;
        }

        // 10. Тип публікації про банкрутство BankruptcyPublicationType

        public static Dictionary<int, string> GetBankruptcyPublicationTypeDict()
        {
            Dictionary<int, string> BankruptcyPublicationTypeDict = new Dictionary<int, string>
            {
                {1, "Оголошення про порушення справи про банкрутство"},
                {2, "Оголошення про порушення справи про банкрутство і відкриття процедури санації"},
                {3, "Оголошення про проведення аукціону з продажу майна"},
                {4, "Оголошення про проведення загальних зборів кредиторів"},
                {5, "Повідомлення про введення процедури санації"},
                {6, "Повідомлення про визнання боржника банкрутом і відкриття ліквідаційної процедури"},
                {7, "Повідомлення про поновлення провадження у справі про банкрутство у зв’язку з визнанням мирової угоди недійсною або її розірвання"},
                {8, "Повідомлення про прийняття до розгляду заяви про затвердження плану санації боржника"},
                {9, "Повідомлення про результати проведення аукціону з продажу майна"},
                {10, "Повідомлення про скасування аукціону з продажу майна"},
                {12, "Інше"}
    };

            return BankruptcyPublicationTypeDict;
        }

        // 11. Цільве призначення 

        public static Dictionary<int, string> GetPurposeDict()
        {
            Dictionary<int, string> PurposeDict = new Dictionary<int, string>
            {
                { 0,"не визначено"},
                { 1,"для ведення товарного сільськогосподарського виробництва"},
                { 2,"для ведення фермерського господарства"},
                { 3,"для ведення особистого селянського господарства"},
                { 4,"для ведення підсобного сільського господарства"},
                { 5,"для індивідуального садівництва"},
                { 6,"для колективного садівництва"},
                { 7,"для городництва"},
                { 8,"для сінокосіння і випасання худоби"},
                { 9,"для дослідних і навчальних цілей"},
                { 10,"для пропаганди передового досвіду ведення сільського господарства"},
                { 11,"для надання послуг у сільському господарстві"},
                { 12,"для розміщення інфраструктури оптових ринків сільськогосподарської продукції"},
                { 13,"для іншого сільськогосподарського призначення"},
                { 14,"для цілей підрозділів 01.01-01.13 та для збереження та використання земель природно-заповідного фонду"},
                { 15,"для будівництва і обслуговування житлового будинку господарських будівель і споруд (присадибна ділянка)"},
                { 16,"для колективного житлового будівництва"},
                { 17,"для будівництва і обслуговування багатоквартирного житлового будинку"},
                { 18,"для будівництва і обслуговування будівель тимчасового проживання"},
                { 19,"для будівництва індивідуальних гаражів"},
                { 20,"для колективного гаражного будівництва"},
                { 21,"для іншої житлової забудови"},
                { 22,"для цілей підрозділів 02.01-02.07 та для збереження та використання земель природно-заповідного фонду"},
                { 23,"для будівництва та обслуговування будівель органів державної влади та місцевого самоврядування"},
                { 24,"для будівництва та обслуговування будівель закладів освіти"},
                { 25,"для будівництва та обслуговування будівель закладів охорони здоров'я та соціальної допомоги"},
                { 26,"для будівництва та обслуговування будівель громадських та релігійних організацій"},
                { 27,"для будівництва та обслуговування будівель закладів культурно-просвітницького обслуговування"},
                { 28,"для будівництва та обслуговування будівель екстериторіальних організацій та органів"},
                { 29,"для будівництва та обслуговування будівель торгівлі"},
                { 30,"для будівництва та обслуговування об'єктів туристичної інфраструктури та закладів громадського харчування"},
                { 31,"для будівництва та обслуговування будівель кредитно-фінансових установ"},
                { 32,"для будівництва та обслуговування будівель ринкової інфраструктури"},
                { 33,"для будівництва та обслуговування будівель і споруд закладів науки"},
                { 34,"для будівництва та обслуговування будівель закладів комунального обслуговування"},
                { 35,"для будівництва та обслуговування будівель закладів побутового обслуговування"},
                { 36,"для розміщення та постійної діяльності органів мнс"},
                { 37,"для будівництва та обслуговування інших будівель громадської забудови"},
                { 38,"для цілей підрозділів 03.01-03.15 та для збереження та використання земель природно-заповідного фонду"},
                { 39,"для збереження та використання біосферних заповідників"},
                { 40,"для збереження та використання природних заповідників"},
                { 41,"для збереження та використання національних природних парків"},
                { 42,"для збереження та використання ботанічних садів"},
                { 43,"для збереження та використання зоологічних парків"},
                { 44,"для збереження та використання дендрологічних парків"},
                { 45,"для збереження та використання парків-пам'яток садово-паркового мистецтва"},
                { 46,"для збереження та використання заказників"},
                { 47,"для збереження та використання заповідних урочищ"},
                { 48,"для збереження та використання пам'яток природи"},
                { 49,"для збереження та використання регіональних ландшафтних парків"},
                { 50,"землі іншого природоохоронного призначення (земельні ділянки в межах яких є природні об'єкти що мають особливу наукову цінність та які надаються для збереження і використання цих об'єктів проведення наукових досліджень освітньої та виховної роботи)"},
                { 51,"для будівництва і обслуговування санаторно-оздоровчих закладів"},
                { 52,"для розробки родовищ природних лікувальних ресурсів"},
                { 53,"для інших оздоровчих цілей"},
                { 54,"для цілей підрозділів 06.01-06.03 та для збереження та використання земель природно-заповідного фонду"},
                { 55,"для будівництва та обслуговування об'єктів рекреаційного призначення"},
                { 56,"для будівництва та обслуговування об'єктів фізичної культури і спорту"},
                { 57,"для індивідуального дачного будівництва"},
                { 58,"для колективного дачного будівництва"},
                { 59,"для цілей підрозділів 07.01-07.04 та для збереження та використання земель природно-заповідного фонду"},
                { 60,"для забезпечення охорони об'єктів культурної спадщини"},
                { 61,"для розміщення та обслуговування музейних закладів"},
                { 62,"для іншого історико-культурного призначення"},
                { 63,"для цілей підрозділів 08.01-08.03 та для збереження та використання земель природно-заповідного фонду"},
                { 64,"для ведення лісового господарства і пов'язаних з ним послуг"},
                { 65,"для іншого лісогосподарського призначення"},
                { 66,"для цілей підрозділів 09.01-09.02 та для збереження та використання земель природно-заповідного фонду"},
                { 67,"для експлуатації та догляду за водними об'єктами"},
                { 68,"для облаштування та догляду за прибережними захисними смугами"},
                { 69,"для експлуатації та догляду за смугами відведення"},
                { 70,"для експлуатації та догляду за гідротехнічними іншими водогосподарськими спорудами і каналами"},
                { 71,"для догляду за береговими смугами водних шляхів"},
                { 72,"для сінокосіння"},
                { 73,"для рибогосподарських потреб"},
                { 74,"для культурно-оздоровчих потреб рекреаційних спортивних і туристичних цілей"},
                { 75,"для проведення науково-дослідних робіт"},
                { 76,"для будівництва та експлуатації гідротехнічних гідрометричних та лінійних споруд"},
                { 77,"для будівництва та експлуатації санаторіїв та інших лікувально-оздоровчих закладів у межах прибережних захисних смуг морів морських заток і лиманів"},
                { 78,"для цілей підрозділів 10.01-10.11 та для збереження та використання земель природно-заповідного фонду"},
                { 79,"для розміщення та експлуатації основних підсобних і допоміжних будівель та споруд підприємствами що пов'язані з користуванням надрами"},
                { 80,"для розміщення та експлуатації основних підсобних і допоміжних будівель та споруд підприємств переробної машинобудівної та іншої промисловості"},
                { 81,"для розміщення та експлуатації основних підсобних і допоміжних будівель та споруд будівельних організацій та підприємств"},
                { 82,"для розміщення та експлуатації основних підсобних і допоміжних будівель та споруд технічної інфраструктури (виробництва та розподілення газу постачання пари та гарячої води збирання очищення та розподілення води)"},
                { 83,"для цілей підрозділів 11.01-11.04 та для збереження та використання земель природно-заповідного фонду"},
                { 84,"для розміщення та експлуатації будівель і споруд залізничного транспорту"},
                { 85,"для розміщення та експлуатації будівель і споруд морського транспорту"},
                { 86,"для розміщення та експлуатації будівель і споруд річкового транспорту"},
                { 87,"для розміщення та експлуатації будівель і споруд автомобільного транспорту та дорожнього господарства"},
                { 88,"для розміщення та експлуатації будівель і споруд авіаційного транспорту"},
                { 89,"для розміщення та експлуатації об'єктів трубопровідного транспорту"},
                { 90,"для розміщення та експлуатації будівель і споруд міського електротранспорту"},
                { 91,"для розміщення та експлуатації будівель і споруд додаткових транспортних послуг та допоміжних операцій"},
                { 92,"для розміщення та експлуатації будівель і споруд іншого наземного транспорту"},
                { 93,"для цілей підрозділів 12.01-12.09 та для збереження та використання земель природно-заповідного фонду"},
                { 202,"для розміщення та експлуатації об'єктів дорожнього сервісу (для влаштування, експлуатації та обслуговування відкритої автостоянки)"},
                { 94,"для розміщення та експлуатації об'єктів і споруд телекомунікацій"},
                { 95,"для розміщення та експлуатації будівель та споруд об'єктів поштового зв'язку"},
                { 96,"для розміщення та експлуатації інших технічних засобів зв'язку"},
                { 97,"для цілей підрозділів 13.01-13.03 та для збереження та використання земель природно-заповідного фонду"},
                { 98,"для розміщення будівництва експлуатації та обслуговування будівель і споруд об'єктів енергогенеруючих підприємств установ і організацій"},
                { 99,"для розміщення будівництва експлуатації та обслуговування будівель і споруд об'єктів передачі електричної та теплової енергії"},
                { 100,"для цілей підрозділів 14.01-14.02 та для збереження та використання земель природно-заповідного фонду"},
                { 101,"для розміщення та постійної діяльності збройних сил україни"},
                { 102,"для розміщення та постійної діяльності внутрішніх військ мвс"},
                { 103,"для розміщення та постійної діяльності державної прикордонної служби україни"},
                { 104,"для розміщення та постійної діяльності служби безпеки україни"},
                { 105,"для розміщення та постійної діяльності державної спеціальної служби транспорту"},
                { 106,"для розміщення та постійної діяльності служби зовнішньої розвідки україни"},
                { 107,"для розміщення та постійної діяльності інших створених відповідно до законів україни військових формувань"},
                { 108,"для цілей підрозділів 15.01-15.07 та для збереження та використання земель природно-заповідного фонду"},
                { 109,"землі запасу (земельні ділянки кожної категорії земель які не надані у власність або користування громадянам чи юридичним особам)"},
                { 110,"землі резервного фонду (землі створені органами виконавчої влади або органами місцевого самоврядування у процесі приватизації сільськогосподарських угідь які були у постійному користуванні відповідних підприємств установ та організацій)"},
                { 111,"землі загального користування (землі будь-якої категорії які використовуються як майдани вулиці проїзди шляхи громадські пасовища сіножаті набережні пляжі парки зеленізони сквери бульвари водні об'єкти загального користування а також інші землі якщо рішенням відповідного органу державної влади чи місцевого самоврядування їх віднесено до земель загального користування)"},
                { 112,"для цілей підрозділів 16.00-18.00 та для збереження та використання земель природно-заповідного фонду"},
                { 113,"іншої комерційної діяльності"},
                { 114,"роздрібної торгівлі та комерційних послуг"},
                { 115,"Підприємств іншої промисловості"},
                { 116,"для ведення товарного сільськогосподарського виробництва"},
                { 117,"для ведення особистого підсобного господарства садівництва городництва сінокосіння і випасання худоби"},
                { 118,"для ведення дослідних і навчальних цілей пропаганди передового досвіду для ведення сільського господарства"},
                { 119,"для ведення підсобного сільського господарства"},
                { 120,"для ведення селянського (фермерського) господарства"},
                { 121,"для традиційних народних промислів і підприємницької діяльності"},
                { 122,"для іншого сільськогосподарського призначення"},
                { 123,"для індивідуального житлового гаражного і дачного будівництва"},
                { 124,"житлових житлово-будівельних гаражно і дачно-будівельних кооперативів"},
                { 125,"промисловості"},
                { 126,"добувної"},
                { 127,"металургії та оброблення металу"},
                { 128,"по виробництву та розполділенню енергії"},
                { 129,"по виробництву будівельних матеріалів (за винятком буд. майданчиків)"},
                { 130,"комерційного використання"},
                { 131,"автотехобслуговування"},
                { 132,"оптової торгівлі та складського господарства"},
                { 133,"роздрібної торгівлі та комерційних послуг"},
                { 134,"ринкової інфраструктури"},
                { 135,"досліджень і розробок"},
                { 136,"громадського призначення"},
                { 137,"державного управління та місцевого самоврядування"},
                { 138,"оборони"},
                { 139,"освіти"},
                { 140,"культури"},
                { 141,"охорони здоров'я і соціальних послуг"},
                { 142,"громадських та релігійних організацій"},
                { 143,"фізичної культури та спорту"},
                { 144,"екстериторіальних організацій і органів"},
                { 145,"іншого громадського призначення"},
                { 146,"змішаного використання"},
                { 147,"житлової забудови і промисловості"},
                { 148,"житлової забудови і комерційного використання"},
                { 149,"житлової забудови і громадського призначення"},
                { 150,"промисловості комерційного використання і громадського призначення"},
                { 151,"транспорту зв'язку"},
                { 152,"залізничного транспорту"},
                { 153,"автомобільного транспорту"},
                { 154,"трубопроводного транспорту"},
                { 155,"морського транспорту"},
                { 156,"внутрішнього водного транспорту"},
                { 157,"повітряного транспорту"},
                { 158,"трамвайного і тролейбусного транспорту"},
                { 159,"метрополітену"},
                { 160,"зв'язку і телекомунікацій"},
                { 161,"інші землі транспорту та зв'язку"},
                { 162,"природоохоронного призначення"},
                { 163,"оздоровчого призначення (для організації профілактики та лікування)"},
                { 164,"рекреаційного призначення"},
                { 165,"історико-культурного призначення"},
                { 166,"для ведення лісового господарства"},
                { 167,"водогосподарських підприємств"},
                { 168,"іншого призначення"},
                { 169,"резервного фонду"},
                { 170,"для ведення товарного сільськогосподарського виробництва"},
                { 171,"для ведення особистого підсобного господарства садівництва городництва сінокосіння і випасання худоби"},
                { 172,"для ведення дослідних і навчальних цілей пропаганди передового досвіду для ведення сільського господарства"},
                { 173,"для ведення підсобного сільського господарства"},
                { 174,"для ведення селянського (фермерського) господарства"},
                { 175,"для традиційних народних промислів і підприємницької діяльності"},
                { 176,"для іншого сільськогосподарського призначення"},
                { 177,"промисловості"},
                { 178,"добувної"},
                { 179,"металургії та оброблення металу"},
                { 180,"по виробництву та розподіленню електроенергії"},
                { 181,"по виробництву будівельних матеріалів (за винятком будівельних майданичиків)"},
                { 182,"підприємств іншої промисловості"},
                { 183,"транспорту зв'язку"},
                { 184,"залізничного транспорту"},
                { 185,"автомобільного транспорту"},
                { 186,"трубопроводного транспорту"},
                { 187,"морського транспорту"},
                { 188,"внутрішнього водного транспорту"},
                { 189,"повітрянного транспорту"},
                { 190,"трамвайного і тролейбусного транспорту"},
                { 191,"метрополітену"},
                { 192,"зв'язку і телекомунікацій"},
                { 193,"інші землі транспорту та зв'язку"},
                { 194,"оборони (для розміщення та постійної діяльності військових частин установ навчальних закладів підприємств організацій)"},
                { 195,"іншого призначення"},
                { 196,"природоохоронного"},
                { 197,"оздоровчого (для організації профілактики та лікування)"},
                { 198,"рекреаційного"},
                { 199,"історико-культурного"},
                { 200,"для ведення лісового господарства"},
                { 201,"водогосподарських підприємств"}
            };

            return PurposeDict;
        }

        // 12. Тип санкцій

        public static Dictionary<int, string> GetSanctionTypeDict()
        {
            Dictionary<int, string> SanctionTypeDict = new Dictionary<int, string>
            {
                { 1, "Спеціальні санкції МЕРТ"},
                { 2, "Чорний список АМКУ"},
                { 3, "Санкції РНБОУ"},
                { 4, "Санкції OFAC міністерства фінансів США"},
                { 6, "Фінансові санкції Великобританії"},
                { 5, "Фінансові санкції ЄС"}
            };

            return SanctionTypeDict;
        }

        // 13. Тип відношення 
        public static Dictionary<int, string> GetRelationTypeDict()
        {
            Dictionary<int, string> SanctionTypeDict = new Dictionary<int, string>
            {
                { 1, "Адреса"},
                { 2, "Керівник"},
                { 3, "Бенефіціар"},
                { 4, "Попередня назва"},
                { 5, "Попередній керівник"},
                { 6, "Попередня адреса"},
                { 7, "Попередній бенефіціар"},
                { 8, "Філія"},
                { 9, "Правонаступник"},
                { 10, "Підписант"},
                { 11, "Власник пакетів акцій"},
            };

            return SanctionTypeDict;
        }


        // 14. Довідник статусів власників речового майна StatusPropertyOwners

        public static Dictionary<int?, string> GetStatusPropertyOwnersDict()
        {
            Dictionary<int?, string> StatusPropertyOwnersDict = new Dictionary<int?, string>
            {
                { 11, "Власник"},
                { 1, "Вся земля"},
                { 3, "Обтяжувач"},
                { 4, "Особа, майно/права якої обтяжуються"},
                { 6, "Іпотекодержатель"},
                { 7, "Майновий поручитель"},
                { 8, "Іпотекодавець"},
                { 9, "Боржник"},
                { 10, "Особа, в інтересах якої встановлено обтяження"},
                { 12, "Правонабувач"},
                { 13, "Правокористувач"},
                { 14, "Землевласник"},
                { 15, "Землеволоділець"},
                { 16, "Інший"},
                { 17, "Наймач"},
                { 18, "Орендар"},
                { 19, "Наймодавець"},
                { 20, "Орендодавець"},
                { 21, "Управитель"},
                { 22, "Вигодонабувач"},
                { 23, "Установник"},
                { 25, "Довірчій власник"}
            };

            return StatusPropertyOwnersDict;
        }

        // 15. Довідник Перелік категорій власності (для GlobalTypeCount)

        public static Dictionary<int, string> GetGlobalTypeCountDict()
        {
            Dictionary<int, string> GlobalTypeCountDict = new Dictionary<int, string>
            {
                { 1, "Власність"},
                { 2, "Інше право"},
                { 3, "Іпотека"},
                { 4, "Обтяження"},
            };

            return GlobalTypeCountDict;
        }


        // 16. Довідник OwnershipCode

        public static Dictionary<int, string> GetOwnershipCodeDict()
        {
            Dictionary<int, string> OwnershipCodeDict = new Dictionary<int, string>
            {
                { 100, "Приватна власність"},
                { 200, "Комунальна власність"},
                { 300, "Державна власність"}
            };

            return OwnershipCodeDict;
        }

        // 17. DcGroupType типи об'єктів нерухомого майна

        public static Dictionary<int, string> GetDcGroupTypeDict()
        {

            Dictionary<int, string> DcGroupTypeDict = new Dictionary<int, string>
            {
                { 1, "Об'єкт нерухомого майна - будинок"},
                { 2, "Земельна ділянка"},
                { 3, "Об'єкт нерухомого майна - квартира"},
                { 4, "Об'єкт нерухомого майна = інше"}
            };

            return DcGroupTypeDict;
        }
    }
}
