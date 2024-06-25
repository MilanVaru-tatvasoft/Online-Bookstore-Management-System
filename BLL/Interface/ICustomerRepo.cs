using DataAccess.CustomModels;
using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface ICustomerRepo
    {
        public CustomerMainPage GetCustomerDashboardData(CustomerMainPage model, int? userId, int pageNumber);
        public List<DashboardList> GetCustomerDashboardTable(CustomerMainPage model, int? userId, int pageNumber);


        public Admin GetAdminData(string email);
        public Customer GetCustomerData(string email);
        public bool RegisterPost(UserProfile model);
        public UserProfile GetUserProfile(int? uId);
        public bool EditUserProfile(UserProfile profile);
        public viewBookModel ViewBookDetails(int bookId, int? userId);
        public OrderData GetOrderDetails(int bookId, int? userId);
        public void GetAddToCart(int bookId, int? userId, int cartId, int quantity);
        public void GetRemoveFromCart(int cartId, int? userId);
        public int confirmOrder(OrderData data, int? userId);
        public OrderData GetCartList(OrderData model,int? UserId);

        public void GetSubmitReviewAndRating(viewBookModel model, int? userId);
        public bool GetPaymentDone(string paymentType, int OrderId, int? userId);

        public PaymentBillDetails getBillDetails(int orderId);
        public OrderData GetOrderHistoy(int? userId);
        public FavoriteModel GetFavoritesPageData(int? userId);
        public bool FavoriteAction(string actionType, int bookId, int? userId);




    }
}
