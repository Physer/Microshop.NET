using System.Runtime.Serialization;

namespace Microshop.ContainerConfiguration.Exceptions;

/// <summary>
/// This exception is thrown when a container has not been initialied before accessing its properties
/// </summary>
internal class ContainerNotInitializedException : Exception
{
    public ContainerNotInitializedException(string? containerConfigurationName) : base($"The container has not been initizlied for configuration: {containerConfigurationName}") { }

    public ContainerNotInitializedException(string? message, Exception? innerException) : base(message, innerException) { }

    protected ContainerNotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
