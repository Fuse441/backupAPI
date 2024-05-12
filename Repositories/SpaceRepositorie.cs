using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Net.Http.Headers;
using colab_api.Models;
using Newtonsoft.Json;

namespace colab_api.Repositories
{
    public class SpaceRepositorie
    {
        private readonly SampleDBContext db;
        public SpaceRepositorie(SampleDBContext db)
        {
            this.db = db;
        }


        public async Task<Space> user(string username)
        {
            return await db.Spaces.FirstOrDefaultAsync(u => u.space_name == username);
        }



        public async Task<Space> GetSpace(int id)
        {
            var item = await db.Spaces.FirstOrDefaultAsync<Space>(i => i.space_id == id);
            return item;
        }

        public async Task<int> CreateSpace(Space space)
        {
            db.Spaces.Add(space); 
            await db.SaveChangesAsync(); 
            return space.space_id;
        }

        public async Task<int> UpdateSpace(Space space)
        {
            db.Spaces.Update(space);
            await db.SaveChangesAsync(); 
            return space.space_id; 
        }

        public async Task<int> DeleteUser(Space space)
        {
           
            db.Spaces.Remove(space);

          
            await db.SaveChangesAsync();

            return space.space_id;
        }
        public async Task<Tuple<int, List<Booking>>> GetReportTransactionExcal()
        {

            var total_item = 0;




            var query = db.Bookings.Where(u => u.booking_status == 1 && u.booking_status == 0);




            var items = await query.ToListAsync();
            total_item = items.Count;
            return new Tuple<int, List<Booking>>(total_item, items);
        }


        public async Task<Tuple<int, List<Space>>> GetSpace(int id, string search,
         int pageNumber = 0, int pageSize = 10,
         int active = -1,string[] amenities = null)
        {
            var total_item = 0;
            var query = db.Spaces.Where(u => u.status == 1);

            if (id != 0)
            {
                query = query.Where(i => i.partner_id == id);
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                query = query.Where(i =>
                    i.space_name.ToLower().Contains(search) ||
                    i.des.ToLower().Contains(search) ||
                    i.space_type.ToLower().Contains(search) ||
                    i.space_details.ToLower().Contains(search) 
                  
                );
            }

            if (amenities != null && amenities.Length > 0)
            {
            
            }


            total_item = await query.CountAsync();
            int start = pageNumber * pageSize;
            query = query.Skip(start).Take(pageSize);
            var items = await query.ToListAsync();
            return new Tuple<int, List<Space>>(total_item, items);
        }



    }


}



