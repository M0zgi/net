using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Image = System.Drawing.Image;

namespace FtpLoad
{
    public partial class Form1 : Form
    {

        string _fullPath = "";
        string _safeFileName = "";
        string _onlyFilePath = "";

        private int _hight;
        private int _widh;

        public Form1()
        {
            InitializeComponent();
            tb_password.UseSystemPasswordChar = true;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем объект FtpWebRequest - он указывает на файл, который будет создан
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + tb_srvUrl.Text + ":21");
                request.Credentials = new NetworkCredential(tb_login.Text, tb_password.Text);
                // устанавливаем метод на загрузку файлов
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string res = reader.ReadToEnd();

                reader.Close();

                MessageBox.Show(res);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_sendFile_Click(object sender, EventArgs e)
        {
            LoadImage();
            ConvertWebp();

            try
            {
                string fullPath = _fullPath;
                string fileName = _safeFileName;

                // Создаем объект FtpWebRequest - он указывает на файл, который будет создан
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + tb_srvUrl.Text + ":21" + "/uploads/" + fileName);
                request.Credentials = new NetworkCredential(tb_login.Text, tb_password.Text);

                // устанавливаем метод на загрузку файлов

                request.Method = WebRequestMethods.Ftp.UploadFile;

                // создаем поток для загрузки файла
                FileStream fs = new FileStream(fullPath, FileMode.Open);
                byte[] fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, fileContents.Length);
                fs.Close();
                request.ContentLength = fileContents.Length;

                // пишем считанный в массив байтов файл в выходной поток
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                // получаем ответ от сервера в виде объекта FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();


                response.Close();

                if (Path.GetExtension("http://127.0.0.1/uploads/" + fileName) == ".webp")
                {
                    using (WebP webp = new WebP())
                        pictureBoxAvatar.Image = webp.Load("http://127.0.0.1/uploads/" + fileName);
                }
                else
                {

                    pictureBoxAvatar.Image = Image.FromFile("http://127.0.0.1/uploads/" + fileName);
                }
                pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom; 

                //pictureBoxAvatar.Load("http://127.0.0.1/uploads/" + fileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void LoadImage()
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image files (*.webp, *.png, *.tif, *.tiff, *.jpg)|*.webp;*.png;*.tif;*.tiff;*.jpg";
                    openFileDialog.FileName = "";
                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        
                        string pathFileName = openFileDialog.FileName;
                        string s = DateTime.Now.ToString("yyyyMMddhhmmss");

                        //_onlyFilePath =
                        //    openFileDialog.FileName.Remove(
                        //        openFileDialog.FileName.IndexOf(openFileDialog.SafeFileName));

                        //_safeFileName = Path.GetFileName(openFileDialog.FileName);
                       

                        //resizeImage(600, 600, pathFileName);

                        _safeFileName = "Avatar_" + s + ".webp";

                        if (Path.GetExtension(pathFileName) == ".webp")
                        {
                            using (WebP webp = new WebP())
                                pictureBoxAvatar.Image = webp.Load(pathFileName);
                        }
                        else
                        {
                            pictureBoxAvatar.Image = Image.FromFile(pathFileName);
                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nIn WebPExample.buttonLoad_Click", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConvertWebp()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            byte[] rawWebP;
            
            try
            {
                if (this.pictureBoxAvatar.Image == null)
                    MessageBox.Show("Please, load an image first");

                //get the picture box image
                Bitmap bmp = (Bitmap)pictureBoxAvatar.Image;

                //Test simple encode in lossly mode in memory with quality 75
                string lossyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _safeFileName + ".webp");
                _fullPath = lossyFileName;
                using (WebP webp = new WebP())
                    rawWebP = webp.EncodeLossy(bmp, 75);


                File.WriteAllBytes(lossyFileName, rawWebP);
                MessageBox.Show("Made " + lossyFileName, "Simple lossy");

                ////Test simple encode in lossless mode in memory
                //string simpleLosslessFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SimpleLossless.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeLossless(bmp);
                //File.WriteAllBytes(simpleLosslessFileName, rawWebP);
                //MessageBox.Show("Made " + simpleLosslessFileName, "Simple lossless");

                ////Test encode in lossly mode in memory with quality 75 and speed 9
                //string advanceLossyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AdvanceLossy.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeLossy(bmp, 71, 9, true);
                //File.WriteAllBytes(advanceLossyFileName, rawWebP);
                //MessageBox.Show("Made " + advanceLossyFileName, "Advance lossy");

                ////Test advance encode lossless mode in memory with speed 9
                //string losslessFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AdvanceLossless.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeLossless(bmp, 9);
                //File.WriteAllBytes(losslessFileName, rawWebP);
                //MessageBox.Show("Made " + losslessFileName, "Advance lossless");

                //Test encode near lossless mode in memory with quality 40 and speed 9
                // quality 100: No-loss (bit-stream same as -lossless).
                // quality 80: Very very high PSNR (around 54dB) and gets an additional 5-10% size reduction over WebP-lossless image.
                // quality 60: Very high PSNR (around 48dB) and gets an additional 20%-25% size reduction over WebP-lossless image.
                // quality 40: High PSNR (around 42dB) and gets an additional 30-35% size reduction over WebP-lossless image.
                // quality 20 (and below): Moderate PSNR (around 36dB) and gets an additional 40-50% size reduction over WebP-lossless image.
                //string nearLosslessFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NearLossless.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeNearLossless(bmp, 40, 9);
                //File.WriteAllBytes(nearLosslessFileName, rawWebP);
                //MessageBox.Show("Made " + nearLosslessFileName, "Near lossless");

                MessageBox.Show("End of Test");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nIn WebPExample.buttonSave_Click", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Info_Click(object sender, EventArgs e)
        {
            int width;
            int height;
            bool has_alpha;
            bool has_animation;
            string format;

            try
            {
                byte[] rawWebp = new System.Net.WebClient().DownloadData("http://127.0.0.1/uploads/" + _safeFileName);

               
                using (WebP webp = new WebP())
                    webp.GetInfo(rawWebp, out width, out height, out has_alpha, out has_animation, out format);
                MessageBox.Show("Width: " + width + "\n" +
                                "Height: " + height + "\n" +
                                "Has alpha: " + has_alpha + "\n" +
                                "Is animation: " + has_animation + "\n" +
                                "Format: " + format, "Information");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nIn WebPExample.buttonInfo_Click", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            //string _safeFileName = "";
            //string _onlyFilePath = "";
            //string lossyFileName = Environment.CurrentDirectory + "\\Temp\\" + _safeFileName;
            //System.IO.File.Copy(stPhotoPath, lossyFileName, true);
            
            Image imgPhoto = Image.FromFile(stPhotoPath); 

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                                                (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                                                (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);
            bmPhoto.Save(Environment.CurrentDirectory + "\\Temp\\" + _safeFileName);

            grPhoto.Dispose();
            imgPhoto.Dispose();

            // return bmPhoto;
        }
    }
}