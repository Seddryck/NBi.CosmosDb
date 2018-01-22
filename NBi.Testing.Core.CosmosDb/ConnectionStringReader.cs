using System;
using System.Xml;

namespace NBi.Testing.Core.CosmosDb
{
    class ConnectionStringReader
    {
        public static string Get(string name)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNodeList nodes = xmldoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
                if (node.Attributes["name"].Value == name)
                    return node.Attributes["connectionString"].Value;
            throw new Exception();
        }

        private static string Get(string name, string api) => $"{Get(name)};api={api}";

        public static string GetLocaleGraph() => Get("LocaleDB", "graph");
        public static string GetAzureGraph() => Get("AzureDB", "graph");
        public static string GetLocaleSql() => Get("LocaleDB", "sql");
        public static string GetAzureSql() => Get("AzureDB", "sql");

        private static string GetFilename()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
                return "ConnectionString.user.config";
            else if (System.IO.File.Exists("ConnectionString.config"))
                return "ConnectionString.config";
            return "";
        }

        


    }
}
