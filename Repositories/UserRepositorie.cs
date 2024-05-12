
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Net.Http.Headers;
using colab_api.Models;
using System.Data;

namespace colab_api.Repositories
{
    public class UserRepositorie
    {
        private readonly SampleDBContext db;
        public UserRepositorie(SampleDBContext db)
        {
            this.db = db;
        }


        public async Task<Users> user(string username)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.username == username);
        }



        public async Task<Users> GetUser(int id)
        {
            var item = await db.Users.FirstOrDefaultAsync<Users>(i => i.id == id);
            return item;
        }
        public async Task<Chat> GetChat(int id)
        {
            var item = await db.Chats.FirstOrDefaultAsync<Chat>(i => i.user_id == id);
            return item;
        }
      
        public async Task<int> CreateUser(Users user)
        {
            db.Users.Add(user); 
            await db.SaveChangesAsync();
            
            return user.id; 
        }

        public async Task<int> UpdateChat(Chat user)
        {
            db.Chats.Update(user);
            await db.SaveChangesAsync();
            return user.chat_id;
        }
        public async Task<int> UpdateUser(Users user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync(); 
            return user.id; 
        }
        public async Task<int> CreateChat(Chat chat)
        {
            db.Chats.Add(chat);
            await db.SaveChangesAsync();
            return chat.chat_id;
        }
        public async Task<int> DeleteUser(Users user)
        {
           
            db.Users.Remove(user);

          
            await db.SaveChangesAsync();

            return user.id;
        }
        public async Task<List<Users>> UserList()
        {
            var items = await db.Users.Where(i => i.active == 1).ToListAsync();

            return items;
        }

        public async Task<Tuple<int, List<Users>>> GetUser(string search,
            int pageNumber = 0, int pageSize = 10,
            int active = -1)
        {

            var total_item = 0;
            var query = db.Users.Where(i => i.active == 1 || i.active == 0);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                query = query.Where(i =>
                    i.first_name.ToLower().Contains(search) ||
                    i.last_name.ToLower().Contains(search) ||
                    i.username.ToLower().Contains(search) ||
                    i.user_type.ToLower().Contains(search)
                );
            }

            total_item = await query.CountAsync();
            int start = pageNumber * pageSize;
            query = query.Skip(start).Take(pageSize);
            var items = await query.ToListAsync();

            return new Tuple<int, List<Users>>(total_item, items);
        }


    }
}
