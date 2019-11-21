
using Excel;
using GolfCentra.Core;
using GolfCentra.Core.DataBase;
using GolfCentra.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;

namespace GolfCentra.Controllers
{
    public class ExeclUploadController : ApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public ExeclUploadController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpPost]
        [ResponseType(typeof(FileUpload))]
        public List<GolferViewModel> UploadExcel()
        {

            try
            {
                int num1 = 0;
                string path1 = HostingEnvironment.MapPath("~/Uploads/UploadExcel/");
                if (!Directory.Exists(HostingEnvironment.MapPath("~/Uploads/UploadExcel/")))
                    Directory.CreateDirectory(HostingEnvironment.MapPath("~/Uploads/UploadExcel/"));
                HttpFileCollection files = HttpContext.Current.Request.Files;
                int index = 0;
                if (index > files.Count - 1)
                    return new List<GolferViewModel>() { new GolferViewModel() { Message = "Failed" } };
                HttpPostedFile httpPostedFile = files[index];
                if (httpPostedFile.ContentLength > 0 && !File.Exists(path1 + Path.GetFileName(httpPostedFile.FileName)))
                {
                    string extension = Path.GetExtension(httpPostedFile.FileName);
                    if (!new List<string>() { ".xls", ".xlsx" }.Contains(extension))
                    {
                        string str = string.Format("Please Upload file of type .xlsx,.xls");
                        return new List<GolferViewModel>() { new GolferViewModel() { Message = "Failed" } };
                    }
                    string str1 = Guid.NewGuid().ToString();
                    string filename = Path.Combine(path1, Path.GetFileName(str1 + extension));
                    httpPostedFile.SaveAs(filename);
                    int num2 = num1 + 1;
                    Path.GetFileName(str1 + extension);
                    List<GolferViewModel> golferViewModels = ImportGolfer(Path.GetFileName(str1 + extension));
                    return golferViewModels;
                }

                return new List<GolferViewModel>() { new GolferViewModel() { Message = "Failed" } };
            }
            catch (Exception ex)
            {


                return new List<GolferViewModel>() { new GolferViewModel() { Message = "Failed" } };
            }
        }


        public List<GolferViewModel> ImportGolfer(string name)
        {
            try
            {
                List<GolferViewModel> golferViewModels = new List<GolferViewModel>();
                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/UploadExcel/" + Path.GetFileName(name)));
                FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader iexcelDataReader = (IExcelDataReader)null;
                DataSet dataSet1 = new DataSet();
                DataSet dataSet2;
                try
                {
                    if (path.EndsWith(".xls"))
                    {
                        iexcelDataReader = ExcelReaderFactory.CreateBinaryReader((Stream)fileStream);
                        iexcelDataReader.IsFirstRowAsColumnNames = true;
                    }
                    if (path.EndsWith(".xlsx"))
                    {
                        iexcelDataReader = ExcelReaderFactory.CreateOpenXmlReader((Stream)fileStream);
                        iexcelDataReader.IsFirstRowAsColumnNames = true;
                    }
                    dataSet2 = iexcelDataReader.AsDataSet();
                    ((IDataReader)iexcelDataReader).Close();
                }
                catch (Exception ex)
                {
                    this.Dispose();
                    return new List<GolferViewModel>() { new GolferViewModel() { Message = "Failed" } };
                }
                DataTable table = dataSet2.Tables[0];

                int count1 = table.Rows.Count;
                int num1 = 0;
                int count2 = table.Rows.Count;
                int num2 = 0;
                int num3 = 0;
                StringBuilder stringBuilder1 = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                {
                    int num4 = table.Rows.IndexOf(row) + 2;
                    GolferViewModel reporter = this.GetReporter(row);
                    num1 = table.Rows.IndexOf(row) + 1;
                    if (!reporter.IsError)
                    {
                        //Success data
                        Golfer golfer = new Golfer()
                        {
                            Name = reporter.Name,
                            Password = reporter.Password,
                            Email = reporter.EmailAddress,
                            Mobile = reporter.Mobile != null ? reporter.Mobile : "",
                            IsActive = true,
                            CreatedOn = System.DateTime.UtcNow,
                            PlatformTypeId = (int)Core.Helper.Enum.EnumPlatformType.Web,
                            MemberTypeId = 1,
                            ClubMemberId = reporter.ClubMemberId
                        };
                        _unitOfWork.GolferRepository.Insert(golfer);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        //Failed Data
                        golferViewModels.Add(reporter);
                    }
                }
                return golferViewModels;
            }
            catch (Exception ex)
            {
                return new List<GolferViewModel>() { new GolferViewModel() { Message = "Failed" } };
            }
        }



        private GolferViewModel GetReporter(DataRow row)
        {
            GolferViewModel golferViewModel = new GolferViewModel();
            golferViewModel.ImportGolferExcelReportModel = new ImportGolferExcelReportModel();
            try
            {

                try
                {
                    golferViewModel.Name = Convert.ToString(row["Name"]);
                    if (golferViewModel.Name == null)
                    {
                        golferViewModel.IsError = true;
                        golferViewModel.ImportGolferExcelReportModel.Name = "Invalid MemberShipId -" + row["Name"];
                    }
                }
                catch (Exception ex)
                {
                    golferViewModel.IsError = true;
                    golferViewModel.ImportGolferExcelReportModel.Name = "Invalid Name-" + row["Name"];
                }

                try
                {
                    golferViewModel.ClubMemberId = Convert.ToString(row["ClubMemberId"]);
                    if (golferViewModel.ClubMemberId == null)
                    {

                        golferViewModel.IsError = true;
                        golferViewModel.ImportGolferExcelReportModel.MembershipId = "Invalid MemberShipId -" + row["ClubMemberId"];
                    }
                    else
                    {
                        if (!CheckMemberShipId(golferViewModel.ClubMemberId)){
                            golferViewModel.IsError = true;
                            golferViewModel.ImportGolferExcelReportModel.MembershipId = " MemberShipId Already Exist -" + row["ClubMemberId"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    golferViewModel.IsError = true;
                    golferViewModel.ImportGolferExcelReportModel.MembershipId = "Invalid MemberShipId -" + row["ClubMemberId"];
                }

                try
                {
                    golferViewModel.EmailAddress = Convert.ToString(row["EMAIL"]);
                    if (golferViewModel.EmailAddress == null)
                    {
                        golferViewModel.IsError = true;
                        golferViewModel.ImportGolferExcelReportModel.Email = "Invalid Email -" + row["EMAIL"];
                    }
                }
                catch (Exception ex)
                {
                    golferViewModel.IsError = true;
                    golferViewModel.ImportGolferExcelReportModel.Email = "Invalid Email-" + row["EMAIL"];
                }

                try
                {
                    golferViewModel.Password = Convert.ToString(row["Password"]);
                    if (golferViewModel.Password == null)
                    {
                        golferViewModel.IsError = true;
                        golferViewModel.ImportGolferExcelReportModel.Password = "Invalid Password -" + row["Password"];
                    }
                }
                catch (Exception ex)
                {
                    golferViewModel.IsError = true;
                    golferViewModel.ImportGolferExcelReportModel.Password = "Invalid Password-" + row["Password"];
                }

                return golferViewModel;
            }
            catch (Exception ex)
            {
                golferViewModel.IsError = true;
                golferViewModel.Message = "Somthing Goes Wrong";
                return golferViewModel;
            }
        }

        private bool CheckMemberShipId(string id)
        {
            Golfer golfer = _unitOfWork.GolferRepository.Get(x => x.IsActive == true && x.ClubMemberId == id);
            if (golfer == null)
                return true;
            return false;
        }
    }
}
