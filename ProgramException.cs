/* 
Nguyễn Minh Tuấn
202419010 
*/
using System.Text;
public class ProgramException
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("==============================================================");
        Console.WriteLine("       KỊCH BẢN KIỂM THỬ: 11.2. LUỒNG PHÁT SINH NGOẠI LỆ      ");
        Console.WriteLine("==============================================================\n");

        IBorrower lecturer = new Lecturer("GV001", "TS. Nguyễn Minh Tuấn");
        IBorrower assistant = new TeachingAssistant("TA001", "Trợ giảng Lê Văn C");

        // ---------------------------------------------------------------------
        // TÌNH HUỐNG 1: Mượn điều khiển điều hòa đang UnderMaintenance
        // Ngoại lệ kỳ vọng: DeviceUnavailableException
        // ---------------------------------------------------------------------
        Console.WriteLine("--- [Test 1] Mượn thiết bị đang ở trạng thái UnderMaintenance ---");
        try
        {
            Device acRemote = new airConditionerRemote("DH02", "Điều hòa Daikin", 2020, 300000m);
            acRemote.Status = DeviceStatus.UnderMaintenance; // Thiết lập trạng thái bảo trì

            BorrowingSlip slip1 = new BorrowingSlip("PM_ERR_01", lecturer, "P.201", DateTime.Now);
            slip1.AddDevice(acRemote, 2);

            Console.WriteLine("❌ THẤT BẠI: Hệ thống không chặn thiết bị đang bảo trì!");
        }
        catch (DeviceUnavailableException ex)
        {
            Console.WriteLine($"✔ THÀNH CÔNG (Bắt đúng ngoại lệ): {ex.GetType().Name}");
            Console.WriteLine($"   Thông báo: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Bắt sai loại ngoại lệ: {ex.GetType().Name} - {ex.Message}");
        }

        // ---------------------------------------------------------------------
        // TÌNH HUỐNG 2: Trợ giảng mượn vượt quá số lượng cho phép (thiết bị thứ 3)
        // Ngoại lệ kỳ vọng: BorrowLimitExceededException
        // ---------------------------------------------------------------------
        Console.WriteLine("\n--- [Test 2] Trợ giảng mượn thiết bị thứ 3 (Giới hạn tối đa là 2) ---");
        try
        {
            BorrowingSlip slip2 = new BorrowingSlip("PM_ERR_02", assistant, "P.202", DateTime.Now);

            Device dev1 = new Microphone("MIC_TA_01", "Micro 1", 2023, 500000m);
            Device dev2 = new Microphone("MIC_TA_02", "Micro 2", 2023, 500000m);
            Device dev3 = new Microphone("MIC_TA_03", "Micro 3", 2023, 500000m);

            slip2.AddDevice(dev1, 1); // Thiết bị 1: OK
            slip2.AddDevice(dev2, 1); // Thiết bị 2: OK
            slip2.AddDevice(dev3, 1); // Thiết bị 3: Phải văng ngoại lệ!

            Console.WriteLine(" THẤT BẠI: Trợ giảng mượn quá 2 thiết bị mà không bị chặn!");
        }
        catch (BorrowLimitExceededException ex)
        {
            Console.WriteLine($" THÀNH CÔNG (Bắt đúng ngoại lệ): {ex.GetType().Name}");
            Console.WriteLine($"   Thông báo: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Bắt sai loại ngoại lệ: {ex.GetType().Name} - {ex.Message}");
        }

        // ---------------------------------------------------------------------
        // TÌNH HUỐNG 3: Thêm cùng một thiết bị hai lần vào phiếu
        // Ngoại lệ kỳ vọng: DuplicateDeviceException
        // ---------------------------------------------------------------------
        Console.WriteLine("\n--- [Test 3] Thêm cùng một micro hai lần vào cùng một phiếu ---");
        try
        {
            BorrowingSlip slip3 = new BorrowingSlip("PM_ERR_03", lecturer, "P.203", DateTime.Now);
            Device micDup = new Microphone("MIC_DUP", "Micro Shure", 2023, 600000m);

            slip3.AddDevice(micDup, 1); // Thêm lần 1: OK
            slip3.AddDevice(micDup, 1); // Thêm lần 2: Phải văng ngoại lệ!

            Console.WriteLine(" THẤT BẠI: Không phát hiện trùng thiết bị trong phiếu!");
        }
        catch (DuplicateDeviceException ex)
        {
            Console.WriteLine($" THÀNH CÔNG (Bắt đúng ngoại lệ): {ex.GetType().Name}");
            Console.WriteLine($"   Thông báo: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Bắt sai loại ngoại lệ: {ex.GetType().Name} - {ex.Message}");
        
        }
        // ---------------------------------------------------------------------
        // TÌNH HUỐNG 4: Số ngày mượn không dương (<= 0)
        // Ngoại lệ kỳ vọng: InvalidBorrowPeriodException
        // ---------------------------------------------------------------------
        Console.WriteLine("\n--- [Test 4] Mượn với số ngày không hợp lệ (mượn 0 ngày) ---");
        try
        {
            BorrowingSlip slip4 = new BorrowingSlip("PM_ERR_04", lecturer, "P.204", DateTime.Now);
            Device prj = new ProjectorRemote("MC_INVALID", "Remote Epson", 2022, 200000m);

            slip4.AddDevice(prj, 0); // borrowDays = 0: Phải văng ngoại lệ!

            Console.WriteLine(" THẤT BẠI: Cho phép mượn với số ngày bằng 0!");
        }
        catch (InvalidBorrowPeriodException ex)
        {
            Console.WriteLine($"✔ THÀNH CÔNG (Bắt đúng ngoại lệ): {ex.GetType().Name}");
            Console.WriteLine($"   Thông báo: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Bắt sai loại ngoại lệ: {ex.GetType().Name} - {ex.Message}");
        }

        // ---------------------------------------------------------------------
        // TÌNH HUỐNG 5: Trả một thiết bị không thuộc phiếu mượn
        // Ngoại lệ kỳ vọng: DeviceNotInSlipException
        // ---------------------------------------------------------------------
        Console.WriteLine("\n--- [Test 5] Trả thiết bị không tồn tại trong phiếu ---");
        try
        {
            BorrowingSlip slip5 = new BorrowingSlip("PM_ERR_05", lecturer, "P.205", DateTime.Now);
            Device devInSlip = new ProjectorRemote("MC_IN_SLIP", "Remote Sony", 2022, 200000m);
            slip5.AddDevice(devInSlip, 1);

            // Trả thiết bị mã "MC_UNKNOWN" không có trong phiếu
            slip5.ReturnDevice("MC_UNKNOWN", DateTime.Now);

            Console.WriteLine(" THẤT BẠI: Trả được thiết bị không có trong phiếu!");
        }
        catch (DeviceNotInSlipException ex)
        {
            Console.WriteLine($"✔ THÀNH CÔNG (Bắt đúng ngoại lệ): {ex.GetType().Name}");
            Console.WriteLine($"   Thông báo: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Bắt sai loại ngoại lệ: {ex.GetType().Name} - {ex.Message}");
        }

        // ---------------------------------------------------------------------
        // TÌNH HUỐNG 6: Trả cùng một thiết bị hai lần
        // Ngoại lệ kỳ vọng: DeviceAlreadyReturnedException
        // ---------------------------------------------------------------------
        Console.WriteLine("\n--- [Test 6] Trả một thiết bị đã trả trước đó ---");
        try
        {
            BorrowingSlip slip6 = new BorrowingSlip("PM_ERR_06", lecturer, "P.206", DateTime.Now);
            Device devTwice = new Microphone("MIC_TWICE", "Micro Không Dây", 2023, 700000m);
            slip6.AddDevice(devTwice, 1);

            // Trả lần 1: OK
            slip6.ReturnDevice(devTwice.DeviceId, DateTime.Now);

            // Trả lần 2: Phải văng ngoại lệ!
            slip6.ReturnDevice(devTwice.DeviceId, DateTime.Now);

            Console.WriteLine(" THẤT BẠI: Cho phép trả cùng 1 thiết bị hai lần!");
        }
        catch (DeviceAlreadyReturnedException ex)
        {
            Console.WriteLine($"✔ THÀNH CÔNG (Bắt đúng ngoại lệ): {ex.GetType().Name}");
            Console.WriteLine($"   Thông báo: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Bắt sai loại ngoại lệ: {ex.GetType().Name} - {ex.Message}");
        }

        // ---------------------------------------------------------------------
        // TÌNH HUỐNG 7: Trả thiết bị với ngày trả trước ngày mượn
        // Ngoại lệ kỳ vọng: ArgumentException
        // ---------------------------------------------------------------------
        Console.WriteLine("\n--- [Test 7] Trả thiết bị với ngày trả trước ngày mượn ---");
        try
        {
            DateTime borrowDate = new DateTime(2025, 4, 15);
            BorrowingSlip slip7 = new BorrowingSlip("PM_ERR_07", lecturer, "P.207", borrowDate);
            Device devDate = new ProjectorRemote("MC_DATE_ERR", "Remote Panasonic", 2022, 300000m);
            slip7.AddDevice(devDate, 2);

            // Mượn ngày 15/04 nhưng trả ngày 10/04 (vô lý)
            DateTime invalidReturnDate = new DateTime(2025, 4, 10);
            slip7.ReturnDevice(devDate.DeviceId, invalidReturnDate);

            Console.WriteLine(" THẤT BẠI: Chấp nhận ngày trả trước ngày mượn!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"✔ THÀNH CÔNG (Bắt đúng ngoại lệ): {ex.GetType().Name}");
            Console.WriteLine($"   Thông báo: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Bắt sai loại ngoại lệ: {ex.GetType().Name} - {ex.Message}");
        }

        Console.WriteLine("\n==============================================================");
        Console.WriteLine(">>> TẤT CẢ KỊCH BẢN NGOẠI LỆ ĐÃ ĐƯỢC BẢO VỆ VÀ KIỂM THỬ XONG! <<<");
        Console.WriteLine("==============================================================");
    }
}