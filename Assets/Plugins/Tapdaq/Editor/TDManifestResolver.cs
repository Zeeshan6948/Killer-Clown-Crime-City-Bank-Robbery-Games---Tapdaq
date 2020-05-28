using UnityEditor;
using System.IO;
using Tapdaq;
using System.Xml;

namespace TDEditor {

	public class TDManifestResolver {

		private const string androidManifestCopyPath = TDPaths.TapdaqResourcesPath + "/TDDefaultAndroidManifest.xml";

		private const string androidManifestDestinationPath = TDPaths.TapdaqAndroidPath + "/AndroidManifest.xml";

		public static void FixAndroidManifest() {
			if (!File.Exists (androidManifestDestinationPath)) {
				if (File.Exists (androidManifestCopyPath) && Directory.Exists(TDPaths.TapdaqAndroidPath)) {
					FileUtil.CopyFileOrDirectory (androidManifestCopyPath, androidManifestDestinationPath);
					AssetDatabase.Refresh ();
				}
			}

            AddAdMobId(TDSettings.getInstance().admob_appid_android);
		}

		public static void RemoveMainManifest() {
			if (File.Exists (androidManifestDestinationPath)) {
				FileUtil.DeleteFileOrDirectory (androidManifestDestinationPath);
			}
		}

        public static void AddAdMobId(string appId)
        {
            string path = Path.GetFullPath(TDPaths.TapdaqAndroidPath + "/AndroidManifest.xml");
            if(File.Exists(path)) {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList list = doc.DocumentElement.GetElementsByTagName("application");
                if(list.Count > 0)
                {
                    XmlNode appNode = list.Item(0);
                    XmlNodeList children = appNode.ChildNodes;
                    for(int i = 0; i < children.Count; i++)
                    {
                        XmlNode node = children.Item(i);
                        XmlAttributeCollection collection = node.Attributes;
                        for(int j =0; j < collection.Count; j++)
                        {
                            XmlNode n = collection.Item(j);
                            if(n.Value.Equals("com.google.android.gms.ads.APPLICATION_ID"))
                            {
                                //Exists
                                appNode.RemoveChild(node);
                                break;
                            }
                        }
                    }
                    if(!string.IsNullOrEmpty(appId))
                    {
                        XmlElement metadata = doc.CreateElement("meta-data");
                        appNode.AppendChild(metadata);

                        XmlAttribute nameAttr = doc.CreateAttribute("android", "name", "http://schemas.android.com/apk/res/android");
                        nameAttr.Value = "com.google.android.gms.ads.APPLICATION_ID";
                        metadata.Attributes.Append(nameAttr);

                        XmlAttribute valueAttr = doc.CreateAttribute("android", "value", "http://schemas.android.com/apk/res/android");
                        valueAttr.Value = appId;
                        metadata.Attributes.Append(valueAttr);
                    }
                }

                doc.Save(path);
            }

        }
	}
}
