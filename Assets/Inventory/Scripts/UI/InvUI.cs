using UnityEngine;
using static Inventory.Inventory;

namespace Inventory
{
	public class InvUI : MonoBehaviour
	{
		[SerializeField]
		private GameObject Cell;
		[SerializeField]
		private GameObject InventoryUi;
		[SerializeField]
		private Transform InventoryUiCellsPanel;
		[SerializeField]
		private Transform InventoryUiCellsPanelBelt;

		private InvCellUI SelectedItem;

		private InvCellUI[] cellsBagpack;
		private InvCellUI[] cellsBelt;

		void Start()
		{
			ReloadUI();
			OnShow += Inventory_OnShow;
			OnHide += Inventory_OnHide;
			OnChangeSelectedItem += InvUI_OnChangeSelectedItem;
			OnAddItem += InvUI_OnAddItem;
			OnDropItem += InvUI_OnDropItem;
			OnGUIReload += Inventory_OnGUIReload;
		}

		private void Inventory_OnGUIReload()
		{
			if (IsOpen)
			{
				for (int i = 0; i < Bagpack.Length; i++)
					cellsBagpack[i].SetInfo(Bagpack[i]);
			}
			for (int i = 0; i < Belt.Length; i++)
				cellsBelt[i].SetInfo(Belt[i]);
		}

		private void InvUI_OnDropItem(Cell cell)
		{
			ReloadGUI();
		}

		private void InvUI_OnAddItem(Cell cell)
		{
			ReloadGUI();
		}

		private void InvUI_OnChangeSelectedItem(Cell cell)
		{
			ReloadGUI();
			if (Inventory.SelectedItem.Id != -1)
			{
				SelectedItem = Instantiate(Cell, InventoryUi.transform).GetComponent<InvCellUI>();
				SelectedItem.SetInfo(Inventory.SelectedItem);
			}
			else if (SelectedItem != null) Destroy(SelectedItem.gameObject);
		}

		private void Inventory_OnHide()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			InventoryUi.SetActive(false);
		}

		private void Inventory_OnShow()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
			InventoryUi.SetActive(true);
			ReloadGUI();
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(InvSettingsKey.InvOpen))
				if (!IsOpen) Show();
				else Hide();
			if (IsOpen && SelectedItem != null)
				SelectedItem.transform.position = Input.mousePosition + new Vector3(51, -51);
		}

		private void ReloadUI()
		{
			if (Cell == null) Debug.LogError("InvUI : Please set Cell");
			if (InventoryUiCellsPanel == null) Debug.LogError("InvUI : Please set InventoryUiCellsPanel");
			if (Cell == null || InventoryUiCellsPanel == null) return;
			cellsBagpack = new InvCellUI[Bagpack.Length];
			cellsBelt = new InvCellUI[Belt.Length];

			for (int i = 0; i < Bagpack.Length; i++)
				cellsBagpack[i] = Instantiate(Cell, InventoryUiCellsPanel).GetComponent<InvCellUI>();
			for (int i = 0; i < Belt.Length; i++)
				cellsBelt[i] = Instantiate(Cell, InventoryUiCellsPanelBelt).GetComponent<InvCellUI>();
			ReloadGUI();
		}

	}
}