// create the module and name it scotchApp
var pastemixApp = angular.module('pastemixApp', ['ngRoute']);

// configure our routes
pastemixApp.config(function ($routeProvider) {
    $routeProvider
        .when('/',
            {
                templateUrl: '/Home/List',
                controller: 'mainController'
            })

        // route for the about page
        .when('/create',
            {
                templateUrl: '/Home/CreatePost',
                controller: 'createController'
            })
        .when('/read/:id',
            {
                templateUrl: '/Home/ReadPost',
                controller: 'readController'
            });
});

// create the controller and inject Angular's $scope
pastemixApp.controller('mainController', function ($scope, $http) {
    $http.get("/Post/GetPosts")
         .then(function(responce) {
                $scope.posts = responce.data;
         });
});

pastemixApp.controller('createController', function ($scope, $http, $location) {
    $scope.submit = function () {
        $http
            .put("/Post/CreatePost", {}, { params: { Title: $scope.title, Text: $scope.text, TimeLimit: $scope.timeLimit } })
            .then(function (responce) {
                if (responce.data.error !== "") {
                    alert(responce.data.error);
                } else {
                    $location.path('/');
                }
            });
    };
});

pastemixApp.controller('readController', function ($scope, $http, $route, $routeParams) {
    $http.get("/Post/GetPost/" + $routeParams.id) 
        .then(function (responce) {
            $scope.post = responce.data;
        });
});