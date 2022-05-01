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
