using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO platekitchenObjectSO;
    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlateRemove;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(KitchenGameManager.Instance.IsGamePlaying() && spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            
            if(plateSpawnAmount < plateSpawnAmountMax)
            {
                plateSpawnAmount++;
                OnPlateSpawn?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (plateSpawnAmount > 0)
            {
                plateSpawnAmount--;
                KitchenObject.SpawnKitchenObject(platekitchenObjectSO, player);
                OnPlateRemove?.Invoke(this, EventArgs.Empty);   
            }

        }
    }
}
