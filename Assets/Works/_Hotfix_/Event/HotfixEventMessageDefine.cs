
namespace Hotfix
{
	public class HotfixEvent
	{
		// 这里只定义热更层的事件类型
		public class ConnectServer : IHotfixEventMessage
		{
			public string Account;
			public string Password;
		}
	}
}