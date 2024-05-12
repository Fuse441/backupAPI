
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
using System.Numerics;
using colab_api.Services.MailService;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace colab_api.Services
{
    public class UserService
    {

        private UserRepositorie userRepositorie;
        private string transaction_id;
        private readonly SampleDBContext db;
        private readonly IEmailService _emailService;


        public UserService(SampleDBContext db, IEmailService emailService)
        {
            this.userRepositorie = new UserRepositorie(db);
            _emailService = emailService;
        }

        public async Task<ResponseData<userloginResponse>> userAuthen(loginRequest request)
        {
            var key = Environment.GetEnvironmentVariable("key");
            var model = new userloginResponse();
            //var temp = new LoginService().EncryptAES256(key, "Fuse27271144");
            var loginName = new LoginService().DecryptAES256(key, request.username);
            //var password = new LoginService().DecryptAES256(key, request.password);

            var user = await userRepositorie.user(loginName);

            if (user != null && user.password == request.password)
            {
                var userProfile = new UserProfile
                {
                    id = user.id,
                    username = user.username,
                    firstname = user.first_name,
                    lastname = user.last_name,
                    email = user.email,
                    birthDay = user.birthday,
                    phoneNumber = user.phone_number,
                    userType = user.user_type,
                };

                // กำหนดค่าของ properties ใน userloginResponse
                model.userProfile = userProfile;
                model.active = user.active;
                // สร้าง JWT และกำหนดค่าให้กับ property jwt
                var jwtGenerator = new GenerateJwtToken();
                string jwtToken = jwtGenerator.GenerateToken(user);
                model.jwt = jwtToken;
                model.userType = user.user_type;

                return new ResponseData<userloginResponse>(httpCodeResponse.Success, model);
            }
            else
            {

                return new ResponseData<userloginResponse>(httpCodeResponse.Unauthorized);
            }

        }
        public async Task<ResponseData<UsersResponse>> QueryUser(UsersRequest query)
        {


            var items = new List<UsersResponse>();
            int total_item = 0;
            int page = query.pageNumber == null ? 0 : query.pageNumber.Value;
            int per_page = query.pageSize == null ? 10 : query.pageSize.Value;

            var temps = await userRepositorie.GetUser(query.search, page, per_page, query.active);



            total_item = temps.Item1;
            foreach (var d in temps.Item2)
            {
                var model = new UsersResponse()
                {
                    id = d.id,
                    firstName = d.first_name,
                    lastName = d.last_name,
                    email = d.email,
                    birthDay = d.birthday,
                    active = d.active == 1 ? true : false,
                    phoneNumber = d.phone_number,
                    username = d.username,
                    userType = d.user_type
                };





                items.Add(model);
            }

            return new ResponseDataPagination<UsersResponse>(httpCodeResponse.Success, items,
               ((page * per_page) + per_page) > total_item ? total_item : ((page * per_page) + per_page),
            temps.Item1);
        }
        public async Task<ResponseData<UsersResponse>> QueryUserById(int id)
        {





            var d = await userRepositorie.GetUser(id);




            var model = new UsersResponse()
            {
                id = d.id,
                firstName = d.first_name,
                lastName = d.last_name,
                email = d.email,
                birthDay = d.birthday,
                active = d.active == 1 ? true : false,
                phoneNumber = d.phone_number,
                username = d.username,
                userType = d.user_type
            };




            return new ResponseData<UsersResponse>(httpCodeResponse.Success, model);

        }

        public async Task<ResponseData<string>> UserContact(UserContactRequest data)
        {


            string msg = $"สวัสดีทีมงาน,\r\n\r\n" +
                $"เราได้รับการติดต่อใหม่ผ่านเว็บไซต์ของเรา นี่คือรายละเอียด:\r\n\r\n" +
                $"- ชื่อ: {data.firstName} {data.lastName}\r\n" +
                $"- อีเมล: {data.email}\r\n- " +
                $"ข้อความ: {data.msg} \r\n\r\n" +
                $"กรุณาติดตามและอัปเดตความคืบหน้าให้ทีมทราบภายใน 24 ชั่วโมง\r\n\r\nขอบคุณ,\r\n[ชื่อคุณ/แผนก]";
       
            await _emailService.SendEmailAsync(data.email, "ทำรายการสำเร็จ", msg);
            return new ResponseData<string>(httpCodeResponse.Success, "Send");
        }


        public async Task<ResponseData<int>> UserUpsert(int userId, UserUpsertRequest data)
        {
          


            var user = await userRepositorie.GetUser(userId);
            var chat = await userRepositorie.GetChat(userId);
            if (user == null )
            {
                var checkUsername = await userRepositorie.user(data.username);
                if (checkUsername == null)
                {
                    user = new Users();
                    chat = new Chat();
                }
                else if (checkUsername != null)
                {
                    return new ResponseData<int>(httpCodeResponse.DataExisted, 0);
                }
              

            }
        
           
            user.first_name = data.firstName;
            user.last_name = data.lastName;
            user.email = data.email;
            user.birthday = data.birthDay;
            user.phone_number = data.phoneNumber;
            user.username = data.username;
           
            if(userId != 0)
            {
                user.password = user.password;
                user.user_type = user.user_type;
            }
            else
            {
                var key = Environment.GetEnvironmentVariable("key");
                var temp = new LoginService().EncryptAES256(key, data.password);
                user.user_type = data.userType;
                user.password = temp;
            }
         
         
           
            
            user.active = data.active;
            int ret = -1;



            if (user.id > 0) {
                ret = await userRepositorie.UpdateUser(user);
            }

            
            else
            {
              
                
                ret = await userRepositorie.CreateUser(user);
                chat.user_id = ret;
                chat.details = "[]";
                ret = await userRepositorie.CreateChat(chat);
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
