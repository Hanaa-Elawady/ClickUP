namespace ClickUp.Service.HandleResponse
{
    public class CustomException :Response
	{
		public CustomException(int _statusCode, string? _details = null, string? _message = null) : base(_statusCode, _message)
		{
			Details = _details;
		}
		public string? Details { get; set; }
	}
}
