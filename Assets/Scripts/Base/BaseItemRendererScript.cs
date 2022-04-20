using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemRendererScript : MonoBehaviour
{
    /* object refs */
    public BaseItemScript BaseItem;
    public GameObject RenderQuadsContainer;

    private List<RenderQuadScript> _renderQuads;
    public void Init()
    {
        this.Clear();
        this.UpdateRenderQuads();
    }

    public void Clear()
    {
        if (this._renderQuads != null)
        {
            foreach (RenderQuadScript renderQuad in this._renderQuads)
            {
                Destroy(renderQuad.gameObject);
            }
        }
        this._renderQuads = new List<RenderQuadScript>();
    }

    public void UpdateRenderQuads()
    {
        //SpriteCollection.TextureData[] textureDataList = this._GetCurrentImageLayers();
        //if (textureDataList != null)
        //{
        //    for (int index = 0; index < textureDataList.Length; index++)
        //    {
        //        this.AddRenderQuad(textureDataList[index], index);
        //    }
        //}

        ////flip renderer for topleft, bottomleft, left
        //if (BaseItem.itemData.configuration.isCharacter)
        //{
        //    if (BaseItem.direction == Common.Direction.BOTTOM_LEFT || BaseItem.direction == Common.Direction.LEFT || BaseItem.direction == Common.Direction.TOP_LEFT)
        //    {
        //        this._FlipRenderers(true);
        //    }
        //    else
        //    {
        //        this._FlipRenderers(false);
        //    }
        //}
    }
}
