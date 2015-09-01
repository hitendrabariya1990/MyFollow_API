var AppRoute = angular.module('AppRoute', ['ngRoute']);
AppRoute.controller('ProductOwnerController', ['$scope', '$http', ProductOwnerController]);
AppRoute.controller('ProductController', ['$scope', '$http', '$routeParams', ProductController]);
AppRoute.controller('EndUserController', ['$scope', '$http', '$routeParams', EndUserController]);
var configFunction = function ($routeProvider) {
    $routeProvider.
        when('/NewProductOwner', {
            templateUrl: 'ProductOwner/Create.html',
            controller: "ProductOwnerController"
            
        })
        .when('/ViewProductOwner/:Id', {
            templateUrl: 'ProductOwner/Details.html',
            controller: "ProductOwnerController"
        })
        .when('/EditProductOwner/:Id', {
            templateUrl: 'ProductOwner/Edit.html',
            controller: "ProductOwnerController"
        })
        .when('/AddNewProduct/:Id', {
            templateUrl: 'ProductOwner/AddProduct.html',
            controller: "ProductController"
        })
        .when('/ListProduct/:Id', {
            templateUrl: 'ProductOwner/ListProduct.html',
            controller: "ProductController"
        })
        .when('/EditProduct/:Id', {
            templateUrl: 'ProductOwner/EditProduct.html',
            controller: "ProductController"
        })
        .when('/ViewProduct/:Id', {
            templateUrl: 'ProductOwner/ViewProduct.html',
            controller: "ProductController"
        })
        .when('/DeleteProduct/:Id', {
            templateUrl: 'ProductOwner/ListProduct.html',
            controller: "ProductController"
        })
        .when('/DeleteImages/:Id', {
            templateUrl: 'ProductOwner/ViewProduct.html',
            controller: "ProductController"
        })
        .when('/ListProductData', {
            templateUrl: 'EndUser/ProductList.html',
            controller: "EndUserController"
        })
    .otherwise('/List', {
        templateUrl: 'EndUser/ProductList.html',
        controller: "EndUserController"
       
    })
    ;
}
configFunction.$inject = ['$routeProvider'];

AppRoute.config(configFunction);
