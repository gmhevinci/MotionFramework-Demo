using System.Diagnostics;

public static class GameLogger
{
	[Conditional("DEBUG")]
	public static void Log(string content)
	{
		UnityEngine.Debug.Log("[Game] " + content);
	}

	public static void Warning(string content)
	{
		UnityEngine.Debug.LogWarning("[Game] " + content);
	}

	public static void Error(string content)
	{
		UnityEngine.Debug.LogError("[Game] " + content);
	}
}