using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DH
{
    public partial class Form1 : Form
    {

        public List<Link> source;
        public Form1()
        {
            InitializeComponent();
            source = new List<Link>();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var kategoriler = new List<Link>();
            kategoriler.Add(new Link { Name = "Tümü", Adres = "https://forum.donanimhaber.com/forumid_193/tt.htm" });
            kategoriler.Add(new Link { Name = "Bilgisayar", Adres = "https://forum.donanimhaber.com/forumid_598/tt.htm" });
            kategoriler.Add(new Link { Name = "Ev Elektroniği", Adres = "https://forum.donanimhaber.com/forumid_599/tt.htm" });
            kategoriler.Add(new Link { Name = "Cep Telefonu", Adres = "https://forum.donanimhaber.com/forumid_600/tt.htm" });
            kategoriler.Add(new Link { Name = "Gıda, Giyim, Aksesuar", Adres = "https://forum.donanimhaber.com/forumid_601/tt.htm" });
            kategoriler.Add(new Link { Name = "Günlük İndirimler", Adres = "https://forum.donanimhaber.com/forumid_602/tt.htm" });
            kategoriler.Add(new Link { Name = "Banka, Kredi vs", Adres = "https://forum.donanimhaber.com/forumid_2119/tt.htm" });
            kategoriler.Add(new Link { Name = "Bebek ürünleri", Adres = "https://forum.donanimhaber.com/forumid_2655/tt.htm" });
            kategoriler.Add(new Link { Name = "Diğer", Adres = "https://forum.donanimhaber.com/forumid_603/tt.htm" });

            cmbCategory.DataSource = kategoriler;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Adres";

            cmbCategory.SelectedIndexChanged += CmbCategory_SelectedIndexChanged;
            Guncelle(kategoriler[0].Adres);
        }

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Link link = cmbCategory.SelectedItem as Link;
            Guncelle(link.Adres);
        }

        private void Guncelle(string url)
        {
            lstResults.DataSource = null;
            lstResults.KeyDown -= LstResults_KeyDown;

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            //var node = htmlDoc.DocumentNode.SelectNodes("//kl-icerik-satir yenikonu");

            var node = htmlDoc.DocumentNode.Descendants("div").ToList();
            var classli = node.Where(d => d.GetAttributeValue("class", "") == "kl-icerik-satir yenikonu").ToList();

            var yeniKonular =
              from div in htmlDoc.DocumentNode.Descendants("div")
                .Where(x => x.GetAttributeValue("class", "") == "kl-icerik-satir yenikonu")
                  //where td.InnerText.Trim() == "Test1"
              select div;

            source.Clear();

            foreach (HtmlNode konu in yeniKonular)
            {
                var baslik = from div in konu.Descendants("div")
                             .Where(d => d.GetAttributeValue("class", "") == "kl-konu")
                             from div2 in div.Descendants("a")
                             select div2;
                var b = baslik.First().InnerText.Trim();
                var link = baslik.First().GetAttributeValue("href", "");
                source.Add(new Link()
                {
                    Name = b,
                    Adres = link
                });
            }

            lstResults.DataSource = source;
            lstResults.DisplayMember = "Name";

            lstResults.KeyDown += LstResults_KeyDown;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void LstResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Link selected = lstResults.SelectedItem as Link;
                Process.Start("https://forum.donanimhaber.com" + selected.Adres);
            }
        }

        private void lstResults_DoubleClick(object sender, EventArgs e)
        {
            Link selected = lstResults.SelectedItem as Link;

            if (selected != null)
                Process.Start("https://forum.donanimhaber.com" + selected.Adres);
        }
    }
}
