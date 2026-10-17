/* 
Nguyễn Minh Tuấn
202419010 
*/
public class BorrowingSlip
{
    // Đơn giá phạt trễ hạn mặc định (20.000 VNĐ / ngày trễ)
    public const decimal DailyLateFee = 20000m;

    public string SlipId { get; }
    public IBorrower Borrower { get; }
    public string TeachingRoom { get; }
    public DateTime BorrowedDate { get; }

    private readonly List<BorrowItem> _items = new();
    public IReadOnlyList<BorrowItem> Items => _items.AsReadOnly();

    public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
    {
        if (string.IsNullOrWhiteSpace(slipId))
            throw new ArgumentException("Mã phiếu mượn (SlipId) không được để trống.", nameof(slipId));

        if (borrower == null)
            throw new ArgumentNullException(nameof(borrower), "Người mượn (Borrower) không được null.");

        if (string.IsNullOrWhiteSpace(teachingRoom))
            throw new ArgumentException("Phòng dạy (TeachingRoom) không được để trống.", nameof(teachingRoom));

        SlipId = slipId;
        Borrower = borrower;
        TeachingRoom = teachingRoom;
        BorrowedDate = borrowedDate;
    }

    /// <summary>
    /// Kiểm tra các quy tắc nghiệp vụ trước khi thêm thiết bị vào phiếu
    /// </summary>
    private void ValidateBorrowing(Device device, int borrowDays)
    {
        // 1. Thiết bị không được null
        if (device == null)
            throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");

        // 2. Số ngày mượn phải lớn hơn 0
        if (borrowDays <= 0)
            throw new InvalidBorrowPeriodException("Số ngày mượn phải lớn hơn 0.");

        // 3. Người mượn chưa đạt giới hạn thiết bị tối đa
        if (_items.Count >= Borrower.GetMaximumDevices())
            throw new BorrowLimitExceededException(Borrower.BorrowerId, Borrower.GetMaximumDevices());

        // 4. Thiết bị chưa tồn tại trong phiếu
        if (_items.Any(i => i.Device.DeviceId.Equals(device.DeviceId, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicateDeviceException(device.DeviceId);

        // 5. Thiết bị phải ở trạng thái Available
        if (device.Status != DeviceStatus.Available)
            throw new DeviceUnavailableException(device.DeviceId, device.Status);
    }

    /// <summary>
    /// Thêm thiết bị vào phiếu mượn đảm bảo tính nhất quán (Atomicity)
    /// </summary>
    public void AddDevice(Device device, int borrowDays)
    {
        ValidateBorrowing(device, borrowDays);

        var item = new BorrowItem(device, BorrowedDate, BorrowedDate.AddDays(borrowDays));
        device.MarkAsBorrowed();
        _items.Add(item);
    }

    /// <summary>
    /// Trả thiết bị và tính phí trễ hạn (nếu có)
    /// </summary>
    public decimal ReturnDevice(string deviceId, DateTime returnedDate)
    {
        // 1. Kiểm tra deviceId
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("Mã thiết bị không được để trống.", nameof(deviceId));

        // 2. Tìm BorrowItem theo mã thiết bị
        var item = _items.FirstOrDefault(i => i.Device.DeviceId.Equals(deviceId, StringComparison.OrdinalIgnoreCase));

        // 3. Nếu không tìm thấy, phát sinh DeviceNotInSlipException
        if (item == null)
            throw new DeviceNotInSlipException(deviceId);

        // 4. Nếu item đã được trả, phát sinh DeviceAlreadyReturnedException
        if (item.IsReturned)
            throw new DeviceAlreadyReturnedException(deviceId);

        // 5 & 6. Kiểm tra ngày trả và ghi nhận ngày trả
        item.Return(returnedDate);

        // 7. Chuyển thiết bị về Available
        item.Device.MarkAsAvailable();

        // Tính và trả về phí phạt trễ hạn của thiết bị này
        decimal lateFee = 0;
        if (returnedDate.Date > item.DueDate.Date)
        {
            int lateDays = (returnedDate.Date - item.DueDate.Date).Days;
            lateFee = lateDays * DailyLateFee;
        }

        return lateFee;
    }

    /// <summary>
    /// Tính tổng phí phạt trễ hạn của cả phiếu
    /// </summary>
    public decimal CalculateTotalLateFee()
    {
        decimal totalFee = 0;
        foreach (var item in _items)
        {
            DateTime effectiveDate = item.IsReturned ? item.ReturnedDate.Value : DateTime.Now;
            if (effectiveDate.Date > item.DueDate.Date)
            {
                int lateDays = (effectiveDate.Date - item.DueDate.Date).Days;
                totalFee += lateDays * DailyLateFee;
            }
        }
        return totalFee;
    }
}

// Alias hỗ trợ nếu project gọi tên class là BorrowSlip
public class BorrowSlip : BorrowingSlip
{
    public BorrowSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
        : base(slipId, borrower, teachingRoom, borrowedDate)
    {
    }
}