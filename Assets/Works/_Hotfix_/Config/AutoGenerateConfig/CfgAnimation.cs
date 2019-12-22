//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgAnimationTab : ConfigTab
	{
		public string AnimName { protected set; get; }
		public int AnimMode { protected set; get; }
		public float IdleToAnim { protected set; get; }
		public float AnimToIdle { protected set; get; }

		public CfgAnimationTab(int id, string animName, int animMode, float idleToAnim, float animToIdle)
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
		public CfgAnimationTab GetCfgTab(int key)
		{
			return GetTab(key) as CfgAnimationTab;
		}
		public void Create()
		{
			AddElement(1001, new CfgAnimationTab(1001, "attack", 1, 0.1f, 0.2f));
			AddElement(1002, new CfgAnimationTab(1002, "attack02", 1, 0.1f, 0.2f));
			AddElement(1003, new CfgAnimationTab(1003, "attack03", 1, 0.1f, 0.2f));
			AddElement(1004, new CfgAnimationTab(1004, "defend", 1, 0.1f, 0.2f));
			AddElement(1005, new CfgAnimationTab(1005, "jump", 1, 0.1f, 0.2f));
			AddElement(1006, new CfgAnimationTab(1006, "taunt", 1, 0.1f, 0.2f));
		}
	}
}
