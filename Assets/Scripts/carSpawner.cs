using UnityEngine;

public class carSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] spawnPoints;

    void Start()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int carIndex = Random.Range(0, carPrefabs.Length);

        Transform spawn = spawnPoints[spawnIndex];
        int direction = (spawnIndex == 0) ? 1 : -1;

        Quaternion rotation = (direction == 1) ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        GameObject car = Instantiate(carPrefabs[carIndex], spawn.position, rotation);

        CarDriver carDriver = car.GetComponent<CarDriver>();
        carDriver.SetDirection(direction);

    }
}
