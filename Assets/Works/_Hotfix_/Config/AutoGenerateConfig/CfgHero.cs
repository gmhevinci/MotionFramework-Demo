//--自动生成  请勿修改--
//--研发人员实现LANG多语言接口--

using MotionEngine.IO;
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgHeroTab : ConfigTab
	{
		public bool IsLeader { protected set; get; }
		public ERaceType Race { protected set; get; }
		public string Name { protected set; get; }
		public string Model { protected set; get; }
		public int Age { protected set; get; }
		public long Power { protected set; get; }
		public float Speed { protected set; get; }
		public double Hp { protected set; get; }
		public List<int> SomeIntParams { protected set; get; }
		public List<long> SomeLongParams { protected set; get; }
		public List<float> SomeFloatParams { protected set; get; }
		public List<double> SomeDoubleParams { protected set; get; }
		public List<string> SomeStringParams { protected set; get; }
		public List<string> SomeLanguageParams { protected set; get; }
		public SkillWrapper SkillWrapper { protected set; get; }

		public CfgHeroTab(int id, bool isLeader, ERaceType race, string name, string model, int age, long power, float speed, double hp, List<int> someIntParams, List<long> someLongParams, List<float> someFloatParams, List<double> someDoubleParams, List<string> someStringParams, List<string> someLanguageParams, SkillWrapper skillWrapper)
		{
			Id = id;
			IsLeader = isLeader;
			Race = race;
			Name = name;
			Model = model;
			Age = age;
			Power = power;
			Speed = speed;
			Hp = hp;
			SomeIntParams = someIntParams;
			SomeLongParams = someLongParams;
			SomeFloatParams = someFloatParams;
			SomeDoubleParams = someDoubleParams;
			SomeStringParams = someStringParams;
			SomeLanguageParams = someLanguageParams;
			SkillWrapper = skillWrapper;
		}
	}

	public partial class CfgHero : AssetConfig
	{
		private static CfgHero _instance;
		public static CfgHero Instance
		{
			get
			{
				if (_instance == null) { _instance = new CfgHero(); _instance.Create(); }
				return _instance;
			}
		}

		private CfgHero() { }
		public CfgHeroTab GetCfgTab(int key)
		{
			return GetTab(key) as CfgHeroTab;
		}
		public void Create()
		{
			AddElement(1001, new CfgHeroTab(1001, true, (ERaceType)1, LANG.Convert(-565198022), "Model/model1", 30, 99999, 7.654f, 9876543.123, new List<int>(){1,2,3}, new List<long>(){9999,8888,7777}, new List<float>(){1.1f,2.2f,3.3f}, new List<double>(){9876543.123,9876543.123}, new List<string>(){"hello","nihao","anning"}, new List<string>(){LANG.Convert(-842349938),LANG.Convert(-842334729),LANG.Convert(-842334793)}, SkillWrapper.Parse("对周围{5}米范围内敌人，造成{100}点伤害")));
			AddElement(1002, new CfgHeroTab(1002, false, (ERaceType)2, LANG.Convert(-1396438525), "Model/model2", 999, 99999, 7.654f, 9876543.123, new List<int>(){1,2,3}, new List<long>(){9999,8888,7777}, new List<float>(){1.1f,2.2f,3.3f}, new List<double>(){9876543.123,9876543.123}, new List<string>(){"hello","nihao","anning"}, new List<string>(){LANG.Convert(-842349938),LANG.Convert(-842334729),LANG.Convert(-842334793)}, SkillWrapper.Parse("对周围{5}米范围内敌人，造成{100}点伤害")));
		}
	}
}
