using UnityEngine;

public static class GameLogger
{
	public static void Log(string content)
	{
		Debug.Log("[Game] " + content);
	}

	public static void Warning(string content)
	{
		Debug.LogWarning("[Game] " + content);
	}

	public static void Error(string content)
	{
		Debug.LogError("[Game] " + content);
	}
}