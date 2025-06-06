using System.Drawing.Printing;
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
        PrintDocument printDocument = new PrintDocument();
        
        // 🟢 ตัวแปรระดับคลาส
        private string patientFullName = "ไม่พบข้อมูลผู้ป่วย";
        private string patientFullhn = "ไม่พบข้อมูลผู้ป่วย";
        private string patientFullpid = "ไม่พบข้อมูลผู้ป่วย";
        private string patientsmsk = "ไม่พบข้อมูลผู้ป่วย";
        private string patientdate = "ไม่พบข้อมูลผู้ป่วย";
        private string ment = "ไม่พบข้อมูลผู้ป่วย";
        private string patientFullClaim = "ไม่พบข้อมูลผู้ป่วย";
        private string patientFullsit = "ไม่พบข้อมูลผู้ป่วย";
        private string patientFullh = "ไม่พบข้อมูลผู้ป่วย";
        private string patientFullsub = "ไม่พบข้อมูลผู้ป่วย";
        private string hcode  = "ไม่พบข้อมูลผู้ป่วย";
        private Font printFont = new Font("Arial", 12);
        private Brush brush = Brushes.Black;

        private static Uri BaseAddress { get; set; } = new Uri("http://localhost:8189");

        private ThaiIDCard idcard;
        // List<ciddata> ciddatas = new List<ciddata>();

        public Form1()
        {
            InitializeComponent();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
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
            ment = "XXXXXXXX";
            // pictureBox1.Image = Image.FromFile(@"../Resources/888.png");
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
            ment = "XXXXXXXX";
            pictureBox1.Image = Image.FromFile(@"../Resources/888.png");
            label8.Show();
            await GetCard();
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
            string Tname = patients.pname ?? "";
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
            lblfhn.Text = patients.hn ?? "";
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
            await btnPrint_Click();

            // ✅ Auto-read again after 10 seconds
            await Task.Delay(5000); // 10-second delay
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


        public void cleatdata()
        {
            // txtMobile.Text = "";
            pictureBox1.Image = Image.FromFile(@"../Resources/888.png");
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
                string urlp = "http://172.16.200.202:8089/api/Hos/getpatienthnimage?_cid=" + data.pid;
                string responsep = await client.GetStringAsync(urlp);
                var patients = JsonConvert.DeserializeObject<ciddata>(responsep);
                string urls = "http://172.16.200.202:8089/api/Hos/GetLatestOpdDepByCid?_cid=" + data.pid;
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
                lblfhn.Text = patients.hn;
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
                        MessageBox.Show("ไม่สามารถส่งข้อมูล:ได้:พบข้อมูลซ้ำ: มีการส่งข้อมูลไปแล้ว", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cleatdata();
                    }
                }
            }
            // }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }
        // private void PrintNow()
        // {
        //     printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

        //     // แสดง Dialog ให้ผู้ใช้เลือกเครื่องพิมพ์ (ถ้าไม่ต้องการ dialog ให้ใช้ printDocument.Print() เลย)
        //     PrintDialog printDialog = new PrintDialog();
        //     printDialog.Document = printDocument;

        //     if (printDialog.ShowDialog() == DialogResult.OK)
        //     {
        //         printDocument.Print();
        //     }
        // }
        // private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        // {
        //     // ตั้งค่าฟอนต์
        //     Font printFont = new Font("Arial", 12);
        //     Brush brush = Brushes.Black;

        //     // ข้อความที่จะพิมพ์
        //     string printText = "นี่คือข้อความที่พิมพ์จาก WinForms!";

        //     // ตำแหน่งที่พิมพ์ (X, Y)
        //     e.Graphics.DrawString(printText, printFont, brush, new PointF(20, 5));
        // }
        // private void btnPrint_Click(object sender, EventArgs e)
        // {
        //     PrintNow();
        // }


        private async Task LoadCardDataAsync()
        {
            string url_checkcard = "http://localhost:8189/api/smartcard/terminals";
            string url = "http://localhost:8189/api/smartcard/read?readImageFlag=false";

            try
            {
                HttpClient client = new HttpClient();

                // ตรวจสอบว่ามีบัตรเสียบอยู่ไหม
                string response_check = await client.GetStringAsync(url_checkcard);
                var datacard = JsonConvert.DeserializeObject<List<checkcard>>(response_check);

                bool isCardPresent = datacard?.Any(x => x.isPresent) ?? false;

                if (!isCardPresent)
                {
                    patientFullName = "ไม่พบบัตร กรุณาเสียบบัตร";
                    return;
                }

                // อ่านข้อมูลจากบัตร
                string response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<ciddata>(response);
                string urlp = "http://172.16.200.202:8089/api/Hos/getpatienthnimage?_cid=" + data.pid;
                string responsep = await client.GetStringAsync(urlp);
                var patients = JsonConvert.DeserializeObject<ciddata>(responsep);

                string title = patients.pname ?? "";
                string fname = data.fname ?? "";
                string lname = data.lname ?? "";
                string hn = patients.hn;
                string pid = data.pid;
                string subInscl = data.subInscl;
                string datetime = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                 if (data.hospMainOp == null || string.IsNullOrWhiteSpace(data.hospMainOp.hcode))
                {
                    if (data.hospMain == null || string.IsNullOrWhiteSpace(data.hospMain.hcode))
                    {
                        hcode = "10734 รพ.สมุทรสาคร";
                    }
                    else
                    {
                        hcode = $"{data.hospMain.hcode ?? ""} {data.hospMain.hname ?? ""}";
                        // document.getElementById('hospMain').value = "99999";
                    }
                }
                else
                {
                    hcode = $"{data.hospMainOp.hcode ?? ""} {data.hospMainOp.hname ?? ""}";
                }
                if (data.hospMain != null)// || string.IsNullOrWhiteSpace(data.hospMain.hcode))
                {
                    if (data.hospMain.hcode == "11304" || data.hospMain.hcode == "11305")
                    {
                        if (data.mainInscl == "(UCS) สิทธิหลักประกันสุขภาพแห่งชาติ" && data.subInscl == "(89) ช่วงอายุ 12-59 ปี")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(71) เด็กอายุไม่เกิน 12 ปีบริบูรณ์")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P1) สิทธิครูเอกชน")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P2) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจากกรมบัญชีกลาง)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P3) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจาก อปท.)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E1) สิทธิเบิกหน่วยงานรัฐ (ตนเอง)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E2) สิทธิเบิกหน่วยงานรัฐ (บุคคลในครอบครัว)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E3) สิทธิเบิกหน่วยงานรัฐ (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G1) หน่วยงานรัฐอื่นๆ")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B6) สิทธิเบิกบุคคลในครอบครัวลูกจ้างชั่วคราวกรุงเทพมหานคร (เบิกใบเสร็จ/หนังสือรับรองสิทธิ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O1) สิทธิเบิกกรมบัญชีกลาง (ข้าราชการ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O2) สิทธิเบิกกรมบัญชีกลาง (ลูกจ้างประจำ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O3) สิทธิเบิกกรมบัญชีกลาง (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O4) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัว)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O5) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B1) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ข้าราชการ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B2) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ลูกจ้างประจำ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B3) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B4) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัว)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B5) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C1) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (เจ้าหน้าที่)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C2) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (พนักงาน)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C3) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C4) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัว)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C5) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C6) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (กรณีได้รับสิทธิเฉพาะหน่วยงาน)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G5) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (ตนเอง)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G6) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (บุคคลในครอบครัว)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L1) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ตนเอง)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L2) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัว)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L3) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L4) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L5) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ข้าราชการการเมือง)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L6) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวข้าราชการการเมือง)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L9) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ยังไม่ระบุตำแหน่ง)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(68) สมาชิกผู้บริจาคโลหิตของสภากาชาดไทย ซึ่งบริจาคโลหิตตั้งแต่ 18 ครั้ง ขึ้นไป")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(72) ผู้มีรายได้น้อย")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(73) นักเรียนมัธยมศึกษาตอนต้น")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(74) คนพิการ")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(66) ผู้ได้รับพระราชทานเหรียญราชการชายแดน")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(67) ผู้ได้รับพระราชทานเหรียญพิทักษ์เสรีชน")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(75) ทหารผ่านศึกชั้น 1-3 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(80) บุคคลในครอบครัวทหารผ่านศึกชั้น 1-3 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(85) ผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(86) บุคคลในครอบครัวของผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(90) ทหารเกณฑ์")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(93) นักเรียนทหาร")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(94) ทหารผ่านศึกชั้น 4 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(95) บุคคลในครอบครัวทหารผ่านศึกชั้น 4 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(96) ทหารพราน")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(97) บุคคลในครอบครัวทหารของกรมสวัสดิการ 3 เหล่าทัพ")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(98) บุคคลในครอบครัวทหารผ่านศึกนอกประจำการบัตรชั้นที่ 1")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(76) พระภิกษุ สามเณร แม่ชี นักบวช และนักพรตในพระพุทธศาสนาซึ่งมีหนังสือสุทธิรับรอง")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(77) ผู้มีอายุเกิน 60 ปีบริบูรณ์")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(81) ผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(87) บุคคลในครอบครัวของผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(82) อาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(88) บุคคลในครอบครัวของอาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(91) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(ราชทัณฑ์)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(92) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(สถานพินิจและสถานสงเคราะห์)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(DIS) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)" && data.subInscl == "(D1) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S1) สิทธิเบิกกองทุนประกันสังคม (ผู้ประกันตน)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S2) สิทธิเบิกประกันสังคม (เบิกส่วนต่างกรมบัญชีกลางได้เฉพาะกรณี)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S3) สิทธิเบิกประกันสังคม (เบิกส่วนต่างจากอปท.ได้เฉพาะกรณี)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else if (data.mainInscl == "(SSI) สิทธิประกันสังคมกรณีทุพพลภาพ" && data.subInscl == "(S6) สิทธิเบิกกองทุนประกันสังคม (ทุพพลภาพ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else
                        {
                            ment = " ";
                        }
                    }
                    else
                    {
                        // ment = "ffffff";
                        if (data.mainInscl == "(UCS) สิทธิหลักประกันสุขภาพแห่งชาติ" && data.subInscl == "(89) ช่วงอายุ 12-59 ปี")
                        {
                            ment = "11 UC เลือกรพ.สมุทรสาคร";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(71) เด็กอายุไม่เกิน 12 ปีบริบูรณ์")
                        {
                            ment = "71 UC เด็กต่ำกว่า 12 ปี ในเขต";
                        }
                        else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P1) สิทธิครูเอกชน")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P2) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจากกรมบัญชีกลาง)")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P3) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจาก อปท.)")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E1) สิทธิเบิกหน่วยงานรัฐ (ตนเอง)")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E2) สิทธิเบิกหน่วยงานรัฐ (บุคคลในครอบครัว)")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E3) สิทธิเบิกหน่วยงานรัฐ (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G1) หน่วยงานรัฐอื่นๆ")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B6) สิทธิเบิกบุคคลในครอบครัวลูกจ้างชั่วคราวกรุงเทพมหานคร (เบิกใบเสร็จ/หนังสือรับรองสิทธิ)")
                        {
                            ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O1) สิทธิเบิกกรมบัญชีกลาง (ข้าราชการ)")
                        {
                            ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O2) สิทธิเบิกกรมบัญชีกลาง (ลูกจ้างประจำ)")
                        {
                            ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O3) สิทธิเบิกกรมบัญชีกลาง (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O4) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัว)")
                        {
                            ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O5) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B1) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ข้าราชการ)")
                        {
                            ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B2) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ลูกจ้างประจำ)")
                        {
                            ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B3) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B4) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัว)")
                        {
                            ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B5) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C1) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (เจ้าหน้าที่)")
                        {
                            ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C2) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (พนักงาน)")
                        {
                            ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C3) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C4) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัว)")
                        {
                            ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C5) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C6) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (กรณีได้รับสิทธิเฉพาะหน่วยงาน)")
                        {
                            ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G5) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (ตนเอง)")
                        {
                            ment = "S7 79องค์การขนส่งมวลชนกรุงเทพ (ตนเอง)";
                        }
                        else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G6) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (บุคคลในครอบครัว)")
                        {
                            ment = "S8 80องค์การขนส่งมวลชนกรุงเทพ (ญาติ)";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L1) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ตนเอง)")
                        {
                            ment = "24 24ข้าราชการ อปท.";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L2) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัว)")
                        {
                            ment = "24 24ข้าราชการ อปท.";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L3) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "24 24ข้าราชการ อปท.";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L4) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                        {
                            ment = "24 24ข้าราชการ อปท.";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L5) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ข้าราชการการเมือง)")
                        {
                            ment = "24 24ข้าราชการ อปท.";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L6) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวข้าราชการการเมือง)")
                        {
                            ment = "24 24ข้าราชการ อปท.";
                        }
                        else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L9) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ยังไม่ระบุตำแหน่ง)")
                        {
                            ment = "24 24ข้าราชการ อปท.";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(68) สมาชิกผู้บริจาคโลหิตของสภากาชาดไทย ซึ่งบริจาคโลหิตตั้งแต่ 18 ครั้ง ขึ้นไป")
                        {
                            ment = "33 บริจาคโลหิต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(72) ผู้มีรายได้น้อย")
                        {
                            ment = "72 UC ผู้มีรายได้น้อยในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(73) นักเรียนมัธยมศึกษาตอนต้น")
                        {
                            ment = "73 UC นักเรียนในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(74) คนพิการ")
                        {
                            ment = "74 UC ผู้พิการในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(66) ผู้ได้รับพระราชทานเหรียญราชการชายแดน")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(67) ผู้ได้รับพระราชทานเหรียญพิทักษ์เสรีชน")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(75) ทหารผ่านศึกชั้น 1-3 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(80) บุคคลในครอบครัวทหารผ่านศึกชั้น 1-3 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(85) ผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(86) บุคคลในครอบครัวของผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(90) ทหารเกณฑ์")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(93) นักเรียนทหาร")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(94) ทหารผ่านศึกชั้น 4 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(95) บุคคลในครอบครัวทหารผ่านศึกชั้น 4 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(96) ทหารพราน")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(97) บุคคลในครอบครัวทหารของกรมสวัสดิการ 3 เหล่าทัพ")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(98) บุคคลในครอบครัวทหารผ่านศึกนอกประจำการบัตรชั้นที่ 1")
                        {
                            ment = "75 UC ทหารผ่านศึกในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(76) พระภิกษุ สามเณร แม่ชี นักบวช และนักพรตในพระพุทธศาสนาซึ่งมีหนังสือสุทธิรับรอง")
                        {
                            ment = "76 UC ภิกษุ ผู้นำศาสนา ในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(77) ผู้มีอายุเกิน 60 ปีบริบูรณ์")
                        {
                            ment = "77 UC ผู้สูงอายุ ในเขต";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(81) ผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                        {
                            ment = "29 UC ปะเภทผู้นำชุมชน";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(87) บุคคลในครอบครัวของผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                        {
                            ment = "29 UC ปะเภทผู้นำชุมชน";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(82) อาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                        {
                            ment = "28 UC ประเภทอาสาสมัคร";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(88) บุคคลในครอบครัวของอาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                        {
                            ment = "28 UC ประเภทอาสาสมัคร";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(91) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(ราชทัณฑ์)")
                        {
                            ment = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                        }
                        else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(92) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(สถานพินิจและสถานสงเคราะห์)")
                        {
                            ment = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                        }
                        else if (data.mainInscl == "(DIS) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)" && data.subInscl == "(D1) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)")
                        {
                            ment = "K9 ผู้ประกันตนคนพิการ รพ.สค. (สปสช)";
                        }
                        else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S1) สิทธิเบิกกองทุนประกันสังคม (ผู้ประกันตน)")
                        {
                            ment = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                        }
                        else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S2) สิทธิเบิกประกันสังคม (เบิกส่วนต่างกรมบัญชีกลางได้เฉพาะกรณี)")
                        {
                            ment = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                        }
                        else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S3) สิทธิเบิกประกันสังคม (เบิกส่วนต่างจากอปท.ได้เฉพาะกรณี)")
                        {
                            ment = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                        }
                        else if (data.mainInscl == "(SSI) สิทธิประกันสังคมกรณีทุพพลภาพ" && data.subInscl == "(S6) สิทธิเบิกกองทุนประกันสังคม (ทุพพลภาพ)")
                        {
                            ment = "10 ชำระเงินเอง";
                        }
                        else
                        {
                            ment = " ";
                        }
                    }
                }
                else
                {
                    // ment = "ffffff";
                    if (data.mainInscl == "(UCS) สิทธิหลักประกันสุขภาพแห่งชาติ" && data.subInscl == "(89) ช่วงอายุ 12-59 ปี")
                    {
                        ment = "11 UC เลือกรพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(71) เด็กอายุไม่เกิน 12 ปีบริบูรณ์")
                    {
                        ment = "71 UC เด็กต่ำกว่า 12 ปี ในเขต";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P1) สิทธิครูเอกชน")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P2) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจากกรมบัญชีกลาง)")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(PVT) สิทธิครูเอกชน" && data.subInscl == "(P3) สิทธิครูเอกชน(เบิกส่วนเกินหนึ่งแสนบาทจาก อปท.)")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E1) สิทธิเบิกหน่วยงานรัฐ (ตนเอง)")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E2) สิทธิเบิกหน่วยงานรัฐ (บุคคลในครอบครัว)")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(E3) สิทธิเบิกหน่วยงานรัฐ (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G1) หน่วยงานรัฐอื่นๆ")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B6) สิทธิเบิกบุคคลในครอบครัวลูกจ้างชั่วคราวกรุงเทพมหานคร (เบิกใบเสร็จ/หนังสือรับรองสิทธิ)")
                    {
                        ment = "20 20สิทธิเบิกจากหน่วยงานต้นสังกัด";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O1) สิทธิเบิกกรมบัญชีกลาง (ข้าราชการ)")
                    {
                        ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O2) สิทธิเบิกกรมบัญชีกลาง (ลูกจ้างประจำ)")
                    {
                        ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O3) สิทธิเบิกกรมบัญชีกลาง (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O4) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัว)")
                    {
                        ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(O5) สิทธิเบิกกรมบัญชีกลาง (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "23 23ข้าราชการขึ้นทะเบียนกระทรวงการคลัง";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B1) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ข้าราชการ)")
                    {
                        ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B2) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ลูกจ้างประจำ)")
                    {
                        ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B3) สิทธิเบิกข้าราชการกรุงเทพมหานคร (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B4) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัว)")
                    {
                        ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(B5) สิทธิเบิกข้าราชการกรุงเทพมหานคร (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "O3 76ข้าราชการเขต ก.ท.ม.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C1) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (เจ้าหน้าที่)")
                    {
                        ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C2) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (พนักงาน)")
                    {
                        ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C3) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C4) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัว)")
                    {
                        ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C5) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(C6) สิทธิเบิกหน่วยงานรัฐหรือรัฐวิสาหกิจ (กรณีได้รับสิทธิเฉพาะหน่วยงาน)")
                    {
                        ment = "P2 78ข้าราชการขึ้นทะเบียน กกต.";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G5) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (ตนเอง)")
                    {
                        ment = "S7 79องค์การขนส่งมวลชนกรุงเทพ (ตนเอง)";
                    }
                    else if (data.mainInscl == "(OFC) สิทธิข้าราชการ/สิทธิหน่วยงานรัฐ" && data.subInscl == "(G6) สิทธิเบิกองค์การขนส่งมวลชนกรุงเทพ (บุคคลในครอบครัว)")
                    {
                        ment = "S8 80องค์การขนส่งมวลชนกรุงเทพ (ญาติ)";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L1) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ตนเอง)")
                    {
                        ment = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L2) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัว)")
                    {
                        ment = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L3) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L4) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวผู้รับเบี้ยหวัดบำนาญ)")
                    {
                        ment = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L5) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ข้าราชการการเมือง)")
                    {
                        ment = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L6) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (บุคคลในครอบครัวข้าราชการการเมือง)")
                    {
                        ment = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(LGO) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น" && data.subInscl == "(L9) สิทธิสวัสดิการพนักงานส่วนท้องถิ่น (ยังไม่ระบุตำแหน่ง)")
                    {
                        ment = "24 24ข้าราชการ อปท.";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(68) สมาชิกผู้บริจาคโลหิตของสภากาชาดไทย ซึ่งบริจาคโลหิตตั้งแต่ 18 ครั้ง ขึ้นไป")
                    {
                        ment = "33 บริจาคโลหิต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(72) ผู้มีรายได้น้อย")
                    {
                        ment = "72 UC ผู้มีรายได้น้อยในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(73) นักเรียนมัธยมศึกษาตอนต้น")
                    {
                        ment = "73 UC นักเรียนในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(74) คนพิการ")
                    {
                        ment = "74 UC ผู้พิการในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(66) ผู้ได้รับพระราชทานเหรียญราชการชายแดน")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(67) ผู้ได้รับพระราชทานเหรียญพิทักษ์เสรีชน")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(75) ทหารผ่านศึกชั้น 1-3 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(80) บุคคลในครอบครัวทหารผ่านศึกชั้น 1-3 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(85) ผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(86) บุคคลในครอบครัวของผู้ได้รับพระราชทานเหรียญงานพระราชสงครามในทวีปยุโรป")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(90) ทหารเกณฑ์")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(93) นักเรียนทหาร")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(94) ทหารผ่านศึกชั้น 4 ที่มีบัตรทหารผ่านศึก รวมถึงผู้ได้รับพระราชทานเหรียญชัยสมรภูมิ")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(95) บุคคลในครอบครัวทหารผ่านศึกชั้น 4 รวมถึงผู้ได้รับพระราชทานเหรียญสมรภูมิ")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(96) ทหารพราน")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(97) บุคคลในครอบครัวทหารของกรมสวัสดิการ 3 เหล่าทัพ")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(98) บุคคลในครอบครัวทหารผ่านศึกนอกประจำการบัตรชั้นที่ 1")
                    {
                        ment = "75 UC ทหารผ่านศึกในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(76) พระภิกษุ สามเณร แม่ชี นักบวช และนักพรตในพระพุทธศาสนาซึ่งมีหนังสือสุทธิรับรอง")
                    {
                        ment = "76 UC ภิกษุ ผู้นำศาสนา ในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(77) ผู้มีอายุเกิน 60 ปีบริบูรณ์")
                    {
                        ment = "77 UC ผู้สูงอายุ ในเขต";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(81) ผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                    {
                        ment = "29 UC ปะเภทผู้นำชุมชน";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(87) บุคคลในครอบครัวของผู้นำชุมชน (กำนัน สารวัตรกำนัน ผู้ใหญ่บ้าน ผู้ช่วยผู้ใหญ่บ้านและแพทย์ประจำตำบล)")
                    {
                        ment = "29 UC ปะเภทผู้นำชุมชน";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(82) อาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                    {
                        ment = "28 UC ประเภทอาสาสมัคร";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(88) บุคคลในครอบครัวของอาสาสมัครสาธารณสุขประจำหมู่บ้าน (อสม.) อาสาสมัครสาธารณสุขกรุงเทพมหานคร")
                    {
                        ment = "28 UC ประเภทอาสาสมัคร";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(91) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(ราชทัณฑ์)")
                    {
                        ment = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                    }
                    else if (data.mainInscl == "(WEL) สิทธิหลักประกันสุขภาพแห่งชาติ (ยกเว้นการร่วมจ่ายค่าบริการ 30 บาท)" && data.subInscl == "(92) ผู้ที่พำนักในสถานที่ภายใต้การดูแลของส่วนราชการ(สถานพินิจและสถานสงเคราะห์)")
                    {
                        ment = "79 ผู้พำนักภายใต้การดูแลของส่วนราชการ";
                    }
                    else if (data.mainInscl == "(DIS) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)" && data.subInscl == "(D1) สิทธิหลักประกันสุขภาพแห่งชาติ (ผู้ประกันตนคนพิการ)")
                    {
                        ment = "K9 ผู้ประกันตนคนพิการ รพ.สค. (สปสช)";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S1) สิทธิเบิกกองทุนประกันสังคม (ผู้ประกันตน)")
                    {
                        ment = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S2) สิทธิเบิกประกันสังคม (เบิกส่วนต่างกรมบัญชีกลางได้เฉพาะกรณี)")
                    {
                        ment = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(SSS) สิทธิประกันสังคม" && data.subInscl == "(S3) สิทธิเบิกประกันสังคม (เบิกส่วนต่างจากอปท.ได้เฉพาะกรณี)")
                    {
                        ment = "34 ประกันสังคมเลือก รพ.สมุทรสาคร";
                    }
                    else if (data.mainInscl == "(SSI) สิทธิประกันสังคมกรณีทุพพลภาพ" && data.subInscl == "(S6) สิทธิเบิกกองทุนประกันสังคม (ทุพพลภาพ)")
                    {
                        ment = "10 ชำระเงินเอง";
                    }
                    else
                    {
                        ment = " ";
                    }
                }

                patientsmsk = $"       โรงพยาบาลสมุทรสาคร";
                patientdate = $"วันที่ : {datetime}";
                patientFullpid = $"เลขบัตรประชาชน : {pid}";
                patientFullhn = $"HN : {hn}";
                patientFullName = $"ชื่อ-สกุล : {title} {fname} {lname}";
                patientFullClaim = $"ClaimCode : ";
                patientFullsit = $"สิทธิ : {subInscl}";
                patientFullh = $"{hcode}";
                patientFullsub = $"สิทธิส่งตรวจ : {ment}";
            }
            catch (Exception ex)
            {
                patientFullName = "เกิดข้อผิดพลาด: " + ex.Message;
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font printFont = new Font("Arial", 12);
            Brush brush = Brushes.Black;

            // พิมพ์ชื่อผู้ป่วย
            e.Graphics.DrawString(patientsmsk, printFont, brush, new PointF(20, 0));
            e.Graphics.DrawString(patientdate, printFont, brush, new PointF(20, 20));
            e.Graphics.DrawString(patientFullhn, printFont, brush, new PointF(20, 40));
            e.Graphics.DrawString(patientFullpid, printFont, brush, new PointF(20, 60));
            e.Graphics.DrawString(patientFullName, printFont, brush, new PointF(20, 80));
            e.Graphics.DrawString(patientFullClaim, printFont, brush, new PointF(20, 100));
            e.Graphics.DrawString(patientFullsit, printFont, brush, new PointF(20, 120));
            e.Graphics.DrawString(patientFullh, printFont, brush, new PointF(20, 140));
            e.Graphics.DrawString(patientFullsub, printFont, brush, new PointF(20, 160));
        }   

        private async Task btnPrint_Click()
        {
            await LoadCardDataAsync(); // ดึงข้อมูลจาก API
            await PrintNow();                // จากนั้นพิมพ์
        }

        private async Task PrintNow()
        {
            // PrintDialog printDialog = new PrintDialog();
            // printDialog.Document = printDocument;

            printDocument.Print();

            // if (printDialog.ShowDialog() == DialogResult.OK)
            // {
            //     printDocument.Print();
            // }
        }


    }
}
