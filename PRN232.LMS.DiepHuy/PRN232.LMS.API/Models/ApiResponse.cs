using System.Xml.Serialization;

namespace PRN232.LMS.API.Models
{
    /// <summary>
    /// Generic API response wrapper for all endpoints
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        
        [XmlArray("Errors")]
        [XmlArrayItem("Error")]
        public List<string> Errors { get; set; }

        // Bắt buộc phải có Constructor không tham số phục vụ XML
        public ApiResponse() 
        { 
            Errors = new List<string>();
        }

        public ApiResponse(bool success, string message, T data = default, List<string> errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        public static ApiResponse<T> CreateSuccess(T data, string message = "Request processed successfully")
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> CreateFailure(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>(false, message, default, errors);
        }
    }

    /// <summary>
    /// Pagination metadata for list responses
    /// </summary>
    public class PaginatedResponse<T>
    {
        [XmlArray("Data")]
        [XmlArrayItem("Item")]
        public List<T> Data { get; set; } = new List<T>();
        
        public PaginationMetadata Pagination { get; set; }

        // Bắt buộc phải có Constructor không tham số phục vụ XML
        public PaginatedResponse() { }
    }

    /// <summary>
    /// Pagination metadata
    /// </summary>
    public class PaginationMetadata
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        // Bắt buộc phải có Constructor không tham số phục vụ XML
        public PaginationMetadata() { }
    }
}