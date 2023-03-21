using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using static Inventory;

public static class Inventory
{
	public enum ItemType : byte
	{

	}
	public static Item[] Items = null;

	public static Cell[] Bagpack = new Cell[15];
	public static Cell[] Belt = new Cell[5];

	public static Cell SelectedItem = new Cell();

	public static void Show() { }
	public static void Hide() { }
	
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
			OnDropItem?.Invoke(new Cell(cells[cellId].Id, num));
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
		if(Bagpack.Length < cellId)
		{
			Bagpack[cellId].Id = -1;
			Bagpack[cellId].Num = 0;
			return true;
		}
		return false;
	}
	public static bool ClearCellBelt(uint cellId) 
	{
		if (Belt.Length < cellId)
		{
			Belt[cellId].Id = -1;
			Belt[cellId].Num = 0;
			return true;
		}
		return false;
	}

	public static void SetSelectedItem(uint cellId) 
	{ 
		
	}
	public static void SetSelectedItemBelt(uint cellId) { }

	public static void SetCellFromSelecteItem(uint cellId) { }
	public static void SetCellFromSelecteItemBelt(uint cellId) { }


	private static int GetId(string name)
	{
		for (int i = 0; i < Items.Length; i++) if (Items[i].Name == name) return i;
		return -1;
	}

	public delegate void ItemEvent(Cell cell);

	public static event ItemEvent OnDropItem;
	public static event ItemEvent OnTakeItem;
	public static event ItemEvent OnAddItem;
	public static event ItemEvent OnChangeSelectedItem;

	public class Item
	{ 
		public readonly string Name;
		public readonly Sprite Icon;
		public readonly ItemType Type;
		public readonly string Description;
		public readonly uint MaxCol;

		public Item(string name, Sprite icon, ItemType type, string description, uint maxCol)
		{
			Name = name;
			Icon = icon;
			Type = type;
			Description = description;
			MaxCol = maxCol;
		}
	}
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
	}
}
