using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GstPlayground
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Borrowed from: https://stackoverflow.com/questions/6499334/best-way-to-change-dictionary-key/6499344 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="fromKey"></param>
        /// <param name="toKey"></param>
        public static void ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            if (dic.ContainsKey(toKey))
            {
                dic[toKey] = value;
            }
            else
            {
                dic.Add(toKey, value);
            }
        }

        /// <summary>
        /// Overload for SerializableStringDictionary.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="fromKey"></param>
        /// <param name="toKey"></param>
        public static void ChangeKey(this SerializableStringDictionary dic, string fromKey, string toKey)
        {
            dic.ChangeKey(fromKey, toKey);
        }


        /// <summary>
        /// Borrowed from: https://stackoverflow.com/a/1483963 
        /// </summary>
        static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public static Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = NativeMethods.BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        /// <summary>
        /// Returns true if Color c is greener than another color. 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="thanThis"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool IsGreener(this Color c, Color thanThis, int threshold = 2)
        {
            //bool ret = false;

            int diff = c.G - thanThis.G;
            if (diff > threshold)
                return true;

            if (c.R < (thanThis.R - threshold) && c.B < (thanThis.B - threshold))
            {
                return true;
            }

            return false;
        }
    }
}
