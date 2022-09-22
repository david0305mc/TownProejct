using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil : MonoBehaviour
{
    public static T MakeGameObject<T>(string objName, Transform parent) where T : MonoBehaviour
    {
        return MakeGameObject(objName, parent).AddComponent<T>();
    }

    public static GameObject MakeGameObject(string objName, Transform parent)
    {   
        var temp = new GameObject(objName);
        temp.transform.SetParent(parent);
        return temp;

    }
    public static RenderQuadScript CreateRenderQuad()
    {
        return MUtilities.CreateInstance(Game.SceneManager.instance.RenderQuad, Game.SceneManager.instance.ItemsContainer, true).GetComponent<RenderQuadScript>();
    }

    public static Vector2 GetScreenPosition(Vector3 position)
    {
        Vector3 screenPos = CameraManager.instance.MainCamera.WorldToScreenPoint(position);
        return screenPos;
    }

    public static float ClockwiseAngleOf3Points(Vector2 A, Vector2 B, Vector2 C)
    {

        Vector2 v1 = A - B;
        Vector2 v2 = C - B;

        var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x) * -1;
        float angle = Vector2.Angle(v1, v2) * sign;

        if (angle < 0)
        {
            angle = 360 + angle;
        }

        return angle;
    }
}
