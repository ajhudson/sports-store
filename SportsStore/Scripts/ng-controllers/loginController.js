(function () {
    var app = angular.module('loginApp', ['ui.bootstrap']);

    app.factory('loginFactory', ['$http', function ($http) {
        loginFactory.authenticate = function (usr, pwd) {

        };
    }]);

    app.controller('LoginController', function () {
        var lc = this;

        lc.init = function () {
            lc.username = "the username";
            lc.password = "the password";
        }

    });
})();