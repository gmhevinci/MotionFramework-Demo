//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgSkillTab : ConfigTab
	{
		public string Name { protected set; get; }
		public int AnimationID { protected set; get; }
		public string Sound { protected set; get; }
		public float Life { protected set; get; }
		public float Cd { protected set; get; }
		public int Mp { protected set; get; }
		public float Delay { protected set; get; }
		public float Percent { protected set; get; }
		public float Range { protected set; get; }

		public CfgSkillTab(int id, string name, int animationID, string sound, float life, float cd, int mp, float delay, float percent, float range)
		{
			Id = id;
			Name = name;
			AnimationID = animationID;
			Sound = sound;
			Life = life;
			Cd = cd;
			Mp = mp;
			Delay = delay;
			Percent = percent;
			Range = range;
		}
	}

	public partial class CfgSkill : AssetConfig
	{
		private static CfgSkill _instance;
		public static CfgSkill Instance
		{
			get
			{
				if (_instance == null) { _instance = new CfgSkill(); _instance.Create(); }
				return _instance;
			}
		}

		private CfgSkill() { }
		public CfgSkillTab GetCfgTab(int key)
		{
			return GetTab(key) as CfgSkillTab;
		}
		public void Create()
		{
			AddElement(1001, new CfgSkillTab(1001, LANG.Convert(88134997), 1002, "", 0.9f, 1f, 0, 0.2f, 100f, 2f));
			AddElement(1002, new CfgSkillTab(1002, LANG.Convert(1351039565), 1005, "", 1.2f, 1f, 0, 0.8f, 50f, 4f));
			AddElement(1003, new CfgSkillTab(1003, LANG.Convert(-1257327582), 1004, "", 1f, 5f, 0, 0f, 0f, 0f));
		}
	}
}
