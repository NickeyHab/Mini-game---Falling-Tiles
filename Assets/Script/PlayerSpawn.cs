using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    public Transform[] Spawnpoints;
    public static PlayerSpawn Instance;
    private bool p2Spawned = false;
    private void Start()
    {
        PlayerInput p1 = PlayerInput.Instantiate(playerPrefabs[0], playerIndex: 0);
        p1.transform.position = GetSpawnPoint(0).position;
    }
    private void Update()
    {
        // Spawn Player 2 only when gamepad input is detected
        if (!p2Spawned && Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            if (playerPrefabs.Length > 1)
            {
                PlayerInput p2 = PlayerInput.Instantiate(playerPrefabs[1], playerIndex: 1);
                p2.transform.position = GetSpawnPoint(1).position;
                p2Spawned = true;
            }
        }
    }
    public Transform GetSpawnPoint(int index)
    {
        if (index < Spawnpoints.Length)
        {
            return Spawnpoints[index];
        }
        return null;
    }
}
