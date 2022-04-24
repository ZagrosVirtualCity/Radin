using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace AppUpdater
{
    public class Updater
    {
        public static string UpdateServer;
        static readonly string UpdateXmlTag = "SPZ"; //Defined in Xml file
        static readonly string UpdateXmlChildTag = "AppVersion"; //Defined in Xml file
        static readonly string UpdateVersionTag = "version"; //Defined in Xml file
        static readonly string UpdateUrlTag = "url"; //Defined in Xml file
        static readonly string UpdateChangeLogTag = "changelog";
        public static string newVersion = string.Empty;
        public static string url = "";
        public static string ChangeLog = "";
        public static string oldEdition = "";

        public static bool IsVersionLater(string newVersion, string oldVersion)
        {
            // split into groups
            string[] cur = newVersion.Split('.');
            string[] cmp = oldVersion.Split('.');
            // get max length and fill the shorter one with zeros
            int len = Math.Max(cur.Length, cmp.Length);
            int[] vs = new int[len];
            int[] cs = new int[len];
            Array.Clear(vs, 0, len);
            Array.Clear(cs, 0, len);
            int idx = 0;
            // skip non digits
            foreach (string n in cur)
            {
                if (!Int32.TryParse(n, out vs[idx]))
                {
                    vs[idx] = -999; // mark for skip later
                }
                idx++;
            }
            idx = 0;
            foreach (string n in cmp)
            {
                if (!Int32.TryParse(n, out cs[idx]))
                {
                    cs[idx] = -999; // mark for skip later
                }
                idx++;
            }
            for (int i = 0; i < len; i++)
            {
                // skip non digits
                if ((vs[i] == -999) || (cs[i] == -999))
                {
                    continue;
                }
                if (vs[i] < cs[i])
                {
                    return (false);
                }
                else if (vs[i] > cs[i])
                {
                    return (true);
                }
            }
            return (false);
        }
        public static bool CheckUpdate()
        {
            newVersion = string.Empty;
            ChangeLog = string.Empty;
            url = "";


            XDocument doc = XDocument.Load(UpdateServer);
            var items = doc
                .Element(XName.Get(UpdateXmlTag))
                .Elements(XName.Get(UpdateXmlChildTag));
            var versionItem = items.Select(ele => ele.Element(XName.Get(UpdateVersionTag)).Value);
            var urlItem = items.Select(ele => ele.Element(XName.Get(UpdateUrlTag)).Value);
            var changelogItem = items.Select(ele => ele.Element(XName.Get(UpdateChangeLogTag)).Value);


            newVersion = versionItem.FirstOrDefault();
            url = urlItem.FirstOrDefault();
            ChangeLog = changelogItem.FirstOrDefault();

            if (Updater.IsVersionLater(newVersion, oldEdition))
                return true;
            else
                return false;
        }
      
    }
}
