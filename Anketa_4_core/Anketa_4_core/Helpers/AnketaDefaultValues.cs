using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;

namespace Anketa_4_core.Helpers
{
    public class AnketaDefaultValues
    {
        AnketaContext context;
        public AnketaDefaultValues()
        {
            context = new AnketaContext();

        }

        /// <summary>
        /// Добавить стандартные уровни резерва
        /// </summary>
        public void AddReservLevels()
        {
            context.ReservLevels.Add(new ReservLevel { ReservLevelName = "Резерв кадров молодых работников" });
            context.ReservLevels.Add(new ReservLevel { ReservLevelName = "Резерв кадров на должности начальников, замначальников отделов, служб, цехов, филиалов общества" });
            context.ReservLevels.Add(new ReservLevel { ReservLevelName = "Резерв кадров начальников, главных инженеров, замначальников филиалов общества" });
            context.ReservLevels.Add(new ReservLevel { ReservLevelName = "Резерв кадров начальников, замначальников, отделов администрации" });
            context.ReservLevels.Add(new ReservLevel { ReservLevelName = "Резерв кадров заместители генерального директора, главного инженера, главного бухгалтера" });
            context.SaveChanges();

        }

        /// <summary>
        /// Добавить роли респондентов
        /// </summary>
        public void AddRespondentRoles()
        {
            context.RespondentRoles.Add(new RespondentRole { Name = "Самооценка", isSelfMark = true });
            context.RespondentRoles.Add(new RespondentRole { Name = "Эксперт-руководитель", isSelfMark = false });
            context.RespondentRoles.Add(new RespondentRole { Name = "Эксперт-коллега", isSelfMark = false });
            context.RespondentRoles.Add(new RespondentRole { Name = "Эксперт-подчинённый", isSelfMark = false });
            context.SaveChanges();

        }


        /// <summary>
        /// Добавить филиалы
        /// </summary>
        public void AddFilials()
        {
            context.Filials.Add(new Filial { FilialName = "Filial_01" });
            context.Filials.Add(new Filial { FilialName = "Filial_02" });
            context.Filials.Add(new Filial { FilialName = "Filial_03" });
            context.Filials.Add(new Filial { FilialName = "Filial_04" });
            context.Filials.Add(new Filial { FilialName = "Filial_05" });
            context.Filials.Add(new Filial { FilialName = "Filial_06" });
            context.Filials.Add(new Filial { FilialName = "Filial_07" });
            context.Filials.Add(new Filial { FilialName = "Filial_08" });
            context.Filials.Add(new Filial { FilialName = "Filial_09" });
            context.Filials.Add(new Filial { FilialName = "Filial_10" });
            context.Filials.Add(new Filial { FilialName = "Filial_11" });
            context.Filials.Add(new Filial { FilialName = "Filial_12" });
            context.Filials.Add(new Filial { FilialName = "Filial_13" });
            context.Filials.Add(new Filial { FilialName = "Filial_14" });
            context.Filials.Add(new Filial { FilialName = "Filial_15" });
            context.Filials.Add(new Filial { FilialName = "Filial_16" });
            context.Filials.Add(new Filial { FilialName = "Filial_17" });
            context.Filials.Add(new Filial { FilialName = "Filial_18" });
            context.Filials.Add(new Filial { FilialName = "Filial_19" });
            context.Filials.Add(new Filial { FilialName = "Filial_20" });
            context.Filials.Add(new Filial { FilialName = "Filial_31" });
            context.Filials.Add(new Filial { FilialName = "Filial_32" });
            context.Filials.Add(new Filial { FilialName = "Filial_33" });
            context.Filials.Add(new Filial { FilialName = "Filial_34" });
            context.Filials.Add(new Filial { FilialName = "Filial_35" });
            context.Filials.Add(new Filial { FilialName = "Filial_36" });
            context.Filials.Add(new Filial { FilialName = "Filial_37" });
            context.Filials.Add(new Filial { FilialName = "Filial_38" });
            context.Filials.Add(new Filial { FilialName = "Filial_39" });
            context.Filials.Add(new Filial { FilialName = "Filial_40" });
            context.Filials.Add(new Filial { FilialName = "Filial_41" });
        }

        /// <summary>
        /// Добавить данные для теста Кеттела
        /// </summary>
        public void AddKettelTest()
        {
            KT_Question q001 = new KT_Question { Number = 1, Text = "Я хорошо понял  инструкцию, которую только что  прочитал:" },
                q002 = new KT_Question { Number = 2, Text = "И готов отвечать на каждый  вопрос так искренне, как только  возможно:" },
                q003 = new KT_Question { Number = 3, Text = "Я бы предпочел временами жить в доме, который находится:" },
                q004 = new KT_Question { Number = 4, Text = "Я чувствую в себе достаточно  сил, чтобы справиться со своими  трудностями:" },
                q005 = new KT_Question { Number = 5, Text = "Я чувствую некоторое беспокойство при виде диких животных, даже если они находятся в прочных клетках:" },
                q006 = new KT_Question { Number = 6, Text = "Я воздерживаюсь от критики  людей и их высказываний:" },
                q007 = new KT_Question { Number = 7, Text = "Я делаю саркастические (язвительные) замечания по поводу людей, если они этого, по-моему, заслуживают:" },
                q008 = new KT_Question { Number = 8, Text = "Мне больше нравится классическая, чем эстрадная музыка:" },
                q009 = new KT_Question { Number = 9, Text = "Если бы я увидел дерущимися  соседских детей, то я:" },
                q010 = new KT_Question { Number = 10, Text = "При общении с людьми я:" },
                q011 = new KT_Question { Number = 11, Text = "По-моему, интереснее быть:" },
                q012 = new KT_Question { Number = 12, Text = "Я остановился бы на улице  скорее, чтобы посмотреть на работу  художника, чем слушать, как ссорятся  люди:" },
                q013 = new KT_Question { Number = 13, Text = "Обычно я могу ладить с самодовольными  людьми, несмотря на то, что они  хвастаются или слишком много о себе воображают:" },
                q014 = new KT_Question { Number = 14, Text = "По лицу человека всегда можно  заметить, что он нечестный:" },
                q015 = new KT_Question { Number = 15, Text = "Было бы хорошо, если бы отпуск (каникулы) был более продолжителен, и каждый был бы обязан его использовать:" },
                q016 = new KT_Question { Number = 16, Text = "Я предпочел бы работу с  возможно большим, но непостоянным  заработком, чем работу со скромным, но постоянным окладом:" },
                q017 = new KT_Question { Number = 17, Text = "Я говорю о своих чувствах:" },
                q018 = new KT_Question { Number = 18, Text = "Время от времени у меня  возникает чувство неопределенной  опасности или внезапного страха  по непонятным причинам:" },
                q019 = new KT_Question { Number = 19, Text = "Когда меня неправильно критикуют  за что-то, в чем я не виноват, я:" },
                q020 = new KT_Question { Number = 20, Text = "За деньги можно купить почти  все:" },
                q021 = new KT_Question { Number = 21, Text = "Моим решением руководит больше:" },
                q022 = new KT_Question { Number = 22, Text = "Большинство людей  были бы больше счастливы, если  бы они были ближе друг к  другу и поступали так же, как  все:" },
                q023 = new KT_Question { Number = 23, Text = "Иногда, когда я смотрю в зеркало, мне трудно разобраться, где у меня правая, а где левая сторона:" },
                q024 = new KT_Question { Number = 24, Text = "При разговоре я предпочитаю:" },
                q025 = new KT_Question { Number = 25, Text = "После того как меня что-то  сильно рассердит, я довольно  быстро успокаиваюсь:" },
                q026 = new KT_Question { Number = 26, Text = "При одинаковом рабочем времени  и заработке было бы интереснее  работать:" },
                q027 = new KT_Question { Number = 27, Text = "На общественные должности меня  выбирали:" },
                q028 = new KT_Question { Number = 28, Text = "«Лопата» относится к «копать», как «нож» относится к:" },
                q029 = new KT_Question { Number = 29, Text = "Иногда я не могу заснуть  потому что какая-нибудь мысль не выходит из головы:" },
                q030 = new KT_Question { Number = 30, Text = "В своей жизни я почти всегда  достигаю поставленных целей:" },
                q031 = new KT_Question { Number = 31, Text = "Устаревший закон следует изменить:" },
                q032 = new KT_Question { Number = 32, Text = "Я чувствую себя  «не в своей тарелке», когда  мне приходится работать над  чем-нибудь, что требует быстрых  действий, результаты которых могут  повлиять на других людей:" },
                q033 = new KT_Question { Number = 33, Text = "Большинство знакомых считают меня интересным рассказчиком:" },
                q034 = new KT_Question { Number = 34, Text = "Когда я вижу неряшливых, неопрятных  людей, я:" },
                q035 = new KT_Question { Number = 35, Text = "Я чувствую себя немного не по себе, если неожиданно оказываюсь в центре внимания группы людей:" },
                q036 = new KT_Question { Number = 36, Text = "Я всегда рад оказаться среди  людей, например, в гостях, на танцах, коллективной встрече:" },
                q037 = new KT_Question { Number = 37, Text = "В школе я предпочитал (или предпочитаю):" },
                q038 = new KT_Question { Number = 38, Text = "Если меня назначают  руководителем чего-либо, я настаиваю  на том, чтобы мои указания  выполнялись, иначе я отказываюсь  от этой работы:" },
                q039 = new KT_Question { Number = 39, Text = "Важнее, чтобы родители:" },
                q040 = new KT_Question { Number = 40, Text = "Участвуя в групповой деятельности, я бы предпочел:" },
                q041 = new KT_Question { Number = 41, Text = "Время от времени у меня  появляется потребность в интересной  физической деятельности:" },
                q042 = new KT_Question { Number = 42, Text = "Я предпочел бы скорее общаться  с вежливыми людьми, чем с грубоватыми и любящими возражать:" },
                q043 = new KT_Question { Number = 43, Text = "Я чувствую себя очень униженным, когда меня критикуют в присутствии  группы людей:" },
                q044 = new KT_Question { Number = 44, Text = "Если меня вызывает начальство, то я:" },
                q045 = new KT_Question { Number = 45, Text = "В наше время требуется:" },
                q046 = new KT_Question { Number = 46, Text = "При чтении я сразу замечаю, когда автор произведения хочет  меня в чем-то убедить:" },
                q047 = new KT_Question { Number = 47, Text = "В юности я принимал участие  в нескольких спортивных мероприятиях:" },
                q048 = new KT_Question { Number = 48, Text = "Я поддерживаю порядок в моей комнате, все вещи всегда лежат на своих местах:" },
                q049 = new KT_Question { Number = 49, Text = "Иногда у меня  возникает чувство напряжения  и беспокойства, когда я вспоминаю, что произошло в течение дня:" },
                q050 = new KT_Question { Number = 50, Text = "Иногда я сомневаюсь, действительно ли люди, с которыми я разговариваю, интересуются тем, что я говорю:" },
                q051 = new KT_Question { Number = 51, Text = "Если бы пришлось выбирать, то  я предпочел бы быть:" },
                q052 = new KT_Question { Number = 52, Text = "На праздники и дни рождения я:" },
                q053 = new KT_Question { Number = 53, Text = "«Усталый» относится к «работе», как «гордый» к:" },
                q054 = new KT_Question { Number = 54, Text = "Какой из следующих предметов  по существу отличается от двух других:" },
                q055 = new KT_Question { Number = 55, Text = "Друзья меня подводили:" },
                q056 = new KT_Question { Number = 56, Text = "У меня есть качества, по которым  я определенно выше большинства  людей:" },
                q057 = new KT_Question { Number = 57, Text = "Когда я расстроен, я стараюсь скрыть свои чувства от других:" },
                q058 = new KT_Question { Number = 58, Text = "Я склонен посещать зрелищные  мероприятия и развлечения:" },
                q059 = new KT_Question { Number = 59, Text = "Я считаю, что  возможность вести себя непринужденно  важнее, чем хорошие манеры и  уважение к существующим правилам  поведения:" },
                q060 = new KT_Question { Number = 60, Text = "Обычно я молчу в присутствии  старших по возрасту, опыту и положению:" },
                q061 = new KT_Question { Number = 61, Text = "Мне трудно говорить или декламировать  перед большой группой людей:" },
                q062 = new KT_Question { Number = 62, Text = "У меня хорошее  чувство ориентировки в незнакомом  месте (мне легко сказать, где  север – восток – юг – запад):" },
                q063 = new KT_Question { Number = 63, Text = "Если кто-нибудь рассердится на  меня, то я:" },
                q064 = new KT_Question { Number = 64, Text = "Встречаясь с несправедливостью, я скорее склонен забыть об  этом, чем реагировать:" },
                q065 = new KT_Question { Number = 65, Text = "Из моей памяти часто выпадают  несущественные тривиальные вещи, например, названия улиц, магазинов:" },
                q066 = new KT_Question { Number = 66, Text = "Мне бы понравилась жизнь ветеринара, лечение и операции на животных:" },
                q067 = new KT_Question { Number = 67, Text = "Я ем со вкусом, не всегда  так аккуратно и тщательно  как другие люди:" },
                q068 = new KT_Question { Number = 68, Text = "Бывают времена, когда у меня  нет настроения видеть кого  бы то ни было:" },
                q069 = new KT_Question { Number = 69, Text = "Иногда меня предупреждают о  том, что в моем голосе и  манерах слишком проявляется  возбуждение:" },
                q070 = new KT_Question { Number = 70, Text = "В юности, если я расходился  во мнении с родителями, то  я:" },
                q071 = new KT_Question { Number = 71, Text = "Я предпочел бы заниматься  самостоятельной работой, а не  совместной с другими:" },
                q072 = new KT_Question { Number = 72, Text = "Мне бы больше понравилась  спокойная жизнь, чем слава и  шумный успех:" },
                q073 = new KT_Question { Number = 73, Text = "В большинстве случаев я чувствую себя зрелым человеком:" },
                q074 = new KT_Question { Number = 74, Text = "Замечания в  мой адрес, которые позволяют  себе некоторые люди, меня больше  расстраивают, чем помогают:" },
                q075 = new KT_Question { Number = 75, Text = "Я всегда способен управлять проявлением своих чувств:" },
                q076 = new KT_Question { Number = 76, Text = "Начиная работу над полезным  изобретением, я бы предпочел:" },
                q077 = new KT_Question { Number = 77, Text = "«Удивление» относится к «странный», как «страх» относится к:" },
                q078 = new KT_Question { Number = 78, Text = "Которая из последующих дробей  отличается от двух других:" },
                q079 = new KT_Question { Number = 79, Text = "Кажется, некоторые люди игнорируют  и избегают меня, хотя я не  знаю, почему:" },
                q080 = new KT_Question { Number = 80, Text = "Отношения ко мне людей не  соответствуют моим добрым намерениям:" },
                q081 = new KT_Question { Number = 81, Text = "Употребление нецензурных выражений  вызывает у меня возмущение, даже  если не присутствуют лица  другого пола:" },
                q082 = new KT_Question { Number = 82, Text = "У меня определенно меньше  друзей, чем у большинства людей:" },
                q083 = new KT_Question { Number = 83, Text = "Я бы очень не хотел находиться  в таком месте, где нет таких  людей, с которыми можно поговорить:" },
                q084 = new KT_Question { Number = 84, Text = "Люди иногда считают меня небрежным, хотя и думают, что я приятный  человек:" },
                q085 = new KT_Question { Number = 85, Text = "Волнение перед выступлением  в присутствии многих людей  я испытывал:" },
                q086 = new KT_Question { Number = 86, Text = "Когда я нахожусь в большой группе людей, то я предпочитаю молчать и предоставляю слово другим:" },
                q087 = new KT_Question { Number = 87, Text = "Я предпочитаю читать:" },
                q088 = new KT_Question { Number = 88, Text = "Когда люди пытаются мною командовать, то я поступаю как раз наоборот:" },
                q089 = new KT_Question { Number = 89, Text = "Начальник или члены моей семьи  критикуют меня только тогда, когда к этому действительно  есть повод:" },
                q090 = new KT_Question { Number = 90, Text = "На улицах или в магазинах  мне не нравится, когда некоторые  люди пристально разглядывают  других:" },
                q091 = new KT_Question { Number = 91, Text = "Во время длительной поездки  я бы предпочел:" },
                q092 = new KT_Question { Number = 92, Text = "В ситуациях, которые могут стать  опасными, я громко разговариваю, хотя это выглядит невежливо  и нарушает спокойствие:" },
                q093 = new KT_Question { Number = 93, Text = "Если знакомые плохо обращаются  со мной и показывают свою неприязнь, то:" },
                q094 = new KT_Question { Number = 94, Text = "Я смущаюсь, когда меня хвалят  или говорят мне комплименты:" },
                q095 = new KT_Question { Number = 95, Text = "Я бы предпочел иметь работу:" },
                q096 = new KT_Question { Number = 96, Text = "Чтобы быть информированным, я  предпочитаю получать сведения:" },
                q097 = new KT_Question { Number = 97, Text = "Мне нравится принимать активное участие в общественной работе:" },
                q098 = new KT_Question { Number = 98, Text = "При выполнении задания я удовлетворяюсь  только тогда, когда должное внимание  будет уделено всем мелочам:" },
                q099 = new KT_Question { Number = 99, Text = "Даже самые незначительные неудачи иногда меня слишком раздражают:" },
                q100 = new KT_Question { Number = 100, Text = "Сон у меня всегда крепкий, я никогда не хожу и не  разговариваю во сне:" },
                q101 = new KT_Question { Number = 101, Text = "Для меня интереснее работа, при  которой:" },
                q102 = new KT_Question { Number = 102, Text = "«Размер» так относится к «длине», как «нечестный» к:" },
                q103 = new KT_Question { Number = 103, Text = "«АБ» так относится к «ГВ», как «СР» относится к:" },
                q104 = new KT_Question { Number = 104, Text = "Когда люди ведут себя неразумно, то я:" },
                q105 = new KT_Question { Number = 105, Text = "Если кто-нибудь громко разговаривает, когда я слушаю музыку:" },
                q106 = new KT_Question { Number = 106, Text = "Меня лучше характеризовать как:" },
                q107 = new KT_Question { Number = 107, Text = "В общественных мероприятиях  я принимаю участие только  тогда, когда это нужно, а в  иных случаях избегаю их:" },
                q108 = new KT_Question { Number = 108, Text = "Быть осторожным и не ждать  хорошего лучше, чем быть оптимистом  и всегда ждать успеха:" },
                q109 = new KT_Question { Number = 109, Text = "Думая о трудностях в своей  работе, я:" },
                q110 = new KT_Question { Number = 110, Text = "Мне легко вступить в контакт  с людьми во время различных  общественных мероприятий:" },
                q111 = new KT_Question { Number = 111, Text = "Когда требуется немного дипломатии и умения убедить, чтобы побудить людей что-либо сделать, обычно об этом просят меня:" },
                q112 = new KT_Question { Number = 112, Text = "Интересно быть:" },
                q113 = new KT_Question { Number = 113, Text = "Если я уверен, что человек  несправедлив или ведет себя  эгоистично, я указываю на это, даже если это связано с  неприятностями:" },
                q114 = new KT_Question { Number = 114, Text = "Иногда я говорю глупости ради  шутки, чтобы удивить людей и  посмотреть, что они на это скажут:" },
                q115 = new KT_Question { Number = 115, Text = "Мне бы понравилось быть газетным  критиком в разделе драмы, театра, концертов:" },
                q116 = new KT_Question { Number = 116, Text = "У меня никогда не бывает  потребности что-нибудь рисовать  или вертеть в руках, ерзать на месте, когда приходится долго сидеть на собрании:" },
                q117 = new KT_Question { Number = 117, Text = "Если кто-нибудь говорит мне  что-то неправильное, то я скорее  подумаю:" },
                q118 = new KT_Question { Number = 118, Text = "Я чувствую, что мне угрожает  какое-то наказание, даже когда  я ничего плохого не сделал:" },
                q119 = new KT_Question { Number = 119, Text = "Мнение о том, что болезнь также  часто бывает от психических, как и от физических факторов, сильно преувеличено:" },
                q120 = new KT_Question { Number = 120, Text = "Торжественность и величие традиционных  церемоний следует сохранить:" },
                q121 = new KT_Question { Number = 121, Text = "Мысль о том, что люди подумают, будто я веду себя необычно  или странно, меня беспокоит:" },
                q122 = new KT_Question { Number = 122, Text = "Выполняя какое-либо дело, я бы  предпочел работать:" },
                q123 = new KT_Question { Number = 123, Text = "У меня бывают периоды, когда  мне трудно избавиться от чувства  жалости к себе:" },
                q124 = new KT_Question { Number = 124, Text = "Часто я слишком быстро начинаю  сердиться на людей:" },
                q125 = new KT_Question { Number = 125, Text = "Я всегда могу без труда  изменить свои старые привычки  и не возвращаться к прежнему:" },
                q126 = new KT_Question { Number = 126, Text = "Если бы зарплата была одинаковой, то я предпочел бы быть:" },
                q127 = new KT_Question { Number = 127, Text = "«Лучшее» так относится к «наихудшее», как «медленное» к:" },
                q128 = new KT_Question { Number = 128, Text = "Каким из приведенных ниже  сочетаний следует продолжить  буквенный ряд РООООРРОООРРР:" },
                q129 = new KT_Question { Number = 129, Text = "Когда приходит время осуществить  то, что я планировал и на  что надеялся, я обнаруживаю, что  уже пропало желание делать  это:" },
                q130 = new KT_Question { Number = 130, Text = "Большей частью я могу продолжать работать тщательно, не обращая внимания на шум, создаваемый другими:" },
                q131 = new KT_Question { Number = 131, Text = "Иногда я говорю посторонним  вещи, кажущиеся мне важными, независимо  от того, спрашивают ли они  об этом:" },
                q132 = new KT_Question { Number = 132, Text = "Много свободного времени я  провожу в разговорах с друзьями  о прошлых развлечениях, от которых  я получал удовольствие:" },
                q133 = new KT_Question { Number = 133, Text = "Мне нравится устраивать какие-нибудь  смелые рискованные выходки «смеха  ради»:" },
                q134 = new KT_Question { Number = 134, Text = "Вид неубранной комнаты очень  раздражает меня:" },
                q135 = new KT_Question { Number = 135, Text = "Я считаю себя общительным  открытым человеком:" },
                q136 = new KT_Question { Number = 136, Text = "В общении я:" },
                q137 = new KT_Question { Number = 137, Text = "Я люблю музыку:" },
                q138 = new KT_Question { Number = 138, Text = "Красота поэмы восхищает меня  больше, чем красота хорошо сделанного  оружия:" },
                q139 = new KT_Question { Number = 139, Text = "Если мое удачное замечание остается незамеченным окружающими, то я:" },
                q140 = new KT_Question { Number = 140, Text = "Мне бы понравилось работать  фотокорреспондентом:" },
                q141 = new KT_Question { Number = 141, Text = "Нужно быть осторожным в общении с незнакомыми, так как можно, например, заразиться:" },
                q142 = new KT_Question { Number = 142, Text = "При поездке за границу я  бы предпочел быть под руководством  экскурсовода, чем самому планировать  маршрут:" },
                q143 = new KT_Question { Number = 143, Text = "Меня справедливо считают упорным и трудолюбивым, но не слишком преуспевающим человеком:" },
                q144 = new KT_Question { Number = 144, Text = "Если люди пользуются моим  хорошим отношением в своих  интересах, то я не возмущаюсь  этим и вскоре об этом забываю:" },
                q145 = new KT_Question { Number = 145, Text = "Если при обсуждении какого-либо  вопроса среди участников возникает  ожесточенный спор, то я предпочитаю:" },
                q146 = new KT_Question { Number = 146, Text = "Я предпочитаю планировать что-либо  самостоятельно, без вмешательства и предложений со стороны других:" },
                q147 = new KT_Question { Number = 147, Text = "Иногда чувство зависти влияет  на мои действия:" },
                q148 = new KT_Question { Number = 148, Text = "Я твердо верю, что начальник  может быть не всегда прав, но он всегда имеет право  быть начальником:" },
                q149 = new KT_Question { Number = 149, Text = "Когда я думаю обо всем, что  еще предстоит сделать, у меня  появляется чувство напряженности:" },
                q150 = new KT_Question { Number = 150, Text = "Когда зрители мне что-либо  кричат во время игры, меня  это не трогает:" },
                q151 = new KT_Question { Number = 151, Text = "Интереснее быть:" },
                q152 = new KT_Question { Number = 152, Text = "Которое из следующих слов  не относится к двум другим:" },
                q153 = new KT_Question { Number = 153, Text = "«Пламя» так относится к «жар», как «роза» относится к:" },
                q154 = new KT_Question { Number = 154, Text = "У меня бывают яркие сновидения, мешающие мне спать:" },
                q155 = new KT_Question { Number = 155, Text = "Если на пути к успеху стоят  серьезные препятствия, я все-таки  предпочитаю рискнуть:" },
                q156 = new KT_Question { Number = 156, Text = "Когда я нахожусь в группе  людей, приступающих к какой-то  работе, то само собой получается, что я оказываюсь во главе  их:" },
                q157 = new KT_Question { Number = 157, Text = "Мне больше нравится в одежде  спокойная корректность, чем бросающаяся в глаза индивидуальность:" },
                q158 = new KT_Question { Number = 158, Text = "Мне больше нравится провести  вечер за спокойным любимым  занятием, чем в оживленной компании:" },
                q159 = new KT_Question { Number = 159, Text = "Я не обращаю внимания на  доброжелательные советы других, даже когда эти советы могли бы быть полезными:" },
                q160 = new KT_Question { Number = 160, Text = "В своих поступках я всегда  стараюсь придерживаться общепринятых  правил поведения:" },
                q161 = new KT_Question { Number = 161, Text = "Мне не очень нравится, когда смотрят, как я работаю:" },
                q162 = new KT_Question { Number = 162, Text = "Иногда приходится применять  силу, потому что не всегда  возможно добиться результата  с помощью утверждения:" },
                q163 = new KT_Question { Number = 163, Text = "В школе я предпочитал (предпочитаю):" },
                q164 = new KT_Question { Number = 164, Text = "Меня иногда огорчало, что обо  мне за глаза отзывались неодобрительно  без всяких к этому причин:" },
                q165 = new KT_Question { Number = 165, Text = "Разговор с простыми людьми, которые всегда придерживаются общепринятых правил и традиций:" },
                q166 = new KT_Question { Number = 166, Text = "Некоторые вещи настолько раздражают  меня, что предпочитаю вообще  не говорить на эти темы:" },
                q167 = new KT_Question { Number = 167, Text = "В воспитании важнее:" },
                q168 = new KT_Question { Number = 168, Text = "Люди считают меня положительным, спокойным человеком, которого не трогают превратности судьбы:" },
                q169 = new KT_Question { Number = 169, Text = "Я считаю, что общество должно  руководствоваться разумом и  отбросить старые привычки или  ненужные традиции:" },
                q170 = new KT_Question { Number = 170, Text = "Думаю, что в современном мире важнее разрешить:" },
                q171 = new KT_Question { Number = 171, Text = "Я лучше усваиваю материал:" },
                q172 = new KT_Question { Number = 172, Text = "Я предпочитаю идти своим путем  вместо того, чтобы действовать в соответствии с принятыми правилами:" },
                q173 = new KT_Question { Number = 173, Text = "Прежде чем выдвигать какой-либо  аргумент, я предпочитаю подождать, пока не буду убежден, что я  прав:" },
                q174 = new KT_Question { Number = 174, Text = "Мелочи иногда невыносимо «действуют  мне на нервы», хотя я и понимаю, что они не существенны:" },
                q175 = new KT_Question { Number = 175, Text = "Под влиянием момента я редко  говорю вещи, о которых потом  очень сожалею:" },
                q176 = new KT_Question { Number = 176, Text = "Если бы меня попросили участвовать  в шефской деятельности, то я  бы:" },
                q177 = new KT_Question { Number = 177, Text = "Которое из следующих слов  не относится к двум другим:" },
                q178 = new KT_Question { Number = 178, Text = "«Скоро» так относится к «никогда», как «близко» к:" },
                q179 = new KT_Question { Number = 179, Text = "Если я невольно нарушил правила  поведения, находясь в обществе, то я вскоре забываю об этом:" },
                q180 = new KT_Question { Number = 180, Text = "Меня считают человеком, которому  обычно в голову приходят хорошие идеи, когда нужно разрешить какую-либо проблему:" },
                q181 = new KT_Question { Number = 181, Text = "Я способен лучше проявить  себя:" },
                q182 = new KT_Question { Number = 182, Text = "Меня считают человеком, полным  энтузиазма:" },
                q183 = new KT_Question { Number = 183, Text = "Мне нравится работа, которая  требует перемен, разнообразия, командировок, даже если она связана с  некоторой опасностью:" },
                q184 = new KT_Question { Number = 184, Text = "Я довольно требовательный человек  и всегда настаиваю на том, чтобы все делалось по возможности  правильно:" },
                q185 = new KT_Question { Number = 185, Text = "Мне нравится работа, требующая  добросовестного отношения, точных  навыков и умений:" },
                q186 = new KT_Question { Number = 186, Text = "Я отношусь к типу энергичных  людей, которые всегда заняты:" },
                q187 = new KT_Question { Number = 187, Text = "Я уверен в том, что не пропустил  ни одного вопроса и на все  ответил как следует:" };


            KT_Answer a001_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q001 },
                a001_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q001 },
                a001_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q001 },
                a002_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q002 },
                a002_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q002 },
                a002_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q002 },
                a003_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "в обжитом городе", Question = q003 },
                a003_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q003 },
                a003_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "одиноко в глухих лесах", Question = q003 },
                a004_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "всегда", Question = q004 },
                a004_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "обычно", Question = q004 },
                a004_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "редко", Question = q004 },
                a005_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q005 },
                a005_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q005 },
                a005_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q005 },
                a006_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q006 },
                a006_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q006 },
                a006_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q006 },
                a007_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "обычно", Question = q007 },
                a007_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q007 },
                a007_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "никогда", Question = q007 },
                a008_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q008 },
                a008_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q008 },
                a008_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q008 },
                a009_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "дал бы им возможность договориться самим", Question = q009 },
                a009_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q009 },
                a009_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "рассудил бы их", Question = q009 },
                a010_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "с готовностью вступаю в разговор", Question = q010 },
                a010_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q010 },
                a010_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "предпочитаю спокойно оставаться в стороне", Question = q010 },
                a011_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "инженером-строителем", Question = q011 },
                a011_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q011 },
                a011_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "драматургом", Question = q011 },
                a012_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q012 },
                a012_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q012 },
                a012_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q012 },
                a013_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q013 },
                a013_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q013 },
                a013_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q013 },
                a014_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q014 },
                a014_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q014 },
                a014_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q014 },
                a015_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "согласен", Question = q015 },
                a015_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q015 },
                a015_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "не согласен", Question = q015 },
                a016_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "согласен", Question = q016 },
                a016_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q016 },
                a016_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "не согласен", Question = q016 },
                a017_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "только если это необходимо", Question = q017 },
                a017_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q017 },
                a017_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "охотно, когда представится возможность", Question = q017 },
                a018_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q018 },
                a018_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q018 },
                a018_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q018 },
                a019_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "не испытываю чувства вины", Question = q019 },
                a019_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q019 },
                a019_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "все же чувствую себя немного виноватым", Question = q019 },
                a020_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q020 },
                a020_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q020 },
                a020_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q020 },
                a021_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "сердце", Question = q021 },
                a021_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "сердце и разум в равной степени", Question = q021 },
                a021_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "разум", Question = q021 },
                a022_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q022 },
                a022_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q022 },
                a022_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q022 },
                a023_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q023 },
                a023_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q023 },
                a023_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q023 },
                a024_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "высказывать свои мысли так, как они приходят мне в голову", Question = q024 },
                a024_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q024 },
                a024_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "сначала сформулировать получше свои мысли", Question = q024 },
                a025_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q025 },
                a025_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q025 },
                a025_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q025 },
                a026_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "плотником или поваром", Question = q026 },
                a026_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q026 },
                a026_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "официантом в хорошем ресторане", Question = q026 },
                a027_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "очень редко", Question = q027 },
                a027_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q027 },
                a027_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "много раз", Question = q027 },
                a028_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«острый»", Question = q028 },
                a028_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«резать»", Question = q028 },
                a028_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«указывать»", Question = q028 },
                a029_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q029 },
                a029_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q029 },
                a029_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q029 },
                a030_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q030 },
                a030_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q030 },
                a030_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q030 },
                a031_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "только после основательного обсуждения", Question = q031 },
                a031_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q031 },
                a031_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "как можно скорее", Question = q031 },
                a032_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q032 },
                a032_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q032 },
                a032_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q032 },
                a033_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q033 },
                a033_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q033 },
                a033_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q033 },
                a034_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "принимаю их такими, как они есть", Question = q034 },
                a034_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q034 },
                a034_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "испытываю отвращение и возмущение", Question = q034 },
                a035_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q035 },
                a035_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q035 },
                a035_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q035 },
                a036_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q036 },
                a036_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q036 },
                a036_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q036 },
                a037_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "заниматься музыкой, пением", Question = q037 },
                a037_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q037 },
                a037_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "выпиливать и мастерить что-либо", Question = q037 },
                a038_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q038 },
                a038_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q038 },
                a038_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q038 },
                a039_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "помогали детям развивать свои чувства", Question = q039 },
                a039_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q039 },
                a039_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "обучали детей сдерживать свои чувства", Question = q039 },
                a040_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "постараться улучшить организацию работы", Question = q040 },
                a040_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q040 },
                a040_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "следить за результатами и соблюдением правил", Question = q040 },
                a041_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q041 },
                a041_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q041 },
                a041_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q041 },
                a042_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q042 },
                a042_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q042 },
                a042_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q042 },
                a043_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q043 },
                a043_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q043 },
                a043_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q043 },
                a044_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "пользуюсь случаем, чтобы попросить о чем-то нужном мне", Question = q044 },
                a044_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q044 },
                a044_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "боюсь, что это связано с какой-нибудь оплошностью в моей работе", Question = q044 },
                a045_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "больше спокойных, солидных людей", Question = q045 },
                a045_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q045 },
                a045_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "больше «идеалистов», планирующих лучшее будущее", Question = q045 },
                a046_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q046 },
                a046_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q046 },
                a046_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q046 },
                a047_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "иногда", Question = q047 },
                a047_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "довольно часто", Question = q047 },
                a047_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "многократно", Question = q047 },
                a048_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q048 },
                a048_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q048 },
                a048_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q048 },
                a049_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q049 },
                a049_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q049 },
                a049_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q049 },
                a050_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q050 },
                a050_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q050 },
                a050_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q050 },
                a051_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "лесником", Question = q051 },
                a051_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q051 },
                a051_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "учителем средней школы", Question = q051 },
                a052_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "люблю делать подарки", Question = q052 },
                a052_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "неопределенно", Question = q052 },
                a052_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "считаю, что делать подарки – довольно неприятная вещь", Question = q052 },
                a053_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«улыбка»", Question = q053 },
                a053_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«успех»", Question = q053 },
                a053_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«счастливый»", Question = q053 },
                a054_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "свеча", Question = q054 },
                a054_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "луна", Question = q054 },
                a054_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "электрический свет", Question = q054 },
                a055_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "очень редко", Question = q055 },
                a055_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q055 },
                a055_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "довольно часто", Question = q055 },
                a056_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q056 },
                a056_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q056 },
                a056_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q056 },
                a057_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q057 },
                a057_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q057 },
                a057_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q057 },
                a058_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "чаще, чем раз в неделю (те чаще, чем большинство", Question = q058 },
                a058_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "примерно раз в неделю (те как большинство", Question = q058 },
                a058_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "реже, чем раз в неделю (те реже, чем большинство", Question = q058 },
                a059_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q059 },
                a059_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q059 },
                a059_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q059 },
                a060_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q060 },
                a060_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q060 },
                a060_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q060 },
                a061_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q061 },
                a061_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q061 },
                a061_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q061 },
                a062_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q062 },
                a062_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q062 },
                a062_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q062 },
                a063_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "постараюсь его успокоить", Question = q063 },
                a063_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q063 },
                a063_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "раздражаюсь", Question = q063 },
                a064_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q064 },
                a064_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q064 },
                a064_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q064 },
                a065_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q065 },
                a065_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q065 },
                a065_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q065 },
                a066_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q066 },
                a066_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q066 },
                a066_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q066 },
                a067_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q067 },
                a067_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q067 },
                a067_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q067 },
                a068_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "очень редко", Question = q068 },
                a068_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q068 },
                a068_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "довольно часто", Question = q068 },
                a069_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q069 },
                a069_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее,", Question = q069 },
                a069_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q069 },
                a070_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "оставался при своем мнении", Question = q070 },
                a070_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q070 },
                a070_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "соглашался с их авторитетом", Question = q070 },
                a071_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q071 },
                a071_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q071 },
                a071_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q071 },
                a072_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q072 },
                a072_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q072 },
                a072_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q072 },
                a073_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q073 },
                a073_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q073 },
                a073_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q073 },
                a074_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "часто", Question = q074 },
                a074_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q074 },
                a074_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "никогда", Question = q074 },
                a075_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q075 },
                a075_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q075 },
                a075_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q075 },
                a076_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "разрабатывать его в лаборатории", Question = q076 },
                a076_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q076 },
                a076_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "заниматься его практической реализацией", Question = q076 },
                a077_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«смелый»", Question = q077 },
                a077_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«тревожный»", Question = q077 },
                a077_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«ужасный»", Question = q077 },
                a078_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "3/7", Question = q078 },
                a078_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "3/9", Question = q078 },
                a078_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "3/11", Question = q078 },
                a079_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q079 },
                a079_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q079 },
                a079_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q079 },
                a080_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "часто", Question = q080 },
                a080_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q080 },
                a080_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "никогда", Question = q080 },
                a081_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q081 },
                a081_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q081 },
                a081_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q081 },
                a082_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q082 },
                a082_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q082 },
                a082_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q082 },
                a083_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q083 },
                a083_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q083 },
                a083_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q083 },
                a084_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q084 },
                a084_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q084 },
                a084_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q084 },
                a085_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "довольно часто", Question = q085 },
                a085_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q085 },
                a085_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "почти никогда", Question = q085 },
                a086_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q086 },
                a086_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q086 },
                a086_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q086 },
                a087_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "реалистические описания военных и политических сражений", Question = q087 },
                a087_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q087 },
                a087_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "роман, где много чувств и воображения", Question = q087 },
                a088_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q088 },
                a088_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q088 },
                a088_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q088 },
                a089_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q089 },
                a089_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q089 },
                a089_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q089 },
                a090_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q090 },
                a090_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q090 },
                a090_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q090 },
                a091_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "читать что-нибудь серьезное, но интересное", Question = q091 },
                a091_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "неопределенно", Question = q091 },
                a091_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "провести время, беседуя с кем-нибудь из пассажиров", Question = q091 },
                a092_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q092 },
                a092_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q092 },
                a092_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q092 },
                a093_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "меня это совершенно не трогает", Question = q093 },
                a093_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q093 },
                a093_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "я расстраиваюсь", Question = q093 },
                a094_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q094 },
                a094_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q094 },
                a094_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q094 },
                a095_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "с постоянным окладом", Question = q095 },
                a095_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q095 },
                a095_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "с большим окладом, который бы зависел от моей способности показать людям, чего я стою", Question = q095 },
                a096_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "в общении с людьми", Question = q096 },
                a096_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q096 },
                a096_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "из литературы", Question = q096 },
                a097_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q097 },
                a097_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q097 },
                a097_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q097 },
                a098_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q098 },
                a098_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q098 },
                a098_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q098 },
                a099_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q099 },
                a099_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q099 },
                a099_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q099 },
                a100_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q100 },
                a100_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q100 },
                a100_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q100 },
                a101_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "нужно разговаривать с людьми", Question = q101 },
                a101_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q101 },
                a101_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нужно заниматься счетами и записями", Question = q101 },
                a102_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«тюрьма»", Question = q102 },
                a102_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«нарушение»", Question = q102 },
                a102_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«кража»", Question = q102 },
                a103_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«ПО»", Question = q103 },
                a103_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«ОП»", Question = q103 },
                a103_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«ТУ»", Question = q103 },
                a104_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "молчу", Question = q104 },
                a104_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q104 },
                a104_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "высказываю свое презрение", Question = q104 },
                a105_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "могу сосредоточиться на музыке, не отвлекаться", Question = q105 },
                a105_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q105 },
                a105_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "чувствую, что это портит мне удовольствие и раздражает", Question = q105 },
                a106_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "вежливого и спокойного", Question = q106 },
                a106_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q106 },
                a106_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "энергичного", Question = q106 },
                a107_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q107 },
                a107_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q107 },
                a107_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q107 },
                a108_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q108 },
                a108_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q108 },
                a108_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q108 },
                a109_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "стараюсь планировать заранее, прежде чем встретить трудность", Question = q109 },
                a109_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q109 },
                a109_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "считаю, что справлюсь с трудностями по мере того, как они возникнут", Question = q109 },
                a110_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q110 },
                a110_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q110 },
                a110_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q110 },
                a111_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q111 },
                a111_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q111 },
                a111_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q111 },
                a112_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "консультантом, помогающим людям выбирать профессию", Question = q112 },
                a112_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q112 },
                a112_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "руководителем технического предприятия", Question = q112 },
                a113_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q113 },
                a113_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q113 },
                a113_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q113 },
                a114_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q114 },
                a114_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q114 },
                a114_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q114 },
                a115_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q115 },
                a115_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q115 },
                a115_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q115 },
                a116_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q116 },
                a116_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q116 },
                a116_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q116 },
                a117_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "он – лжец", Question = q117 },
                a117_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q117 },
                a117_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "по-видимому, он плохо информирован", Question = q117 },
                a118_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "часто", Question = q118 },
                a118_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q118 },
                a118_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "никогда", Question = q118 },
                a119_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q119 },
                a119_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q119 },
                a119_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q119 },
                a120_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q120 },
                a120_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q120 },
                a120_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q120 },
                a121_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "очень", Question = q121 },
                a121_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "немного", Question = q121 },
                a121_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "совсем не беспокоит", Question = q121 },
                a122_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "в составе коллектива", Question = q122 },
                a122_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q122 },
                a122_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "самостоятельно", Question = q122 },
                a123_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "часто", Question = q123 },
                a123_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q123 },
                a123_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "никогда", Question = q123 },
                a124_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q124 },
                a124_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q124 },
                a124_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q124 },
                a125_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q125 },
                a125_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q125 },
                a125_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q125 },
                a126_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "адвокатом", Question = q126 },
                a126_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q126 },
                a126_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "пилотом или капитаном судна", Question = q126 },
                a127_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«быстрое»", Question = q127 },
                a127_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«лучшее»", Question = q127 },
                a127_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«быстрейшее»", Question = q127 },
                a128_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "ОРРР", Question = q128 },
                a128_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "ООРР", Question = q128 },
                a128_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "РООО", Question = q128 },
                a129_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q129 },
                a129_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q129 },
                a129_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q129 },
                a130_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q130 },
                a130_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q130 },
                a130_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q130 },
                a131_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q131 },
                a131_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q131 },
                a131_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q131 },
                a132_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q132 },
                a132_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q132 },
                a132_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q132 },
                a133_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q133 },
                a133_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q133 },
                a133_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q133 },
                a134_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q134 },
                a134_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q134 },
                a134_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q134 },
                a135_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q135 },
                a135_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q135 },
                a135_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q135 },
                a136_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "свободно проявляю свои чувства", Question = q136 },
                a136_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q136 },
                a136_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "держу свои переживания «при себе»", Question = q136 },
                a137_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "легкую, живую", Question = q137 },
                a137_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q137 },
                a137_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "чувствительную", Question = q137 },
                a138_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q138 },
                a138_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q138 },
                a138_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q138 },
                a139_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "смирюсь с этим", Question = q139 },
                a139_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q139 },
                a139_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "даю людям возможность услышать его еще раз", Question = q139 },
                a140_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q140 },
                a140_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q140 },
                a140_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q140 },
                a141_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q141 },
                a141_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q141 },
                a141_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q141 },
                a142_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q142 },
                a142_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q142 },
                a142_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q142 },
                a143_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q143 },
                a143_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q143 },
                a143_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q143 },
                a144_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q144 },
                a144_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q144 },
                a144_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q144 },
                a145_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "увидеть, кто же «победил»", Question = q145 },
                a145_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q145 },
                a145_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "чтобы спор разрешился мирно", Question = q145 },
                a146_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q146 },
                a146_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q146 },
                a146_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q146 },
                a147_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q147 },
                a147_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q147 },
                a147_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q147 },
                a148_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q148 },
                a148_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q148 },
                a148_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q148 },
                a149_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q149 },
                a149_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q149 },
                a149_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q149 },
                a150_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q150 },
                a150_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q150 },
                a150_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q150 },
                a151_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "художником", Question = q151 },
                a151_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q151 },
                a151_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "организатором культурных развлечений", Question = q151 },
                a152_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "любые", Question = q152 },
                a152_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "некоторые", Question = q152 },
                a152_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "большинство", Question = q152 },
                a153_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«шип»", Question = q153 },
                a153_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«красивые лепестки»", Question = q153 },
                a153_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«аромат»", Question = q153 },
                a154_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "часто", Question = q154 },
                a154_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "иногда", Question = q154 },
                a154_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "практически никогда", Question = q154 },
                a155_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q155 },
                a155_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q155 },
                a155_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q155 },
                a156_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q156 },
                a156_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q156 },
                a156_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q156 },
                a157_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q157 },
                a157_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q157 },
                a157_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q157 },
                a158_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q158 },
                a158_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q158 },
                a158_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q158 },
                a159_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "иногда", Question = q159 },
                a159_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "почти никогда", Question = q159 },
                a159_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "никогда", Question = q159 },
                a160_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q160 },
                a160_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q160 },
                a160_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q160 },
                a161_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q161 },
                a161_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q161 },
                a161_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q161 },
                a162_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q162 },
                a162_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q162 },
                a162_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q162 },
                a163_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "русский язык и литературу", Question = q163 },
                a163_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q163 },
                a163_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "математику или арифметику", Question = q163 },
                a164_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q164 },
                a164_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q164 },
                a164_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q164 },
                a165_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "часто вполне интересен и содержателен", Question = q165 },
                a165_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q165 },
                a165_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "раздражает меня, потому что ограничивается мелочами", Question = q165 },
                a166_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q166 },
                a166_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q166 },
                a166_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q166 },
                a167_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "относиться к ребенку с достаточной любовью", Question = q167 },
                a167_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q167 },
                a167_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "выработать нужные привычки и отношение к жизни", Question = q167 },
                a168_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q168 },
                a168_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q168 },
                a168_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q168 },
                a169_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q169 },
                a169_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q169 },
                a169_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q169 },
                a170_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "вопросы нравственности", Question = q170 },
                a170_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q170 },
                a170_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "разногласия между странами мира", Question = q170 },
                a171_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "читая хорошо написанную книгу", Question = q171 },
                a171_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q171 },
                a171_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "участвуя в обсуждении вопроса", Question = q171 },
                a172_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q172 },
                a172_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q172 },
                a172_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q172 },
                a173_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "всегда", Question = q173 },
                a173_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "обычно", Question = q173 },
                a173_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "только если это целесообразно", Question = q173 },
                a174_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q174 },
                a174_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q174 },
                a174_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q174 },
                a175_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q175 },
                a175_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q175 },
                a175_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q175 },
                a176_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "согласился", Question = q176 },
                a176_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q176 },
                a176_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "вежливо сказал, что занят", Question = q176 },
                a177_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "широкий", Question = q177 },
                a177_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "зигзагообразный", Question = q177 },
                a177_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "прямой", Question = q177 },
                a178_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "«нигде»", Question = q178 },
                a178_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "«далеко»", Question = q178 },
                a178_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "«где-то»", Question = q178 },
                a179_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q179 },
                a179_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q179 },
                a179_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q179 },
                a180_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q180 },
                a180_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q180 },
                a180_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q180 },
                a181_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "в трудных ситуациях, когда нужно сохранить самообладание", Question = q181 },
                a181_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q181 },
                a181_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "когда требуется умение ладить с людьми", Question = q181 },
                a182_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q182 },
                a182_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q182 },
                a182_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q182 },
                a183_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q183 },
                a183_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q183 },
                a183_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q183 },
                a184_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "верно", Question = q184 },
                a184_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q184 },
                a184_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "неверно", Question = q184 },
                a185_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q185 },
                a185_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "нечто среднее", Question = q185 },
                a185_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q185 },
                a186_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q186 },
                a186_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q186 },
                a186_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q186 },
                a187_1 = new KT_Answer { AnswerNumber = " а", AnswerText = "да", Question = q187 },
                a187_2 = new KT_Answer { AnswerNumber = " в", AnswerText = "не уверен", Question = q187 },
                a187_3 = new KT_Answer { AnswerNumber = " с", AnswerText = "нет", Question = q187 };

            KT_Category cat001 = new KT_Category { CategoryName = "A" },
                cat002 = new KT_Category { CategoryName = "B" },
                cat003 = new KT_Category { CategoryName = "C" },
                cat004 = new KT_Category { CategoryName = "E" },
                cat005 = new KT_Category { CategoryName = "F" },
                cat006 = new KT_Category { CategoryName = "G" },
                cat007 = new KT_Category { CategoryName = "H" },
                cat008 = new KT_Category { CategoryName = "I" },
                cat009 = new KT_Category { CategoryName = "L" },
                cat010 = new KT_Category { CategoryName = "M" },
                cat011 = new KT_Category { CategoryName = "N" },
                cat012 = new KT_Category { CategoryName = "O" },
                cat013 = new KT_Category { CategoryName = "Q1" },
                cat014 = new KT_Category { CategoryName = "Q2" },
                cat015 = new KT_Category { CategoryName = "Q3" },
                cat016 = new KT_Category { CategoryName = "Q4" };

            KT_KeyForCategory kT_Key001 = new KT_KeyForCategory { Category = cat001, Answer = a003_1, Score = 2 },
                kT_Key002 = new KT_KeyForCategory { Category = cat001, Answer = a003_2, Score = 1 },
                kT_Key003 = new KT_KeyForCategory { Category = cat003, Answer = a004_1, Score = 2 },
                kT_Key004 = new KT_KeyForCategory { Category = cat003, Answer = a004_2, Score = 1 },
                kT_Key005 = new KT_KeyForCategory { Category = cat003, Answer = a005_2, Score = 1 },
                kT_Key006 = new KT_KeyForCategory { Category = cat003, Answer = a005_3, Score = 2 },
                kT_Key007 = new KT_KeyForCategory { Category = cat004, Answer = a006_2, Score = 1 },
                kT_Key008 = new KT_KeyForCategory { Category = cat004, Answer = a006_3, Score = 2 },
                kT_Key009 = new KT_KeyForCategory { Category = cat004, Answer = a007_1, Score = 2 },
                kT_Key010 = new KT_KeyForCategory { Category = cat004, Answer = a007_2, Score = 1 },
                kT_Key011 = new KT_KeyForCategory { Category = cat005, Answer = a008_2, Score = 1 },
                kT_Key012 = new KT_KeyForCategory { Category = cat005, Answer = a008_3, Score = 2 },
                kT_Key013 = new KT_KeyForCategory { Category = cat006, Answer = a009_2, Score = 1 },
                kT_Key014 = new KT_KeyForCategory { Category = cat006, Answer = a009_3, Score = 2 },
                kT_Key015 = new KT_KeyForCategory { Category = cat007, Answer = a010_1, Score = 2 },
                kT_Key016 = new KT_KeyForCategory { Category = cat007, Answer = a010_2, Score = 1 },
                kT_Key017 = new KT_KeyForCategory { Category = cat008, Answer = a011_2, Score = 1 },
                kT_Key018 = new KT_KeyForCategory { Category = cat008, Answer = a011_3, Score = 2 },
                kT_Key019 = new KT_KeyForCategory { Category = cat008, Answer = a012_1, Score = 2 },
                kT_Key020 = new KT_KeyForCategory { Category = cat008, Answer = a012_2, Score = 1 },
                kT_Key021 = new KT_KeyForCategory { Category = cat009, Answer = a013_2, Score = 1 },
                kT_Key022 = new KT_KeyForCategory { Category = cat009, Answer = a013_3, Score = 2 },
                kT_Key023 = new KT_KeyForCategory { Category = cat010, Answer = a014_2, Score = 1 },
                kT_Key024 = new KT_KeyForCategory { Category = cat010, Answer = a014_3, Score = 2 },
                kT_Key025 = new KT_KeyForCategory { Category = cat010, Answer = a015_2, Score = 1 },
                kT_Key026 = new KT_KeyForCategory { Category = cat010, Answer = a015_3, Score = 2 },
                kT_Key027 = new KT_KeyForCategory { Category = cat011, Answer = a016_2, Score = 1 },
                kT_Key028 = new KT_KeyForCategory { Category = cat011, Answer = a016_3, Score = 2 },
                kT_Key029 = new KT_KeyForCategory { Category = cat011, Answer = a017_1, Score = 2 },
                kT_Key030 = new KT_KeyForCategory { Category = cat011, Answer = a017_2, Score = 1 },
                kT_Key031 = new KT_KeyForCategory { Category = cat012, Answer = a018_1, Score = 2 },
                kT_Key032 = new KT_KeyForCategory { Category = cat012, Answer = a018_2, Score = 1 },
                kT_Key033 = new KT_KeyForCategory { Category = cat012, Answer = a019_3, Score = 2 },
                kT_Key034 = new KT_KeyForCategory { Category = cat012, Answer = a019_2, Score = 1 },
                kT_Key035 = new KT_KeyForCategory { Category = cat013, Answer = a020_1, Score = 2 },
                kT_Key036 = new KT_KeyForCategory { Category = cat013, Answer = a020_2, Score = 1 },
                kT_Key037 = new KT_KeyForCategory { Category = cat013, Answer = a021_2, Score = 1 },
                kT_Key038 = new KT_KeyForCategory { Category = cat013, Answer = a021_3, Score = 2 },
                kT_Key039 = new KT_KeyForCategory { Category = cat014, Answer = a022_2, Score = 1 },
                kT_Key040 = new KT_KeyForCategory { Category = cat014, Answer = a022_3, Score = 2 },
                kT_Key041 = new KT_KeyForCategory { Category = cat015, Answer = a023_2, Score = 1 },
                kT_Key042 = new KT_KeyForCategory { Category = cat015, Answer = a023_3, Score = 2 },
                kT_Key043 = new KT_KeyForCategory { Category = cat015, Answer = a024_2, Score = 1 },
                kT_Key044 = new KT_KeyForCategory { Category = cat015, Answer = a024_3, Score = 2 },
                kT_Key045 = new KT_KeyForCategory { Category = cat016, Answer = a025_2, Score = 1 },
                kT_Key046 = new KT_KeyForCategory { Category = cat016, Answer = a025_3, Score = 2 },
                kT_Key047 = new KT_KeyForCategory { Category = cat001, Answer = a026_2, Score = 1 },
                kT_Key048 = new KT_KeyForCategory { Category = cat001, Answer = a026_3, Score = 2 },
                kT_Key049 = new KT_KeyForCategory { Category = cat001, Answer = a027_2, Score = 1 },
                kT_Key050 = new KT_KeyForCategory { Category = cat001, Answer = a027_3, Score = 2 },
                kT_Key051 = new KT_KeyForCategory { Category = cat002, Answer = a028_2, Score = 1 },
                kT_Key052 = new KT_KeyForCategory { Category = cat003, Answer = a029_2, Score = 1 },
                kT_Key053 = new KT_KeyForCategory { Category = cat003, Answer = a029_3, Score = 2 },
                kT_Key054 = new KT_KeyForCategory { Category = cat003, Answer = a030_1, Score = 2 },
                kT_Key055 = new KT_KeyForCategory { Category = cat003, Answer = a030_2, Score = 1 },
                kT_Key056 = new KT_KeyForCategory { Category = cat004, Answer = a031_2, Score = 1 },
                kT_Key057 = new KT_KeyForCategory { Category = cat004, Answer = a031_3, Score = 2 },
                kT_Key058 = new KT_KeyForCategory { Category = cat004, Answer = a032_2, Score = 1 },
                kT_Key059 = new KT_KeyForCategory { Category = cat004, Answer = a032_3, Score = 2 },
                kT_Key060 = new KT_KeyForCategory { Category = cat005, Answer = a033_1, Score = 2 },
                kT_Key061 = new KT_KeyForCategory { Category = cat005, Answer = a033_2, Score = 1 },
                kT_Key062 = new KT_KeyForCategory { Category = cat006, Answer = a034_2, Score = 1 },
                kT_Key063 = new KT_KeyForCategory { Category = cat006, Answer = a034_3, Score = 2 },
                kT_Key064 = new KT_KeyForCategory { Category = cat007, Answer = a035_2, Score = 1 },
                kT_Key065 = new KT_KeyForCategory { Category = cat007, Answer = a035_3, Score = 2 },
                kT_Key066 = new KT_KeyForCategory { Category = cat007, Answer = a036_1, Score = 2 },
                kT_Key067 = new KT_KeyForCategory { Category = cat007, Answer = a036_2, Score = 1 },
                kT_Key068 = new KT_KeyForCategory { Category = cat008, Answer = a037_1, Score = 2 },
                kT_Key069 = new KT_KeyForCategory { Category = cat008, Answer = a037_2, Score = 1 },
                kT_Key070 = new KT_KeyForCategory { Category = cat009, Answer = a038_1, Score = 2 },
                kT_Key071 = new KT_KeyForCategory { Category = cat009, Answer = a038_2, Score = 1 },
                kT_Key072 = new KT_KeyForCategory { Category = cat010, Answer = a039_1, Score = 2 },
                kT_Key073 = new KT_KeyForCategory { Category = cat010, Answer = a039_2, Score = 1 },
                kT_Key074 = new KT_KeyForCategory { Category = cat010, Answer = a040_1, Score = 2 },
                kT_Key075 = new KT_KeyForCategory { Category = cat010, Answer = a040_2, Score = 1 },
                kT_Key076 = new KT_KeyForCategory { Category = cat011, Answer = a041_2, Score = 1 },
                kT_Key077 = new KT_KeyForCategory { Category = cat011, Answer = a041_3, Score = 2 },
                kT_Key078 = new KT_KeyForCategory { Category = cat011, Answer = a042_1, Score = 2 },
                kT_Key079 = new KT_KeyForCategory { Category = cat011, Answer = a042_2, Score = 1 },
                kT_Key080 = new KT_KeyForCategory { Category = cat012, Answer = a043_1, Score = 2 },
                kT_Key081 = new KT_KeyForCategory { Category = cat012, Answer = a043_2, Score = 1 },
                kT_Key082 = new KT_KeyForCategory { Category = cat012, Answer = a044_2, Score = 1 },
                kT_Key083 = new KT_KeyForCategory { Category = cat012, Answer = a044_3, Score = 2 },
                kT_Key084 = new KT_KeyForCategory { Category = cat013, Answer = a045_2, Score = 1 },
                kT_Key085 = new KT_KeyForCategory { Category = cat013, Answer = a045_3, Score = 2 },
                kT_Key086 = new KT_KeyForCategory { Category = cat013, Answer = a046_1, Score = 2 },
                kT_Key087 = new KT_KeyForCategory { Category = cat013, Answer = a046_2, Score = 1 },
                kT_Key088 = new KT_KeyForCategory { Category = cat014, Answer = a047_1, Score = 2 },
                kT_Key089 = new KT_KeyForCategory { Category = cat014, Answer = a047_2, Score = 1 },
                kT_Key090 = new KT_KeyForCategory { Category = cat015, Answer = a048_1, Score = 2 },
                kT_Key091 = new KT_KeyForCategory { Category = cat015, Answer = a048_2, Score = 1 },
                kT_Key092 = new KT_KeyForCategory { Category = cat016, Answer = a049_1, Score = 2 },
                kT_Key093 = new KT_KeyForCategory { Category = cat016, Answer = a049_2, Score = 1 },
                kT_Key094 = new KT_KeyForCategory { Category = cat016, Answer = a050_1, Score = 2 },
                kT_Key095 = new KT_KeyForCategory { Category = cat016, Answer = a050_2, Score = 1 },
                kT_Key096 = new KT_KeyForCategory { Category = cat001, Answer = a051_2, Score = 1 },
                kT_Key097 = new KT_KeyForCategory { Category = cat001, Answer = a051_3, Score = 2 },
                kT_Key098 = new KT_KeyForCategory { Category = cat001, Answer = a052_2, Score = 1 },
                kT_Key099 = new KT_KeyForCategory { Category = cat001, Answer = a052_3, Score = 2 },
                kT_Key100 = new KT_KeyForCategory { Category = cat002, Answer = a053_2, Score = 1 },
                kT_Key101 = new KT_KeyForCategory { Category = cat002, Answer = a054_2, Score = 1 },
                kT_Key102 = new KT_KeyForCategory { Category = cat003, Answer = a055_1, Score = 2 },
                kT_Key103 = new KT_KeyForCategory { Category = cat003, Answer = a055_2, Score = 1 },
                kT_Key104 = new KT_KeyForCategory { Category = cat004, Answer = a056_1, Score = 2 },
                kT_Key105 = new KT_KeyForCategory { Category = cat004, Answer = a056_2, Score = 1 },
                kT_Key106 = new KT_KeyForCategory { Category = cat004, Answer = a057_2, Score = 1 },
                kT_Key107 = new KT_KeyForCategory { Category = cat004, Answer = a057_3, Score = 2 },
                kT_Key108 = new KT_KeyForCategory { Category = cat005, Answer = a058_1, Score = 2 },
                kT_Key109 = new KT_KeyForCategory { Category = cat005, Answer = a058_2, Score = 1 },
                kT_Key110 = new KT_KeyForCategory { Category = cat006, Answer = a059_2, Score = 1 },
                kT_Key111 = new KT_KeyForCategory { Category = cat006, Answer = a059_3, Score = 2 },
                kT_Key112 = new KT_KeyForCategory { Category = cat007, Answer = a060_2, Score = 1 },
                kT_Key113 = new KT_KeyForCategory { Category = cat007, Answer = a060_3, Score = 2 },
                kT_Key114 = new KT_KeyForCategory { Category = cat007, Answer = a061_2, Score = 1 },
                kT_Key115 = new KT_KeyForCategory { Category = cat007, Answer = a061_3, Score = 2 },
                kT_Key116 = new KT_KeyForCategory { Category = cat008, Answer = a062_2, Score = 1 },
                kT_Key117 = new KT_KeyForCategory { Category = cat008, Answer = a062_3, Score = 2 },
                kT_Key118 = new KT_KeyForCategory { Category = cat009, Answer = a063_2, Score = 1 },
                kT_Key119 = new KT_KeyForCategory { Category = cat009, Answer = a063_3, Score = 2 },
                kT_Key120 = new KT_KeyForCategory { Category = cat009, Answer = a064_2, Score = 1 },
                kT_Key121 = new KT_KeyForCategory { Category = cat009, Answer = a064_3, Score = 2 },
                kT_Key122 = new KT_KeyForCategory { Category = cat010, Answer = a065_1, Score = 2 },
                kT_Key123 = new KT_KeyForCategory { Category = cat010, Answer = a065_2, Score = 1 },
                kT_Key124 = new KT_KeyForCategory { Category = cat011, Answer = a066_2, Score = 1 },
                kT_Key125 = new KT_KeyForCategory { Category = cat011, Answer = a066_3, Score = 2 },
                kT_Key126 = new KT_KeyForCategory { Category = cat011, Answer = a067_2, Score = 1 },
                kT_Key127 = new KT_KeyForCategory { Category = cat011, Answer = a067_3, Score = 2 },
                kT_Key128 = new KT_KeyForCategory { Category = cat012, Answer = a068_2, Score = 1 },
                kT_Key129 = new KT_KeyForCategory { Category = cat012, Answer = a068_3, Score = 2 },
                kT_Key130 = new KT_KeyForCategory { Category = cat012, Answer = a069_1, Score = 2 },
                kT_Key131 = new KT_KeyForCategory { Category = cat012, Answer = a069_2, Score = 1 },
                kT_Key132 = new KT_KeyForCategory { Category = cat013, Answer = a070_1, Score = 2 },
                kT_Key133 = new KT_KeyForCategory { Category = cat013, Answer = a070_2, Score = 1 },
                kT_Key134 = new KT_KeyForCategory { Category = cat014, Answer = a071_1, Score = 2 },
                kT_Key135 = new KT_KeyForCategory { Category = cat014, Answer = a071_2, Score = 1 },
                kT_Key136 = new KT_KeyForCategory { Category = cat014, Answer = a072_1, Score = 2 },
                kT_Key137 = new KT_KeyForCategory { Category = cat014, Answer = a072_2, Score = 1 },
                kT_Key138 = new KT_KeyForCategory { Category = cat015, Answer = a073_1, Score = 2 },
                kT_Key139 = new KT_KeyForCategory { Category = cat015, Answer = a073_2, Score = 1 },
                kT_Key140 = new KT_KeyForCategory { Category = cat016, Answer = a074_1, Score = 2 },
                kT_Key141 = new KT_KeyForCategory { Category = cat016, Answer = a074_2, Score = 1 },
                kT_Key142 = new KT_KeyForCategory { Category = cat016, Answer = a075_2, Score = 1 },
                kT_Key143 = new KT_KeyForCategory { Category = cat016, Answer = a075_3, Score = 2 },
                kT_Key144 = new KT_KeyForCategory { Category = cat001, Answer = a076_2, Score = 1 },
                kT_Key145 = new KT_KeyForCategory { Category = cat001, Answer = a076_3, Score = 2 },
                kT_Key146 = new KT_KeyForCategory { Category = cat002, Answer = a077_3, Score = 1 },
                kT_Key147 = new KT_KeyForCategory { Category = cat002, Answer = a078_2, Score = 1 },
                kT_Key148 = new KT_KeyForCategory { Category = cat003, Answer = a079_2, Score = 1 },
                kT_Key149 = new KT_KeyForCategory { Category = cat003, Answer = a079_3, Score = 2 },
                kT_Key150 = new KT_KeyForCategory { Category = cat003, Answer = a080_2, Score = 1 },
                kT_Key151 = new KT_KeyForCategory { Category = cat003, Answer = a080_3, Score = 2 },
                kT_Key152 = new KT_KeyForCategory { Category = cat004, Answer = a081_2, Score = 1 },
                kT_Key153 = new KT_KeyForCategory { Category = cat004, Answer = a081_3, Score = 2 },
                kT_Key154 = new KT_KeyForCategory { Category = cat005, Answer = a082_2, Score = 1 },
                kT_Key155 = new KT_KeyForCategory { Category = cat005, Answer = a082_3, Score = 2 },
                kT_Key156 = new KT_KeyForCategory { Category = cat005, Answer = a083_1, Score = 2 },
                kT_Key157 = new KT_KeyForCategory { Category = cat005, Answer = a083_2, Score = 1 },
                kT_Key158 = new KT_KeyForCategory { Category = cat006, Answer = a084_2, Score = 1 },
                kT_Key159 = new KT_KeyForCategory { Category = cat006, Answer = a084_3, Score = 2 },
                kT_Key160 = new KT_KeyForCategory { Category = cat007, Answer = a085_2, Score = 1 },
                kT_Key161 = new KT_KeyForCategory { Category = cat007, Answer = a085_3, Score = 2 },
                kT_Key162 = new KT_KeyForCategory { Category = cat007, Answer = a086_2, Score = 1 },
                kT_Key163 = new KT_KeyForCategory { Category = cat007, Answer = a086_3, Score = 2 },
                kT_Key164 = new KT_KeyForCategory { Category = cat008, Answer = a087_2, Score = 1 },
                kT_Key165 = new KT_KeyForCategory { Category = cat008, Answer = a087_3, Score = 2 },
                kT_Key166 = new KT_KeyForCategory { Category = cat009, Answer = a088_1, Score = 2 },
                kT_Key167 = new KT_KeyForCategory { Category = cat009, Answer = a088_2, Score = 1 },
                kT_Key168 = new KT_KeyForCategory { Category = cat009, Answer = a089_2, Score = 1 },
                kT_Key169 = new KT_KeyForCategory { Category = cat009, Answer = a089_3, Score = 2 },
                kT_Key170 = new KT_KeyForCategory { Category = cat010, Answer = a090_2, Score = 1 },
                kT_Key171 = new KT_KeyForCategory { Category = cat010, Answer = a090_3, Score = 2 },
                kT_Key172 = new KT_KeyForCategory { Category = cat010, Answer = a091_1, Score = 2 },
                kT_Key173 = new KT_KeyForCategory { Category = cat010, Answer = a091_2, Score = 1 },
                kT_Key174 = new KT_KeyForCategory { Category = cat011, Answer = a092_2, Score = 1 },
                kT_Key175 = new KT_KeyForCategory { Category = cat011, Answer = a092_3, Score = 2 },
                kT_Key176 = new KT_KeyForCategory { Category = cat012, Answer = a093_2, Score = 1 },
                kT_Key177 = new KT_KeyForCategory { Category = cat012, Answer = a093_3, Score = 2 },
                kT_Key178 = new KT_KeyForCategory { Category = cat012, Answer = a094_1, Score = 2 },
                kT_Key179 = new KT_KeyForCategory { Category = cat012, Answer = a094_2, Score = 1 },
                kT_Key180 = new KT_KeyForCategory { Category = cat013, Answer = a095_2, Score = 1 },
                kT_Key181 = new KT_KeyForCategory { Category = cat013, Answer = a095_3, Score = 2 },
                kT_Key182 = new KT_KeyForCategory { Category = cat014, Answer = a096_2, Score = 1 },
                kT_Key183 = new KT_KeyForCategory { Category = cat014, Answer = a096_3, Score = 2 },
                kT_Key184 = new KT_KeyForCategory { Category = cat014, Answer = a097_2, Score = 1 },
                kT_Key185 = new KT_KeyForCategory { Category = cat014, Answer = a097_3, Score = 2 },
                kT_Key186 = new KT_KeyForCategory { Category = cat015, Answer = a098_1, Score = 2 },
                kT_Key187 = new KT_KeyForCategory { Category = cat015, Answer = a098_2, Score = 1 },
                kT_Key188 = new KT_KeyForCategory { Category = cat016, Answer = a099_1, Score = 2 },
                kT_Key189 = new KT_KeyForCategory { Category = cat016, Answer = a099_2, Score = 1 },
                kT_Key190 = new KT_KeyForCategory { Category = cat016, Answer = a100_2, Score = 1 },
                kT_Key191 = new KT_KeyForCategory { Category = cat016, Answer = a100_3, Score = 2 },
                kT_Key192 = new KT_KeyForCategory { Category = cat001, Answer = a101_1, Score = 2 },
                kT_Key193 = new KT_KeyForCategory { Category = cat001, Answer = a101_2, Score = 1 },
                kT_Key194 = new KT_KeyForCategory { Category = cat002, Answer = a102_3, Score = 1 },
                kT_Key195 = new KT_KeyForCategory { Category = cat002, Answer = a103_2, Score = 1 },
                kT_Key196 = new KT_KeyForCategory { Category = cat003, Answer = a104_1, Score = 2 },
                kT_Key197 = new KT_KeyForCategory { Category = cat003, Answer = a104_2, Score = 1 },
                kT_Key198 = new KT_KeyForCategory { Category = cat003, Answer = a105_1, Score = 2 },
                kT_Key199 = new KT_KeyForCategory { Category = cat003, Answer = a105_2, Score = 1 },
                kT_Key200 = new KT_KeyForCategory { Category = cat004, Answer = a106_2, Score = 1 },
                kT_Key201 = new KT_KeyForCategory { Category = cat004, Answer = a106_3, Score = 2 },
                kT_Key202 = new KT_KeyForCategory { Category = cat005, Answer = a107_2, Score = 1 },
                kT_Key203 = new KT_KeyForCategory { Category = cat005, Answer = a107_3, Score = 2 },
                kT_Key204 = new KT_KeyForCategory { Category = cat005, Answer = a108_2, Score = 1 },
                kT_Key205 = new KT_KeyForCategory { Category = cat005, Answer = a108_3, Score = 2 },
                kT_Key206 = new KT_KeyForCategory { Category = cat006, Answer = a109_1, Score = 2 },
                kT_Key207 = new KT_KeyForCategory { Category = cat006, Answer = a109_2, Score = 1 },
                kT_Key208 = new KT_KeyForCategory { Category = cat007, Answer = a110_1, Score = 2 },
                kT_Key209 = new KT_KeyForCategory { Category = cat007, Answer = a110_2, Score = 1 },
                kT_Key210 = new KT_KeyForCategory { Category = cat007, Answer = a111_1, Score = 2 },
                kT_Key211 = new KT_KeyForCategory { Category = cat007, Answer = a111_2, Score = 1 },
                kT_Key212 = new KT_KeyForCategory { Category = cat008, Answer = a112_1, Score = 2 },
                kT_Key213 = new KT_KeyForCategory { Category = cat008, Answer = a112_2, Score = 1 },
                kT_Key214 = new KT_KeyForCategory { Category = cat009, Answer = a113_1, Score = 2 },
                kT_Key215 = new KT_KeyForCategory { Category = cat009, Answer = a113_2, Score = 1 },
                kT_Key216 = new KT_KeyForCategory { Category = cat009, Answer = a114_1, Score = 2 },
                kT_Key217 = new KT_KeyForCategory { Category = cat009, Answer = a114_2, Score = 1 },
                kT_Key218 = new KT_KeyForCategory { Category = cat010, Answer = a115_1, Score = 2 },
                kT_Key219 = new KT_KeyForCategory { Category = cat010, Answer = a115_2, Score = 1 },
                kT_Key220 = new KT_KeyForCategory { Category = cat010, Answer = a116_1, Score = 2 },
                kT_Key221 = new KT_KeyForCategory { Category = cat010, Answer = a116_2, Score = 1 },
                kT_Key222 = new KT_KeyForCategory { Category = cat011, Answer = a117_1, Score = 2 },
                kT_Key223 = new KT_KeyForCategory { Category = cat011, Answer = a117_2, Score = 1 },
                kT_Key224 = new KT_KeyForCategory { Category = cat012, Answer = a118_1, Score = 2 },
                kT_Key225 = new KT_KeyForCategory { Category = cat012, Answer = a118_2, Score = 1 },
                kT_Key226 = new KT_KeyForCategory { Category = cat012, Answer = a119_1, Score = 2 },
                kT_Key227 = new KT_KeyForCategory { Category = cat012, Answer = a119_2, Score = 1 },
                kT_Key228 = new KT_KeyForCategory { Category = cat013, Answer = a120_2, Score = 1 },
                kT_Key229 = new KT_KeyForCategory { Category = cat013, Answer = a120_3, Score = 2 },
                kT_Key230 = new KT_KeyForCategory { Category = cat014, Answer = a121_2, Score = 1 },
                kT_Key231 = new KT_KeyForCategory { Category = cat014, Answer = a121_3, Score = 2 },
                kT_Key232 = new KT_KeyForCategory { Category = cat014, Answer = a122_2, Score = 1 },
                kT_Key233 = new KT_KeyForCategory { Category = cat014, Answer = a122_3, Score = 2 },
                kT_Key234 = new KT_KeyForCategory { Category = cat015, Answer = a123_2, Score = 1 },
                kT_Key235 = new KT_KeyForCategory { Category = cat015, Answer = a123_3, Score = 2 },
                kT_Key236 = new KT_KeyForCategory { Category = cat016, Answer = a124_1, Score = 2 },
                kT_Key237 = new KT_KeyForCategory { Category = cat016, Answer = a124_2, Score = 1 },
                kT_Key238 = new KT_KeyForCategory { Category = cat016, Answer = a125_2, Score = 1 },
                kT_Key239 = new KT_KeyForCategory { Category = cat016, Answer = a125_3, Score = 2 },
                kT_Key240 = new KT_KeyForCategory { Category = cat001, Answer = a126_1, Score = 2 },
                kT_Key241 = new KT_KeyForCategory { Category = cat001, Answer = a126_2, Score = 1 },
                kT_Key242 = new KT_KeyForCategory { Category = cat002, Answer = a127_3, Score = 1 },
                kT_Key243 = new KT_KeyForCategory { Category = cat002, Answer = a128_2, Score = 1 },
                kT_Key244 = new KT_KeyForCategory { Category = cat003, Answer = a129_2, Score = 1 },
                kT_Key245 = new KT_KeyForCategory { Category = cat003, Answer = a129_3, Score = 2 },
                kT_Key246 = new KT_KeyForCategory { Category = cat003, Answer = a130_1, Score = 2 },
                kT_Key247 = new KT_KeyForCategory { Category = cat003, Answer = a130_2, Score = 1 },
                kT_Key248 = new KT_KeyForCategory { Category = cat004, Answer = a131_1, Score = 2 },
                kT_Key249 = new KT_KeyForCategory { Category = cat004, Answer = a131_2, Score = 1 },
                kT_Key250 = new KT_KeyForCategory { Category = cat005, Answer = a132_1, Score = 2 },
                kT_Key251 = new KT_KeyForCategory { Category = cat005, Answer = a132_2, Score = 1 },
                kT_Key252 = new KT_KeyForCategory { Category = cat005, Answer = a133_1, Score = 2 },
                kT_Key253 = new KT_KeyForCategory { Category = cat005, Answer = a133_2, Score = 1 },
                kT_Key254 = new KT_KeyForCategory { Category = cat006, Answer = a134_1, Score = 2 },
                kT_Key255 = new KT_KeyForCategory { Category = cat006, Answer = a134_2, Score = 1 },
                kT_Key256 = new KT_KeyForCategory { Category = cat007, Answer = a135_1, Score = 2 },
                kT_Key257 = new KT_KeyForCategory { Category = cat007, Answer = a135_2, Score = 1 },
                kT_Key258 = new KT_KeyForCategory { Category = cat007, Answer = a136_1, Score = 2 },
                kT_Key259 = new KT_KeyForCategory { Category = cat007, Answer = a136_2, Score = 1 },
                kT_Key260 = new KT_KeyForCategory { Category = cat008, Answer = a137_2, Score = 1 },
                kT_Key261 = new KT_KeyForCategory { Category = cat008, Answer = a137_3, Score = 2 },
                kT_Key262 = new KT_KeyForCategory { Category = cat008, Answer = a138_1, Score = 2 },
                kT_Key263 = new KT_KeyForCategory { Category = cat008, Answer = a138_2, Score = 1 },
                kT_Key264 = new KT_KeyForCategory { Category = cat009, Answer = a139_2, Score = 1 },
                kT_Key265 = new KT_KeyForCategory { Category = cat009, Answer = a139_3, Score = 2 },
                kT_Key266 = new KT_KeyForCategory { Category = cat010, Answer = a140_1, Score = 2 },
                kT_Key267 = new KT_KeyForCategory { Category = cat010, Answer = a140_2, Score = 1 },
                kT_Key268 = new KT_KeyForCategory { Category = cat010, Answer = a141_2, Score = 1 },
                kT_Key269 = new KT_KeyForCategory { Category = cat010, Answer = a141_3, Score = 2 },
                kT_Key270 = new KT_KeyForCategory { Category = cat011, Answer = a142_1, Score = 2 },
                kT_Key271 = new KT_KeyForCategory { Category = cat011, Answer = a142_2, Score = 1 },
                kT_Key272 = new KT_KeyForCategory { Category = cat012, Answer = a143_1, Score = 2 },
                kT_Key273 = new KT_KeyForCategory { Category = cat012, Answer = a143_2, Score = 1 },
                kT_Key274 = new KT_KeyForCategory { Category = cat012, Answer = a144_2, Score = 1 },
                kT_Key275 = new KT_KeyForCategory { Category = cat012, Answer = a144_3, Score = 2 },
                kT_Key276 = new KT_KeyForCategory { Category = cat013, Answer = a145_1, Score = 2 },
                kT_Key277 = new KT_KeyForCategory { Category = cat013, Answer = a145_2, Score = 1 },
                kT_Key278 = new KT_KeyForCategory { Category = cat014, Answer = a146_1, Score = 2 },
                kT_Key279 = new KT_KeyForCategory { Category = cat014, Answer = a146_2, Score = 1 },
                kT_Key280 = new KT_KeyForCategory { Category = cat015, Answer = a147_2, Score = 1 },
                kT_Key281 = new KT_KeyForCategory { Category = cat015, Answer = a147_3, Score = 2 },
                kT_Key282 = new KT_KeyForCategory { Category = cat015, Answer = a148_1, Score = 2 },
                kT_Key283 = new KT_KeyForCategory { Category = cat015, Answer = a148_2, Score = 1 },
                kT_Key284 = new KT_KeyForCategory { Category = cat016, Answer = a149_1, Score = 2 },
                kT_Key285 = new KT_KeyForCategory { Category = cat016, Answer = a149_2, Score = 1 },
                kT_Key286 = new KT_KeyForCategory { Category = cat016, Answer = a150_2, Score = 1 },
                kT_Key287 = new KT_KeyForCategory { Category = cat016, Answer = a150_3, Score = 2 },
                kT_Key288 = new KT_KeyForCategory { Category = cat001, Answer = a151_2, Score = 1 },
                kT_Key289 = new KT_KeyForCategory { Category = cat001, Answer = a151_3, Score = 2 },
                kT_Key290 = new KT_KeyForCategory { Category = cat002, Answer = a152_1, Score = 1 },
                kT_Key291 = new KT_KeyForCategory { Category = cat002, Answer = a153_3, Score = 1 },
                kT_Key292 = new KT_KeyForCategory { Category = cat003, Answer = a154_2, Score = 1 },
                kT_Key293 = new KT_KeyForCategory { Category = cat003, Answer = a154_3, Score = 2 },
                kT_Key294 = new KT_KeyForCategory { Category = cat004, Answer = a155_1, Score = 2 },
                kT_Key295 = new KT_KeyForCategory { Category = cat004, Answer = a155_2, Score = 1 },
                kT_Key296 = new KT_KeyForCategory { Category = cat004, Answer = a156_1, Score = 2 },
                kT_Key297 = new KT_KeyForCategory { Category = cat004, Answer = a156_2, Score = 1 },
                kT_Key298 = new KT_KeyForCategory { Category = cat005, Answer = a157_2, Score = 1 },
                kT_Key299 = new KT_KeyForCategory { Category = cat005, Answer = a157_3, Score = 2 },
                kT_Key300 = new KT_KeyForCategory { Category = cat005, Answer = a158_2, Score = 1 },
                kT_Key301 = new KT_KeyForCategory { Category = cat005, Answer = a158_3, Score = 2 },
                kT_Key302 = new KT_KeyForCategory { Category = cat006, Answer = a159_2, Score = 1 },
                kT_Key303 = new KT_KeyForCategory { Category = cat006, Answer = a159_3, Score = 2 },
                kT_Key304 = new KT_KeyForCategory { Category = cat006, Answer = a160_1, Score = 2 },
                kT_Key305 = new KT_KeyForCategory { Category = cat006, Answer = a160_2, Score = 1 },
                kT_Key306 = new KT_KeyForCategory { Category = cat007, Answer = a161_2, Score = 1 },
                kT_Key307 = new KT_KeyForCategory { Category = cat007, Answer = a161_3, Score = 2 },
                kT_Key308 = new KT_KeyForCategory { Category = cat008, Answer = a162_2, Score = 1 },
                kT_Key309 = new KT_KeyForCategory { Category = cat008, Answer = a162_3, Score = 2 },
                kT_Key310 = new KT_KeyForCategory { Category = cat008, Answer = a163_1, Score = 2 },
                kT_Key311 = new KT_KeyForCategory { Category = cat008, Answer = a163_2, Score = 1 },
                kT_Key312 = new KT_KeyForCategory { Category = cat009, Answer = a164_1, Score = 2 },
                kT_Key313 = new KT_KeyForCategory { Category = cat009, Answer = a164_2, Score = 1 },
                kT_Key314 = new KT_KeyForCategory { Category = cat010, Answer = a165_2, Score = 1 },
                kT_Key315 = new KT_KeyForCategory { Category = cat010, Answer = a165_3, Score = 2 },
                kT_Key316 = new KT_KeyForCategory { Category = cat010, Answer = a166_2, Score = 1 },
                kT_Key317 = new KT_KeyForCategory { Category = cat010, Answer = a166_3, Score = 2 },
                kT_Key318 = new KT_KeyForCategory { Category = cat011, Answer = a167_1, Score = 2 },
                kT_Key319 = new KT_KeyForCategory { Category = cat011, Answer = a167_2, Score = 1 },
                kT_Key320 = new KT_KeyForCategory { Category = cat012, Answer = a168_2, Score = 1 },
                kT_Key321 = new KT_KeyForCategory { Category = cat012, Answer = a168_3, Score = 2 },
                kT_Key322 = new KT_KeyForCategory { Category = cat013, Answer = a169_1, Score = 2 },
                kT_Key323 = new KT_KeyForCategory { Category = cat013, Answer = a169_2, Score = 1 },
                kT_Key324 = new KT_KeyForCategory { Category = cat013, Answer = a170_2, Score = 1 },
                kT_Key325 = new KT_KeyForCategory { Category = cat013, Answer = a170_3, Score = 2 },
                kT_Key326 = new KT_KeyForCategory { Category = cat014, Answer = a171_1, Score = 2 },
                kT_Key327 = new KT_KeyForCategory { Category = cat014, Answer = a171_2, Score = 1 },
                kT_Key328 = new KT_KeyForCategory { Category = cat015, Answer = a172_2, Score = 1 },
                kT_Key329 = new KT_KeyForCategory { Category = cat015, Answer = a172_3, Score = 2 },
                kT_Key330 = new KT_KeyForCategory { Category = cat015, Answer = a173_1, Score = 2 },
                kT_Key331 = new KT_KeyForCategory { Category = cat015, Answer = a173_2, Score = 1 },
                kT_Key332 = new KT_KeyForCategory { Category = cat016, Answer = a174_1, Score = 2 },
                kT_Key333 = new KT_KeyForCategory { Category = cat016, Answer = a174_2, Score = 1 },
                kT_Key334 = new KT_KeyForCategory { Category = cat016, Answer = a175_2, Score = 1 },
                kT_Key335 = new KT_KeyForCategory { Category = cat016, Answer = a175_3, Score = 2 },
                kT_Key336 = new KT_KeyForCategory { Category = cat001, Answer = a176_1, Score = 2 },
                kT_Key337 = new KT_KeyForCategory { Category = cat001, Answer = a176_2, Score = 1 },
                kT_Key338 = new KT_KeyForCategory { Category = cat002, Answer = a177_1, Score = 1 },
                kT_Key339 = new KT_KeyForCategory { Category = cat002, Answer = a178_1, Score = 1 },
                kT_Key340 = new KT_KeyForCategory { Category = cat003, Answer = a179_1, Score = 2 },
                kT_Key341 = new KT_KeyForCategory { Category = cat003, Answer = a179_2, Score = 1 },
                kT_Key342 = new KT_KeyForCategory { Category = cat004, Answer = a180_1, Score = 2 },
                kT_Key343 = new KT_KeyForCategory { Category = cat004, Answer = a180_2, Score = 1 },
                kT_Key344 = new KT_KeyForCategory { Category = cat004, Answer = a181_1, Score = 2 },
                kT_Key345 = new KT_KeyForCategory { Category = cat004, Answer = a181_2, Score = 1 },
                kT_Key346 = new KT_KeyForCategory { Category = cat005, Answer = a182_1, Score = 2 },
                kT_Key347 = new KT_KeyForCategory { Category = cat005, Answer = a182_2, Score = 1 },
                kT_Key348 = new KT_KeyForCategory { Category = cat005, Answer = a183_1, Score = 2 },
                kT_Key349 = new KT_KeyForCategory { Category = cat005, Answer = a183_2, Score = 1 },
                kT_Key350 = new KT_KeyForCategory { Category = cat006, Answer = a184_1, Score = 2 },
                kT_Key351 = new KT_KeyForCategory { Category = cat006, Answer = a184_2, Score = 1 },
                kT_Key352 = new KT_KeyForCategory { Category = cat006, Answer = a185_1, Score = 2 },
                kT_Key353 = new KT_KeyForCategory { Category = cat006, Answer = a185_2, Score = 1 },
                kT_Key354 = new KT_KeyForCategory { Category = cat007, Answer = a186_1, Score = 2 },
                kT_Key355 = new KT_KeyForCategory { Category = cat007, Answer = a186_2, Score = 1 };

            KT_Mark mark001 = new KT_Mark { Category = cat001, LevelWord = "Н", LevelWordInReport = "G", WordInReport = "F", Mark = 0, ScoreFrom = 0, ScoreTo = 2 },
                mark002 = new KT_Mark { Category = cat001, LevelWord = "Н", LevelWordInReport = "G", WordInReport = "F", Mark = 1, ScoreFrom = 3, ScoreTo = 4 },
                mark003 = new KT_Mark { Category = cat001, LevelWord = "Н", LevelWordInReport = "G", WordInReport = "F", Mark = 2, ScoreFrom = 5, ScoreTo = 6 },
                mark004 = new KT_Mark { Category = cat001, LevelWord = "Н", LevelWordInReport = "G", WordInReport = "F", Mark = 3, ScoreFrom = 7, ScoreTo = 7 },
                mark005 = new KT_Mark { Category = cat001, LevelWord = "С", LevelWordInReport = "G", WordInReport = "F", Mark = 4, ScoreFrom = 8, ScoreTo = 8 },
                mark006 = new KT_Mark { Category = cat001, LevelWord = "С", LevelWordInReport = "G", WordInReport = "F", Mark = 5, ScoreFrom = 9, ScoreTo = 9 },
                mark007 = new KT_Mark { Category = cat001, LevelWord = "С", LevelWordInReport = "G", WordInReport = "F", Mark = 6, ScoreFrom = 10, ScoreTo = 11 },
                mark008 = new KT_Mark { Category = cat001, LevelWord = "С", LevelWordInReport = "G", WordInReport = "F", Mark = 7, ScoreFrom = 12, ScoreTo = 12 },
                mark009 = new KT_Mark { Category = cat001, LevelWord = "В", LevelWordInReport = "G", WordInReport = "F", Mark = 8, ScoreFrom = 13, ScoreTo = 13 },
                mark010 = new KT_Mark { Category = cat001, LevelWord = "В", LevelWordInReport = "G", WordInReport = "F", Mark = 9, ScoreFrom = 14, ScoreTo = 15 },
                mark011 = new KT_Mark { Category = cat001, LevelWord = "В", LevelWordInReport = "G", WordInReport = "F", Mark = 10, ScoreFrom = 16, ScoreTo = 99 },
                mark012 = new KT_Mark { Category = cat002, LevelWord = "Н", LevelWordInReport = "I", WordInReport = "H", Mark = 1, ScoreFrom = 0, ScoreTo = 1 },
                mark013 = new KT_Mark { Category = cat002, LevelWord = "Н", LevelWordInReport = "I", WordInReport = "H", Mark = 2, ScoreFrom = 2, ScoreTo = 3 },
                mark014 = new KT_Mark { Category = cat002, LevelWord = "Н", LevelWordInReport = "I", WordInReport = "H", Mark = 3, ScoreFrom = 4, ScoreTo = 4 },
                mark015 = new KT_Mark { Category = cat002, LevelWord = "С", LevelWordInReport = "I", WordInReport = "H", Mark = 4, ScoreFrom = 5, ScoreTo = 5 },
                mark016 = new KT_Mark { Category = cat002, LevelWord = "С", LevelWordInReport = "I", WordInReport = "H", Mark = 5, ScoreFrom = 6, ScoreTo = 6 },
                mark017 = new KT_Mark { Category = cat002, LevelWord = "С", LevelWordInReport = "I", WordInReport = "H", Mark = 6, ScoreFrom = 7, ScoreTo = 7 },
                mark018 = new KT_Mark { Category = cat002, LevelWord = "С", LevelWordInReport = "I", WordInReport = "H", Mark = 7, ScoreFrom = 8, ScoreTo = 8 },
                mark019 = new KT_Mark { Category = cat002, LevelWord = "В", LevelWordInReport = "I", WordInReport = "H", Mark = 8, ScoreFrom = 9, ScoreTo = 9 },
                mark020 = new KT_Mark { Category = cat002, LevelWord = "В", LevelWordInReport = "I", WordInReport = "H", Mark = 9, ScoreFrom = 10, ScoreTo = 10 },
                mark021 = new KT_Mark { Category = cat002, LevelWord = "В", LevelWordInReport = "I", WordInReport = "H", Mark = 10, ScoreFrom = 11, ScoreTo = 99 },
                mark022 = new KT_Mark { Category = cat003, LevelWord = "Н", LevelWordInReport = "K", WordInReport = "J", Mark = 0, ScoreFrom = 0, ScoreTo = 0 },
                mark023 = new KT_Mark { Category = cat003, LevelWord = "Н", LevelWordInReport = "K", WordInReport = "J", Mark = 1, ScoreFrom = 1, ScoreTo = 6 },
                mark024 = new KT_Mark { Category = cat003, LevelWord = "Н", LevelWordInReport = "K", WordInReport = "J", Mark = 2, ScoreFrom = 7, ScoreTo = 8 },
                mark025 = new KT_Mark { Category = cat003, LevelWord = "Н", LevelWordInReport = "K", WordInReport = "J", Mark = 3, ScoreFrom = 9, ScoreTo = 10 },
                mark026 = new KT_Mark { Category = cat003, LevelWord = "С", LevelWordInReport = "K", WordInReport = "J", Mark = 4, ScoreFrom = 11, ScoreTo = 12 },
                mark027 = new KT_Mark { Category = cat003, LevelWord = "С", LevelWordInReport = "K", WordInReport = "J", Mark = 5, ScoreFrom = 13, ScoreTo = 13 },
                mark028 = new KT_Mark { Category = cat003, LevelWord = "С", LevelWordInReport = "K", WordInReport = "J", Mark = 6, ScoreFrom = 14, ScoreTo = 15 },
                mark029 = new KT_Mark { Category = cat003, LevelWord = "С", LevelWordInReport = "K", WordInReport = "J", Mark = 7, ScoreFrom = 16, ScoreTo = 17 },
                mark030 = new KT_Mark { Category = cat003, LevelWord = "В", LevelWordInReport = "K", WordInReport = "J", Mark = 8, ScoreFrom = 18, ScoreTo = 20 },
                mark031 = new KT_Mark { Category = cat003, LevelWord = "В", LevelWordInReport = "K", WordInReport = "J", Mark = 9, ScoreFrom = 21, ScoreTo = 21 },
                mark032 = new KT_Mark { Category = cat003, LevelWord = "В", LevelWordInReport = "K", WordInReport = "J", Mark = 10, ScoreFrom = 22, ScoreTo = 99 },
                mark033 = new KT_Mark { Category = cat004, LevelWord = "Н", LevelWordInReport = "M", WordInReport = "L", Mark = 0, ScoreFrom = 0, ScoreTo = 1 },
                mark034 = new KT_Mark { Category = cat004, LevelWord = "Н", LevelWordInReport = "M", WordInReport = "L", Mark = 1, ScoreFrom = 2, ScoreTo = 4 },
                mark035 = new KT_Mark { Category = cat004, LevelWord = "Н", LevelWordInReport = "M", WordInReport = "L", Mark = 2, ScoreFrom = 5, ScoreTo = 6 },
                mark036 = new KT_Mark { Category = cat004, LevelWord = "Н", LevelWordInReport = "M", WordInReport = "L", Mark = 3, ScoreFrom = 7, ScoreTo = 7 },
                mark037 = new KT_Mark { Category = cat004, LevelWord = "С", LevelWordInReport = "M", WordInReport = "L", Mark = 4, ScoreFrom = 8, ScoreTo = 9 },
                mark038 = new KT_Mark { Category = cat004, LevelWord = "С", LevelWordInReport = "M", WordInReport = "L", Mark = 5, ScoreFrom = 10, ScoreTo = 10 },
                mark039 = new KT_Mark { Category = cat004, LevelWord = "С", LevelWordInReport = "M", WordInReport = "L", Mark = 6, ScoreFrom = 11, ScoreTo = 12 },
                mark040 = new KT_Mark { Category = cat004, LevelWord = "С", LevelWordInReport = "M", WordInReport = "L", Mark = 7, ScoreFrom = 13, ScoreTo = 13 },
                mark041 = new KT_Mark { Category = cat004, LevelWord = "В", LevelWordInReport = "M", WordInReport = "L", Mark = 8, ScoreFrom = 14, ScoreTo = 15 },
                mark042 = new KT_Mark { Category = cat004, LevelWord = "В", LevelWordInReport = "M", WordInReport = "L", Mark = 9, ScoreFrom = 16, ScoreTo = 17 },
                mark043 = new KT_Mark { Category = cat004, LevelWord = "В", LevelWordInReport = "M", WordInReport = "L", Mark = 10, ScoreFrom = 18, ScoreTo = 99 },
                mark044 = new KT_Mark { Category = cat005, LevelWord = "Н", LevelWordInReport = "O", WordInReport = "N", Mark = 0, ScoreFrom = 0, ScoreTo = 3 },
                mark045 = new KT_Mark { Category = cat005, LevelWord = "Н", LevelWordInReport = "O", WordInReport = "N", Mark = 1, ScoreFrom = 4, ScoreTo = 5 },
                mark046 = new KT_Mark { Category = cat005, LevelWord = "Н", LevelWordInReport = "O", WordInReport = "N", Mark = 2, ScoreFrom = 6, ScoreTo = 7 },
                mark047 = new KT_Mark { Category = cat005, LevelWord = "Н", LevelWordInReport = "O", WordInReport = "N", Mark = 3, ScoreFrom = 8, ScoreTo = 9 },
                mark048 = new KT_Mark { Category = cat005, LevelWord = "С", LevelWordInReport = "O", WordInReport = "N", Mark = 4, ScoreFrom = 10, ScoreTo = 10 },
                mark049 = new KT_Mark { Category = cat005, LevelWord = "С", LevelWordInReport = "O", WordInReport = "N", Mark = 5, ScoreFrom = 11, ScoreTo = 12 },
                mark050 = new KT_Mark { Category = cat005, LevelWord = "С", LevelWordInReport = "O", WordInReport = "N", Mark = 6, ScoreFrom = 13, ScoreTo = 14 },
                mark051 = new KT_Mark { Category = cat005, LevelWord = "С", LevelWordInReport = "O", WordInReport = "N", Mark = 7, ScoreFrom = 15, ScoreTo = 16 },
                mark052 = new KT_Mark { Category = cat005, LevelWord = "В", LevelWordInReport = "O", WordInReport = "N", Mark = 8, ScoreFrom = 17, ScoreTo = 18 },
                mark053 = new KT_Mark { Category = cat005, LevelWord = "В", LevelWordInReport = "O", WordInReport = "N", Mark = 9, ScoreFrom = 19, ScoreTo = 20 },
                mark054 = new KT_Mark { Category = cat005, LevelWord = "В", LevelWordInReport = "O", WordInReport = "N", Mark = 10, ScoreFrom = 21, ScoreTo = 99 },
                mark055 = new KT_Mark { Category = cat006, LevelWord = "Н", LevelWordInReport = "Q", WordInReport = "P", Mark = 0, ScoreFrom = 0, ScoreTo = 1 },
                mark056 = new KT_Mark { Category = cat006, LevelWord = "Н", LevelWordInReport = "Q", WordInReport = "P", Mark = 1, ScoreFrom = 2, ScoreTo = 4 },
                mark057 = new KT_Mark { Category = cat006, LevelWord = "Н", LevelWordInReport = "Q", WordInReport = "P", Mark = 2, ScoreFrom = 5, ScoreTo = 7 },
                mark058 = new KT_Mark { Category = cat006, LevelWord = "Н", LevelWordInReport = "Q", WordInReport = "P", Mark = 3, ScoreFrom = 8, ScoreTo = 9 },
                mark059 = new KT_Mark { Category = cat006, LevelWord = "С", LevelWordInReport = "Q", WordInReport = "P", Mark = 4, ScoreFrom = 10, ScoreTo = 11 },
                mark060 = new KT_Mark { Category = cat006, LevelWord = "С", LevelWordInReport = "Q", WordInReport = "P", Mark = 5, ScoreFrom = 12, ScoreTo = 13 },
                mark061 = new KT_Mark { Category = cat006, LevelWord = "С", LevelWordInReport = "Q", WordInReport = "P", Mark = 6, ScoreFrom = 14, ScoreTo = 14 },
                mark062 = new KT_Mark { Category = cat006, LevelWord = "С", LevelWordInReport = "Q", WordInReport = "P", Mark = 7, ScoreFrom = 15, ScoreTo = 16 },
                mark063 = new KT_Mark { Category = cat006, LevelWord = "В", LevelWordInReport = "Q", WordInReport = "P", Mark = 8, ScoreFrom = 17, ScoreTo = 17 },
                mark064 = new KT_Mark { Category = cat006, LevelWord = "В", LevelWordInReport = "Q", WordInReport = "P", Mark = 9, ScoreFrom = 18, ScoreTo = 18 },
                mark065 = new KT_Mark { Category = cat006, LevelWord = "В", LevelWordInReport = "Q", WordInReport = "P", Mark = 10, ScoreFrom = 19, ScoreTo = 99 },
                mark066 = new KT_Mark { Category = cat007, LevelWord = "Н", LevelWordInReport = "S", WordInReport = "R", Mark = 0, ScoreFrom = 0, ScoreTo = 0 },
                mark067 = new KT_Mark { Category = cat007, LevelWord = "Н", LevelWordInReport = "S", WordInReport = "R", Mark = 1, ScoreFrom = 1, ScoreTo = 3 },
                mark068 = new KT_Mark { Category = cat007, LevelWord = "Н", LevelWordInReport = "S", WordInReport = "R", Mark = 2, ScoreFrom = 4, ScoreTo = 5 },
                mark069 = new KT_Mark { Category = cat007, LevelWord = "Н", LevelWordInReport = "S", WordInReport = "R", Mark = 3, ScoreFrom = 6, ScoreTo = 7 },
                mark070 = new KT_Mark { Category = cat007, LevelWord = "С", LevelWordInReport = "S", WordInReport = "R", Mark = 4, ScoreFrom = 8, ScoreTo = 9 },
                mark071 = new KT_Mark { Category = cat007, LevelWord = "С", LevelWordInReport = "S", WordInReport = "R", Mark = 5, ScoreFrom = 10, ScoreTo = 13 },
                mark072 = new KT_Mark { Category = cat007, LevelWord = "С", LevelWordInReport = "S", WordInReport = "R", Mark = 6, ScoreFrom = 14, ScoreTo = 14 },
                mark073 = new KT_Mark { Category = cat007, LevelWord = "С", LevelWordInReport = "S", WordInReport = "R", Mark = 7, ScoreFrom = 15, ScoreTo = 16 },
                mark074 = new KT_Mark { Category = cat007, LevelWord = "В", LevelWordInReport = "S", WordInReport = "R", Mark = 8, ScoreFrom = 17, ScoreTo = 18 },
                mark075 = new KT_Mark { Category = cat007, LevelWord = "В", LevelWordInReport = "S", WordInReport = "R", Mark = 9, ScoreFrom = 19, ScoreTo = 20 },
                mark076 = new KT_Mark { Category = cat007, LevelWord = "В", LevelWordInReport = "S", WordInReport = "R", Mark = 10, ScoreFrom = 21, ScoreTo = 99 },
                mark077 = new KT_Mark { Category = cat008, LevelWord = "Н", LevelWordInReport = "U", WordInReport = "T", Mark = 1, ScoreFrom = 0, ScoreTo = 3 },
                mark078 = new KT_Mark { Category = cat008, LevelWord = "Н", LevelWordInReport = "U", WordInReport = "T", Mark = 2, ScoreFrom = 4, ScoreTo = 4 },
                mark079 = new KT_Mark { Category = cat008, LevelWord = "Н", LevelWordInReport = "U", WordInReport = "T", Mark = 3, ScoreFrom = 5, ScoreTo = 5 },
                mark080 = new KT_Mark { Category = cat008, LevelWord = "С", LevelWordInReport = "U", WordInReport = "T", Mark = 4, ScoreFrom = 6, ScoreTo = 6 },
                mark081 = new KT_Mark { Category = cat008, LevelWord = "С", LevelWordInReport = "U", WordInReport = "T", Mark = 5, ScoreFrom = 7, ScoreTo = 8 },
                mark082 = new KT_Mark { Category = cat008, LevelWord = "С", LevelWordInReport = "U", WordInReport = "T", Mark = 6, ScoreFrom = 9, ScoreTo = 9 },
                mark083 = new KT_Mark { Category = cat008, LevelWord = "С", LevelWordInReport = "U", WordInReport = "T", Mark = 7, ScoreFrom = 10, ScoreTo = 11 },
                mark084 = new KT_Mark { Category = cat008, LevelWord = "В", LevelWordInReport = "U", WordInReport = "T", Mark = 8, ScoreFrom = 12, ScoreTo = 13 },
                mark085 = new KT_Mark { Category = cat008, LevelWord = "В", LevelWordInReport = "U", WordInReport = "T", Mark = 9, ScoreFrom = 14, ScoreTo = 14 },
                mark086 = new KT_Mark { Category = cat008, LevelWord = "В", LevelWordInReport = "U", WordInReport = "T", Mark = 10, ScoreFrom = 15, ScoreTo = 99 },
                mark087 = new KT_Mark { Category = cat009, LevelWord = "Н", LevelWordInReport = "W", WordInReport = "V", Mark = 0, ScoreFrom = 0, ScoreTo = 1 },
                mark088 = new KT_Mark { Category = cat009, LevelWord = "Н", LevelWordInReport = "W", WordInReport = "V", Mark = 1, ScoreFrom = 2, ScoreTo = 3 },
                mark089 = new KT_Mark { Category = cat009, LevelWord = "Н", LevelWordInReport = "W", WordInReport = "V", Mark = 2, ScoreFrom = 4, ScoreTo = 5 },
                mark090 = new KT_Mark { Category = cat009, LevelWord = "Н", LevelWordInReport = "W", WordInReport = "V", Mark = 3, ScoreFrom = 6, ScoreTo = 6 },
                mark091 = new KT_Mark { Category = cat009, LevelWord = "С", LevelWordInReport = "W", WordInReport = "V", Mark = 4, ScoreFrom = 7, ScoreTo = 7 },
                mark092 = new KT_Mark { Category = cat009, LevelWord = "С", LevelWordInReport = "W", WordInReport = "V", Mark = 5, ScoreFrom = 8, ScoreTo = 9 },
                mark093 = new KT_Mark { Category = cat009, LevelWord = "С", LevelWordInReport = "W", WordInReport = "V", Mark = 6, ScoreFrom = 10, ScoreTo = 10 },
                mark094 = new KT_Mark { Category = cat009, LevelWord = "С", LevelWordInReport = "W", WordInReport = "V", Mark = 7, ScoreFrom = 11, ScoreTo = 11 },
                mark095 = new KT_Mark { Category = cat009, LevelWord = "В", LevelWordInReport = "W", WordInReport = "V", Mark = 8, ScoreFrom = 12, ScoreTo = 13 },
                mark096 = new KT_Mark { Category = cat009, LevelWord = "В", LevelWordInReport = "W", WordInReport = "V", Mark = 9, ScoreFrom = 14, ScoreTo = 15 },
                mark097 = new KT_Mark { Category = cat009, LevelWord = "В", LevelWordInReport = "W", WordInReport = "V", Mark = 10, ScoreFrom = 16, ScoreTo = 99 },
                mark098 = new KT_Mark { Category = cat010, LevelWord = "Н", LevelWordInReport = "Y", WordInReport = "X", Mark = 0, ScoreFrom = 0, ScoreTo = 1 },
                mark099 = new KT_Mark { Category = cat010, LevelWord = "Н", LevelWordInReport = "Y", WordInReport = "X", Mark = 1, ScoreFrom = 2, ScoreTo = 3 },
                mark100 = new KT_Mark { Category = cat010, LevelWord = "Н", LevelWordInReport = "Y", WordInReport = "X", Mark = 2, ScoreFrom = 4, ScoreTo = 5 },
                mark101 = new KT_Mark { Category = cat010, LevelWord = "Н", LevelWordInReport = "Y", WordInReport = "X", Mark = 3, ScoreFrom = 6, ScoreTo = 7 },
                mark102 = new KT_Mark { Category = cat010, LevelWord = "С", LevelWordInReport = "Y", WordInReport = "X", Mark = 4, ScoreFrom = 8, ScoreTo = 8 },
                mark103 = new KT_Mark { Category = cat010, LevelWord = "С", LevelWordInReport = "Y", WordInReport = "X", Mark = 5, ScoreFrom = 9, ScoreTo = 9 },
                mark104 = new KT_Mark { Category = cat010, LevelWord = "С", LevelWordInReport = "Y", WordInReport = "X", Mark = 6, ScoreFrom = 10, ScoreTo = 11 },
                mark105 = new KT_Mark { Category = cat010, LevelWord = "С", LevelWordInReport = "Y", WordInReport = "X", Mark = 7, ScoreFrom = 12, ScoreTo = 12 },
                mark106 = new KT_Mark { Category = cat010, LevelWord = "В", LevelWordInReport = "Y", WordInReport = "X", Mark = 8, ScoreFrom = 13, ScoreTo = 14 },
                mark107 = new KT_Mark { Category = cat010, LevelWord = "В", LevelWordInReport = "Y", WordInReport = "X", Mark = 9, ScoreFrom = 15, ScoreTo = 16 },
                mark108 = new KT_Mark { Category = cat010, LevelWord = "В", LevelWordInReport = "Y", WordInReport = "X", Mark = 10, ScoreFrom = 17, ScoreTo = 99 },
                mark109 = new KT_Mark { Category = cat011, LevelWord = "Н", LevelWordInReport = "AA", WordInReport = "Z", Mark = 0, ScoreFrom = 0, ScoreTo = 1 },
                mark110 = new KT_Mark { Category = cat011, LevelWord = "Н", LevelWordInReport = "AA", WordInReport = "Z", Mark = 1, ScoreFrom = 2, ScoreTo = 4 },
                mark111 = new KT_Mark { Category = cat011, LevelWord = "Н", LevelWordInReport = "AA", WordInReport = "Z", Mark = 2, ScoreFrom = 5, ScoreTo = 5 },
                mark112 = new KT_Mark { Category = cat011, LevelWord = "Н", LevelWordInReport = "AA", WordInReport = "Z", Mark = 3, ScoreFrom = 6, ScoreTo = 7 },
                mark113 = new KT_Mark { Category = cat011, LevelWord = "С", LevelWordInReport = "AA", WordInReport = "Z", Mark = 4, ScoreFrom = 8, ScoreTo = 8 },
                mark114 = new KT_Mark { Category = cat011, LevelWord = "С", LevelWordInReport = "AA", WordInReport = "Z", Mark = 5, ScoreFrom = 9, ScoreTo = 10 },
                mark115 = new KT_Mark { Category = cat011, LevelWord = "С", LevelWordInReport = "AA", WordInReport = "Z", Mark = 6, ScoreFrom = 11, ScoreTo = 11 },
                mark116 = new KT_Mark { Category = cat011, LevelWord = "С", LevelWordInReport = "AA", WordInReport = "Z", Mark = 7, ScoreFrom = 12, ScoreTo = 13 },
                mark117 = new KT_Mark { Category = cat011, LevelWord = "В", LevelWordInReport = "AA", WordInReport = "Z", Mark = 8, ScoreFrom = 14, ScoreTo = 15 },
                mark118 = new KT_Mark { Category = cat011, LevelWord = "В", LevelWordInReport = "AA", WordInReport = "Z", Mark = 9, ScoreFrom = 16, ScoreTo = 16 },
                mark119 = new KT_Mark { Category = cat011, LevelWord = "В", LevelWordInReport = "AA", WordInReport = "Z", Mark = 10, ScoreFrom = 17, ScoreTo = 99 },
                mark120 = new KT_Mark { Category = cat012, LevelWord = "Н", LevelWordInReport = "AC", WordInReport = "AB", Mark = 0, ScoreFrom = 0, ScoreTo = 1 },
                mark121 = new KT_Mark { Category = cat012, LevelWord = "Н", LevelWordInReport = "AC", WordInReport = "AB", Mark = 1, ScoreFrom = 2, ScoreTo = 4 },
                mark122 = new KT_Mark { Category = cat012, LevelWord = "Н", LevelWordInReport = "AC", WordInReport = "AB", Mark = 2, ScoreFrom = 5, ScoreTo = 6 },
                mark123 = new KT_Mark { Category = cat012, LevelWord = "Н", LevelWordInReport = "AC", WordInReport = "AB", Mark = 3, ScoreFrom = 7, ScoreTo = 7 },
                mark124 = new KT_Mark { Category = cat012, LevelWord = "С", LevelWordInReport = "AC", WordInReport = "AB", Mark = 4, ScoreFrom = 8, ScoreTo = 9 },
                mark125 = new KT_Mark { Category = cat012, LevelWord = "С", LevelWordInReport = "AC", WordInReport = "AB", Mark = 5, ScoreFrom = 10, ScoreTo = 11 },
                mark126 = new KT_Mark { Category = cat012, LevelWord = "С", LevelWordInReport = "AC", WordInReport = "AB", Mark = 6, ScoreFrom = 12, ScoreTo = 13 },
                mark127 = new KT_Mark { Category = cat012, LevelWord = "С", LevelWordInReport = "AC", WordInReport = "AB", Mark = 7, ScoreFrom = 14, ScoreTo = 15 },
                mark128 = new KT_Mark { Category = cat012, LevelWord = "В", LevelWordInReport = "AC", WordInReport = "AB", Mark = 8, ScoreFrom = 16, ScoreTo = 16 },
                mark129 = new KT_Mark { Category = cat012, LevelWord = "В", LevelWordInReport = "AC", WordInReport = "AB", Mark = 9, ScoreFrom = 17, ScoreTo = 18 },
                mark130 = new KT_Mark { Category = cat012, LevelWord = "В", LevelWordInReport = "AC", WordInReport = "AB", Mark = 10, ScoreFrom = 19, ScoreTo = 99 },
                mark131 = new KT_Mark { Category = cat013, LevelWord = "Н", LevelWordInReport = "AE", WordInReport = "AD", Mark = 0, ScoreFrom = 0, ScoreTo = 1 },
                mark132 = new KT_Mark { Category = cat013, LevelWord = "Н", LevelWordInReport = "AE", WordInReport = "AD", Mark = 1, ScoreFrom = 2, ScoreTo = 3 },
                mark133 = new KT_Mark { Category = cat013, LevelWord = "Н", LevelWordInReport = "AE", WordInReport = "AD", Mark = 2, ScoreFrom = 4, ScoreTo = 4 },
                mark134 = new KT_Mark { Category = cat013, LevelWord = "Н", LevelWordInReport = "AE", WordInReport = "AD", Mark = 3, ScoreFrom = 5, ScoreTo = 6 },
                mark135 = new KT_Mark { Category = cat013, LevelWord = "С", LevelWordInReport = "AE", WordInReport = "AD", Mark = 4, ScoreFrom = 7, ScoreTo = 7 },
                mark136 = new KT_Mark { Category = cat013, LevelWord = "С", LevelWordInReport = "AE", WordInReport = "AD", Mark = 5, ScoreFrom = 8, ScoreTo = 8 },
                mark137 = new KT_Mark { Category = cat013, LevelWord = "С", LevelWordInReport = "AE", WordInReport = "AD", Mark = 6, ScoreFrom = 9, ScoreTo = 10 },
                mark138 = new KT_Mark { Category = cat013, LevelWord = "С", LevelWordInReport = "AE", WordInReport = "AD", Mark = 7, ScoreFrom = 11, ScoreTo = 11 },
                mark139 = new KT_Mark { Category = cat013, LevelWord = "В", LevelWordInReport = "AE", WordInReport = "AD", Mark = 8, ScoreFrom = 12, ScoreTo = 13 },
                mark140 = new KT_Mark { Category = cat013, LevelWord = "В", LevelWordInReport = "AE", WordInReport = "AD", Mark = 9, ScoreFrom = 14, ScoreTo = 14 },
                mark141 = new KT_Mark { Category = cat013, LevelWord = "В", LevelWordInReport = "AE", WordInReport = "AD", Mark = 10, ScoreFrom = 15, ScoreTo = 99 },
                mark142 = new KT_Mark { Category = cat014, LevelWord = "Н", LevelWordInReport = "AG", WordInReport = "AF", Mark = 1, ScoreFrom = 0, ScoreTo = 2 },
                mark143 = new KT_Mark { Category = cat014, LevelWord = "Н", LevelWordInReport = "AG", WordInReport = "AF", Mark = 2, ScoreFrom = 3, ScoreTo = 3 },
                mark144 = new KT_Mark { Category = cat014, LevelWord = "Н", LevelWordInReport = "AG", WordInReport = "AF", Mark = 3, ScoreFrom = 4, ScoreTo = 5 },
                mark145 = new KT_Mark { Category = cat014, LevelWord = "С", LevelWordInReport = "AG", WordInReport = "AF", Mark = 4, ScoreFrom = 6, ScoreTo = 6 },
                mark146 = new KT_Mark { Category = cat014, LevelWord = "С", LevelWordInReport = "AG", WordInReport = "AF", Mark = 5, ScoreFrom = 7, ScoreTo = 7 },
                mark147 = new KT_Mark { Category = cat014, LevelWord = "С", LevelWordInReport = "AG", WordInReport = "AF", Mark = 6, ScoreFrom = 8, ScoreTo = 9 },
                mark148 = new KT_Mark { Category = cat014, LevelWord = "С", LevelWordInReport = "AG", WordInReport = "AF", Mark = 7, ScoreFrom = 10, ScoreTo = 11 },
                mark149 = new KT_Mark { Category = cat014, LevelWord = "В", LevelWordInReport = "AG", WordInReport = "AF", Mark = 8, ScoreFrom = 12, ScoreTo = 12 },
                mark150 = new KT_Mark { Category = cat014, LevelWord = "В", LevelWordInReport = "AG", WordInReport = "AF", Mark = 9, ScoreFrom = 13, ScoreTo = 14 },
                mark151 = new KT_Mark { Category = cat014, LevelWord = "В", LevelWordInReport = "AG", WordInReport = "AF", Mark = 10, ScoreFrom = 15, ScoreTo = 99 },
                mark152 = new KT_Mark { Category = cat015, LevelWord = "Н", LevelWordInReport = "AI", WordInReport = "AH", Mark = 0, ScoreFrom = 0, ScoreTo = 2 },
                mark153 = new KT_Mark { Category = cat015, LevelWord = "Н", LevelWordInReport = "AI", WordInReport = "AH", Mark = 1, ScoreFrom = 3, ScoreTo = 5 },
                mark154 = new KT_Mark { Category = cat015, LevelWord = "Н", LevelWordInReport = "AI", WordInReport = "AH", Mark = 2, ScoreFrom = 6, ScoreTo = 7 },
                mark155 = new KT_Mark { Category = cat015, LevelWord = "Н", LevelWordInReport = "AI", WordInReport = "AH", Mark = 3, ScoreFrom = 8, ScoreTo = 9 },
                mark156 = new KT_Mark { Category = cat015, LevelWord = "С", LevelWordInReport = "AI", WordInReport = "AH", Mark = 4, ScoreFrom = 10, ScoreTo = 10 },
                mark157 = new KT_Mark { Category = cat015, LevelWord = "С", LevelWordInReport = "AI", WordInReport = "AH", Mark = 5, ScoreFrom = 11, ScoreTo = 12 },
                mark158 = new KT_Mark { Category = cat015, LevelWord = "С", LevelWordInReport = "AI", WordInReport = "AH", Mark = 6, ScoreFrom = 13, ScoreTo = 13 },
                mark159 = new KT_Mark { Category = cat015, LevelWord = "С", LevelWordInReport = "AI", WordInReport = "AH", Mark = 7, ScoreFrom = 14, ScoreTo = 15 },
                mark160 = new KT_Mark { Category = cat015, LevelWord = "В", LevelWordInReport = "AI", WordInReport = "AH", Mark = 8, ScoreFrom = 16, ScoreTo = 16 },
                mark161 = new KT_Mark { Category = cat015, LevelWord = "В", LevelWordInReport = "AI", WordInReport = "AH", Mark = 9, ScoreFrom = 17, ScoreTo = 17 },
                mark162 = new KT_Mark { Category = cat015, LevelWord = "В", LevelWordInReport = "AI", WordInReport = "AH", Mark = 10, ScoreFrom = 18, ScoreTo = 99 },
                mark163 = new KT_Mark { Category = cat016, LevelWord = "Н", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 1, ScoreFrom = 0, ScoreTo = 1 },
                mark164 = new KT_Mark { Category = cat016, LevelWord = "Н", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 2, ScoreFrom = 2, ScoreTo = 3 },
                mark165 = new KT_Mark { Category = cat016, LevelWord = "Н", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 3, ScoreFrom = 4, ScoreTo = 5 },
                mark166 = new KT_Mark { Category = cat016, LevelWord = "С", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 4, ScoreFrom = 6, ScoreTo = 8 },
                mark167 = new KT_Mark { Category = cat016, LevelWord = "С", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 5, ScoreFrom = 9, ScoreTo = 10 },
                mark168 = new KT_Mark { Category = cat016, LevelWord = "С", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 6, ScoreFrom = 11, ScoreTo = 12 },
                mark169 = new KT_Mark { Category = cat016, LevelWord = "С", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 7, ScoreFrom = 13, ScoreTo = 15 },
                mark170 = new KT_Mark { Category = cat016, LevelWord = "В", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 8, ScoreFrom = 16, ScoreTo = 17 },
                mark171 = new KT_Mark { Category = cat016, LevelWord = "В", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 9, ScoreFrom = 18, ScoreTo = 19 },
                mark172 = new KT_Mark { Category = cat016, LevelWord = "В", LevelWordInReport = "AK", WordInReport = "AJ", Mark = 10, ScoreFrom = 20, ScoreTo = 99 };


            context.KT_Questions.AddRange(new List<KT_Question> { q001, q002, q003, q004, q005, q006, q007, q008, q009, q010, q011, q012, q013, q014, q015, q016, q017, q018, q019, q020, q021, q022, q023, q024, q025, q026, q027, q028, q029, q030, q031, q032, q033, q034, q035, q036, q037, q038, q039, q040, q041, q042, q043, q044, q045, q046, q047, q048, q049, q050, q051, q052, q053, q054, q055, q056, q057, q058, q059, q060, q061, q062, q063, q064, q065, q066, q067, q068, q069, q070, q071, q072, q073, q074, q075, q076, q077, q078, q079, q080, q081, q082, q083, q084, q085, q086, q087, q088, q089, q090, q091, q092, q093, q094, q095, q096, q097, q098, q099, q100, q101, q102, q103, q104, q105, q106, q107, q108, q109, q110, q111, q112, q113, q114, q115, q116, q117, q118, q119, q120, q121, q122, q123, q124, q125, q126, q127, q128, q129, q130, q131, q132, q133, q134, q135, q136, q137, q138, q139, q140, q141, q142, q143, q144, q145, q146, q147, q148, q149, q150, q151, q152, q153, q154, q155, q156, q157, q158, q159, q160, q161, q162, q163, q164, q165, q166, q167, q168, q169, q170, q171, q172, q173, q174, q175, q176, q177, q178, q179, q180, q181, q182, q183, q184, q185, q186, q187 });

            context.KT_Answers.AddRange(new List<KT_Answer> {a001_1, a002_1, a003_1, a004_1, a005_1, a006_1, a007_1, a008_1, a009_1, a010_1, a011_1, a012_1, a013_1, a014_1, a015_1, a016_1, a017_1, a018_1, a019_1, a020_1, a021_1, a022_1, a023_1, a024_1, a025_1, a026_1, a027_1, a028_1, a029_1, a030_1, a031_1, a032_1, a033_1, a034_1, a035_1, a036_1, a037_1, a038_1, a039_1, a040_1, a041_1, a042_1, a043_1, a044_1, a045_1, a046_1, a047_1, a048_1, a049_1, a050_1, a051_1, a052_1, a053_1, a054_1, a055_1, a056_1, a057_1, a058_1, a059_1, a060_1, a061_1, a062_1, a063_1, a064_1, a065_1, a066_1, a067_1, a068_1, a069_1, a070_1, a071_1, a072_1, a073_1, a074_1, a075_1, a076_1, a077_1, a078_1, a079_1, a080_1, a081_1, a082_1, a083_1, a084_1, a085_1, a086_1, a087_1, a088_1, a089_1, a090_1, a091_1, a092_1, a093_1, a094_1, a095_1, a096_1, a097_1, a098_1, a099_1, a100_1, a101_1, a102_1, a103_1, a104_1, a105_1, a106_1, a107_1, a108_1, a109_1, a110_1, a111_1, a112_1, a113_1, a114_1, a115_1, a116_1, a117_1, a118_1, a119_1, a120_1, a121_1, a122_1, a123_1, a124_1, a125_1, a126_1, a127_1, a128_1, a129_1, a130_1, a131_1, a132_1, a133_1, a134_1, a135_1, a136_1, a137_1, a138_1, a139_1, a140_1, a141_1, a142_1, a143_1, a144_1, a145_1, a146_1, a147_1, a148_1, a149_1, a150_1, a151_1, a152_1, a153_1, a154_1, a155_1, a156_1, a157_1, a158_1, a159_1, a160_1, a161_1, a162_1, a163_1, a164_1, a165_1, a166_1, a167_1, a168_1, a169_1, a170_1, a171_1, a172_1, a173_1, a174_1, a175_1, a176_1, a177_1, a178_1, a179_1, a180_1, a181_1, a182_1, a183_1, a184_1, a185_1, a186_1, a187_1,
                                                             a001_2, a002_2, a003_2, a004_2, a005_2, a006_2, a007_2, a008_2, a009_2, a010_2, a011_2, a012_2, a013_2, a014_2, a015_2, a016_2, a017_2, a018_2, a019_2, a020_2, a021_2, a022_2, a023_2, a024_2, a025_2, a026_2, a027_2, a028_2, a029_2, a030_2, a031_2, a032_2, a033_2, a034_2, a035_2, a036_2, a037_2, a038_2, a039_2, a040_2, a041_2, a042_2, a043_2, a044_2, a045_2, a046_2, a047_2, a048_2, a049_2, a050_2, a051_2, a052_2, a053_2, a054_2, a055_2, a056_2, a057_2, a058_2, a059_2, a060_2, a061_2, a062_2, a063_2, a064_2, a065_2, a066_2, a067_2, a068_2, a069_2, a070_2, a071_2, a072_2, a073_2, a074_2, a075_2, a076_2, a077_2, a078_2, a079_2, a080_2, a081_2, a082_2, a083_2, a084_2, a085_2, a086_2, a087_2, a088_2, a089_2, a090_2, a091_2, a092_2, a093_2, a094_2, a095_2, a096_2, a097_2, a098_2, a099_2, a100_2, a101_2, a102_2, a103_2, a104_2, a105_2, a106_2, a107_2, a108_2, a109_2, a110_2, a111_2, a112_2, a113_2, a114_2, a115_2, a116_2, a117_2, a118_2, a119_2, a120_2, a121_2, a122_2, a123_2, a124_2, a125_2, a126_2, a127_2, a128_2, a129_2, a130_2, a131_2, a132_2, a133_2, a134_2, a135_2, a136_2, a137_2, a138_2, a139_2, a140_2, a141_2, a142_2, a143_2, a144_2, a145_2, a146_2, a147_2, a148_2, a149_2, a150_2, a151_2, a152_2, a153_2, a154_2, a155_2, a156_2, a157_2, a158_2, a159_2, a160_2, a161_2, a162_2, a163_2, a164_2, a165_2, a166_2, a167_2, a168_2, a169_2, a170_2, a171_2, a172_2, a173_2, a174_2, a175_2, a176_2, a177_2, a178_2, a179_2, a180_2, a181_2, a182_2, a183_2, a184_2, a185_2, a186_2, a187_2,
                                                             a001_3, a002_3, a003_3, a004_3, a005_3, a006_3, a007_3, a008_3, a009_3, a010_3, a011_3, a012_3, a013_3, a014_3, a015_3, a016_3, a017_3, a018_3, a019_3, a020_3, a021_3, a022_3, a023_3, a024_3, a025_3, a026_3, a027_3, a028_3, a029_3, a030_3, a031_3, a032_3, a033_3, a034_3, a035_3, a036_3, a037_3, a038_3, a039_3, a040_3, a041_3, a042_3, a043_3, a044_3, a045_3, a046_3, a047_3, a048_3, a049_3, a050_3, a051_3, a052_3, a053_3, a054_3, a055_3, a056_3, a057_3, a058_3, a059_3, a060_3, a061_3, a062_3, a063_3, a064_3, a065_3, a066_3, a067_3, a068_3, a069_3, a070_3, a071_3, a072_3, a073_3, a074_3, a075_3, a076_3, a077_3, a078_3, a079_3, a080_3, a081_3, a082_3, a083_3, a084_3, a085_3, a086_3, a087_3, a088_3, a089_3, a090_3, a091_3, a092_3, a093_3, a094_3, a095_3, a096_3, a097_3, a098_3, a099_3, a100_3, a101_3, a102_3, a103_3, a104_3, a105_3, a106_3, a107_3, a108_3, a109_3, a110_3, a111_3, a112_3, a113_3, a114_3, a115_3, a116_3, a117_3, a118_3, a119_3, a120_3, a121_3, a122_3, a123_3, a124_3, a125_3, a126_3, a127_3, a128_3, a129_3, a130_3, a131_3, a132_3, a133_3, a134_3, a135_3, a136_3, a137_3, a138_3, a139_3, a140_3, a141_3, a142_3, a143_3, a144_3, a145_3, a146_3, a147_3, a148_3, a149_3, a150_3, a151_3, a152_3, a153_3, a154_3, a155_3, a156_3, a157_3, a158_3, a159_3, a160_3, a161_3, a162_3, a163_3, a164_3, a165_3, a166_3, a167_3, a168_3, a169_3, a170_3, a171_3, a172_3, a173_3, a174_3, a175_3, a176_3, a177_3, a178_3, a179_3, a180_3, a181_3, a182_3, a183_3, a184_3, a185_3, a186_3, a187_3 });

            context.KT_Categories.AddRange(new List<KT_Category> { cat001, cat002, cat003, cat004, cat005, cat006, cat007, cat008, cat009, cat010, cat011, cat012, cat013, cat014, cat015, cat016 });

            context.KT_Keys.AddRange(new List<KT_KeyForCategory> { kT_Key001, kT_Key002, kT_Key003, kT_Key004, kT_Key005, kT_Key006, kT_Key007, kT_Key008, kT_Key009, kT_Key010, kT_Key011, kT_Key012, kT_Key013, kT_Key014, kT_Key015, kT_Key016, kT_Key017, kT_Key018, kT_Key019, kT_Key020, kT_Key021, kT_Key022, kT_Key023, kT_Key024, kT_Key025, kT_Key026, kT_Key027, kT_Key028, kT_Key029, kT_Key030, kT_Key031, kT_Key032, kT_Key033, kT_Key034, kT_Key035, kT_Key036, kT_Key037, kT_Key038, kT_Key039, kT_Key040, kT_Key041, kT_Key042, kT_Key043, kT_Key044, kT_Key045, kT_Key046, kT_Key047, kT_Key048, kT_Key049, kT_Key050, kT_Key051, kT_Key052, kT_Key053, kT_Key054, kT_Key055, kT_Key056, kT_Key057, kT_Key058, kT_Key059, kT_Key060, kT_Key061, kT_Key062, kT_Key063, kT_Key064, kT_Key065, kT_Key066, kT_Key067, kT_Key068, kT_Key069, kT_Key070, kT_Key071, kT_Key072, kT_Key073, kT_Key074, kT_Key075, kT_Key076, kT_Key077, kT_Key078, kT_Key079, kT_Key080, kT_Key081, kT_Key082, kT_Key083, kT_Key084, kT_Key085, kT_Key086, kT_Key087, kT_Key088, kT_Key089, kT_Key090, kT_Key091, kT_Key092, kT_Key093, kT_Key094, kT_Key095, kT_Key096, kT_Key097, kT_Key098, kT_Key099, kT_Key100, kT_Key101, kT_Key102, kT_Key103, kT_Key104, kT_Key105, kT_Key106, kT_Key107, kT_Key108, kT_Key109, kT_Key110, kT_Key111, kT_Key112, kT_Key113, kT_Key114, kT_Key115, kT_Key116, kT_Key117, kT_Key118, kT_Key119, kT_Key120, kT_Key121, kT_Key122, kT_Key123, kT_Key124, kT_Key125, kT_Key126, kT_Key127, kT_Key128, kT_Key129, kT_Key130, kT_Key131, kT_Key132, kT_Key133, kT_Key134, kT_Key135, kT_Key136, kT_Key137, kT_Key138, kT_Key139, kT_Key140, kT_Key141, kT_Key142, kT_Key143, kT_Key144, kT_Key145, kT_Key146, kT_Key147, kT_Key148, kT_Key149, kT_Key150, kT_Key151, kT_Key152, kT_Key153, kT_Key154, kT_Key155, kT_Key156, kT_Key157, kT_Key158, kT_Key159, kT_Key160, kT_Key161, kT_Key162, kT_Key163, kT_Key164, kT_Key165, kT_Key166, kT_Key167, kT_Key168, kT_Key169, kT_Key170, kT_Key171, kT_Key172, kT_Key173, kT_Key174, kT_Key175, kT_Key176, kT_Key177, kT_Key178, kT_Key179, kT_Key180, kT_Key181, kT_Key182, kT_Key183, kT_Key184, kT_Key185, kT_Key186, kT_Key187, kT_Key188, kT_Key189, kT_Key190, kT_Key191, kT_Key192, kT_Key193, kT_Key194, kT_Key195, kT_Key196, kT_Key197, kT_Key198, kT_Key199, kT_Key200, kT_Key201, kT_Key202, kT_Key203, kT_Key204, kT_Key205, kT_Key206, kT_Key207, kT_Key208, kT_Key209, kT_Key210, kT_Key211, kT_Key212, kT_Key213, kT_Key214, kT_Key215, kT_Key216, kT_Key217, kT_Key218, kT_Key219, kT_Key220, kT_Key221, kT_Key222, kT_Key223, kT_Key224, kT_Key225, kT_Key226, kT_Key227, kT_Key228, kT_Key229, kT_Key230, kT_Key231, kT_Key232, kT_Key233, kT_Key234, kT_Key235, kT_Key236, kT_Key237, kT_Key238, kT_Key239, kT_Key240, kT_Key241, kT_Key242, kT_Key243, kT_Key244, kT_Key245, kT_Key246, kT_Key247, kT_Key248, kT_Key249, kT_Key250, kT_Key251, kT_Key252, kT_Key253, kT_Key254, kT_Key255, kT_Key256, kT_Key257, kT_Key258, kT_Key259, kT_Key260, kT_Key261, kT_Key262, kT_Key263, kT_Key264, kT_Key265, kT_Key266, kT_Key267, kT_Key268, kT_Key269, kT_Key270, kT_Key271, kT_Key272, kT_Key273, kT_Key274, kT_Key275, kT_Key276, kT_Key277, kT_Key278, kT_Key279, kT_Key280, kT_Key281, kT_Key282, kT_Key283, kT_Key284, kT_Key285, kT_Key286, kT_Key287, kT_Key288, kT_Key289, kT_Key290, kT_Key291, kT_Key292, kT_Key293, kT_Key294, kT_Key295, kT_Key296, kT_Key297, kT_Key298, kT_Key299, kT_Key300, kT_Key301, kT_Key302, kT_Key303, kT_Key304, kT_Key305, kT_Key306, kT_Key307, kT_Key308, kT_Key309, kT_Key310, kT_Key311, kT_Key312, kT_Key313, kT_Key314, kT_Key315, kT_Key316, kT_Key317, kT_Key318, kT_Key319, kT_Key320, kT_Key321, kT_Key322, kT_Key323, kT_Key324, kT_Key325, kT_Key326, kT_Key327, kT_Key328, kT_Key329, kT_Key330, kT_Key331, kT_Key332, kT_Key333, kT_Key334, kT_Key335, kT_Key336, kT_Key337, kT_Key338, kT_Key339, kT_Key340, kT_Key341, kT_Key342, kT_Key343, kT_Key344, kT_Key345, kT_Key346, kT_Key347, kT_Key348, kT_Key349, kT_Key350, kT_Key351, kT_Key352, kT_Key353, kT_Key354, kT_Key355 });

            context.KT_Marks.AddRange(new List<KT_Mark> { mark001, mark002, mark003, mark004, mark005, mark006, mark007, mark008, mark009, mark010, mark011, mark012, mark013, mark014, mark015, mark016, mark017, mark018, mark019, mark020, mark021, mark022, mark023, mark024, mark025, mark026, mark027, mark028, mark029, mark030, mark031, mark032, mark033, mark034, mark035, mark036, mark037, mark038, mark039, mark040, mark041, mark042, mark043, mark044, mark045, mark046, mark047, mark048, mark049, mark050, mark051, mark052, mark053, mark054, mark055, mark056, mark057, mark058, mark059, mark060, mark061, mark062, mark063, mark064, mark065, mark066, mark067, mark068, mark069, mark070, mark071, mark072, mark073, mark074, mark075, mark076, mark077, mark078, mark079, mark080, mark081, mark082, mark083, mark084, mark085, mark086, mark087, mark088, mark089, mark090, mark091, mark092, mark093, mark094, mark095, mark096, mark097, mark098, mark099, mark100, mark101, mark102, mark103, mark104, mark105, mark106, mark107, mark108, mark109, mark110, mark111, mark112, mark113, mark114, mark115, mark116, mark117, mark118, mark119, mark120, mark121, mark122, mark123, mark124, mark125, mark126, mark127, mark128, mark129, mark130, mark131, mark132, mark133, mark134, mark135, mark136, mark137, mark138, mark139, mark140, mark141, mark142, mark143, mark144, mark145, mark146, mark147, mark148, mark149, mark150, mark151, mark152, mark153, mark154, mark155, mark156, mark157, mark158, mark159, mark160, mark161, mark162, mark163, mark164, mark165, mark166, mark167, mark168, mark169, mark170, mark171, mark172 });
        
            context.SaveChanges();

        }


        public void AddCompetention_1year()
        {
            var blok_1 = new Comp_Block { Year = 1, WordInReport = "J", BlockName = "Компетенция «Готовность к изменениям»" };
            var blok_2 = new Comp_Block { Year = 1, WordInReport = "P", BlockName = "Компетенция «Мотивация и развитие подчиненных»" };
            var blok_3 = new Comp_Block { Year = 1, WordInReport = "H", BlockName = "Компетенция «Ориентация на результат»" };
            var blok_4 = new Comp_Block { Year = 1, WordInReport = "O", BlockName = "Компетенция «Планирование деятельности подразделения»" };
            var blok_5 = new Comp_Block { Year = 1, WordInReport = "I", BlockName = "Компетенция «Профессиональное развитие»" };
            var blok_6 = new Comp_Block { Year = 1, WordInReport = "G", BlockName = "Компетенция «Стрессоустойчивость и решение проблем»" };
            var blok_7 = new Comp_Block { Year = 1, WordInReport = "F", BlockName = "Компетенция «Системное мышление»" };
            var blok_8 = new Comp_Block { Year = 1, WordInReport = "N", BlockName = "Компетенция «Умение обеспечить результат»" };
            var blok_9 = new Comp_Block { Year = 1, WordInReport = "K", BlockName = "Компетенция «Умение отстаивать свою позицию»" };

            context.Comp_Blocks.Add(blok_1);
            context.Comp_Blocks.Add(blok_2);
            context.Comp_Blocks.Add(blok_3);
            context.Comp_Blocks.Add(blok_4);
            context.Comp_Blocks.Add(blok_5);
            context.Comp_Blocks.Add(blok_6);
            context.Comp_Blocks.Add(blok_7);
            context.Comp_Blocks.Add(blok_8);
            context.Comp_Blocks.Add(blok_9);

            context.SaveChanges();

            var question_1 = new Comp_Question { Block = blok_1, QuestionNumber = 1, QuestionText = "Вы - начальник службы. В службе четко распределены обязанности между всеми сотрудниками, в том числе в части формирования отчетности: сбор информации, заполнение форм и т.д. Сегодня на планерке у начальника филиала Вы узнали, что двух сотрудников вашей службы переводят на неопределенный срок в другое структурное подразделение с изменением трудовых функций." };
            var question_2 = new Comp_Question { Block = blok_1, QuestionNumber = 2, QuestionText = "Вы – начальник службы. В вашей службе и еще нескольких структурных подразделениях начинается экспериментальное внедрение нового программного обеспечения для ежедневного использования. Руководители структурных подразделений не понимают необходимости замены существующего программного обеспечения, поскольку на текущий момент старое ПО работает удовлетворительно." };
            var question_3 = new Comp_Question { Block = blok_1, QuestionNumber = 3, QuestionText = "В филиале две службы с разной численностью объединяют. Вас назначили руководителем объединённой службы, с подчиненными Вы не знакомы. Некоторые подчиненные весьма обеспокоены, когда речь идет о распределении важных задач, графике отпусков." };
            var question_4 = new Comp_Question { Block = blok_2, QuestionNumber = 4, QuestionText = "В должностные обязанности подчиненных Вашей службы входит еженедельное составление отчетов о проделанной работе. Сотрудники считают эту задачу нудной и рутинной, но относятся к ее выполнению серьезно и каждую пятницу составляют соответствующий отчет. Работник, который недавно работает в службе, жалуется в процессе работы над отчетом вслух, другие сотрудники также начали проявлять недовольство." };
            var question_5 = new Comp_Question { Block = blok_2, QuestionNumber = 5, QuestionText = "Вы руководитель службы филиала. В вашем подчинении работает специалист М. в настоящее время он является одним из наиболее авторитетных и инициативных специалистов, занимается решением производственных проблем, имеет опыт управления. \nВ начале текущего года компания начала переход на новое оборудование, после чего М. стал испытывать сложности в новой для себя области, о чем Вам несколько раз говорил." };
            var question_6 = new Comp_Question { Block = blok_2, QuestionNumber = 6, QuestionText = "Вы руководитель службы филиала. В Вашем коллективе есть сотрудник, который скорее числится, чем работает. Его это положение устраивает, а Вас нет." };
            var question_7 = new Comp_Question { Block = blok_3, QuestionNumber = 7, QuestionText = "Руководитель филиала поручил Вам написать отчет по итогам внедрения системы учета расхода топлива за прошлый год. Эта задача является новой для Вас. Ваш руководитель подробно объяснил, что нужно делать. Вы начали работать над отчетом и понимаете, что возникли вопросы. Вы не уверены, как действовать дальше. Ваш руководитель сегодня занят." };
            var question_8 = new Comp_Question { Block = blok_3, QuestionNumber = 8, QuestionText = "Вы ответственный за работу по проекту и пытаетесь завершить его, но не успеваете к назначенному сроку. Проекту систематически дается самый низкий приоритет в Вашей службе, и поэтому не хватает ресурсов. Тем не менее, ваш непосредственный руководитель считает, что проект перспективный, и именно поэтому он поручил его Вам." };
            var question_9 = new Comp_Question { Block = blok_3, QuestionNumber = 9, QuestionText = "Вы — начальник цеха. После реорганизации необходимо срочно перекомплектовать несколько бригад согласно новому штатному расписанию." };
            var question_10 = new Comp_Question { Block = blok_4, QuestionNumber = 10, QuestionText = "Вы начальник службы филиала. Сотрудники службы занимаются консолидацией большого количества информации, которая поступает преимущественно на бумажных носителях. Это значительно усложняет последующую работу с данными, Вы понимаете, что этот процесс необходимо каким-то образом оптимизировать, и эта работа перерастет в целый проект, который потребует привлечения специалистов из других структурных подразделений.  Руководство филиала поддержало идею и ждет от Вас плана деятельности Вашего подразделения по реализации проекта в течение 2 и 3 квартала текущего года." };
            var question_11 = new Comp_Question { Block = blok_4, QuestionNumber = 11, QuestionText = "Вы начальник цеха. В одном из двух действующих цехов замена оборудования N включена в план ремонта на третий квартал текущего года. Но в начале первого квартала была проведена внутренняя проверка, показавшая что есть риск аварийного выхода оборудования из строя уже во втором квартале, а второй квартал – это время повышенного спроса на продукцию компании. Замена оборудования ранее запланированного срока невозможна." };
            var question_12 = new Comp_Question { Block = blok_4, QuestionNumber = 12, QuestionText = "Вы начальник службы филиала. В компании развита система стажировок. В вашу службу направили молодого специалиста. Время стажировки специалиста совпало с напряженным периодом планирования МТР на следующий год. Каждый специалист в вашей службе загружен по максимуму. Помощь вам сейчас не помешает, и молодой специалист получит полезный опыт. Именно поэтому вы решили максимально включить специалиста в процесс и поручили ему выполнить часть работ, связанную с внесением информации о заявках в информационную систему (ИС)." };
            var question_13 = new Comp_Question { Block = blok_5, QuestionNumber = 13, QuestionText = "Вы начальник службы филиала. В прошлом году вы и несколько ваших коллег -  руководителей производственных служб закончили программу повышения квалификации по направлению «Управление инновационной деятельностью предприятия». Совпадение или нет, но на очередном совещании начальник филиала поручил вам выполнить новую для подразделения и сложную для вас задачу." };
            var question_14 = new Comp_Question { Block = blok_5, QuestionNumber = 14, QuestionText = "Вы начальник службы в филиале. Вы заметили, что некоторые рабочие процессы не оптимальны. На основе своего предыдущего опыта и знаний, Вы считаете, что их можно улучшить с целью повышения производительности." };
            var question_15 = new Comp_Question { Block = blok_5, QuestionNumber = 15, QuestionText = "Новый сотрудник присоединился к коллективу службы. Во время рабочего собрания руководитель поручил всем задания, которые должны быть готовы к концу недели. Все подтвердили, что поняли принципы работы. Вы заметили, что новый сотрудник вел себя нерешительно во время встречи. После собрания, он сказал Вам, что чувствует себя перегруженным от количества заданий, возложенных на него." };
            var question_16 = new Comp_Question { Block = blok_6, QuestionNumber = 16, QuestionText = "Вы – руководитель службы филиала. Ваша служба значительно отстает от поставленных сроков запуска нового оборудования. Коллектив прилагает максимальные усилия, остается сверхурочно. В это время к Вам подходит один из самых опытных сотрудников, который отвечает за решение основных вопросов по запуску оборудования, и оповещает Вас о своем уходе из компании." };
            var question_17 = new Comp_Question { Block = blok_6, QuestionNumber = 17, QuestionText = "Вы – начальник службы в филиале. Руководите работой над сложным техническим проектом. Сегодня была обнаружена серьезная неисправность, для устранения которой потребуется время, и Вы понимаете, что можете не уложиться в необходимый срок." };
            var question_18 = new Comp_Question { Block = blok_6, QuestionNumber = 18, QuestionText = "Вас назначили начальников службы филиала. Под руководством вашего предшественника службе не удавалось добиваться высоких результатов работы службы. Вам поставлена задача повысить производительность труда." };
            var question_19 = new Comp_Question { Block = blok_7, QuestionNumber = 19, QuestionText = "Вы назначены начальником службы. Для Вас это непростая задача, которая потребует много сил и быстрой адаптации к новому функционалу руководителя и содержанию работы. Вы знаете, некоторых из Ваших новых подчиненных, но большинство пришли из совершенно разных областей.  Один из сотрудников был принят в службу по рекомендации как исключительно перспективный специалист. Тем не менее, Ваши впечатления о нем не самые приятные." };
            var question_20 = new Comp_Question { Block = blok_7, QuestionNumber = 20, QuestionText = "Вы руководитель службы в филиале. Одна из ваших подчиненных работает в службе больше года и свои обязанности выполняет качественно и в срок. Но в последнее время Вы стали замечать, что она опаздывает почти каждый день на 10 – 15 минут. В общем, Вы могли бы закрыть на это глаза, но есть корпоративные правила, исполнения которых требуют прежде всего от Вас. Пару раз ей было сделано замечание, что следует все же вовремя приходить, хотя бы потому что это прописано в трудовом договоре. Вы предупредили, что будете оформлять все письменно и составлять служебные записки об ее опозданиях. Тогда сотрудница согласилась с Вами и пообещала, что ситуация изменится. Но все осталось так же. Кроме того, Вы планировали повышать ее в должности, но теперь думаете, следует ли это делать." };
            var question_21 = new Comp_Question { Block = blok_7, QuestionNumber = 21, QuestionText = "Вы руководитель службы, в состав которого входят группы по направлениям деятельности. В течение последнего времени в подразделении возникли серьезные проблемы, такие как: снижение мотивации, большое количество больничных, ухудшение результатов деятельности. Вы предполагаете, что это связано с сотрудниками конкретной группы." };
            var question_22 = new Comp_Question { Block = blok_8, QuestionNumber = 22, QuestionText = "Вы начальник службы филиала. Ежеквартально Вы предоставляете отчет о планируемых расходах МТР в профильный отдел администрации компании. В этот раз период предоставления информации совпал с вашей командировкой. На время вашего отсутствия вас будет заменять ваш подчиненный." };
            var question_23 = new Comp_Question { Block = blok_8, QuestionNumber = 23, QuestionText = "Вы – руководитель службы филиала. Вы поручили одному из опытных сотрудников задачу подготовить документы для отправки в вышестоящую организацию, еженедельно по пятницам. При проверке перед отправкой документов Вы обнаружили ошибки." };
            var question_24 = new Comp_Question { Block = blok_8, QuestionNumber = 24, QuestionText = "Вы – руководитель службы. Один из ваших опытных подчиненных в скором времени уходит на повышение в отдел администрации компании. Часть его нагрузки ляжет на молодого специалиста, который не успел набраться опыта работы и не испытывает большого желания обучаться и развиваться в этом направлении." };
            var question_25 = new Comp_Question { Block = blok_9, QuestionNumber = 25, QuestionText = "Вы начальник службы филиала. В компании внедряется новый, более оптимальный на ваш взгляд, способ ведения отчетности, разработанный внешними консультантами. Это повлечет за собой ряд изменений в обработке документации. Вы чувствуете сильное сопротивление со стороны сотрудников вашей службы, которые хотели бы сохранить привычный способ работы." };
            var question_26 = new Comp_Question { Block = blok_9, QuestionNumber = 26, QuestionText = "Вы возглавляете группу экспертов из разных служб филиалов, которая работает над проектом. На очередном совещании Вы представили результаты своей работы. Один из коллег оспаривает Ваш анализ, «набрасываясь» на Вас. Он выступает категорически против Ваших результатов." };
            var question_27 = new Comp_Question { Block = blok_9, QuestionNumber = 27, QuestionText = "Коллега попросила Вашей помощи в работе над поручением. Она показала свой подход и просила следовать ему. Но Вы считаете, что Ваш подход является более эффективным и менее затратным." };

            context.Comp_Questions.Add(question_1);
            context.Comp_Questions.Add(question_2);
            context.Comp_Questions.Add(question_3);
            context.Comp_Questions.Add(question_4);
            context.Comp_Questions.Add(question_5);
            context.Comp_Questions.Add(question_6);
            context.Comp_Questions.Add(question_7);
            context.Comp_Questions.Add(question_8);
            context.Comp_Questions.Add(question_9);
            context.Comp_Questions.Add(question_10);
            context.Comp_Questions.Add(question_11);
            context.Comp_Questions.Add(question_12);
            context.Comp_Questions.Add(question_13);
            context.Comp_Questions.Add(question_14);
            context.Comp_Questions.Add(question_15);
            context.Comp_Questions.Add(question_16);
            context.Comp_Questions.Add(question_17);
            context.Comp_Questions.Add(question_18);
            context.Comp_Questions.Add(question_19);
            context.Comp_Questions.Add(question_20);
            context.Comp_Questions.Add(question_21);
            context.Comp_Questions.Add(question_22);
            context.Comp_Questions.Add(question_23);
            context.Comp_Questions.Add(question_24);
            context.Comp_Questions.Add(question_25);
            context.Comp_Questions.Add(question_26);
            context.Comp_Questions.Add(question_27);

            context.SaveChanges();


            var answer_1А = new Comp_Answer { Correct_Answer = 2, Question = question_1, Number = "А", Text = "Самостоятельно перераспределите обязанности, которые исполняли переведённые сотрудники, между оставшимися подчиненными, тем самым внедрите новый способ взаимодействия" };
            var answer_1Б = new Comp_Answer { Correct_Answer = 4, Question = question_1, Number = "Б", Text = "Скажете подчиненным о том, что им придется адаптироваться к новому способу взаимодействия, так как задачи, которые стоят перед службой остаются прежними" };
            var answer_1В = new Comp_Answer { Correct_Answer = 3, Question = question_1, Number = "В", Text = "В ближайшее время организуете встречу с вашими подчиненными, на которой сообщите об изменениях и обсудите их предложения по поводу сложившейся ситуации" };
            var answer_1Г = new Comp_Answer { Correct_Answer = 1, Question = question_1, Number = "Г", Text = "Проведете личные встречи с каждым из сотрудников службы, на котором обсудите их предложения и подходящий план действий в условиях новой нагрузки" };
            var answer_2А = new Comp_Answer { Correct_Answer = 1, Question = question_2, Number = "А", Text = "Составите график плавного перехода на новое ПО с учетом предложений коллег из смежных служб, но предварительно Вы изучите особенности работы на новом ПО, обсудите свои предложения с начальником филиала " };
            var answer_2Б = new Comp_Answer { Correct_Answer = 4, Question = question_2, Number = "Б", Text = "Поддержите мнение коллег из структурных подразделений о нецелесообразности внедрения ПО, выразите свое отношение на ближайшей планерке у руководства филиала" };
            var answer_2В = new Comp_Answer { Correct_Answer = 2, Question = question_2, Number = "В", Text = "Обозначите руководству филиала свою заинтересованность во внедрении нового ПО. Обратитесь к начальнику филиала с просьбой инициировать процесс внедрения " };
            var answer_2Г = new Comp_Answer { Correct_Answer = 3, Question = question_2, Number = "Г", Text = "Направите на почту всем коллегам информацию о внедряемом ПО для ознакомления. Вы считаете, что внедрение полезно для работы службы " };
            var answer_3А = new Comp_Answer { Correct_Answer = 3, Question = question_3, Number = "А", Text = "Направите приветственное письмо по электронной почте подчиненным и организуете встречу, чтобы объяснить Ваши ожидания и требования к сотрудникам" };
            var answer_3Б = new Comp_Answer { Correct_Answer = 1, Question = question_3, Number = "Б", Text = "Объясните подчиненным, что это возможность улучшить деятельность подразделения, перераспределить обязанности между всеми членами коллектива и предложите, как это можно сделать" };
            var answer_3В = new Comp_Answer { Correct_Answer = 2, Question = question_3, Number = "В", Text = "Организуете встречу объединенного коллектива, где каждый сможет внести предложения по дальнейшей работе службы, чтобы результативность не пострадала" };
            var answer_3Г = new Comp_Answer { Correct_Answer = 4, Question = question_3, Number = "Г", Text = "Вы решили ничего не предпринимать, пусть каждый самостоятельно разберется в ситуации, а потом уже видно будет как организовать работу с новым коллективом" };
            var answer_4А = new Comp_Answer { Correct_Answer = 1, Question = question_4, Number = "А", Text = "Объясните новому сотруднику, как его поведение в этой ситуации в целом влияет на результаты работы коллектива. Затем обсудите с ним шаги, которые могли бы облегчить выполнение его задачи" };
            var answer_4Б = new Comp_Answer { Correct_Answer = 4, Question = question_4, Number = "Б", Text = "Отправите всем своим подчиненным уведомление по электронной почте, в котором напомните, что данное задание обязательно к выполнению" };
            var answer_4В = new Comp_Answer { Correct_Answer = 3, Question = question_4, Number = "В", Text = "Обсудите сложившуюся ситуацию с подчиненными на ближайшей планерке. Обозначите важность отчета и скажете, что Вас беспокоит отсутствие мотивации выполнять это задание" };
            var answer_4Г = new Comp_Answer { Correct_Answer = 2, Question = question_4, Number = "Г", Text = "Поговорите наедине с новым сотрудником и скажете ему, что такое отношение к задаче неприемлемо. Напомните ему, если его поведение не изменится, то Вы примите меры" };
            var answer_5А = new Comp_Answer { Correct_Answer = 3, Question = question_5, Number = "А", Text = "Обозначите М., что ждете от него лучших результатов по работе с новым оборудованием и попросите углубиться в этом вопросе" };
            var answer_5Б = new Comp_Answer { Correct_Answer = 4, Question = question_5, Number = "Б", Text = "Объясните М., что его беспокойство по поводу недостатка знаний Вы не разделяете, так как он всегда выполнял порученные задачи" };
            var answer_5В = new Comp_Answer { Correct_Answer = 2, Question = question_5, Number = "В", Text = "Обсудите с М. приемлемые для него даты участия в корпоративном обучении, сформируете заявку на следующий год" };
            var answer_5Г = new Comp_Answer { Correct_Answer = 1, Question = question_5, Number = "Г", Text = "Обсудите с М. успехи и проблемы при освоении нового оборудования. Разберете пути решения сложностей, с которыми он столкнулся" };
            var answer_6А = new Comp_Answer { Correct_Answer = 3, Question = question_6, Number = "А", Text = "Поговорите с сотрудником с глазу на глаз о том, что Вас не устраивает такое положение дел, напомните еще раз про его функционал, который он обязан выполнять" };
            var answer_6Б = new Comp_Answer { Correct_Answer = 4, Question = question_6, Number = "Б", Text = "Соберете сотрудников Вашей службы. Расскажите о сложившейся ситуации. Выслушаете предложения о том, как поступить с этим сотрудником. И примите итоговое решение" };
            var answer_6В = new Comp_Answer { Correct_Answer = 2, Question = question_6, Number = "В", Text = "Проанализируете сложности, с которыми сталкивается сотрудник в ходе выполнения рабочих задач, и отправите его на курсы повышения квалификации" };
            var answer_6Г = new Comp_Answer { Correct_Answer = 1, Question = question_6, Number = "Г", Text = "Обсудите с сотрудником низкие результаты работы, придете к единому пониманию причин таких показателей, поможете найти возможные пути их преодоления" };
            var answer_7А = new Comp_Answer { Correct_Answer = 4, Question = question_7, Number = "А", Text = "Продолжите работать над текущими задачами. Вернусь к отчету, если руководитель запросит информацию" };
            var answer_7Б = new Comp_Answer { Correct_Answer = 2, Question = question_7, Number = "Б", Text = "Начнете работать над теми пунктами отчета, которые точно понимаете, что необходимо делать. К остальному вернетесь позже" };
            var answer_7В = new Comp_Answer { Correct_Answer = 1, Question = question_7, Number = "В", Text = "Обратитесь к аналогичным отчетам за последние несколько лет, и в соответствии с ними подготовите информацию" };
            var answer_7Г = new Comp_Answer { Correct_Answer = 3, Question = question_7, Number = "Г", Text = "Обсудите с более опытным коллегой работу над отчетом для прояснения своего понимания" };
            var answer_8А = new Comp_Answer { Correct_Answer = 2, Question = question_8, Number = "А", Text = "Попросите включить вопрос продвижения проекта в повестку дня на предстоящей планерке в службе, где объясните проблемы, обратитесь за помощью к коллегам" };
            var answer_8Б = new Comp_Answer { Correct_Answer = 4, Question = question_8, Number = "Б", Text = "Вам кажется, что этот проект важен только для вашего руководителя. Поэтому Вы вернетесь к основным задачам, и будете ждать более четких инструкций и поддержки" };
            var answer_8В = new Comp_Answer { Correct_Answer = 1, Question = question_8, Number = "В", Text = "Встретитесь с вашим руководителем и обсудите нехватку времени и средств, выделяемых на проект, а также необходимость выделять больше ресурсов" };
            var answer_8Г = new Comp_Answer { Correct_Answer = 3, Question = question_8, Number = "Г", Text = "Решите посвятить проекту выходные и вечернее время. Ваш руководитель выразил свою уверенность в Вас, назначая на этот проект" };
            var answer_9А = new Comp_Answer { Correct_Answer = 2, Question = question_9, Number = "А", Text = "Изучите все списки и личные дела работников цеха, предложите свой проект на собрании коллектива" };
            var answer_9Б = new Comp_Answer { Correct_Answer = 4, Question = question_9, Number = "Б", Text = "Предложите решить этот вопрос отделу кадров. Ведь планирование штатного расписания - это их работа" };
            var answer_9В = new Comp_Answer { Correct_Answer = 1, Question = question_9, Number = "В", Text = "Предложите высказать свои пожелания всем заинтересованным лицам, проанализируете информацию" };
            var answer_9Г = new Comp_Answer { Correct_Answer = 3, Question = question_9, Number = "Г", Text = "Определите, кто будет возглавлять новые бригады, затем поручите этим людям внести свои предложения по составу бригад" };
            var answer_10А = new Comp_Answer { Correct_Answer = 1, Question = question_10, Number = "А", Text = "Обсудите с руководителями смежных служб основные этапы, задачи проекта, возможность взаимовыгодного сотрудничества между подразделениями" };
            var answer_10Б = new Comp_Answer { Correct_Answer = 2, Question = question_10, Number = "Б", Text = "Распределите задачи только своим подчинённым в рамках проекта с учетом конечных сроков реализации проекта и возможности подключения специалистов к этой работе " };
            var answer_10В = new Comp_Answer { Correct_Answer = 3, Question = question_10, Number = "В", Text = "Спланируете основные этапы работы под проектом, на встрече с подчиненными вашей службы предложите им самостоятельно распределить задачи и обозначить сроки" };
            var answer_10Г = new Comp_Answer { Correct_Answer = 4, Question = question_10, Number = "Г", Text = "Организуете встречу с начальником службы по автоматизации и дадите ему понять, что работа над проектом по автоматизации – это направление деятельности его отдела" };
            var answer_11А = new Comp_Answer { Correct_Answer = 4, Question = question_11, Number = "А", Text = "Организуете качественный ремонт станка в соответствии с запланированными сроками – в третьем квартале текущего года" };
            var answer_11Б = new Comp_Answer { Correct_Answer = 3, Question = question_11, Number = "Б", Text = "Организуете усиленный контроль за соблюдением технологического регламента при работе на станке" };
            var answer_11В = new Comp_Answer { Correct_Answer = 2, Question = question_11, Number = "В", Text = "Перераспределите нагрузку, таким образом, чтобы оборудование N работало не в полном объеме" };
            var answer_11Г = new Comp_Answer { Correct_Answer = 1, Question = question_11, Number = "Г", Text = "Скорректируете объем выпуска продукции таким образом, чтобы увеличить запасы ко второму кварталу года на случай аварийной остановки производства" };
            var answer_12А = new Comp_Answer { Correct_Answer = 4, Question = question_12, Number = "А", Text = "Скажете молодому специалисту, что в его задаче самое важное – это с высокой точностью переносить информацию из бумажного носителя в форму ИС " };
            var answer_12Б = new Comp_Answer { Correct_Answer = 3, Question = question_12, Number = "Б", Text = "Поручите молодому специалисту в кратчайшие сроки внести информацию о заявках в ИС. При возникновении вопросов обращаться к коллегам в службе " };
            var answer_12В = new Comp_Answer { Correct_Answer = 2, Question = question_12, Number = "В", Text = "Предоставите молодому сотруднику доступ к заявкам, которые были сформированы в информационной системе в прошлом году. На их примере ему будет нетрудно разобраться" };
            var answer_12Г = new Comp_Answer { Correct_Answer = 1, Question = question_12, Number = "Г", Text = "Предложите специалисту изучить регламент по заполнению формы-заявки, в котором подробно описаны шаги, типовые ошибки и способы их решения" };
            var answer_13А = new Comp_Answer { Correct_Answer = 4, Question = question_13, Number = "А", Text = "Переговорите с начальником филиала и обозначите свою неготовность работать над задачей по причине высокой загруженности" };
            var answer_13Б = new Comp_Answer { Correct_Answer = 3, Question = question_13, Number = "Б", Text = "Дождетесь более четких разъяснений о содержании задачи от начальника филиала. И только тогда займетесь организацией работы" };
            var answer_13В = new Comp_Answer { Correct_Answer = 2, Question = question_13, Number = "В", Text = "Подойдете ответственно к новой задаче и уделите часть рабочего и личного времени для того, чтобы изучить проблематику вопроса" };
            var answer_13Г = new Comp_Answer { Correct_Answer = 1, Question = question_13, Number = "Г", Text = "Инициируете работу над задачей с участием специалистов ключевых служб филиала под вашим руководством" };
            var answer_14А = new Comp_Answer { Correct_Answer = 4, Question = question_14, Number = "А", Text = "Не будете инициировать работу по оптимизации рабочих процессов, так как сейчас в этом нет большой необходимости." };
            var answer_14Б = new Comp_Answer { Correct_Answer = 3, Question = question_14, Number = "Б", Text = "Продолжите наблюдать за рабочими процессами, предложите свою помощь только тогда, когда поймете, что возникли трудности и команда не справляется." };
            var answer_14В = new Comp_Answer { Correct_Answer = 2, Question = question_14, Number = "В", Text = "Будете отслеживать в течение следующего месяца текущие результаты работы и по итогу проинформируете сотрудников каким образом процессы могут быть улучшены" };
            var answer_14Г = new Comp_Answer { Correct_Answer = 1, Question = question_14, Number = "Г", Text = "Организуете встречу сотрудников подразделения и обозначите процессы, которые требуют изменений и ваши предложения по этому поводу. Вы планируете сопровождать процесс изменений. " };
            var answer_15А = new Comp_Answer { Correct_Answer = 1, Question = question_15, Number = "А", Text = "Обсудите с коллегой в чем конкретно у него возникают сложности, предложите попробовать несколько вариантов по решению задач, будете интересоваться у него тем, как продвигается работ" };
            var answer_15Б = new Comp_Answer { Correct_Answer = 3, Question = question_15, Number = "Б", Text = "Сконцентрируетесь на выполнении своих задач. Если у сотрудника возникнут сложности, то Вы ответите на его вопросы" };
            var answer_15В = new Comp_Answer { Correct_Answer = 4, Question = question_15, Number = "В", Text = "Обозначите свою высокую рабочую нагрузку на текущей неделе, и порекомендуете обратиться к менее загруженному коллеге по отделу. " };
            var answer_15Г = new Comp_Answer { Correct_Answer = 2, Question = question_15, Number = "Г", Text = "Поделитесь с коллегой своим способом выполнения задач. Вам кажется, что он сможет выполнить все задачи к назначенному сроку, если будет следовать вашим рекомендациям" };
            var answer_16А = new Comp_Answer { Correct_Answer = 4, Question = question_16, Number = "А", Text = "Скажете сотруднику, что он поступает безответственно по отношению к Вам и коллективу в целом. И на этом закончите диалог." };
            var answer_16Б = new Comp_Answer { Correct_Answer = 2, Question = question_16, Number = "Б", Text = "Выслушиваете подчиненного, задумываетесь о том, как перераспределить его обязанности." };
            var answer_16В = new Comp_Answer { Correct_Answer = 1, Question = question_16, Number = "В", Text = "Выслушаете подчиненного, проясните причины увольнения, попытаетесь прийти к взаимовыгодному решению." };
            var answer_16Г = new Comp_Answer { Correct_Answer = 3, Question = question_16, Number = "Г", Text = "Оповестите вышестоящего руководителя о сложившейся неприятной для вас ситуации." };
            var answer_17А = new Comp_Answer { Correct_Answer = 1, Question = question_17, Number = "А", Text = "Обсудите с командой корректирующие действия, распланируете работу таким образом, чтобы завершить проект в срок" };
            var answer_17Б = new Comp_Answer { Correct_Answer = 3, Question = question_17, Number = "Б", Text = "Обсудите с начальником филиала сложившуюся ситуацию и попросите его перенести сроки сдачи проекта" };
            var answer_17В = new Comp_Answer { Correct_Answer = 2, Question = question_17, Number = "В", Text = "Завершите в срок основные работы по проекту, уделив внимание устранению поломки" };
            var answer_17Г = new Comp_Answer { Correct_Answer = 4, Question = question_17, Number = "Г", Text = "Приостановите проект на время выяснения причины поломки и ее устранения" };
            var answer_18А = new Comp_Answer { Correct_Answer = 1, Question = question_18, Number = "А", Text = "Изучите ситуацию, поговорите со своими сотрудниками, предложите вариант решения ситуации в отделе" };
            var answer_18Б = new Comp_Answer { Correct_Answer = 2, Question = question_18, Number = "Б", Text = "Внесёте в работу отдела корректировки, которые, по Вашему мнению, способны повысить результативность работы сотрудников " };
            var answer_18В = new Comp_Answer { Correct_Answer = 3, Question = question_18, Number = "В", Text = "Обратитесь к вышестоящему руководителю за разъяснениями по сложившейся ситуации. Так как решение неочевидно" };
            var answer_18Г = new Comp_Answer { Correct_Answer = 4, Question = question_18, Number = "Г", Text = "Выясните, кто виновен в неудовлетворительных результатах работы, и примите меры" };
            var answer_19А = new Comp_Answer { Correct_Answer = 1, Question = question_19, Number = "А", Text = "Внимательно понаблюдаете за сотрудником в процессе выполнения ваших поручений и в командной работе, будете отслеживать результаты" };
            var answer_19Б = new Comp_Answer { Correct_Answer = 4, Question = question_19, Number = "Б", Text = "Посодействуете переводу сотрудника в другую службу, как вам кажется, там он сможет быть более успешным" };
            var answer_19В = new Comp_Answer { Correct_Answer = 3, Question = question_19, Number = "В", Text = "Запросите информацию из личного дела сотрудника у отдела кадров и внимательно ознакомитесь с ней" };
            var answer_19Г = new Comp_Answer { Correct_Answer = 2, Question = question_19, Number = "Г", Text = "Организуете работу сотрудника в паре с одним из коллег по отделу. И будете ориентироваться на его мнение, таким образом сможете оценить его качества" };
            var answer_20А = new Comp_Answer { Correct_Answer = 4, Question = question_20, Number = "А", Text = "Вы убеждены, что такие незначительные опоздания не имеют никакого значения. Раз на качество работы это никак не влияет. Зачем отчитывать сотрудников по таким мелочам" };
            var answer_20Б = new Comp_Answer { Correct_Answer = 3, Question = question_20, Number = "Б", Text = "Вы планируете поговорить с сотрудницей о том, что дисциплина должна соблюдаться при любых условиях. Сделаете официальный выговор, чтобы изменить ее отношение к работе" };
            var answer_20В = new Comp_Answer { Correct_Answer = 2, Question = question_20, Number = "В", Text = "Вы переговорите с сотрудницей по поводу ее опозданий и требований компании о соблюдении дисциплины. Предложите сместить начало работы. " };
            var answer_20Г = new Comp_Answer { Correct_Answer = 1, Question = question_20, Number = "Г", Text = "Вы организуете беседу с сотрудницей и совместно найдете подходящее решение, чтобы скорректировать график рабочего дня" };
            var answer_21А = new Comp_Answer { Correct_Answer = 2, Question = question_21, Number = "А", Text = "Вызовите руководителя группы, чтобы он объяснился и дал свою оценку происходящему" };
            var answer_21Б = new Comp_Answer { Correct_Answer = 4, Question = question_21, Number = "Б", Text = "Так как до сих пор не поступили жалобы от других сотрудников вашего отдела, оставляете все, как есть, чтобы все само разрешилось" };
            var answer_21В = new Comp_Answer { Correct_Answer = 1, Question = question_21, Number = "В", Text = "Попытаетесь узнать информацию, побеседовав с руководителем и сотрудниками группы" };
            var answer_21Г = new Comp_Answer { Correct_Answer = 3, Question = question_21, Number = "Г", Text = "На одной из планёрок с сотрудниками отдела Вы непосредственно спросите об этом у руководителя группы" };
            var answer_22А = new Comp_Answer { Correct_Answer = 1, Question = question_22, Number = "А", Text = "Поручая ему задачу вы дали точно понять, что она важна для отдела и дает возможность проявить себя. " };
            var answer_22Б = new Comp_Answer { Correct_Answer = 2, Question = question_22, Number = "Б", Text = "При планировании работ на неделю попрошу уделить внимание выполнению этой задачи." };
            var answer_22В = new Comp_Answer { Correct_Answer = 3, Question = question_22, Number = "В", Text = "На встрече с сотрудником напомню ему, что вопросы, касающиеся МТР на контроле у руководства!" };
            var answer_22Г = new Comp_Answer { Correct_Answer = 4, Question = question_22, Number = "Г", Text = "В последний рабочий день перед командировкой Вы напомните сотруднику, что все задачи на нем! " };
            var answer_23А = new Comp_Answer { Correct_Answer = 4, Question = question_23, Number = "А", Text = "Скажете сотруднику о найденной ошибке, и в следующий раз поручите подобное задание другому сотруднику." };
            var answer_23Б = new Comp_Answer { Correct_Answer = 3, Question = question_23, Number = "Б", Text = "Попросите сотрудника быть более внимательным при составлении подобных документов." };
            var answer_23В = new Comp_Answer { Correct_Answer = 2, Question = question_23, Number = "В", Text = "Обсудите ситуацию, выясните причины. Исходя из полученной информации, найдете выход из ситуации. " };
            var answer_23Г = new Comp_Answer { Correct_Answer = 1, Question = question_23, Number = "Г", Text = "Введете в практику подразделения в ходе рабочих планерок рассматривать случаи типичных ошибок и сложностей при выполнении задач." };
            var answer_24А = new Comp_Answer { Correct_Answer = 1, Question = question_24, Number = "А", Text = "Обоснуете причину необходимости работы с новым оборудованием, предоставите всю имеющуюся документацию для ознакомления. Выразите готовность оказать необходимую помощь" };
            var answer_24Б = new Comp_Answer { Correct_Answer = 4, Question = question_24, Number = "Б", Text = "Пригласите сотрудника к себе в кабинет и напомните, что в конце месяца ему предстоит самостоятельно работать на оборудовании и нужно как можно быстрее его освоить. На нем лежит большая ответственность!" };
            var answer_24В = new Comp_Answer { Correct_Answer = 3, Question = question_24, Number = "В", Text = "Предложите сотруднику взять на себя дополнительные задачи и при условии качественного выполнения будете готовы обсуждать его премирование " };
            var answer_24Г = new Comp_Answer { Correct_Answer = 2, Question = question_24, Number = "Г", Text = "Озвучите на планерке поручение сотруднику о необходимости в течение 3 недель ознакомиться с документацией, касающейся оборудования, подготовить список вопросов для прояснения у опытного сотрудника" };
            var answer_25А = new Comp_Answer { Correct_Answer = 4, Question = question_25, Number = "А", Text = "Дождетесь выхода приказа о внедрении нового способа работы. Ознакомите сотрудников службы с приказом " };
            var answer_25Б = new Comp_Answer { Correct_Answer = 3, Question = question_25, Number = "Б", Text = "Скажете коллегам о том, что это решение руководства и им придется адаптироваться к новому способу работы" };
            var answer_25В = new Comp_Answer { Correct_Answer = 1, Question = question_25, Number = "В", Text = "Проведете личные встречи с сотрудниками, чтобы выяснить причины скептического отношения и согласуете поэтапный план внедрения" };
            var answer_25Г = new Comp_Answer { Correct_Answer = 2, Question = question_25, Number = "Г", Text = "Организуете собрание с подчиненными, чтобы узнать об их сомнениях, при этом приведете доводы в пользе нового способа работы" };
            var answer_26А = new Comp_Answer { Correct_Answer = 4, Question = question_26, Number = "А", Text = "Вы выслушали доводы коллеги и переходите к следующей теме совещания, чтобы не затягивать время совещания" };
            var answer_26Б = new Comp_Answer { Correct_Answer = 2, Question = question_26, Number = "Б", Text = "Ваш анализ является результатом серьезной работы, в результатах вы уверены" };
            var answer_26В = new Comp_Answer { Correct_Answer = 1, Question = question_26, Number = "В", Text = "Вы объясните подход, который Вы выбрали, и попытаетесь понять, в чем Ваши взгляды различаются" };
            var answer_26Г = new Comp_Answer { Correct_Answer = 3, Question = question_26, Number = "Г", Text = "Вы будете ссылаться на то, что руководитель филиала разделяет вашу точку зрения" };
            var answer_27А = new Comp_Answer { Correct_Answer = 1, Question = question_27, Number = "А", Text = "Перед началом работы, Вы содержательно обсудите эту ситуацию с коллегой и убедите ее в целесообразности использовать ваш подход к работе" };
            var answer_27Б = new Comp_Answer { Correct_Answer = 3, Question = question_27, Number = "Б", Text = "Как и договорились, Вы будете работать по ее схеме, но по окончанию работы Вы сообщите, что в будущем это можно делать эффективнее" };
            var answer_27В = new Comp_Answer { Correct_Answer = 2, Question = question_27, Number = "В", Text = "Вы решаете использовать свой подход, который является более эффективным. И сообщите об этом коллеге" };
            var answer_27Г = new Comp_Answer { Correct_Answer = 4, Question = question_27, Number = "Г", Text = "Вы предпочтете следовать подходу, который соблюдает Ваша коллега, чтобы выполнить задачу" };


            context.Comp_Answers.Add(answer_1А);
            context.Comp_Answers.Add(answer_1Б);
            context.Comp_Answers.Add(answer_1В);
            context.Comp_Answers.Add(answer_1Г);
            context.Comp_Answers.Add(answer_2А);
            context.Comp_Answers.Add(answer_2Б);
            context.Comp_Answers.Add(answer_2В);
            context.Comp_Answers.Add(answer_2Г);
            context.Comp_Answers.Add(answer_3А);
            context.Comp_Answers.Add(answer_3Б);
            context.Comp_Answers.Add(answer_3В);
            context.Comp_Answers.Add(answer_3Г);
            context.Comp_Answers.Add(answer_4А);
            context.Comp_Answers.Add(answer_4Б);
            context.Comp_Answers.Add(answer_4В);
            context.Comp_Answers.Add(answer_4Г);
            context.Comp_Answers.Add(answer_5А);
            context.Comp_Answers.Add(answer_5Б);
            context.Comp_Answers.Add(answer_5В);
            context.Comp_Answers.Add(answer_5Г);
            context.Comp_Answers.Add(answer_6А);
            context.Comp_Answers.Add(answer_6Б);
            context.Comp_Answers.Add(answer_6В);
            context.Comp_Answers.Add(answer_6Г);
            context.Comp_Answers.Add(answer_7А);
            context.Comp_Answers.Add(answer_7Б);
            context.Comp_Answers.Add(answer_7В);
            context.Comp_Answers.Add(answer_7Г);
            context.Comp_Answers.Add(answer_8А);
            context.Comp_Answers.Add(answer_8Б);
            context.Comp_Answers.Add(answer_8В);
            context.Comp_Answers.Add(answer_8Г);
            context.Comp_Answers.Add(answer_9А);
            context.Comp_Answers.Add(answer_9Б);
            context.Comp_Answers.Add(answer_9В);
            context.Comp_Answers.Add(answer_9Г);
            context.Comp_Answers.Add(answer_10А);
            context.Comp_Answers.Add(answer_10Б);
            context.Comp_Answers.Add(answer_10В);
            context.Comp_Answers.Add(answer_10Г);
            context.Comp_Answers.Add(answer_11А);
            context.Comp_Answers.Add(answer_11Б);
            context.Comp_Answers.Add(answer_11В);
            context.Comp_Answers.Add(answer_11Г);
            context.Comp_Answers.Add(answer_12А);
            context.Comp_Answers.Add(answer_12Б);
            context.Comp_Answers.Add(answer_12В);
            context.Comp_Answers.Add(answer_12Г);
            context.Comp_Answers.Add(answer_13А);
            context.Comp_Answers.Add(answer_13Б);
            context.Comp_Answers.Add(answer_13В);
            context.Comp_Answers.Add(answer_13Г);
            context.Comp_Answers.Add(answer_14А);
            context.Comp_Answers.Add(answer_14Б);
            context.Comp_Answers.Add(answer_14В);
            context.Comp_Answers.Add(answer_14Г);
            context.Comp_Answers.Add(answer_15А);
            context.Comp_Answers.Add(answer_15Б);
            context.Comp_Answers.Add(answer_15В);
            context.Comp_Answers.Add(answer_15Г);
            context.Comp_Answers.Add(answer_16А);
            context.Comp_Answers.Add(answer_16Б);
            context.Comp_Answers.Add(answer_16В);
            context.Comp_Answers.Add(answer_16Г);
            context.Comp_Answers.Add(answer_17А);
            context.Comp_Answers.Add(answer_17Б);
            context.Comp_Answers.Add(answer_17В);
            context.Comp_Answers.Add(answer_17Г);
            context.Comp_Answers.Add(answer_18А);
            context.Comp_Answers.Add(answer_18Б);
            context.Comp_Answers.Add(answer_18В);
            context.Comp_Answers.Add(answer_18Г);
            context.Comp_Answers.Add(answer_19А);
            context.Comp_Answers.Add(answer_19Б);
            context.Comp_Answers.Add(answer_19В);
            context.Comp_Answers.Add(answer_19Г);
            context.Comp_Answers.Add(answer_20А);
            context.Comp_Answers.Add(answer_20Б);
            context.Comp_Answers.Add(answer_20В);
            context.Comp_Answers.Add(answer_20Г);
            context.Comp_Answers.Add(answer_21А);
            context.Comp_Answers.Add(answer_21Б);
            context.Comp_Answers.Add(answer_21В);
            context.Comp_Answers.Add(answer_21Г);
            context.Comp_Answers.Add(answer_22А);
            context.Comp_Answers.Add(answer_22Б);
            context.Comp_Answers.Add(answer_22В);
            context.Comp_Answers.Add(answer_22Г);
            context.Comp_Answers.Add(answer_23А);
            context.Comp_Answers.Add(answer_23Б);
            context.Comp_Answers.Add(answer_23В);
            context.Comp_Answers.Add(answer_23Г);
            context.Comp_Answers.Add(answer_24А);
            context.Comp_Answers.Add(answer_24Б);
            context.Comp_Answers.Add(answer_24В);
            context.Comp_Answers.Add(answer_24Г);
            context.Comp_Answers.Add(answer_25А);
            context.Comp_Answers.Add(answer_25Б);
            context.Comp_Answers.Add(answer_25В);
            context.Comp_Answers.Add(answer_25Г);
            context.Comp_Answers.Add(answer_26А);
            context.Comp_Answers.Add(answer_26Б);
            context.Comp_Answers.Add(answer_26В);
            context.Comp_Answers.Add(answer_26Г);
            context.Comp_Answers.Add(answer_27А);
            context.Comp_Answers.Add(answer_27Б);
            context.Comp_Answers.Add(answer_27В);
            context.Comp_Answers.Add(answer_27Г);

            context.SaveChanges();
        }

        public void AddCompetention_2year()
        {
            var blok_1 = new Comp_Block { Year = 2, WordInReport = "J", BlockName = "Компетенция «Готовность к изменениям»" };
            var blok_2 = new Comp_Block { Year = 2, WordInReport = "P", BlockName = "Компетенция «Мотивация и развитие подчиненных»" };
            var blok_3 = new Comp_Block { Year = 2, WordInReport = "H", BlockName = "Компетенция «Ориентация на результат»" };
            var blok_4 = new Comp_Block { Year = 2, WordInReport = "O", BlockName = "Компетенция «Планирование деятельности подразделения»" };
            var blok_5 = new Comp_Block { Year = 2, WordInReport = "I", BlockName = "Компетенция «Профессиональное развитие»" };
            var blok_6 = new Comp_Block { Year = 2, WordInReport = "G", BlockName = "Компетенция «Стрессоустойчивость и решение проблем»" };
            var blok_7 = new Comp_Block { Year = 2, WordInReport = "N", BlockName = "Компетенция «Умение обеспечить результат»" };
            var blok_8 = new Comp_Block { Year = 2, WordInReport = "K", BlockName = "Компетенция «Умение отстаивать свою позицию»" };
            var blok_9 = new Comp_Block { Year = 2, WordInReport = "F", BlockName = "Компетенция «Системное мышление»" };

            context.Comp_Blocks.Add(blok_1);
            context.Comp_Blocks.Add(blok_2);
            context.Comp_Blocks.Add(blok_3);
            context.Comp_Blocks.Add(blok_4);
            context.Comp_Blocks.Add(blok_5);
            context.Comp_Blocks.Add(blok_6);
            context.Comp_Blocks.Add(blok_7);
            context.Comp_Blocks.Add(blok_8);
            context.Comp_Blocks.Add(blok_9);

            context.SaveChanges();

            var question_1 = new Comp_Question { Block = blok_1, QuestionNumber = 1, QuestionText = "Вы – руководитель отдела. В текущем году в филиале запланирован переход на другую компьютерную платформу. Сегодня дошла очередь до Вашего ПК, платформа была переустановлена на новую." };
            var question_2 = new Comp_Question { Block = blok_1, QuestionNumber = 2, QuestionText = "Вы - руководитель отдела. Весь предыдущий год Ваша команда разрабатывала новое направление в деятельности компании и в этом году наступил период внедрения нововведений и их экспертной оценки. В это же время руководство приняло решение внедрить еще одно крупное направление, которое поручили Вашему отделу. " };
            var question_3 = new Comp_Question { Block = blok_1, QuestionNumber = 3, QuestionText = "Вы являетесь одним из преуспевающих сотрудников отдела и считаете, что достойны более высокой должности. В связи с увеличением количества сотрудников в вашем подразделении вводят дополнительную ставку заместителя начальника. Один из ваших коллег, не самый результативный, но и не отстающий сообщил о своем намерении занять эту должность. " };
            var question_4 = new Comp_Question { Block = blok_2, QuestionNumber = 4, QuestionText = "Вы - руководитель отдела. К Вашей команде больше полугода назад присоединился новый специалист. Вы не отмечаете качественных изменений в его развитии. Присутствие нового специалиста значительно замедляет работу отдела." };
            var question_5 = new Comp_Question { Block = blok_2, QuestionNumber = 5, QuestionText = "Вы - руководитель отдела. Вы решили предложить сотруднику новую и творческую задачу, выполнение которой будет способствовать более полной реализации его профессионального потенциала. Однако сотрудник без большого воодушевления встретил эту новость, отказывается выполнять задачу. Вы решили не просто побеседовать, а поставили себе цель замотивировать сотрудника. " };
            var question_6 = new Comp_Question { Block = blok_2, QuestionNumber = 6, QuestionText = "Вы - руководитель отдела. В Вашем отделе есть специалист Николай. Он работает в компании с момента ее основания. В настоящее время является одним из наиболее авторитетных и инициативных специалистов, занимающихся решением производственных проблем, имеет опыт управления. В последнее время в связи с закупкой нового оборудования и приходом значительного количества молодых специалистов в отдел, Николай стал ощущать недостаток знаний в новых областях организации и технологии производства. " };
            var question_7 = new Comp_Question { Block = blok_3, QuestionNumber = 7, QuestionText = "Вы – руководитель отдела. Вашему подчиненному в дополнение к основным обязанностям была поручена важная задача. Однако подчиненный второй раз не выполнил Ваше задание в срок, хотя твердо обещал, что подобного больше не повторится и попросил дополнительно 3 рабочих дня для завершения задания." };
            var question_8 = new Comp_Question { Block = blok_3, QuestionNumber = 8, QuestionText = "Вы получили электронное письмо от руководителя, что в ближайшее время начнется работа над программой по вашему направлению деятельности. В письме обозначены тема, список сотрудников, которые вовлечены в реализацию программы. Вам не ясно, что надо получить в конечном итоге." };
            var question_9 = new Comp_Question { Block = blok_3, QuestionNumber = 9, QuestionText = "Руководитель приглашает вас и еще двух сотрудников на встречу. Он описывает интересный проект в малознакомой для вас области, который качественно повысит эффективность деятельности всей компании. Предлагает одному из вас взять ответственность за этот проект. " };
            var question_10 = new Comp_Question { Block = blok_4, QuestionNumber = 10, QuestionText = "Вы совмещаете работу в производственном отделе с должностью молодёжного лидера филиала компании. В этом году руководство филиала приняло решение присоединиться к всероссийской экологической акции по уборке природных территорий от бытового мусора, которая проводится в последнюю субботу августа. Место проведения акции – довольно протяженный участок городского пляжа с прилегающей лесной территорией. Организацию и контроль проведения акции поручили Вам. Вы понимаете, что проведение акции имеет важное значение для компании, но при этом в августе бОльшая часть молодежного актива будет в отпуске и в филиале запланировано выполнение особо важных производственных работ." };
            var question_11 = new Comp_Question { Block = blok_4, QuestionNumber = 11, QuestionText = "Вы – руководитель отдела. На совете руководителей было определено, что Вы возглавите проект по проведению ежегодной конференции молодых специалистов и новаторов производства в компании. Вы запланировали провести открытие конференции при участии приглашенного гостя, во вторник после обеда. Накануне помощник приглашенного спикера подтвердила его участие, при этом, сообщила, что во вторник до обеда у него встреча с директором компании. " };
            var question_12 = new Comp_Question { Block = blok_4, QuestionNumber = 12, QuestionText = "Вы - руководитель отдела. Один из ключевых специалистов вашего отдела пришел к вам обсудить ситуацию: ему предложили перейти в другой отдел, ему это интересно, он видит перспективы, но понимает, что найти ему замену -  задача не из легких. " };
            var question_13 = new Comp_Question { Block = blok_5, QuestionNumber = 13, QuestionText = "Вы недавно присоединились к команде сотрудников, которая работает над проектом, у которого скоро истекают сроки сдачи. Куратор Вашего направления дал Вам задания в рамках проекта, с которыми у Вас возникли сложности, поскольку Вы не имеете достаточного опыта в этой области. " };
            var question_14 = new Comp_Question { Block = blok_5, QuestionNumber = 14, QuestionText = "Вы – сотрудник отдела. В перерыве коллега из другого отдела поделился с Вами, что сотрудники научно-технической лаборатории разработали иной подход в работе с имеющимся у Вас оборудованием. " };
            var question_15 = new Comp_Question { Block = blok_5, QuestionNumber = 15, QuestionText = "Вы уже давно занимаете должность специалиста отдела и Ваше мастерство в конкретной области не вызывает сомнения у других, и Ваш вышестоящий руководитель периодически просит поделиться своими знаниями. Тем не менее, один коллега, имеющий большой стаж работы, ставит под сомнение Ваш опыт." };
            var question_16 = new Comp_Question { Block = blok_6, QuestionNumber = 16, QuestionText = "Вы – руководитель группы в филиале. В последнее время Вы работаете в напряженном графике. Вы также готовите для начальника филиала предложение о переходе на новую сметно-нормативную базу, что позволит уменьшить объем ручной работы десяти сотрудников отдела. На утренней планерке руководитель сообщает, что через 30 минут вам нужно быть на совещании у начальника филиала в качестве докладчика. Вы понимаете, что сегодняшний день расписан по минутам и на подготовку нет времени." };
            var question_17 = new Comp_Question { Block = blok_6, QuestionNumber = 17, QuestionText = "Вы руководите коллективом из пяти человек в условиях открытого пространства одного кабинета. В отделе принято включать радио и слушать спокойную музыку. Несколько дней назад четверо сотрудников поругались с М. Он включал быструю музыку, которая не нравилась другим сотрудникам. Через пару дней работники обратились к Вам и попросили помочь решить проблему. Вы решили поговорить с М. и попросить его войти в положение. Однако он сказал, что ему не нравится та музыка, которую обычно включают другие сотрудники. А эта волна помогает ему сконцентрироваться. Он чувствует, что его права ущемляются и он не желает более терпеть подобного отношения к себе. " };
            var question_18 = new Comp_Question { Block = blok_6, QuestionNumber = 18, QuestionText = "Руководитель отдела просит Вас поработать над небольшой частью своего доклада, утверждая, что это не займет слишком много времени. У вас есть две недели, чтобы это сделать. За три дня до сдачи доклада руководитель сообщает, что требования к нему изменились. Кроме того, просит подготовить презентацию для защиты. " };
            var question_19 = new Comp_Question { Block = blok_7, QuestionNumber = 19, QuestionText = "Вашему подразделению предстоит работа по очистке оборудования от краски. В подразделении есть сотрудник, который ранее выполнял подобного рода работы, и Вы решили поручить задачу именно ему." };
            var question_20 = new Comp_Question { Block = blok_7, QuestionNumber = 20, QuestionText = "Вы - начальник отдела. К одному из ваших подчиненных обращаются сотрудники из других отделов с просьбой дать совет и информацию o работе оборудования, поскольку он достаточно компетентный. Сотрудник с энтузиазмом отвечал на просьбы коллег и давал содержательные советы. На днях вы вызвали его к себе и заручились готовностью сотрудника приступить к работе по внедрению аналогичного оборудования в вашем отделе, на работу отведено 3 месяца." };
            var question_21 = new Comp_Question { Block = blok_7, QuestionNumber = 21, QuestionText = "Вы – руководитель отдела. Пару дней назад в Ваш отдел пришел вновь принятый специалист, недавно окончивший университет и не испытывающий большого желания работать. В Компании принято в первые дни пребывания нового специалиста на рабочем месте ознакамливать с различными инструкциями, касающимися предстоящей деятельности." };
            var question_22 = new Comp_Question { Block = blok_8, QuestionNumber = 22, QuestionText = "Вы - начальник отдела. Профильный департамент ждет конкретную информацию, обязательных требований к форме отчета нет. Информацию вы готовите совместно с Вашим коллегой, также начальником отдела. Возник вопрос в отношении формы отчета, ваш коллега имеет свою устойчивую позицию в отношении допустимой формы отчета и судя по его «настрою» он не намерен Вам уступать в этом вопросе." };
            var question_23 = new Comp_Question { Block = blok_8, QuestionNumber = 23, QuestionText = "Вы - руководитель отдела. Главный офис во всех своих филиалах внедрил новые директивы, которые должны быть приняты во внимание Вашими сотрудниками при обработке документов и отчетов. При просмотре некоторого числа отчетов, Вы заметили, что Ваши подчинённые не соблюдают правила новых директив, чем вы крайне недовольны. " };
            var question_24 = new Comp_Question { Block = blok_8, QuestionNumber = 24, QuestionText = "Вы - руководитель отдела. Одной из групп отдела вы на свое усмотрение для работы выделили технику нового поколения. Стали поступать запросы от других служб, которые тоже хотели бы иметь подобную. Ваш бюджет не позволяет обеспечить всех такой техникой, и Вы считаете, что это не обязательно для всех. " };
            var question_25 = new Comp_Question { Block = blok_9, QuestionNumber = 25, QuestionText = "Вы - руководитель отдела. За последние восемь месяцев количество заказов уменьшилось. Прослеживается отрицательная динамика развития компании. На совещании руководителей обсуждалось решение - начать увольнения. Работникам решили пока ничего не сообщать. Но информация о планах все равно просочилась, и сотрудники начали реагировать: темп работы крайне снизился, многие ушли на больничный. " };
            var question_26 = new Comp_Question { Block = blok_9, QuestionNumber = 26, QuestionText = "Вы руководитель отдела. В настоящее время формируется резерв кадров, но вам из своих подчиненных некого включать. Руководитель смежного отдела рекомендовал Вам одного из своих починенных. Вы слышали, что работник обладает высокой квалификацией, но замкнутый. " };
            var question_27 = new Comp_Question { Block = blok_9, QuestionNumber = 27, QuestionText = "Вас назначили руководителем отдела. Вы узнали, что ваш предшественник был уволен из-за невыполнения плана работы отдела и регулярных опозданий сотрудников. Вы понимаете, что вас назначили для того, чтобы повлиять на ситуацию. " };

            context.Comp_Questions.Add(question_1);
            context.Comp_Questions.Add(question_2);
            context.Comp_Questions.Add(question_3);
            context.Comp_Questions.Add(question_4);
            context.Comp_Questions.Add(question_5);
            context.Comp_Questions.Add(question_6);
            context.Comp_Questions.Add(question_7);
            context.Comp_Questions.Add(question_8);
            context.Comp_Questions.Add(question_9);
            context.Comp_Questions.Add(question_10);
            context.Comp_Questions.Add(question_11);
            context.Comp_Questions.Add(question_12);
            context.Comp_Questions.Add(question_13);
            context.Comp_Questions.Add(question_14);
            context.Comp_Questions.Add(question_15);
            context.Comp_Questions.Add(question_16);
            context.Comp_Questions.Add(question_17);
            context.Comp_Questions.Add(question_18);
            context.Comp_Questions.Add(question_19);
            context.Comp_Questions.Add(question_20);
            context.Comp_Questions.Add(question_21);
            context.Comp_Questions.Add(question_22);
            context.Comp_Questions.Add(question_23);
            context.Comp_Questions.Add(question_24);
            context.Comp_Questions.Add(question_25);
            context.Comp_Questions.Add(question_26);
            context.Comp_Questions.Add(question_27);

            context.SaveChanges();

            var answer_1А = new Comp_Answer { Question = question_1, Number = "А", Text = "После работы на новой платформе соберете совещание, где расскажете о возможностях новой платформы и поделитесь с коллегами как быстрее адаптировать деятельность к проводимым изменениям", Correct_Answer = 1 };
            var answer_1Б = new Comp_Answer { Question = question_1, Number = "Б", Text = "Сегодняшний рабочий день начнете с изучения инструкции по работе с новой платформой,  будете активно ее испытывать,  открывая дополнительные возможности", Correct_Answer = 2 };
            var answer_1В = new Comp_Answer { Question = question_1, Number = "В", Text = "Позвоните программистам и попросите рассказать Вам о шагах при выполнении некоторых операций,  которые вы считаете необходимыми на сегодняшний день", Correct_Answer = 3 };
            var answer_1Г = new Comp_Answer { Question = question_1, Number = "Г", Text = "После нескольких попыток работы с новым программным обеспечением, Вы продолжите работать на прежней компьютерной платформе.", Correct_Answer = 4 };
            var answer_2А = new Comp_Answer { Question = question_2, Number = "А", Text = "В ближайшее время Вы планируете организовать обсуждение,  на котором четко распределите обязанности каждого участника команды,  согласуете с руководителем", Correct_Answer = 1 };
            var answer_2Б = new Comp_Answer { Question = question_2, Number = "Б", Text = "Вы принимаете решение пойти к руководству и объяснить о невозможности внедрения двух направлений одновременно", Correct_Answer = 3 };
            var answer_2В = new Comp_Answer { Question = question_2, Number = "В", Text = "Вы решаете оставить разработанное Вами ранее направление и сосредоточиться на разработке и внедрении нового", Correct_Answer = 4 };
            var answer_2Г = new Comp_Answer { Question = question_2, Number = "Г", Text = "Вы решаете,  что следует подключить экспертов к более точному определению процессов,  степени внедрения каждого из направлений для принятия решения", Correct_Answer = 2 };
            var answer_3А = new Comp_Answer { Question = question_3, Number = "А", Text = "Поговорить с коллегами и заручиться их поддержкой.", Correct_Answer = 3 };
            var answer_3Б = new Comp_Answer { Question = question_3, Number = "Б", Text = "Поговорить с руководителем отдела о сложившейся ситуации и своих амбициях по этому поводу.", Correct_Answer = 2 };
            var answer_3В = new Comp_Answer { Question = question_3, Number = "В", Text = "Проанализировать сложившуюся ситуацию, выявить возможные сложности.", Correct_Answer = 1 };
            var answer_3Г = new Comp_Answer { Question = question_3, Number = "Г", Text = "Выполнять свои должностные обязанности и смотреть как будет дальше развиваться ситуация.", Correct_Answer = 4 };
            var answer_4А = new Comp_Answer { Question = question_4, Number = "А", Text = "Вы предложите сотруднику найти и освоить курсы повышения квалификации, чтобы лучше разбираться в процессах отдела.", Correct_Answer = 3 };
            var answer_4Б = new Comp_Answer { Question = question_4, Number = "Б", Text = "Вы объясните сотруднику,  что он должен быть более эффективным,  самостоятельным, чтобы стать неотъемлемой частью команды.", Correct_Answer = 4 };
            var answer_4В = new Comp_Answer { Question = question_4, Number = "В", Text = "Обсудите с сотрудником результаты его работы за последние полгода,  сложности,  с которыми он столкнулся. Расскажите о своих наблюдениях за ним в процессе решения задач, совместно определите способы развития сотрудника", Correct_Answer = 1 };
            var answer_4Г = new Comp_Answer { Question = question_4, Number = "Г", Text = "Вы составите и ознакомите сотрудника с планом,  согласно которому он будет проводить время с каждым членом команды,  чтобы ознакомиться со всеми процедурами и методами работы.", Correct_Answer = 2 };
            var answer_5А = new Comp_Answer { Question = question_5, Number = "А", Text = "Вы обсудите с подчиненным сложности,  которые он отмечает,  значимость выполнения задачи для его профессионального роста и дальнейших перспектив развития", Correct_Answer = 1 };
            var answer_5Б = new Comp_Answer { Question = question_5, Number = "Б", Text = "На встрече Вы уделите внимание тому, чтобы сотрудник понял содержание задачи за счет детального описания этапов работы", Correct_Answer = 2 };
            var answer_5В = new Comp_Answer { Question = question_5, Number = "В", Text = "Вы дадите понять сотруднику, что качественное выполнение задачи будет отмечено материально", Correct_Answer = 3 };
            var answer_5Г = new Comp_Answer { Question = question_5, Number = "Г", Text = "Вы проговорите сотруднику,  что ждете от него качественно результата даже в тех задачах,  которые по какой-то причине ему не интересны", Correct_Answer = 4 };
            var answer_6А = new Comp_Answer { Question = question_6, Number = "А", Text = "При встрече с сотрудником обозначите, что ждете от него лучших результатов по новым направлениям деятельности компании. Ведь он один из ключевых сотрудников отдела", Correct_Answer = 3 };
            var answer_6Б = new Comp_Answer { Question = question_6, Number = "Б", Text = "На встрече обсудить итоговое решение производственной проблемы,  над которой работает Николай.  Его озабоченность по поводу недостатка знаний Вы не разделяете,  ведь он и не такие «вершины брал»", Correct_Answer = 4 };
            var answer_6В = new Comp_Answer { Question = question_6, Number = "В", Text = "Обсудите с Николаем его актуальные задачи. Поинтересуетесь потребностью в обучении, так как подходит период формирования заявки на обучение специалистов.", Correct_Answer = 2 };
            var answer_6Г = new Comp_Answer { Question = question_6, Number = "Г", Text = "Обсудите с Николаем рабочие задачи,  а также перспективы,  которые он для себя отмечает в компании. Разберете конкретные предложения сотрудника, варианты решений сложностей с которыми он столкнулся", Correct_Answer = 1 };
            var answer_7А = new Comp_Answer { Question = question_7, Number = "А", Text = "Дождетесь окончания срока выполнения задания,  а затем сурово поговорите наедине,  предупредив о недопустимости безответственного отношения к задаче.", Correct_Answer = 4 };
            var answer_7Б = new Comp_Answer { Question = question_7, Number = "Б", Text = "Поговорите с подчиненным о причинах повторного срыва, обсудите корректировки отклонений/невыполнения задания.", Correct_Answer = 1 };
            var answer_7В = new Comp_Answer { Question = question_7, Number = "В", Text = "Вы решили немедленно перепоручить задание другому сотруднику, так как поведение сотрудника указывает на низкую исполнительскую дисциплину.", Correct_Answer = 3 };
            var answer_7Г = new Comp_Answer { Question = question_7, Number = "Г", Text = "Вы решили спланировать ежедневные встречи с сотрудником, чтобы лично контролировать ход выполнения поставленной задачи.", Correct_Answer = 2 };
            var answer_8А = new Comp_Answer { Question = question_8, Number = "А", Text = "На встрече с руководителем отдела проясните у него требования к программе.", Correct_Answer = 3 };
            var answer_8Б = new Comp_Answer { Question = question_8, Number = "Б", Text = "Проясните представление о планируемой работе у руководителя, внесете свои предложения.", Correct_Answer = 2 };
            var answer_8В = new Comp_Answer { Question = question_8, Number = "В", Text = "Подготовите свои предложения и организуете обсуждение основных аспектов программы с коллегами, участвующими в нем.", Correct_Answer = 1 };
            var answer_8Г = new Comp_Answer { Question = question_8, Number = "Г", Text = "Подождете новых указаний по поводу программы, прежде чем что-то предпринимать.", Correct_Answer = 4 };
            var answer_9А = new Comp_Answer { Question = question_9, Number = "А", Text = "Предложу руководителю разделить ответственность за проект между нами тремя по своим направлениям,  понимая,  что у всех нас много работы.", Correct_Answer = 3 };
            var answer_9Б = new Comp_Answer { Question = question_9, Number = "Б", Text = "Возьмусь реализовывать данный проект, так как он позволит оптимизировать работу всего подразделения.", Correct_Answer = 2 };
            var answer_9В = new Comp_Answer { Question = question_9, Number = "В", Text = "Возьму на себя ответственность за проект, так как ранее имел опыт внедрения новых проектов.", Correct_Answer = 1 };
            var answer_9Г = new Comp_Answer { Question = question_9, Number = "Г", Text = "Возьму на себя ответственность за те задания,  которые будут поручены руководителем,  так как у меня очень много задач и я мало знаком со спецификой проекта.", Correct_Answer = 4 };
            var answer_10А = new Comp_Answer { Question = question_10, Number = "А", Text = "Вы распределили участников по двум направлениям работ: уборка участка городского пляжа и уборка прилегающей лесной территорией, определили основные задачи для них. Ваша команда справится.", Correct_Answer = 1 };
            var answer_10Б = new Comp_Answer { Question = question_10, Number = "Б", Text = "Вы распланируете основные работы по уборке территории,  а с ответственными будет понятно ближе к началу мероприятия,  ведь не может быть, чтобы все были заняты, кто-то будет свободен.", Correct_Answer = 4 };
            var answer_10В = new Comp_Answer { Question = question_10, Number = "В", Text = "Так как территория большая,  а с участниками могут возникнуть сложности,  решаете начать мероприятие раньше, чем последняя суббота августа.", Correct_Answer = 2 };
            var answer_10Г = new Comp_Answer { Question = question_10, Number = "Г", Text = "План без закрепленных ответственных – не будет работать.  Поэтому решили самостоятельно назначить ответственных из числа работников производственных отделов молодежного актива,  они,  как вам кажется, очень ответственные.", Correct_Answer = 3 };
            var answer_11А = new Comp_Answer { Question = question_11, Number = "А", Text = "Встречи у директора расписаны по времени, никаких оснований для беспокойства нет. Спикер начнет вовремя.", Correct_Answer = 4 };
            var answer_11Б = new Comp_Answer { Question = question_11, Number = "Б", Text = "В программе конференции все расписано по минутам и возможности что-то скорректировать у вас нет. Будете ориентировать участников на сохранение спокойствия в ожидании спикера.", Correct_Answer = 3 };
            var answer_11В = new Comp_Answer { Question = question_11, Number = "В", Text = "Вы понимаете,  что ситуация непростая и подготовите презентацию,  которую продемонстрируете участникам конференции в случае необходимости.", Correct_Answer = 1 };
            var answer_11Г = new Comp_Answer { Question = question_11, Number = "Г", Text = "В случае,  если встанет необходимость заполнить паузу перед выступлением спикера,  то Вы планируете напомнить про организационные моменты конференции.", Correct_Answer = 2 };
            var answer_12А = new Comp_Answer { Question = question_12, Number = "А", Text = "Чтобы не страдало качество работы пообещаю отпустить этого сотрудника, как только найду ему подходящую замену.", Correct_Answer = 3 };
            var answer_12Б = new Comp_Answer { Question = question_12, Number = "Б", Text = "Отпущу сотрудника в другой отдел,  так как понимаю,  что через некоторое время ему станет скучно работать здесь и он все равно уйдет, возможно, из компании.", Correct_Answer = 4 };
            var answer_12В = new Comp_Answer { Question = question_12, Number = "В", Text = "Организую сотруднику двухнедельную стажировку в другом отделе, за это время оформлю предварительную заявку на открывшуюся вакансию.", Correct_Answer = 1 };
            var answer_12Г = new Comp_Answer { Question = question_12, Number = "Г", Text = "Так как в данный момент никто лучше этого сотрудника не знает специфики работы, предложу ему самому найти и подготовить себе достойную замену.", Correct_Answer = 2 };
            var answer_13А = new Comp_Answer { Question = question_13, Number = "А", Text = "Начнете работать над заданиями,  запишите вопросы,  с которыми возникли сложности, а затем обратитесь к Вашему куратору за разъяснениями.", Correct_Answer = 2 };
            var answer_13Б = new Comp_Answer { Question = question_13, Number = "Б", Text = "Обсудите с руководителем возможность увеличения срока выполнения задания,  так как вам потребуется больше времени для поиска информации,  изучения опыта решения похожих задач для того чтобы справиться с заданием.", Correct_Answer = 1 };
            var answer_13В = new Comp_Answer { Question = question_13, Number = "В", Text = "Перед началом работы, спросите у коллег сталкивались ли они уже с подобными задачами и как действовали.", Correct_Answer = 3 };
            var answer_13Г = new Comp_Answer { Question = question_13, Number = "Г", Text = "Вы планируете обратиться к куратору с просьбой перепоручить задачу другому более опытному коллеге.", Correct_Answer = 4 };
            var answer_14А = new Comp_Answer { Question = question_14, Number = "А", Text = "Вы поинтересуетесь у сотрудников лаборатории информацией, проанализируете предложенный ими подход. Обсудите с коллегами целесообразность и необходимость данного подхода у вас в отделе.", Correct_Answer = 1 };
            var answer_14Б = new Comp_Answer { Question = question_14, Number = "Б", Text = "Вы примете к сведению информацию от коллеги. Займетесь изучением вопроса, если большинство сотрудников вашего отдела выскажутся в поддержку нового подхода.", Correct_Answer = 3 };
            var answer_14В = new Comp_Answer { Question = question_14, Number = "В", Text = "Вы решили в случае необходимости отстаивать существующий подход к работе с оборудованием. Работать в новом формате,  по Вашему мнению,  необходимости нет.", Correct_Answer = 4 };
            var answer_14Г = new Comp_Answer { Question = question_14, Number = "Г", Text = "Вы изучите ключевые аспекты нового подхода в работе с оборудованием, найдете возможность обсудить с опытными коллегами возможности и ограничения подхода.", Correct_Answer = 2 };
            var answer_15А = new Comp_Answer { Question = question_15, Number = "А", Text = "Вы будете периодически показывать коллегам из окружения оппонента свою профессиональную экспертность.", Correct_Answer = 2 };
            var answer_15Б = new Comp_Answer { Question = question_15, Number = "Б", Text = "Обсужу с коллегой, в чем конкретно у него сомнения и предложу совместно разработать обучающую программу для новичков", Correct_Answer = 1 };
            var answer_15В = new Comp_Answer { Question = question_15, Number = "В", Text = "Если большинство принимают Ваши знания и опыт, Вы не будете обращать внимание на одного сомневающегося коллегу.", Correct_Answer = 4 };
            var answer_15Г = new Comp_Answer { Question = question_15, Number = "Г", Text = "Буду доказывать свою компетентность сомневающемуся сотруднику при любом удобном случае.", Correct_Answer = 3 };
            var answer_16А = new Comp_Answer { Question = question_16, Number = "А", Text = "Я пойду. Тема совещания знакома, есть чем поделиться с руководством.", Correct_Answer = 2 };
            var answer_16Б = new Comp_Answer { Question = question_16, Number = "Б", Text = "Откажусь. Скажу, что сегодня у меня очень много дел.", Correct_Answer = 4 };
            var answer_16В = new Comp_Answer { Question = question_16, Number = "В", Text = "Пойду при условии,  что буду выступать с докладной частью,  а диалог с руководством будет вести начальник отдела", Correct_Answer = 3 };
            var answer_16Г = new Comp_Answer { Question = question_16, Number = "Г", Text = "Я непременно пойду и захвачу с собой документы для согласования моих предложений", Correct_Answer = 1 };
            var answer_17А = new Comp_Answer { Question = question_17, Number = "А", Text = "Вы соберете всех сотрудников отдела,  будете способствовать тому,  чтобы они пришли к взаимовыгодному решению.", Correct_Answer = 1 };
            var answer_17Б = new Comp_Answer { Question = question_17, Number = "Б", Text = "Вы скажете М.,  что понимаете его,  но он работает не один, а многих сотрудников эта музыка отвлекает. Слушать в наушниках музыку.", Correct_Answer = 2 };
            var answer_17В = new Comp_Answer { Question = question_17, Number = "В", Text = "Это не та задача,  в которую нужно включаться,  разберутся сами.", Correct_Answer = 4 };
            var answer_17Г = new Comp_Answer { Question = question_17, Number = "Г", Text = "Обсудите варианты решения сложившейся ситуации со «старожилом» отдела, после этого предпримете меры по урегулированию ситуации.", Correct_Answer = 3 };
            var answer_18А = new Comp_Answer { Question = question_18, Number = "А", Text = "Вы скажете руководителю подразделения,  что данная работа требует гораздо больше времени,  и что Вы не можете взять ее на себя.", Correct_Answer = 4 };
            var answer_18Б = new Comp_Answer { Question = question_18, Number = "Б", Text = "Вы согласитесь выполнить задание в обозначенное время на основании предыдущей презентации.", Correct_Answer = 1 };
            var answer_18В = new Comp_Answer { Question = question_18, Number = "В", Text = "Вы согласитесь и возьмете работу на дом, так как в рабочее время совершенно нет возможности заниматься этим.", Correct_Answer = 2 };
            var answer_18Г = new Comp_Answer { Question = question_18, Number = "Г", Text = "Вы объясните,  что у вас недостаточно времени,  чтобы самому сделать эту работу, но Вы готовы дать общие рекомендации по докладу другому сотруднику, а также проконтролировать его работу.", Correct_Answer = 3 };
            var answer_19А = new Comp_Answer { Question = question_19, Number = "А", Text = "Вы обсудите с сотрудником оборудование на котором необходимо провести работы, и установите сроки выполнения", Correct_Answer = 2 };
            var answer_19Б = new Comp_Answer { Question = question_19, Number = "Б", Text = "Вы доведете до сведения сотрудника,  что только при условии качественной очистки оборудования от краски,  возможно сохранить эстетический внешний вид оборудования", Correct_Answer = 4 };
            var answer_19В = new Comp_Answer { Question = question_19, Number = "В", Text = "Вы опишите в деталях все шаги,  которые необходимо выполнить для решения задачи,  обозначите срок, обсудите наличие требуемых материалов и по окончании встречи еще раз переспросите у сотрудника все ли ему понятно", Correct_Answer = 3 };
            var answer_19Г = new Comp_Answer { Question = question_19, Number = "Г", Text = "Вы обсудите с сотрудником сроки выполнения работ и уточните, что ему необходимо предоставить для выполнения порученной задачи", Correct_Answer = 1 };
            var answer_20А = new Comp_Answer { Question = question_20, Number = "А", Text = "Еженедельно будете запрашивать подробный письменный отчет у подчиненного о ходе работы над задачей с детальным планом работы на следующую неделю", Correct_Answer = 4 };
            var answer_20Б = new Comp_Answer { Question = question_20, Number = "Б", Text = "Вы обозначите подчиненному, что в случае возникновения затруднений необходимо незамедлительно обращаться к Вам для оперативного решения вопросов", Correct_Answer = 1 };
            var answer_20В = new Comp_Answer { Question = question_20, Number = "В", Text = "На каждой планерке Вы будете напоминать подчиненному о задаче и требовать подойти к ее решению с полной ответственностью", Correct_Answer = 3 };
            var answer_20Г = new Comp_Answer { Question = question_20, Number = "Г", Text = "После определения основных шагов по внедрению,  условитесь о том,  чтобы сотрудник информировал вас по итогам завершения каждого из этапов", Correct_Answer = 2 };
            var answer_21А = new Comp_Answer { Question = question_21, Number = "А", Text = "При разговоре с новым сотрудником поставить задачу об изучении предоставленных инструкций до 30.07.20** и после этой даты рассмотреть вопросы,  вызвавшие затруднения в процессе изучения,  совместно.", Correct_Answer = 1 };
            var answer_21Б = new Comp_Answer { Question = question_21, Number = "Б", Text = "Поставить задачу об изучении инструкций. В случае затруднений мотивировать подчинённого на продолжение работы тем, что по окончанию периода изучения и сдачи экзамена будет начисляться премия.", Correct_Answer = 4 };
            var answer_21В = new Comp_Answer { Question = question_21, Number = "В", Text = "При беседе объяснить работнику, что является основными целями этого периода ознакомления с инструкциями. Предложить проработать предоставленные документы совместно со специалистом отдела.", Correct_Answer = 3 };
            var answer_21Г = new Comp_Answer { Question = question_21, Number = "Г", Text = "При разговоре с новым сотрудником предоставить различные инструкции,  касающимися предстоящей деятельности,  и поставить задачу изучить их не позднее 30.07.20**.", Correct_Answer = 2 };
            var answer_22А = new Comp_Answer { Question = question_22, Number = "А", Text = "Вы намерены заручиться поддержкой руководства в отношении Ваших предложений и непременно скажете об этом Вашему оппоненту.", Correct_Answer = 4 };
            var answer_22Б = new Comp_Answer { Question = question_22, Number = "Б", Text = "Вы отмечаете настрой коллеги и решимость лоббировать свои интересы,  и понимаете,  что самый лучший вариант – это принять его позицию, сложно будет выдержать напор его аргументов.", Correct_Answer = 3 };
            var answer_22В = new Comp_Answer { Question = question_22, Number = "В", Text = "Не станете вступать с коллегой в обсуждение, сделаете отчет по предложенной вами форме.", Correct_Answer = 2 };
            var answer_22Г = new Comp_Answer { Question = question_22, Number = "Г", Text = "Вы намерены встретиться с коллегой,  чтобы обсудить его вопросы и сомнения,  представить преимущества предложенной Вами формы.", Correct_Answer = 1 };
            var answer_23А = new Comp_Answer { Question = question_23, Number = "А", Text = "На очередном совещании Вы еще раз проинформируете о новых директивах, и обсудите их влияние на результативность отдела.", Correct_Answer = 1 };
            var answer_23Б = new Comp_Answer { Question = question_23, Number = "Б", Text = "Представите сотрудникам перечень требований к обработке документов и отчетов в соответствии с новыми директивами и попросите о его соблюдении.", Correct_Answer = 2 };
            var answer_23В = new Comp_Answer { Question = question_23, Number = "В", Text = "Отправите напоминания о важности новых директив по электронной почте и продолжите проверять отчеты, чтобы избежать несоответствий", Correct_Answer = 3 };
            var answer_23Г = new Comp_Answer { Question = question_23, Number = "Г", Text = "Вы продолжите проверять отчеты, исправляя неточности в содержании", Correct_Answer = 4 };
            var answer_24А = new Comp_Answer { Question = question_24, Number = "А", Text = "Вы решили ослабить напряженность,  объясняя,  что на данный момент бюджет не позволяет обеспечить всех, но, как только это будет возможно, это будет сделано.", Correct_Answer = 4 };
            var answer_24Б = new Comp_Answer { Question = question_24, Number = "Б", Text = "Вы пригласите всех сотрудников отдела и сообщите подход, которым вы руководствовались при распределении техники.", Correct_Answer = 1 };
            var answer_24В = new Comp_Answer { Question = question_24, Number = "В", Text = "Вы попросите каждую службу подготовить краткое обоснование необходимости техники. На этом основании Вы примете решение.", Correct_Answer = 2 };
            var answer_24Г = new Comp_Answer { Question = question_24, Number = "Г", Text = "Вы определяете ответственного и место хранения техники,  чтобы другие группы могли одалживать её,  когда это необходимо.", Correct_Answer = 3 };
            var answer_25А = new Comp_Answer { Question = question_25, Number = "А", Text = "Вы со своими коллегами по отделу обеспокоены ситуацией,  сейчас не самое лучшее время анализировать происходящее,  надо действовать.", Correct_Answer = 4 };
            var answer_25Б = new Comp_Answer { Question = question_25, Number = "Б", Text = "Вы намерены занять сторону коллектива и выразить вышестоящему руководству недовольство в отношении решения - не сообщать коллективу о планируемых увольнениях.", Correct_Answer = 3 };
            var answer_25В = new Comp_Answer { Question = question_25, Number = "В", Text = "Вы согласны с тем, что в данной ситуации возможно только одно решение – увольнения. Необходимо ориентироваться исключительно на перспективу.", Correct_Answer = 2 };
            var answer_25Г = new Comp_Answer { Question = question_25, Number = "Г", Text = "По вашему мнению,  в этой ситуации необходимо рассмотреть возможность сокращения рабочего дня,  перехода на неполную рабочую ставку.", Correct_Answer = 1 };
            var answer_26А = new Comp_Answer { Question = question_26, Number = "А", Text = "Вы открыто говорите, что не хотите «чужака» в своем отделе. Вы не планируете менять своего мнения в дальнейшем.", Correct_Answer = 4 };
            var answer_26Б = new Comp_Answer { Question = question_26, Number = "Б", Text = "Совета коллеги вполне достаточно. Если он видит в характеристиках работника положительные черты, это является основанием для принятия решения.", Correct_Answer = 3 };
            var answer_26В = new Comp_Answer { Question = question_26, Number = "В", Text = "Обратитесь к коллегам, чтобы получить как можно больше информации о сотруднике. На одно только мнение руководителя не стоит полагаться.", Correct_Answer = 2 };
            var answer_26Г = new Comp_Answer { Question = question_26, Number = "Г", Text = "Прежде всего уточните, видит ли себя сам сотрудник в должности резерва. Затем провести оценку его индивидуальных качеств.", Correct_Answer = 1 };
            var answer_27А = new Comp_Answer { Question = question_27, Number = "А", Text = "Буду вести учет информации об опозданиях моих сотрудников, с точной фиксацией времени нахождения на рабочем месте.", Correct_Answer = 3 };
            var answer_27Б = new Comp_Answer { Question = question_27, Number = "Б", Text = "Ничего предпринимать не буду, так как только назначен на должность и еще не заработал авторитет у сотрудников.", Correct_Answer = 4 };
            var answer_27В = new Comp_Answer { Question = question_27, Number = "В", Text = "Поговорю с сотрудниками о том, что дисциплина и планы должны выполняться. В случае невыполнения буду применять дисциплинарные меры.", Correct_Answer = 2 };
            var answer_27Г = new Comp_Answer { Question = question_27, Number = "Г", Text = "Скорректирую работу отдела,  учитывая информацию от подчиненных о причинах снижения дисциплины и задачи,  стоящие перед подразделением.", Correct_Answer = 1 };


            context.Comp_Answers.Add(answer_1А);
            context.Comp_Answers.Add(answer_1Б);
            context.Comp_Answers.Add(answer_1В);
            context.Comp_Answers.Add(answer_1Г);
            context.Comp_Answers.Add(answer_2А);
            context.Comp_Answers.Add(answer_2Б);
            context.Comp_Answers.Add(answer_2В);
            context.Comp_Answers.Add(answer_2Г);
            context.Comp_Answers.Add(answer_3А);
            context.Comp_Answers.Add(answer_3Б);
            context.Comp_Answers.Add(answer_3В);
            context.Comp_Answers.Add(answer_3Г);
            context.Comp_Answers.Add(answer_4А);
            context.Comp_Answers.Add(answer_4Б);
            context.Comp_Answers.Add(answer_4В);
            context.Comp_Answers.Add(answer_4Г);
            context.Comp_Answers.Add(answer_5А);
            context.Comp_Answers.Add(answer_5Б);
            context.Comp_Answers.Add(answer_5В);
            context.Comp_Answers.Add(answer_5Г);
            context.Comp_Answers.Add(answer_6А);
            context.Comp_Answers.Add(answer_6Б);
            context.Comp_Answers.Add(answer_6В);
            context.Comp_Answers.Add(answer_6Г);
            context.Comp_Answers.Add(answer_7А);
            context.Comp_Answers.Add(answer_7Б);
            context.Comp_Answers.Add(answer_7В);
            context.Comp_Answers.Add(answer_7Г);
            context.Comp_Answers.Add(answer_8А);
            context.Comp_Answers.Add(answer_8Б);
            context.Comp_Answers.Add(answer_8В);
            context.Comp_Answers.Add(answer_8Г);
            context.Comp_Answers.Add(answer_9А);
            context.Comp_Answers.Add(answer_9Б);
            context.Comp_Answers.Add(answer_9В);
            context.Comp_Answers.Add(answer_9Г);
            context.Comp_Answers.Add(answer_10А);
            context.Comp_Answers.Add(answer_10Б);
            context.Comp_Answers.Add(answer_10В);
            context.Comp_Answers.Add(answer_10Г);
            context.Comp_Answers.Add(answer_11А);
            context.Comp_Answers.Add(answer_11Б);
            context.Comp_Answers.Add(answer_11В);
            context.Comp_Answers.Add(answer_11Г);
            context.Comp_Answers.Add(answer_12А);
            context.Comp_Answers.Add(answer_12Б);
            context.Comp_Answers.Add(answer_12В);
            context.Comp_Answers.Add(answer_12Г);
            context.Comp_Answers.Add(answer_13А);
            context.Comp_Answers.Add(answer_13Б);
            context.Comp_Answers.Add(answer_13В);
            context.Comp_Answers.Add(answer_13Г);
            context.Comp_Answers.Add(answer_14А);
            context.Comp_Answers.Add(answer_14Б);
            context.Comp_Answers.Add(answer_14В);
            context.Comp_Answers.Add(answer_14Г);
            context.Comp_Answers.Add(answer_15А);
            context.Comp_Answers.Add(answer_15Б);
            context.Comp_Answers.Add(answer_15В);
            context.Comp_Answers.Add(answer_15Г);
            context.Comp_Answers.Add(answer_16А);
            context.Comp_Answers.Add(answer_16Б);
            context.Comp_Answers.Add(answer_16В);
            context.Comp_Answers.Add(answer_16Г);
            context.Comp_Answers.Add(answer_17А);
            context.Comp_Answers.Add(answer_17Б);
            context.Comp_Answers.Add(answer_17В);
            context.Comp_Answers.Add(answer_17Г);
            context.Comp_Answers.Add(answer_18А);
            context.Comp_Answers.Add(answer_18Б);
            context.Comp_Answers.Add(answer_18В);
            context.Comp_Answers.Add(answer_18Г);
            context.Comp_Answers.Add(answer_19А);
            context.Comp_Answers.Add(answer_19Б);
            context.Comp_Answers.Add(answer_19В);
            context.Comp_Answers.Add(answer_19Г);
            context.Comp_Answers.Add(answer_20А);
            context.Comp_Answers.Add(answer_20Б);
            context.Comp_Answers.Add(answer_20В);
            context.Comp_Answers.Add(answer_20Г);
            context.Comp_Answers.Add(answer_21А);
            context.Comp_Answers.Add(answer_21Б);
            context.Comp_Answers.Add(answer_21В);
            context.Comp_Answers.Add(answer_21Г);
            context.Comp_Answers.Add(answer_22А);
            context.Comp_Answers.Add(answer_22Б);
            context.Comp_Answers.Add(answer_22В);
            context.Comp_Answers.Add(answer_22Г);
            context.Comp_Answers.Add(answer_23А);
            context.Comp_Answers.Add(answer_23Б);
            context.Comp_Answers.Add(answer_23В);
            context.Comp_Answers.Add(answer_23Г);
            context.Comp_Answers.Add(answer_24А);
            context.Comp_Answers.Add(answer_24Б);
            context.Comp_Answers.Add(answer_24В);
            context.Comp_Answers.Add(answer_24Г);
            context.Comp_Answers.Add(answer_25А);
            context.Comp_Answers.Add(answer_25Б);
            context.Comp_Answers.Add(answer_25В);
            context.Comp_Answers.Add(answer_25Г);
            context.Comp_Answers.Add(answer_26А);
            context.Comp_Answers.Add(answer_26Б);
            context.Comp_Answers.Add(answer_26В);
            context.Comp_Answers.Add(answer_26Г);
            context.Comp_Answers.Add(answer_27А);
            context.Comp_Answers.Add(answer_27Б);
            context.Comp_Answers.Add(answer_27В);
            context.Comp_Answers.Add(answer_27Г);

            context.SaveChanges();

        }
    }
}
