using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Tapdaq {
	[Serializable]
	public class TDAdEvent {
		public string adType;
		public string message;
		public string tag;
		public TDAdError error;

		public TDAdEvent() {
		}

		public TDAdEvent(string adType, string message, string tag = null) {
			this.adType = adType;
			this.message = message;
			this.tag = tag;
		}

		public TDAdType GetTypeOfEvent()  {
			if (adType == "INTERSTITIAL")
				return TDAdType.TDAdTypeInterstitial;
			
			if (adType == "BANNER")
				return TDAdType.TDAdTypeBanner;
			
			if (adType == "VIDEO")
				return TDAdType.TDAdTypeVideo;
			
			if (adType == "REWARD_AD")
				return TDAdType.TDAdTypeRewardedVideo;
			return TDAdType.TDAdTypeNone;
		}

		public bool IsInterstitialEvent()  {
			return GetTypeOfEvent() == TDAdType.TDAdTypeInterstitial;
		}

		public bool IsVideoEvent()  {
			return GetTypeOfEvent() == TDAdType.TDAdTypeVideo;
		}

		public bool IsRewardedVideoEvent()  {
			return GetTypeOfEvent() == TDAdType.TDAdTypeRewardedVideo;
		}

		public bool IsBannerEvent()  {
			return GetTypeOfEvent() == TDAdType.TDAdTypeBanner;
		}
	}
}