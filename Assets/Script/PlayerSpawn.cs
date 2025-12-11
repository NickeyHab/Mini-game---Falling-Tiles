using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    public static PlayerSpawn Instance;
    public Transform[] Spawnpoints;
    private int m_playerCount = 0;
    private bool p2Spawned = false;
    private void Start()
    {
        PlayerInput p1 = PlayerInput.Instantiate(playerPrefabs[0], playerIndex: 0);
    }
    private void Update()
    {
        // Spawn Player 2 when IJKL input is detected
        if (!p2Spawned && (
            Keyboard.current.iKey.wasPressedThisFrame ||
            Keyboard.current.jKey.wasPressedThisFrame ||
            Keyboard.current.kKey.wasPressedThisFrame ||
            Keyboard.current.lKey.wasPressedThisFrame))
        {
            PlayerInput p2 = PlayerInput.Instantiate(playerPrefabs[1], playerIndex: 1);
            p2Spawned = true;
        }
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput != null && m_playerCount < Spawnpoints.Length)
        {
            playerInput.transform.position = Spawnpoints[m_playerCount].position;
            
            // Manually switch action map based on player index
            string actionMapName = m_playerCount == 0 ? "Player1" : "Player2";
            playerInput.SwitchCurrentActionMap(actionMapName);
            
            m_playerCount++;
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
