using UnityEngine;
using System.Collections;

public class TileTouch : MonoBehaviour
{
    public string hexTileTag = "HexTile";
    
    [Header("Tile Destruction")]
    [SerializeField] private float fallDelay = 10f;
    [SerializeField] private float fallSpeed = 10f;
    [SerializeField] private float fadeDuration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hexTileTag))
        {
            StartCoroutine(DestroyTile(other.gameObject));
        }
    }

    private IEnumerator DestroyTile(GameObject HexagonPrefab)
    {
        Renderer renderer = HexagonPrefab.GetComponent<Renderer>();

        float elapsedTime = 0f;
        Material material = renderer.material;
        Color startColor = material.color;
        yield return new WaitForSeconds(fallDelay);

        // Fall and fade out the tile
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            HexagonPrefab.transform.position += Vector3.down * fallSpeed * Time.deltaTime; // Move tile downwards
            
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Calculate new alpha value
            
            Color newColor = startColor; // Create a new color based on the start color
            newColor.a = alpha; // Update alpha
            material.color = newColor; // Apply new color to material
            
            yield return null;
        }

        // Destroy the tile after fade completes
        Destroy(HexagonPrefab);
        yield break;
    }
}
