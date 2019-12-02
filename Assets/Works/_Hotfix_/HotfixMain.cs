using UnityEngine;

namespace Hotfix
{
	public static class HotfixMain
	{
		public static void Start()
		{
			Debug.Log("hello world");

			HotfixNetManager.Instance.Start();
			HotfixTest.Instance.Start();
		}
		public static void Update()
		{
			HotfixNetManager.Instance.Update();
			HotfixTest.Instance.Update();
		}
		public static void LateUpdate()
		{
		}
	}
}