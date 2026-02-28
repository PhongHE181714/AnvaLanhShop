namespace GreenShop.Service
{
    public class MomoSettings
    {
        public string PartnerCode { get; set; } = "";
        public string AccessKey { get; set; } = "";
        public string SecretKey { get; set; } = "";
        public string ReturnUrl { get; set; } = "";
        public string NotifyUrl { get; set; } = "";
        public string RequestType { get; set; } = "captureWallet";
        public string Endpoint { get; set; } = "https://test-payment.momo.vn/v2/gateway/api/create";
    }
}