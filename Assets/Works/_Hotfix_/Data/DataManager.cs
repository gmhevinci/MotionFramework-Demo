using System.Collections;
using System.Collections.Generic;

namespace Hotfix
{
	public class DataManager
	{
		public static readonly DataManager Instance = new DataManager();

		// 数据集合
		private readonly List<DataBase> _allDatas = new List<DataBase>();

		private DataManager()
		{
		}
		public void Start()
		{
			// 创建所有数据类
			foreach (int v in System.Enum.GetValues(typeof(EDataType)))
			{
				string name = System.Enum.GetName(typeof(EDataType), v);
				System.Type type = HotfixTypeHelper.GetType(name);
				if(type == null)
					throw new System.Exception($"Not found class {name}");
				DataBase instance = (DataBase)System.Activator.CreateInstance(type);
				_allDatas.Add(instance);
			}

			for (int i = 0; i < _allDatas.Count; i++)
			{
				_allDatas[i].Start();
			}
		}
		public void Update()
		{
			for (int i = 0; i < _allDatas.Count; i++)
			{
				_allDatas[i].Update();
			}
		}
		public void ClearAll()
		{
			for (int i = 0; i < _allDatas.Count; i++)
			{
				_allDatas[i].Destroy();
				_allDatas[i].RemoveAllListener();
			}
			_allDatas.Clear();
		}
		
		public void HandleNetMessage(IHotfixNetMessage msg)
		{
			for (int i = 0; i < _allDatas.Count; i++)
			{
				_allDatas[i].OnHandleNetMessage(msg);
			}
		}
	}
}