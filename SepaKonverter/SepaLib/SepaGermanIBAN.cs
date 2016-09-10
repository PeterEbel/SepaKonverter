
namespace SepaLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    
    public class SepaGermanIBAN
    {
        private const string COMMERZBANKNOCHKSUM = "10040010:10040060:10040061:10040062:10040063:20040020:20040060:20040061:20040062:20040063:25040060:25040061:29040060:29040061:30040060:30040061:30040062:30040063:31040060:31040061:36040060:36040061:37040037:37040060:37040061:44040060:44040061:47840080:48040060:48040061:50040033:50040050:50040060:50040061:50040062:50040063:55040060:55040061:60040060:60040061:67040060:67040061:69440060:70040060:70040061:70040062:70040063:70040070:76040060:76040062:85040060:85040061:86040060:86040061:10080085:10080086:10080087:10080089:10089260:10089999:20080085:20080086:20080087:20080088:20080089:20080091:20080092:20080093:20080094:20080095:20089200:21089201:23089201:25080085:25089220:26589210:26989221:27089221:29089210:30080080:30080081:30080082:30080083:30080084:30080085:30080086:30080087:30080088:30080089:30089300:30089302:33080001:33080085:33080086:33080087:33080088:35080085:35080086:35080087:35080088:35080089:36080085:36089321:37080085:37080086:37080087:37080088:37080089:37080090:37080091:37080092:37080093:37080094:37080095:37080098:37089340:37089342:40080085:44080085:44089320:44580085:48089350:50080077:50080086:50080087:50080088:50080089:50080091:50089400:50580085:50680085:50880085:50880086:51080085:51080086:51089410:51380085:52080085:55080085:55080086:60080085:60080086:60080087:60080088:60089450:63080085:67080085:67080086:67089440:68080085:68080086:70080085:70080086:70080087:70080088:70089470:70089472:76080055:76080085:76080086:76089480:76089482:79080085:79589402:85080085:85080086:85089270:86080085:86080086:86089280";
        private const string COMMERZBANKNOIBAN = "10080900:12080000:13080000:14080000:15080000:16080000:17080000:18080000:20080055:20080057:21080050:21280002:21480003:21580000:22180000:22181400:22280000:24080000:24180001:25480021:25780022:25980027:26080024:26281420:26580070:26880063:26981062:28280012:29280011:30080055:30080057:31080015:32080010:33080030:34080031:34280032:36280071:36580072:40080040:41280043:42080082:42680081:43080083:44080055:44080057:44580070:45080060:46080010:47880031:49080025:50080055:50080057:50080081:50080082:50680002:50780006:50880050:51080000:51380040:52080080:53080030:54080021:54280023:54580020:54680022:55080065:57080070:58580074:59080090:60080055:60080057:60380002:60480008:61080006:61281007:61480001:62080012:62280012:63080015:64080014:64380011:65080009:65180005:65380003:66280053:66680013:67280051:69280035:70080056:70080057:70380006:71180005:72180002:73180011:73380004:73480013:74180009:74380007:75080003:76080053:79080052:79380051:79580099:80080000:81080000:82080000:83080000:84080000:85080200:86080055:86080057:87080000";
        private static readonly Dictionary<string, string> g_vBankCodeMap;
        private static readonly Dictionary<string, int> g_vBankCodeToRuleID;
        private static readonly Dictionary<string, string> g_vExHypoDictionary;
        private static readonly SepaGermanPseudoAcctInfo[] g_vPseudoAcctInfos;
        private List<SepaGermanBundesbankInfo> m_vBundesbankTable;
        
        static SepaGermanIBAN()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("10120760", "10020890");
            dictionary.Add("13061088", "13061078");
            dictionary.Add("13091084", "13061078");
            dictionary.Add("20730000", "20030000");
            dictionary.Add("20730051", "20030000");
            dictionary.Add("20730053", "20030000");
            dictionary.Add("20730054", "20030000");
            dictionary.Add("21030000", "20030000");
            dictionary.Add("21030092", "20030000");
            dictionary.Add("21030093", "20030000");
            dictionary.Add("21030094", "20030000");
            dictionary.Add("21030095", "20030000");
            dictionary.Add("21230085", "20030000");
            dictionary.Add("21230086", "20030000");
            dictionary.Add("21430070", "20030000");
            dictionary.Add("21530080", "20030000");
            dictionary.Add("21630060", "20030000");
            dictionary.Add("21630061", "20030000");
            dictionary.Add("21630062", "20030000");
            dictionary.Add("21630063", "20030000");
            dictionary.Add("21730040", "20030000");
            dictionary.Add("21730042", "20030000");
            dictionary.Add("21730043", "20030000");
            dictionary.Add("21730044", "20030000");
            dictionary.Add("21730045", "20030000");
            dictionary.Add("21730046", "20030000");
            dictionary.Add("21830030", "20030000");
            dictionary.Add("21830032", "20030000");
            dictionary.Add("21830033", "20030000");
            dictionary.Add("21830034", "20030000");
            dictionary.Add("21830035", "20030000");
            dictionary.Add("22130075", "20030000");
            dictionary.Add("22230020", "20030000");
            dictionary.Add("22230022", "20030000");
            dictionary.Add("22230023", "20030000");
            dictionary.Add("22230025", "20030000");
            dictionary.Add("23030000", "20030000");
            dictionary.Add("24030000", "20030000");
            dictionary.Add("24130000", "20030000");
            dictionary.Add("25030000", "20030000");
            dictionary.Add("25060701", "52060410");
            dictionary.Add("25069384", "26991066");
            dictionary.Add("25430000", "20030000");
            dictionary.Add("25730000", "20030000");
            dictionary.Add("25930000", "20030000");
            dictionary.Add("25991911", "25991528");
            dictionary.Add("26030000", "20030000");
            dictionary.Add("26661912", "28069994");
            dictionary.Add("26760005", "28069956");
            dictionary.Add("27030000", "20030000");
            dictionary.Add("27090077", "26991066");
            dictionary.Add("28069293", "28069956");
            dictionary.Add("28069755", "28563749");
            dictionary.Add("28562863", "28563749");
            dictionary.Add("30120764", "30220190");
            dictionary.Add("35020030", "36020030");
            dictionary.Add("36010800", "50010200");
            dictionary.Add("36220030", "36020030");
            dictionary.Add("36520030", "36020030");
            dictionary.Add("37021100", "37020900");
            dictionary.Add("37021300", "37020900");
            dictionary.Add("37021400", "37020900");
            dictionary.Add("37069354", "37069412");
            dictionary.Add("37069577", "37069125");
            dictionary.Add("38050000", "37050198");
            dictionary.Add("44360002", "44160014");
            dictionary.Add("47261429", "47261603");
            dictionary.Add("50020160", "50320191");
            dictionary.Add("50060500", "52060410");
            dictionary.Add("50069828", "51362514");
            dictionary.Add("50761613", "50761333");
            dictionary.Add("50810900", "66010200");
            dictionary.Add("52060400", "52060410");
            dictionary.Add("52069129", "52360059");
            dictionary.Add("54020474", "54020090");
            dictionary.Add("54220576", "54220091");
            dictionary.Add("54520071", "54520194");
            dictionary.Add("54620574", "54620093");
            dictionary.Add("54760900", "52060410");
            dictionary.Add("54820674", "54520194");
            dictionary.Add("55190028", "55190000");
            dictionary.Add("55190050", "55190000");
            dictionary.Add("55190064", "55190000");
            dictionary.Add("55190065", "55190000");
            dictionary.Add("55190068", "55190000");
            dictionary.Add("55190088", "55190000");
            dictionary.Add("55190094", "55190000");
            dictionary.Add("58561250", "58564788");
            dictionary.Add("58790100", "58760954");
            dictionary.Add("59091500", "59092000");
            dictionary.Add("59091800", "59092000");
            dictionary.Add("59391100", "59392200");
            dictionary.Add("59394200", "59392200");
            dictionary.Add("59491000", "59291200");
            dictionary.Add("59491200", "59291200");
            dictionary.Add("60010700", "66010700");
            dictionary.Add("60060606", "52060410");
            dictionary.Add("60069235", "64261853");
            dictionary.Add("60069371", "65191500");
            dictionary.Add("60069463", "65362499");
            dictionary.Add("60069549", "60069417");
            dictionary.Add("60069716", "63290110");
            dictionary.Add("60120050", "60020290");
            dictionary.Add("62250182", "60050101");
            dictionary.Add("63020450", "63020086");
            dictionary.Add("64191210", "64261853");
            dictionary.Add("64191700", "64291010");
            dictionary.Add("64263273", "64292020");
            dictionary.Add("65090100", "63090100");
            dictionary.Add("65120091", "60020290");
            dictionary.Add("65490130", "63090100");
            dictionary.Add("66020150", "66020286");
            dictionary.Add("66051220", "66050101");
            dictionary.Add("66060800", "52060410");
            dictionary.Add("66069573", "68062105");
            dictionary.Add("66190100", "66190000");
            dictionary.Add("67020259", "67020190");
            dictionary.Add("67220464", "54520194");
            dictionary.Add("68020460", "68020186");
            dictionary.Add("68361394", "68391500");
            dictionary.Add("70030111", "79330111");
            dictionary.Add("70120900", "70020270");
            dictionary.Add("70169322", "70169331");
            dictionary.Add("70320305", "70320090");
            dictionary.Add("72020240", "72020070");
            dictionary.Add("72069108", "72290100");
            dictionary.Add("72120079", "72120078");
            dictionary.Add("72120207", "72120078");
            dictionary.Add("73161455", "73369264");
            dictionary.Add("73191500", "63090100");
            dictionary.Add("73320442", "73320073");
            dictionary.Add("73420546", "73420071");
            dictionary.Add("74020414", "74020074");
            dictionary.Add("74120514", "74320073");
            dictionary.Add("74320307", "74320073");
            dictionary.Add("75020314", "75020073");
            dictionary.Add("75069076", "75069061");
            dictionary.Add("76020214", "76020070");
            dictionary.Add("76330111", "79330111");
            dictionary.Add("77030111", "79330111");
            dictionary.Add("77069893", "78060896");
            dictionary.Add("78020429", "78020070");
            dictionary.Add("78062488", "77069908");
            dictionary.Add("78330111", "79330111");
            dictionary.Add("79020325", "79020076");
            dictionary.Add("79320432", "79320075");
            dictionary.Add("79520533", "79520070");
            dictionary.Add("79562225", "79562514");
            dictionary.Add("80054000", "80053000");
            dictionary.Add("81050000", "80055500");
            dictionary.Add("82060800", "52060410");
            dictionary.Add("84030111", "79330111");
            dictionary.Add("85020890", "85020086");
            dictionary.Add("86020880", "86020086");
            g_vBankCodeMap = dictionary;
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("72020700", 2);
            dictionary2.Add("51010800", 3);
            dictionary2.Add("10050000", 4);
            dictionary2.Add("70150000", 6);
            dictionary2.Add("37050198", 7);
            dictionary2.Add("10020200", 8);
            dictionary2.Add("20120200", 8);
            dictionary2.Add("25020200", 8);
            dictionary2.Add("30020500", 8);
            dictionary2.Add("50020200", 8);
            dictionary2.Add("55020000", 8);
            dictionary2.Add("60120200", 8);
            dictionary2.Add("70220200", 8);
            dictionary2.Add("68351976", 9);
            dictionary2.Add("50050201", 10);
            dictionary2.Add("32050000", 11);
            dictionary2.Add("50850049", 12);
            dictionary2.Add("40050000", 13);
            dictionary2.Add("44050000", 13);
            dictionary2.Add("10090603", 14);
            dictionary2.Add("12090640", 14);
            dictionary2.Add("20090602", 14);
            dictionary2.Add("21090619", 14);
            dictionary2.Add("23092620", 14);
            dictionary2.Add("25090608", 14);
            dictionary2.Add("26560625", 14);
            dictionary2.Add("27090618", 14);
            dictionary2.Add("28090633", 14);
            dictionary2.Add("29090605", 14);
            dictionary2.Add("30060601", 14);
            dictionary2.Add("33060616", 14);
            dictionary2.Add("35060632", 14);
            dictionary2.Add("36060610", 14);
            dictionary2.Add("37060615", 14);
            dictionary2.Add("39060630", 14);
            dictionary2.Add("40060614", 14);
            dictionary2.Add("44060604", 14);
            dictionary2.Add("50090607", 14);
            dictionary2.Add("50890634", 14);
            dictionary2.Add("51090636", 14);
            dictionary2.Add("52090611", 14);
            dictionary2.Add("53390635", 14);
            dictionary2.Add("54690623", 14);
            dictionary2.Add("55060831", 14);
            dictionary2.Add("57060612", 14);
            dictionary2.Add("59090626", 14);
            dictionary2.Add("60090609", 14);
            dictionary2.Add("66090621", 14);
            dictionary2.Add("67090617", 14);
            dictionary2.Add("68090622", 14);
            dictionary2.Add("70090606", 14);
            dictionary2.Add("75090629", 14);
            dictionary2.Add("76090613", 14);
            dictionary2.Add("77390628", 14);
            dictionary2.Add("79090624", 14);
            dictionary2.Add("37060193", 15);
            dictionary2.Add("37160087", 0x10);
            dictionary2.Add("38060186", 0x11);
            dictionary2.Add("39060180", 0x12);
            dictionary2.Add("50130100", 0x13);
            dictionary2.Add("50220200", 0x13);
            dictionary2.Add("70030800", 0x13);
            dictionary2.Add("76026000", 20);
            dictionary2.Add("35020030", 0x15);
            dictionary2.Add("36220030", 0x15);
            dictionary2.Add("36520030", 0x15);
            dictionary2.Add("43060967", 0x16);
            dictionary2.Add("26590025", 0x17);
            dictionary2.Add("36060295", 0x18);
            dictionary2.Add("60250184", 0x19);
            dictionary2.Add("60450193", 0x19);
            dictionary2.Add("61050181", 0x19);
            dictionary2.Add("61150185", 0x19);
            dictionary2.Add("61450191", 0x19);
            dictionary2.Add("62050181", 0x19);
            dictionary2.Add("62250182", 0x19);
            dictionary2.Add("63050181", 0x19);
            dictionary2.Add("64050181", 0x19);
            dictionary2.Add("64150182", 0x19);
            dictionary2.Add("64450288", 0x19);
            dictionary2.Add("65050281", 0x19);
            dictionary2.Add("65350186", 0x19);
            dictionary2.Add("35060190", 0x1a);
            dictionary2.Add("32060362", 0x1b);
            dictionary2.Add("25050299", 0x1c);
            dictionary2.Add("51210800", 0x1d);
            dictionary2.Add("13091054", 30);
            dictionary2.Add("10120760", 0x1f);
            dictionary2.Add("30120764", 0x1f);
            dictionary2.Add("50020160", 0x1f);
            dictionary2.Add("54020474", 0x1f);
            dictionary2.Add("54220576", 0x1f);
            dictionary2.Add("54520071", 0x1f);
            dictionary2.Add("54620574", 0x1f);
            dictionary2.Add("54820674", 0x1f);
            dictionary2.Add("60120050", 0x1f);
            dictionary2.Add("63020450", 0x1f);
            dictionary2.Add("66020150", 0x1f);
            dictionary2.Add("67020259", 0x1f);
            dictionary2.Add("67220464", 0x1f);
            dictionary2.Add("68020460", 0x1f);
            dictionary2.Add("70020001", 0x1f);
            dictionary2.Add("70320305", 0x1f);
            dictionary2.Add("72020240", 0x1f);
            dictionary2.Add("72120207", 0x1f);
            dictionary2.Add("73320442", 0x1f);
            dictionary2.Add("73420546", 0x1f);
            dictionary2.Add("74020414", 0x1f);
            dictionary2.Add("74120514", 0x1f);
            dictionary2.Add("74320307", 0x1f);
            dictionary2.Add("75020314", 0x1f);
            dictionary2.Add("76020214", 0x1f);
            dictionary2.Add("78020429", 0x1f);
            dictionary2.Add("79020325", 0x1f);
            dictionary2.Add("79320432", 0x1f);
            dictionary2.Add("79520533", 0x1f);
            dictionary2.Add("85020890", 0x1f);
            dictionary2.Add("86020880", 0x1f);
            dictionary2.Add("10020890", 0x20);
            dictionary2.Add("16020086", 0x20);
            dictionary2.Add("17020086", 0x20);
            dictionary2.Add("18020086", 0x20);
            dictionary2.Add("30220190", 0x20);
            dictionary2.Add("33020190", 0x20);
            dictionary2.Add("36020186", 0x20);
            dictionary2.Add("37020090", 0x20);
            dictionary2.Add("38020090", 0x20);
            dictionary2.Add("44020090", 0x20);
            dictionary2.Add("48020086", 0x20);
            dictionary2.Add("50320191", 0x20);
            dictionary2.Add("50520190", 0x20);
            dictionary2.Add("50820292", 0x20);
            dictionary2.Add("51020186", 0x20);
            dictionary2.Add("54020090", 0x20);
            dictionary2.Add("54220091", 0x20);
            dictionary2.Add("54520194", 0x20);
            dictionary2.Add("54620093", 0x20);
            dictionary2.Add("55020486", 0x20);
            dictionary2.Add("56020086", 0x20);
            dictionary2.Add("57020086", 0x20);
            dictionary2.Add("58520086", 0x20);
            dictionary2.Add("59020090", 0x20);
            dictionary2.Add("59320087", 0x20);
            dictionary2.Add("60320291", 0x20);
            dictionary2.Add("60420186", 0x20);
            dictionary2.Add("61120286", 0x20);
            dictionary2.Add("61420086", 0x20);
            dictionary2.Add("63020086", 0x20);
            dictionary2.Add("63220090", 0x20);
            dictionary2.Add("64020186", 0x20);
            dictionary2.Add("65020186", 0x20);
            dictionary2.Add("66020286", 0x20);
            dictionary2.Add("67020190", 0x20);
            dictionary2.Add("67220286", 0x20);
            dictionary2.Add("68020186", 0x20);
            dictionary2.Add("69020190", 0x20);
            dictionary2.Add("69220186", 0x20);
            dictionary2.Add("70021180", 0x20);
            dictionary2.Add("70025175", 0x20);
            dictionary2.Add("70320090", 0x20);
            dictionary2.Add("70321194", 0x20);
            dictionary2.Add("70322192", 0x20);
            dictionary2.Add("71020072", 0x20);
            dictionary2.Add("71021270", 0x20);
            dictionary2.Add("71022182", 0x20);
            dictionary2.Add("71023173", 0x20);
            dictionary2.Add("71120077", 0x20);
            dictionary2.Add("71120078", 0x20);
            dictionary2.Add("71121176", 0x20);
            dictionary2.Add("71122183", 0x20);
            dictionary2.Add("72020070", 0x20);
            dictionary2.Add("72021271", 0x20);
            dictionary2.Add("72021876", 0x20);
            dictionary2.Add("72120078", 0x20);
            dictionary2.Add("72122181", 0x20);
            dictionary2.Add("72220074", 0x20);
            dictionary2.Add("72223182", 0x20);
            dictionary2.Add("73120075", 0x20);
            dictionary2.Add("73320073", 0x20);
            dictionary2.Add("73321177", 0x20);
            dictionary2.Add("73322380", 0x20);
            dictionary2.Add("73420071", 0x20);
            dictionary2.Add("73421478", 0x20);
            dictionary2.Add("74020074", 0x20);
            dictionary2.Add("74120071", 0x20);
            dictionary2.Add("74220075", 0x20);
            dictionary2.Add("74221170", 0x20);
            dictionary2.Add("74320073", 0x20);
            dictionary2.Add("75020073", 0x20);
            dictionary2.Add("75021174", 0x20);
            dictionary2.Add("75220070", 0x20);
            dictionary2.Add("75320075", 0x20);
            dictionary2.Add("76020070", 0x20);
            dictionary2.Add("76220073", 0x20);
            dictionary2.Add("76320072", 0x20);
            dictionary2.Add("76420080", 0x20);
            dictionary2.Add("76520071", 0x20);
            dictionary2.Add("77020070", 0x20);
            dictionary2.Add("77120073", 0x20);
            dictionary2.Add("77320072", 0x20);
            dictionary2.Add("78020070", 0x20);
            dictionary2.Add("78320076", 0x20);
            dictionary2.Add("79320075", 0x20);
            dictionary2.Add("79520070", 0x20);
            dictionary2.Add("80020086", 0x20);
            dictionary2.Add("80020087", 0x20);
            dictionary2.Add("82020086", 0x20);
            dictionary2.Add("82020087", 0x20);
            dictionary2.Add("82020088", 0x20);
            dictionary2.Add("83020086", 0x20);
            dictionary2.Add("83020087", 0x20);
            dictionary2.Add("83020088", 0x20);
            dictionary2.Add("84020086", 0x20);
            dictionary2.Add("84020087", 0x20);
            dictionary2.Add("85020086", 0x20);
            dictionary2.Add("86020086", 0x20);
            dictionary2.Add("87020086", 0x20);
            dictionary2.Add("87020087", 0x20);
            dictionary2.Add("87020088", 0x20);
            dictionary2.Add("70020270", 0x21);
            dictionary2.Add("60020290", 0x22);
            dictionary2.Add("79020076", 0x23);
            dictionary2.Add("20050000", 0x24);
            dictionary2.Add("21050000", 0x24);
            dictionary2.Add("23050000", 0x24);
            dictionary2.Add("20110700", 0x25);
            dictionary2.Add("25090300", 0x26);
            dictionary2.Add("26691213", 0x26);
            dictionary2.Add("28591579", 0x26);
            dictionary2.Add("25621327", 0x27);
            dictionary2.Add("26520017", 0x27);
            dictionary2.Add("26521703", 0x27);
            dictionary2.Add("26522319", 0x27);
            dictionary2.Add("26620010", 0x27);
            dictionary2.Add("26621413", 0x27);
            dictionary2.Add("26720028", 0x27);
            dictionary2.Add("28021002", 0x27);
            dictionary2.Add("28021301", 0x27);
            dictionary2.Add("28021504", 0x27);
            dictionary2.Add("28021623", 0x27);
            dictionary2.Add("28021705", 0x27);
            dictionary2.Add("28021906", 0x27);
            dictionary2.Add("28022015", 0x27);
            dictionary2.Add("28022412", 0x27);
            dictionary2.Add("28022511", 0x27);
            dictionary2.Add("28022620", 0x27);
            dictionary2.Add("28022822", 0x27);
            dictionary2.Add("28023224", 0x27);
            dictionary2.Add("28023325", 0x27);
            dictionary2.Add("28220026", 0x27);
            dictionary2.Add("28222208", 0x27);
            dictionary2.Add("28222621", 0x27);
            dictionary2.Add("28320014", 0x27);
            dictionary2.Add("28321816", 0x27);
            dictionary2.Add("28420007", 0x27);
            dictionary2.Add("28421030", 0x27);
            dictionary2.Add("28520009", 0x27);
            dictionary2.Add("28521518", 0x27);
            dictionary2.Add("29121731", 0x27);
            dictionary2.Add("68051310", 40);
            dictionary2.Add("62220000", 0x29);
            dictionary2.Add("60651070", 0x2b);
            dictionary2.Add("68050101", 0x2c);
            dictionary2.Add("50210130", 0x2d);
            dictionary2.Add("50210131", 0x2d);
            dictionary2.Add("50210132", 0x2d);
            dictionary2.Add("50210133", 0x2d);
            dictionary2.Add("50210134", 0x2d);
            dictionary2.Add("50210135", 0x2d);
            dictionary2.Add("50210136", 0x2d);
            dictionary2.Add("50210137", 0x2d);
            dictionary2.Add("50210138", 0x2d);
            dictionary2.Add("50210139", 0x2d);
            dictionary2.Add("50210140", 0x2d);
            dictionary2.Add("50210141", 0x2d);
            dictionary2.Add("50210142", 0x2d);
            dictionary2.Add("50210143", 0x2d);
            dictionary2.Add("50210144", 0x2d);
            dictionary2.Add("50210145", 0x2d);
            dictionary2.Add("50210146", 0x2d);
            dictionary2.Add("50210147", 0x2d);
            dictionary2.Add("50210148", 0x2d);
            dictionary2.Add("50210149", 0x2d);
            dictionary2.Add("50210150", 0x2d);
            dictionary2.Add("50210151", 0x2d);
            dictionary2.Add("50210152", 0x2d);
            dictionary2.Add("50210153", 0x2d);
            dictionary2.Add("50210154", 0x2d);
            dictionary2.Add("50210155", 0x2d);
            dictionary2.Add("50210156", 0x2d);
            dictionary2.Add("50210157", 0x2d);
            dictionary2.Add("50210158", 0x2d);
            dictionary2.Add("50210159", 0x2d);
            dictionary2.Add("50210160", 0x2d);
            dictionary2.Add("50210161", 0x2d);
            dictionary2.Add("50210162", 0x2d);
            dictionary2.Add("50210163", 0x2d);
            dictionary2.Add("50210164", 0x2d);
            dictionary2.Add("50210165", 0x2d);
            dictionary2.Add("50210166", 0x2d);
            dictionary2.Add("50210167", 0x2d);
            dictionary2.Add("50210168", 0x2d);
            dictionary2.Add("50210169", 0x2d);
            dictionary2.Add("50210170", 0x2d);
            dictionary2.Add("50210171", 0x2d);
            dictionary2.Add("50210172", 0x2d);
            dictionary2.Add("50210173", 0x2d);
            dictionary2.Add("50210174", 0x2d);
            dictionary2.Add("50210175", 0x2d);
            dictionary2.Add("50210176", 0x2d);
            dictionary2.Add("50210177", 0x2d);
            dictionary2.Add("50210178", 0x2d);
            dictionary2.Add("50210179", 0x2d);
            dictionary2.Add("50210180", 0x2d);
            dictionary2.Add("50210181", 0x2d);
            dictionary2.Add("50210182", 0x2d);
            dictionary2.Add("50210183", 0x2d);
            dictionary2.Add("50210184", 0x2d);
            dictionary2.Add("50210185", 0x2d);
            dictionary2.Add("50210186", 0x2d);
            dictionary2.Add("50210187", 0x2d);
            dictionary2.Add("50210188", 0x2d);
            dictionary2.Add("50210189", 0x2d);
            dictionary2.Add("50510120", 0x2d);
            dictionary2.Add("50510121", 0x2d);
            dictionary2.Add("50510122", 0x2d);
            dictionary2.Add("50510123", 0x2d);
            dictionary2.Add("50510124", 0x2d);
            dictionary2.Add("50510125", 0x2d);
            dictionary2.Add("50510126", 0x2d);
            dictionary2.Add("50510127", 0x2d);
            dictionary2.Add("50510128", 0x2d);
            dictionary2.Add("50510129", 0x2d);
            dictionary2.Add("50510130", 0x2d);
            dictionary2.Add("50510131", 0x2d);
            dictionary2.Add("50510132", 0x2d);
            dictionary2.Add("50510133", 0x2d);
            dictionary2.Add("50510134", 0x2d);
            dictionary2.Add("50510135", 0x2d);
            dictionary2.Add("50510136", 0x2d);
            dictionary2.Add("50510137", 0x2d);
            dictionary2.Add("50510138", 0x2d);
            dictionary2.Add("50510139", 0x2d);
            dictionary2.Add("50510140", 0x2d);
            dictionary2.Add("50510141", 0x2d);
            dictionary2.Add("50510142", 0x2d);
            dictionary2.Add("50510143", 0x2d);
            dictionary2.Add("50510144", 0x2d);
            dictionary2.Add("50510145", 0x2d);
            dictionary2.Add("50510146", 0x2d);
            dictionary2.Add("50510147", 0x2d);
            dictionary2.Add("50510148", 0x2d);
            dictionary2.Add("50510149", 0x2d);
            dictionary2.Add("50510150", 0x2d);
            dictionary2.Add("50510151", 0x2d);
            dictionary2.Add("50510152", 0x2d);
            dictionary2.Add("50510153", 0x2d);
            dictionary2.Add("50510154", 0x2d);
            dictionary2.Add("50510155", 0x2d);
            dictionary2.Add("50510156", 0x2d);
            dictionary2.Add("50510157", 0x2d);
            dictionary2.Add("50510158", 0x2d);
            dictionary2.Add("50510159", 0x2d);
            dictionary2.Add("50510160", 0x2d);
            dictionary2.Add("50510161", 0x2d);
            dictionary2.Add("50510162", 0x2d);
            dictionary2.Add("50510163", 0x2d);
            dictionary2.Add("50510164", 0x2d);
            dictionary2.Add("50510165", 0x2d);
            dictionary2.Add("50510166", 0x2d);
            dictionary2.Add("50510167", 0x2d);
            dictionary2.Add("50510168", 0x2d);
            dictionary2.Add("50510169", 0x2d);
            dictionary2.Add("50510170", 0x2d);
            dictionary2.Add("50510171", 0x2d);
            dictionary2.Add("50510172", 0x2d);
            dictionary2.Add("50510173", 0x2d);
            dictionary2.Add("50510174", 0x2d);
            dictionary2.Add("50510175", 0x2d);
            dictionary2.Add("50510176", 0x2d);
            dictionary2.Add("50510177", 0x2d);
            dictionary2.Add("50510178", 0x2d);
            dictionary2.Add("50510179", 0x2d);
            dictionary2.Add("50510180", 0x2d);
            dictionary2.Add("10120600", 0x2e);
            dictionary2.Add("25020600", 0x2e);
            dictionary2.Add("10033300", 0x2f);
            dictionary2.Add("20133300", 0x2f);
            dictionary2.Add("36033300", 0x2f);
            dictionary2.Add("50033300", 0x2f);
            dictionary2.Add("55033300", 0x2f);
            dictionary2.Add("60133300", 0x2f);
            dictionary2.Add("70133300", 0x2f);
            dictionary2.Add("86033300", 0x2f);
            g_vBankCodeToRuleID = dictionary2;
            g_vPseudoAcctInfos = new SepaGermanPseudoAcctInfo[] { 
                new SepaGermanPseudoAcctInfo("10050000", "135", "0990021440"), new SepaGermanPseudoAcctInfo("10050000", "1111", "6600012020"), new SepaGermanPseudoAcctInfo("10050000", "1900", "0920019005"), new SepaGermanPseudoAcctInfo("10050000", "7878", "0780008006"), new SepaGermanPseudoAcctInfo("10050000", "8888", "0250030942"), new SepaGermanPseudoAcctInfo("10050000", "9595", "1653524703"), new SepaGermanPseudoAcctInfo("10050000", "97097", "0013044150"), new SepaGermanPseudoAcctInfo("10050000", "112233", "0630025819"), new SepaGermanPseudoAcctInfo("10050000", "336666", "6604058903"), new SepaGermanPseudoAcctInfo("10050000", "484848", "0920018963"), new SepaGermanPseudoAcctInfo("30040000", "0000000036", "0002611036"), new SepaGermanPseudoAcctInfo("47880031", "0000000050", "0519899900"), new SepaGermanPseudoAcctInfo("47840065", "0000000050", "0001501030"), new SepaGermanPseudoAcctInfo("47840065", "0000000055", "0001501030"), new SepaGermanPseudoAcctInfo("70080000", "0000000094", "0928553201"), new SepaGermanPseudoAcctInfo("70040041", "0000000094", "0002128080"), 
                new SepaGermanPseudoAcctInfo("47840065", "0000000099", "0001501030"), new SepaGermanPseudoAcctInfo("37080040", "0000000100", "0269100000"), new SepaGermanPseudoAcctInfo("38040007", "0000000100", "0001191600"), new SepaGermanPseudoAcctInfo("37080040", "0000000111", "0215022000"), new SepaGermanPseudoAcctInfo("51080060", "0000000123", "0012299300"), new SepaGermanPseudoAcctInfo("36040039", "0000000150", "0001616200"), new SepaGermanPseudoAcctInfo("68080030", "0000000202", "0416520200"), new SepaGermanPseudoAcctInfo("30040000", "0000000222", "0348010002"), new SepaGermanPseudoAcctInfo("38040007", "0000000240", "0001090240"), new SepaGermanPseudoAcctInfo("69240075", "0000000444", "0004455200"), new SepaGermanPseudoAcctInfo("60080000", "0000000502", "0901581400"), new SepaGermanPseudoAcctInfo("60040071", "0000000502", "0005259502"), new SepaGermanPseudoAcctInfo("55040022", "0000000555", "0002110500"), new SepaGermanPseudoAcctInfo("39080005", "0000000556", "0204655600"), new SepaGermanPseudoAcctInfo("39040013", "0000000556", "0001065556"), new SepaGermanPseudoAcctInfo("57080070", "0000000661", "0604101200"), 
                new SepaGermanPseudoAcctInfo("26580070", "0000000700", "0710000000"), new SepaGermanPseudoAcctInfo("50640015", "0000000777", "0002222222"), new SepaGermanPseudoAcctInfo("30040000", "0000000999", "0001237999"), new SepaGermanPseudoAcctInfo("86080000", "0000001212", "0480375900"), new SepaGermanPseudoAcctInfo("37040044", "0000001888", "0212129101"), new SepaGermanPseudoAcctInfo("25040066", "0000001919", "0001419191"), new SepaGermanPseudoAcctInfo("10080000", "0000001987", "0928127700"), new SepaGermanPseudoAcctInfo("50040000", "0000002000", "0007284003"), new SepaGermanPseudoAcctInfo("20080000", "0000002222", "0903927200"), new SepaGermanPseudoAcctInfo("38040007", "0000003366", "0003853330"), new SepaGermanPseudoAcctInfo("37080040", "0000004004", "0233533500"), new SepaGermanPseudoAcctInfo("37080040", "0000004444", "0233000300"), new SepaGermanPseudoAcctInfo("43080083", "0000004630", "0825110100"), new SepaGermanPseudoAcctInfo("50080000", "0000006060", "0096736100"), new SepaGermanPseudoAcctInfo("10040000", "0000007878", "0002678787"), new SepaGermanPseudoAcctInfo("10080000", "0000008888", "0928126501"), 
                new SepaGermanPseudoAcctInfo("50080000", "0000009000", "0026492100"), new SepaGermanPseudoAcctInfo("79080052", "0000009696", "0300021700"), new SepaGermanPseudoAcctInfo("79040047", "0000009696", "0006802102"), new SepaGermanPseudoAcctInfo("39080005", "0000009800", "0208457000"), new SepaGermanPseudoAcctInfo("50080000", "0000042195", "0900333200"), new SepaGermanPseudoAcctInfo("32040024", "0000047800", "0001555150"), new SepaGermanPseudoAcctInfo("37080040", "0000055555", "0263602501"), new SepaGermanPseudoAcctInfo("38040007", "0000055555", "0003055555"), new SepaGermanPseudoAcctInfo("50080000", "0000101010", "0090003500"), new SepaGermanPseudoAcctInfo("50040000", "0000101010", "0003110111"), new SepaGermanPseudoAcctInfo("37040044", "0000102030", "0002223444"), new SepaGermanPseudoAcctInfo("86080000", "0000121200", "0480375900"), new SepaGermanPseudoAcctInfo("66280053", "0000121212", "0625242400"), new SepaGermanPseudoAcctInfo("16080000", "0000123456", "0012345600"), new SepaGermanPseudoAcctInfo("29080010", "0000124124", "0107502000"), new SepaGermanPseudoAcctInfo("37080040", "0000182002", "0216603302"), 
                new SepaGermanPseudoAcctInfo("12080000", "0000212121", "4050462200"), new SepaGermanPseudoAcctInfo("37080040", "0000300000", "0983307900"), new SepaGermanPseudoAcctInfo("37040044", "0000300000", "0003000007"), new SepaGermanPseudoAcctInfo("37080040", "0000333333", "0270330000"), new SepaGermanPseudoAcctInfo("38040007", "0000336666", "0001052323"), new SepaGermanPseudoAcctInfo("55040022", "0000343434", "0002179000"), new SepaGermanPseudoAcctInfo("85080000", "0000400000", "0459488501"), new SepaGermanPseudoAcctInfo("37080040", "0000414141", "0041414100"), new SepaGermanPseudoAcctInfo("38040007", "0000414141", "0001080001"), new SepaGermanPseudoAcctInfo("20080000", "0000505050", "0500100600"), new SepaGermanPseudoAcctInfo("37080040", "0000555666", "0055566600"), new SepaGermanPseudoAcctInfo("20080000", "0000666666", "0900732500"), new SepaGermanPseudoAcctInfo("30080000", "0000700000", "0800005000"), new SepaGermanPseudoAcctInfo("70080000", "0000700000", "0750055500"), new SepaGermanPseudoAcctInfo("70080000", "0000900000", "0319966601"), new SepaGermanPseudoAcctInfo("37080040", "0000909090", "0269100000"), 
                new SepaGermanPseudoAcctInfo("38040007", "0000909090", "0001191600"), new SepaGermanPseudoAcctInfo("70080000", "0000949494", "0575757500"), new SepaGermanPseudoAcctInfo("70080000", "0001111111", "0448060000"), new SepaGermanPseudoAcctInfo("70040041", "0001111111", "0001521400"), new SepaGermanPseudoAcctInfo("10080000", "0001234567", "0920192001"), new SepaGermanPseudoAcctInfo("38040007", "0001555555", "0002582666"), new SepaGermanPseudoAcctInfo("76040061", "0002500000", "0004821468"), new SepaGermanPseudoAcctInfo("16080000", "0003030400", "4205227110"), new SepaGermanPseudoAcctInfo("37080040", "0005555500", "0263602501"), new SepaGermanPseudoAcctInfo("75040062", "0006008833", "0600883300"), new SepaGermanPseudoAcctInfo("12080000", "0007654321", "0144000700"), new SepaGermanPseudoAcctInfo("70080000", "0007777777", "0443540000"), new SepaGermanPseudoAcctInfo("70040041", "0007777777", "0002136000"), new SepaGermanPseudoAcctInfo("64140036", "0008907339", "0890733900"), new SepaGermanPseudoAcctInfo("70080000", "0009000000", "0319966601"), new SepaGermanPseudoAcctInfo("61080006", "0009999999", "0202427500"), 
                new SepaGermanPseudoAcctInfo("12080000", "0012121212", "4101725100"), new SepaGermanPseudoAcctInfo("29080010", "0012412400", "0107502000"), new SepaGermanPseudoAcctInfo("34280032", "0014111935", "0645753800"), new SepaGermanPseudoAcctInfo("38040007", "0043434343", "0001181635"), new SepaGermanPseudoAcctInfo("30080000", "0070000000", "0800005000"), new SepaGermanPseudoAcctInfo("70080000", "0070000000", "0750055500"), new SepaGermanPseudoAcctInfo("44040037", "0111111111", "0003205655"), new SepaGermanPseudoAcctInfo("70040041", "0400500500", "0004005005"), new SepaGermanPseudoAcctInfo("60080000", "0500500500", "0901581400"), new SepaGermanPseudoAcctInfo("60040071", "0500500500", "0005127006"), new SepaGermanPseudoAcctInfo("70150000", "1111111", "20228888"), new SepaGermanPseudoAcctInfo("70150000", "7777777", "903286003"), new SepaGermanPseudoAcctInfo("70150000", "34343434", "1000506517"), new SepaGermanPseudoAcctInfo("70150000", "70000", "18180018"), new SepaGermanPseudoAcctInfo("37050198", "111", "1115"), new SepaGermanPseudoAcctInfo("37050198", "221", "23002157"), 
                new SepaGermanPseudoAcctInfo("37050198", "1888", "18882068"), new SepaGermanPseudoAcctInfo("37050198", "2006", "1900668508"), new SepaGermanPseudoAcctInfo("37050198", "2626", "1900730100"), new SepaGermanPseudoAcctInfo("37050198", "3004", "1900637016"), new SepaGermanPseudoAcctInfo("37050198", "3636", "23002447"), new SepaGermanPseudoAcctInfo("37050198", "4000", "4028"), new SepaGermanPseudoAcctInfo("37050198", "4444", "17368"), new SepaGermanPseudoAcctInfo("37050198", "5050", "73999"), new SepaGermanPseudoAcctInfo("37050198", "8888", "1901335750"), new SepaGermanPseudoAcctInfo("37050198", "43430", "1901693331"), new SepaGermanPseudoAcctInfo("37050198", "46664", "1900399856"), new SepaGermanPseudoAcctInfo("37050198", "55555", "34407379"), new SepaGermanPseudoAcctInfo("37050198", "102030", "1900480466"), new SepaGermanPseudoAcctInfo("37050198", "151515", "57762957"), new SepaGermanPseudoAcctInfo("37050198", "222222", "2222222"), new SepaGermanPseudoAcctInfo("37050198", "300000", "9992959"), 
                new SepaGermanPseudoAcctInfo("37050198", "333333", "33217"), new SepaGermanPseudoAcctInfo("37050198", "414141", "92817"), new SepaGermanPseudoAcctInfo("37050198", "606060", "91025"), new SepaGermanPseudoAcctInfo("37050198", "909090", "90944"), new SepaGermanPseudoAcctInfo("37050198", "2602024", "5602024"), new SepaGermanPseudoAcctInfo("37050198", "3000000", "9992959"), new SepaGermanPseudoAcctInfo("37050198", "7777777", "2222222"), new SepaGermanPseudoAcctInfo("37050198", "8090100", "38901"), new SepaGermanPseudoAcctInfo("37050198", "14141414", "43597665"), new SepaGermanPseudoAcctInfo("37050198", "15000023", "15002223"), new SepaGermanPseudoAcctInfo("37050198", "15151515", "57762957"), new SepaGermanPseudoAcctInfo("37050198", "22222222", "2222222"), new SepaGermanPseudoAcctInfo("37050198", "200820082", "1901783868"), new SepaGermanPseudoAcctInfo("37050198", "222220022", "2222222"), new SepaGermanPseudoAcctInfo("50050201", "2000", "222000"), new SepaGermanPseudoAcctInfo("50050201", "800000", "180802"), 
                new SepaGermanPseudoAcctInfo("32050000", "1000", "8010001"), new SepaGermanPseudoAcctInfo("32050000", "47800", "47803"), new SepaGermanPseudoAcctInfo("37060193", "556", "0000101010"), new SepaGermanPseudoAcctInfo("37060193", "888", "0031870011"), new SepaGermanPseudoAcctInfo("37060193", "4040", "4003600101"), new SepaGermanPseudoAcctInfo("37060193", "5826", "1015826017"), new SepaGermanPseudoAcctInfo("37060193", "25000", "0025000110"), new SepaGermanPseudoAcctInfo("37060193", "393393", "0033013019"), new SepaGermanPseudoAcctInfo("37060193", "444555", "0032230016"), new SepaGermanPseudoAcctInfo("37060193", "603060", "6002919018"), new SepaGermanPseudoAcctInfo("37060193", "2120041", "0002130041"), new SepaGermanPseudoAcctInfo("37060193", "80868086", "4007375013"), new SepaGermanPseudoAcctInfo("37060193", "400569017", "4000569017"), new SepaGermanPseudoAcctInfo("37160087", "300000", "18128012"), new SepaGermanPseudoAcctInfo("38060186", "100", "2009090013"), new SepaGermanPseudoAcctInfo("38060186", "111", "2111111017"), 
                new SepaGermanPseudoAcctInfo("38060186", "240", "2100240010"), new SepaGermanPseudoAcctInfo("38060186", "4004", "2204004016"), new SepaGermanPseudoAcctInfo("38060186", "4444", "2044444014"), new SepaGermanPseudoAcctInfo("38060186", "6060", "2016060014"), new SepaGermanPseudoAcctInfo("38060186", "102030", "1102030016"), new SepaGermanPseudoAcctInfo("38060186", "333333", "2033333016"), new SepaGermanPseudoAcctInfo("38060186", "909090", "2009090013"), new SepaGermanPseudoAcctInfo("38060186", "50005000", "5000500013"), new SepaGermanPseudoAcctInfo("39060180", "556", "120440110"), new SepaGermanPseudoAcctInfo("39060180", "5435435430", "543543543"), new SepaGermanPseudoAcctInfo("39060180", "2157", "121787016"), new SepaGermanPseudoAcctInfo("39060180", "9800", "120800019"), new SepaGermanPseudoAcctInfo("39060180", "202050", "1221864014"), new SepaGermanPseudoAcctInfo("50070010", "9999", "92777202"), new SepaGermanPseudoAcctInfo("43060967", "1111111", "2222200000"), new SepaGermanPseudoAcctInfo("26590025", "700", "1000700800"), 
                new SepaGermanPseudoAcctInfo("36060295", "94", "1694"), new SepaGermanPseudoAcctInfo("36060295", "248", "17248"), new SepaGermanPseudoAcctInfo("36060295", "345", "17345"), new SepaGermanPseudoAcctInfo("36060295", "400", "14400"), new SepaGermanPseudoAcctInfo("70020270", "22222", "5803435253"), new SepaGermanPseudoAcctInfo("70020270", "1111111", "39908140"), new SepaGermanPseudoAcctInfo("70020270", "94", "2711931"), new SepaGermanPseudoAcctInfo("70020270", "7777777", "5800522694"), new SepaGermanPseudoAcctInfo("70020270", "55555", "5801800000"), new SepaGermanPseudoAcctInfo("60020290", "500500500", "4340111112"), new SepaGermanPseudoAcctInfo("60020290", "502", "4340118001"), new SepaGermanPseudoAcctInfo("79020076", "9696", "1490196966"), new SepaGermanPseudoAcctInfo("68050101", "202", "2282022")
             };
            Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
            dictionary3.Add("100", "76020070");
            dictionary3.Add("101", "10020890");
            dictionary3.Add("102", "78320076");
            dictionary3.Add("103", "79320075");
            dictionary3.Add("104", "76320072");
            dictionary3.Add("105", "79020076");
            dictionary3.Add("106", "79320075");
            dictionary3.Add("107", "79320075");
            dictionary3.Add("108", "77320072");
            dictionary3.Add("109", "79320075");
            dictionary3.Add("110", "76220073");
            dictionary3.Add("111", "76020070");
            dictionary3.Add("112", "79320075");
            dictionary3.Add("113", "76020070");
            dictionary3.Add("114", "76020070");
            dictionary3.Add("115", "76520071");
            dictionary3.Add("117", "77120073");
            dictionary3.Add("118", "76020070");
            dictionary3.Add("119", "75320075");
            dictionary3.Add("120", "72120078");
            dictionary3.Add("121", "76220073");
            dictionary3.Add("122", "76320072");
            dictionary3.Add("123", "76420080");
            dictionary3.Add("124", "76320072");
            dictionary3.Add("125", "79520070");
            dictionary3.Add("126", "77320072");
            dictionary3.Add("127", "78020070");
            dictionary3.Add("128", "78020070");
            dictionary3.Add("129", "77120073");
            dictionary3.Add("130", "78020070");
            dictionary3.Add("131", "78020070");
            dictionary3.Add("132", "60020290");
            dictionary3.Add("134", "78020070");
            dictionary3.Add("135", "77020070");
            dictionary3.Add("136", "79520070");
            dictionary3.Add("137", "79320075");
            dictionary3.Add("138", "61120286");
            dictionary3.Add("139", "66020286");
            dictionary3.Add("140", "79020076");
            dictionary3.Add("142", "64020186");
            dictionary3.Add("143", "60020290");
            dictionary3.Add("144", "79020076");
            dictionary3.Add("145", "66020286");
            dictionary3.Add("146", "72120078");
            dictionary3.Add("147", "72223182");
            dictionary3.Add("148", "76520071");
            dictionary3.Add("149", "79020076");
            dictionary3.Add("150", "76020070");
            dictionary3.Add("151", "76320072");
            dictionary3.Add("152", "78320076");
            dictionary3.Add("154", "70020270");
            dictionary3.Add("155", "76520071");
            dictionary3.Add("156", "76020070");
            dictionary3.Add("157", "10020890");
            dictionary3.Add("158", "70020270");
            dictionary3.Add("159", "54520194");
            dictionary3.Add("160", "70020270");
            dictionary3.Add("161", "54520194");
            dictionary3.Add("162", "70020270");
            dictionary3.Add("163", "70020270");
            dictionary3.Add("164", "70020270");
            dictionary3.Add("166", "71120078");
            dictionary3.Add("167", "74320073");
            dictionary3.Add("168", "70320090");
            dictionary3.Add("169", "79020076");
            dictionary3.Add("170", "70020270");
            dictionary3.Add("172", "70020270");
            dictionary3.Add("174", "70020270");
            dictionary3.Add("175", "72120078");
            dictionary3.Add("176", "74020074");
            dictionary3.Add("177", "74320073");
            dictionary3.Add("178", "70020270");
            dictionary3.Add("181", "77320072");
            dictionary3.Add("182", "79520070");
            dictionary3.Add("183", "70020270");
            dictionary3.Add("185", "70020270");
            dictionary3.Add("186", "79020076");
            dictionary3.Add("188", "70020270");
            dictionary3.Add("189", "70020270");
            dictionary3.Add("190", "76020070");
            dictionary3.Add("191", "77020070");
            dictionary3.Add("192", "70025175");
            dictionary3.Add("193", "85020086");
            dictionary3.Add("194", "76020070");
            dictionary3.Add("196", "72020070");
            dictionary3.Add("198", "76320072");
            dictionary3.Add("199", "70020270");
            dictionary3.Add("201", "76020070");
            dictionary3.Add("202", "76020070");
            dictionary3.Add("203", "76020070");
            dictionary3.Add("204", "76020070");
            dictionary3.Add("205", "79520070");
            dictionary3.Add("206", "79520070");
            dictionary3.Add("207", "71120078");
            dictionary3.Add("208", "73120075");
            dictionary3.Add("209", "18020086");
            dictionary3.Add("210", "10020890");
            dictionary3.Add("211", "60020290");
            dictionary3.Add("212", "51020186");
            dictionary3.Add("214", "75020073");
            dictionary3.Add("215", "63020086");
            dictionary3.Add("216", "75020073");
            dictionary3.Add("217", "79020076");
            dictionary3.Add("218", "59020090");
            dictionary3.Add("219", "79520070");
            dictionary3.Add("220", "73322380");
            dictionary3.Add("221", "73120075");
            dictionary3.Add("222", "73421478");
            dictionary3.Add("223", "74320073");
            dictionary3.Add("224", "73322380");
            dictionary3.Add("225", "74020074");
            dictionary3.Add("227", "75020073");
            dictionary3.Add("228", "71120078");
            dictionary3.Add("229", "80020086");
            dictionary3.Add("230", "72120078");
            dictionary3.Add("231", "72020070");
            dictionary3.Add("232", "75021174");
            dictionary3.Add("233", "71020072");
            dictionary3.Add("234", "71022182");
            dictionary3.Add("235", "74320073");
            dictionary3.Add("236", "71022182");
            dictionary3.Add("237", "76020070");
            dictionary3.Add("238", "63020086");
            dictionary3.Add("239", "70020270");
            dictionary3.Add("240", "75320075");
            dictionary3.Add("241", "76220073");
            dictionary3.Add("243", "72020070");
            dictionary3.Add("245", "72120078");
            dictionary3.Add("246", "74320073");
            dictionary3.Add("247", "60020290");
            dictionary3.Add("248", "85020086");
            dictionary3.Add("249", "73321177");
            dictionary3.Add("250", "73420071");
            dictionary3.Add("251", "70020270");
            dictionary3.Add("252", "70020270");
            dictionary3.Add("253", "70020270");
            dictionary3.Add("254", "10020890");
            dictionary3.Add("255", "50820292");
            dictionary3.Add("256", "71022182");
            dictionary3.Add("257", "83020086");
            dictionary3.Add("258", "79320075");
            dictionary3.Add("259", "71120077");
            dictionary3.Add("260", "10020890");
            dictionary3.Add("261", "70025175");
            dictionary3.Add("262", "72020070");
            dictionary3.Add("264", "74020074");
            dictionary3.Add("267", "63020086");
            dictionary3.Add("268", "70320090");
            dictionary3.Add("269", "71122183");
            dictionary3.Add("270", "82020086");
            dictionary3.Add("271", "75020073");
            dictionary3.Add("272", "73420071");
            dictionary3.Add("274", "63020086");
            dictionary3.Add("276", "70020270");
            dictionary3.Add("277", "74320073");
            dictionary3.Add("278", "71120077");
            dictionary3.Add("279", "10020890");
            dictionary3.Add("281", "71120078");
            dictionary3.Add("282", "70020270");
            dictionary3.Add("283", "72020070");
            dictionary3.Add("284", "79320075");
            dictionary3.Add("286", "54520194");
            dictionary3.Add("287", "70020270");
            dictionary3.Add("288", "75220070");
            dictionary3.Add("291", "77320072");
            dictionary3.Add("292", "76020070");
            dictionary3.Add("293", "72020070");
            dictionary3.Add("294", "54520194");
            dictionary3.Add("295", "70020270");
            dictionary3.Add("296", "70020270");
            dictionary3.Add("299", "72020070");
            dictionary3.Add("301", "85020086");
            dictionary3.Add("302", "54520194");
            dictionary3.Add("304", "70020270");
            dictionary3.Add("308", "70020270");
            dictionary3.Add("309", "54520194");
            dictionary3.Add("310", "72020070");
            dictionary3.Add("312", "74120071");
            dictionary3.Add("313", "76320072");
            dictionary3.Add("314", "70020270");
            dictionary3.Add("315", "70020270");
            dictionary3.Add("316", "70020270");
            dictionary3.Add("317", "70020270");
            dictionary3.Add("318", "70020270");
            dictionary3.Add("320", "71022182");
            dictionary3.Add("321", "75220070");
            dictionary3.Add("322", "79520070");
            dictionary3.Add("324", "70020270");
            dictionary3.Add("326", "85020086");
            dictionary3.Add("327", "72020070");
            dictionary3.Add("328", "72020070");
            dictionary3.Add("329", "70020270");
            dictionary3.Add("330", "76020070");
            dictionary3.Add("331", "70020270");
            dictionary3.Add("333", "70020270");
            dictionary3.Add("334", "75020073");
            dictionary3.Add("335", "70020270");
            dictionary3.Add("337", "80020086");
            dictionary3.Add("341", "10020890");
            dictionary3.Add("342", "10020890");
            dictionary3.Add("344", "70020270");
            dictionary3.Add("345", "77020070");
            dictionary3.Add("346", "76020070");
            dictionary3.Add("350", "79320075");
            dictionary3.Add("351", "79320075");
            dictionary3.Add("352", "70020270");
            dictionary3.Add("353", "70020270");
            dictionary3.Add("354", "72223182");
            dictionary3.Add("355", "72020070");
            dictionary3.Add("356", "70020270");
            dictionary3.Add("358", "54220091");
            dictionary3.Add("359", "76220073");
            dictionary3.Add("360", "80020087");
            dictionary3.Add("361", "70020270");
            dictionary3.Add("362", "70020270");
            dictionary3.Add("363", "70020270");
            dictionary3.Add("366", "72220074");
            dictionary3.Add("367", "70020270");
            dictionary3.Add("368", "10020890");
            dictionary3.Add("369", "76520071");
            dictionary3.Add("370", "85020086");
            dictionary3.Add("371", "70020270");
            dictionary3.Add("373", "70020270");
            dictionary3.Add("374", "73120075");
            dictionary3.Add("375", "70020270");
            dictionary3.Add("379", "70020270");
            dictionary3.Add("380", "70020270");
            dictionary3.Add("381", "70020270");
            dictionary3.Add("382", "79520070");
            dictionary3.Add("383", "72020070");
            dictionary3.Add("384", "72020070");
            dictionary3.Add("386", "70020270");
            dictionary3.Add("387", "70020270");
            dictionary3.Add("389", "70020270");
            dictionary3.Add("390", "67020190");
            dictionary3.Add("391", "70020270");
            dictionary3.Add("392", "70020270");
            dictionary3.Add("393", "54520194");
            dictionary3.Add("394", "70020270");
            dictionary3.Add("396", "70020270");
            dictionary3.Add("398", "66020286");
            dictionary3.Add("399", "87020088");
            dictionary3.Add("401", "30220190");
            dictionary3.Add("402", "36020186");
            dictionary3.Add("403", "38020090");
            dictionary3.Add("404", "30220190");
            dictionary3.Add("405", "68020186");
            dictionary3.Add("406", "48020086");
            dictionary3.Add("407", "37020090");
            dictionary3.Add("408", "68020186");
            dictionary3.Add("409", "10020890");
            dictionary3.Add("410", "66020286");
            dictionary3.Add("411", "60420186");
            dictionary3.Add("412", "57020086");
            dictionary3.Add("422", "70020270");
            dictionary3.Add("423", "70020270");
            dictionary3.Add("424", "76020070");
            dictionary3.Add("426", "70025175");
            dictionary3.Add("427", "50320191");
            dictionary3.Add("428", "70020270");
            dictionary3.Add("429", "85020086");
            dictionary3.Add("432", "70020270");
            dictionary3.Add("434", "60020290");
            dictionary3.Add("435", "76020070");
            dictionary3.Add("436", "76020070");
            dictionary3.Add("437", "70020270");
            dictionary3.Add("438", "70020270");
            dictionary3.Add("439", "70020270");
            dictionary3.Add("440", "70020270");
            dictionary3.Add("441", "70020270");
            dictionary3.Add("442", "85020086");
            dictionary3.Add("443", "55020486");
            dictionary3.Add("444", "50520190");
            dictionary3.Add("446", "80020086");
            dictionary3.Add("447", "70020270");
            dictionary3.Add("450", "30220190");
            dictionary3.Add("451", "44020090");
            dictionary3.Add("452", "70020270");
            dictionary3.Add("453", "70020270");
            dictionary3.Add("456", "10020890");
            dictionary3.Add("457", "87020086");
            dictionary3.Add("458", "54520194");
            dictionary3.Add("459", "61120286");
            dictionary3.Add("460", "70020270");
            dictionary3.Add("461", "70020270");
            dictionary3.Add("462", "70020270");
            dictionary3.Add("463", "70020270");
            dictionary3.Add("465", "70020270");
            dictionary3.Add("466", "10020890");
            dictionary3.Add("467", "10020890");
            dictionary3.Add("468", "70020270");
            dictionary3.Add("469", "60320291");
            dictionary3.Add("470", "65020186");
            dictionary3.Add("471", "84020087");
            dictionary3.Add("472", "76020070");
            dictionary3.Add("473", "74020074");
            dictionary3.Add("476", "78320076");
            dictionary3.Add("477", "78320076");
            dictionary3.Add("478", "87020088");
            dictionary3.Add("480", "70020270");
            dictionary3.Add("481", "70020270");
            dictionary3.Add("482", "84020087");
            dictionary3.Add("484", "70020270");
            dictionary3.Add("485", "50320191");
            dictionary3.Add("486", "70020270");
            dictionary3.Add("488", "67220286");
            dictionary3.Add("489", "10020890");
            dictionary3.Add("490", "10020890");
            dictionary3.Add("491", "16020086");
            dictionary3.Add("492", "10020890");
            dictionary3.Add("494", "79020076");
            dictionary3.Add("495", "87020088");
            dictionary3.Add("497", "87020087");
            dictionary3.Add("499", "79020076");
            dictionary3.Add("502", "10020890");
            dictionary3.Add("503", "10020890");
            dictionary3.Add("505", "70020270");
            dictionary3.Add("506", "10020890");
            dictionary3.Add("507", "87020086");
            dictionary3.Add("508", "86020086");
            dictionary3.Add("509", "83020087");
            dictionary3.Add("510", "80020086");
            dictionary3.Add("511", "83020086");
            dictionary3.Add("513", "85020086");
            dictionary3.Add("515", "17020086");
            dictionary3.Add("518", "82020086");
            dictionary3.Add("519", "83020086");
            dictionary3.Add("522", "10020890");
            dictionary3.Add("523", "70020270");
            dictionary3.Add("524", "85020086");
            dictionary3.Add("525", "70020270");
            dictionary3.Add("527", "82020088");
            dictionary3.Add("528", "10020890");
            dictionary3.Add("530", "10020890");
            dictionary3.Add("531", "10020890");
            dictionary3.Add("533", "50320191");
            dictionary3.Add("534", "70020270");
            dictionary3.Add("536", "85020086");
            dictionary3.Add("538", "82020086");
            dictionary3.Add("540", "65020186");
            dictionary3.Add("541", "80020087");
            dictionary3.Add("545", "18020086");
            dictionary3.Add("546", "10020890");
            dictionary3.Add("547", "10020890");
            dictionary3.Add("548", "10020890");
            dictionary3.Add("549", "82020087");
            dictionary3.Add("555", "79020076");
            dictionary3.Add("560", "79320075");
            dictionary3.Add("567", "86020086");
            dictionary3.Add("572", "10020890");
            dictionary3.Add("580", "70020270");
            dictionary3.Add("581", "70020270");
            dictionary3.Add("601", "74320073");
            dictionary3.Add("602", "70020270");
            dictionary3.Add("603", "70020270");
            dictionary3.Add("604", "70020270");
            dictionary3.Add("605", "70020270");
            dictionary3.Add("606", "70020270");
            dictionary3.Add("607", "74320073");
            dictionary3.Add("608", "72020070");
            dictionary3.Add("609", "72020070");
            dictionary3.Add("610", "72020070");
            dictionary3.Add("611", "72020070");
            dictionary3.Add("612", "71120077");
            dictionary3.Add("613", "70020270");
            dictionary3.Add("614", "72020070");
            dictionary3.Add("615", "70025175");
            dictionary3.Add("616", "73420071");
            dictionary3.Add("617", "68020186");
            dictionary3.Add("618", "73120075");
            dictionary3.Add("619", "60020290");
            dictionary3.Add("620", "71120077");
            dictionary3.Add("621", "71120077");
            dictionary3.Add("622", "74320073");
            dictionary3.Add("623", "72020070");
            dictionary3.Add("624", "71020072");
            dictionary3.Add("625", "71023173");
            dictionary3.Add("626", "71020072");
            dictionary3.Add("627", "71021270");
            dictionary3.Add("628", "71120077");
            dictionary3.Add("629", "73120075");
            dictionary3.Add("630", "71121176");
            dictionary3.Add("631", "71022182");
            dictionary3.Add("632", "70020270");
            dictionary3.Add("633", "74320073");
            dictionary3.Add("634", "70020270");
            dictionary3.Add("635", "70320090");
            dictionary3.Add("636", "70320090");
            dictionary3.Add("637", "72120078");
            dictionary3.Add("638", "72120078");
            dictionary3.Add("640", "70020270");
            dictionary3.Add("641", "70020270");
            dictionary3.Add("643", "74320073");
            dictionary3.Add("644", "70020270");
            dictionary3.Add("645", "70020270");
            dictionary3.Add("646", "70020270");
            dictionary3.Add("647", "70020270");
            dictionary3.Add("648", "72120078");
            dictionary3.Add("649", "72122181");
            dictionary3.Add("650", "54520194");
            dictionary3.Add("652", "71021270");
            dictionary3.Add("653", "70020270");
            dictionary3.Add("654", "70020270");
            dictionary3.Add("655", "72120078");
            dictionary3.Add("656", "71120078");
            dictionary3.Add("657", "71020072");
            dictionary3.Add("658", "68020186");
            dictionary3.Add("659", "54520194");
            dictionary3.Add("660", "54620093");
            dictionary3.Add("661", "74320073");
            dictionary3.Add("662", "73120075");
            dictionary3.Add("663", "70322192");
            dictionary3.Add("664", "72120078");
            dictionary3.Add("665", "70321194");
            dictionary3.Add("666", "73322380");
            dictionary3.Add("667", "60020290");
            dictionary3.Add("668", "60020290");
            dictionary3.Add("669", "73320073");
            dictionary3.Add("670", "75020073");
            dictionary3.Add("671", "74220075");
            dictionary3.Add("672", "74020074");
            dictionary3.Add("673", "74020074");
            dictionary3.Add("674", "74120071");
            dictionary3.Add("675", "74020074");
            dictionary3.Add("676", "74020074");
            dictionary3.Add("677", "72020070");
            dictionary3.Add("678", "72020070");
            dictionary3.Add("679", "54520194");
            dictionary3.Add("680", "71120077");
            dictionary3.Add("681", "67020190");
            dictionary3.Add("682", "78020070");
            dictionary3.Add("683", "71020072");
            dictionary3.Add("684", "70020270");
            dictionary3.Add("685", "70020270");
            dictionary3.Add("686", "70020270");
            dictionary3.Add("687", "70020270");
            dictionary3.Add("688", "70020270");
            dictionary3.Add("689", "70020270");
            dictionary3.Add("690", "76520071");
            dictionary3.Add("692", "70020270");
            dictionary3.Add("693", "73420071");
            dictionary3.Add("694", "70021180");
            dictionary3.Add("695", "70320090");
            dictionary3.Add("696", "74320073");
            dictionary3.Add("697", "54020090");
            dictionary3.Add("698", "73320073");
            dictionary3.Add("710", "30220190");
            dictionary3.Add("711", "70020270");
            dictionary3.Add("712", "10020890");
            dictionary3.Add("714", "76020070");
            dictionary3.Add("715", "75020073");
            dictionary3.Add("717", "74320073");
            dictionary3.Add("718", "87020086");
            dictionary3.Add("719", "37020090");
            dictionary3.Add("720", "30220190");
            dictionary3.Add("723", "77320072");
            dictionary3.Add("733", "83020087");
            dictionary3.Add("798", "70020270");
            g_vExHypoDictionary = dictionary3;
        }
        
        private SepaGermanBundesbankInfo _FindBundesbankInfo(string sBankCode)
        {
            SepaGermanBundesbankInfo item = new SepaGermanBundesbankInfo(sBankCode);
            int num = this.m_vBundesbankTable.BinarySearch(item);
            if (num < 0)
            {
                return null;
            }
            return this.m_vBundesbankTable[num];
        }
        
        private static string _MakeIBAN(string sBankCode, string sAcctNo)
        {
            SepaIBAN aiban = new SepaIBAN("DE", sBankCode, sAcctNo.PadLeft(10, '0'));
            return aiban.IBAN;
        }
        
        public static bool CheckAcctNo06(string sAcctNo)
        {
            int num = (((((((((sAcctNo[8] - '0') * 2) + ((sAcctNo[7] - '0') * 3)) + ((sAcctNo[6] - '0') * 4)) + ((sAcctNo[5] - '0') * 5)) + ((sAcctNo[4] - '0') * 6)) + ((sAcctNo[3] - '0') * 7)) + ((sAcctNo[2] - '0') * 2)) + ((sAcctNo[1] - '0') * 3)) + ((sAcctNo[0] - '0') * 4);
            num = 11 - (num % 11);
            num = num % 10;
            return ((sAcctNo[9] - '0') == num);
        }
        
        public static bool CheckAcctNo13(string sAcctNo)
        {
            int[] numArray = new int[] { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 
                7, 8, 9
             };
            int num = (((((sAcctNo[1] - '0') + numArray[(sAcctNo[2] - '0') * 2]) + (sAcctNo[3] - '0')) + numArray[(sAcctNo[4] - '0') * 2]) + (sAcctNo[5] - '0')) + numArray[(sAcctNo[6] - '0') * 2];
            num = 10 - (num % 10);
            if (num == 10)
            {
                num = 0;
            }
            return ((sAcctNo[7] - '0') == num);
        }
        
        public static bool CheckAcctNo63(string sAcctNo)
        {
            int[] numArray = new int[] { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 
                7, 8, 9
             };
            if (sAcctNo[0] != '0')
            {
                return false;
            }
            int num = (((((sAcctNo[1] - '0') + numArray[(sAcctNo[2] - '0') * 2]) + (sAcctNo[3] - '0')) + numArray[(sAcctNo[4] - '0') * 2]) + (sAcctNo[5] - '0')) + numArray[(sAcctNo[6] - '0') * 2];
            num = 10 - (num % 10);
            if (num == 10)
            {
                num = 0;
            }
            return ((sAcctNo[7] - '0') == num);
        }
        
        public static bool CheckAcctNo76(string sAcctNo)
        {
            int num = ((((((sAcctNo[1] - '0') * 7) + ((sAcctNo[2] - '0') * 6)) + ((sAcctNo[3] - '0') * 5)) + ((sAcctNo[4] - '0') * 4)) + ((sAcctNo[5] - '0') * 3)) + ((sAcctNo[6] - '0') * 2);
            num = num % 11;
            return ((sAcctNo[7] - '0') == num);
        }
        
        public static bool CheckAcctNoC7(string sAcctNo)
        {
            if (!CheckAcctNo63(sAcctNo))
            {
                return CheckAcctNo06(sAcctNo);
            }
            return true;
        }
        
        public string ConvertPseudoAccts(string sBankCode, string sAcctNo)
        {
            if ((sBankCode == null) || (sAcctNo == null))
            {
                throw new ArgumentNullException();
            }
            if ((sBankCode.Length != 8) || !SepaUtil.IsNumeric(sBankCode))
            {
                throw new ArgumentException();
            }
            string str = sAcctNo.TrimStart(new char[] { '0' });
            for (int i = 0; i < g_vPseudoAcctInfos.Length; i++)
            {
                SepaGermanPseudoAcctInfo info = g_vPseudoAcctInfos[i];
                if ((sBankCode == info.BankCode) && (str == info.PseudoAcctNo))
                {
                    return info.RealAcctNo;
                }
            }
            return sAcctNo;
        }
        
        public string DetermineBankCode(string sBankCode)
        {
            if (g_vBankCodeMap.ContainsKey(sBankCode))
            {
                sBankCode = g_vBankCodeMap[sBankCode];
            }
            if (this.m_vBundesbankTable != null)
            {
                SepaGermanBundesbankInfo info = this._FindBundesbankInfo(sBankCode);
                if (info == null)
                {
                    return null;
                }
                if (info.NewBankCode != null)
                {
                    sBankCode = info.NewBankCode;
                }
            }
            return sBankCode;
        }
        
        public string DetermineBIC(string sBankCode)
        {
            if (this.m_vBundesbankTable == null)
            {
                return null;
            }
            SepaGermanBundesbankInfo info = this._FindBundesbankInfo(sBankCode);
            if (info == null)
            {
                return null;
            }
            return info.BIC;
        }
        
        public SepaGermanIBANResult DetermineIBAN(string sBankCode, string sAcctNo, int nRuleID, out string sIBAN, out string sBIC)
        {
            if ((sBankCode == null) || (sAcctNo == null))
            {
                throw new ArgumentNullException();
            }
            if ((sBankCode.Length != 8) || !SepaUtil.IsNumeric(sBankCode))
            {
                throw new ArgumentException();
            }
            if (((sAcctNo.Length < 2) || (sAcctNo.Length > 10)) || !SepaUtil.IsNumeric(sAcctNo))
            {
                throw new ArgumentException();
            }
            if ((nRuleID < 0) || (nRuleID > 0x270f))
            {
                throw new ArgumentException();
            }
            SepaGermanIBANResult oK = SepaGermanIBANResult.OK;
            sIBAN = null;
            sBIC = null;
            sBankCode = this.DetermineBankCode(sBankCode);
            if (sBankCode == null)
            {
                return SepaGermanIBANResult.UnknownBankCode;
            }
            sBIC = this.DetermineBIC(sBankCode);
            sAcctNo = this.ConvertPseudoAccts(sBankCode, sAcctNo);
            sAcctNo = sAcctNo.PadLeft(10, '0');
            long num = long.Parse(sAcctNo);
            switch (nRuleID)
            {
                case 0:
                case 4:
                case 6:
                case 7:
                case 10:
                case 11:
                case 15:
                case 0x10:
                case 0x11:
                case 0x12:
                case 0x16:
                case 0x17:
                case 0x18:
                case 0x1a:
                case 0x1b:
                case 30:
                case 0x2c:
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 1:
                    return SepaGermanIBANResult.Invalid;
                
                case 2:
                    if (!(sAcctNo.Substring(7, 2) == "86") && !(sAcctNo.Substring(7, 1) == "6"))
                    {
                        sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                        return oK;
                    }
                    return SepaGermanIBANResult.Invalid;
                
                case 3:
                    if (!(sAcctNo == "6161604670"))
                    {
                        sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                        return oK;
                    }
                    return SepaGermanIBANResult.Invalid;
                
                case 5:
                    if (!"10045050:50040033:70045050:10040085:35040085:36040085:44040085:50040085:67040085:82040085".Contains(sBankCode))
                    {
                        if (("10080900:12080000:13080000:14080000:15080000:16080000:17080000:18080000:20080055:20080057:21080050:21280002:21480003:21580000:22180000:22181400:22280000:24080000:24180001:25480021:25780022:25980027:26080024:26281420:26580070:26880063:26981062:28280012:29280011:30080055:30080057:31080015:32080010:33080030:34080031:34280032:36280071:36580072:40080040:41280043:42080082:42680081:43080083:44080055:44080057:44580070:45080060:46080010:47880031:49080025:50080055:50080057:50080081:50080082:50680002:50780006:50880050:51080000:51380040:52080080:53080030:54080021:54280023:54580020:54680022:55080065:57080070:58580074:59080090:60080055:60080057:60380002:60480008:61080006:61281007:61480001:62080012:62280012:63080015:64080014:64380011:65080009:65180005:65380003:66280053:66680013:67280051:69280035:70080056:70080057:70380006:71180005:72180002:73180011:73380004:73480013:74180009:74380007:75080003:76080053:79080052:79380051:79580099:80080000:81080000:82080000:83080000:84080000:85080200:86080055:86080057:87080000".Contains(sBankCode) && (num >= 0x3b7c4580L)) && (num <= 0x3b9328dfL))
                        {
                            return SepaGermanIBANResult.Invalid;
                        }
                        char ch = sBankCode[3];
                        if (ch == '4')
                        {
                            if (sBankCode.Substring(3, 3) != "411")
                            {
                                sBIC = "COBADEFFXXX";
                            }
                            if (!"10040010:10040060:10040061:10040062:10040063:20040020:20040060:20040061:20040062:20040063:25040060:25040061:29040060:29040061:30040060:30040061:30040062:30040063:31040060:31040061:36040060:36040061:37040037:37040060:37040061:44040060:44040061:47840080:48040060:48040061:50040033:50040050:50040060:50040061:50040062:50040063:55040060:55040061:60040060:60040061:67040060:67040061:69440060:70040060:70040061:70040062:70040063:70040070:76040060:76040062:85040060:85040061:86040060:86040061:10080085:10080086:10080087:10080089:10089260:10089999:20080085:20080086:20080087:20080088:20080089:20080091:20080092:20080093:20080094:20080095:20089200:21089201:23089201:25080085:25089220:26589210:26989221:27089221:29089210:30080080:30080081:30080082:30080083:30080084:30080085:30080086:30080087:30080088:30080089:30089300:30089302:33080001:33080085:33080086:33080087:33080088:35080085:35080086:35080087:35080088:35080089:36080085:36089321:37080085:37080086:37080087:37080088:37080089:37080090:37080091:37080092:37080093:37080094:37080095:37080098:37089340:37089342:40080085:44080085:44089320:44580085:48089350:50080077:50080086:50080087:50080088:50080089:50080091:50089400:50580085:50680085:50880085:50880086:51080085:51080086:51089410:51380085:52080085:55080085:55080086:60080085:60080086:60080087:60080088:60089450:63080085:67080085:67080086:67089440:68080085:68080086:70080085:70080086:70080087:70080088:70089470:70089472:76080055:76080085:76080086:76089480:76089482:79080085:79589402:85080085:85080086:85089270:86080085:86080086:86089280".Contains(sBankCode))
                            {
                                bool flag = CheckAcctNo13(sAcctNo);
                                string str = null;
                                bool flag2 = false;
                                if (sAcctNo.StartsWith("000"))
                                {
                                    str = sAcctNo.Substring(2) + "00";
                                    flag2 = CheckAcctNo13(str);
                                }
                                if (flag)
                                {
                                    if (flag2)
                                    {
                                        sAcctNo = str;
                                        oK = SepaGermanIBANResult.BestGuess;
                                    }
                                }
                                else if (flag2)
                                {
                                    sAcctNo = str;
                                }
                                else
                                {
                                    return SepaGermanIBANResult.Invalid;
                                }
                            }
                        }
                        if ((ch == '8') && !"10040010:10040060:10040061:10040062:10040063:20040020:20040060:20040061:20040062:20040063:25040060:25040061:29040060:29040061:30040060:30040061:30040062:30040063:31040060:31040061:36040060:36040061:37040037:37040060:37040061:44040060:44040061:47840080:48040060:48040061:50040033:50040050:50040060:50040061:50040062:50040063:55040060:55040061:60040060:60040061:67040060:67040061:69440060:70040060:70040061:70040062:70040063:70040070:76040060:76040062:85040060:85040061:86040060:86040061:10080085:10080086:10080087:10080089:10089260:10089999:20080085:20080086:20080087:20080088:20080089:20080091:20080092:20080093:20080094:20080095:20089200:21089201:23089201:25080085:25089220:26589210:26989221:27089221:29089210:30080080:30080081:30080082:30080083:30080084:30080085:30080086:30080087:30080088:30080089:30089300:30089302:33080001:33080085:33080086:33080087:33080088:35080085:35080086:35080087:35080088:35080089:36080085:36089321:37080085:37080086:37080087:37080088:37080089:37080090:37080091:37080092:37080093:37080094:37080095:37080098:37089340:37089342:40080085:44080085:44089320:44580085:48089350:50080077:50080086:50080087:50080088:50080089:50080091:50089400:50580085:50680085:50880085:50880086:51080085:51080086:51089410:51380085:52080085:55080085:55080086:60080085:60080086:60080087:60080088:60089450:63080085:67080085:67080086:67089440:68080085:68080086:70080085:70080086:70080087:70080088:70089470:70089472:76080055:76080085:76080086:76089480:76089482:79080085:79589402:85080085:85080086:85089270:86080085:86080086:86089280".Contains(sBankCode))
                        {
                            bool flag3 = CheckAcctNo76(sAcctNo);
                            string str2 = null;
                            bool flag4 = false;
                            if (sAcctNo.StartsWith("00"))
                            {
                                str2 = sAcctNo.Substring(2) + "00";
                                flag4 = CheckAcctNo76(str2);
                            }
                            if (!flag3)
                            {
                                if (!flag4)
                                {
                                    return SepaGermanIBANResult.Invalid;
                                }
                                sAcctNo = str2;
                            }
                            else if (flag4)
                            {
                                oK = SepaGermanIBANResult.BestGuess;
                            }
                        }
                        sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                        return oK;
                    }
                    return SepaGermanIBANResult.Invalid;
                
                case 8:
                    sBankCode = "50020200";
                    sBIC = "BHFBDEFF500";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 9:
                    if ((sBankCode == "68351976") && sAcctNo.StartsWith("1116"))
                    {
                        sAcctNo = "3047" + sAcctNo.Substring(4);
                    }
                    sBankCode = "68351557";
                    sBIC = "SOLADES1SFH";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 12:
                    sBankCode = "50050000";
                    sBIC = this.DetermineBIC(sBankCode);
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 13:
                    sBankCode = "30050000";
                    sBIC = this.DetermineBIC(sBankCode);
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 14:
                    sBankCode = "30060601";
                    sBIC = "DAAEDEDDXXX";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x13:
                    sBankCode = "50120383";
                    sBIC = "DELBDE33XXX";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 20:
                    if (!(sBankCode == "10020000"))
                    {
                        bool flag5 = CheckAcctNo63(sAcctNo);
                        string str3 = null;
                        bool flag6 = false;
                        if (sAcctNo.StartsWith("000"))
                        {
                            str3 = sAcctNo.Substring(2) + "00";
                            flag6 = CheckAcctNo63(str3);
                        }
                        else if ((sAcctNo[0] != '0') && sAcctNo.EndsWith("000"))
                        {
                            str3 = "0" + sAcctNo.Substring(0, 9);
                            flag6 = CheckAcctNo63(str3);
                        }
                        if (flag5)
                        {
                            if (flag6)
                            {
                                oK = SepaGermanIBANResult.BestGuess;
                            }
                        }
                        else if (flag6)
                        {
                            sAcctNo = str3;
                        }
                        else
                        {
                            if (sBankCode == "76026000")
                            {
                                CheckAcctNo06(sAcctNo);
                            }
                            return SepaGermanIBANResult.Invalid;
                        }
                        sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                        return oK;
                    }
                    return SepaGermanIBANResult.Invalid;
                
                case 0x15:
                    sBankCode = "36020030";
                    sBIC = "NBAGDE3EXXX";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x19:
                    sBankCode = "60050101";
                    sBIC = this.DetermineBIC(sBankCode);
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x1c:
                    sBankCode = "25050180";
                    sBIC = "SPKHDE2HXXX";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x1d:
                    if ((sAcctNo[0] != '0') && (sAcctNo[3] == '0'))
                    {
                        sAcctNo = "0" + sAcctNo.Remove(3, 1);
                    }
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x1f:
                case 0x20:
                case 0x21:
                case 0x22:
                case 0x23:
                    if (sAcctNo[0] != '0')
                    {
                        string key = sAcctNo.Substring(0, 3);
                        if (g_vExHypoDictionary.ContainsKey(key))
                        {
                            sBankCode = g_vExHypoDictionary[key];
                            sBIC = this.DetermineBIC(sBankCode);
                        }
                    }
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x24:
                    if (((((num > 0x1869fL) && ((num < 0xdbba0L) || (num > 0x1c9c37fL))) && ((num < 0x3938700L) || (num > 0x5f5e0ffL))) && (((num < 0x35a4e900L) || (num > 0x3b9ac9ffL)) && ((num < 0x77359400L) || (num > 0xb2d05dffL)))) && (((num < 0x1a7316700L) || (num > 0x1faa3b4ffL)) && ((num < 0x200999600L) || (num > 0x2187119ffL))))
                    {
                        if (sAcctNo.StartsWith("0000"))
                        {
                            sAcctNo = sAcctNo.Substring(3) + "000";
                        }
                        sBankCode = "21050000";
                        sBIC = this.DetermineBIC(sBankCode);
                        sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                        return oK;
                    }
                    return SepaGermanIBANResult.Invalid;
                
                case 0x25:
                    sBankCode = "30010700";
                    sBIC = "BOTKDEDXXXX";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x26:
                    sBankCode = "28590075";
                    sBIC = "GENODEF1LER";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x27:
                    sBankCode = "28020050";
                    sBIC = this.DetermineBIC(sBankCode);
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 40:
                    sBankCode = "68052328";
                    sBIC = "SOLADES1STF";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x29:
                    sBIC = "GENODEFFXXX";
                    sIBAN = "DE96500604000000011404";
                    return oK;
                
                case 0x2a:
                    if (sAcctNo.StartsWith("00"))
                    {
                        if (((num >= 0x301fd30L) && (num <= 0x30204ffL)) || ((num >= 0x3021888L) && (num <= 0x3021c6fL)))
                        {
                            sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                            return oK;
                        }
                        if ((sAcctNo[5] != '0') || (sAcctNo[6] == '0'))
                        {
                            return SepaGermanIBANResult.Invalid;
                        }
                        sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                        return oK;
                    }
                    return SepaGermanIBANResult.Invalid;
                
                case 0x2b:
                    sBankCode = "66650085";
                    sBIC = "PZHSDE66XXX";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x2d:
                    sBIC = "ESSEDE5FXXX";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
                
                case 0x2e:
                    sBankCode = "31010833";
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    sBIC = this.DetermineBIC(sBankCode);
                    return oK;
                
                case 0x2f:
                    if ((num >= 0x989680L) && (num <= 0x5f5e0ffL))
                    {
                        sAcctNo = sAcctNo.Substring(2) + "00";
                    }
                    sIBAN = _MakeIBAN(sBankCode, sAcctNo);
                    return oK;
            }
            sIBAN = _MakeIBAN(sBankCode, sAcctNo);
            return SepaGermanIBANResult.BestGuess;
        }
        
        public int DetermineRuleID(string sBankCode)
        {
            if (sBankCode == null)
            {
                throw new ArgumentNullException();
            }
            if ((sBankCode.Length != 8) || !SepaUtil.IsNumeric(sBankCode))
            {
                throw new ArgumentException();
            }
            int ruleID = -1;
            if (this.m_vBundesbankTable != null)
            {
                SepaGermanBundesbankInfo info = this._FindBundesbankInfo(sBankCode);
                if (info == null)
                {
                    return -1;
                }
                ruleID = info.RuleID;
            }
            if (ruleID >= 0)
            {
                return ruleID;
            }
            if (g_vBankCodeToRuleID.ContainsKey(sBankCode))
            {
                return g_vBankCodeToRuleID[sBankCode];
            }
            if ((sBankCode[3] == '4') || (sBankCode[3] == '8'))
            {
                return 5;
            }
            if (sBankCode[3] == '7')
            {
                return 20;
            }
            if (sBankCode.EndsWith("00000"))
            {
                return 0x2a;
            }
            return 0;
        }
        
        public static string GetAcctNo(string sIBAN)
        {
            string str = null;
            if ((SepaIBAN.IsValid(sIBAN) && (SepaIBAN.GetCountryCode(sIBAN) == "DE")) && (sIBAN.Length > 12))
            {
                str = sIBAN.Substring(12);
            }
            return str;
        }
        
        public static string GetBankCode(string sIBAN)
        {
            string str = null;
            if ((SepaIBAN.IsValid(sIBAN) && (SepaIBAN.GetCountryCode(sIBAN) == "DE")) && (sIBAN.Length > 12))
            {
                str = sIBAN.Substring(4, 8);
            }
            return str;
        }
        
        public void LoadBundesbankTableFile(string sFileName)
        {
            using (StreamReader reader = new StreamReader(sFileName))
            {
                string str;
                this.m_vBundesbankTable = new List<SepaGermanBundesbankInfo>();
            Label_0012:
                str = reader.ReadLine();
                if (str != null)
                {
                    if (str[8] == '1')
                    {
                        SepaGermanBundesbankInfo item = new SepaGermanBundesbankInfo(str.Substring(0, 8));
                        string str3 = str.Substring(0x8b, 11).Trim();
                        if (str3 != "")
                        {
                            item.BIC = str3;
                        }
                        string str4 = str.Substring(160, 8);
                        if (str4 != "00000000")
                        {
                            item.NewBankCode = str4;
                        }
                        if (str.Length >= 0xae)
                        {
                            item.RuleID = int.Parse(str.Substring(0xa8, 4));
                        }
                        else
                        {
                            item.RuleID = -1;
                        }
                        this.m_vBundesbankTable.Add(item);
                    }
                    goto Label_0012;
                }
                reader.Close();
            }
        }
    }
}
