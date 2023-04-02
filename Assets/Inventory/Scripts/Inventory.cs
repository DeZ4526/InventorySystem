using System;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Editor;

namespace Inventory
{
	public static class Inventory
	{
		static ItemsDatabase Database;

		static Inventory()
		{
			if (Database == null)
			{
				Database = (ItemsDatabase)Resources.Load("ItemsSettings");
				Items = Database.items.ToArray();
				ErrorObject = Database.ErrorObject;
			}
			for (int i = 0; i < Bagpack.Length; i++)
				Bagpack[i] = new Cell();
			for (int i = 0; i < Belt.Length; i++)
				Belt[i] = new Cell();
		}
		public enum ItemType : byte
		{
			None,
			Weapon,
			Food,
			Item
		}

		public static Item[] Items = null;

		public static Cell[] Bagpack = new Cell[20];
		public static Cell[] Belt = new Cell[5];

		public static Cell SelectedItem = new Cell();
		public static GameObject ErrorObject = null;

		public static bool IsOpen { get; private set; }

		public static void Show()
		{
			IsOpen = true;
			OnShow?.Invoke();
		}
		public static void Hide()
		{
			IsOpen = false;
			OnHide?.Invoke();
		}

		public static bool AddItem(Cell cell)
		{
			if (!AddItm(cell, Bagpack))
				return AddItm(cell, Belt);
			return true;

			static bool AddItm(Cell cell, Cell[] cells)
			{
				for (int i = 0; i < cells.Length; i++)
				{
					if (cells[i].Id == -1)
					{
						cells[i].Id = cell.Id;
						cells[i].Num = cell.Num;

						OnAddItem?.Invoke(cell);
						return true;
					}
					else if (cells[i].Id == cell.Id)
					{
						if (Items[cells[i].Id].MaxCol >= cell.Num + cells[i].Num)
						{
							cells[i].Num += cell.Num;
							OnAddItem?.Invoke(cell);
							return true;
						}
						else
						{
							cell.Num = Items[cells[i].Id].MaxCol - cells[i].Num;
							uint temp = Items[cells[i].Id].MaxCol - cells[i].Num;
							cells[i].Num = Items[cells[i].Id].MaxCol;
							OnAddItem?.Invoke(new Cell(cells[i].Id, temp));
						}
					}
				}
				return false;
			}
		}

		public static int Find(uint id)
		{
			if (id >= Items.Length) Debug.LogError("Find error");
			for (int i = 0; i < Bagpack.Length; i++)
				if (Bagpack[i].Id == id) return i;
			return -1;
		}
		public static int Find(string name)
		{
			int id = GetId(name);
			return id < 0 ? -1 : Find((uint)id);
		}

		public static int FindBelt(uint id)
		{
			if (id >= Items.Length) Debug.LogError("Find error");
			for (int i = 0; i < Belt.Length; i++)
				if (Belt[i].Id == id) return i;
			return -1;
		}
		public static int FindBelt(string name)
		{
			int id = GetId(name);
			return id < 0 ? -1 : FindBelt((uint)id);
		}

		public static int[] FindCells(uint id)
		{
			if (id >= Items.Length) Debug.LogError("Find error");
			List<int> result = new List<int>();
			for (int i = 0; i < Bagpack.Length; i++)
				if (Bagpack[i].Id == id) result.Add(i);
			return result.ToArray();
		}
		public static int[] FindCells(string name)
		{
			int id = GetId(name);
			return id < 0 ? null : FindCells((uint)id);
		}


		public static bool Drop(uint cellId) => Drop(cellId, 1);
		public static bool Drop(uint cellId, uint num) => DropObject(cellId, num, Bagpack);
		public static bool DropBelt(uint cellId) => DropBelt(cellId, 1);
		public static bool DropBelt(uint cellId, uint num) => DropObject(cellId, num, Belt);

		private static bool DropObject(uint cellId, uint num, Cell[] cells)
		{
			if (cells.Length <= cellId)
			{
				Debug.LogError("Drop object error\nObject not found");
				return false;
			}
			else if (cells[cellId].Id >= 0)
			{
				if (num > cells[cellId].Num)
				{
					num = cells[cellId].Num;
					Debug.LogError("Drop object error\nThe required quantity is too large");
				}
				Cell dropC = new Cell(cells[cellId].Id, num);
				cells[cellId].Num -= num;
				if (cells[cellId].Num <= 0)
					cells[cellId].Clear();
				OnDropItem?.Invoke(dropC);
				return true;
			}
			else
			{
				Debug.LogError("Drop object error\nObject not found");
				return false;
			}
		}
		public static uint GetNumberItems(uint id, bool belt = true)
		{
			uint result = 0;
			for (int i = 0; i < Bagpack.Length; i++)
				if (Bagpack[i].Id == id)
					result += Bagpack[i].Num;
			if (belt)
			{
				for (int i = 0; i < Belt.Length; i++)
					if (Belt[i].Id == id)
						result += Belt[i].Num;
			}
			return result;
		}

		public static bool ClearCell(uint cellId)
		{
			if (Bagpack.Length < cellId)
			{
				Bagpack[cellId].Clear();
				return true;
			}
			return false;
		}
		public static bool ClearCellBelt(uint cellId)
		{
			if (Belt.Length < cellId)
			{
				Belt[cellId].Clear();
				return true;
			}
			return false;
		}

		public static void SetSelectedItem(Cell cell)
		{
			if (SelectedItem.Id == -1)
			{
				SelectedItem.Id = cell.Id;
				SelectedItem.Num = cell.Num;
				cell.Clear();
			}
			else if (cell.Id == -1)
			{
				cell.Id = SelectedItem.Id;
				cell.Num = SelectedItem.Num;
				SelectedItem.Clear();
			}
			else if (cell.Id == SelectedItem.Id)
			{
				if (cell.Num + SelectedItem.Num <= Items[SelectedItem.Id].MaxCol)
				{
					cell.Id = SelectedItem.Id;
					cell.Num += SelectedItem.Num;
					SelectedItem.Clear();
				}
				else
				{
					SelectedItem.Num = Items[SelectedItem.Id].MaxCol - cell.Num;
					cell.Num = Items[SelectedItem.Id].MaxCol;
				}
			}
			OnChangeSelectedItem?.Invoke(cell);
		}



		private static int GetId(string name)
		{
			for (int i = 0; i < Items.Length; i++) if (Items[i].Name == name) return i;
			return -1;
		}

		public static void ReloadGUI() 
			=> OnGUIReload?.Invoke();

		public delegate void ItemEvent(Cell cell);


		public static event ItemEvent OnDropItem;
		public static event ItemEvent OnAddItem;
		public static event ItemEvent OnChangeSelectedItem;

		public static event Action OnShow;
		public static event Action OnHide;

		public static event Action OnGUIReload;

		[System.Serializable]
		public class Item
		{
			public string Name;
			public Sprite Icon;
			public ItemType Type;
			[SerializeField]
			private GameObject gameObject;
			public GameObject SpawnObject { get => gameObject ?? ErrorObject; set => gameObject = value; }
			public string Description;
			public uint MaxCol;

			public Item(string name, Sprite icon, ItemType type, string description, uint maxCol)
			{
				Name = name;
				Icon = icon;
				Type = type;
				Description = description;
				MaxCol = maxCol;
			}
		}
		[System.Serializable]
		public class Cell
		{
			public int Id;
			public uint Num;
			public Cell()
			{
				Id = -1;
				Num = 0;
			}
			public Cell(int id, uint num)
			{
				Id = id;
				Num = num;
			}
			public void Clear()
			{
				Id = -1;
				Num = 0;
			}
		}
	}
}