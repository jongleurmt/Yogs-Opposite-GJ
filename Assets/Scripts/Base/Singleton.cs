using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    /// <summary>
    /// The singleton instance.
    /// </summary>
    public static T Instance { get; private set; } = null;

    [SerializeField, Tooltip("Disable if only this object should be cleaned up.")]
    private bool m_DontDestroy = true;

    // Assigns the main instance on script initialization.
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            if (m_DontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this, true);
        }
    }

    // Cleans up the instance references.
    protected virtual void OnDestroy()
    {
        if (Instance == this as T)
            Instance = null;
    }
}