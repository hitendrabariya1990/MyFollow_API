var AppRoute = angular.module('AppRoute', []);
AppRoute.controller('EndUserController', ['$scope', '$http', '$routeParams', EndUserController]);

function EndUserController($scope, $http, $routeParams) {
    $scope.loading = false;
    $scope.addMode = false;

 
    //Used to display the data  
    $scope.GetallProductsList = function () {
        $http.get('/api/EndUserAPI/GetProductsList').success(function (data) {
            $scope.ProductList = data;
            $scope.loading = false;

        })
    };

    //Get Single ProductDetails
    $scope.viewProductDetails = function () {
        var id = $routeParams.Id;
        $http.get('api/EndUserAPI/GetProductsDetails/?id=' + id).success(function (data) {
            $scope.ProductData = data;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };
  
}