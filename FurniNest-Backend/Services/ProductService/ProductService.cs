﻿using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FurniNest_Backend.DataContext;
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;
using FurniNest_Backend.Services.CloudinaryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FurniNest_Backend.Services.ProductService
{
    public class ProductService:IProductService
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        
        public ProductService(AppDbContext context,ICloudinaryService cloudinaryService,IMapper mapper,ILogger<ProductService> logger) 
        { 
            _context = context;
            _cloudinaryService = cloudinaryService;
             _mapper = mapper;
            _logger = logger;
        }


        public async Task<int> AddProduct(AddProductDTO newProduct,IFormFile image)
        {

            if (newProduct == null) {

                throw new ArgumentNullException(nameof(newProduct), "Product cannot be null");
                
            }

            var categoryCheck = await _context.Categories.FirstOrDefaultAsync(x=>x.CategoryId==newProduct.CategoryId);

            if (categoryCheck == null) {
                throw new InvalidOperationException("Category with this Id doesn't exist");



            }
            var categoryExist = _context.Categories.FirstOrDefault(x => x.CategoryId == newProduct.CategoryId);
             if (categoryExist == null) throw new Exception("Category with this Id doesn't exist");

            try
            {
                var imgUrl = await _cloudinaryService.UploadProductImage(image);
                var AddingProduct = _mapper.Map<Product>(newProduct);
                AddingProduct.Image = imgUrl;
                await _context.Products.AddAsync(AddingProduct);
                await _context.SaveChangesAsync();
                return AddingProduct.ProductId;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {

                throw new Exception($"An Exception has been occuured while creating new product {ex.Message}");
            }


        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            try
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if (existingProduct == null)
                {
                    throw new InvalidOperationException("Wrong Product Id");

                }

                var viewProduct = _mapper.Map<ProductDTO>(existingProduct);

                return viewProduct;



            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);

            }
            catch (Exception ex) {
                throw new Exception($"An Exception has been occuured while retrieving product using ID, {ex.Message}");
            }



        }

        public async Task<bool> UpdateProduct(int id, AddProductDTO updtProduct ,IFormFile image=null)
        {

            try
            {
                if (updtProduct == null)
                {
                    throw new ArgumentNullException("Update Product cannot be null");
                }
                var existProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == updtProduct.CategoryId);
                if (categoryExist == null)
                {
                    throw new KeyNotFoundException("Category with the provided ID not found, unable to modify product.");
                }

                
                //if (existProduct == null)
                //{
                //    throw new KeyNotFoundException("Product with the provided ID not found, unable to modify product.");
                //}

                if(existProduct!=null)
                {

                    existProduct.Name = updtProduct.Name;
                    existProduct.Price = updtProduct.Price;
                    existProduct.Rating = updtProduct.Rating;
                    existProduct.CategoryId = updtProduct.CategoryId;
                    existProduct.Brand = updtProduct.Brand;


                    if (image != null && image.Length > 0)
                    {
                        try
                        {
                            var imgUrl = await _cloudinaryService.UploadProductImage(image);
                            existProduct.Image = imgUrl;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error uploading image: " + ex.Message);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception($"Product with {id} not found");
                }
            }
            catch (DbException ex)
            {
               
                throw new Exception("Database error occurred: " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An exception occurred while updating the product: {ex.Message}");
            }







        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {

            var products = await _context.Products.Include(x=>x.Category).ToListAsync();

            if (products.Count > 0)
            {

                var allProduct = products.Select(x =>
                new ProductDTO
                {
                    Name = x.Name,
                    Price = x.Price,
                    Brand = x.Brand,
                    Image = x.Image,
                    Category = x.Category.Name,
                    Rating = x.Rating
                }
                ).ToList();
                return allProduct;
            }
            return new List<ProductDTO>();

        }






    }
}