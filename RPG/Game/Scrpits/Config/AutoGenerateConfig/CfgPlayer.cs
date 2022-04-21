//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

	public class CfgPlayerTable : ConfigTable
	{
		public string Name { protected set; get; }
		public int AvatarID { protected set; get; }
		public float BodyRadius { protected set; get; }
		public float MoveSpeed { protected set; get; }
		public string Skills { protected set; get; }
		public double Hp { protected set; get; }
		public double Mp { protected set; get; }
		public double Damage { protected set; get; }
		public double Armor { protected set; get; }

		public CfgPlayerTable(int id, string name, int avatarID, float bodyRadius, float moveSpeed, string skills, double hp, double mp, double damage, double armor)
		{
			Id = id;
			Name = name;
			AvatarID = avatarID;
			BodyRadius = bodyRadius;
			MoveSpeed = moveSpeed;
			Skills = skills;
			Hp = hp;
			Mp = mp;
			Damage = damage;
			Armor = armor;
		}
	}

	public partial class CfgPlayer : AssetConfig
	{
		private static CfgPlayer _instance;
		public static CfgPlayer Instance
		{
			get
			{
				if (_instance == null) { _instance = new CfgPlayer(); _instance.Create(); }
				return _instance;
			}
		}

		private CfgPlayer() { }
		public CfgPlayerTable GetConfigTable(int key)
		{
			return GetTable(key) as CfgPlayerTable;
		}
		public void Create()
		{
			AddElement(1, new CfgPlayerTable(1, LANG.Convert(91216312), 1001, 1.2f, 4f, "10001", 100, 100, 20, 10));
		}
	}