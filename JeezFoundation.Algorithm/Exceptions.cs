﻿namespace JeezFoundation.Algorithm;

/// <summary>Represents a bug in the JeezFoundation.Algorithm project. Please report it.</summary>
public class TowelBugException : Exception
{
    internal static readonly string reportMessage =
        Environment.NewLine +
        Environment.NewLine +
        "Please submit this issue to the JeezFoundation.Algorithm GitHub repository. " +
        Environment.NewLine +
        "https://github.com/ZacharyPatten/Towel/issues/new/choose";

    /// <summary>Represents a bug in the JeezFoundation.Algorithm project. Please report it.</summary>
    /// <param name="message">The message of the exception.</param>
    public TowelBugException(string message) : base(message + reportMessage) { }

    /// <summary>Represents a bug in the JeezFoundation.Algorithm project. Please report it.</summary>
    /// <param name="message">The message of the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public TowelBugException(string message, Exception innerException) : base(message + reportMessage, innerException) { }
}

/// <summary>Thrown when a data structure operation fails due to external corruption.</summary>
public class CorruptedDataStructureException : Exception
{
    /// <summary>Thrown when a data structure operation fails due to external corruption.</summary>
    public CorruptedDataStructureException() : base() { }

    /// <summary>Thrown when a data structure operation fails due to external corruption.</summary>
    /// <param name="message">The message of the exception.</param>
    public CorruptedDataStructureException(string message) : base(message) { }

    /// <summary>Thrown when a data structure operation fails due to external corruption.</summary>
    /// <param name="message">The message of the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public CorruptedDataStructureException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>Thrown when the inequality syntax is missused.</summary>
public class InequalitySyntaxException : Exception
{
    /// <summary>Thrown when the inequality syntax is missused.</summary>
    public InequalitySyntaxException() : base() { }

    /// <summary>Thrown when the inequality syntax is missused.</summary>
    /// <param name="message">The message of the exception.</param>
    public InequalitySyntaxException(string message) : base(message) { }

    /// <summary>Thrown when the inequality syntax is missused.</summary>
    /// <param name="message">The message of the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public InequalitySyntaxException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>Thrown when an internal documentation method is called.</summary>
public class DocumentationMethodException : Exception
{
    /// <summary>Thrown when an internal documentation method is called.</summary>
    public DocumentationMethodException() : base() { }

    /// <summary>Thrown when an internal documentation method is called.</summary>
    /// <param name="message">The message of the exception.</param>
    public DocumentationMethodException(string message) : base(message) { }

    /// <summary>Thrown when an internal documentation method is called.</summary>
    /// <param name="message">The message of the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public DocumentationMethodException(string message, Exception innerException) : base(message, innerException) { }
}