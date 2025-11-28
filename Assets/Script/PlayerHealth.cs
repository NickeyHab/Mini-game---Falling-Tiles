using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ResetFloor"))
        {
            transform.position = GameObject.Find("Spawnpoint1").transform.position;  
        }
    }
}
