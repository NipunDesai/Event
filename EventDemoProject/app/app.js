/// <reference path="../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-resource.d.ts" />
var apiPaths = {
    getAllEventList: 'api/Event/getAllEventList',
    createNewEvent: 'api/Event/createNewEvent',
    eventChange: 'api/Event/eventChange'
};
// Create the module and define its dependencies.
var app = angular.module('app', [
    // Angular modules 
    'ngResource',
    // animations
    'ngRoute',
    'ui.bootstrap'
]);
app.config(function ($routeProvider) {
    $routeProvider.when('/EventHomePage', {
        templateUrl: 'Templates/EventHome.html'
    }).when('/CreateNewEvent', {
        templateUrl: 'Templates/CreateNewEvent.html'
    });
});
//# sourceMappingURL=app.js.map