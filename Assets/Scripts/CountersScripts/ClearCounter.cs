using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
         if(!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);//spawn obiectul din mana pe masa
            }
            else
            {
                //player not carryng anything
            }
        }
         else
        {
            if (player.HasKitchenObject())
            {   //try to get the plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {   //try to get the ingredient
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
                else
                {
                    //player is not carryng Plate but somthing else
                    if(GetKitchenObject().TryGetPlate(out  plateKitchenObject))
                    {   
                        //incearca sa adauge obiectul din mana in reteta
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {   
                            //daca reuseste sa adauge obiectul din mana in lista farfuriei, obiectul din mana se distruge
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
               
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    
}
