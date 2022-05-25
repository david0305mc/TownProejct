using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemRendererScript : MonoBehaviour
{
    /* object refs */
    public BaseItemScript BaseItem;
    public GameObject RenderQuadsContainer;

    private List<RenderQuadScript> _renderQuads;
    private RenderQuadScript _groundPatch;
    public GameData.State oldState;
    public void Init()
    {
        this.Clear();
        this.UpdateRenderQuads();
    }

    public void Refresh()
    {
        if (BaseItem.state != oldState)
        {
            Clear();
            UpdateRenderQuads();
            oldState = BaseItem.state;
        }
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

    public void AddRenderQuad(SpriteCollection.TextureData textureData, int layer)
    {
        if (textureData == null)
        {
            return;
        }

        if (textureData.texture == null)
        {
            return;
        }

        RenderQuadScript renderQuadInstance = GameUtil.CreateRenderQuad();
        renderQuadInstance.transform.SetParent(this.RenderQuadsContainer.transform);

        //POSITIONING AND SCALING
        Vector3 defaultImgSize = new Vector3(1.4142f, 1.4142f, 1.4142f) * 4 * textureData.scale / 100.0f / textureData.parent.gridSize;
        float heightFactor = ((float)textureData.texture.height / (float)textureData.texture.width) * ((float)textureData.numberOfColumns / textureData.numberOfRows);

        float offsetX = (1.414f / 256.0f) * textureData.offsetX * 4 / textureData.parent.gridSize;
        float offsetY = (1.414f / 256.0f) * textureData.offsetY * 4 / textureData.parent.gridSize;

        renderQuadInstance.transform.localPosition = new Vector3(offsetX, offsetY, 0);
        renderQuadInstance.transform.localRotation = Quaternion.Euler(Vector3.zero);
        renderQuadInstance.transform.localScale = new Vector3(defaultImgSize.x, defaultImgSize.x * heightFactor, 1);

        renderQuadInstance.SetData(textureData, layer);
        renderQuadInstance.GetComponent<TextureSheetAnimationScript>().SetTextureSheetData(textureData.numberOfColumns, textureData.numberOfRows, textureData.framesCount, textureData.fps);
        _renderQuads.Add(renderQuadInstance);

        if (layer == 0)
        {
            _groundPatch = renderQuadInstance;
        }
    }

    public void UpdateRenderQuads()
    {
        SpriteCollection.TextureData[] textureDataList = this._GetCurrentImageLayers();
        if (textureDataList != null)
        {
            for (int index = 0; index < textureDataList.Length; index++)
            {
                this.AddRenderQuad(textureDataList[index], index);
            }
        }

        //flip renderer for topleft, bottomleft, leftW
        if (BaseItem.itemData.configuration.isCharacter)
        {
            if (BaseItem.direction == GameData.Direction.BOTTOM_LEFT || BaseItem.direction == GameData.Direction.LEFT || BaseItem.direction == GameData.Direction.TOP_LEFT)
            {
                this.FlipRenderers(true);
            }
            else
            {
                this.FlipRenderers(false);
            }
        }
    }

    private void FlipRenderers(bool value)
    {
        if (value)
        {
            RenderQuadsContainer.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            RenderQuadsContainer.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private SpriteCollection.TextureData[] _GetCurrentImageLayers()
    {
        List<SpriteCollection.TextureData> layers = new List<SpriteCollection.TextureData>();

        GameData.State state = BaseItem.state;
        GameData.Direction direction = BaseItem.direction;

        List<int> spriteIds = BaseItem.itemData.GetSprites(state);
        
        if (spriteIds == null || spriteIds.Count == 0)
        {
            return null;
        }

        for (int index = 0; index < spriteIds.Count; index++)
        {
            SpriteCollection.SpriteData sprite = Sprites.GetSprite(spriteIds[index]);
            layers.Add(sprite.GetTextureData(direction));
        }

        return layers.ToArray();
    }
}
