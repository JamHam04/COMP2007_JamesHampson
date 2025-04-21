using System.Collections;
using UnityEngine;

public class carSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f; // Time between car spawns

    void Start()
    {
        StartCoroutine(SpawnCarsLoop());
    }

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
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int carIndex = Random.Range(0, carPrefabs.Length);

        Transform spawn = spawnPoints[spawnIndex];
        int direction = (spawnIndex % 2 == 0) ? 1 : -1;

        Quaternion rotation = (direction == 1) ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        GameObject car = Instantiate(carPrefabs[carIndex], spawn.position, rotation);

        CarDriver carDriver = car.GetComponent<CarDriver>();
        if (carDriver != null)
        {
            carDriver.SetDirection(direction);
        }
    }

}
