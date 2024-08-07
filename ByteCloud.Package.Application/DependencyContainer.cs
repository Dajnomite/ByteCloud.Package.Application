using ByteCloud.Package.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ByteCloud.Package.Application
{
    /// <summary>
    /// Manages the registration and resolution of application dependencies.
    /// </summary>
    public class DependencyContainer
    {
        protected IServiceCollection ServiceCollection { get; }
        protected ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainer"/> class.
        /// </summary>
        public DependencyContainer()
        {
            ServiceCollection = new ServiceCollection();
        }

        /// <summary>
        /// Registers a singleton service of the type specified in <typeparamref name="TService"/> with an instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <returns>The current <see cref="DependencyContainer"/> instance.</returns>
        public DependencyContainer AddSingleton<TService>(TService instance) where TService : class
        {
            ServiceCollection.AddSingleton<TService>(instance);
            return this;
        }

        /// <summary>
        /// Registers a transient service of the type specified in <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>The current <see cref="DependencyContainer"/> instance.</returns>
        public DependencyContainer AddTransient<TService>() where TService : class
        {
            ServiceCollection.AddTransient<TService>();
            return this;
        }

        /// <summary>
        /// Builds the service provider and prepares the container for service resolution.
        /// </summary>
        /// <returns>The current <see cref="DependencyContainer"/> instance.</returns>
        public DependencyContainer Build()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
            return this;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="TService">The type of the service object to get.</typeparam>
        /// <returns>A service object of type <typeparamref name="TService"/>.
        /// -or-
        /// null if there is no service object of type <typeparamref name="TService"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the Build method hasn't been called before retrieving services.</exception>
        public TService? GetService<TService>() where TService : class
        {
            Log.Assert(ServiceProvider != null, "Service provider was null. Did you call Build?");
            return ServiceProvider?.GetService<TService>();
        }
    }
}