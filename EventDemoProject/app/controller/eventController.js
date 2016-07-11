/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular-resource.d.ts" />
var eventController = (function () {
    function eventController($scope, eventServices, $rootScope, $location) {
        var _this = this;
        this.$scope = $scope;
        this.eventServices = eventServices;
        this.$rootScope = $rootScope;
        this.$location = $location;
        this.$scope.getAllEventList = function () { return _this.getAllEventList(); };
        this.$scope.openDatepickerDialog = function ($event) { return _this.openDatepickerDialog($event); };
        this.$scope.opened = false;
        this.$scope.mytime = "";
        this.$scope.time1 = new Date();
        this.$scope.showMeridian = true;
        this.$scope.hourStep = 1;
        this.$scope.minuteStep = 15;
        this.$scope.createNewEvent = function (event, date) { return _this.createNewEvent(event, date); };
        this.$scope.location = '';
        this.$scope.event = new Model.Event();
        this.$scope.eventButton = false;
        this.$scope.eventCollection = [];
        this.$scope.eventChange = function (status, eventId) { return _this.eventChange(status, eventId); };
        //this.$scope.eventSelect = 0;
        this.$scope.eventSelection = false;
    }
    eventController.prototype.getAllEventList = function () {
        var controllerscope = this.$scope;
        var promise = this.eventServices.getAllEventList();
        controllerscope.eventCollection = [];
        promise.then(function (result) {
            console.log("get all event list");
            for (var i = 0; i < result.length; i++) {
                controllerscope.eventCollection.push(result[i]);
            }
        });
    };
    eventController.prototype.openDatepickerDialog = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        this.$scope.opened = true;
    };
    eventController.prototype.createNewEvent = function (event, date) {
        var _this = this;
        if (this.$scope.eventButton == false) {
            var controllerScope = this.$scope;
            var today = new Date(date);
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            var min = today.getUTCMinutes();
            var sec = today.getUTCSeconds();
            var hrs = today.getUTCHours();
            var getdate = dd + '-' + mm + '-' + yyyy;
            var time = hrs + ':' + min + ':' + sec;
            var ssss = getdate + ' ' + time;
            var s = this.$scope.location;
            var ss = s.split(',');
            event.Latitude = ss[0];
            event.Longitude = ss[1];
            event.EndDateTime = ssss.toLocaleString();
            event.Location = this.$rootScope.locationDetails;
            this.$scope.eventButton = true;
            var promise = this.eventServices.createNewEvent(event);
            promise.then(function () {
                console.log('create new event.');
                _this.$location.path('/EventHomePage');
            });
        }
    };
    eventController.prototype.eventChange = function (status, eventId) {
        // var eventJoin = new Model.EventJoinAc();
        // eventJoin.Status = status;
        // eventJoin.EventId = eventId;
        this.$scope.eventSelection = false;
        var controllerScope = this.$scope;
        var promise = this.eventServices.eventChange(status, eventId);
        promise.then(function (result) {
            console.log('Event change successfully.');
            for (var i = 0; i < controllerScope.eventCollection.length; i++) {
                if (controllerScope.eventCollection[i].EventId == eventId) {
                    controllerScope.eventCollection[i].GoingCount = result.GoingCount;
                    controllerScope.eventCollection[i].MayBe = result.MayBe;
                    break;
                }
            }
        });
    };
    eventController.controllerId = "eventController";
    return eventController;
}());
app.controller(eventController.controllerId, ['$scope', 'eventServices', '$rootScope', '$location', function ($scope, eventServices, $rootScope, $location) {
        return new eventController($scope, eventServices, $rootScope, $location);
    }
]);
//# sourceMappingURL=eventController.js.map