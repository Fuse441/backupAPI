using colab_api.Models;
using colab_api.Repositories;
using colab_api.Requests.space;
using colab_api.Requests.user;
using colab_api.Responses.space;
using colab_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;
  

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
        
    }
    [HttpGet]
    [Route("transaction/export/{id}")]
    public async Task<IActionResult> ExportTransaction([FromQuery] BookingRequest queryParam, int id)
    {

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            var result = await _bookingService.ExportTransaction(queryParam, id);
            var fileName = "Transaction " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            // สร้าง Sheet ใหม่
            var worksheet = package.Workbook.Worksheets.Add("MySheet");

            // เขียนข้อมูลลงในเซลล์
            int rowIndex = 1;

            // กำหนดหัวคอลัมน์เอง
            var columnHeaders = new string[] { "ชื่อจริง", "นามสกุล", "อีเมล","เบอร์โทรศัพท์","ชื่อสถานที่","คำอธิบาย","รายละเอียด","ประเภท","เวลาเริ่มจอง","เวลาสิ้นสุด","วันที่จอง","สถานะ" };

            // เขียนข้อมูลหัวคอลัมน์
            for (int i = 0; i < columnHeaders.Length; i++)
            {
                worksheet.Cells[rowIndex, i + 1].Value = columnHeaders[i];
            }

            // เขียนข้อมูลลงในเซลล์
            foreach (var item in result)
            {
                rowIndex++;

                // กำหนดค่าข้อมูลของแต่ละคอลัมน์
                worksheet.Cells[rowIndex, 1].Value = item.firstName;
                worksheet.Cells[rowIndex, 2].Value = item.lastName;
                worksheet.Cells[rowIndex, 3].Value = item.email;
                worksheet.Cells[rowIndex, 4].Value = item.phoneNumber;
                worksheet.Cells[rowIndex, 5].Value = item.spaceName;
                worksheet.Cells[rowIndex, 6].Value = item.spaceDescription;
                worksheet.Cells[rowIndex, 7].Value = item.spaceDetails;
                worksheet.Cells[rowIndex, 8].Value = item.spaceType;
                worksheet.Cells[rowIndex, 9].Value = item.bookingStartTime;
                worksheet.Cells[rowIndex, 10].Value = item.bookingEndTime;
                worksheet.Cells[rowIndex, 11].Value = item.bookingDate;
                worksheet.Cells[rowIndex, 12].Value = item.bookingStatus == 1 ? "กำลังดำเนินการ" : "เสร็จสิ้น" ;

            }

            var file = new System.IO.FileInfo(fileName);
            package.SaveAs(file);
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

            var stream = new System.IO.MemoryStream(package.GetAsByteArray());

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
    [HttpGet]
    [Route("chart/{id}")]
    public async Task<ActionResult> ChartBooking([FromQuery] BookingRequest query, int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _bookingService.ChartBooking(query,id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("CheckStatus/{id}")]
    public async Task<ActionResult> CheckBooking(int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _bookingService.CheckBookingStatus(id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    [HttpPost]
    [Route("UpdateStatus/{id}")]
    public async Task<ActionResult> UpdateBooking([FromRoute] int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _bookingService.UpdateBookingStatus(id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<ActionResult> Booking(int id, BookingUpsertRequest data)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _bookingService.BookingSpace(id, data);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("selectDate/{id}/{date}")]
    public async Task<ActionResult> GetBookingSelectDate([FromRoute] int id, [FromRoute] string date)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _bookingService.QueryBookingSelectDate(id,date);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetBookingById([FromRoute] int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _bookingService.QueryBooking(id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    //[HttpPost]
    //[Route("{id}")]
    //public async Task<ActionResult> UpsertSpace(int id, SpaceUpsertRequest query)
    //{
    //    try
    //    {
    //        var transaction_id = Guid.NewGuid().ToString("n");
    //        if (!ModelState.IsValid)
    //        {
    //            return StatusCode(400);
    //        }
    //        var result = await _spaceService.SpaceUpsert(id,query);
    //        return StatusCode(200, result);



    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500);
    //    }
    //}
    //[HttpDelete]
    //[Route("{id}")]
    //public async Task<ActionResult> DeleteSpace(int id)
    //{
    //    try
    //    {
    //        var transaction_id = Guid.NewGuid().ToString("n");
    //        if (!ModelState.IsValid)
    //        {
    //            return StatusCode(400);
    //        }
    //        var result = await _spaceService.SpaceDelete(id);
    //        return StatusCode(200, result);



    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500);
    //    }
    //}

}
