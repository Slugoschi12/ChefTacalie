using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSuccess;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipesSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax=4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {   
        Instance = this;
        waitingRecipesSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if(KitchenGameManager.Instance.IsGamePlaying() && waitingRecipesSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipesSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < waitingRecipesSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipesSOList[i];
            //verifica daca nr de ingrediente din farfurie este acelasi ca cel din reteta
            if(waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //has the same nr of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchemnObjectSO in waitingRecipeSO.KitchenObjectSOList)
                {
                    //trece prin toate ingredientele din reteta
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //trece prin toate ingredientele din farfurie

                        if(plateKitchenObjectSO == recipeKitchemnObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound)
                    {
                        // ingredientul din reteta nu a fost gasit pe farfurie
                        plateContentsMatchesRecipe = false;
                    }
                }
                if(plateContentsMatchesRecipe)
                {
                    //player deliver the correct recipe
                    successfulRecipesAmount++;
                    waitingRecipesSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        //no matches found
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipesSOList;
    }
    public int GetSuccesRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
