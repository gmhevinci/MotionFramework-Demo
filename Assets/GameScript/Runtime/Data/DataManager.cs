using System.Collections;
using System.Collections.Generic;
using MotionFramework;

public class DataManager : ModuleSingleton<DataManager>, IModule
{
	private readonly List<DataBase> _allDatas = new List<DataBase>();

	void IModule.OnCreate(object createParam)
	{
		// 创建所有数据类
		foreach (int v in System.Enum.GetValues(typeof(EDataType)))
		{
			string name = System.Enum.GetName(typeof(EDataType), v);
			System.Type type = System.Type.GetType(name);
			if (type == null)
				throw new System.Exception($"Not found class {name}");
			DataBase instance = (DataBase)System.Activator.CreateInstance(type);
			_allDatas.Add(instance);
		}

		for (int i = 0; i < _allDatas.Count; i++)
		{
			_allDatas[i].OnCreate();
		}
	}
	void IModule.OnUpdate()
	{
		for (int i = 0; i < _allDatas.Count; i++)
		{
			_allDatas[i].OnUpdate();
		}
	}
	void IModule.OnGUI()
	{
	}

	public void ClearAll()
	{
		for (int i = 0; i < _allDatas.Count; i++)
		{
			_allDatas[i].OnDestroy();
			_allDatas[i].ClearEventGroup();
		}
		_allDatas.Clear();
	}
}