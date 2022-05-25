using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class SceneManager : MonoBehaviour
	{
		private int _swordMan_ID = 3068;
		//private int _archer_ID = 5492;

		public static SceneManager instance;
		// prefab
		public GameObject BaseItem;
		public Material RenderQuadMaterial;
		public GameObject RenderQuad;

		// object ref
		public GameObject ItemsContainer;
	

		private Dictionary<int, BaseItemScript> _itemInstances;
		private BaseItemScript selectedItem;

		public Dictionary<int, BaseItemScript> GetItemInstances()
		{
			return _itemInstances;
		}

		private void Awake()
		{
			instance = this;
			_itemInstances = new Dictionary<int, BaseItemScript>();
			GroundManager.instance.UpdateAllNodes();
            CameraManager.instance.OnTapItemAction += OnTapItem;
			CameraManager.instance.OnTapGroundAction += OnTapGround;

        }
        private void OnDestroy()
        {
            CameraManager.instance.OnTapItemAction -= OnTapItem;
			CameraManager.instance.OnTapGroundAction -= OnTapGround;
		}

        private void Start()
        {
            Init();    
        }

        private void Init()
        {
            LoadUserScene();   
        }

        private void LoadUserScene()
        {
			var sceneData =	DatabaseManager.Instance.GetScene();
			foreach (var item in sceneData.items)
			{
				AddItem(item.itemId, item.instanceId, item.posX, item.posZ, true, true);
			}
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

		public BaseItemScript AddItem(int itemId, int instanceID, bool immediate, bool ownedItem)
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
			return this.AddItem(itemId, instanceID, posX, posZ, immediate, ownedItem);
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

        private void OnTapItem(CameraManager.CameraEvent evt)
        {
			if (selectedItem != null)
			{
				selectedItem.SetSelected(false);
			}
			selectedItem = evt.baseItem;
			selectedItem.SetSelected(true);

        }

		private void OnTapGround(CameraManager.CameraEvent evt)
		{
			if (selectedItem != null)
			{
				selectedItem.SetSelected(false);
				selectedItem = null;
			}

			evt.point.x = Mathf.Clamp(evt.point.x, 0, GroundManager.nodeWidth - 1);
			evt.point.z  = Mathf.Clamp(evt.point.z, 0, GroundManager.nodeHeight - 1);
			var unit = AddItem(_swordMan_ID, -1, true, true);
			unit.SetPosition(evt.point);
			unit.Attacker.AttackNearestTarget();

		}
	}

}
