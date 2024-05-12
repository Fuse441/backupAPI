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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Cryptography;

namespace colab_api.Services
{
    public class SpaceService 
    {
       
        private SpaceRepositorie spaceRepositorie;
        private UserRepositorie userRepositorie;
        private string transaction_id;
        private readonly SampleDBContext db;



        public SpaceService(SampleDBContext db)
        {
            this.spaceRepositorie = new SpaceRepositorie(db);
            this.userRepositorie = new UserRepositorie(db);
        }

        //public async Task<ResponseData<userloginResponse>> userAuthen(loginRequest request)
        //{
        //    var key = Environment.GetEnvironmentVariable("key");
        //    var model = new userloginResponse();
        //    //var temp = new LoginService().EncryptAES256(key, "Fuse27271144");
        //    var loginName = new LoginService().DecryptAES256(key, request.username);
        //    //var password = new LoginService().DecryptAES256(key, request.password);

        //    var user = await spaceRepositorie.user(loginName);

        //    if (user != null && user.password == request.password)
        //    {
        //        var userProfile = new UserProfile
        //        {
        //            username = user.username,
        //            firstname = user.first_name,
        //            lastname = user.last_name,
        //            email = user.email,
        //            birthDay = user.birthday,
        //            phoneNumber = user.phone_number
        //        };

        //        // กำหนดค่าของ properties ใน userloginResponse
        //        model.userProfile = userProfile;
        //        model.active = user.active;
        //        // สร้าง JWT และกำหนดค่าให้กับ property jwt
        //        var jwtGenerator = new GenerateJwtToken();
        //        string jwtToken = jwtGenerator.GenerateToken(user);
        //        model.jwt = jwtToken;
        //        model.userType = user.user_type;

        //        return new ResponseData<userloginResponse>(httpCodeResponse.Success, model);
        //    }
        //    else
        //    {

        //        return new ResponseData<userloginResponse>(httpCodeResponse.Unauthorized);
        //    }

        //}
       

        public async Task<ResponseData<SpaceResponse>> QueryUserById(int id, SpaceRequest query)
        {

            var items = new List<SpaceResponse>();
            var user = await userRepositorie.GetUser(id);
            int total_item = 0;
            int page = query.pageNumber == null ? 0 : query.pageNumber.Value;
            int per_page = query.pageSize == null ? 10 : query.pageSize.Value;
            var temps = await spaceRepositorie.GetSpace(id,query.search, page, per_page, query.active);



            total_item = temps.Item1;
            foreach (var d in temps.Item2)
            {
                var model = new SpaceResponse()
                {
                    spaceId = d.space_id,
                    spaceName = d.space_name,
                    spaceDescription = d.des,
                    spaceType = d.space_type,
                    spaceDetails = d.space_details,
                    status = d.status == 1 ? true : false,
                    partnerId = d.partner_id,
                    image = d.image
                };

               




                items.Add(model);
            }

            return new ResponseDataPagination<SpaceResponse>(httpCodeResponse.Success, items,
               ((page * per_page) + per_page) > total_item ? total_item : ((page * per_page) + per_page),
            temps.Item1);

        }

        public async Task<ResponseData<SpaceResponse>> QuerySpace(SpaceRequest query)
        {

                
            var items = new List<SpaceResponse>();
            int total_item = 0;
            int page = query.pageNumber == null ? 0 : query.pageNumber.Value;
            int per_page = query.pageSize == null ? 10 : query.pageSize.Value;

            var temps = await spaceRepositorie.GetSpace(0,query.search, page, per_page, query.active,query.amenities);
            var pId = await userRepositorie.UserList();


            total_item = temps.Item1;
            foreach (var d in temps.Item2)
            {
                var model = new SpaceResponse()
                {
                    spaceId = d.space_id,
                    spaceName = d.space_name,
                    spaceDescription = d.des,
                    spaceType = d.space_type,
                    spaceDetails = d.space_details,
                    status = d.status == 1 ? true : false,
                    image = d.image
                  
                };

                //if (pId.Where(i => i.id == d.space_id).Any())
                //{
                //    var t = pId.Where(i => i.id == d.space_id).First();
                //    model.partnerId = t.first_name;
                //}




                items.Add(model);
            }

            return new ResponseDataPagination<SpaceResponse>(httpCodeResponse.Success, items,
               ((page * per_page) + per_page) > total_item ? total_item : ((page * per_page) + per_page),
            temps.Item1);
        }

        public async Task<ResponseData<int>> SpaceUpsert(int userId, SpaceUpsertRequest data)
        {



            var user = await spaceRepositorie.GetSpace(userId);
            if (user == null)
            {
                user = new Space();

            }
            user.space_name = data.spaceName;
            user.des = data.spaceDescription;
            user.status = data.status == true ? 1 : 0;
            user.space_details = data.spaceDetails;
            user.partner_id = data.partnerId;
            user.space_type = data.spaceType;
            user.image = data.image;
          
            int ret = -1;



            if (userId > 0)
            {
                ret = await spaceRepositorie.UpdateSpace(user);
            }


            else
            {
                ret = await spaceRepositorie.CreateSpace(user);
            }

            return new ResponseData<int>(httpCodeResponse.Success, ret);
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
