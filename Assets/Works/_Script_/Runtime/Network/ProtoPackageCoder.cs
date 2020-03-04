using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Network;

public class ProtoPackageCoder : DefaultNetworkPackageCoder
{
	public ProtoPackageCoder()
	{
		PackageSizeFieldType = EPackageSizeFieldType.UShort;
		MessageIDFieldType = EMessageIDFieldType.UShort;
	}

	protected override byte[] EncodeInternal(object msgObj)
	{
		return ProtobufHelper.Encode(msgObj);
	}

	protected override object DecodeInternal(Type classType, byte[] bodyBytes)
	{
		return ProtobufHelper.Decode(classType, bodyBytes);
	}
}