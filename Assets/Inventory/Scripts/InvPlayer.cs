using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvPlayer : MonoBehaviour
{
	[SerializeField]
	private Camera m_Camera;

	void Start()
	{
		Inventory.OnDropItem += Inventory_OnDropItem;
	}

	private void Inventory_OnDropItem(Inventory.Cell cell)
	{
		GameObject spawnObject = Inventory.Items[cell.Id].SpawnObject;
		GameObject IsSpawn = Instantiate(spawnObject, transform.position, transform.rotation);
		if (IsSpawn.GetComponent<InvItem>())
		{
			InvItem item = IsSpawn.GetComponent<InvItem>();
			item.item.Id = cell.Id;
			item.item.Num = cell.Num;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(InvSettingsKey.TakeItem))
		{
			RaycastHit hi = new RaycastHit();

			if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hi, 5))
			{
				GameObject hitGM = hi.collider.gameObject;
				if (hitGM.GetComponent<InvItem>())
					if (Inventory.AddItem(hitGM.GetComponent<InvItem>().item))
						Destroy(hitGM);
			}
		}
		else if (Input.GetKeyUp(KeyCode.N))
		{
			Inventory.Drop(0);
		}
	}
}