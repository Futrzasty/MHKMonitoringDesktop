using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace MHKITMonitoring1
{
    public partial class Form1 : Form
    {
        List<Status> m = new List<Status>();
        
        public Form1()
        {
            InitializeComponent();
            SaveWindow.GeometryFromString(Properties.Settings.Default.WindowGeometry, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Net.WebClient wc = new System.Net.WebClient();

            m.Clear();

            String grp = "";
            byte[] raw;
            String[] hosts;
            String[] param;
            String[] nagios;
            
            grp = "Środowisko";
            raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getEnvHostList.php");
            hosts = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
            foreach (String element in hosts)
            {
                raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getEnvHostParamsbyHost.php?"+element);
                param = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
                foreach (String elementp in param)
                {
                    raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getEnvHostStatebyHost.php?"+element+";"+elementp);
                    Status temp = new Status();
                    temp = JsonConvert.DeserializeObject<Status>(System.Text.Encoding.UTF8.GetString(raw));
                    temp.Group = grp;
                    temp.Name = temp.Name + " (" + elementp + ")";
                    m.Add(temp);
                }
            }    


            grp = "Serwery";
            raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getServerHostList.php");
            hosts = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
            foreach (String element in hosts)
            {
                raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getServerHostParamsbyHost.php?" + element);
                param = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
                foreach (String elementp in param)
                {
                    raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getServerHostStatebyHost.php?" + element + ";" + elementp);
                    Status temp = new Status();
                    temp = JsonConvert.DeserializeObject<Status>(System.Text.Encoding.UTF8.GetString(raw));
                    temp.Group = grp;
                    m.Add(temp);
                }
            }


            grp = "Tunele IPSec";
            raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getIpsecHostIDs.php");
            hosts = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
            foreach (String element in hosts)
            {
                 raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getIpsecHostStatebyID.php?" + element);
                 Status temp = new Status();
                 temp = JsonConvert.DeserializeObject<Status>(System.Text.Encoding.UTF8.GetString(raw));
                 temp.Group = grp;
                 if (temp.Value == "&nbsp;") temp.Value = "Brak odpowiedzi";
                 m.Add(temp);
            }

            grp = "Strony WWW";
            raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getWwwHostIDs.php");
            hosts = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
            foreach (String element in hosts)
            {
                raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getWwwHostStatebyID.php?" + element);
                Status temp = new Status();
                temp = JsonConvert.DeserializeObject<Status>(System.Text.Encoding.UTF8.GetString(raw));
                temp.Group = grp;
                m.Add(temp);
            }

            grp = "Łącza internetowe";
            raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getNetLinkIDs.php");
            hosts = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
            foreach (String element in hosts)
            {
                raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getNetLinkStatebyID.php?" + element);
                Status temp = new Status();
                temp = JsonConvert.DeserializeObject<Status>(System.Text.Encoding.UTF8.GetString(raw));
                temp.Group = grp;
                m.Add(temp);
            }

            grp = "Nagios";
            raw = wc.DownloadData("http://monitoring.mhk.local/JSON/getNagiosStatus.php");
            nagios = JsonConvert.DeserializeObject<String[]>(System.Text.Encoding.UTF8.GetString(raw));
            String ns = "";
            for (int i = 0; i < nagios.Length/7; i++)
            {
                Status nagios_status = new Status();
                nagios_status.Group = grp;
                if (nagios[0 + i * 7] != "OLD HOST")
                {
                    ns = nagios[0 + i * 7];
                    nagios_status.Name = nagios[0 + i * 7] + " (" + nagios[1 + i * 7] + ")";
                }
                else
                {
                    nagios_status.Name = ns + " (" + nagios[1 + i * 7] + ")";
                }

                
                nagios_status.Value = System.Net.WebUtility.HtmlDecode(nagios[6 + i * 7]);
                nagios_status.Value_S1 = null;
                switch (nagios[2 + i * 7])
                {
                    case "CRITICAL":
                        nagios_status.Color = "red";
                        break;
                    case "WARNING":
                        nagios_status.Color = "yellow";
                        break;
                    case "UNKNOWN":
                        nagios_status.Color = "orange";
                        break;
                    default:
                        nagios_status.Color = "gray";
                        break;
                }
                m.Add(nagios_status);
            }

            objectListView1.SetObjects(m);
            //objectListView1.BuildGroups(hGroup, SortOrder.None);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void objectListView1_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            Status temp = (Status)e.Model;
            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml(temp.Color);
        }

        private void objectListView1_FormatCell(object sender, BrightIdeasSoftware.FormatCellEventArgs e)
        {
            if (e.ColumnIndex == this.Value_S1.Index)
            {
                Status temp = (Status)e.Model;
                if (temp.Color_S1 != null)
                    e.SubItem.BackColor = System.Drawing.ColorTranslator.FromHtml(temp.Color_S1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String version = "v.0.0.0.0";
            version = "v."+Application.ProductVersion;
            this.Text = "MHK IT Monitoring System" + " " + version;
            button1.PerformClick();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.WindowGeometry = SaveWindow.GeometryToString(this);
            Properties.Settings.Default.Save();
        }
    }
}
