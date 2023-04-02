using Inventory.Editor;
using UnityEditor;
using UnityEngine;
using static Inventory.Inventory;

public class ItemsSettingsWindow : EditorWindow
{
	[SerializeField] 
	private ItemsDatabase Database;
	
	private int SelectedItemId = -1;
	private Vector2 scrollPosition;
	
	private static EditorWindow window;
	
	[MenuItem("Inventory/Items")]
	static void Init()
	{
		window = GetWindow<ItemsSettingsWindow>();
		window.titleContent = new GUIContent("Items settings");
	}

	public void Awake() 
		=> Database = (ItemsDatabase)Resources.Load("ItemsSettings");
	void OnGUI()
	{
		if (Database == null) return;
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(250), GUILayout.Height(window.position.height));
		for (int i = 0; i < Database.items.Count; i++)
			if (GUILayout.Button(Database.items[i].Name, GUILayout.Height(40)))
				SelectedItemId = i;
		
		if (GUILayout.Button("(Add)", GUILayout.Height(60)))
			Database.items.Add(new Item("New item " + Database.items.Count, null, ItemType.None, "", 64));
		EditorGUILayout.EndScrollView();

		if(SelectedItemId != -1 && SelectedItemId < Database.items.Count)
		{
			Database.items[SelectedItemId].Icon = (Sprite)EditorGUI.ObjectField(new Rect(256, 5, 120, 120), "", Database.items[SelectedItemId].Icon, typeof(Sprite), false);

			GUI.Label(new Rect(380, 0, window.position.width - 370, 30), "Name : ");
			Database.items[SelectedItemId].Name = GUI.TextArea(new Rect(380, 30, window.position.width - 385, 30), Database.items[SelectedItemId].Name);

			GUI.Label(new Rect(380, 60, window.position.width - 370, 30), "Description : ");
			Database.items[SelectedItemId].Description = GUI.TextArea(new Rect(380, 90, window.position.width - 385, 35), Database.items[SelectedItemId].Description);

			Database.items[SelectedItemId].SpawnObject = (GameObject)EditorGUI.ObjectField(new Rect(256, 130, window.position.width - 261, 30), "Spawn object : ", Database.items[SelectedItemId].SpawnObject, typeof(GameObject), false);

			GUI.Label(new Rect(256, 160, window.position.width - 250, 30), "Maximum quantity: ");
			Database.items[SelectedItemId].MaxCol = (uint)EditorGUI.IntSlider(new Rect(256, 190, window.position.width - 261, 30), (int)Database.items[SelectedItemId].MaxCol, 1, 1000);

			GUI.Label(new Rect(256, 220, window.position.width - 250, 30), "Item type: ");
			Database.items[SelectedItemId].Type = (ItemType)EditorGUI.EnumPopup(new Rect(256, 250, window.position.width - 261, 30), Database.items[SelectedItemId].Type);
			if(GUI.Button(new Rect(256, 280, window.position.width - 261, 30), "(DELETE)"))
				Database.items.RemoveAt(SelectedItemId);
		}
		else
		{
			EditorGUI.HelpBox(new Rect(256, 5, window.position.width - 261, window.position.height - 10), "Select item in the left list", MessageType.Info);
		}
	}
}