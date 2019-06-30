using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using GestionReciclaje.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public Guid GetIdUser()
       {
           var currentUser = Helper.HttpContext.Current.User.Claims;
           var result = Guid.Empty;
           foreach (var i in currentUser)
           {
               if (i.Type.Equals("nameid"))
               {
                   result = Guid.Parse(i.Value);
               }
           }

           return result;
       }
        [HttpGet]   
        public async Task<IActionResult> GetAll([FromQuery]UserParams userParams)
        {
           // var currentUserId=GetIdUser();
            
           // var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var userFromRepo = await _repo.GetUser(currentUserId);
           // userParams.UserId = currentUserId;
            
            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserListDto>>(users);
           

            return Ok(new{
                List=usersToReturn,
                TotalRecords =userParams.TotalRecords
            });
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating user {id} failed on save");
        }
        
    }
}