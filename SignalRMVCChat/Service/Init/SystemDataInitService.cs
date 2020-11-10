using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SignalRMVCChat.Models.GapChatContext;

namespace SignalRMVCChat.Service.Init
{
    public class SystemDataInitService
    {
        private static string _IranCities =
            @"آب‌بر-آب‌پخش-آبادان-آباده-آباده طشک-آبدان-آبدانان-آبسرد-آبش‌احمد-آبعلی-آبگرم-آبی‌بیگلو-آبیک-آذرشهر-آرادان-آران و بیدگل-آرمرده-آرین‌شهر-آزادشهر-آستارا-آستانه-آستانه اشرفیه-آسمان‌آباد-آشتیان-آشخانه-آغاجاری-آق‌قلا-آقکند-آلاشت-آلونی-آمل-آواجیق-آوج-آیسک-آلنی-ابرکوه-ابریشم-ابوزیدآباد-ابوحمیظه-ابهر-احمدآباد صولت-احمدآباد-احمدسرگوراب-اختیارآباد-ادیمی-اراک-ارجمند-ارداق-اردبیل-اردستان-اردکان-اردکان-اردل-ارزوئیه-ارسک-ارسنجان-ارکواز-ارمغان‌خانه-ارومیه-اروندکنار-ازگله-ازنا-ازندریان-اژیه-اسارا-اسالم-اسپکه-استهبان-اسدآباد-اسدیه-اسفدن-اسفراین-اسفرورین-اسکو-اسلام‌آباد غرب-اسلام‌شهر-اسلامیه-اسلام‌شهر آق‌گل-اسیر-اشترینان-اشتهارد-اشکذر-اشکنان-اشنویه-اصفهان-اصلاندوز-اتاقور-افزر-افوس-اقبالیه-اقلید-الشتر-الوان-الوند-الیگودرز-امام حسن-امام‌شهر-املش-امیدیه-امیرکلا-امیریه-امین‌شهر-انابد-انار-انارستان-انارک-انبارآلوم-اندوهجرد-اندیشه-اندیمشک-اوز-اهر-اهرم-اهل-اهواز-ایج-ایذه-ایرانشهر-ایزدخواست-ایزدشهر-ایلام-ایلخچی-ایمان‌شهر-اینچه‌برون-ایوان-ایوانکی-ایواوغلی-ایور-باب انار-باباحیدر-بابارشانی-بابل-بابلسر-باجگیران-باخرز-بادرود-بار-باروق-بازار جمعه-بازرگان-باسمنج-باشت-باغ بهادران-باغ‌ملک-باغستان-باغین-بافت-بافران-بافق-باقرشهر-بالاده-بانه-بانه‌وره-بایگ-باینگان-بجستان-بجنورد-بخشایش-بدره-برازجان-بردخون-بردستان-بردسکن-بردسیر-برزک-برزول-برف‌انبار-بروات-بروجرد-بروجن-بره‌سر-بزمان-بزنجان-بستان-بستان‌آباد-بستک-بسطام-بشرویه-بفروئیه-بلبان‌آباد-بلداجی-بلده-بم-بمپور-بن-بناب-بناب جدید-بنارویه-بنت-بنجار-بندر امام خمینی-بندر انزلی-بندر ترکمن-بندر جاسک-بندر دیر-بندر دیلم-بندر ریگ-بندرعباس-بندر کنگان-بندر گز-بندر گناوه-بندر لنگه-بندر ماهشهر-بنک-بوانات-بوشهر-بوکان-بومهن-بوئین‌زهرا-بویین سفلی-بوئین و میاندشت-بهاباد-بهار-بهاران‌شهر-بهارستان-بهبهان-بهرمان-بهشهر-بهمن-بهنمیر-بیارجمند-بیجار-بیدخت-بیدستان-بیرجند-بیرم-بیستون-بیضا-بیکا-بیله‌سوار-پاتاوه-پارس‌آباد-پارسیان-پاریز-پاکدشت-پاوه-پردنجان-پردیس-پرندک (زرندیه)-پره‌سر-پل‌دختر-پل سفید-پلدشت-پول-پهله-پیرانشهر-پیربکران-پیش‌قلعه-پیشوا-پیشین-تازه‌آباد-تازه‌شهر-تازه‌کند-تازه‌کند انگوت-تاکستان-طوالش-تایباد-تبریز-تخت-تربت جام-تربت حیدریه-ترک-ترکالکی-ترکمانچای-تسوج-تفت-تفرش-تکاب-تنکابن-تنکمان-تنگ ارم-توتکابن-توحید-تودشک-توره-تویسرکان-تهران-تیتکانلو-تیران-تیکمه‌داش-جاجرم-جالق-جاورسیان-جایزان-جبالبارز-جعفرآباد-جعفریه-جغتای-جلفا-جلین-جم-جناح-جنت‌شهر-جنت‌مکان-جندق-جنگل-جوادآباد-جوانرود-جوپار-جورقان-جوزدان-جوزم-جوشقان و کامو-جوکار-جونقان-جویبار-جویم-جهرم-جیرفت-جیرنده-چابکسر-چاپشلو-چادگان-چارک-چاف و چمخاله-چالانچولان-چالوس-چاه بهار-چترود-چرام-چرمهین-چغادک-چغامیش-چغلوندی-چقابل-چکنه-چلگرد-چمران-چمستان-چمگردان-چناران-چناره-چوار-چوبر-چورزق-چویبده-چهارباغ-چهاربرج-چهاردانگه-چیتاب-حاجی‌آباد-حاجی‌آباد-حاجی‌آباد-حبیب‌آباد-حر-حسامی-حسن‌آباد-حسن‌آباد-حسن‌آباد-حسینیه-حصارگرمخان-حلب-حمزه-حمیدیا-حمیدیه-حمیل-حنا-حویق-خاتون آباد-جزیره خارگ-خاروانا-خاش-خاکعلی-خالدآباد-خامنه-خان‌ببین-خانوک-خانه زنیان-خاوران-خداجو-خرامه-خرم‌آباد (لرستان)-خرم‌آباد (مازندران)-خرمدره-خرمدشت (تاکستان)-خرمشهر-خرو (نیشابور)-خسروشهر-خشت-خشکبیجار-خشکرود-خضرآباد-خضری دشت‌بیاض-خلخال-خلیفان-خلیل‌آباد-خلیل کرد-خلیل‌شهر-خمارلو-خمام-خمیر-خمین-خمینی‌شهر-خنج-خنداب-خواجه-خواف-خوانسار-خور-خور-خوراسگان-خورزوق-خورسند-خورموج-خوسف-خوش‌رودپی-خومه‌زار-خوی-داوودآباد-دزفول-دستجرد-دلیجان (مرکزی)-رشت-ساروق-ساری-ساوه-سردرود-سرعین-سرو-سطر-سفیدسنگ-سگزی-سلطان‌شهر-سماله-سنخواست-سنگر-سورشجان-سوسنگرد-سهرورد-سیدان-سیرکان-سیمین‌شهر-شهر جدید تیس-شازند-شوشتر-شوط-شیراز-طالقان-غرق‌آباد-فسا-قائم‌شهر-قم-قنوات-قورچی‌باشی-قهدریجان-قیر-کارچان-کاظم‌آباد-کردکوی-کرمان-کرمانشاه-کشکوئیه-کوهبنان-کوهپایه-کهنوج-کیاکلا-گوجان-گراش-گتاب-گرماب-گرمی-گشت-گلدشت-گلستان-گلمورتی-کوموش دپه-گوراب زرمیخ-گوهران-گیلانغرب-لار-لامرد-لپوئی-لطیفی-لوجلی-لوندویل-مادوان-ماکو-ماهان-مجلسی-محمدآباد (اصفهان)-محمدشهر-محمودآباد (آذربایجان غربی)-مرادلو-مردهک-مرند-مریوان-مشراگه-مشگین‌شهر-معلم‌کلایه-ملایر-منج-موچش-مؤمن‌آباد-مهدی‌شهر(سنگسر)-استان سمنان-مهردشت-میاندوآب-میداود-میمند-مینودشت-مورموری-نازک علیا-نایین-ندوشن-نسیم‌شهر-نصرآباد (خراسان رضوی)-نظام‌شهر-نقده-نگور-دهستان نوبندگان-نودژ-نورآباد (فارس)-نوشهر-نهاوند-نیر (اردبیل)-نیک‌شهر-واجارگاه-ورامین-ورزنه-ویس-هادیشهر-هرمز-هشتجین-هفتگل-هماشهر (کرمان)-هندیجان-هیدوچ-";


        private static string _IranCitiesEng =
            "Abbar-Ab-Pakhsh-Abadan-Abadeh-Abadeh Tashk-Abdan-Abdanan-Absard-Abshahmad-Abali-Abgarm-Abi-Bigloo-Abik-Azarshahr-Aradan-Aran and Bidgol-Armardeh-Arineshahr-Azadshahr-Astara-Astane-Astaneh-Astane Ashtian-Ashkhaneh-Aghajari-Aqqala-Aqkand-Alasht-Aloni-Amol-Avajiq-Avaj-Aisk-Alani-Abarkuh-Abrisham-Abuzidabad-Abu Hamizeh-Abhar-Ahmadabad Solat-Ahmadabad-Ahmad Sargorab-Ekhtiarabad -Ardbil-Ardestan-Ardakan-Ardakan-Ardal-Arzooieh-Arsak-Arsanjan-Arkavaz-Armaghanekhaneh-Urmia-Arvandkenar-Azgeleh-Azna-Azandarian-Ajiyeh-Asara-Asalem-Speke-Estahban-Asadabad-Asadieh-Esfadan-Esfarayen -Asco-Islamabad-West-Islam-Shahr-Islamieh-Islam-Shahr Aqgol-Asir-Ashtrinan-Eshtehard-Ashkzar-Ashkanan-Oshnoyeh-Isfahan-Aslanduz-Odzour-Afzar-Afos-Eghbaliyeh-Eghlid-Alshtar-Alvan-Alvand-Aligudarz-Imam Hassan- Imamshahr-Amlash-Omidieh-Amirkola-Amiriyeh-Aminshahr-Anabed-Anar-Anaristan-Anarestan-Anarak-AnbarAlum-Andohjard-Andisheh-Andimeshk-Oz-Ahar-Ahram-Ahl-Ahvaz-Ij-Izeh-Iranshahr-Izdkhast-Izadshahr-Ilam Ilkhchi-Imanshahr-Inchehbrun-Ivan-Ivanki-Ivaoghli-Iver-Bab Anar-Baba Haidar-Babarshani-Babol-Babolsar-Bajgiran-Bakhr Z-Badrud-Bar-Barough-Friday-Bazaar-Bazargan-Basmanj-Basht-Bahadoran Garden-Bagh-e-Molk-Baghistan-Baghin-Baft-Bafran-Bafgh-Baqershahr-Baladeh-Baneh-Banehoreh-Bayg-Bayangan-Bajestan-Bojnourd-Bakhshaish- Badreh-Borazjan-Bardkhoon-Bardestan-Bardaskan-Bardsir-Barzak-Barzul-Barf‌nbar-Barvat-Borujerd-Borujen-Barhsar-Bazman-Boznjan-Bostan-Bostan-Abad-Bastak-Bastam-Bashruyeh-Bfruyeh-Belban-Abad-Beldam Bampur-Ben-Bonab-New Bonab-Benaroyeh-Bint-Benjar-Imam Khomeini Port-Anzali Port-Turkmen Port-Jask Port-Deir Port-Deylam Port-Rig Port-Bandar Abbas-Kangan Port-Gaz Port-Genaveh Port-Lengeh Port -Bandar Mahshahr-Bank-Bavanat-Bushehr-Bukan-Bumhan-Buin-Zahra-Lower Buin-Buin and Miandasht-Bahabad-Spring-Spring-City-Baharestan-Behbahan-Bahrman-Behshahr-Bahman-Bahnmir-Biarjomand-Bijar-Bidakht-Bidokht-Bidakht -Biram-Biston-Beiza-Bika-Bilesavar-Pataveh-Parsabad-Parsian-Paris-Pakdasht-Paveh-Pardanjan-Pardis-Parandak (Zarandieh) -Prehsar-Pol-e-Dokhtar-Pol-e-Sefid-Pol-e-Dasht-Pul-Pahleh-Piranshahr -Pishva-Pishin-Taze-Abad-Taze-Shahr-Taze-Taze-Tone-Torbat-Jam-Torbat-Heydariyeh-Turk-Turkalki-Turkmanchay-Tasuj-Taft-Tafresh-Takab-Tan Kabon-Tonkman-Tang Eram-Tutkabon-Tohid-Todeshk-Toure-Tuyserkan-Tehran-Titkanlu-Tiran-Tikmehdash-Jajarm-Jaleq-Javarsian-Jaizan-Jabalbarz-Jafarabad-Jafaria-Jaghtai-Jolfa-Jan-Jam -Janatmakan-Jandagh-Jangal-Javadabad-Javanrood-Jopar-Jorqan-Jozdan-Jozem-Joshqan and Kamo-Jokar-Junqan-Joybar-Joyem-Jahrom-Jiroft-Jirandeh-Chabaksar-Chapshlo-Chadegan-Charchal Chaf -Chalous-Chah-e-Bahar-Chatroud-Charam-Charmahin-Chghadak-Chaghamish-Chaghlundi-Chaqabel-Chekneh-Chelgard-Chamran-Chamestan-Chamgardan-Chenaran-Chenare-Chavar-Choobar-Chorzaq-Choybdeh-Chaharbagh-Chaharbarj Haji-Abad-Haji-Abad-Haji-Abad-Habib-Abad-Hur-Hesami-Hassan-Abad-Hassan-Abad-Hassan-Abad-Hosseinieh-Hesar Garm Khan-Aleppo-Hamzeh-Hamidia-Hamidieh-Hamil-Hana-Haviq-Khatun Abad-Khavan Kharaz Khameneh-Khanbabin-Khanuk-Khaneh-e-Zanian-Khavaran-Khodajo-Kharameh-Khorramabad (Lorestan) -Khorramabad (Mazandaran) -Khorramdareh-Khorramdasht (Takestan) -Khorramshahr-Khoro (Neyshabur) -Khosroshahr-Khasht-Khoshkbijar-Khoshkroud Dasht-e Bayaz-Khalkhal-Khalifan-Khalilabad-Khalil-Kurd-Khalilshahr-Khomarloo-Khomem-Khamir-Khomein-Khomeini-Shahr-Khanj-Khondab-Khajeh-Khaf-Khansar-Khor-Khor-Khorasgan-Khorzooq-Khorsand-Khormo J-Khosaf-Khoshroodpi-Khomehzar-Khoi-Davoodabad-Dezful-Dastjerd-Delijan (Central) -Rasht-Sarough-Sari-Saveh-Sardrood-Sarein-Sarv-Setar-Sefidsang-Segzi-Sultan-Shahr-Samaleh-Sankhast-Sangar-Sourjan -Sosangard-Suhraward-Seydan-Sirkan-Simin-Shahr-New city Tis-Shazand-Shushtar-Shut-Shiraz-Taleghan-Ghargabad-Fasa-Ghaemshahr-Qom-Qanvat-Ghorchi-Bashi-Qahdarijan-Qir-Karchan-Kazemoy-Kermanshah-Kermanshah -Kashkuyeh-Kuhbanan-Kuhpayeh-Kahnooj-Kiakla-Gujan-Gerash-Getab-Garmab-Garmi-Gasht-Goldasht-Golestan-Golmorti-Kumush Depe-Gorab Zarmikh-Gohran-Gilangharb-Lar-Lamerd-Lepui-Latifi-Lujl -Madwan-Mako-Mahan-Majlisi-Mohammadabad (Isfahan) -Mohammadshahr-Mahmudabad (West Azerbaijan) -Moradloo-Mardak-Marand-Marivan-Meshrageh-Meshginshahr-Moallem-Kalayeh-Malayer-Manj-Mouchesh-Mo'menabad-Mahdi Shahr Semnan-Mehrdasht-Miandoab-Midavud-Meymand-Minoodasht-Mormori-Nazak-Olya-Nain-Nodoshan-Nasimshahr-Nasrabad (Khorasan Razavi) -Nizam-Shahr-Naqadeh-Negor-Nobestangan-Nodej-Nurad-Noorabad (Fars) -Noshahr (Ardabil) -Nikushahr-Vajargah-Varamin-Varzaneh-Weiss-Hadishahr-Hormoz-Hashtjin-Haftgol-Hamashahr (Kerman) -Handijan-Hidouch-";


        private static string _IranStatesEng =
            "East Azerbaijan-West Azerbaijan-Ardabil-Isfahan-Alborz-Ilam-Bushehr-Tehran-Chaharmahal and Bakhtiari-South Khorasan-Razavi Khorasan-North Khorasan-Khuzestan-Zanjan-Semnan-Sistan and Baluchestan-Fars-Qazvin-Qom-Kurdistan-Kerman -Kermanshah-Kohgiluyeh and Boyerahmad-Golestan-Gilan-Lorestan-Mazandaran-Markazi-Hormozgan-Hamadan-Yazd-";

        private static string _IranStates =
            @"آذربایجان شرقی-آذربایجان غربی-اردبیل-اصفهان-البرز-ایلام-بوشهر-تهران-چهارمحال و بختیاری-خراسان جنوبی-خراسان رضوی-خراسان شمالی-خوزستان-زنجان-سمنان-سیستان و بلوچستان-فارس-قزوین-قم-کردستان-کرمان-کرمانشاه-کهگیلویه و بویراحمد-گلستان-گیلان-لرستان-مازندران-مرکزی-هرمزگان-همدان-یزد-";


        public static List<UserCity> UserCities
        {
            get
            {
                var cities = _IranCities.Split('-');
                var citiesEng = _IranCitiesEng.Split('-');

                List<UserCity> list = new List<UserCity>();
                for (int i = 0; i < cities.Length; i++)
                {
                    list.Add(new UserCity
                    {
                        name = cities[i],
                        engName = citiesEng[i]
                    });
                }

                return list;
            }
        }

        public static List<UserState> UserStates
        {
            get
            {
                var states = _IranStates.Split('-');
                var statesEng = _IranStatesEng.Split('-');

                List<UserState> list = new List<UserState>();
                for (int i = 0; i < states.Length; i++)
                {
                    list.Add(new UserState
                    {
                        name = states[i],
                        engName = statesEng[i]
                    });
                }

                return list;
            }
        }


        public static UserCity GetUserCity(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return null;
            }


            var isFind = UserCities.FirstOrDefault(s =>
                s.engName.ToLower().Trim().Contains(city.ToLower().Trim()));

            return isFind;
        }
        
        public static UserState GetUserState(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return null;
            }


            var isFind = UserStates.FirstOrDefault(s =>
                s.engName.ToLower().Trim().Contains(city.ToLower().Trim()));

            return isFind;
        }
    }
}