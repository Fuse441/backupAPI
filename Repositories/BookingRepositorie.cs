using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Net.Http.Headers;
using colab_api.Models;
using colab_api.Responses.space;

namespace colab_api.Repositories
{
    public class BookingRepositorie
    {
        private readonly SampleDBContext db;
        public BookingRepositorie(SampleDBContext db)
        {
            this.db = db;
        }


        public async Task<Space> user(string username)
        {
            return await db.Spaces.FirstOrDefaultAsync(u => u.space_name == username);
        }

        public async Task<Booking> GetSpace(int id)
        {
            var item = await db.Bookings.FirstOrDefaultAsync<Booking>(i => i.space_id == id);
            return item;
        }

        public async Task<Booking> GetBooking(int id)
        {
            var item = await db.Bookings.FirstOrDefaultAsync<Booking>(i => i.booking_id == id);
            return item;
        }
        public async Task<Booking> GetBookingBySpaceId(int id)
        {
            var item = await db.Bookings.FirstOrDefaultAsync<Booking>(i => i.space_id == id && i.booking_status == 1);
            return item;
        }
        public async Task<Booking> GetUser(int id)
        {
            var item = await db.Bookings.FirstOrDefaultAsync<Booking>(i => i.user_id == id && i.booking_status == 1);
            return item;
        }
        public async Task<int> CreateBooking(Booking booking)
        {
            db.Bookings.Add(booking); 
            await db.SaveChangesAsync(); 
            return booking.booking_id;
        }

        public async Task<int> UpdateBooking(Booking booking)
        {
            db.Bookings.Update(booking);
            await db.SaveChangesAsync(); 
            return booking.booking_id; 
        }

        public async Task<int> DeleteUser(Space space)
        {
           
            db.Spaces.Remove(space);

          
            await db.SaveChangesAsync();

            return space.space_id;
        }
        public async Task<(int totalItems, List<Booking> items, List<ChartResponse> bookingCounts)> ChartSpace(
     int partnerId, string search, int pageNumber = 0, int pageSize = 10, int active = -1, string[] amenities = null)
        {
            var queryBookings = from booking in db.Bookings
                                join space in db.Spaces on booking.space_id equals space.space_id
                                where space.partner_id == partnerId
                                select booking;

            var totalItems = await queryBookings.CountAsync();

            int start = pageNumber * pageSize;
            var pagedQuery = queryBookings.Skip(start).Take(pageSize);
            var items = await pagedQuery.ToListAsync();

            var bookingCounts = queryBookings
                .GroupBy(b => b.space_id)
                .Select(g => new ChartResponse { spaceId = g.Key, count = g.Count() })
                .ToList();

            return (totalItems, items, bookingCounts);
        }

      


        public async Task<Tuple<int, List<Booking>>> GetSpace(int id, string search,
         int pageNumber = 0, int pageSize = 10,
         int active = -1, string[] amenities = null)
        {
            var total_item = 0;
            var query = db.Bookings.Where(u => u.booking_status == 1);

            if (id != 0)
            {
                query = query.Where(i => i.user_id == id);
            }

            //if (!string.IsNullOrEmpty(search))
            //{
            //    search = search.ToLower();

            //    query = query.Where(i =>
            //        i.space_name.ToLower().Contains(search) ||
            //        i.des.ToLower().Contains(search) ||
            //        i.space_type.ToLower().Contains(search) ||
            //        i.space_details.ToLower().Contains(search)

            //    );
            //}

            if (amenities != null && amenities.Length > 0)
            {

            }


            total_item = await query.CountAsync();
            int start = pageNumber * pageSize;
            query = query.Skip(start).Take(pageSize);
            var items = await query.ToListAsync();
            return new Tuple<int, List<Booking>>(total_item, items);
        }
        public async Task<Tuple<int, List<Booking>>> GetReportTransaction(int id, string search,
         int pageNumber = 0, int pageSize = 10,
         int active = -1, string[] amenities = null)
        {

            var total_item = 0;
            var queryPartner = db.Spaces.Where(u => u.partner_id == id);

            // เลือกเฉพาะ space_id จาก queryPartner
            var spaceIds = queryPartner.Select(u => u.space_id);

            // กรองข้อมูลใน queryBooking โดยเช็คว่า space_id อยู่ใน spaceIds
            var queryBooking = db.Bookings.Where(u => spaceIds.Contains(u.space_id));

            //if (id != 0)
            //{
            //    query = query.Where(i => i.user_id == id);
            //}


            //if (!string.IsNullOrEmpty(search))
            //{
            //    query = query.Where(i =>
            //        i.terminal_id.Contains(search) ||
            //        i.citizen_id.Contains(search) ||
            //        i.product_code.Contains(search) ||
            //        i.transaction_id.Contains(search) ||
            //        i.ref_transaction_id.Contains(search) ||
            //        i.nhso_authen_code.Contains(search) ||
            //        i.nhso_maininscl.Contains(search) ||
            //        i.nhso_maininscl_name.Contains(search) ||
            //        i.nhso_message.Contains(search) ||
            //        i.nhso_result.Contains(search) ||
            //        i.nhso_status.Contains(search)
            //    );
            //}

            total_item = await queryBooking.CountAsync();
                int start = pageNumber * pageSize;

            queryBooking = queryBooking.Skip(start).Take(pageSize);
                var items = await queryBooking.ToListAsync();


                return new Tuple<int, List<Booking>>(total_item, items);
            
        }

        public async Task<Tuple<int, List<Booking>>> GetBookingSelectDate(int spaceId,DateOnly date)
        {

            var total_item = 0;
            var query = db.Bookings.Where(u => u.space_id == spaceId && u.booking_date == date && u.booking_status == 1);

          

            total_item = await query.CountAsync();
          
            var items = await query.ToListAsync();

            return new Tuple<int, List<Booking>>(total_item, items);
        }
        public async Task<Tuple<int, List<Booking>>> GetBookingBySpace(int spaceId)
        {

            var total_item = 0;
            var query = db.Bookings.Where(u => u.space_id == spaceId);



            total_item = await query.CountAsync();

            var items = await query.ToListAsync();

            return new Tuple<int, List<Booking>>(total_item, items);
        }

    }
}
