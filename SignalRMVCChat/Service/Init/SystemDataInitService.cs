using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using SignalRMVCChat.Models.GapChatContext;

namespace SignalRMVCChat.Service.Init
{
    public class SystemDataInitService
    {

        private static List<UserCity> _UserCities;

        public static List<UserCity> UserCities
        {
            get
            {
                if (_UserCities==null)
                {
                    _UserCities = JsonConvert.DeserializeObject<List<UserCity>>(SystemDataJson.CitiesJson);
                }
                return _UserCities;
            }
        }
        private static string _IranStatesEng =
            "East Azerbaijan-West Azerbaijan-Ardabil-Isfahan-Alborz-Ilam-Bushehr-Tehran-Chaharmahal and Bakhtiari-South Khorasan-Razavi Khorasan-North Khorasan-Khuzestan-Zanjan-Semnan-Sistan and Baluchestan-Fars-Qazvin-Qom-Kurdistan-Kerman -Kermanshah-Kohgiluyeh and Boyerahmad-Golestan-Gilan-Lorestan-Mazandaran-Markazi-Hormozgan-Hamadan-Yazd-";

        private static string _IranStates =
                @"آذربایجان شرقی-آذربایجان غربی-اردبیل-اصفهان-البرز-ایلام-بوشهر-تهران-چهارمحال و بختیاری-خراسان جنوبی-خراسان رضوی-خراسان شمالی-خوزستان-زنجان-سمنان-سیستان و بلوچستان-فارس-قزوین-قم-کردستان-کرمان-کرمانشاه-کهگیلویه و بویراحمد-گلستان-گیلان-لرستان-مازندران-مرکزی-هرمزگان-همدان-یزد-";

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
      
        /*
        private static string _IranCities =
            @"آب‌بر-آب‌پخش-آبادان-آباده-آباده طشک-آبدان-آبدانان-آبسرد-آبش‌احمد-آبعلی-آبگرم-آبی‌بیگلو-آبیک-آذرشهر-آرادان-آران و بیدگل-آرمرده-آرین‌شهر-آزادشهر-آستارا-آستانه-آستانه اشرفیه-آسمان‌آباد-آشتیان-آشخانه-آغاجاری-آق‌قلا-آقکند-آلاشت-آلونی-آمل-آواجیق-آوج-آیسک-آلنی-ابرکوه-ابریشم-ابوزیدآباد-ابوحمیظه-ابهر-احمدآباد صولت-احمدآباد-احمدسرگوراب-اختیارآباد-ادیمی-اراک-ارجمند-ارداق-اردبیل-اردستان-اردکان-اردکان-اردل-ارزوئیه-ارسک-ارسنجان-ارکواز-ارمغان‌خانه-ارومیه-اروندکنار-ازگله-ازنا-ازندریان-اژیه-اسارا-اسالم-اسپکه-استهبان-اسدآباد-اسدیه-اسفدن-اسفراین-اسفرورین-اسکو-اسلام‌آباد غرب-اسلام‌شهر-اسلامیه-اسلام‌شهر آق‌گل-اسیر-اشترینان-اشتهارد-اشکذر-اشکنان-اشنویه-اصفهان-اصلاندوز-اتاقور-افزر-افوس-اقبالیه-اقلید-الشتر-الوان-الوند-الیگودرز-امام حسن-امام‌شهر-املش-امیدیه-امیرکلا-امیریه-امین‌شهر-انابد-انار-انارستان-انارک-انبارآلوم-اندوهجرد-اندیشه-اندیمشک-اوز-اهر-اهرم-اهل-اهواز-ایج-ایذه-ایرانشهر-ایزدخواست-ایزدشهر-ایلام-ایلخچی-ایمان‌شهر-اینچه‌برون-ایوان-ایوانکی-ایواوغلی-ایور-باب انار-باباحیدر-بابارشانی-بابل-بابلسر-باجگیران-باخرز-بادرود-بار-باروق-بازار جمعه-بازرگان-باسمنج-باشت-باغ بهادران-باغ‌ملک-باغستان-باغین-بافت-بافران-بافق-باقرشهر-بالاده-بانه-بانه‌وره-بایگ-باینگان-بجستان-بجنورد-بخشایش-بدره-برازجان-بردخون-بردستان-بردسکن-بردسیر-برزک-برزول-برف‌انبار-بروات-بروجرد-بروجن-بره‌سر-بزمان-بزنجان-بستان-بستان‌آباد-بستک-بسطام-بشرویه-بفروئیه-بلبان‌آباد-بلداجی-بلده-بم-بمپور-بن-بناب-بناب جدید-بنارویه-بنت-بنجار-بندر امام خمینی-بندر انزلی-بندر ترکمن-بندر جاسک-بندر دیر-بندر دیلم-بندر ریگ-بندرعباس-بندر کنگان-بندر گز-بندر گناوه-بندر لنگه-بندر ماهشهر-بنک-بوانات-بوشهر-بوکان-بومهن-بوئین‌زهرا-بویین سفلی-بوئین و میاندشت-بهاباد-بهار-بهاران‌شهر-بهارستان-بهبهان-بهرمان-بهشهر-بهمن-بهنمیر-بیارجمند-بیجار-بیدخت-بیدستان-بیرجند-بیرم-بیستون-بیضا-بیکا-بیله‌سوار-پاتاوه-پارس‌آباد-پارسیان-پاریز-پاکدشت-پاوه-پردنجان-پردیس-پرندک (زرندیه)-پره‌سر-پل‌دختر-پل سفید-پلدشت-پول-پهله-پیرانشهر-پیربکران-پیش‌قلعه-پیشوا-پیشین-تازه‌آباد-تازه‌شهر-تازه‌کند-تازه‌کند انگوت-تاکستان-طوالش-تایباد-تبریز-تخت-تربت جام-تربت حیدریه-ترک-ترکالکی-ترکمانچای-تسوج-تفت-تفرش-تکاب-تنکابن-تنکمان-تنگ ارم-توتکابن-توحید-تودشک-توره-تویسرکان-تهران-تیتکانلو-تیران-تیکمه‌داش-جاجرم-جالق-جاورسیان-جایزان-جبالبارز-جعفرآباد-جعفریه-جغتای-جلفا-جلین-جم-جناح-جنت‌شهر-جنت‌مکان-جندق-جنگل-جوادآباد-جوانرود-جوپار-جورقان-جوزدان-جوزم-جوشقان و کامو-جوکار-جونقان-جویبار-جویم-جهرم-جیرفت-جیرنده-چابکسر-چاپشلو-چادگان-چارک-چاف و چمخاله-چالانچولان-چالوس-چاه بهار-چترود-چرام-چرمهین-چغادک-چغامیش-چغلوندی-چقابل-چکنه-چلگرد-چمران-چمستان-چمگردان-چناران-چناره-چوار-چوبر-چورزق-چویبده-چهارباغ-چهاربرج-چهاردانگه-چیتاب-حاجی‌آباد-حاجی‌آباد-حاجی‌آباد-حبیب‌آباد-حر-حسامی-حسن‌آباد-حسن‌آباد-حسن‌آباد-حسینیه-حصارگرمخان-حلب-حمزه-حمیدیا-حمیدیه-حمیل-حنا-حویق-خاتون آباد-جزیره خارگ-خاروانا-خاش-خاکعلی-خالدآباد-خامنه-خان‌ببین-خانوک-خانه زنیان-خاوران-خداجو-خرامه-خرم‌آباد (لرستان)-خرم‌آباد (مازندران)-خرمدره-خرمدشت (تاکستان)-خرمشهر-خرو (نیشابور)-خسروشهر-خشت-خشکبیجار-خشکرود-خضرآباد-خضری دشت‌بیاض-خلخال-خلیفان-خلیل‌آباد-خلیل کرد-خلیل‌شهر-خمارلو-خمام-خمیر-خمین-خمینی‌شهر-خنج-خنداب-خواجه-خواف-خوانسار-خور-خور-خوراسگان-خورزوق-خورسند-خورموج-خوسف-خوش‌رودپی-خومه‌زار-خوی-داوودآباد-دزفول-دستجرد-دلیجان (مرکزی)-رشت-ساروق-ساری-ساوه-سردرود-سرعین-سرو-سطر-سفیدسنگ-سگزی-سلطان‌شهر-سماله-سنخواست-سنگر-سورشجان-سوسنگرد-سهرورد-سیدان-سیرکان-سیمین‌شهر-شهر جدید تیس-شازند-شوشتر-شوط-شیراز-طالقان-غرق‌آباد-فسا-قائم‌شهر-قم-قنوات-قورچی‌باشی-قهدریجان-قیر-کارچان-کاظم‌آباد-کردکوی-کرمان-کرمانشاه-کشکوئیه-کوهبنان-کوهپایه-کهنوج-کیاکلا-گوجان-گراش-گتاب-گرماب-گرمی-گشت-گلدشت-گلستان-گلمورتی-کوموش دپه-گوراب زرمیخ-گوهران-گیلانغرب-لار-لامرد-لپوئی-لطیفی-لوجلی-لوندویل-مادوان-ماکو-ماهان-مجلسی-محمدآباد (اصفهان)-محمدشهر-محمودآباد (آذربایجان غربی)-مرادلو-مردهک-مرند-مریوان-مشراگه-مشگین‌شهر-معلم‌کلایه-ملایر-منج-موچش-مؤمن‌آباد-مهدی‌شهر(سنگسر)-استان سمنان-مهردشت-میاندوآب-میداود-میمند-مینودشت-مورموری-نازک علیا-نایین-ندوشن-نسیم‌شهر-نصرآباد (خراسان رضوی)-نظام‌شهر-نقده-نگور-دهستان نوبندگان-نودژ-نورآباد (فارس)-نوشهر-نهاوند-نیر (اردبیل)-نیک‌شهر-واجارگاه-ورامین-ورزنه-ویس-هادیشهر-هرمز-هشتجین-هفتگل-هماشهر (کرمان)-هندیجان-هیدوچ-";


        private static string _IranCitiesEng =
            "Karaj-Fardis-Kamal Shahr-Nazarabad-Mohammadshahr-Hashtgerd-Mahdasht-Meshkin Dasht-Chaharbagh-Shahr-e Jadid-e Hashtgerd-Eshtehard-Garmdarreh-Golsar-Kuhsar-Taleqan-Asara-Tankaman-Ardabil-Parsabad-Meshginshahr-Khalkhal-Germi-Bileh Savar-Namin-Jafarabad-Kivi-Anbaran-Abi Beyglu-Nir-Hashatjin-Sareyn-Aslan Duz-Eslamabad-e Qadim-Tazeh Kand-e Qadim-Qasabeh-Lahrud-Hir-Kolowr-Razey-Tazeh Kand-e Angut-Fakhrabad-Kuraim-Moradlu-Bushehr-Borazjan-Bandar Ganaveh-Khormoj-Bandar Kangan-Jam-Bandar Deylam-Bandar-e Deyr-Ali Shahr-Choghadak-Ab Pakhsh-Ahram-Vahdatiyeh-Kaki-Bank-Nakhl Taqi-Asaluyeh-Kharg-Dalaki-Sadabad-Shabankareh-Abdan-Bandar Rig-Bardestan-Bord Khun-Delvar-Bandar Siraf-Dowrahak-Baduleh-Tang-e Eram-Abad-Anarestan-Shonbeh-Imam Hassan-Kalameh-Riz-Bushkan-Shahr-e Kord-Borujen-Farsan-Farrokh Shahr-Lordegan-Hafshejan-Saman-Junqan-Kian-Faradonbeh-Ben-Sureshjan-Babaheydar-Boldaji-Ardal-Naqneh-Pardanjan-Shalamzar-Gahru-Gujan-Sefiddasht-Gandoman-Taqanak-Sardasht-Sudjan-Naghan-Dastana-Cholicheh-Vardanjan-Kaj-Dashtak-Nafech-Mal-e Khalifeh-Chelgard-Aluni-Haruni-Sar Khun-Bazoft-Manj-e Nesa-Samsami-Tabriz-Maragheh-Marand-Ahar-Mianeh-Bonab-Sahand-Sarab-Azarshahr-Hadishahr-Ajab Shir-Sardrud-Malekan-Shabestar-Bostanabad-Hashtrud-Osku-Ilkhchi-Mamqan-Khosrowshah-Basmenj-Gugan-Heris-Yamchi-Kaleybar-Shendabad-Sufian-Jolfa-Koshksaray-Tasuj-Torkamanchay-Kolvanaq-Leylan-Sis-Bakhshayesh-Qarah Aghaj-Mehraban-Teymurlu-Varzaqan-Zarnaq-Hurand-Khajeh-Benab e Marand-Sharabian-Vayqan-Mobarak Shahr-Sharafkhaneh-Kuzeh Kanan-Achachi-Duzduzan-Kharvana-Khamaneh-Tekmeh Dash-Aqkand-Abish Ahmad-Zonuz-Tark-Khomarlu-Kharaju-Siah Rud-Nazarkahrizi-Jowan Qaleh-Malek Kian-Shahriar-Isfahan-Kashan-Khomeyni Shahr-Najafabad-Shahin Shahr-Shahreza-Fuladshahr-Baharestan-Mobarakeh-Aran va Bidgol [3]-Golpayegan-Zarrin Shahr-Dorcheh Piaz-Dowlatabad-Falavarjan-Qahderijan-Khvorzuq-Nain-Semirom-Kelishad va Sudarjan [4]-Goldasht-Gaz-Abrisham-Khansar-Tiran-Daran-Sedeh Lenjan-Dizicheh-Varnamkhast-Dehaqan-Dastgerd-Ardestan-Chamgardan-Badrud-Imanshahr-Natanz-Chermahin-Fereydunshahr-Pir Bakran-Kushk-Varzaneh-Nushabad-Baharan Shahr-Kahriz Sang-Bagh-e Bahadoran-Zibashahr-Chadegan-Talkhuncheh-Golshahr-Buin va Miandasht [5]-Qahjaverestan-Zayandeh Rud-Gorgab-Habibabad-Majlesi-Zavareh-Dehaq-Alavicheh-Zazeran-Manzariyeh-Karkevand-Asgharabad-Khur-Nasrabad-Guged-Harand-Jowzdan-Shadpurabad-Abuzeydabad-Sefidshahr-Meymeh-Barf Anbar-Meshkat-Hana-Rozveh-Komeshcheh-Vazvan-Kuhpayeh-Sin-Golshan-Damaneh-Sagzi-Hasanabad-Nikabad-Mohammadabad-Mahabad-Asgaran-Jandaq-Baghshad-Tudeshk-Jowsheqan va Kamu [6]-Ziar-Qamsar-Afus-Rezvanshahr-Ezhiyeh-Khaledabad-Farrokhi-Kamu va Chugan-Barzok-Tarq-Vanak-Komeh-Neyasar-Bafran-Anarak-Lay Bid-Shiraz-Marvdasht-Jahrom-Fasa-Kazerun-Sadra-Darab-Firuzabad-Lar-Abadeh-Nurabad-Neyriz-Eqlid-Estahban-Gerash-Zarqan-Kavar-Lamerd-Safashahr-Qaemiyeh-Hajjiabad-Farashband-Qir-Evaz-Khonj-Kharameh-Sarvestan-Arsanjan-Saadat Shahr-Qaderabad-Ardakan-Dobiran-Jannat Shahr-Galleh Dar-Soghad-Meymand-Darian-Zahedshahr-Khesht-Surian-Banaruiyeh-Masiri-Lapui-Karzin-Seyyedan-Eshkanan-Shahr-e Pir-Beyram-Bahman-Qotbabad-Juyom-Abadeh Tashk-Khur-Mohr-Beyza-Latifi-Bab Anar-Qarah Bolagh-Ij-Khumeh Zar-Konartakhteh-Miyanshahr-Izadkhast-Emam Shahr-Runiz-Mobarakabad-Sedeh-Sheshdeh-Khavaran-Meshkan-Varavi-Emad Deh-Fadami-Alamarvdasht-Khaneh Zenyan-Baladeh-Nujin-Korehi-Dezhkord-Surmaq-Mazayjan-Dehram-Kuhenjan-Khuzi-Kupon-Now Bandegan-Ahel-Hesami-Khaniman-Do Borji-Qatruyeh-Nowdan-Kamfiruz-Hamashahr [7]-Efzar-Asir-Ramjerd-Hasanabad-Soltan Shahr-Madar-e Soleyman-Baba Monir-Duzeh-Arad-Fishvar-Rasht-Bandar-e Anzali-Lahijan-Langarud-Hashtpar-Astara-Sowme'eh Sara-Astaneh-ye Ashrafiyeh-Rudsar-Fuman-Khomam-Siahkal-Rezvanshahr-Manjil-Amlash-Kiashahr-Rostamabad-Lowshan-Rudbar-Kelachay-Masal-Lasht-e Nesha-Lavandevil-Kuchesfahan-Asalem-Rahimabad-Sangar-Chaf and Chamkhaleh [8]-Chaboksar-Shaft-Pareh Sar-Luleman-Khoshk-e Bijar-Marjaghal-Kumeleh-Shalman-Bazar Jomeh-Chubar-Gurab Zarmikh-Vajargah-Haviq-Rudboneh-Lisar-Jirandeh-Rankuh-Ahmadsargurab-Maklavan-Tutkabon-Barehsar-Otaqvar-Deylaman-Masuleh-Gorgan-Gonbad-e Kavus-Aliabad-e Katul-Bandar Torkaman-Azadshahr-Kordkuy-Kalaleh-Aqqala-Minudasht-Galikash-Bandar-e Gaz-Gomishan-Siminshahr-Fazelabad-Ramian-Khan Bebin-Daland-Neginshahr-Now Kandeh-Sarkhon Kalateh-Jelin-e Olya-Anbar Olum-Maraveh Tappeh-Faraghi-Tatar-e Olya-Sangdevin-Mazraeh-Now Deh Khanduz-Incheh Borun-Hamadan-Malayer-Asadabad-Nahavand-Tuyserkan-Bahar-Kabudarahang-Lalejin-Famenin-Razan-Maryanaj-Azandarian-Qorveh-e Darjazin-Juraqan-Giyan-Mohajeran-Salehabad-Sarkan-Firuzan-Samen-Damaq-Barzul-Qahavand-Ajin-Shirin Su-Jowkar-Gol Tappeh-Farasfaj-Zangeneh-Bandar Abbas-Minab-Qeshm-Rudan-Bandar Lengeh-Kish-Hajjiabad-Kong-Dargahan-Bandar Khamir-Jask-Parsian-Bastak-Bika-Jenah-Hasht Bandi-Ruydar-Hormuz-Suza-Qaleh Qazi-Sirik-Tirur-Dashti-Tazian-e Pain-Abu Musa-Bandar Charak-Garuk-Ziarat-e Ali-Fin-Kushk-e Nar-Takht-Kuhestak-Lamazan-Senderk-Fareghan-Sardasht-Gowharan-Sargaz-Dezghan-Kukherd-Ilam-Abdanan-Eyvan-Dehloran-Darreh Shahr-Arakvaz-Mehran-Sarableh-Asemanabad-Towhid-Chavar-Badreh-Shabab-Pahleh-Delgosha-Murmuri-Zarneh-Lumar-Musian-Meymeh-Sarabbagh-Salehabad-Mirza Hoseynabad-Mehr-Balavah-Kerman-Sirjan-Rafsanjan-Jiroft-Bam-Zarand-Kahnuj-Shahr-e Babak-Baft-Bardsir-Baravat-Ravar-Mohamadabad-Najaf Shahr-Mahan-Anbarabad-Manujan-Anar-Rudbar-Rabor-Qaleh Ganj-Kuhbanan-Darb-e Behesht-Baghin-Rayen-Negar-Mes-e Sarcheshmeh-Ekhtiarabad-Golbaf-Zeydabad-Zangiabad-Khursand-Pariz-Dehaj-Koshkuiyeh-Golzar-Mohammadabad-e Gonbaki-Fahraj-Arzuiyeh-Zeh-e Kalut-Jebalbarez-Chatrud-Yazdan Shahr-Nowdezh-Faryab-Reyhan Shahr-Kian Shahr-Bezenjan-Lalehzar-Aminshahr-Dow Sari-Boluk-Bahreman-Narmashir-Shahdad-Khatunabad-Jupar-Balvard-Jowzam-Kazemabad-Khanuk-Mohiabad-Anduhjerd-Khvaju Shahr-Safayyeh-Hamashahr-Dashtkar-Mardehek-Nezamshahr-Hanza-Hojedk-Kermanshah-Eslamabad-e Gharb-Kangavar-Harsin-Javanrud-Sarpol-e Zahab-Sonqor-Sahneh-Gilan-e Gharb-Paveh-Ravansar-Qasr-e Shirin-Tazehabad-Kerend-Gharb-Gahvareh-Kuzaran-Banevreh-Gowdin-Nowdeshah-Shahrak-e Rijab-Shahu-Sarmast-Bisotun-Bayangan-Nowsud-Ezgeleh-Homeyl-Satar-Robat-Miyan Rahan-Halashi-Sumar-Ahvaz-Dezful-Abadan-Bandar-e Mahshahr-Andimeshk-Khorramshahr-Behbahan-Izeh-Shushtar-Masjed Soleyman-Omidiyeh-Bandar-e Emam Khomeyni-Kut-e Abdollah [9]-Shush-Ramhormoz-Shadegan-Susangerd-Hendijan-Ramshir-Sheyban-Hamidiyeh-Gotvand-Bagh-e Malek-Chamran-Lali-Haftkel-Hoveyzeh-Veys-Mollasani-Aghajari-Sharaft-Dezab-Arvandkenar-Shamsabad-Mianrud-Qaleh Tall-Safiabad-Horr-Hosseinabad-Bostan-Chavibdeh-Elhayi-Seydun-Saleh Shahr-Sardasht-Alvan-Hamzeh-Torkalaki-Darkhoveyn-Siah Mansur-Jannat Makan-Abu Homeyzeh-Dehdez-Cham Golak-Mansuriyeh-Sardaran-Shahrak-e Babak-Kut-e Seyyed Naim-Tashan [10]-Khanafereh-Rafi-Meydavud-Saleh Moshatat-Guriyeh-Jayezan-Saland-Minushahr-Qaleh-ye Khvajeh-Bidrubeh-Moshrageh-Choghamish-Hoseyniyeh-Somaleh-Abezhdan-Zahreh-Golgir-Moqavemat-Shirin Shahr-Yasuj-Dogonbadan-Dehdasht-Charam-Likak-Madavan-Landeh-Basht-Sisakht-Suq-Dishmok-Qaleh Raisi-Margown-Pataveh-Sarfaryab-Chitab-Garab-e Sofla-Sanandaj-Saqqez-Qorveh-Marivan-Baneh-Kamyaran-Divandarreh-Bijar-Dehgolan-Kani Dinar-Serishabad-Delbaran-Sarvabad-Bolbanabad-Yasukand-Muchesh-Aavraman Takht-Dezej-Armardeh-Saheb-Zarrineh-Tup Aghaj-Buin-e Sofla-Shuyesheh-Kani Sur-Pir Taj-Bardeh Rasheh-Babarashani-Chenareh-Khorramabad-Borujerd-Dorud-Kuhdasht-Aligudarz-Nurabad-Azna-Aleshtar-Pol-e Dokhtar-Kunani-Mamulan-Chaqabol-Oshtorinan-Garab-Sepiddasht-Firuzabad-Zagheh-Darb-e Gonbad-Veysian-Bayranshahr-Sarab-e Dowreh-Momenabad-Chalanchulan-Shulehabad-e Olya-Haft Cheshmeh-Arak-Saveh-Khomeyn-Mahallat-Delijan-Shazand-Mamuniyeh-Mahajeran-Tafresh-Milajerd-Ashtian-Komijan-Khondab-Shahbaz-Astaneh-Zaviyeh-Parandak-Nimvar-Farmahin-Davudabad-Gharqabad-Khoshkrud-Javersiyan-Aveh-Karchan-Nowbaran-Khenejin-Naraq-Tureh-Saruq-Hendudur-Qurchi Bashi-Razeghi-Bazneh-Sari-Babol-Amol-Qaem Shahr-Behshahr-Chalus-Neka-Babolsar-Tonekabon-Nowshahr-Fereydunkenar-Ramsar-Juybar-Mahmudabad-Amirkola-Nur-Galugah-Ketalem and Sadat Shahr [11]-Zirab-Abbasabad-Kelardasht-Rostamkola-Khorramabad-Shirud-Chamestan-Khalil Shahr-Hachirud-Arateh-Salman Shahr-Surak-Shirgah-Pol Sefid-Kiakola-Bahnemir-Royan-Izadshahr-Gatab-Sorkhrud-Marzanabad-Nashtarud-Kelarabad-Galugah-Kiasar-Kalleh Bast-Zargarmahalleh-Pul-Kojur-Khush Rudpey-Imamzadeh Abdollah-Kuhi Kheyl-Baladeh-Dabudasht-Alasht-Rineh-Pain Hular-Marzikola-Gazanak-Farim-Bojnord-Shirvan-Esfarayen-Jajarm-Garmeh-Ashkhaneh-Faruj-Raz-Daraq-Ziarat-Safiabad-Ivar-Shahrabad-e Khavar-Titkanlu-Chenaran-Qazi-Shoqan-Sankhvast-Pish Qaleh-Lujali-Hesar-e Garmkhan-Qushkhaneh-Qazvin-Alvand-Eqbaliyeh-Takestan-Abyek-Mohammadiyeh [12]-Bidestan-Mahmudabad Nemuneh-Buin Zahra-Sharifiyeh-Shal-Esfarvarin-Danesfahan-Ziaabad-Khorramdasht-Narjeh-Abgarm-Sagzabad-Ardak-Avaj-Khak-e Ali-Moallem Kalayeh-Kuhin-Razmian-Sirdan-Qom-Qanavat-Jafariyeh-Dastjerd-Kahak-Salafchegan-Mashhad-Nishapur-Sabzevar-Torbat-e Heydarieh-Kashmar-Quchan-Torbat-e Jam-Taybad-Chenaran-Sarakhs-Gonabad-Fariman-Shahr Jadid-e Golbahar-Dargaz-Khaf-Bardaskan-Torqabeh-Feyzabad-Neqab-Shandiz-Kharv-Khalilabad-Sangan-Bajestan-Kariz-Mashhad Rizeh-Dowlatabad-Bakharz-Razaviyeh-Salehabad-Farhadgerd-Golmakan-Ahmadabad-e Sowlat-Nasrabad-Nilshahr-Nashtifan-Kalat-Jangal-Salami-Joghatai-Anabad-Kondor-Roshtkhar-Firuzeh-Darrud-Sefid Sang-Soltanabad-Rivash-Bidokht-Qasemabad-Qalandarabad-Kakhk-Rud Ab-Bar-Shahr-e Zow-Bayg-Yunesi-Shadmehr-Kadkan-Qadamgah-Davarzan-Now Khandan-Chapeshlu-Sheshtomad-Shahrabad-Lotfabad-Robat-e Sang-Chekneh-Eshqabad-Hemmatabad-Malekabad-Mazdavand-Bajgiran-Semnan-Shahrud-Damghan-Garmsar-Mehdishahr-Aradan-Eyvanki-Bastam-Sorkheh-Shahmirzad-Mojen-Kalateh Khij-Darjazin-Meyami-Kalateh Rudbar-Dibaj-Beyarjomand-Amiriyeh-Rudian-Kohanabad-Zahedan-Zabol-Iranshahr-Chabahar-Saravan-Khash-Konarak-Jaleq-Nik Shahr-Pishin-Zehak-Qasr-e Qand-Suran-Fanuj-Bampur-Zaboli-Mohamadan-Galmurti-Rasak-Mirjaveh-Dust Mohammad-Bent-Nosratabad-Bazman-Negur-Mohamadi-Bonjar-Gosht-Ali Akbar-Espakeh-Zarabad-Adimi-Nukabad-Mohammadabad-Sirkan-Hiduj-Sarbaz-Ramshar-Birjand-Qaen-Tabas-Ferdows-Nehbandan-Boshruyeh-Sarayan-Sarbisheh-Eslamiyeh-Ayask-Khezri Dasht Beyaz-Eshqabad-Hajjiabad-Asadiyeh-Seh Qaleh-Nimbeluk-Tabas-e Masina-Khusf-Esfeden-Arianshahr-Deyhuk-Mud-Shusef-Eresk-Qohestan-Gazik-Zohan-Mohammadshahr-Tehran [13]-Eslamshahr-Golestan-Qods-Malard-Varamin-Shahriar-Qarchak-Nasimshahr-Pakdasht-Pardis-Andisheh-Robat Karim-Parand-Salehieh-Damavand-Baqershahr-Baghestan-Bumahen-Chahardangeh-Pishva-Sabashahr-Kahrizak-Vahidieh-Nasirshahr-Ferdowsieh-Hasanabad-Safadasht-Rudehen-Shahedshahr-Ferunabad-Lavasan-Firuzkuh-Ahmadabad-e Mostowfi-Absard-Sharifabad-Fasham-Javadabad-Shemshak [14]-Kilan-Abali-Arjomand-Urmia-Khoy-Bukan-Mahabad-Miandoab-Salmas-Piranshahr-Naqadeh-Takab-Maku-Oshnavieh-Sardasht-Shahin Dezh-Qarah Zia od Din-Showt-Rabat-Siah Cheshmeh-Poldasht-Bazargan-Tazeh Shahr-Mohammadyar-Chahar Borj-Firuraq-Dizaj Diz-Nushin-Mahmudabad-Mirabad-Qatur-Baruq-Keshavarz-Gerd Kashaneh-Ivughli-Nazok-e Olya-Qushchi-Nalus-Marganlar-Avajiq-Serow-Silvaneh-Simmineh-Zurabad-Khalifan-Yazd-Meybod-Ardakan-Bafq-Hamidiya-Mehriz-Abarkuh-Taft-Shahediyeh-Ashkezar-Herat-Zarach-Marvast-Mehrdasht-Behabad-Bafruiyeh-Ahmadabad-Nadushan-Aqda-Nir-Khezrabad-Rezvanshahr-Zanjan-Abhar-Khorramdarreh-Qeydar-Hidaj-Sain Qaleh-Soltaniyeh-Sojas-Sohrevard-Zarrin Rud-Ab Bar-Mah Neshan-Garmab-Dandi-Nurabad-Karasf-Zarrinabad-Armaghankhaneh-Chavarzaq-Halab-Nik Pey-";


  

        /*public static List<UserCity> UserCities
        {
            get
            {
                var cities = SystemDataJson.CitiesJson.Split('-');
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

      */


        public static UserCity GetUserCity(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return null;
            }


            var isFind = UserCities.FirstOrDefault(s =>
                s.engName?.ToLower()?.Trim().Contains(city.ToLower().Trim())==true);

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



    public class SystemDataInitServiceTest
    {

        [Test]
        public void GetUserCityTest()
        {
            var res=SystemDataInitService.GetUserCity("Tabriz");
            
            Assert.NotNull(res,"شهر باید جواب داشته باشد");


            var res2=SystemDataInitService.GetUserState("West Azerbaijan");
            
            Assert.NotNull(res2,"استان باید جواب داشته باشد");



        }
    }
}