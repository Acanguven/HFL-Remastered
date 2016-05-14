'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('rotations', function (Websocket, $scope, $http, $state, $stateParams, $rootScope, $timeout) {
    if (!$rootScope.authenticated) {
        $state.go("login");
        return;
    }
    $scope.user = $rootScope.user;
    NET.disableMove();
    $rootScope.disableMove = true;
    $scope.initDone = false;
    $scope.plumbInstance = jsPlumb.getInstance({ Container: $("#diagramContainer") });

    NET.loadSmurfs(function (smurfs) {
        $scope.smurfs = smurfs;
    });

    NET.loadGroups(function (groups) {
        $scope.groups = groups;
    });

    var anchors = [
                    [1, 0.2, 1, 0],
                    [0.8, 1, 0, 1],
                    [0, 0.8, -1, 0],
                    [0.2, 0, 0, -1]
    ];

    var anchors2 = [
                    [1, 0.8, 1, 0],
                    [0.2, 1, 0, 1],
                    [0, 0.2, -1, 0],
                    [0.8, 0, 0, -1]
    ];

    var exampleColor = "#00f";

    var exampleEndpoint = {
        anchor: anchors,
        endpoint: "Dot",
        paintStyle: { width: 10, height: 10, fillStyle: "lightgreen" },
        isSource: true,
        reattach: true,
        scope: "lightgray",
        connectorStyle: {
            lineWidth: 3,
            strokeStyle: "gray",
            overlays: [
                ["Arrow", { width: 15, length: 12, location: 0.67 }]
            ]
        },
        isTarget: false,
        uuid: "out"
    };

    var exampleEndpoint2 = {
        anchor: anchors2,
        endpoint: "Rectangle",
        paintStyle: { width: 20, height: 20, fillStyle: "lightblue" },
        isSource: false,
        reattach: true,
        scope: "lightgray",
        connectorStyle: {
            lineWidth: 3,
            strokeStyle: "gray",
            overlays: [
                ["Arrow", { width: 15, length: 12, location: 0.67 }]
            ]
        },
        isTarget: true,
        uuid: "in"
    };

    $scope.plumbInstance.Defaults.Overlays = [
            ["Arrow", {
                location: 0.7,
                id: "arrow",
                length: 14,
                foldback: 0.8
            }],
    ];

    $scope.rotationList = [];

    NET.loadRotations(function (rots) {
        rots.sort(sortPos);
        $scope.rotationList = rots;
    });

    $scope.saveRequested = false;
    $scope.freshRequested = false;

    $scope.$on('ngRepeatFinished', function (ngRepeatFinishedEvent) {
        if (!$scope.initDone) {
            $scope.setupBindings();
        }
        if ($scope.freshRequested) {
            $scope.freshBindings();
            $scope.freshRequested = false;
        }
        if ($scope.saveRequested) {
            $scope.save();
            $scope.saveRequested = false;
        }
    });


    $scope.initRotations = function () {
        angular.element(document).ready(function () {
            $scope.plumbInstance.ready(function () {

                $scope.plumbInstance.bind("connection", function (ci) {
                    var eps = ci.connection.endpoints;
                    $scope.buildQueue(ci.sourceId, ci.targetId);
                    $scope.save();
                });

                $scope.plumbInstance.bind("beforeDrop", function (info) {
                    if (info.sourceId === info.targetId) { //source and target ID's are same
                        return false;
                    } else {
                        return true;
                    }
                });

                $scope.plumbInstance.bind("click", function (conn) {
                    $scope.plumbInstance.detach(conn);
                    $scope.buildQueue();
                    $scope.save();
                });
            });
        });
    }

    $scope.setupBindings = function () {
        for (var x = 0 ; x < $scope.rotationList.length; x++) {
            var div = $("#" + $scope.rotationList[x].id);
            div.css("left", $scope.rotationList[x].left + "%");
            div.css("top", $scope.rotationList[x].top + "%");
        }

        var endPointList = [];
        for (var x = 0 ; x < $scope.rotationList.length; x++) {
            var endPoints = {
                inPoint: $scope.plumbInstance.addEndpoint($("#" + $scope.rotationList[x].id), exampleEndpoint2),
                outPoint: $scope.plumbInstance.addEndpoint($("#" + $scope.rotationList[x].id), exampleEndpoint)
            }
            endPointList[($scope.rotationList[x].id).toString()] = endPoints;
        }


        for (var x = 0 ; x < $scope.rotationList.length; x++) {
            if (typeof ($scope.rotationList[x].queuePos) != "undefined" && $scope.rotationList[x].queuePos != "") {

                var pos = parseInt($scope.rotationList[x].queuePos);
                var nextNumber = (pos + 1).toString();

                var nextPos = $scope.rotationList.findPos(nextNumber);
                if (nextPos !== false) {
                    var outPoint = endPointList[$scope.rotationList[x].id].outPoint;
                    var inPoint = endPointList[$scope.rotationList[nextPos].id].inPoint;
                    $scope.plumbInstance.connect({ source: outPoint, target: inPoint });
                } else {
                    var outPoint = endPointList[$scope.rotationList[x].id].outPoint;
                    var inPoint = endPointList[$scope.rotationList[$scope.rotationList.findPos("1")].id].inPoint;
                    $scope.plumbInstance.connect({ source: outPoint, target: inPoint });
                }
            }
        }


        $scope.plumbInstance.draggable($(".item"), {
            containment: "parent",
            stop: function (event, ui) {
                $scope.save();
            }
        });

        $scope.plumbInstance.repaintEverything();
        $scope.initDone = true;
        $scope.buildQueue();

    }

    $scope.selectTaskObj = function (obj) {
        if (obj.selected) {
            obj.selected = false;
        } else {
            obj.selected = true;
        }
    }

    $scope.freshBindings = function () {

        $scope.plumbInstance.addEndpoint($(".item").last(), exampleEndpoint);
        $scope.plumbInstance.addEndpoint($(".item").last(), exampleEndpoint2);
        $scope.plumbInstance.draggable($(".item").last(), {
            containment: "parent",
            stop: function (event, ui) {
                $scope.save();
            }
        });
        $scope.plumbInstance.repaintEverything();

    }

    $scope.newRotation = {};

    $scope.addConnections = function (newRotation) {
        var newRot = angular.copy(newRotation);
        newRot.id = "" + Date.now();
        newRot.left = 1;
        newRot.top = 1;
        newRot.smurfs = [];
        newRot.groups = [];
        if (newRot.type == 'task') {
            for (var x = 0; x < $scope.smurfs.length; x++) {
                if ($scope.smurfs[x].selected) {
                    newRot.smurfs.push($scope.smurfs[x]);
                }
            }

            for (var x = 0; x < $scope.groups.length; x++) {
                if ($scope.groups[x].selected) {
                    newRot.groups.push($scope.groups[x]);
                }
            }
        }
        $scope.rotationList.push(newRot);
        $scope.freshRequested = true;
        $scope.buildQueue();
        $scope.saveRequested = true;
    }

    $scope.$on('smurfDropped', function (e, args) {
        var obj;
        if ($(args.ui.helper.context).attr("type") == "smurf") {
            obj = $scope.smurfs.SmurfsfindByUsernameRegion($(args.ui.helper.context).attr("username"), $(args.ui.helper.context).attr("region"));
        } else {
            obj = $scope.groups.GroupfindById($(args.ui.helper.context).attr("id"));
        }
        var rotIndex = $scope.rotationList.GroupfindById($(args.event.target).attr("id"));

        if (obj !== false && rotIndex !== false) {
            $scope.$apply(function () {
                if ($(args.ui.helper.context).attr("type") == "smurf") {
                    if ($scope.rotationList[rotIndex].smurfs.length > 0) {
                        if ($scope.rotationList[rotIndex].smurfs.SmurfsfindByUsernameRegion($(args.ui.helper.context).attr("username"), $(args.ui.helper.context).attr("region")) === false) {
                            $scope.rotationList[rotIndex].smurfs.push($scope.smurfs[obj]);
                        }
                    } else {
                        $scope.rotationList[rotIndex].smurfs.push($scope.smurfs[obj]);
                    }
                } else {
                    if ($scope.rotationList[rotIndex].groups.length > 0) {
                        if ($scope.rotationList[rotIndex].groups.GroupfindById($(args.event.target).attr("id")) === false) {
                            $scope.rotationList[rotIndex].groups.push($scope.groups[obj]);
                        }
                    } else {
                        $scope.rotationList[rotIndex].groups.push($scope.groups[obj]);
                    }
                }
            });
        }
    });

    $scope.deleteRotation = function (rotation) {
        var index = $scope.rotationList.indexOf(rotation);
        $scope.rotationList.splice(index, 1);

        var eps = $scope.plumbInstance.getEndpoints(document.getElementById(rotation.id));
        for (var i = 0; i < eps.length; i++) {
            $scope.plumbInstance.deleteEndpoint(eps[i]);
        }
        $scope.buildQueue();
        $scope.save();
    }

    $scope.startPoint = null;

    $scope.buildQueue = function (source, target) {
        if ($scope.initDone) {
            $scope.saveLocked = true;
            for (var x = 0; x < $scope.rotationList.length; x++) {
                $scope.rotationList[x].queuePos = "";
            }
            var connectionList = $scope.plumbInstance.getAllConnections();
            var last = null;
            var preTarget = null;
            for (var x = 0; x < connectionList.length; x++) {
                var objId = connectionList[x].sourceId;
                if (x == 0) {
                    $scope.startPoint = $scope.rotationList.findById(objId);
                }
                if (preTarget && preTarget == objId || x == 0) {
                    $scope.rotationList.findById(objId).queuePos = (x + 1);
                    last = [connectionList[x].targetId, x];
                }
                preTarget = connectionList[x].targetId;
            }
            if (last && connectionList.length > 0 && last[0] != $scope.startPoint.id) {
                $scope.rotationList.findById(last[0]).queuePos = last[1] + 2;
            } else {
                if (connectionList.length > 1) {
                    console.log("Loop completed");
                }
            }
            $scope.saveLocked = false;
        }
    }

    $scope.removeSmurf = function (smurf, rotation) {
        var index = rotation.smurfs.indexOf(smurf);
        rotation.smurfs.splice(index, 1);
    }

    $scope.removeGroup = function (group, rotation) {
        var index = rotation.groups.indexOf(group);
        rotation.groups.splice(index, 1);
    }

    $scope.save = function () {
        if ($scope.initDone) {
            var saveArr = angular.copy($scope.rotationList);
            saveArr.sort(sortPos);
            for (var x = 0; x < saveArr.length; x++) {
                var id = saveArr[x].id;
                var div = document.getElementById(id);

                if (div) {
                    var wide = div.parentElement.offsetWidth;
                    var height = div.parentElement.offsetHeight;

                    var percentLeft = div.offsetLeft / wide * 100;
                    var percentTop = div.offsetTop / height * 100;

                    saveArr[x].left = percentLeft;
                    saveArr[x].top = percentTop;
                }
            }
            if (!$scope.saveLocked) {
                NET.saveRotations(saveArr);
            }
        }
    }

    Array.prototype.findById = function (id) {
        for (var x = 0; x < this.length; x++) {
            if (id == this[x]['id']) {
                return this[x];
            }
        }
    }

    Array.prototype.findPos = function (pos) {
        for (var x = 0; x < this.length; x++) {
            if (pos == this[x]['queuePos']) {
                return x;
            }
        }
        return false;
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

    function sortPos(a, b) {
        if (a.queuePos < b.queuePos)
            return -1;
        else if (a.queuePos > b.queuePos)
            return 1;
        else
            return 0;
    }

    $scope.$watch("rotationList", function () {
        $scope.save();
    }, true);

    $scope.initRotations();

    $scope.$on("$destroy", function () {
        NET.enableMove();
        $rootScope.disableMove = false;
        $scope.plumbInstance.reset();
    });
});