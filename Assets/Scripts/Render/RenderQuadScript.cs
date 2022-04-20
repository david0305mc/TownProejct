using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQuadScript : MonoBehaviour
{
    public MeshFilter MeshFilter { get; set; }
    public MeshRenderer MeshRenderer { get; set; }

    public void SetData(SpriteCollection.TextureData textureData, int layer)
    {
        MeshRenderer.material = Sprites.GetTextureMaterial(textureData.texture, textureData.parent.renderingLayer, textureData.parent.renderingOrder);
    }

}
