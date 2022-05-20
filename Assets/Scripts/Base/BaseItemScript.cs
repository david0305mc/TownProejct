using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemScript : MonoBehaviour
{
    public int instanceId;
    public GameData.State state;
	public GameData.Direction direction;
	public BaseItemRendererScript Renderer;

    public WalkerScript Walker;
    public ItemsCollection.ItemData itemData;
    public List<BaseItemScript> connectedItems;

    private Vector3 deltaDistance;
    private bool isStart = false;

    private Dictionary<float, GameData.Direction> _angleToDirectionMap;

    private void Awake()
    {
        this._angleToDirectionMap = new Dictionary<float, GameData.Direction>();
        this._angleToDirectionMap.Add(0, GameData.Direction.BOTTOM_RIGHT);
        this._angleToDirectionMap.Add(51, GameData.Direction.BOTTOM);
        this._angleToDirectionMap.Add(110, GameData.Direction.BOTTOM_LEFT);
        this._angleToDirectionMap.Add(153, GameData.Direction.LEFT);
        this._angleToDirectionMap.Add(190, GameData.Direction.TOP_LEFT);
        this._angleToDirectionMap.Add(220, GameData.Direction.TOP);
        this._angleToDirectionMap.Add(290, GameData.Direction.TOP_RIGHT);
        this._angleToDirectionMap.Add(357, GameData.Direction.RIGHT);
    }
    public void OnItemDragStart(Vector3 pos)
    {

        deltaDistance = transform.localPosition - pos;
        transform.localPosition = new Vector3(Mathf.Floor(pos.x), 0, Mathf.Floor(pos.z));
        isStart = true;
    }

    public void OnItemDrag(Vector3 pos)
    {
        if (!isStart)
            return;
        
        var point = deltaDistance + pos;
        //var point = pos;
        Renderer.transform.localPosition = new Vector3(Mathf.Floor(point.x), 0, Mathf.Floor(point.z));
        Debug.Log($"transform.localPosition {transform.localPosition}");
    }

	public void SetItemData(int itemId, int posX, int posZ)
	{
		this.itemData = Items.GetItem(itemId);
		this.gameObject.name = itemData.name + " [INSTANCE]";
        this.Renderer.Init();
        this.Walker.SetData(this);
        connectedItems = new List<BaseItemScript>();
        this.SetSize(Vector3.one * itemData.gridSize);
		this.SetPosition(new Vector3(posX, 0, posZ));
        this.UpdateConnectedItems();
	}

    public void SetSelected(bool value)
    {
        UpdateConnectedItems();
        if (value)
        {
            MPopupManager.Inst.ShowItemOptionUI();
        }
        else
        {
            MPopupManager.Inst.CloseAllWindow();
        }
    }

    private void UpdateConnectedItems()
    {
        //add builder to builder hut
        if (this.itemData.name == "BuilderHut")
        {
            //if (Game.SceneManager.instance.selectedItem == this)
            //{
            //    //that means the hut is on drag
            //    //builder comes to hut only after on stop drag
            //    return;
            //}

            BaseItemScript builder = null;
            if (this.connectedItems.Count > 0)
            {
                builder = this.connectedItems[0];
            }

            if (builder == null)
            {
                builder = Game.SceneManager.instance.AddItem(8420, true, false);

                builder.SetPosition(transform.localPosition = new Vector3(0, 0, 0));
                //builder.SetPosition(this.GetRandomFrontCellPosition());

                //connect builder item to the builder hut
                this.connectedItems.Add(builder);

                //connect this builder hut item to builder
                builder.connectedItems.Add(this);
            }

            builder.ReturnBuilder();
        }
    }

    public void SetSize(Vector3 size)
	{
		this.transform.localScale = size;
	}

	private void SetPosition(Vector3 position)
	{
		this.transform.localPosition = position;
	}
	/// <summary>
	/// Sets the state.
	/// </summary>
	/// <param name="state">State.</param>
	public void SetState(GameData.State state)
	{
		if (state != this.state)
		{
			this.state = state;
		}
	}

    public Vector3 GetPosition()
    {
        return this.transform.localPosition;
    }
    public Vector3 GetSize()
    {
        return new Vector3(this.transform.localScale.x, 0, this.transform.localScale.z);
    }
    public Vector3[] GetFrontCells()
    {
        int sizeX = (int)this.GetSize().x;
        int size = 2 * sizeX + 1;
        Vector3[] cells = new Vector3[size];
        int index = 0;

        for (int x = 0; x <= sizeX; x++)
        {
            for (int z = 0; z <= sizeX; z++)
            {
                if (x == sizeX || z == sizeX)
                {
                    Vector3 cellPos = this.GetPosition() + new Vector3(x, 0, z);
                    if (cellPos.x < 0 || cellPos.x >= GroundManager.nodeWidth || cellPos.z < 0 || cellPos.z >= GroundManager.nodeHeight)
                    {
                        //avoid cells out of the grid
                        continue;
                    }
                    cells[index] = cellPos;
                    index++;
                }
            }
        }

        return cells;
    }

    public Vector3 GetRandomFrontCellPosition()
    {
        Vector3[] frontCells = GetFrontCells();
        return frontCells[UnityEngine.Random.Range(0, frontCells.Length - 1)];
    }

    public void ReturnBuilder()
    {
        if (itemData.name == "Builder")
        {
            Walker.WalkToPosition(connectedItems[0].GetRandomFrontCellPosition());
            Walker.OnFinishWalk += OnFinishWalkReturnBuilder;
        }
    }

    private void OnFinishWalkReturnBuilder()
    {
        this.SetState(GameData.State.IDLE);
        this.Walker.OnFinishWalk -= OnFinishWalkReturnBuilder;
    }

    public void LookAt(Vector3 point)
    {
        Vector2 a = GameUtil.GetScreenPosition(this.transform.position - new Vector3(0, 0, 1));
        Vector2 b = GameUtil.GetScreenPosition(this.transform.position);
        Vector2 c = GameUtil.GetScreenPosition(point);

        float angle = GameUtil.ClockwiseAngleOf3Points(a, b, c);
        this.SetAngle(angle);
    }
    public void SetAngle(float angle)
    {
        GameData.Direction direction = GameData.Direction.BOTTOM_RIGHT;
        float minAnge = 999;
        foreach (KeyValuePair<float, GameData.Direction> entry in this._angleToDirectionMap)
        {
            float a = Mathf.Abs(angle - entry.Key);
            if (a < minAnge)
            {
                minAnge = a;
                direction = entry.Value;
            }
        }
        this.SetDirection(direction);
    }
    public void SetDirection(GameData.Direction direction)
    {
        this.direction = direction;
        this.UpdateConnectedItems();
    }

}
