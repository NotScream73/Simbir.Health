namespace Simbir.Health.Hospital.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
