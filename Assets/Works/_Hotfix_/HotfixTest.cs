using UnityEngine;
using UnityEngine.UI;
using MotionEngine.Res;
using MotionEngine.Event;
using MotionGame;

namespace Hotfix
{
	public class HotfixTest
	{
		public static readonly HotfixTest Instance = new HotfixTest();

		private AssetObject _uiRoot;
		private AssetObject _uiLogin;
		private Transform _uiDesktop;
		private UISprite _loginSprite;
		private bool _isSendLoginMsg = false;

		public void Start()
		{
			// 加载UIRoot
			_uiRoot = new AssetObject();
			_uiRoot.Load("UIPanel/UIRoot", OnUIRootLoad);

			// 向Mono层注册事件监听
			EventManager.Instance.AddListener(EventMessageTag.HotfixTag.ToString(), OnHandleMonoEvent);
		}
		public void Update()
		{
			// 当网络连接成功之后发送测试消息
			if (_isSendLoginMsg == false && NetManager.Instance.State == MotionGame.ENetworkState.Connected)
			{
				_isSendLoginMsg = true;
				SendTestMsg();
			}
		}

		private void OnHandleMonoEvent(IEventMessage msg)
		{
			if(msg is TestEventMsg)
			{
				TestEventMsg temp = msg as TestEventMsg;
				Debug.Log($"这里是Hotfix层 {temp.Value}");
			}
		}
		private void OnUIRootLoad(Asset asset)
		{
			if (asset.Result != EAssetResult.OK)
				return;

			GameObject root = _uiRoot.GetMainAsset<GameObject>();
			GameObject.DontDestroyOnLoad(root);

			// 获取桌面对象
			// 说明：该对象主要用于调整齐刘海
			_uiDesktop = root.transform.FindChildByName("UIDesktop");

			// 加载登录界面
			_uiLogin = new AssetObject();
			_uiLogin.Load("UIPanel/UILogin", OnUILoginLoad);
		}
		private void OnUILoginLoad(Asset asset)
		{
			if (asset.Result != EAssetResult.OK)
				return;

			GameObject go = _uiLogin.GetMainAsset<GameObject>();
			go.transform.SetParent(_uiDesktop, false);

			// 获取UIManifest组件
			UIManifest manifest = go.GetComponent<UIManifest>();

			// 获取UISprite组件
			_loginSprite = manifest.GetComponent("UILogin/BtnLogin", "UISprite") as UISprite;

			// 通过配表数据设置文本
			var hero1 = CfgHero.Instance.GetCfgTab(1001);
			var text1 = manifest.GetComponent("UILogin/Text1", "Text") as Text;
			text1.text = hero1.Name;
			Debug.Log($"热更新表格数据：{hero1.Name}");

			// 通过配表数据设置文本
			var hero2 = CfgHero.Instance.GetCfgTab(1002);
			var text2 = manifest.GetComponent("UILogin/Text2", "Text") as Text;
			text2.text = hero2.Name;
			Debug.Log($"热更新表格数据：{hero2.Name}");

			// 监听按钮点击事件
			Button btnLogin = manifest.GetComponent("UILogin/BtnLogin", "Button") as Button;
			btnLogin.onClick.AddListener(OnClickLogin);
		}
		private void OnClickLogin()
		{
			// 播放点击音效
			AudioManager.Instance.PlaySound("click");

			// 替换按钮图片
			if (_loginSprite.SpriteName == "button_1")
				_loginSprite.SpriteName = "button_2";
			else
				_loginSprite.SpriteName = "button_1";

			// 连接到ET5.0服务器
			if (NetManager.Instance.State == MotionGame.ENetworkState.Disconnect)
				NetManager.Instance.ConnectServer("127.0.0.1", 10002, typeof(NetProtoPackageParser));

			// 向Mono层发送测试事件
			TestEventMsg eventMsg = new TestEventMsg()
			{
				Value = $"test event from hotfix {Time.frameCount}",
			};
			EventManager.Instance.Send(EventMessageTag.TestTag.ToString(), eventMsg);
		}
		private void SendTestMsg()
		{
			C2R_Login msg = new C2R_Login();
			msg.RpcId = 100;
			msg.Account = "test";
			msg.Password = "1234567";
			HotfixNetManager.Instance.SendMsg(msg);
		}
	}
}
