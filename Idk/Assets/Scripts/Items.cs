using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemData", menuName="Item")]
public class Items : ScriptableObject
{
    public string name;
    public Sprite icon;
    public int amount;
    public bool isCollected = false;

    public void Use() { }
}
