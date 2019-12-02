//--自动生成  请勿修改--
//--研发人员实现LANG多语言接口--

using MotionGame;
using MotionEngine.IO;
using System.Collections.Generic;

public class CfgHeroTab : ConfigTab
{
	public bool IsLeader { protected set; get; }
	public ERaceType Race { protected set; get; }
	public string Name { protected set; get; }
	public string Model { protected set; get; }
	public int Age { protected set; get; }
	public long Power { protected set; get; }
	public float Speed { protected set; get; }
	public double Hp { protected set; get; }
	public List<int> SomeIntParams { protected set; get; }
	public List<long> SomeLongParams { protected set; get; }
	public List<float> SomeFloatParams { protected set; get; }
	public List<double> SomeDoubleParams { protected set; get; }
	public List<string> SomeStringParams { protected set; get; }
	public List<string> SomeLanguageParams { protected set; get; }
	public SkillWrapper SkillWrapper { protected set; get; }

	public override void ReadByte(ByteBuffer byteBuf)
	{
		Id = byteBuf.ReadInt();
		IsLeader = byteBuf.ReadBool();
		Race = StringConvert.IndexToEnum<ERaceType>(byteBuf.ReadInt());
		Name =  LANG.Convert(byteBuf.ReadInt());
		Model = byteBuf.ReadUTF();
		Age = byteBuf.ReadInt();
		Power = byteBuf.ReadLong();
		Speed = byteBuf.ReadFloat();
		Hp = byteBuf.ReadDouble();
		SomeIntParams = byteBuf.ReadListInt();
		SomeLongParams = byteBuf.ReadListLong();
		SomeFloatParams = byteBuf.ReadListFloat();
		SomeDoubleParams = byteBuf.ReadListDouble();
		SomeStringParams = byteBuf.ReadListUTF();
		SomeLanguageParams = LANG.Convert(byteBuf.ReadListInt());
		SkillWrapper = SkillWrapper.Parse(byteBuf);
	}
}

[ConfigAttribute(nameof(EConfigType.Hero))]
public partial class CfgHero : AssetConfig
{
	protected override ConfigTab ReadTab(ByteBuffer byteBuffer)
	{
		CfgHeroTab tab = new CfgHeroTab{};
		tab.ReadByte(byteBuffer);
		return tab;
	}
}
