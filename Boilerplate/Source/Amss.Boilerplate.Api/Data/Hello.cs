namespace Amss.Boilerplate.Api.Data
{
    using ServiceStack.ServiceHost;

    [RestService("/hello")]
    [RestService("/hello/{Name}")]
    public class Hello
    {
        public string Name { get; set; }
    }
}