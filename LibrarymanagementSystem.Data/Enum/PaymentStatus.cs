
public enum PaymentStatus
{
    Pending = 1,     // في انتظار تأكيد من بوابة الدفع
    Completed = 2,   // تم الدفع بنجاح
    Failed = 3,      // فشل الدفع
    Cancelled = 4,   // تم إلغاء العملية
    Refunded = 5     // تم استرجاع المبلغ
}
