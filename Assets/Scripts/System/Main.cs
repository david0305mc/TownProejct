using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;

    private void Awake()
    {
        instance = this;
        Items.LoadItems();
        Sprites.LoadSprites();
    }
}
