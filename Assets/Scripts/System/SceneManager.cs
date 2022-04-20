using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
	// prefab
	public GameObject BaseItem;
    public Material RenderQuadMaterial;

    // object ref
    public GameObject ItemsContainer;

	private Dictionary<int, BaseItemScript> _itemInstances;


	private void Awake()
    {
        instance = this;
		_itemInstances = new Dictionary<int, BaseItemScript>();
		GroundManager.instance.UpdateAllNodes();

    }

	private int _GetUnusedInstanceId()
	{
		int instanceId = Random.Range(10000, 99999);
		if (this._itemInstances.ContainsKey(instanceId))
		{
			return _GetUnusedInstanceId();
		}
		return instanceId;
	}

	public BaseItemScript AddItem(int itemId, bool immediate, bool ownedItem)
	{
		int posX = 0;
		int posZ = 0;
		if (!immediate)
		{
			ItemsCollection.ItemData itemData = Items.GetItem(itemId);
			Vector3 freePosition = GroundManager.instance.GetRandomFreePositionForItem(itemData.gridSize, itemData.gridSize);
			posX = (int)freePosition.x;
			posZ = (int)freePosition.z;
		}
		return this.AddItem(itemId, -1, posX, posZ, immediate, ownedItem);
	}
	/// <summary>
	/// Adds the item with itemId. where itemId is the id which we registered with item prefab as unique.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="itemId">Item identifier.</param>
	public BaseItemScript AddItem(int itemId, int instanceId, int posX, int posZ, bool immediate, bool ownedItem)
	{
		BaseItemScript instance = MUtilities.CreateInstance(this.BaseItem, this.ItemsContainer, true).GetComponent<BaseItemScript>();

		if (instanceId == -1)
		{
			instanceId = this._GetUnusedInstanceId();
		}

		instance.instanceId = instanceId;
		this._itemInstances.Add(instanceId, instance);

		instance.SetItemData(itemId, posX, posZ);
		instance.SetState(GameData.State.IDLE);

		//		GroundManager.Cell freeCell = GroundManager.instance.GetRandomFreeCellForItem (instance);
		//		instance.SetPosition (GroundManager.instance.CellToPosition (freeCell));

		//if (!instance.itemData.configuration.isCharacter)
		//{
		//	GroundManager.instance.UpdateBaseItemNodes(instance, GroundManager.Action.ADD);
		//}

		//instance.ownedItem = ownedItem;
		return instance;
	}
}
