using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Helpers
{
    internal static class ReflectionHelpers
    {
        public static void ThrowExceptionIfTheresMoreThenOneCtor(this ConstructorInfo[] ctors, CQRSRouteDescriptor descriptor, ILogger logger)
        {
            if (ctors.Length > 1)
            {
                logger.LogError($"Type {descriptor.UnderlyingType.FullName} has more then one constructore. We support only a commands with one constructor.");
                throw new InvalidOperationException();
            }
        }

        public static object[] ResolveCtorArguments(this ConstructorInfo ctor, IServiceScope scope)
        {
            return ctor.GetParameters()
                .Select(param => scope.ServiceProvider.GetRequiredService(param.ParameterType))
                .ToArray();
        }

        public static async Task<object> HandleQueryAndGetResult(Type type, object[] ctorArgs, object argument)
        {
            var query = Activator.CreateInstance(type, ctorArgs);
            var method = type
                .GetMethod(nameof(IQueryHandler<object, object>.Handle), BindingFlags.Instance | BindingFlags.Public);
            var methodInvoke = (Task)method.Invoke(query, argument != null ? new[] { argument } : null);
            await methodInvoke.ConfigureAwait(false);
            return methodInvoke.GetType()
                .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public)
                .GetValue(methodInvoke);
        }
    }
}
