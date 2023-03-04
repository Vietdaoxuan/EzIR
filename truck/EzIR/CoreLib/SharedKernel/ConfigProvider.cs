using Microsoft.Extensions.Configuration;

namespace CoreLib.SharedKernel
{
    /// <summary>
    /// Lấy config của ứng dụng từ project gọi đến project này.
    /// </summary>
    public static class ConfigProvider
    {
        /// <summary>
        /// Gets or sets config của ứng dụng.
        /// </summary>
        public static IConfiguration Configuration { get; set; }
    }
}
