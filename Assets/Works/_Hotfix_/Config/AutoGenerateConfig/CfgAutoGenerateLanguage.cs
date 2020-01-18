//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgAutoGenerateLanguageTable : ConfigTable
	{
		public string Lang { protected set; get; }

		public CfgAutoGenerateLanguageTable(int id, string lang)
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
		public CfgAutoGenerateLanguageTable GetConfigTable(int key)
		{
			return GetTable(key) as CfgAutoGenerateLanguageTable;
		}
		public void Create()
		{
			AddElement(178940420, new CfgAutoGenerateLanguageTable(178940420, "蓝色步兵"));
			AddElement(178979727, new CfgAutoGenerateLanguageTable(178979727, "红色步兵"));
			AddElement(1421729370, new CfgAutoGenerateLanguageTable(1421729370, "步兵队长"));
			AddElement(91216312, new CfgAutoGenerateLanguageTable(91216312, "战士"));
			AddElement(88134997, new CfgAutoGenerateLanguageTable(88134997, "普通攻击"));
			AddElement(1351039565, new CfgAutoGenerateLanguageTable(1351039565, "跳跃"));
			AddElement(-1257327582, new CfgAutoGenerateLanguageTable(-1257327582, "盾牌防守"));
			AddElement(-347589321, new CfgAutoGenerateLanguageTable(-347589321, "进入游戏"));
			AddElement(-487833582, new CfgAutoGenerateLanguageTable(-487833582, "设置"));
			AddElement(1015751757, new CfgAutoGenerateLanguageTable(1015751757, "音量"));
			AddElement(-50860896, new CfgAutoGenerateLanguageTable(-50860896, "开关"));
			AddElement(337257549, new CfgAutoGenerateLanguageTable(337257549, "音乐"));
			AddElement(-384687027, new CfgAutoGenerateLanguageTable(-384687027, "音效"));
			AddElement(107007938, new CfgAutoGenerateLanguageTable(107007938, "确定"));
		}
	}
}
