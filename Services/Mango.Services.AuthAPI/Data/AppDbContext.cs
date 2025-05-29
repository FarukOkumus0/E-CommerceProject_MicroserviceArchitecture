using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//modelBuilder.Entity<Coupon>().HasData(new Coupon
			//{
			//	CouponId = 1,
			//	CouponCode = "10OFF",
			//	DiscountAmount = 10,
			//	MinAmount = 40
			//});

			//modelBuilder.Entity<Coupon>().HasData(new Coupon
			//{
			//	CouponId = 2,
			//	CouponCode = "20OFF",
			//	DiscountAmount = 20,
			//	MinAmount = 40
			//});
		}
	}
}
