using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    public static CollectiblesController Instance;

    public GameObject[] powerballPrefabs; 

    private void Awake()
    {
        Instance = this;
    }

    public void GeneratePowerball(Vector3 position)
    {
        if (powerballPrefabs == null || powerballPrefabs.Length == 0)
        {
            Debug.LogError("? No powerball prefabs assigned in CollectiblesController!");
            return;
        }

        int index = Random.Range(0, powerballPrefabs.Length);
        Instantiate(powerballPrefabs[index], position, Quaternion.identity);

        Debug.Log("? Powerball spawned: " + powerballPrefabs[index].name);
    }
}
