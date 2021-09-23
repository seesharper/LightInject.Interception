namespace LightInject.Interception.Tests
{
    using System;

    using LightInject.Interception.Tests.NamespaceB;

    public interface ITarget
    {        
    }

    public interface IDisposableTarget : IDisposable
    {
        
    }


    public class TargetWithGetHashCodeOverride : ITarget
    {
        private readonly int hashCode;

        public TargetWithGetHashCodeOverride(int hashCode)
        {
            this.hashCode = hashCode;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }

    public interface ITargetWithMethod
    {
        string GetValue();
    }

    public class BaseTarget : ITargetWithMethod
    {
        public virtual string GetValue() => nameof(BaseTarget);
    }

    public class InheritedTargetWithoutOverride : BaseTarget
    {
        // No method override
    }

    public class InheritedTargetWithOverride : BaseTarget
    {
        public override string GetValue() => nameof(InheritedTargetWithOverride);
    }


    public interface IAdditionalInterface
    {
    }

    public interface IMethodWithNoParameters
    {
        void Execute();
    }

    public interface IClassWithGenericClassContraint<T>
        where T : class, new()
    {
    }

    public interface IClassWithProperty
    {
        string Value { get; set; }
    }

    public class ClassWithProperty : IClassWithProperty
    {
        string IClassWithProperty.Value { get; set; }
    }

    public class ClassWithTargetReturnType
    {
        public virtual ClassWithTargetReturnType Execute()
        {
            return this;
        }
    }


    public interface IClassWithValueTypeProperty
    {
        string Value { get; set; }
    }

    public interface IClassWithEvent
    {
        event EventHandler<EventArgs> SomeEvent;
    }

    //public class ClassWithEvent : IClassWithEvent
    //{
    //    event EventHandler<EventArgs> IClassWithEvent.SomeEvent
    //    {
    //        add
    //        {                                
    //            throw new NotImplementedException();
    //        }
    //        remove
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}

    public interface IMethodWithGenericConstraint
    {
        void Execute<T>(T value) where T : class, new();
    }

    
    public interface IMethodWithReferenceTypeParameter
    {
        void Execute(string value);
    }

    public class MethodWithReferenceTypeParameter : IMethodWithReferenceTypeParameter
    {
        public void Execute(string value)
        {

        }
    }

    public interface IMethodWithValueTypeParameter
    {
        void Execute(int value);
    }

    public class MethodWithValueTypeParameter : IMethodWithValueTypeParameter
    {
        public void Execute(int value)
        {
            
        }
    }

    public interface IMethodWithNullableParameter
    {
        void Execute(int? value);
    }

    public interface IMethodWithReferenceTypeRefParameter
    {
        void Execute(ref string value);
    }

    public class MethodWithReferenceTypeRefParameter : IMethodWithReferenceTypeRefParameter
    {
        private readonly string valueToReturn;

        public MethodWithReferenceTypeRefParameter(string valueToReturn)
        {
            this.valueToReturn = valueToReturn;
        }

        public void Execute(ref string value)
        {
            value = valueToReturn;
        }
    }

    public interface IMethodWithValueTypeRefParameter
    {
        void Execute(ref int value);
    }

    public class MethodWithValueTypeRefParameter : IMethodWithValueTypeRefParameter
    {
        public void Execute(ref int value)
        {
            value = 84;
        }
    }

    //public class MethodWithValueTypeRefParameter : IMethodWithValueTypeRefParameter
    //{
    //    public void Execute(ref ValueTypeFoo value)
    //    {
    //        value = new ValueTypeFoo { Value = "AnotherValue" };
    //    }
    //}

    public interface IMethodWithValueTypeOutParameter
    {
        void Execute(out int value);
    }

    public class MethodWithValueTypeOutParameter : IMethodWithValueTypeOutParameter
    {
        private readonly int valueToReturn;

        public MethodWithValueTypeOutParameter(int valueToReturn)
        {
            this.valueToReturn = valueToReturn;
        }

        public void Execute(out int value)
        {
            value = valueToReturn;
        }
    }

    public interface IMethodWithReferenceTypeOutParameter
    {
        void Execute(out string value);
    }

    public class MethodWithReferenceTypeOutParameter : IMethodWithReferenceTypeOutParameter
    {
        private readonly string valueToReturn;

        public MethodWithReferenceTypeOutParameter(string valueToReturn)
        {
            this.valueToReturn = valueToReturn;
        }

        public void Execute(out string value)
        {
            value = valueToReturn;
        }
    }

    public interface IMethodWithReferenceTypeReturnValue
    {
        string Execute();
    }

    public interface IMethodWithValueTypeReturnValue
    {
        int Execute();
    }

    public interface IMethodWithGenericReturnValue
    {
        T Execute<T>();
    }

    public interface IMethodWithGenericParameter
    {
        void Execute<T>(T value);
    }
    
    public class MethodWithGenericParameter: IMethodWithGenericParameter
    {
        public void Execute<T>(T value)
        {
            
        }
    }

    public interface IDerivedFromGenericMethod : IMethodWithGenericParameter
    {
        
    }

    public interface IMethodWithGenericParameterThatHasClassConstraint
    {
        void Execute<T>(T value) where T : class;
    }

    public interface IMethodWithGenericParameterThatHasStructConstraint
    {
        void Execute<T>(T value) where T : struct;
    }
    
    public interface IMethodWithCovariantTypeParameter<out T>
    {
        T Execute();
    }

    public interface IMethodWithContravariantTypeParameter<in T>
    {
        void Execute(T value);
    }

    public interface IMethodWithGenericParameterThatHasNewConstraint
    {
        void Execute<T>(T value) where T : new();
    }

    public interface IMethodWithGenericParameterThatHasNestedContraint
    {
        void Execute<T>(T value) where T : IComparable<T>;
    }

    public interface IMethodWithTypeLevelGenericParameter<T>
    {
        void Execute(T value);
    }


    public interface IMethodWithEnumParameter
    {
        void Execute(StringSplitOptions options);
    }

    public interface IMethodWithEnumOutParameter
    {
        void Execute(out StringSplitOptions options);
    }

    public interface IMethodWithEnumRefParameter
    {
        void Execute(ref StringSplitOptions options);
    }

    public class MethodWithEnumRefParameter : IMethodWithEnumRefParameter
    {
        public void Execute(ref StringSplitOptions options)
        {
            options = StringSplitOptions.None;
        }
    }

    public interface IMethodWithEnumReturnValue
    {
        StringSplitOptions Execute();
    }

    public class ReferenceTypeFoo
    {
        public string Value { get; set; }
    }

    public struct ValueTypeFoo
    {
        public string Value { get; set; }
    }


    public interface IMethodWithTargetReturnType
    {
        IMethodWithTargetReturnType Execute();
    }

    public interface IMethodWithObjectReturnType
    {
        object Execute();
    }


    public interface IClassWithOverloadedMethods
    {
        void Execute();

        void Execute(string value);

    }


    public interface IA
    {
        void Execute();
    }

    namespace NamespaceA
    {
        public interface IMethodWithNoParameters
        {
            void Execute();
        }
    }

    namespace NamespaceB
    {
        public interface IMethodWithNoParameters
        {
            void Execute();
        }
    }
    
    
    public class CustomAttribute : Attribute
    {
        
    }

    [CustomAttribute]
    public class ClassWithCustomAttribute
    {

    }

    public class CustomAttributeWithStaticConstructor : Attribute
    {
        static CustomAttributeWithStaticConstructor()
        {
            
        }
    }


    public class CustomAttributeWithNamedArgument : Attribute
    {
        public int Value { get; set; }
    }

    public class CustomAttributeWithPublicField : Attribute
    {
        public int Value;
    }

    public class CustomAttributeWithConstructorArgument : Attribute
    {
        public int Value { get; private set; }

        public CustomAttributeWithConstructorArgument(int value)
        {
            Value = value;
        }
    }

    [CustomAttributeWithStaticConstructor]
    public class ClassWithCustomAttributeWithStaticConstructor
    {
        
    }

    [CustomAttributeWithNamedArgument(Value = 42)]
    public class ClassWithCustomAttributeWithNamedArgument
    {
        
    }

    [CustomAttributeWithPublicField(Value = 42)]
    public class ClassWithCustomAttributeWithPublicField
    {

    }

    [CustomAttributeWithConstructorArgument(42)]
    public class ClassWithCustomAttributeWithConstructorArgument
    {

    }

    public interface IClassWithTwoMethods
    {
        void FirstMethod();
        void SecondMethod();
    }

    public class ClassWithTwoMethods : IClassWithTwoMethods
    {
        public void FirstMethod()
        {
            
        }

        public void SecondMethod()
        {
            
        }
    }

    public interface IClassWithOneMethod
    {
        void FirstMethod();        
    }

    public class ClassWithOneMethod : IClassWithOneMethod
    {
        public void FirstMethod()
        {
            
        }        
    }

    public class ClassWithOverriddenEquals
    {
        protected ClassWithOverriddenEquals()
        {
        }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is ClassWithOverriddenEquals) &&
                ((ClassWithOverriddenEquals)obj).Name == this.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }


    public class ClassWithVirtualMemberCallInConstructor
    {
        public ClassWithVirtualMemberCallInConstructor()
        {
            Execute();
        }

        public virtual void Execute()
        {
            
        }
    }


    public class ClassWithConstructorArguments : IMethodWithValueTypeReturnValue
    {
        public int Execute()
        {
            return 42;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class CustomConstructorParameterAttribute : Attribute
    {
    }

    public class ClassWithAttributedParameterInConstructor
    {
        public ClassWithAttributedParameterInConstructor([CustomConstructorParameter] string input)
        {
        }
    }

    public interface IClassWithThreeMethods
    {
        void A();
        void B();
        void C();
    }

    public interface IMethodWithCovariantTypeParameterAndGenericArgument<out TEntity>
    {
        TEntity GetById<TId>(TId id);
    }

    public class Retval
    {
        public string Value { get { return "value"; } }
    }

    public class MethodWithCovariantTypeParameterAndGenericArgument : IMethodWithCovariantTypeParameterAndGenericArgument<Retval>
    {
        public Retval GetById<TId>(TId arg)
        {
            return new Retval();
        }
    }
}