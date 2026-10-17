/* 
Nguyễn Minh Tuấn
202419010 
*/
public abstract class Device
{
    public string DeviceId { get; }
    public string DeviceName { get; private set; }
    public int YearOfUsage { get; set; }
    public decimal PurchasePrice { get; }
    public DeviceStatus Status { get; set; }

//constructor
    protected Device(string deviceId, string deviceName, int yearOfUsage, decimal purchasePrice, DeviceStatus status = DeviceStatus.Active)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("Mã thiết bị không được để trống.");
        
        if (string.IsNullOrWhiteSpace(deviceName))
            throw new ArgumentException("Tên thiết bị không được để trống.");

        if (purchasePrice <= 0)
            throw new ArgumentException("Giá mua phải lớn hơn 0.");
            
        if (yearOfUsage > DateTime.Now.Year)
            throw new ArgumentException("Năm đưa vào sử dụng không được lớn hơn năm hiện tại.");
        DeviceId = deviceId;
        DeviceName = deviceName;
        YearOfUsage = yearOfUsage;
        PurchasePrice = purchasePrice;
        Status = status;
    }

    //method
    public abstract decimal CalculateAnnualMaintenanceCost();
    public void MarkAsBorrowed()
    {
        if (Status != DeviceStatus.Available)
                throw new InvalidOperationException($"Thiết bị {DeviceId} không ở trạng thái sẵn sàng để mượn.");
            Status = DeviceStatus.Borrowed;
    }
    public void MarkAsAvailable() 
    {
        Status = DeviceStatus.Available;
    }
    public override string ToString() {return $"Device ID: {DeviceId}, Name: {DeviceName}";}
}
public enum DeviceStatus
{
    Active,
    UnderMaintenance,
    OutOfService,
    Available,
    Borrowed
}

///
/// Các lớp con của Devide
/// 

public class Microphone: Device
{
    public Microphone(string deviceId, string deviceName, int yearOfUsage, decimal purchasePrice, DeviceStatus status = DeviceStatus.Active)
        : base(deviceId, deviceName, yearOfUsage, purchasePrice, status)
    {
    }
    public override decimal CalculateAnnualMaintenanceCost()
        {
            // đề k có 
            return 0; 
        }
}
public class ProjectorRemote: Device
{
    public ProjectorRemote(string deviceId, string deviceName, int yearOfUsage, decimal purchasePrice, DeviceStatus status = DeviceStatus.Active)
        : base(deviceId, deviceName, yearOfUsage, purchasePrice, status)
    {
    }
    public override decimal CalculateAnnualMaintenanceCost()
    {
        //đề k có
        return 0; 
    }
}
public class airConditionerRemote: Device
{
    public airConditionerRemote(string deviceId, string deviceName, int yearOfUsage, decimal purchasePrice, DeviceStatus status = DeviceStatus.Active)
        : base(deviceId, deviceName, yearOfUsage, purchasePrice, status)
    {
    }
    public override decimal CalculateAnnualMaintenanceCost()
    {
        //đề k có
        return 0; 
    }
}
