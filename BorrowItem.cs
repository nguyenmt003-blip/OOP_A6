/* 
Nguyễn Minh Tuấn
202419010 
*/
public class BorrowItem
{
    // Thuộc tính tham chiếu thiết bị (hỗ trợ cả Device và TeachingDevice theo đặc tả)
    public Device Device { get; }
    public Device TeachingDevice => Device;
    public DateTime DueDate {get; private set;}
    public DateTime BorrowedDate { get; }
    public int FromPeriod {get; set;}
    public int ToPeriod {get; set;}
    public DateTime? ReturnedDate {get; private set;}
    public bool IsReturned {get;}

    //constructor 

    public BorrowItem(Device device, DateTime borrowedDate, DateTime dueDate, int fromPeriod = 1, int toPeriod = 1)
{
    // 1. Device không được null
    if (device == null)
        throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");

    // 2. DueDate phải sau hoặc bằng BorrowedDate
    if (dueDate.Date < borrowedDate.Date)
        throw new ArgumentException("Hạn trả (DueDate) phải sau hoặc bằng ngày mượn (BorrowedDate).");

    // 3. Quy tắc tiết mượn: toPeriod phải >= fromPeriod và tiết mượn phải dương
    if (fromPeriod <= 0)
        throw new ArgumentOutOfRangeException(nameof(fromPeriod), "Tiết bắt đầu mượn phải lớn hơn 0.");
    if (toPeriod < fromPeriod)
        throw new ArgumentException("Tiết kết thúc (toPeriod) phải lớn hơn hoặc bằng tiết bắt đầu (fromPeriod).");

        Device = device;
        BorrowedDate = borrowedDate;
        DueDate = dueDate;
        FromPeriod = fromPeriod;
        ToPeriod = toPeriod;

        // 4. ReturnedDate ban đầu là null
        ReturnedDate = null;
    }


    /// Constructor tiện ích khi mượn theo ngày
  
    public BorrowItem(Device device, DateTime borrowedDate, DateTime dueDate)
        : this(device, borrowedDate, dueDate, 1, 1)
    {
    }


    /// Constructor tiện ích khi mượn theo tiết trong ngày

    public BorrowItem(Device device, DateTime borrowedDate, int fromPeriod, int toPeriod)
        : this(device, borrowedDate, borrowedDate, fromPeriod, toPeriod)
    {
    }


    /// Ghi nhận trả thiết bị

    public void Return(DateTime returnedDate)
    {
        // Không được trả một BorrowItem hai lần
        if (IsReturned)
            throw new InvalidOperationException($"Thiết bị '{Device.DeviceId}' trong phiếu này đã được trả trước đó.");

        // Ngày trả không được trước ngày mượn
        if (returnedDate.Date < BorrowedDate.Date)
            throw new ArgumentException("Ngày trả không được trước ngày mượn.");

        ReturnedDate = returnedDate;
    }
}