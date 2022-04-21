using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Event;
using MotionFramework.Window;

[Window((int)EWindowLayer.Panel, false)]
sealed class UIMain : CanvasWindow
{
	public override void OnCreate()
	{
		AddButtonListener("UIMain/Meum/Button (Settings)", OnClickSetting);
		AddButtonListener("UIMain/Meum/Button (Shop)", OnClikShop);
		AddButtonListener("UIMain/Spell/Button (Attack)", OnClickSkill1);
		AddButtonListener("UIMain/Spell/Button (Shield)", OnClickSkill2);
		AddButtonListener("UIMain/Spell/Button (Swords)", OnClickSkill3);
	}
	public override void OnDestroy()
	{
	}
	public override void OnRefresh()
	{
	}
	public override void OnUpdate()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Alpha1))
			OnClickSkill1();
		if (Input.GetKeyDown(KeyCode.Alpha2))
			OnClickSkill2();
		if (Input.GetKeyDown(KeyCode.Alpha3))
			OnClickSkill3();
#endif
	}

	private void OnClickSetting()
	{
		UITools.OpenWindow<UISetting>();
	}
	private void OnClikShop()
	{
	}
	private void OnClickSkill1()
	{
		BattleEvent.PlayerSpell msg = new BattleEvent.PlayerSpell
		{
			SkillID = 1001
		};
		EventManager.Instance.SendMessage(msg);
	}
	private void OnClickSkill2()
	{
		BattleEvent.PlayerSpell msg = new BattleEvent.PlayerSpell
		{
			SkillID = 1003
		};
		EventManager.Instance.SendMessage(msg);
	}
	private void OnClickSkill3()
	{
		BattleEvent.PlayerSpell msg = new BattleEvent.PlayerSpell
		{
			SkillID = 1002
		};
		EventManager.Instance.SendMessage(msg);
	}
}