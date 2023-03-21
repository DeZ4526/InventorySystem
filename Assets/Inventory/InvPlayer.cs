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
		
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.E))
		{

		}
	}
}