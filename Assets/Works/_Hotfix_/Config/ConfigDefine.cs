using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
	public class SkillWrapper
	{
		public List<float> Params = null;

		public static SkillWrapper Parse(string str)
		{
			//TODO 这里是自定义格式解析的地方
			SkillWrapper wrapper = new SkillWrapper();
			wrapper.Params = MotionEngine.IO.StringConvert.StringToParams(str);
			return wrapper;
		}
	}
}
