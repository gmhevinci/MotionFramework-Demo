using System;
using System.Collections;
using System.Collections.Generic;
using MotionEngine;

namespace Hotfix
{
	/// <summary>
	/// 配表数据类
	/// </summary>
	public abstract class ConfigTab
	{
		public int Id { get; protected set; }
	}

	public abstract class AssetConfig
	{
		/// <summary>
		/// 配表数据集合
		/// </summary>
		protected readonly Dictionary<int, ConfigTab> _tabs = new Dictionary<int, ConfigTab>();


		/// <summary>
		/// 添加元素
		/// </summary>
		protected void AddElement(int key, ConfigTab element)
		{
			if (_tabs.ContainsKey(key))
			{
				LogSystem.Log(ELogType.Warning, $"{this.GetType().Name} Element is already exsit, Key is {key}");
				return;
			}

			_tabs.Add(key, element);
		}

		/// <summary>
		/// 获取数据，如果不存在报警告
		/// </summary>
		public ConfigTab GetTab(int key)
		{
			if (_tabs.ContainsKey(key))
			{
				return _tabs[key];
			}
			else
			{
				LogSystem.Log(ELogType.Warning, $"Faild to get tab. File is {this.GetType().Name}, key is {key}");
				return null;
			}
		}

		/// <summary>
		/// 获取数据，如果不存在不会报警告
		/// </summary>
		public bool TryGetTab(int key, out ConfigTab tab)
		{
			return _tabs.TryGetValue(key, out tab);
		}

		/// <summary>
		/// 是否包含Key
		/// </summary>
		public bool ContainsKey(int key)
		{
			return _tabs.ContainsKey(key);
		}

		/// <summary>
		/// 获取所有Key
		/// </summary>
		public List<int> GetKeys()
		{
			List<int> keys = new List<int>();
			foreach (var tab in _tabs)
			{
				keys.Add(tab.Key);
			}
			return keys;
		}
	}
}