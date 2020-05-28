using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tapdaq {
	[Serializable]
	public class TDVideoReward {
		public string EventId;
		public string RewardName;
		public int RewardAmount;
		public string Location;
		public string Tag;
		public bool RewardValid;
		public object RewardJson;
	}
}
