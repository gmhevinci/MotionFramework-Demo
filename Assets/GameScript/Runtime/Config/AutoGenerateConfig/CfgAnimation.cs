//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

public class CfgAnimationTable : ConfigTable
{
	public string AnimName { protected set; get; }
	public int AnimMode { protected set; get; }
	public float IdleToAnim { protected set; get; }
	public float AnimToIdle { protected set; get; }

	public CfgAnimationTable(int id, string animName, int animMode, float idleToAnim, float animToIdle)
	{
		Id = id;
		AnimName = animName;
		AnimMode = animMode;
		IdleToAnim = idleToAnim;
		AnimToIdle = animToIdle;
	}
}

public partial class CfgAnimation : AssetConfig
{
	private static CfgAnimation _instance;
	public static CfgAnimation Instance
	{
		get
		{
			if (_instance == null) { _instance = new CfgAnimation(); _instance.Create(); }
			return _instance;
		}
	}

	private CfgAnimation() { }
	public CfgAnimationTable GetConfigTable(int key)
	{
		return GetTable(key) as CfgAnimationTable;
	}
	public void Create()
	{
		AddElement(1001, new CfgAnimationTable(1001, "attack", 1, 0.1f, 0.2f));
		AddElement(1002, new CfgAnimationTable(1002, "attack02", 1, 0.1f, 0.2f));
		AddElement(1003, new CfgAnimationTable(1003, "attack03", 1, 0.1f, 0.2f));
		AddElement(1004, new CfgAnimationTable(1004, "defend", 1, 0.1f, 0.2f));
		AddElement(1005, new CfgAnimationTable(1005, "jump", 1, 0.1f, 0.2f));
		AddElement(1006, new CfgAnimationTable(1006, "taunt", 1, 0.1f, 0.2f));
	}
}