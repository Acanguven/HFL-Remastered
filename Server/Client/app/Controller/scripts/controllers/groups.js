'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:groups
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('groups', function (Websocket, $scope, $http, $state, $stateParams, $rootScope, $interval) {
    if (!$rootScope.authenticated) {
        $state.go("login");
        return;
    }
    NET.resize(1160, 750);
    NET.disableMove();
    $rootScope.disableMove = true;
    var initDone = 0;

    NET.loadSmurfs(function (smurfs) {
        $scope.smurfs = smurfs;
        initDone++;
    });

    $rootScope.helpVideo = "https://www.youtube.com/watch?v=_Yhyp-_hX2s";

    NET.loadGroups(function (groups) {
        $scope.groups = groups;
        initDone++;
    });

    $scope.addGroup = function () {
        var group = {};
        group.id = Date.now().toString();
        group.name = "New Group";
        group.smurfs = [];
        group.queue = 32;
        $scope.groups.push(group);
    }

    $scope.deleteGroup = function (gr) {
        var index = $scope.groups.indexOf(gr);
        $scope.groups.splice(index, 1);
    }

    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        $(".smurfDrag").draggable({
            containment: '.container',
            cursor: 'move',
            helper: 'clone',
            zIndex: 100000,
            scroll: false,
            start: function () { },
            stop: function (event, ui) { }
        }).mousedown(function () { });
    });

    $scope.$on('smurfDropped', function (e, args) {
        var smurfIndex = $scope.smurfs.SmurfsfindByUsernameRegion($(args.ui.helper.context).attr("username"), $(args.ui.helper.context).attr("region"));
        var groupIndex = $scope.groups.GroupfindById($(args.event.target).attr("id"))
        if (smurfIndex !== false && groupIndex !== false) {
            $scope.$apply(function () {
                if ($scope.groups[groupIndex].smurfs.length > 0) {
                    if ($(args.ui.helper.context).attr("region") == $scope.groups[groupIndex].smurfs[0].region) {
                        if ($scope.groups[groupIndex].smurfs.SmurfsfindByUsernameRegion($(args.ui.helper.context).attr("username"), $(args.ui.helper.context).attr("region")) === false) {
                            $scope.groups[groupIndex].smurfs.push($scope.smurfs[smurfIndex]);
                        }
                    }
                } else {
                    if ($scope.groups[groupIndex].smurfs.SmurfsfindByUsernameRegion($(args.ui.helper.context).attr("username"), $(args.ui.helper.context).attr("region")) === false) {
                        $scope.groups[groupIndex].smurfs.push($scope.smurfs[smurfIndex]);
                    }
                }
            })
        }
    });

    $scope.remove = function (smurf, group) {
        var index = group.smurfs.indexOf(smurf);
        group.smurfs.splice(index, 1);
    }

    Array.prototype.SmurfsfindByUsernameRegion = function (username, region) {
        for (var x = 0; x < this.length; x++) {
            if (this[x].username == username && this[x].region == region) {
                return x;
            }
        }
        return false;
    }

    Array.prototype.GroupfindById = function (id) {
        for (var x = 0; x < this.length; x++) {
            if (this[x].id == id) {
                return x;
            }
        }
        return false;
    }

    $scope.$watch("groups", function () {
        if (initDone > 1) {
            NET.saveGroups($scope.groups);
        }
    }, true)


    $scope.$on("$destroy", function () {
        NET.enableMove();
        $rootScope.disableMove = false;
        $rootScope.helpVideo = false;
    });
});