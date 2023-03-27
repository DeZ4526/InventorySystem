using UnityEngine;

namespace Inventory
{
	public class InvPlayer : MonoBehaviour
	{
		[SerializeField]
		private Camera m_Camera;
		[SerializeField]
		private Transform Paws;
		private Transform[] childPaws;
		private uint itemIntPaws = 0;

		void Start()
		{
			Inventory.OnDropItem += Inventory_OnDropItem;
			childPaws = new Transform[Paws.childCount];
			for (int i = 0; i < Paws.childCount; i++)
				childPaws[i] = Paws.GetChild(i);
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
			else if (Input.GetKeyUp(KeyCode.G))
			{
				Inventory.DropBelt(itemIntPaws);
				GetOnPaws(Inventory.Belt[itemIntPaws]);
			}
			else
				for (int i = 1; i < 10; i++)
					if (Input.GetKeyDown(i.ToString()))
						if (i <= Inventory.Belt.Length)
						{
							GetOnPaws(Inventory.Belt[i - 1]);
							itemIntPaws = (uint)i - 1;
						}
		}
		public void GetOnPaws(Inventory.Cell cell)
		{

			if (cell.Id > Inventory.Items.Length || cell.Id < 0)
			{
				for (int i = 0; i < childPaws.Length; i++)
					childPaws[i].gameObject.SetActive(false);
				return;
			}
			for (int i = 0; i < childPaws.Length; i++)
				childPaws[i].gameObject.SetActive(
					childPaws[i].name == Inventory.Items[cell.Id].Name);

		}
	}
}