namespace PRN232.LMS.Repositories.Models
{
    /// <summary>
    /// Query parameters for list endpoints
    /// Supports search, sort, paging, field selection, and expansion
    /// </summary>
    public class QueryParameters
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int Page { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        /// <summary>
        /// Search keyword for filtering
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Comma-separated sort fields. Prefix with '-' for descending order
        /// Example: "fullName,-dateOfBirth"
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// Comma-separated field names to include in response
        /// Example: "studentId,fullName,email"
        /// </summary>
        public string? Fields { get; set; }

        /// <summary>
        /// Comma-separated related entities to include
        /// Example: "student,course"
        /// </summary>
        public string? Expand { get; set; }

        public List<string> GetExpandFields()
        {
            if (string.IsNullOrEmpty(Expand))
                return new List<string>();

            return Expand.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }

        public List<string> GetSelectFields()
        {
            if (string.IsNullOrEmpty(Fields))
                return new List<string>();

            return Fields.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }

        public Dictionary<string, bool> GetSortFields()
        {
            var result = new Dictionary<string, bool>();

            if (string.IsNullOrEmpty(Sort))
                return result;

            var sortFields = Sort.Split(',');
            foreach (var field in sortFields)
            {
                var trimmed = field.Trim();
                if (trimmed.StartsWith("-"))
                {
                    result[trimmed.Substring(1)] = false; // descending
                }
                else
                {
                    result[trimmed] = true; // ascending
                }
            }

            return result;
        }
    }
}
