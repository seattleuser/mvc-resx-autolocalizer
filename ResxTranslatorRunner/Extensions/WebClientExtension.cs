using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace ResxTranslatorRunner.Extensions
{
    /// <summary>
    /// Webclient helper extension
    /// Download response in chunks
    /// </summary>
    public static class WebClientExtension
    {

        /// <summary>
        /// DownloadStringUsingResponseEncoding
        /// </summary>
        /// <param name="client"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string DownloadStringUsingResponseEncoding(this WebClient client, string address)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            return DownloadStringUsingResponseEncoding(client, client.DownloadData(address));
        }

        /// <summary>
        /// DownloadStringUsingResponseEncoding
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DownloadStringUsingResponseEncoding(WebClient client, byte[] data)
        {
            Debug.Assert(client != null);
            Debug.Assert(data != null);

            var contentType = client.GetResponseContentType();

            var encoding = contentType == null || string.IsNullOrEmpty(contentType.CharSet)
                   ? client.Encoding
                   : Encoding.GetEncoding(contentType.CharSet);

            return encoding.GetString(data);
        }

        /// <summary>
        /// GetResponseContentType
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static ContentType GetResponseContentType(this WebClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            var headers = client.ResponseHeaders;
            if (headers == null)
            {
                throw new InvalidOperationException("Response headers not available.");
            }

            var header = headers["Content-Type"];

            return !string.IsNullOrEmpty(header)
               ? new ContentType(header)
               : null;
        }


        private static IEnumerable<string> SplitIntoChunks(string text, int chunkSize)
        {
            int offset = 0;
            if (offset > text.Length)
            {
                yield return text;
                yield break;
            }

            while (offset < text.Length)
            {
                int size = Math.Min(chunkSize, text.Length - offset);
                yield return text.Substring(offset, size);
                offset += size;
            }
        }
    }
}
