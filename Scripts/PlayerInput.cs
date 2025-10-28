using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public event EventHandler OnPlayer1Jump;

    private GameInput playerInputActions;



    private void Awake()
    {
        playerInputActions = new GameInput();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (OnPlayer1Jump != null)
        {
            OnPlayer1Jump?.Invoke(this, EventArgs.Empty);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public Vector2 GetMovement1VectorNormalized()
    {
        Vector2 input1Vector = playerInputActions.Player.Move.ReadValue<Vector2>();

        input1Vector = input1Vector.normalized;

        return input1Vector;
    }
}
