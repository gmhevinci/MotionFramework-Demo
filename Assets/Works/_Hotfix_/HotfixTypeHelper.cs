using System.Collections;
using System.Collections.Generic;
using System;

namespace Hotfix
{
	public static class HotfixTypeHelper
	{
		public static T GetAttribute<T>(System.Type type) where T : Attribute
		{
			var attributeArray = type.GetCustomAttributes(typeof(T), false);
			return attributeArray[0] as T;
		}

		public static Type GetType(string typeName)
		{
			List<Type> types = ILRManager.Instance.HotfixAssemblyTypes;
			for (int i = 0; i < types.Count; i++)
			{
				System.Type type = types[i];
				if (type.Name == typeName)
					return type;
			}
			return null;
		}
	}
}