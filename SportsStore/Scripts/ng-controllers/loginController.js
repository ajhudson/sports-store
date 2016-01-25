(function () {
    var app = angular.module('loginApp', ['ui.bootstrap']);
    /*
    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-formurlencoded;charset=utf-8'
    }]);*/

    app.factory('authFactory', ['$http', '$location', function ($http, $location) {
        var authFactory = {};
        var baseUrl = $location.protocol() + "://" + $location.host() + ":" + $location.port() + "/";

        // authenticate user
        authFactory.authenticate = function (usr, pwd) {
            var authenticatedOk = false;
            var dataToPost = $.param({
                grant_type: 'password',
                username: usr,
                password: pwd
            });

            var req = {
                method: 'POST',
                url: '/authenticate',
                headers: {
                    'Content-Type': 'application/x-www-formurlencoded;charset=utf-8'
                },
                data: dataToPost
            };

            return $http(req);
        };

        // get products list
        authFactory.getProducts = function (authToken) {
            var req = {
                method: 'GET',
                url: baseUrl + 'api/products/'
            };
            return $http(req);
        }

        // delete product
        authFactory.deleteProduct = function (authToken, productId) {
            var req = {
                method: 'DELETE',
                headers: {
                    Authorization: "bearer " + authToken
                },
                url: baseUrl + 'api/products/' + productId
            };

            return $http(req);
        }

        // update product
        authFactory.updateProduct = function (authToken, product) {
            var req = {
                method: 'POST',
                headers: {
                    Authorization: "bearer " + authToken
                },
                url: baseUrl + 'api/products/' + product.Id,
                data: product
            };

            return $http(req);
        };

        return authFactory;
    }]);

    app.controller('LoginController', ['authFactory', function (authFactory) {
        var lc = this;
        lc.username = "";
        lc.password = "";
        lc.alerts = [];
        lc.attemptedLogin = false;
        lc.authenticatedOk = false;
        lc.accessToken = "";
        lc.alerts = [];
        lc.products = [];
        lc.selectedProductId = 0;
        lc.editMode = false;
        lc.newPrice = 0;

        lc.init = function () {
        }

        lc.isAuthenticatedDisplay = function () {
            return lc.authenticatedOk ? "Yes" : "No";
        }

        lc.setSelectedProduct = function (id) {
            lc.selectedProductId = id;
        }

        lc.clearSelectedProduct = function () {
            lc.selectedProductId = 0;
        }

        lc.changeProduct = function () {
            lc.editMode = true;
            lc.newPrice = 0;
        }

        lc.showUsernameValidationHelper = function () {
            return lc.authenticateForm.username.$error.required && lc.authenticateForm.username.$dirty;
        };

        lc.showPasswordValidationHelper = function () {
            return lc.authenticateForm.password.$error.required && lc.authenticateForm.password.$dirty;
        }

        lc.removeAlert = function (index) {
            lc.alerts.splice(index, 1);
        }

        lc.authenticate = function () {
            lc.attemptedLogin = false;
            authFactory.authenticate(lc.username, lc.password).then(function (successResponse) {
                lc.authenticatedOk = true;
                lc.accessToken = successResponse.data['access_token'];
                lc.attemptedLogin = true;
                lc.alerts.push({ type: 'success', msg: 'Successfully authenticated' });
            }, function (failedResponse) {
                var errMsg = failedResponse.status + " " + failedResponse.statusText + " " + failedResponse.data.error;
                lc.alerts.push({ type: 'danger', msg: 'Failed authentication' });
                lc.attemptedLogin = true;
            });
        }

        lc.getProducts = function () {
            authFactory.getProducts(lc.accessToken).then(function (successResponse) {
                var i = 0;
                lc.products.length = 0;
                lc.selectedProductId = 0;
                lc.editMode = false;

                if (successResponse.data.length > 0) {
                    for (i = 0; i < successResponse.data.length; i++) {
                        lc.products.push(successResponse.data[i]);
                    }
                }
            }, function (failedResponse) {
                var errMsg = failedResponse.status + " " + failedResponse.statusText + " " + failedResponse.data.error;
                lc.alerts.push({ type: 'danger', msg: 'Failed authentication' });
            });
        }

        lc.deleteProduct = function () {
            authFactory.deleteProduct(lc.accessToken, lc.selectedProductId).then(function (successResponse) {
                lc.alerts.push({ type: "info", msg: 'Deleted OK' });
                lc.getProducts();
            }, function (failedResponse) {
                lc.alerts.push({ type: 'danger', msg: failedResponse.statusText });
            });
        }

        lc.changePrice = function (index) {
            var product = lc.products[index];
            product.Price = lc.newPrice;

            authFactory.updateProduct(lc.accessToken, product).then(function (successResponse) {
                lc.alerts.push({ type: 'info', msg: 'Product updated successfully' });
                lc.getProducts();
            }, function (failedResponse) {
                var errMsgs = [];
                var m = "";
                var i = 0;

                errMsgs.push(failedResponse.responseText);

                if (failedResponse.data.ModelState) {
                    for (m in failedResponse.data.ModelState) {
                        if (failedResponse.data.ModelState[m].length) {
                            for (i = 0; i < failedResponse.data.ModelState[m].length; i++) {
                                errMsgs.push(failedResponse.data.ModelState[m][i]);
                            }
                        }
                    }
                }

                lc.alerts.push({ type: 'danger', msg: errMsgs.join("") });
            });

        }
    }]);
})();