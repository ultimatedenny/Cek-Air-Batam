using System;
using System.Collections.Generic;
using System.Text;

namespace Cek_Air_Batam.Models
{
    public class DataPelanggan
    {
        public string nosamb { get; set; }
        public string nama { get; set; }
        public string alamat { get; set; }
        public string status { get; set; }
        public string golongan { get; set; }
        public int totaltagihan { get; set; }
    }

    public class RootDataPelanggan
    {
        public string mssg { get; set; }
        public DataPelanggan data { get; set; }
    }


    public class DataDetailTagihan
    {
        public string nosamb { get; set; }
        public string periode { get; set; }
        public int stanlalu { get; set; }
        public int stanskrg { get; set; }
        public string tanggalbaca { get; set; }
        public int stanangkat { get; set; }
        public int pakai { get; set; }
        public int pakaiBlk1 { get; set; }
        public int tarif1 { get; set; }
        public int subtotalBlk1 { get; set; }
        public int pakaiBlk2 { get; set; }
        public int tarif2 { get; set; }
        public int subtotalBlk2 { get; set; }
        public int pakaiBlk3 { get; set; }
        public int tarif3 { get; set; }
        public int subtotalBlk3 { get; set; }
        public int pakaiBlk4 { get; set; }
        public int tarif4 { get; set; }
        public int subtotalBlk4 { get; set; }
        public int pakaiBlk5 { get; set; }
        public int tarif5 { get; set; }
        public int subtotalBlk5 { get; set; }
        public int biayapemakaian { get; set; }
        public int administrasi { get; set; }
        public int pemeliharaan { get; set; }
        public int retribusi { get; set; }
        public int airlimbah { get; set; }
        public int pelayanan { get; set; }
        public int meterai { get; set; }
        public int ppn { get; set; }
        public int lebihbayar { get; set; }
        public int kurangbayar { get; set; }
        public int persenppn { get; set; }
        public int dendatunggakan { get; set; }
        public int total { get; set; }
    }

    public class RootDetailTagihan
    {
        public string mssg { get; set; }
        public DataDetailTagihan data { get; set; }
    }


    public class DataTagihan
    {
        public string tipedata { get; set; }
        public string jenis { get; set; }
        public string periode { get; set; }
        public string ketjenis { get; set; }
        public string flaglunas { get; set; }
        public string tglbayar { get; set; }
        public string kasir { get; set; }
        public object total { get; set; }
        public object ColorStatus { get; set; }
    }

    public class RootDataTagihan
    {
        public string mssg { get; set; }
        public List<DataTagihan> data { get; set; }
    }
}
