using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private float volume = 1f;
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagarer_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CutingCounter.OnAnyCut += CutingCounter_OnAnyCut;
        Player.Instance.OnPikedSomething += Player_OnPikedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDop, baseCounter.transform.position);
    }

    private void Player_OnPikedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup,Player.Instance.transform.position);
    }

    //sender-cel ce declanseaza eventul
    private void CutingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CutingCounter cuttingCounter = sender as CutingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    

    private void DeliveryManagarer_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void PlayFootStepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if(volume > 1f)
            volume = 0f;
    }
    public float GetVolume()
    {
        return volume;
    }
   
}
