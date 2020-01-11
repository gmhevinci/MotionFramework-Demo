
namespace Hotfix
{
	/// <summary>
	/// 热更层的事件标签
	/// </summary>
	public enum HotfixEventMessageTag
	{
		BattleEvent,
		LoginEvent,
	}
	
	/// <summary>
	/// 登录事件
	/// </summary>
	public class LoginEvent
	{
		public class ConnectServer : IHotfixEventMessage
		{
			public string Account;
			public string Password;
		}
	}

	/// <summary>
	/// 战斗事件
	/// </summary>
	public class BattleEvent
	{
		public class CharacterDead : IHotfixEventMessage
		{
			public int EntityID;
		}

		public class DamageHurt : IHotfixEventMessage
		{
			public int SourceEntityID;
			public int TargetEntityID;
			public double Damage;
		}

		public class PlayerSpell : IHotfixEventMessage
		{
			public int SkillID;
		}
	}
}