namespace CleanArchitecture.Net7.WebApi.Features.Common
{
    public abstract class BaseResponse : BaseMessage
    {
        public BaseResponse(Guid correlationId) : base()
        {
            base._correlationId = correlationId;
        }

        public BaseResponse()
        {
        }

        public bool IsError { get; set; } = false;
        public string Message { get; set; }
    }
}
