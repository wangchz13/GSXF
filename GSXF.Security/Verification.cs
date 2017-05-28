using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace GSXF.Security
{
    public class Verification
    {
        public static string Text(int Length = 4)
        {
            char[] ver = new Char[Length];
            Random random = new Random();
            char[] dict = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for(int i = 0; i < Length; i++)
            {
                ver[i] = dict[random.Next(dict.Length - 1)];
            }
            return new string(ver);
        }

        public static Bitmap Image(string verificationText)
        {
            int width = 100,
                height = 20;
            SizeF textSize;
            Bitmap bitmap = new Bitmap(HttpContext.Current.Server.MapPath("~/Content/img/Texture.jpg"), true);
            TextureBrush brush = new TextureBrush(bitmap);
            Font _font = new Font("Arial", 14, FontStyle.Bold);
            Bitmap image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);
            //清空背景色
            g.Clear(Color.White);
            //绘制验证码
            textSize = g.MeasureString(verificationText, _font);
            g.DrawString(verificationText, _font, brush, (width - textSize.Width) / 2, (height - textSize.Height) / 2);
            return image;
        }
    }
}
