using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public float carSpeed = 5f;
    public float carDistance = 20f;
    public float minSpeed = 0f;

    private float currentSpeed;
    private int direction = 1; 

    void Start()
    {
        // Set car speed
        currentSpeed = carSpeed;
    }

    void Update()
    {
        // Calculate distance to car
        float distanceToCarAhead = GetDistanceToCarAhead();

        if (distanceToCarAhead > 0)
        {
            // Slow down smoothly
            float slowdownFactor = Mathf.Clamp01(distanceToCarAhead / carDistance); 
            currentSpeed = Mathf.Lerp(minSpeed, carSpeed, slowdownFactor);
        }

        // Move forward
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // Destroy past boundaries (Tunnels)
        if ((direction == 1 && transform.position.z > 100f) ||
            (direction == -1 && transform.position.z < 0f))
        {
            Destroy(gameObject);
        }
    }

    float GetDistanceToCarAhead()
    {
        // Use raycast to detect cars infront
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, carDistance))
        {
            // Check for Car tag
            if (hit.collider.CompareTag("Car"))
            {
                return hit.distance; // Get distance to that car
            }
        }
        return -1f;
    }

    public void SetDirection(int dir)
    {
        // Set direction depending on which side of road it instantiates on
        direction = dir;
    }

}
