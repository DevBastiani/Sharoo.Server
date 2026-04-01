namespace Sharoo.Server.Application.DTOs.Todos.Filter
{
    public class TodoFilterRequest
    {
        /// <summary>
        /// Filter todos created on or after this date (UTC). Format: yyyy-MM-dd or ISO 8601.
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Filter todos created on or before this date (UTC). Format: yyyy-MM-dd or ISO 8601.
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Filter todos completed on or after this date (UTC). Format: yyyy-MM-dd or ISO 8601.
        /// Only applies to todos where IsDone is true.
        /// </summary>
        public DateTime? CompletedFrom { get; set; }

        /// <summary>
        /// Filter todos completed on or before this date (UTC). Format: yyyy-MM-dd or ISO 8601.
        /// Only applies to todos where IsDone is true.
        /// </summary>
        public DateTime? CompletedTo { get; set; }
    }
}
