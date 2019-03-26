namespace ResourceExporter
{
    using System.Resources;

    class Program
    {
        static void Main(string[] args)
        {
            PutToResx();
        }


        public static void PutToResx()
        {
            // Define a resource file named CarResources.resx.
            using (ResXResourceWriter resx = new ResXResourceWriter(@".\Country_ja_jp.resx"))
            {
                WifiSecurity.Helper.CountryLocalizationMap.CountryLocalizedNames.ForEach(
                    x =>
                        {
                            resx.AddResource(x.CountryCode, x.CountryLocalName);
                        });
            }

            using (ResXResourceWriter resx = new ResXResourceWriter(@".\Country_en_us.resx"))
            {
                WifiSecurity.Helper.CountryLocalizationMap.CountryLocalizedNames.ForEach(
                    x =>
                        {
                            resx.AddResource(x.CountryCode, x.EnglishName);
                        });
            }
        }
    }
}
