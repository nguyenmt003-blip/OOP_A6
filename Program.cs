/* 
Nguyễn Minh Tuấn
202419010 
*/
using System;
using System.Text;

public class Program
{
    public static void Main1(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("==============================================================");
        Console.WriteLine("        KỊCH BẢN KIỂM THỬ: 11.1. LUỒNG THÀNH CÔNG             ");
        Console.WriteLine("==============================================================\n");

        try
        {
            // -------------------------------------------------------------
            // Bước 1: Tạo một giảng viên (sử dụng class Lecture của bạn)
            // -------------------------------------------------------------
            IBorrower lecturer = new Lecturer("GV001", "TS. Nguyễn Minh Tuấn");
            Console.WriteLine($"[1] Đã tạo giảng viên: {lecturer.FullName} (Mã: {lecturer.BorrowerId}, Tối đa: {lecturer.GetMaximumDevices()} thiết bị)");

            // -------------------------------------------------------------
            // Bước 2: Tạo một điều khiển máy chiếu, một điều khiển điều hòa và một micro không dây
            // -------------------------------------------------------------
            Device projectorRemote = new ProjectorRemote("MC01", "Điều khiển máy chiếu Sony", 2022, 450000m);
            Device acRemote = new airConditionerRemote("DH01", "Điều khiển điều hòa Daikin", 2021, 300000m);
            Device microphone = new Microphone("MIC01", "Micro không dây Shure", 2023, 1200000m);

            Console.WriteLine("[2] Đã khởi tạo 3 thiết bị:");
            Console.WriteLine($"    - [{projectorRemote.DeviceId}] | Trạng thái: {projectorRemote.Status}");
            Console.WriteLine($"    - [{acRemote.DeviceId}] | Trạng thái: {acRemote.Status}");
            Console.WriteLine($"    - [{microphone.DeviceId}] | Trạng thái: {microphone.Status}");

            // -------------------------------------------------------------
            // Bước 3: Tạo phiếu mượn cho một phòng dạy
            // -------------------------------------------------------------
            DateTime borrowDate = new DateTime(2025, 4, 10);
            BorrowingSlip slip1 = new BorrowingSlip("PM2025-01", lecturer, "Phòng học 402 - Giảng đường A2", borrowDate);
            Console.WriteLine($"\n[3] Đã tạo phiếu mượn: {slip1.SlipId} cho {slip1.TeachingRoom} ngày {slip1.BorrowedDate:dd/MM/yyyy}");

            // -------------------------------------------------------------
            // Bước 4: Thêm điều khiển máy chiếu và micro vào phiếu
            // - Mượn máy chiếu 3 ngày (Hạn trả: 13/04/2025)
            // - Mượn micro 2 ngày (Hạn trả: 12/04/2025)
            // -------------------------------------------------------------
            slip1.AddDevice(projectorRemote, 3);
            slip1.AddDevice(microphone, 2);

            Console.WriteLine("\n[4] Đã thêm thiết bị vào phiếu:");
            foreach (var item in slip1.Items)
            {
                Console.WriteLine($"    + Mượn mã: {item.Device.DeviceId} | Hạn trả: {item.DueDate:dd/MM/yyyy} | Trạng thái thiết bị: {item.Device.Status}");
            }

            // -------------------------------------------------------------
            // Bước 5: Trả điều khiển máy chiếu đúng hạn
            // Hẹn ngày 13/04/2025 -> Trả đúng ngày 13/04/2025
            // -------------------------------------------------------------
            DateTime returnDateProjector = new DateTime(2025, 4, 13);
            decimal feeProjector = slip1.ReturnDevice(projectorRemote.DeviceId, returnDateProjector);
            Console.WriteLine($"\n[5] Trả thiết bị '{projectorRemote.DeviceId}':");
            Console.WriteLine($"    - Ngày trả: {returnDateProjector:dd/MM/yyyy} (Đúng hạn)");
            Console.WriteLine($"    - Tiền phạt: {feeProjector:N0} VNĐ");
            Console.WriteLine($"    - Trạng thái thiết bị sau trả: {projectorRemote.Status}");

            // -------------------------------------------------------------
            // Bước 6: Trả micro muộn 2 ngày
            // Hẹn ngày 12/04/2025 -> Trả ngày 14/04/2025 (Muộn 2 ngày)
            // -------------------------------------------------------------
            DateTime returnDateMic = new DateTime(2025, 4, 14);
            decimal feeMic = slip1.ReturnDevice(microphone.DeviceId, returnDateMic);
            Console.WriteLine($"\n[6] Trả thiết bị '{microphone.DeviceId}':");
            Console.WriteLine($"    - Hạn trả: 12/04/2025 | Ngày trả thực tế: {returnDateMic:dd/MM/yyyy} (Muộn 2 ngày)");
            Console.WriteLine($"    - Tiền phạt trễ hạn: {feeMic:N0} VNĐ");
            Console.WriteLine($"    - Trạng thái thiết bị sau trả: {microphone.Status}");

            // Tạo thêm Phiếu 2 (chưa trả) để phục vụ kiểm thử bước 7
            BorrowingSlip slip2 = new BorrowingSlip("PM2025-02", lecturer, "Phòng học 501 - Nhà B", DateTime.Now);
            slip2.AddDevice(acRemote, 1);

            // Danh sách phiếu mượn
            List<BorrowingSlip> allSlips = new List<BorrowingSlip>();
            allSlips.Add(slip1);
            allSlips.Add(slip2);

            // -------------------------------------------------------------
            // Bước 7: Hiển thị các phiếu trả muộn, chưa trả
            // -------------------------------------------------------------
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("       [7] HIỂN THỊ CÁC PHIẾU TRẢ MUỘN VÀ PHIẾU CHƯA TRẢ      ");
            Console.WriteLine("==============================================================");

            // A. Phiếu có thiết bị trả muộn
            Console.WriteLine("\n--- DANH SÁCH PHIẾU CÓ THIẾT BỊ TRẢ MUỘN ---");
            var lateSlips = allSlips.Where(s => s.Items.Any(i => i.IsReturned && i.ReturnedDate.HasValue && i.ReturnedDate.Value.Date > i.DueDate.Date)).ToList();

            if (lateSlips.Count == 0)
            {
                Console.WriteLine("Không có phiếu nào trả muộn.");
            }
            else
            {
                foreach (var s in lateSlips)
                {
                    Console.WriteLine($"Phiếu: {s.SlipId} | Người mượn: {s.Borrower.FullName} | Tổng phạt: {s.CalculateTotalLateFee():N0} VNĐ");
                    foreach (var it in s.Items.Where(i => i.IsReturned && i.ReturnedDate.HasValue && i.ReturnedDate.Value.Date > i.DueDate.Date))
                    {
                        int lateDays = (it.ReturnedDate.Value.Date - it.DueDate.Date).Days;
                        Console.WriteLine($"   * Mã thiết bị: {it.Device.DeviceId} (Trễ {lateDays} ngày)");
                    }
                }
            }

            // B. Phiếu chưa trả (còn thiết bị chưa hoàn trả)
            Console.WriteLine("\n--- DANH SÁCH PHIẾU CHƯA TRẢ XONG ---");
            var unreturnedSlips = allSlips.Where(s => s.Items.Any(i => !i.IsReturned)).ToList();

            if (unreturnedSlips.Count == 0)
            {
                Console.WriteLine("Tất cả phiếu mượn đều đã được trả đầy đủ.");
            }
            else
            {
                foreach (var s in unreturnedSlips)
                {
                    Console.WriteLine($"Phiếu: {s.SlipId} | Phòng: {s.TeachingRoom} | Người mượn: {s.Borrower.FullName}");
                    foreach (var it in s.Items.Where(i => !i.IsReturned))
                    {
                        Console.WriteLine($"   * Thiết bị đang giữ: {it.Device.DeviceId} | Hạn trả: {it.DueDate:dd/MM/yyyy}");
                    }
                }
            }

            Console.WriteLine("\n>>> KIỂM THỬ LUỒNG THÀNH CÔNG HOÀN TẤT VÀ CHÍNH XÁC! <<<");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LỖI]: {ex.Message}");
        }
    }
}