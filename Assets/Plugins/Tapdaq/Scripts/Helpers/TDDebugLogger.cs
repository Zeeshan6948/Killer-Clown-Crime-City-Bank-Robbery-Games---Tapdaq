using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tapdaq;

public class TDDebugLogger {
	
	public static void Log(object obj) {
		if (TDSettings.getInstance().isDebugMode) {
			Debug.Log (obj);
		}
	}

	public static void LogWarning(object obj) {
		if (TDSettings.getInstance().isDebugMode) {
			Debug.LogWarning (obj);
		}
	}

	public static void LogError(object obj) {
		if (TDSettings.getInstance().isDebugMode) {
			Debug.LogError (obj);
		}
	}

	public static void LogException(System.Exception obj) {
		if (TDSettings.getInstance().isDebugMode) {
			Debug.LogException (obj);
		}
	}
}
