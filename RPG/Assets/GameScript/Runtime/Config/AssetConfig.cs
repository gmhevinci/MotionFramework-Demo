using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 配表数据类
/// </summary>
public abstract class ConfigTable
{
	public int Id { get; protected set; }
}

public abstract class AssetConfig
{
	/// <summary>
	/// 配表数据集合
	/// </summary>
	protected readonly Dictionary<int, ConfigTable> _tables = new Dictionary<int, ConfigTable>();


	/// <summary>
	/// 添加元素
	/// </summary>
	protected void AddElement(int key, ConfigTable element)
	{
		if (_tables.ContainsKey(key))
		{
			GameLog.Warning($"{this.GetType().Name} Element is already exsit, Key is {key}");
			return;
		}

		_tables.Add(key, element);
	}

	/// <summary>
	/// 获取数据，如果不存在报警告
	/// </summary>
	public ConfigTable GetTable(int key)
	{
		if (_tables.ContainsKey(key))
		{
			return _tables[key];
		}
		else
		{
			GameLog.Warning($"Faild to get tab. File is {this.GetType().Name}, key is {key}");
			return null;
		}
	}

	/// <summary>
	/// 获取数据，如果不存在不会报警告
	/// </summary>
	public bool TryGetTable(int key, out ConfigTable tab)
	{
		return _tables.TryGetValue(key, out tab);
	}

	/// <summary>
	/// 是否包含Key
	/// </summary>
	public bool ContainsKey(int key)
	{
		return _tables.ContainsKey(key);
	}

	/// <summary>
	/// 获取所有Key
	/// </summary>
	public List<int> GetKeys()
	{
		List<int> keys = new List<int>();
		foreach (var tab in _tables)
		{
			keys.Add(tab.Key);
		}
		return keys;
	}
}