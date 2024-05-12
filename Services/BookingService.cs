using colab_api.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using colab_api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using colab_api.Responses.@base;
using colab_api.Responses;
using colab_api.Services;
using colab_api.Responses.user;
using colab_api.Responses.space;
using colab_api.Requests.user;
using colab_api.Requests.space;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using colab_api.Services.MailService;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace colab_api.Services
{
    public class BookingService
    {
       
        private SpaceRepositorie spaceRepositorie;
        private UserRepositorie userRepositorie;
        private BookingRepositorie bookingRepositorie;
        private string transaction_id;
        private readonly SampleDBContext db;
        private readonly IEmailService _emailService;


        public BookingService(SampleDBContext db, IEmailService emailService)
        {
            this.spaceRepositorie = new SpaceRepositorie(db);
            this.userRepositorie = new UserRepositorie(db);
            this.bookingRepositorie = new BookingRepositorie(db);
            
            _emailService = emailService;
        }
        public async Task<List<BookingResponse>> ExportTransaction(BookingRequest query, int id)
        {

            var items = new List<BookingResponse>();
            int total_item = 0;

            int page = query.pageNumber == null ? 0 : query.pageNumber.Value;
            int per_page = query.pageSize == null ? 10 : query.pageSize.Value;
            var temps = await bookingRepositorie.GetReportTransaction(id, query.search, page, per_page, query.active);
           

          






            foreach (var d in temps.Item2)
            {
                var user = await userRepositorie.GetUser(d.user_id);
                var space = await spaceRepositorie.GetSpace(d.space_id);
                var model = new BookingResponse()
                {
                   
                    firstName = user.first_name,
                    lastName = user.last_name,
                    email = user.email,
                    phoneNumber = user.phone_number,
                    spaceId = d.space_id,
                    spaceName = space.space_name,
                    spaceDescription = space.des,
                    spaceDetails = space.space_details,
                    spaceType = space.space_type,
                    bookingStartTime = d.booking_start_time,
                    bookingEndTime = d.booking_end_time,
                    bookingDate = d.booking_date,
                    bookingStatus = d.booking_status,
                };
           
                

                items.Add(model);

            }
         
          

            return items;
        }
        public async Task<ResponseData<List<BookingResponse>>> QueryBooking(int id)
        {
            try
            {
       
                var bookingData = await bookingRepositorie.GetBookingBySpace(id);
                if (bookingData == null || bookingData.Item2 == null || !bookingData.Item2.Any())
                {
                    return new ResponseData<List<BookingResponse>>(httpCodeResponse.NotFound, null);
                }

                var bookings = bookingData.Item2.Select(d => new BookingResponse()
                {
                    bookingId = d.booking_id,
                    userId = d.user_id,
                    spaceId = d.space_id,
                    bookingStartTime = d.booking_start_time,
                    bookingEndTime = d.booking_end_time,
                    bookingDate = d.booking_date,
                    bookingStatus = d.booking_status,
                    additionalNote = d.additional_notes
                }).ToList();

                return new ResponseData<List<BookingResponse>>(httpCodeResponse.Success, bookings);
            }
            catch (Exception ex)
            {
                // Log the exception details here and handle accordingly
                return new ResponseData<List<BookingResponse>>(httpCodeResponse.InternalServerError, null);
            }
        }

        public async Task<ResponseData<List<BookingResponse>>> QueryBookingSelectDate(int id , string dateString)
        {
            try
            {
                DateOnly date = DateOnly.Parse(dateString);
                var bookingData = await bookingRepositorie.GetBookingSelectDate(id,date);
                if (bookingData == null || bookingData.Item2 == null || !bookingData.Item2.Any())
                {
                    return new ResponseData<List<BookingResponse>>(httpCodeResponse.NotFound, null);
                }

                var bookings = bookingData.Item2.Select(d => new BookingResponse()
                {
                    bookingStartTime = d.booking_start_time,
                    bookingEndTime = d.booking_end_time,
                    bookingDate = d.booking_date,
                    bookingStatus = d.booking_status,
          
                }).ToList();

                return new ResponseData<List<BookingResponse>>(httpCodeResponse.Success, bookings);
            }
            catch (Exception ex)
            {
                // Log the exception details here and handle accordingly
                return new ResponseData<List<BookingResponse>>(httpCodeResponse.InternalServerError, null);
            }
        }
        public async Task<ResponseData<int>> UpdateBookingStatus(int spaceId)
        {
            var booking = await bookingRepositorie.GetBookingBySpaceId(spaceId);

            try
            {
                if (booking == null)
                {
                    // Returns null data with NotFound status if no booking exists
                    return new ResponseData<int>(httpCodeResponse.NotFound, 404);
                }

                var bookings = new Booking();
                bookings = booking;
      
                bookings.booking_status = 0;
           
                
                await bookingRepositorie.UpdateBooking(bookings);
                return new ResponseData<int>(httpCodeResponse.Success, 200);

            }
            catch (Exception ex)
            {
                // Log the exception details here and handle accordingly
                return new ResponseData<int>(httpCodeResponse.InternalServerError, 500);
            }
        }

        public async Task<List<ChartResponse>> ChartBooking(BookingRequest query,int userId)
        {
            var items = new List<ChartResponse>();
            int total_item = 0;

            int page = query.pageNumber == null ? 0 : query.pageNumber.Value;
            int per_page = query.pageSize == null ? 10 : query.pageSize.Value;
            var temps = await bookingRepositorie.ChartSpace(userId, query.search, page, per_page, query.active);









            foreach (var d in temps.Item3)
            {

                var space = await spaceRepositorie.GetSpace(d.spaceId);

                var model = new ChartResponse()
                {
                    spaceId = d.spaceId,
                    spaceName = space.space_name,
                    count = d.count,
                   
                };



                items.Add(model);

            }



            return items;

        }

        public async Task<ResponseData<BookingResponse>> CheckBookingStatus(int spaceId)
        {
            var booking = await bookingRepositorie.GetBookingBySpaceId(spaceId);

            try
            {
                if (booking == null)
                {
                    // Returns null data with NotFound status if no booking exists
                    return new ResponseData<BookingResponse>(httpCodeResponse.NotFound, null);
                }
                TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
                TimeSpan timeDifference = (currentTime - booking.booking_start_time).Duration();
                if (timeDifference.TotalHours > 3)
                {
               
                    return new ResponseData<BookingResponse>(httpCodeResponse.BadRequest, null);
                }
                var bookings = new BookingResponse()
                {
                    bookingStartTime = booking.booking_start_time,
                    bookingEndTime = booking.booking_end_time,
                    bookingDate = booking.booking_date,
                    bookingStatus = booking.booking_status,

                };
                return new ResponseData<BookingResponse>(httpCodeResponse.Success, bookings);

            }
            catch (Exception ex)
            {
                // Log the exception details here and handle accordingly
                return new ResponseData<BookingResponse>(httpCodeResponse.InternalServerError, null);
            }


        }


        public async Task<ResponseData<int>> BookingSpace(int bookingId, BookingUpsertRequest data)
        {
            int? status = 0;
            var booking = await bookingRepositorie.GetBooking(bookingId);
            var checkUserBooking = await bookingRepositorie.GetUser(data.userId);
            if(checkUserBooking != null)
            {
                status = checkUserBooking.booking_status;
            }
            else
            {
                status = 0;
            }
           
            if (booking == null && status == 0)
            {
                booking = new Booking();

            }
            else
            {
                return new ResponseData<int>(httpCodeResponse.BadRequest, 404);
            }
           

            booking.user_id = data.userId;
            booking.space_id = data.spaceId;
            booking.booking_start_time = data.startTime;
            booking.booking_end_time = data.endTime;
            booking.booking_date = data.bookingDate;
            booking.booking_status = data.bookingStatus;
            booking.additional_notes = data.note;


            var spaceid = await spaceRepositorie.GetSpace(data.spaceId);
            var userid = await userRepositorie.GetUser(data.userId);
           
            var spaceName = spaceid.space_name; 
            var email = userid.email;
            var name = userid.username;
            var phone = userid.phone_number;
            var chatid = await userRepositorie.GetChat(data.userId);
            


            int ret = -1;
            if (booking.booking_id > 0)
            {
                // Update existing booking
                //ret = await bookingRepositorie.UpdateBooking(booking);
            }
            else
            {
                // Create new booking
                
                var detailsList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(chatid.details);

                string msg = $"เรียน {name},\r\n\r\n" +
                    $"ขอบคุณสำหรับการจองที่ {spaceName} ของเรา! เรายินดีที่จะแจ้งให้ท่านทราบว่าการจองของคุณได้รับการยืนยันแล้ว ด้วยรายละเอียดดังนี้:\r\n\r\n- " +
                    $"**วันที่จอง:** {data.startTime}\r\n- " +
                    $"**เวลา:** {data.startTime} ถึง {data.endTime}\r\n- " +
                    $"\r\n\r\nกรุณาแสดงอีเมลนี้หรือหมายเลขการจอง {GenerateBookingCode(phone)} ที่จุดลงทะเบียนที่ ชื่อสถานที่/กิจกรรม เมื่อเดินทางมาถึงในวันที่นัดหมาย.\r\n\r\nหากมีคำถามหรือต้องการเปลี่ยนแปลงการจอง กรุณาติดต่อเราที่ 0645098990 หรือ website.colab.service@gmail.com.\r\n\r\n" +
                    $"ขอให้คุณมีประสบการณ์ที่ยอดเยี่ยมกับเรา!\r\n\r\n" +
                    $"ขอแสดงความนับถือ,\r\nCO-LAB Service\r\n";
                var newEntry = new Dictionary<string, string>
                {
                    ["message"] = msg
                };
                detailsList.Add(newEntry);
                var updatedDetails = JsonConvert.SerializeObject(detailsList);
                chatid.details = updatedDetails;
                await userRepositorie.UpdateChat(chatid);
                ret = await bookingRepositorie.CreateBooking(booking);
                await _emailService.SendEmailAsync(email, "ทำรายการสำเร็จ", msg);
            }

            return new ResponseData<int>(httpCodeResponse.Success, ret);
        }
        public string GenerateBookingCode(string userPhoneNumber)
        {
 
            Random _random = new Random();
            string randomCode = _random.Next(1000, 9999).ToString();


            string bookingCode = $"{userPhoneNumber}COLAB{randomCode}";

            return bookingCode;
        }
      

        public async Task<ResponseData<int>> SpaceDelete(int userId)
        {


            var user = await spaceRepositorie.GetSpace(userId);



            int ret = -1;
            if (user.space_id > 0)

                ret = await spaceRepositorie.DeleteUser(user);



            return new ResponseData<int>(httpCodeResponse.Success, ret);
        }



    }
}
