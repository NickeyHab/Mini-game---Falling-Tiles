using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerHealth : MonoBehaviour
{
    private PlayerInput playerInput;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ResetFloor")) //To respawn player when falling off the platform
        {
            int index = playerInput.playerIndex;
            Transform spawnPoint = PlayerSpawn.Instance.GetSpawnPoint(index);
            transform.position = spawnPoint.position;
        }
    }
}
