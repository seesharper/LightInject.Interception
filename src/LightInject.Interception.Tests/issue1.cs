namespace LightInject.Interception.Tests
{
    using System;
    using Xunit;

    public class Issue_1
    {
        [Fact]
        public void ShouldInterceptDisposeFromBaseClass()
        {
            var container = new ServiceContainer();
            container.Register<MyClass>();
            container.Intercept(
                           sr => sr.ServiceType == typeof(MyClass),
                           (factory, definition) => DefineProxyType(definition, new SampleInterceptor()));
            var test = container.GetInstance<MyClass>();    
            test.Dispose();                                            
        }
        

        private static void DefineProxyType(ProxyDefinition definition, IInterceptor myInterceptor)
        {
            definition.Implement(
                () => myInterceptor,
                m => m.IsDeclaredBy(definition.TargetType) && m.IsPublic);
        }
    }


    public class MyClass : BaseClass
    {
    }

    public abstract class BaseClass : IDisposable
    {
        public virtual void Dispose()
        {
        }
    }
}