using ByteCloud.Package.Logging;
using System;

namespace ByteCloud.Package.Application
{
    /// <summary>
    /// Represents an environment variable.
    /// </summary>
    public class EnvironmentVariable
    {
        public string Name { get; }
        public string Value { get; }

        public EnvironmentVariable(string name)
        {
            Assert.NotNullOrWhitespace(() => name);

            Name = name;
            Value = Environment.GetEnvironmentVariable(Name, EnvironmentVariableTarget.Process);

            if (string.IsNullOrWhiteSpace(Value))
            {
                Value = Environment.GetEnvironmentVariable(Name, EnvironmentVariableTarget.Machine);

                if (string.IsNullOrWhiteSpace(Value))
                {
                    Value = Environment.GetEnvironmentVariable(Name, EnvironmentVariableTarget.User);

                    if (string.IsNullOrWhiteSpace(Value))
                        Log.Error($"Environment variable '{Name}' was not found.");
                }
            }
        }
    }
}