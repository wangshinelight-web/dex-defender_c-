using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DexDefender
{
    public partial class Form1 : Form
    {
        public string fileNameOnly = "BlueGlucose.apk";
        public string strPath = "";
        public string outputDir = "decompile/BlueGlucose.apk";

        public string key1 = "        <meta-data android:name=\"RealApplication\" android:value=\"" + "{0}\"/>\n";
        public string key2 = "        <meta-data android:name=\"ProtectKey\" android:value=\"" + "{0}\"/> \n         <activity android:name=\"com.khc124.apkprotector.activities.CopyClipActivity\"/>";



        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void DecompileApk()
        {
            if (strPath == null || strPath == "") return;

            fileNameOnly = Path.GetFileName(strPath);
            label_Path.Text = strPath.Substring(0, 20) + "..." + strPath.Substring(strPath.Length - 20);

            var runner = new ApktoolRedirector(listBox_Print);


            /* runner.OutputReceived += msg => listBox_Print.Items.Add($"[INFO] {msg}");
             runner.ErrorReceived += msg => listBox_Print.Items.Add($"[ERROR] {msg}");*/

            //int exitCode = runner.Execute(strPath, "decompile/" + fileNameOnly + "/");
            outputDir = "decompile/" + fileNameOnly + "/";

            string cmd = $"-jar tools/apktool.jar d -f --no-src \"{strPath}\" -o \"{outputDir}\"";

            int exitCode = runner.Execute(cmd);

            if (exitCode == 0)
            {
                listBox_Print.Items.Add($" APKTool反编译执行完成。");
                button1.Enabled = true;
            }
             
            

        }

        private void btn_open_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult res = dlg.ShowDialog();
            if(res == DialogResult.OK)
            {
                listBox_Print.Items.Clear();
                strPath = dlg.FileName;
                label_Path.Text = strPath;

                DirectoryHelper.SafeDeleteDirectory(strPath, 3);

                if (strPath.LastIndexOf(".apk") > 0)
                {
                    Thread thread = new Thread(DecompileApk);
                    thread.Start(); 
                }
                else
                {
                    MessageBox.Show("It is not APK");
                    return;
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory("decompile");
            Directory.CreateDirectory("compile");

        }


        public static sbyte[] JavaToCSharp(byte[] javaBytes)
        {
            return javaBytes.Select(b => (sbyte)b).ToArray();
        }

        public string GetAppNameFromManifest()
        {
            string[] lines = File.ReadAllLines(outputDir + "/AndroidManifest.xml");
            if(lines != null)
            {
                string package = "";

                foreach (string line in lines)
                {
                    string idx = "android:name=";
                    int pos = 0;

                   

                    if (line.IndexOf("manifest") >= 0)
                    {
                        if ((pos = line.IndexOf("package=")) >= 0)
                        {
                            string tmp1 = line.Substring(pos);
                            string[] tmps = tmp1.Split('"');
                            package = tmps[1];
                        }

                    }

                    if ((pos = line.IndexOf(idx)) >= 0 && line.IndexOf("application") >= 0)
                    {
                        string appName = line.Substring(pos + idx.Length);
                        string[] lns = appName.Split(' ');
                        appName = lns[0];
                        appName = appName.Replace('"', ' ');
                        appName = appName.Replace('>', ' ');
                        appName = appName.Trim();
                        if (appName[0] == '.')
                            appName = package + appName;
                        return appName;
                    }
                }
               
            }
            return "";
        }

        private string MakeAppXml(string idx, string idx2, string line)
        {
            string appProp = "";
            string cmd = "";

            string[] props = line.Split(' ');
            foreach (string prop in props)
            {

                if (prop.IndexOf(idx) >= 0)
                {
                    appProp += "android:name=\"com.khc124.apkprotector.ProxyApplication\"";
                    appProp += " ";
                }
                else if (prop.IndexOf(idx2) >= 0)
                {
                    appProp += "android:extractNativeLibs=\"false\"";
                    appProp += " ";
                }
                else
                {
                    appProp += prop;
                    appProp += " ";
                }

            }
            cmd = appProp;
            return cmd;
        }

        public void MakeManifesModified(string sKey1, string sKey2)
        {
            string xmlPath = outputDir + "/AndroidManifest.xml";
            string sreKey1 = String.Format(key1, sKey1);
            string sreKey2 = String.Format(key2, sKey2);            

            string tag = "</application>";
          
            string[] lines = File.ReadAllLines(xmlPath);


            using (StreamWriter sw = new StreamWriter(xmlPath))
            {                  
                
                if (lines != null)
                {
                    string cmd = "";
                    foreach (string line in lines)
                    {
                        string appProp = "";
                        string idx = "android:name=";
                        string idx2 = "android:extractNativeLibs=";
                        int pos = 0;
                        cmd = line;
                        
                        if (line.IndexOf("application") >= 0)
                        {

                            if((pos = line.IndexOf(idx)) >= 0 )
                            {

                                cmd = MakeAppXml(idx, idx2, line);
                                if (cmd.IndexOf(">") < 0)
                                    cmd += ">";

                            }
                            else
                            {
                                if (line.IndexOf(tag) < 0)
                                {
                                    cmd = line.Replace('>', ' ');
                                    cmd += "android:name=\"com.khc124.apkprotector.ProxyApplication\">";
                                    cmd = MakeAppXml(idx, idx2, cmd);
                                    if (cmd.IndexOf(">") < 0)
                                        cmd += ">";                                    
                                }
                                
                            }

                            if (line.IndexOf(tag) > 0)
                            {
                                if (sKey1 != "" && sKey1.Length > 0)
                                    sw.WriteLine(sreKey1);
                                sw.WriteLine(sreKey2);
                            }
                            sw.WriteLine(cmd);

                        }
                        else { sw.WriteLine(cmd); }

                    }
                    

                    sw.Close();
                    sw.Dispose();

                    Thread.Sleep(500);
                   

                }
            }
            listBox_Print.Items.Add($" APKTool Dex 加固执行完成。");
            return;

        }

        public void CopyDirectory(string sourceDir, string destinationDir)
        {
            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException("源目录不存在");

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(destinationDir, Path.GetFileName(file)));

            foreach (var dir in Directory.GetDirectories(sourceDir))
                CopyDirectory(dir, Path.Combine(destinationDir, Path.GetFileName(dir)));
        }

        private void MovePackLibs()
        {
            string[] dirs = Directory.GetDirectories(outputDir);

            foreach(string dir in dirs)
            {
                if(dir.IndexOf("lib") >= 0)
                {
                    if (Directory.Exists(outputDir + "/lib/armeabi-v7a"))
                    {
                        File.Copy("tools/lib/armeabi-v7a/libapkprotector.so", outputDir + "/lib/armeabi-v7a/libapkprotector.so");
                    }
                    if (Directory.Exists(outputDir + "/lib/arm64-v8a"))
                    {
                        File.Copy("tools/lib/arm64-v8a/libapkprotector.so", outputDir + "/lib/arm64-v8a/libapkprotector.so");
                    }

                }
                else
                {
                    CopyDirectory("tools/lib", outputDir + "/lib");
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string strAZ = "ABCDEF1234567890";

            string strAppKey = "";
            Random rand = new Random();

            for (int k = 0; k < 30; k++)
            {                
                int randomNumber = rand.Next(1, 15); // 最大值是31（不包含）
                strAppKey += strAZ[randomNumber];
            }


            string strKey2 = strAppKey;

            string tmp = GetAppNameFromManifest();
            string strKey1 = Cryptor.StringEncoder(tmp, 2);

            // get key from random string
            strKey2 = Cryptor.StringEncoder(strKey2, 2);

            // make AndroidManifest.xml modified.

            MakeManifesModified(strKey1, strKey2);

            // from directory get classes*.dex, all classes are encrypted.

            MakeAssetsModified(strKey2);


            Thread.Sleep(500);
            File.Copy("tools/dexloader.dex", outputDir + "classes.dex");


            MovePackLibs();


            btn_compile_apk.Enabled = true;

        }

        private void MakeAssetsModified(string strKey2)
        {
            string[] dexFiles = Directory.GetFiles(outputDir, "*.dex");

            string assetsDir = outputDir + "/assets";
            if(!Directory.Exists(assetsDir))
                assetsDir = outputDir + "/unknown/assets";

            Directory.CreateDirectory(assetsDir);

            string dexOutDir = assetsDir + "/dex/";
            Directory.CreateDirectory(dexOutDir);

            for (int i = 1; i < dexFiles.Length + 1; i++)
            {
                string strPath = "";
                if (i == 1)
                    strPath = outputDir + "/classes.dex";
                else
                    strPath = outputDir + "/classes" + String.Format("{0}" + ".dex", i);


                if (strPath.IndexOf("dexloader") > 0) continue;

                ZlibDecompressor.DeflateFile(strPath, strPath + "_enc");

                FileStream reader = new FileStream(strPath + "_enc", FileMode.Open);
                string strDexFile = dexOutDir + String.Format("classes-v{0}.bin", i);
                FileStream writer = new FileStream(strDexFile, FileMode.Append);

                Cryptor.Encoder(strKey2, reader, writer);
                reader.Close();
                reader.Dispose();
                writer.Close();
                writer.Dispose();
                Thread.Sleep(100);

            }

            string[] files = Directory.GetFiles(outputDir, "*.dex");
            foreach (string file in files)
            {
                File.Delete(file);
                File.Delete(file + "_enc");
            }
        }

        public void CompileApk()
        {
            var runner = new ApktoolRedirector(listBox_Print);
            string strApkPath = "compile/" + fileNameOnly;
            string cmd = $"-jar tools/apktool.jar b -f -o \"{strApkPath}\"  \"{outputDir}\"";

            int exitCode = runner.Execute(cmd);
            if (exitCode == 0)
            {
                listBox_Print.Items.Add($" APKTool 编译执行完成。");
                Thread.Sleep(500);
                SignApk();
                MessageBox.Show("All are completed !!!");
                Application.Exit();
            }

        }

        public void SignApk()
        {
            string rootDir = AppDomain.CurrentDomain.BaseDirectory;

            var runner = new ApktoolRedirector(listBox_Print);
            
            string cmd = $" -p -f -v 4 {rootDir}compile/{fileNameOnly}  {rootDir}compile/{fileNameOnly}_";
            string path = $"{rootDir}tools\\";

            int exitCode = runner.ExecuteW(path, cmd);
            if (exitCode == 0)
                listBox_Print.Items.Add($" APKTool 对齐执行完成。");


            // java -jar apksigner.jar sign --v3-signing-enabled true --ks keystore.jks --out signed.apk aligned.apk

            cmd = $"-jar  {rootDir}tools\\apksigner.jar  sign  --ks {rootDir}tools\\111.jks --ks-pass pass:111111 --v2-signing-enabled true --v3-signing-enabled true --out {rootDir}compile/{fileNameOnly}  {rootDir}compile/{fileNameOnly}_";

            //string cmd = $"-jar  {rootDir}tools\\apksigner.jar  sign --key  {rootDir}tools\\apkeasytool.pk8 --cert {rootDir}tools\\apkeasytool.pem --out {rootDir}compile/{fileNameOnly}  {rootDir}compile/{fileNameOnly}";


            exitCode = runner.Execute(cmd);
            if (exitCode == 0)
                listBox_Print.Items.Add($" APKTool 签名执行完成。");

            File.Delete($"{rootDir}compile/{fileNameOnly}_");
        }


        private void btn_compile_apk_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(CompileApk);
            thread.Start();


        }


        private void button2_Click(object sender, EventArgs e)
        {

            string kkey1 = "恐恜恞思恝恇思恑恟恆恖恇恜恜恇恛思恲恃恃";

            string kkey2 = "怀怂怀怆怀怊怀怇怀怆怀怀怀恲怀怄怀怀怀怃怀怊怀怂怀怂怀怂怀恰";


            string strKey2 = "3135393435333A373330393131313C";
            string strKey1 = Cryptor.StringEncoder(strKey2, 2);



            string encDexPath = "G:/KHC_WORK/Decoder/decompile/door_lock/assets/dex";


            string[] files = Directory.GetFiles(encDexPath);

            foreach(string file in files)
            {
                 FileStream reader = new FileStream(file, FileMode.Open);
                 FileStream writer = new FileStream(file+".tmp", FileMode.Append);

                 Cryptor.Encoder(kkey2, reader, writer);

                 reader.Close();
                 writer.Close();
                 writer.Dispose();
                 reader.Dispose();
            }

             files = Directory.GetFiles(encDexPath, "*.tmp");

            foreach (string file in files)
            {
  
                ZlibDecompressor.InflateFile(file, file + ".dex");
               
            }



        }
    }
}
