﻿using AutoMapper;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.AddressDTOs;
using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.Services.AddressService
{
    public class AddressService:IAddressService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AddressService(AppDbContext context,IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        
        }

        public async Task<ApiResponse<int>> CreateShippingAddress(int userUId, OrderAddressDTO orderAddressDTO)
        {
            try
            {
                if (orderAddressDTO == null)
                {
                    return new ApiResponse<int>(400, "Bad Request,Address is null");


                }


                // A user can have atmost 3 address

                var addressCount = await _context.ShippingAddresses.CountAsync(x => x.UserId == userUId);

                if (addressCount >= 3)
                {
                    return new ApiResponse<int>(422, "User cannot have more than 3 addresses.");
                }



                var newAddress = _mapper.Map<ShippingAddress>(orderAddressDTO);
                newAddress.UserId = userUId;
                await _context.ShippingAddresses.AddAsync(newAddress);
                await _context.SaveChangesAsync();


                return new ApiResponse<int>(200, "Successfully Created Shipping Address",newAddress.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<List<ViewAddressDTO>>> GetShippingAddress(int userId)
        {

            var userAddress = await _context.Users.Include(x => x.ShippingAddresses).FirstOrDefaultAsync(x => x.Id == userId);

            if (userAddress == null)
            {
                return new ApiResponse<List<ViewAddressDTO>>(401, "Invalid User");
            }

         

            var res = new List<ViewAddressDTO>();

            foreach (var address in userAddress.ShippingAddresses)
            {
                var showAddress = _mapper.Map<ViewAddressDTO>(address);
                res.Add(showAddress);
            }

            return new ApiResponse<List<ViewAddressDTO>>(200, "Shipping Address Fetched Successfully", res);


        }

        
        public async Task<bool> RemoveShippingAddressByUser(int userId, int addressId)
        {

            var userAddress = await _context.Users.Include(x => x.ShippingAddresses).FirstOrDefaultAsync(x=>x.Id==userId);

            var checkAddress=userAddress.ShippingAddresses.FirstOrDefault(x=>x.Id==addressId);

            if (checkAddress == null || userAddress == null)
            {
                
                return false;

            }

            userAddress.ShippingAddresses.Remove(checkAddress);

            await _context.SaveChangesAsync();
            return true;


        }
    }
}
