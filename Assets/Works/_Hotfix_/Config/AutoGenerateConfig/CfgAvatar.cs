//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgAvatarTab : ConfigTab
	{
		public string HeadIcon { protected set; get; }
		public string Model { protected set; get; }
		public float ModelScale { protected set; get; }
		public float BuffScale { protected set; get; }
		public float RunAnimationSpeed { protected set; get; }
		public List<string> AttackSounds { protected set; get; }
		public List<string> GetHitSounds { protected set; get; }
		public string DeadSound { protected set; get; }
		public string DeadEffect { protected set; get; }
		public string BornEffect { protected set; get; }
		public float HudPointHeight { protected set; get; }

		public CfgAvatarTab(int id, string headIcon, string model, float modelScale, float buffScale, float runAnimationSpeed, List<string> attackSounds, List<string> getHitSounds, string deadSound, string deadEffect, string bornEffect, float hudPointHeight)
		{
			Id = id;
			HeadIcon = headIcon;
			Model = model;
			ModelScale = modelScale;
			BuffScale = buffScale;
			RunAnimationSpeed = runAnimationSpeed;
			AttackSounds = attackSounds;
			GetHitSounds = getHitSounds;
			DeadSound = deadSound;
			DeadEffect = deadEffect;
			BornEffect = bornEffect;
			HudPointHeight = hudPointHeight;
		}
	}

	public partial class CfgAvatar : AssetConfig
	{
		private static CfgAvatar _instance;
		public static CfgAvatar Instance
		{
			get
			{
				if (_instance == null) { _instance = new CfgAvatar(); _instance.Create(); }
				return _instance;
			}
		}

		private CfgAvatar() { }
		public CfgAvatarTab GetCfgTab(int key)
		{
			return GetTab(key) as CfgAvatarTab;
		}
		public void Create()
		{
			AddElement(1001, new CfgAvatarTab(1001, "", "Model/Character/footman_Blue", 1f, 1f, 4.5f, new List<string>(){"Audio/Footman/Attack"}, new List<string>(){"Audio/Footman/Hit1","Audio/Footman/Hit2","Audio/Footman/Hit3"}, "Audio/Footman/Death", "", "", 1f));
			AddElement(1002, new CfgAvatarTab(1002, "", "Model/Character/footman_Green", 1f, 1f, 4.5f, new List<string>(){"Audio/Footman/Attack"}, new List<string>(){"Audio/Footman/Hit1","Audio/Footman/Hit2","Audio/Footman/Hit3"}, "Audio/Footman/Death", "", "", 1f));
			AddElement(1003, new CfgAvatarTab(1003, "", "Model/Character/footman_Red", 1f, 1f, 4.5f, new List<string>(){"Audio/Footman/Attack"}, new List<string>(){"Audio/Footman/Hit1","Audio/Footman/Hit2","Audio/Footman/Hit3"}, "Audio/Footman/Death", "", "", 1f));
			AddElement(1004, new CfgAvatarTab(1004, "", "Model/Character/footman_Yellow", 2f, 2f, 4.5f, new List<string>(){"Audio/Footman/Attack"}, new List<string>(){"Audio/Footman/Hit1","Audio/Footman/Hit2","Audio/Footman/Hit3"}, "Audio/Footman/Death", "", "", 1f));
		}
	}
}
