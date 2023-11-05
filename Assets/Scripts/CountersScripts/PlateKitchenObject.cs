using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> kitchenObjectsSOList;
    [SerializeField]private List<KitchenObjectSO> validKitchenObjectSOList;
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        //pentru dubluri
        if(kitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectsSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
        
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectsSOList;
    }
}
