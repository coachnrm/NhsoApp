using System.Text;
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


        public async void GetCard()
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
            lblhcode.Text = $"{data.hospMain.hcode} {data.hospMain.hname}";

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

            Optional: pictureBox1.Image = Base64ToImage(patients.pImage);

            // ✅ Auto-read again after 10 seconds
            await Task.Delay(5000); // 10-second delay
            GetCard(); // Loop again
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
        private async Task bntSentData_ClickAsync(object sender, EventArgs e) // ====ของเดิม
        {
            // if (txtMobile.Text == "")
            // {
            //     MessageBox.Show("กรุณากรอกหมายเลขโทรศัพท์", "Error ไม่สามารถทำรายการได้", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //     txtMobile.Focus();
            // }
            if (_correlationId == null)
            {
                MessageBox.Show("กรุณาตรวจสอบข้อมูล หรือ ติดต่อเจ้าหน้าที่", "Error ไม่สามารถทำรายการได้", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string url = "http://localhost:8189/api/nhso-service/confirm-save";
                var s = new sendData();
                s.pid = _pid;
                // MessageBox.Show(s.pid);
                s.claimType = data_ctype;//comboBox1.SelectedValue.ToString();
                                         // MessageBox.Show(s.claimType);
                s.mobile = txtMobile.Text;//_mobile;
                                          // MessageBox.Show(s.mobile);
                s.correlationId = _correlationId;
                //  MessageBox.Show(s.correlationId);

                //POST ==== DATA====
                var xpost = POSTDataCommit(s, url); // ขอเดิมใช้งานได้อยู่

                //  new posrdata ยังไม่ได้ test 
                List<RtClame> rtClames = new List<RtClame>();

                rtClames = await NewsentToClan(s.pid, s.claimType, s.mobile, s.correlationId);

                if (rtClames != null)
                {
                    foreach (var item in rtClames)
                    {
                        string cid = item.pid;
                        string a = item.claimCode;
                        string b = item.claimType;
                        string c = item.correlationId;
                        string d = item.createdDate;
                    }
                }

                if (xpost == true)
                {
                    cleatdata();
                    s.pid = null;
                    s.claimType = null;
                    s.correlationId = null;
                    _correlationId = null;
                }
                else
                {
                    MessageBox.Show("error");
                    cleatdata();
                    s.pid = null;
                    s.claimType = null;
                    s.correlationId = null;
                    _correlationId = null;
                }
                //===END===POST==DATA=====

                //var statuss = CommitData(s, url);
                // if(statuss.Status.ToString == "404")
                // {

                // }

            }
        }

        public void cleatdata()
        {
            txtMobile.Text = "";
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
        public bool POSTDataCommit(object json, string url)
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
                    string Tname = sit.pname;
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
                    s.mobile = txtMobile.Text;//_mobile;
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
            //async void GGG()
            //{
               
            //    rtClames = await NewsentToClan(_pid, data_ctype, txtMobile.Text, _correlationId);
            //    if (rtClames != null)
            //    {
            //        foreach (var item in rtClames)
            //        {
            //            string cid = item.pid;
            //            string a = item.claimCode;
            //            string b = item.claimType;
            //            string c = item.correlationId;
            //            string d = item.createdDate;
            //        }
            //    }
            //}
            //======
            //private async Task CreateCompanyWithHttpRequestMessage()
            //{
            //    //sentData s = new sentData
            //    //{
            //    //    s.pid = "";
            //    //    //s.claimType = "USA",
            //    //    //s.mobile = "Hawk IT Street 365"
            //    //    //s.correlationId = 
            //    //};

            //    var s = new sentData();
            //    s.pid = "";
            //    s.claimType = "";
            //    s.mobile = "";
            //    s.correlationId = "";



            //    //var company = JsonSerializer.Serialize(companyForCreation);
            //    //var request = new HttpRequestMessage(HttpMethod.Post, "companies");
            //    //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    //request.Content = new StringContent(company, Encoding.UTF8);
            //    //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    //var response = await _httpClient.SendAsync(request);
            //    //response.EnsureSuccessStatusCode();
            //    //var content = await response.Content.ReadAsStringAsync();
            //    //var createdCompany = JsonSerializer.Deserialize<CompanyDto>(content, _options);
            //}
            ////======
       // }

        private void timer1_Tick(object sender, EventArgs e)
        {
          lblDateTime.Text =  DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }
    }
}
