/* 
Nguyễn Minh Tuấn
202419010 
*/
using System;

//Ngoại lệ cơ sở
public class BorrowingException : Exception
{
    public BorrowingException() : base() { }

    public BorrowingException(string message) 
        : base(message) 
    { 
    }

    public BorrowingException(string message, Exception innerException) 
        : base(message, innerException) 
    { 
    }
}

//  Các ngoại lệ dẫn xuất bắt buộc

//khi thiết bị không ở trạng thái Available
public class DeviceUnavailableException : BorrowingException
{
    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }

    public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus)
        : base($"Thiết bị '{deviceId}' không sẵn sàng để mượn. Trạng thái hiện tại: {currentStatus}.")
    {
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }

    public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus, string message)
        : base(message)
    {
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }
}

//  khi người mượn đã đạt số thiết bị tối đa
public class BorrowLimitExceededException : BorrowingException
{
    public string BorrowerId { get; }
    public int MaximumDevices { get; }

    public BorrowLimitExceededException(string borrowerId, int maximumDevices)
        : base($"Người mượn '{borrowerId}' đã vượt quá số lượng thiết bị tối đa cho phép ({maximumDevices} thiết bị).")
    {
        BorrowerId = borrowerId;
        MaximumDevices = maximumDevices;
    }

    public BorrowLimitExceededException(string borrowerId, int maximumDevices, string message)
        : base(message)
    {
        BorrowerId = borrowerId;
        MaximumDevices = maximumDevices;
    }
}

// khi số ngày mượn không dương hoặc vượt quá số ngày cho phép của thiết bị
public class InvalidBorrowPeriodException : BorrowingException
{
    public string DeviceId { get; }
    public int RequestedDays { get; }
    public int MaxAllowedDays { get; }

    public InvalidBorrowPeriodException(string deviceId, int requestedDays, int maxAllowedDays)
        : base($"Thời gian mượn {requestedDays} ngày cho thiết bị '{deviceId}' không hợp lệ (Số ngày cho phép tối đa: {maxAllowedDays}).")
    {
        DeviceId = deviceId;
        RequestedDays = requestedDays;
        MaxAllowedDays = maxAllowedDays;
    }

    public InvalidBorrowPeriodException(string message) 
        : base(message)
    {
        DeviceId = string.Empty;
        RequestedDays = 0;
        MaxAllowedDays = 0;
    }
}

// khi cùng một thiết bị đã tồn tại trong phiếu
public class DuplicateDeviceException : BorrowingException
{
    public string DeviceId { get; }

    public DuplicateDeviceException(string deviceId)
        : base($"Thiết bị '{deviceId}' đã tồn tại trong phiếu mượn này.")
    {
        DeviceId = deviceId;
    }

    public DuplicateDeviceException(string deviceId, string message)
        : base(message)
    {
        DeviceId = deviceId;
    }
}

// khi yêu cầu trả một thiết bị không thuộc phiếu
public class DeviceNotInSlipException : BorrowingException
{
    public string DeviceId { get; }

    public DeviceNotInSlipException(string deviceId)
        : base($"Thiết bị '{deviceId}' không thuộc phiếu mượn này.")
    {
        DeviceId = deviceId;
    }

    public DeviceNotInSlipException(string deviceId, string message)
        : base(message)
    {
        DeviceId = deviceId;
    }
}

// Phát sinh khi thiết bị trong phiếu đã được trả trước đó
public class DeviceAlreadyReturnedException : BorrowingException
{
    public string DeviceId { get; }

    public DeviceAlreadyReturnedException(string deviceId)
        : base($"Thiết bị '{deviceId}' trong phiếu mượn này đã được trả trước đó.")
    {
        DeviceId = deviceId;
    }

    public DeviceAlreadyReturnedException(string deviceId, string message)
        : base(message)
    {
        DeviceId = deviceId;
    }
}