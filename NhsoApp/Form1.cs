using System.Text;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NhsoApp.Models;
using ThaiNationalIDCard;

namespace NhsoApp
{
    public partial class Form1 : Form
    {
        string returnValueData;
        string data_ctype;
        string data_clemdetail;
        string Tname;
        string Tnation;
        string _pid;
        string _claimType;
        string _mobile;
        string _correlationId;
        private static HttpClient _httpClient = new HttpClient();

        private static Uri BaseAddress { get; set; } = new Uri("http://localhost:8189");

        private ThaiIDCard idcard;
        // List<ciddata> ciddatas = new List<ciddata>();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //  idcard = new ThaiIDCard();
            lblfcid.Text = "X XXXX XXXXX XX X";
            lblfhn.Text = "XXXXXXX";
            lblhcode.Text = "XXXXXXX";
            lblsubInscl.Text = "XXXXXXXXXXX";
            lblcorrelationId.Text = "XXXXXXXXXXX";
            lblbirthDate.Text = "XX XX XXX";
            lblfname.Text = "XX XXXXX  XXXXX";
            // lblnation.Text = "XX";
            lblsex.Text = "XX";
            lblage.Text = "XX";
            lbldepartment.Text = "XXXXXXXX";
            label8.Show();
            GetCard();
            //  idcard.MonitorStart 

            // label7.Text = DateTime.Now.ToString();

        }
        private async Task F_Load()
        {
            //  idcard = new ThaiIDCard();
            lblfcid.Text = "X XXXX XXXXX XX X";
            lblfhn.Text = "XXXXXXX";
            lblhcode.Text = "XXXXXXX";
            lblsubInscl.Text = "XXXXXXXXXXX";
            lblcorrelationId.Text = "XXXXXXXXXXX";
            lblbirthDate.Text = "XX XX XXX";
            lblfname.Text = "XX XXXXX  XXXXX";
            // lblnation.Text = "XX";
            lblsex.Text = "XX";
            lblage.Text = "XX";
            lbldepartment.Text = "XXXXXXXX";
            pictureBox1.Image = "";
            label8.Show();
            GetCard();
            //  idcard.MonitorStart 

            // label7.Text = DateTime.Now.ToString();

        }

        private void bntRead_Click(object sender, EventArgs e)
        {
            // GetCard();  // อ่านบัตร
        }
        public class ClaimType
        {
            public string claimType { get; set; }
            public string claimTypeName { get; set; }
        }

        public async Task GetCard()
        {
            string url_checkcard = "http://localhost:8189/api/smartcard/terminals";
            string url = "http://localhost:8189/api/smartcard/read?readImageFlag=false";
            
            HttpClient client = new HttpClient();

            // Check for card
            string response_readcard = await client.GetStringAsync(url_checkcard);
            var datacard = JsonConvert.DeserializeObject<List<checkcard>>(response_readcard);
            bool tnamecard = datacard.Any(x => x.isPresent);

            if (!tnamecard)
            {
                // ❌ No MessageBox — just wait 5 seconds and retry
                await Task.Delay(5000); // 5 seconds delay
                GetCard(); // Retry
                return;
            }

            // ✅ Card is present — read card data
            string response = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<ciddata>(response);
            string urlp = "http://172.16.200.202:8089/api/Hos/getpatienthnimage?_cid=" + data.pid;
            string responsep = await client.GetStringAsync(urlp);
            var patients = JsonConvert.DeserializeObject<ciddata>(responsep);

            // Convert title
            string Tname = patients.pname  ?? "";
            // switch (tname)
            // {
            //     case "001": Tname = "นายแพทย์"; break;
            //     case "002": Tname = "ด.ญ."; break;
            //     case "003": Tname = "นาย"; break;
            //     case "004": Tname = "นางสาว"; break;
            //     default: Tname = ""; break;
            // }

            // Convert nationality
            string tnation = data.nation;
            lblfhn.Text= patients.hn  ?? "";
            // tname.Text= patients.pname;
            Tnation = tnation == "099" ? "ไทย" : "";

            // Set values to UI
            lblfname.Text = Tname + " " + data.fname + "  " + data.lname;
            lblsubInscl.Text = data.subInscl;
            lblcorrelationId.Text = data.correlationId;
            // lblnation.Text = Tnation;
            lblfcid.Text = data.pid;
            // lblhcode.Text = $"{data.hospMain.hcode} {data.hospMain.hname}";

            string dd = data.birthDate.Substring(6);
            string mm = data.birthDate.Substring(4, 2);
            string yy = data.birthDate.Substring(0, 4);
            var thaiMonths = new Dictionary<string, string>
                {
                    { "01", "มกราคม" },
                    { "02", "กุมภาพันธ์" },
                    { "03", "มีนาคม" },
                    { "04", "เมษายน" },
                    { "05", "พฤษภาคม" },
                    { "06", "มิถุนายน" },
                    { "07", "กรกฎาคม" },
                    { "08", "สิงหาคม" },
                    { "09", "กันยายน" },
                    { "10", "ตุลาคม" },
                    { "11", "พฤศจิกายน" },
                    { "12", "ธันวาคม" }
                };

            // แปลงเดือนเป็นชื่อภาษาไทย
            string thaiMonth = thaiMonths.ContainsKey(mm) ? thaiMonths[mm] : "";

            // แสดงวันเกิดรูปแบบ: 01 มกราคม 2567
            lblbirthDate.Text = $"{dd} {thaiMonth} {yy}";
            // lblbirthDate.Text = dd + "-" + mm + "-" + yy;
            lblage.Text = data.age;
            lblsex.Text = data.sex;
            
            
            if (data.hospMainOp == null || string.IsNullOrWhiteSpace(data.hospMainOp.hcode))
            {
                if (data.hospMain == null || string.IsNullOrWhiteSpace(data.hospMain.hcode))
                {
                    lblhcode.Text = "10734 รพ.สมุทรสาคร";
                }
                else
                {
                    lblhcode.Text = $"{data.hospMain.hcode ?? ""} {data.hospMain.hname ?? ""}";
                    // document.getElementById('hospMain').value = "99999";
                }
            }
            else
            {
                lblhcode.Text = $"{data.hospMainOp.hcode ?? ""} {data.hospMainOp.hname ?? ""}";
            }
            if (data.hospMain != null )// || string.IsNullOrWhiteSpace(data.hospMain.hcode))
            {
                if (data.hospMain.hcode == "11304" || data.hospMain.hcode == "11305")
                {
                    if (data.mainInscl == "(UCS) สิทธิหลักประกันสุขภาพแห่งชาติ" && data.subInscl == "(89) ช่วงอายุ 12-59 ปี")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(71) เด็กอายุไม่เกิน 12 ปีบริบูรณ์")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P1) สิทธิครูเอกชน")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P2) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจากกรมบัญชีกลาง)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P3) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจาก อปท.)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E1) สิทธิเบิกหน่วยงานรัฐ (ตนเอง)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E2) สิทธิเบิกหน่วยงานรัฐ (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E3) สิทธิเบิกหน่วยงานรัฐ (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G1) หน่วยงานรัฐอื่นๆ")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B6) สิทธิเบิกบุคคลในครอบครัวลูกจ้างชั่วคราวกรุงเทพมหานคร (เบิกใบเสร็จ/หนังสือรับรองสิทธิ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O1) สิทธิเบิกกรมบัญชีกลาง (ข้าราชการ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O2) สิทธิเบิกกรมบัญชีกลาง (ลูกจ้างประจำ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O3) สิทธิเบิกกรมบัญชีกลาง (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O4) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O5) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B1) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ข้าราชการ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B2) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ลูกจ้างประจำ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B3) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B4) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B5) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C1) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (เจ้าหน้าที่)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C2) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (พนักงาน)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C3) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C4) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C5) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C6) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (กรณีได้รับสิทธิเฉพาะหน่วยงาน)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G5) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (ตนเอง)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G6) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L1) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ตนเอง)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L2) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L3) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L4) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L5) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ข้าราชการการเมือง)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L6) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวข้าราชการการเมือง)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L9) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ยังไม่ระบุตำแหน่ง)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(68) สมาชิกผู้บริจาคโลหิตของสภากาชาดไทย ซึ่งบริจาคโลหิตตั้งแต่ 18 ครั้ง ขึ้นไป")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(72) ผู้มีรายได้น้อย")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(73) นักเรียนมัธยมศึกษาตอนต้น")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(74) คนพิการ")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(66) ผู้ได้รับพระราชทานเหรียญราชการชายแดน")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(67) ผู้ได้รับพระราชทานเหรียญพิทักษ์เสรีชน")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(75) ทหารผ่านศึกชั้น 1-3 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(80) บุคคลในครอบครัวทหารผ่านศึกชั้น 1-3 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(85) ผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(86) บุคคลในครอบครัวของผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(90) ทหารเกณฑ์")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(93) นักเรียนทหาร")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(94) ทหารผ่านศึกชั้น 4 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(95) บุคคลในครอบครัวทหารผ่านศึกชั้น 4 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(96) ทหารพราน")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(97) บุคคลในครอบครัวทหารของกรมสวัสดิการ 3 เหล่าทัพ")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(98) บุคคลในครอบครัวทหารผ่านศึกนอกประจำการบัตรชั้นที่ 1")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(76) พระภิกษุ สามเณร แม่ชี นักบวช และนักพรตในพระพุทธศาสนาซึ่งมีหนังสือสุทธิรับรอง")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(77) ผู้มีอายุเกิน 60 ปีบริบูรณ์")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(81) ผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(87) บุคคลในครอบครัวของผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(82) อาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(88) บุคคลในครอบครัวของอาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(91) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(ราชทัณฑ์)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(92) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(สถานพินิจและสถานสงเคราะห์)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(DIS) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)" && data.subInscl == "(D1) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S1) สิทธิเบิกกองทุนประกันสังคม (ผู้ประกันตน)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S2) สิทธิเบิกประกันสังคม (เบิกส่วนต่างกรมบัญชีกลางได้เฉพาะกรณี)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S3) สิทธิเบิกประกันสังคม (เบิกส่วนต่างจากอปท.ได้เฉพาะกรณี)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else if (data.mainInscl == "(SSI) สิทธิประกันสังคมกรณีทุพพลภาพ" && data.subInscl == "(S6) สิทธิเบิกกองทุนประกันสังคม (ทุพพลภาพ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else
                    {
                        lbldepartment.Text = " ";
                    }
                }
                else
                {
                    // lbldepartment.Text = "ffffff";
                    if (data.mainInscl == "(UCS) สิทธิหลักประกันสุขภาพแห่งชาติ" && data.subInscl == "(89) ช่วงอายุ 12-59 ปี")
                    {
                        lbldepartment.Text = "11 UC เลือกรพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(71) เด็กอายุไม่เกิน 12 ปีบริบูรณ์")
                    {
                        lbldepartment.Text = "71 UC เด็กต่ำกว่า 12 ปี ในเขต";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P1) สิทธิครูเอกชน")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P2) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจากกรมบัญชีกลาง)")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P3) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจาก อปท.)")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E1) สิทธิเบิกหน่วยงานรัฐ (ตนเอง)")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E2) สิทธิเบิกหน่วยงานรัฐ (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E3) สิทธิเบิกหน่วยงานรัฐ (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G1) หน่วยงานรัฐอื่นๆ")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B6) สิทธิเบิกบุคคลในครอบครัวลูกจ้างชั่วคราวกรุงเทพมหานคร (เบิกใบเสร็จ/หนังสือรับรองสิทธิ)")
                    {
                        lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O1) สิทธิเบิกกรมบัญชีกลาง (ข้าราชการ)")
                    {
                        lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O2) สิทธิเบิกกรมบัญชีกลาง (ลูกจ้างประจำ)")
                    {
                        lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O3) สิทธิเบิกกรมบัญชีกลาง (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O4) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O5) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B1) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ข้าราชการ)")
                    {
                        lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B2) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ลูกจ้างประจำ)")
                    {
                        lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B3) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B4) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B5) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C1) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (เจ้าหน้าที่)")
                    {
                        lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C2) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (พนักงาน)")
                    {
                        lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C3) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C4) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C5) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C6) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (กรณีได้รับสิทธิเฉพาะหน่วยงาน)")
                    {
                        lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G5) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (ตนเอง)")
                    {
                        lbldepartment.Text = "S7 79องค์การขนส่งมวลชนกรุงเทพ (ตนเอง)";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G6) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "S8 80องค์การขนส่งมวลชนกรุงเทพ (ญาติ)";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L1) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ตนเอง)")
                    {
                        lbldepartment.Text = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L2) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัว)")
                    {
                        lbldepartment.Text = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L3) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L4) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        lbldepartment.Text = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L5) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ข้าราชการการเมือง)")
                    {
                        lbldepartment.Text = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L6) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวข้าราชการการเมือง)")
                    {
                        lbldepartment.Text = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L9) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ยังไม่ระบุตำแหน่ง)")
                    {
                        lbldepartment.Text = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(68) สมาชิกผู้บริจาคโลหิตของสภากาชาดไทย ซึ่งบริจาคโลหิตตั้งแต่ 18 ครั้ง ขึ้นไป")
                    {
                        lbldepartment.Text = "33 บริจาคโลหิต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(72) ผู้มีรายได้น้อย")
                    {
                        lbldepartment.Text = "72 UC ผู้มีรายได้น้อยในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(73) นักเรียนมัธยมศึกษาตอนต้น")
                    {
                        lbldepartment.Text = "73 UC นักเรียนในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(74) คนพิการ")
                    {
                        lbldepartment.Text = "74 UC ผู้พิการในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(66) ผู้ได้รับพระราชทานเหรียญราชการชายแดน")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(67) ผู้ได้รับพระราชทานเหรียญพิทักษ์เสรีชน")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(75) ทหารผ่านศึกชั้น 1-3 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(80) บุคคลในครอบครัวทหารผ่านศึกชั้น 1-3 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(85) ผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(86) บุคคลในครอบครัวของผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(90) ทหารเกณฑ์")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(93) นักเรียนทหาร")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(94) ทหารผ่านศึกชั้น 4 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(95) บุคคลในครอบครัวทหารผ่านศึกชั้น 4 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(96) ทหารพราน")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(97) บุคคลในครอบครัวทหารของกรมสวัสดิการ 3 เหล่าทัพ")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(98) บุคคลในครอบครัวทหารผ่านศึกนอกประจำการบัตรชั้นที่ 1")
                    {
                        lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(76) พระภิกษุ สามเณร แม่ชี นักบวช และนักพรตในพระพุทธศาสนาซึ่งมีหนังสือสุทธิรับรอง")
                    {
                        lbldepartment.Text = "76 UC ภิกษุ ผู้นำศาสนา ในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(77) ผู้มีอายุเกิน 60 ปีบริบูรณ์")
                    {
                        lbldepartment.Text = "77 UC ผู้สูงอายุ ในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(81) ผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                    {
                        lbldepartment.Text = "29 UC ปะเภทผู้นำชุมชน";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(87) บุคคลในครอบครัวของผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                    {
                        lbldepartment.Text = "29 UC ปะเภทผู้นำชุมชน";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(82) อาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                    {
                        lbldepartment.Text = "28 UC ประเภทอาสาสมัคร";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(88) บุคคลในครอบครัวของอาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                    {
                        lbldepartment.Text = "28 UC ประเภทอาสาสมัคร";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(91) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(ราชทัณฑ์)")
                    {
                        lbldepartment.Text = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(92) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(สถานพินิจและสถานสงเคราะห์)")
                    {
                        lbldepartment.Text = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                    }
                    else if (data.mainInscl == "(DIS) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)" && data.subInscl == "(D1) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)")
                    {
                        lbldepartment.Text = "K9 ผู้ประกันตนคนพิการ รพ.สค. (สปสช)";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S1) สิทธิเบิกกองทุนประกันสังคม (ผู้ประกันตน)")
                    {
                        lbldepartment.Text = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S2) สิทธิเบิกประกันสังคม (เบิกส่วนต่างกรมบัญชีกลางได้เฉพาะกรณี)")
                    {
                        lbldepartment.Text = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S3) สิทธิเบิกประกันสังคม (เบิกส่วนต่างจากอปท.ได้เฉพาะกรณี)")
                    {
                        lbldepartment.Text = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(SSI) สิทธิประกันสังคมกรณีทุพพลภาพ" && data.subInscl == "(S6) สิทธิเบิกกองทุนประกันสังคม (ทุพพลภาพ)")
                    {
                        lbldepartment.Text = "10 ชำระเงินเอง";
                    }
                    else
                    {
                        lbldepartment.Text = " ";
                    }
                }
            }else
            {
                // lbldepartment.Text = "ffffff";
                if (data.mainInscl == "(UCS) สิทธิหลักประกันสุขภาพแห่งชาติ" && data.subInscl == "(89) ช่วงอายุ 12-59 ปี")
                {
                    lbldepartment.Text = "11 UC เลือกรพ.สมุทรสาคร";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(71) เด็กอายุไม่เกิน 12 ปีบริบูรณ์")
                {
                    lbldepartment.Text = "71 UC เด็กต่ำกว่า 12 ปี ในเขต";
                }
                else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P1) สิทธิครูเอกชน")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P2) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจากกรมบัญชีกลาง)")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P3) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจาก อปท.)")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E1) สิทธิเบิกหน่วยงานรัฐ (ตนเอง)")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E2) สิทธิเบิกหน่วยงานรัฐ (บุคคลในครอบครัว)")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E3) สิทธิเบิกหน่วยงานรัฐ (ผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G1) หน่วยงานรัฐอื่นๆ")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B6) สิทธิเบิกบุคคลในครอบครัวลูกจ้างชั่วคราวกรุงเทพมหานคร (เบิกใบเสร็จ/หนังสือรับรองสิทธิ)")
                {
                    lbldepartment.Text = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O1) สิทธิเบิกกรมบัญชีกลาง (ข้าราชการ)")
                {
                    lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O2) สิทธิเบิกกรมบัญชีกลาง (ลูกจ้างประจำ)")
                {
                    lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O3) สิทธิเบิกกรมบัญชีกลาง (ผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O4) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัว)")
                {
                    lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O5) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B1) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ข้าราชการ)")
                {
                    lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B2) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ลูกจ้างประจำ)")
                {
                    lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B3) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B4) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัว)")
                {
                    lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B5) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "O3 76ข้าราชการเขต ก.ท.ม.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C1) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (เจ้าหน้าที่)")
                {
                    lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C2) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (พนักงาน)")
                {
                    lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C3) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (ผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C4) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัว)")
                {
                    lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C5) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C6) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (กรณีได้รับสิทธิเฉพาะหน่วยงาน)")
                {
                    lbldepartment.Text = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G5) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (ตนเอง)")
                {
                    lbldepartment.Text = "S7 79องค์การขนส่งมวลชนกรุงเทพ (ตนเอง)";
                }
                else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G6) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (บุคคลในครอบครัว)")
                {
                    lbldepartment.Text = "S8 80องค์การขนส่งมวลชนกรุงเทพ (ญาติ)";
                }
                else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L1) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ตนเอง)")
                {
                    lbldepartment.Text = "24 24ข้าราชการ อปท.";
                }
                else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L2) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัว)")
                {
                    lbldepartment.Text = "24 24ข้าราชการ อปท.";
                }
                else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L3) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "24 24ข้าราชการ อปท.";
                }
                else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L4) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                {
                    lbldepartment.Text = "24 24ข้าราชการ อปท.";
                }
                else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L5) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ข้าราชการการเมือง)")
                {
                    lbldepartment.Text = "24 24ข้าราชการ อปท.";
                }
                else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L6) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวข้าราชการการเมือง)")
                {
                    lbldepartment.Text = "24 24ข้าราชการ อปท.";
                }
                else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L9) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ยังไม่ระบุตำแหน่ง)")
                {
                    lbldepartment.Text = "24 24ข้าราชการ อปท.";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(68) สมาชิกผู้บริจาคโลหิตของสภากาชาดไทย ซึ่งบริจาคโลหิตตั้งแต่ 18 ครั้ง ขึ้นไป")
                {
                    lbldepartment.Text = "33 บริจาคโลหิต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(72) ผู้มีรายได้น้อย")
                {
                    lbldepartment.Text = "72 UC ผู้มีรายได้น้อยในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(73) นักเรียนมัธยมศึกษาตอนต้น")
                {
                    lbldepartment.Text = "73 UC นักเรียนในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(74) คนพิการ")
                {
                    lbldepartment.Text = "74 UC ผู้พิการในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(66) ผู้ได้รับพระราชทานเหรียญราชการชายแดน")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(67) ผู้ได้รับพระราชทานเหรียญพิทักษ์เสรีชน")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(75) ทหารผ่านศึกชั้น 1-3 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(80) บุคคลในครอบครัวทหารผ่านศึกชั้น 1-3 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(85) ผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(86) บุคคลในครอบครัวของผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(90) ทหารเกณฑ์")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(93) นักเรียนทหาร")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(94) ทหารผ่านศึกชั้น 4 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(95) บุคคลในครอบครัวทหารผ่านศึกชั้น 4 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(96) ทหารพราน")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(97) บุคคลในครอบครัวทหารของกรมสวัสดิการ 3 เหล่าทัพ")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(98) บุคคลในครอบครัวทหารผ่านศึกนอกประจำการบัตรชั้นที่ 1")
                {
                    lbldepartment.Text = "75 UC ทหารผ่านศึกในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(76) พระภิกษุ สามเณร แม่ชี นักบวช และนักพรตในพระพุทธศาสนาซึ่งมีหนังสือสุทธิรับรอง")
                {
                    lbldepartment.Text = "76 UC ภิกษุ ผู้นำศาสนา ในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(77) ผู้มีอายุเกิน 60 ปีบริบูรณ์")
                {
                    lbldepartment.Text = "77 UC ผู้สูงอายุ ในเขต";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(81) ผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                {
                    lbldepartment.Text = "29 UC ปะเภทผู้นำชุมชน";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(87) บุคคลในครอบครัวของผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                {
                    lbldepartment.Text = "29 UC ปะเภทผู้นำชุมชน";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(82) อาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                {
                    lbldepartment.Text = "28 UC ประเภทอาสาสมัคร";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(88) บุคคลในครอบครัวของอาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                {
                    lbldepartment.Text = "28 UC ประเภทอาสาสมัคร";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(91) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(ราชทัณฑ์)")
                {
                    lbldepartment.Text = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                }
                else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(92) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(สถานพินิจและสถานสงเคราะห์)")
                {
                    lbldepartment.Text = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                }
                else if (data.mainInscl == "(DIS) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)" && data.subInscl == "(D1) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)")
                {
                    lbldepartment.Text = "K9 ผู้ประกันตนคนพิการ รพ.สค. (สปสช)";
                }
                else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S1) สิทธิเบิกกองทุนประกันสังคม (ผู้ประกันตน)")
                {
                    lbldepartment.Text = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                }
                else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S2) สิทธิเบิกประกันสังคม (เบิกส่วนต่างกรมบัญชีกลางได้เฉพาะกรณี)")
                {
                    lbldepartment.Text = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                }
                else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S3) สิทธิเบิกประกันสังคม (เบิกส่วนต่างจากอปท.ได้เฉพาะกรณี)")
                {
                    lbldepartment.Text = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                }
                else if (data.mainInscl == "(SSI) สิทธิประกันสังคมกรณีทุพพลภาพ" && data.subInscl == "(S6) สิทธิเบิกกองทุนประกันสังคม (ทุพพลภาพ)")
                {
                    lbldepartment.Text = "10 ชำระเงินเอง";
                }
                else
                {
                    lbldepartment.Text = " ";
                }
            }
            _pid = data.pid;
            _correlationId = data.correlationId;

            if (data.claimTypes != null)
            {
                var _ctype = data.claimTypes.FirstOrDefault();
                data_ctype = _ctype.claimType;
                data_clemdetail = _ctype.claimTypeName;
            }

             pictureBox1.Image = Base64ToImage(patients.pImage);

            // Json preparation before using PostCard

            var json_post = new sendData()
            {
                pid = data.pid,
                claimType = data_ctype,
                mobile = "034429333", // If `data.mobile` exists, else use a default or extract from elsewhere
                correlationId = data.correlationId
            };
            string url_post = "http://localhost:8189/api/nhso-service/confirm-save";

            // function post data to NHSO //
            // PostCard();
            await POSTDataCommit(json_post, url_post);

            // ✅ Auto-read again after 10 seconds
            await Task.Delay(3000); // 10-second delay
            await F_Load();
            // await GetCard(); // Loop again
        }


        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // MessageBox.Show(comboBox1.SelectedValue.ToString());
        }
        // private async Task bntSentData_ClickAsync(object sender, EventArgs e) // ====ของเดิม
        // {
        //     // if (txtMobile.Text == "")
        //     // {
        //     //     MessageBox.Show("กรุณากรอกหมายเลขโทรศัพท์", "Error ไม่สามารถทำรายการได้", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //     //     txtMobile.Focus();
        //     // }
        //     if (_correlationId == null)
        //     {
        //         MessageBox.Show("กรุณาตรวจสอบข้อมูล หรือ ติดต่อเจ้าหน้าที่", "Error ไม่สามารถทำรายการได้", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //     }
        //     else
        //     {
        //         string url = "http://localhost:8189/api/nhso-service/confirm-save";
        //         var s = new sendData();
        //         s.pid = _pid;
        //         // MessageBox.Show(s.pid);
        //         s.claimType = data_ctype;//comboBox1.SelectedValue.ToString();
        //                                  // MessageBox.Show(s.claimType);
        //         // s.mobile = txtMobile.Text;//_mobile;
        //                                   // MessageBox.Show(s.mobile);
        //         s.correlationId = _correlationId;
        //         //  MessageBox.Show(s.correlationId);

        //         //POST ==== DATA====
        //         var xpost = POSTDataCommit(s, url); // ขอเดิมใช้งานได้อยู่

        //         //  new posrdata ยังไม่ได้ test 
        //         List<RtClame> rtClames = new List<RtClame>();

        //         rtClames = await NewsentToClan(s.pid, s.claimType, s.mobile, s.correlationId);

        //         if (rtClames != null)
        //         {
        //             foreach (var item in rtClames)
        //             {
        //                 string cid = item.pid;
        //                 string a = item.claimCode;
        //                 string b = item.claimType;
        //                 string c = item.correlationId;
        //                 string d = item.createdDate;
        //             }
        //         }

        //         if (xpost == true)
        //         {
        //             cleatdata();
        //             s.pid = null;
        //             s.claimType = null;
        //             s.correlationId = null;
        //             _correlationId = null;
        //         }
        //         else
        //         {
        //             MessageBox.Show("error");
        //             cleatdata();
        //             s.pid = null;
        //             s.claimType = null;
        //             s.correlationId = null;
        //             _correlationId = null;
        //         }
        //         //===END===POST==DATA=====

        //         //var statuss = CommitData(s, url);
        //         // if(statuss.Status.ToString == "404")
        //         // {

        //         // }

        //     }
        // }

        public void cleatdata()
        {
            // txtMobile.Text = "";
            pictureBox1.Image = Image.FromFile(@"Resources\777.png");
            lblbirthDate.Text = "XX-XX-XXX";
            lblfname.Text = "XX XXXXX  XXXXX";
            // lblnation.Text = "XX";
            lblsex.Text = "XX";
            lblage.Text = "XX";
            //comboBox1.DataSource = null;
            //comboBox1.Text = "";
            //comboBox1.Items.Clear();
            _correlationId = "";
            _pid = "";

        }
        //public async void PostD()
        //{
        //    string url = "http://localhost:8189/api/nhso-service/confirm-save";

        //    HttpClient clientPost = new HttpClient();

        //    var s = new sentData();
        //    s.pid = _pid;
        //    MessageBox.Show(s.pid);
        //    s.claimType = comboBox1.SelectedValue.ToString();
        //    MessageBox.Show(s.claimType);
        //    s.mobile = txtMobile.Text;//_mobile;
        //    MessageBox.Show(s.mobile);
        //    s.correlationId = _correlationId;
        //    MessageBox.Show(s.correlationId);


        //    clientPost.BaseAddress = new Uri(url);
        //    var respont = clientPost.PostJsonAsync(s).Result;
        //}

        //  ของเดิมใช้งานได้อยู่ แต่ทดลองเอาออก
        public async Task<bool> POSTDataCommit(object json, string url)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = _httpClient.PostAsync(url, content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                else
                {

                    returnValueData = result.Content.ReadAsStringAsync().Result;
                    // throw new Exception($"Failed to POST data: ({result.StatusCode}): {returnValue}");
                    List<RtClame> rtClames = new List<RtClame>();


                    return false; // returnValue.ToList()); //returnValue.ToList());
                }
                // 092-2465732  พีพวนา  
                //090-1975166   พี่น้อย 
            }
        }

        //===== ของใหม่่

        public async Task<List<RtClame>> NewsentToClan(string _pid, string _claimType, string _mobile, string _correlationId)//(object json)
        {

            var param = new Dictionary<string, string>();

            param.Add("pid", _pid);
            param.Add("claimType", _claimType);
            param.Add("mobile", _mobile);
            param.Add("correlationId", _correlationId);

            //var obj = new sentData();
            //{
            //    obj.pid = "Markoff",
            //    obj.claimType = "Chaney",
            //    dateOfBirth = new MyDate
            //    //{
            //    //    year = 1901,
            //    //    month = 4,
            //    //    day = 30
            //    //}
            //};

            //var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            var content = new FormUrlEncodedContent(param);
            // var content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)json);

            var clientS = new HttpClient();
            clientS.BaseAddress = BaseAddress;
            var respont = await clientS.PostAsync("/api/nhso-service/confirm-save", content);

            if (respont.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsondata = respont.Content.ReadAsStringAsync();
                //  return JObject.Parse(await jsondata).ToObject<RtClame>();

                return JObject.Parse(await jsondata).ToObject<List<RtClame>>();
            }
            else
            {
                return null;
            }

        }



        //public async Task<JsonObject> PostAsync(string uri, string data)
        //{
        //    var httpClient = new HttpClient();
        //    response = await httpClient.PostAsync(uri, new StringContent(data));

        //    response.EnsureSuccessStatusCode();

        //    string content = await response.Content.ReadAsStringAsync();
        //    return await Task.Run(() => JsonObject.Parse(content));
        //}

        public static async Task<string> CommitData(object json, string url)
        {
            using var client = new HttpClient();
            var jsondata = JsonConvert.SerializeObject(json);
            var data = new StringContent(jsondata, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;
            return response.StatusCode.ToString();
        }
        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }
        private async void bntSentData_Click(object sender, EventArgs e)  // == แก้ใหม่
        {
            // if (txtMobile.Text == "")
            // {
            //     MessageBox.Show("กรุณากรอกหมายเลขโทรศัพท์", "Error ไม่สามารถทำรายการได้", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //     txtMobile.Focus();
            //     return;
            // }
            // else
            // {
                //==============================
                string url_checkcard = "http://localhost:8189/api/smartcard/terminals";
                //string url = "http://localhost:8189/api/smartcard/read?readImageFlag=false";
                string url1 = "http://localhost:8189/api/smartcard/read?readImageFlag=true";
                HttpClient client = new HttpClient();


                //checkcard=======
                string response_readcard = await client.GetStringAsync(url_checkcard);
                var datacard = JsonConvert.DeserializeObject<List<checkcard>>(response_readcard);
                //  var datacard = JsonConvert.DeserializeObject<checkcard>(response_readcard);

                bool tnamecard = true;
                foreach (var item in datacard)
                {
                    tnamecard = item.isPresent;
                }
                if (tnamecard == false)
                {
                    MessageBox.Show("ไม่พบบัตร !!! กรุณาเสียบบัตรประชาชน.", "Error ไม่สามารถทำรายการได้", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                else
                {
                    string response = await client.GetStringAsync(url1);
                    var data = JsonConvert.DeserializeObject<ciddata>(response);
                    string urlp = "http://172.16.200.202:8089/api/Hos/getpatienthnimage?_cid="+ data.pid;
                    string responsep = await client.GetStringAsync(urlp);
                    var patients = JsonConvert.DeserializeObject<ciddata>(responsep);
                    string urls = "http://172.16.200.202:8089/api/Hos/GetLatestOpdDepByCid?_cid="+ data.pid;
                    string responses = await client.GetStringAsync(urls);
                    var sit = JsonConvert.DeserializeObject<ciddata>(responses);
                    string Tname = patients.pname;
                    // string nextPttype = sit.nextPttype;
                    // string name = sit.name;
                    // string department = sit.department;

                    // switch (tname)
                    // {
                    //     case "003":
                    //         Tname = "นาย";
                    //         break;
                    //     case "004":
                    //         Tname = "นางสาว";
                    //         break;
                    //     default:
                    //         break;
                    // }

                    string tnation = data.nation;
                    switch (tnation)
                    {
                        case "099":
                            Tnation = "ไทย";
                            break;
                        default:
                            break;
                    }

                    lblfname.Text = Tname + " " + data.fname + "  " + data.lname;
                    // lblnation.Text = Tnation;//data.nation;

                    string dd = data.birthDate.Substring(6);
                    string mm = data.birthDate.Substring(4, 2);
                    string yy = data.birthDate.Substring(0, 4);
                    var thaiMonths = new Dictionary<string, string>
                        {
                            { "01", "มกราคม" },
                            { "02", "กุมภาพันธ์" },
                            { "03", "มีนาคม" },
                            { "04", "เมษายน" },
                            { "05", "พฤษภาคม" },
                            { "06", "มิถุนายน" },
                            { "07", "กรกฎาคม" },
                            { "08", "สิงหาคม" },
                            { "09", "กันยายน" },
                            { "10", "ตุลาคม" },
                            { "11", "พฤศจิกายน" },
                            { "12", "ธันวาคม" }
                        };

                    // แปลงเดือนเป็นชื่อภาษาไทย
                    string thaiMonth = thaiMonths.ContainsKey(mm) ? thaiMonths[mm] : "";

                    // แสดงวันเกิดรูปแบบ: 01 มกราคม 2567
                    lblbirthDate.Text = $"{dd} {thaiMonth} {yy}";
                    // lblbirthDate.Text = dd + "-" + mm + "-" + yy;//data.birthDate;
                    lblage.Text = data.age;
                    lblsex.Text = data.sex;
                    lblfcid.Text = data.pid;
                    lblhcode.Text = $"{data.hospMain.hcode} {data.hospMain.hname}";
                    lblfhn.Text =  patients.hn;
                    lblsubInscl.Text = data.subInscl;
                    lblcorrelationId.Text = data.correlationId;

                    _pid = data.pid;
                    _correlationId = data.correlationId;

                    // string ctype;

                    if (data.claimTypes != null)
                    {
                        var _ctype = data.claimTypes.FirstOrDefault();
                        //  ctype = _ctype.ToString();
                        data_ctype = _ctype.claimType.ToString();
                        data_clemdetail = _ctype.claimTypeName.ToString();
                    }


                    //=================
                    pictureBox1.Image = Base64ToImage(data.image);
                    //==============================
                    
                    string url = "http://localhost:8189/api/nhso-service/confirm-save";
                    var s = new sendData();
                    s.pid = _pid;
                    // MessageBox.Show(s.pid);
                    s.claimType = data_ctype;
                    // MessageBox.Show(s.claimType);
                    // s.mobile = txtMobile.Text;//_mobile;
                                              // MessageBox.Show(s.mobile);
                    s.correlationId = _correlationId;
                    //  MessageBox.Show(s.correlationId);

                    //POST ==== DATA====
                    var xpost = POSTDataCommit(s, url); // ขอเดิมใช้งานได้อยู่
                                                        //new posrdata ยังไม่ได้ test 
                                                        //  rtClames = NewsentToClan(_pid, data_ctype, txtMobile.Text, _correlationId); //NewsentToClan(Convert.ToString(s.pid), Convert.ToString(s.claimType), Convert.ToString(s.mobile), Convert.ToString(s.correlationId));
                                                        //  GGG();


                    if (returnValueData != null)
                    {
                        var yourdata = JsonConvert.DeserializeObject<RtClame>(returnValueData);

                        if (yourdata.pid != null)
                        {
                            //print

                            //CrystalReport1 cp = new CrystalReport1();
                            //cp.Parameter_clamNumber1 = yourdata.claimType;

                            //ReportViewer reportViewer = new ReportViewer
                            //{
                            //    ProcessingMode = ProcessingMode.Local,
                            //    SizeToReportContent = true
                            //};
                            ////C:\CARD-CID\NSHO-READ\NSHO-READ\Report1.rdlc
                            //reportViewer.LocalReport.ReportPath = Path.Combine("C:\\CARD-CID\\NSHO-READ\\NSHO-READ", "Report1.rdlc");
                           
                          //  return View();



                            MessageBox.Show(Convert.ToString(yourdata.pid));
                            MessageBox.Show(Convert.ToString(yourdata.claimType));
                            MessageBox.Show(Convert.ToString(yourdata.correlationId));
                            MessageBox.Show(Convert.ToString(yourdata.createdDate));
                            MessageBox.Show(Convert.ToString(yourdata.claimCode));

                            //printdata
                            MessageBox.Show("ส่งข้อมูลสำเร็จ");
                            cleatdata();
                        }
                        else 
                        {
                            MessageBox.Show("ไม่สามารถส่งข้อมูล:ได้:พบข้อมูลซ้ำ: มีการส่งข้อมูลไปแล้ว","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cleatdata();
                        }
                    }
                }
            // }
           
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
          lblDateTime.Text =  DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }
    }
}
