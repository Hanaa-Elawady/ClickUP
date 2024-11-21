﻿namespace ClickUp.Service.HandleResponse
{
    public class ValidationErrorResponse :CustomException
	{
		public ValidationErrorResponse() : base(400)
		{
		}
		public IEnumerable<string> Errors { get; set; }
	}
}
