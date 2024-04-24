using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Cek_Air_Batam.Models;
using Newtonsoft.Json;
using MarcTron.Plugin;
using MarcTron.Plugin.CustomEventArgs;
using System.Security.Cryptography;

namespace Cek_Air_Batam
{
    public partial class MainPage : ContentPage
    {
        string key01 = "BSINFO.";
        string key02 = DateTime.Now.ToString("yyyy-MM-dd");
        string key03 = ".PDAM2017.";
        string key04 = DateTime.Now.ToString("HH:mm:ss");
        string hash = "";
        //https://apiservicekotabatamv1v1.limasakti.co.id/api/pelanggan/fed71a5fe09391433062be104210a3fd24538306/12345//2022-02-06.14:54:14
        string appelanggan = "https://apiservicekotabatamv1.limasakti.co.id/api/pelanggan/";
        //string apitagihan = "https://apiservicekotabatamv1.limasakti.co.id/api/tagihan/";
        //string aptagihandetail = "https://apiservicekotabatamv1.limasakti.co.id/api/detailtagihan/";
        string pelanggan = "";
        string tagihan = "";
        string detailtagihan = "";
        string timestamp = "//2021-07-12.22:48:20";
        string sha_key = "/ae4679ef9fdb2767634b496516eb9d89721ecdf6/";
        private bool _shouldSetEvents = true;
        string NoPelanggan = "";
        string LastNoPelanggan = "";
        bool refresh = false;
        string RewardId = "ca-app-pub-5151102131443886/8668371399";
        string InterstitialId = "ca-app-pub-5151102131443886/3483493320";
        string BannedId1 = "ca-app-pub-5151102131443886/4264800998";
        public MainPage()
        {
            InitializeComponent();
            string source = key01 + key02 + key03 + key04;
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
            CrossMTAdmob.Current.UserPersonalizedAds = true;
            CrossMTAdmob.Current.ComplyWithFamilyPolicies = true;
            CrossMTAdmob.Current.UseRestrictedDataProcessing = true;
            CrossMTAdmob.Current.TestDevices = new List<string>() { "C3726BF6-B01C-433D-B645-10CA1AD1ABE1", "C3726BF6B01C433DB64510CA1AD1ABE1" };
            CrossMTAdmob.Current.LoadRewardedVideo(RewardId);
            CrossMTAdmob.Current.TagForChildDirectedTreatment = MTTagForChildDirectedTreatment.TagForChildDirectedTreatmentUnspecified;
            CrossMTAdmob.Current.TagForUnderAgeOfConsent = MTTagForUnderAgeOfConsent.TagForUnderAgeOfConsentUnspecified;
            CrossMTAdmob.Current.MaxAdContentRating = MTMaxAdContentRating.MaxAdContentRatingG;
            CollectionViewTagihan.IsVisible = false;
            CekDetailButton.IsVisible = false;
            BannerAds.AdsId = BannedId1;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetEvents();
        }
        private void MyAds_AdVOpened(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show($"BannerAds: Opened");
        }

        private void MyAds_AdVImpression(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show($"BannerAds: Impression");
        }

        private void MyAds_AdVClosed(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show($"BannerAds: Closed");
        }

        private void MyAds_AdsClicked(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show($"BannerAds: Clicked");
        }

        private void MyAds_AdFailedToLoad(object sender, MTEventArgs e)
        {
            //DependencyService.Get<Toast>().Show($"BannerAds: {e.ErrorCode} - {e.ErrorMessage}");
        }

        private void MyAds_AdLoaded(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show($"BannerAds: Loaded");
        }

        public void SetEvents()
        {
            if (_shouldSetEvents)
            {
                _shouldSetEvents = false;
                CrossMTAdmob.Current.OnRewardedVideoStarted += Current_OnRewardedVideoStarted;
                CrossMTAdmob.Current.OnRewarded += Current_OnRewarded;
                CrossMTAdmob.Current.OnRewardedVideoAdClosed += Current_OnRewardedVideoAdClosed;
                CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += Current_OnRewardedVideoAdFailedToLoad;
                CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication += Current_OnRewardedVideoAdLeftApplication;
                CrossMTAdmob.Current.OnRewardedVideoAdLoaded += Current_OnRewardedVideoAdLoaded;
                CrossMTAdmob.Current.OnRewardedVideoAdOpened += Current_OnRewardedVideoAdOpened;
                CrossMTAdmob.Current.OnRewardedVideoAdCompleted += Current_OnRewardedVideoAdCompleted;
            }
        }
        public void getbtn_click(object sender, EventArgs e)
        {
            CekDetailButton.IsVisible = false;
            CollectionViewTagihan.IsVisible = false;
            NoPelanggan = NoSamb.Text;
            refresh = false;
            GetPelanggan();
        }
        public async void GetPelanggan()
        {
            try
            {
                pelanggan = "https://apiservicekotabatamv1.limasakti.co.id/api/pelanggan" + sha_key + NoSamb.Text + timestamp;
                getbtn.IsEnabled = false;
                if (string.IsNullOrEmpty(NoSamb.Text))
                {
                    await DisplayAlert("", "Harap isi nomor pelanggan anda", "Dismiss");
                    getbtn.IsEnabled = true;
                }
                else
                {
                    getbtn.Text = "Loading...";
                    string url = appelanggan + hash + "/" + NoSamb.Text + "//" + key02 + "." + key04;
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(pelanggan);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        RootDataPelanggan Model = JsonConvert.DeserializeObject<RootDataPelanggan>(content);
                        if (Model.mssg == "oke")
                        {
                            NOPEL.Text = ": " + Model.data.nosamb;
                            NAMA.Text = ": " + Model.data.nama;
                            ALAMAT.Text = ": " + Model.data.alamat;
                            STATUS.Text = ": " + Model.data.status;
                            GOLONGAN.Text = ": " + Model.data.golongan;
                            TOTALTAGIHAN.Text = ": Rp." + Model.data.totaltagihan;
                            GetTagihan();
                        }
                        else
                        {
                            getbtn.IsEnabled = true;
                            getbtn.Text = "Cek Tagihan";
                            await DisplayAlert("", Model.mssg.ToString(), "Dismiss");
                        }
                    }
                    else
                    {
                        getbtn.Text = "Cek Tagihan";
                        getbtn.IsEnabled = true;
                        await DisplayAlert("", response.Content.ToString(), "Dismiss");
                    }
                }
            }
            catch (Exception msg)
            {
                getbtn.Text = "Cek Tagihan";
                getbtn.IsEnabled = true;
                await DisplayAlert("", msg.Message.ToString(), "Dismiss");
            }
        }
        public async void GetTagihan()
        {
            try
            {
                tagihan = "https://apiservicekotabatamv1.limasakti.co.id/api/tagihan" + sha_key + NoSamb.Text + timestamp;
                getbtn.IsEnabled = false;
                if (string.IsNullOrEmpty(NoSamb.Text))
                {
                    await DisplayAlert("", "Harap isi nomor pelanggan anda", "Dismiss");
                    getbtn.IsEnabled = true;
                }
                else
                {
                    getbtn.Text = "Loading...";
                    string url = appelanggan + hash + "/" + NoSamb.Text + "//" + key02 + "." + key04;

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(tagihan);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        RootDataTagihan Model = JsonConvert.DeserializeObject<RootDataTagihan>(content);
                        List<DataTagihan> Model2 = new List<DataTagihan>();
                        Model2.Clear();
                        foreach (var result in Model.data)
                        {
                            if (Convert.ToString(result.flaglunas) == "1")
                            {
                                Model2.Add(new DataTagihan()
                                {
                                    tipedata = result.tipedata.ToString().ToUpper(),
                                    jenis = result.jenis.ToString().ToUpper(),
                                    periode = result.periode.ToString().ToUpper(),
                                    ketjenis = result.ketjenis.ToString().ToUpper(),
                                    flaglunas = "LUNAS",
                                    tglbayar = result.tglbayar.ToString().ToUpper(),
                                    kasir = result.kasir.ToString().ToUpper(),
                                    total = result.total.ToString().ToUpper(),
                                    ColorStatus = "#31B057",
                                });
                            }
                            else
                            {
                                Model2.Add(new DataTagihan()
                                {
                                    tipedata = result.tipedata.ToString().ToUpper(),
                                    jenis = result.jenis.ToString().ToUpper(),
                                    periode = result.periode.ToString().ToUpper(),
                                    ketjenis = result.ketjenis.ToString().ToUpper(),
                                    flaglunas = "BELUM LUNAS",
                                    tglbayar = result.tglbayar.ToString().ToUpper(),
                                    kasir = result.kasir.ToString().ToUpper(),
                                    total = result.total.ToString().ToUpper(),
                                    ColorStatus = "#E52A34",
                                });
                            }
                        }
                        CollectionViewTagihan.ItemsSource = Model2;
                        getbtn.Text = "Cek Tagihan";
                        getbtn.IsEnabled = true;
                        if (refresh == true)
                        {
                            CekDetailButton.IsVisible = false;
                        }
                        else
                        {
                            CekDetailButton.IsVisible = true;
                        }
                    }
                    else
                    {
                        getbtn.Text = "Cek Tagihan";
                        getbtn.IsEnabled = true;
                        await DisplayAlert("", response.Content.ToString(), "Dismiss");
                    }
                }
            }
            catch (Exception msg)
            {
                getbtn.Text = "Cek Tagihan";
                getbtn.IsEnabled = true;
                await DisplayAlert("", msg.Message.ToString(), "Dismiss");
            }
        }
        private void CekDetailBulan(object sender, EventArgs e)
        {
            var item = (SwipeItemView)sender;
            string _item = item.CommandParameter.ToString();
            GetTagihanBulan(_item);
        }
        public async void GetTagihanBulan(string bulan)
        {
            try
            {
                //https://apiservicekotabatamv1.limasakti.co.id/api/detailtagihan/8f612b79b14b449715ff11d9709a8ee06391bd3b/123456/202109/2021-10-06.5:12:33
                detailtagihan = "https://apiservicekotabatamv1.limasakti.co.id/api/detailtagihan/8f612b79b14b449715ff11d9709a8ee06391bd3b/" + NoSamb.Text + "/" + bulan + "/2021-10-06.5:12:33";
                //detailtagihan = "https://apiservicekotabatamv1.limasakti.co.id/api/detailtagihan" + sha_key + NoSamb.Text + bulan + "/" + timestamp;
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(detailtagihan);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RootDetailTagihan Model = JsonConvert.DeserializeObject<RootDetailTagihan>(content);
                    List<RootDetailTagihan> Model2 = new List<RootDetailTagihan>();

                    await DisplayAlert("Detail Tagihan: "+ bulan + "", 
                        "No. sambungan: " + Model.data.nosamb + "\n" +
                        "Periode: " + Model.data.periode + "\n" +
                        "STAN lalu:" + Model.data.stanlalu + "\n" +
                        "STAN sekarang: " + Model.data.stanskrg + "\n" +
                        "Tanggal baca: " + Model.data.tanggalbaca + "\n" +
                        "STAN angkat: " + Model.data.stanangkat + "\n" +
                        "Pakai(m3): " + Model.data.pakai + "\n" +
                        "-------------------------------------\n" +
                        "Blok I\n" +
                        "Pakai: " + Model.data.pakaiBlk1 + "\n" +
                        "Tarif: Rp." + Model.data.tarif1 + "\n" +
                        "Subtotal: Rp." + Model.data.subtotalBlk1 + "\n" +
                       "-------------------------------------\n" +
                        "Blok II\n" +
                        "Pakai: " + Model.data.pakaiBlk2 + "\n" +
                        "Tarif: Rp." + Model.data.tarif2 + "\n" +
                        "Subtotal: Rp." + Model.data.subtotalBlk2 + "\n" +
                        "-------------------------------------\n" +
                        "Blok III\n" +
                        "Pakai: " + Model.data.pakaiBlk3 + "\n" +
                        "Tarif: Rp." + Model.data.tarif3 + "\n" +
                        "Subtotal: Rp." + Model.data.subtotalBlk3 + "\n" +
                        "-------------------------------------\n" +
                        "Blok IV\n" +
                        "Pakai: " + Model.data.pakaiBlk4+ "\n" +
                        "Tarif: Rp." + Model.data.tarif4 + "\n" +
                        "Subtotal: Rp." + Model.data.subtotalBlk4 + "\n" +
                        "-------------------------------------\n" +
                        "Blok V\n" +
                        "Pakai: " + Model.data.pakaiBlk5 + "\n" +
                        "Tarif: Rp." + Model.data.tarif5 + "\n" +
                        "Subtotal: Rp." + Model.data.subtotalBlk5 + "\n" +
                         "-------------------------------------\n" +
                        "Biaya Administrasi: Rp." + Model.data.administrasi + "\n" +
                        "Biaya Pemakaian: Rp." + Model.data.biayapemakaian + "\n" +
                        "Biaya Pemeliharaan: Rp." + Model.data.pemeliharaan + "\n" +
                        "Biaya Retribusi: Rp." + Model.data.retribusi + "\n" +
                        "Biaya Air limbah: Rp." + Model.data.airlimbah + "\n" +
                        "Biaya Pelayanan: Rp." + Model.data.pelayanan + "\n" +
                        "Biaya Meterai: Rp." + Model.data.meterai + "\n" +
                        "Biaya PPN: Rp." + Model.data.ppn + "\n" +
                        "Biaya Lebih Bayar: Rp." + Model.data.lebihbayar + "\n" +
                        "Biaya Kurang Bayar: Rp." + Model.data.kurangbayar + "\n" +
                        "Biaya Persenppn: Rp." + Model.data.persenppn + "\n" +
                        "Biaya Dendatunggakan: Rp." + Model.data.dendatunggakan + "\n" +
                        "Biaya Total: Rp." + Model.data.total + "\n" , "Dismiss");
                }
                else
                {
                    await DisplayAlert("", response.IsSuccessStatusCode.ToString(), "Dismiss");
                }
            }
            catch (Exception msg)
            {
                await DisplayAlert("", msg.Message.ToString(), "Dismiss");
            }
        }
        public void RefReceive(object sender, EventArgs e)
        {
            GetPelanggan();
            RefreshReceive.IsRefreshing = false;
            refresh = true;
        }
        private async void CekDetailClicked(object sender, EventArgs e)
        {
            try
            {
                if (LastNoPelanggan != NoPelanggan)
                {
                    CrossMTAdmob.Current.LoadRewardedVideo(RewardId);
                    await Task.Delay(500);
                    if (CrossMTAdmob.Current.IsRewardedVideoLoaded() == true)
                    {
                        CrossMTAdmob.Current.ShowRewardedVideo();
                    }
                    else
                    {
                        CollectionViewTagihan.IsVisible = true;
                        CekDetailButton.IsVisible = false;
                        LastNoPelanggan = NoPelanggan;
                        NoPelanggan = "";
                    }
                }
                else
                {
                    CollectionViewTagihan.IsVisible = true;
                    CekDetailButton.IsVisible = false;
                    LastNoPelanggan = NoPelanggan;
                }
            }
            catch (Exception msg)
            {
                DependencyService.Get<Toast>().Show(msg.Message.ToString());
            }
        }
        private void Current_OnRewardedVideoAdLoaded(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show("Current_OnRewardedVideoAdLoaded");
        }
        private void Current_OnRewardedVideoAdOpened(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show("Current_OnRewardedVideoAdOpened");
        }
        private void Current_OnRewardedVideoAdLeftApplication(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show("Current_OnRewardedVideoAdLeftApplication");
            CollectionViewTagihan.IsVisible = false;
        }
        private void Current_OnRewardedVideoAdFailedToLoad(object sender, MTEventArgs e)
        {
            //DependencyService.Get<Toast>().Show("Current_OnRewardedVideoAdFailedToLoad");
            CollectionViewTagihan.IsVisible = false;
        }
        private void Current_OnRewardedVideoAdClosed(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show("Current_OnRewardedVideoAdClosed");
        }
        private void Current_OnRewarded(object sender, MTEventArgs e)
        {
            //DependencyService.Get<Toast>().Show($"OnRewarded: {e.RewardType} - {e.RewardAmount}");
            CollectionViewTagihan.IsVisible = true;
            CekDetailButton.IsVisible = false;
            LastNoPelanggan = NoPelanggan;
            NoPelanggan = "";
        }
        private void Current_OnRewardedVideoStarted(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show("Current_OnRewardedVideoStarted");
        }
        private void Current_OnRewardedVideoAdCompleted(object sender, EventArgs e)
        {
            //DependencyService.Get<Toast>().Show("Current_OnRewardedVideoAdCompleted");
        }
    }
}
