namespace Service.Exceptions
{
    public class BaseNotFoundException : Exception
    {
        public BaseNotFoundException(string message) : base(message) { }
    }
}
