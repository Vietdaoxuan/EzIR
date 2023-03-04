using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Commons
{
    public static class CheckUtils
    {
        /// <summary>
        /// Kiểm tra ký tự tiếng việt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ContainsUnicodeCharacter(string input)
        {
            bool ok = false;
            foreach (char c in input)
            {
                if ((c > 47 && c < 58))
                    ok = true;
                else if ((c > 64 && c < 91))
                    ok = true;
                else if ((c > 96 && c < 123))
                {
                    ok = true;
                }
                else if (c == 45 || c == 46 || c == 95)
                {
                    ok = true;
                }
                else
                {
                    ok = false;
                    break;
                }
            }
            return ok;
        }

        /// <summary>
        /// Get size file (Megabyte)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static double GetSizeFile(this IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return 0;
                }

                if (file.Length < 1)
                {
                    return 0;
                }

                var size = (double)(file.Length / 1024) / 1024; // (MB)

                return size;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string CheckValidFileImage(this IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return "ImageIsInvalid";
                }

                if (file.Length < 1)
                {
                    return "ImageIsInvalid";
                }

                var fileExtention = file.FileName.Substring(file.FileName.Length - 4, 4).ToLower();

                string[] validImageFomat =
                {
                    ".png", ".jpg", ".bmp", "gif", "jpeg",
                };

                if (!validImageFomat.Contains(fileExtention))
                {
                    return "ImageIsInvalid";
                }
            }
            catch (Exception)
            {
                return "ImageIsInvalid";
            }

            return string.Empty;
        }

        /// <summary>
        /// Lưu file vào path tùy chọn
        /// </summary>
        /// <param name="file"></param>
        /// <param name="destinationFolder"></param>
        /// <param name="fileName">tùy chọn tên thay thế tên file gốc</param>
        /// <returns></returns>
        public static async Task<string> GetSavedFilePath(this IFormFile file, string destinationFolder, string fileName = "")
        {
            string resultPath = null;
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = file.FileName;
                }

                var filePath = Path.GetFullPath(destinationFolder + "/" + fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                resultPath = destinationFolder + "/" + fileName;
                return resultPath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string CheckValidFilePdf(this IFormFile file)
        {
            var result = string.Empty;
            try
            {
                if (file == null)
                {
                    return result;
                }

                if (file.Length < 1)
                {
                    return result;
                }

                var fileExtention = file.FileName.Substring(file.FileName.Length - 4, 4).ToLower();
                string[] validImageFomat =
                {
                    ".pdf",
                };
                if (!validImageFomat.Contains(fileExtention))
                {
                    result = "Không phải định dạng file pdf ";
                    return result;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// CheckValidFileExcel: Check định dạng fle excel (.xlsx)
        /// </summary>
        /// <param name="file">File input đầu vào</param>
        /// <returns></returns>
        public static string CheckValidFileExcel(this IFormFile file)
        {
            var result = string.Empty;

            try
            {
                if (file == null)
                {
                    return result;
                }

                if (file.Length < 1)
                {
                    return result;
                }

                var fileExtention = file.FileName.Substring(file.FileName.Length - 5, 5).ToLower();
                string[] validImageFomat =
                {
                    ".xlsx",
                };
                if (!validImageFomat.Contains(fileExtention))
                {
                    result = "Không phải định dạng file Excel .xlsx ";
                    return result;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static async Task<string> ReadAsStringAsync(this IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    result.AppendLine(await reader.ReadToEndAsync());
                }
            }

            return result.ToString();
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }


}
