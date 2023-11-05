using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    [SerializeField] private CuttingResiceSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);//spawn obiectul din mana pe masa
                    cuttingProgress = 0;
                    CuttingResiceSO cuttingResiceSO = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = (float)cuttingProgress / cuttingResiceSO.cuttingProgressMax
                    });
                }
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
            }
            else
            {

                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        
    }
    public override void InteractAlternate(Player player)
    {   //verifica daca obiectul de pe cutting poate fi taiat 
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {

            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingResiceSO cuttingResiceSO = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingResiceSO.cuttingProgressMax
            });
            if (cuttingProgress >= cuttingResiceSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }   
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchcenObjectSO)
    {
        CuttingResiceSO cuttingObjectOutput = GetCuttingRecipeWithInput(inputKitchcenObjectSO);
        return cuttingObjectOutput != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {   
        CuttingResiceSO cuttingObjectOutput = GetCuttingRecipeWithInput(inputKitchenObjectSO);
        if (cuttingObjectOutput != null)
        {
            return cuttingObjectOutput.output;
        }else
            return null;
    }
    private CuttingResiceSO GetCuttingRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingResiceSO cuttingResiceSO in cuttingRecipeSOArray)
        {
            if (cuttingResiceSO.input == inputKitchenObjectSO)
                return cuttingResiceSO;
        }
        return null;
    }
}
