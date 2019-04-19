using Xunit;

namespace LightInject.Interception.Tests
{
    public class issue21
    {
        [Fact]
        public void ImplementInterceptedMethod_ShouldFindBaseClassDeclaredMethods_WhenTargetTypeIsInterface()
        {
            var container = new ServiceContainer();
            container.Register<IIssue21, IssueClass21>();
            container.Intercept(
                sr => sr.ServiceType == typeof(IIssue21),
                (factory, definition) => DefineProxyType(definition, new SampleInterceptor()));

            IIssue21 test = null;

            var ex = Record.Exception(() => test = container.GetInstance<IIssue21>());
            Assert.Null(ex);
            Assert.NotNull(test);

            test.InterceptMe();
        }

        private static void DefineProxyType(ProxyDefinition definition, IInterceptor myInterceptor)
        {
            definition.Implement(
                () => myInterceptor,
                m => m.IsDeclaredBy(definition.TargetType) && m.IsPublic);
        }
    }

    public interface IIssue21
    {
        void InterceptMe();
    }

    public abstract class Base21Class : IIssue21
    {
        public virtual void InterceptMe()
        {
        }
    }

    public class IssueClass21 : Base21Class
    {
    }
}