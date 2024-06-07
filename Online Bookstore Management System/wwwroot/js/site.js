


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



    console.log(formData);
    $.ajax({
        method: "POST",
        url: "/Home/searchBooks",
        contentType: false,
        processData: false,
        data: formData, 
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
                $('#addBookModal').modal('hide').on('hidden.bs.modal', function () {
                    AdminBookList();
                });
            }
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}
function Updatebook() {
    event.preventDefault();
    var formdata = new FormData($('#addBookdetails')[0]);

    $.ajax({
        method: "POST",
        url: "/Admin/updateBook",
        data: formdata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.code == 401) {
                swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Book Updated",
                    showConfirmButton: false,
                    timer: 1500
                })
                $('#addBookModal').modal('hide').on('hidden.bs.modal', function () {
                    AdminBookList();
                });


               
            } else if (result.code == 402) {
                swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: "Book not exist",
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
function DeleteBooksPage(bookId) {
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
                url: "/Admin/getDeleteBook",
                data: { bookId: bookId },

                success: function (result) {
                    if (result.code==401)
                        Swal.fire({
                            title: "Deleted!",
                            text: "Removed",
                            icon: "success",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            AdminBookList();
                        });


                },
                error: function () {
                    alert('Error loading partial view');
                }
            });
        }
    });

}


function GetAdminProfile() {
    $.ajax({
        method: "GET",
        url: "/Admin/getAdminProfile",

        success: function (result) {
            $('#AdminDash').empty()
            $('#AdminDash').html(result)
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function editAdminProfile() {
    event.preventDefault();
    $.ajax({
        method: "POST",
        url: "/Admin/editAdminProfile",
        data: $('#AdminProfileData').serialize(),


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

function GetAuthersList() {
    $.ajax({
        method: "GET",
        url: "/Admin/getAutherList",

        success: function (result) {
            $('#AdminDash').empty()
            $('#AdminDash').html(result)
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function GetCategoryList() {
    $.ajax({
        method: "GET",
        url: "/Admin/getCategoryList",

        success: function (result) {
            $('#AdminDash').empty()
            $('#AdminDash').html(result)
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function getAddorUpdateAuther(authorId) {
    $.ajax({
        method: "get",
        url: "/Admin/getAddAuthor",
        data: { authorId: authorId },

        success: function (result) {

            $('#AuthorModal').html(result);
            $('#addAuthorModal').modal('show');

        },
        error: function () {
            console.error('Error loading partial view');
            alert('Error loading partial view');
        }
    });
}
function DeleteAuther(authorId) {
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
                url: "/Admin/getDeleteAuthor",
                data: { authorId: authorId },

                success: function (result) {
                    if (result.code == 401)
                        Swal.fire({
                            title: "Deleted!",
                            text: "Removed",
                            icon: "success",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            GetAuthersList();
                        });


                },
                error: function () {
                    alert('Error loading partial view');
                }
            });
        }
    });

}

function getAddorUpdateCategory(categoryId) {
    $.ajax({
        method: "get",
        url: "/Admin/getAddCategory",
        data: { categoryId: categoryId },

        success: function (result) {

            $('#CategoryModal').html(result);
            $('#addCategoryModal').modal('show');

        },
        error: function () {
            console.error('Error loading partial view');
            alert('Error loading partial view');
        }
    });
}

function DeleteCategory(categoryId) {
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
                url: "/Admin/getDeleteCategory",
                data: { categoryId: categoryId },

                success: function (result) {
                    if (result.code == 401)
                        Swal.fire({
                            title: "Deleted!",
                            text: "Removed",
                            icon: "success",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            GetCategoryList();
                        });


                },
                error: function () {
                    alert('Error loading partial view');
                }
            });
        }
    });

}

function AddOrUpdateAuthor() {
    event.preventDefault();

    $.ajax({
        method: "POST",
        url: "/Admin/addOrUpdateAuthor",
        data: $('#addUpdateAuthorForm').serialize(),
   
        success: function (result) {
            if (result.code == 401) {
                swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Done",
                    showConfirmButton: false,
                    timer: 1500
                })
                $('#addAuthorModal').modal('hide').on('hidden.bs.modal', function () {
                    GetAuthersList();
                });



            } else if (result.code == 402) {
                swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: "Auther exist",
                    showConfirmButton: false,
                    timer: 1500
                })
                $('#addAuthorModal').modal('hide');
                GetAuthersList();

            }
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}

function AddOrUpdateCategory() {
    event.preventDefault();

    $.ajax({
        method: "POST",
        url: "/Admin/addOrUpdateCategory",
        data: $('#addUpdateCategoryForm').serialize(),

        success: function (result) {
            if (result.code == 401) {
                swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Done",
                    showConfirmButton: false,
                    timer: 1500
                })
                $('#addCategoryModal').modal('hide').on('hidden.bs.modal', function () {
                    GetCategoryList();
                });



            } else if (result.code == 402) {
                swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: "Auther exist",
                    showConfirmButton: false,
                    timer: 1500
                })
                $('#addCategoryModal').modal('hide')
                GetCategoryList();

            }
        },
        error: function () {
            alert('Error loading partial view');
        }
    });
}
