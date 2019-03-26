namespace WifiSecurity.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class CountryLocalizationMap
    {
        //// todo move to resources - to exlude ours from localization.

        /// <summary>
        /// The country japanize names.
        /// </summary>
        [XmlArray]
        public static readonly List<CountryLocalized> CountryLocalizedNames =
            new List<CountryLocalized>()
                {
                    { new CountryLocalized("NO", "ノルウェー", "Norway") },
                    { new CountryLocalized("DE", "ドイツ", "Germany") },
                    { new CountryLocalized("HK", "香港", "Hong Kong") },
                    { new CountryLocalized("RU", "ロシア連邦", "Russian Federation") },
                    { new CountryLocalized("JP", "日本", "Japan") },
                    { new CountryLocalized("DK", "デンマーク", "Denmark") },
                    { new CountryLocalized("FR", "フランス", "France") },
                    { new CountryLocalized("UA", "ウクライナ", "Ukraine") },
                    { new CountryLocalized("BR", "ブラジル", "Brazil") },
                    { new CountryLocalized("SE", "スウェーデン", "Sweden") },
                    { new CountryLocalized("SG", "シンガポール", "Singapore") },
                    { new CountryLocalized("GB", "イギリス", "United Kingdom") },
                    { new CountryLocalized("ID", "インドネシア", "Indonesia") },
                    { new CountryLocalized("IE", "アイルランド", "Ireland") },
                    { new CountryLocalized("CA", "カナダ", "Canada") },
                    { new CountryLocalized("US", "アメリカ", "United States") },
                    { new CountryLocalized("CH", "スイス", "Switzerland") },
                    { new CountryLocalized("IN", "インド", "India") },
                    { new CountryLocalized("MX", "メキシコ", "Mexico") },
                    { new CountryLocalized("IT", "イタリア", "Italy") },
                    { new CountryLocalized("ES", "スペイン", "Spain") },
                    { new CountryLocalized("AU", "オーストラリア", "Australia") },
                    { new CountryLocalized("CZ", "チェコ共和国", "Czech Republic") },
                    { new CountryLocalized("RO", "ルーマニア", "Romania") },
                    { new CountryLocalized("TR", "トルコ", "Turkey") },
                    { new CountryLocalized("NL", "オランダ", "Netherlands") },
                    { new CountryLocalized("AR", "アルゼンチン", "Argentina") },
                };

        public class CountryLocalized
        {
            public CountryLocalized()
            {
            }

            public CountryLocalized(string code, string localizedName)
            {
                this.CountryCode = code;
                this.CountryLocalName = localizedName;
            }

            public CountryLocalized(string code, string localizedName, string englishName)
            {
                this.CountryCode = code;
                this.CountryLocalName = localizedName;
                this.EnglishName = englishName;
            }

            [XmlElement]
            public string CountryCode { get; set; }

            [XmlElement]
            public string CountryLocalName { get; set; }

            [XmlElement]
            public string EnglishName { get; set; }
        }
    }
}