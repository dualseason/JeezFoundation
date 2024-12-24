﻿using System.Reflection;

namespace JeezFoundation.Algorithm;

/// <summary>Contains static helpers for handling command line input and output.</summary>
public static class CommandLine
{
    /// <summary>Handles the command line arguments by invoking the relative <see cref="CommandAttribute"/> method in the calling <see cref="Assembly"/>.</summary>
    /// <param name="args">The command line arguments.</param>
    public static void HandleArguments(string[]? args = null)
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        args ??= Environment.GetCommandLineArgs();
        if (args.Length < 1)
        {
            Console.Error.WriteLine("No command provided.");
            return;
        }
        ListArray<MethodInfo> commandMatches = new();
        foreach (MethodInfo possibleCommand in assembly.GetMethodInfosWithAttribute<CommandAttribute>())
        {
            string command = possibleCommand.Name;
            if (command == args[0])
            {
                commandMatches.Add(possibleCommand);
            }
        }
        if (commandMatches.Count > 1)
        {
            throw new Exception("syntax error: multiple matching commands");
        }
        else if (commandMatches.Count <= 0)
        {
            string arg = args[0].ToLower().Replace("-", string.Empty);
            if (arg is "help" || arg is "h")
            {
                if (args.Length is 2)
                {
                    DefaultHelp(assembly, args[1]);
                }
                else
                {
                    DefaultHelp(assembly);
                }
                return;
            }
            if (arg is "version" ||
                arg is "v")
            {
                DefaultVersion(assembly);
                return;
            }
            Console.Error.WriteLine("command not found");
            return;
        }
        if ((args.Length - 1) % 2 != 0)
        {
            Console.Error.WriteLine($"Invalid argument count.");
            return;
        }
        MethodInfo methodInfo = commandMatches[0];
        if (!methodInfo.IsStatic)
        {
            throw new Exception("syntax error: relative command not static");
        }

        MapHashLinked<int, string, StringEquate, StringHash> parameterMap = new();
        int parameterCount = 0;
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        foreach (ParameterInfo parameterInfo in parameterInfos)
        {
            if (parameterInfo.Name is null) throw new Exception("encountered a null parameter name");
            parameterMap.Add(parameterInfo.Name, parameterCount++);
        }
        object?[] parameters = new object[parameterCount];
        for (int i = 1; i < args.Length; i += 2)
        {
            string arg = args[i];
            if (arg.Length < 3 || arg[0] is not '-' || arg[1] is not '-')
            {
                Console.Error.WriteLine($"Invalid parameter {arg} in index {i}.");
                return;
            }
            arg = arg[2..];
            var (success, _, index) = parameterMap.TryGet(arg);
            if (!success)
            {
                Console.Error.WriteLine($"Invalid parameter --{arg} in index {i}.");
                return;
            }
            if (parameters[index] is not null)
            {
                Console.Error.WriteLine($"Duplicate parameters provided --{arg}.");
                return;
            }
            Type parameterType = parameterInfos[index].ParameterType;
            if (parameterType == typeof(string))
            {
                parameters[index] = args[i + 1];
            }
            else
            {
                MethodInfo? tryParse;
                ConstructorInfo? constuctor;
                if ((tryParse = Meta.GetTryParseMethod(parameterType)) is not null)
                {
                    object[] tryParseParameters = new object[2];
                    tryParseParameters[0] = args[i + 1];
                    object? result = tryParse.Invoke(null, tryParseParameters);
                    if (result is not bool resultBool || !resultBool)
                    {
                        Console.Error.WriteLine($"Could not parse parameter value --{arg} {args[i + 1]}.");
                        return;
                    }
                    parameters[index] = tryParseParameters[1];
                }
                else if ((constuctor = parameterType.GetConstructor(Ɐ(typeof(string)))) is not null)
                {
                    parameters[index] = constuctor.Invoke(Ɐ(args[i + 1]));
                }
                else
                {
                    throw new Exception("syntax error: invalid type used (no tryparse found)");
                }
            }
        }
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i] is null)
            {
                if (!parameterInfos[i].HasDefaultValue)
                {
                    Console.Error.WriteLine($"Missing required parameter --{parameterInfos[i].Name} {parameterInfos[i].ParameterType}.");
                    return;
                }
                parameters[i] = parameterInfos[i].DefaultValue;
            }
        }
        methodInfo.Invoke(null, parameters);
    }

    /// <summary>Gets the default output for the version command.</summary>
    /// <param name="assembly">The application assembly.</param>
    public static void DefaultVersion(Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        AssemblyName assemblyName = assembly.GetName();
        Console.WriteLine($"Assembly: {assemblyName.Name}");
        Console.WriteLine($"Version: {assemblyName.Version}");
    }

    /// <summary>Gets the default output for the help command.</summary>
    /// <param name="assembly">The application assembly.</param>
    /// <param name="command">The command to get the default help output for.</param>
    public static void DefaultHelp(Assembly? assembly = null, string? command = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        if (command is null)
        {
            DefaultVersion(assembly);
            Console.WriteLine("Commands:");
            foreach (MethodInfo methodInfo in assembly.GetMethodInfosWithAttribute<CommandAttribute>())
            {
                Console.WriteLine("  " + methodInfo.Name);
            }
            Console.WriteLine(@"Use ""Help X"" for detailed info per ""X"" command.");
        }
        else
        {
            ListArray<MethodInfo> commandMatches = new();
            foreach (MethodInfo methodInfo in assembly.GetMethodInfosWithAttribute<CommandAttribute>())
            {
                string methodName = methodInfo.Name;
                if (methodName == command)
                {
                    commandMatches.Add(methodInfo);
                }
            }
            if (commandMatches.Count > 1)
            {
                throw new Exception("syntax error: multiple matching commands");
            }
            else if (commandMatches.Count <= 0)
            {
                Console.Error.WriteLine("command not found");
                return;
            }
            else
            {
                MethodInfo methodInfo = commandMatches[0];
                string? documentation = Meta.GetDocumentation(methodInfo);
                if (documentation is null)
                {
                    Console.WriteLine("Parameters:");
                    foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                    {
                        Console.WriteLine("--" + parameterInfo.Name);
                    }
                }
                else
                {
                    Console.WriteLine(documentation);
                }
            }
        }
    }

    /// <summary>Indicates that a method is invocable from the command line arguments.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    { }
}