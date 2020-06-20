namespace GISControl.Model.Help
{
    class Singleton<T> where T : class, new()
    {
        private static T _instance = null;

        public Singleton() { }

        public static T instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }
    }
}
