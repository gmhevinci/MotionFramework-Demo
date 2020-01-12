using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Utility;
using MotionFramework.Audio;

namespace Hotfix
{
	public class CharacterSkill
	{
		#region Skill
		private class Skill
		{
			private readonly EntityCharacter _owner;
			private readonly CfgSkillTab _skillTable;
			private readonly CfgAnimationTab _animTable;
			private readonly OnceTimer _delayTimer;
			private bool _isSpelling = false;
			private float _lifeTimer = 0;
			private float _cdTimer = 0;

			public int SkillID
			{
				get
				{
					return _skillTable.Id;
				}
			}

			public Skill(EntityCharacter owner, int skillID)
			{
				_owner = owner;
				_skillTable = CfgSkill.Instance.GetCfgTab(skillID);
				_delayTimer = new OnceTimer(_skillTable.Delay);

				// 初始化技能动画
				if (_skillTable.AnimationID > 0)
				{
					_animTable = CfgAnimation.Instance.GetCfgTab(_skillTable.AnimationID);
					_owner.CharAnim.InitAnimState(_animTable.AnimName, CharacterAnimation.EAnimationLayer.SkillLayer, (WrapMode)_animTable.AnimMode);
				}
			}
			public void Update(float deltaTime)
			{
				_cdTimer -= deltaTime;

				if (_isSpelling == false)
					return;

				// 伤害判定
				if (_delayTimer.Update(deltaTime))
				{
					if (_skillTable.Range > 0 && _skillTable.Percent > 0)
					{
						_owner.World.ProcessSpellAttack(_owner, _skillTable.Range, _skillTable.Percent);
					}
				}

				_lifeTimer += deltaTime;
				if (_lifeTimer > _skillTable.Life)
					Forbid();
			}

			public void Spell()
			{
				_isSpelling = true;
				_cdTimer = _skillTable.Cd;

				// 消耗魔法
				_owner.CharData.SpellMagic(_skillTable.Mp);

				// 播放技能动画
				if (_animTable != null)
					_owner.CharAnim.PlayAnim(_animTable.AnimName, _animTable.IdleToAnim);
			}
			public void Forbid()
			{
				_delayTimer.Reset();
				_isSpelling = false;
				_lifeTimer = 0;

				// 播放Idle动画
				if (_animTable != null)
				{
					if (_owner.CharMove.IsMoving)
						_owner.CharAnim.PlayAnim(_owner.CharData.CurrentRunAnimName, _animTable.AnimToIdle);
					else
						_owner.CharAnim.PlayAnim(_owner.CharData.CurrentIdleAnimName, _animTable.AnimToIdle);
				}
			}
			public bool CheckCD()
			{
				return _cdTimer > 0.001f;
			}
			public bool CheckMP()
			{
				return _owner.CharData.MP >= _skillTable.Mp;
			}
			public bool IsLife()
			{
				return _isSpelling;
			}
		}
		#endregion

		private readonly EntityCharacter _owner;
		private readonly List<Skill> _skills = new List<Skill>();

		public CharacterSkill(EntityCharacter owner)
		{
			_owner = owner;
		}
		public void Update(float deltaTime)
		{
			for (int i = 0; i < _skills.Count; i++)
			{
				_skills[i].Update(deltaTime);
			}
		}
		public void Spell(int skillID)
		{
			if (_owner.CharData.IsCanSepll() == false)
				return;

			Skill skill = GetSkill(skillID);

			// 注意：如果不存在则创建技能
			if (skill == null)
			{
				skill = new Skill(_owner, skillID);
				_skills.Add(skill);
			}

			// 检测条件
			if (IsAnyLife())
				return;
			if (skill.CheckCD())
				return;
			if (skill.CheckMP() == false)
				return;

			// 技能释放
			skill.Spell();

			// 随机播放角色攻击音效
			string soundName = _owner.Avatar.GetRandomAttackSound();
			if (string.IsNullOrEmpty(soundName) == false)
				AudioManager.Instance.PlaySound(soundName);
		}
		public void ForbidAll()
		{
			for (int i = 0; i < _skills.Count; i++)
			{
				_skills[i].Forbid();
			}
		}
		public bool IsAnyLife()
		{
			for (int i = 0; i < _skills.Count; i++)
			{
				if (_skills[i].IsLife())
					return true;
			}
			return false;
		}

		private Skill GetSkill(int skillID)
		{
			for (int i = 0; i < _skills.Count; i++)
			{
				if (_skills[i].SkillID == skillID)
					return _skills[i];
			}
			return null;
		}
		private bool IsContains(int skillID)
		{
			for (int i = 0; i < _skills.Count; i++)
			{
				if (_skills[i].SkillID == skillID)
					return true;
			}
			return false;
		}
	}
}