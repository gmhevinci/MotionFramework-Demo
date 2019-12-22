
namespace Hotfix
{
	// 定义热更层的事件类型
	public class HotfixEvent
	{
		public class ConnectServer : IHotfixEventMessage
		{
			public string Account;
			public string Password;
		}
	}

	// 战斗事件类型
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