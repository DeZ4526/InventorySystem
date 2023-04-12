using Inventory.Craft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Craft.Editor
{
	[CreateAssetMenu(fileName = "CraftDatabase", menuName = "Inventory/CraftDatabase")]
	public class CraftDatabase : ScriptableObject
	{
		public List<CraftSystem.Recipe> Items = new List<CraftSystem.Recipe>();
	}
}
