using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/**/
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using System.Drawing.Imaging;

namespace CySoft.Utility
{
   public  class QRCode
   {
       /// <summary>
       /// 生成二维码
       /// </summary>
       /// <param name="destImg">中间图标</param>
       /// <param name="data">数据</param>
       /// <param name="filefilepath">保存路径</param>
       public static void CreatQRCode(string destImg, string data, string filepath,int qrcodeScale=10)
       {
           #region 生成二维码
           QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
           qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
           qrCodeEncoder.QRCodeScale = qrcodeScale;//大小
           qrCodeEncoder.QRCodeVersion = 8;
           qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
         
           Image imgBack = qrCodeEncoder.Encode(data);
           if (!string.IsNullOrEmpty(destImg))
           {
               try
               {
                   Image img = Image.FromFile(destImg);

                   if (img.Height != 80 || img.Width != 80)
                   {
                       img = QRCode.KiResizeImage(img, 80, 80, 0);
                   }
                   Graphics g = Graphics.FromImage(imgBack);
                   g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
                   g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
               }
               catch(Exception ex)
               {
               }
           }
           

           //保存
           FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
           imgBack.Save(fs, ImageFormat.Jpeg);
           fs.Close();
           imgBack.Dispose();
           #endregion
       }

        /// <summary>  
        /// 处理图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="Mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public static Image KiResizeImage(System.Drawing.Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                System.Drawing.Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }

       
    }
}
