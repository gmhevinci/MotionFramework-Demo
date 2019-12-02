//--自动生成  请勿修改--
//--研发人员实现LANG多语言接口--

using MotionGame;
using MotionEngine.IO;
using System.Collections.Generic;

public class CfgAutoGenerateLanguageTab : ConfigTab
{
	public string Lang { protected set; get; }

	public override void ReadByte(ByteBuffer byteBuf)
	{
		Id = byteBuf.ReadInt();
		Lang = byteBuf.ReadUTF();
	}
}

[ConfigAttribute(nameof(EConfigType.AutoGenerateLanguage))]
public partial class CfgAutoGenerateLanguage : AssetConfig
{
	protected override ConfigTab ReadTab(ByteBuffer byteBuffer)
	{
		CfgAutoGenerateLanguageTab tab = new CfgAutoGenerateLanguageTab{};
		tab.ReadByte(byteBuffer);
		return tab;
	}
}
