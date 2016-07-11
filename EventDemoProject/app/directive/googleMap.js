app.directive('googleMap', function () {
    return {
        restrict: 'EA',
        require: '?ngModel',
        scope: {
            myModel: '=ngModel'
        },
        link: function (scope, element, attrs, ngModel) {

            var mapOptions;
            var googleMap;
            var searchMarker;
            var searchLatLng;

            ngModel.$render = function () {
                searchLatLng = new google.maps.LatLng(scope.myModel.latitude, scope.myModel.longitude);

                mapOptions = {
                    center: searchLatLng,
                    zoom: 15,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                googleMap = new google.maps.Map(element[0], mapOptions);

                searchMarker = new google.maps.Marker({
                    position: searchLatLng,
                    map: googleMap,
                    draggable: false
                });

                google.maps.event.addListener(searchMarker, 'dragend', function () {
                    scope.$apply(function () {
                        scope.myModel.latitude = searchMarker.getPosition().lat();
                        scope.myModel.longitude = searchMarker.getPosition().lng();
                    });
                }.bind(this));

            };

            scope.$watch('myModel', function (value) {
                var myPosition = new google.maps.LatLng(scope.myModel.latitude, scope.myModel.longitude);
                searchMarker.setPosition(myPosition);
            }, true);
        }
    }
});

