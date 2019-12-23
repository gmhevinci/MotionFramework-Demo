//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgUILanguageTab : ConfigTab
	{
		public string Lang { protected set; get; }

		public CfgUILanguageTab(int id, string lang)
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
		public CfgUILanguageTab GetCfgTab(int key)
		{
			return GetTab(key) as CfgUILanguageTab;
		}
		public void Create()
		{
			AddElement("UILogin1".GetHashCode(), new CfgUILanguageTab("UILogin1".GetHashCode(), LANG.Convert(-347589321)));
			AddElement("UISetting1".GetHashCode(), new CfgUILanguageTab("UISetting1".GetHashCode(), LANG.Convert(-487833582)));
			AddElement("UISetting2".GetHashCode(), new CfgUILanguageTab("UISetting2".GetHashCode(), LANG.Convert(1015751757)));
			AddElement("UISetting3".GetHashCode(), new CfgUILanguageTab("UISetting3".GetHashCode(), LANG.Convert(-50860896)));
			AddElement("UISetting4".GetHashCode(), new CfgUILanguageTab("UISetting4".GetHashCode(), LANG.Convert(337257549)));
			AddElement("UISetting5".GetHashCode(), new CfgUILanguageTab("UISetting5".GetHashCode(), LANG.Convert(-384687027)));
			AddElement("UICommon1".GetHashCode(), new CfgUILanguageTab("UICommon1".GetHashCode(), LANG.Convert(107007938)));
		}
	}
}
