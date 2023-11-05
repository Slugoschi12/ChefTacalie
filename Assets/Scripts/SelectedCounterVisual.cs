using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // Start is called before the first frame update
    //Awake se executa inainte de start
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }
    private void Player_OnSelectedCounterChanged(object sender,Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
           
        }
    }
    private void Show()
    {
        foreach (GameObject gameObject in visualGameObjectArray)
        { 
            gameObject.SetActive(true); 
        }
    }
    private void Hide()
    {
        foreach (GameObject gameObject in visualGameObjectArray)
        {
            gameObject.SetActive(false);
        }
    }
}