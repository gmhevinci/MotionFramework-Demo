using UnityEngine;

namespace Hotfix
{
	public static class HotfixMain
	{
		public static void Start()
		{
			HotfixLogger.Log("hello hotfix world");

			AudioPlayerSetting.InitAudioSetting();

			// 初始化数据管理器
			DataManager.Instance.Start();

			// 初始化界面管理器
			UIManager.Instance.Start();

			// 初始化网络管理器
			HotfixNetManager.Instance.Start();

			// 初始化状态机管理器
			var prepare = new HotfixStatePrepare(EHotfixStateType.Prepare);
			var notice = new HotfixStateNotice(EHotfixStateType.Notice);
			var login = new HotfixStateLogin(EHotfixStateType.Login);
			var town = new HotfixStateTown(EHotfixStateType.Town);
			HotfixFsmManager.Instance.AddState(prepare);
			HotfixFsmManager.Instance.AddState(notice);
			HotfixFsmManager.Instance.AddState(login);
			HotfixFsmManager.Instance.AddState(town);
			HotfixFsmManager.Instance.Run(EHotfixStateType.Prepare);
		}
		public static void Update()
		{
			HotfixFsmManager.Instance.Update();
			HotfixNetManager.Instance.Update();
			DataManager.Instance.Update();
			UIManager.Instance.Update();
		}
		public static void LateUpdate()
		{
		}
		public static string UILanguage(string key)
		{
			CfgUILanguageTab table = CfgUILanguage.Instance.GetCfgTab(key.GetHashCode());
			if (table == null)
				return key;
			else
				return table.Lang;
		}
	}
}