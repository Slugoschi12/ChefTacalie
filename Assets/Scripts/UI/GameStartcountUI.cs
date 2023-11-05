using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartcountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startCountText;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;
        Hide();
    }

    private void KitchenGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsStartCuntdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        startCountText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
