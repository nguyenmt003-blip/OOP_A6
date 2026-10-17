/* 
Nguyễn Minh Tuấn
202419010 
*/
public class TeachingAssistant : IBorrower
{
    public string BorrowerId { get; }
    public string FullName { get; }

    //constructor 
    public TeachingAssistant(string borrowerId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(borrowerId))
            throw new ArgumentException("Mã trợ giảng không được để trống.");
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Tên trợ giảng không được để trống.");

        BorrowerId = borrowerId;
        FullName = fullName;
    }

    //method 
    public int GetMaximumDevices()
    {
        return 2; // Trợ giảng có thể mượn tối đa 2 thiết bị
    }
}   