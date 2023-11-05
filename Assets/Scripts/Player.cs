using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Player : MonoBehaviour,IKitchenObjectParent{
    
    private const float rotationSpeed = 10;
    private bool isWalking;
    private Vector3 lastInteractDir;

    public event EventHandler OnPikedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;//generic
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    [SerializeField] private float moveSpeed = 7f;//face vizibil in inspector si poate fi mod de acolo
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersMasck;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    public static Player Instance { get; private set; }
    

    private void Awake()
    {
        if( Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {   if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if(selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    private void Update(){
        PlayerMove();
        Interactions();
    }
    private void Interactions()
    {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 2f;
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        //daca se detecteaza coliziune, select counter ia info despre obiectul respectiv, in caz contrar , acesta este null
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,countersMasck))//out RaycastHit returneaza informatii despre obiectul lovit.
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))//daca obiectul lovit are un "ClearCounter", apelezi functia Interact()
            {
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }else
            {
                SetSelectedCounter(null);
            }
        }else
        {
            SetSelectedCounter(null);
        }
    }
    private void PlayerMove()
    {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight,playerRadius, moveDir, moveDistance);
        if(!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirX, moveDistance);
            if(canMove)
            {
                moveDir = moveDirX;
            }else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirZ, moveDistance);
                if(canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
        if(canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);//rotatie catre directia de miscare
        //slerp- interpolare sferica intre 2 vectori
       // Debug.Log(inputVector);
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPikedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
