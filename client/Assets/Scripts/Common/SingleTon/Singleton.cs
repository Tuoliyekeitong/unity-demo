public abstract class Singleton<T> where T : class, new()
{
    public static T Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new T();
            return _Instance;
        }
    }

    public static void ClearInstance()
    {
        _Instance = null;
    }

    private static T _Instance = null;
}
