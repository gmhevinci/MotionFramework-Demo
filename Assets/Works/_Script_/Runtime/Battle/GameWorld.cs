using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld
{
	private readonly List<EntityCharacter> _entitys = new List<EntityCharacter>();

	public void Init()
	{
		EntityPlayer player = new EntityPlayer();
		player.Create(new Vector3(-6, 0, 0), Vector3.zero);
		_entitys.Add(player);

		EntityCharacter monster = new EntityCharacter();
		monster.Create(new Vector3(-10, 0, 0), Vector3.zero);
		_entitys.Add(monster);
	}
	public void Update()
	{
		for(int i=0; i< _entitys.Count; i++)
		{
			_entitys[i].Update();
		}
	}
	public void Destroy()
	{
		for (int i = 0; i < _entitys.Count; i++)
		{
			_entitys[i].Destroy();
		}
		_entitys.Clear();
	}
}