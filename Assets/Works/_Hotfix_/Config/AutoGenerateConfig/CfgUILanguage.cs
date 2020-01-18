//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgUILanguageTable : ConfigTable
	{
		public string Lang { protected set; get; }

		public CfgUILanguageTable(int id, string lang)
		{
			Id = id;
			Lang = lang;
		}
	}

	public partial class CfgUILanguage : AssetConfig
	{
		private static CfgUILanguage _instance;
		public static CfgUILanguage Instance
		{
			get
			{
				if (_instance == null) { _instance = new CfgUILanguage(); _instance.Create(); }
				return _instance;
			}
		}

		private CfgUILanguage() { }
		public CfgUILanguageTable GetConfigTable(int key)
		{
			return GetTable(key) as CfgUILanguageTable;
		}
		public void Create()
		{
			AddElement("UILogin1".GetHashCode(), new CfgUILanguageTable("UILogin1".GetHashCode(), LANG.Convert(-347589321)));
			AddElement("UISetting1".GetHashCode(), new CfgUILanguageTable("UISetting1".GetHashCode(), LANG.Convert(-487833582)));
			AddElement("UISetting2".GetHashCode(), new CfgUILanguageTable("UISetting2".GetHashCode(), LANG.Convert(1015751757)));
			AddElement("UISetting3".GetHashCode(), new CfgUILanguageTable("UISetting3".GetHashCode(), LANG.Convert(-50860896)));
			AddElement("UISetting4".GetHashCode(), new CfgUILanguageTable("UISetting4".GetHashCode(), LANG.Convert(337257549)));
			AddElement("UISetting5".GetHashCode(), new CfgUILanguageTable("UISetting5".GetHashCode(), LANG.Convert(-384687027)));
			AddElement("UICommon1".GetHashCode(), new CfgUILanguageTable("UICommon1".GetHashCode(), LANG.Convert(107007938)));
		}
	}
}
