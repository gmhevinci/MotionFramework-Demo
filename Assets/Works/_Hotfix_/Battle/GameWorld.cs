using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
	public class GameWorld
	{
		private static int EntityID = 0;

		private readonly List<EntityObject> _entitys = new List<EntityObject>();
		private EntityPlayer _mainPlayer;

		public void Init()
		{
			_mainPlayer = CreatePlayer(EHeroType.Soldier, new Vector3(-6, 0, 0), Vector3.zero);
			if (_mainPlayer != null)
				_entitys.Add(_mainPlayer);

			EntityMonster monster = CreateMonster(1, new Vector3(-10, 0, 0), Vector3.zero);
			if (monster != null)
				_entitys.Add(monster);

			HotfixEventManager.Instance.AddListener(HotfixEventMessageTag.BattleEvent, OnHandleBattleEvent);
		}
		public void Update()
		{
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < _entitys.Count; i++)
			{
				_entitys[i].Update(deltaTime);
			}
		}
		public void Destroy()
		{
			for (int i = 0; i < _entitys.Count; i++)
			{
				_entitys[i].Destroy();
			}
			_entitys.Clear();
		}
		public void OnHandleBattleEvent(IHotfixEventMessage msg)
		{
			if (msg is BattleEvent.CharacterDead)
			{
				BattleEvent.CharacterDead message = msg as BattleEvent.CharacterDead;
				EntityObject entity = GetEntity(message.EntityID);
				if (entity != null)
				{
					entity.HandleEvent(msg);
				}
			}

			if(msg is BattleEvent.PlayerSpell)
			{
				_mainPlayer.HandleEvent(msg);
			}
		}

		/// <summary>
		/// 判定技能伤害
		/// </summary>
		public void ProcessSpellAttack(EntityCharacter source, float range, float percent)
		{
			Vector3 position = source.Root.transform.position;
			for (int i = 0; i < _entitys.Count; i++)
			{
				EntityObject entity = _entitys[i];
				if (entity.EntityID == source.EntityID)
					continue;

				if (entity is EntityCharacter)
				{
					EntityCharacter target = entity as EntityCharacter;
					float distance = Vector3.Distance(position, target.Root.transform.position);
					distance -= target.CharData.BodyRadius;
					if (distance < range)
					{
						// 计算最终伤害值
						double damage = source.CharData.Damage * (percent / 100f) - target.CharData.Armor;
						if (damage < 1)
							damage = 1;

						BattleEvent.DamageHurt msg = new BattleEvent.DamageHurt();
						msg.SourceEntityID = source.EntityID;
						msg.TargetEntityID = target.EntityID;
						msg.Damage = damage;
						target.HandleEvent(msg);
					}
				}
			}
		}

		private EntityObject GetEntity(int entityID)
		{
			for (int i = 0; i < _entitys.Count; i++)
			{
				if (_entitys[i].EntityID == entityID)
					return _entitys[i];
			}
			return null;
		}
		private EntityPlayer CreatePlayer(EHeroType heroType, Vector3 pos, Vector3 rot)
		{
			CfgPlayerTab playerTable = CfgPlayer.Instance.GetCfgTab((int)heroType);
			if (playerTable == null)
				return null;

			EntityPlayer player = new EntityPlayer(++EntityID, heroType);
			player.Create(this, playerTable.AvatarID, pos, rot);
			return player;
		}
		private EntityMonster CreateMonster(int monsterID, Vector3 pos, Vector3 rot)
		{
			CfgMonsterTab monsterTable = CfgMonster.Instance.GetCfgTab(monsterID);
			if (monsterTable == null)
				return null;

			EntityMonster monster = new EntityMonster(++EntityID, monsterID);
			monster.Create(this, monsterTable.AvatarID, pos, rot);
			return monster;
		}
	}
}