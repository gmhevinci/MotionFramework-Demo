using MotionGame;

public partial class CfgHero
{
	public static CfgHeroTab GetCfgTab(int key)
	{
		CfgHero cfg = CfgManager.Instance.GetConfig(EConfigType.Hero.ToString()) as CfgHero;
		return cfg.GetTab(key) as CfgHeroTab;
	}
}