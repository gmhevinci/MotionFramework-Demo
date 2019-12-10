using System.Collections;
using System.Collections.Generic;
using MotionFramework.IO;

namespace Hotfix
{
	public class SkillWrapper
	{
		public List<float> Params = null;

		public static SkillWrapper Parse(string str)
		{
			// 注意：这里是自定义格式解析的地方
			SkillWrapper wrapper = new SkillWrapper();
			wrapper.Params = StringConvert.StringToParams(str);
			return wrapper;
		}
	}
}
