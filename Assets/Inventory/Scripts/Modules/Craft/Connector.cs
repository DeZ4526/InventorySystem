using UnityEngine;

namespace Inventory.Craft
{
	public static class Connector
	{
		public static void Init()
		{
			CraftSystem.OnCraft += CraftSystem_OnCraft;
		}

		private static void CraftSystem_OnCraft(CraftSystem.Recipe recipe)
		{
			Inventory.ReloadGUI();
		}
	}
}