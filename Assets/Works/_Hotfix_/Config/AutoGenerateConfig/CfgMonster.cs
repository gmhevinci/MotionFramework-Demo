//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgMonsterTab : ConfigTab
	{
		public string Name { protected set; get; }
		public int AvatarID { protected set; get; }
		public float DestroyTime { protected set; get; }
		public float BodyRadius { protected set; get; }
		public float MoveSpeed { protected set; get; }
		public string Skills { protected set; get; }
		public float AiGuardRange { protected set; get; }
		public float AiFightRange { protected set; get; }
		public double Hp { protected set; get; }
		public double Mp { protected set; get; }
		public double Damage { protected set; get; }
		public double Armor { protected set; get; }

		public CfgMonsterTab(int id, string name, int avatarID, float destroyTime, float bodyRadius, float moveSpeed, string skills, float aiGuardRange, float aiFightRange, double hp, double mp, double damage, double armor)
		{
			Id = id;
			Name = name;
			AvatarID = avatarID;
			DestroyTime = destroyTime;
			BodyRadius = bodyRadius;
			MoveSpeed = moveSpeed;
			Skills = skills;
			AiGuardRange = aiGuardRange;
			AiFightRange = aiFightRange;
			Hp = hp;
			Mp = mp;
			Damage = damage;
			Armor = armor;
		}
	}

	public partial class CfgMonster : AssetConfig
	{
		private static CfgMonster _instance;
		public static CfgMonster Instance
		{
			get
			{
				if (_instance == null) { _instance = new CfgMonster(); _instance.Create(); }
				return _instance;
			}
		}

		private CfgMonster() { }
		public CfgMonsterTab GetCfgTab(int key)
		{
			return GetTab(key) as CfgMonsterTab;
		}
		public void Create()
		{
			AddElement(1, new CfgMonsterTab(1, LANG.Convert(178940420), 1002, 10f, 1.2f, 4f, "10001", 8f, 20f, 100, 0, 20, 0));
			AddElement(2, new CfgMonsterTab(2, LANG.Convert(178979727), 1003, 10f, 1.2f, 4f, "10001", 8f, 20f, 200, 0, 10, 10));
			AddElement(3, new CfgMonsterTab(3, LANG.Convert(1421729370), 1004, 30f, 2.4f, 4f, "10001", 8f, 20f, 1000, 0, 10, 10));
		}
	}
}
