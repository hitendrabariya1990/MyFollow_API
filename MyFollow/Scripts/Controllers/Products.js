var AppRoute = angular.module('AppRoute', []);
AppRoute.controller('ProductController', ['$scope', '$http', '$routeParams', ProductController]);

function ProductController($scope, $http,$routeParams) {
    $scope.loading = false;
    $scope.addMode = false;
    $scope.getIframeSrc = function (src) {
        return 'https://www.youtube.com/embed/' + src;
    };
    //Add Products
    $scope.addProducts = function () {
        $scope.loading = false;
        var poid = $('.abc').val().toString();
        var txtimg = $('#txtimg').val().toString();
        this.newProduct.Poid = poid;
        this.newProduct.UploadImagesName = txtimg;
        $http.post('/api/ProductsAPI', this.newProduct).success(function (data) {
           
            //$scope.addMode = false;
            //$scope.Products.push(data);
            // $scope.GetallProducts();
            alert('Product Added Sucessfully.');
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Adding ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    //Used to display the data  
    $scope.GetallProducts = function () {
        var id = $('.abc').val().toString();
        $http.get('/api/ProductsAPI/GetProductsList/?id=' + id).success(function(data) {
            $scope.ProductList = data;
            $scope.loading = false;

        });
    };

    //Get Single ProductDetails
    $scope.viewProduct = function () {
        var id = $routeParams.Id;
        $http.get('api/ProductsAPI/GetProducts/?id=' + id).success(function (data) {
            $scope.ProductData = data;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    //Delete Product
    $scope.deleteProduct = function () {
        $scope.loading = false;
        var id = $routeParams.Id;
        $http.delete('/api/ProductsAPI/DeleteProducts/?id=' + id).success(function (data) {
            alert("Product Deleted Successfully!!");
            $scope.GetallProducts();
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    $scope.EditProduct = function (ProductData) {
        $scope.loading = true;
        //var ProductData1 = this.ProductData;
        //alert(emp);
        $http.put('/api/ProductsAPI/EditProducts/?id=' + ProductData.Id, ProductData).success(function (data) {
            alert("Edit Successfully!!");
            //  emp.editMode = false;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    $scope.deleteImages = function (items) {
        $scope.loading = false;
       $http.delete('/api/ProductsAPI/DeleteImage/?id=' + items.Id).success(function (data) {
            alert("Image Deleted Successfully!!");
            $scope.ProductData = data;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };

     //Add Products
    $scope.addMainProducts = function () {
        $scope.loading = false;
        var poid = $('.abc').val().toString();
        this.newProduct.Poid = poid;
        $http.post('/api/ProductsAPI/PostMainProduct', this.newProduct).success(function (data) {
           alert('Product Added Sucessfully.');
            $scope.GetallMainProducts();
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Adding ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    $scope.GetallMainProducts = function () {
        var id = $('.abc').val().toString();
        $http.get('/api/ProductsAPI/GetMainProductsList/?id=' + id).success(function (data) {
            $scope.MainProductList = data;
            $scope.loading = false;

        });
    };

    $scope.deleteMainProduct = function (items) {
        $scope.loading = false;
       // var id = $routeParams.Id;
        $http.delete('/api/ProductsAPI/DeleteMainProducts/?id=' + items.id).success(function (data) {
            alert("Product Deleted Successfully!!");
            $scope.GetallMainProducts();
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };
}