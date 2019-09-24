using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FAN.Nvelocity
{
    /// <summary>
    /// 将URL路径Hash一个唯一的字符串
    /// </summary>
    public static class UrlHasher
    {

        /// <summary>
        /// Controls how many subfolders to use for disk caching. Rounded to the next power of to. (1->2, 3->4, 5->8, 9->16, 17->32, 33->64, 65->128,129->256,etc.)
        /// NTFS does not handle more than 8,000 files per folder well. Larger folders also make cleanup more resource-intensive.
        /// Defaults to 32, which combined with the default setting of 400 images per folder, allows for scalability to 12,800 actively used image versions. 
        /// For example, given a desired cache size of 100,000 items, this should be set to 256.
        /// </summary>
        private const int SUB_FOLDER_COUNT = 64;

        /// <summary>
        /// Builds a key for the cached version, using the hashcode of the normalized URL.
        /// if subfolders > 1, dirSeparator will be used to separate the subfolder and the key. 
        /// No extension is appended.
        /// I.e, a13514\124211ab132592 or 12412ababc12141
        /// </summary>
        /// <param name="pathAndQuery"></param>
        /// <param name="phyFilePath"></param>
        /// <param name="dirSeparator"></param>
        /// <returns></returns>
        public static string Hash(Uri uri, string phyFilePath, string dirSeparator)
        {
            string result = null;
            using (SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                string pathAndQuery = uri.PathAndQuery;//wangyunpeng，区分URL大小写。2016-7-20。
                if (File.Exists(phyFilePath))
                {
                    DateTime sourceModifiedUtcTick = File.GetLastWriteTimeUtc(phyFilePath);
                    if (sourceModifiedUtcTick != DateTime.MinValue)
                    {
                        pathAndQuery += string.Concat("|", sourceModifiedUtcTick.Ticks.ToString(NumberFormatInfo.InvariantInfo));
                    }
                }

                byte[] hashs = sha256.ComputeHash(new System.Text.UTF8Encoding().GetBytes(pathAndQuery));
                //If configured, place files in subfolders.
                string subFolderName = GetSubfolder(hashs, SUB_FOLDER_COUNT) + dirSeparator;

                //Can't use base64 hash... filesystem has case-insensitive lookup.
                //Would use base32, but too much code to bloat the resizer. Simple base16 encoding is fine
                result = subFolderName + Base16Encode(hashs);
                Array.Clear(hashs, 0, hashs.Length);
                hashs = null;
            }
            return result ?? string.Empty;
        }
        /// <summary>
        /// Returns a string for the subfolder name. The bits used are from the end of the hash - this should make
        /// the hashes in each directory more unique, and speed up performance (8.3 filename calculations are slow when lots of files share the same first 6 chars.
        /// Returns null if not configured. Rounds subfolders up to the nearest power of two.
        /// </summary>
        /// <param name="hashs"></param>
        /// <param name="subFolderCount"></param>
        /// <returns></returns>
        private static string GetSubfolder(byte[] hashs, int subFolderCount)
        {
            int bits = (int)Math.Ceiling(Math.Log(subFolderCount, 2)); //Log2 to find the number of bits. round up.
            Debug.Assert(bits > 0);
            Debug.Assert(bits <= hashs.Length * 8);

            byte[] subFolders = new byte[(int)Math.Ceiling((double)bits / 8.0)]; //Round up to bytes.
            Array.Copy(hashs, hashs.Length - subFolders.Length, subFolders, 0, subFolders.Length);
            subFolders[0] = (byte)((int)subFolders[0] >> ((subFolders.Length * 8) - bits)); //Set extra bits to 0.
            string result = Base16Encode(subFolders);
            Array.Clear(subFolders, 0, subFolders.Length);
            subFolders = null;
            return result;
        }

        private static string Base16Encode(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x", NumberFormatInfo.InvariantInfo).PadLeft(2, '0'));
            }
            string result = sb.ToString();
            sb.Clear();
            sb = null;
            return result;
        }
    }
}
