
using colab_api.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using colab_api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using colab_api.Requests;
using colab_api.Responses.@base;
using colab_api.Responses;
using colab_api.Services;
using colab_api.Responses.user;
using colab_api.Requests.user;

namespace colab_api.Services
{
    public class ChatService
    {

        private UserRepositorie userRepositorie;
        private string transaction_id;
        private readonly SampleDBContext db;



        public ChatService(SampleDBContext db)
        {
            this.userRepositorie = new UserRepositorie(db);

        }

        public async Task<ResponseData<ChatResponse>> QueryChat(int userId)
        {
            var chatData = await userRepositorie.GetChat(userId);
            if (chatData == null)
            {
                return new ResponseData<ChatResponse>(httpCodeResponse.Success, null);
            }

            var response = new ChatResponse
            {
                id = chatData.chat_id,
                user_id = chatData.user_id,
                details = chatData.details
            };

            return new ResponseData<ChatResponse>(httpCodeResponse.Success, response);
        }




        public async Task<ResponseData<int>> UserUpsert(int userId, UserUpsertRequest data)
        {
          


            var user = await userRepositorie.GetUser(userId);
            if (user == null)
            {
                user = new Users();

            }
            user.first_name = data.firstName;
            user.last_name = data.lastName;
            user.email = data.email;
            user.birthday = data.birthDay;
            user.phone_number = data.phoneNumber;
            user.username = data.username;
            user.password = data.password;
            user.user_type = data.userType;
            user.active = data.active;
            int ret = -1;



            if (user.id > 0) {
                ret = await userRepositorie.UpdateUser(user);
            }

            
            else
            {
                ret = await userRepositorie.CreateUser(user);
            }

            return new ResponseData<int>(httpCodeResponse.Success, ret);
        }
        public async Task<ResponseData<int>> UserDelete(int userId)
        {


            var user = await userRepositorie.GetUser(userId);



            int ret = -1;
            if (user.id > 0)

                ret = await userRepositorie.DeleteUser(user);



            return new ResponseData<int>(httpCodeResponse.Success, ret);
        }



    }
}
