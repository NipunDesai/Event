/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular-resource.d.ts" />


interface IeventControllerScope extends ng.IScope {
    getAllEventList: Function;
    openDatepickerDialog: Function;
    opened: boolean;
    mytime: any;
    time1: Date;
    showMeridian: boolean;
    hourStep: number;
    minuteStep: number;
    createNewEvent: (event: Model.Event, date: any) => void;
    location: any;
    event: Model.Event;
    eventButton: boolean;
    eventCollection : any;
    eventChange: Function;
    eventSelection:boolean;
}

interface IeventController {

}

class eventController implements IeventController {
    static controllerId = "eventController";

    constructor(private $scope: IeventControllerScope, public eventServices: IeventServices, public $rootScope, private $location: ng.ILocationService) {
        this.$scope.getAllEventList = () => this.getAllEventList();
        this.$scope.openDatepickerDialog = ($event: any) => this.openDatepickerDialog($event);
        this.$scope.opened = false;
        this.$scope.mytime = "";
        this.$scope.time1 = new Date();
        this.$scope.showMeridian = true;
        this.$scope.hourStep = 1;
        this.$scope.minuteStep = 15;
        this.$scope.createNewEvent = (event: Model.Event, date: any) => this.createNewEvent(event, date);
        this.$scope.location = '';
        this.$scope.event = new Model.Event();
        this.$scope.eventButton = false;
        this.$scope.eventCollection = [];
        this.$scope.eventChange = (status: number, eventId: number) => this.eventChange(status, eventId);
        //this.$scope.eventSelect = 0;
        this.$scope.eventSelection = false;
    }
    private getAllEventList() {
        var controllerscope = this.$scope;
        var promise = this.eventServices.getAllEventList();
        controllerscope.eventCollection = [];
        promise.then((result) => {
            console.log("get all event list");
            for (var i = 0; i < result.length; i++) {
                controllerscope.eventCollection.push(result[i]);
            }
        });
    }
    private openDatepickerDialog($event) {
        $event.preventDefault();
        $event.stopPropagation();

        this.$scope.opened = true;

    }

    private createNewEvent(event: Model.Event, date) {
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
            promise.then(() => {
                console.log('create new event.');
                this.$location.path('/EventHomePage');
            });
     
        }
       
    }

    private eventChange(status, eventId) {
       // var eventJoin = new Model.EventJoinAc();
       // eventJoin.Status = status;
       // eventJoin.EventId = eventId;
        this.$scope.eventSelection = false;

        var controllerScope = this.$scope;
        var promise = this.eventServices.eventChange(status,eventId);
        promise.then((result) => {
            console.log('Event change successfully.');
            for (var i = 0; i < controllerScope.eventCollection.length; i++) {
                if (controllerScope.eventCollection[i].EventId == eventId) {
                    controllerScope.eventCollection[i].GoingCount = result.GoingCount;
                    controllerScope.eventCollection[i].MayBe = result.MayBe;
                    break;
                }
            }
        });
    }
}

app.controller(eventController.controllerId, ['$scope', 'eventServices', '$rootScope', '$location', ($scope, eventServices, $rootScope, $location) => {
    return new eventController($scope, eventServices, $rootScope, $location);
}

]);
