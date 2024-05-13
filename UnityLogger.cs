using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public static class UnityLogger
{
    static StringBuilder builder = new(150);
    static StackTrace stackTrace = new StackTrace();
    static StackFrame stackFrame = new StackFrame();
    static System.Type extractedType;

    public static System.Enum NonSpecifiedEnum = null;
    public static Dictionary<System.Type, System.Enum> TypeToLogCategory = new();
    public static Dictionary<string, System.Enum> StringComparisonToCategory = new();


    public static void LogWithTag(System.Enum category = null, string log = "", UnityEngine.Object obj = null, bool autoAddTag = true)
    {
        builder.Clear();

        System.Enum foundCategory = category;

        if (autoAddTag && (category == null || category == NonSpecifiedEnum))
        {
            stackFrame = new StackFrame(1);
            extractedType = stackFrame.GetMethod().DeclaringType;
            foundCategory = GetLogCategory(extractedType);
        }

        builder.AppendFormat("{0} : {1}", foundCategory, log);

#if !RELEASE_MODE
        if (obj != null)
            UnityEngine.Debug.Log(builder, obj);
        else
            UnityEngine.Debug.Log(builder);
#endif
    }

    public static void LogWarningWithTag(System.Enum category = null, string log = "", UnityEngine.Object obj = null, bool autoAddTag = true)
    {
        builder.Clear();

        System.Enum foundCategory = category;

        if (autoAddTag && (category == null || category == NonSpecifiedEnum))
        {
            stackFrame = new StackFrame(1);
            extractedType = stackFrame.GetMethod().DeclaringType;
            foundCategory = GetLogCategory(extractedType);
        }

        builder.AppendFormat("{0} : {1}", foundCategory, log);

#if !RELEASE_MODE
        if (obj != null)
            UnityEngine.Debug.LogWarning(builder, obj);
        else
            UnityEngine.Debug.LogWarning(builder);
#endif
    }

    public static void LogErrorWithTag(System.Enum category = null, string log = "", UnityEngine.Object obj = null, bool autoAddTag = true)
    {
        builder.Clear();

        System.Enum foundCategory = category;

        if (autoAddTag && (category == null || category == NonSpecifiedEnum))
        {
            stackFrame = new StackFrame(1);
            extractedType = stackFrame.GetMethod().DeclaringType;
            foundCategory = GetLogCategory(extractedType);
        }

        builder.AppendFormat("{0} : {1}", foundCategory, log);

        if (obj != null)
            UnityEngine.Debug.LogError(builder, obj);
        else
            UnityEngine.Debug.LogError(builder);
    }


    public static void SetNonSpecifiedEnum(System.Enum value) => NonSpecifiedEnum = value;

    public static void SetStringComparisonToEnum(Dictionary<string, System.Enum> dictionary)
    {
        StringComparisonToCategory = dictionary;
    }

    public static void AddStringComparisonToEnum(string comparison, System.Enum value)
    {
        StringComparisonToCategory.TryAdd(comparison, value);
    }

    [RuntimeInitializeOnLoadMethod]
    static void ClearDictionaries()
    {
        TypeToLogCategory.Clear();
    }

    static System.Enum GetLogCategory(System.Type type)
    {
        if (TypeToLogCategory.TryGetValue(type, out System.Enum logCategory))
            return logCategory;

        string typeString = type.ToString();

        foreach (var stringToCategory in StringComparisonToCategory)
        {
            if (!typeString.Contains(stringToCategory.Key, System.StringComparison.OrdinalIgnoreCase))
                continue;

            TypeToLogCategory.Add(type, stringToCategory.Value);

            return stringToCategory.Value;
        }

        TypeToLogCategory.Add(type, NonSpecifiedEnum);

        return NonSpecifiedEnum;
    }
}

