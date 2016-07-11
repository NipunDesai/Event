/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular-resource.d.ts" />
var eventServices = (function () {
    function eventServices($resource, $q) {
        this.$resource = $resource;
        this.$q = $q;
        this.getEventList = $resource(apiPaths.getAllEventList);
        this.createEvent = $resource(apiPaths.createNewEvent);
        this.eventSelection = $resource(apiPaths.eventChange + '/:eventJoinAc/:eventId', { eventJoinAc: '@eventJoinAc', eventId: '@eventId' }, { save: { method: "POST" } });
    }
    eventServices.prototype.getAllEventList = function () {
        return this.getEventList.query().$promise;
    };
    eventServices.prototype.createNewEvent = function (event) {
        return this.createEvent.save(event).$promise;
    };
    eventServices.prototype.eventChange = function (eventJoinAc, eventId) {
        return this.eventSelection.save({ eventJoinAc: eventJoinAc, eventId: eventId }).$promise;
    };
    eventServices.serviceId = "eventServices";
    return eventServices;
}());
app.service('eventServices', ['$resource', '$q', function ($resource, $q) {
        return new eventServices($resource, $q);
    }]);
//# sourceMappingURL=eventServices.js.map