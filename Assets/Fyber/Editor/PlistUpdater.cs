using UnityEngine;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace FyberPlugin.Editor
{
    public class PlistUpdater
    {
        private static XmlNode FindPlistDictNode(XmlDocument doc)
        {
            var curr = doc.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals("plist") && curr.ChildNodes.Count == 1)
                {
                    var dict = curr.FirstChild;
                    if (dict.Name.Equals("dict"))
                        return dict;
                }
                curr = curr.NextSibling;
            }
            return null;
        }

        private static XmlElement AddChildElement(XmlDocument doc, XmlNode parent, string elementName, string innerText = null)
        {
            var newElement = doc.CreateElement(elementName);
            if (!string.IsNullOrEmpty(innerText))
                newElement.InnerText = innerText;

            parent.AppendChild(newElement);
            return newElement;
        }

        private static XmlNode HasKey(XmlNode dict, string keyName)
        {
            var curr = dict.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals("key") && curr.InnerText.Equals(keyName))
                    return curr;
                curr = curr.NextSibling;
            }
            return null;
        }

        private static XmlNode GetNetworkNode(XmlNode dict, XmlDocument doc)
        {
            var name = HasKey(doc.DocumentElement, "name").NextSibling.InnerText;
            var curr = dict.FirstChild;
            while (curr != null)
            {
                if (curr.InnerText.Contains(name))
                    return curr;
                curr = curr.NextSibling;
            }
            return null;
        }


        public static void UpdatePlist(string path, string xmlNodeString)
        {
            const string fileName = "Info.plist";
            string fullPath = Path.Combine(path, fileName);

            var doc = new XmlDocument();
            doc.Load(fullPath);

            var dict = FindPlistDictNode(doc);
            if (dict == null)
            {
                Debug.LogError("Error parsing " + fullPath);
                return;
            }

            var config = new XmlDocument();
            config.LoadXml(xmlNodeString);

            if (config.FirstChild.Name.Equals("root"))
            {
                foreach (XmlNode node in config.FirstChild.ChildNodes)
                {
                    XmlNode imported = doc.ImportNode(node, true);
                    dict.AppendChild(imported);
                }
            }
            else
            {
                //the xml should end up looking like this
                /*
				<key>adapters</key>
				<array>
					<dict>
						<key>name</key>
						<string>NetworkAbc</string>
						<key>settings</key>
						<dict>
							<key>SPNetworkAbcAppId</key>
							<string>ThisISMyAppID</string>
						</dict>
					</dict>
				</array>
				*/
                XmlNode networks = HasKey(dict, "adapters");

                if (networks == null)
                {
                    networks = AddChildElement(doc, dict, "key", "adapters");
                    AddChildElement(doc, dict, "array");
                }

                XmlNode array = networks.NextSibling;

                var networkNode = GetNetworkNode(array, config);
                if (networkNode != null)
                {
                    array.RemoveChild(networkNode);
                }

                var networkConfig = AddChildElement(doc, array, "dict");
                foreach (XmlNode node in config.DocumentElement.ChildNodes)
                {
                    XmlNode imported = doc.ImportNode(node, true);
                    networkConfig.AppendChild(imported);
                }
            }

            //Strip whitespace from empty strings
            Regex nonwhite = new Regex("\\S");
            XmlNodeList elemList = doc.GetElementsByTagName("string");

            for (int i = 0; i < elemList.Count; i++)
            {
                if (!nonwhite.IsMatch(elemList[i].InnerText))
                {
                    elemList[i].InnerText = "";
                }
            }

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.None
            };

            XmlWriter xmlwriter = XmlWriter.Create(fullPath, settings);
            doc.Save(xmlwriter);
            xmlwriter.Close();

            //the xml writer barfs writing out part of the plist header.
            //so we replace the part that it wrote incorrectly here
            var reader = new StreamReader(fullPath);
            string textPlist = reader.ReadToEnd();
            reader.Close();

            int fixupStart = textPlist.IndexOf("<!DOCTYPE plist PUBLIC", System.StringComparison.Ordinal);
            if (fixupStart <= 0)
                return;
            int fixupEnd = textPlist.IndexOf('>', fixupStart);
            if (fixupEnd <= 0)
                return;

            string fixedPlist = textPlist.Substring(0, fixupStart);
            fixedPlist += "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">";
            fixedPlist += textPlist.Substring(fixupEnd + 1);

            var writer = new StreamWriter(fullPath, false);
            writer.Write(fixedPlist);
            writer.Close();
        }
    }
}
