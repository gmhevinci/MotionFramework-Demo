//--自动生成  请勿修改--
//--研发人员实现LANG多语言接口--

using MotionEngine.IO;
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgAutoGenerateLanguageTab : ConfigTab
	{
		public string Lang { protected set; get; }

		public CfgAutoGenerateLanguageTab(int id, string lang)
		{
			Id = id;
			Lang = lang;
		}
	}

	public partial class CfgAutoGenerateLanguage : AssetConfig
	{
		private static CfgAutoGenerateLanguage _instance;
		public static CfgAutoGenerateLanguage Instance
		{
			get
			{
				if (_instance == null) { _instance = new CfgAutoGenerateLanguage(); _instance.Create(); }
				return _instance;
			}
		}

		private CfgAutoGenerateLanguage() { }
		public CfgAutoGenerateLanguageTab GetCfgTab(int key)
		{
			return GetTab(key) as CfgAutoGenerateLanguageTab;
		}
		public void Create()
		{
			AddElement(1401253547, new CfgAutoGenerateLanguageTab(1401253547, "亚历山大"));
			AddElement(1725046866, new CfgAutoGenerateLanguageTab(1725046866, "亚里士多德"));
			AddElement(-842349938, new CfgAutoGenerateLanguageTab(-842349938, "甲"));
			AddElement(-842334729, new CfgAutoGenerateLanguageTab(-842334729, "乙"));
			AddElement(-842334793, new CfgAutoGenerateLanguageTab(-842334793, "丙"));
			AddElement(-565198022, new CfgAutoGenerateLanguageTab(-565198022, "瓦王"));
			AddElement(-1396438525, new CfgAutoGenerateLanguageTab(-1396438525, "伊利丹"));
		}
	}
}
