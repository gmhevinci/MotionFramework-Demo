using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
	public class GameWorld
	{
		private static int EntityID = 0;

		private readonly List<EntityCharacter> _entitys = new List<EntityCharacter>();

		public void Init()
		{
			EntityPlayer player = new EntityPlayer(++EntityID);
			player.Create("Model/Character/footman_Blue", new Vector3(-6, 0, 0), Vector3.zero);
			_entitys.Add(player);

			EntityCharacter monster = new EntityCharacter(++EntityID);
			monster.Create("Model/Character/footman_Blue", new Vector3(-10, 0, 0), Vector3.zero);
			_entitys.Add(monster);
		}
		public void Update()
		{
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < _entitys.Count; i++)
			{
				_entitys[i].Update(deltaTime);
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
}