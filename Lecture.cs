/* 
Nguyễn Minh Tuấn
202419010 
*/
public class Lecture : IBorrower
{
    public string BorrowerId { get; }
    public string FullName { get; }

    //constructor 
    public Lecture(string borrowerId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(borrowerId))
            throw new ArgumentException("Mã giảng viên không được để trống.");
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Tên giảng viên không được để trống.");

        BorrowerId = borrowerId;
        FullName = fullName;
    }

    //method 
    public int GetMaximumDevices()
    {
        return 3; // Giảng viên có thể mượn tối đa 3 thiết bị
    }
}