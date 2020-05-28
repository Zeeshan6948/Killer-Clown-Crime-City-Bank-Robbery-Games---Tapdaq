// <copyright file="GPGSDependencies.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using UnityEditor;
using Tapdaq;
using UnityEngine;
using System.Reflection;
using System.Xml.Serialization;
using TDXMLSchema;
using System.IO;

[InitializeOnLoad]
public class TDDependencies : AssetPostprocessor
{
	private static string TAPDAQ_ANDROID_VERSION = "7.4.1";
	private static string TAPDAQ_IOS_VERSION = "7.4.0";

    private static string DEPDENCIES_DIRECTORY = "/Plugins/Tapdaq/Editor/TapdaqDependencies.xml";
    private static string TAPDAQ_REPOSITORY = "http://android-sdk.tapdaq.com";

    public static object svcSupport;

    // Manual Integration
    // This value may be set as low as 17.2.0+ ****
    private static string playServicesIdentityVersion = "17.0.0";
    private static string playServicesAdsVersion = "18.2.0";
    private static string playServicesBaseVersion = "17.1.0";
    private static string playServicesGcmVersion = "17.0.0";

    // DO NOT CHANGE THESE VALUES 
    private static string minimumLifecycleVersion = "1.+";

    private static string minimumSupportLibraryVersion = "24.0.0+";
    private static string minimumInMobiSupportLibraryVersion = "24.0.0+";
    private static string minimumFANSupportLibraryVersion = "24.0.0+";
    private static string minimumVungleSupportLibraryVersion = "26.0.0+";

    public static string minTargetSDKVersion = "9.0";

	public static TDSettings settings;

    public static string cocoapods_respository = "https://github.com/tapdaq/cocoapods-specs.git";

    public static Dependencies dependencies = new Dependencies();

	static TDDependencies()
	{
		if (!settings)
		{
			settings = TDSettings.getInstance();
		}

		RegisterDependencies();
	}

	public static void RegisterDependencies()
	{
        dependencies = new Dependencies();

        RegisterAndroidDependencies();
		RegisterIOSDependencies();
        ResolveAdapters();
        SaveDependencies();
	}

    private static void SaveDependencies()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dependencies));
        string path = Application.dataPath;
        StreamWriter writer = new StreamWriter(path + DEPDENCIES_DIRECTORY);
        serializer.Serialize(writer.BaseStream, dependencies);
        writer.Close();
    }

    public static void RegisterAndroidDependencies()
	{
        if(settings.useCocoapodsMaven)
        {
            if (settings.useCocoapodsMaven)
            {
                List<String> repositories = new List<string>();
                repositories.Add(TAPDAQ_REPOSITORY);
                // Fetch Tapdaq SDK
                FetchAndroid("com.tapdaq.sdk:Tapdaq-BaseSDK:" + TAPDAQ_ANDROID_VERSION, repositories);
                FetchAndroid("com.tapdaq.sdk:Tapdaq-UnityBridge:" + TAPDAQ_ANDROID_VERSION, repositories);
            }
        } else
        {
            RegisterManualAndroidDependencies();
        }
	}

	public static void FetchAndroid(string spec, List<String> repositories)
	{
		Debug.Log("FetchAndroid: " + spec);
		// Cretae an AndroidPackage object and add to Dependenies class
		Repositories r = new Repositories();
		r.Repository = repositories;
		AndroidPackage p = new AndroidPackage();
		p.Spec = spec;
		p.Repositories = r;

		dependencies.AndroidPackages.AndroidPackage.Add(p);
	}


	public static void ResolveAdapters()
	{
        if (TDSettings.getInstance().useCocoapodsMaven)
        {
            // Check status of each network and resolve if enabled
            foreach (TDNetwork network in TDSettings.getInstance().networks)
            {
                Debug.Log("Adapter: " + network.name + "- Android: " + network.mavenAdapterDependency + " Enabled: " + network.androidEnabled + " - iOS: " + network.iOSEnabled);

                if (network.iOSEnabled)
                {
                    FetchiOSPod("Tapdaq/" + network.cocoapodsAdapterDependency, TAPDAQ_IOS_VERSION, network.sdkTargetVersion.iOS);
                }

                if (network.androidEnabled)
                {
                    List<String> repositories = new List<string>();
                    repositories.Add(TAPDAQ_REPOSITORY);

                    if (network.mavenNetworkRepository != null)
                    {
                        repositories.Add(network.mavenNetworkRepository);
                    }
                    FetchAndroid(network.mavenAdapterDependency + TAPDAQ_ANDROID_VERSION, repositories);
                }
            }
        }
    }

	public static void RegisterIOSDependencies()
	{
        if (TDSettings.getInstance().useCocoapodsMaven)
		{
			// ADD TAPDAQ POD  
			Sources s = new Sources();
			s.Source = cocoapods_respository;

			dependencies.IosPods.Sources = s;

			FetchiOSPod("Tapdaq", TAPDAQ_IOS_VERSION, "9.0");
		}
	}

	public static void FetchiOSPod(string podName, string version, string minTargetSDK)
	{
		// Cretae an iosPod object and add to Dependenies class
		IosPod p = new IosPod();
		p.Name = podName;
		p.Version = version;
		p.MinTargetSdk = minTargetSDK;

		dependencies.IosPods.IosPod.Add(p);
	}

    public static void RegisterManualAndroidDependencies()
    {
#if UNITY_ANDROID
        // Setup the resolver using reflection as the module may not be
        // available at compile time.
        Type playServicesSupport = Google.VersionHandler.FindClass(
            "Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
        if (playServicesSupport == null)
        {
            return;
        }

        svcSupport = svcSupport ?? Google.VersionHandler.InvokeStaticMethod(
            playServicesSupport, "CreateInstance",
            new object[] {
                "GooglePlayGames",
                EditorPrefs.GetString("AndroidSdkRoot"),
                "ProjectSettings"
            });

        string playServicesVersion = playServicesAdsVersion;
        string supportLibraryVersion = minimumSupportLibraryVersion;

        if (AssetDatabase.FindAssets("TapdaqFANAdapter").Length > 0)
        {
            supportLibraryVersion = getHighestVersion(supportLibraryVersion, minimumFANSupportLibraryVersion);
        }

        if (AssetDatabase.FindAssets("TapdaqInMobiAdapter").Length > 0)
        {
            supportLibraryVersion = getHighestVersion(supportLibraryVersion, minimumInMobiSupportLibraryVersion);
        }

        if (AssetDatabase.FindAssets("TapdaqVungleAdapter").Length > 0)
        {
            supportLibraryVersion = getHighestVersion(supportLibraryVersion, minimumVungleSupportLibraryVersion);
        }

        if (AssetDatabase.FindAssets("TapdaqZPlayAdapter").Length > 0)
        {
            supportLibraryVersion = getHighestVersion(supportLibraryVersion, minimumSupportLibraryVersion);
        }

        if (AssetDatabase.FindAssets("Tapdaq", new[] { "Assets/Plugins/Android" }).Length > 0)
        {
            //Required by Tapdaq
            Google.VersionHandler.InvokeInstanceMethod(
            svcSupport, "DependOn",
            new object[] { "com.google.android.gms", "play-services-ads-identifier", playServicesIdentityVersion },
        namedArgs: new Dictionary<string, object>() {
            {"packageIds", new string[] { "extra-google-m2repository" } }
        });

            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "android.arch.lifecycle", "extensions", minimumLifecycleVersion },
            namedArgs: new Dictionary<string, object>() {
            {"packageIds", new string[] { "extra-android-m2repository" } }
            });
        }

        // Required by AdColony
        if (AssetDatabase.FindAssets("TapdaqAdColonyAdapter").Length > 0)
        {
            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "com.google.android.gms", "play-services-gcm", playServicesGcmVersion },
            namedArgs: new Dictionary<string, object>() {
                { "packageIds", new string[] { "extra-google-m2repository" } }
            });
        }

        //Required by AdMob
        if (AssetDatabase.FindAssets("TapdaqAdMobAdapter").Length > 0)
        {
            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "com.google.android.gms", "play-services-ads", playServicesAdsVersion },
                namedArgs: new Dictionary<string, object>() {
                    { "packageIds", new string[] { "extra-google-m2repository" } }
                });
        }

        //Required by Chartboost
        if (AssetDatabase.FindAssets("TapdaqChartboostAdapter").Length > 0)
        {
            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "com.google.android.gms", "play-services-base", playServicesBaseVersion },
            namedArgs: new Dictionary<string, object>() {
                {"packageIds", new string[] { "extra-android-m2repository" } }
            });
        }

        //Required by InMobi
        if (AssetDatabase.FindAssets("TapdaqInMobiAdapter").Length > 0)
        {
            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "com.android.support", "recyclerview-v7", supportLibraryVersion },
                namedArgs: new Dictionary<string, object>() {
                {"packageIds", new string[] { "extra-android-m2repository" } }
            });

            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "com.android.support", "support-v4", supportLibraryVersion },
                namedArgs: new Dictionary<string, object>() {
                {"packageIds", new string[] { "extra-android-m2repository" } }
            });
        }

        //Required by FAN
        if (AssetDatabase.FindAssets("TapdaqFANAdapter").Length > 0)
        {
            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "com.android.support", "recyclerview-v7", supportLibraryVersion },
            namedArgs: new Dictionary<string, object>() {
                {"packageIds", new string[] { "extra-android-m2repository" } }
            });
        }

        if (AssetDatabase.FindAssets("TapdaqZPlayAdapter").Length > 0)
        {
            Google.VersionHandler.InvokeInstanceMethod(
                svcSupport, "DependOn",
                new object[] { "com.android.support", "support-v4", supportLibraryVersion },
                namedArgs: new Dictionary<string, object>() {
                {"packageIds", new string[] { "extra-android-m2repository" } }
            });
        }
#endif
    }

    private static string cleanVersionString(string version)
    {
        version = version.Replace("+", "");
        if (version.EndsWith(".", StringComparison.Ordinal))
        {
            version += "0";
        }
        return version;
    }

    // Compares two version numbers. Must have at least 2 numbers e.g. 1.0
    // Plus symbols will be removed for comparison but return value will include them
    private static string getHighestVersion(string v, string v1)
    {
        var version1 = new Version(cleanVersionString(v));
        var version2 = new Version(cleanVersionString(v1));

        var result = version1.CompareTo(version2);
        if (result > 0)
            return v;
        else if (result < 0)
            return v1;

        return v;
    }

    // Handle delayed loading of the dependency resolvers.
    private static void OnPostprocessAllAssets(
		string[] importedAssets, string[] deletedAssets,
		string[] movedAssets, string[] movedFromPath)
	{
		foreach (string asset in importedAssets)
		{
			if (asset.Contains("IOSResolver") ||
				asset.Contains("JarResolver"))
			{
				RegisterDependencies();
				break;
			}
		}
	}
}

// Classes necessary to allow us to serialize XML
namespace TDXMLSchema
{
	[XmlRoot(ElementName = "repositories")]
	public class Repositories
	{
		[XmlElement(ElementName = "repository")]
		public List<string> Repository { get; set; }
	}

	[XmlRoot(ElementName = "androidPackage")]
	public class AndroidPackage
	{
		[XmlElement(ElementName = "repositories")]
		public Repositories Repositories { get; set; }
		[XmlAttribute(AttributeName = "spec")]
		public string Spec { get; set; }
	}

	[XmlRoot(ElementName = "androidPackages")]
	public class AndroidPackages
	{
		[XmlElement(ElementName = "androidPackage")]
		public List<AndroidPackage> AndroidPackage = new List<AndroidPackage>();
	}

	[XmlRoot(ElementName = "sources")]
	public class Sources
	{
		[XmlElement(ElementName = "source")]
		public string Source { get; set; }
	}

	[XmlRoot(ElementName = "iosPod")]
	public class IosPod
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "minTargetSdk")]
		public string MinTargetSdk { get; set; }
	}

	[XmlRoot(ElementName = "iosPods")]
	public class IosPods
	{
		[XmlElement(ElementName = "sources")]
		public Sources Sources = new Sources();
		[XmlElement(ElementName = "iosPod")]
		public List<IosPod> IosPod = new List<IosPod>();
	}

	[XmlRoot(ElementName = "dependencies")]
	public class Dependencies
	{
		[XmlElement(ElementName = "androidPackages")]
		public AndroidPackages AndroidPackages = new AndroidPackages();
		[XmlElement(ElementName = "iosPods")]
		public IosPods IosPods = new IosPods();
	}
}