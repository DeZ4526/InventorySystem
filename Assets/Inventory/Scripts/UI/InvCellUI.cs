using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
	public class InvCellUI : MonoBehaviour
	{
		private Inventory.Cell cell;
		[SerializeField]
		private Image Ico;
		[SerializeField]
		private Text Num;

		public void Click()
		{
			if (Inventory.IsOpen) Inventory.SetSelectedItem(cell);
		}

		public void SetInfo(Inventory.Cell cell)
		{
			this.cell = cell;
			if (cell != null && cell.Id >= 0 && cell.Id < Inventory.Items.Length && cell.Num > 0)
			{
				Ico.sprite = Inventory.Items[cell.Id].Icon;
				Num.text = cell.Num.ToString();
			}
			else
			{
				Ico.sprite = null;
				Num.text = "0";
			}
		}
	}
}