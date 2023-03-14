using System;
using System.Windows;

namespace ImageChecker.Models
{
    public class PositionConverter
    {
        public static Point GetPositionBasedOnCenter(int x, int y, int width, int height, double scale)
        {
            if (Math.Abs(scale - 1.0) < 0.01)
            {
                // 等倍ならば値の修正は必要ないため入力値を加工せずに返す。
                return new Point(x, y);
            }

            var centerX = width * scale / 2;
            var centerY = height * scale / 2;

            var fixX = ((width * scale) - width) / 2;
            var fixY = ((height * scale) - height) / 2;

            return new Point(x + fixX, y + fixY);
        }
    }
}