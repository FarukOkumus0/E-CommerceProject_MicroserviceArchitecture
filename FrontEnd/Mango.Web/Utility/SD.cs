namespace Mango.Web.Utility
{
	public class SD
	{
		public static string CouponAPIBase { get; set; }
		public static string ProductAPIBase { get; set; }
		public static string AuthAPIBase { get; set; }
		public static string ShoppingCartAPIBase { get; set; }
		public static string OrderAPIBase { get; set; }

		public const string RoleAdmin = "ADMİN";
		public const string RoleCustomer = "MÜŞTERİ";
		public const string TokenCookie = "JWTToken";

		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}

		public const string Status_Pending = "Sipariş Alındı";
		public const string Status_Approved = "Onaylandı";
		public const string Status_ReadyForPickUp = "Teslim İçin Hazır";
		public const string Status_Completed = "Tamamlandı";
		public const string Status_Refunded = "Kayıp";
		public const string Status_Cancelled = "İptal Edildi";

		public enum ContentType
		{
			Json,
			MultipartFormData,
		}

	}
}
