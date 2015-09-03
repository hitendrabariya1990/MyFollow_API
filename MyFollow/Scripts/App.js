"use strict";

angular.module("App", ["ngRoute"]).
  config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {
      $routeProvider
        .when("/", { templateUrl: "/Home/Home" })
        .when("/contact", { templateUrl: "/Home/Contact" })
        .otherwise({ redirectTo: "/" });

      $locationProvider.html5Mode(false).hashPrefix("!");
  }]);