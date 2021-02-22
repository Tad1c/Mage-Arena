using System.Collections;
using UnityEngine;

/**
 * Temporary solution to toggle logs on and off
 * We could use some asset instead of writing own log system
 */
public static class MyLog
{
    public static bool isLoggingEnabled = true;

    public static void D(string message) {
        if(isLoggingEnabled) Debug.Log(message);
    }

    public static void E(string message)
    {
        if (isLoggingEnabled) Debug.LogError(message);
    }

}
