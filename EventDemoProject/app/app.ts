/// <reference path="../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-resource.d.ts" />

var apiPaths = {
    getAllEventList: 'api/Event/getAllEventList',
    createNewEvent: 'api/Event/createNewEvent',
    eventChange:'api/Event/eventChange'
};

interface Iapp extends ng.IModule { }

// Create the module and define its dependencies.
var app: Iapp = angular.module('app', [
    // Angular modules 
    'ngResource',       // $resource for REST queries
          // animations
    'ngRoute'  ,         // routing
    'ui.bootstrap'
    // Custom modules 

    // 3rd Party Modules
]);


app.config(function ($routeProvider) {
    $routeProvider.when('/EventHomePage', {
        templateUrl:'Templates/EventHome.html'
    }).when('/CreateNewEvent', {
        templateUrl: 'Templates/CreateNewEvent.html' 
    });
});
