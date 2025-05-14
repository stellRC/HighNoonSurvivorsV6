using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    public static PersistentObjects Instance;

    [Header("Persistent Objects")]
    [SerializeField]
    private GameObject[] _persistentObjects;

    private void Awake()
    {
        if (Instance != null)
        {
            CleanUpAndDestroy();
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in _persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    // Destroy duplicates of persistent objects
    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in _persistentObjects)
        {
            Destroy(obj);
        }
        // game manager destroys itself
        Destroy(gameObject);
    }
}
