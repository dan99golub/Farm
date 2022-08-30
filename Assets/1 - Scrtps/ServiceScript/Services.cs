namespace ServiceScript
{
    public class Services<T>
    {
        private T _service;

        private static Services<T> _i;
        public static Services<T> S=>_i??=new Services<T>();

        public T Get() => _service;

        public T Set(T service) => _service = service;
    }
}