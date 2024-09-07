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
        this.Date = date;
        this.DeviceId = deviceId;
    }
}

public class Device
{
    public readonly int Id;
    public readonly string Name;

    public Device(int id, string name)
    {
        this.Id = id;
        this.Name = name;
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
    {
        var dates = new DateTime[times.Length];
        for (var i = 0; i < times.Length; i++)
        {
            dates[i] = new DateTime(
                (int)times[i][2],
                (int)times[i][1],
                (int)times[i][0]
            );
        }

        return dates;
    }

    private static Device[] GetDevices(List<Dictionary<string, object>> devices)
    {
        var deviceArray = new Device[devices.Count];
        for (var i = 0; i < devices.Count; i++)
        {
            deviceArray[i] = new Device(
                (int)devices[i]["DeviceId"],
                (string)devices[i]["Name"]
            );
        }

        return deviceArray;
    }

    private static List<string> FindDevicesFailedBeforeDate(DateTime currentDate, Failure[] failures,
        Device[] devices)
    {
        if (failures.Length < 1) return new List<string>();
        return (from failure in failures
                where failure.Date < currentDate && failure.IsSerious
                select devices
                    .First(device => device.Id == failure.DeviceId).Name)
            .ToList();
    }
}