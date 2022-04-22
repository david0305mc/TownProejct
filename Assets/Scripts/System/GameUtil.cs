using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil : MonoBehaviour
{
    public static RenderQuadScript CreateRenderQuad()
    {
        return MUtilities.CreateInstance(Game.SceneManager.instance.RenderQuad, Game.SceneManager.instance.ItemsContainer, true).GetComponent<RenderQuadScript>();
    }

}
