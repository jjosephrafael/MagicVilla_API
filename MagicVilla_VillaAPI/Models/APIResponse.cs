using System.Net;

namespace MagicVilla_VillaAPI.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        // Return the type
        public HttpStatusCode StatusCode { get; set; }
        // Return if Success or not
        public bool IsSuccess { get; set; } = true;
        // Return Error Message
        public List<string> ErrorMessages { get; set; }
        // Return result
        public object Result { get; set; }
    }
}
