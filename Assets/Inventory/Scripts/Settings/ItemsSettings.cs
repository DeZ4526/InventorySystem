using UnityEngine;

namespace Inventory
{
	public class ItemsSettings : MonoBehaviour
	{
		[SerializeField]
		private Inventory.Item[] Items;
		[SerializeField]
		private GameObject errorObject;
		public GameObject ErrorObject { get => errorObject; }

		void Start()
		{
			Inventory.Items ??= Items;
			Inventory.ErrorObject ??= ErrorObject;
		}
	}
}
