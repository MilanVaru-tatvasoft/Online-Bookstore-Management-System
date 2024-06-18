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
        public Customer_MainPage getdata(Customer_MainPage model, int? userId, int pageNumber);
        public List<DashboardList> getTableData( int? userId, int pageNumber);

        public Admin getAdminData(string email);
        public Customer getCustomerData(string email);
        public bool registerPost(RegisterVm model);
        public UserProfile getUserProfile(int? uId);
        public bool editUserProfile(UserProfile profile);
        public viewBookModel viewBookDetails(int bookId, int? userId);
        public OrderData getOrderDetails(int bookId, int? userId);
        public void GetAddToCart(int bookId, int? userId, int cartId, int quantity);
        public void GetRemoveFromCart(int cartId, int? userId);
        public int confirmOrder(OrderData data, int? userId);
        public OrderData getCartList(int? UserId);
        public void getSubmitReviewAndRating(viewBookModel model, int? userId);
        public bool getPaymentDone(string paymentType, int OrderId, int? userId);
        public PaymentBillDetails getBillDetails(int orderId);
        public OrderData GetOrderHistoy(int? userId);



    }
}
