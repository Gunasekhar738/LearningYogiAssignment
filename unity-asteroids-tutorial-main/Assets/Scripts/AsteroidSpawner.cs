using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float spawnDistance = 12f;
    public int amountPerSpawn = 1;
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;
    [Range(1f, 20f)]
    public float spawnRate = 1;
    [Range(0.0001f, 0.01f)]
    public float decreaseAmount = 0.01f;

    private void Start()
    {
      //  InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
        StartCoroutine("CollisionRoutine");
    }
    IEnumerator CollisionRoutine()
    {

        yield return new WaitForSeconds(spawnRate);
        Spawn();
        spawnRate -= decreaseAmount;
        if (spawnRate <= 0.1f) spawnRate = 0.1f;
        print(spawnRate);
        StartCoroutine("CollisionRoutine");
    }
    public void Spawn()
    {
        for (int i = 0; i < amountPerSpawn; i++)
        {
            // Choose a random direction from the center of the spawner and
            // spawn the asteroid a distance away
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDistance);

            // Calculate a random variance in the asteroid's rotation which will
            // cause its trajectory to change
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            // Create the new asteroid by cloning the prefab and set a random
            // size within the range
            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            // Set the trajectory to move in the direction of the spawner
            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }

}
