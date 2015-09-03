var AppRoute = angular.module('AppRoute', []);
AppRoute.controller('EndUserController', ['$scope', '$http', '$routeParams', EndUserController]);

function EndUserController($scope, $http, $routeParams) {
    $scope.loading = false;
    $scope.addMode = false;
    $scope.getIframeSrc = function (src) {
        return 'https://www.youtube.com/embed/' + src;
    };
    $scope.FollowProducts = function (items) {

        $http.post('/api/EndUserAPI', items).success(function (data) {
            $scope.GetallProductsList();
            $scope.loading = false;

        });
    };

    $scope.UnFollowProducts = function (items) {
      
        $http.delete('/api/EndUserAPI/UnFollowProducts/?id=' + items.Id).success(function (data) {
            $scope.GetallProductsList();
            $scope.loading = false;

        });
    };

    //Used to display the data  
    $scope.GetallProductsList = function () {
        $http.get('/api/EndUserAPI/GetProductsList').success(function(data) {
            $scope.ProductList = data;
            $scope.loading = false;

        });
    };

    $scope.GetallFollowProductsList = function () {
        $http.get('/api/EndUserAPI/GetFollowProductList').success(function (data) {
            $scope.FollowProductList = data;
            $scope.loading = false;

        });
    };

    //Get Single ProductDetails
    $scope.viewFollowProductDetails = function () {
        var id = $routeParams.Id;
        $http.get('/api/EndUserAPI/GetFollowProductsDetails/?id=' + id).success(function (data) {
            $scope.ProductData = data;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };
  
}