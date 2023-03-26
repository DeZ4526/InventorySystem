using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public static class CraftSystem
{
	public static Recipe[] Recipes;

	public static Recipe[] GetAvailableRecipes()
	{
		List<Recipe> result = new List<Recipe>();
		for (int i = 0; i < Recipes.Length; i++)
			if (IsAvailableToCraft(Recipes[i]))
				result.Add(Recipes[i]);
		return result.ToArray();
	}

	public static bool IsAvailableToCraft(Recipe recipe)
	{
		int r = recipe.Input.Length;
		for (int i = 0; i < recipe.Input.Length; i++)
		{
			for (int j = 0; j < Bagpack.Length; j++)
			{
				if (recipe.Input[i].Id == Bagpack[j].Id && recipe.Input[i].Num <= Bagpack[j].Num)
				{
					r--;
					break;
				}
			}
		}
		return r == 0;
	}

	public static bool Craft(Recipe recipe)
	{
		if (IsAvailableToCraft(recipe))
		{
			if (AddItem(recipe.Output))
			{
				for (int i = 0; i < recipe.Input.Length; i++)
				{
					for (int j = 0; j < Bagpack.Length; j++)
					{
						if (recipe.Input[i].Id == Bagpack[j].Id && recipe.Input[i].Num <= Bagpack[j].Num)
						{
							Bagpack[j].Num -= recipe.Input[i].Num;
							if (Bagpack[j].Num <= 0) Bagpack[j].Clear();
							break;
						}
					}
				}
				OnCraft?.Invoke(recipe);
				return true;
			}
			else return false;

		}
		else return false;
	}

	public delegate void ItemEvent(Recipe recipe);

	public static event ItemEvent OnCraft;
	[System.Serializable]
	public class Recipe
	{
		public string Name;
		public string Description;
		[SerializeField]
		public Cell Output;
		[SerializeField]
		public Cell[] Input;
	}
}
