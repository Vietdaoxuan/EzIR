namespace CoreLib.Entities
{
    /// <summary>
    /// Kết quả trả về khi thực thi sp hoặc http request.
    /// </summary>
    public class CResponseMessage
    {
        /// <summary>
        /// Gets or sets mã lỗi.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets nội dung.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets dữ liệu đi kèm.
        /// </summary>
        public object Data { get; set; }


        public CResponseMessage(int code, string message)
        {
            this.Code = code;
            this.Message = message;

        }

        public CResponseMessage()
        {
        }
    }
}
