using System;

namespace NhsoApp.Models;

public class ciddata
{
    public string pid { get; set; }
    public string hn { get; set; }
    public string titleName { get; set; }
    public string pname { get; set; }
    public string fname { get; set; }
    public string lname { get; set; }
    public string nation { get; set; }
    public string birthDate { get; set; }
    public string sex { get; set; }
    public DateTime transDate { get; set; }
    public string mainInscl { get; set; }
    public string subInscl { get; set; }
    public string age { get; set; }
    public DateTime checkDate { get; set; }
    public IList<ClaimType> claimTypes { get; set; }
    public HospMainOp hospMainOp { get; set; }
    public HospMain hospMain { get; set; }
    public string image { get; set; }
    public string pImage { get; set; }
    public string correlationId { get; set; }
    public class HospMainOp
    {
        public string hcode { get; set; }
        public string hname { get; set; }
    }
    public class HospMain
    {
        public string hcode { get; set; }
        public string hname { get; set; }
    }

    public class ClaimType
    {
        public string claimType { get; set; }
        public string claimTypeName { get; set; }
    }
}



