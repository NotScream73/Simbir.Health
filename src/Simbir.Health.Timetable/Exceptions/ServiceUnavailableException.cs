namespace Simbir.Health.Timetable.Exceptions
{
    public class ServiceUnavailableException : ApiException
    {
        public ServiceUnavailableException(string message) : base(message) { }
    }
}
