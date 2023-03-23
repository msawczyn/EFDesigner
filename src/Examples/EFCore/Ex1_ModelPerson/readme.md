
# EF Core IoC with the Dependency injection framework 

[MSDN Fundamentals](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)

[MSDN Dependency Injection](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion)

[Wikipedia Inversion Of Control](https://en.wikipedia.org/wiki/Inversion_of_control)

## Why DI is a treat
In EF Core it's common to pass some DbContextOptions to the constructor.

So in general, a constructor looks like this:

public BlexzWebDb(DbContextOptions<BlexzWebDb> options) : base(options)

As you can see there, there is no valid overload in the form of a parameter-less constructor:

Thus, this does not work:

using (var db = new BlexzWebDb())
Obviously, you can pass in an Option object in the constructor but there is an alternative. So,

Instead

.Net Core has IoC implemented in it's roots. Okay, this means; you don't create a context, you ask the framework to give you one, based on some rules you defined before.
Example: somewhere you will register your dbcontext, (Startup.cs):

//typical configuration part of .net core
public void ConfigureServices(IServiceCollection services)
{
    //some mvc 
    services.AddMvc();
  
    //hey, options! 
    services.AddDbContextPool<BlexzWebDb>(options => 
           options.UseSqlServer(Configuration.GetConnectionString("BlexzWebConnection")));
    //...etc

Now the registering part is done, you can retrieve your context from the framework. E.g.: inversion of control through a constructor in your controller:

public class SomeController : Controller
{
    private readonly BlexzWebDb _db;

    //the framework handles this
    public SomeController(BlexzWebDb db)
    {
        _db = db;
    }

    //etc.

why?

So, why not just provide the arguments and new it?

There is nothing wrong with the use of new - there are a lot of scenario's in which it works best.

But, Inversion Of Control is considered to be a good practice. When doing asp dotnet core you're likely to use it quite often because most libraries provide extension methods to use it. If you are not familiar with it, and your research allow it; you should definitely give it a try.

