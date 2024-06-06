


function RegisterPost() {
    event.preventDefault();
    $.ajax({
        method: "POST",
        url: "/Home/registerPost",
        data: $('#RegisterForm').serialize(),


        success: function (result) {
            if (result.code == 401) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Account created",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    // Redirect to the login page
                    window.location.href = '/Home/Index';
                });
            }
            else if (result.code == 402) {
                Swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: "Account already Exist",
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}
function SearchParamsbooks() {
    event.preventDefault();

    let checkboxes = document.querySelectorAll(".autherData:checked");
    let checkboxes2 = document.querySelectorAll(".categorytdata:checked");
    let checkboxes3 = document.querySelectorAll(".publisherdata:checked");
    const search2 = [];

    var formData = new FormData($('#searchForm')[0]); 
    checkboxes.forEach(function (checkbox) {
        formData.append('search2', checkbox.value);
        
    });
    checkboxes2.forEach(function (checkbox) {
        formData.append('search3', checkbox.value);
        
    });
    checkboxes3.forEach(function (checkbox) {
        formData.append('search4', checkbox.value);
        
    });

    console.log(search2); // Output the selected values to console


    console.log(formData);
    $.ajax({
        method: "POST",
        url: "/Home/searchBooks",
        contentType: false,
        processData: false,
        data: formData, // Convert formData to query string
        success: function (result) {
            $('#custDashboard').html(result);

        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}
function getcustDash() {
    $.ajax({
        method: "POST",
        url: "/Home/getcustDash",
        data: $('#searchForm').serialize(),

        success: function (result) {
            $('#custDashboard').html(result)
            $('#UserProfile').empty()
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}
function GetUserProfile() {
    $.ajax({
        method: "GET",
        url: "/Home/GetUserProfile",

        success: function (result) {
            $('#custDashboard').empty()
            $('#UserProfile').html(result)
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function editUserProfile() {
    event.preventDefault();
    $.ajax({
        method: "POST",
        url: "/Home/editUserProfile",
        data: $('#UserProfileData').serialize(),


        success: function (result) {
            if (result.code == 401) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Profile updated",
                    showConfirmButton: false,
                    timer: 1500
                })
               
            }
            else if (result.code == 402) {
                Swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: "error occured",
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}
function ViewBooksPage(bookId) {
    $.ajax({
        method: "GET",
        url: "/Home/ViewBooksPage",
        data: { bookId: bookId },
        success: function (result) {
            $('#custDashboard').empty()
            $('#UserProfile').empty()
            $('#UserProfile').html(result)
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function GetOrderDatailsPage(bookId) {
    $.ajax({
        method: "GET",
        url: "/Home/GetOrderDatailsPage",
        data: { bookId: bookId },

        success: function (result) {
            $('#ModalAction').html(result);
            $('#orderDetailsModal').modal('show');
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function getAddToCart(bookId, cartId) {
    event.preventDefault();

    $.ajax({
        method: "GET",
        url: "/Home/getAddToCart",
        data: { bookId: bookId, cartId: cartId },


        success: function (result) {
            if (result.code == 401) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: " Added To Cart",
                    showConfirmButton: false,
                    timer: 1500
                })
                ViewBooksPage(bookId);
            }

        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function getRemoveFromCart(bookId ,cartId) {
    event.preventDefault();
    Swal.fire({
        title: "Are you sure?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, remove It"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                method: "GET",
                url: "/Home/getRemoveFromCart",
                data: { cartId: cartId },

                success: function (result) {
                   
                        Swal.fire({
                            title: "Deleted!",
                            text: "Removed",
                            icon: "success",
                            showConfirmButton: false,
                            timer: 1500

                        });
                    ViewBooksPage(bookId); 

                },
                error: function () {
                    alert('Error loading partial view');
                }
            });
        }
    });
   
}
function getOrderConfirmation() {
    event.preventDefault();
    $.ajax({
        method: "POST",
        url: "/Home/getOrderConfirmation",
        data: $('#confirmOrderForm').serialize(),


        success: function (result) {

            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Order Confirmed",
                showConfirmButton: false,
                timer: 1500
            })
            ViewBooksPage(result);

        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function AdminDashboard() {
    $.ajax({
        method: "get",
        url: "/Admin/AdminDashboard2",
       

        success: function (result) {
            $('#AdminDash').html(result)
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}
function AdminBookList() {
    $.ajax({
        method: "get",
        url: "/Admin/getAdminBookList",


        success: function (result) {
            $('#AdminDash').html(result)
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function Addbook() {
    event.preventDefault();
    var formdata = new FormData($('#addBookdetails')[0]); 

    $.ajax({
        method: "POST",
        url: "/Admin/AddBook",
        data: formdata,
        contentType: false,
        processData:false,
        success: function (result) {
            if (result.code == 401) {
                swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: "Book already exist",
                    showConfirmButton: false,
                    timer: 1500
                })
            } else if (result.code == 402) {
                swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Book Added",
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}


function resetpassword() {
    event.preventDefault();

    $.ajax({
        method: "POST",
        url: "/Home/ResetPassword",
        data: $('#resetForm').serialize(),

        success: function (result) {
            if (result.code == 401) {
                swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "password changed",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    window.location.href = "/Home/Logout";

                });




            } else {
                swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Error",
                    showConfirmButton: false,
                    timer: 1500
                })
                getResetPasswordPage(result.email)
            }
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}


