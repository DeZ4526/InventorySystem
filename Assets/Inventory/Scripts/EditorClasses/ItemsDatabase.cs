using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Editor
{
	[CreateAssetMenu(fileName = "ItemsSettings", menuName = "Inventory/ItemsSettings")]
	public class ItemsDatabase : ScriptableObject
	{
		[SerializeField]
		public List<Inventory.Item> items = new List<Inventory.Item>();
		[SerializeField]
		private GameObject errorObject;

		public GameObject ErrorObject { get => errorObject; }
	}
}