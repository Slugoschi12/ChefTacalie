using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]//afiseaza lista de structuri in inspector
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;//referinta catre farfurie
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectsList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectsList)
        {
                kitchenObjectSOGameObject.gameObject.SetActive(false);
           
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
      foreach(KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectsList)
        {//verificam daca kitchenObjectSOGameObject se potriveste cu cel primit in event
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {   //da enable la obiectul primit
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
