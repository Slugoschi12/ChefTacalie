using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]//apare in meniu ca obiect scriptabil
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
