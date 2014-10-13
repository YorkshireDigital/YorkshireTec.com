
/*!
 * angular-clndr 0.2.0
 * 10KB, http://10kb.nl/
 * License: MIT
 */

(function() {
  var module;

  module = angular.module('tien.clndr', []);

  module.directive('tienClndr', function() {
    return {
      restrict: 'E',
      replace: true,
      transclude: true,
      scope: {
        clndr: '=tienClndrObject',
        events: '=tienClndrEvents'
      },
      controller: ["$scope", "$element", "$attrs", "$transclude", function($scope, $element, $attrs, $transclude) {
        return $transclude(function(clone, scope) {
          var render;
          $element.append(clone);
          $scope.$watch('events', function(val) {
            if (val != null ? val.length : void 0) {
              return $scope.clndr.setEvents(angular.copy(val));
            }
          });
          render = function(data) {
            return angular.extend(scope, data);
          };
          return $scope.clndr = angular.element("<div/>").clndr({
            render: render,
            startWithMonth: moment().format('YYYY-MM'),
            forceSixRows: true,
            multiDayEvents: {
                startDate: 'start',
                endDate: 'end'
            },
            daysOfTheWeek: [
              '<span class="header-day__sm">S</span><span class="header-day__md">Sun</span><span class="header-day__lg">Sunday</span>',
              '<span class="header-day__sm">M</span><span class="header-day__md">Mon</span><span class="header-day__lg">Monday</span>',
              '<span class="header-day__sm">T</span><span class="header-day__md">Tue</span><span class="header-day__lg">Tuesday</span>',
              '<span class="header-day__sm">W</span><span class="header-day__md">Wed</span><span class="header-day__lg">Wednesday</span>',
              '<span class="header-day__sm">T</span><span class="header-day__md">Thu</span><span class="header-day__lg">Thursday</span>',
              '<span class="header-day__sm">F</span><span class="header-day__md">Fri</span><span class="header-day__lg">Friday</span>',
              '<span class="header-day__sm">S</span><span class="header-day__md">Sat</span><span class="header-day__lg">Saturday</span>'
            ]
          });
        });
      }]
    };
  });

}).call(this);
