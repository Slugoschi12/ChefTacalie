using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static CutingCounter;

public class StoveCounter : BaseCounter,IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurnedRecipeSO[] burnedRecipeSOArray;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {   
        Idle,
        Frying,
        Fried,
        Burned
    }
    private float fryingTimer;
    private float burnTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurnedRecipeSO burningRecipeSO;
    private State state;
    private void Start()
    {
        state= State.Idle;  
    }
    private void Update()
    {   
        
        if(HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                    {
                        //fride
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burnTimer = 0f;
                        burningRecipeSO = GetburningRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burnTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burnTimer / burningRecipeSO.burningTimerMax
                    });
                    if (burnTimer >= burningRecipeSO.burningTimerMax)
                    {
                        //fride
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        burnTimer = 0f;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
            
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //cara ceva ce poate fi prajit
                    player.GetKitchenObject().SetKitchenObjectParent(this);//spawn obiectul din mana pe masa
                    fryingRecipeSO = GetFrayingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
                    progressNormalized = fryingTimer/fryingRecipeSO.fryingTimerMax});
                }
            }
            else{/*player not carryng anything*/}
        }
        else
        {
            if (!player.HasKitchenObject())
            {//nu are nimic in mana/Ia ce este pe stove
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });

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
                            state = State.Idle;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = state
                            });
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = 0f
                            });
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
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchcenObjectSO)
    {
        FryingRecipeSO fryingObjectOutput = GetFrayingRecipeWithInput(inputKitchcenObjectSO);
        return fryingObjectOutput != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingObjectOutput = GetFrayingRecipeWithInput(inputKitchenObjectSO);
        if (fryingObjectOutput != null)
        {
            return fryingObjectOutput.output;
        }
        else
            return null;
    }
    private FryingRecipeSO GetFrayingRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingResiceSO in fryingRecipeSOArray)
        {
            if (fryingResiceSO.input == inputKitchenObjectSO)
                return fryingResiceSO;
        }
        return null;
    }
    private BurnedRecipeSO GetburningRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurnedRecipeSO burningResiceSO in burnedRecipeSOArray)
        {
            if (burningResiceSO.input == inputKitchenObjectSO)
                return burningResiceSO;
        }
        return null;
    }
}
