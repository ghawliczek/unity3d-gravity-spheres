using System;
using UnityEngine;

[Serializable]
public class Pool
{
    public string Tag;
    public GameObject Prefab;
    public int Size;
    public bool Expandable;
}