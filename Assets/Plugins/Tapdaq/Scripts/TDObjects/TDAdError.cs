using System;
using System.Collections.Generic;

namespace Tapdaq
{
	[Serializable]
	public class TDAdError
	{
		public int code;
		public string message;
		public Dictionary<String, List<TDAdError>> subErrors;
	}
}

