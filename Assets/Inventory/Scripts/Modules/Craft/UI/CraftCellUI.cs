using UnityEngine;
using UnityEngine.UI;
using static Inventory.Craft.CraftSystem;

namespace Inventory.Craft
{
	public class CraftCellUI : MonoBehaviour
	{
		[SerializeField]
		private Text Name;
		[SerializeField]
		private Text Description;

		[SerializeField]
		private Transform inputCells;
		[SerializeField]
		private GameObject inputCell;

		public void SetInfo(CraftSystem.Recipe recipe)
		{
			if (Name != null) Name.text = recipe.Name;
			if (Description != null) Description.text = recipe.Description;

			if (inputCells != null && inputCell != null)
			{
				RectTransform tr = inputCells.GetComponent<RectTransform>();
				tr.sizeDelta = new Vector3(inputCell.GetComponent<RectTransform>().localScale.x * recipe.Input.Length, 0);
				for (int i = 0; i < recipe.Input.Length; i++)
				{
					GameObject button = Instantiate(inputCell, inputCells);
					button.GetComponent<InvCellUI>().SetInfo(recipe.Input[i]);
				}
			}

		}
	}
}