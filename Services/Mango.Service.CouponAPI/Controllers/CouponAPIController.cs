﻿using AutoMapper;
using Mango.Service.CouponAPI.Data;
using Mango.Service.CouponAPI.Models;
using Mango.Service.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.CouponAPI.Controllers
{
	[Route("api/coupon")]
	[ApiController]
	[Authorize]
	public class CouponAPIController : ControllerBase
	{
		private readonly AppDbContext _db;
		private ResponseDto _response;
		private IMapper _mapper;

		public CouponAPIController(AppDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
			_response = new ResponseDto();
		}

		[HttpGet]
		public ResponseDto Get()
		{
			try
			{
				IEnumerable<Coupon> objList = _db.Coupons.ToList();
				_response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;

			}
			return _response;
		}

		[HttpGet]
		[Route("{id:int}")]
		public ResponseDto Get(int id)
		{
			try
			{
				Coupon obj = _db.Coupons.First(u => u.CouponId == id);
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpGet]
		[Route("GetByCode/{code}")]
		public ResponseDto GetByCode(string code)
		{
			try
			{
				Coupon obj = _db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());				
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpPost]
		[Authorize(Roles ="ADMİN")]
		public ResponseDto Post([FromBody] CouponDto couponDto)
		{
			try
			{
				Coupon obj = _mapper.Map<Coupon>(couponDto);
				_db.Coupons.Add(obj);
				_db.SaveChanges();


				var options = new Stripe.CouponCreateOptions 
				{ 
					 
					AmountOff = (long)(couponDto.DiscountAmount*100), 
					Name = couponDto.CouponCode, 
					Currency="usd",
				    Id = couponDto.CouponCode
				};
				var service = new Stripe.CouponService();
				Stripe.Coupon coupon = service.Create(options);


				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpPut]
		[Authorize(Roles = "ADMİN")]

		public ResponseDto Put([FromBody] CouponDto couponDto)
		{
			try
			{
				Coupon obj = _mapper.Map<Coupon>(couponDto);
				_db.Coupons.Update(obj);
				_db.SaveChanges();
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpDelete]
		[Route("{id:int}")]
		[Authorize(Roles = "ADMİN")]
		public ResponseDto Delete(int id)
		{
			try
			{
				Coupon obj = _db.Coupons.First(u=> u.CouponId==id);
				_db.Coupons.Remove(obj);
				_db.SaveChanges();


				var service = new Stripe.CouponService();
				Stripe.Coupon coupon = service.Delete(obj.CouponCode);


			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}
	}
}
