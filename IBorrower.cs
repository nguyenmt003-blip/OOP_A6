/* 
Nguyễn Minh Tuấn
202419010 
*/
public interface IBorrower
{
    public string BorrowerId { get; }
    public string FullName { get; }
    public int GetMaximumDevices(); 
}  