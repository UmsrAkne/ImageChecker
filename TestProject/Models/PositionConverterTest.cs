using System.Windows;
using ImageChecker.Models;
using NUnit.Framework;

namespace TestProject.Models
{
    public class PositionConverterTest
    {
        [Test]
        public void テスト()
        {

            // 拡大なし、画像が真ん中に写っている状態を想定
            Assert.IsTrue(
                Point.Equals(
                    new Point(0, 0),
                    PositionConverter.GetPositionBasedOnCenter(0, 0, 1280, 720, 1.0))
            );

            // 倍率 1.5倍 画像の左上部分が写っている状態
            Assert.AreEqual(320, (int)PositionConverter.GetPositionBasedOnCenter(0, 0, 1280, 720, 1.5).X);
            Assert.AreEqual(180, (int)PositionConverter.GetPositionBasedOnCenter(0, 0, 1280, 720, 1.5).Y);

            // 倍率 1.5倍 画像が画面真ん中に写っている状態
            Assert.AreEqual(0, (int)PositionConverter.GetPositionBasedOnCenter(-320, -180, 1280, 720, 1.5).X);
            Assert.AreEqual(0, (int)PositionConverter.GetPositionBasedOnCenter(-320, -180, 1280, 720, 1.5).Y);

            // 倍率 1.5倍 画像の右下部分が写っている状態
            Assert.AreEqual(-320, (int)PositionConverter.GetPositionBasedOnCenter(-640, -360, 1280, 720, 1.5).X);
            Assert.AreEqual(-180, (int)PositionConverter.GetPositionBasedOnCenter(-640, -360, 1280, 720, 1.5).Y);

            // 倍率 2.0倍 画像の左上部分が写っている状態
            Assert.AreEqual(640, (int)PositionConverter.GetPositionBasedOnCenter(0, 0, 1280, 720, 2.0).X);
            Assert.AreEqual(360, (int)PositionConverter.GetPositionBasedOnCenter(0, 0, 1280, 720, 2.0).Y);

            // 倍率 2.0倍 画像が画面真ん中に写っている状態
            Assert.AreEqual(0, (int)PositionConverter.GetPositionBasedOnCenter(-640, -360, 1280, 720, 2.0).X);
            Assert.AreEqual(0, (int)PositionConverter.GetPositionBasedOnCenter(-640, -360, 1280, 720, 2.0).Y);

            // 倍率 2.0倍 画像の右下部分が写っている状態
            Assert.AreEqual(-640, (int)PositionConverter.GetPositionBasedOnCenter(-1280, -720, 1280, 720, 2.0).X);
            Assert.AreEqual(-360, (int)PositionConverter.GetPositionBasedOnCenter(-1280, -720, 1280, 720, 2.0).Y);
        }
    }
}