namespace LightInject.Interception.Tests
{
    using System;
    using System.CodeDom;
    using System.Configuration;
    using System.Threading.Tasks;
    using Moq;
    using SampleLibrary;
    using Xunit;
    [Collection("Interception")]
    public class AsyncTests
    {
        [Fact]
        public async Task ShouldInvokeAsyncTask()
        {
            var targetMock = new Mock<IMethodWithTaskReturnValue>();                        
            var proxy = CreateProxy(targetMock.Object);

            await proxy.Execute();

            targetMock.Verify(m => m.Execute(),Times.Once);
        }

        [Fact]
        public async Task ShouldInvokeAsyncOfTTask()
        {
            Mock<IMethodWithTaskOfTReturnValue> targetMock = new Mock<IMethodWithTaskOfTReturnValue>();
            targetMock.Setup(m => m.Execute()).ReturnsAsync(42);
            var proxy = CreateProxy(targetMock.Object);

            var result = await proxy.Execute();

            Assert.Equal(42, result);
        }

        [Fact]
        public void ShouldInvokeSynchronousMethod()
        {
            Mock<IMethodWithNoParameters> targetMock = new Mock<IMethodWithNoParameters>();
            var proxy = CreateProxy(targetMock.Object);

            proxy.Execute();

            targetMock.Verify(m => m.Execute(), Times.Once);
        }

        [Fact]
        public async Task ShouldInterceptAsyncTaskMethod()
        {
            Mock<IInterceptor> targetInterceptor = new Mock<IInterceptor>();
            var sampleInterceptor = new SampleAsyncInterceptor(targetInterceptor.Object);            
            var targetMock = new Mock<IMethodWithTaskReturnValue>();
            var proxy = CreateProxy(targetMock.Object, sampleInterceptor);
            await proxy.Execute();    
            Assert.True(sampleInterceptor.InterceptedTaskMethod);        
        }

        [Fact]
        public async Task ShouldInterceptAsyncTaskOfTMethod()
        {
            Mock<IInterceptor> targetInterceptor = new Mock<IInterceptor>();
            var sampleInterceptor = new SampleAsyncInterceptor(targetInterceptor.Object);
            var targetMock = new Mock<IMethodWithTaskOfTReturnValue>();
            var proxy = CreateProxy(targetMock.Object, sampleInterceptor);
            await proxy.Execute();
            Assert.True(sampleInterceptor.InterceptedTaskOfTMethod);
        }

        [Fact]
        public async Task ShouldNotCallTargetForTaskOfTMethod()
        {
            Mock<IInterceptor> targetInterceptor = new Mock<IInterceptor>();
            var sampleInterceptor = new SampleAsyncInterceptor(targetInterceptor.Object);
            var targetMock = new Mock<IMethodWithTaskOfTReturnValue>();
            var proxy = CreateProxy(targetMock.Object, sampleInterceptor);
            await proxy.Execute();
            targetInterceptor.Verify(m => m.Invoke(It.IsAny<IInvocationInfo>()),Times.Never);
        }

        [Fact]
        public async Task ShouldNotCallTargetForTaskMethod()
        {
            Mock<IInterceptor> targetInterceptor = new Mock<IInterceptor>();
            var sampleInterceptor = new SampleAsyncInterceptor(targetInterceptor.Object);
            var targetMock = new Mock<IMethodWithTaskReturnValue>();
            var proxy = CreateProxy(targetMock.Object, sampleInterceptor);
            await proxy.Execute();
            targetInterceptor.Verify(m => m.Invoke(It.IsAny<IInvocationInfo>()), Times.Never);
        }

        [Fact]
        public async Task ShouldInterceptAsyncTaskMethodUsingContainerDecoratedInterceptor()
        {
            bool interceptedTaskMethod = false;            
            var container = new ServiceContainer();
            Mock<IMethodWithTaskReturnValue> targetMock = new Mock<IMethodWithTaskReturnValue>();
            container.Register(facory => targetMock.Object);
            container.Register<IInterceptor, SampleInterceptor>();
            container.Decorate<IInterceptor>(
                (factory, interceptor) =>
                    new LambdaAsyncInterceptor(() => interceptedTaskMethod = true,
                        null, interceptor));
            container.Intercept(sr => sr.ServiceType == typeof(IMethodWithTaskReturnValue), factory => factory.GetInstance<IInterceptor>());
            var instance = container.GetInstance<IMethodWithTaskReturnValue>();

            await instance.Execute();

            Assert.True(interceptedTaskMethod);            

        }

        [Fact]
        public async Task ShouldInterceptAsyncTaskOfTMethodUsingContainerDecoratedInterceptor()
        {            
            bool interceptedTaskOfTMethod = false;
            var container = new ServiceContainer();
            Mock<IMethodWithTaskOfTReturnValue> targetMock = new Mock<IMethodWithTaskOfTReturnValue>();
            container.Register(facory => targetMock.Object);
            container.Register<IInterceptor, SampleInterceptor>();
            container.Decorate<IInterceptor>(
                (factory, interceptor) =>
                    new LambdaAsyncInterceptor(null,
                        () => interceptedTaskOfTMethod = true, interceptor));
            container.Intercept(sr => sr.ServiceType == typeof(IMethodWithTaskOfTReturnValue), factory => factory.GetInstance<IInterceptor>());
            var instance = container.GetInstance<IMethodWithTaskOfTReturnValue>();

            await instance.Execute();

            
            Assert.True(interceptedTaskOfTMethod);

        }

        private T CreateProxy<T>(T target)
        {
            ProxyBuilder proxyBuilder = new ProxyBuilder();
            ProxyDefinition proxyDefinition = new ProxyDefinition(typeof(T), () => target);
            proxyDefinition.Implement(() => new SampleAsyncInterceptor(new SampleInterceptor()));
            var proxyType = proxyBuilder.GetProxyType(proxyDefinition);
            var proxy = (T)Activator.CreateInstance(proxyType);
            return proxy;
        }

        private T CreateProxy<T>(T target, IInterceptor interceptor)
        {
            ProxyBuilder proxyBuilder = new ProxyBuilder();
            ProxyDefinition proxyDefinition = new ProxyDefinition(typeof(T), () => target);
            proxyDefinition.Implement(() => interceptor);
            var proxyType = proxyBuilder.GetProxyType(proxyDefinition);
            var proxy = (T)Activator.CreateInstance(proxyType);
            return proxy;
        }
    }



    internal class SampleAsyncInterceptor : AsyncInterceptor
    {
        public bool InterceptedTaskOfTMethod;

        public bool InterceptedTaskMethod;

        public SampleAsyncInterceptor(IInterceptor targetInterceptor) : base(targetInterceptor)
        {
        }

        protected override async Task InvokeAsync(IInvocationInfo invocationInfo)
        {
            InterceptedTaskMethod = true;
            // Before method invocation
            await base.InvokeAsync(invocationInfo);
            // After method invocation
        }

        protected override async Task<T> InvokeAsync<T>(IInvocationInfo invocationInfo)
        {
            InterceptedTaskOfTMethod = true;
            // Before method invocation
            var value = await base.InvokeAsync<T>(invocationInfo);
            // After method invocation           
            return value;
        }
    }

    internal class LambdaAsyncInterceptor : AsyncInterceptor
    {
        private readonly Action interceptedTaskMethod;
        private readonly Action interceptedTaskOfTMethod;

        public LambdaAsyncInterceptor(Action interceptedTaskMethod, Action interceptedTaskOfTMethod, IInterceptor targetInterceptor) : base(targetInterceptor)
        {
            this.interceptedTaskMethod = interceptedTaskMethod;
            this.interceptedTaskOfTMethod = interceptedTaskOfTMethod;
        }

        protected override Task InvokeAsync(IInvocationInfo invocationInfo)
        {
            interceptedTaskMethod();
            return base.InvokeAsync(invocationInfo);
        }

        protected override Task<T> InvokeAsync<T>(IInvocationInfo invocationInfo)
        {
            interceptedTaskOfTMethod();
            return base.InvokeAsync<T>(invocationInfo);
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