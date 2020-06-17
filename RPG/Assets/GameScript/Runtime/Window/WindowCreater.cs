using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Utility;

internal class WindowCreater
{
	private readonly Dictionary<EWindowType, Type> _types = new Dictionary<EWindowType, Type>();
	private string _assemblyName;

	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="assemblyName">窗口类所在的程序集</param>
	public void Initialize(string assemblyName)
	{
		if (string.IsNullOrEmpty(assemblyName))
			throw new ArgumentNullException();
		_assemblyName = assemblyName;

		List<Type> types = AssemblyUtility.GetAssignableAttributeTypes(assemblyName, typeof(UIWindow), typeof(WindowAttribute));
		for (int i = 0; i < types.Count; i++)
		{
			System.Type type = types[i];
			WindowAttribute attribute = Attribute.GetCustomAttribute(type, typeof(WindowAttribute)) as WindowAttribute;

			// 判断是否重复
			if (_types.ContainsKey(attribute.WindowType))
				throw new Exception($"Window type {attribute.WindowType} already exist.");

			// 添加到集合
			_types.Add(attribute.WindowType, type);
		}
	}

	/// <summary>
	/// 创建类的实例
	/// </summary>
	/// <param name="windowType">窗口类型</param>
	/// <returns>如果类型没有在程序集里定义会发生异常</returns>
	public UIWindow CreateInstance(EWindowType windowType)
	{
		UIWindow window = null;
		if (_types.TryGetValue(windowType, out Type type))
		{
			WindowAttribute attribute = Attribute.GetCustomAttribute(type, typeof(WindowAttribute)) as WindowAttribute;
			window = (UIWindow)Activator.CreateInstance(type);
			window.Init(attribute.WindowType, attribute.WindowLayer);
		}

		if (window == null)
			throw new KeyNotFoundException($"{nameof(UIWindow)} {windowType} is not define in assembly {_assemblyName}");

		return window;
	}
}