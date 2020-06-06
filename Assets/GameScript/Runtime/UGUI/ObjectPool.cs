//--------------------------------------------------
// Unity Technology
//--------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.UI
{
	public class ObjectPool<T> where T : new()
	{
		private readonly Stack<T> _stack = new Stack<T>();
		private readonly UnityAction<T> _actionOnGet;
		private readonly UnityAction<T> _actionOnRelease;

		public int CountAll { get; private set; }
		public int CountActive { get { return CountAll - CountInactive; } }
		public int CountInactive { get { return _stack.Count; } }

		public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
		{
			_actionOnGet = actionOnGet;
			_actionOnRelease = actionOnRelease;
		}

		/// <summary>
		/// 从池子里获取一个对象
		/// </summary>
		public T Get()
		{
			T element;
			if (_stack.Count == 0)
			{
				element = new T();
				CountAll++;
			}
			else
			{
				element = _stack.Pop();
			}

			_actionOnGet?.Invoke(element);
			return element;
		}

		/// <summary>
		/// 回收一个对象到池子
		/// </summary>
		public void Release(T element)
		{
			if (_stack.Count > 0 && ReferenceEquals(_stack.Peek(), element))
				throw new System.Exception("Internal error. Trying to destroy object that is already released to pool.");

			_actionOnRelease?.Invoke(element);
			_stack.Push(element);
		}
	}

	/// <summary>
	/// List Pool
	/// </summary>
	internal static class ListPool<T>
	{
		// Object pool to avoid allocations.
		private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, l => l.Clear());

		public static List<T> Get()
		{
			return s_ListPool.Get();
		}

		public static void Release(List<T> toRelease)
		{
			s_ListPool.Release(toRelease);
		}
	}

	/// <summary>
	/// HashSet Pool
	/// </summary>
	internal static class HashSetPool<T>
	{
		// Object pool to avoid allocations.
		private static readonly ObjectPool<HashSet<T>> s_ListPool = new ObjectPool<HashSet<T>>(null, l => l.Clear());

		public static HashSet<T> Get()
		{
			return s_ListPool.Get();
		}

		public static void Release(HashSet<T> toRelease)
		{
			s_ListPool.Release(toRelease);
		}
	}
}