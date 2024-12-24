﻿using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JeezFoundation.Algorithm.Mathematics;

/// <summary>Contains definitions necessary for the generic Symbolics class.</summary>
public static class Symbolics
{
    #region OperatorPriority Enum

    internal enum OperatorPriority
    {
#pragma warning disable CA1069 // Enums values should not be duplicated
#pragma warning disable SA1602 // Enumeration items should be documented
        Addition = 1,
        Subtraction = 1,
        Multiplication = 2,
        Division = 2,
        Exponents = 3,
        Roots = 3,
        Logical = 4,
        Negation = 5,
        Factorial = 6,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore CA1069 // Enums values should not be duplicated
    }

    #endregion OperatorPriority Enum

    #region Attributes

    [AttributeUsage(AttributeTargets.Class)]
    internal abstract class RepresentationAttribute : Attribute
    {
        internal string[] _representations;

        internal RepresentationAttribute(string a, params string[] b)
        {
            if (string.IsNullOrWhiteSpace(a))
            {
                throw new ArgumentException(
                    "There is a BUG in " + nameof(JeezFoundation.Algorithm) + ". A " +
                    nameof(Symbolics) + "." + nameof(RepresentationAttribute) + " representation is invalid.");
            }
            foreach (string @string in b)
            {
                if (string.IsNullOrWhiteSpace(@string))
                {
                    throw new ArgumentException(
                        "There is a BUG in " + nameof(JeezFoundation.Algorithm) + ". A " +
                        nameof(Symbolics) + "." + nameof(RepresentationAttribute) + " representation is invalid.");
                }
            }
            _representations = new string[b.Length + 1];
            _representations[0] = a;
            for (int i = 1, j = 0; j < b.Length; i++, j++)
            {
                _representations[i] = b[j];
            }
        }

        internal string[] Representations
        {
            get
            {
                return _representations;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class OperationAttribute : RepresentationAttribute
    {
        internal OperationAttribute(string a, params string[] b) : base(a, b)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class LeftUnaryOperatorAttribute : Attribute
    {
        internal readonly string Representation;
        internal readonly OperatorPriority Priority;

        internal LeftUnaryOperatorAttribute(string representation, OperatorPriority operatorPriority) : base()
        {
            Representation = representation;
            Priority = operatorPriority;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class RightUnaryOperatorAttribute : Attribute
    {
        internal readonly string Representation;
        internal readonly OperatorPriority Priority;

        internal RightUnaryOperatorAttribute(string representation, OperatorPriority operatorPriority) : base()
        {
            Representation = representation;
            Priority = operatorPriority;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class BinaryOperatorAttribute : Attribute
    {
        internal readonly string Representation;
        internal readonly OperatorPriority Priority;

        internal BinaryOperatorAttribute(string representation, OperatorPriority operatorPriority) : base()
        {
            Representation = representation;
            Priority = operatorPriority;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class KnownConstantAttribute : RepresentationAttribute
    {
        internal KnownConstantAttribute(string a, params string[] b) : base(a, b)
        {
        }
    }

    #endregion Attributes

    #region Expression + Inheriters

    #region Expression

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

    /// <summary>Abstract base class for mathematical expressions.</summary>
    public abstract class Expression
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public virtual Expression Simplify() => Clone();

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public virtual Expression Substitute(string variable, Expression expression) => Clone();

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <typeparam name="T">The type of value to substitute in for the variable.</typeparam>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="value">The value to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public Expression Substitute<T>(string variable, T value) => SubstitutionHack(variable, new Constant<T>(value));

        internal Expression SubstitutionHack(string variable, Expression expression) => Substitute(variable, expression);

        /// <summary>Derives this expression.</summary>
        /// <param name="variable">The variable to derive in relation to.</param>
        /// <returns>The result of the derivation.</returns>
#warning TODO

        public virtual Expression Derive(string variable) => throw new NotImplementedException("This feature is still in development");

        /// <summary>Integrates this expression.</summary>
        /// <param name="variable">The variable to integrate in relation to.</param>
        /// <returns>The result of the integration.</returns>
#warning TODO

        public virtual Expression Integrate(string variable) => throw new NotImplementedException("This feature is still in development");

        /// <summary>Creates a copy of the expression.</summary>
        /// <returns>A copy of the expression.</returns>
        public abstract Expression Clone();

        /// <summary>Negates an expression.</summary>
        /// <param name="a">The expression to negate.</param>
        /// <returns>The result of the negation.</returns>
        public static Expression operator -(Expression a) => new Negate(a);

        /// <summary>Adds two expressions.</summary>
        /// <param name="a">The first expression of the addition.</param>
        /// <param name="b">The second expression of the addition.</param>
        /// <returns>The result of the addition.</returns>
        public static Expression operator +(Expression a, Expression b) => new Add(a, b);

        /// <summary>Subtracts two expressions.</summary>
        /// <param name="a">The first expression of the subtraction.</param>
        /// <param name="b">The second expression of the subtraction.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Expression operator -(Expression a, Expression b) => new Subtract(a, b);

        /// <summary>Multiplies two expressions.</summary>
        /// <param name="a">The first expression of the multiplication.</param>
        /// <param name="b">The second expression of the multiplication.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Expression operator *(Expression a, Expression b) => new Multiply(a, b);

        /// <summary>Divides two expressions.</summary>
        /// <param name="a">The first expression of the division.</param>
        /// <param name="b">The second expression of the division.</param>
        /// <returns>The result of the division.</returns>
        public static Expression operator /(Expression a, Expression b) => new Divide(a, b);

        /// <summary>Wraps two expressions in an equality check.</summary>
        /// <param name="a">The left side of the equality.</param>
        /// <param name="b">The right side of the equality.</param>
        /// <returns>The expressions wrapped in an equality check.</returns>
        public static Expression operator ==(Expression a, Expression b) => new Equal(a, b);

        /// <summary>Wraps two expressions in an inequality check.</summary>
        /// <param name="a">The left side of the inequality.</param>
        /// <param name="b">The right side of the inequality.</param>
        /// <returns>The expressions wrapped in an inequality check.</returns>
        public static Expression operator !=(Expression a, Expression b) => new NotEqual(a, b);

        /// <summary>Wraps two expressions in a less than check.</summary>
        /// <param name="a">The left side of the less than.</param>
        /// <param name="b">The right side of the less than.</param>
        /// <returns>The expressions wrapped in an less than check.</returns>
        public static Expression operator <(Expression a, Expression b) => new LessThan(a, b);

        /// <summary>Wraps two expressions in a greater than check.</summary>
        /// <param name="a">The left side of the greater than.</param>
        /// <param name="b">The right side of the greater than.</param>
        /// <returns>The expressions wrapped in an greater than check.</returns>
        public static Expression operator >(Expression a, Expression b) => new GreaterThan(a, b);

        /// <summary>Takes one expression to the power of another.</summary>
        /// <param name="a">The first expression of the power operation.</param>
        /// <param name="b">The second expression of the power operation.</param>
        /// <returns>The result of the power operation.</returns>
        public static Expression operator ^(Expression a, Expression b) => new Power(a, b);
    }

    #endregion Expression

    #region Variable

    /// <summary>A variable in a symbolic mathematics expression.</summary>
    public class Variable : Expression
    {
        /// <summary>The name of the variable.</summary>
        public string Name { get; }

        /// <summary>Constructs a new variable.</summary>
        /// <param name="name">The name of the vairable.</param>
        public Variable(string name)
        { Name = name; }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Variable(Name);

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression)
        {
            if (Name == variable)
            {
                return expression.Clone();
            }
            else
            {
                return base.Substitute(variable, expression);
            }
        }

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "[" + Name + "]";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Variable)
            {
                return Name.Equals(b as Variable);
            }
            return false;
        }

        /// <summary>Standard hash function.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => Name.GetHashCode();
    }

    #endregion Variable

    #region Constant + Inheriters

    #region Constant

    /// <summary>Represents a constant numerical value.</summary>
    public abstract class Constant : Expression
    {
        /// <summary>True if this is a known constant value.</summary>
        public virtual bool IsKnownConstant => false;

        /// <summary>True if this numeric value is zero (0).</summary>
        public virtual bool IsZero => false;

        /// <summary>True if this numeric value is one (1).</summary>
        public virtual bool IsOne => false;

        /// <summary>True if this numeric value is two (2).</summary>
        public virtual bool IsTwo => false;

        /// <summary>True if this numeric value is three (3).</summary>
        public virtual bool IsThree => false;

        /// <summary>True if this numeric value is π (pi).</summary>
        public virtual bool IsPi => false;

        /// <summary>Determines if the constant is negative.</summary>
        public abstract bool IsNegative { get; }

        internal virtual Expression Simplify(Operation operation, params Expression[] operands)
        {
            return this;
        }

        internal static System.Collections.Generic.Dictionary<Type, Func<object, Expression>> preCompiledConstructors =
            new();

        internal static Expression BuildGeneric(object value)
        {
            Type valueType = value.GetType();
            if (preCompiledConstructors.TryGetValue(valueType, out var preCompiledConstructor))
            {
                return preCompiledConstructor(value);
            }
            else
            {
                Type constantType = typeof(Constant<>).MakeGenericType(valueType);
                ConstructorInfo? constructorInfo = constantType.GetConstructor(Ɐ(valueType));
                if (constructorInfo is null)
                {
                    throw new TowelBugException($"Encountered null {nameof(ConstructorInfo)} in {nameof(BuildGeneric)}.");
                }
                ParameterExpression A = System.Linq.Expressions.Expression.Parameter(typeof(object));
                NewExpression newExpression = System.Linq.Expressions.Expression.New(constructorInfo, System.Linq.Expressions.Expression.Convert(A, valueType));
                Func<object, Expression> newFunction = System.Linq.Expressions.Expression.Lambda<Func<object, Expression>>(newExpression, A).Compile();
                preCompiledConstructors.Add(valueType, newFunction);
                return newFunction(value);
            }
        }
    }

    #endregion Constant

    #region KnownConstantOfUnknownType + Inheriters

    #region KnownConstantOfUknownType

    /// <summary>Abstract base class for known constants of unknown types.</summary>
    public abstract class KnownConstantOfUnknownType : Constant
    {
        /// <summary>True if this numeric value is a known value.</summary>
        public override bool IsKnownConstant => true;

        internal abstract Constant<T> ApplyType<T>();
    }

    #endregion KnownConstantOfUknownType

    #region Pi

    /// <summary>Represents the π (pi).</summary>
    [KnownConstant("π")]
    public class Pi : KnownConstantOfUnknownType
    {
        /// <summary>Constructs a new instance of pi.</summary>
        public Pi() : base() { }

        /// <summary>True if this numeric value is π (pi).</summary>
        public override bool IsPi => true;

        /// <summary>Determines if the constant is negative.</summary>
        public override bool IsNegative => false;

        internal override Constant<T> ApplyType<T>() => new Pi<T>();

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Pi();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "π";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Pi)
            {
                return true;
            }
            return false;
        }

        /// <summary>The default hash code for this instance.</summary>
        /// <returns>The computed hash code.</returns>
        public override int GetHashCode() => HashCode;

        internal static readonly int HashCode = nameof(Pi).GetHashCode();
    }

    #endregion Pi

    #region Zero

    /// <summary>Represents zero (0).</summary>
    public class Zero : KnownConstantOfUnknownType
    {
        /// <summary>Constructs a new zero (0) value.</summary>
        public Zero() : base() { }

        /// <summary>True if this numeric value is zero (0).</summary>
        public override bool IsZero => true;

        /// <summary>Determines if the constant is negative.</summary>
        public override bool IsNegative => false;

        internal override Constant<T> ApplyType<T>() => new Zero<T>();

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Zero();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "0";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Zero)
            {
                return true;
            }
            return false;
        }

        internal static readonly int HashCode = nameof(Zero).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Zero

    #region One

    /// <summary>Represents the value of one (1).</summary>
    public class One : KnownConstantOfUnknownType
    {
        /// <summary>Constructs a new one (1) constant.</summary>
        public One() : base() { }

        /// <summary>True if this numeric value is one (1).</summary>
        public override bool IsOne => true;

        /// <summary>Determines if the constant is negative.</summary>
        public override bool IsNegative => false;

        internal override Constant<T> ApplyType<T>() => new One<T>();

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new One();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "1";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is One)
            {
                return true;
            }
            return false;
        }

        internal static readonly int HashCode = nameof(One).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion One

    #region Two

    /// <summary>Represents the value of two (2).</summary>
    public class Two : KnownConstantOfUnknownType
    {
        /// <summary>Constructs a new value of two (2).</summary>
        public Two() : base() { }

        /// <summary>True if this numeric value is two (2).</summary>
        public override bool IsTwo => true;

        /// <summary>Determines if the constant is negative.</summary>
        public override bool IsNegative => false;

        internal override Constant<T> ApplyType<T>() => new Two<T>();

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Two();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "2";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Two)
            {
                return true;
            }
            return false;
        }

        internal static readonly int HashCode = nameof(Two).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Two

    #region Three

    /// <summary>Represents the value of three (3).</summary>
    public class Three : KnownConstantOfUnknownType
    {
        /// <summary>Constructs a new value of three (3).</summary>
        public Three() : base() { }

        /// <summary>True if this numeric value is three (3).</summary>
        public override bool IsThree => true;

        /// <summary>Determines if the constant is negative.</summary>
        public override bool IsNegative => false;

        internal override Constant<T> ApplyType<T>() => new Three<T>();

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Three();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "3";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Three)
            {
                return true;
            }
            return false;
        }

        internal static readonly int HashCode = nameof(Three).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Three

    #endregion KnownConstantOfUnknownType + Inheriters

    #region Constant<T> + Inheriters

    #region Constant<T>

    /// <summary>Represents a numeric constant.</summary>
    /// <typeparam name="T">The generic type of the numeric value.</typeparam>
    public class Constant<T> : Constant
    {
        /// <summary>The value of this numeric constant.</summary>
        public readonly T Value;

        /// <summary>True if this numeric value is zero (0).</summary>
        public override bool IsZero => Equate(Value, JeezFoundation.Algorithm.Constant<T>.Zero);

        /// <summary>True if this numeric value is one (1).</summary>
        public override bool IsOne => Equate(Value, JeezFoundation.Algorithm.Constant<T>.One);

        /// <summary>True if this numeric value is two (2).</summary>
        public override bool IsTwo => Equate(Value, JeezFoundation.Algorithm.Constant<T>.Two);

        /// <summary>True if this numeric value is three (3).</summary>
        public override bool IsThree => Equate(Value, JeezFoundation.Algorithm.Constant<T>.Three);

        /// <summary>Determines if the constant is negative.</summary>
        public override bool IsNegative => IsNegative(Value);

        /// <summary>Constructs a new numeric constant.</summary>
        /// <param name="constant">The value of the numeric constant.</param>
        public Constant(T constant)
        { Value = constant; }

        internal override Expression Simplify(Operation operation, params Expression[] operands)
        {
            return operation.SimplifyHack<T>(operands);
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Constant<T>(Value);

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string? ToString() => Value?.ToString();

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Constant<T> B)
            {
                return Equate(Value, B.Value);
            }
            return false;
        }

        internal static readonly int HashCode = nameof(Constant<T>).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Constant<T>

    #region KnownConstantOfKnownType<T> + Inheriters

    #region KnownConstantOfKnownType<T>

    /// <summary>Abstract base class for known constants of unknown types.</summary>
    /// <typeparam name="T">The type of the known constant value.</typeparam>
    public abstract class KnownConstantOfKnownType<T> : Constant<T>
    {
        /// <summary>True if this numeric value is a known value.</summary>
        public override bool IsKnownConstant => true;

        /// <summary>Constructs a new constant of a known type.</summary>
        /// <param name="constant">The value of the known constant.</param>
        public KnownConstantOfKnownType(T constant) : base(constant) { }
    }

    #endregion KnownConstantOfKnownType<T>

    #region Pi<T>

    /// <summary>Represents the value of π (pi).</summary>
    /// <typeparam name="T">The generic type of the numeric.</typeparam>
    public class Pi<T> : KnownConstantOfKnownType<T>
    {
        /// <summary>Constructs a new value of π (pi).</summary>
        public Pi() : base(JeezFoundation.Algorithm.Constant<T>.Pi) { }

        /// <summary>True if the value is π (pi).</summary>
        public override bool IsPi => true;

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Pi<T>();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "π";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Pi<T>)
            {
                return true;
            }
            return false;
        }

        internal new static readonly int HashCode = nameof(Pi<T>).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Pi<T>

    #region Zero<T>

    /// <summary>Represents the value of zero (0).</summary>
    /// <typeparam name="T">The generic type of the numeric value.</typeparam>
    public class Zero<T> : KnownConstantOfKnownType<T>
    {
        /// <summary>Constructs a new zero (0) value.</summary>
        public Zero() : base(JeezFoundation.Algorithm.Constant<T>.Zero) { }

        /// <summary>True if the value is zero (0).</summary>
        public override bool IsZero => true;

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Zero<T>();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "0";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Zero<T>)
            {
                return true;
            }
            return false;
        }

        internal new static readonly int HashCode = nameof(Zero<T>).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Zero<T>

    #region One<T>

    /// <summary>Represents the value of one (1).</summary>
    /// <typeparam name="T">The generic type of the numeric value.</typeparam>
    public class One<T> : KnownConstantOfKnownType<T>
    {
        /// <summary>Constructs a new one (1) value.</summary>
        public One() : base(JeezFoundation.Algorithm.Constant<T>.One) { }

        /// <summary>True if the value is one (1).</summary>
        public override bool IsOne => true;

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new One<T>();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "1";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is One<T>)
            {
                return true;
            }
            return false;
        }

        internal new static readonly int HashCode = nameof(One<T>).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion One<T>

    #region Two<T>

    /// <summary>Represents the value of two (2).</summary>
    /// <typeparam name="T">The generic type of the numeric value.</typeparam>
    public class Two<T> : KnownConstantOfKnownType<T>
    {
        /// <summary>Constructs a new value of two (2).</summary>
        public Two() : base(JeezFoundation.Algorithm.Constant<T>.Two) { }

        /// <summary>True if the value is two (2).</summary>
        public override bool IsTwo => true;

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Two<T>();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "2";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Two<T>)
            {
                return true;
            }
            return false;
        }

        internal new static readonly int HashCode = nameof(Two<T>).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Two<T>

    #region Three<T>

    /// <summary>Represents the value of three (3).</summary>
    /// <typeparam name="T">The generic type of the numeric value.</typeparam>
    public class Three<T> : KnownConstantOfKnownType<T>
    {
        /// <summary>Constructs a new value of three.</summary>
        public Three() : base(JeezFoundation.Algorithm.Constant<T>.Three) { }

        /// <summary>True if the value is three (3).</summary>
        public override bool IsThree => true;

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Three<T>();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "3";

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is Three<T>)
            {
                return true;
            }
            return false;
        }

        internal new static readonly int HashCode = nameof(Three<T>).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion Three<T>

    #region True

    /// <summary>Represents the value of true.</summary>
    public class True : KnownConstantOfKnownType<bool>
    {
        /// <summary>Constructs a new value of true.</summary>
        public True() : base(true) { }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new True();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => true.ToString();

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is True)
            {
                return true;
            }
            return false;
        }

        internal new static readonly int HashCode = nameof(True).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion True

    #region False

    /// <summary>Represents the value of false.</summary>
    public class False : KnownConstantOfKnownType<bool>
    {
        /// <summary>Constructs a new false value.</summary>
        public False() : base(true) { }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new False();

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => false.ToString();

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is False)
            {
                return true;
            }
            return false;
        }

        internal new static readonly int HashCode = nameof(False).GetHashCode();

        /// <summary>The default hash code computation.</summary>
        /// <returns>The computed hash code for this instance.</returns>
        public override int GetHashCode() => HashCode;
    }

    #endregion False

    #endregion KnownConstantOfKnownType<T> + Inheriters

    #endregion Constant<T> + Inheriters

    #endregion Constant + Inheriters

    #region Operation + Inheriters

    #region Operation

    /// <summary>Abstract base class for all symbolic mathematics operations.</summary>
    public abstract class Operation : Expression
    {
        /// <summary>Interface for symbolic mathematics operations that involve numeric computation.</summary>
        public interface IMathematical
        { }

        /// <summary>Interface for symbolic mathematics operations that involve logical computation.</summary>
        public interface ILogical
        { }

        internal virtual Expression Simplify<T>(params Expression[] operands)
        {
            return this;
        }

        internal Expression SimplifyHack<T>(params Expression[] operands)
        {
            return Simplify<T>(operands);
        }
    }

    #endregion Operation

    #region Unary + Inheriters

    #region Unary

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

    /// <summary>Abstract base class for all symbolic mathematics unary operations.</summary>
    public abstract class Unary : Operation
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>The operand of th unary operation.</summary>
        public Expression A { get; set; }

        /// <summary>Constructs a new unary operation.</summary>
        /// <param name="a">The operand of the unary operation.</param>
        public Unary(Expression a) : base() { A = a; }

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is not null && GetType() == b.GetType())
            {
                return A.Equals(((Unary)b).A);
            }
            return false;
        }
    }

    #endregion Unary

    #region Simplification

    /// <summary>Represents a mathematical simplification operation.</summary>
    [Operation("Simplify")]
    public class Simplification : Unary
    {
        /// <summary>Constructs a new simplification operation.</summary>
        /// <param name="a">The expression to simplify.</param>
        public Simplification(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify() => A.Simplify();

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Simplification(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Simplification(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "Simplify(" + A + ")";
    }

    #endregion Simplification

    #region Negate

    /// <summary>Represents a negation operation.</summary>
    [LeftUnaryOperator("-", OperatorPriority.Negation)]
    public class Negate : Unary, Operation.IMathematical
    {
        /// <summary>Constructs a new negation operation.</summary>
        /// <param name="a">The expression to negate.</param>
        public Negate(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            #region Computation

            // Rule: [-A] => [B] where A is constant and B is -A
            if (OPERAND is Constant constant)
            {
                return constant.Simplify(this, OPERAND);
            }

            #endregion Computation

            return -OPERAND;
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a)
            {
                return new Constant<T>(Negation(a.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Negate(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Negate(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString()
        {
            if (A is not Constant && A is not Variable)
            {
                return "-(" + A + ")";
            }
            return "-" + A;
        }
    }

    #endregion Negate

    #region NaturalLog

    /// <summary>Represents a natural log operation.</summary>
    public class NaturalLog : Unary, Operation.IMathematical
    {
        /// <summary>Constructs a new natural log operation.</summary>
        /// <param name="a">The expression to compute the natrual log of.</param>
        public NaturalLog(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            #region Computation

            // Rule: [A] => [B] where A is constant and B is ln(A)
            if (OPERAND is Constant constant)
            {
                return constant.Simplify(this, OPERAND);
            }

            #endregion Computation

            return new NaturalLog(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a)
            {
                return new Constant<T>(NaturalLogarithm(a.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new NaturalLog(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new NaturalLog(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "ln(" + A + ")";
    }

    #endregion NaturalLog

    #region SquareRoot

    /// <summary>Represents a square root operation </summary>
    public class SquareRoot : Unary, Operation.IMathematical
    {
        /// <summary>Constructs a new square root operation.</summary>
        /// <param name="a">The expression to compute the square root of.</param>
        public SquareRoot(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            #region Computation

            // Rule: [A] => [B] where A is constant and B is sqrt(A)
            if (OPERAND is Constant constant)
            {
                return constant.Simplify(this, OPERAND);
            }

            #endregion Computation

            return new SquareRoot(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a)
            {
                return new Constant<T>(SquareRoot(a.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new SquareRoot(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new SquareRoot(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "√(" + A + ")";
    }

    #endregion SquareRoot

    #region Exponential

    /// <summary>Represents an exponential operation.</summary>
    public class Exponential : Unary, Operation.IMathematical
    {
        /// <summary>Constructs a new exponential operation.</summary>
        /// <param name="a">The expression to compute the exponetial function of.</param>
        public Exponential(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            #region Computation

            // Rule: [A] => [B] where A is constant and B is e ^ A
            if (OPERAND is Constant constant)
            {
                return constant.Simplify(this, constant);
            }

            #endregion Computation

            return new Exponential(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a)
            {
                return new Constant<T>(Exponential(a.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Exponential(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Exponential(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "e^(" + A + ")";
    }

    #endregion Exponential

    #region Factorial

    /// <summary>Represents a factorial operation.</summary>
    [RightUnaryOperator("!", OperatorPriority.Factorial)]
    public class Factorial : Unary, Operation.IMathematical
    {
        /// <summary>Constructs a new factorial operation.</summary>
        /// <param name="a">The operand of the factorial operation.</param>
        public Factorial(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            #region Computation

            // Rule: [A!] => [B] where A is constant and B is A!
            if (OPERAND is Constant constant)
            {
                return constant.Simplify(this, constant);
            }

            #endregion Computation

            return new Factorial(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a)
            {
                return new Constant<T>(Factorial(a.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Factorial(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Factorial(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString()
        {
            static bool RequiresParentheses(string expressionString)
            {
                foreach (char c in expressionString)
                {
                    if (!char.IsDigit(c) && c != '.')
                    {
                        return true;
                    }
                }
                return false;
            }

            string? a = A.ToString();
            if (a is not null && RequiresParentheses(a))
            {
                a = "(" + a + ")";
            }
            return a + "!";
        }
    }

    #endregion Factorial

    #region Invert

    /// <summary>Represents a reciprical/invert operation.</summary>
    public class Invert : Unary, Operation.IMathematical
    {
        /// <summary>Constructs a new reciprical/invert operation.</summary>
        /// <param name="a">Teh expression to recipricate/invert.</param>
        public Invert(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            #region Computation

            // Rule: [A] => [B] where A is constant and B is 1 / A
            if (OPERAND is Constant constant)
            {
                return constant.Simplify(this, OPERAND);
            }

            #endregion Computation

            return new Invert(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a)
            {
                return new Constant<T>(Inversion(a.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Invert(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Invert(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => "(1 / " + A + ")";
    }

    #endregion Invert

    #region Trigonometry + Inheriters

    #region Trigonometry

    /// <summary>Represents one of the trigonometry functions.</summary>
    public abstract class Trigonometry : Unary, Operation.IMathematical
    {
        /// <summary>Constructs a new trigonometry expression.</summary>
        /// <param name="a">The parameter to the trigonometry expression.</param>
        public Trigonometry(Expression a) : base(a) { }
    }

    #endregion Trigonometry

    #region Sine

    /// <summary>Represents the sine trigonometric function.</summary>
    public class Sine : Trigonometry, Operation.IMathematical
    {
        /// <summary>Constructs a new sine expression.</summary>
        /// <param name="a">The parameter to the sine expression.</param>
        public Sine(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            return new Sine(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
#warning TODO
            // if (A is Constant<T> a)
            // {
            //     //return new Constant<T>(Compute.Sine(a.Value));
            // }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Sine(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Sine(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => nameof(Sine) + "(" + A + ")";
    }

    #endregion Sine

    #region Cosine

    /// <summary>Represents the cosine trigonometric function.</summary>
    public class Cosine : Trigonometry, Operation.IMathematical
    {
        /// <summary>Constructs a new cosine expression.</summary>
        /// <param name="a">The parameter to the cosine expression.</param>
        public Cosine(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            return new Cosine(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
#warning TODO
            // if (A is Constant<T> a)
            // {
            //     //return new Constant<T>(Compute.Cosine(a.Value));
            // }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Cosine(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Cosine(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => nameof(Cosine) + "(" + A + ")";
    }

    #endregion Cosine

    #region Tangent

    /// <summary>Represents the tanget trigonometric function.</summary>
    public class Tangent : Trigonometry, Operation.IMathematical
    {
        /// <summary>Constructs a new tangent expression.</summary>
        /// <param name="a">The parameter to the tangent expression.</param>
        public Tangent(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            return new Tangent(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
#warning TODO
            // if (A is Constant<T> a)
            // {
            //     //return new Constant<T>(Compute.Tanget(a.Value));
            // }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Tangent(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Tangent(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => nameof(Tangent) + "(" + A + ")";
    }

    #endregion Tangent

    #region Cosecant

    /// <summary>Represents the cosecant trigonometric function.</summary>
    public class Cosecant : Trigonometry, Operation.IMathematical
    {
        /// <summary>Constructs a new cosecant expression.</summary>
        /// <param name="a">The parameter to the cosecant expression.</param>
        public Cosecant(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            return new Cosecant(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
#warning TODO
            // if (A is Constant<T> a)
            // {
            //     //return new Constant<T>(Compute.Cosecant(a.Value));
            // }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Cosecant(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Cosecant(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => nameof(Cosecant) + "(" + A + ")";
    }

    #endregion Cosecant

    #region Secant

    /// <summary>Represents the secant trigonometric function.</summary>
    public class Secant : Trigonometry, Operation.IMathematical
    {
        /// <summary>Constructs a new secant expression.</summary>
        /// <param name="a">The parameter to the secant expression.</param>
        public Secant(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            return new Secant(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
#warning TODO
            // if (A is Constant<T> a)
            // {
            //     //return new Constant<T>(Compute.Secant(a.Value));
            // }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Secant(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Secant(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => nameof(Secant) + "(" + A + ")";
    }

    #endregion Secant

    #region Cotangent

    /// <summary>Represents the cotangent trigonometric function.</summary>
    public class Cotangent : Trigonometry, Operation.IMathematical
    {
        /// <summary>Constructs a new cotangent expression.</summary>
        /// <param name="a">The parameter to the cotangent expression.</param>
        public Cotangent(Expression a) : base(a) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression OPERAND = A.Simplify();

            return new Cotangent(OPERAND);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
#warning TODO
            // if (A is Constant<T> a)
            // {
            //     //return new Constant<T>(Compute.Cotangent(a.Value));
            // }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Cotangent(A.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Cotangent(A.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => nameof(Cotangent) + "(" + A + ")";
    }

    #endregion Cotangent

    #endregion Trigonometry + Inheriters

    #endregion Unary + Inheriters

    #region Binary + Inheriters

    #region Binary

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

    /// <summary>Abstract base class for all symbolic mathematics binary operations.</summary>
    public abstract class Binary : Operation
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>The first operand of the binary operation.</summary>
        public Expression A { get; set; }

        /// <summary>The second operand of the binary operation.</summary>
        public Expression B { get; set; }

        /// <summary>Constructs a new binary operation.</summary>
        /// <param name="a">The left operand of the binary operation.</param>
        /// <param name="b">The right operand of the binary operation.</param>
        public Binary(Expression a, Expression b)
        {
            A = a;
            B = b;
        }

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is not null && GetType() == b.GetType())
            {
                return A.Equals(((Binary)b).A) && B.Equals(((Binary)b).B);
            }
            return false;
        }
    }

    #endregion Binary

    #region AddOrSubtract + Inheriters

    #region AddOrSubtract

    /// <summary>Represents an addition or a subtraction operation.</summary>
    public abstract class AddOrSubtract : Binary, Operation.IMathematical
    {
        /// <summary>Constructs a new addition or subtraction operation.</summary>
        /// <param name="a">The left operand of the operation.</param>
        /// <param name="b">The right operand of the operation.</param>
        public AddOrSubtract(Expression a, Expression b) : base(a, b) { }
    }

    #endregion AddOrSubtract

    #region Add

    /// <summary>Represents an addition operation.</summary>
    [BinaryOperator("+", OperatorPriority.Addition)]
    public class Add : AddOrSubtract
    {
        /// <summary>Constructs a new addition operation.</summary>
        /// <param name="a">The left operand of the addition operation.</param>
        /// <param name="b">The right operand of the addition operation.</param>
        public Add(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A + B] => [C] where A is constant, B is constant, and C is A + B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            #region Additive Identity Property

            {   // Rule: [X + 0] => [X]
                if (RIGHT is Constant right && right.IsZero)
                {
                    return LEFT;
                }
            }
            {   // Rule: [0 + X] => [X]
                if (LEFT is Constant left && left.IsZero)
                {
                    return RIGHT;
                }
            }

            #endregion Additive Identity Property

            #region Commutative/Associative Property

            {   // Rule: ['X + A' + B] => [X + C] where A is constant, B is constant, and C is A + B
                if (LEFT is Add ADD && ADD.B is Constant A && RIGHT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = ADD.A;
                    return X + C;
                }
            }
            {   // Rule: ['A + X' + B] => [X + C] where A is constant, B is constant, and C is A + B
                if (LEFT is Add ADD && ADD.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = ADD.B;
                    return X + C;
                }
            }
            {   // Rule: [B + 'X + A'] => [X + C] where A is constant, B is constant, and C is A + B
                if (RIGHT is Add ADD && ADD.B is Constant A && LEFT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = ADD.A;
                    return X + C;
                }
            }
            {   // Rule: [B + 'A + X'] => [X + C] where A is constant, B is constant, and C is A + B
                if (RIGHT is Add ADD && ADD.A is Constant A && LEFT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = ADD.B;
                    return X + C;
                }
            }
            {   // Rule: ['X - A' + B] => [X + C] where A is constant, B is constant, and C is B - A
                if (LEFT is Subtract SUBTRACT && SUBTRACT.B is Constant A && RIGHT is Constant B)
                {
                    var C = (B - A).Simplify();
                    var X = SUBTRACT.A;
                    return X + C;
                }
            }
            {   // Rule: ['A - X' + B] => [C - X] where A is constant, B is constant, and C is A + B
                if (LEFT is Subtract SUBTRACT && SUBTRACT.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = SUBTRACT.B;
                    return C - X;
                }
            }
            {   // Rule: [B + 'X - A'] => [X + C] where A is constant, B is constant, and C is B - A
                if (RIGHT is Subtract SUBTRACT && SUBTRACT.B is Constant A && LEFT is Constant B)
                {
                    var C = (B - A).Simplify();
                    var X = SUBTRACT.A;
                    return C + X;
                }
            }
            {   // Rule: [B + 'A - X'] => [C - X] where A is constant, B is constant, and C is A + B
                if (RIGHT is Subtract SUBTRACT && SUBTRACT.A is Constant A && LEFT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = SUBTRACT.B;
                    return C - X;
                }
            }

            #endregion Commutative/Associative Property

            return LEFT + RIGHT;
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<T>(Addition(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Add(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Add(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString()
        {
            string? a = A.ToString();
            string? b = B.ToString();
            {
                if ((A is Multiply || A is Divide) && A is Constant CONSTANT && CONSTANT.IsNegative)
                {
                    a = "(" + a + ")";
                }
            }
            {
                if (B is Add || B is Subtract || A is Multiply || A is Divide)
                {
                    b = "(" + b + ")";
                }
            }
            {
                if (B is Constant CONSTANT && CONSTANT.IsNegative)
                {
                    return a + " - " + Negation(B as Constant);
                }
            }
            return a + " + " + b;
        }
    }

    #endregion Add

    #region Subtract

    /// <summary>Represents a subtraction operation.</summary>
    [BinaryOperator("-", OperatorPriority.Subtraction)]
    public class Subtract : AddOrSubtract
    {
        /// <summary>Constructs a new subtraction operation.</summary>
        /// <param name="a">The left operand of the subtraction operation.</param>
        /// <param name="b">The right operand of the subtraction operation.</param>
        public Subtract(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A - B] => [C] where A is constant, B is constant, and C is A - B
                if (LEFT is Constant left && RIGHT is Constant right)
                {
                    return left.Simplify(this, left, right);
                }
            }

            #endregion Computation

            #region Identity Property

            {   // Rule: [X - 0] => [X]
                if (RIGHT is Constant right && right.IsZero)
                {
                    return LEFT;
                }
            }
            {   // Rule: [0 - X] => [-X]
                if (LEFT is Constant left && left.IsZero)
                {
                    return new Negate(RIGHT);
                }
            }

            #endregion Identity Property

            #region Commutative/Associative Property

            {   // Rule: ['X - A' - B] => [X - C] where A is constant, B is constant, and C is A + B
                if (LEFT is Subtract SUBTRACT && SUBTRACT.B is Constant A && RIGHT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = SUBTRACT.A;
                    return X - C;
                }
            }
            {    // Rule: ['A - X' - B] => [C - X] where A is constant, B is constant, and C is A - B
                if (LEFT is Subtract SUBTRACT && SUBTRACT.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A - B).Simplify();
                    var X = SUBTRACT.B;
                    return C - X;
                }
            }
            {   // Rule: [B - 'X - A'] => [C - X] where A is constant, B is constant, and C is B - A
                if (RIGHT is Subtract SUBTRACT && SUBTRACT.B is Constant A && LEFT is Constant B)
                {
                    var C = (B - A).Simplify();
                    var X = SUBTRACT.A;
                    return C - X;
                }
            }
            {   // Rule: [B - 'A - X'] => [C - X] where A is constant, B is constant, and C is B - A
                if (RIGHT is Subtract SUBTRACT && SUBTRACT.A is Constant A && LEFT is Constant B)
                {
                    var C = (B - A).Simplify();
                    var X = SUBTRACT.A;
                    return C - X;
                }
            }
            {   // Rule: ['X + A' - B] => [X + C] where A is constant, B is constant, and C is A - B
                if (LEFT is Add ADD && ADD.B is Constant A && RIGHT is Constant B)
                {
                    var C = (A - B).Simplify();
                    var X = ADD.A;
                    return X + C;
                }
            }
            {   // Rule: ['A + X' - B] => [C + X] where A is constant, B is constant, and C is A - B
                if (LEFT is Add ADD && ADD.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A - B).Simplify();
                    var X = ADD.B;
                    return C + X;
                }
            }
            {   // Rule: [B - 'X + A'] => [C - X] where A is constant, B is constant, and C is A + B
                if (RIGHT is Add ADD && ADD.B is Constant A && LEFT is Constant B)
                {
                    var C = (A + B).Simplify();
                    var X = ADD.A;
                    return C - X;
                }
            }
            {   // Rule: [B - 'A + X'] => [C + X] where A is constant, B is constant, and C is B - A
                if (RIGHT is Add ADD && ADD.A is Constant A && LEFT is Constant B)
                {
                    var C = (B - A).Simplify();
                    var X = ADD.B;
                    return C + X;
                }
            }

            #endregion Commutative/Associative Property

            return LEFT - RIGHT;
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<T>(Subtraction(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Subtract(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Subtract(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString()
        {
            string? a = A.ToString();
            if (A is Multiply || A is Divide)
            {
                a = "(" + a + ")";
            }
            string? b = B.ToString();
            if (B is Add || B is Subtract || A is Multiply || A is Divide)
            {
                b = "(" + b + ")";
            }
            return a + " - " + b;
        }
    }

    #endregion Subtract

    #endregion AddOrSubtract + Inheriters

    #region MultiplyOrDivide + Inheriters

    #region MultiplyOrDivide

    /// <summary>Abstract base class for multiplication and division operations.</summary>
    public abstract class MultiplyOrDivide : Binary, Operation.IMathematical
    {
        /// <summary>Constructs a new multiplication or division operation.</summary>
        /// <param name="a">The left operand of the operation.</param>
        /// <param name="b">The right operand of the operation.</param>
        public MultiplyOrDivide(Expression a, Expression b) : base(a, b) { }
    }

    #endregion MultiplyOrDivide

    #region Multiply

    /// <summary>Represents a multiplication operation.</summary>
    [BinaryOperator("*", OperatorPriority.Multiplication)]
    public class Multiply : MultiplyOrDivide
    {
        /// <summary>Constructs a new multiplication operation.</summary>
        /// <param name="a">The left operand of the multiplication operation.</param>
        /// <param name="b">The right operand of the multiplication operation.</param>
        public Multiply(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A * B] => [C] where A is constant, B is constant, and C is A * B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            #region Zero Property

            {   // Rule: [X * 0] => [0]
                if (RIGHT is Constant CONSTANT && CONSTANT.IsZero)
                {
                    return CONSTANT;
                }
            }
            {   // Rule: [0 * X] => [0]
                if (LEFT is Constant CONSTANT && CONSTANT.IsZero)
                {
                    return CONSTANT;
                }
            }

            #endregion Zero Property

            #region Identity Property

            {   // Rule: [X * 1] => [X]
                if (RIGHT is Constant CONSTANT && CONSTANT.IsOne)
                {
                    return LEFT;
                }
            }
            {   // Rule: [1 * X] => [X]
                if (LEFT is Constant CONSTANT && CONSTANT.IsOne)
                {
                    return RIGHT;
                }
            }

            #endregion Identity Property

            #region Commutative/Associative Property

            {   // Rule: [(X * A) * B] => [X * C] where A is constant, B is constant, and C is A * B
                if (LEFT is Multiply MULTIPLY && MULTIPLY.B is Constant A && RIGHT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = MULTIPLY.A;
                    return X * C;
                }
            }
            {   // Rule: [(A * X) * B] => [X * C] where A is constant, B is constant, and C is A * B
                if (LEFT is Multiply MULTIPLY && MULTIPLY.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = MULTIPLY.B;
                    return X * C;
                }
            }
            {   // Rule: [B * (X * A)] => [X * C] where A is constant, B is constant, and C is A * B
                if (RIGHT is Multiply MULTIPLY && MULTIPLY.B is Constant A && LEFT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = MULTIPLY.A;
                    return X * C;
                }
            }
            {   // Rule: [B * (A * X)] => [X * C] where A is constant, B is constant, and C is A * B
                if (RIGHT is Multiply MULTIPLY && MULTIPLY.A is Constant A && LEFT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = MULTIPLY.B;
                    return X * C;
                }
            }
            {   // Rule: [(X / A) * B] => [X * C] where A is constant, B is constant, and C is B / A
                if (LEFT is Divide DIVIDE && DIVIDE.B is Constant A && RIGHT is Constant B)
                {
                    var C = (B / A).Simplify();
                    var X = DIVIDE.A;
                    return X * C;
                }
            }
            {   // Rule: [(A / X) * B] => [C / X] where A is constant, B is constant, and C is A * B
                if (LEFT is Divide DIVIDE && DIVIDE.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = DIVIDE.B;
                    return C / X;
                }
            }
            {   // Rule: [B * (X / A)] => [X * C] where A is constant, B is constant, and C is B / A
                if (RIGHT is Divide DIVIDE && DIVIDE.B is Constant A && LEFT is Constant B)
                {
                    var C = (B / A).Simplify();
                    var X = DIVIDE.A;
                    return X * C;
                }
            }
            {   // Rule: [B * (A / X)] => [C / X] where A is constant, B is constant, and C is A * B
                if (RIGHT is Divide DIVIDE && DIVIDE.A is Constant A && LEFT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = DIVIDE.B;
                    return C / X;
                }
            }

            #endregion Commutative/Associative Property

            #region Distributive Property

            // {   // Rule: [X * (A +/- B)] => [X * A + X * B] where X is Variable
            //     if ((LEFT is Variable VARIABLE && RIGHT is AddOrSubtract ADDORSUBTRACT))
            //     {
            //         // This might not be necessary
            //     }
            // }
            // {   // Rule: [(A +/- B) * X] => [X * A + X * B] where X is Variable
            //     if ((RIGHT is Variable VARIABLE && LEFT is AddOrSubtract ADDORSUBTRACT))
            //     {
            //         // This might not be necessary
            //     }
            // }

            #endregion Distributive Property

            #region Duplicate Variable Multiplications

            {   // Rule: [X * X] => [X ^ 2] where X is Variable
                if (LEFT is Variable X1 && RIGHT is Variable X2 && X1.Name == X2.Name)
                {
                    return X1 ^ new Two();
                }
            }

            #endregion Duplicate Variable Multiplications

            #region Multiplication With Powered Variables

            {   // Rule: [(V ^ A) * (V ^ B)] => [V ^ C] where A is constant, B is constant, V is a variable, and C is A + B
                if (LEFT is Power POWER1 && RIGHT is Power POWER2 &&
                    POWER1.A is Variable V1 && POWER2.A is Variable V2 && V1.Name == V2.Name &&
                    POWER1.B is Constant A && POWER2.B is Constant B)
                {
                    var C = (A + B).Simplify();
                    return V1 ^ C;
                }
            }

            #endregion Multiplication With Powered Variables

            return LEFT * RIGHT;
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<T>(Multiplication(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Multiply(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Multiply(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString()
        {
            string? a = A.ToString();
            string? b = B.ToString();
            if (B is Multiply || B is Divide)
            {
                b = "(" + b + ")";
            }
            else if (A is Constant a_const && a_const.IsKnownConstant && B is Constant)
            {
                return b + a;
            }
            else if (A is Constant && B is Constant b_const && b_const.IsKnownConstant)
            {
                return a + b;
            }
            return a + " * " + b;
        }
    }

    #endregion Multiply

    #region Divide

    /// <summary>Represents a division operation.</summary>
    [BinaryOperator("/", OperatorPriority.Division)]
    public class Divide : MultiplyOrDivide
    {
        /// <summary>Constructs a new division operation.</summary>
        /// <param name="a">The left operand of the division operation.</param>
        /// <param name="b">The right operand of the division operation.</param>
        public Divide(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Error Handling

            {   // Rule: [X / 0] => Error
                if (RIGHT is Constant CONSTANT && CONSTANT.IsZero)
                {
                    throw new DivideByZeroException();
                }
            }

            #endregion Error Handling

            #region Computation

            {   // Rule: [A / B] => [C] where A is constant, B is constant, and C is A / B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            #region Zero Property

            {   // Rule: [0 / X] => [0]
                if (LEFT is Constant CONSTANT && CONSTANT.IsZero)
                {
                    return CONSTANT;
                }
            }

            #endregion Zero Property

            #region Identity Property

            {   // Rule: [X / 1] => [X]
                if (RIGHT is Constant CONSTANT && CONSTANT.IsOne)
                {
                    return LEFT;
                }
            }

            #endregion Identity Property

            #region Commutative/Associative Property

            {   // Rule: [(X / A) / B] => [X / C] where A is constant, B is constant, and C is A * B
                if (LEFT is Divide DIVIDE && DIVIDE.B is Constant A && RIGHT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = DIVIDE.A;
                    return X / C;
                }
            }
            {   // Rule: [(A / X) / B] => [C / X] where A is constant, B is constant, and C is A / B
                if (LEFT is Divide DIVIDE && DIVIDE.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A / B).Simplify();
                    var X = DIVIDE.B;
                    return C / X;
                }
            }
            {   // Rule: [B / (X / A)] => [C / X] where A is constant, B is constant, and C is B / A
                if (RIGHT is Divide DIVIDE && DIVIDE.B is Constant A && LEFT is Constant B)
                {
                    var C = (B / A).Simplify();
                    var X = DIVIDE.A;
                    return C / X;
                }
            }
            {   // Rule: [B / (A / X)] => [C / X] where A is constant, B is constant, and C is B / A
                if (RIGHT is Divide DIVIDE && DIVIDE.A is Constant A && LEFT is Constant B)
                {
                    var C = (B / A).Simplify();
                    var X = DIVIDE.B;
                    return C / X;
                }
            }
            {   // Rule: [(X * A) / B] => [X * C] where A is constant, B is constant, and C is A / B
                if (LEFT is Multiply MULTIPLY && MULTIPLY.B is Constant A && RIGHT is Constant B)
                {
                    var C = (A / B).Simplify();
                    var X = MULTIPLY.A;
                    return X * C;
                }
            }
            {   // Rule: [(A * X) / B] => [X * C] where A is constant, B is constant, and C is A / B
                if (LEFT is Multiply MULTIPLY && MULTIPLY.A is Constant A && RIGHT is Constant B)
                {
                    var C = (A / B).Simplify();
                    var X = MULTIPLY.B;
                    return X * C;
                }
            }
            {   // Rule: [B / (X * A)] => [C / X] where A is constant, B is constant, and C is A * B
                if (RIGHT is Multiply MULTIPLY && MULTIPLY.B is Constant A && LEFT is Constant B)
                {
                    var C = (A * B).Simplify();
                    var X = MULTIPLY.A;
                    return C / X;
                }
            }
            {   // Rule: [B / (A * X)] => [X * C] where A is constant, B is constant, and C is B / A
                if (RIGHT is Multiply MULTIPLY && MULTIPLY.A is Constant A && LEFT is Constant B)
                {
                    var C = (B / A).Simplify();
                    var X = MULTIPLY.B;
                    return X * C;
                }
            }

            #endregion Commutative/Associative Property

            #region Distributive Property

            // {   // Rule: [X / (A +/- B)] => [X / A + X / B] where where A is constant, B is constant, and X is Variable
            //     if ((LEFT is Variable VARIABLE && RIGHT is AddOrSubtract ADDORSUBTRACT))
            //     {
            //         // This might not be necessary
            //     }
            // }
            // {   // Rule: [(A +/- B) / X] => [(A / X) + (B / X)] where where A is constant, B is constant, and X is Variable
            //     if ((RIGHT is Variable VARIABLE && LEFT is AddOrSubtract ADDORSUBTRACT))
            //     {
            //         // This might not be necessary
            //     }
            // }

            #endregion Distributive Property

            #region Division With Powered Variables

            {   // Rule: [(V ^ A) / (V ^ B)] => [V ^ C] where A is constant, B is constant, V is a variable, and C is A - B
                if (LEFT is Power POWER1 && RIGHT is Power POWER2 &&
                    POWER1.A is Variable V1 && POWER2.A is Variable V2 && V1.Name == V2.Name &&
                    POWER1.B is Constant A && POWER2.B is Constant B)
                {
                    var C = (A - B).Simplify();
                    return V1 ^ C;
                }
            }

            #endregion Division With Powered Variables

            return LEFT / RIGHT;
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<T>(Division(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Divide(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Divide(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString()
        {
            string? a = A.ToString();
            string? b = B.ToString();
            if (B is Multiply || B is Divide)
            {
                b = "(" + b + ")";
            }
            return a + " / " + b;
        }
    }

    #endregion Divide

    #endregion MultiplyOrDivide + Inheriters

    #region Power

    /// <summary>Represents a power operation.</summary>
    [BinaryOperator("^", OperatorPriority.Exponents)]
    public class Power : Binary, Operation.IMathematical
    {
        /// <summary>Constructs a new power operation.</summary>
        /// <param name="a">The left operand of the power operation.</param>
        /// <param name="b">The right operand of the power operation.</param>
        public Power(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A ^ B] => [C] where A is constant, B is constant, and C is A ^ B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            #region Zero Base

            {   // Rule: [0 ^ X] => [0]
                if (LEFT is Constant CONSTANT && CONSTANT.IsZero)
                {
                    return CONSTANT;
                }
            }

            #endregion Zero Base

            #region One Power

            {   // Rule: [X ^ 1] => [X]
                if (RIGHT is Constant CONSTANT && CONSTANT.IsOne)
                {
                    return LEFT;
                }
            }

            #endregion One Power

            #region Zero Power

            {   // Rule: [X ^ 0] => [1]
                if (RIGHT is Constant CONSTANT && CONSTANT.IsZero)
                {
                    return new One();
                }
            }

            #endregion Zero Power

            return LEFT ^ RIGHT;
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<T>(Power(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Power(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Power(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString()
        {
            static bool RequiresParentheses(string expressionString)
            {
                foreach (char c in expressionString)
                {
                    if (!char.IsDigit(c) && c != '.')
                    {
                        return true;
                    }
                }
                return false;
            }

            string? a = A.ToString();
            string? b = B.ToString();
            if (a is not null && RequiresParentheses(a))
            {
                a = "(" + a + ")";
            }
            if (b is not null && RequiresParentheses(b))
            {
                b = "(" + b + ")";
            }
            return a + " ^ " + b;
        }
    }

    #endregion Power

    #region Root

    /// <summary>Represents a root operation.</summary>
    public class Root : Binary, Operation.IMathematical
    {
        /// <summary>Constructs a new root operation.</summary>
        /// <param name="a">The base of the root operation.</param>
        /// <param name="b">The root (inverted exponent) value of the operation.</param>
        public Root(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A ^ (1 / B)] => [C] where A is constant, B is constant, and C is A ^ (1 / B)
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            return new Root(LEFT, RIGHT);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<T>(Root(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Root(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Root(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => A + " ^ (1 / " + B + ")";
    }

    #endregion Root

    #region Equal

    /// <summary>Represents an equality operation between two expressions.</summary>
    [BinaryOperator("=", OperatorPriority.Logical)]
    public class Equal : Binary, Operation.ILogical
    {
        /// <summary>Constructs a new equality operation between two expressions.</summary>
        /// <param name="a">The left expression of the equality.</param>
        /// <param name="b">The right expression of the equality.</param>
        public Equal(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A == B] => [C] where A is constant, B is constant, and C is A == B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            return LEFT == RIGHT;
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<bool>(Equate(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new Equal(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new Equal(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => A + " = " + B;
    }

    #endregion Equal

    #region NotEqual

    /// <summary>Represents an equality operation between two expressions.</summary>
    [BinaryOperator("≠", OperatorPriority.Logical)]
    public class NotEqual : Binary, Operation.ILogical
    {
        /// <summary>Constructs a new inequality operation between two expressions.</summary>
        /// <param name="a">The left expression of the inequality.</param>
        /// <param name="b">The right expression of the inequality.</param>
        public NotEqual(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A == B] => [C] where A is constant, B is constant, and C is A != B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            return new NotEqual(LEFT, RIGHT);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<bool>(Inequate(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new NotEqual(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new NotEqual(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => A + " ≠ " + B;
    }

    #endregion NotEqual

    #region LessThan

    /// <summary>Represents a less than operation.</summary>
    [BinaryOperator("<", OperatorPriority.Logical)]
    public class LessThan : Binary, Operation.ILogical
    {
        /// <summary>Constructs a new less than operation.</summary>
        /// <param name="a">The left expression of the less than operation.</param>
        /// <param name="b">The right expression of the less than operation.</param>
        public LessThan(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A == B] => [C] where A is constant, B is constant, and C is A < B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            return new LessThan(LEFT, RIGHT);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<bool>(LessThan(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new LessThan(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new LessThan(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => A + " < " + B;
    }

    #endregion LessThan

    #region GreaterThan

    /// <summary>Represents a greater than operation.</summary>
    [BinaryOperator(">", OperatorPriority.Logical)]
    public class GreaterThan : Binary, Operation.ILogical
    {
        /// <summary>Constructs a new greater than operation.</summary>
        /// <param name="a">The left expression of the greater than operation.</param>
        /// <param name="b">The right expression of the greater than operation.</param>
        public GreaterThan(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A == B] => [C] where A is constant, B is constant, and C is A > B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            return new GreaterThan(LEFT, RIGHT);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<bool>(GreaterThan(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new GreaterThan(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new GreaterThan(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => A + " < " + B;
    }

    #endregion GreaterThan

    #region LessThanOrEqual

    /// <summary>Represents a less than or equal to operation.</summary>
    [BinaryOperator("<=", OperatorPriority.Logical)]
    public class LessThanOrEqual : Binary, Operation.ILogical
    {
        /// <summary>Constructs a new less than or equal to operation.</summary>
        /// <param name="a">The left expression of the less than or equal to operation.</param>
        /// <param name="b">The right expression of the less than or equal to operation.</param>
        public LessThanOrEqual(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A == B] => [C] where A is constant, B is constant, and C is A <= B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            return new LessThanOrEqual(LEFT, RIGHT);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<bool>(LessThanOrEqual(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new LessThanOrEqual(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new LessThanOrEqual(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => A + " < " + B;
    }

    #endregion LessThanOrEqual

    #region GreaterThanOrEqual

    /// <summary>Represents a greater than or equal to operation.</summary>
    [BinaryOperator(">=", OperatorPriority.Logical)]
    public class GreaterThanOrEqual : Binary, Operation.ILogical
    {
        /// <summary>Constructs a new greater than or equal to operation.</summary>
        /// <param name="a">The left expression of the greater than or equal to operation.</param>
        /// <param name="b">The right expression of the greater than or equal to operation.</param>
        public GreaterThanOrEqual(Expression a, Expression b) : base(a, b) { }

        /// <summary>Simplifies the mathematical expression.</summary>
        /// <returns>The simplified mathematical expression.</returns>
        public override Expression Simplify()
        {
            Expression LEFT = A.Simplify();
            Expression RIGHT = B.Simplify();

            #region Computation

            {   // Rule: [A == B] => [C] where A is constant, B is constant, and C is A >= B
                if (LEFT is Constant A && RIGHT is Constant B)
                {
                    return A.Simplify(this, A, B);
                }
            }

            #endregion Computation

            return new GreaterThanOrEqual(LEFT, RIGHT);
        }

        internal override Expression Simplify<T>(params Expression[] operands)
        {
            if (operands[0] is Constant<T> a && operands[1] is Constant<T> b)
            {
                return new Constant<bool>(GreaterThanOrEqual(a.Value, b.Value));
            }
            return base.Simplify<T>();
        }

        /// <summary>Clones this expression.</summary>
        /// <returns>A clone of this expression.</returns>
        public override Expression Clone() => new GreaterThanOrEqual(A.Clone(), B.Clone());

        /// <summary>Substitutes an expression for all occurences of a variable.</summary>
        /// <param name="variable">The variable to be substititued.</param>
        /// <param name="expression">The expression to substitute for each occurence of a variable.</param>
        /// <returns>The resulting expression of the substitution.</returns>
        public override Expression Substitute(string variable, Expression expression) =>
            new GreaterThanOrEqual(A.Substitute(variable, expression), B.Substitute(variable, expression));

        /// <summary>Standard conversion to a string representation.</summary>
        /// <returns>The string represnetation of this expression.</returns>
        public override string ToString() => A + " < " + B;
    }

    #endregion GreaterThanOrEqual

    #endregion Binary + Inheriters

    #region Ternary + Inheriters

    #region Ternary

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

    /// <summary>Abstract base class for ternary operations.</summary>
    public abstract class Ternary : Operation
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>The first operand of the ternary operation.</summary>
        public Expression A { get; set; }

        /// <summary>The second operand of the ternary operation.</summary>
        public Expression B { get; set; }

        /// <summary>The third operand of the ternary operation.</summary>
        public Expression C { get; set; }

        /// <summary>Constructs a new ternary operation.</summary>
        /// <param name="a">The first operand of the ternary operation.</param>
        /// <param name="b">The second operand of the ternary operation.</param>
        /// <param name="c">The third operand of the ternary operation.</param>
        public Ternary(Expression a, Expression b, Expression c)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>Standard equality check.</summary>
        /// <param name="b">The object to check for equality with.</param>
        /// <returns>True if equal. False if not.</returns>
        public override bool Equals(object? b)
        {
            if (b is not null && GetType() == b.GetType())
            {
                return A.Equals(((Ternary)b).A) && B.Equals(((Ternary)b).B) && C.Equals(((Ternary)b).C);
            }
            return false;
        }
    }

    #endregion Ternary

    #endregion Ternary + Inheriters

    #region Multinary + Inheriters

    #region Multinary

    /// <summary>Abstract base class for multinary operations.</summary>
    public abstract class Multinary : Operation
    {
        /// <summary>The operands of the multinary operation.</summary>
        public Expression[] Operands { get; set; }

        /// <summary>Constructs a new multinary operation.</summary>
        /// <param name="operands">The operands of the multinary operation.</param>
        public Multinary(Expression[] operands)
        {
            Operands = operands;
        }
    }

    #endregion Multinary

    #endregion Multinary + Inheriters

    #endregion Operation + Inheriters

    #endregion Expression + Inheriters

    #region Parsers

    // Notes:
    // Parsing uses the Expression class hierarchy with the class atributes to build
    // a library of parsing constants via reflection. Once built, the parsing library
    // is used as a reference to contruct the proper Expression type via a contruction
    // delegate.

    #region Runtime Built Parsing Libary

    // Library Building Fields
    internal static bool ParseableLibraryBuilt;

    internal static readonly object ParseableLibraryLock = new();

    // Regex Expressions
    internal const string ParenthesisPattern = @"\(.*\)";

    internal static string? ParsableOperationsRegexPattern;
    internal static string? ParsableOperatorsRegexPattern;
    internal static string? ParsableKnownConstantsRegexPattern;
    internal static string? SpecialStringsPattern;

    // Operation Refrences
    internal static System.Collections.Generic.Dictionary<string, Func<Expression, Unary>>? ParsableUnaryOperations;

    internal static System.Collections.Generic.Dictionary<string, Func<Expression, Expression, Binary>>? ParsableBinaryOperations;
    internal static System.Collections.Generic.Dictionary<string, Func<Expression, Expression, Expression, Ternary>>? ParsableTernaryOperations;
    internal static System.Collections.Generic.Dictionary<string, Func<Expression[], Multinary>>? ParsableMultinaryOperations;

    // Operator References
    internal static System.Collections.Generic.Dictionary<string, (OperatorPriority, Func<Expression, Unary>)>? ParsableLeftUnaryOperators;

    internal static System.Collections.Generic.Dictionary<string, (OperatorPriority, Func<Expression, Unary>)>? ParsableRightUnaryOperators;
    internal static System.Collections.Generic.Dictionary<string, (OperatorPriority, Func<Expression, Expression, Binary>)>? ParsableBinaryOperators;

    // Known Constant References
    internal static System.Collections.Generic.Dictionary<string, Func<KnownConstantOfUnknownType>>? ParsableKnownConstants;

    #region Reflection Code (Actually Building the Parsing Library)

    internal static void BuildParsableOperationLibrary()
    {
        lock (ParseableLibraryLock)
        {
            if (ParseableLibraryBuilt)
            {
                return;
            }

            // Unary Operations
            ParsableUnaryOperations = new System.Collections.Generic.Dictionary<string, Func<Expression, Unary>>();
            ParsableLeftUnaryOperators = new System.Collections.Generic.Dictionary<string, (OperatorPriority, Func<Expression, Unary>)>();
            ParsableRightUnaryOperators = new System.Collections.Generic.Dictionary<string, (OperatorPriority, Func<Expression, Unary>)>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetDerivedTypes<Unary>().Where(x => !x.IsAbstract))
            {
                ConstructorInfo? constructorInfo = type.GetConstructor(Ɐ(typeof(Expression)));
                if (constructorInfo is null)
                {
                    throw new TowelBugException($"Could not find a {nameof(ConstructorInfo)} when building parsing library");
                }
                ParameterExpression A = System.Linq.Expressions.Expression.Parameter(typeof(Expression));
                NewExpression newExpression = System.Linq.Expressions.Expression.New(constructorInfo, A);
                Func<Expression, Unary> newFunction = System.Linq.Expressions.Expression.Lambda<Func<Expression, Unary>>(newExpression, A).Compile();
                string operationName = type.Name;
                if (operationName.Contains('+'))
                {
                    int index = operationName.LastIndexOf('+');
                    operationName = operationName[(index + 1)..];
                }
                ParsableUnaryOperations.Add(operationName.ToLower(), newFunction);
                OperationAttribute? operationAttribute = type.GetCustomAttribute<OperationAttribute>();
                if (operationAttribute is not null)
                {
                    foreach (string representation in operationAttribute.Representations)
                    {
                        ParsableUnaryOperations.Add(representation.ToLower(), newFunction);
                    }
                }

                // Left Unary Operators
                foreach (LeftUnaryOperatorAttribute @operator in type.GetCustomAttributes<LeftUnaryOperatorAttribute>())
                {
                    ParsableLeftUnaryOperators.Add(@operator.Representation.ToLower(), (@operator.Priority, newFunction));
                }

                // Right Unary Operators
                foreach (RightUnaryOperatorAttribute @operator in type.GetCustomAttributes<RightUnaryOperatorAttribute>())
                {
                    ParsableRightUnaryOperators.Add(@operator.Representation.ToLower(), (@operator.Priority, newFunction));
                }
            }

            // Binary Operations
            ParsableBinaryOperations = new System.Collections.Generic.Dictionary<string, Func<Expression, Expression, Binary>>();
            ParsableBinaryOperators = new System.Collections.Generic.Dictionary<string, (OperatorPriority, Func<Expression, Expression, Binary>)>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetDerivedTypes<Binary>().Where(x => !x.IsAbstract))
            {
                ConstructorInfo? constructorInfo = type.GetConstructor(Ɐ(typeof(Expression), typeof(Expression)));
                if (constructorInfo is null)
                {
                    throw new TowelBugException($"Could not find a {nameof(ConstructorInfo)} when building parsing library");
                }
                ParameterExpression A = System.Linq.Expressions.Expression.Parameter(typeof(Expression));
                ParameterExpression B = System.Linq.Expressions.Expression.Parameter(typeof(Expression));
                NewExpression newExpression = System.Linq.Expressions.Expression.New(constructorInfo, A, B);
                Func<Expression, Expression, Binary> newFunction = System.Linq.Expressions.Expression.Lambda<Func<Expression, Expression, Binary>>(newExpression, A, B).Compile();
                string operationName = type.Name;
                if (operationName.Contains('+'))
                {
                    int index = operationName.LastIndexOf('+');
                    operationName = operationName[(index + 1)..];
                }
                ParsableBinaryOperations.Add(operationName.ToLower(), newFunction);
                OperationAttribute? operationAttribute = type.GetCustomAttribute<OperationAttribute>();
                if (operationAttribute is not null)
                {
                    foreach (string representation in operationAttribute.Representations)
                    {
                        ParsableBinaryOperations.Add(representation.ToLower(), newFunction);
                    }
                }

                // Binary Operators
                foreach (BinaryOperatorAttribute @operator in type.GetCustomAttributes<BinaryOperatorAttribute>())
                {
                    ParsableBinaryOperators.Add(@operator.Representation.ToLower(), (@operator.Priority, newFunction));
                }
            }

            // Ternary Operations
            ParsableTernaryOperations = new System.Collections.Generic.Dictionary<string, Func<Expression, Expression, Expression, Ternary>>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetDerivedTypes<Ternary>().Where(x => !x.IsAbstract))
            {
                ConstructorInfo? constructorInfo = type.GetConstructor(Ɐ(typeof(Expression), typeof(Expression), typeof(Expression)));
                if (constructorInfo is null)
                {
                    throw new TowelBugException($"Could not find a {nameof(ConstructorInfo)} when building parsing library");
                }
                ParameterExpression A = System.Linq.Expressions.Expression.Parameter(typeof(Expression));
                ParameterExpression B = System.Linq.Expressions.Expression.Parameter(typeof(Expression));
                ParameterExpression C = System.Linq.Expressions.Expression.Parameter(typeof(Expression));
                NewExpression newExpression = System.Linq.Expressions.Expression.New(constructorInfo, A, B, C);
                Func<Expression, Expression, Expression, Ternary> newFunction = System.Linq.Expressions.Expression.Lambda<Func<Expression, Expression, Expression, Ternary>>(newExpression, A, B, C).Compile();
                string operationName = type.Name;
                if (operationName.Contains('+'))
                {
                    int index = operationName.LastIndexOf('+');
                    operationName = operationName[(index + 1)..];
                }
                ParsableTernaryOperations.Add(operationName.ToLower(), newFunction);
                OperationAttribute? operationAttribute = type.GetCustomAttribute<OperationAttribute>();
                if (operationAttribute is not null)
                {
                    foreach (string representation in operationAttribute.Representations)
                    {
                        ParsableTernaryOperations.Add(representation.ToLower(), newFunction);
                    }
                }
            }

            // Multinary Operations
            ParsableMultinaryOperations = new System.Collections.Generic.Dictionary<string, Func<Expression[], Multinary>>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetDerivedTypes<Multinary>().Where(x => !x.IsAbstract))
            {
                ConstructorInfo? constructorInfo = type.GetConstructor(Ɐ(typeof(Expression[])));
                if (constructorInfo is null)
                {
                    throw new TowelBugException($"Could not find a {nameof(ConstructorInfo)} when building parsing library");
                }
                ParameterExpression A = System.Linq.Expressions.Expression.Parameter(typeof(Expression[]));
                NewExpression newExpression = System.Linq.Expressions.Expression.New(constructorInfo, A);
                Func<Expression[], Multinary> newFunction = System.Linq.Expressions.Expression.Lambda<Func<Expression[], Multinary>>(newExpression, A).Compile();
                string operationName = type.Name;
                if (operationName.Contains('+'))
                {
                    int index = operationName.LastIndexOf('+');
                    operationName = operationName[(index + 1)..];
                }
                ParsableMultinaryOperations.Add(operationName.ToLower(), newFunction);
                OperationAttribute? operationAttribute = type.GetCustomAttribute<OperationAttribute>();
                if (operationAttribute is not null)
                {
                    foreach (string representation in operationAttribute.Representations)
                    {
                        ParsableMultinaryOperations.Add(representation.ToLower(), newFunction);
                    }
                }
            }

            // Known Constants
            ParsableKnownConstants = new System.Collections.Generic.Dictionary<string, Func<KnownConstantOfUnknownType>>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetDerivedTypes<KnownConstantOfUnknownType>().Where(x => !x.IsAbstract))
            {
                ConstructorInfo? constructorInfo = type.GetConstructor(Type.EmptyTypes);
                if (constructorInfo is null)
                {
                    throw new TowelBugException($"Could not find a {nameof(ConstructorInfo)} when building parsing library");
                }
                NewExpression newExpression = System.Linq.Expressions.Expression.New(constructorInfo);
                Func<KnownConstantOfUnknownType> newFunction = System.Linq.Expressions.Expression.Lambda<Func<KnownConstantOfUnknownType>>(newExpression).Compile();
                // string knownConstant = type.ConvertToCsharpSource();
                // if (knownConstant.Contains('+'))
                // {
                //     int index = knownConstant.LastIndexOf('+');
                //     knownConstant = knownConstant.Substring(index + 1);
                // }
                // ParsableKnownConstants.Add(knownConstant.ToLower(), newFunction);
                KnownConstantAttribute? knownConstantAttribute = type.GetCustomAttribute<KnownConstantAttribute>();
                if (knownConstantAttribute is not null)
                {
                    foreach (string representation in knownConstantAttribute.Representations)
                    {
                        ParsableKnownConstants.Add(representation.ToLower(), newFunction);
                    }
                }
            }

            // Build a regex to match any operation
            System.Collections.Generic.IEnumerable<string> operations =
                ParsableUnaryOperations.Keys.Concat(
                    ParsableBinaryOperations.Keys.Concat(
                        ParsableTernaryOperations.Keys.Concat(
                            ParsableMultinaryOperations.Keys))).Select(x => Regex.Escape(x));
            ParsableOperationsRegexPattern = string.Join(@"\s*\(.*\)|", operations) + @"\s *\(.*\)";

            // Build a regex to match any operator
            System.Collections.Generic.IEnumerable<string> operators =
                ParsableLeftUnaryOperators.Keys.Concat(
                    ParsableRightUnaryOperators.Keys.Concat(
                        ParsableBinaryOperators.Keys)).Select(x => Regex.Escape(x));
            ParsableOperatorsRegexPattern = string.Join("|", operators);

            System.Collections.Generic.IEnumerable<string> knownConstants =
                ParsableKnownConstants.Keys.Select(x => Regex.Escape(x));
            ParsableKnownConstantsRegexPattern = string.Join("|", knownConstants);

            SpecialStringsPattern = string.Join("|", operators.Append(Regex.Escape("(")).Append(Regex.Escape(")")));

            ParseableLibraryBuilt = true;
        }
    }

    #endregion Reflection Code (Actually Building the Parsing Library)

    #endregion Runtime Built Parsing Libary

    #region System.Linq.Expression

    /// <summary>Parses a mathematical expression from a linq expression.</summary>
    /// <param name="e">The linq expression to parse.</param>
    /// <returns>The parsed symbolic mathematics linq expression.</returns>
    public static Expression Parse(System.Linq.Expressions.Expression e)
    {
        Expression ParseRecursive(System.Linq.Expressions.Expression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.Lambda => ParseRecursive(((LambdaExpression)expression).Body),
                ExpressionType.Constant => Constant.BuildGeneric(((ConstantExpression)expression).Value ?? throw new ArgumentException(paramName: nameof(e), message: "contains null value")),
                ExpressionType.Parameter => new Variable(((ParameterExpression)expression).Name ?? throw new ArgumentException(paramName: nameof(e), message: "contains null parameter name")),
                ExpressionType.Negate => new Negate(ParseRecursive(((UnaryExpression)expression).Operand)),
                ExpressionType.UnaryPlus => ParseRecursive(((UnaryExpression)expression).Operand),
                ExpressionType.Add => new Add(ParseRecursive(((BinaryExpression)expression).Left), ParseRecursive(((BinaryExpression)expression).Right)),
                ExpressionType.Subtract => new Subtract(ParseRecursive(((BinaryExpression)expression).Left), ParseRecursive(((BinaryExpression)expression).Right)),
                ExpressionType.Multiply => new Multiply(ParseRecursive(((BinaryExpression)expression).Left), ParseRecursive(((BinaryExpression)expression).Right)),
                ExpressionType.Divide => new Divide(ParseRecursive(((BinaryExpression)expression).Left), ParseRecursive(((BinaryExpression)expression).Right)),
                ExpressionType.Power => new Power(ParseRecursive(((BinaryExpression)expression).Left), ParseRecursive(((BinaryExpression)expression).Right)),
                ExpressionType.Call => ParseMethodCall((MethodCallExpression)expression),
                _ => throw new ArgumentException("The expression could not be parsed.", nameof(e)),
            };
        }

        Expression ParseMethodCall(MethodCallExpression methodCallExpression)
        {
            MethodInfo methodInfo = methodCallExpression.Method;
            if (methodInfo is null)
            {
                throw new ArgumentException("The expression could not be parsed.", nameof(e));
            }

            Expression[]? arguments = null;
            if (methodCallExpression.Arguments is not null)
            {
                arguments = new Expression[methodCallExpression.Arguments.Count];
                for (int i = 0; i < arguments.Length; i++)
                    arguments[i] = ParseRecursive(methodCallExpression.Arguments[i]);
            }

            if (!ParseableLibraryBuilt)
            {
                BuildParsableOperationLibrary();
            }

            string operation = methodInfo.Name.ToLower();

            if (arguments is null)
            {
                throw new TowelBugException("zero parameter operations are not yet implemented");
            }

            switch (arguments.Length)
            {
                case 1:
                    if (ParsableUnaryOperations!.TryGetValue(operation, out var newUnaryFunction))
                    {
                        return newUnaryFunction(arguments[0]);
                    }
                    break;

                case 2:
                    if (ParsableBinaryOperations!.TryGetValue(operation, out var newBinaryFunction))
                    {
                        return newBinaryFunction(arguments[0], arguments[1]);
                    }
                    break;

                case 3:
                    if (ParsableTernaryOperations!.TryGetValue(operation, out var newTernaryFunction))
                    {
                        return newTernaryFunction(arguments[0], arguments[1], arguments[2]);
                    }
                    break;
            }

            if (ParsableMultinaryOperations!.TryGetValue(operation, out var newMultinaryFunction))
            {
                return newMultinaryFunction(arguments);
            }

            throw new ArgumentException("The expression could not be parsed.", nameof(e));
        }

        try
        {
            return ParseRecursive(e);
        }
        catch (ArithmeticException arithmeticException)
        {
            throw new ArgumentException("The expression could not be parsed.", nameof(e), arithmeticException);
        }
    }

    #endregion System.Linq.Expression

    #region string

    /// <summary>Parses a symbolic methematics expression with the assumption that it will simplify to a constant.</summary>
    /// <typeparam name="T">The generic numerical type to recieve as the outputted type.</typeparam>
    /// <param name="string">The string to be parse.</param>
    /// <param name="tryParse">A function for parsing numerical values into the provided generic type.</param>
    /// <returns>The parsed expression simplified down to a constant value.</returns>
    public static T ParseAndSimplifyToConstant<T>(string @string, Func<string, (bool Success, T? Value)>? tryParse = null)
    {
        tryParse ??= Statics.TryParse<T>;
        var simplified = Parse(@string, tryParse).Simplify();
        if (simplified is Constant<T> constant)
        {
            return constant.Value;
        }
        else
        {
            throw new ArgumentException(paramName: nameof(@string), message: $"{nameof(@string)} could not be simplified to a constant value");
        }
    }

    /// <summary>Parses a string into a JeezFoundation.Algorithm.Mathematics.Symbolics expression tree.</summary>
    /// <typeparam name="T">The type to convert any constants into (ex: float, double, etc).</typeparam>
    /// <param name="string">The expression string to parse.</param>
    /// <param name="tryParse">A parsing function for the provided generic type. This is optional, but highly recommended.</param>
    /// <returns>The parsed JeezFoundation.Algorithm.Mathematics.Symbolics expression tree.</returns>
    public static Expression Parse<T>(string @string, Func<string, (bool Success, T? Value)>? tryParse = null)
    {
        tryParse ??= Statics.TryParse<T>;
        // Build The Parsing Library
        if (!ParseableLibraryBuilt)
        {
            BuildParsableOperationLibrary();
        }
        // Error Handling
        if (string.IsNullOrWhiteSpace(@string))
        {
            throw new ArgumentException("The expression could not be parsed. { " + @string + " }", nameof(@string));
        }
        // Trim
        @string = @string.Trim();
        // Parse The Next Non-Nested Operator If One Exist
        if (TryParseNonNestedOperatorExpression<T>(@string, tryParse, out Expression? ParsedNonNestedOperatorExpression))
        {
            return ParsedNonNestedOperatorExpression!;
        }
        // Parse The Next Parenthesis If One Exists
        if (TryParseParenthesisExpression<T>(@string, tryParse, out Expression? ParsedParenthesisExpression))
        {
            return ParsedParenthesisExpression!;
        }
        // Parse The Next Operation If One Exists
        if (TryParseOperationExpression<T>(@string, tryParse, out Expression? ParsedOperationExpression))
        {
            return ParsedOperationExpression!;
        }
        // Parse The Next Set Of Variables If Any Exist
        if (TryParseVariablesExpression<T>(@string, tryParse, out Expression? ParsedVeriablesExpression))
        {
            return ParsedVeriablesExpression!;
        }
        // Parse The Next Known Constant Expression If Any Exist
        if (TryParseKnownConstantExpression<T>(@string, tryParse, out Expression? ParsedKnownConstantExpression))
        {
            return ParsedKnownConstantExpression!;
        }
        // Parse The Next Constant Expression If Any Exist
        if (TryParseConstantExpression<T>(@string, tryParse, out Expression? ParsedConstantExpression))
        {
            return ParsedConstantExpression!;
        }
        // Invalid Or Non-Supported Expression
        throw new ArgumentException("The expression could not be parsed. { " + @string + " }", nameof(@string));
    }

    internal static bool TryParseNonNestedOperatorExpression<T>(string @string, Func<string, (bool Success, T? Value)> tryParse, out Expression? expression)
    {
        // Try to match the operators pattern built at runtime based on the symbolic tree hierarchy
        MatchCollection operatorMatches = Regex.Matches(@string, ParsableOperatorsRegexPattern!, RegexOptions.RightToLeft);
        MatchCollection specialStringMatches = Regex.Matches(@string, SpecialStringsPattern!, RegexOptions.RightToLeft);
        if (operatorMatches.Count > 0)
        {
            // Find the first operator with the highest available priority
            Match? @operator = null;
            OperatorPriority priority = default;
            int currentOperatorMatch = 0;
            int scope = 0;
            bool isUnaryLeftOperator = false;
            bool isUnaryRightOperator = false;
            bool isBinaryOperator = false;
            for (int i = @string.Length - 1; i >= 0; i--)
            {
                switch (@string[i])
                {
                    case ')': scope++; break;
                    case '(': scope--; break;
                }

                // Handle Input Errors
                if (scope < 0)
                {
                    throw new ArgumentException("The expression could not be parsed. { " + @string + " }", nameof(@string));
                }

                Match currentMatch = operatorMatches[currentOperatorMatch];
                if (currentMatch.Index == i)
                {
                    if (scope is 0)
                    {
                        Match? previousMatch = currentOperatorMatch != 0 ? operatorMatches[currentOperatorMatch - 1] : null;
                        Match? nextMatch = currentOperatorMatch != operatorMatches.Count - 1 ? operatorMatches[currentOperatorMatch + 1] : null;

                        // We found an operator in the current scope
                        // Now we need to determine if it is a unary-left, unary-right, or binary operator

                        bool IsUnaryLeftOperator()
                        {
                            if (!ParsableLeftUnaryOperators!.ContainsKey(currentMatch.Value))
                            {
                                return false;
                            }

                            int rightIndex = currentMatch.Index - currentMatch.Length + 1;
                            if (rightIndex <= 0)
                            {
                                return true;
                            }
                            Match? leftSpecialMatch = null;
                            foreach (Match match in specialStringMatches)
                            {
                                if (match.Index < currentMatch.Index)
                                {
                                    leftSpecialMatch = match;
                                    break;
                                }
                            }
                            if (leftSpecialMatch is null)
                            {
                                string substring = @string[..rightIndex];
                                return string.IsNullOrWhiteSpace(substring);
                            }
                            else if (ParsableRightUnaryOperators!.ContainsKey(leftSpecialMatch.Value)) // This will need to be fixed in the future
                            {
                                return false;
                            }
                            else
                            {
                                int leftIndex = leftSpecialMatch.Index + 1;
                                string substring = @string[leftIndex..rightIndex];
                                return string.IsNullOrWhiteSpace(substring);
                            }
                        }

                        bool IsUnaryRightOperator()
                        {
                            if (!ParsableRightUnaryOperators!.ContainsKey(currentMatch.Value))
                            {
                                return false;
                            }

                            int leftIndex = currentMatch.Index;
                            if (leftIndex >= @string.Length - 1)
                            {
                                return true;
                            }
                            Match? rightSpecialMatch = null;
                            foreach (Match match in specialStringMatches)
                            {
                                if (match.Index <= currentMatch.Index)
                                {
                                    break;
                                }
                                rightSpecialMatch = match;
                            }
                            if (rightSpecialMatch is null)
                            {
                                return string.IsNullOrWhiteSpace(@string[(leftIndex + 1)..]);
                            }
                            else
                            {
                                int rightIndex = rightSpecialMatch.Index - rightSpecialMatch.Length;
                                return string.IsNullOrWhiteSpace(@string[leftIndex..rightIndex]);
                            }
                        }

                        if (IsUnaryLeftOperator())
                        {
                            if (@operator is null || priority > ParsableLeftUnaryOperators![currentMatch.Value].Item1)
                            {
                                @operator = currentMatch;
                                isUnaryLeftOperator = true;
                                isUnaryRightOperator = false;
                                isBinaryOperator = false;
                                priority = ParsableLeftUnaryOperators![currentMatch.Value].Item1;
                            }
                        }
                        else if (IsUnaryRightOperator())
                        {
                            if (@operator is null || priority > ParsableRightUnaryOperators![currentMatch.Value].Item1)
                            {
                                @operator = currentMatch;
                                isUnaryLeftOperator = false;
                                isUnaryRightOperator = true;
                                isBinaryOperator = false;
                                priority = ParsableRightUnaryOperators![currentMatch.Value].Item1;
                            }
                        }
                        else
                        {
                            if (ParsableBinaryOperators!.ContainsKey(currentMatch.Value))
                            {
                                // Binary Operator
                                if (@operator is null || priority > ParsableBinaryOperators[currentMatch.Value].Item1)
                                {
                                    @operator = currentMatch;
                                    isUnaryLeftOperator = false;
                                    isUnaryRightOperator = false;
                                    isBinaryOperator = true;
                                    priority = ParsableBinaryOperators[currentMatch.Value].Item1;
                                }
                            }
                        }
                    }
                    currentOperatorMatch++;

                    if (currentOperatorMatch >= operatorMatches.Count)
                    {
                        break;
                    }
                }
            }

            // if an operator was found, parse the expression
            if (@operator is not null)
            {
                if (isUnaryLeftOperator)
                {
                    string a = @string[(@operator.Index + @operator.Length)..];
                    Expression A = Parse(a, tryParse);
                    expression = ParsableLeftUnaryOperators![@operator.Value].Item2(A);
                    return true;
                }
                else if (isUnaryRightOperator)
                {
                    string a = @string[..@operator.Index];
                    Expression A = Parse(a, tryParse);
                    expression = ParsableRightUnaryOperators![@operator.Value].Item2(A);
                    return true;
                }
                else if (isBinaryOperator)
                {
                    string a = @string[..@operator.Index];
                    Expression A = Parse(a, tryParse);
                    string b = @string[(@operator.Index + @operator.Length)..];
                    Expression B = Parse(b, tryParse);
                    expression = ParsableBinaryOperators![@operator.Value].Item2(A, B);
                    return true;
                }
            }
        }

        // No non-nested operator patterns found. Fall back.
        expression = null;
        return false;
    }

    internal static bool TryParseParenthesisExpression<T>(string @string, Func<string, (bool Success, T? Value)> tryParse, out Expression? expression)
    {
        // Try to match a parenthesis pattern.
        Match parenthesisMatch = Regex.Match(@string, ParenthesisPattern);
        Match operationMatch = Regex.Match(@string, ParsableOperationsRegexPattern!);
        if (parenthesisMatch.Success)
        {
            if (operationMatch.Success && parenthesisMatch.Index > operationMatch.Index)
            {
                // The next set of parenthesis are part of an operation. Fall back and
                // let the TryParseOperationExpression handle it.
                expression = null;
                return false;
            }

            // Parse the nested expression
            string nestedExpression = parenthesisMatch.Value.Substring(1, parenthesisMatch.Length - 2);
            expression = Parse(nestedExpression, tryParse);

            // Check for implicit multiplications to the left of the parenthesis pattern
            if (parenthesisMatch.Index > 0)
            {
                string leftExpression = @string[..parenthesisMatch.Index];
                expression *= Parse(leftExpression, tryParse);
            }

            // Check for implicit multiplications to the right of the parenthesis pattern
            int right_start = parenthesisMatch.Index + parenthesisMatch.Length;
            if (right_start != @string.Length)
            {
                string rightExpression = @string[right_start..];
                expression *= Parse(rightExpression, tryParse);
            }

            // Parsing was successful
            return true;
        }

        // No parenthesis pattern found. Fall back.
        expression = null;
        return false;
    }

    internal static bool TryParseOperationExpression<T>(string @string, Func<string, (bool Success, T? Value)> tryParse, out Expression? expression)
    {
        expression = null;
        Match operationMatch = Regex.Match(@string, ParsableOperationsRegexPattern!);

        if (operationMatch.Success)
        {
            string operationMatch_Value = operationMatch.Value;
            string operation = operationMatch_Value[..operationMatch_Value.IndexOf('(')];
            Match parenthesisMatch = Regex.Match(@string, ParenthesisPattern);
            string parenthesisMatch_Value = parenthesisMatch.Value;
            ListArray<string> operandSplits = SplitOperands(parenthesisMatch_Value[1..^1]);

            switch (operandSplits.Count)
            {
                case 1:
                    if (ParsableUnaryOperations!.TryGetValue(operation, out Func<Expression, Unary>? newUnaryFunction))
                    {
                        expression = newUnaryFunction(Parse<T>(operandSplits[0]));
                    }
                    break;

                case 2:
                    if (ParsableBinaryOperations!.TryGetValue(operation, out Func<Expression, Expression, Binary>? newBinaryFunction))
                    {
                        expression = newBinaryFunction(Parse<T>(operandSplits[0]), Parse<T>(operandSplits[1]));
                    }
                    break;

                case 3:
                    if (ParsableTernaryOperations!.TryGetValue(operation, out Func<Expression, Expression, Expression, Ternary>? newTernaryFunction))
                    {
                        expression = newTernaryFunction(Parse<T>(operandSplits[0]), Parse<T>(operandSplits[2]), Parse<T>(operandSplits[2]));
                    }
                    break;
            }
            if (ParsableMultinaryOperations!.TryGetValue(operation, out Func<Expression[], Multinary>? newMultinaryFunction))
            {
                expression = newMultinaryFunction(operandSplits.Select(x => Parse<T>(x)).ToArray());
            }
            if (expression is null)
            {
                throw new ArgumentException("The expression could not be parsed. { " + @string + " }", nameof(@string));
            }
            // handle implicit multiplications if any exist
            if (operationMatch.Index != 0) // Left
            {
                Expression A = Parse(@string[..operationMatch.Index], tryParse);
                expression *= A;
            }
            if (operationMatch.Length + operationMatch.Index < @string.Length) // Right
            {
                Expression A = Parse(@string[(operationMatch.Length + operationMatch.Index)..], tryParse);
                expression *= A;
            }
            return true;
        }

        // No operation pattern found. Fall back.
        return false;
    }

    internal static ListArray<string> SplitOperands(string @string)
    {
        ListArray<string> operands = new();
        int scope = 0;
        int operandStart = 0;
        for (int i = 0; i < @string.Length; i++)
        {
            switch (@string[i])
            {
                case '(': scope++; break;
                case ')': scope--; break;
                case ',':
                    if (scope is 0)
                    {
                        operands.Add(@string[operandStart..i]);
                    }
                    break;
            }
        }
        if (scope != 0)
        {
            throw new ArgumentException("The expression could not be parsed. { " + @string + " }", nameof(@string));
        }
        operands.Add(@string[operandStart..]);
        return operands;
    }

    internal static bool TryParseVariablesExpression<T>(string @string, Func<string, (bool Success, T? Value)> tryParse, out Expression? parsedExpression)
    {
        string variablePattern = @"\[.*\]";

        // extract and parse variables
        System.Collections.Generic.IEnumerable<Expression> variables =
            Regex.Matches(@string, variablePattern)
            .Cast<Match>()
            .Select(x => new Variable(x.Value[1..^1]));

        // if no variables, fall back
        if (!variables.Any())
        {
            parsedExpression = null;
            return false;
        }

        // assume the remaining string splits are constants and try to parse them
        System.Collections.Generic.IEnumerable<Expression?> constants =
            Regex.Split(@string, variablePattern)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x =>
            {
                TryParseConstantExpression(x, tryParse, out var exp);
                return exp;
            });

        // multiply all the expressions together, starting with the constants because
        // it will look better if converted to a string
        bool set = false;
        parsedExpression = null;
        foreach (var constant in constants.Concat(variables))
        {
            if (!set)
            {
                parsedExpression = constant;
                set = true;
            }
            else
            {
                if (parsedExpression is null)
                {
                    throw new TowelBugException($"Encountered null {nameof(parsedExpression)} in {nameof(TryParseVariablesExpression)}.");
                }
                if (constant is null)
                {
                    throw new TowelBugException($"Encountered null {nameof(constant)} in {nameof(TryParseVariablesExpression)}.");
                }
                parsedExpression *= constant;
            }
        }
        return true;
    }

    internal static bool TryParseKnownConstantExpression<T>(string @string, Func<string, (bool Success, T? Value)> tryParse, out Expression? parsedExpression)
    {
        Match knownConstantMatch = Regex.Match(@string, ParsableKnownConstantsRegexPattern!);

        if (knownConstantMatch.Success)
        {
            parsedExpression = ParsableKnownConstants![knownConstantMatch.Value]().ApplyType<T>();

            // implied multiplications to the left and right
            if (knownConstantMatch.Index != 0)
            {
                Expression A = Parse<T>(@string[..knownConstantMatch.Index], tryParse);
                parsedExpression *= A;
            }
            if (knownConstantMatch.Index < @string.Length - 1)
            {
                Expression B = Parse<T>(@string[(knownConstantMatch.Index + 1)..], tryParse);
                parsedExpression *= B;
            }
            return true;
        }

        parsedExpression = null;
        return false;
    }

    internal static bool TryParseConstantExpression<T>(string @string, Func<string, (bool Success, T? Value)> tryParse, out Expression? parsedExpression)
    {
        var (parseSuccess, parseValue) = tryParse(@string);
        if (parseSuccess)
        {
            parsedExpression = new Constant<T?>(parseValue);
            return true;
        }
        int decimalIndex = -1;
        for (int i = 0; i < @string.Length; i++)
        {
            char character = @string[i];
            if (character == '.')
            {
                if (decimalIndex >= 0 || i == @string.Length - 1)
                {
                    parsedExpression = null;
                    return false;
                }
                decimalIndex = i;
            }
            if ('0' > character && character > '9')
            {
                parsedExpression = null;
                return false;
            }
        }
        if (decimalIndex >= 0)
        {
            string wholeNumberString;
            if (decimalIndex is 0)
            {
                wholeNumberString = "0";
            }
            else
            {
                wholeNumberString = @string[..decimalIndex];
            }
            string decimalPlacesString = @string[(decimalIndex + 1)..];

            int zeroCount = 0;
            while (decimalPlacesString[zeroCount] == '0')
            {
                zeroCount++;
            }

            if (int.TryParse(wholeNumberString, out int wholeNumberInt) &&
                int.TryParse(decimalPlacesString, out int decimalPlacesInt))
            {
                T wholeNumber = Convert<int, T>(wholeNumberInt);
                T decimalPlaces = Convert<int, T>(decimalPlacesInt);
                while (GreaterThanOrEqual(decimalPlaces, JeezFoundation.Algorithm.Constant<T>.One))
                {
                    decimalPlaces = Division(decimalPlaces, JeezFoundation.Algorithm.Constant<T>.Ten);
                }
                for (; zeroCount > 0; zeroCount--)
                {
                    decimalPlaces = Division(decimalPlaces, JeezFoundation.Algorithm.Constant<T>.Ten);
                }
                parsedExpression = new Constant<T>(Addition(wholeNumber, decimalPlaces));
                return true;
            }
        }
        else
        {
            if (int.TryParse(@string, out int parsedInt))
            {
                parsedExpression = new Constant<T>(Convert<int, T>(parsedInt));
                return true;
            }
        }
        parsedExpression = null;
        return false;
    }

    #endregion string

    #endregion Parsers
}