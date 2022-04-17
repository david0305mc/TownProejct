using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class TestObject : ScriptableObject
{
    public int number;
    public bool toggle;
    public string[] texts;
}
