using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
	public static class HotfixLogger 
	{
		public static void Log(string content)
		{
			Debug.Log("[Hotfix] " + content);
		}
		
		public static void Warning(string content)
		{
			Debug.LogWarning("[Hotfix] " + content);
		}

		public static void Error(string content)
		{
			Debug.LogError("[Hotfix] " + content);
		}
	}
}