using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GameConfiguration")]
public class GameConfiguration : ScriptableObject
{

    [TextArea(3, 4)] [SerializeField] string GlobalDescriptionText;

    [Range(0.1f, 5f)]
    public float SpaceshipAcceleration;

    [Range(0.01f, 1f)]
    public float SpaceshipRotationSpeed;

    [Range(10f, 100f)]
    public int SpaceshipHealth;

    [TextArea(6, 8)] [SerializeField] string AsteroidSettings;

    [TextArea(6, 12)] [SerializeField] string HowToCreateaNewPowerUp;

}
