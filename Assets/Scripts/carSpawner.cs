using System.Collections;
using UnityEngine;

public class carSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f; // Time between car spawns

    void Start()
    {
        StartCoroutine(SpawnCarsLoop()); // Use coroutine to spawn cars
    }

    // Continously spawn cars
    IEnumerator SpawnCarsLoop()
    {
        while (true)
        {
            SpawnCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCar()
    {
        // Spawn random car at random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int carIndex = Random.Range(0, carPrefabs.Length);

        Transform spawn = spawnPoints[spawnIndex];
        int direction = (spawnIndex % 2 == 0) ? 1 : -1; // Direction of car for each spawn point

        // Set rotation (depending on spawn point direction)
        Quaternion rotation = (direction == 1) ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        GameObject car = Instantiate(carPrefabs[carIndex], spawn.position, rotation);

        CarDriver carDriver = car.GetComponent<CarDriver>();
        if (carDriver != null)
        {
            carDriver.SetDirection(direction); // Set direction in the driver script
        }
    }

}
