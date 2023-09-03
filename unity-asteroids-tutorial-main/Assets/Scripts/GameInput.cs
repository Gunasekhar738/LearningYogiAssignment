using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public Player player;
    SpaceShipActionMap spaceShipAction;
    private InputAction left;
    private InputAction right;
    private InputAction shoot;
    private InputAction forward;
    private void Awake()
    {
        spaceShipAction = new SpaceShipActionMap();
        spaceShipAction.SpaceShip.Enable();

        left = spaceShipAction.SpaceShip.RotateLeft;
        left.Enable();

        right = spaceShipAction.SpaceShip.RotateRight;
        right.Enable();

        shoot = spaceShipAction.SpaceShip.Shoot;
        shoot.performed += ctx => isShooting();
        shoot.Enable();

        forward = spaceShipAction.SpaceShip.MoveForward;
        forward.Enable();



    }

    public void isShooting()
    {
        print("Shoot is called from game input");
        player.ShootNewInput();
       
    }
    public float GetRotationValue()
    {
        if (left.IsPressed()) return 1;

        if (right.IsPressed()) return -1;
      
        else return 0;
       
    }

    public bool isShootPressed()
    {
        if (shoot.ReadValue<float>() > 0.1f) return true;
        else return false;


    }

    public bool isForwardPressed()
    {
        if (forward.IsPressed()) return true;
        else return false;


    }

  
}
