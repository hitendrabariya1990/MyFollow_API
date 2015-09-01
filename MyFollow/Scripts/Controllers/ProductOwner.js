var AppRoute = angular.module('AppRoute', []);//set and get the angular module
AppRoute.controller('ProductOwnerController', ['$scope', '$http', ProductOwnerController]);
//var ProductOwnerController = angular.module("ProductOwnerController", []);

function ProductOwnerController($scope, $http, $location) {
    $scope.loading = false;
    $scope.addMode = false;
    $scope.ProductOwnerData = {};
    $scope.toggleEdit = function () {
        this.ProductOwner.editMode = !this.ProductOwner.editMode;
    };
    $scope.toggleAdd = function () {
        $scope.addMode = !$scope.addMode;
    };

    //Used to save a record after edit  
    $scope.save = function (ProductOwner) {
        $scope.loading = false;
        //alert(emp);
        $http.put('/api/ProductOwnersAPI/PutProductOwner/?id=' + ProductOwner.Id, ProductOwner).success(function (data) {
            alert("Edit Successfully!!");
          //  emp.editMode = false;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    //Used to add a new record  
    $scope.add = function () {
        $scope.loading = false;
        //var Email = $location.Email;
        this.newProductOwner.EmailId = "pappu.9096@gmail.com";
        $http.post('/api/ProductOwnersAPI', this.newProductOwner).success(function (data) {
            alert("Added Successfully!!");
            $scope.addMode = false;
            $scope.ProductOwners.push(data);
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Adding ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    //Used to edit a record  
    $scope.deleteProductOwner = function () {
        $scope.loading = true;
        var ProductOwnerid = this.ProductOwner.Id;
        $http.delete('/api/ProductOwnersAPI/' + id).success(function (data) {
            alert("Deleted Successfully!!");
            $.each($scope.ProductOwners, function (i) {
                if ($scope.ProductOwners[i].Id === id) {
                    $scope.ProductOwners.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };

    //Get Single Record
    $scope.viewProductOwner = function () {
        var id = $('.abc').val().toString();
           $http.get('/api/ProductOwnersAPI/GetProductOwner/?id='+ id).success(function (data) {
               $scope.ProductOwnerData = data;
                $scope.loading = true;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving ProductOwner! " + data;
            $scope.loading = false;

        });
    };
    // AppRoute.controller('ProductOwnerController', ['$scope', '$http', ProductOwnerController]);
 
}