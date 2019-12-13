using System;

namespace Hotfix
{
	[AttributeUsage(AttributeTargets.Class)]
	public class WindowAttribute : Attribute
	{
		public EWindowType WindowType;
		public EWindowLayer WindowLayer;

		public WindowAttribute(EWindowType type, EWindowLayer layer)
		{
			WindowType = type;
			WindowLayer = layer;
		}
	}
}