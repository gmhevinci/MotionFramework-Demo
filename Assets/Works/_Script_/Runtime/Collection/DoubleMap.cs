//--------------------------------------------------
// ET Framework
// Copyright (c) 2018 tanghai
//--------------------------------------------------
using System;
using System.Collections.Generic;

public class DoubleMap<K, V>
{
	private readonly Dictionary<K, V> _kv = new Dictionary<K, V>();
	private readonly Dictionary<V, K> _vk = new Dictionary<V, K>();

	public DoubleMap()
	{
	}
	public DoubleMap(int capacity)
	{
		_kv = new Dictionary<K, V>(capacity);
		_vk = new Dictionary<V, K>(capacity);
	}

	public void ForEach(Action<K, V> action)
	{
		if (action == null)
		{
			return;
		}
		Dictionary<K, V>.KeyCollection keys = _kv.Keys;
		foreach (K key in keys)
		{
			action(key, _kv[key]);
		}
	}

	public List<K> Keys
	{
		get
		{
			return new List<K>(_kv.Keys);
		}
	}
	public List<V> Values
	{
		get
		{
			return new List<V>(_vk.Keys);
		}
	}

	public void Add(K key, V value)
	{
		if (key == null || value == null || _kv.ContainsKey(key) || _vk.ContainsKey(value))
		{
			return;
		}
		_kv.Add(key, value);
		_vk.Add(value, key);
	}

	public V GetValueByKey(K key)
	{
		if (key != null && _kv.ContainsKey(key))
		{
			return _kv[key];
		}
		return default(V);
	}
	public K GetKeyByValue(V value)
	{
		if (value != null && _vk.ContainsKey(value))
		{
			return _vk[value];
		}
		return default(K);
	}

	public void RemoveByKey(K key)
	{
		if (key == null)
		{
			return;
		}
		V value;
		if (!_kv.TryGetValue(key, out value))
		{
			return;
		}

		_kv.Remove(key);
		_vk.Remove(value);
	}
	public void RemoveByValue(V value)
	{
		if (value == null)
		{
			return;
		}

		K key;
		if (!_vk.TryGetValue(value, out key))
		{
			return;
		}

		_kv.Remove(key);
		_vk.Remove(value);
	}

	public void Clear()
	{
		_kv.Clear();
		_vk.Clear();
	}

	public bool ContainsKey(K key)
	{
		if (key == null)
		{
			return false;
		}
		return _kv.ContainsKey(key);
	}
	public bool ContainsValue(V value)
	{
		if (value == null)
		{
			return false;
		}
		return _vk.ContainsKey(value);
	}
	public bool Contains(K key, V value)
	{
		if (key == null || value == null)
		{
			return false;
		}
		return _kv.ContainsKey(key) && _vk.ContainsKey(value);
	}
}