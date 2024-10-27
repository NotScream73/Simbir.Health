namespace Simbir.Health.Document.Exceptions
{
    public class ServiceUnavailableException : ApiException
    {
        public ServiceUnavailableException(string message) : base(message) { }
    }
}
