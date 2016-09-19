namespace LightInject.Interception.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;
    public class AsyncTests
    {
        [Fact]
        public async Task ShouldInterceptAsyncTask()
        {
            Mock<IMethodWithTaskReturnValue> targetMock = new Mock<IMethodWithTaskReturnValue>();            

            ProxyBuilder proxyBuilder = new ProxyBuilder();
            ProxyDefinition proxyDefinition = new ProxyDefinition(typeof(IMethodWithTaskReturnValue),() => targetMock.Object);
            proxyDefinition.Implement(() => new AsyncInterceptor());
            var proxyType = proxyBuilder.GetProxyType(proxyDefinition);
            var proxy = (IMethodWithTaskReturnValue)Activator.CreateInstance(proxyType);
            await proxy.Execute();
            targetMock.Verify(m => m.Execute(),Times.Once);
        }

        [Fact]
        public async Task ShouldInterceptAsyncOfTTask()
        {
            Mock<IMethodWithTaskOfTReturnValue> targetMock = new Mock<IMethodWithTaskOfTReturnValue>();
            targetMock.Setup(m => m.Execute()).ReturnsAsync(42);

            ProxyBuilder proxyBuilder = new ProxyBuilder();
            ProxyDefinition proxyDefinition = new ProxyDefinition(typeof(IMethodWithTaskOfTReturnValue), () => targetMock.Object);
            proxyDefinition.Implement(() => new AsyncInterceptor());
            var proxyType = proxyBuilder.GetProxyType(proxyDefinition);
            var proxy = (IMethodWithTaskOfTReturnValue)Activator.CreateInstance(proxyType);
            var result = await proxy.Execute();
            Assert.Equal(42, result);
        }

    }



    public class AsyncInterceptor : Interceptor
    {
        public override object Invoke(IInvocationInfo invocationInfo)
        {
            // Before method invocation            
            var value = base.Invoke(invocationInfo);            
            // After method invocation
            return value;
        }

        protected override async Task InvokeAsync(IInvocationInfo invocationInfo)
        {
            // Before method invocation
            await base.InvokeAsync(invocationInfo);
            // After method invocation
        }

        protected override async Task<T> InvokeAsync<T>(IInvocationInfo invocationInfo)
        {
            // Before method invocation
            var value = await base.InvokeAsync<T>(invocationInfo);
            // After method invocation
            return value;
        }
    }

    public interface IMethodWithTaskReturnValue
    {
        Task Execute();
    }

    public interface IMethodWithTaskOfTReturnValue
    {
        Task<int> Execute();
    }    
}