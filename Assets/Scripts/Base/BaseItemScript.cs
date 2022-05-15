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

	public ItemsCollection.ItemData itemData;

    private Vector3 deltaDistance;
    private bool isStart = false;

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
        this.SetSize(Vector3.one * itemData.gridSize);
		this.SetPosition(new Vector3(posX, 0, posZ));
        this.UpdateConnectedItems();
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
            //if (this.connectedItems.Count > 0)
            //{
            //    builder = this.connectedItems[0];
            //}

            if (builder == null)
            {
                builder = Game.SceneManager.instance.AddItem(8420, true, false);

                builder.SetPosition(transform.localPosition = new Vector3(0, 0, 0));
                //builder.SetPosition(this.GetRandomFrontCellPosition());

                //connect builder item to the builder hut
                //this.connectedItems.Add(builder);

                //connect this builder hut item to builder
                //builder.connectedItems.Add(this);
            }

            //builder.ReturnBuilder();
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
}
