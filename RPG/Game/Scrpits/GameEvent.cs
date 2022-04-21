using MotionFramework.Event;

/// <summary>
/// 登录事件
/// </summary>
public class LoginEvent
{
	public class ConnectServer : IEventMessage
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
	public class CharacterDead : IEventMessage
	{
		public int EntityID;
	}

	public class DamageHurt : IEventMessage
	{
		public int SourceEntityID;
		public int TargetEntityID;
		public double Damage;
	}

	public class PlayerSpell : IEventMessage
	{
		public int SkillID;
	}
}