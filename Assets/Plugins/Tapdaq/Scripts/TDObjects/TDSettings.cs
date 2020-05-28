using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tapdaq {

	public class TDSettings : ScriptableObject {
		private static TDSettings instance;

		public const string pluginVersion = "unity_7.4.2";
		
		public string ios_applicationID = "";
		public string ios_clientKey = "";
		public string android_applicationID = "";
		public string android_clientKey = "";

        public string admob_appid_ios = "";
        public string admob_appid_android = "";

        public bool isDebugMode = false;
		public bool autoReloadAds = false;

        public bool useCocoapodsMaven = false;

		[SerializeField]
		public List<TDNetwork> networks = new List<TDNetwork>(TDNetwork.Networks.ToArray());

		[SerializeField]
		public List<TestDevice> testDevices = new List<TestDevice>();

        public static TDSettings getInstance() {
			if (instance == null) {
				TDSettings[] settings = Resources.LoadAll<TDSettings> ("Tapdaq");
                if(settings != null && settings.Length > 0)
                {
                    instance = settings[0];
                } else
                {
                    return new TDSettings();
                }
			}
            return instance;
		}
	}


	public enum TestDeviceType {
		Android,
		iOS
	}

	[Serializable]
	public class TestDevice {

		public string name;
		public TestDeviceType type;

		public string adMobId;
		public string facebookId;

		public TestDevice(string deviceName, TestDeviceType deviceType) {
			name = deviceName;
			type = deviceType;
		}
	}

	[Serializable]
	public class TestDevicesList {
	
		[SerializeField]
		public List<string> adMobDevices = new List<string>();
		[SerializeField]
		public List<string> facebookDevices = new List<string>();

		public TestDevicesList(List<TestDevice> devices, TestDeviceType deviceType) {

			foreach (var device in devices) {
				if (device.type == deviceType) {
					if (!string.IsNullOrEmpty (device.adMobId)) {
						adMobDevices.Add (device.adMobId);
					}
					if (!string.IsNullOrEmpty (device.facebookId)) {
						facebookDevices.Add (device.facebookId);
					}
				}
			}
		}

		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this);
		}

		public string GetAdMobListJson() {
			return JsonConvert.SerializeObject (adMobDevices);
		}

		public string GetFacebookListJson() {
			return JsonConvert.SerializeObject (facebookDevices);
		}
	}
}