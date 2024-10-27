namespace Simbir.Health.Timetable.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
