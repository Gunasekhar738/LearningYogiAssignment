using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUpType
{
    ShootUpgrade,
    ForceField

}

[CreateAssetMenu(menuName = "PowerUp")]


public class PowerUpInfo : ScriptableObject {

    public PowerUpType powerUpType;
    public string powerUpName = "Power Up";
    [TextArea(3,4)]  [SerializeField] string powerDescriptionText;
    public Sprite powerSprite;

    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;

    public float movementSpeed = 50f;
    public float maxLifetime = 30f;

    public float size = 1f;

   
}
