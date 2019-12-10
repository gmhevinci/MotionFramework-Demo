using MotionFramework.Config;

public partial class CfgHero
{
	public static CfgHeroTab GetCfgTab(int key)
	{
		CfgHero cfg = ConfigManager.Instance.GetConfig(EConfigType.Hero.ToString()) as CfgHero;
		return cfg.GetTab(key) as CfgHeroTab;
	}
}