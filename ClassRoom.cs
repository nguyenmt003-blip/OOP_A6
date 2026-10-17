/* 
Nguyễn Minh Tuấn
202419010 
*/
public class LabRoom
{
    public string RoomId { get; }
    public string RoomName { get; private set; }
    public int Capacity { get; private set; }
    private readonly List<Device> _devices = new();

    //constructor
    public LabRoom(string roomId, string roomName, int capacity)
    {
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ArgumentException("Mã phòng không được để trống.");
        RoomId = roomId;
        RoomName = roomName;
        Capacity = capacity;
    }

    //method
    public IReadOnlyList<Device> Devices => _devices.AsReadOnly();
    public void AddDevice(Device device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device), "Không thể thêm thiết bị null.");

        if (_devices.Any(d => d.DeviceId.Equals(device.DeviceId, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Thiết bị với mã '{device.DeviceId}' đã tồn tại trong phòng.");

        _devices.Add(device);
    }
    public bool RemoveDevice(string deviceId)
    {
        var device = FindDevice(deviceId);
        return device != null && _devices.Remove(device);
    }
    public Device? FindDevice(string deviceId)
    {
        var device = _devices.FirstOrDefault(d => d.DeviceId.Equals(deviceId, StringComparison.OrdinalIgnoreCase));
        return device;
    }
    public decimal CalculateAnnualMaintenanceCost()
    {
        return _devices.Sum(d => d.CalculateAnnualMaintenanceCost());
    }
    public List<Device> GetDevicesRequiringMaintenance()
    {
        int currentYear = DateTime.Now.Year;
        return _devices.Where(d => d.Status == DeviceStatus.UnderMaintenance || (currentYear - d.YearOfUsage) > 5)
                        .ToList();
    }
}
