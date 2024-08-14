namespace FireBaseIntegration.DTO
{
    public class SQLPayload
    {
        public bool status { get; set; }
        public string sqlissue { get; set; }
    }
    public class ArgPayload
    {
        public List<SourceSetPayload> srclst { get; set; }
    }
}
