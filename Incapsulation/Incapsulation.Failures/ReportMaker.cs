namespace Incapsulation.Failures;

public enum FailureType
{
    UnexpectedShutdown,
    ShortNonResponding,
    HardwareFailures,
    ConnectionProblems
}

public class Failure
{
    public readonly DateTime Date;
    public readonly int DeviceId;
    private readonly FailureType type;
    public bool IsSerious => type is FailureType.UnexpectedShutdown or FailureType.HardwareFailures;

    public Failure(FailureType type, DateTime date, int deviceId)
    {
        this.type = type;
        Date = date;
        DeviceId = deviceId;
    }
}

public class Device
{
    public readonly int Id;
    public readonly string Name;

    public Device(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class ReportMaker
{
    /// <summary>
    /// </summary>
    /// <param name="day"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <param name="failureTypes">
    /// 0 for unexpected shutdown, 
    /// 1 for short non-responding, 
    /// 2 for hardware failures, 
    /// 3 for connection problems
    /// </param>
    /// <param name="deviceId"></param>
    /// <param name="times"></param>
    /// <param name="devices"></param>
    /// <returns></returns>
    public static List<string> FindDevicesFailedBeforeDateObsolete(
        int day,
        int month,
        int year,
        int[] failureTypes,
        int[] deviceId,
        object[][] times,
        List<Dictionary<string, object>> devices)
    {
        var currentDate = new DateTime(year, month, day);
        var dates = TimesToDateTimes(times);
        var deviceArray = GetDevices(devices);

        var failures = failureTypes
            .Select((type, i) => new Failure((FailureType)type, dates[i], deviceId[i]))
            .ToArray();

        return FindDevicesFailedBeforeDate(
            currentDate,
            failures,
            deviceArray
        );
    }

    private static DateTime[] TimesToDateTimes(object[][] times)
        => times
            .Select(time => new DateTime(
                (int)time[2],
                (int)time[1],
                (int)time[0]
            ))
            .ToArray();

    private static Device[] GetDevices(List<Dictionary<string, object>> devices)
        => devices
            .Select(device => new Device(
                (int)device["DeviceId"],
                (string)device["Name"]
            ))
            .ToArray();

    private static List<string> FindDevicesFailedBeforeDate(DateTime currentDate, Failure[] failures,
        Device[] devices)
    {
        if (failures.Length < 1) return new List<string>();
        return failures
            .Where(failure => failure.Date < currentDate && failure.IsSerious)
            .Select(failure => devices.First(device => device.Id == failure.DeviceId).Name)
            .ToList();
    }
}