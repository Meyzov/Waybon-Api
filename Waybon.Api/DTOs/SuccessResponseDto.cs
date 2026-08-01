namespace Waybon.Api.DTOs
{
    public class SuccessResponseDto
    {
        private bool _success;
        public bool Success
        {
            get
            {
                return _success;
            }

            set
            {
                _success = value;
            }
        }
    }
}
