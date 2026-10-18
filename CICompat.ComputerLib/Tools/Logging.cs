namespace CICompat.ComputerLib.Tools;

internal static class Logging {
    public static void Info(object logContent) => Plugin.Logger.LogInfo(logContent);

    public static void Warning(object logContent) => Plugin.Logger.LogWarning(logContent);

    public static void Error(object logContent) => Plugin.Logger.LogError(logContent);
}