﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.IO.Compression;
using System.Threading;
using SimpleJSON;
using System.Web.Script.Serialization;
using GameLauncherUpdater.App;

namespace GameLauncherUpdater
{
    public partial class Updater : Form {
        string tempNameZip = Path.GetTempFileName();
        string version;

        public Updater() {
            InitializeComponent();
        }

        public void error(string error) {
            Information.Text = error.ToString();
            Delay.WaitSeconds(2);
            Process.GetProcessById(Process.GetCurrentProcess().Id).Kill();
        }

        public void success(string success) {
            Information.Text = success.ToString();
        }

        public void DoUpdate() {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length == 2) {
                Process.GetProcessById(Convert.ToInt32(args[1])).Kill();
            }

            if (File.Exists("GameLauncher.exe")) {
                var versionInfo = FileVersionInfo.GetVersionInfo("GameLauncher.exe");
                version = versionInfo.ProductVersion;
            } else {
                version = "0.0.0.0";
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new WebClient();
            Uri StringToUri = new Uri("https://api.worldunited.gg/update.php?version=" + version);
            client.Headers.Add("user-agent", "GameLauncherUpdater " + Application.ProductVersion + " (+https://github.com/SoapBoxRaceWorld/GameLauncher_NFSW)");
            client.CancelAsync();
            client.DownloadStringAsync(StringToUri);
            client.DownloadStringCompleted += (sender2, e2) => {
                try
                {
					JSONNode json = JSON.Parse(e2.Result);

                    if (json["payload"]["update_exists"] != false)
                    {
                        Thread thread = new Thread(() => {
                            WebClient client2 = new WebClient();
                            client2.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                            client2.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
                            client2.DownloadFileAsync(new Uri(json["payload"]["update"]["download_url"]), tempNameZip);
						});
                        thread.Start();
                    }
                    else
                    {
                        Process.Start(@"GameLauncher.exe");
                        error("Starting GameLauncher.exe");
                    }
                }
                catch 
                {
                    Information.Text = "Failed to Connect to Main API --> Connecting to GitHub API";
                    letsTellToGithubThatWeWannaUseGithubUpdateWayNow();
                }
            };
        }

        public void letsTellToGithubThatWeWannaUseGithubUpdateWayNow() {
            var client3 = new WebClient();
            Uri StringToUri2 = new Uri("https://api.github.com/repos/SoapboxRaceWorld/GameLauncher_NFSW/releases/latest");
            client3.Headers.Add("user-agent", "GameLauncherUpdater " + Application.ProductVersion + " (+https://github.com/SoapBoxRaceWorld/GameLauncher_NFSW)");
            client3.CancelAsync();
            client3.DownloadStringAsync(StringToUri2);
            client3.DownloadStringCompleted += (sender3, e3) => {
                try
                {
                    ReleaseModel json = new JavaScriptSerializer().Deserialize<ReleaseModel>(e3.Result);

                    if (version != json.tag_name)
                    {
                        Thread thread = new Thread(() => {
                            WebClient client4 = new WebClient();
                            client4.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                            client4.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
                            client4.DownloadFileAsync(new Uri("http://github.com/SoapboxRaceWorld/GameLauncher_NFSW/releases/download/" + json.tag_name + "/Release_" + json.tag_name + ".zip"), tempNameZip);
                        });
                        thread.Start();
                    }
                    else
                    {
                        Process.Start(@"GameLauncher.exe");
                        error("Starting GameLauncher.exe");
                    }
                }
                catch (Exception ex)
                {
                    error("Failed to update.\n" + ex.Message);
                }
            };
        }

        private string FormatFileSize(long byteCount) {
            double[] numArray = new double[] { 1073741824, 1048576, 1024, 0 };
            string[] strArrays = new string[] { "GB", "MB", "KB", "Bytes" };
            for (int i = 0; i < (int)numArray.Length; i++) {
                if ((double)byteCount >= numArray[i]) {
                    return string.Concat(string.Format("{0:0.00}", (double)byteCount / numArray[i]), strArrays[i]);
                }
            }

            return "0 Bytes";
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            this.BeginInvoke((MethodInvoker)delegate {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                Information.Text = "Downloaded " + FormatFileSize(e.BytesReceived) + " of " + FormatFileSize(e.TotalBytesToReceive);
                DownloadProgress.Style = ProgressBarStyle.Blocks;
                DownloadProgress.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            this.BeginInvoke((MethodInvoker)delegate {
                DownloadProgress.Style = ProgressBarStyle.Marquee;

                string updatePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
                using (ZipArchive archive = ZipFile.OpenRead(tempNameZip)) {
                    int numFiles = archive.Entries.Count;
                    int current = 1;

                    DownloadProgress.Style = ProgressBarStyle.Blocks;

                    foreach (ZipArchiveEntry entry in archive.Entries) {
                        string fullName = entry.FullName;

                        if (fullName.Substring(fullName.Length - 1) == "/") {
                            string folderName = fullName.Remove(fullName.Length - 1);
                            if (Directory.Exists(folderName)) {
                                Directory.Delete(folderName, true);
                            }

                            Directory.CreateDirectory(folderName);
                        } else {
                            if (fullName != "GameLauncherUpdater.exe") {
                                if (File.Exists(fullName)) {
                                    File.Delete(fullName);
                                }

                                Information.Text = "Extracting: " + fullName;
								try { entry.ExtractToFile(Path.Combine(updatePath, fullName)); } catch { }
                                Delay.WaitMSeconds(200);
                            }
                        }

                        DownloadProgress.Value = (int)((long)100 * current / numFiles);
                        current++;
                    }
                }

                Process.Start(@"GameLauncher.exe");
                error("Update completed. Starting GameLauncher.exe");
            });
        }

        private void Form1_Load(object sender, EventArgs e) {
			this.BeginInvoke((MethodInvoker)delegate {
				DoUpdate();
			});
		}
    }
}
