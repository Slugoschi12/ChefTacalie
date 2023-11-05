using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform plateVisualPrifab;

    private List<GameObject> plateVisualGameObjectList;
    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        platesCounter.OnPlateSpawn += PlatesCounter_OnPlateSpawn;
        platesCounter.OnPlateRemove += PlatesCounter_OnPlateRemove;
        
    }

    private void PlatesCounter_OnPlateRemove(object sender, System.EventArgs e)
    {
        GameObject lastPlate = plateVisualGameObjectList[plateVisualGameObjectList.Count -1];
        plateVisualGameObjectList.Remove(lastPlate);
        Destroy(lastPlate);
    }

    private void PlatesCounter_OnPlateSpawn(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrifab,counterTopPoint);
        float plateOffsetY = .1f;
        //modifica pozitia y a farfuriei in functie de cate farfurii s-au spawn-at
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
