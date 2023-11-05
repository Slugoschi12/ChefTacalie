using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction; 
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private string PLAYER_PREFS_BINDINGS = "InputBindings";

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alt,
        Pause,
    }
    private PlayerInputAction inputActions;
    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputAction();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));//incarca ultimile setari inainte de a inchide aplicatia
        }
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        inputActions.Player.Pause.performed += Pause_performed;
        
    }

    private void OnDestroy()
    {   //pentru a rezolva o eroare ce tine de distrugerea obiectului GameInput
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }
    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);        
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);//daca onInteractAction e null, nu face nimic. In caz contrar executa functia
    }
    public Vector2 GetInputVectorNormalized()
    {  
        
        //PlayerInputAction este un sistem de inputuri realizat de unity.
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        
        
        //vechiul sistem
        //if (Input.GetKey(KeyCode.W))
        //{
        //    inputVector.y = +1;

        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    inputVector.y = -1;

        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    inputVector.x = -1;

        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    inputVector.x = +1;

        //}
        inputVector = inputVector.normalized;

        return inputVector;
    }
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {   default:
            case Binding.Move_Up:
                return inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Alt:
                return inputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        inputActions.Player.Disable();
        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = inputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = inputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = inputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = inputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = inputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alt:
                inputAction = inputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                inputActions.Player.Enable();
                onActionRebound();


                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, inputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }
}
