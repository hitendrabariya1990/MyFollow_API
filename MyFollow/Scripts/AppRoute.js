"use strict";

var AppRoute = angular.module('AppRoute', ['ngRoute']);
AppRoute.controller('ProductOwnerController', ['$scope', '$http', ProductOwnerController]);
AppRoute.controller('ProductController', ['$scope', '$http', '$routeParams', ProductController]);
AppRoute.controller('EndUserController', ['$scope', '$http', '$routeParams', EndUserController]);
var configFunction = function ($routeProvider, $locationProvider) {
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
        .when('/AddNewMainProduct/:Id', {
            templateUrl: 'ProductOwner/AddMainProduct.html',
            controller: "ProductController"
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
        .when('/DeleteImage/:Id', {
            templateUrl: 'ProductOwner/EditProduct.html',
            controller: "ProductController"
        })
        .when('/ListProductData', {
            templateUrl: 'ProductList.html',
            controller: "EndUserController"
        })
        .when('/ListFollowProducts', {
            templateUrl: 'FollowProductList.html',
            controller: "EndUserController"
        })
         .when('/ViewProductData/:Id', {
             templateUrl: 'ProductDetails.html',
             controller: "EndUserController"
         })
    .otherwise('/', {
        templateUrl: 'Index.html',
        controller: "EndUserController"
    });
    
}
configFunction.$inject = ['$routeProvider', '$locationProvider'];
AppRoute.config(configFunction);

AppRoute.config(function ($sceDelegateProvider) {
    $sceDelegateProvider.resourceUrlWhitelist([
      'self',
      'https://www.youtube.com/**'
    ]);
});
