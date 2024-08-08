namespace FireBaseIntegration.DTO
{
    public record Destination
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
        public string lineNo { get; set; }
        public string location { get; set; }
        public string New_Quantity { get; set; }
        public string User { get; set; }
        public DateTime Update_Date { get; set; }
        public DateTime Update_Time { get; set; }
        public bool Sent_to_SQL { get; set; }
    }
}
