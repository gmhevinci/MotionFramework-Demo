using System;
using System.Collections;
using System.Collections.Generic;

namespace MotionGame
{
	internal class NetProtoHandler
	{
		private static Dictionary<ushort, Type> _types = new Dictionary<ushort, Type>();

		public static void RegisterMsgType(ushort msgID, Type protoType)
		{
			// 判断是否重复
			if (_types.ContainsKey(msgID))
			{
				throw new Exception($"Msg {msgID} class {nameof(protoType)} already exist.");
			}

			_types.Add(msgID, protoType);
		}

		public static Type Handle(ushort msgID)
		{
			Type type;
			if (_types.TryGetValue(msgID, out type))
			{
				return type;
			}
			else
			{
				throw new KeyNotFoundException($"Package {msgID} is not define.");
			}
		}
		public static Type TryHandle(ushort msgID)
		{
			Type type;
			_types.TryGetValue(msgID, out type);
			return type;
		}
	}
}