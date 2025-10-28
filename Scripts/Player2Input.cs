using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player2Input : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public event EventHandler OnPlayer2Jump;

    private GameInput player2InputActions;



    private void Awake()
    {
        player2InputActions = new GameInput();
        player2InputActions.Player2.Enable();

        player2InputActions.Player2.Jump.performed += Jump2_performed;
    }

    private void Jump2_performed(InputAction.CallbackContext obj)
    {
        if (OnPlayer2Jump != null)
        {
            OnPlayer2Jump?.Invoke(this, EventArgs.Empty);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    public Vector2 GetMovement2VectorNormalized()
    {
        Vector2 input2Vector = player2InputActions.Player2.Move.ReadValue<Vector2>();

        input2Vector = input2Vector.normalized;

        return input2Vector;
    }
}
