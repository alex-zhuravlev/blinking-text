namespace Core
{
    public interface ItmSingleton
    {
        void InitInstance();
    }

    public static class tmSingleton<T> where T : class, ItmSingleton, new()
    {
        public static T Instance
        {
            get
            {
                if (s_oInstance != null) return s_oInstance;
                s_oInstance = new T();
                s_oInstance.InitInstance();
                return s_oInstance;
            }
        }

        public static bool IsInstanceCreated()
        {
            return s_oInstance != null;
        }

        public static void AutoDestroy()
        {
            s_oInstance = null;
        }

        private static T s_oInstance = null;

    }
}