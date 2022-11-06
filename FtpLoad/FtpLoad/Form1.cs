using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace FtpLoad
{
    public partial class Form1 : Form
    {

        string _fullPath = "";
        string _safeFileName = "";

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
    }
}