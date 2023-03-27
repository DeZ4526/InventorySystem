using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Craft
{
	public class CraftUI : MonoBehaviour
	{
		[SerializeField]
		private GameObject CraftUICell;
		[SerializeField]
		private GameObject CraftUIPanel;
		[SerializeField]
		private Transform CraftUICellsPanel;

		private List<GameObject> CraftUICells = new List<GameObject>();

		void Start()
		{
			Inventory.OnShow += Inventory_OnShow;
			Inventory.OnHide += Inventory_OnHide;
		}

		private void Inventory_OnHide()
		{
			CraftUIPanel.SetActive(false);
		}

		private void Inventory_OnShow()
		{
			Reload();
			CraftUIPanel.SetActive(true);
		}

		// Update is called once per frame
		void Reload()
		{
			if (CraftUICell == null) return;
			for (int i = 0; i < CraftUICells.Count; i++)
				Destroy(CraftUICells[i]);
			CraftUICells.Clear();
			CraftSystem.Recipe[] recipes = CraftSystem.GetAvailableRecipes();
			RectTransform tr = CraftUICellsPanel.GetComponent<RectTransform>();
			tr.sizeDelta = new Vector3(0, CraftUICell.GetComponent<RectTransform>().localScale.y * recipes.Length);
			for (int i = 0; i < recipes.Length; i++)
				AddElement(recipes[i]);
		}
		private void AddElement(CraftSystem.Recipe recipe)
		{
			GameObject button = Instantiate(CraftUICell, CraftUICellsPanel);
			button.GetComponent<Button>().onClick.AddListener(delegate { Craft(recipe); });
			button.GetComponent<CraftCellUI>().SetInfo(recipe);
			CraftUICells.Add(button);
		}
		public void Craft(CraftSystem.Recipe recipe)
		{
			CraftSystem.Craft(recipe);
			Reload();
		}
	}
}