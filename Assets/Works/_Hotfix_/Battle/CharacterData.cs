using System.Collections;
using System.Collections.Generic;
using MotionFramework.Utility;

namespace Hotfix
{
	public class CharacterData
	{
		private readonly EntityCharacter _owner;
		private readonly RepeatTimer _tickTimer = new RepeatTimer(0, 3f);
		private double MaxHP;
		private double MaxMP;

		public string CurrentIdleAnimName = "idle"; //角色当前的待机动作名称
		public string CurrentRunAnimName = "run"; //角色当前的跑步动作名称

		public float BodyRadius { private set; get; } = 1f;
		public float MoveSpeed { private set; get; } = 1f;
		public double HP { private set; get; }
		public double MP { private set; get; }
		public double Damage { private set; get; }
		public double Armor { private set; get; }

		/// <summary>
		/// 是否死亡
		/// </summary>
		public bool IsDead { private set; get; } = false;


		public CharacterData(EntityCharacter owner)
		{
			_owner = owner;
		}
		public void InitData(float bodyRadius, float moveSpeed, double hp, double mp, double damage, double armor)
		{
			BodyRadius = bodyRadius;
			MoveSpeed = moveSpeed;
			MaxHP = hp;
			MaxMP = mp;
			HP = hp;
			MP = mp;
			Damage = damage;
			Armor = armor;
		}
		public void Update(float deltaTime)
		{
			if(_tickTimer.Update(deltaTime))
			{
				HP += 1;
				if (HP > MaxHP)
					HP = MaxHP;

				MP += 1;
				if (MP > MaxMP)
					MP = MaxMP;
			}
		}

		public void DamageHurt(double damage)
		{
			HP -= damage;
			if (HP < 0)
			{
				HP = 0;
				if (IsDead == false)
				{
					IsDead = true;

					BattleEvent.CharacterDead msg = new BattleEvent.CharacterDead();
					msg.EntityID = _owner.EntityID;
					HotfixEventManager.Instance.Send(HotfixEventMessageTag.BattleEvent, msg);
				}
			}
		}
		public void SpellMagic(double magic)
		{
			MP -= magic;
			if (MP < 0)
			{
				MP = 0;
			}			
		}
		public bool IsCanRotate()
		{
			return IsDead == false;
		}
		public bool IsCanMove()
		{
			return IsDead == false;
		}
		public bool IsCanSepll()
		{
			return IsDead == false;
		}
	}
}