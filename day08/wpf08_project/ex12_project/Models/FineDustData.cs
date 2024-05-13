using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex12_project.Models
{
    internal class FineDustData
    {
        public int Id { get; set; } // 추가생성, DB에 넣을 때 사용할 값
        public string Place { get; set; } // 위치
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Humidity { get; set; } 
        public double Temperature { get; set; } 
        public double CO2 { get; set; } 
        public double PM10 { get; set; } // PM 10mm 미세먼지 측정값
        public double PM25 { get; set; } // PM 2.5mm 초미세먼지 측정값
        public double PM1 { get; set; }
        public double Coordx { get; set; } // 경도
        public double Coordy { get; set; } // 위도

        public DateTime Collection_Date { get; set; } 
        

        public static readonly string SELECT_QUERY = @"SELECT [Place]
                                                             ,[PM1]
                                                             ,[PM10]
                                                             ,[PM25]
                                                             ,[CO2]
                                                             ,[Temperature]
                                                             ,[Humidity]
                                                             ,[Latitude]
                                                             ,[Longitude]
                                                             ,[Collection_Date]
                                                             ,[Id]
                                                         FROM [dbo].[FineDustData]";

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[FineDustData]
                                                                        ([Place]
                                                                        ,[PM1]
                                                                        ,[PM10]
                                                                        ,[PM25]
                                                                        ,[CO2]
                                                                        ,[Temperature]
                                                                        ,[Humidity]
                                                                        ,[Latitude]
                                                                        ,[Longitude]
                                                                        ,[Collection_Date]
                                                                        ,[Id])
                                                                  VALUES
                                                                        (@Place
                                                                        ,@PM1
                                                                        ,@PM10
                                                                        ,@PM25
                                                                        ,@CO2
                                                                        ,@Temperature
                                                                        ,@Humidity
                                                                        ,@Latitude
                                                                        ,@Longitude
                                                                        ,@Collection_Date
                                                                        ,@Id)";

        public static readonly string GETDATE_QUERY = @"SELECT CONVERT(CHAR(10), Collection_Date, 23) AS Save_Date
                                                          FROM [dbo].[FineDustData]
                                                         GROUP BY CONVERT(CHAR(10), Collection_Date, 23)";
    }
}
