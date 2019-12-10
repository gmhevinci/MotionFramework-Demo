using System;
using System.ComponentModel;
using Google.Protobuf;

/// <summary>
/// Protobuf帮助类
/// NOTE：这里可以灵活的替换为任何Protobuf库
/// </summary>
public static class ProtobufHelper
{
	public static byte[] Encode(System.Object msgObj)
	{
		Google.Protobuf.IMessage message = (Google.Protobuf.IMessage)msgObj;
		return message.ToByteArray();
	}

	public static System.Object Decode(System.Type type, byte[] msgBytes)
	{
		object message = Activator.CreateInstance(type);
		((Google.Protobuf.IMessage)message).MergeFrom(msgBytes, 0, msgBytes.Length);
		ISupportInitialize iSupportInitialize = message as ISupportInitialize;
		if (iSupportInitialize != null)
			iSupportInitialize.EndInit();
		return message;
	}

	public static System.Object Decode(object msgObj, byte[] msgBytes)
	{
		((Google.Protobuf.IMessage)msgObj).MergeFrom(msgBytes, 0, msgBytes.Length);
		ISupportInitialize iSupportInitialize = msgObj as ISupportInitialize;
		if (iSupportInitialize != null)
			iSupportInitialize.EndInit();
		return msgObj;
	}
}