using System.Collections;
using System.Collections.Generic;
using MotionEngine.IO;

public class SkillWrapper
{
	public List<float> Params = null;

	public static SkillWrapper Parse(ByteBuffer buffer)
	{
		string content = buffer.ReadUTF();
		SkillWrapper wrapper = new SkillWrapper();
		wrapper.Params = StringConvert.StringToParams(content);
		return wrapper;
	}
}