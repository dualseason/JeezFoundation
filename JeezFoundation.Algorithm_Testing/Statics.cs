﻿using System.Runtime.CompilerServices;

namespace Towel_Testing;

[TestClass]
public partial class Statics_Testing
{
	#region source...

#pragma warning disable IDE1006 // Naming Styles

	[TestMethod]
	public void sourcelineNumber_Testing() =>
		sourcelinenumberTest(sourcelinenumber());
	public static void sourcelinenumberTest(int result, [CallerLineNumber] int expected = default) =>
		Assert.IsTrue(result == expected);

	[TestMethod]
	public void sourcefilename_Testing() =>
		sourcefilepathTest(sourcefilepath());
	public static void sourcefilepathTest(string result, [CallerFilePath] string? expected = default) =>
		Assert.IsTrue(result == expected);

	[TestMethod]
	public void sourcemembername_Testing() =>
		sourcemembernameTest(sourcemembername());
	public static void sourcemembernameTest(string result, [CallerMemberName] string? expected = default) =>
		Assert.IsTrue(result == expected);

	[TestMethod]
	public void sourceof_Testing()
	{
#pragma warning disable CS8519 // The given expression never matches the provided pattern.
		sourceofTest(sourceof(1 is 2).Source, 1 is 2);
		sourceof(1 is 2, out string source);
		sourceofTest(source, 1 is 2);
#pragma warning restore CS8519 // The given expression never matches the provided pattern.
	}

	public static void sourceofTest<T>(string result, T expression, [CallerArgumentExpression("expression")] string? expected = default)
	{
		Assert.IsTrue(result == expected);
	}

#pragma warning restore IDE1006 // Naming Styles

	#endregion

	#region TryParse_Testing

	[TestMethod]
	public void TryParse_Testing()
	{
		// successful parse
		Assert.IsTrue(TryParse<int>("1") == (true, 1));
		Assert.IsTrue(TryParse<float>("1.2") == (true, 1.2f));
		Assert.IsTrue(TryParse<double>("1.23") == (true, 1.23d));
		Assert.IsTrue(TryParse<decimal>("1.234") == (true, 1.234m));
		Assert.IsTrue(TryParse<ConsoleColor>("Red") == (true, ConsoleColor.Red));
		Assert.IsTrue(TryParse<StringComparison>("Ordinal") == (true, StringComparison.Ordinal));

		// parse fails
		Assert.IsTrue(TryParse<int>("a") == (false, default(int)));
		Assert.IsTrue(TryParse<float>("a") == (false, default(float)));
		Assert.IsTrue(TryParse<double>("a") == (false, default(double)));
		Assert.IsTrue(TryParse<decimal>("a") == (false, default(decimal)));
		Assert.IsTrue(TryParse<ConsoleColor>("a") == (false, default(ConsoleColor)));
		Assert.IsTrue(TryParse<StringComparison>("a") == (false, default(StringComparison)));
	}

	#endregion

	#region Convert_Testing

	[TestMethod]
	public void Convert_Testing()
	{
		{ // int
			Assert.IsTrue(Convert<int, int>(7) == 7);
			Assert.IsTrue(Convert<int, float>(7) == 7f);
			Assert.IsTrue(Convert<int, double>(7) == 7d);
			Assert.IsTrue(Convert<int, decimal>(7) == 7m);
		}
		{ // float
			Assert.IsTrue(Convert<float, int>(7f) == 7);
			Assert.IsTrue(Convert<float, float>(7f) == 7f);
			Assert.IsTrue(Convert<float, double>(7f) == 7d);
			Assert.IsTrue(Convert<float, decimal>(7f) == 7m);
		}
		{ // double
			Assert.IsTrue(Convert<double, int>(7d) == 7);
			Assert.IsTrue(Convert<double, float>(7d) == 7f);
			Assert.IsTrue(Convert<double, double>(7d) == 7d);
			Assert.IsTrue(Convert<double, decimal>(7d) == 7m);
		}
		{ // decimal
			Assert.IsTrue(Convert<decimal, int>(7m) == 7);
			Assert.IsTrue(Convert<decimal, float>(7m) == 7f);
			Assert.IsTrue(Convert<decimal, double>(7m) == 7d);
			Assert.IsTrue(Convert<decimal, decimal>(7m) == 7m);
		}
	}

	#endregion

	#region Negation_Testing

	[TestMethod]
	public void Negation_Testing()
	{
		{ // int
			Assert.IsTrue(Negation(-3) == 3);
			Assert.IsTrue(Negation(-2) == 2);
			Assert.IsTrue(Negation(-1) == 1);
			Assert.IsTrue(Negation(0) == 0);
			Assert.IsTrue(Negation(1) == -1);
			Assert.IsTrue(Negation(2) == -2);
			Assert.IsTrue(Negation(3) == -3);
		}
		{ // float
			Assert.IsTrue(Negation(-3f) == 3f);
			Assert.IsTrue(Negation(-2f) == 2f);
			Assert.IsTrue(Negation(-1f) == 1f);
			Assert.IsTrue(Negation(-0.5f) == 0.5f);
			Assert.IsTrue(Negation(0f) == 0f);
			Assert.IsTrue(Negation(0.5f) == -0.5f);
			Assert.IsTrue(Negation(1f) == -1f);
			Assert.IsTrue(Negation(2f) == -2f);
			Assert.IsTrue(Negation(3f) == -3f);
		}
		{ // double
			Assert.IsTrue(Negation(-3d) == 3d);
			Assert.IsTrue(Negation(-2d) == 2d);
			Assert.IsTrue(Negation(-1d) == 1d);
			Assert.IsTrue(Negation(-0.5d) == 0.5d);
			Assert.IsTrue(Negation(0d) == 0d);
			Assert.IsTrue(Negation(0.5d) == -0.5d);
			Assert.IsTrue(Negation(1d) == -1d);
			Assert.IsTrue(Negation(2d) == -2d);
			Assert.IsTrue(Negation(3d) == -3d);
		}
		{ // decimal
			Assert.IsTrue(Negation(-3m) == 3m);
			Assert.IsTrue(Negation(-2m) == 2m);
			Assert.IsTrue(Negation(-1m) == 1m);
			Assert.IsTrue(Negation(-0.5m) == 0.5m);
			Assert.IsTrue(Negation(0m) == 0m);
			Assert.IsTrue(Negation(0.5m) == -0.5m);
			Assert.IsTrue(Negation(1m) == -1m);
			Assert.IsTrue(Negation(2m) == -2m);
			Assert.IsTrue(Negation(3m) == -3m);
		}
	}

	#endregion

	#region Addition_Testing

	[TestMethod]
	public void Addition_Testing()
	{
		// Binary

		// int
		Assert.IsTrue(Addition(0, 0) == 0 + 0);
		Assert.IsTrue(Addition(1, 1) == 1 + 1);
		Assert.IsTrue(Addition(1, 2) == 1 + 2);
		Assert.IsTrue(Addition(-1, 1) == -1 + 1);
		Assert.IsTrue(Addition(1, -1) == 1 + -1);
		Assert.IsTrue(Addition(-1, -1) == -1 + -1);
		// float
		Assert.IsTrue(Addition(0f, 0f) == 0f + 0f);
		Assert.IsTrue(Addition(1f, 1f) == 1f + 1f);
		Assert.IsTrue(Addition(1f, 2f) == 1f + 2f);
		Assert.IsTrue(Addition(-1f, 1f) == -1f + 1f);
		Assert.IsTrue(Addition(1f, -1f) == 1f + -1f);
		Assert.IsTrue(Addition(-1f, -1f) == -1f + -1f);
		// double
		Assert.IsTrue(Addition(0d, 0d) == 0d + 0d);
		Assert.IsTrue(Addition(1d, 1d) == 1d + 1d);
		Assert.IsTrue(Addition(1d, 2d) == 1d + 2d);
		Assert.IsTrue(Addition(-1d, 1d) == -1d + 1d);
		Assert.IsTrue(Addition(1d, -1d) == 1d + -1d);
		Assert.IsTrue(Addition(-1d, -1d) == -1d + -1d);
		// decimal
		Assert.IsTrue(Addition(0m, 0m) == 0m + 0m);
		Assert.IsTrue(Addition(1m, 1m) == 1m + 1m);
		Assert.IsTrue(Addition(1m, 2m) == 1m + 2m);
		Assert.IsTrue(Addition(-1m, 1m) == -1m + 1m);
		Assert.IsTrue(Addition(1m, -1m) == 1m + -1m);
		Assert.IsTrue(Addition(-1m, -1m) == -1m + -1m);

		// Stepper

		// int
		Assert.IsTrue(Addition(1, 2, 3) == 1 + 2 + 3);
		Assert.IsTrue(Addition(-1, -2, -3) == -1 + -2 + -3);
		// float
		Assert.IsTrue(Addition(1f, 2f, 3f) == 1f + 2f + 3f);
		Assert.IsTrue(Addition(-1f, -2f, -3f) == -1f + -2f + -3f);
		// double
		Assert.IsTrue(Addition(1d, 2d, 3d) == 1d + 2d + 3d);
		Assert.IsTrue(Addition(-1d, -2d, -3d) == -1d + -2d + -3d);
		// decimal
		Assert.IsTrue(Addition(1m, 2m, 3m) == 1m + 2m + 3m);
		Assert.IsTrue(Addition(-1m, -2m, -3m) == -1m + -2m + -3m);
	}

	#endregion

	#region Subtraction_Testing

	[TestMethod]
	public void Subtraction_Testing()
	{
		// Binary

		// int
		Assert.IsTrue(Subtraction(0, 0) == 0 - 0);
		Assert.IsTrue(Subtraction(1, 1) == 1 - 1);
		Assert.IsTrue(Subtraction(1, 2) == 1 - 2);
		Assert.IsTrue(Subtraction(-1, 1) == -1 - 1);
		Assert.IsTrue(Subtraction(1, -1) == 1 - -1);
		Assert.IsTrue(Subtraction(-1, -1) == -1 - -1);
		// float
		Assert.IsTrue(Subtraction(0f, 0f) == 0f - 0f);
		Assert.IsTrue(Subtraction(1f, 1f) == 1f - 1f);
		Assert.IsTrue(Subtraction(1f, 2f) == 1f - 2f);
		Assert.IsTrue(Subtraction(-1f, 1f) == -1f - 1f);
		Assert.IsTrue(Subtraction(1f, -1f) == 1f - -1f);
		Assert.IsTrue(Subtraction(-1f, -1f) == -1f - -1f);
		// double
		Assert.IsTrue(Subtraction(0d, 0d) == 0d - 0d);
		Assert.IsTrue(Subtraction(1d, 1d) == 1d - 1d);
		Assert.IsTrue(Subtraction(1d, 2d) == 1d - 2d);
		Assert.IsTrue(Subtraction(-1d, 1d) == -1d - 1d);
		Assert.IsTrue(Subtraction(1d, -1d) == 1d - -1d);
		Assert.IsTrue(Subtraction(-1d, -1d) == -1d - -1d);
		// decimal
		Assert.IsTrue(Subtraction(0m, 0m) == 0m - 0m);
		Assert.IsTrue(Subtraction(1m, 1m) == 1m - 1m);
		Assert.IsTrue(Subtraction(1m, 2m) == 1m - 2m);
		Assert.IsTrue(Subtraction(-1m, 1m) == -1m - 1m);
		Assert.IsTrue(Subtraction(1m, -1m) == 1 - -1m);
		Assert.IsTrue(Subtraction(-1m, -1m) == -1m - -1m);

		// Stepper

		// int
		Assert.IsTrue(Subtraction(1, 2, 3) == 1 - 2 - 3);
		Assert.IsTrue(Subtraction(-1, -2, -3) == -1 - -2 - -3);
		// float
		Assert.IsTrue(Subtraction(1f, 2f, 3f) == 1f - 2f - 3f);
		Assert.IsTrue(Subtraction(-1f, -2f, -3f) == -1f - -2f - -3f);
		// double
		Assert.IsTrue(Subtraction(1d, 2d, 3d) == 1d - 2d - 3d);
		Assert.IsTrue(Subtraction(-1d, -2d, -3d) == -1d - -2d - -3d);
		// decimal
		Assert.IsTrue(Subtraction(1m, 2m, 3m) == 1m - 2m - 3m);
		Assert.IsTrue(Subtraction(-1m, -2m, -3m) == -1m - -2m - -3m);
	}

	#endregion

	#region Multiplication_Testing

	[TestMethod]
	public void Multiplication_Testing()
	{
		// Binary

		// int
		Assert.IsTrue(Multiplication(0, 0) == 0 * 0);
		Assert.IsTrue(Multiplication(1, 1) == 1 * 1);
		Assert.IsTrue(Multiplication(1, 2) == 1 * 2);
		Assert.IsTrue(Multiplication(-1, 1) == -1 * 1);
		Assert.IsTrue(Multiplication(1, -1) == 1 * -1);
		Assert.IsTrue(Multiplication(-1, -1) == -1 * -1);
		// float
		Assert.IsTrue(Multiplication(0f, 0f) == 0f * 0f);
		Assert.IsTrue(Multiplication(1f, 1f) == 1f * 1f);
		Assert.IsTrue(Multiplication(1f, 2f) == 1f * 2f);
		Assert.IsTrue(Multiplication(-1f, 1f) == -1f * 1f);
		Assert.IsTrue(Multiplication(1f, -1f) == 1f * -1f);
		Assert.IsTrue(Multiplication(-1f, -1f) == -1f * -1f);
		// double
		Assert.IsTrue(Multiplication(0d, 0d) == 0d * 0d);
		Assert.IsTrue(Multiplication(1d, 1d) == 1d * 1d);
		Assert.IsTrue(Multiplication(1d, 2d) == 1d * 2d);
		Assert.IsTrue(Multiplication(-1d, 1d) == -1d * 1d);
		Assert.IsTrue(Multiplication(1d, -1d) == 1d * -1d);
		Assert.IsTrue(Multiplication(-1d, -1d) == -1d * -1d);
		// decimal
		Assert.IsTrue(Multiplication(0m, 0m) == 0m * 0m);
		Assert.IsTrue(Multiplication(1m, 1m) == 1m * 1m);
		Assert.IsTrue(Multiplication(1m, 2m) == 1m * 2m);
		Assert.IsTrue(Multiplication(-1m, 1m) == -1m * 1m);
		Assert.IsTrue(Multiplication(1m, -1m) == 1m * -1m);
		Assert.IsTrue(Multiplication(-1m, -1m) == -1m * -1m);

		// Stepper

		// int
		Assert.IsTrue(Multiplication(1, 2, 3) == 1 * 2 * 3);
		Assert.IsTrue(Multiplication(-1, -2, -3) == -1 * -2 * -3);
		// float
		Assert.IsTrue(Multiplication(1f, 2f, 3f) == 1f * 2f * 3f);
		Assert.IsTrue(Multiplication(-1f, -2f, -3f) == -1f * -2f * -3f);
		// double
		Assert.IsTrue(Multiplication(1d, 2d, 3d) == 1d * 2d * 3d);
		Assert.IsTrue(Multiplication(-1d, -2d, -3d) == -1d * -2d * -3d);
		// decimal
		Assert.IsTrue(Multiplication(1m, 2m, 3m) == 1m * 2m * 3m);
		Assert.IsTrue(Multiplication(-1m, -2m, -3m) == -1m * -2m * -3m);
	}

	#endregion

	#region Division_Testing

	[TestMethod]
	public void Division_Testing()
	{
		// Binary

		// int
		Assert.ThrowsException<DivideByZeroException>(() => Division(0, 0));
		Assert.IsTrue(Division(1, 1) == 1 / 1);
		Assert.IsTrue(Division(2, 1) == 2 / 1);
		Assert.IsTrue(Division(4, 2) == 4 / 2);
		Assert.IsTrue(Division(-4, 2) == -4 / 2);
		Assert.IsTrue(Division(4, -2) == 4 / -2);
		Assert.IsTrue(Division(-4, -2) == -4 / -2);
		// float
		Assert.IsTrue(float.IsNaN(Division(0f, 0f)));
		Assert.IsTrue(Division(1f, 1f) == 1f / 1f);
		Assert.IsTrue(Division(2f, 1f) == 2f / 1f);
		Assert.IsTrue(Division(4f, 2f) == 4f / 2f);
		Assert.IsTrue(Division(-4f, 2f) == -4f / 2f);
		Assert.IsTrue(Division(4f, -2f) == 4f / -2f);
		Assert.IsTrue(Division(-4f, -2f) == -4f / -2f);
		// double
		Assert.IsTrue(double.IsNaN(Division(0d, 0d)));
		Assert.IsTrue(Division(1d, 1d) == 1d / 1d);
		Assert.IsTrue(Division(2d, 1d) == 2d / 1d);
		Assert.IsTrue(Division(4d, 2d) == 4d / 2d);
		Assert.IsTrue(Division(-4d, 2d) == -4d / 2d);
		Assert.IsTrue(Division(4d, -2d) == 4d / -2d);
		Assert.IsTrue(Division(-4d, -2d) == -4d / -2d);
		// decimal
		Assert.ThrowsException<DivideByZeroException>(() => Division(0m, 0m));
		Assert.IsTrue(Division(1m, 1m) == 1m / 1m);
		Assert.IsTrue(Division(2m, 1m) == 2m / 1m);
		Assert.IsTrue(Division(4m, 2m) == 4m / 2m);
		Assert.IsTrue(Division(-4m, 2m) == -4m / 2m);
		Assert.IsTrue(Division(4m, -2m) == 4m / -2m);
		Assert.IsTrue(Division(-4m, -2m) == -4m / -2m);

		// Stepper

		// int
		Assert.IsTrue(Division(100, 10, 10) == 100 / 10 / 10);
		Assert.IsTrue(Division(-100, -10, -10) == -100 / -10 / -10);
		// float
		Assert.IsTrue(Division(100f, 10f, 10f) == 100f / 10f / 10f);
		Assert.IsTrue(Division(-100f, -10f, -10f) == -100f / -10f / -10f);
		// double
		Assert.IsTrue(Division(100d, 10d, 10d) == 100d / 10d / 10d);
		Assert.IsTrue(Division(-100d, -10d, -10d) == -100d / -10d / -10d);
		// decimal
		Assert.IsTrue(Division(100m, 10m, 10m) == 100m / 10m / 10m);
		Assert.IsTrue(Division(-100m, -10m, -10m) == -100m / -10m / -10m);
	}

	#endregion

	#region Inversion_Testing

	[TestMethod]
	public void Inversion_Testing()
	{
		// Note: not entirely sure about the invert method... :/ may remove it

		Assert.IsTrue(Inversion(1) == 1 / 1);
		Assert.IsTrue(Inversion(-1) == 1 / -1);
		Assert.IsTrue(Inversion(2) == 1 / 2);

		Assert.IsTrue(Inversion(0f) == 1f / 0f);
		Assert.IsTrue(Inversion(1f) == 1f / 1f);
		Assert.IsTrue(Inversion(-1f) == 1f / -1f);
		Assert.IsTrue(Inversion(2f) == 1f / 2f);

		Assert.IsTrue(Inversion(0d) == 1d / 0d);
		Assert.IsTrue(Inversion(1d) == 1d / 1d);
		Assert.IsTrue(Inversion(-1d) == 1d / -1d);
		Assert.IsTrue(Inversion(2d) == 1d / 2d);

		Assert.IsTrue(Inversion(1m) == 1m / 1m);
		Assert.IsTrue(Inversion(-1m) == 1m / -1m);
		Assert.IsTrue(Inversion(2m) == 1m / 2m);

		Assert.ThrowsException<DivideByZeroException>(() => Inversion(0));
		Assert.ThrowsException<DivideByZeroException>(() => Inversion(0m));
	}

	#endregion

	#region Remainder_Testing

	[TestMethod]
	public void Remainder_Testing()
	{
		// Binary

		// int
		Assert.IsTrue(Remainder(0, 1) == 0 % 1);
		Assert.IsTrue(Remainder(1, 1) == 1 % 1);
		Assert.IsTrue(Remainder(8, 3) == 8 % 3);
		Assert.IsTrue(Remainder(-8, 3) == -8 % 3);
		Assert.IsTrue(Remainder(8, -3) == 8 % 3);
		Assert.IsTrue(Remainder(-8, -3) == -8 % 3);

		// float
		Assert.IsTrue(float.IsNaN(Remainder(0f, 0f)));
		Assert.IsTrue(Remainder(0f, 1f) == 0f % 1f);
		Assert.IsTrue(Remainder(1f, 1f) == 1f % 1f);
		Assert.IsTrue(Remainder(8f, 3f) == 8f % 3f);
		Assert.IsTrue(Remainder(-8f, 3f) == -8f % 3f);
		Assert.IsTrue(Remainder(8f, -3f) == 8f % -3f);
		Assert.IsTrue(Remainder(-8f, -3f) == -8f % -3f);

		// double
		Assert.IsTrue(double.IsNaN(Remainder(0d, 0d)));
		Assert.IsTrue(Remainder(0d, 1d) == 0d % 1d);
		Assert.IsTrue(Remainder(1d, 1d) == 1d % 1d);
		Assert.IsTrue(Remainder(8d, 3d) == 8d % 3d);
		Assert.IsTrue(Remainder(-8d, 3d) == -8d % 3d);
		Assert.IsTrue(Remainder(8d, -3d) == 8d % -3d);
		Assert.IsTrue(Remainder(-8d, -3d) == -8d % -3d);

		// decimal
		Assert.IsTrue(Remainder(0m, 1m) == 0m % 1m);
		Assert.IsTrue(Remainder(1m, 1m) == 1m % 1m);
		Assert.IsTrue(Remainder(8m, 3m) == 8m % 3m);
		Assert.IsTrue(Remainder(-8m, 3m) == -8m % 3m);
		Assert.IsTrue(Remainder(8m, -3m) == 8m % -3m);
		Assert.IsTrue(Remainder(-8m, -3m) == -8m % -3m);

		// Stepper

		// int
		Assert.IsTrue(Remainder(15, 8, 3) == 15 % 8 % 3);
		Assert.IsTrue(Remainder(-15, -8, -3) == -15 % -8 % -3);
		// float
		Assert.IsTrue(Remainder(15f, 8f, 3f) == 15f % 8f % 3f);
		Assert.IsTrue(Remainder(-15f, -8f, -3f) == -15f % -8f % -3f);
		// double
		Assert.IsTrue(Remainder(15d, 8d, 3d) == 15d % 8d % 3d);
		Assert.IsTrue(Remainder(-15d, -8d, -3d) == -15d % -8d % -3d);
		// decimal
		Assert.IsTrue(Remainder(15m, 8m, 3m) == 15m % 8m % 3m);
		Assert.IsTrue(Remainder(-15m, -8m, -3m) == -15m % -8m % -3m);

		Assert.ThrowsException<DivideByZeroException>(() => Remainder(0, 0));
		Assert.ThrowsException<DivideByZeroException>(() => Remainder(0m, 0m));
	}

	#endregion

	#region Power_Testing

	[TestMethod]
	public void Power_Testing()
	{
		// Binary

		// int
		Assert.IsTrue(Power(0, 0) == 1);
		Assert.IsTrue(Power(1, 1) == 1);
		Assert.IsTrue(Power(2, 1) == 2);
		Assert.IsTrue(Power(2, 2) == 4);
		Assert.IsTrue(Power(-2, 2) == 4);
		Assert.IsTrue(Power(-2, 3) == -8);

		// float
		Assert.IsTrue(Power(0f, 0f) == 1f);
		Assert.IsTrue(Power(1f, 1f) == 1f);
		Assert.IsTrue(Power(2f, 1f) == 2f);
		Assert.IsTrue(Power(2f, 2f) == 4f);
		Assert.IsTrue(Power(-2f, 2f) == 4f);
		Assert.IsTrue(Power(-2f, 3f) == -8f);

		// double
		Assert.IsTrue(Power(0d, 0d) == 1d);
		Assert.IsTrue(Power(1d, 1d) == 1d);
		Assert.IsTrue(Power(2d, 1d) == 2d);
		Assert.IsTrue(Power(2d, 2d) == 4d);
		Assert.IsTrue(Power(-2d, 2d) == 4d);
		Assert.IsTrue(Power(-2d, 3d) == -8d);

		// decimal
		Assert.IsTrue(Power(0m, 0m) == 1m);
		Assert.IsTrue(Power(1m, 1m) == 1m);
		Assert.IsTrue(Power(2m, 1m) == 2m);
		Assert.IsTrue(Power(2m, 2m) == 4m);
		Assert.IsTrue(Power(-2m, 2m) == 4m);
		Assert.IsTrue(Power(-2m, 3m) == -8m);
	}

	#endregion

	#region Equate_Testing

	[TestMethod]
	public void Equate_Testing()
	{
		Assert.IsTrue(Equate(0, 0));
		Assert.IsTrue(Equate(1, 1));
		Assert.IsTrue(Equate(2, 2));
		Assert.IsFalse(Equate(0, 1));

		Assert.IsTrue(Equate(0f, 0f));
		Assert.IsTrue(Equate(1f, 1f));
		Assert.IsTrue(Equate(2f, 2f));
		Assert.IsFalse(Equate(0f, 1f));

		Assert.IsTrue(Equate(0d, 0d));
		Assert.IsTrue(Equate(1d, 1d));
		Assert.IsTrue(Equate(2d, 2d));
		Assert.IsFalse(Equate(0d, 1d));

		Assert.IsTrue(Equate(0m, 0m));
		Assert.IsTrue(Equate(1m, 1m));
		Assert.IsTrue(Equate(2m, 2m));
		Assert.IsFalse(Equate(0m, 1m));

		// More than 2 operands

		Assert.IsTrue(Equate(0, 0, 0));
		Assert.IsTrue(Equate(1, 1, 1));
		Assert.IsTrue(Equate(2, 2, 2));

		Assert.IsFalse(Equate(0, 0, 1));
		Assert.IsFalse(Equate(1, 1, 2));
		Assert.IsFalse(Equate(2, 2, 3));

		Assert.IsFalse(Equate(0, 1, 0));
		Assert.IsFalse(Equate(1, 2, 1));
		Assert.IsFalse(Equate(2, 3, 2));

		Assert.IsFalse(Equate(1, 0, 0));
		Assert.IsFalse(Equate(2, 1, 1));
		Assert.IsFalse(Equate(3, 2, 2));

		// nulls

		Assert.IsTrue(Equate<string?>(null, null));
		Assert.IsFalse(Equate<string?>(null, ""));
		Assert.IsFalse(Equate<string?>("", null));
		Assert.IsTrue(Equate("", ""));
	}

	#endregion

	#region EqualToLeniency_Testing

	[TestMethod]
	public void EqualToLeniency_Testing()
	{
		Assert.IsTrue(EqualToLeniency(0, 0, 0));
		Assert.IsTrue(EqualToLeniency(1, 1, 0));
		Assert.IsTrue(EqualToLeniency(2, 2, 0));

		Assert.IsTrue(EqualToLeniency(0f, 0f, 0f));
		Assert.IsTrue(EqualToLeniency(1f, 1f, 0f));
		Assert.IsTrue(EqualToLeniency(2f, 2f, 0f));

		Assert.IsTrue(EqualToLeniency(0d, 0d, 0d));
		Assert.IsTrue(EqualToLeniency(1d, 1d, 0d));
		Assert.IsTrue(EqualToLeniency(2d, 2d, 0d));

		Assert.IsTrue(EqualToLeniency(0m, 0m, 0m));
		Assert.IsTrue(EqualToLeniency(1m, 1m, 0m));
		Assert.IsTrue(EqualToLeniency(2m, 2m, 0m));

		Assert.IsTrue(EqualToLeniency(0, 1, 1));
		Assert.IsTrue(EqualToLeniency(1, 2, 1));
		Assert.IsTrue(EqualToLeniency(2, 3, 1));

		Assert.IsTrue(EqualToLeniency(0f, 1f, 1f));
		Assert.IsTrue(EqualToLeniency(1f, 2f, 1f));
		Assert.IsTrue(EqualToLeniency(2f, 3f, 1f));

		Assert.IsTrue(EqualToLeniency(0d, 1d, 1d));
		Assert.IsTrue(EqualToLeniency(1d, 2d, 1d));
		Assert.IsTrue(EqualToLeniency(2d, 3d, 1d));

		Assert.IsTrue(EqualToLeniency(0m, 1m, 1m));
		Assert.IsTrue(EqualToLeniency(1m, 2m, 1m));
		Assert.IsTrue(EqualToLeniency(2m, 3m, 1m));

		Assert.IsFalse(EqualToLeniency(0, 2, 1));
		Assert.IsFalse(EqualToLeniency(1, 3, 1));
		Assert.IsFalse(EqualToLeniency(2, 4, 1));

		Assert.IsFalse(EqualToLeniency(0f, 2f, 1f));
		Assert.IsFalse(EqualToLeniency(1f, 3f, 1f));
		Assert.IsFalse(EqualToLeniency(2f, 4f, 1f));

		Assert.IsFalse(EqualToLeniency(0d, 2d, 1d));
		Assert.IsFalse(EqualToLeniency(1d, 3d, 1d));
		Assert.IsFalse(EqualToLeniency(2d, 4d, 1d));

		Assert.IsFalse(EqualToLeniency(0m, 2m, 1m));
		Assert.IsFalse(EqualToLeniency(1m, 3m, 1m));
		Assert.IsFalse(EqualToLeniency(2m, 4m, 1m));
	}

	#endregion

	#region SineTaylorSeries_Testing

	[TestMethod]
	public void SineTaylorSeries_Testing()
	{
		double sine_zero = SineTaylorSeries(new Angle<double>(0d, Angle.Units.Radians));
		Assert.IsTrue(sine_zero == 0d);

		double sine_pi = SineTaylorSeries(new Angle<double>(Constant<double>.Pi, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(sine_pi, 0d, .00001d));

		double sine_2pi = SineTaylorSeries(new Angle<double>(Constant<double>.Pi2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(sine_2pi, 0d, .00001d));

		double sine_halfPi = SineTaylorSeries(new Angle<double>(Constant<double>.Pi / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(sine_halfPi, 1d, .00001d));

		double sine_3halfsPi = SineTaylorSeries(new Angle<double>(Constant<double>.Pi * 3 / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(sine_3halfsPi, -1d, .00001d));

		double sine_neghalfPi = SineTaylorSeries(new Angle<double>(-Constant<double>.Pi / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(sine_neghalfPi, -1d, .00001d));

		double sine_neg3halfsPi = SineTaylorSeries(new Angle<double>(-Constant<double>.Pi * 3 / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(sine_neg3halfsPi, 1d, .00001d));
	}

	#endregion

	#region CosineTaylorSeries_Testing

	[TestMethod]
	public void CosineTaylorSeries_Testing()
	{
		double cosine_zero = CosineTaylorSeries(new Angle<double>(0d, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(cosine_zero, 1d, .00001d));

		double cosine_pi = CosineTaylorSeries(new Angle<double>(Constant<double>.Pi, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(cosine_pi, -1d, .00001d));

		double cosine_2pi = CosineTaylorSeries(new Angle<double>(Constant<double>.Pi2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(cosine_2pi, 1d, .00001d));

		double cosine_halfPi = CosineTaylorSeries(new Angle<double>(Constant<double>.Pi / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(cosine_halfPi, 0d, .00001d));

		double cosine_3halfsPi = CosineTaylorSeries(new Angle<double>(Constant<double>.Pi * 3 / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(cosine_3halfsPi, 0d, .00001d));

		double cosine_neghalfPi = CosineTaylorSeries(new Angle<double>(-Constant<double>.Pi / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(cosine_neghalfPi, 0d, .00001d));

		double cosine_neg3halfsPi = CosineTaylorSeries(new Angle<double>(-Constant<double>.Pi * 3 / 2, Angle.Units.Radians));
		Assert.IsTrue(EqualToLeniency(cosine_neg3halfsPi, 0d, .00001d));
	}

	#endregion

	#region SquareRoot_Testing

	[TestMethod]
	public void SquareRoot_Testing()
	{
		// int
		Assert.IsTrue(SquareRoot(1) == 1);
		Assert.IsTrue(SquareRoot(2) == 1);
		Assert.IsTrue(SquareRoot(3) == 1);
		Assert.IsTrue(SquareRoot(4) == 2);
		Assert.IsTrue(SquareRoot(5) == 2);
		Assert.IsTrue(SquareRoot(6) == 2);
		Assert.IsTrue(SquareRoot(7) == 2);
		Assert.IsTrue(SquareRoot(8) == 2);
		Assert.IsTrue(SquareRoot(9) == 3);
		Assert.IsTrue(SquareRoot(10) == 3);
	}

	#endregion

	#region IsPrime_Testing

	[TestMethod]
	public void IsPrime_Testing()
	{
		int[] values =
		{
			2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101,
			103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199,
		};
		ISet<int> primeNumbers = SetHashLinked.New<int>();
		foreach (int i in values)
		{
			primeNumbers.Add(i);
		}
		for (int i = 2; i < 200; i++)
		{
			if (primeNumbers.Contains(i))
			{
				Assert.IsTrue(IsPrime(i), i.ToString());
			}
			else
			{
				Assert.IsFalse(IsPrime(i), i.ToString());
			}
		}
	}

	#endregion

	#region IsEven_Testing

	[TestMethod]
	public void IsEven_Testing()
	{
		Random random = new();

		{ // int
			bool isEven = true;
			for (int i = -100; i < 100; i++)
			{
				Assert.IsTrue(isEven ? IsEven(i) : !IsEven(i));
				isEven = !isEven;
			}
		}
		{ // float
			bool isEven = true;
			for (float i = -100f; i < 100; i++)
			{
				Assert.IsTrue(isEven ? IsEven(i) : !IsEven(i));
				isEven = !isEven;

				// only whole numbers can be even... test a random rational value
				float randomRatio = (float)random.NextDouble();
				if (randomRatio > 0d)
				{
					Assert.IsFalse(IsEven(i + randomRatio));
				}
			}
			random.NextDouble();
		}
		{ // double
			bool isEven = true;
			for (double i = -100; i < 100d; i++)
			{
				Assert.IsTrue(isEven ? IsEven(i) : !IsEven(i));
				isEven = !isEven;

				// only whole numbers can be even... test a random rational value
				double randomRatio = random.NextDouble();
				if (randomRatio > 0d)
				{
					Assert.IsFalse(IsEven(i + randomRatio));
				}
			}
		}
		{ // decimal
			bool isEven = true;
			for (decimal i = -100; i < 100m; i++)
			{
				Assert.IsTrue(isEven ? IsEven(i) : !IsEven(i));
				isEven = !isEven;

				// only whole numbers can be even... test a random rational value
				decimal randomRatio = (decimal)random.NextDouble();
				if (randomRatio > 0m)
				{
					Assert.IsFalse(IsEven(i + randomRatio));
				}
			}
		}
	}

	#endregion

	#region IsOdd_Testing

	[TestMethod]
	public void IsOdd_Testing()
	{
		Random random = new();

		{ // int
			bool isOdd = false;
			for (int i = -100; i < 100; i++)
			{
				Assert.IsTrue(isOdd == IsOdd(i));
				isOdd = !isOdd;
			}
		}
		{ // float
			bool isOdd = false;
			for (float i = -100f; i < 100; i++)
			{
				Assert.IsTrue(isOdd == IsOdd(i));
				isOdd = !isOdd;

				// only whole numbers can be even... test a random rational value
				float randomRatio = (float)random.NextDouble();
				if (randomRatio > 0d)
				{
					Assert.IsFalse(IsOdd(i + randomRatio));
				}
			}
			random.NextDouble();
		}
		{ // double
			bool isOdd = false;
			for (double i = -100; i < 100d; i++)
			{
				Assert.IsTrue(isOdd == IsOdd(i));
				isOdd = !isOdd;

				// only whole numbers can be even... test a random rational value
				double randomRatio = random.NextDouble();
				if (randomRatio > 0d)
				{
					Assert.IsFalse(IsOdd(i + randomRatio));
				}
			}
		}
		{ // decimal
			bool isOdd = false;
			for (decimal i = -100; i < 100m; i++)
			{
				Assert.IsTrue(isOdd == IsOdd(i));
				isOdd = !isOdd;

				// only whole numbers can be even... test a random rational value
				decimal randomRatio = random.NextDecimal(0, 10000) / 10000;
				if (randomRatio > 0m)
				{
					Assert.IsFalse(IsOdd(i + randomRatio));
				}
			}
		}
		{ // Ref<int>
			bool isOdd = false;
			for (int i = -100; i < 100; i++)
			{
				Assert.IsTrue(isOdd == IsOdd<Ref<int>>(i));
				isOdd = !isOdd;
			}
		}
		{ // Ref<float>
			bool isOdd = false;
			for (float i = -100f; i < 100; i++)
			{
				Assert.IsTrue(isOdd == IsOdd<Ref<float>>(i));
				isOdd = !isOdd;

				// only whole numbers can be even... test a random rational value
				float randomRatio = (float)random.NextDouble();
				if (randomRatio > 0d)
				{
					Assert.IsFalse(IsOdd<Ref<float>>(i + randomRatio));
				}
			}
			random.NextDouble();
		}
		{ // Ref<double>
			bool isOdd = false;
			for (double i = -100; i < 100d; i++)
			{
				Assert.IsTrue(isOdd == IsOdd<Ref<double>>(i));
				isOdd = !isOdd;

				// only whole numbers can be even... test a random rational value
				double randomRatio = random.NextDouble();
				if (randomRatio > 0d)
				{
					Assert.IsFalse(IsOdd<Ref<double>>(i + randomRatio));
				}
			}
		}
		{ // Ref<decimal>
			bool isOdd = false;
			for (decimal i = -100; i < 100m; i++)
			{
				Assert.IsTrue(isOdd == IsOdd<Ref<decimal>>(i));
				isOdd = !isOdd;

				// only whole numbers can be even... test a random rational value
				decimal randomRatio = random.NextDecimal(0, 10000) / 10000;
				if (randomRatio > 0m)
				{
					Assert.IsFalse(IsOdd<Ref<decimal>>(i + randomRatio));
				}
			}
		}
	}

	#endregion

	#region AbsoluteValue_Testing

	[TestMethod]
	public void AbsoluteValue_Testing()
	{
		{ // int
			Assert.IsTrue(AbsoluteValue(-3) == 3);
			Assert.IsTrue(AbsoluteValue(-2) == 2);
			Assert.IsTrue(AbsoluteValue(-1) == 1);
			Assert.IsTrue(AbsoluteValue(0) == 0);
			Assert.IsTrue(AbsoluteValue(1) == 1);
			Assert.IsTrue(AbsoluteValue(2) == 2);
			Assert.IsTrue(AbsoluteValue(3) == 3);
		}
		{ // float
			Assert.IsTrue(AbsoluteValue(-3f) == 3f);
			Assert.IsTrue(AbsoluteValue(-2f) == 2f);
			Assert.IsTrue(AbsoluteValue(-1f) == 1f);
			Assert.IsTrue(AbsoluteValue(-0.5f) == 0.5f);
			Assert.IsTrue(AbsoluteValue(0f) == 0f);
			Assert.IsTrue(AbsoluteValue(0.5f) == 0.5f);
			Assert.IsTrue(AbsoluteValue(1f) == 1f);
			Assert.IsTrue(AbsoluteValue(2f) == 2f);
			Assert.IsTrue(AbsoluteValue(3f) == 3f);
		}
		{ // double
			Assert.IsTrue(AbsoluteValue(-3d) == 3d);
			Assert.IsTrue(AbsoluteValue(-2d) == 2d);
			Assert.IsTrue(AbsoluteValue(-1d) == 1d);
			Assert.IsTrue(AbsoluteValue(-0.5d) == 0.5d);
			Assert.IsTrue(AbsoluteValue(0d) == 0d);
			Assert.IsTrue(AbsoluteValue(0.5d) == 0.5d);
			Assert.IsTrue(AbsoluteValue(1d) == 1d);
			Assert.IsTrue(AbsoluteValue(2d) == 2d);
			Assert.IsTrue(AbsoluteValue(3d) == 3d);
		}
		{ // decimal
			Assert.IsTrue(AbsoluteValue(-3m) == 3m);
			Assert.IsTrue(AbsoluteValue(-2m) == 2m);
			Assert.IsTrue(AbsoluteValue(-1m) == 1m);
			Assert.IsTrue(AbsoluteValue(-0.5m) == 0.5m);
			Assert.IsTrue(AbsoluteValue(0m) == 0m);
			Assert.IsTrue(AbsoluteValue(0.5m) == 0.5m);
			Assert.IsTrue(AbsoluteValue(1m) == 1m);
			Assert.IsTrue(AbsoluteValue(2m) == 2m);
			Assert.IsTrue(AbsoluteValue(3m) == 3m);
		}
		{ // Ref<int>

			Assert.IsTrue(AbsoluteValue<Ref<int>>(-3) == 3);
			Assert.IsTrue(AbsoluteValue<Ref<int>>(-2) == 2);
			Assert.IsTrue(AbsoluteValue<Ref<int>>(-1) == 1);
			Assert.IsTrue(AbsoluteValue<Ref<int>>( 0) == 0);
			Assert.IsTrue(AbsoluteValue<Ref<int>>( 1) == 1);
			Assert.IsTrue(AbsoluteValue<Ref<int>>( 2) == 2);
			Assert.IsTrue(AbsoluteValue<Ref<int>>( 3) == 3);
		}
		{ // Ref<float>
			Assert.IsTrue(AbsoluteValue<Ref<float>>(-3f) == 3f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>(-2f) == 2f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>(-1f) == 1f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>(-0.5f) == 0.5f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>( 0f) == 0f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>( 0.5f) == 0.5f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>( 1f) == 1f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>( 2f) == 2f);
			Assert.IsTrue(AbsoluteValue<Ref<float>>( 3f) == 3f);
		}
		{ // Ref<double>
			Assert.IsTrue(AbsoluteValue<Ref<double>>(-3d) == 3d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(-2d) == 2d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(-1d) == 1d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(-0.5d) == 0.5d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(0d) == 0d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(0.5d) == 0.5d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(1d) == 1d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(2d) == 2d);
			Assert.IsTrue(AbsoluteValue<Ref<double>>(3d) == 3d);
		}
		{ // Ref<decimal>
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(-3m) == 3m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(-2m) == 2m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(-1m) == 1m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(-0.5m) == 0.5m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(0m) == 0m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(0.5m) == 0.5m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(1m) == 1m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(2m) == 2m);
			Assert.IsTrue(AbsoluteValue<Ref<decimal>>(3m) == 3m);
		}
	}

	#endregion

	#region Maximum_Testing

	[TestMethod]
	public void Maximum_Testing()
	{
		{ // int
			Assert.IsTrue(Maximum(compare: null, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5) is (10, 5));
			Assert.IsTrue(Maximum(compare: null, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5) is (0, 5));
			Assert.IsTrue(Maximum(compare: null, 0, 4, 3, 2, 1, 5, -1, -2, -3, -4, -5) is (5, 5));
		}
		{ // float
			Assert.IsTrue(Maximum(compare: null, -5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f, 5f) is (10, 5f));
			Assert.IsTrue(Maximum(compare: null, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f) is (0, 5f));
			Assert.IsTrue(Maximum(compare: null, 0f, 4f, 3f, 2f, 1f, 5f, -1f, -2f, -3f, -4f, -5f) is (5, 5f));
			Assert.IsTrue(Maximum(compare: null, -0.5f, -0.4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 0.4f, 0.5f) is (8, 3f));
		}
		{ // double
			Assert.IsTrue(Maximum(compare: null, -5d, -4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 4d, 5d) is (10, 5d));
			Assert.IsTrue(Maximum(compare: null, 5d, 4d, 3d, 2d, 1d, 0d, -1d, -2d, -3d, -4d, -5d) is (0, 5d));
			Assert.IsTrue(Maximum(compare: null, 0d, 4d, 3d, 2d, 1d, 5d, -1d, -2d, -3d, -4d, -5d) is (5, 5d));
			Assert.IsTrue(Maximum(compare: null, -0.5d, -0.4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 0.4d, 0.5d) is (8, 3d));
		}
		{ // decimal
			Assert.IsTrue(Maximum(compare: null, -5m, -4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 4m, 5m) is (10, 5m));
			Assert.IsTrue(Maximum(compare: null, 5m, 4m, 3m, 2m, 1m, 0m, -1m, -2m, -3m, -4m, -5m) is (0, 5m));
			Assert.IsTrue(Maximum(compare: null, 0m, 4m, 3m, 2m, 1m, 5m, -1m, -2m, -3m, -4m, -5m) is (5, 5m));
			Assert.IsTrue(Maximum(compare: null, -0.5m, -0.4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 0.4m, 0.5m) is (8, 3m));
		}
	}

	#endregion

	#region MaximumValue_Testing

	[TestMethod]
	public void MaximumValue_Testing()
	{
		{ // int
			Assert.IsTrue(MaximumValue(compare: null, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5) is 5);
			Assert.IsTrue(MaximumValue(compare: null, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5) is 5);
			Assert.IsTrue(MaximumValue(compare: null, 0, 4, 3, 2, 1, 5, -1, -2, -3, -4, -5) is 5);
		}
		{ // float
			Assert.IsTrue(MaximumValue(compare: null, -5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f, 5f) is 5f);
			Assert.IsTrue(MaximumValue(compare: null, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f) is 5f);
			Assert.IsTrue(MaximumValue(compare: null, 0f, 4f, 3f, 2f, 1f, 5f, -1f, -2f, -3f, -4f, -5f) is 5f);
			Assert.IsTrue(MaximumValue(compare: null, -0.5f, -0.4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 0.4f, 0.5f) is 3f);
		}
		{ // double
			Assert.IsTrue(MaximumValue(compare: null, -5d, -4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 4d, 5d) is 5d);
			Assert.IsTrue(MaximumValue(compare: null, 5d, 4d, 3d, 2d, 1d, 0d, -1d, -2d, -3d, -4d, -5d) is 5d);
			Assert.IsTrue(MaximumValue(compare: null, 0d, 4d, 3d, 2d, 1d, 5d, -1d, -2d, -3d, -4d, -5d) is 5d);
			Assert.IsTrue(MaximumValue(compare: null, -0.5d, -0.4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 0.4d, 0.5d) is 3d);
		}
		{ // decimal
			Assert.IsTrue(MaximumValue(compare: null, -5m, -4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 4m, 5m) is 5m);
			Assert.IsTrue(MaximumValue(compare: null, 5m, 4m, 3m, 2m, 1m, 0m, -1m, -2m, -3m, -4m, -5m) is 5m);
			Assert.IsTrue(MaximumValue(compare: null, 0m, 4m, 3m, 2m, 1m, 5m, -1m, -2m, -3m, -4m, -5m) is 5m);
			Assert.IsTrue(MaximumValue(compare: null, -0.5m, -0.4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 0.4m, 0.5m) is 3m);
		}
	}

	#endregion

	#region MaximumIndex_Testing

	[TestMethod]
	public void MaximumIndex_Testing()
	{
		{ // int
			Assert.IsTrue(MaximumIndex(compare: null, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5) is 10);
			Assert.IsTrue(MaximumIndex(compare: null, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5) is 0);
			Assert.IsTrue(MaximumIndex(compare: null, 0, 4, 3, 2, 1, 5, -1, -2, -3, -4, -5) is 5);
		}
		{ // float
			Assert.IsTrue(MaximumIndex(compare: null, -5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f, 5f) is 10);
			Assert.IsTrue(MaximumIndex(compare: null, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f) is 0);
			Assert.IsTrue(MaximumIndex(compare: null, 0f, 4f, 3f, 2f, 1f, 5f, -1f, -2f, -3f, -4f, -5f) is 5);
			Assert.IsTrue(MaximumIndex(compare: null, -0.5f, -0.4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 0.4f, 0.5f) is 8);
		}
		{ // double
			Assert.IsTrue(MaximumIndex(compare: null, -5d, -4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 4d, 5d) is 10);
			Assert.IsTrue(MaximumIndex(compare: null, 5d, 4d, 3d, 2d, 1d, 0d, -1d, -2d, -3d, -4d, -5d) is 0);
			Assert.IsTrue(MaximumIndex(compare: null, 0d, 4d, 3d, 2d, 1d, 5d, -1d, -2d, -3d, -4d, -5d) is 5);
			Assert.IsTrue(MaximumIndex(compare: null, -0.5d, -0.4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 0.4d, 0.5d) is 8);
		}
		{ // decimal
			Assert.IsTrue(MaximumIndex(compare: null, -5m, -4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 4m, 5m) is 10);
			Assert.IsTrue(MaximumIndex(compare: null, 5m, 4m, 3m, 2m, 1m, 0m, -1m, -2m, -3m, -4m, -5m) is 0);
			Assert.IsTrue(MaximumIndex(compare: null, 0m, 4m, 3m, 2m, 1m, 5m, -1m, -2m, -3m, -4m, -5m) is 5);
			Assert.IsTrue(MaximumIndex(compare: null, -0.5m, -0.4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 0.4m, 0.5m) is 8);
		}
	}

	#endregion

	#region Minimum_Testing

	[TestMethod]
	public void Minimum_Testing()
	{
		{ // int
			Assert.IsTrue(Minimum(compare: null, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5) is (0, -5));
			Assert.IsTrue(Minimum(compare: null, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5) is (10, -5));
			Assert.IsTrue(Minimum(compare: null, 0, 4, 3, 2, 1, 5, -1, -2, -3, -4, -5) is (10, -5));
		}
		{ // float
			Assert.IsTrue(Minimum(compare: null, -5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f, 5f) is (0, -5f));
			Assert.IsTrue(Minimum(compare: null, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f) is (10, -5f));
			Assert.IsTrue(Minimum(compare: null, 0f, 4f, 3f, 2f, 1f, 5f, -1f, -2f, -3f, -4f, -5f) is (10, -5f));
			Assert.IsTrue(Minimum(compare: null, -0.5f, -0.4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 0.4f, 0.5f) is (2, -3f));
		}
		{ // double
			Assert.IsTrue(Minimum(compare: null, -5d, -4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 4d, 5d) is (0, -5d));
			Assert.IsTrue(Minimum(compare: null, 5d, 4d, 3d, 2d, 1d, 0d, -1d, -2d, -3d, -4d, -5d) is (10, -5d));
			Assert.IsTrue(Minimum(compare: null, 0d, 4d, 3d, 2d, 1d, 5d, -1d, -2d, -3d, -4d, -5d) is (10, -5d));
			Assert.IsTrue(Minimum(compare: null, -0.5d, -0.4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 0.4d, 0.5d) is (2, -3d));
		}
		{ // decimal
			Assert.IsTrue(Minimum(compare: null, -5m, -4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 4m, 5m) is (0, -5m));
			Assert.IsTrue(Minimum(compare: null, 5m, 4m, 3m, 2m, 1m, 0m, -1m, -2m, -3m, -4m, -5m) is (10, -5m));
			Assert.IsTrue(Minimum(compare: null, 0m, 4m, 3m, 2m, 1m, 5m, -1m, -2m, -3m, -4m, -5m) is (10, -5m));
			Assert.IsTrue(Minimum(compare: null, -0.5m, -0.4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 0.4m, 0.5m) is (2, -3m));
		}
	}

	#endregion

	#region MinimumValue_Testing

	[TestMethod]
	public void MinimumValue_Testing()
	{
		{ // int
			Assert.IsTrue(MinimumValue(compare: null, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5) is -5);
			Assert.IsTrue(MinimumValue(compare: null, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5) is -5);
			Assert.IsTrue(MinimumValue(compare: null, 0, 4, 3, 2, 1, 5, -1, -2, -3, -4, -5) is -5);
		}
		{ // float
			Assert.IsTrue(MinimumValue(compare: null, -5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f, 5f) is -5f);
			Assert.IsTrue(MinimumValue(compare: null, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f) is -5f);
			Assert.IsTrue(MinimumValue(compare: null, 0f, 4f, 3f, 2f, 1f, 5f, -1f, -2f, -3f, -4f, -5f) is -5f);
			Assert.IsTrue(MinimumValue(compare: null, -0.5f, -0.4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 0.4f, 0.5f) is -3f);
		}
		{ // double
			Assert.IsTrue(MinimumValue(compare: null, -5d, -4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 4d, 5d) is -5d);
			Assert.IsTrue(MinimumValue(compare: null, 5d, 4d, 3d, 2d, 1d, 0d, -1d, -2d, -3d, -4d, -5d) is -5d);
			Assert.IsTrue(MinimumValue(compare: null, 0d, 4d, 3d, 2d, 1d, 5d, -1d, -2d, -3d, -4d, -5d) is -5d);
			Assert.IsTrue(MinimumValue(compare: null, -0.5d, -0.4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 0.4d, 0.5d) is -3d);
		}
		{ // decimal
			Assert.IsTrue(MinimumValue(compare: null, -5m, -4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 4m, 5m) is -5m);
			Assert.IsTrue(MinimumValue(compare: null, 5m, 4m, 3m, 2m, 1m, 0m, -1m, -2m, -3m, -4m, -5m) is -5m);
			Assert.IsTrue(MinimumValue(compare: null, 0m, 4m, 3m, 2m, 1m, 5m, -1m, -2m, -3m, -4m, -5m) is -5m);
			Assert.IsTrue(MinimumValue(compare: null, -0.5m, -0.4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 0.4m, 0.5m) is -3m);
		}
	}

	#endregion

	#region MinimumIndex_Testing

	[TestMethod]
	public void MinimumIndex_Testing()
	{
		{ // int
			Assert.IsTrue(MinimumIndex(compare: null, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5) is 0);
			Assert.IsTrue(MinimumIndex(compare: null, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5) is 10);
			Assert.IsTrue(MinimumIndex(compare: null, 0, 4, 3, 2, 1, 5, -1, -2, -3, -4, -5) is 10);
		}
		{ // float
			Assert.IsTrue(MinimumIndex(compare: null, -5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f, 5f) is 0);
			Assert.IsTrue(MinimumIndex(compare: null, 5f, 4f, 3f, 2f, 1f, 0f, -1f, -2f, -3f, -4f, -5f) is 10);
			Assert.IsTrue(MinimumIndex(compare: null, 0f, 4f, 3f, 2f, 1f, 5f, -1f, -2f, -3f, -4f, -5f) is 10);
			Assert.IsTrue(MinimumIndex(compare: null, -0.5f, -0.4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 0.4f, 0.5f) is 2);
		}
		{ // double
			Assert.IsTrue(MinimumIndex(compare: null, -5d, -4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 4d, 5d) is 0);
			Assert.IsTrue(MinimumIndex(compare: null, 5d, 4d, 3d, 2d, 1d, 0d, -1d, -2d, -3d, -4d, -5d) is 10);
			Assert.IsTrue(MinimumIndex(compare: null, 0d, 4d, 3d, 2d, 1d, 5d, -1d, -2d, -3d, -4d, -5d) is 10);
			Assert.IsTrue(MinimumIndex(compare: null, -0.5d, -0.4d, -3d, -2d, -1d, 0d, 1d, 2d, 3d, 0.4d, 0.5d) is 2);
		}
		{ // decimal
			Assert.IsTrue(MinimumIndex(compare: null, -5m, -4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 4m, 5m) is 0);
			Assert.IsTrue(MinimumIndex(compare: null, 5m, 4m, 3m, 2m, 1m, 0m, -1m, -2m, -3m, -4m, -5m) is 10);
			Assert.IsTrue(MinimumIndex(compare: null, 0m, 4m, 3m, 2m, 1m, 5m, -1m, -2m, -3m, -4m, -5m) is 10);
			Assert.IsTrue(MinimumIndex(compare: null, -0.5m, -0.4m, -3m, -2m, -1m, 0m, 1m, 2m, 3m, 0.4m, 0.5m) is 2);
		}
	}

	#endregion

	#region LeastCommonMultiple_Testing

	[TestMethod]
	public void LeastCommonMultiple_Testing()
	{
		{ // int
			Assert.IsTrue(LeastCommonMultiple(1, 2, 3) == 6);
			Assert.IsTrue(LeastCommonMultiple(1, 2, 3, 4) == 12);
			Assert.IsTrue(LeastCommonMultiple(1, 2, 3, 4, 5) == 60);
			Assert.IsTrue(LeastCommonMultiple(1, 2, 3, 4, 5, 6) == 60);
			Assert.IsTrue(LeastCommonMultiple(1, 2, 3, 4, 5, 6, 7) == 420);
			Assert.IsTrue(LeastCommonMultiple(1, 2, 3, 4, 5, 6, 7, 8) == 840);
		}
		{ // float
			Assert.IsTrue(LeastCommonMultiple(1f, 2f, 3f) == 6f);
			Assert.IsTrue(LeastCommonMultiple(1f, 2f, 3f, 4f) == 12f);
			Assert.IsTrue(LeastCommonMultiple(1f, 2f, 3f, 4f, 5f) == 60f);
			Assert.IsTrue(LeastCommonMultiple(1f, 2f, 3f, 4f, 5f, 6f) == 60f);
			Assert.IsTrue(LeastCommonMultiple(1f, 2f, 3f, 4f, 5f, 6f, 7f) == 420f);
			Assert.IsTrue(LeastCommonMultiple(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f) == 840f);
		}
		{ // double
			Assert.IsTrue(LeastCommonMultiple(1d, 2d, 3d) == 6d);
			Assert.IsTrue(LeastCommonMultiple(1d, 2d, 3d, 4d) == 12d);
			Assert.IsTrue(LeastCommonMultiple(1d, 2d, 3d, 4d, 5d) == 60d);
			Assert.IsTrue(LeastCommonMultiple(1d, 2d, 3d, 4d, 5d, 6d) == 60d);
			Assert.IsTrue(LeastCommonMultiple(1d, 2d, 3d, 4d, 5d, 6d, 7d) == 420d);
			Assert.IsTrue(LeastCommonMultiple(1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d) == 840d);
		}
		{ // decimal
			Assert.IsTrue(LeastCommonMultiple(1m, 2m, 3m) == 6m);
			Assert.IsTrue(LeastCommonMultiple(1m, 2m, 3m, 4m) == 12m);
			Assert.IsTrue(LeastCommonMultiple(1m, 2m, 3m, 4m, 5m) == 60m);
			Assert.IsTrue(LeastCommonMultiple(1m, 2m, 3m, 4m, 5m, 6m) == 60m);
			Assert.IsTrue(LeastCommonMultiple(1m, 2m, 3m, 4m, 5m, 6m, 7m) == 420m);
			Assert.IsTrue(LeastCommonMultiple(1m, 2m, 3m, 4m, 5m, 6m, 7m, 8m) == 840m);
		}
	}

	#endregion

	#region GreatestCommonFactor_Testing

	[TestMethod]
	public void GreatestCommonFactor_Testing()
	{
		{ // int
			Assert.IsTrue(GreatestCommonFactor(10, 20, 40) == 10);
			Assert.IsTrue(GreatestCommonFactor(15, 25, 45) == 5);
			Assert.IsTrue(GreatestCommonFactor(2, 2, 4) == 2);
			Assert.IsTrue(GreatestCommonFactor(3, 5, 7) == 1);
			Assert.IsTrue(GreatestCommonFactor(-2, -4, -6) == 2);
		}
		{ // float
			Assert.IsTrue(GreatestCommonFactor(10f, 20f, 40f) == 10f);
			Assert.IsTrue(GreatestCommonFactor(15f, 25f, 45f) == 5f);
			Assert.IsTrue(GreatestCommonFactor(2f, 2f, 4f) == 2f);
			Assert.IsTrue(GreatestCommonFactor(3f, 5f, 7f) == 1f);
			Assert.IsTrue(GreatestCommonFactor(-2f, -4f, -6f) == 2f);
		}
		{ // double
			Assert.IsTrue(GreatestCommonFactor(10d, 20d, 40d) == 10d);
			Assert.IsTrue(GreatestCommonFactor(15d, 25d, 45d) == 5d);
			Assert.IsTrue(GreatestCommonFactor(2d, 2d, 4d) == 2d);
			Assert.IsTrue(GreatestCommonFactor(3d, 5d, 7d) == 1d);
			Assert.IsTrue(GreatestCommonFactor(-2d, -4d, -6d) == 2d);
		}
		{ // decimal
			Assert.IsTrue(GreatestCommonFactor(10m, 20m, 40m) == 10m);
			Assert.IsTrue(GreatestCommonFactor(15m, 25m, 45m) == 5m);
			Assert.IsTrue(GreatestCommonFactor(2m, 2m, 4m) == 2m);
			Assert.IsTrue(GreatestCommonFactor(3m, 5m, 7m) == 1m);
			Assert.IsTrue(GreatestCommonFactor(-2m, -4m, -6m) == 2m);
		}
	}

	#endregion

	#region Factorial_Testing

	[TestMethod]
	public void Factorial_Testing()
	{
		{ // int
			Assert.IsTrue(Factorial(1) == 1);
			Assert.IsTrue(Factorial(2) == 2);
			Assert.IsTrue(Factorial(3) == 6);
			Assert.IsTrue(Factorial(4) == 24);
			Assert.IsTrue(Factorial(5) == 120);
		}
		{ // float
			Assert.IsTrue(Factorial(1f) == 1);
			Assert.IsTrue(Factorial(2f) == 2);
			Assert.IsTrue(Factorial(3f) == 6);
			Assert.IsTrue(Factorial(4f) == 24);
			Assert.IsTrue(Factorial(5f) == 120);
		}
		{ // double
			Assert.IsTrue(Factorial(1d) == 1);
			Assert.IsTrue(Factorial(2d) == 2);
			Assert.IsTrue(Factorial(3d) == 6);
			Assert.IsTrue(Factorial(4d) == 24);
			Assert.IsTrue(Factorial(5d) == 120);
		}
		{ // decimal
			Assert.IsTrue(Factorial(1m) == 1);
			Assert.IsTrue(Factorial(2m) == 2);
			Assert.IsTrue(Factorial(3m) == 6);
			Assert.IsTrue(Factorial(4m) == 24);
			Assert.IsTrue(Factorial(5m) == 120);
		}
	}

	#endregion

	#region BinomialCoefficient_Testing

	[TestMethod]
	public void BinomialCoefficient_Testing()
	{
		{ // int
			Assert.IsTrue(BinomialCoefficient(1, 1) == 1);
			Assert.IsTrue(BinomialCoefficient(7, 2) == 21);
			Assert.IsTrue(BinomialCoefficient(8, 4) == 70);
			Assert.IsTrue(BinomialCoefficient(10, 3) == 120);
			Assert.IsTrue(BinomialCoefficient(11, 5) == 462);
		}
		{ // float
			Assert.IsTrue(BinomialCoefficient(1f, 1f) == 1f);
			Assert.IsTrue(BinomialCoefficient(7f, 2f) == 21f);
			Assert.IsTrue(BinomialCoefficient(8f, 4f) == 70f);
			Assert.IsTrue(BinomialCoefficient(10f, 3f) == 120f);
			Assert.IsTrue(BinomialCoefficient(11f, 5f) == 462f);
		}
		{ // double
			Assert.IsTrue(BinomialCoefficient(1d, 1d) == 1d);
			Assert.IsTrue(BinomialCoefficient(7d, 2d) == 21d);
			Assert.IsTrue(BinomialCoefficient(8d, 4d) == 70d);
			Assert.IsTrue(BinomialCoefficient(10d, 3d) == 120d);
			Assert.IsTrue(BinomialCoefficient(11d, 5d) == 462d);
		}
		{ // decimal
			Assert.IsTrue(BinomialCoefficient(1m, 1m) == 1m);
			Assert.IsTrue(BinomialCoefficient(7m, 2m) == 21m);
			Assert.IsTrue(BinomialCoefficient(8m, 4m) == 70m);
			Assert.IsTrue(BinomialCoefficient(10m, 3m) == 120m);
			Assert.IsTrue(BinomialCoefficient(11m, 5m) == 462m);
		}
	}

	#endregion

	#region Median_Testing

	[TestMethod]
	public void Median_Testing()
	{
		{ // int
			Assert.IsTrue(Median(1, 2, 3, 4, 5) == 3);
			Assert.IsTrue(Median(3, 1, 2, 4, 5) == 3);
			Assert.IsTrue(Median(1, 2, 4, 5, 3) == 3);
		}
		{ // float
			Assert.IsTrue(Median(1f, 2f, 3f, 4f, 5f) == 3f);
			Assert.IsTrue(Median(3f, 1f, 2f, 4f, 5f) == 3f);
			Assert.IsTrue(Median(1f, 2f, 4f, 5f, 3f) == 3f);

			Assert.IsTrue(Median(1f, 2f, 3f, 4f, 5f, 6f) == 3.5f);
		}
		{ // double
			Assert.IsTrue(Median(1d, 2d, 3d, 4d, 5d) == 3d);
			Assert.IsTrue(Median(3d, 1d, 2d, 4d, 5d) == 3d);
			Assert.IsTrue(Median(1d, 2d, 4d, 5d, 3d) == 3d);

			Assert.IsTrue(Median(1d, 2d, 3d, 4d, 5d, 6d) == 3.5d);
		}
		{ // decimal
			Assert.IsTrue(Median(1m, 2m, 3m, 4m, 5m) == 3m);
			Assert.IsTrue(Median(3m, 1m, 2m, 4m, 5m) == 3m);
			Assert.IsTrue(Median(1m, 2m, 4m, 5m, 3m) == 3m);

			Assert.IsTrue(Median(1m, 2m, 3m, 4m, 5m, 6m) == 3.5m);
		}
	}

	#endregion

	#region Mode_Testing

	[TestMethod]
	public void Mode_Testing()
	{
		{ // int
			ListArray<int> list = new();
			Mode(x => list.Add(x), 1, 2, 3, 4, 5, 1);
			Assert.IsTrue(list.Count is 1);
			Assert.IsTrue(list[0] is 1);
		}
		{ // float
			ListArray<float> list = new();
			Mode(x => list.Add(x), 1f, 2f, 3f, 4f, 5f, 1f);
			Assert.IsTrue(list.Count is 1);
			Assert.IsTrue(list[0] is 1f);
		}
		{ // double
			ListArray<double> list = new();
			Mode(x => list.Add(x), 1d, 2d, 3d, 4d, 5d, 1d);
			Assert.IsTrue(list.Count is 1);
			Assert.IsTrue(list[0] is 1d);
		}
		{ // decimal
			ListArray<decimal> list = new();
			Mode(x => list.Add(x), 1m, 2m, 3m, 4m, 5m, 1m);
			Assert.IsTrue(list.Count is 1);
			Assert.IsTrue(list[0] is 1m);
		}
	}

	#endregion

	#region Clamp

	[TestMethod]
	public void Clamp_Testing()
	{
		{ // int
			Assert.IsTrue(Clamp(5, 3, 7) == 5);
			Assert.IsTrue(Clamp(3, 5, 7) == 5);
			Assert.IsTrue(Clamp(9, 3, 7) == 7);
		}
		{ // float
			Assert.IsTrue(Clamp(5f, 3f, 7f) == 5f);
			Assert.IsTrue(Clamp(3f, 5f, 7f) == 5f);
			Assert.IsTrue(Clamp(9f, 3f, 7f) == 7f);
		}
		{ // double
			Assert.IsTrue(Clamp(5d, 3d, 7d) == 5d);
			Assert.IsTrue(Clamp(3d, 5d, 7d) == 5d);
			Assert.IsTrue(Clamp(9d, 3d, 7d) == 7d);
		}
		{ // decimal
			Assert.IsTrue(Clamp(5m, 3m, 7m) == 5m);
			Assert.IsTrue(Clamp(3m, 5m, 7m) == 5m);
			Assert.IsTrue(Clamp(9m, 3m, 7m) == 7m);
		}

		{ // Ref<int>
			Assert.IsTrue(Clamp<Ref<int>>(5, 3, 7) == 5);
			Assert.IsTrue(Clamp<Ref<int>>(3, 5, 7) == 5);
			Assert.IsTrue(Clamp<Ref<int>>(9, 3, 7) == 7);
		}
		{ // Ref<float>
			Assert.IsTrue(Clamp<Ref<float>>(5f, 3f, 7f) == 5f);
			Assert.IsTrue(Clamp<Ref<float>>(3f, 5f, 7f) == 5f);
			Assert.IsTrue(Clamp<Ref<float>>(9f, 3f, 7f) == 7f);
		}
		{ // Ref<double>
			Assert.IsTrue(Clamp<Ref<double>>(5d, 3d, 7d) == 5d);
			Assert.IsTrue(Clamp<Ref<double>>(3d, 5d, 7d) == 5d);
			Assert.IsTrue(Clamp<Ref<double>>(9d, 3d, 7d) == 7d);
		}
		{ // Ref<decimal>
			Assert.IsTrue(Clamp<Ref<decimal>>(5m, 3m, 7m) == 5m);
			Assert.IsTrue(Clamp<Ref<decimal>>(3m, 5m, 7m) == 5m);
			Assert.IsTrue(Clamp<Ref<decimal>>(9m, 3m, 7m) == 7m);
		}
	}

	#endregion

	#region InequalTo_Testing

	[TestMethod]
	public void InequalTo_Testing()
	{
		Assert.IsTrue(Inequate(0, 1));
		Assert.IsTrue(Inequate(-1, 1));
		Assert.IsFalse(Inequate(1, 1));
		Assert.IsTrue(Inequate(6, 7));

		Assert.IsTrue(Inequate(0f, 1f));
		Assert.IsTrue(Inequate(-1f, 1f));
		Assert.IsFalse(Inequate(1f, 1f));
		Assert.IsTrue(Inequate(6f, 7f));

		Assert.IsTrue(Inequate(0d, 1d));
		Assert.IsTrue(Inequate(-1d, 1d));
		Assert.IsFalse(Inequate(1d, 1d));
		Assert.IsTrue(Inequate(6d, 7d));

		Assert.IsTrue(Inequate(0m, 1m));
		Assert.IsTrue(Inequate(-1m, 1m));
		Assert.IsFalse(Inequate(1m, 1m));
		Assert.IsTrue(Inequate(6m, 7m));
	}

	#endregion

	#region Comparison_Testing

	[TestMethod]
	public void Comparison_Testing()
	{
		Assert.IsTrue(Compare(0, 0) is Equal);
		Assert.IsTrue(Compare(-1, 0) is Less);
		Assert.IsTrue(Compare(1, 0) is Greater);
		Assert.IsTrue(Compare(777, 333) is Greater);
		Assert.IsTrue(Compare(333, 777) is Less);
		Assert.IsTrue(Compare(777, 777) is Equal);

		Assert.IsTrue(Compare(0f, 0f) is Equal);
		Assert.IsTrue(Compare(-1f, 0f) is Less);
		Assert.IsTrue(Compare(1f, 0f) is Greater);
		Assert.IsTrue(Compare(777f, 333f) is Greater);
		Assert.IsTrue(Compare(333f, 777f) is Less);
		Assert.IsTrue(Compare(777f, 777f) is Equal);

		Assert.IsTrue(Compare(0d, 0d) is Equal);
		Assert.IsTrue(Compare(-1d, 0d) is Less);
		Assert.IsTrue(Compare(1d, 0d) is Greater);
		Assert.IsTrue(Compare(777d, 333d) is Greater);
		Assert.IsTrue(Compare(333d, 777d) is Less);
		Assert.IsTrue(Compare(777d, 777d) is Equal);

		Assert.IsTrue(Compare(0m, 0m) is Equal);
		Assert.IsTrue(Compare(-1m, 0m) is Less);
		Assert.IsTrue(Compare(1m, 0m) is Greater);
		Assert.IsTrue(Compare(777m, 333m) is Greater);
		Assert.IsTrue(Compare(333m, 777m) is Less);
		Assert.IsTrue(Compare(777m, 777m) is Equal);
	}

	#endregion

	#region LessThan_Testing

	[TestMethod]
	public void LessThan_Testing()
	{
		Assert.IsFalse(LessThan(0, 0));
		Assert.IsTrue(LessThan(-1, 0));
		Assert.IsFalse(LessThan(1, 0));
		Assert.IsFalse(LessThan(777, 333));
		Assert.IsTrue(LessThan(333, 777));
		Assert.IsFalse(LessThan(777, 777));

		Assert.IsFalse(LessThan(0f, 0f));
		Assert.IsTrue(LessThan(-1f, 0f));
		Assert.IsFalse(LessThan(1f, 0f));
		Assert.IsFalse(LessThan(777f, 333f));
		Assert.IsTrue(LessThan(333f, 777f));
		Assert.IsFalse(LessThan(777f, 777f));

		Assert.IsFalse(LessThan(0d, 0d));
		Assert.IsTrue(LessThan(-1d, 0d));
		Assert.IsFalse(LessThan(1d, 0d));
		Assert.IsFalse(LessThan(777d, 333d));
		Assert.IsTrue(LessThan(333d, 777d));
		Assert.IsFalse(LessThan(777d, 777d));

		Assert.IsFalse(LessThan(0m, 0m));
		Assert.IsTrue(LessThan(-1m, 0m));
		Assert.IsFalse(LessThan(1m, 0m));
		Assert.IsFalse(LessThan(777m, 333m));
		Assert.IsTrue(LessThan(333m, 777m));
		Assert.IsFalse(LessThan(777m, 777m));
	}

	#endregion

	#region GreaterThan_Testing

	[TestMethod]
	public void GreaterThan_Testing()
	{
		Assert.IsFalse(GreaterThan(0, 0));
		Assert.IsFalse(GreaterThan(-1, 0));
		Assert.IsTrue(GreaterThan(1, 0));
		Assert.IsTrue(GreaterThan(777, 333));
		Assert.IsFalse(GreaterThan(333, 777));
		Assert.IsFalse(GreaterThan(777, 777));

		Assert.IsFalse(GreaterThan(0f, 0f));
		Assert.IsFalse(GreaterThan(-1f, 0f));
		Assert.IsTrue(GreaterThan(1f, 0f));
		Assert.IsTrue(GreaterThan(777f, 333f));
		Assert.IsFalse(GreaterThan(333f, 777f));
		Assert.IsFalse(GreaterThan(777f, 777f));

		Assert.IsFalse(GreaterThan(0d, 0d));
		Assert.IsFalse(GreaterThan(-1d, 0d));
		Assert.IsTrue(GreaterThan(1d, 0d));
		Assert.IsTrue(GreaterThan(777d, 333d));
		Assert.IsFalse(GreaterThan(333d, 777d));
		Assert.IsFalse(GreaterThan(777d, 777d));

		Assert.IsFalse(GreaterThan(0m, 0m));
		Assert.IsFalse(GreaterThan(-1m, 0m));
		Assert.IsTrue(GreaterThan(1m, 0m));
		Assert.IsTrue(GreaterThan(777m, 333m));
		Assert.IsFalse(GreaterThan(333m, 777m));
		Assert.IsFalse(GreaterThan(777m, 777m));
	}

	#endregion

	#region LessThanOrEqual_Testing

	[TestMethod]
	public void LessThanOrEqual_Testing()
	{
		Assert.IsTrue(LessThanOrEqual(0, 0));
		Assert.IsTrue(LessThanOrEqual(-1, 0));
		Assert.IsFalse(LessThanOrEqual(1, 0));
		Assert.IsFalse(LessThanOrEqual(777, 333));
		Assert.IsTrue(LessThanOrEqual(333, 777));
		Assert.IsTrue(LessThanOrEqual(777, 777));

		Assert.IsTrue(LessThanOrEqual(0f, 0f));
		Assert.IsTrue(LessThanOrEqual(-1f, 0f));
		Assert.IsFalse(LessThanOrEqual(1f, 0f));
		Assert.IsFalse(LessThanOrEqual(777f, 333f));
		Assert.IsTrue(LessThanOrEqual(333f, 777f));
		Assert.IsTrue(LessThanOrEqual(777f, 777f));

		Assert.IsTrue(LessThanOrEqual(0d, 0d));
		Assert.IsTrue(LessThanOrEqual(-1d, 0d));
		Assert.IsFalse(LessThanOrEqual(1d, 0d));
		Assert.IsFalse(LessThanOrEqual(777d, 333d));
		Assert.IsTrue(LessThanOrEqual(333d, 777d));
		Assert.IsTrue(LessThanOrEqual(777d, 777d));

		Assert.IsTrue(LessThanOrEqual(0m, 0m));
		Assert.IsTrue(LessThanOrEqual(-1m, 0m));
		Assert.IsFalse(LessThanOrEqual(1m, 0m));
		Assert.IsFalse(LessThanOrEqual(777m, 333m));
		Assert.IsTrue(LessThanOrEqual(333m, 777m));
		Assert.IsTrue(LessThanOrEqual(777m, 777m));
	}

	#endregion

	#region GreaterThanOrEqual_Testing

	[TestMethod]
	public void GreaterThanOrEqual_Testing()
	{
		Assert.IsTrue(GreaterThanOrEqual(0, 0));
		Assert.IsFalse(GreaterThanOrEqual(-1, 0));
		Assert.IsTrue(GreaterThanOrEqual(1, 0));
		Assert.IsTrue(GreaterThanOrEqual(777, 333));
		Assert.IsFalse(GreaterThanOrEqual(333, 777));
		Assert.IsTrue(GreaterThanOrEqual(777, 777));

		Assert.IsTrue(GreaterThanOrEqual(0f, 0f));
		Assert.IsFalse(GreaterThanOrEqual(-1f, 0f));
		Assert.IsTrue(GreaterThanOrEqual(1f, 0f));
		Assert.IsTrue(GreaterThanOrEqual(777f, 333f));
		Assert.IsFalse(GreaterThanOrEqual(333f, 777f));
		Assert.IsTrue(GreaterThanOrEqual(777f, 777f));

		Assert.IsTrue(GreaterThanOrEqual(0d, 0d));
		Assert.IsFalse(GreaterThanOrEqual(-1d, 0d));
		Assert.IsTrue(GreaterThanOrEqual(1d, 0d));
		Assert.IsTrue(GreaterThanOrEqual(777d, 333d));
		Assert.IsFalse(GreaterThanOrEqual(333d, 777d));
		Assert.IsTrue(GreaterThanOrEqual(777d, 777d));

		Assert.IsTrue(GreaterThanOrEqual(0m, 0m));
		Assert.IsFalse(GreaterThanOrEqual(-1m, 0m));
		Assert.IsTrue(GreaterThanOrEqual(1m, 0m));
		Assert.IsTrue(GreaterThanOrEqual(777m, 333m));
		Assert.IsFalse(GreaterThanOrEqual(333m, 777m));
		Assert.IsTrue(GreaterThanOrEqual(777m, 777m));
	}

	#endregion

	#region FactorPrimes_Testing

	public static void FactorPrimes_Test<T>(T value, params T[] expectedFactors)
	{
		IList<T> list = new ListLinked<T>();
		expectedFactors.Stepper(x => list.Add(x));
		Assert.IsTrue(list.Count > 0);
		FactorPrimes(value, x => list.RemoveFirst(x));
		Assert.IsTrue(list.Count == 0);
	}

	[TestMethod]
	public void FactorPrimes_Testing()
	{
		{ // int
			FactorPrimes_Test<int>(2,  /* factors: */ 2);
			FactorPrimes_Test<int>(4,  /* factors: */ 2, 2);
			FactorPrimes_Test<int>(7,  /* factors: */ 7);
			FactorPrimes_Test<int>(9,  /* factors: */ 3, 3);
			FactorPrimes_Test<int>(10, /* factors: */ 2, 5);
			FactorPrimes_Test<int>(15, /* factors: */ 3, 5);
			FactorPrimes_Test<int>(21, /* factors: */ 7, 3);
		}
		{ // decimal
			FactorPrimes_Test<decimal>(2,  /* factors: */ 2);
			FactorPrimes_Test<decimal>(4,  /* factors: */ 2, 2);
			FactorPrimes_Test<decimal>(7,  /* factors: */ 7);
			FactorPrimes_Test<decimal>(9,  /* factors: */ 3, 3);
			FactorPrimes_Test<decimal>(10, /* factors: */ 2, 5);
			FactorPrimes_Test<decimal>(15, /* factors: */ 3, 5);
			FactorPrimes_Test<decimal>(21, /* factors: */ 7, 3);
		}
	}

	#endregion
}
