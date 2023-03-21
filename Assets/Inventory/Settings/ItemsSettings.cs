using UnityEngine;

public class ItemsSettings : MonoBehaviour
{
	[SerializeField]
	private Inventory.Item[] Items;

	void Start() => Inventory.Items ??= Items;
}
