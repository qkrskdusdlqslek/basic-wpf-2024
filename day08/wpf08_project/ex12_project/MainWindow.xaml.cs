using ex12_project.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ex12_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboDateFromDB();
        }

        private void InitComboDateFromDB()
        {
            using (SqlConnection conn = new SqlConnection (Helpers.Common.CONNSTRING))
            {
                conn.Open ();   
                SqlCommand cmd = new SqlCommand (Models.FineDustData.GETDATE_QUERY, conn);
                SqlDataAdapter adapter = new SqlDataAdapter (cmd);
                DataSet dSet = new DataSet();
                adapter.Fill (dSet);
                List<string> saveDates = new List<string>();

                foreach (DataRow row in dSet.Tables[0].Rows)
                {
                    saveDates.Add(Convert.ToString(row["Save Date"]));
                }

                CboReqTime.ItemsSource = saveDates;
            }
        }

        // 조회
        private async void BtnReqServiceCenter_Click(object sender, RoutedEventArgs e)
        {
            string openApiUri = "http://openapi.airgwangsan.kr:8080/Gwangsan/getDustDataAPI?apiId=01";
            string result = string.Empty;

            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync ();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd ();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
            }

            var jsonResult = JObject.Parse (result);
            var status = Convert.ToString(jsonResult["response"]["resultCode"]);

            if (status == "01")
            {
                var data = jsonResult["response"]["items"];
                var jsonArray = data as JArray;

                var fineDustDatas = new List<FineDustData>();
                foreach (var item in jsonArray)
                {
                    fineDustDatas.Add(new FineDustData()
                    {
                       Id = 0,
                       Place = Convert.ToString(item["place"]),
                       PM1 = Convert.ToDouble(item["PM1"]),
                       PM10 = Convert.ToDouble(item["PM10"]),
                       PM25 = Convert.ToDouble(item["PM2_5"]),
                       CO2 = Convert.ToDouble(item["CO2"]),
                       Temperature = Convert.ToDouble(item["TEMPERATURE"]),
                       Humidity = Convert.ToDouble(item["HUMIDITY"]),
                       Latitude = Convert.ToDouble(item["LATITUDE"]),
                       Longitude = Convert.ToDouble(item["LONGITUDE"]),
                       Collection_Date = Convert.ToDateTime(item["COLLECTION_DATE"]),

                    });
                }

                this.DataContext = fineDustDatas;
                StsResult.Content = $"OpenAPI {fineDustDatas.Count}건 조회완료";
               
                
            }
        }

        private async void BtnSaveData_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.Items.Count == 0)
            {
                await this.ShowMessageAsync("저장오류", "실시간 조회후 저장하십시오.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var insRes = 0;
                    foreach (FineDustData item in GrdResult.Items)
                    {
                        SqlCommand cmd = new SqlCommand(Models.FineDustData.INSERT_QUERY, conn);
                        cmd.Parameters.AddWithValue("@Place", item.Place);
                        cmd.Parameters.AddWithValue("@PM1", item.PM1);
                        cmd.Parameters.AddWithValue("@PM10", item.PM10);
                        cmd.Parameters.AddWithValue("@PM25", item.PM25);
                        cmd.Parameters.AddWithValue("@CO2", item.CO2);
                        cmd.Parameters.AddWithValue("@Temperature", item.Temperature);
                        cmd.Parameters.AddWithValue("@Humidity", item.Humidity);
                        cmd.Parameters.AddWithValue("@Latitude", item.Latitude);
                        cmd.Parameters.AddWithValue("@Longitude", item.Longitude);
                        cmd.Parameters.AddWithValue("@Collection_Date", item.Collection_Date);

                        insRes += cmd.ExecuteNonQuery();
                    }

                    if (insRes > 0)
                    {
                        await this.ShowMessageAsync("저장", "DB저장성공");
                        StsResult.Content = $"DB저장 {insRes}건 성공";
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("저장오류", $"저장오류 {ex.Message}");
            }
            InitComboDateFromDB();
        }

        private void CboReqTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var curItem = GrdResult.SelectedItem as FineDustData; 
        }

        private void CboReqLocal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}