using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Tapdaq
{
	[Serializable]
	public class TDNetwork
	{
		[SerializeField]
		public readonly string name;
        [SerializeField]
        public TDSDKTargetVersion sdkTargetVersion;
        [SerializeField]
		public readonly string cocoapodsAdapterDependency;
		[SerializeField]
		public readonly string mavenAdapterDependency;
		[SerializeField]
		public readonly string mavenNetworkRepository;
		[SerializeField]
		public bool androidEnabled = false;
		[SerializeField]
		public bool iOSEnabled = false;

        public TDNetwork(string network_name, TDSDKTargetVersion targetSdkVersion, string cocoapods_dependency, string maven_dependency, string maven_network_repository = null)
		{
			this.name = network_name;
            this.sdkTargetVersion = targetSdkVersion;
			this.cocoapodsAdapterDependency = cocoapods_dependency;
			this.mavenAdapterDependency = maven_dependency;
			this.mavenNetworkRepository = maven_network_repository;
		}

        public static bool IsAndroidSupported(TDNetwork network)
		{
			return network.mavenAdapterDependency != null;
		}

		public static bool IsIOSSupported(TDNetwork network)
		{
			return network.cocoapodsAdapterDependency != null;
		}

		[SerializeField]
		public static readonly List<TDNetwork> Networks = new List<TDNetwork>(new TDNetwork[]
		{
			new TDNetwork("AdColony", new TDSDKTargetVersion("9.0", "14"),"AdColony", "com.tapdaq.sdk:TapdaqAdColonyAdapter:", "https://adcolony.bintray.com/AdColony"),
			new TDNetwork("AdMob", new TDSDKTargetVersion("9.0", "14"), "AdMob", "com.tapdaq.sdk:TapdaqAdMobAdapter:"),
			new TDNetwork("AppLovin", new TDSDKTargetVersion("9.0", "14"), "AppLovin", "com.tapdaq.sdk:TapdaqAppLovinAdapter:"),
			new TDNetwork("Chartboost", new TDSDKTargetVersion("9.0", "14"), "Chartboost", "com.tapdaq.sdk:TapdaqChartboostAdapter:"),
			new TDNetwork("Facebook AN", new TDSDKTargetVersion("9.0", "14"), "FAN", "com.tapdaq.sdk:TapdaqFANAdapter:"),
			new TDNetwork("InMobi", new TDSDKTargetVersion("9.0", "14"), "InMobi", "com.tapdaq.sdk:TapdaqInmobiAdapter:"),
			new TDNetwork("ironSource", new TDSDKTargetVersion("9.0", "14"), "IronSource", "com.tapdaq.sdk:TapdaqIronsourceAdapter:", "https://dl.bintray.com/ironsource-mobile/android-sdk"),
			new TDNetwork("Maio", new TDSDKTargetVersion("9.0", "14"), "Maio", "com.tapdaq.sdk:TapdaqMaioAdapter:", "https://imobile-maio.github.io/maven"),
			new TDNetwork("Mintegral", new TDSDKTargetVersion("9.0", "14"), "Mintegral", "com.tapdaq.sdk:TapdaqMintegralAdapter:"),
			new TDNetwork("Tapjoy", new TDSDKTargetVersion("9.0", "14"), "Tapjoy", "com.tapdaq.sdk:TapdaqTapjoyAdapter:", "https://tapjoy.bintray.com/maven"),
			new TDNetwork("TikTok", new TDSDKTargetVersion("9.0", null), "TikTok", null),
			new TDNetwork("UnityAds", new TDSDKTargetVersion("9.0", "14"), "UnityAds", "com.tapdaq.sdk:TapdaqUnityAdsAdapter:"),
			new TDNetwork("Vungle", new TDSDKTargetVersion("9.0", "14"), "Vungle", "com.tapdaq.sdk:TapdaqVungleAdapter:", "https://jitpack.io"),
			new TDNetwork("YouAppi", new TDSDKTargetVersion("10.0", "14"), "YouAppi", "com.tapdaq.sdk:TapdaqYouAppiAdapter:", "http://repository.youappi.com/repository/release"),
			new TDNetwork("ZPlay", new TDSDKTargetVersion("9.0", "14"), "ZPlay", "com.tapdaq.sdk:TapdaqZPlayAdapter:")
		});
	}

    [Serializable]
    public class TDSDKTargetVersion
    {
        [SerializeField]
        public readonly string iOS;
        [SerializeField]
        public readonly string android;

        public TDSDKTargetVersion(string iosTarget, string androidTarget)
        {
            iOS = iosTarget;
            android = androidTarget;
        }
    }
}

