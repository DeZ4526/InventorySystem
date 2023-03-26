using UnityEngine;

public class CraftSettings : MonoBehaviour
{
	[SerializeField]
	private CraftSystem.Recipe[] Items;
	
	void Start() => CraftSystem.Recipes ??= Items;
}