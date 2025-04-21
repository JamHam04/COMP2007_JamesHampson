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
        currentSpeed = carSpeed;
    }

    void Update()
    {
        float distanceToCarAhead = GetDistanceToCarAhead();

        if (distanceToCarAhead > 0)
        {
            // Slow down smoothly
            float slowdownFactor = Mathf.Clamp01(distanceToCarAhead / carDistance); 
            currentSpeed = Mathf.Lerp(minSpeed, carSpeed, slowdownFactor);
        }


        transform.position += transform.forward * currentSpeed * Time.deltaTime;


        if ((direction == 1 && transform.position.z > 100f) ||
            (direction == -1 && transform.position.z < 0f))
        {
            Destroy(gameObject);
        }
    }

    float GetDistanceToCarAhead()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, carDistance))
        {
            if (hit.collider.CompareTag("Car"))
            {
                return hit.distance;
            }
        }
        return -1f;
    }

    public void SetDirection(int dir)
    {
        direction = dir;
    }

}
