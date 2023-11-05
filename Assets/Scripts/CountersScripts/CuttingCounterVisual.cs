using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private const string CUT = "Cut";
    [SerializeField] private CutingCounter cutingCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        cutingCounter.OnCut += CutingCounter_OnCut;
    }

    private void CutingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
