using DeviceId;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Achievers
{
    public class Common
    {
        private static Dictionary<string, string> lstSpecialWord = null;
        public static string DeviceIdTest = "U2FuZ0hhbmRzb21l";
        public static string LicenseFile = Environment.CurrentDirectory + "/d5bdb0697ef84e78ba0eb0cce2143572.txt";
        public static int AppId = 918;
        private const string SecretKey = "qTsT63XJNtEKk0zwp7Q8aG0D0/kjs57WfE+SsEpC/6/p5nLNDeNYIhleGZz/m7ENlxtiXgCoh1Pl9kLaTKUH0A==";

        public static string Decrypt(string encrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(encrypt);

            MD5CryptoServiceProvider hashMd5 = new MD5CryptoServiceProvider();
            keyArray = hashMd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecretKey));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private static string DeviceId;

        public static string GetDeviceId()
        {
            if (DeviceId == null)
            {
                DeviceId = new DeviceIdBuilder().AddProcessorId().AddMotherboardSerialNumber().AddSystemDriveSerialNumber().ToString();
            }
            return DeviceId;
        }

        public static string ReplaceSpecializedWords(string key)
        {
            if (lstSpecialWord == null)
            {
                List<TempClass> items = JsonConvert.DeserializeObject<List<TempClass>>(@"[{ sub: 'anatomy', display: 'anatomy ' },{ sub: 'anatomy phẫu', display: 'anatomy##phẫu thuật ' },{ sub: 'architecture', display: 'architecture ' },{ sub: 'architecture kiến', display: '	architecture##kiến trúc ' },{ sub: 'architecture ktrúc', display: 'architecture##kiến trúc ' },{ sub: 'art nghệ', display: 'art##nghệ thuật ' },{ sub: 'astronomy thiên', display: 'astronomy##thiên văn học ' },{ sub: 'biology', display: 'biology ' },{ sub: 'biology sinh', display: 'biology##sinh học ' },{ sub: 'business', display: 'business ' },{ sub: 'business kinh', display: 'business##kinh doanh ' },{ sub: 'business tin', display: 'business##kinh doanh ' },{ sub: 'chemistry', display: 'chemistry ' },{ sub: 'chemistry hoá', display: 'chemistry##hóa học ' },{ sub: 'computing', display: 'computing ' },{ sub: 'computing tin', display: 'computing##máy tính ' },{ sub: 'economics', display: 'economics ' },{ sub: 'economics kinh', display: 'economics##kinh tế ' },{ sub: 'finance tài', display: 'finance##tài chính ' },{ sub: 'geology địa', display: 'geology##địa chất ' },{ sub: 'geometry địa', display: 'geometry##địa hình ' },{ sub: 'geometry hình', display: 'geometry##địa hình ' },{ sub: 'grammar', display: 'grammar ' },{ sub: 'grammar ngữ', display: 'grammar##ngữ pháp ' },{ sub: 'law', display: 'law ' },{ sub: 'law luật', display: 'law##luật ' },{ sub: 'linguistics', display: 'linguistics ' },{ sub: 'linguistics ngôn', display: 'linguistics##ngôn ngữ học ' },{ sub: 'mathematics', display: 'mathematics ' },{ sub: 'mathematics toán', display: 'mathematics##toán học ' },{ sub: 'medical', display: 'medical ' },{ sub: 'medical y', display: 'medical##y học ' },{ sub: 'music', display: 'music ' },{ sub: 'music nhạc', display: 'music ##âm nhạc ' },{ sub: 'philosophy', display: 'philosophy ' },{ sub: 'philosophy triết', display: 'philosophy##triết học ' },{ sub: 'phonetics', display: 'phonetics ' },{ sub: 'phonetics ngữ', display: 'phonetics##ngữ âm học ' },{ sub: 'physics', display: 'physics ' },{ sub: 'physics lý', display: 'physics##vật lí học ' },{ sub: 'politics chính', display: 'politics##chính trị ' },{ sub: 'psychology tâm', display: 'psychology##tâm lý học ' },{ sub: 'religion', display: 'religion ' },{ sub: 'religion tôn', display: 'religion##tôn giáo ' },{ sub: 'sport thể', display: 'sport##thể thao ' },{ sub: 'statistics tkê', display: 'statistics##số liệu thống kê ' },{ sub: 'technical', display: 'technical ' },]");
                lstSpecialWord = items.ToDictionary(x => x.sub, x => x.display);
            }
            if (lstSpecialWord.ContainsKey(key))
            {
                return lstSpecialWord[key];
            }
            return key;
        }

        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static bool Ping()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                return (from face in interfaces
                        where face.OperationalStatus == OperationalStatus.Up
                        where (face.NetworkInterfaceType != NetworkInterfaceType.Tunnel) && (face.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        select face.GetIPv4Statistics()).Any(statistics => (statistics.BytesReceived > 0) && (statistics.BytesSent > 0));
            }

            return false;
        }
    }

    public class DataRoutedEventArgs : RoutedEventArgs
    {
        public object Data { get; set; }
    }

    public delegate void DataRoutedEventHandler(object sender, DataRoutedEventArgs e);

    public class TempClass
    {
        public string sub { get; set; }
        public string display { get; set; }
    }

    public enum GameName
    {
        LookAndMatch = 1,
        MatchWord = 2,
        ListenAndSay = 3,
        Unscamble = 4,
        Spelltheword = 5,
        Hangman = 6,
        Wordsearch = 7,
        Memory = 8
    }
}